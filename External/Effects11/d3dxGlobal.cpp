//////////////////////////////////////////////////////////////////////////////
//
//  Copyright (C) Microsoft Corporation.  All Rights Reserved.
//
//  File:       d3dxGlobal.cpp
//  Content:    D3DX11 Effects implementation for helper data structures
//
//////////////////////////////////////////////////////////////////////////////

#include "pchfx.h"
#include <intsafe.h>

namespace D3DX11Core
{

//////////////////////////////////////////////////////////////////////////
// CMemoryStream - A class to simplify reading binary data
//////////////////////////////////////////////////////////////////////////

CMemoryStream::CMemoryStream()
{
    m_pData = NULL;
    m_cbData = 0;
    m_readPtr = 0;
}

CMemoryStream::~CMemoryStream()
{
}

HRESULT CMemoryStream::SetData(const void *pData, SIZE_T size)
{
    m_pData = (BYTE*) pData;
    m_cbData = size;
    m_readPtr = 0;

    return S_OK;
}

HRESULT CMemoryStream::ReadAtOffset(SIZE_T offset, SIZE_T size, void **ppData)
{
    if (offset >= m_cbData)
        return E_FAIL;

    m_readPtr = offset;
    return Read(ppData, size);
}

HRESULT CMemoryStream::ReadAtOffset(SIZE_T offset, LPCSTR *ppString)
{
    if (offset >= m_cbData)
        return E_FAIL;

    m_readPtr = offset;
    return Read(ppString);
}

HRESULT CMemoryStream::Read(void **ppData, SIZE_T size)
{
    SIZE_T temp = m_readPtr + size;

    if (temp < m_readPtr || temp > m_cbData)
        return E_FAIL;

    *ppData = m_pData + m_readPtr;
    m_readPtr = temp;
    return S_OK;
}

HRESULT CMemoryStream::Read(UINT *pDword)
{
    UINT *pTempDword;
    HRESULT hr;

    hr = Read((void**) &pTempDword, sizeof(UINT));
    if (FAILED(hr))
        return E_FAIL;

    *pDword = *pTempDword;
    return S_OK;
}

HRESULT CMemoryStream::Read(LPCSTR *ppString)
{
    SIZE_T iChar;

    for(iChar=m_readPtr; m_pData[iChar]; iChar++)
    {
        if (iChar > m_cbData)
            return E_FAIL;      
    }

    *ppString = (LPCSTR) (m_pData + m_readPtr);
    m_readPtr = iChar;

    return S_OK;
}

SIZE_T CMemoryStream::GetPosition()
{
    return m_readPtr;
}

HRESULT CMemoryStream::Seek(SIZE_T offset)
{
    if (offset > m_cbData)
        return E_FAIL;

    m_readPtr = offset;
    return S_OK;
}

}

//////////////////////////////////////////////////////////////////////////
// CDataBlock - used to dynamically build up the effect file in memory
//////////////////////////////////////////////////////////////////////////

CDataBlock::CDataBlock()
{
    m_size = 0;
    m_maxSize = 0;
    m_pData = NULL;
    m_pNext = NULL;
    m_IsAligned = FALSE;
}

CDataBlock::~CDataBlock()
{
    SAFE_DELETE_ARRAY(m_pData);
    SAFE_DELETE(m_pNext);
}

void CDataBlock::EnableAlignment()
{
    m_IsAligned = TRUE;
}

HRESULT CDataBlock::AddData(const void *pvNewData, UINT bufferSize, CDataBlock **ppBlock)
{
    HRESULT hr = S_OK;
    UINT bytesToCopy;
    const BYTE *pNewData = (const BYTE*) pvNewData;

    if (m_maxSize == 0)
    {
        // This is a brand new DataBlock, fill it up
        m_maxSize = max(8192, bufferSize);

        VN( m_pData = NEW BYTE[m_maxSize] );
    }

    D3DXASSERT(m_pData == AlignToPowerOf2(m_pData, c_DataAlignment));

    bytesToCopy = min(m_maxSize - m_size, bufferSize);
    memcpy(m_pData + m_size, pNewData, bytesToCopy);
    pNewData += bytesToCopy;
    
    if (m_IsAligned)
    {
        D3DXASSERT(m_size == AlignToPowerOf2(m_size, c_DataAlignment));
        m_size += AlignToPowerOf2(bytesToCopy, c_DataAlignment);
    }
    else
    {
        m_size += bytesToCopy;
    }
    
    bufferSize -= bytesToCopy;
    *ppBlock = this;

    if (bufferSize != 0)
    {
        D3DXASSERT(NULL == m_pNext); // make sure we're not overwriting anything

        // Couldn't fit all data into this block, spill over into next
        VN( m_pNext = NEW CDataBlock() );
        if (m_IsAligned)
        {
            m_pNext->EnableAlignment();
        }
        VH( m_pNext->AddData(pNewData, bufferSize, ppBlock) );
    }

lExit:
    return hr;
}

