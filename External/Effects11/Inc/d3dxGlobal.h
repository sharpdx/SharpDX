//////////////////////////////////////////////////////////////////////////////
//
//  Copyright (C) Microsoft Corporation.  All Rights Reserved.
//
//  File:       D3DXGlobal.h
//  Content:    D3DX11 Effects helper defines and data structures
//
//////////////////////////////////////////////////////////////////////////////

#pragma once

#pragma warning(disable : 4100 4127 4189 4201 4245 4389 4505 4701 4706)

namespace D3DX11Debug
{
}

using namespace D3DX11Debug;

#include "d3dx11dbg.h"

#define SAFE_RELEASE(p)       { if (p) { (p)->Release();  (p) = NULL; } }
#define SAFE_ADDREF(p)        { if (p) { (p)->AddRef();               } }

#define SAFE_DELETE_ARRAY(p)  { delete [](p); p = NULL; }
#define SAFE_DELETE(p)        { delete (p); p = NULL;  }

#if FXDEBUG
#define __BREAK_ON_FAIL       { __debugbreak(); }
#else
#define __BREAK_ON_FAIL 
#endif

#define VA(x, action) { hr = (x); if (FAILED(hr)) { action; __BREAK_ON_FAIL;                     return hr;  } }
#define VNA(x,action) {           if (!(x))       { action; __BREAK_ON_FAIL; hr = E_OUTOFMEMORY; goto lExit; } }
#define VBA(x,action) {           if (!(x))       { action; __BREAK_ON_FAIL; hr = E_FAIL;        goto lExit; } }
#define VHA(x,action) { hr = (x); if (FAILED(hr)) { action; __BREAK_ON_FAIL;                     goto lExit; } }

#define V(x)          { VA (x, 0) }
#define VN(x)         { VNA(x, 0) }
#define VB(x)         { VBA(x, 0) }
#define VH(x)         { VHA(x, 0) }

#define VBD(x,str)         { VBA(x, DPF(1,str)) }
#define VHD(x,str)         { VHA(x, DPF(1,str)) }

#define VEASSERT(x)   { hr = (x); if (FAILED(hr)) { __BREAK_ON_FAIL; D3DXASSERT(!#x);                     goto lExit; } }
#define VNASSERT(x)   {           if (!(x))       { __BREAK_ON_FAIL; D3DXASSERT(!#x); hr = E_OUTOFMEMORY; goto lExit; } }

#define ANALYSIS_ASSUME(x)      { D3DXASSERT(x); __analysis_assume(x); __assume(x); }

#define D3DX11FLTASSIGN(a,b)    { *reinterpret_cast< UINT32* >(&(a)) = *reinterpret_cast< UINT32* >(&(b)); }

// Preferred data alignment -- must be a power of 2!
static const UINT c_DataAlignment = sizeof(UINT_PTR);

D3DX11INLINE UINT AlignToPowerOf2(UINT Value, UINT Alignment)
{
    D3DXASSERT((Alignment & (Alignment - 1)) == 0);
    // to align to 2^N, add 2^N - 1 and AND with all but lowest N bits set
    ANALYSIS_ASSUME(Alignment > 0 && Value < MAXDWORD - Alignment);
    return (Value + Alignment - 1) & (~(Alignment - 1));
}

D3DX11INLINE void * AlignToPowerOf2(void *pValue, UINT_PTR Alignment)
{
    D3DXASSERT((Alignment & (Alignment - 1)) == 0);
    // to align to 2^N, add 2^N - 1 and AND with all but lowest N bits set
    return (void *)(((UINT_PTR)pValue + Alignment - 1) & (~((UINT_PTR)Alignment - 1)));
}


// Fast memcpy
D3DX11INLINE void dwordMemcpy( __out_bcount(uByteCount) void * __restrict pDest, __in_bcount(uByteCount) CONST void * __restrict pSource, UINT uByteCount)
{
    UINT i;
    D3DXASSERT(uByteCount % 4 == 0);
#ifdef _AMD64_
    const UINT qwordCount = uByteCount >> 3;

    __int64* src64 = (__int64*) pSource;
    __int64* dst64 = (__int64*) pDest;

    for (i=0; i<(qwordCount & 0x3); i++)
    {
        *(dst64) = *(src64);
        dst64++;
        src64++;
    }

    for (; i<qwordCount; i+= 4)
    {
        *(dst64     ) = *(src64     );
        *(dst64 + 1 ) = *(src64 + 1 );
        *(dst64 + 2 ) = *(src64 + 2 );
        *(dst64 + 3 ) = *(src64 + 3 );
        dst64 += 4;
        src64 += 4;
    }

    ANALYSIS_ASSUME( dst64 - static_cast< __int64* >(pDest) <= uByteCount - 4 );
    ANALYSIS_ASSUME( src64 - static_cast< const __int64* >(pSource) <= uByteCount - 4 );
    if( uByteCount & 0x4 )
    {
        *((UINT*)dst64) = *((UINT*)src64);
    }
#else
    const UINT dwordCount = uByteCount >> 2;

    for (i=0; i<(dwordCount & 0x3); i++)
    {
#pragma prefast(suppress: __WARNING_UNRELATED_LOOP_TERMINATION, "(dwordCount & 03) < dwordCount")
        ((UINT*)pDest)[i  ] = ((UINT*)pSource)[i  ];
    }
    for (; i<dwordCount; i+= 4)
    {
        ((UINT*)pDest)[i  ] = ((UINT*)pSource)[i  ];
        ((UINT*)pDest)[i+1] = ((UINT*)pSource)[i+1];
        ((UINT*)pDest)[i+2] = ((UINT*)pSource)[i+2];
        ((UINT*)pDest)[i+3] = ((UINT*)pSource)[i+3];
    }
#endif
}


