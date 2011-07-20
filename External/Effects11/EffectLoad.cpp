//////////////////////////////////////////////////////////////////////////////
//
//  Copyright (C) Microsoft Corporation.  All Rights Reserved.
//
//  File:       EffectLoad.cpp
//  Content:    D3DX11 Effects file loading code
//
//////////////////////////////////////////////////////////////////////////////

#include "pchfx.h"

#include <EffectStates11.h>

#define PRIVATENEW new(m_BulkHeap)

namespace D3DX11Effects
{

LPCSTR g_szEffectLoadArea = "D3D11EffectLoader";

SRasterizerBlock g_NullRasterizer;
SDepthStencilBlock g_NullDepthStencil;
SBlendBlock g_NullBlend;
SShaderResource g_NullTexture;
SInterface g_NullInterface;
SUnorderedAccessView g_NullUnorderedAccessView;
SRenderTargetView g_NullRenderTargetView;
SDepthStencilView g_NullDepthStencilView;

// these VTables must be setup in the proper order:
// 1) SetShader
// 2) SetConstantBuffers
// 3) SetSamplers
// 4) SetShaderResources
// 5) CreateShader
SD3DShaderVTable g_vtPS = {
    (void (__stdcall ID3D11DeviceContext::*)(ID3D11DeviceChild*, ID3D11ClassInstance*const*, UINT)) &ID3D11DeviceContext::PSSetShader,
    &ID3D11DeviceContext::PSSetConstantBuffers,
    &ID3D11DeviceContext::PSSetSamplers,
    &ID3D11DeviceContext::PSSetShaderResources,
    (HRESULT (__stdcall ID3D11Device::*)(const void *, SIZE_T, ID3D11ClassLinkage*, ID3D11DeviceChild **)) &ID3D11Device::CreatePixelShader
};

SD3DShaderVTable g_vtVS = {
    (void (__stdcall ID3D11DeviceContext::*)(ID3D11DeviceChild*, ID3D11ClassInstance*const*, UINT)) &ID3D11DeviceContext::VSSetShader,
    &ID3D11DeviceContext::VSSetConstantBuffers,
    &ID3D11DeviceContext::VSSetSamplers,
    &ID3D11DeviceContext::VSSetShaderResources,
    (HRESULT (__stdcall ID3D11Device::*)(const void *, SIZE_T, ID3D11ClassLinkage*, ID3D11DeviceChild **)) &ID3D11Device::CreateVertexShader
};

SD3DShaderVTable g_vtGS = {
    (void (__stdcall ID3D11DeviceContext::*)(ID3D11DeviceChild*, ID3D11ClassInstance*const*, UINT)) &ID3D11DeviceContext::GSSetShader,
    &ID3D11DeviceContext::GSSetConstantBuffers,
    &ID3D11DeviceContext::GSSetSamplers,
    &ID3D11DeviceContext::GSSetShaderResources,
    (HRESULT (__stdcall ID3D11Device::*)(const void *, SIZE_T, ID3D11ClassLinkage*, ID3D11DeviceChild **)) &ID3D11Device::CreateGeometryShader
};

SD3DShaderVTable g_vtHS = {
    (void (__stdcall ID3D11DeviceContext::*)(ID3D11DeviceChild*, ID3D11ClassInstance*const*, UINT)) &ID3D11DeviceContext::HSSetShader,
    &ID3D11DeviceContext::HSSetConstantBuffers,
    &ID3D11DeviceContext::HSSetSamplers,
    &ID3D11DeviceContext::HSSetShaderResources,
    (HRESULT (__stdcall ID3D11Device::*)(const void *, SIZE_T, ID3D11ClassLinkage*, ID3D11DeviceChild **)) &ID3D11Device::CreateHullShader
};

SD3DShaderVTable g_vtDS = {
    (void (__stdcall ID3D11DeviceContext::*)(ID3D11DeviceChild*, ID3D11ClassInstance*const*, UINT)) &ID3D11DeviceContext::DSSetShader,
    &ID3D11DeviceContext::DSSetConstantBuffers,
    &ID3D11DeviceContext::DSSetSamplers,
    &ID3D11DeviceContext::DSSetShaderResources,
    (HRESULT (__stdcall ID3D11Device::*)(const void *, SIZE_T, ID3D11ClassLinkage*, ID3D11DeviceChild **)) &ID3D11Device::CreateDomainShader
};

SD3DShaderVTable g_vtCS = {
    (void (__stdcall ID3D11DeviceContext::*)(ID3D11DeviceChild*, ID3D11ClassInstance*const*, UINT)) &ID3D11DeviceContext::CSSetShader,
    &ID3D11DeviceContext::CSSetConstantBuffers,
    &ID3D11DeviceContext::CSSetSamplers,
    &ID3D11DeviceContext::CSSetShaderResources,
    (HRESULT (__stdcall ID3D11Device::*)(const void *, SIZE_T, ID3D11ClassLinkage*, ID3D11DeviceChild **)) &ID3D11Device::CreateComputeShader
};

SShaderBlock g_NullVS(&g_vtVS);
SShaderBlock g_NullGS(&g_vtGS);
SShaderBlock g_NullPS(&g_vtPS);
SShaderBlock g_NullHS(&g_vtHS);
SShaderBlock g_NullDS(&g_vtDS);
SShaderBlock g_NullCS(&g_vtCS);

D3D10_SHADER_VARIABLE_TYPE GetSimpleParameterTypeFromObjectType(EObjectType ObjectType)
{
    switch (ObjectType)
    {
    case EOT_String:
        return D3D10_SVT_STRING;
    case EOT_Blend:
        return D3D10_SVT_BLEND;        
    case EOT_DepthStencil:
        return D3D10_SVT_DEPTHSTENCIL;        
    case EOT_Rasterizer:
        return D3D10_SVT_RASTERIZER;        
    case EOT_PixelShader:
    case EOT_PixelShader5:
        return D3D10_SVT_PIXELSHADER;        
    case EOT_VertexShader:
    case EOT_VertexShader5:
        return D3D10_SVT_VERTEXSHADER;        
    case EOT_GeometryShader:
    case EOT_GeometryShaderSO:
    case EOT_GeometryShader5:
        return D3D10_SVT_GEOMETRYSHADER;    
    case EOT_HullShader5:
        return D3D11_SVT_HULLSHADER;        
    case EOT_DomainShader5:
        return D3D11_SVT_DOMAINSHADER;        
    case EOT_ComputeShader5:
        return D3D11_SVT_COMPUTESHADER;        
    case EOT_RenderTargetView:
        return D3D10_SVT_RENDERTARGETVIEW;
    case EOT_DepthStencilView:
        return D3D10_SVT_DEPTHSTENCILVIEW;
    case EOT_Texture:
    case EOT_Texture1D:
    case EOT_Texture1DArray:
    case EOT_Texture2D:
    case EOT_Texture2DArray:
    case EOT_Texture2DMS:
    case EOT_Texture2DMSArray:
    case EOT_Texture3D:
    case EOT_TextureCube:
    case EOT_TextureCubeArray:
        return D3D10_SVT_TEXTURE;
    case EOT_Buffer:
        return D3D10_SVT_BUFFER;        
    case EOT_Sampler:
        return D3D10_SVT_SAMPLER;
    case EOT_ByteAddressBuffer:
        return D3D11_SVT_BYTEADDRESS_BUFFER;
    case EOT_StructuredBuffer:
        return D3D11_SVT_STRUCTURED_BUFFER;
    case EOT_RWTexture1D:
        return D3D11_SVT_RWTEXTURE1D;
    case EOT_RWTexture1DArray:
        return D3D11_SVT_RWTEXTURE1DARRAY;
    case EOT_RWTexture2D:
        return D3D11_SVT_RWTEXTURE2D;
    case EOT_RWTexture2DArray:
        return D3D11_SVT_RWTEXTURE2DARRAY;
    case EOT_RWTexture3D:
        return D3D11_SVT_RWTEXTURE3D;
    case EOT_RWBuffer:
        return D3D11_SVT_RWBUFFER;
    case EOT_RWByteAddressBuffer:
        return D3D11_SVT_RWBYTEADDRESS_BUFFER;
    case EOT_RWStructuredBuffer:
    case EOT_RWStructuredBufferAlloc:
    case EOT_RWStructuredBufferConsume:
        return D3D11_SVT_RWSTRUCTURED_BUFFER;
    case EOT_AppendStructuredBuffer:
        return D3D11_SVT_APPEND_STRUCTURED_BUFFER;
    case EOT_ConsumeStructuredBuffer:
        return D3D11_SVT_CONSUME_STRUCTURED_BUFFER;
    default:
        D3DXASSERT(0);
    }
    return D3D10_SVT_VOID;
}

inline HRESULT VerifyPointer(UINT  oBase, UINT  dwSize, UINT  dwMaxSize)
{
    UINT  dwAdd = oBase + dwSize;
    if (dwAdd < oBase || dwAdd > dwMaxSize)
        return E_FAIL;
    return S_OK;
}

//////////////////////////////////////////////////////////////////////////
// EffectHeap 
// A simple class which assists in adding data to a block of memory
//////////////////////////////////////////////////////////////////////////

CEffectHeap::CEffectHeap()
{
    m_pData = NULL;
    m_dwSize = m_dwBufferSize = 0;
}

CEffectHeap::~CEffectHeap()
{
    SAFE_DELETE_ARRAY(m_pData);
}

UINT  CEffectHeap::GetSize()
{
    return m_dwSize;
}

HRESULT CEffectHeap::ReserveMemory(UINT  dwSize)
{
    HRESULT hr = S_OK;

    D3DXASSERT(!m_pData);
    D3DXASSERT(dwSize == AlignToPowerOf2(dwSize, c_DataAlignment));

    m_dwBufferSize = dwSize;

    VN( m_pData = NEW BYTE[m_dwBufferSize] );
    
    // make sure that we have machine word alignment
    D3DXASSERT(m_pData == AlignToPowerOf2(m_pData, c_DataAlignment));

lExit:
    return hr;
}

HRESULT CEffectHeap::AddString(const char *pString, __deref_out_z char **ppPointer)
{
    size_t size = strlen(pString) + 1;
    D3DXASSERT( size <= 0xffffffff );
    return AddData(pString, (UINT)size, (void**) ppPointer);
}

// This data is forcibly aligned, so make sure you account for that in calculating heap size
template <bool bCopyData>
HRESULT CEffectHeap::AddDataInternal(const void *pData, UINT  dwSize, void **ppPointer)
{
    CCheckedDword chkFinalSize( m_dwSize );
    UINT  finalSize;
    HRESULT hr = S_OK;

    chkFinalSize += dwSize;
    chkFinalSize += c_DataAlignment; // account for alignment

    VHD( chkFinalSize.GetValue(&finalSize), "Overflow while adding data to Effect heap."  );
    
    // align original value
    finalSize = AlignToPowerOf2(finalSize - c_DataAlignment, c_DataAlignment);
    VBD( finalSize <= m_dwBufferSize, "Overflow adding data to Effect heap." );

    *ppPointer = m_pData + m_dwSize;
    D3DXASSERT(*ppPointer == AlignToPowerOf2(*ppPointer, c_DataAlignment));

    if( bCopyData )
    {
        memcpy(*ppPointer, pData, dwSize);
    }
    m_dwSize = finalSize;

lExit:
    if (FAILED(hr))
        *ppPointer = NULL;

    return hr;
}

HRESULT CEffectHeap::AddData(const void *pData, UINT  dwSize, void **ppPointer)
{
    return AddDataInternal<true>( pData, dwSize, ppPointer );
}

// Moves a string from the general heap to the private heap and modifies the pointer to
//   point to the new memory block.
// The general heap is freed as a whole, so we don't worry about leaking the given string pointer.
// This data is forcibly aligned, so make sure you account for that in calculating heap size
HRESULT CEffectHeap::MoveString(__deref_inout_z char **ppString)
{
    HRESULT hr;
    char *pNewPointer;

    if (*ppString == NULL)
        return S_OK;

    hr = AddString(*ppString, &pNewPointer);
    *ppString = pNewPointer;

    return hr;
}

// Allocates space but does not move data
// The general heap is freed as a whole, so we don't worry about leaking the given string pointer.
// This data is forcibly aligned, so make sure you account for that in calculating heap size
HRESULT CEffectHeap::MoveEmptyDataBlock(void **ppData, UINT  size)
{
    HRESULT hr;
    void *pNewPointer;

    hr = AddDataInternal<false>(*ppData, size, &pNewPointer);

    *ppData = pNewPointer;
    if (size == 0)
    {
        // To help catch bugs, set zero-byte blocks to null. There's no real reason to do this
        *ppData = NULL;
    }

    return hr;
}

// Moves an array of SInterfaceParameters from the general heap to the private heap and modifies the pointer to
//   point to the new memory block.
// The general heap is freed as a whole, so we don't worry about leaking the given string pointer.
// This data is forcibly aligned, so make sure you account for that in calculating heap size
HRESULT CEffectHeap::MoveInterfaceParameters(UINT InterfaceCount, __in_ecount(1) SShaderBlock::SInterfaceParameter **ppInterfaces)
{
    HRESULT hr;
    SShaderBlock::SInterfaceParameter *pNewPointer;

    if (*ppInterfaces == NULL)
        return S_OK;

    VBD( InterfaceCount <= D3D11_SHADER_MAX_INTERFACES, "Internal loading error: InterfaceCount > D3D11_SHADER_MAX_INTERFACES." );
    VH( AddData(*ppInterfaces, InterfaceCount * sizeof(SShaderBlock::SInterfaceParameter), (void**)&pNewPointer) );

    for( UINT i=0; i < InterfaceCount; i++ )
    {
        VH( MoveString( &pNewPointer[i].pName ) );
    }

    *ppInterfaces = pNewPointer;

lExit:
    return hr;
}


// Moves data from the general heap to the private heap and modifies the pointer to
//   point to the new memory block 
// The general heap is freed as a whole, so we don't worry about leaking the given pointer.
// This data is forcibly aligned, so make sure you account for that in calculating heap size
HRESULT CEffectHeap::MoveData(void **ppData, UINT  size)
{
    HRESULT hr;
    void *pNewPointer;

    hr = AddData(*ppData, size, &pNewPointer);
    
    *ppData = pNewPointer;
    if (size == 0)
    {
        // To help catch bugs, set zero-byte blocks to null. There's no real reason to do this
        *ppData = NULL;
    }

    return hr;
}


//////////////////////////////////////////////////////////////////////////
// Load API 
//////////////////////////////////////////////////////////////////////////

HRESULT CEffect::LoadEffect(CONST void *pEffectBuffer, UINT  cbEffectBuffer)
{
    HRESULT hr = S_OK;
    CEffectLoader loader;

    if (!pEffectBuffer)
    {
        DPF(0, "%s: pEffectBuffer is NULL.", g_szEffectLoadArea);
        VH( E_INVALIDARG );
    }
    
    VH( loader.LoadEffect(this, pEffectBuffer, cbEffectBuffer) );

lExit:
    if( FAILED( hr ) )
    {
        // Release here because m_pShaderBlocks may still be in loader.m_BulkHeap if loading failed before we reallocated the memory
        ReleaseShaderRefection();
    }
    return hr;
}

//////////////////////////////////////////////////////////////////////////
// CEffectLoader
// A helper class which loads an effect
//////////////////////////////////////////////////////////////////////////

HRESULT CEffectLoader::GetUnstructuredDataBlock(UINT offset, UINT  *pdwSize, void **ppData)
{
    HRESULT hr = S_OK;
    UINT  *pBlockSize;

    VH( m_msUnstructured.ReadAtOffset(offset, sizeof(*pBlockSize), (void**) &pBlockSize ) );
    *pdwSize = *pBlockSize;

    VH( m_msUnstructured.Read(ppData, *pdwSize) );

lExit:
    return hr;
}

// position in buffer is lost on error
//
// This function should be used in 1:1 conjunction with CEffectHeap::MoveString;
// that is, any string added to the reflection heap with this function
// must be relocated with MoveString at some point later on.
HRESULT CEffectLoader::GetStringAndAddToReflection(UINT offset, __out_ecount_full(1) char **ppString)
{
    HRESULT hr = S_OK;
    LPCSTR pName;
    SIZE_T oldPos;
    
    if (offset == 0)
    {
        *ppString = NULL;
        goto lExit;
    }

    oldPos = m_msUnstructured.GetPosition();

    VH( m_msUnstructured.ReadAtOffset(offset, &pName) );
    m_ReflectionMemory += AlignToPowerOf2( (UINT)strlen(pName) + 1, c_DataAlignment);
    *ppString = const_cast<char*>(pName);
    
    m_msUnstructured.Seek(oldPos);

lExit:
    return hr;
}

// position in buffer is lost on error
//
// This function should be used in 1:1 conjunction with CEffectHeap::MoveInterfaceParameters;
// that is, any array of parameters added to the reflection heap with this function
// must be relocated with MoveInterfaceParameters at some point later on.
HRESULT CEffectLoader::GetInterfaceParametersAndAddToReflection( UINT InterfaceCount, UINT offset, __out_ecount_full(1) SShaderBlock::SInterfaceParameter **ppInterfaces )
{
    HRESULT hr = S_OK;
    SBinaryInterfaceInitializer* pInterfaceInitializer;
    SIZE_T oldPos;

    if (offset == 0)
    {
        *ppInterfaces = NULL;
        goto lExit;
    }

    oldPos = m_msUnstructured.GetPosition();

    VBD( InterfaceCount <= D3D11_SHADER_MAX_INTERFACES, "Internal loading error: InterfaceCount > D3D11_SHADER_MAX_INTERFACES." );
    m_ReflectionMemory += AlignToPowerOf2(InterfaceCount * sizeof(SShaderBlock::SInterfaceParameter), c_DataAlignment);
    D3DXASSERT( ppInterfaces );
    (*ppInterfaces) = PRIVATENEW SShaderBlock::SInterfaceParameter[InterfaceCount];
    VN( *ppInterfaces );

    VHD( m_msUnstructured.ReadAtOffset(offset, sizeof(SBinaryInterfaceInitializer) * InterfaceCount, (void**)&pInterfaceInitializer),
         "Invalid pEffectBuffer: cannot read interface initializer." );

    for( UINT i=0; i < InterfaceCount; i++ )
    {
        (*ppInterfaces)[i].Index = pInterfaceInitializer[i].ArrayIndex;
        VHD( m_msUnstructured.ReadAtOffset(pInterfaceInitializer[i].oInstanceName, const_cast<LPCSTR*>(&(*ppInterfaces)[i].pName)),
             "Invalid pEffectBuffer: cannot read interface initializer." );
        m_ReflectionMemory += AlignToPowerOf2( (UINT)strlen((*ppInterfaces)[i].pName) + 1, c_DataAlignment);
    }

    m_msUnstructured.Seek(oldPos);

lExit:
    return hr;
}

HRESULT CEffectLoader::FixupCBPointer(SConstantBuffer **ppCB)
{
    HRESULT hr = S_OK;
    
    SIZE_T index = (SConstantBuffer*)*ppCB - m_pOldCBs;
    D3DXASSERT( index * sizeof(SConstantBuffer) == ((size_t)(SConstantBuffer*)*ppCB - (size_t)m_pOldCBs) );
    VBD( index < m_pEffect->m_CBCount, "Internal loading error: invalid constant buffer index." );
    *ppCB = (SConstantBuffer*)(m_pEffect->m_pCBs + index);
    
lExit:
    return hr;
}

HRESULT CEffectLoader::FixupShaderPointer(SShaderBlock **ppShaderBlock)
{
    HRESULT hr = S_OK;
    if (*ppShaderBlock != &g_NullVS && *ppShaderBlock != &g_NullGS && *ppShaderBlock != &g_NullPS &&
        *ppShaderBlock != &g_NullHS && *ppShaderBlock != &g_NullDS && *ppShaderBlock != &g_NullCS && 
        *ppShaderBlock != NULL)
    {
        SIZE_T index = *ppShaderBlock - m_pOldShaders;
        D3DXASSERT( index * sizeof(SShaderBlock) == ((size_t)*ppShaderBlock - (size_t)m_pOldShaders) );
        VBD( index < m_pEffect->m_ShaderBlockCount, "Internal loading error: invalid shader index."  );
        *ppShaderBlock = m_pEffect->m_pShaderBlocks + index;
    }
lExit:
    return hr;
}

HRESULT CEffectLoader::FixupDSPointer(SDepthStencilBlock **ppDSBlock)
{
    HRESULT hr = S_OK;
    if (*ppDSBlock != &g_NullDepthStencil && *ppDSBlock != NULL)
    {
        SIZE_T index = *ppDSBlock - m_pOldDS;
        D3DXASSERT( index * sizeof(SDepthStencilBlock) == ((size_t)*ppDSBlock - (size_t)m_pOldDS) );
        VBD( index < m_pEffect->m_DepthStencilBlockCount, "Internal loading error: invalid depth-stencil state index." );
        *ppDSBlock = m_pEffect->m_pDepthStencilBlocks + index;
    }
lExit:
    return hr;
}

HRESULT CEffectLoader::FixupABPointer(SBlendBlock **ppABBlock)
{
    HRESULT hr = S_OK;
    if (*ppABBlock != &g_NullBlend && *ppABBlock != NULL)
    {
        SIZE_T index = *ppABBlock - m_pOldAB;
        D3DXASSERT( index * sizeof(SBlendBlock) == ((size_t)*ppABBlock - (size_t)m_pOldAB) );
        VBD( index < m_pEffect->m_BlendBlockCount, "Internal loading error: invalid blend state index." );
        *ppABBlock = m_pEffect->m_pBlendBlocks + index;
    }
lExit:
    return hr;
}

HRESULT CEffectLoader::FixupRSPointer(SRasterizerBlock **ppRSBlock)
{
    HRESULT hr = S_OK;
    if (*ppRSBlock != &g_NullRasterizer && *ppRSBlock != NULL)
    {
        SIZE_T index = *ppRSBlock - m_pOldRS;
        D3DXASSERT( index * sizeof(SRasterizerBlock) == ((size_t)*ppRSBlock - (size_t)m_pOldRS) );
        VBD( index < m_pEffect->m_RasterizerBlockCount, "Internal loading error: invalid rasterizer state index." );
        *ppRSBlock = m_pEffect->m_pRasterizerBlocks + index;
    }
lExit:
    return hr;
}

HRESULT CEffectLoader::FixupSamplerPointer(SSamplerBlock **ppSampler)
{
    HRESULT hr = S_OK;
    SIZE_T index = *ppSampler - m_pOldSamplers;
    D3DXASSERT( index * sizeof(SSamplerBlock) == ((size_t)*ppSampler - (size_t)m_pOldSamplers) );
    VBD( index < m_pEffect->m_SamplerBlockCount, "Internal loading error: invalid sampler index." );
    *ppSampler = m_pEffect->m_pSamplerBlocks + index;

lExit:
    return hr;
}

HRESULT CEffectLoader::FixupInterfacePointer(SInterface **ppInterface, bool CheckBackgroundInterfaces)
{
    HRESULT hr = S_OK;
    if (*ppInterface != &g_NullInterface && *ppInterface != NULL)
    {
        SIZE_T index = *ppInterface - m_pOldInterfaces;
        if(index < m_OldInterfaceCount)
        {
            D3DXASSERT( index * sizeof(SInterface) == ((size_t)*ppInterface - (size_t)m_pOldInterfaces) );
            *ppInterface = m_pEffect->m_pInterfaces + index;
        }
        else
        {
            VBD( CheckBackgroundInterfaces, "Internal loading error: invalid interface pointer." );
            for( index=0; index < m_BackgroundInterfaces.GetSize(); index++ )
            {
                if( *ppInterface == m_BackgroundInterfaces[ (UINT)index ] )
                {
                    // The interfaces m_BackgroundInterfaces were concatenated to the original ones in m_pEffect->m_pInterfaces
                    *ppInterface = m_pEffect->m_pInterfaces + (m_OldInterfaceCount + index);
                    break;
                }
            }
            VBD( index < m_BackgroundInterfaces.GetSize(), "Internal loading error: invalid interface pointer." );
        }
    }

lExit:
    return hr;
}

HRESULT CEffectLoader::FixupShaderResourcePointer(SShaderResource **ppResource)
{
    HRESULT hr = S_OK;
    if (*ppResource != &g_NullTexture && *ppResource != NULL)
    {
        SIZE_T index = *ppResource - m_pOldShaderResources;
        D3DXASSERT( index * sizeof(SShaderResource) == ((size_t)*ppResource - (size_t)m_pOldShaderResources) );
        
        // could be a TBuffer or a texture; better check first
        if (index < m_pEffect->m_ShaderResourceCount)
        {
            *ppResource = m_pEffect->m_pShaderResources + index;
        }
        else
        {
            // if this is a TBuffer, then the shader resource pointer
            // actually points into a SConstantBuffer's TBuffer field
            index = (SConstantBuffer*)*ppResource - (SConstantBuffer*)&m_pOldCBs->TBuffer;
            D3DXASSERT( index * sizeof(SConstantBuffer) == ((size_t)(SConstantBuffer*)*ppResource - (size_t)(SConstantBuffer*)&m_pOldCBs->TBuffer) );
            VBD( index < m_pEffect->m_CBCount, "Internal loading error: invalid SRV index." );
            *ppResource = &m_pEffect->m_pCBs[index].TBuffer;
        }
    }
    
lExit:
    return hr;
}

HRESULT CEffectLoader::FixupUnorderedAccessViewPointer(SUnorderedAccessView **ppUnorderedAccessView)
{
    HRESULT hr = S_OK;
    if (*ppUnorderedAccessView != &g_NullUnorderedAccessView && *ppUnorderedAccessView != NULL)
    {
        SIZE_T index = *ppUnorderedAccessView - m_pOldUnorderedAccessViews;
        D3DXASSERT( index * sizeof(SUnorderedAccessView) == ((size_t)*ppUnorderedAccessView - (size_t)m_pOldUnorderedAccessViews) );

        VBD( index < m_pEffect->m_UnorderedAccessViewCount, "Internal loading error: invalid UAV index." );
        *ppUnorderedAccessView = m_pEffect->m_pUnorderedAccessViews + index;
    }

lExit:
    return hr;
}

HRESULT CEffectLoader::FixupRenderTargetViewPointer(SRenderTargetView **ppRenderTargetView)
{
    HRESULT hr = S_OK;
    if (*ppRenderTargetView != &g_NullRenderTargetView && *ppRenderTargetView != NULL)
    {
        SIZE_T index = *ppRenderTargetView - m_pOldRenderTargetViews;
        D3DXASSERT( index * sizeof(SRenderTargetView) == ((size_t)*ppRenderTargetView - (size_t)m_pOldRenderTargetViews) );
        VBD( index < m_pEffect->m_RenderTargetViewCount, "Internal loading error: invalid RTV index." );
        *ppRenderTargetView = m_pEffect->m_pRenderTargetViews + index;
    }

lExit:
    return hr;
}

HRESULT CEffectLoader::FixupDepthStencilViewPointer(SDepthStencilView **ppDepthStencilView)
{
    HRESULT hr = S_OK;
    if (*ppDepthStencilView != &g_NullDepthStencilView && *ppDepthStencilView != NULL)
    {
        SIZE_T index = *ppDepthStencilView - m_pOldDepthStencilViews;
        D3DXASSERT( index * sizeof(SDepthStencilView) == ((size_t)*ppDepthStencilView - (size_t)m_pOldDepthStencilViews) );
        VBD( index < m_pEffect->m_DepthStencilViewCount, "Internal loading error: invalid DSV index." );
        *ppDepthStencilView = m_pEffect->m_pDepthStencilViews + index;
    }

lExit:
    return hr;
}

HRESULT CEffectLoader::FixupStringPointer(SString **ppString)
{
    HRESULT hr = S_OK;
    SIZE_T index = *ppString - m_pOldStrings;
    D3DXASSERT( index * sizeof(SString) == ((size_t)*ppString - (size_t)m_pOldStrings) );
    VBD(index < m_pEffect->m_StringCount, "Internal loading error: invalid string index." );
    *ppString = m_pEffect->m_pStrings + index;
lExit:
    return hr;
}

HRESULT CEffectLoader::FixupMemberDataPointer(SMemberDataPointer **ppMemberData)
{
    HRESULT hr = S_OK;
    SIZE_T index = *ppMemberData - m_pOldMemberDataBlocks;
    D3DXASSERT( index * sizeof(SMemberDataPointer) == ((size_t)*ppMemberData - (size_t)m_pOldMemberDataBlocks) );
    VBD( index < m_pEffect->m_MemberDataCount, "Internal loading error: invalid member block index." );
    *ppMemberData = m_pEffect->m_pMemberDataBlocks + index;
lExit:
    return hr;
}

HRESULT CEffectLoader::FixupVariablePointer(SGlobalVariable **ppVar)
{
    HRESULT hr = S_OK;
    SIZE_T index = *ppVar - m_pOldVars;

    if( index < m_pEffect->m_VariableCount )
    {
        D3DXASSERT( index * sizeof(SGlobalVariable) == ((size_t)*ppVar - (size_t)m_pOldVars) );
        *ppVar = m_pEffect->m_pVariables + index;
    }
    else if( m_pvOldMemberInterfaces )
    {
        // When cloning, m_pvOldMemberInterfaces may be non-NULL, and *ppVar may point to a variable in it.
        const SIZE_T Members = m_pvOldMemberInterfaces->GetSize();
        for( index=0; index < Members; index++ )
        {
            if( (ID3DX11EffectVariable*)(*m_pvOldMemberInterfaces)[ (UINT)index] == (ID3DX11EffectVariable*)*ppVar )
            {
                break;
            }
        }
        VBD( index < Members, "Internal loading error: invalid member pointer." );
        *ppVar = (SGlobalVariable*)m_pEffect->m_pMemberInterfaces[ (UINT)index];
    }
lExit:
    return hr;
}

HRESULT CEffectLoader::FixupGroupPointer(SGroup **ppGroup)
{
    HRESULT hr = S_OK;
    if( *ppGroup != NULL )
    {
        SIZE_T index = *ppGroup - m_pOldGroups;
        D3DXASSERT( index * sizeof(SGroup) == ((size_t)*ppGroup - (size_t)m_pOldGroups) );
        VBD( index < m_pEffect->m_GroupCount, "Internal loading error: invalid group index." );
        *ppGroup = m_pEffect->m_pGroups + index;
    }
lExit:
    return hr;
}

HRESULT GetEffectVersion( UINT effectFileTag, DWORD* pVersion )
{
    D3DXASSERT( pVersion != NULL );
    if( !pVersion )
        return E_FAIL;

    for( UINT i = 0; i < NUM_EFFECT10_VERSIONS; i++ )
    {
        if( g_EffectVersions[i].m_Tag == effectFileTag )
        {
            *pVersion = g_EffectVersions[i].m_Version;
            return S_OK;
        }
    }

    return E_FAIL;
}

HRESULT CEffectLoader::LoadEffect(CEffect *pEffect, CONST void *pEffectBuffer, UINT  cbEffectBuffer)
{
    HRESULT hr = S_OK;
    UINT  i, varSize, cMemberDataBlocks;
    CCheckedDword chkVariables = 0;

    // Used for cloning
    m_pvOldMemberInterfaces = NULL;

    m_BulkHeap.EnableAlignment();

    D3DXASSERT(pEffect && pEffectBuffer);
    m_pEffect = pEffect;
    m_EffectMemory = m_ReflectionMemory = 0;

    VN( m_pEffect->m_pReflection = NEW CEffectReflection() );
    m_pReflection = m_pEffect->m_pReflection;

    // Begin effect load
    VN( m_pEffect->m_pTypePool = NEW CEffect::CTypeHashTable );
    VN( m_pEffect->m_pStringPool = NEW CEffect::CStringHashTable );
    VN( m_pEffect->m_pPooledHeap = NEW CDataBlockStore );
    m_pEffect->m_pPooledHeap->EnableAlignment();
    m_pEffect->m_pTypePool->SetPrivateHeap(m_pEffect->m_pPooledHeap);
    m_pEffect->m_pStringPool->SetPrivateHeap(m_pEffect->m_pPooledHeap);

    VH( m_pEffect->m_pTypePool->AutoGrow() );
    VH( m_pEffect->m_pStringPool->AutoGrow() );

    // Load from blob
    m_pData = (BYTE*)pEffectBuffer;
    m_dwBufferSize = cbEffectBuffer;

    VH( m_msStructured.SetData(m_pData, m_dwBufferSize) );

    // At this point, we assume that the blob is valid
    VHD( m_msStructured.Read((void**) &m_pHeader, sizeof(*m_pHeader)), "pEffectBuffer is too small." );

    // Verify the version
    if( FAILED( hr = GetEffectVersion( m_pHeader->Tag, &m_Version ) ) )
    {
        DPF(0, "Effect version is unrecognized.  This runtime supports fx_5_0 to %s.", g_EffectVersions[NUM_EFFECT10_VERSIONS-1].m_pName );
        VH( hr );
    }

    if( m_pHeader->RequiresPool() || m_pHeader->Pool.cObjectVariables > 0 || m_pHeader->Pool.cNumericVariables > 0 )
    {
        DPF(0, "Effect11 does not support EffectPools." );
        VH( E_FAIL );
    }

    // Get shader block count
    VBD( m_pHeader->cInlineShaders <= m_pHeader->cTotalShaders, "Invalid Effect header: cInlineShaders > cTotalShaders." );

    // Make sure the counts for the Effect don't overflow
    chkVariables = m_pHeader->Effect.cObjectVariables;
    chkVariables += m_pHeader->Effect.cNumericVariables;
    chkVariables += m_pHeader->cInterfaceVariables;
    chkVariables *= sizeof(SGlobalVariable);
    VH( chkVariables.GetValue(&varSize) );

    // Make sure the counts for the SMemberDataPointers don't overflow
    chkVariables = m_pHeader->cClassInstanceElements;
    chkVariables += m_pHeader->cBlendStateBlocks;
    chkVariables += m_pHeader->cRasterizerStateBlocks;
    chkVariables += m_pHeader->cDepthStencilBlocks;
    chkVariables += m_pHeader->cSamplers;
    chkVariables += m_pHeader->Effect.cCBs; // Buffer (for CBuffers and TBuffers)
    chkVariables += m_pHeader->Effect.cCBs; // SRV (for TBuffers)
    VHD( chkVariables.GetValue(&cMemberDataBlocks), "Overflow: too many Effect variables." );

    // Allocate effect resources
    VN( m_pEffect->m_pCBs = PRIVATENEW SConstantBuffer[m_pHeader->Effect.cCBs] );
    VN( m_pEffect->m_pDepthStencilBlocks = PRIVATENEW SDepthStencilBlock[m_pHeader->cDepthStencilBlocks] );
    VN( m_pEffect->m_pRasterizerBlocks = PRIVATENEW SRasterizerBlock[m_pHeader->cRasterizerStateBlocks] );
    VN( m_pEffect->m_pBlendBlocks = PRIVATENEW SBlendBlock[m_pHeader->cBlendStateBlocks] );
    VN( m_pEffect->m_pSamplerBlocks = PRIVATENEW SSamplerBlock[m_pHeader->cSamplers] );
    
    // we allocate raw bytes for variables because they are polymorphic types that need to be placement new'ed
    VN( m_pEffect->m_pVariables = (SGlobalVariable *)PRIVATENEW BYTE[varSize] );
    VN( m_pEffect->m_pAnonymousShaders = PRIVATENEW SAnonymousShader[m_pHeader->cInlineShaders] );

    VN( m_pEffect->m_pGroups = PRIVATENEW SGroup[m_pHeader->cGroups] );
    VN( m_pEffect->m_pShaderBlocks = PRIVATENEW SShaderBlock[m_pHeader->cTotalShaders] );
    VN( m_pEffect->m_pStrings = PRIVATENEW SString[m_pHeader->cStrings] );
    VN( m_pEffect->m_pShaderResources = PRIVATENEW SShaderResource[m_pHeader->cShaderResources] );
    VN( m_pEffect->m_pUnorderedAccessViews = PRIVATENEW SUnorderedAccessView[m_pHeader->cUnorderedAccessViews] );
    VN( m_pEffect->m_pInterfaces = PRIVATENEW SInterface[m_pHeader->cInterfaceVariableElements] );
    VN( m_pEffect->m_pMemberDataBlocks = PRIVATENEW SMemberDataPointer[cMemberDataBlocks] );
    VN( m_pEffect->m_pRenderTargetViews = PRIVATENEW SRenderTargetView[m_pHeader->cRenderTargetViews] );
    VN( m_pEffect->m_pDepthStencilViews = PRIVATENEW SDepthStencilView[m_pHeader->cDepthStencilViews] );

    UINT oStructured = m_pHeader->cbUnstructured + sizeof(SBinaryHeader5);
    VHD( m_msStructured.Seek(oStructured), "Invalid pEffectBuffer: Missing structured data block." );
    VH( m_msUnstructured.SetData(m_pData + sizeof(SBinaryHeader5), oStructured - sizeof(SBinaryHeader5)) );

    VH( LoadCBs() );
    VH( LoadObjectVariables() );
    VH( LoadInterfaceVariables() );
    VH( LoadGroups() );

    // Build shader dependencies
    for (i=0; i<m_pEffect->m_ShaderBlockCount; i++)
    {
        VH( BuildShaderBlock(&m_pEffect->m_pShaderBlocks[i]) );
    }
    
    for( UINT iGroup=0; iGroup<m_pHeader->cGroups; iGroup++ )
    {
        SGroup *pGroup = &m_pEffect->m_pGroups[iGroup];
        pGroup->HasDependencies = FALSE;

        for( UINT iTechnique=0; iTechnique < pGroup->TechniqueCount; iTechnique++ )
        {
            STechnique* pTech = &pGroup->pTechniques[iTechnique];
            pTech->HasDependencies = FALSE;

            for( UINT iPass=0; iPass < pTech->PassCount; iPass++ )
            {
                SPassBlock *pPass = &pTech->pPasses[iPass];

                pTech->HasDependencies |= pPass->CheckDependencies();
            }
            pGroup->HasDependencies |= pTech->HasDependencies;
        }
    }

    VH( InitializeReflectionDataAndMoveStrings() );
    VH( ReallocateReflectionData() );
    VH( ReallocateEffectData() );

    VB( m_pReflection->m_Heap.GetSize() == m_ReflectionMemory );
    
    // Verify that all of the various block/variable types were loaded
    VBD( m_pEffect->m_VariableCount == (m_pHeader->Effect.cObjectVariables + m_pHeader->Effect.cNumericVariables + m_pHeader->cInterfaceVariables), "Internal loading error: mismatched variable count." );
    VBD( m_pEffect->m_ShaderBlockCount == m_pHeader->cTotalShaders, "Internal loading error: mismatched shader block count." );
    VBD( m_pEffect->m_AnonymousShaderCount == m_pHeader->cInlineShaders, "Internal loading error: mismatched anonymous variable count." );
    VBD( m_pEffect->m_ShaderResourceCount == m_pHeader->cShaderResources, "Internal loading error: mismatched SRV count." );
    VBD( m_pEffect->m_InterfaceCount == m_pHeader->cInterfaceVariableElements + m_BackgroundInterfaces.GetSize(), "Internal loading error: mismatched interface count." );
    VBD( m_pEffect->m_UnorderedAccessViewCount == m_pHeader->cUnorderedAccessViews, "Internal loading error: mismatched UAV count." );
    VBD( m_pEffect->m_MemberDataCount == cMemberDataBlocks, "Internal loading error: mismatched member data block count." );
    VBD( m_pEffect->m_RenderTargetViewCount == m_pHeader->cRenderTargetViews, "Internal loading error: mismatched RTV count." );
    VBD( m_pEffect->m_DepthStencilViewCount == m_pHeader->cDepthStencilViews, "Internal loading error: mismatched DSV count." );
    VBD( m_pEffect->m_DepthStencilBlockCount == m_pHeader->cDepthStencilBlocks, "Internal loading error: mismatched depth-stencil state count." );
    VBD( m_pEffect->m_BlendBlockCount == m_pHeader->cBlendStateBlocks, "Internal loading error: mismatched blend state count." );
    VBD( m_pEffect->m_RasterizerBlockCount == m_pHeader->cRasterizerStateBlocks, "Internal loading error: mismatched rasterizer state count." );
    VBD( m_pEffect->m_SamplerBlockCount == m_pHeader->cSamplers, "Internal loading error: mismatched sampler count." );
    VBD( m_pEffect->m_StringCount == m_pHeader->cStrings, "Internal loading error: mismatched string count." );

    // Uncomment if you really need this information
    // DPF(0, "Effect heap size: %d, reflection heap size: %d, allocations avoided: %d", m_EffectMemory, m_ReflectionMemory, m_BulkHeap.m_cAllocations);
    
lExit:
    return hr;
}

// position in buffer is lost on error
HRESULT CEffectLoader::LoadStringAndAddToPool(__out_ecount_full(1) char **ppString, UINT  dwOffset)
{
    HRESULT hr = S_OK;
    char *pName;
    UINT  hash;
    SIZE_T oldPos;
    CEffect::CStringHashTable::CIterator iter;
    UINT  len;

    if (dwOffset == 0)
    {
        *ppString = NULL;
        goto lExit;
    }

    oldPos = m_msUnstructured.GetPosition();

    VHD( m_msUnstructured.ReadAtOffset(dwOffset, (LPCSTR *) &pName), "Invalid pEffectBuffer: cannot read string." );
    len = (UINT)strlen(pName);
    hash = ComputeHash((BYTE *)pName, len);
    if (FAILED(m_pEffect->m_pStringPool->FindValueWithHash(pName, hash, &iter)))
    {
        D3DXASSERT( m_pEffect->m_pPooledHeap != NULL );
        VN( (*ppString) = new(*m_pEffect->m_pPooledHeap) char[len + 1] );
        memcpy(*ppString, pName, len + 1);
        VHD( m_pEffect->m_pStringPool->AddValueWithHash(*ppString, hash), "Internal loading error: failed to add string to pool." );
    }
    else
    {
        *ppString = const_cast<LPSTR>(iter.GetData());
    }

    m_msUnstructured.Seek(oldPos);

lExit:
    return hr;
}

HRESULT CEffectLoader::LoadTypeAndAddToPool(SType **ppType, UINT  dwOffset)
{
    HRESULT hr = S_OK;
    SBinaryType *psType;
    SBinaryNumericType *pNumericType;
    EObjectType *pObjectType;
    UINT  cMembers, iMember, cInterfaces;
    UINT  oBaseClassType;
    SType temporaryType;
    CEffect::CTypeHashTable::CIterator iter;
    BYTE *pHashBuffer;
    UINT  hash;
    SVariable *pTempMembers = NULL;
    
    m_HashBuffer.Empty();

    VHD( m_msUnstructured.ReadAtOffset(dwOffset, sizeof(SBinaryType), (void**) &psType), "Invalid pEffectBuffer: cannot read type." );
    VHD( LoadStringAndAddToPool(&temporaryType.pTypeName, psType->oTypeName), "Invalid pEffectBuffer: cannot read type name." );
    temporaryType.VarType = psType->VarType;
    temporaryType.Elements = psType->Elements;
    temporaryType.TotalSize = psType->TotalSize;
    temporaryType.Stride = psType->Stride;
    temporaryType.PackedSize = psType->PackedSize;

    // sanity check elements, size, stride, etc.
    UINT  cElements = max(1, temporaryType.Elements);
    VBD( cElements * temporaryType.Stride == AlignToPowerOf2(temporaryType.TotalSize, SType::c_RegisterSize), "Invalid pEffectBuffer: invalid type size." );
    VBD( temporaryType.Stride % SType::c_RegisterSize == 0, "Invalid pEffectBuffer: invalid type stride." );
    VBD( temporaryType.PackedSize <= temporaryType.TotalSize && temporaryType.PackedSize % cElements == 0, "Invalid pEffectBuffer: invalid type packed size." );

    switch(temporaryType.VarType)
    {
    case EVT_Object:
        VHD( m_msUnstructured.Read((void**) &pObjectType, sizeof(UINT)), "Invalid pEffectBuffer: cannot read object type." );
        temporaryType.ObjectType = *pObjectType;
        VBD( temporaryType.VarType > EOT_Invalid && temporaryType.VarType < EOT_Count, "Invalid pEffectBuffer: invalid object type." );
        
        VN( pHashBuffer = m_HashBuffer.AddRange(sizeof(temporaryType.VarType) + sizeof(temporaryType.Elements) + 
            sizeof(temporaryType.pTypeName) + sizeof(temporaryType.ObjectType)) );
        memcpy(pHashBuffer, &temporaryType.VarType, sizeof(temporaryType.VarType)); 
        pHashBuffer += sizeof(temporaryType.VarType);
        memcpy(pHashBuffer, &temporaryType.Elements, sizeof(temporaryType.Elements)); 
        pHashBuffer += sizeof(temporaryType.Elements);
        memcpy(pHashBuffer, &temporaryType.pTypeName, sizeof(temporaryType.pTypeName)); 
        pHashBuffer += sizeof(temporaryType.pTypeName);
        memcpy(pHashBuffer, &temporaryType.ObjectType, sizeof(temporaryType.ObjectType)); 
        break;

    case EVT_Interface:
        temporaryType.InterfaceType = NULL; 

        VN( pHashBuffer = m_HashBuffer.AddRange(sizeof(temporaryType.VarType) + sizeof(temporaryType.Elements) + 
            sizeof(temporaryType.pTypeName) + sizeof(temporaryType.ObjectType)) );
        memcpy(pHashBuffer, &temporaryType.VarType, sizeof(temporaryType.VarType)); 
        pHashBuffer += sizeof(temporaryType.VarType);
        memcpy(pHashBuffer, &temporaryType.Elements, sizeof(temporaryType.Elements)); 
        pHashBuffer += sizeof(temporaryType.Elements);
        memcpy(pHashBuffer, &temporaryType.pTypeName, sizeof(temporaryType.pTypeName)); 
        pHashBuffer += sizeof(temporaryType.pTypeName);
        memcpy(pHashBuffer, &temporaryType.ObjectType, sizeof(temporaryType.ObjectType)); 
        break;

    case EVT_Numeric:
        VHD( m_msUnstructured.Read((void**) &pNumericType, sizeof(SBinaryNumericType)), "Invalid pEffectBuffer: cannot read numeric type." );
        temporaryType.NumericType = *pNumericType;
        VBD( temporaryType.NumericType.Rows >= 1 && temporaryType.NumericType.Rows <= 4 &&
             temporaryType.NumericType.Columns >= 1 && temporaryType.NumericType.Columns <= 4 &&
             temporaryType.NumericType.NumericLayout != ENL_Invalid && temporaryType.NumericType.NumericLayout < ENL_Count &&
             temporaryType.NumericType.ScalarType > EST_Invalid && temporaryType.NumericType.ScalarType < EST_Count,
             "Invalid pEffectBuffer: invalid numeric type.");

        if (temporaryType.NumericType.NumericLayout != ENL_Matrix)
        {
            VBD( temporaryType.NumericType.IsColumnMajor == FALSE, "Invalid pEffectBuffer: only matricies can be column major." );
        }

        VN( pHashBuffer = m_HashBuffer.AddRange(sizeof(temporaryType.VarType) + sizeof(temporaryType.Elements) + 
            sizeof(temporaryType.pTypeName) + sizeof(temporaryType.NumericType)) );
        memcpy(pHashBuffer, &temporaryType.VarType, sizeof(temporaryType.VarType)); 
        pHashBuffer += sizeof(temporaryType.VarType);
        memcpy(pHashBuffer, &temporaryType.Elements, sizeof(temporaryType.Elements)); 
        pHashBuffer += sizeof(temporaryType.Elements);
        memcpy(pHashBuffer, &temporaryType.pTypeName, sizeof(temporaryType.pTypeName)); 
        pHashBuffer += sizeof(temporaryType.pTypeName);
        memcpy(pHashBuffer, &temporaryType.NumericType, sizeof(temporaryType.NumericType)); 
        break;

    case EVT_Struct:
        VHD( m_msUnstructured.Read(&cMembers), "Invalid pEffectBuffer: cannot read struct." );

        temporaryType.StructType.Members = cMembers;

        VN( pTempMembers = NEW SVariable[cMembers] );
        temporaryType.StructType.pMembers = pTempMembers;
        
        // read up all of the member descriptors at once
        SBinaryType::SBinaryMember *psMember;
        VHD( m_msUnstructured.Read((void**) &psMember, cMembers * sizeof(*psMember)), "Invalid pEffectBuffer: cannot read struct members." );

        {
            // Determine if this type implements an interface
            VHD( m_msUnstructured.Read(&oBaseClassType), "Invalid pEffectBuffer: cannot read base class type." );
            VHD( m_msUnstructured.Read(&cInterfaces), "Invalid pEffectBuffer: cannot read interfaces." );
            if( cInterfaces > 0 )
            {
                temporaryType.StructType.ImplementsInterface = 1;
                temporaryType.StructType.HasSuperClass = ( oBaseClassType > 0 ) ? 1 : 0;
            }
            else if( oBaseClassType > 0 )
            {
                // Get parent type and copy its ImplementsInterface
                SType* pBaseClassType;
                VH( LoadTypeAndAddToPool(&pBaseClassType, oBaseClassType) );
                temporaryType.StructType.ImplementsInterface = pBaseClassType->StructType.ImplementsInterface;
                temporaryType.StructType.HasSuperClass = 1;
            }
            // Read (and ignore) the interface types
            UINT *poInterface;
            VHD( m_msUnstructured.Read((void**) &poInterface, cInterfaces * sizeof(poInterface)), "Invalid pEffectBuffer: cannot read interface types." );
        }

        UINT  totalSize;
        totalSize = 0;
        for (iMember=0; iMember<cMembers; iMember++)
        {   
            SVariable *pMember;
            
            pMember = temporaryType.StructType.pMembers + iMember;

            VBD( psMember[iMember].Offset == totalSize || 
                 psMember[iMember].Offset == AlignToPowerOf2(totalSize, SType::c_RegisterSize),
                 "Internal loading error: invalid member offset." );

            pMember->Data.Offset = psMember[iMember].Offset;

            VH( LoadTypeAndAddToPool(&pMember->pType, psMember[iMember].oType) );
            VH( LoadStringAndAddToPool(&pMember->pName, psMember[iMember].oName) );
            VH( LoadStringAndAddToPool(&pMember->pSemantic, psMember[iMember].oSemantic) );
            
            totalSize = psMember[iMember].Offset + pMember->pType->TotalSize;
        }
        VBD( AlignToPowerOf2(totalSize, SType::c_RegisterSize) == temporaryType.Stride, "Internal loading error: invlid type size." );

        VN( pHashBuffer = m_HashBuffer.AddRange(sizeof(temporaryType.VarType) + sizeof(temporaryType.Elements) + 
            sizeof(temporaryType.pTypeName) + sizeof(temporaryType.StructType.Members) + cMembers * sizeof(SVariable)) );

        memcpy(pHashBuffer, &temporaryType.VarType, sizeof(temporaryType.VarType)); 
        pHashBuffer += sizeof(temporaryType.VarType);
        memcpy(pHashBuffer, &temporaryType.Elements, sizeof(temporaryType.Elements)); 
        pHashBuffer += sizeof(temporaryType.Elements);
        memcpy(pHashBuffer, &temporaryType.pTypeName, sizeof(temporaryType.pTypeName)); 
        pHashBuffer += sizeof(temporaryType.pTypeName);
        memcpy(pHashBuffer, &temporaryType.StructType.Members, sizeof(temporaryType.StructType.Members)); 
        pHashBuffer += sizeof(temporaryType.StructType.Members);
        memcpy(pHashBuffer, temporaryType.StructType.pMembers, cMembers * sizeof(SVariable));
        break;

    default:
        D3DXASSERT(0);
        VHD( E_FAIL, "Internal loading error: invalid variable type." );
    }

    hash = ComputeHash(&m_HashBuffer[0], m_HashBuffer.GetSize());
    if (FAILED(m_pEffect->m_pTypePool->FindValueWithHash(&temporaryType, hash, &iter)))
    {
        D3DXASSERT( m_pEffect->m_pPooledHeap != NULL );

        // allocate real member array, if necessary
        if (temporaryType.VarType == EVT_Struct)
        {
            VN( temporaryType.StructType.pMembers = new(*m_pEffect->m_pPooledHeap) SVariable[temporaryType.StructType.Members] );
            memcpy(temporaryType.StructType.pMembers, pTempMembers, temporaryType.StructType.Members * sizeof(SVariable));
        }

        // allocate real type
        VN( (*ppType) = new(*m_pEffect->m_pPooledHeap) SType );
        memcpy(*ppType, &temporaryType, sizeof(temporaryType));
        ZeroMemory(&temporaryType, sizeof(temporaryType));
        VH( m_pEffect->m_pTypePool->AddValueWithHash(*ppType, hash) );
    }
    else
    {
        *ppType = iter.GetData();
    }

lExit:
    SAFE_DELETE_ARRAY(pTempMembers);
    return hr;
}

// Numeric data in annotations are tightly packed (unlike in CBs which follow D3D11 packing rules).  This unpacks them.
UINT CEffectLoader::UnpackData(BYTE *pDestData, BYTE *pSrcData, UINT  PackedDataSize, SType *pType, UINT  *pBytesRead)
{
    UINT  bytesRead = 0;
    UINT  i, j, k;
    UINT  registers, entries;
    HRESULT hr = S_OK;
    UINT  elementsToCopy = max(pType->Elements, 1);

    switch (pType->VarType)
    {
    case EVT_Struct:
        for (i = 0; i < elementsToCopy; ++ i)
        {
            for (j = 0; j < pType->StructType.Members; ++ j)
            {
                UINT  br;
                D3DXASSERT((UINT_PTR)pType->StructType.pMembers[j].pType == (UINT)(UINT_PTR)pType->StructType.pMembers[j].pType);
                D3DXASSERT(PackedDataSize > bytesRead);                    

                VH( UnpackData(pDestData + pType->StructType.pMembers[j].Data.Offset, 
                    pSrcData + bytesRead, PackedDataSize - bytesRead, 
                    pType->StructType.pMembers[j].pType, &br) );
                
                bytesRead += br;
            }
            pDestData += pType->Stride;
        }
        break;

    case EVT_Numeric:
        if (pType->NumericType.IsPackedArray)
        {
            // No support for packed arrays
            D3DXASSERT(0);
            VHD(E_FAIL, "Internal loading error: packed arrays are not supported." );
        }
        else
        {
            UINT  bytesToCopy;

            if (pType->NumericType.IsColumnMajor)
            {
                registers = pType->NumericType.Columns;
                entries = pType->NumericType.Rows;
                bytesToCopy = entries * registers * SType::c_ScalarSize;

                for (i = 0; i < elementsToCopy; ++ i)
                {
                    for (j = 0; j < registers; ++ j)
                    {
                        for (k = 0; k < entries; ++ k)
                        {
                            // type cast to an arbitrary scalar
                            ((UINT*)pDestData)[k] = ((UINT*)pSrcData)[k * registers + j];
                        }
                        pDestData += SType::c_RegisterSize; // advance to next register
                    }
                    pSrcData += bytesToCopy;
                    bytesRead += bytesToCopy;
                }
            }
            else
            {
                registers = pType->NumericType.Rows;
                entries = pType->NumericType.Columns;
                bytesToCopy = entries * SType::c_ScalarSize;

                for (i = 0; i < elementsToCopy; ++ i)
                {
                    for (j = 0; j < registers; ++ j)
                    {
                        memcpy(pDestData, pSrcData, bytesToCopy);

                        pDestData += SType::c_RegisterSize; // advance to next register
                        pSrcData += bytesToCopy;
                        bytesRead += bytesToCopy;
                    }
                }
            }
        }
        break;

    default:
        // shouldn't be called on non-struct/numeric types
        D3DXASSERT(0);
        VHD(E_FAIL, "Internal loading error: UnpackData should not be called on non-struct, non-numeric types." );
    }  

lExit:
    *pBytesRead = bytesRead;
    return hr;
}

// Read info from the compiled blob and initialize a numeric variable
HRESULT CEffectLoader::LoadNumericVariable(SConstantBuffer *pParentCB)
{
    HRESULT hr = S_OK;
    SBinaryNumericVariable *psVar;
    SGlobalVariable *pVar;
    SType *pType;
    void *pDefaultValue;

    // Read variable info
    VHD( m_msStructured.Read((void**) &psVar, sizeof(*psVar)), "Invalid pEffectBuffer: cannot read numeric variable." );
    VBD( m_pEffect->m_VariableCount < (m_pHeader->Effect.cObjectVariables + m_pHeader->Effect.cNumericVariables + m_pHeader->cInterfaceVariables),
        "Internal loading error: invalid variable counts.");
    pVar = &m_pEffect->m_pVariables[m_pEffect->m_VariableCount];
    
    // Get type
    VH( LoadTypeAndAddToPool(&pType, psVar->oType) );
    
    // Make sure the right polymorphic type is created
    VH( PlacementNewVariable(pVar, pType, FALSE) );

    if (psVar->Flags & D3DX11_EFFECT_VARIABLE_EXPLICIT_BIND_POINT)
    {
        pVar->ExplicitBindPoint = psVar->Offset;
    }
    else
    {
        pVar->ExplicitBindPoint = -1;
    }

    pVar->pEffect = m_pEffect;
    pVar->pType = pType;
    pVar->pCB = pParentCB;
    pVar->Data.pGeneric = pParentCB->pBackingStore + psVar->Offset;
    VBD( psVar->Offset + pVar->pType->TotalSize <= pVar->pCB->Size, "Invalid pEffectBuffer: invalid variable offset." );

    if (pType->VarType == EVT_Struct && pType->StructType.ImplementsInterface && !pParentCB->IsTBuffer)
    {
        pVar->MemberDataOffsetPlus4 = m_pEffect->m_MemberDataCount * sizeof(SMemberDataPointer) + 4;
        m_pEffect->m_MemberDataCount += max(pType->Elements,1);
    }

    // Get name & semantic
    VHD( GetStringAndAddToReflection(psVar->oName, &pVar->pName), "Invalid pEffectBuffer: cannot read variable name." );
    VHD( GetStringAndAddToReflection(psVar->oSemantic, &pVar->pSemantic), "Invalid pEffectBuffer: cannot read variable semantic." );

    // Ensure the variable fits in the CBuffer and doesn't overflow
    VBD( pType->TotalSize + psVar->Offset <= pParentCB->Size &&
         pType->TotalSize + psVar->Offset >= pType->TotalSize, "Invalid pEffectBuffer: variable does not fit in CB." );

    ZeroMemory(pVar->Data.pGeneric, pType->TotalSize);

    // Get default value
    if (0 != psVar->oDefaultValue)
    {
        UINT  bytesUnpacked;
        VHD( m_msUnstructured.ReadAtOffset(psVar->oDefaultValue, pType->PackedSize, &pDefaultValue), "Invalid pEffectBuffer: cannot read default value." );
        VH( UnpackData((BYTE*) pVar->Data.pGeneric, (BYTE*) pDefaultValue, pType->PackedSize, pType, &bytesUnpacked) );
        VBD( bytesUnpacked == pType->PackedSize, "Invalid pEffectBuffer: invalid type packed size.");
    }
    
    // We need to use offsets until we fixup
    pVar->Data.Offset = psVar->Offset;

    // Read annotations
    VH( LoadAnnotations(&pVar->AnnotationCount, &pVar->pAnnotations) );

    m_pEffect->m_VariableCount++;

lExit:
    return hr;
}

// Read info from the compiled blob and initialize a constant buffer
HRESULT CEffectLoader::LoadCBs()
{
    HRESULT hr = S_OK;
    UINT  iCB, iVar;

    for (iCB=0; iCB<m_pHeader->Effect.cCBs; iCB++)
    {
        SBinaryConstantBuffer *psCB;
        SConstantBuffer *pCB;

        VHD( m_msStructured.Read((void**) &psCB, sizeof(*psCB)), "Invalid pEffectBuffer: cannot read CB." );
        pCB = &m_pEffect->m_pCBs[iCB];

        VHD( GetStringAndAddToReflection(psCB->oName, &pCB->pName), "Invalid pEffectBuffer: cannot read CB name." );

        pCB->IsTBuffer = (psCB->Flags & SBinaryConstantBuffer::c_IsTBuffer) != 0 ? TRUE : FALSE;
        pCB->IsSingle = (psCB->Flags & SBinaryConstantBuffer::c_IsSingle) != 0 ? TRUE : FALSE;
        pCB->Size = psCB->Size;
        pCB->ExplicitBindPoint = psCB->ExplicitBindPoint;
        VBD( pCB->Size == AlignToPowerOf2(pCB->Size, SType::c_RegisterSize), "Invalid pEffectBuffer: CB size not a power of 2." );
        VN( pCB->pBackingStore = PRIVATENEW BYTE[pCB->Size] );
        
        pCB->MemberDataOffsetPlus4 = m_pEffect->m_MemberDataCount * sizeof(SMemberDataPointer) + 4;
        m_pEffect->m_MemberDataCount += 2;

        // point this CB to variables that it owns
        pCB->VariableCount = psCB->cVariables;
        if (pCB->VariableCount > 0)
        {
            pCB->pVariables = &m_pEffect->m_pVariables[m_pEffect->m_VariableCount];
        }
        else
        {
            pCB->pVariables = NULL;
        }

        // Read annotations
        VH( LoadAnnotations(&pCB->AnnotationCount, &pCB->pAnnotations) );

        for (iVar=0; iVar<psCB->cVariables; iVar++)
        {
            VH( LoadNumericVariable(pCB) );
        }
    }

    m_pEffect->m_CBCount = m_pHeader->Effect.cCBs;

lExit:
    return hr;
}

// Used by LoadAssignment to initialize members on load
HRESULT CEffectLoader::ExecuteConstantAssignment(SBinaryConstant *pConstant, void *pLHS, D3D10_SHADER_VARIABLE_TYPE lhsType)
{
    HRESULT hr = S_OK;

    switch(pConstant->Type)
    {
    case EST_UInt:
    case EST_Int:
    case EST_Bool:
        switch(lhsType)
        {
        case D3D10_SVT_BOOL:
        case D3D10_SVT_INT:
        case D3D10_SVT_UINT:
            *(UINT*) pLHS = pConstant->iValue;
            break;

        case D3D10_SVT_UINT8:
            *(BYTE*) pLHS = (BYTE) pConstant->iValue;
            break;

        case D3D10_SVT_FLOAT:
            *(float*) pLHS = (float) pConstant->iValue;
            break;

        default:
            VHD( E_FAIL, "Internal loading error: invalid left-hand assignment type." );
        }
        break;

    case EST_Float:
        switch(lhsType)
        {
        case D3D10_SVT_BOOL:
        case D3D10_SVT_INT:
        case D3D10_SVT_UINT:
            *(UINT*) pLHS = (UINT) pConstant->fValue;
            break;

        case D3D10_SVT_UINT8:
            *(BYTE*) pLHS = (BYTE) pConstant->fValue;
            break;

        case D3D10_SVT_FLOAT:
            *(float*) pLHS = pConstant->fValue;
            break;

        default:
            VHD( E_FAIL, "Internal loading error: invalid left-hand assignment type." );
        }
        break;

    default:
        VHD( E_FAIL, "Internal loading error: invalid left-hand assignment type." );
    }

lExit:
    return hr;
}


// Read info from the compiled blob and initialize a set of assignments
HRESULT CEffectLoader::LoadAssignments( UINT Assignments, SAssignment **ppAssignments, BYTE *pBackingStore, UINT *pRTVAssignments, UINT *pFinalAssignments )
{
    HRESULT hr = S_OK;
    UINT  i, j;

    SBinaryAssignment *psAssignments;
    UINT  finalAssignments = 0;             // the number of assignments worth keeping    
    UINT  renderTargetViewAssns = 0;        // Number of render target view assns, used by passes since SetRTV is a vararg call

    *pFinalAssignments = 0;
    if (pRTVAssignments)
        *pRTVAssignments = 0;

    VHD( m_msStructured.Read((void**) &psAssignments, sizeof(*psAssignments) * Assignments), "Invalid pEffectBuffer: cannot read assignments." );

    // allocate enough room to store all of the assignments (even though some may go unused)
    VN( (*ppAssignments) = PRIVATENEW SAssignment[Assignments] )
    
    //
    // In this loop, we read assignments 1-by-1, keeping some and discarding others.
    // We write to the "next" assignment which is given by &(*ppAssignments)[finalAssignments];
    // if an assignment is worth keeping, we increment finalAssignments.
    // This means that if you want to keep an assignment, you must be careful to initialize
    // all members of SAssignment because old values from preceding discarded assignments might remain.
    //
    for (i = 0; i < Assignments; ++ i)
    {
        SGlobalVariable *pVarArray, *pVarIndex, *pVar;
        const char *pGlobalVarName;
        SAssignment *pAssignment = &(*ppAssignments)[finalAssignments];
        BYTE *pLHS;

        VBD( psAssignments[i].iState < NUM_STATES, "Invalid pEffectBuffer: invalid assignment state." );
        VBD( psAssignments[i].Index < g_lvGeneral[psAssignments[i].iState].m_Indices, "Invalid pEffectBuffer: invalid assignment index." );

        pAssignment->LhsType = g_lvGeneral[psAssignments[i].iState].m_LhsType;

        // Count RenderTargetView assignments
        if (pAssignment->LhsType == ELHS_RenderTargetView)
            renderTargetViewAssns++;

        switch (g_lvGeneral[psAssignments[i].iState].m_Type)
        {
        case D3D10_SVT_UINT8:
            D3DXASSERT(g_lvGeneral[psAssignments[i].iState].m_Cols == 1); // BYTE arrays not supported
            pAssignment->DataSize = sizeof(BYTE);
            // Store an offset for destination instead of a pointer so that it's easy to relocate it later
            
            break;

        case D3D10_SVT_BOOL:
        case D3D10_SVT_INT:
        case D3D10_SVT_UINT:
        case D3D10_SVT_FLOAT:
            pAssignment->DataSize = SType::c_ScalarSize * g_lvGeneral[psAssignments[i].iState].m_Cols;
            break;

        case D3D10_SVT_RASTERIZER:
            pAssignment->DataSize = sizeof(SRasterizerBlock);
            break;

        case D3D10_SVT_DEPTHSTENCIL:
            pAssignment->DataSize = sizeof(SDepthStencilBlock);
            break;

        case D3D10_SVT_BLEND:
            pAssignment->DataSize = sizeof(SBlendBlock);
            break;

        case D3D10_SVT_VERTEXSHADER:
        case D3D10_SVT_GEOMETRYSHADER:
        case D3D10_SVT_PIXELSHADER:
        case D3D11_SVT_HULLSHADER:
        case D3D11_SVT_DOMAINSHADER:
        case D3D11_SVT_COMPUTESHADER:
            pAssignment->DataSize = sizeof(SShaderBlock);
            break;

        case D3D10_SVT_TEXTURE:
        case D3D10_SVT_TEXTURE1D:
        case D3D10_SVT_TEXTURE2D:
        case D3D10_SVT_TEXTURE2DMS:
        case D3D10_SVT_TEXTURE3D:
        case D3D10_SVT_TEXTURECUBE:
        case D3D10_SVT_TEXTURECUBEARRAY:
        case D3D11_SVT_BYTEADDRESS_BUFFER:
        case D3D11_SVT_STRUCTURED_BUFFER:
            pAssignment->DataSize = sizeof(SShaderResource);
            break;

        case D3D10_SVT_RENDERTARGETVIEW:
            pAssignment->DataSize = sizeof(SRenderTargetView);
            break;

        case D3D10_SVT_DEPTHSTENCILVIEW:
            pAssignment->DataSize = sizeof(SDepthStencilView);
            break;

        case D3D11_SVT_RWTEXTURE1D:
        case D3D11_SVT_RWTEXTURE1DARRAY:
        case D3D11_SVT_RWTEXTURE2D:
        case D3D11_SVT_RWTEXTURE2DARRAY:
        case D3D11_SVT_RWTEXTURE3D:
        case D3D11_SVT_RWBUFFER:
        case D3D11_SVT_RWBYTEADDRESS_BUFFER:
        case D3D11_SVT_RWSTRUCTURED_BUFFER:
        case D3D11_SVT_APPEND_STRUCTURED_BUFFER:
        case D3D11_SVT_CONSUME_STRUCTURED_BUFFER:
            pAssignment->DataSize = sizeof(SUnorderedAccessView);
            break;

        case D3D11_SVT_INTERFACE_POINTER:
            pAssignment->DataSize = sizeof(SInterface);
            break;

        default:
            D3DXASSERT(0);
            VHD( E_FAIL, "Internal loading error: invalid assignment type.");
        }

        UINT lhsStride;
        if( g_lvGeneral[psAssignments[i].iState].m_Stride > 0 )
            lhsStride = g_lvGeneral[psAssignments[i].iState].m_Stride;
        else
            lhsStride = pAssignment->DataSize;

        // Store only the destination offset so that the backing store pointers can be easily fixed up later
        pAssignment->Destination.Offset = g_lvGeneral[psAssignments[i].iState].m_Offset + lhsStride * psAssignments[i].Index;

        // As a result, you should use pLHS in this function instead of the destination pointer
        pLHS = pBackingStore + pAssignment->Destination.Offset;

        switch (psAssignments[i].AssignmentType)
        {
        case ECAT_Constant: // e.g. LHS = 1; or LHS = NULL;
            UINT  *pNumConstants;
            SBinaryConstant *pConstants;

            VHD( m_msUnstructured.ReadAtOffset(psAssignments[i].oInitializer, sizeof(UINT), (void**) &pNumConstants), "Invalid pEffectBuffer: cannot read NumConstants." );
            VHD( m_msUnstructured.Read((void **)&pConstants, sizeof(SBinaryConstant) * (*pNumConstants)), "Invalid pEffectBuffer: cannot read constants." );

            if(pAssignment->IsObjectAssignment())
            {
                // make sure this is a NULL assignment
                VBD( *pNumConstants == 1 && (pConstants[0].Type == EST_Int || pConstants[0].Type == EST_UInt) && pConstants[0].iValue == 0,
                    "Invalid pEffectBuffer: non-NULL constant assignment to object.");

                switch (pAssignment->LhsType)
                {
                case ELHS_DepthStencilBlock:
                    *((void **)pLHS) = &g_NullDepthStencil;
                    break;
                case ELHS_BlendBlock:
                    *((void **)pLHS) = &g_NullBlend;
                    break;
                case ELHS_RasterizerBlock:
                    *((void **)pLHS) = &g_NullRasterizer;
                    break;
                case ELHS_VertexShaderBlock:
                    *((void **)pLHS) = &g_NullVS;
                    break;
                case ELHS_PixelShaderBlock:
                    *((void **)pLHS) = &g_NullPS;
                    break;
                case ELHS_GeometryShaderBlock:
                    *((void **)pLHS) = &g_NullGS;
                    break;
                case ELHS_HullShaderBlock:
                    *((void **)pLHS) = &g_NullHS;
                    break;
                case ELHS_DomainShaderBlock:
                    *((void **)pLHS) = &g_NullDS;
                    break;
                case ELHS_ComputeShaderBlock:
                    *((void **)pLHS) = &g_NullCS;
                    break;
                case ELHS_Texture:
                    *((void **)pLHS) = &g_NullTexture;
                    break;
                case ELHS_DepthStencilView:
                    *((void **)pLHS) = &g_NullDepthStencilView;
                    break;
                case ELHS_RenderTargetView:
                    *((void **)pLHS) = &g_NullRenderTargetView;
                    break;
                default:
                    D3DXASSERT(0);
                }
            }
            else
            {
                VBD( *pNumConstants == g_lvGeneral[psAssignments[i].iState].m_Cols, "Internal loading error: mismatch constant count." );
                for (j = 0; j < *pNumConstants; ++ j)
                {
                    VH( ExecuteConstantAssignment(pConstants + j, pLHS, g_lvGeneral[psAssignments[i].iState].m_Type) );
                    pLHS += SType::c_ScalarSize; // arrays of constants will always be regular scalar sized, never byte-sized
                }
            }

            // Can get rid of this assignment
            break;

        case ECAT_Variable: // e.g. LHS = myVar;
            VHD( m_msUnstructured.ReadAtOffset(psAssignments[i].oInitializer, &pGlobalVarName), "Invalid pEffectBuffer: cannot read variable name." );

            VBD( pVar = m_pEffect->FindVariableByName(pGlobalVarName), "Loading error: cannot find variable name." );

            if (pAssignment->IsObjectAssignment())
            {
                VBD( pVar->pType->VarType == EVT_Object && 
                     GetSimpleParameterTypeFromObjectType(pVar->pType->ObjectType) == g_lvGeneral[psAssignments[i].iState].m_Type,
                     "Loading error: invalid variable type or object type." );

                // Write directly into the state block's backing store
                *((void **)pLHS) = pVar->Data.pGeneric;

                // Now we can get rid of this assignment
            }
            else
            {
                VBD( pVar->pType->BelongsInConstantBuffer(), "Invalid pEffectBuffer: assignment type mismatch." );

                pAssignment->DependencyCount = 1;
                VN( pAssignment->pDependencies = PRIVATENEW SAssignment::SDependency[pAssignment->DependencyCount] );
                pAssignment->pDependencies->pVariable = pVar;

                // Store an offset for numeric values instead of a pointer so that it's easy to relocate it later
                pAssignment->Source.Offset = pVar->Data.Offset;
                pAssignment->AssignmentType = ERAT_NumericVariable;

                // Can't get rid of this assignment
                ++ finalAssignments;
            }
            break;

        case ECAT_ConstIndex: // e.g. LHS = myGS[1]
            SBinaryAssignment::SConstantIndex *psConstIndex;

            VHD( m_msUnstructured.ReadAtOffset(psAssignments[i].oInitializer, sizeof(*psConstIndex), (void**) &psConstIndex),
                "Invalid pEffectBuffer: cannot read assignment initializer." );
            VHD( m_msUnstructured.ReadAtOffset(psConstIndex->oArrayName, &pGlobalVarName), "Invalid pEffectBuffer: cannot read array name." );

            VBD( pVarArray = m_pEffect->FindVariableByName(pGlobalVarName), "Loading error: cannot find array name." );

            if (pAssignment->IsObjectAssignment())
            {
                VBD( psConstIndex->Index < pVarArray->pType->Elements, "Invalid pEffectBuffer: out of bounds array index." );
                VBD( pVarArray->pType->VarType == EVT_Object && 
                     GetSimpleParameterTypeFromObjectType(pVarArray->pType->ObjectType) == g_lvGeneral[psAssignments[i].iState].m_Type,
                     "Loading error: invalid variable type or object type." );

                // Write directly into the state block's backing store
                *((void **)pLHS) = GetBlockByIndex(pVarArray->pType->VarType, pVarArray->pType->ObjectType, pVarArray->Data.pGeneric, psConstIndex->Index);
                VBD( NULL != *((void **)pLHS), "Internal loading error: invalid block." );

                // Now we can get rid of this assignment
            }
            else
            {
                VBD( pVarArray->pType->BelongsInConstantBuffer(), "Invalid pEffectBuffer: assignment type mismatch." );

                pAssignment->DependencyCount = 1;
                VN( pAssignment->pDependencies = PRIVATENEW SAssignment::SDependency[pAssignment->DependencyCount] );
                pAssignment->pDependencies->pVariable = pVarArray;

                CCheckedDword chkDataLen = psConstIndex->Index;
                UINT  dataLen;
                chkDataLen *= SType::c_ScalarSize;
                chkDataLen += pAssignment->DataSize;
                VHD( chkDataLen.GetValue(&dataLen), "Overflow: assignment size." );
                VBD( dataLen <= pVarArray->pType->TotalSize, "Internal loading error: assignment size mismatch" );

                pAssignment->Source.Offset = pVarArray->Data.Offset + psConstIndex->Index * SType::c_ScalarSize;

                // _NumericConstIndex is not used here because _NumericVariable 
                // does the same stuff in a more general fashion with no perf hit.  
                pAssignment->AssignmentType = ERAT_NumericVariable;

                // Can't get rid of this assignment
                ++ finalAssignments;
            }
            break;

        case ECAT_VariableIndex: // e.g. LHS = myVar[numLights];
            SBinaryAssignment::SVariableIndex *psVarIndex;

            VHD( m_msUnstructured.ReadAtOffset(psAssignments[i].oInitializer, sizeof(*psVarIndex), (void**) &psVarIndex),
                 "Invalid pEffectBuffer: cannot read assignment initializer." );
            VHD( m_msUnstructured.ReadAtOffset(psVarIndex->oArrayName, &pGlobalVarName), "Invalid pEffectBuffer: cannot read variable name." );
            VBD( pVarArray = m_pEffect->FindVariableByName(pGlobalVarName), "Loading error: cannot find variable name." );

            VHD( m_msUnstructured.ReadAtOffset(psVarIndex->oIndexVarName, &pGlobalVarName), "Invalid pEffectBuffer: cannot read index variable name." );
            VBD( pVarIndex = m_pEffect->FindVariableByName(pGlobalVarName), "Loading error: cannot find index variable name." );

            // Only support integer indices
            VBD( pVarIndex->pType->VarType == EVT_Numeric && (pVarIndex->pType->NumericType.ScalarType == EST_Int || pVarIndex->pType->NumericType.ScalarType == EST_UInt),
                 "Invalid pEffectBuffer: invalid index variable type.");
            VBD( pVarArray->pType->Elements > 0, "Invalid pEffectBuffer: array variable is not an array." );

            pVarIndex->pCB->IsUsedByExpression = TRUE;

            if (pAssignment->IsObjectAssignment())
            {
                VBD( pVarArray->pType->VarType == EVT_Object && 
                     GetSimpleParameterTypeFromObjectType(pVarArray->pType->ObjectType) == g_lvGeneral[psAssignments[i].iState].m_Type,
                     "Loading error: invalid variable type or object type." );

                // MaxElements is only 16-bits wide
                VBD( pVarArray->pType->Elements <= 0xFFFF, "Internal error: array size is too large." ); 
                pAssignment->MaxElements = pVarArray->pType->Elements;

                pAssignment->DependencyCount = 1;
                VN( pAssignment->pDependencies = PRIVATENEW SAssignment::SDependency[pAssignment->DependencyCount] );
                pAssignment->pDependencies[0].pVariable = pVarIndex;

                // Point this assignment to the start of the variable's object array.
                // When this assignment is dirty, we write the value of this pointer plus
                // the index given by its one dependency directly into the destination
                pAssignment->Source = pVarArray->Data;
                pAssignment->AssignmentType = ERAT_ObjectVariableIndex;
            }
            else
            {
                VBD( pVarArray->pType->BelongsInConstantBuffer(), "Invalid pEffectBuffer: assignment type mismatch." );

                pAssignment->DependencyCount = 2;
                VN( pAssignment->pDependencies = PRIVATENEW SAssignment::SDependency[pAssignment->DependencyCount] );
                pAssignment->pDependencies[0].pVariable = pVarIndex;
                pAssignment->pDependencies[1].pVariable = pVarArray;

                // When pVarIndex is updated, we update the source pointer.
                // When pVarArray is updated, we copy data from the source to the destination.
                pAssignment->Source.pGeneric = NULL;
                pAssignment->AssignmentType = ERAT_NumericVariableIndex;
            }

            // Can't get rid of this assignment
            ++ finalAssignments;

            break;

        case ECAT_ExpressionIndex:// e.g. LHS = myVar[a + b * c];
        case ECAT_Expression: // e.g. LHS = a + b * c;
            // we do not support FXLVM
            VHD( E_NOTIMPL, "FXLVM Expressions (complex assignments like myVar[i*2]) are not supported in Effects11." );
            break;

        case ECAT_InlineShader:
        case ECAT_InlineShader5:
            UINT  cbShaderBin;
            BYTE *pShaderBin;
            SShaderBlock *pShaderBlock;
            SAnonymousShader *pAnonShader;
            union
            {
                SBinaryAssignment::SInlineShader *psInlineShader;
                SBinaryShaderData5 *psInlineShader5;
            };

            // Inline shader assignments must be object types
            D3DXASSERT(pAssignment->IsObjectAssignment());

            C_ASSERT( offsetof(SBinaryAssignment::SInlineShader,oShader) == offsetof(SBinaryShaderData5,oShader) );
            C_ASSERT( offsetof(SBinaryAssignment::SInlineShader,oSODecl) == offsetof(SBinaryShaderData5,oSODecls) );
            if( psAssignments[i].AssignmentType == ECAT_InlineShader )
            {
                VHD( m_msUnstructured.ReadAtOffset(psAssignments[i].oInitializer, sizeof(*psInlineShader), (void**) &psInlineShader),
                     "Invalid pEffectBuffer: cannot read inline shader." );
            }
            else
            {
                VHD( m_msUnstructured.ReadAtOffset(psAssignments[i].oInitializer, sizeof(*psInlineShader5), (void**) &psInlineShader5),
                    "Invalid pEffectBuffer: cannot read inline shader." );
            }
            
            VBD( m_pEffect->m_ShaderBlockCount < m_pHeader->cTotalShaders, "Internal loading error: shader count is out incorrect." );
            VBD( m_pEffect->m_AnonymousShaderCount < m_pHeader->cInlineShaders, "Internal loading error: anonymous shader count is out incorrect." );

            pShaderBlock = &m_pEffect->m_pShaderBlocks[m_pEffect->m_ShaderBlockCount];
            pAnonShader = &m_pEffect->m_pAnonymousShaders[m_pEffect->m_AnonymousShaderCount];
            pAnonShader->pShaderBlock = pShaderBlock;

            ++ m_pEffect->m_ShaderBlockCount;
            ++ m_pEffect->m_AnonymousShaderCount;

            // Write directly into the state block's backing store
            *((void **)pLHS) = pShaderBlock;

            VHD( GetUnstructuredDataBlock(psInlineShader->oShader, &cbShaderBin, (void **) &pShaderBin), "Invalid pEffectBuffer: cannot read inline shader block." );

            if (cbShaderBin > 0)
            {
                VN( pShaderBlock->pReflectionData = PRIVATENEW SShaderBlock::SReflectionData );

                pShaderBlock->pReflectionData->BytecodeLength = cbShaderBin;
                pShaderBlock->pReflectionData->pBytecode = (BYTE*) pShaderBin;
                pShaderBlock->pReflectionData->pStreamOutDecls[0] =
                pShaderBlock->pReflectionData->pStreamOutDecls[1] =
                pShaderBlock->pReflectionData->pStreamOutDecls[2] =
                pShaderBlock->pReflectionData->pStreamOutDecls[3] = NULL;
                pShaderBlock->pReflectionData->RasterizedStream = 0;
                pShaderBlock->pReflectionData->IsNullGS = FALSE;
                pShaderBlock->pReflectionData->pReflection = NULL;
                pShaderBlock->pReflectionData->InterfaceParameterCount = 0;
                pShaderBlock->pReflectionData->pInterfaceParameters = NULL;
            }

            switch (pAssignment->LhsType)
            {
            case ELHS_PixelShaderBlock:
                pShaderBlock->pVT = &g_vtPS;
                VBD( psInlineShader->oSODecl == NULL, "Internal loading error: pixel shaders cannot have stream out decls." );
                break;
            
            case ELHS_GeometryShaderBlock:
                pShaderBlock->pVT = &g_vtGS;
                if( psAssignments[i].AssignmentType == ECAT_InlineShader )
                {
                    if (psInlineShader->oSODecl)
                    {
                        // This is a GS with SO
                        VHD( GetStringAndAddToReflection(psInlineShader->oSODecl, &pShaderBlock->pReflectionData->pStreamOutDecls[0]),
                             "Invalid pEffectBuffer: cannot read SO decl." );
                    }
                }
                else
                {
                    // This is a GS with addressable stream out
                    for( UINT iDecl=0; iDecl < psInlineShader5->cSODecls; ++iDecl )
                    {
                        if (psInlineShader5->oSODecls[iDecl])
                        {
                            VHD( GetStringAndAddToReflection(psInlineShader5->oSODecls[iDecl], &pShaderBlock->pReflectionData->pStreamOutDecls[iDecl]),
                                "Invalid pEffectBuffer: cannot read SO decl." );
                        }
                    }
                    pShaderBlock->pReflectionData->RasterizedStream = psInlineShader5->RasterizedStream;
                }
                break;

            case ELHS_VertexShaderBlock:
                pShaderBlock->pVT = &g_vtVS;
                VBD( psInlineShader->oSODecl == NULL, "Internal loading error: vertex shaders cannot have stream out decls." );
                break;

            case ELHS_HullShaderBlock:
                pShaderBlock->pVT = &g_vtHS;
                VBD( psInlineShader->oSODecl == NULL, "Internal loading error: hull shaders cannot have stream out decls." );
                break;

            case ELHS_DomainShaderBlock:
                pShaderBlock->pVT = &g_vtDS;
                VBD( psInlineShader->oSODecl == NULL, "Internal loading error: domain shaders cannot have stream out decls." );
                break;

            case ELHS_ComputeShaderBlock:
                pShaderBlock->pVT = &g_vtCS;
                VBD( psInlineShader->oSODecl == NULL, "Internal loading error: compute shaders cannot have stream out decls." );
                break;

            case ELHS_GeometryShaderSO:
                D3DXASSERT(0); // Should never happen

            default:
                VHD( E_FAIL, "Internal loading error: invalid shader type."  );
            }

            if( psAssignments[i].AssignmentType == ECAT_InlineShader5 )
            {
                pShaderBlock->pReflectionData->InterfaceParameterCount = psInlineShader5->cInterfaceBindings;
                VH( GetInterfaceParametersAndAddToReflection( psInlineShader5->cInterfaceBindings, psInlineShader5->oInterfaceBindings, &pShaderBlock->pReflectionData->pInterfaceParameters ) );
            }

            // Now we can get rid of this assignment
            break;

        default:
            D3DXASSERT(0);

        }
    }

    *pFinalAssignments = finalAssignments;
    if (pRTVAssignments)
        *pRTVAssignments = renderTargetViewAssns;

lExit:
    return hr;
}


// Read info from the compiled blob and initialize an object variable
HRESULT CEffectLoader::LoadObjectVariables()
{
    HRESULT hr = S_OK;
    UINT  iBlock;
    UINT  cBlocks;

    cBlocks = m_pHeader->Effect.cObjectVariables;

    for (iBlock=0; iBlock<cBlocks; iBlock++)
    {
        SBinaryObjectVariable *psBlock;
        SGlobalVariable *pVar;
        SType *pType;
        UINT  iElement;
        UINT  elementsToRead;
        CCheckedDword chkElementsTotal;
        UINT  elementsTotal;

        // Read variable info
        VHD( m_msStructured.Read((void**) &psBlock, sizeof(*psBlock)), "Invalid pEffectBuffer: cannot read object variable." );
        VBD( m_pEffect->m_VariableCount < (m_pHeader->Effect.cObjectVariables + m_pHeader->Effect.cNumericVariables + m_pHeader->cInterfaceVariables),
             "Internal loading error: variable count mismatch." );
        pVar = &m_pEffect->m_pVariables[m_pEffect->m_VariableCount];
        
        // Get type
        VH( LoadTypeAndAddToPool(&pType, psBlock->oType) );

        // Make sure the right polymorphic type is created
        VH( PlacementNewVariable(pVar, pType, FALSE) );

        pVar->pEffect = m_pEffect;
        pVar->pType = pType;
        pVar->pCB = NULL;
        pVar->ExplicitBindPoint = psBlock->ExplicitBindPoint;

        if( pType->IsStateBlockObject() )
        {
            pVar->MemberDataOffsetPlus4 = m_pEffect->m_MemberDataCount * sizeof(SMemberDataPointer) + 4;
            m_pEffect->m_MemberDataCount += max(pType->Elements,1);
        }

        // Get name
        VHD( GetStringAndAddToReflection(psBlock->oName, &pVar->pName), "Invalid pEffectBuffer: cannot read object variable name." );
        VHD( GetStringAndAddToReflection(psBlock->oSemantic, &pVar->pSemantic), "Invalid pEffectBuffer: cannot read object variable semantic." );

        m_pEffect->m_VariableCount++;
        elementsToRead = max(1, pType->Elements);
        chkElementsTotal = elementsToRead;

        if (pType->IsStateBlockObject())
        {
            // State blocks
            EBlockType blockType;
            UINT  *maxBlockCount;
            UINT  *currentBlockCount;

            switch (pType->ObjectType)
            {
            case EOT_Blend:
                pVar->Data.pBlock = &m_pEffect->m_pBlendBlocks[m_pEffect->m_BlendBlockCount];
                maxBlockCount = &m_pHeader->cBlendStateBlocks;
                currentBlockCount = &m_pEffect->m_BlendBlockCount;
                blockType = EBT_Blend;
                break;

            case EOT_DepthStencil:
                pVar->Data.pBlock = &m_pEffect->m_pDepthStencilBlocks[m_pEffect->m_DepthStencilBlockCount];
                maxBlockCount = &m_pHeader->cDepthStencilBlocks;
                currentBlockCount = &m_pEffect->m_DepthStencilBlockCount;
                blockType = EBT_DepthStencil;
                break;

            case EOT_Rasterizer:
                pVar->Data.pBlock = &m_pEffect->m_pRasterizerBlocks[m_pEffect->m_RasterizerBlockCount];
                maxBlockCount = &m_pHeader->cRasterizerStateBlocks;
                currentBlockCount = &m_pEffect->m_RasterizerBlockCount;
                blockType = EBT_Rasterizer;
                break;

            default:
                VB(pType->IsSampler());
                pVar->Data.pBlock = &m_pEffect->m_pSamplerBlocks[m_pEffect->m_SamplerBlockCount];
                maxBlockCount = &m_pHeader->cSamplers;
                currentBlockCount = &m_pEffect->m_SamplerBlockCount;
                blockType = EBT_Sampler;
            }

            chkElementsTotal += *currentBlockCount;
            VHD( chkElementsTotal.GetValue(&elementsTotal), "Overflow: vaiable elements." );
            VBD( elementsTotal <= *maxBlockCount, "Internal loading error: element count overflow." );
            
            *currentBlockCount += elementsToRead;

            for (iElement = 0; iElement < elementsToRead; ++ iElement)
            {
                SBaseBlock *pCurrentBlock;
                UINT  cAssignments;
                
                pCurrentBlock = (SBaseBlock *) GetBlockByIndex(pVar->pType->VarType, pVar->pType->ObjectType, pVar->Data.pGeneric, iElement);
                VBD( NULL != pCurrentBlock, "Internal loading error: find state block." );

                pCurrentBlock->BlockType = blockType;

                VHD( m_msStructured.Read(&cAssignments), "Invalid pEffectBuffer: cannot read state block assignments." );

                VH( LoadAssignments( cAssignments, &pCurrentBlock->pAssignments, (BYTE*)pCurrentBlock, NULL, &pCurrentBlock->AssignmentCount ) );
            }
        }
        else if (pType->IsShader())
        {
            // Shaders

            chkElementsTotal += m_pEffect->m_ShaderBlockCount;
            VHD( chkElementsTotal.GetValue(&elementsTotal), "Overflow: shader block count." );
            VBD( elementsTotal <= m_pHeader->cTotalShaders, "Invalid pEffectBuffer: shader count mismatch." );

            pVar->Data.pShader = &m_pEffect->m_pShaderBlocks[m_pEffect->m_ShaderBlockCount];

            for (iElement=0; iElement<elementsToRead; iElement++)
            {
                UINT  cbShaderBin;
                void *pShaderBin;
                SShaderBlock *pShaderBlock;

                union
                {
                    UINT *pOffset;
                    SBinaryGSSOInitializer *psInlineGSSO4;
                    SBinaryShaderData5 *psInlineShader5;
                };

                C_ASSERT( offsetof(SBinaryGSSOInitializer,oShader) == 0 );
                C_ASSERT( offsetof(SBinaryShaderData5,oShader) == 0 );


                pShaderBlock = &m_pEffect->m_pShaderBlocks[m_pEffect->m_ShaderBlockCount];
                m_pEffect->m_ShaderBlockCount++;

                // Get shader binary
                switch (pType->ObjectType)
                {
                case EOT_VertexShader:
                case EOT_GeometryShader:
                case EOT_PixelShader:
                    VHD( m_msStructured.Read((void**)&pOffset, sizeof(*pOffset)), "Invalid pEffectBuffer: cannot read shader block." );
                    break;

                case EOT_GeometryShaderSO:
                    VHD( m_msStructured.Read((void**)&psInlineGSSO4, sizeof(*psInlineGSSO4)), "Invalid pEffectBuffer: cannot read inline GS with SO." );
                    break;

                case EOT_VertexShader5:
                case EOT_GeometryShader5:
                case EOT_HullShader5:
                case EOT_DomainShader5:
                case EOT_PixelShader5:
                case EOT_ComputeShader5:
                    VHD( m_msStructured.Read((void**)&psInlineShader5, sizeof(*psInlineShader5)), "Invalid pEffectBuffer: cannot read inline shader." );
                    break;

                default:
                    VH( E_FAIL );
                }

                VHD( GetUnstructuredDataBlock(*pOffset, &cbShaderBin, &pShaderBin), "Invalid pEffectBuffer: cannot read shader byte code." );

                if (cbShaderBin > 0)
                {
                    VN( pShaderBlock->pReflectionData = PRIVATENEW SShaderBlock::SReflectionData );

                    pShaderBlock->pReflectionData->BytecodeLength = cbShaderBin;
                    pShaderBlock->pReflectionData->pBytecode = (BYTE*) pShaderBin;
                    pShaderBlock->pReflectionData->pStreamOutDecls[0] =
                    pShaderBlock->pReflectionData->pStreamOutDecls[1] =
                    pShaderBlock->pReflectionData->pStreamOutDecls[2] =
                    pShaderBlock->pReflectionData->pStreamOutDecls[3] = NULL;
                    pShaderBlock->pReflectionData->RasterizedStream = 0;
                    pShaderBlock->pReflectionData->IsNullGS = FALSE;
                    pShaderBlock->pReflectionData->pReflection = NULL;
                    pShaderBlock->pReflectionData->InterfaceParameterCount = 0;
                    pShaderBlock->pReflectionData->pInterfaceParameters = NULL;
                }

                switch (pType->ObjectType)
                {
                case EOT_PixelShader:
                    pShaderBlock->pVT = &g_vtPS;
                    break;

                case EOT_GeometryShaderSO:
                    // Get StreamOut decl
                    //VH( m_msStructured.Read(&dwOffset) );
                    if (cbShaderBin > 0)
                    {
                        VHD( GetStringAndAddToReflection(psInlineGSSO4->oSODecl, &pShaderBlock->pReflectionData->pStreamOutDecls[0]),
                             "Invalid pEffectBuffer: cannot read stream out decl." );
                    }
                    pShaderBlock->pVT = &g_vtGS;
                    break;

                case EOT_VertexShader5:
                case EOT_GeometryShader5:
                case EOT_HullShader5:
                case EOT_DomainShader5:
                case EOT_PixelShader5:
                case EOT_ComputeShader5:
                    // Get StreamOut decls
                    if (cbShaderBin > 0)
                    {
                        for( UINT iDecl=0; iDecl < psInlineShader5->cSODecls; ++iDecl )
                        {
                            VHD( GetStringAndAddToReflection(psInlineShader5->oSODecls[iDecl], &pShaderBlock->pReflectionData->pStreamOutDecls[iDecl]),
                                 "Invalid pEffectBuffer: cannot read stream out decls." );
                        }
                        pShaderBlock->pReflectionData->RasterizedStream = psInlineShader5->RasterizedStream;
                        pShaderBlock->pReflectionData->InterfaceParameterCount = psInlineShader5->cInterfaceBindings;
                        VH( GetInterfaceParametersAndAddToReflection( psInlineShader5->cInterfaceBindings, psInlineShader5->oInterfaceBindings, &pShaderBlock->pReflectionData->pInterfaceParameters ) );
                    }
                    switch (pType->ObjectType)
                    {
                    case EOT_VertexShader5:
                        pShaderBlock->pVT = &g_vtVS;
                        break;
                    case EOT_GeometryShader5:
                        pShaderBlock->pVT = &g_vtGS;
                        break;
                    case EOT_HullShader5:
                        pShaderBlock->pVT = &g_vtHS;
                        break;
                    case EOT_DomainShader5:
                        pShaderBlock->pVT = &g_vtDS;
                        break;
                    case EOT_PixelShader5:
                        pShaderBlock->pVT = &g_vtPS;
                        break;
                    case EOT_ComputeShader5:
                        pShaderBlock->pVT = &g_vtCS;
                        break;
                    default:
                        VH( E_FAIL );
                    }
                    break;

                case EOT_GeometryShader:
                    pShaderBlock->pVT = &g_vtGS;
                    break;

                case EOT_VertexShader:
                    pShaderBlock->pVT = &g_vtVS;
                    break;

                default:
                    VHD( E_FAIL, "Invalid pEffectBuffer: invalid shader type." );
                }
            }
        }
        else if (pType->IsObjectType(EOT_String))
        {
            // Strings
            
            chkElementsTotal += m_pEffect->m_StringCount;
            VHD( chkElementsTotal.GetValue(&elementsTotal), "Overflow: string object count." );
            VBD( elementsTotal <= m_pHeader->cStrings, "Invalid pEffectBuffer: string count mismatch." );

            pVar->Data.pString = &m_pEffect->m_pStrings[m_pEffect->m_StringCount];

            for (iElement=0; iElement<elementsToRead; iElement++)
            {
                UINT  dwOffset;
                SString *pString;

                pString = &m_pEffect->m_pStrings[m_pEffect->m_StringCount];
                m_pEffect->m_StringCount++;

                // Get string
                VHD( m_msStructured.Read(&dwOffset), "Invalid pEffectBuffer: cannot read string offset." );
                VHD( GetStringAndAddToReflection(dwOffset, &pString->pString), "Invalid pEffectBuffer: cannot read string." );
            }
        }
        else if (pType->IsShaderResource())
        {   
            // Textures/buffers
            
            chkElementsTotal += m_pEffect->m_ShaderResourceCount;
            VHD( chkElementsTotal.GetValue(&elementsTotal), "Overflow: SRV object count." );
            VBD( elementsTotal <= m_pHeader->cShaderResources, "Invalid pEffectBuffer: SRV count mismatch." );

            pVar->Data.pShaderResource = &m_pEffect->m_pShaderResources[m_pEffect->m_ShaderResourceCount];
            m_pEffect->m_ShaderResourceCount += elementsToRead;
        }
        else if (pType->IsUnorderedAccessView())
        {   
            // UnorderedAccessViews

            chkElementsTotal += m_pEffect->m_UnorderedAccessViewCount;
            VHD( chkElementsTotal.GetValue(&elementsTotal), "Overflow: UAV object count." );
            VBD( elementsTotal <= m_pHeader->cUnorderedAccessViews, "Invalid pEffectBuffer: UAV count mismatch." );

            pVar->Data.pUnorderedAccessView = &m_pEffect->m_pUnorderedAccessViews[m_pEffect->m_UnorderedAccessViewCount];
            m_pEffect->m_UnorderedAccessViewCount += elementsToRead;
        }
        else if (pType->IsRenderTargetView())
        {            
            // RenderTargets

            chkElementsTotal += m_pEffect->m_RenderTargetViewCount;
            VHD( chkElementsTotal.GetValue(&elementsTotal), "Overflow: RTV object count." );
            VBD( elementsTotal <= m_pHeader->cRenderTargetViews, "Invalid pEffectBuffer: RTV count mismatch." );

            pVar->Data.pRenderTargetView = &m_pEffect->m_pRenderTargetViews[m_pEffect->m_RenderTargetViewCount];
            m_pEffect->m_RenderTargetViewCount += elementsToRead;
        }
        else if (pType->IsDepthStencilView())
        {            
            // DepthStencilViews

            chkElementsTotal += m_pEffect->m_DepthStencilViewCount;
            VHD( chkElementsTotal.GetValue(&elementsTotal), "Overflow: DSV object count." );
            VBD( elementsTotal <= m_pHeader->cDepthStencilViews, "Invalid pEffectBuffer: DSV count mismatch." );

            pVar->Data.pDepthStencilView = &m_pEffect->m_pDepthStencilViews[m_pEffect->m_DepthStencilViewCount];
            m_pEffect->m_DepthStencilViewCount += elementsToRead;
        }
        else
        {
            VHD( E_FAIL, "Invalid pEffectBuffer: DSV count mismatch." );
        }

        // Read annotations
        VH( LoadAnnotations(&pVar->AnnotationCount, &pVar->pAnnotations) );
    }
lExit:
    return hr;
}


// Read info from the compiled blob and initialize an interface variable
HRESULT CEffectLoader::LoadInterfaceVariables()
{
    HRESULT hr = S_OK;
    UINT  iBlock;
    UINT  cBlocks;

    cBlocks = m_pHeader->cInterfaceVariables;

    for (iBlock=0; iBlock<cBlocks; iBlock++)
    {
        SBinaryInterfaceVariable *psBlock;
        SGlobalVariable *pVar;
        SType *pType;
        UINT  elementsToRead;
        CCheckedDword chkElementsTotal;
        UINT  elementsTotal;
        void *pDefaultValue;

        // Read variable info
        VHD( m_msStructured.Read((void**) &psBlock, sizeof(*psBlock)), "Invalid pEffectBuffer: cannot read interface block." );
        VBD( m_pEffect->m_VariableCount < (m_pHeader->Effect.cObjectVariables + m_pHeader->Effect.cNumericVariables + m_pHeader->cInterfaceVariables),
             "Internal loading error: variable count mismatch." );
        pVar = &m_pEffect->m_pVariables[m_pEffect->m_VariableCount];

        // Get type
        VH( LoadTypeAndAddToPool(&pType, psBlock->oType) );

        // Make sure the right polymorphic type is created
        VH( PlacementNewVariable(pVar, pType, FALSE) );

        pVar->pEffect = m_pEffect;
        pVar->pType = pType;
        pVar->pCB = NULL;
        pVar->ExplicitBindPoint = (UINT)-1;
        pVar->pSemantic = NULL;

        // Get name
        VHD( GetStringAndAddToReflection(psBlock->oName, &pVar->pName), "Invalid pEffectBuffer: cannot read interface name." );

        m_pEffect->m_VariableCount++;
        elementsToRead = max(1, pType->Elements);
        chkElementsTotal = elementsToRead;

        VBD( pType->IsInterface(), "Internal loading error: invlaid type for interface." );

        chkElementsTotal += m_pEffect->m_InterfaceCount;
        VHD( chkElementsTotal.GetValue(&elementsTotal), "Overflow: interface count." );
        VBD( elementsTotal <= m_pHeader->cInterfaceVariableElements, "Invalid pEffectBuffer: interface count mismatch." );

        pVar->Data.pInterface = &m_pEffect->m_pInterfaces[m_pEffect->m_InterfaceCount];
        m_pEffect->m_InterfaceCount += elementsToRead;

        // Get default value
        if (0 != psBlock->oDefaultValue)
        {
            VHD( m_msUnstructured.ReadAtOffset(psBlock->oDefaultValue, elementsToRead * sizeof(SBinaryInterfaceInitializer), &pDefaultValue),
                 "Invalid pEffectBuffer: cannot read interface initializer offset." );
            for( UINT i=0; i < elementsToRead; i++ )
            {
                SBinaryInterfaceInitializer* pInterfaceInit = &((SBinaryInterfaceInitializer*)pDefaultValue)[i];
                LPCSTR pClassInstanceName;
                VHD( m_msUnstructured.ReadAtOffset(pInterfaceInit->oInstanceName, &pClassInstanceName), "Invalid pEffectBuffer: cannot read interface initializer." );

                SGlobalVariable *pCIVariable = m_pEffect->FindVariableByName(pClassInstanceName);
                VBD( pCIVariable != NULL, "Loading error: cannot find class instance for interface initializer." );
                VBD( pCIVariable->pType->IsClassInstance(), "Loading error: variable type mismatch for interface initializer." );
                if( pInterfaceInit->ArrayIndex == (UINT)-1 )
                {
                    VBD( pCIVariable->pType->Elements == 0, "Loading error: array mismatch for interface initializer." );
                    pVar->Data.pInterface[i].pClassInstance = (SClassInstanceGlobalVariable*)pCIVariable;
                }
                else
                {
                    VBD( pCIVariable->pType->Elements > 0, "Loading error: array mismatch for interface initializer." );
                    VBD( pInterfaceInit->ArrayIndex < pCIVariable->pType->Elements, "Loading error: array index out of range." );

                    SMember* pMember = (SMember*)pCIVariable->GetElement( pInterfaceInit->ArrayIndex );
                    VBD( pMember->IsValid(), "Loading error: cannot find member by name." );
                    VBD( pMember->pType->IsClassInstance(), "Loading error: member type mismatch for interface initializer." );
                    pVar->Data.pInterface[i].pClassInstance = (SClassInstanceGlobalVariable*)pMember;
                }
            }
        }


        // Read annotations
        VH( LoadAnnotations(&pVar->AnnotationCount, &pVar->pAnnotations) );
    }
lExit:
    return hr;
}


// Read info from the compiled blob and initialize a group (and contained techniques and passes)
HRESULT CEffectLoader::LoadGroups()
{
    HRESULT hr = S_OK;
    UINT iGroup;
    UINT TechniquesInEffect = 0;

    for( iGroup=0; iGroup<m_pHeader->cGroups; iGroup++ )
    {
        SGroup *pGroup = &m_pEffect->m_pGroups[iGroup];
        SBinaryGroup *psGroup;

        // Read group info
        VHD( m_msStructured.Read((void**) &psGroup, sizeof(*psGroup)), "Invalid pEffectBuffer: cannot read group." );
        pGroup->TechniqueCount = psGroup->cTechniques;
        VN( pGroup->pTechniques = PRIVATENEW STechnique[pGroup->TechniqueCount] );
        VHD( GetStringAndAddToReflection(psGroup->oName, &pGroup->pName), "Invalid pEffectBuffer: cannot read group name." );

        if( pGroup->pName == NULL )
        {
            VBD( m_pEffect->m_pNullGroup == NULL, "Internal loading error: multiple NULL groups." );
            m_pEffect->m_pNullGroup = pGroup;
        }

        // Read annotations
        VH( LoadAnnotations(&pGroup->AnnotationCount, &pGroup->pAnnotations) );

        UINT iTechnique;
        for( iTechnique=0; iTechnique < psGroup->cTechniques; iTechnique++ )
        {
            VH( LoadTechnique( &pGroup->pTechniques[iTechnique] ) );
        }
        TechniquesInEffect += psGroup->cTechniques;
    }

    VBD( TechniquesInEffect == m_pHeader->cTechniques, "Loading error: technique count mismatch." );
    m_pEffect->m_TechniqueCount = m_pHeader->cTechniques;
    m_pEffect->m_GroupCount = m_pHeader->cGroups;

lExit:
    return hr;
}


// Read info from the compiled blob and initialize a technique (and contained passes)
HRESULT CEffectLoader::LoadTechnique( STechnique* pTech )
{
    HRESULT hr = S_OK;
    UINT  iPass;

    SBinaryTechnique *psTech;

    // Read technique info
    VHD( m_msStructured.Read((void**) &psTech, sizeof(*psTech)), "Invalid pEffectBuffer: cannot read technique." );
    pTech->PassCount = psTech->cPasses;
    VN( pTech->pPasses = PRIVATENEW SPassBlock[pTech->PassCount] );
    VHD( GetStringAndAddToReflection(psTech->oName, &pTech->pName), "Invalid pEffectBuffer: cannot read technique name." );

    // Read annotations
    VH( LoadAnnotations(&pTech->AnnotationCount, &pTech->pAnnotations) );

    for (iPass=0; iPass<psTech->cPasses; iPass++)
    {
        SBinaryPass *psPass;
        SPassBlock *pPass = &pTech->pPasses[iPass];

        // Read pass info
        VHD( m_msStructured.Read((void**) &psPass, sizeof(SBinaryPass)), "Invalid pEffectBuffer: cannot read pass." );
        VHD( GetStringAndAddToReflection(psPass->oName, &pPass->pName), "Invalid pEffectBuffer: cannot read pass name." );
        
        // Read annotations
        VH( LoadAnnotations(&pPass->AnnotationCount, &pPass->pAnnotations) );

        VH( LoadAssignments( psPass->cAssignments, &pPass->pAssignments, (BYTE*)pPass, &pPass->BackingStore.RenderTargetViewCount, &pPass->AssignmentCount ) );
        VBD( pPass->BackingStore.RenderTargetViewCount <= D3D11_SIMULTANEOUS_RENDER_TARGET_COUNT, "Invalid pEffectBuffer: too many RTVs in pass." );

        // Initialize other pass information
        pPass->pEffect = m_pEffect;
        pPass->BlockType = EBT_Pass;
    }

lExit:
    return hr;
}


// Read info from the compiled blob and initialize a set of annotations
HRESULT CEffectLoader::LoadAnnotations(UINT  *pcAnnotations, SAnnotation **ppAnnotations)
{
    HRESULT hr = S_OK;
    UINT  cAnnotations, i, oData;
    SAnnotation *pAnnotations = NULL;

    VHD( m_msStructured.Read(&cAnnotations), "Invalid pEffectBuffer: cannot read anootation count." );

    if (cAnnotations)
    {
        UINT  annotationsSize;
        CCheckedDword chkAnnotationsSize;

        chkAnnotationsSize = cAnnotations;
        chkAnnotationsSize *= sizeof(SAnnotation);
        VHD( chkAnnotationsSize.GetValue(&annotationsSize), "Overflow in annotations."  );
        
        // we allocate raw bytes for annotations because they are polymorphic types that need to be placement new'ed
        VN( pAnnotations = (SAnnotation *) PRIVATENEW BYTE[annotationsSize] );
        
        for (i=0; i<cAnnotations; i++)
        {
            SBinaryAnnotation *psAnnotation;
            SAnnotation *pAn = &pAnnotations[i];
            SType *pType;

            VHD( m_msStructured.Read((void**) &psAnnotation, sizeof(SBinaryAnnotation)), "Invalid pEffectBuffer: cannot read annotation."  );

            VH( LoadTypeAndAddToPool(&pType, psAnnotation->oType) );

            // Make sure the right polymorphic type is created
            VH( PlacementNewVariable(pAn, pType, TRUE) );

            pAn->pEffect = m_pEffect;
            pAn->pType = pType;

            VHD( GetStringAndAddToReflection(psAnnotation->oName, &pAn->pName), "Invalid pEffectBuffer: cannot read annotation name."  );

            if (pType->IsObjectType(EOT_String))
            {
                UINT  cElements = max(1, pType->Elements);
                UINT  j;
                VN( pAn->Data.pString = PRIVATENEW SString[cElements] );
                for (j = 0; j < cElements; ++ j)
                {
                    // Read initializer offset
                    VHD( m_msStructured.Read(&oData), "Invalid pEffectBuffer: cannot read string."  );
                    VHD( GetStringAndAddToReflection(oData, &pAn->Data.pString[j].pString), "Invalid pEffectBuffer: cannot read string initializer."  );
                }
            }
            else if (pType->BelongsInConstantBuffer())
            {
                void *pDefaultValue;
                UINT  bytesUnpacked;
                
                // Read initializer offset
                VHD( m_msStructured.Read(&oData), "Invalid pEffectBuffer: cannot read annotation."  );

                VBD( oData != 0, "Invalid pEffectBuffer: invalid anotation offset." );

                VN( pAn->Data.pGeneric = PRIVATENEW BYTE[pType->TotalSize] );
                ZeroMemory(pAn->Data.pGeneric, pType->TotalSize);
                VHD( m_msUnstructured.ReadAtOffset(oData, pType->PackedSize, &pDefaultValue), "Invalid pEffectBuffer: cannot read variable default value."  );
                VH( UnpackData((BYTE*) pAn->Data.pGeneric, (BYTE*) pDefaultValue, pType->PackedSize, pType, &bytesUnpacked) );
                VBD( bytesUnpacked == pType->PackedSize, "Invalid pEffectBuffer: packed sizes to not match." );
            }
            else
            {
                VHD( E_FAIL, "Invalid pEffectBuffer: invalid annotation type." );
            }
        }
    }

    *pcAnnotations = cAnnotations;
    *ppAnnotations = pAnnotations;
lExit:

    return hr;
}

//////////////////////////////////////////////////////////////////////////
// Build shader block dependencies from shader metadata
//////////////////////////////////////////////////////////////////////////

//
// Grabs shader resource dependency information from the bytecode of the shader
// (cbuffer, tbuffer, texture, buffer, sampler, and UAV dependencies),
// and sets up the given SShaderBlock to point to the dependencies within the effect
//
HRESULT CEffectLoader::GrabShaderData(SShaderBlock *pShaderBlock)
{
    HRESULT hr = S_OK;
    UINT  i, j;
    CEffectVector<SRange> vRanges[ER_Count], *pvRange;
    SRange *pRange = NULL;
    CEffectVector<SConstantBuffer*> vTBuffers;
    
    //////////////////////////////////////////////////////////////////////////
    // Step 1: iterate through the resource binding structures and build
    // an "optimized" list of all of the dependencies

    D3D11_SHADER_DESC ShaderDesc;
    pShaderBlock->pReflectionData->pReflection->GetDesc( &ShaderDesc );

    // Since we have the shader desc, let's find out if this is a NULL GS
    if( D3D11_SHVER_GET_TYPE( ShaderDesc.Version ) == D3D11_SHVER_VERTEX_SHADER && pShaderBlock->GetShaderType() == EOT_GeometryShader )
    {
        pShaderBlock->pReflectionData->IsNullGS = TRUE;
    }

    pShaderBlock->CBDepCount = pShaderBlock->ResourceDepCount = pShaderBlock->TBufferDepCount = pShaderBlock->SampDepCount = 0;
    pShaderBlock->UAVDepCount = pShaderBlock->InterfaceDepCount = 0;

    for(i = 0; i < ShaderDesc.BoundResources; i++)
    {
        LPCSTR pName;
        UINT bindPoint, size;
        ERanges eRange;
        SShaderResource *pShaderResource = NULL;
        SUnorderedAccessView *pUnorderedAccessView = NULL;
        SSamplerBlock *pSampler = NULL;
        SConstantBuffer *pCB = NULL;
        SVariable *pVariable = NULL;
        BOOL isFX9TextureLoad = FALSE;
        D3D11_SHADER_INPUT_BIND_DESC ResourceDesc;

        pShaderBlock->pReflectionData->pReflection->GetResourceBindingDesc( i, &ResourceDesc );

        // HUGE ASSUMPTION: the bindpoints we read in the shader metadata are sorted;
        // i.e. bindpoints are steadily increasing
        // If this assumption is not met, then we will hit an assert below

        pName = ResourceDesc.Name;
        bindPoint = ResourceDesc.BindPoint;
        size = ResourceDesc.BindCount;

        switch( ResourceDesc.Type )
        {
        case D3D10_SIT_CBUFFER:
            eRange = ER_CBuffer;
            
            pCB = m_pEffect->FindCB(pName);
            VBD( NULL != pCB, "Loading error: cannot find cbuffer." );
            VBD( size == 1, "Loading error: cbuffer arrays are not supported." );
            break;

        case D3D10_SIT_TBUFFER:
            eRange = ER_Texture;
            
            pCB = m_pEffect->FindCB(pName);
            VBD( NULL != pCB, "Loading error: cannot find tbuffer." );
            VBD( FALSE != pCB->IsTBuffer, "Loading error: cbuffer found where tbuffer is expected." );
            VBD( size == 1, "Loading error: tbuffer arrays are not supported." );
            pShaderResource = &pCB->TBuffer;
            break;

        case D3D10_SIT_TEXTURE: 
        case D3D11_SIT_STRUCTURED:
        case D3D11_SIT_BYTEADDRESS:
            eRange = ER_Texture;

            pVariable = m_pEffect->FindVariableByNameWithParsing(pName);
            VBD( pVariable != NULL, "Loading error: cannot find SRV variable." );
            UINT elements;
            elements = max(1, pVariable->pType->Elements);
            VBD( size <= elements, "Loading error: SRV array size mismatch." );

            if (pVariable->pType->IsShaderResource())
            {
                // this is just a straight texture assignment
                pShaderResource = pVariable->Data.pShaderResource;
            }
            else
            {
                // This is a FX9/HLSL9-style texture load instruction that specifies only a sampler
                VBD( pVariable->pType->IsSampler(), "Loading error: shader dependency is neither an SRV nor sampler.");
                isFX9TextureLoad = TRUE;
                pSampler = pVariable->Data.pSampler;
                // validate that all samplers actually used (i.e. based on size, not elements) in this variable have a valid TEXTURE assignment
                for (j = 0; j < size; ++ j)
                {
                    if (NULL == pSampler[j].BackingStore.pTexture)
                    {
                        // print spew appropriately for samplers vs sampler arrays
                        if (0 == pVariable->pType->Elements)
                        {
                            DPF(0, "%s: Sampler %s does not have a texture bound to it, even though the sampler is used in a DX9-style texture load instruction", g_szEffectLoadArea, pName);
                        }
                        else
                        {
                            DPF(0, "%s: Sampler %s[%d] does not have a texture bound to it, even though the sampler array is used in a DX9-style texture load instruction", g_szEffectLoadArea, pName, j);
                        }
                        
                        VH( E_FAIL );
                    }
                }
            }
            break;

        case D3D11_SIT_UAV_RWTYPED:
        case D3D11_SIT_UAV_RWSTRUCTURED:
        case D3D11_SIT_UAV_RWBYTEADDRESS:
        case D3D11_SIT_UAV_APPEND_STRUCTURED:
        case D3D11_SIT_UAV_CONSUME_STRUCTURED:
        case D3D11_SIT_UAV_RWSTRUCTURED_WITH_COUNTER:
            eRange = ER_UnorderedAccessView;

            pVariable = m_pEffect->FindVariableByNameWithParsing(pName);
            VBD( pVariable != NULL, "Loading error: cannot find UAV variable." );
            VBD( size <= max(1, pVariable->pType->Elements), "Loading error: UAV array index out of range." );
            VBD( pVariable->pType->IsUnorderedAccessView(), "Loading error: UAV variable expected." );
            pUnorderedAccessView = pVariable->Data.pUnorderedAccessView;
            break;

        case D3D10_SIT_SAMPLER:
            eRange = ER_Sampler;

            pVariable = m_pEffect->FindVariableByNameWithParsing(pName);
            VBD( pVariable != NULL, "Loading error: cannot find sampler variable." );
            VBD( size <= max(1, pVariable->pType->Elements), "Loading error: sampler array index out of range." );
            VBD( pVariable->pType->IsSampler(), "Loading error: sampler variable expected." );
            pSampler = pVariable->Data.pSampler;
            break;

        default:
            VHD( E_FAIL, "Internal loading error: unexpected shader dependency type." );
        };

        //
        // Here's where the "optimized" part comes in; whenever there's
        // a resource dependency, see if it's located contiguous to
        // an existing resource dependency and merge them together
        // if possible
        //
        UINT  rangeCount;
        pvRange = &vRanges[eRange];
        rangeCount = pvRange->GetSize();

        if ( rangeCount > 0 )
        {
            // Can we continue an existing range?
            pRange = &( (*pvRange)[rangeCount - 1] );

            // Make sure that bind points are strictly increasing,
            // otherwise this algorithm breaks and we'd get worse runtime performance
            D3DXASSERT(pRange->last <= bindPoint);

            if ( pRange->last != bindPoint )
            {
                if( eRange != ER_UnorderedAccessView )
                {
                    // No we can't. Begin a new range by setting rangeCount to 0 and triggering the next IF
                    rangeCount = 0;
                }
                else
                {
                    // UAVs will always be located in one range, as they are more expensive to set
                    while(pRange->last < bindPoint)
                    {
                        VHD( pRange->vResources.Add(&g_NullUnorderedAccessView), "Internal loading error: cannot add UAV to range." );
                        pRange->last++;
                    }
                }
            }
        }

        if ( rangeCount == 0 )
        {
            VN( pRange = pvRange->Add() );
            pRange->start = bindPoint;
        }

        pRange->last = bindPoint + size;

        switch( ResourceDesc.Type )
        {
        case D3D10_SIT_CBUFFER:
            VHD( pRange->vResources.Add(pCB), "Internal loading error: cannot add cbuffer to range." );
            break;
        case D3D10_SIT_TBUFFER:
            VHD( pRange->vResources.Add(pShaderResource), "Internal loading error: cannot add tbuffer to range." );
            VHD( vTBuffers.Add( (SConstantBuffer*)pCB ), "Internal loading error: cannot add tbuffer to vector." );
            break;
        case D3D10_SIT_TEXTURE:
        case D3D11_SIT_STRUCTURED:
        case D3D11_SIT_BYTEADDRESS:
            if (isFX9TextureLoad)
            {
                // grab all of the textures from each sampler
                for (j = 0; j < size; ++ j)
                {
                    VHD( pRange->vResources.Add(pSampler[j].BackingStore.pTexture), "Internal loading error: cannot add SRV to range." );
                }
            }
            else
            {
                // add the whole array
                for (j = 0; j < size; ++ j)
                {
                    VHD( pRange->vResources.Add(pShaderResource + j), "Internal loading error: cannot add SRV to range." );
                }
            }
            break;
        case D3D11_SIT_UAV_RWTYPED:
        case D3D11_SIT_UAV_RWSTRUCTURED:
        case D3D11_SIT_UAV_RWBYTEADDRESS:
        case D3D11_SIT_UAV_APPEND_STRUCTURED:
        case D3D11_SIT_UAV_CONSUME_STRUCTURED:
        case D3D11_SIT_UAV_RWSTRUCTURED_WITH_COUNTER:
            // add the whole array
            for (j = 0; j < size; ++ j)
            {
                VHD( pRange->vResources.Add(pUnorderedAccessView + j), "Internal loading error: cannot add UAV to range." );
            }
            break;
        case D3D10_SIT_SAMPLER:
            // add the whole array
            for (j = 0; j < size; ++ j)
            {
                VHD( pRange->vResources.Add(pSampler + j), "Internal loading error: cannot add sampler to range." );
            }
            break;
        default:
            VHD( E_FAIL, "Internal loading error: unexpected shader dependency type." );
        }
    }


    //////////////////////////////////////////////////////////////////////////
    // Step 2: iterate through the interfaces and build
    // an "optimized" list of all of the dependencies

    UINT NumInterfaces = pShaderBlock->pReflectionData->pReflection->GetNumInterfaceSlots();
    UINT CurInterfaceParameter = 0;
    if( NumInterfaces > 0 )
    {
        D3DXASSERT( ShaderDesc.ConstantBuffers > 0 );

        for( i=0; i < ShaderDesc.ConstantBuffers; i++ )
        {
            ID3D11ShaderReflectionConstantBuffer* pCB = pShaderBlock->pReflectionData->pReflection->GetConstantBufferByIndex(i);
            VN( pCB );
            D3D11_SHADER_BUFFER_DESC CBDesc;
            VHD( pCB->GetDesc( &CBDesc ), "Internal loading error: cannot get CB desc." );
            if( CBDesc.Type != D3D11_CT_INTERFACE_POINTERS )
            {
                continue;
            }

            for( UINT iVar=0; iVar < CBDesc.Variables; iVar++ )
            {
                ID3D11ShaderReflectionVariable* pInterfaceVar = pCB->GetVariableByIndex( iVar );
                VN( pInterfaceVar );
                D3D11_SHADER_VARIABLE_DESC InterfaceDesc;
                pInterfaceVar->GetDesc( &InterfaceDesc );

                LPCSTR pName;
                UINT bindPoint, size;
                SGlobalVariable *pVariable = NULL;
                SInterface *pInterface = NULL;
                UINT VariableElements;

                pName = InterfaceDesc.Name;
                bindPoint = InterfaceDesc.StartOffset;
                size = InterfaceDesc.Size;

                if( bindPoint == (UINT)-1 )
                {
                    continue;
                }

                D3DXASSERT( InterfaceDesc.uFlags & D3D11_SVF_INTERFACE_POINTER );
                if( InterfaceDesc.uFlags & D3D11_SVF_INTERFACE_PARAMETER )
                {
                    // This interface pointer is a parameter to the shader
                    if( pShaderBlock->pReflectionData->InterfaceParameterCount == 0 )
                    {
                        // There may be no interface parameters in this shader if it was compiled but had no interfaced bound to it.
                        // The shader cannot be set (correctly) in any pass.
                        continue;
                    }
                    else
                    {
                        VBD( CurInterfaceParameter < pShaderBlock->pReflectionData->InterfaceParameterCount,
                             "Internal loading error: interface count mismatch.");
                        SShaderBlock::SInterfaceParameter* pInterfaceInfo;
                        pInterfaceInfo = &pShaderBlock->pReflectionData->pInterfaceParameters[CurInterfaceParameter];
                        ++CurInterfaceParameter;
                        SGlobalVariable *pParent = m_pEffect->FindVariableByName(pInterfaceInfo->pName);
                        VBD( pParent != NULL, "Loading error: cannot find parent type." );
                        if( pInterfaceInfo->Index == (UINT)-1 )
                        {
                            pVariable = pParent;
                            VariableElements = pVariable->pType->Elements;
                        }
                        else
                        {
                            // We want a specific index of the variable (ex. "MyVar[2]")
                            VBD( size == 1, "Loading error: interface array type mismatch." );
                            pVariable = (SGlobalVariable*)pParent->GetElement( pInterfaceInfo->Index );
                            VBD( pVariable->IsValid(), "Loading error: interface array index out of range." );
                            VariableElements = 0;
                        }
                    }
                }
                else
                {
                    // This interface pointer is a global interface used in the shader
                    pVariable = m_pEffect->FindVariableByName(pName);
                    VBD( pVariable != NULL, "Loading error: cannot find interface variable." );
                    VariableElements = pVariable->pType->Elements;
                }
                VBD( size <= max(1, VariableElements), "Loading error: interface array size mismatch." );
                if( pVariable->pType->IsInterface() )
                {
                    pInterface = pVariable->Data.pInterface;
                }
                else if( pVariable->pType->IsClassInstance() )
                {
                    // For class instances, we create background interfaces which point to the class instance.  This is done so
                    // the shader can always expect SInterface dependencies, rather than a mix of SInterfaces and class instances
                    VN( pInterface = PRIVATENEW SInterface[size] );
                    if( VariableElements == 0 )
                    {
                        D3DXASSERT( size == 1 );
                        pInterface[0].pClassInstance = (SClassInstanceGlobalVariable*)pVariable;
                        m_BackgroundInterfaces.Add( &pInterface[0] );
                    }
                    else
                    {
                        // Fill each element of the SInstance array individually
                        VBD( size == VariableElements, "Loading error: class instance array size mismatch." );
                        for( UINT iElement=0; iElement < size; iElement++ )
                        {
                            SGlobalVariable *pElement = (SGlobalVariable*)pVariable->GetElement( iElement );
                            VBD( pElement->IsValid(), "Internal loading error: class instance array index out of range." );
                            pInterface[iElement].pClassInstance = (SClassInstanceGlobalVariable*)pElement;
                            m_BackgroundInterfaces.Add( &pInterface[iElement] );
                        }
                    }
                }
                else
                {
                    VHD( E_FAIL, "Loading error: invalid interface initializer variable type.");
                }

                //
                // Here's where the "optimized" part comes in; whenever there's
                // a resource dependency, see if it's located contiguous to
                // an existing resource dependency and merge them together
                // if possible
                //
                UINT  rangeCount;
                pvRange = &vRanges[ER_Interfaces];
                rangeCount = pvRange->GetSize();

                VBD( rangeCount <= 1, "Internal loading error: invalid range count." );

                if ( rangeCount == 0 )
                {
                    VN( pRange = pvRange->Add() );
                    pRange->start = pRange->last = 0;
                }
                else
                {
                    pRange = &( (*pvRange)[0] );
                }

                if( bindPoint < pRange->last )
                {
                    // add interfaces into the range that already exists
                    VBD( bindPoint + size < pRange->last, "Internal loading error: range overlap." );
                    for( j = 0; j < size; ++ j )
                    {
                        pRange->vResources[j + bindPoint] = pInterface + j;
                    }
                }
                else
                {
                    // add interfaces to the end of the range

                    // add missing interface slots, if necessary
                    while(pRange->last < bindPoint)
                    {
                        VHD( pRange->vResources.Add(&g_NullInterface), "Internal loading error: cannot add NULL interface to range." );
                        pRange->last++;
                    }

                    D3DXASSERT( bindPoint == pRange->last );
                    for( j=0; j < size; ++ j )
                    {
                        VHD( pRange->vResources.Add(pInterface + j), "Internal loading error: cannot at interface to range." );
                    }
                    pRange->last = bindPoint + size;
                }
            }

            // There is only one interface cbuffer
            break;
        }
    }

    //////////////////////////////////////////////////////////////////////////
    // Step 3: allocate room in pShaderBlock for all of the dependency 
    // pointers and then hook them up

    pShaderBlock->SampDepCount = vRanges[ ER_Sampler ].GetSize();
    pShaderBlock->CBDepCount = vRanges[ ER_CBuffer ].GetSize();
    pShaderBlock->InterfaceDepCount = vRanges[ ER_Interfaces ].GetSize();
    pShaderBlock->ResourceDepCount = vRanges[ ER_Texture ].GetSize();
    pShaderBlock->UAVDepCount = vRanges[ ER_UnorderedAccessView ].GetSize();
    pShaderBlock->TBufferDepCount = vTBuffers.GetSize();

    VN( pShaderBlock->pSampDeps = PRIVATENEW SShaderSamplerDependency[pShaderBlock->SampDepCount] );
    VN( pShaderBlock->pCBDeps = PRIVATENEW SShaderCBDependency[pShaderBlock->CBDepCount] );
    VN( pShaderBlock->pInterfaceDeps = PRIVATENEW SInterfaceDependency[pShaderBlock->InterfaceDepCount] );
    VN( pShaderBlock->pResourceDeps = PRIVATENEW SShaderResourceDependency[pShaderBlock->ResourceDepCount] );
    VN( pShaderBlock->pUAVDeps = PRIVATENEW SUnorderedAccessViewDependency[pShaderBlock->UAVDepCount] );
    VN( pShaderBlock->ppTbufDeps = PRIVATENEW SConstantBuffer*[pShaderBlock->TBufferDepCount] );

    for (i=0; i<pShaderBlock->CBDepCount; ++i)
    {
        SShaderCBDependency *pDep = &pShaderBlock->pCBDeps[i];

        pRange = &vRanges[ER_CBuffer][i];

        pDep->StartIndex = pRange->start;
        pDep->Count = pRange->last - pDep->StartIndex;
        pDep->ppFXPointers = PRIVATENEW SConstantBuffer*[ pDep->Count ];
        pDep->ppD3DObjects = PRIVATENEW ID3D11Buffer*[ pDep->Count ];

        D3DXASSERT(pDep->Count == pRange->vResources.GetSize());
        for (j=0; j<pDep->Count; ++j)
        {
            pDep->ppFXPointers[j] = (SConstantBuffer *)pRange->vResources[j];
            pDep->ppD3DObjects[j] = NULL;
        }
    }

    for (i=0; i<pShaderBlock->SampDepCount; ++i)
    {
        SShaderSamplerDependency *pDep = &pShaderBlock->pSampDeps[i];

        pRange = &vRanges[ER_Sampler][i];

        pDep->StartIndex = pRange->start;
        pDep->Count = pRange->last - pDep->StartIndex;
        pDep->ppFXPointers = PRIVATENEW SSamplerBlock*[ pDep->Count ];
        pDep->ppD3DObjects = PRIVATENEW ID3D11SamplerState*[ pDep->Count ];

        D3DXASSERT(pDep->Count == pRange->vResources.GetSize());
        for (j=0; j<pDep->Count; ++j)
        {
            pDep->ppFXPointers[j] = (SSamplerBlock *) pRange->vResources[j];
            pDep->ppD3DObjects[j] = NULL;
        }
    }

    for (i=0; i<pShaderBlock->InterfaceDepCount; ++i)
    {
        SInterfaceDependency *pDep = &pShaderBlock->pInterfaceDeps[i];

        pRange = &vRanges[ER_Interfaces][i];

        pDep->StartIndex = pRange->start;
        pDep->Count = pRange->last - pDep->StartIndex;
        pDep->ppFXPointers = PRIVATENEW SInterface*[ pDep->Count ];
        pDep->ppD3DObjects = PRIVATENEW ID3D11ClassInstance*[ pDep->Count ];

        D3DXASSERT(pDep->Count == pRange->vResources.GetSize());
        for (j=0; j<pDep->Count; ++j)
        {
            pDep->ppFXPointers[j] = (SInterface *) pRange->vResources[j];
            pDep->ppD3DObjects[j] = NULL;
        }
    }

    for (i=0; i<pShaderBlock->ResourceDepCount; ++i)
    {
        SShaderResourceDependency *pDep = &pShaderBlock->pResourceDeps[i];

        pRange = &vRanges[ER_Texture][i];

        pDep->StartIndex = pRange->start;
        pDep->Count = pRange->last - pDep->StartIndex;
        pDep->ppFXPointers = PRIVATENEW SShaderResource*[ pDep->Count ];
        pDep->ppD3DObjects = PRIVATENEW ID3D11ShaderResourceView*[ pDep->Count ];

        D3DXASSERT(pDep->Count == pRange->vResources.GetSize());
        for (j=0; j<pDep->Count; ++j)
        {
            pDep->ppFXPointers[j] = (SShaderResource *) pRange->vResources[j];
            pDep->ppD3DObjects[j] = NULL;
        }
    }

    for (i=0; i<pShaderBlock->UAVDepCount; ++i)
    {
        SUnorderedAccessViewDependency *pDep = &pShaderBlock->pUAVDeps[i];

        pRange = &vRanges[ER_UnorderedAccessView][i];

        pDep->StartIndex = pRange->start;
        pDep->Count = pRange->last - pDep->StartIndex;
        pDep->ppFXPointers = PRIVATENEW SUnorderedAccessView*[ pDep->Count ];
        pDep->ppD3DObjects = PRIVATENEW ID3D11UnorderedAccessView*[ pDep->Count ];

        D3DXASSERT(pDep->Count == pRange->vResources.GetSize());
        for (j=0; j<pDep->Count; ++j)
        {
            pDep->ppFXPointers[j] = (SUnorderedAccessView *) pRange->vResources[j];
            pDep->ppD3DObjects[j] = NULL;
        }
    }

    if (pShaderBlock->TBufferDepCount > 0)
    {
        memcpy(pShaderBlock->ppTbufDeps, &vTBuffers[0], pShaderBlock->TBufferDepCount * sizeof(SConstantBuffer*));
    }

lExit:
    return hr;
}

// Create shader reflection interface and grab dependency info
HRESULT CEffectLoader::BuildShaderBlock(SShaderBlock *pShaderBlock)
{
    HRESULT hr = S_OK;

    // unused shader block? that's not right
    VBD( pShaderBlock->pVT != NULL, "Internal loading error: NULL shader vtable." );

    D3DXASSERT(pShaderBlock->pD3DObject == NULL);

    if (NULL == pShaderBlock->pReflectionData)
    {
        // File contains a shader variable without an assigned shader, or this is a null assignment.
        // Usually, this is called by one of these guys:
        // SetVertexShader( NULL );
        // or 
        // vertexshader g_VS = NULL;
        return S_OK;
    }

    // Initialize the reflection interface
    VHD( D3DReflect( pShaderBlock->pReflectionData->pBytecode, pShaderBlock->pReflectionData->BytecodeLength, IID_ID3D11ShaderReflection, (void**)&pShaderBlock->pReflectionData->pReflection ),
         "Internal loading error: cannot create shader reflection object." );

    // Get dependencies
    VH( GrabShaderData( pShaderBlock ) );

    // Grab input signatures for VS
    if( EOT_VertexShader == pShaderBlock->GetShaderType() )
    {
        D3DXASSERT( pShaderBlock->pInputSignatureBlob == NULL );
        VHD( D3DGetInputSignatureBlob( pShaderBlock->pReflectionData->pBytecode, pShaderBlock->pReflectionData->BytecodeLength, &pShaderBlock->pInputSignatureBlob ),
             "Internal loading error: cannot get input signature." );
    }

lExit:
    return hr;
}

#undef PRIVATENEW


//////////////////////////////////////////////////////////////////////////
// Code to relocate data to private heaps (reflection & runtime effect)
//
// Important note about alignment: all reasonable chunks of data are 
// machine word aligned (that is, any piece of data moved as a whole is 
// aligned as a whole.  This means that when computing m_ReflectionMemory
// or m_EffectMemory, each addition is aligned.  This also means 
// that, when later relocating that same memory, you must call MoveData
// or MoveString on the same chunks that were aligned.  This is 
// because:   Align(a * b) != a * Align(b).
//////////////////////////////////////////////////////////////////////////

//////////////////////////////////////////////////////////////////////////
// Reflection reallocation code
//////////////////////////////////////////////////////////////////////////

HRESULT CEffectLoader::CalculateAnnotationSize(UINT  cAnnotations, SAnnotation *pAnnotations)
{
    HRESULT hr = S_OK;
    UINT  i;

    m_ReflectionMemory += AlignToPowerOf2(cAnnotations * sizeof(SAnnotation), c_DataAlignment);
    for (i=0; i<cAnnotations; i++)
    {
        if (pAnnotations[i].pType->BelongsInConstantBuffer())
        {
            m_ReflectionMemory += AlignToPowerOf2(pAnnotations[i].pType->TotalSize, c_DataAlignment);
        }
        else
        {
            VBD( pAnnotations[i].pType->IsObjectType(EOT_String), "Invalid pEffectBuffer: invalid annotation type." );

            UINT  cElements;
            cElements = max(1, pAnnotations[i].pType->Elements);
            
            m_ReflectionMemory += AlignToPowerOf2(cElements * sizeof(SString), c_DataAlignment);
            
        }
    }

lExit:
    return hr;
}

HRESULT CEffectLoader::ReallocateAnnotationData(UINT  cAnnotations, SAnnotation **ppAnnotations)
{
    HRESULT hr = S_OK;
    UINT  i;
    SAnnotation *pAnnotations;

    VHD( m_pReflection->m_Heap.MoveData((void**) ppAnnotations, cAnnotations * sizeof(SAnnotation)),
         "Internal loading error: cannot move annotation data." );
    pAnnotations = *ppAnnotations;

    for (i=0; i<cAnnotations; i++)
    {
        SAnnotation *pAn = &pAnnotations[i];
        pAn->pEffect = m_pEffect;

        VHD( m_pReflection->m_Heap.MoveString(&pAn->pName), "Internal loading error: cannot move annotation name." );

        // Reallocate type later
        if (pAn->pType->BelongsInConstantBuffer())
        {
            VHD( m_pReflection->m_Heap.MoveData( &pAn->Data.pGeneric, pAn->pType->TotalSize ), "Internal loading error: cannot move annotation data." );
        }
        else if (pAnnotations[i].pType->IsObjectType(EOT_String))
        {
            UINT  j;
            UINT  cElements = max(1, pAn->pType->Elements);
                        
            VHD( m_pReflection->m_Heap.MoveData((void**) &pAn->Data.pString, cElements * sizeof(SString)), "Internal loading error: cannot move annotation string." );
            for (j = 0; j < cElements; ++ j)
            {
                VHD( m_pReflection->m_Heap.MoveString(&pAn->Data.pString[j].pString), "Internal loading error: cannot move annotation string element." );
            }
        }
        else
        {
            VHD( E_FAIL, "Invalid pEffectBuffer: invalid annotation type." );
        }
    }

lExit:
    return hr;
}

HRESULT CEffectLoader::InitializeReflectionDataAndMoveStrings( UINT KnownSize )
{
    HRESULT hr = S_OK;
    UINT  i, j, k;
    UINT  cbStrings;
    CEffectHeap *pHeap = &m_pReflection->m_Heap;

    // Get byte counts
    cbStrings = m_pEffect->m_StringCount * sizeof( SString );

    if( KnownSize )
    {
        m_ReflectionMemory = KnownSize;
    }
    else
    {
        m_ReflectionMemory += AlignToPowerOf2(cbStrings, c_DataAlignment);

        for (i=0; i<m_pEffect->m_CBCount; i++)
        {
            VH( CalculateAnnotationSize(m_pEffect->m_pCBs[i].AnnotationCount, m_pEffect->m_pCBs[i].pAnnotations) );
        }

        for (i=0; i<m_pEffect->m_VariableCount; i++)
        {
            VH( CalculateAnnotationSize(m_pEffect->m_pVariables[i].AnnotationCount, m_pEffect->m_pVariables[i].pAnnotations) );
        }

        for (i=0; i<m_pEffect->m_GroupCount; i++)
        {
            VH( CalculateAnnotationSize(m_pEffect->m_pGroups[i].AnnotationCount, m_pEffect->m_pGroups[i].pAnnotations) );

            for (j=0; j<m_pEffect->m_pGroups[i].TechniqueCount; j++)
            {
                VH( CalculateAnnotationSize(m_pEffect->m_pGroups[i].pTechniques[j].AnnotationCount, m_pEffect->m_pGroups[i].pTechniques[j].pAnnotations) );

                for (k=0; k<m_pEffect->m_pGroups[i].pTechniques[j].PassCount; k++)
                {
                    VH( CalculateAnnotationSize(m_pEffect->m_pGroups[i].pTechniques[j].pPasses[k].AnnotationCount, m_pEffect->m_pGroups[i].pTechniques[j].pPasses[k].pAnnotations) );
                }
            }
        }

        // Calculate shader reflection data size
        for (i=0; i<m_pEffect->m_ShaderBlockCount; i++)
        {
            if (NULL != m_pEffect->m_pShaderBlocks[i].pReflectionData)
            {
                m_ReflectionMemory += AlignToPowerOf2(sizeof(SShaderBlock::SReflectionData), c_DataAlignment);
                m_ReflectionMemory += AlignToPowerOf2(m_pEffect->m_pShaderBlocks[i].pReflectionData->BytecodeLength, c_DataAlignment);
                // stream out decl is handled as a string, and thus its size is already factored because of GetStringAndAddToReflection
            }
        }
    }

    VHD( pHeap->ReserveMemory(m_ReflectionMemory), "Internal loading error: failed to reserve reflection memory." );

    // Strings are handled separately because we are moving them to reflection
    m_pOldStrings = m_pEffect->m_pStrings;
    VHD( pHeap->MoveData((void**) &m_pEffect->m_pStrings, cbStrings), "Internal loading error: cannot move string data." );
    for(i=0; i<m_pEffect->m_StringCount; i++)
    {
        VHD( pHeap->MoveString( &m_pEffect->m_pStrings[i].pString), "Internal loading error: cannot move string pointer." );
    }

lExit:
    return hr;
}

// Move all reflection data to private heap
HRESULT CEffectLoader::ReallocateReflectionData( bool Cloning )
{
    HRESULT hr = S_OK;
    UINT  i, j, k;
    CEffectHeap *pHeap = &m_pReflection->m_Heap;

    for(i=0; i<m_pEffect->m_CBCount; i++)
    {
        VHD( pHeap->MoveString( &m_pEffect->m_pCBs[i].pName ), "Internal loading error: cannot move CB name." );
        VH( ReallocateAnnotationData(m_pEffect->m_pCBs[i].AnnotationCount, &m_pEffect->m_pCBs[i].pAnnotations) );
    }

    for(i=0; i<m_pEffect->m_VariableCount; i++)
    {
        VHD( pHeap->MoveString( &m_pEffect->m_pVariables[i].pName ), "Internal loading error: cannot move variable name." );
        VHD( pHeap->MoveString( &m_pEffect->m_pVariables[i].pSemantic ), "Internal loading error: cannot move variable semantic." );
        VH( ReallocateAnnotationData(m_pEffect->m_pVariables[i].AnnotationCount, &m_pEffect->m_pVariables[i].pAnnotations) );
    }

    for(i=0; i<m_pEffect->m_GroupCount; i++)
    {
        VHD( pHeap->MoveString( &m_pEffect->m_pGroups[i].pName ), "Internal loading error: cannot move group name." );
        VH( ReallocateAnnotationData(m_pEffect->m_pGroups[i].AnnotationCount, &m_pEffect->m_pGroups[i].pAnnotations) );

        for(j=0; j<m_pEffect->m_pGroups[i].TechniqueCount; j++)
        {
            VHD( pHeap->MoveString( &m_pEffect->m_pGroups[i].pTechniques[j].pName ), "Internal loading error: cannot move technique name." );
            VH( ReallocateAnnotationData(m_pEffect->m_pGroups[i].pTechniques[j].AnnotationCount, &m_pEffect->m_pGroups[i].pTechniques[j].pAnnotations) );
            
            for(k=0; k<m_pEffect->m_pGroups[i].pTechniques[j].PassCount; k++)
            {
                VHD( pHeap->MoveString( &m_pEffect->m_pGroups[i].pTechniques[j].pPasses[k].pName ), "Internal loading error: cannot move pass name." );
                VH( ReallocateAnnotationData(m_pEffect->m_pGroups[i].pTechniques[j].pPasses[k].AnnotationCount, &m_pEffect->m_pGroups[i].pTechniques[j].pPasses[k].pAnnotations) );
            }
        }
    }

    if( !Cloning )
    {
        // When not cloning, every member in m_pMemberInterfaces is from a global variable, so we can take pName and pSemantic
        // from the parent variable, which were updated above
        for (i = 0; i < m_pEffect->m_pMemberInterfaces.GetSize(); ++ i)
        {
            SMember* pMember = m_pEffect->m_pMemberInterfaces[i];
            SGlobalVariable* pTopLevelEntity = (SGlobalVariable*)pMember->pTopLevelEntity;
            VH( FixupVariablePointer( &pTopLevelEntity ) );
            pMember->pName = pTopLevelEntity->pName;
            pMember->pSemantic = pTopLevelEntity->pSemantic;
        }
    }

    // Move shader bytecode
    for (i=0; i<m_pEffect->m_ShaderBlockCount; i++)
    {
        if (NULL != m_pEffect->m_pShaderBlocks[i].pReflectionData)
        {
            VHD( pHeap->MoveData((void**)&m_pEffect->m_pShaderBlocks[i].pReflectionData, sizeof(SShaderBlock::SReflectionData)),
                 "Internal loading error: cannot move shader reflection block." );
            VHD( pHeap->MoveData((void**)&m_pEffect->m_pShaderBlocks[i].pReflectionData->pBytecode, m_pEffect->m_pShaderBlocks[i].pReflectionData->BytecodeLength),
                 "Internal loading error: cannot move shader bytecode.");
            for( UINT iDecl=0; iDecl < D3D11_SO_STREAM_COUNT; ++iDecl )
            {
                VHD( pHeap->MoveString(&m_pEffect->m_pShaderBlocks[i].pReflectionData->pStreamOutDecls[iDecl]), "Internal loading error: cannot move SO decl." );
            }
            VH( pHeap->MoveInterfaceParameters(m_pEffect->m_pShaderBlocks[i].pReflectionData->InterfaceParameterCount, &m_pEffect->m_pShaderBlocks[i].pReflectionData->pInterfaceParameters ) );
        }
        
    }

lExit:
    return hr;
}

//////////////////////////////////////////////////////////////////////////
// Runtime effect reallocation code
//////////////////////////////////////////////////////////////////////////

template<class T> HRESULT CEffectLoader::ReallocateBlockAssignments(T* &pBlocks, UINT  cBlocks, T* pOldBlocks)
{
    HRESULT hr = S_OK;
    CEffectHeap *pHeap = &m_pEffect->m_Heap;
    UINT  i, j;

    for(i=0; i<cBlocks; i++)
    {
        T *pBlock = &pBlocks[i];
        VHD( pHeap->MoveData((void**) &pBlock->pAssignments, sizeof(SAssignment)*pBlock->AssignmentCount), "Internal loading error: cannot move assignment count." );

        for (j=0; j<pBlock->AssignmentCount; j++)
        {
            SAssignment *pAssignment = &pBlock->pAssignments[j];
            UINT  cbDeps;

            // When cloning, convert pointers back into offsets
            if( pOldBlocks )
            {
                T *pOldBlock = &pOldBlocks[i];
                pAssignment->Destination.Offset = (UINT)( (UINT_PTR)pAssignment->Destination.pGeneric - (UINT_PTR)pOldBlock ) ;
            }

            // Convert destination pointers from offset to real pointer
            pAssignment->Destination.pGeneric = (BYTE*) pBlock + pAssignment->Destination.Offset;

            // Make sure the data pointer points into the backing store
            VBD( pAssignment->Destination.pGeneric >= &pBlock->BackingStore && 
                 pAssignment->Destination.pGeneric < (BYTE*) &pBlock->BackingStore + sizeof(pBlock->BackingStore), 
                 "Internal loading error: assignment destination out of range." );

            // Fixup dependencies
            cbDeps = pAssignment->DependencyCount * sizeof(SAssignment::SDependency);
            VHD( pHeap->MoveData((void**) &pAssignment->pDependencies, cbDeps), "Internal loading error: cannot move assignment dependencies." );

            SGlobalVariable *pOldVariable = NULL;
            for(UINT  iDep=0; iDep<pAssignment->DependencyCount; iDep++)
            {
                SAssignment::SDependency *pDep = &pAssignment->pDependencies[iDep];
                // We ignore all but the last variable because below, we only use the last dependency
                pOldVariable = pDep->pVariable;
                VH( FixupVariablePointer(&pDep->pVariable) );
            }

            // Fixup source pointers
            switch(pAssignment->LhsType)
            {
            case ELHS_VertexShaderBlock:
            case ELHS_PixelShaderBlock:
            case ELHS_GeometryShaderBlock:
            case ELHS_HullShaderBlock:
            case ELHS_DomainShaderBlock:
            case ELHS_ComputeShaderBlock:
                VH( FixupShaderPointer(&pAssignment->Source.pShader) );
                break;

            case ELHS_DepthStencilBlock:
                VH( FixupDSPointer((SDepthStencilBlock**)&pAssignment->Source.pBlock) );
                break;
            case ELHS_BlendBlock:
                VH( FixupABPointer((SBlendBlock**) &pAssignment->Source.pBlock) );
                break;
            case ELHS_RasterizerBlock:
                VH( FixupRSPointer((SRasterizerBlock**) &pAssignment->Source.pBlock) );
                break;

            case ELHS_Texture:
                VH( FixupShaderResourcePointer((SShaderResource**) &pAssignment->Source.pShaderResource) );
                break;

            default:
                // Non-object assignment (must have at least one dependency or it would have been pruned by now)
                D3DXASSERT( !pAssignment->IsObjectAssignment() && pAssignment->DependencyCount > 0 );

                // Numeric variables must be relocated before this function is called
                
                switch (pAssignment->AssignmentType)
                {
                case ERAT_NumericVariable:
                case ERAT_NumericVariableIndex:
                    // the variable or variable array is always the last dependency in the chain
                    SGlobalVariable *pVariable;
                    pVariable = pAssignment->pDependencies[pAssignment->DependencyCount - 1].pVariable;
                    D3DXASSERT( pVariable->pType->BelongsInConstantBuffer() && NULL != pVariable->pCB );

                    // When cloning, convert pointers back into offsets
                    if( pOldBlocks )
                    {
                        VBD( pOldVariable != NULL, "Internal loading error: pOldVariable is NULL." );
                        pAssignment->Source.Offset = pAssignment->Source.pNumeric - pOldVariable->pCB->pBackingStore;
                    }

                    // Convert from offset to pointer
                    pAssignment->Source.pNumeric = pVariable->pCB->pBackingStore + pAssignment->Source.Offset;
                    break;

                default:
                    // Shouldn't be able to get here
                    D3DXASSERT(0);
                    VHD( E_FAIL, "Loading error: invalid assignment type." );
                }
                break;

            case ELHS_Invalid:
                VHD( E_FAIL, "Loading error: invalid assignment type." );
            }

            D3DXASSERT(m_pEffect->m_LocalTimer > 0);
            m_pEffect->EvaluateAssignment(pAssignment);
        }
    }

lExit:
    return hr;
}

template<class T> UINT  CEffectLoader::CalculateBlockAssignmentSize(T* &pBlocks, UINT  cBlocks)
{
    UINT  dwSize = 0;
    UINT  i, j;

    for(i=0; i<cBlocks; i++)
    {
        SBaseBlock *pBlock = &pBlocks[i];
        dwSize += AlignToPowerOf2(pBlock->AssignmentCount * sizeof(SAssignment), c_DataAlignment);
        
        for (j=0; j<pBlock->AssignmentCount; j++)
        {
            SAssignment *pAssignment = &pBlock->pAssignments[j];
            
            dwSize += AlignToPowerOf2(pAssignment->DependencyCount * sizeof(SAssignment::SDependency), c_DataAlignment);
        }
    }

    return dwSize;
}

HRESULT CEffectLoader::ReallocateShaderBlocks()
{
    HRESULT hr = S_OK;
    UINT  i, j, k;
    CEffectHeap *pHeap = &m_pEffect->m_Heap;
    const char* pError = "Internal loading error: cannot move shader data.";
    
    for (i=0; i<m_pEffect->m_ShaderBlockCount; i++)
    {
        SShaderBlock *pShader = &m_pEffect->m_pShaderBlocks[i];

        // pShader->pReflection data and all of its members (bytecode, SO decl, etc.) are handled by ReallocateReflectionData()
        VHD( pHeap->MoveData((void**) &pShader->pCBDeps, pShader->CBDepCount * sizeof(SShaderCBDependency)), pError );
        VHD( pHeap->MoveData((void**) &pShader->pSampDeps, pShader->SampDepCount * sizeof(SShaderSamplerDependency)), pError );
        VHD( pHeap->MoveData((void**) &pShader->pInterfaceDeps, pShader->InterfaceDepCount * sizeof(SInterfaceDependency)), pError );
        VHD( pHeap->MoveData((void**) &pShader->pResourceDeps, pShader->ResourceDepCount * sizeof(SShaderResourceDependency)), pError );
        VHD( pHeap->MoveData((void**) &pShader->pUAVDeps, pShader->UAVDepCount * sizeof(SUnorderedAccessViewDependency)), pError );
        VHD( pHeap->MoveData((void**) &pShader->ppTbufDeps, pShader->TBufferDepCount * sizeof(SConstantBuffer*)), pError );
        
        for (j=0; j<pShader->CBDepCount; j++)
        {
            SShaderCBDependency *pCBDeps = &pShader->pCBDeps[j];
            VHD( pHeap->MoveData((void**) &pCBDeps->ppD3DObjects, pCBDeps->Count * sizeof(ID3D11Buffer*)), pError );
            VHD( pHeap->MoveData((void**) &pCBDeps->ppFXPointers, pCBDeps->Count * sizeof(SConstantBuffer*)), pError );

            for (k=0; k<pCBDeps->Count; k++)
            {
                VH( FixupCBPointer( &pCBDeps->ppFXPointers[k] ) );
            }
        }

        for (j=0; j<pShader->SampDepCount; j++)
        {
            SShaderSamplerDependency *pSampDeps = &pShader->pSampDeps[j];
            VHD( pHeap->MoveData((void**) &pSampDeps->ppD3DObjects, pSampDeps->Count * sizeof(ID3D11SamplerState*)), pError );
            VHD( pHeap->MoveData((void**) &pSampDeps->ppFXPointers, pSampDeps->Count * sizeof(SSamplerBlock*)), pError );

            for (k=0; k<pSampDeps->Count; k++)
            {
                VH( FixupSamplerPointer(&pSampDeps->ppFXPointers[k]) );
            }
        }

        for (j=0; j<pShader->InterfaceDepCount; j++)
        {
            SInterfaceDependency *pInterfaceDeps = &pShader->pInterfaceDeps[j];
            VHD( pHeap->MoveData((void**) &pInterfaceDeps->ppD3DObjects, pInterfaceDeps->Count * sizeof(ID3D11ClassInstance*)), pError );
            VHD( pHeap->MoveData((void**) &pInterfaceDeps->ppFXPointers, pInterfaceDeps->Count * sizeof(SInterface*)), pError );

            for (k=0; k<pInterfaceDeps->Count; k++)
            {
                VH( FixupInterfacePointer(&pInterfaceDeps->ppFXPointers[k], true) );
            }
        }

        for (j=0; j<pShader->ResourceDepCount; j++)
        {
            SShaderResourceDependency *pResourceDeps = &pShader->pResourceDeps[j];
            VHD( pHeap->MoveData((void**) &pResourceDeps->ppD3DObjects, pResourceDeps->Count * sizeof(ID3D11ShaderResourceView*)), pError );
            VHD( pHeap->MoveData((void**) &pResourceDeps->ppFXPointers, pResourceDeps->Count * sizeof(SShaderResource*)), pError );

            for (k=0; k<pResourceDeps->Count; k++)
            {
                VH( FixupShaderResourcePointer(&pResourceDeps->ppFXPointers[k]) );
            }
        }

        for (j=0; j<pShader->UAVDepCount; j++)
        {
            SUnorderedAccessViewDependency *pUAVDeps = &pShader->pUAVDeps[j];
            VHD( pHeap->MoveData((void**) &pUAVDeps->ppD3DObjects, pUAVDeps->Count * sizeof(ID3D11UnorderedAccessView*)), pError );
            VHD( pHeap->MoveData((void**) &pUAVDeps->ppFXPointers, pUAVDeps->Count * sizeof(SUnorderedAccessView*)), pError );

            for (k=0; k<pUAVDeps->Count; k++)
            {
                VH( FixupUnorderedAccessViewPointer(&pUAVDeps->ppFXPointers[k]) );
            }
        }

        for (j=0; j<pShader->TBufferDepCount; j++)
        {
            VH( FixupCBPointer( &pShader->ppTbufDeps[j] ) );
        }
    }

lExit:
    return hr;
}


UINT  CEffectLoader::CalculateShaderBlockSize()
{
    UINT  dwSize = 0;
    UINT  i, j;
    
    for (i=0; i<m_pEffect->m_ShaderBlockCount; i++)
    {
        SShaderBlock *pShader = &m_pEffect->m_pShaderBlocks[i];

        dwSize += AlignToPowerOf2(pShader->CBDepCount * sizeof(SShaderCBDependency), c_DataAlignment);
        dwSize += AlignToPowerOf2(pShader->SampDepCount * sizeof(SShaderSamplerDependency), c_DataAlignment);
        dwSize += AlignToPowerOf2(pShader->InterfaceDepCount * sizeof(SInterfaceDependency), c_DataAlignment);
        dwSize += AlignToPowerOf2(pShader->ResourceDepCount * sizeof(SShaderResourceDependency), c_DataAlignment);
        dwSize += AlignToPowerOf2(pShader->UAVDepCount * sizeof(SUnorderedAccessViewDependency), c_DataAlignment);
        dwSize += AlignToPowerOf2(pShader->TBufferDepCount * sizeof(SConstantBuffer*), c_DataAlignment);

        for (j=0; j<pShader->CBDepCount; j++)
        {
            SShaderCBDependency *pCBDeps = &pShader->pCBDeps[j];
            dwSize += AlignToPowerOf2(pCBDeps->Count * sizeof(ID3D11Buffer*), c_DataAlignment);
            dwSize += AlignToPowerOf2(pCBDeps->Count * sizeof(SConstantBuffer*), c_DataAlignment);
        }

        for (j=0; j<pShader->SampDepCount; j++)
        {
            SShaderSamplerDependency *pSampDeps = &pShader->pSampDeps[j];
            dwSize += AlignToPowerOf2(pSampDeps->Count * sizeof(ID3D11SamplerState*), c_DataAlignment);
            dwSize += AlignToPowerOf2(pSampDeps->Count * sizeof(SSamplerBlock*), c_DataAlignment);
        }

        for (j=0; j<pShader->InterfaceDepCount; j++)
        {
            SInterfaceDependency *pInterfaceDeps = &pShader->pInterfaceDeps[j];
            dwSize += AlignToPowerOf2(pInterfaceDeps->Count * sizeof(ID3D11ClassInstance*), c_DataAlignment);
            dwSize += AlignToPowerOf2(pInterfaceDeps->Count * sizeof(SInterface*), c_DataAlignment);
        }

        for (j=0; j<pShader->ResourceDepCount; j++)
        {
            SShaderResourceDependency *pResourceDeps = &pShader->pResourceDeps[j];
            dwSize += AlignToPowerOf2(pResourceDeps->Count * sizeof(ID3D11ShaderResourceView*), c_DataAlignment);
            dwSize += AlignToPowerOf2(pResourceDeps->Count * sizeof(SShaderResource*), c_DataAlignment);
        }

        for (j=0; j<pShader->UAVDepCount; j++)
        {
            SUnorderedAccessViewDependency *pUAVDeps = &pShader->pUAVDeps[j];
            dwSize += AlignToPowerOf2(pUAVDeps->Count * sizeof(ID3D11UnorderedAccessView*), c_DataAlignment);
            dwSize += AlignToPowerOf2(pUAVDeps->Count * sizeof(SUnorderedAccessView*), c_DataAlignment);
        }
    }

    return dwSize;
}

// Move all (non-reflection) effect data to private heap
HRESULT CEffectLoader::ReallocateEffectData( bool Cloning )
{
    HRESULT hr = S_OK;
    UINT i, j;
    CEffectHeap *pHeap = &m_pEffect->m_Heap;
    UINT cbCBs = sizeof(SConstantBuffer) * m_pEffect->m_CBCount;
    UINT cbVariables = sizeof(SGlobalVariable) * m_pEffect->m_VariableCount;
    UINT cbGroups = sizeof(STechnique) * m_pEffect->m_GroupCount;
    UINT cbShaders = sizeof(SShaderBlock) * m_pEffect->m_ShaderBlockCount;
    UINT cbDS = sizeof(SDepthStencilBlock) * m_pEffect->m_DepthStencilBlockCount;
    UINT cbAB = sizeof(SBlendBlock) * m_pEffect->m_BlendBlockCount;
    UINT cbRS = sizeof(SRasterizerBlock) * m_pEffect->m_RasterizerBlockCount;
    UINT cbSamplers = sizeof(SSamplerBlock) * m_pEffect->m_SamplerBlockCount;
    UINT cbMemberDatas = sizeof(SMemberDataPointer) * m_pEffect->m_MemberDataCount;
    UINT cbInterfaces = sizeof(SInterface) * m_pEffect->m_InterfaceCount;
    UINT cbBackgroundInterfaces = sizeof(SInterface) * m_BackgroundInterfaces.GetSize();
    UINT cbShaderResources = sizeof(SShaderResource) * m_pEffect->m_ShaderResourceCount;
    UINT cbUnorderedAccessViews = sizeof(SUnorderedAccessView) * m_pEffect->m_UnorderedAccessViewCount;
    UINT cbRenderTargetViews = sizeof(SRenderTargetView) * m_pEffect->m_RenderTargetViewCount;
    UINT cbDepthStencilViews = sizeof(SDepthStencilView) * m_pEffect->m_DepthStencilViewCount;
    UINT cbAnonymousShaders = sizeof(SAnonymousShader) * m_pEffect->m_AnonymousShaderCount;

    // Calculate memory needed
    m_EffectMemory += AlignToPowerOf2(cbCBs, c_DataAlignment);
    m_EffectMemory += AlignToPowerOf2(cbVariables, c_DataAlignment);
    m_EffectMemory += AlignToPowerOf2(cbGroups, c_DataAlignment);
    m_EffectMemory += AlignToPowerOf2(cbShaders, c_DataAlignment);
    m_EffectMemory += AlignToPowerOf2(cbMemberDatas, c_DataAlignment);
    m_EffectMemory += AlignToPowerOf2(cbInterfaces + cbBackgroundInterfaces, c_DataAlignment);
    m_EffectMemory += AlignToPowerOf2(cbShaderResources, c_DataAlignment);
    m_EffectMemory += AlignToPowerOf2(cbUnorderedAccessViews, c_DataAlignment);
    m_EffectMemory += AlignToPowerOf2(cbRenderTargetViews, c_DataAlignment);
    m_EffectMemory += AlignToPowerOf2(cbDepthStencilViews, c_DataAlignment);
    m_EffectMemory += AlignToPowerOf2(cbDS, c_DataAlignment);
    m_EffectMemory += AlignToPowerOf2(cbAB, c_DataAlignment);
    m_EffectMemory += AlignToPowerOf2(cbRS, c_DataAlignment);
    m_EffectMemory += AlignToPowerOf2(cbSamplers, c_DataAlignment);
    m_EffectMemory += AlignToPowerOf2(cbAnonymousShaders, c_DataAlignment);

    m_EffectMemory += CalculateShaderBlockSize();

    for (i=0; i<m_pEffect->m_CBCount; i++)
    {
        SConstantBuffer *pCB = &m_pEffect->m_pCBs[i];

        m_EffectMemory += AlignToPowerOf2(pCB->Size, c_DataAlignment);
    }

    for (i=0; i<m_pEffect->m_GroupCount; i++)
    {
        SGroup *pGroup = &m_pEffect->m_pGroups[i];

        m_EffectMemory += AlignToPowerOf2(pGroup->TechniqueCount * sizeof(STechnique), c_DataAlignment);

        for (j=0; j<pGroup->TechniqueCount; j++)
        {
            STechnique *pTech = &pGroup->pTechniques[j];

            m_EffectMemory += AlignToPowerOf2(pTech->PassCount * sizeof(SPassBlock), c_DataAlignment);
            m_EffectMemory += CalculateBlockAssignmentSize(pTech->pPasses, pTech->PassCount);
        }
    };

    m_EffectMemory += CalculateBlockAssignmentSize(m_pEffect->m_pBlendBlocks, m_pEffect->m_BlendBlockCount);
    m_EffectMemory += CalculateBlockAssignmentSize(m_pEffect->m_pDepthStencilBlocks, m_pEffect->m_DepthStencilBlockCount);
    m_EffectMemory += CalculateBlockAssignmentSize(m_pEffect->m_pRasterizerBlocks, m_pEffect->m_RasterizerBlockCount);
    m_EffectMemory += CalculateBlockAssignmentSize(m_pEffect->m_pSamplerBlocks, m_pEffect->m_SamplerBlockCount);

    // Reserve memory
    VHD( pHeap->ReserveMemory(m_EffectMemory), "Internal loading error: cannot reserve effect memory." );

    // Move DataMemberPointer blocks
    m_pOldMemberDataBlocks = m_pEffect->m_pMemberDataBlocks;
    VHD( pHeap->MoveData((void**) &m_pEffect->m_pMemberDataBlocks, cbMemberDatas), "Internal loading error: cannot move member data blocks." );

    // Move CBs
    m_pOldCBs = m_pEffect->m_pCBs;
    VHD( pHeap->MoveData((void**) &m_pEffect->m_pCBs, cbCBs), "Internal loading error: cannot move CB count." );
    for (i=0; i<m_pEffect->m_CBCount; i++)
    {
        SConstantBuffer *pCB = &m_pEffect->m_pCBs[i];

        VHD( pHeap->MoveData((void**) &pCB->pBackingStore, pCB->Size), "Internal loading error: cannot move CB backing store." );

        if( !Cloning )
        {
            // When creating the effect, MemberDataOffsetPlus4 is used, not pMemberData
            if( pCB->MemberDataOffsetPlus4 )
            {
                pCB->pMemberData = (SMemberDataPointer*)( (BYTE*)m_pEffect->m_pMemberDataBlocks + ( pCB->MemberDataOffsetPlus4 - 4 ) );
            }
        }
        else if (pCB->pMemberData)
        {
            // When cloning an effect, pMemberData points to valid data in the original effect
            VH( FixupMemberDataPointer( &pCB->pMemberData ) );
        }
    }

    // Move numeric variables; move all variable types
    m_pOldVars = m_pEffect->m_pVariables;
    VHD( pHeap->MoveData((void**) &m_pEffect->m_pVariables, cbVariables), "Internal loading error: cannot move variable count." );
    for (i=0; i<m_pEffect->m_VariableCount; i++)
    {
        SGlobalVariable *pVar = &m_pEffect->m_pVariables[i];
        pVar->pEffect = m_pEffect;

        if( Cloning && pVar->pType->BelongsInConstantBuffer())
        {
            // Convert pointer back to offset
            // pVar->pCB refers to the old CB
            pVar->Data.Offset = (UINT_PTR)pVar->Data.pGeneric - (UINT_PTR)pVar->pCB->pBackingStore;
        }

        if (pVar->pCB)
        {
            VH( FixupCBPointer( &pVar->pCB ) );
        }

        if( !Cloning )
        {
            // When creating the effect, MemberDataOffsetPlus4 is used, not pMemberData
            if( pVar->MemberDataOffsetPlus4 )
            {
                pVar->pMemberData = (SMemberDataPointer*)( (BYTE*)m_pEffect->m_pMemberDataBlocks + ( pVar->MemberDataOffsetPlus4 - 4 ) );
            }
        }
        else if (pVar->pMemberData)
        {
            // When cloning an effect, pMemberData points to valid data in the original effect
            VH( FixupMemberDataPointer( &pVar->pMemberData ) );
        }

        if (pVar->pType->BelongsInConstantBuffer())
        {
            // Convert from offsets to pointers
            pVar->Data.pGeneric = pVar->pCB->pBackingStore + pVar->Data.Offset;
        }
    }

    // Fixup each CB's array of child variable pointers
    for (i=0; i<m_pEffect->m_CBCount; i++)
    {
        SConstantBuffer *pCB = &m_pEffect->m_pCBs[i];
        pCB->pEffect = m_pEffect;

        if (pCB->pVariables != NULL)
        {
            VH( FixupVariablePointer(&pCB->pVariables) );
        }
    }

    // Move shaders
    m_pOldShaders = m_pEffect->m_pShaderBlocks;
    VHD( pHeap->MoveData((void**) &m_pEffect->m_pShaderBlocks, cbShaders), "Internal loading error: cannot move shader count." );

    // Move interfaces, combining global interfaces and those that were created during shader initialization
    m_pOldInterfaces = m_pEffect->m_pInterfaces;
    m_OldInterfaceCount = m_pEffect->m_InterfaceCount;
    VHD( pHeap->MoveEmptyDataBlock((void**) &m_pEffect->m_pInterfaces, cbInterfaces + cbBackgroundInterfaces), "Internal loading error: cannot move shader." );
    memcpy( m_pEffect->m_pInterfaces, m_pOldInterfaces, cbInterfaces );
    for( i=0; i < m_BackgroundInterfaces.GetSize(); i++ )
    {
        D3DXASSERT( m_BackgroundInterfaces[i] != NULL );
        BYTE* pDst = (BYTE*)m_pEffect->m_pInterfaces  + ( m_pEffect->m_InterfaceCount * sizeof(SInterface) );
        memcpy( pDst, m_BackgroundInterfaces[i], sizeof(SInterface) );
        m_pEffect->m_InterfaceCount++;
    }

    m_pOldShaderResources = m_pEffect->m_pShaderResources;
    VHD( pHeap->MoveData((void**) &m_pEffect->m_pShaderResources, cbShaderResources), "Internal loading error: cannot move SRVs." );

    m_pOldUnorderedAccessViews = m_pEffect->m_pUnorderedAccessViews;
    VHD( pHeap->MoveData((void**) &m_pEffect->m_pUnorderedAccessViews, cbUnorderedAccessViews), "Internal loading error: cannot move UAVS." );

    m_pOldRenderTargetViews = m_pEffect->m_pRenderTargetViews;
    VHD( pHeap->MoveData((void**) &m_pEffect->m_pRenderTargetViews, cbRenderTargetViews), "Internal loading error: cannot move RTVs." );

    m_pOldDepthStencilViews = m_pEffect->m_pDepthStencilViews;
    VHD( pHeap->MoveData((void**) &m_pEffect->m_pDepthStencilViews, cbDepthStencilViews), "Internal loading error: cannot move DSVs." );

    m_pOldDS = m_pEffect->m_pDepthStencilBlocks;
    VHD( pHeap->MoveData((void**) &m_pEffect->m_pDepthStencilBlocks, cbDS), "Internal loading error: cannot move depth-stencil state blocks." );
    VH( ReallocateBlockAssignments(m_pEffect->m_pDepthStencilBlocks, m_pEffect->m_DepthStencilBlockCount, Cloning ? m_pOldDS : NULL) );
    
    m_pOldAB = m_pEffect->m_pBlendBlocks;
    VHD( pHeap->MoveData((void**) &m_pEffect->m_pBlendBlocks, cbAB), "Internal loading error: cannot move blend state blocks." );
    VH( ReallocateBlockAssignments(m_pEffect->m_pBlendBlocks, m_pEffect->m_BlendBlockCount, Cloning ? m_pOldAB : NULL) );

    m_pOldRS = m_pEffect->m_pRasterizerBlocks;
    VHD( pHeap->MoveData((void**) &m_pEffect->m_pRasterizerBlocks, cbRS), "Internal loading error: cannot move rasterizer state blocks." );
    VH( ReallocateBlockAssignments(m_pEffect->m_pRasterizerBlocks, m_pEffect->m_RasterizerBlockCount, Cloning ? m_pOldRS : NULL) );

    m_pOldSamplers = m_pEffect->m_pSamplerBlocks;
    VHD( pHeap->MoveData((void**) &m_pEffect->m_pSamplerBlocks, cbSamplers), "Internal loading error: cannot move samplers." );
    VH( ReallocateBlockAssignments(m_pEffect->m_pSamplerBlocks, m_pEffect->m_SamplerBlockCount, Cloning ? m_pOldSamplers : NULL) );
    
    // Fixup sampler backing stores
    for (i=0; i<m_pEffect->m_SamplerBlockCount; ++i)
    {
        VH( FixupShaderResourcePointer(&m_pEffect->m_pSamplerBlocks[i].BackingStore.pTexture) );
    }

    // Fixup each interface's class instance variable pointer
    for (i=0; i<m_pEffect->m_InterfaceCount; i++)
    {
        SInterface *pInterface = &m_pEffect->m_pInterfaces[i];

        if (pInterface->pClassInstance != NULL)
        {
            VH( FixupVariablePointer( (SGlobalVariable**)&pInterface->pClassInstance ) );
        }
    }

    // Fixup pointers for non-numeric variables
    for (i=0; i<m_pEffect->m_VariableCount; i++)
    {
        SGlobalVariable *pVar = &m_pEffect->m_pVariables[i];

        if (pVar->pType->IsShader())
        {
            VH( FixupShaderPointer(&pVar->Data.pShader) );
        }
        else if (pVar->pType->IsShaderResource())
        {
            VH( FixupShaderResourcePointer(&pVar->Data.pShaderResource) );
        }
        else if (pVar->pType->IsUnorderedAccessView())
        {
            VH( FixupUnorderedAccessViewPointer(&pVar->Data.pUnorderedAccessView) );
        }
        else if (pVar->pType->IsInterface())
        {
            VH( FixupInterfacePointer(&pVar->Data.pInterface, false) );
        }
        else if (pVar->pType->IsObjectType(EOT_String))
        {
            if( !m_pEffect->IsOptimized() )
            {
                VH( FixupStringPointer(&pVar->Data.pString) );
            }
        }
        else if (pVar->pType->IsStateBlockObject())
        {
            switch(pVar->pType->ObjectType)
            {
                case EOT_DepthStencil:
                    VH( FixupDSPointer((SDepthStencilBlock**) &pVar->Data.pBlock) );
                    break;
                case EOT_Blend:
                    VH( FixupABPointer((SBlendBlock**) &pVar->Data.pBlock) );
                    break;
                case EOT_Rasterizer:
                    VH( FixupRSPointer((SRasterizerBlock**) &pVar->Data.pBlock) );
                    break;
                case EOT_Sampler:
                    VB(pVar->pType->IsSampler());
                    VH( FixupSamplerPointer((SSamplerBlock**) &pVar->Data.pBlock) );
                    break;
                default:
                    VH( E_FAIL );
            }
        }
        else if (pVar->pType->VarType == EVT_Struct || pVar->pType->VarType == EVT_Numeric)
        {
            if( pVar->pType->IsClassInstance() )
            {
                // do nothing
            }
            else
            {
                // do nothing
            }
        }
        else if (pVar->pType->IsRenderTargetView())
        {
            VH( FixupRenderTargetViewPointer(&pVar->Data.pRenderTargetView) );
        } 
        else if (pVar->pType->IsDepthStencilView())
        {
            VH( FixupDepthStencilViewPointer(&pVar->Data.pDepthStencilView) );
        }
        else
        {
            VHD( E_FAIL, "Internal loading error: Invalid variable type." );
        }
    }

    // Fixup created members
    for (i = 0; i < m_pEffect->m_pMemberInterfaces.GetSize(); ++ i)
    {
        SMember* pMember = m_pEffect->m_pMemberInterfaces[i];
        SGlobalVariable** ppTopLevelEntity = (SGlobalVariable**)&pMember->pTopLevelEntity;
        VN( *ppTopLevelEntity );

        // This might be set to false later, for supporting textures inside classes
        const bool bGlobalMemberDataBlock = true;

        if( Cloning )
        {
            if( pMember->pType->BelongsInConstantBuffer() )
            {
                D3DXASSERT( pMember->Data.pGeneric == NULL || (*ppTopLevelEntity)->pEffect->m_Heap.IsInHeap(pMember->Data.pGeneric) );
                pMember->Data.Offset = (UINT)( (BYTE*)pMember->Data.pGeneric - (BYTE*)(*ppTopLevelEntity)->pCB->pBackingStore );
            }
            if( bGlobalMemberDataBlock && pMember->pMemberData )
            {
                pMember->MemberDataOffsetPlus4 = (UINT)( (BYTE*)pMember->pMemberData - (BYTE*)(*ppTopLevelEntity)->pEffect->m_pMemberDataBlocks ) + 4;
            }
        }

        VH( FixupVariablePointer( ppTopLevelEntity ) );

        if (pMember->pType->BelongsInConstantBuffer())
        {
            // Convert from offsets to pointers
            pMember->Data.pGeneric = (*ppTopLevelEntity)->pCB->pBackingStore + pMember->Data.Offset;
        }
        if( bGlobalMemberDataBlock && pMember->MemberDataOffsetPlus4 )
        {
            pMember->pMemberData = (SMemberDataPointer*)( (BYTE*)m_pEffect->m_pMemberDataBlocks + ( pMember->MemberDataOffsetPlus4 - 4 ) );
        }
    }

    // Fixup shader data
    VH( ReallocateShaderBlocks() );

    // Move groups, techniques, and passes
    m_pOldGroups = m_pEffect->m_pGroups;
    VHD( pHeap->MoveData((void**) &m_pEffect->m_pGroups, cbGroups), "Internal loading error: cannot move groups." );
    for (i=0; i<m_pEffect->m_GroupCount; i++)
    {
        SGroup *pGroup = &m_pEffect->m_pGroups[i];
        UINT  cbTechniques;

        cbTechniques = pGroup->TechniqueCount * sizeof(STechnique);
        VHD( pHeap->MoveData((void**) &pGroup->pTechniques, cbTechniques), "Internal loading error: cannot move techniques." );

        for (j=0; j<pGroup->TechniqueCount; j++)
        {
            STechnique *pTech = &pGroup->pTechniques[j];
            UINT  cbPass;
            UINT  iPass;

            cbPass = pTech->PassCount * sizeof(SPassBlock);
            SPassBlock* pOldPasses = Cloning ? pTech->pPasses : NULL;
            VHD( pHeap->MoveData((void**) &pTech->pPasses, cbPass), "Internal loading error: cannot move passes." );

            for (iPass = 0; iPass < pTech->PassCount; ++ iPass)
            {
                pTech->pPasses[iPass].pEffect = m_pEffect;

                // Fixup backing store pointers in passes
                VH( FixupABPointer((SBlendBlock**) &pTech->pPasses[iPass].BackingStore.pBlendBlock) );
                VH( FixupDSPointer((SDepthStencilBlock**) &pTech->pPasses[iPass].BackingStore.pDepthStencilBlock) );
                VH( FixupRSPointer((SRasterizerBlock**) &pTech->pPasses[iPass].BackingStore.pRasterizerBlock) );
                VH( FixupShaderPointer((SShaderBlock**) &pTech->pPasses[iPass].BackingStore.pVertexShaderBlock) );
                VH( FixupShaderPointer((SShaderBlock**) &pTech->pPasses[iPass].BackingStore.pPixelShaderBlock) );
                VH( FixupShaderPointer((SShaderBlock**) &pTech->pPasses[iPass].BackingStore.pGeometryShaderBlock) );
                VH( FixupShaderPointer((SShaderBlock**) &pTech->pPasses[iPass].BackingStore.pHullShaderBlock) );
                VH( FixupShaderPointer((SShaderBlock**) &pTech->pPasses[iPass].BackingStore.pDomainShaderBlock) );
                VH( FixupShaderPointer((SShaderBlock**) &pTech->pPasses[iPass].BackingStore.pComputeShaderBlock) );
                VH( FixupDepthStencilViewPointer( &pTech->pPasses[iPass].BackingStore.pDepthStencilView) );
                for (UINT iRT = 0; iRT < D3D11_SIMULTANEOUS_RENDER_TARGET_COUNT; iRT++)
                {
                    VH( FixupRenderTargetViewPointer( &pTech->pPasses[iPass].BackingStore.pRenderTargetViews[iRT] ) );
                }
            }        

            VH( ReallocateBlockAssignments( pTech->pPasses, pTech->PassCount, pOldPasses ) );
        }
    }
    VH( FixupGroupPointer( &m_pEffect->m_pNullGroup ) );

    // Move anonymous shader variables
    VHD( pHeap->MoveData((void **) &m_pEffect->m_pAnonymousShaders, cbAnonymousShaders), "Internal loading error: cannot move anonymous shaders." );
    for (i=0; i<m_pEffect->m_AnonymousShaderCount; ++i)
    {
        SAnonymousShader *pAnonymousShader = m_pEffect->m_pAnonymousShaders + i;
        VH( FixupShaderPointer((SShaderBlock**) &pAnonymousShader->pShaderBlock) );
    }

    VBD( pHeap->GetSize() == m_EffectMemory, "Loading error: effect size mismatch." );

lExit:
    return hr;
}

}