void* CDataBlock::Allocate(UINT bufferSize, CDataBlock **ppBlock)
{
    void *pRetValue;
    UINT temp = m_size + bufferSize;

    if (temp < m_size)
        return NULL;

    *ppBlock = this;

    if (m_maxSize == 0)
    {
        // This is a brand new DataBlock, fill it up
        m_maxSize = max(8192, bufferSize);

        m_pData = NEW BYTE[m_maxSize];
        if (!m_pData)
            return NULL;
        memset(m_pData, 0xDD, m_maxSize);
    }
    else if (temp > m_maxSize)
    {
        D3DXASSERT(NULL == m_pNext); // make sure we're not overwriting anything

        // Couldn't fit data into this block, spill over into next
        m_pNext = NEW CDataBlock();
        if (!m_pNext)
            return NULL;
        if (m_IsAligned)
        {
            m_pNext->EnableAlignment();
        }

        return m_pNext->Allocate(bufferSize, ppBlock);
    }

    D3DXASSERT(m_pData == AlignToPowerOf2(m_pData, c_DataAlignment));

    pRetValue = m_pData + m_size;
    if (m_IsAligned)
    {
        D3DXASSERT(m_size == AlignToPowerOf2(m_size, c_DataAlignment));
        m_size = AlignToPowerOf2(temp, c_DataAlignment);
    }
    else
    {
        m_size = temp;
    }

    return pRetValue;
}


//////////////////////////////////////////////////////////////////////////

CDataBlockStore::CDataBlockStore()
{
    m_pFirst = NULL;
    m_pLast = NULL;
    m_Size = 0;
    m_Offset = 0;
    m_IsAligned = FALSE;

#if _DEBUG
    m_cAllocations = 0;
#endif
}

CDataBlockStore::~CDataBlockStore()
{
    // Can't just do SAFE_DELETE(m_pFirst) since it blows the stack when deleting long chains of data
    CDataBlock* pData = m_pFirst;
    while(pData)
    {
        CDataBlock* pCurrent = pData;
        pData = pData->m_pNext;
        pCurrent->m_pNext = NULL;
        delete pCurrent;
    }

    // m_pLast will be deleted automatically
}

void CDataBlockStore::EnableAlignment()
{
    m_IsAligned = TRUE;
}

HRESULT CDataBlockStore::AddString(LPCSTR pString, UINT *pOffset)
{
    size_t strSize = strlen(pString) + 1;
    D3DXASSERT( strSize <= 0xffffffff );
    return AddData(pString, (UINT)strSize, pOffset);
}

HRESULT CDataBlockStore::AddData(const void *pNewData, UINT bufferSize, UINT *pCurOffset)
{
    HRESULT hr = S_OK;

    if (bufferSize == 0)
    {        
        if (pCurOffset)
        {
            *pCurOffset = 0;
        }
        goto lExit;
    }

    if (!m_pFirst)
    {
        VN( m_pFirst = NEW CDataBlock() );
        if (m_IsAligned)
        {
            m_pFirst->EnableAlignment();
        }
        m_pLast = m_pFirst;
    }

    if (pCurOffset)
        *pCurOffset = m_Size + m_Offset;

    VH( m_pLast->AddData(pNewData, bufferSize, &m_pLast) );
    m_Size += bufferSize;

lExit:
    return hr;
}

void* CDataBlockStore::Allocate(UINT bufferSize)
{
    void *pRetValue = NULL;

#if _DEBUG
    m_cAllocations++;
#endif

    if (!m_pFirst)
    {
        m_pFirst = NEW CDataBlock();
        if (!m_pFirst)
            return NULL;

        if (m_IsAligned)
        {
            m_pFirst->EnableAlignment();
        }
        m_pLast = m_pFirst;
    }

    if (FAILED(UIntAdd(m_Size, bufferSize, &m_Size)))
        return NULL;

    pRetValue = m_pLast->Allocate(bufferSize, &m_pLast);
    if (!pRetValue)
        return NULL;

    return pRetValue;
}

UINT CDataBlockStore::GetSize()
{
    return m_Size;
}