namespace D3DX11Core
{

//////////////////////////////////////////////////////////////////////////
// CMemoryStream - A class to simplify reading binary data
//////////////////////////////////////////////////////////////////////////
class CMemoryStream
{
    BYTE    *m_pData;
    SIZE_T  m_cbData;
    SIZE_T  m_readPtr;

public:
    HRESULT SetData(const void *pData, SIZE_T size);

    HRESULT Read(UINT *pUint);
    HRESULT Read(void **ppData, SIZE_T size);
    HRESULT Read(LPCSTR *ppString);

    HRESULT ReadAtOffset(SIZE_T offset, SIZE_T size, void **ppData);
    HRESULT ReadAtOffset(SIZE_T offset, LPCSTR *ppString);

    SIZE_T  GetPosition();
    HRESULT Seek(SIZE_T offset);

    CMemoryStream();
    ~CMemoryStream();
};

}

#if FXDEBUG

namespace D3DX11Debug
{

// This variable indicates how many ticks to go before rolling over
// all of the timer variables in the FX system.
// It is read from the system registry in debug builds; in retail the high bit is simply tested.

_declspec(selectany) unsigned int g_TimerRolloverCount = 0x80000000;
}

#endif // FXDEBUG


//////////////////////////////////////////////////////////////////////////
// CEffectVector - A vector implementation
//////////////////////////////////////////////////////////////////////////

template<class T> class CEffectVector
{
protected:
#if _DEBUG
    T       *m_pCastData; // makes debugging easier to have a casted version of the data
#endif // _DEBUG

    BYTE    *m_pData;
    UINT    m_MaxSize;
    UINT    m_CurSize;

    HRESULT Grow()
    {
        return Reserve(m_CurSize + 1);
    }

    HRESULT Reserve(UINT DesiredSize)
    {
        if (DesiredSize > m_MaxSize)
        {
            BYTE *pNewData;
            UINT newSize = max(m_MaxSize * 2, DesiredSize);

            if (newSize < 16)
                newSize = 16;

            if ((newSize < m_MaxSize) || (newSize < m_CurSize) || (newSize >= UINT_MAX / sizeof(T)))
            {
                m_hLastError = E_OUTOFMEMORY;
                return m_hLastError;
            }

            pNewData = NEW BYTE[newSize * sizeof(T)];
            if (pNewData == NULL)
            {
                m_hLastError = E_OUTOFMEMORY;
                return m_hLastError;
            }

            if (m_pData)
            {
                memcpy(pNewData, m_pData, m_CurSize * sizeof(T));
                delete []m_pData;
            }

            m_pData = pNewData;
            m_MaxSize = newSize;
        }
#if _DEBUG
        m_pCastData = (T*) m_pData;
#endif // _DEBUG
        return S_OK;
    }

public:
    HRESULT m_hLastError;

    CEffectVector<T>()
    {
        m_hLastError = S_OK;
#if _DEBUG
        m_pCastData = NULL;
#endif // _DEBUG
        m_pData = NULL;
        m_CurSize = m_MaxSize = 0;
    }

    ~CEffectVector<T>()
    {
        Clear();
    }

    // cleanly swaps two vectors -- useful for when you want
    // to reallocate a vector and copy data over, then swap them back
    void SwapVector(CEffectVector<T> &vOther)
    {
        BYTE tempData[sizeof(*this)];

        memcpy(tempData, this, sizeof(*this));
        memcpy(this, &vOther, sizeof(*this));
        memcpy(&vOther, tempData, sizeof(*this));
    }

    HRESULT CopyFrom(CEffectVector<T> &vOther)
    {
        HRESULT hr = S_OK;
        Clear();
        VN( m_pData = NEW BYTE[vOther.m_MaxSize * sizeof(T)] );
        
        m_CurSize = vOther.m_CurSize;
        m_MaxSize = vOther.m_MaxSize;
        m_hLastError = vOther.m_hLastError;

        UINT i;
        for (i = 0; i < m_CurSize; ++ i)
        {
            ((T*)m_pData)[i] = ((T*)vOther.m_pData)[i];
        }

lExit:

#if _DEBUG
        m_pCastData = (T*) m_pData;
#endif // _DEBUG

        return hr;
    }

    void Clear()
    {
        Empty();
        SAFE_DELETE_ARRAY(m_pData);
        m_MaxSize = 0;
#if _DEBUG
        m_pCastData = NULL;
#endif // _DEBUG
    }

    void ClearWithoutDestructor()
    {
        m_CurSize = 0;
        m_hLastError = S_OK;
        SAFE_DELETE_ARRAY(m_pData);
        m_MaxSize = 0;

#if _DEBUG
        m_pCastData = NULL;
#endif // _DEBUG
    }

    void Empty()
    {
        UINT i;
        
        // manually invoke destructor on all elements
        for (i = 0; i < m_CurSize; ++ i)
        {   
            ((T*)m_pData + i)->~T();
        }
        m_CurSize = 0;
        m_hLastError = S_OK;
    }

    T* Add()
    {
        if (FAILED(Grow()))
            return NULL;

        // placement new
        return new((T*)m_pData + (m_CurSize ++)) T;
    }

    T* AddRange(UINT count)
    {
        if (m_CurSize + count < m_CurSize)
        {
            m_hLastError = E_OUTOFMEMORY;
            return NULL;
        }

        if (FAILED(Reserve(m_CurSize + count)))
            return NULL;

        T *pData = (T*)m_pData + m_CurSize;
        UINT i;
        for (i = 0; i < count; ++ i)
        {
            new(pData + i) T;
        }
        m_CurSize += count;
        return pData;
    }

    HRESULT Add(const T& var)
    {
        if (FAILED(Grow()))
            return m_hLastError;

        memcpy((T*)m_pData + m_CurSize, &var, sizeof(T));
        m_CurSize++;

        return S_OK;
    }

    HRESULT AddRange(const T *pVar, UINT count)
    {
        if (m_CurSize + count < m_CurSize)
        {
            m_hLastError = E_OUTOFMEMORY;
            return m_hLastError;
        }

        if (FAILED(Reserve(m_CurSize + count)))
            return m_hLastError;

        memcpy((T*)m_pData + m_CurSize, pVar, count * sizeof(T));
        m_CurSize += count;

        return S_OK;
    }

    HRESULT Insert(const T& var, UINT index)
    {
        D3DXASSERT(index < m_CurSize);
        
        if (FAILED(Grow()))
            return m_hLastError;

        memmove((T*)m_pData + index + 1, (T*)m_pData + index, (m_CurSize - index) * sizeof(T));
        memcpy((T*)m_pData + index, &var, sizeof(T));
        m_CurSize++;

        return S_OK;
    }

    HRESULT InsertRange(const T *pVar, UINT index, UINT count)
    {
        D3DXASSERT(index < m_CurSize);
        
        if (m_CurSize + count < m_CurSize)
        {
            m_hLastError = E_OUTOFMEMORY;
            return m_hLastError;
        }

        if (FAILED(Reserve(m_CurSize + count)))
            return m_hLastError;

        memmove((T*)m_pData + index + count, (T*)m_pData + index, (m_CurSize - index) * sizeof(T));
        memcpy((T*)m_pData + index, pVar, count * sizeof(T));
        m_CurSize += count;

        return S_OK;
    }

    inline T& operator[](UINT index)
    {
        D3DXASSERT(index < m_CurSize);
        return ((T*)m_pData)[index];
    }

    // Deletes element at index and shifts all other values down
    void Delete(UINT index)
    {
        D3DXASSERT(index < m_CurSize);

        -- m_CurSize;
        memmove((T*)m_pData + index, (T*)m_pData + index + 1, (m_CurSize - index) * sizeof(T));
    }

    // Deletes element at index and moves the last element into its place
    void QuickDelete(UINT index)
    {
        D3DXASSERT(index < m_CurSize);

        -- m_CurSize;
        memcpy((T*)m_pData + index, (T*)m_pData + m_CurSize, sizeof(T));
    }

    inline UINT GetSize() const
    {
        return m_CurSize;
    }

    inline T* GetData() const
    {
        return (T*)m_pData;
    }

    UINT FindIndexOf(void *pEntry) const
    {
        UINT i;

        for (i = 0; i < m_CurSize; ++ i)
        {   
            if (((T*)m_pData + i) == pEntry)
                return i;
        }

        return -1;
    }

    void Sort(int (__cdecl *pfnCompare)(const void *pElem1, const void *pElem2))
    {
        qsort(m_pData, m_CurSize, sizeof(T), pfnCompare);
    }
};

//////////////////////////////////////////////////////////////////////////
// CEffectVectorOwner - implements a vector of ptrs to objects. The vector owns the objects.
//////////////////////////////////////////////////////////////////////////
template<class T> class CEffectVectorOwner : public CEffectVector<T*>
{
public:
    ~CEffectVectorOwner<T>()
    {
        Clear();
        UINT i;

        for (i=0; i<m_CurSize; i++)
            SAFE_DELETE(((T**)m_pData)[i]);

        SAFE_DELETE_ARRAY(m_pData);
    }

    void Clear()
    {
        Empty();
        SAFE_DELETE_ARRAY(m_pData);
        m_MaxSize = 0;
    }

    void Empty()
    {
        UINT i;

        // manually invoke destructor on all elements
        for (i = 0; i < m_CurSize; ++ i)
        {
            SAFE_DELETE(((T**)m_pData)[i]);
        }
        m_CurSize = 0;
        m_hLastError = S_OK;
    }

    void Delete(UINT index)
    {
        D3DXASSERT(index < m_CurSize);

        SAFE_DELETE(((T**)m_pData)[index]);

        CEffectVector<T*>::Delete(index);
    }
};


//////////////////////////////////////////////////////////////////////////
// Checked UINT, DWORD64
// Use CheckedNumber only with UINT and DWORD64
//////////////////////////////////////////////////////////////////////////
template <class T, T MaxValue> class CheckedNumber
{
    T       m_Value;
    BOOL    m_bValid;

public:
    CheckedNumber<T, MaxValue>()
    {
        m_Value = 0;
        m_bValid = TRUE;
    }

    CheckedNumber<T, MaxValue>(const T &value)
    {
        m_Value = value;
        m_bValid = TRUE;
    }

    CheckedNumber<T, MaxValue>(const CheckedNumber<T, MaxValue> &value)
    {
        m_bValid = value.m_bValid;
        m_Value = value.m_Value;
    }

    CheckedNumber<T, MaxValue> &operator+(const CheckedNumber<T, MaxValue> &other)
    {
        CheckedNumber<T, MaxValue> Res(*this);
        return Res+=other;
    }

    CheckedNumber<T, MaxValue> &operator+=(const CheckedNumber<T, MaxValue> &other)
    {
        if (!other.m_bValid)
        {
            m_bValid = FALSE;
        }
        else
        {
            m_Value += other.m_Value;

            if (m_Value < other.m_Value)
                m_bValid = FALSE;
        }

        return *this;
    }

    CheckedNumber<T, MaxValue> &operator*(const CheckedNumber<T, MaxValue> &other)
    {
        CheckedNumber<T, MaxValue> Res(*this);
        return Res*=other;
    }

    CheckedNumber<T, MaxValue> &operator*=(const CheckedNumber<T, MaxValue> &other)
    {
        if (!other.m_bValid)
        {
            m_bValid = FALSE;
        }
        else
        {
            if (other.m_Value != 0)
            {
                if (m_Value > MaxValue / other.m_Value)
                {
                    m_bValid = FALSE;
                }
            }
            m_Value *= other.m_Value;
        }

        return *this;
    }

    HRESULT GetValue(T *pValue)
    {
        if (!m_bValid)
        {
            *pValue = -1;
            return E_FAIL;
        }

        *pValue = m_Value;
        return S_OK;
    }
};

typedef CheckedNumber<UINT, _UI32_MAX> CCheckedDword;
typedef CheckedNumber<DWORD64, _UI64_MAX> CCheckedDword64;


//////////////////////////////////////////////////////////////////////////
// Data Block Store - A linked list of allocations
//////////////////////////////////////////////////////////////////////////

class CDataBlock
{
protected:
    UINT        m_size;
    UINT        m_maxSize;
    BYTE        *m_pData;
    CDataBlock  *m_pNext;

    BOOL        m_IsAligned;        // Whether or not to align the data to c_DataAlignment

public:
    // AddData appends an existing use buffer to the data block
    HRESULT AddData(const void *pNewData, UINT bufferSize, CDataBlock **ppBlock);

    // Allocate reserves bufferSize bytes of contiguous memory and returns a pointer to the user
    void*   Allocate(UINT bufferSize, CDataBlock **ppBlock);

    void    EnableAlignment();

    CDataBlock();
    ~CDataBlock();

    friend class CDataBlockStore;
};


class CDataBlockStore
{
protected:
    CDataBlock  *m_pFirst;
    CDataBlock  *m_pLast;
    UINT        m_Size;
    UINT        m_Offset;           // m_Offset gets added to offsets returned from AddData & AddString. Use this to set a global for the entire string block
    BOOL        m_IsAligned;        // Whether or not to align the data to c_DataAlignment

public:
#if _DEBUG
    UINT		m_cAllocations;
#endif

public:
    HRESULT AddString(LPCSTR pString, UINT *pOffset);                          // Writes a null-terminated string to buffer
    HRESULT AddData(const void *pNewData, UINT bufferSize, UINT *pOffset);     // Writes data block to buffer

    void*   Allocate(UINT buffferSize);                                        // Memory allocator support
    UINT    GetSize();
    void    EnableAlignment();

    CDataBlockStore();
    ~CDataBlockStore();
};

// Custom allocator that uses CDataBlockStore
// The trick is that we never free, so we don't have to keep as much state around
// Use PRIVATENEW in CEffectLoader

static void* __cdecl operator new(size_t s, CDataBlockStore &pAllocator)
{
    D3DXASSERT( s <= 0xffffffff );
    return pAllocator.Allocate( (UINT)s );
}

static void __cdecl operator delete(void* p, CDataBlockStore &pAllocator)
{
}


//////////////////////////////////////////////////////////////////////////
// Hash table
//////////////////////////////////////////////////////////////////////////

#define HASH_MIX(a,b,c) \
{ \
    a -= b; a -= c; a ^= (c>>13); \
    b -= c; b -= a; b ^= (a<<8); \
    c -= a; c -= b; c ^= (b>>13); \
    a -= b; a -= c; a ^= (c>>12);  \
    b -= c; b -= a; b ^= (a<<16); \
    c -= a; c -= b; c ^= (b>>5); \
    a -= b; a -= c; a ^= (c>>3);  \
    b -= c; b -= a; b ^= (a<<10); \
    c -= a; c -= b; c ^= (b>>15); \
}

static UINT ComputeHash(BYTE *pb, UINT cbToHash)
{
    UINT a;
    UINT b;
    UINT c;
    UINT cbLeft;

    cbLeft = cbToHash;

    a = b = 0x9e3779b9; // the golden ratio; an arbitrary value
    c = 0;

    while (cbLeft >= 12)
    {
        UINT *pdw = reinterpret_cast<UINT *>(pb);

        a += pdw[0];
        b += pdw[1];
        c += pdw[2];

        HASH_MIX(a,b,c);
        pb += 12; 
        cbLeft -= 12;
    }

    c += cbToHash;

    switch(cbLeft) // all the case statements fall through
    {
    case 11: c+=((UINT) pb[10] << 24);
    case 10: c+=((UINT) pb[9]  << 16);
    case 9 : c+=((UINT) pb[8]  <<  8);
        // the first byte of c is reserved for the length
    case 8 : b+=((UINT) pb[7]  << 24);
    case 7 : b+=((UINT) pb[6]  << 16);
    case 6 : b+=((UINT) pb[5]  <<  8);
    case 5 : b+=pb[4];
    case 4 : a+=((UINT) pb[3]  << 24);
    case 3 : a+=((UINT) pb[2]  << 16);
    case 2 : a+=((UINT) pb[1]  <<  8);
    case 1 : a+=pb[0];
    }

    HASH_MIX(a,b,c);

    return c;
}

static UINT ComputeHashLower(BYTE *pb, UINT cbToHash)
{
    UINT a;
    UINT b;
    UINT c;
    UINT cbLeft;

    cbLeft = cbToHash;

    a = b = 0x9e3779b9; // the golden ratio; an arbitrary value
    c = 0;

    while (cbLeft >= 12)
    {
        BYTE pbT[12];
        for( UINT i = 0; i < 12; i++ )
            pbT[i] = (BYTE)tolower(pb[i]);

        UINT *pdw = reinterpret_cast<UINT *>(pbT);

        a += pdw[0];
        b += pdw[1];
        c += pdw[2];

        HASH_MIX(a,b,c);
        pb += 12; 
        cbLeft -= 12;
    }

    c += cbToHash;

    BYTE pbT[12];
    for( UINT i = 0; i < cbLeft; i++ )
        pbT[i] = (BYTE)tolower(pb[i]);

    switch(cbLeft) // all the case statements fall through
    {
    case 11: c+=((UINT) pbT[10] << 24);
    case 10: c+=((UINT) pbT[9]  << 16);
    case 9 : c+=((UINT) pbT[8]  <<  8);
        // the first byte of c is reserved for the length
    case 8 : b+=((UINT) pbT[7]  << 24);
    case 7 : b+=((UINT) pbT[6]  << 16);
    case 6 : b+=((UINT) pbT[5]  <<  8);
    case 5 : b+=pbT[4];
    case 4 : a+=((UINT) pbT[3]  << 24);
    case 3 : a+=((UINT) pbT[2]  << 16);
    case 2 : a+=((UINT) pbT[1]  <<  8);
    case 1 : a+=pbT[0];
    }

    HASH_MIX(a,b,c);

    return c;
}


static UINT ComputeHash(LPCSTR pString)
{
    return ComputeHash((BYTE*) pString, (UINT)strlen(pString));
}


// 1) these numbers are prime
// 2) each is slightly less than double the last
// 4) each is roughly in between two powers of 2;
//    (2^n hash table sizes are VERY BAD; they effectively truncate your
//     precision down to the n least significant bits of the hash)
static const UINT c_PrimeSizes[] = 
{
    11,
    23,
    53,
    97,
    193,
    389,
    769,
    1543,
    3079,
    6151,
    12289,
    24593,
    49157,
    98317,
    196613,
    393241,
    786433,
    1572869,
    3145739,
    6291469,
    12582917,
    25165843,
    50331653,
    100663319,
    201326611,
    402653189,
    805306457,
    1610612741,
};

static const UINT c_NumPrimes = sizeof(c_PrimeSizes) / sizeof(UINT);

template<typename T, BOOL (*pfnIsEqual)(const T &Data1, const T &Data2)>
class CEffectHashTable
{
protected:

    struct SHashEntry
    {
        UINT        Hash;
        T           Data;
        SHashEntry  *pNext;
    };

    // Array of hash entries
    SHashEntry  **m_rgpHashEntries;
    UINT        m_NumHashSlots;
    UINT        m_NumEntries;
    bool        m_bOwnHashEntryArray;

public:
    class CIterator
    {
        friend class CEffectHashTable;

    protected:
        SHashEntry **ppHashSlot;
        SHashEntry *pHashEntry;

    public:
        T GetData()
        {
            D3DXASSERT(NULL != pHashEntry);
            return pHashEntry->Data;
        }

        UINT GetHash()
        {
            D3DXASSERT(NULL != pHashEntry);
            return pHashEntry->Hash;
        }
    };

    CEffectHashTable()
    {
        m_rgpHashEntries = NULL;
        m_NumHashSlots = 0;
        m_NumEntries = 0;
        m_bOwnHashEntryArray = false;
    }

    HRESULT Initialize(CEffectHashTable *pOther)
    {
        HRESULT hr = S_OK;
        SHashEntry **rgpNewHashEntries = NULL;
        SHashEntry ***rgppListEnds = NULL;
        UINT i;
        UINT valuesMigrated = 0;
        UINT actualSize;

        Cleanup();

        actualSize = pOther->m_NumHashSlots;
        VN( rgpNewHashEntries = NEW SHashEntry*[actualSize] );

        ZeroMemory(rgpNewHashEntries, sizeof(SHashEntry*) * actualSize);

        // Expensive operation: rebuild the hash table
        CIterator iter, nextIter;
        pOther->GetFirstEntry(&iter);
        while (!pOther->PastEnd(&iter))
        {
            UINT index = iter.GetHash() % actualSize;

            // we need to advance to the next element
            // before we seize control of this element and move
            // it to the new table
            nextIter = iter;
            pOther->GetNextEntry(&nextIter);

            // seize this hash entry, migrate it to the new table
            SHashEntry *pNewEntry;
            VN( pNewEntry = NEW SHashEntry );
            
            pNewEntry->pNext = rgpNewHashEntries[index];
            pNewEntry->Data = iter.pHashEntry->Data;
            pNewEntry->Hash = iter.pHashEntry->Hash;
            rgpNewHashEntries[index] = pNewEntry;

            iter = nextIter;
            ++ valuesMigrated;
        }

        D3DXASSERT(valuesMigrated == pOther->m_NumEntries);

        m_rgpHashEntries = rgpNewHashEntries;
        m_NumHashSlots = actualSize;
        m_NumEntries = pOther->m_NumEntries;
        m_bOwnHashEntryArray = true;
        rgpNewHashEntries = NULL;

lExit:
        SAFE_DELETE_ARRAY( rgpNewHashEntries );
        return hr;
    }

protected:
    void CleanArray()
    {
        if (m_bOwnHashEntryArray)
        {
            SAFE_DELETE_ARRAY(m_rgpHashEntries);
            m_bOwnHashEntryArray = false;
        }
    }

public:
    void Cleanup()
    {
        UINT i;
        for (i = 0; i < m_NumHashSlots; ++ i)
        {
            SHashEntry *pCurrentEntry = m_rgpHashEntries[i];
            SHashEntry *pTempEntry;
            while (NULL != pCurrentEntry)
            {
                pTempEntry = pCurrentEntry->pNext;
                SAFE_DELETE(pCurrentEntry);
                pCurrentEntry = pTempEntry;
                -- m_NumEntries;
            }
        }
        CleanArray();
        m_NumHashSlots = 0;
        D3DXASSERT(m_NumEntries == 0);
    }

    ~CEffectHashTable()
    {
        Cleanup();
    }

    static UINT GetNextHashTableSize(__in UINT DesiredSize)
    {
        // figure out the next logical size to use
        for (UINT i = 0; i < c_NumPrimes; ++ i)
        {
            if (c_PrimeSizes[i] >= DesiredSize)
            {
                return c_PrimeSizes[i];
            }
        }

        return DesiredSize;
    }
    
    // O(n) function
    // Grows to the next suitable size (based off of the prime number table)
    // DesiredSize is merely a suggestion
    HRESULT Grow(__in UINT DesiredSize,
                 __in UINT ProvidedArraySize = 0,
                 __in_ecount_opt(ProvidedArraySize)
                 void** ProvidedArray = NULL,
                 __in bool OwnProvidedArray = false)
    {
        HRESULT hr = S_OK;
        SHashEntry **rgpNewHashEntries = NULL;
        SHashEntry ***rgppListEnds = NULL;
        UINT valuesMigrated = 0;
        UINT actualSize;

        VB( DesiredSize > m_NumHashSlots );

        actualSize = GetNextHashTableSize(DesiredSize);

        if (ProvidedArray &&
            ProvidedArraySize >= actualSize)
        {
            rgpNewHashEntries = reinterpret_cast<SHashEntry**>(ProvidedArray);
        }
        else
        {
            OwnProvidedArray = true;
            
            VN( rgpNewHashEntries = NEW SHashEntry*[actualSize] );
        }
        
        ZeroMemory(rgpNewHashEntries, sizeof(SHashEntry*) * actualSize);

        // Expensive operation: rebuild the hash table
        CIterator iter, nextIter;
        GetFirstEntry(&iter);
        while (!PastEnd(&iter))
        {
            UINT index = iter.GetHash() % actualSize;

            // we need to advance to the next element
            // before we seize control of this element and move
            // it to the new table
            nextIter = iter;
            GetNextEntry(&nextIter);

            // seize this hash entry, migrate it to the new table
            iter.pHashEntry->pNext = rgpNewHashEntries[index];
            rgpNewHashEntries[index] = iter.pHashEntry;

            iter = nextIter;
            ++ valuesMigrated;
        }

        D3DXASSERT(valuesMigrated == m_NumEntries);

        CleanArray();
        m_rgpHashEntries = rgpNewHashEntries;
        m_NumHashSlots = actualSize;
        m_bOwnHashEntryArray = OwnProvidedArray;

lExit:
        return hr;
    }

    HRESULT AutoGrow()
    {
        // arbitrary heuristic -- grow if 1:1
        if (m_NumEntries >= m_NumHashSlots)
        {
            // grows this hash table so that it is roughly 50% full
            return Grow(m_NumEntries * 2 + 1);
        }
        return S_OK;
    }

#if _DEBUG
    void PrintHashTableStats()
    {
        if (m_NumHashSlots == 0)
        {
            DPF(0, "Uninitialized hash table!");
            return;
        }
        
        UINT i;
        float variance = 0.0f;
        float mean = (float)m_NumEntries / (float)m_NumHashSlots;
        UINT unusedSlots = 0;

        DPF(0, "Hash table slots: %d, Entries in table: %d", m_NumHashSlots, m_NumEntries);

        for (i = 0; i < m_NumHashSlots; ++ i)
        {
            UINT entries = 0;
            SHashEntry *pCurrentEntry = m_rgpHashEntries[i];

            while (NULL != pCurrentEntry)
            {
                SHashEntry *pCurrentEntry2 = m_rgpHashEntries[i];
                
                // check other hash entries in this slot for hash collisions or duplications
                while (pCurrentEntry2 != pCurrentEntry)
                {
                    if (pCurrentEntry->Hash == pCurrentEntry2->Hash)
                    {
                        if (pfnIsEqual(pCurrentEntry->Data, pCurrentEntry2->Data))
                        {
                            D3DXASSERT(0);
                            DPF(0, "Duplicate entry (identical hash, identical data) found!");
                        }
                        else
                        {
                            DPF(0, "Hash collision (hash: %d)", pCurrentEntry->Hash);
                        }
                    }
                    pCurrentEntry2 = pCurrentEntry2->pNext;
                }

                pCurrentEntry = pCurrentEntry->pNext;
                ++ entries;
            }

            if (0 == entries)
            {
                ++ unusedSlots;
            }
            
            // mean must be greater than 0 at this point
            variance += (float)entries * (float)entries / mean;
        }

        variance /= max(1.0f, (m_NumHashSlots - 1));
        variance -= (mean * mean);

        DPF(0, "Mean number of entries per slot: %f, Standard deviation: %f, Unused slots; %d", mean, variance, unusedSlots);
    }
#endif // _DEBUG

    // S_OK if element is found, E_FAIL otherwise
    HRESULT FindValueWithHash(T Data, UINT Hash, CIterator *pIterator)
    {
        D3DXASSERT(m_NumHashSlots > 0);

        UINT index = Hash % m_NumHashSlots;
        SHashEntry *pEntry = m_rgpHashEntries[index];
        while (NULL != pEntry)
        {
            if (Hash == pEntry->Hash && pfnIsEqual(pEntry->Data, Data))
            {
                pIterator->ppHashSlot = m_rgpHashEntries + index;
                pIterator->pHashEntry = pEntry;
                return S_OK;
            }
            pEntry = pEntry->pNext;
        }
        return E_FAIL;
    }

    // S_OK if element is found, E_FAIL otherwise
    HRESULT FindFirstMatchingValue(UINT Hash, CIterator *pIterator)
    {
        D3DXASSERT(m_NumHashSlots > 0);

        UINT index = Hash % m_NumHashSlots;
        SHashEntry *pEntry = m_rgpHashEntries[index];
        while (NULL != pEntry)
        {
            if (Hash == pEntry->Hash)
            {
                pIterator->ppHashSlot = m_rgpHashEntries + index;
                pIterator->pHashEntry = pEntry;
                return S_OK;
            }
            pEntry = pEntry->pNext;
        }
        return E_FAIL;
    }

    // Adds data at the specified hash slot without checking for existence
    HRESULT AddValueWithHash(T Data, UINT Hash)
    {
        HRESULT hr = S_OK;

        D3DXASSERT(m_NumHashSlots > 0);

        SHashEntry *pHashEntry;
        UINT index = Hash % m_NumHashSlots;

        VN( pHashEntry = NEW SHashEntry );
        pHashEntry->pNext = m_rgpHashEntries[index];
        pHashEntry->Data = Data;
        pHashEntry->Hash = Hash;
        m_rgpHashEntries[index] = pHashEntry;

        ++ m_NumEntries;

lExit:
        return hr;
    }

    // Iterator code:
    //
    // CMyHashTable::CIterator myIt;
    // for (myTable.GetFirstEntry(&myIt); !myTable.PastEnd(&myIt); myTable.GetNextEntry(&myIt)
    // { myTable.GetData(&myIt); }
    void GetFirstEntry(CIterator *pIterator)
    {
        SHashEntry **ppEnd = m_rgpHashEntries + m_NumHashSlots;
        pIterator->ppHashSlot = m_rgpHashEntries;
        while (pIterator->ppHashSlot < ppEnd)
        {
            if (NULL != *(pIterator->ppHashSlot))
            {
                pIterator->pHashEntry = *(pIterator->ppHashSlot);
                return;
            }
            ++ pIterator->ppHashSlot;
        }
    }

    BOOL PastEnd(CIterator *pIterator)
    {
        SHashEntry **ppEnd = m_rgpHashEntries + m_NumHashSlots;
        D3DXASSERT(pIterator->ppHashSlot >= m_rgpHashEntries && pIterator->ppHashSlot <= ppEnd);
        return (pIterator->ppHashSlot == ppEnd);
    }

    void GetNextEntry(CIterator *pIterator)
    {
        SHashEntry **ppEnd = m_rgpHashEntries + m_NumHashSlots;
        D3DXASSERT(pIterator->ppHashSlot >= m_rgpHashEntries && pIterator->ppHashSlot <= ppEnd);
        D3DXASSERT(NULL != pIterator->pHashEntry);

        pIterator->pHashEntry = pIterator->pHashEntry->pNext;
        if (NULL != pIterator->pHashEntry)
        {
            return;
        }

        ++ pIterator->ppHashSlot;
        while (pIterator->ppHashSlot < ppEnd)
        {
            pIterator->pHashEntry = *(pIterator->ppHashSlot);
            if (NULL != pIterator->pHashEntry)
            {
                return;
            }
            ++ pIterator->ppHashSlot;
        }
        // hit the end of the list, ppHashSlot == ppEnd
    }

    void RemoveEntry(CIterator *pIterator)
    {
        SHashEntry *pTemp;
        SHashEntry **ppPrev;
        SHashEntry **ppEnd = m_rgpHashEntries + m_NumHashSlots;

        D3DXASSERT(pIterator && !PastEnd(pIterator));
        ppPrev = pIterator->ppHashSlot;
        pTemp = *ppPrev;
        while (pTemp)
        {
            if (pTemp == pIterator->pHashEntry)
            {
                *ppPrev = pTemp->pNext;
                pIterator->ppHashSlot = ppEnd;
                delete pTemp;
                return;
            }
            ppPrev = &pTemp->pNext;
            pTemp = pTemp->pNext;
        }

        // Should never get here
        D3DXASSERT(0);
    }

};

// Allocates the hash slots on the regular heap (since the
// hash table can grow), but all hash entries are allocated on
// a private heap

template<typename T, BOOL (*pfnIsEqual)(const T &Data1, const T &Data2)>
class CEffectHashTableWithPrivateHeap : public CEffectHashTable<T, pfnIsEqual>
{
protected:
    CDataBlockStore *m_pPrivateHeap;

public:
    CEffectHashTableWithPrivateHeap()
    {
        m_pPrivateHeap = NULL;
    }

    void Cleanup()
    {
        CleanArray();
        m_NumHashSlots = 0;
        m_NumEntries = 0;
    }

    ~CEffectHashTableWithPrivateHeap()
    {
        Cleanup();
    }

    // Call this only once
    void SetPrivateHeap(CDataBlockStore *pPrivateHeap)
    {
        D3DXASSERT(NULL == m_pPrivateHeap);
        m_pPrivateHeap = pPrivateHeap;
    }

    // Adds data at the specified hash slot without checking for existence
    HRESULT AddValueWithHash(T Data, UINT Hash)
    {
        HRESULT hr = S_OK;

        D3DXASSERT(NULL != m_pPrivateHeap);
        D3DXASSERT(m_NumHashSlots > 0);

        SHashEntry *pHashEntry;
        UINT index = Hash % m_NumHashSlots;

        VN( pHashEntry = new(*m_pPrivateHeap) SHashEntry );
        pHashEntry->pNext = m_rgpHashEntries[index];
        pHashEntry->Data = Data;
        pHashEntry->Hash = Hash;
        m_rgpHashEntries[index] = pHashEntry;

        ++ m_NumEntries;

lExit:
        return hr;
    }
};
