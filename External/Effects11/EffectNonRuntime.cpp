//////////////////////////////////////////////////////////////////////////////
//
//  Copyright (C) Microsoft Corporation.  All Rights Reserved.
//
//  File:       EffectNonRuntime.cpp
//  Content:    D3DX11 Effect low-frequency utility functions
//              These functions are not intended to be called regularly.  They
//              are typically called when creating, cloning, or optimizing an 
//              Effect, or reflecting a variable.
//
//////////////////////////////////////////////////////////////////////////////

#include "pchfx.h"
#include "SOParser.h"

namespace D3DX11Effects
{

extern SUnorderedAccessView g_NullUnorderedAccessView;

SBaseBlock::SBaseBlock()
: BlockType(EBT_Invalid)
, IsUserManaged(FALSE)
, AssignmentCount(0)
, pAssignments(NULL)
{

}

SPassBlock::SPassBlock()
{
    pName = NULL;
    AnnotationCount = 0;
    pAnnotations = NULL;
    InitiallyValid = TRUE;
    HasDependencies = FALSE;
    ZeroMemory(&BackingStore, sizeof(BackingStore));
}

STechnique::STechnique()
: pName(NULL)
, PassCount(0)
, pPasses(NULL)
, AnnotationCount(0)
, pAnnotations(NULL)
, InitiallyValid( TRUE )
, HasDependencies( FALSE )
{
}

SGroup::SGroup()
: pName(NULL)
, TechniqueCount(0)
, pTechniques(NULL)
, AnnotationCount(0)
, pAnnotations(NULL)
, InitiallyValid( TRUE )
, HasDependencies( FALSE )
{
}

SDepthStencilBlock::SDepthStencilBlock()
{
    pDSObject = NULL;
    ZeroMemory(&BackingStore, sizeof(BackingStore));
    IsValid = TRUE;

    BackingStore.BackFace.StencilFailOp = D3D11_STENCIL_OP_KEEP;
    BackingStore.BackFace.StencilFunc = D3D11_COMPARISON_ALWAYS;
    BackingStore.BackFace.StencilPassOp = D3D11_STENCIL_OP_KEEP;
    BackingStore.BackFace.StencilDepthFailOp = D3D11_STENCIL_OP_KEEP;
    BackingStore.DepthEnable = TRUE;
    BackingStore.DepthFunc = D3D11_COMPARISON_LESS;
    BackingStore.DepthWriteMask = D3D11_DEPTH_WRITE_MASK_ALL;
    BackingStore.FrontFace.StencilFailOp = D3D11_STENCIL_OP_KEEP;
    BackingStore.FrontFace.StencilFunc = D3D11_COMPARISON_ALWAYS;
    BackingStore.FrontFace.StencilPassOp = D3D11_STENCIL_OP_KEEP;
    BackingStore.FrontFace.StencilDepthFailOp = D3D11_STENCIL_OP_KEEP;
    BackingStore.StencilEnable = FALSE;
    BackingStore.StencilReadMask = D3D11_DEFAULT_STENCIL_READ_MASK;
    BackingStore.StencilWriteMask = D3D11_DEFAULT_STENCIL_WRITE_MASK;
}

SBlendBlock::SBlendBlock()
{
    pBlendObject = NULL;
    ZeroMemory(&BackingStore, sizeof(BackingStore));
    IsValid = TRUE;

    BackingStore.AlphaToCoverageEnable = FALSE;
    BackingStore.IndependentBlendEnable = TRUE;
    for( UINT i=0; i < D3D11_SIMULTANEOUS_RENDER_TARGET_COUNT; i++ )
    {
        BackingStore.RenderTarget[i].SrcBlend = D3D11_BLEND_ONE;
        BackingStore.RenderTarget[i].DestBlend = D3D11_BLEND_ZERO;
        BackingStore.RenderTarget[i].BlendOp = D3D11_BLEND_OP_ADD;
        BackingStore.RenderTarget[i].SrcBlendAlpha = D3D11_BLEND_ONE;
        BackingStore.RenderTarget[i].DestBlendAlpha = D3D11_BLEND_ZERO;
        BackingStore.RenderTarget[i].BlendOpAlpha = D3D11_BLEND_OP_ADD;
        memset(&BackingStore.RenderTarget[i].RenderTargetWriteMask, 0x0F, sizeof(BackingStore.RenderTarget[i].RenderTargetWriteMask));
    }
}

SRasterizerBlock::SRasterizerBlock()
{
    pRasterizerObject = NULL;
    ZeroMemory(&BackingStore, sizeof(BackingStore));
    IsValid = TRUE;

    BackingStore.AntialiasedLineEnable = FALSE;
    BackingStore.CullMode = D3D11_CULL_BACK;
    BackingStore.DepthBias = D3D11_DEFAULT_DEPTH_BIAS;
    BackingStore.DepthBiasClamp = D3D11_DEFAULT_DEPTH_BIAS_CLAMP;
    BackingStore.FillMode = D3D11_FILL_SOLID;
    BackingStore.FrontCounterClockwise = FALSE;
    BackingStore.MultisampleEnable = FALSE;
    BackingStore.ScissorEnable = FALSE;
    BackingStore.SlopeScaledDepthBias = D3D11_DEFAULT_SLOPE_SCALED_DEPTH_BIAS;
    BackingStore.DepthClipEnable = TRUE;
}

SSamplerBlock::SSamplerBlock()
{
    pD3DObject = NULL;
    ZeroMemory(&BackingStore, sizeof(BackingStore));

    BackingStore.SamplerDesc.AddressU = D3D11_TEXTURE_ADDRESS_CLAMP;
    BackingStore.SamplerDesc.AddressV = D3D11_TEXTURE_ADDRESS_CLAMP;
    BackingStore.SamplerDesc.AddressW = D3D11_TEXTURE_ADDRESS_CLAMP;
    BackingStore.SamplerDesc.BorderColor[3] = D3D11_DEFAULT_BORDER_COLOR_COMPONENT;
    BackingStore.SamplerDesc.BorderColor[2] = D3D11_DEFAULT_BORDER_COLOR_COMPONENT;
    BackingStore.SamplerDesc.BorderColor[1] = D3D11_DEFAULT_BORDER_COLOR_COMPONENT;
    BackingStore.SamplerDesc.BorderColor[0] = D3D11_DEFAULT_BORDER_COLOR_COMPONENT;
    BackingStore.SamplerDesc.ComparisonFunc = D3D11_COMPARISON_NEVER;
    BackingStore.SamplerDesc.Filter = D3D11_FILTER_MIN_MAG_MIP_LINEAR;
    BackingStore.SamplerDesc.MaxAnisotropy = (UINT32) D3D11_DEFAULT_MAX_ANISOTROPY;
    BackingStore.SamplerDesc.MipLODBias = D3D11_DEFAULT_MIP_LOD_BIAS;
    BackingStore.SamplerDesc.MinLOD = -FLT_MAX;
    BackingStore.SamplerDesc.MaxLOD = FLT_MAX;
}

SShaderBlock::SShaderBlock(SD3DShaderVTable *pVirtualTable)
{
    IsValid = TRUE;

    pVT = pVirtualTable;

    pReflectionData = NULL;
    
    pD3DObject = NULL;

    CBDepCount = 0;
    pCBDeps = NULL;

    SampDepCount = 0;
    pSampDeps = NULL;

    InterfaceDepCount = 0;
    pInterfaceDeps = NULL;

    ResourceDepCount = 0;
    pResourceDeps = NULL;

    UAVDepCount = 0;
    pUAVDeps = NULL;

    TBufferDepCount = 0;
    ppTbufDeps = NULL;

    pInputSignatureBlob = NULL;
}

HRESULT SShaderBlock::OnDeviceBind()
{
    HRESULT hr = S_OK;
    UINT  i, j;

    // Update all CB deps
    for (i=0; i<CBDepCount; i++)
    {
        D3DXASSERT(pCBDeps[i].Count);

        for (j=0; j<pCBDeps[i].Count; j++)
        {
            pCBDeps[i].ppD3DObjects[j] = pCBDeps[i].ppFXPointers[j]->pD3DObject;

            if ( !pCBDeps[i].ppD3DObjects[j] )
                VH( E_FAIL );
        }
    }

    // Update all sampler deps
    for (i=0; i<SampDepCount; i++)
    {
        D3DXASSERT(pSampDeps[i].Count);

        for (j=0; j<pSampDeps[i].Count; j++)
        {
            pSampDeps[i].ppD3DObjects[j] = pSampDeps[i].ppFXPointers[j]->pD3DObject;

            if ( !pSampDeps[i].ppD3DObjects[j] )
                VH( E_FAIL );
        }
    }

    // Texture deps will be set automatically on use since they are initially marked dirty.

lExit:
    return hr;
}

extern SD3DShaderVTable g_vtVS;
extern SD3DShaderVTable g_vtGS;
extern SD3DShaderVTable g_vtPS;
extern SD3DShaderVTable g_vtHS;
extern SD3DShaderVTable g_vtDS;
extern SD3DShaderVTable g_vtCS;

EObjectType SShaderBlock::GetShaderType()
{
    if (&g_vtVS == pVT)
        return EOT_VertexShader;
    else if (&g_vtGS == pVT)
        return EOT_GeometryShader;
    else if (&g_vtPS == pVT)
        return EOT_PixelShader;
    else if (&g_vtHS == pVT)
        return EOT_HullShader5;
    else if (&g_vtDS == pVT)
        return EOT_DomainShader5;
    else if (&g_vtCS == pVT)
        return EOT_ComputeShader5;
    
    return EOT_Invalid;
}

#define _SET_BIT(bytes, x) (bytes[x / 8] |= (1 << (x % 8)))

HRESULT SShaderBlock::ComputeStateBlockMask(D3DX11_STATE_BLOCK_MASK *pStateBlockMask)
{
    HRESULT hr = S_OK;
    UINT i, j;
    BYTE *pSamplerMask = NULL, *pShaderResourceMask = NULL, *pConstantBufferMask = NULL, *pUnorderedAccessViewMask = NULL, *pInterfaceMask = NULL;

    switch (GetShaderType())
    {
    case EOT_VertexShader:
    case EOT_VertexShader5:
        pStateBlockMask->VS = 1;
        pSamplerMask = pStateBlockMask->VSSamplers;
        pShaderResourceMask = pStateBlockMask->VSShaderResources;
        pConstantBufferMask = pStateBlockMask->VSConstantBuffers;
        pInterfaceMask = pStateBlockMask->VSInterfaces;
        pUnorderedAccessViewMask = NULL;
        break;

    case EOT_GeometryShader:
    case EOT_GeometryShader5:
        pStateBlockMask->GS = 1;
        pSamplerMask = pStateBlockMask->GSSamplers;
        pShaderResourceMask = pStateBlockMask->GSShaderResources;
        pConstantBufferMask = pStateBlockMask->GSConstantBuffers;
        pInterfaceMask = pStateBlockMask->GSInterfaces;
        pUnorderedAccessViewMask = NULL;
        break;

    case EOT_PixelShader:
    case EOT_PixelShader5:
        pStateBlockMask->PS = 1;
        pSamplerMask = pStateBlockMask->PSSamplers;
        pShaderResourceMask = pStateBlockMask->PSShaderResources;
        pConstantBufferMask = pStateBlockMask->PSConstantBuffers;
        pInterfaceMask = pStateBlockMask->PSInterfaces;
        pUnorderedAccessViewMask = &pStateBlockMask->PSUnorderedAccessViews;
        break;

    case EOT_HullShader5:
        pStateBlockMask->HS = 1;
        pSamplerMask = pStateBlockMask->HSSamplers;
        pShaderResourceMask = pStateBlockMask->HSShaderResources;
        pConstantBufferMask = pStateBlockMask->HSConstantBuffers;
        pInterfaceMask = pStateBlockMask->HSInterfaces;
        pUnorderedAccessViewMask = NULL;
        break;

    case EOT_DomainShader5:
        pStateBlockMask->DS = 1;
        pSamplerMask = pStateBlockMask->DSSamplers;
        pShaderResourceMask = pStateBlockMask->DSShaderResources;
        pConstantBufferMask = pStateBlockMask->DSConstantBuffers;
        pInterfaceMask = pStateBlockMask->DSInterfaces;
        pUnorderedAccessViewMask = NULL;
        break;

    case EOT_ComputeShader5:
        pStateBlockMask->CS = 1;
        pSamplerMask = pStateBlockMask->CSSamplers;
        pShaderResourceMask = pStateBlockMask->CSShaderResources;
        pConstantBufferMask = pStateBlockMask->CSConstantBuffers;
        pInterfaceMask = pStateBlockMask->CSInterfaces;
        pUnorderedAccessViewMask = &pStateBlockMask->CSUnorderedAccessViews;
        break;

    default:
        D3DXASSERT(0);
        VH(E_FAIL);
    }

    for (i = 0; i < SampDepCount; ++ i)
    {
        for (j = 0; j < pSampDeps[i].Count; ++ j)
        {
            _SET_BIT(pSamplerMask, (pSampDeps[i].StartIndex + j));
        }
    }

    for (i = 0; i < InterfaceDepCount; ++ i)
    {
        for (j = 0; j < pInterfaceDeps[i].Count; ++ j)
        {
            _SET_BIT(pInterfaceMask, (pInterfaceDeps[i].StartIndex + j));
        }
    }

    for (i = 0; i < ResourceDepCount; ++ i)
    {
        for (j = 0; j < pResourceDeps[i].Count; ++ j)
        {
            _SET_BIT(pShaderResourceMask, (pResourceDeps[i].StartIndex + j));
        }
    }

    for (i = 0; i < CBDepCount; ++ i)
    {
        for (j = 0; j < pCBDeps[i].Count; ++ j)
        {
            _SET_BIT(pConstantBufferMask, (pCBDeps[i].StartIndex + j));
        }
    }

    for (i = 0; i < UAVDepCount; ++ i)
    {
        D3DXASSERT( pUnorderedAccessViewMask != NULL );
        for (j = 0; j < pUAVDeps[i].Count; ++ j)
        {
            if( pUAVDeps[i].ppFXPointers[j] != &g_NullUnorderedAccessView )
                _SET_BIT(pUnorderedAccessViewMask, (pUAVDeps[i].StartIndex + j));
        }
    }

lExit:
    return hr;
}

#undef _SET_BIT

HRESULT SShaderBlock::GetShaderDesc(D3DX11_EFFECT_SHADER_DESC *pDesc, BOOL IsInline)
{
    HRESULT hr = S_OK;
    
    ZeroMemory(pDesc, sizeof(*pDesc));

    pDesc->pInputSignature = pInputSignatureBlob ? (const BYTE*)pInputSignatureBlob->GetBufferPointer() : NULL;
    pDesc->IsInline = IsInline;

    if (NULL != pReflectionData)
    {
        // initialize these only if present; otherwise leave them NULL or 0
        pDesc->pBytecode = pReflectionData->pBytecode;
        pDesc->BytecodeLength = pReflectionData->BytecodeLength;
        for( UINT iDecl=0; iDecl < D3D11_SO_STREAM_COUNT; ++iDecl )
        {
            pDesc->SODecls[iDecl] = pReflectionData->pStreamOutDecls[iDecl];
        }
        pDesc->RasterizedStream = pReflectionData->RasterizedStream;

        // get # of input & output signature entries
        D3DXASSERT( pReflectionData->pReflection != NULL );

        D3D11_SHADER_DESC ShaderDesc;
        pReflectionData->pReflection->GetDesc( &ShaderDesc );
        pDesc->NumInputSignatureEntries = ShaderDesc.InputParameters;
        pDesc->NumOutputSignatureEntries = ShaderDesc.OutputParameters;
        pDesc->NumPatchConstantSignatureEntries = ShaderDesc.PatchConstantParameters;
    }
lExit:
    return hr;
}

HRESULT SShaderBlock::GetVertexShader(ID3D11VertexShader **ppVS)
{
    if (EOT_VertexShader == GetShaderType() ||
        EOT_VertexShader5 == GetShaderType())
    {
        *ppVS = (ID3D11VertexShader *) pD3DObject;
        SAFE_ADDREF(*ppVS);
        return S_OK;
    }
    else
    {
        *ppVS = NULL;
        DPF(0, "ID3DX11EffectShaderVariable::GetVertexShader: This shader variable is not a vertex shader");
        return D3DERR_INVALIDCALL;
    }
}

HRESULT SShaderBlock::GetGeometryShader(ID3D11GeometryShader **ppGS)
{
    if (EOT_GeometryShader == GetShaderType() ||
        EOT_GeometryShaderSO == GetShaderType() ||
        EOT_GeometryShader5 == GetShaderType())
    {
        *ppGS = (ID3D11GeometryShader *) pD3DObject;
        SAFE_ADDREF(*ppGS);
        return S_OK;
    }
    else
    {
        *ppGS = NULL;
        DPF(0, "ID3DX11EffectShaderVariable::GetGeometryShader: This shader variable is not a geometry shader");
        return D3DERR_INVALIDCALL;
    }
}

HRESULT SShaderBlock::GetPixelShader(ID3D11PixelShader **ppPS)
{
    if (EOT_PixelShader == GetShaderType() ||
        EOT_PixelShader5 == GetShaderType())
    {
        *ppPS = (ID3D11PixelShader *) pD3DObject;
        SAFE_ADDREF(*ppPS);
        return S_OK;
    }
    else
    {
        *ppPS = NULL;
        DPF(0, "ID3DX11EffectShaderVariable::GetPixelShader: This shader variable is not a pixel shader");
        return D3DERR_INVALIDCALL;
    }
}

HRESULT SShaderBlock::GetHullShader(ID3D11HullShader **ppHS)
{
    if (EOT_HullShader5 == GetShaderType())
    {
        *ppHS = (ID3D11HullShader *) pD3DObject;
        SAFE_ADDREF(*ppHS);
        return S_OK;
    }
    else
    {
        *ppHS = NULL;
        DPF(0, "ID3DX11EffectShaderVariable::GetHullShader: This shader variable is not a hull shader");
        return D3DERR_INVALIDCALL;
    }
}

HRESULT SShaderBlock::GetDomainShader(ID3D11DomainShader **ppDS)
{
    if (EOT_DomainShader5 == GetShaderType())
    {
        *ppDS = (ID3D11DomainShader *) pD3DObject;
        SAFE_ADDREF(*ppDS);
        return S_OK;
    }
    else
    {
        *ppDS = NULL;
        DPF(0, "ID3DX11EffectShaderVariable::GetDomainShader: This shader variable is not a domain shader");
        return D3DERR_INVALIDCALL;
    }
}

HRESULT SShaderBlock::GetComputeShader(ID3D11ComputeShader **ppCS)
{
    if (EOT_ComputeShader5 == GetShaderType())
    {
        *ppCS = (ID3D11ComputeShader *) pD3DObject;
        SAFE_ADDREF(*ppCS);
        return S_OK;
    }
    else
    {
        *ppCS = NULL;
        DPF(0, "ID3DX11EffectShaderVariable::GetComputeShader: This shader variable is not a compute shader");
        return D3DERR_INVALIDCALL;
    }
}

HRESULT SShaderBlock::GetSignatureElementDesc(ESigType SigType, UINT Element, D3D11_SIGNATURE_PARAMETER_DESC *pDesc)
{
    HRESULT hr = S_OK;
    LPCSTR pFuncName;
    switch( SigType )
    {
    case ST_Input:
#pragma prefast(suppress:__WARNING_UNUSED_POINTER_ASSIGNMENT, "pFuncName used in DPF")
        pFuncName = "ID3DX11EffectShaderVariable::GetInputSignatureElementDesc";
        break;
    case ST_Output:
#pragma prefast(suppress:__WARNING_UNUSED_POINTER_ASSIGNMENT, "pFuncName used in DPF")
        pFuncName = "ID3DX11EffectShaderVariable::GetOutputSignatureElementDesc";
        break;
    case ST_PatchConstant:
#pragma prefast(suppress:__WARNING_UNUSED_POINTER_ASSIGNMENT, "pFuncName used in DPF")
        pFuncName = "ID3DX11EffectShaderVariable::GetPatchConstantSignatureElementDesc";
        break;
    default:
        D3DXASSERT( false );
        return E_FAIL;
    };

    if (NULL != pReflectionData)
    {
        // get # of signature entries
        D3DXASSERT( pReflectionData->pReflection != NULL );

        D3D11_SHADER_DESC ShaderDesc;
        VH( pReflectionData->pReflection->GetDesc( &ShaderDesc ) );

        D3D11_SIGNATURE_PARAMETER_DESC ParamDesc;
        if( pReflectionData->IsNullGS )
        {
            switch( SigType )
            {
            case ST_Input:
                // The input signature for a null-GS is the output signature of the previous VS
                SigType = ST_Output;
                break;
            case ST_PatchConstant:
                // GeometryShaders cannot have patch constant signatures
                return E_INVALIDARG;
            };
        }

        switch( SigType )
        {
        case ST_Input:
            if( Element >= ShaderDesc.InputParameters )
            {
                DPF( 0, "%s: Invalid Element index (%d) specified", pFuncName, Element );
                VH( E_INVALIDARG );
            }
            VH( pReflectionData->pReflection->GetInputParameterDesc( Element, &ParamDesc ) );
            break;
        case ST_Output:
            if( Element >= ShaderDesc.OutputParameters )
            {
                DPF( 0, "%s: Invalid Element index (%d) specified", pFuncName, Element );
                VH( E_INVALIDARG );
            }
            VH( pReflectionData->pReflection->GetOutputParameterDesc( Element, &ParamDesc ) );
            break;
        case ST_PatchConstant:
            if( Element >= ShaderDesc.PatchConstantParameters )
            {
                DPF( 0, "%s: Invalid Element index (%d) specified", pFuncName, Element );
                VH( E_INVALIDARG );
            }
            VH( pReflectionData->pReflection->GetPatchConstantParameterDesc( Element, &ParamDesc ) );
            break;
        };

        pDesc->SemanticName = ParamDesc.SemanticName;
        pDesc->SystemValueType = ParamDesc.SystemValueType;

        // Pixel shaders need to be special-cased as they don't technically output SVs
        if( pDesc->SystemValueType == D3D10_NAME_UNDEFINED && GetShaderType() == EOT_PixelShader )
        {
            if( _stricmp(pDesc->SemanticName, "SV_TARGET") == 0 )
            {
                pDesc->SystemValueType = D3D10_NAME_TARGET;
            } 
            else if( _stricmp(pDesc->SemanticName, "SV_DEPTH") == 0 )
            {
                pDesc->SystemValueType = D3D10_NAME_DEPTH;
            } 
            else if( _stricmp(pDesc->SemanticName, "SV_COVERAGE") == 0 )
            {
                pDesc->SystemValueType = D3D10_NAME_COVERAGE;
            }
        }

        pDesc->SemanticIndex = ParamDesc.SemanticIndex;
        pDesc->Register = ParamDesc.Register;
        pDesc->Mask = ParamDesc.Mask;
        pDesc->ComponentType = ParamDesc.ComponentType;
        pDesc->ReadWriteMask = ParamDesc.ReadWriteMask;
    }
    else
    {
        DPF(0, "%s: Cannot get signatures; shader bytecode is not present", pFuncName);
        VH( D3DERR_INVALIDCALL );
    }
    
lExit:
    return hr;
}

SString::SString()
{
    pString = NULL;
}

SRenderTargetView::SRenderTargetView()
{
    pRenderTargetView = NULL;
}

SDepthStencilView::SDepthStencilView()
{
    pDepthStencilView = NULL;
}

void * GetBlockByIndex(EVarType VarType, EObjectType ObjectType, void *pBaseBlock, UINT  Index)
{
    switch( VarType )
    {
    case EVT_Interface:
        return (SInterface *)pBaseBlock + Index;
    case EVT_Object:
        switch (ObjectType)
        {
        case EOT_Blend:
            return (SBlendBlock *)pBaseBlock + Index;
        case EOT_DepthStencil:
            return (SDepthStencilBlock *)pBaseBlock + Index;
        case EOT_Rasterizer:
            return (SRasterizerBlock *)pBaseBlock + Index;
        case EOT_PixelShader:
        case EOT_PixelShader5:
        case EOT_GeometryShader:
        case EOT_GeometryShaderSO:
        case EOT_GeometryShader5:
        case EOT_VertexShader:
        case EOT_VertexShader5:
        case EOT_HullShader5:
        case EOT_DomainShader5:
        case EOT_ComputeShader5:
            return (SShaderBlock *)pBaseBlock + Index;
        case EOT_String:
            return (SString *)pBaseBlock + Index;
        case EOT_Sampler:
            return (SSamplerBlock *)pBaseBlock + Index;
        case EOT_Buffer:
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
        case EOT_ByteAddressBuffer:
        case EOT_StructuredBuffer:
            return (SShaderResource *)pBaseBlock + Index;
        case EOT_DepthStencilView:
            return (SDepthStencilView *)pBaseBlock + Index;
        case EOT_RenderTargetView:
            return (SRenderTargetView *)pBaseBlock + Index;
        case EOT_RWTexture1D:
        case EOT_RWTexture1DArray:
        case EOT_RWTexture2D:
        case EOT_RWTexture2DArray:
        case EOT_RWTexture3D:
        case EOT_RWBuffer:
        case EOT_RWByteAddressBuffer:
        case EOT_RWStructuredBuffer:
        case EOT_RWStructuredBufferAlloc:
        case EOT_RWStructuredBufferConsume:
        case EOT_AppendStructuredBuffer:
        case EOT_ConsumeStructuredBuffer:    
            return (SUnorderedAccessView *)pBaseBlock + Index;
        default:
            D3DXASSERT(0);
            return NULL;
        }
    default:
        D3DXASSERT(0);
        return NULL;
    }
}

CEffect::CEffect( UINT Flags )
{
    m_RefCount = 1;

    m_pVariables = NULL;
    m_pAnonymousShaders = NULL;
    m_pGroups = NULL;
    m_pNullGroup = NULL;
    m_pShaderBlocks = NULL;
    m_pDepthStencilBlocks = NULL;
    m_pBlendBlocks = NULL;
    m_pRasterizerBlocks = NULL;
    m_pSamplerBlocks = NULL;
    m_pCBs = NULL;
    m_pStrings = NULL;
    m_pMemberDataBlocks = NULL;
    m_pInterfaces = NULL;
    m_pShaderResources = NULL;
    m_pUnorderedAccessViews = NULL;
    m_pRenderTargetViews = NULL;
    m_pDepthStencilViews = NULL;
    m_pDevice = NULL;
    m_pClassLinkage = NULL;
    m_pContext = NULL;

    m_VariableCount = 0;
    m_AnonymousShaderCount = 0;
    m_ShaderBlockCount = 0;
    m_DepthStencilBlockCount = 0;
    m_BlendBlockCount = 0;
    m_RasterizerBlockCount = 0;
    m_SamplerBlockCount = 0;
    m_StringCount = 0;
    m_MemberDataCount = 0;
    m_InterfaceCount = 0;
    m_ShaderResourceCount = 0;
    m_UnorderedAccessViewCount = 0;
    m_RenderTargetViewCount = 0;
    m_DepthStencilViewCount = 0;
    m_CBCount = 0;
    m_TechniqueCount = 0;
    m_GroupCount = 0;

    m_pReflection = NULL;
    m_LocalTimer = 1;
    m_Flags = Flags;
    m_FXLIndex = 0;

    m_pTypePool = NULL;
    m_pStringPool = NULL;
    m_pPooledHeap = NULL;
    m_pOptimizedTypeHeap = NULL;
}

void CEffect::ReleaseShaderRefection()
{
    for( UINT i = 0; i < m_ShaderBlockCount; ++ i )
    {
        SAFE_RELEASE( m_pShaderBlocks[i].pInputSignatureBlob );
        if( m_pShaderBlocks[i].pReflectionData )
        {
            SAFE_RELEASE( m_pShaderBlocks[i].pReflectionData->pReflection );
        }
    }
}

CEffect::~CEffect()
{
    ID3D11InfoQueue *pInfoQueue = NULL;

    // Mute debug spew
    if (m_pDevice)
        m_pDevice->QueryInterface(__uuidof(ID3D11InfoQueue), (void**) &pInfoQueue);

    if (pInfoQueue)
    {
        D3D11_INFO_QUEUE_FILTER filter;
        D3D11_MESSAGE_CATEGORY messageCategory = D3D11_MESSAGE_CATEGORY_STATE_SETTING;
        ZeroMemory(&filter, sizeof(filter));

        filter.DenyList.NumCategories = 1;
        filter.DenyList.pCategoryList = &messageCategory;
        pInfoQueue->PushStorageFilter(&filter);
    }

    UINT  i;

    if( NULL != m_pDevice )
    {
        // if m_pDevice == NULL, then we failed LoadEffect(), which means ReleaseShaderReflection was already called.

        // Release the shader reflection info, as it was not created on the private heap
        // This must be called before we delete m_pReflection
        ReleaseShaderRefection();
    }

    SAFE_DELETE( m_pReflection );
    SAFE_DELETE( m_pTypePool );
    SAFE_DELETE( m_pStringPool );
    SAFE_DELETE( m_pPooledHeap );
    SAFE_DELETE( m_pOptimizedTypeHeap );

    // this code assumes the effect has been loaded & relocated,
    // so check for that before freeing the resources

    if (NULL != m_pDevice)
    {
        // Keep the following in line with AddRefAllForCloning

        D3DXASSERT(NULL == m_pRasterizerBlocks || m_Heap.IsInHeap(m_pRasterizerBlocks));
        for (i = 0; i < m_RasterizerBlockCount; ++ i)
        {
            SAFE_RELEASE(m_pRasterizerBlocks[i].pRasterizerObject);
        }

        D3DXASSERT(NULL == m_pBlendBlocks || m_Heap.IsInHeap(m_pBlendBlocks));
        for (i = 0; i < m_BlendBlockCount; ++ i)
        {
            SAFE_RELEASE(m_pBlendBlocks[i].pBlendObject);
        }

        D3DXASSERT(NULL == m_pDepthStencilBlocks || m_Heap.IsInHeap(m_pDepthStencilBlocks));
        for (i = 0; i < m_DepthStencilBlockCount; ++ i)
        {
            SAFE_RELEASE(m_pDepthStencilBlocks[i].pDSObject);
        }

        D3DXASSERT(NULL == m_pSamplerBlocks || m_Heap.IsInHeap(m_pSamplerBlocks));
        for (i = 0; i < m_SamplerBlockCount; ++ i)
        {
            SAFE_RELEASE(m_pSamplerBlocks[i].pD3DObject);
        }

        D3DXASSERT(NULL == m_pShaderResources || m_Heap.IsInHeap(m_pShaderResources));
        for (i = 0; i < m_ShaderResourceCount; ++ i)
        {
            SAFE_RELEASE(m_pShaderResources[i].pShaderResource);
        }

        D3DXASSERT(NULL == m_pUnorderedAccessViews || m_Heap.IsInHeap(m_pUnorderedAccessViews));
        for (i = 0; i < m_UnorderedAccessViewCount; ++ i)
        {
            SAFE_RELEASE(m_pUnorderedAccessViews[i].pUnorderedAccessView);
        }

        D3DXASSERT(NULL == m_pRenderTargetViews || m_Heap.IsInHeap(m_pRenderTargetViews));
        for (i = 0; i < m_RenderTargetViewCount; ++ i)
        {
            SAFE_RELEASE(m_pRenderTargetViews[i].pRenderTargetView);
        }

        D3DXASSERT(NULL == m_pDepthStencilViews || m_Heap.IsInHeap(m_pDepthStencilViews));
        for (i = 0; i < m_DepthStencilViewCount; ++ i)
        {
            SAFE_RELEASE(m_pDepthStencilViews[i].pDepthStencilView);
        }

        D3DXASSERT(NULL == m_pMemberDataBlocks || m_Heap.IsInHeap(m_pMemberDataBlocks));
        for (i = 0; i < m_MemberDataCount; ++ i)
        {
            switch( m_pMemberDataBlocks[i].Type )
            {
            case MDT_ClassInstance:
                SAFE_RELEASE(m_pMemberDataBlocks[i].Data.pD3DClassInstance);
                break;
            case MDT_BlendState:
                SAFE_RELEASE(m_pMemberDataBlocks[i].Data.pD3DEffectsManagedBlendState);
                break;
            case MDT_DepthStencilState:
                SAFE_RELEASE(m_pMemberDataBlocks[i].Data.pD3DEffectsManagedDepthStencilState);
                break;
            case MDT_RasterizerState:
                SAFE_RELEASE(m_pMemberDataBlocks[i].Data.pD3DEffectsManagedRasterizerState);
                break;
            case MDT_SamplerState:
                SAFE_RELEASE(m_pMemberDataBlocks[i].Data.pD3DEffectsManagedSamplerState);
                break;
            case MDT_Buffer:
                SAFE_RELEASE(m_pMemberDataBlocks[i].Data.pD3DEffectsManagedConstantBuffer);
                break;
            case MDT_ShaderResourceView:
                SAFE_RELEASE(m_pMemberDataBlocks[i].Data.pD3DEffectsManagedTextureBuffer);
                break;
            default:
                D3DXASSERT( false );
            }
        }

        D3DXASSERT(NULL == m_pCBs || m_Heap.IsInHeap(m_pCBs));
        for (i = 0; i < m_CBCount; ++ i)
        {
            SAFE_RELEASE(m_pCBs[i].TBuffer.pShaderResource);
            SAFE_RELEASE(m_pCBs[i].pD3DObject);
        }

        D3DXASSERT(NULL == m_pShaderBlocks || m_Heap.IsInHeap(m_pShaderBlocks));
        for (i = 0; i < m_ShaderBlockCount; ++ i)
        {
            SAFE_RELEASE(m_pShaderBlocks[i].pD3DObject);
        }

        SAFE_RELEASE( m_pDevice );
    }
    SAFE_RELEASE( m_pClassLinkage );
    D3DXASSERT( m_pContext == NULL );

    // Restore debug spew
    if (pInfoQueue)
    {
        pInfoQueue->PopStorageFilter();
        SAFE_RELEASE(pInfoQueue);
    }
}

// AddRef all D3D object when cloning
void CEffect::AddRefAllForCloning( CEffect* pEffectSource )
{
    UINT  i;

    // Keep the following in line with ~CEffect

    D3DXASSERT( m_pDevice != NULL );

    for( UINT i = 0; i < m_ShaderBlockCount; ++ i )
    {
        SAFE_ADDREF( m_pShaderBlocks[i].pInputSignatureBlob );
        if( m_pShaderBlocks[i].pReflectionData )
        {
            SAFE_ADDREF( m_pShaderBlocks[i].pReflectionData->pReflection );
        }
    }

    D3DXASSERT(NULL == m_pRasterizerBlocks || pEffectSource->m_Heap.IsInHeap(m_pRasterizerBlocks));
    for (i = 0; i < m_RasterizerBlockCount; ++ i)
    {
        SAFE_ADDREF(m_pRasterizerBlocks[i].pRasterizerObject);
    }

    D3DXASSERT(NULL == m_pBlendBlocks || pEffectSource->m_Heap.IsInHeap(m_pBlendBlocks));
    for (i = 0; i < m_BlendBlockCount; ++ i)
    {
        SAFE_ADDREF(m_pBlendBlocks[i].pBlendObject);
    }

    D3DXASSERT(NULL == m_pDepthStencilBlocks || pEffectSource->m_Heap.IsInHeap(m_pDepthStencilBlocks));
    for (i = 0; i < m_DepthStencilBlockCount; ++ i)
    {
        SAFE_ADDREF(m_pDepthStencilBlocks[i].pDSObject);
    }

    D3DXASSERT(NULL == m_pSamplerBlocks || pEffectSource->m_Heap.IsInHeap(m_pSamplerBlocks));
    for (i = 0; i < m_SamplerBlockCount; ++ i)
    {
        SAFE_ADDREF(m_pSamplerBlocks[i].pD3DObject);
    }

    D3DXASSERT(NULL == m_pShaderResources || pEffectSource->m_Heap.IsInHeap(m_pShaderResources));
    for (i = 0; i < m_ShaderResourceCount; ++ i)
    {
        SAFE_ADDREF(m_pShaderResources[i].pShaderResource);
    }

    D3DXASSERT(NULL == m_pUnorderedAccessViews || pEffectSource->m_Heap.IsInHeap(m_pUnorderedAccessViews));
    for (i = 0; i < m_UnorderedAccessViewCount; ++ i)
    {
        SAFE_ADDREF(m_pUnorderedAccessViews[i].pUnorderedAccessView);
    }

    D3DXASSERT(NULL == m_pRenderTargetViews || pEffectSource->m_Heap.IsInHeap(m_pRenderTargetViews));
    for (i = 0; i < m_RenderTargetViewCount; ++ i)
    {
        SAFE_ADDREF(m_pRenderTargetViews[i].pRenderTargetView);
    }

    D3DXASSERT(NULL == m_pDepthStencilViews || pEffectSource->m_Heap.IsInHeap(m_pDepthStencilViews));
    for (i = 0; i < m_DepthStencilViewCount; ++ i)
    {
        SAFE_ADDREF(m_pDepthStencilViews[i].pDepthStencilView);
    }

    D3DXASSERT(NULL == m_pMemberDataBlocks || pEffectSource->m_Heap.IsInHeap(m_pMemberDataBlocks));
    for (i = 0; i < m_MemberDataCount; ++ i)
    {
        switch( m_pMemberDataBlocks[i].Type )
        {
        case MDT_ClassInstance:
            SAFE_ADDREF(m_pMemberDataBlocks[i].Data.pD3DClassInstance);
            break;
        case MDT_BlendState:
            SAFE_ADDREF(m_pMemberDataBlocks[i].Data.pD3DEffectsManagedBlendState);
            break;
        case MDT_DepthStencilState:
            SAFE_ADDREF(m_pMemberDataBlocks[i].Data.pD3DEffectsManagedDepthStencilState);
            break;
        case MDT_RasterizerState:
            SAFE_ADDREF(m_pMemberDataBlocks[i].Data.pD3DEffectsManagedRasterizerState);
            break;
        case MDT_SamplerState:
            SAFE_ADDREF(m_pMemberDataBlocks[i].Data.pD3DEffectsManagedSamplerState);
            break;
        case MDT_Buffer:
            SAFE_ADDREF(m_pMemberDataBlocks[i].Data.pD3DEffectsManagedConstantBuffer);
            break;
        case MDT_ShaderResourceView:
            SAFE_ADDREF(m_pMemberDataBlocks[i].Data.pD3DEffectsManagedTextureBuffer);
            break;
        default:
            D3DXASSERT( false );
        }
    }

    // There's no need to AddRef CBs, since they are recreated
    D3DXASSERT(NULL == m_pCBs || pEffectSource->m_Heap.IsInHeap(m_pCBs));
    for (i = 0; i < m_CBCount; ++ i)
    {
        SAFE_ADDREF(m_pCBs[i].TBuffer.pShaderResource);
        SAFE_ADDREF(m_pCBs[i].pD3DObject);
    }

    D3DXASSERT(NULL == m_pShaderBlocks || pEffectSource->m_Heap.IsInHeap(m_pShaderBlocks));
    for (i = 0; i < m_ShaderBlockCount; ++ i)
    {
        SAFE_ADDREF(m_pShaderBlocks[i].pD3DObject);
    }

    SAFE_ADDREF( m_pDevice );

    SAFE_ADDREF( m_pClassLinkage );
    D3DXASSERT( m_pContext == NULL );
}

HRESULT CEffect::QueryInterface(REFIID iid, LPVOID *ppv)
{
    HRESULT hr = S_OK;

    if(NULL == ppv)
    {
        DPF(0, "ID3DX11Effect::QueryInterface: NULL parameter");
        hr = E_INVALIDARG;
        goto EXIT;
    }

    *ppv = NULL;
    if(IsEqualIID(iid, IID_IUnknown))
    {
        *ppv = (IUnknown *) this;
    }
    else if(IsEqualIID(iid, IID_ID3DX11Effect))
    {
        *ppv = (ID3DX11Effect *) this;
    }
    else
    {
        return E_NOINTERFACE;
    }

    AddRef();

EXIT:
    return hr;
}

ULONG CEffect::AddRef()
{
    return ++ m_RefCount;
}

ULONG CEffect::Release()
{
    if (-- m_RefCount > 0)
    {
        return m_RefCount;
    }
    else
    {
        delete this;
    }

    return 0;
}

// In all shaders, replace pOldBufferBlock with pNewBuffer, if pOldBufferBlock is a dependency
void CEffect::ReplaceCBReference(SConstantBuffer *pOldBufferBlock, ID3D11Buffer *pNewBuffer)
{
    UINT iShaderBlock;

    for (iShaderBlock=0; iShaderBlock<m_ShaderBlockCount; iShaderBlock++)
    {
        for (UINT iCBDep = 0; iCBDep < m_pShaderBlocks[iShaderBlock].CBDepCount; iCBDep++)
        {
            for (UINT iCB = 0; iCB < m_pShaderBlocks[iShaderBlock].pCBDeps[iCBDep].Count; iCB++)
            {
                if (m_pShaderBlocks[iShaderBlock].pCBDeps[iCBDep].ppFXPointers[iCB] == pOldBufferBlock)
                    m_pShaderBlocks[iShaderBlock].pCBDeps[iCBDep].ppD3DObjects[iCB] = pNewBuffer;
            }
        }
    }
}

// In all shaders, replace pOldSamplerBlock with pNewSampler, if pOldSamplerBlock is a dependency
void CEffect::ReplaceSamplerReference(SSamplerBlock *pOldSamplerBlock, ID3D11SamplerState *pNewSampler)
{
    UINT iShaderBlock;

    for (iShaderBlock=0; iShaderBlock<m_ShaderBlockCount; iShaderBlock++)
    {
        for (UINT iSamplerDep = 0; iSamplerDep < m_pShaderBlocks[iShaderBlock].SampDepCount; iSamplerDep++)
        {
            for (UINT iSampler = 0; iSampler < m_pShaderBlocks[iShaderBlock].pSampDeps[iSamplerDep].Count; iSampler++)
            {
                if (m_pShaderBlocks[iShaderBlock].pSampDeps[iSamplerDep].ppFXPointers[iSampler] == pOldSamplerBlock)
                    m_pShaderBlocks[iShaderBlock].pSampDeps[iSamplerDep].ppD3DObjects[iSampler] = pNewSampler;
            }
        }
    }
}

// Call BindToDevice after the effect has been fully loaded.
// BindToDevice will release all D3D11 objects and create new ones on the new device
HRESULT CEffect::BindToDevice(ID3D11Device *pDevice)
{
    HRESULT hr = S_OK;

    // Set new device
    if (pDevice == NULL)
    {
        DPF(0, "ID3DX11Effect: pDevice must point to a valid D3D11 device");
        return D3DERR_INVALIDCALL;
    }

    if (m_pDevice != NULL)
    {
        DPF(0, "ID3DX11Effect: Internal error, rebinding effects to a new device is not supported");
        return D3DERR_INVALIDCALL;
    }

    bool featureLevelGE11 = ( pDevice->GetFeatureLevel() >= D3D_FEATURE_LEVEL_11_0 );

    pDevice->AddRef();
    SAFE_RELEASE(m_pDevice);
    m_pDevice = pDevice;
    VH( m_pDevice->CreateClassLinkage( &m_pClassLinkage ) );

    // Create all constant buffers
    SConstantBuffer *pCB = m_pCBs;
    SConstantBuffer *pCBLast = m_pCBs + m_CBCount;
    for(; pCB != pCBLast; pCB++)
    {
        SAFE_RELEASE(pCB->pD3DObject);
        SAFE_RELEASE(pCB->TBuffer.pShaderResource);

        // This is a CBuffer
        if (pCB->Size > 0)
        {
            if (pCB->IsTBuffer)
            {
                D3D11_BUFFER_DESC bufDesc;
                // size is always register aligned
                bufDesc.ByteWidth = pCB->Size;
                bufDesc.Usage = D3D11_USAGE_DEFAULT;
                bufDesc.BindFlags = D3D11_BIND_SHADER_RESOURCE;
                bufDesc.CPUAccessFlags = 0;
                bufDesc.MiscFlags = 0;

                VH( pDevice->CreateBuffer( &bufDesc, NULL, &pCB->pD3DObject) );
                
                D3D11_SHADER_RESOURCE_VIEW_DESC viewDesc;
                viewDesc.Format = DXGI_FORMAT_R32G32B32A32_UINT;
                viewDesc.ViewDimension = D3D11_SRV_DIMENSION_BUFFER;
                viewDesc.Buffer.ElementOffset = 0;
                viewDesc.Buffer.ElementWidth = pCB->Size / SType::c_RegisterSize;

                VH( pDevice->CreateShaderResourceView( pCB->pD3DObject, &viewDesc, &pCB->TBuffer.pShaderResource) );
            }
            else
            {
                D3D11_BUFFER_DESC bufDesc;
                // size is always register aligned
                bufDesc.ByteWidth = pCB->Size;
                bufDesc.Usage = D3D11_USAGE_DEFAULT;
                bufDesc.BindFlags = D3D11_BIND_CONSTANT_BUFFER;
                bufDesc.CPUAccessFlags = 0;
                bufDesc.MiscFlags = 0;

                VH( pDevice->CreateBuffer( &bufDesc, NULL, &pCB->pD3DObject) );
                pCB->TBuffer.pShaderResource = NULL;
            }

            pCB->IsDirty = TRUE;
        }
        else
        {
            pCB->IsDirty = FALSE;
        }
    }

    // Create all RasterizerStates
    SRasterizerBlock *pRB = m_pRasterizerBlocks;
    SRasterizerBlock *pRBLast = m_pRasterizerBlocks + m_RasterizerBlockCount;
    for(; pRB != pRBLast; pRB++)
    {
        SAFE_RELEASE(pRB->pRasterizerObject);
        if( SUCCEEDED( m_pDevice->CreateRasterizerState( &pRB->BackingStore, &pRB->pRasterizerObject) ) )
            pRB->IsValid = TRUE;
        else
            pRB->IsValid = FALSE;
    }

    // Create all DepthStencils
    SDepthStencilBlock *pDS = m_pDepthStencilBlocks;
    SDepthStencilBlock *pDSLast = m_pDepthStencilBlocks + m_DepthStencilBlockCount;
    for(; pDS != pDSLast; pDS++)
    {
        SAFE_RELEASE(pDS->pDSObject);
        if( SUCCEEDED( m_pDevice->CreateDepthStencilState( &pDS->BackingStore, &pDS->pDSObject) ) )
            pDS->IsValid = TRUE;
        else
            pDS->IsValid = FALSE;
    }

    // Create all BlendStates
    SBlendBlock *pBlend = m_pBlendBlocks;
    SBlendBlock *pBlendLast = m_pBlendBlocks + m_BlendBlockCount;
    for(; pBlend != pBlendLast; pBlend++)
    {
        SAFE_RELEASE(pBlend->pBlendObject);
        if( SUCCEEDED( m_pDevice->CreateBlendState( &pBlend->BackingStore, &pBlend->pBlendObject ) ) )
            pBlend->IsValid = TRUE;
        else
            pBlend->IsValid = FALSE;
    }

    // Create all Samplers
    SSamplerBlock *pSampler = m_pSamplerBlocks;
    SSamplerBlock *pSamplerLast = m_pSamplerBlocks + m_SamplerBlockCount;
    for(; pSampler != pSamplerLast; pSampler++)
    {
        SAFE_RELEASE(pSampler->pD3DObject);

        VH( m_pDevice->CreateSamplerState( &pSampler->BackingStore.SamplerDesc, &pSampler->pD3DObject) );
    }

    // Create all shaders
    ID3D11ClassLinkage* neededClassLinkage = featureLevelGE11 ? m_pClassLinkage : NULL;
    SShaderBlock *pShader = m_pShaderBlocks;
    SShaderBlock *pShaderLast = m_pShaderBlocks + m_ShaderBlockCount;
    for(; pShader != pShaderLast; pShader++)
    {
        SAFE_RELEASE(pShader->pD3DObject);

        if (NULL == pShader->pReflectionData)
        {
            // NULL shader. It's one of these:
            // PixelShader ps;
            // or
            // SetPixelShader( NULL );
            continue;
        }
        
        if (pShader->pReflectionData->pStreamOutDecls[0] || pShader->pReflectionData->pStreamOutDecls[1] || 
            pShader->pReflectionData->pStreamOutDecls[2] || pShader->pReflectionData->pStreamOutDecls[3] )
        {
            // This is a geometry shader, process it's data
            CSOParser soParser;
            VH( soParser.Parse(pShader->pReflectionData->pStreamOutDecls) );
            UINT strides[4];
            soParser.GetStrides( strides );
            hr = m_pDevice->CreateGeometryShaderWithStreamOutput((UINT*) pShader->pReflectionData->pBytecode,
                                                                pShader->pReflectionData->BytecodeLength,
                                                                soParser.GetDeclArray(),
                                                                soParser.GetDeclCount(),
                                                                strides,
                                                                featureLevelGE11 ? 4 : 1,
                                                                pShader->pReflectionData->RasterizedStream,
                                                                neededClassLinkage,
                                                                (ID3D11GeometryShader**) &pShader->pD3DObject);
            if (FAILED(hr))
            {
                DPF(1, "ID3DX11Effect::Load - failed to create GeometryShader with StreamOutput decl: \"%s\"", soParser.GetErrorString() );
                pShader->IsValid = FALSE;
                hr = S_OK;
            }
        }
        else
        {
            // This is a regular shader
            if( pShader->pReflectionData->RasterizedStream == D3D11_SO_NO_RASTERIZED_STREAM )
                pShader->IsValid = FALSE;
            else 
            {
                if( FAILED( (m_pDevice->*(pShader->pVT->pCreateShader))( (UINT *) pShader->pReflectionData->pBytecode, pShader->pReflectionData->BytecodeLength, neededClassLinkage, &pShader->pD3DObject) ) )
                {
                    DPF(1, "ID3DX11Effect::Load - failed to create shader" );
                    pShader->IsValid = FALSE;
                }
            }
        }

        // Update all dependency pointers
        VH( pShader->OnDeviceBind() );
    }

    // Initialize the member data pointers for all variables
    UINT CurMemberData = 0;
    for (UINT i = 0; i < m_VariableCount; ++ i)
    {
        if( m_pVariables[i].pMemberData )
        {
            if( m_pVariables[i].pType->IsClassInstance() )
            {
                for (UINT j = 0; j < max(m_pVariables[i].pType->Elements,1); ++j)
                {
                    D3DXASSERT( CurMemberData < m_MemberDataCount );
                    ID3D11ClassInstance** ppCI = &(m_pVariables[i].pMemberData + j)->Data.pD3DClassInstance;
                    (m_pVariables[i].pMemberData + j)->Type = MDT_ClassInstance;
                    (m_pVariables[i].pMemberData + j)->Data.pD3DClassInstance = NULL;
                    if( m_pVariables[i].pType->TotalSize > 0 )
                    {
                        // ignore failures in GetClassInstance;
                        m_pClassLinkage->GetClassInstance( m_pVariables[i].pName, j, ppCI );
                    }
                    else
                    {
                        // The HLSL compiler optimizes out zero-sized classes, so we have to create class instances from scratch
                        if( FAILED( m_pClassLinkage->CreateClassInstance( m_pVariables[i].pType->pTypeName, 0, 0, 0, 0, ppCI ) ) )
                        {
                            DPF(0, "ID3DX11Effect: Out of memory while trying to create new class instance interface");
                        }
                    }
                    CurMemberData++;
                }
            }
            else if( m_pVariables[i].pType->IsStateBlockObject() )
            {
                for (UINT j = 0; j < max(m_pVariables[i].pType->Elements,1); ++j)
                {
                    switch( m_pVariables[i].pType->ObjectType )
                    {
                    case EOT_Blend:
                        (m_pVariables[i].pMemberData + j)->Type = MDT_BlendState;
                        (m_pVariables[i].pMemberData + j)->Data.pD3DEffectsManagedBlendState = NULL;
                        break;
                    case EOT_Rasterizer:
                        (m_pVariables[i].pMemberData + j)->Type = MDT_RasterizerState;
                        (m_pVariables[i].pMemberData + j)->Data.pD3DEffectsManagedRasterizerState = NULL;
                        break;
                    case EOT_DepthStencil:
                        (m_pVariables[i].pMemberData + j)->Type = MDT_DepthStencilState;
                        (m_pVariables[i].pMemberData + j)->Data.pD3DEffectsManagedDepthStencilState = NULL;
                        break;
                    case EOT_Sampler:
                        (m_pVariables[i].pMemberData + j)->Type = MDT_SamplerState;
                        (m_pVariables[i].pMemberData + j)->Data.pD3DEffectsManagedSamplerState = NULL;
                        break;
                    default:
                        VB( FALSE );
                    }
                    CurMemberData++;
                }
            }
            else
            {
                VB( FALSE );
            }
        }
    }
    for(pCB = m_pCBs; pCB != pCBLast; pCB++)
    {
        (pCB->pMemberData + 0)->Type = MDT_Buffer;
        (pCB->pMemberData + 0)->Data.pD3DEffectsManagedConstantBuffer = NULL;
        CurMemberData++;
        (pCB->pMemberData + 1)->Type = MDT_ShaderResourceView;
        (pCB->pMemberData + 1)->Data.pD3DEffectsManagedTextureBuffer = NULL;
        CurMemberData++;
    }


    // Determine which techniques and passes are known to be invalid
    for( UINT iGroup=0; iGroup < m_GroupCount; iGroup++ )
    {
        SGroup* pGroup = &m_pGroups[iGroup];
        pGroup->InitiallyValid = TRUE;

        for( UINT iTech=0; iTech < pGroup->TechniqueCount; iTech++ )
        {
            STechnique* pTechnique = &pGroup->pTechniques[iTech];
            pTechnique->InitiallyValid = TRUE;
           
            for( UINT iPass = 0; iPass < pTechnique->PassCount; iPass++ )
            {
                SPassBlock* pPass = &pTechnique->pPasses[iPass];
                pPass->InitiallyValid = TRUE;

                if( pPass->BackingStore.pBlendBlock != NULL && !pPass->BackingStore.pBlendBlock->IsValid )
                    pPass->InitiallyValid = FALSE;
                if( pPass->BackingStore.pDepthStencilBlock != NULL && !pPass->BackingStore.pDepthStencilBlock->IsValid )
                    pPass->InitiallyValid = FALSE;
                if( pPass->BackingStore.pRasterizerBlock != NULL && !pPass->BackingStore.pRasterizerBlock->IsValid )
                    pPass->InitiallyValid = FALSE;
                if( pPass->BackingStore.pVertexShaderBlock != NULL && !pPass->BackingStore.pVertexShaderBlock->IsValid )
                    pPass->InitiallyValid = FALSE;
                if( pPass->BackingStore.pPixelShaderBlock != NULL && !pPass->BackingStore.pPixelShaderBlock->IsValid )
                    pPass->InitiallyValid = FALSE;
                if( pPass->BackingStore.pGeometryShaderBlock != NULL && !pPass->BackingStore.pGeometryShaderBlock->IsValid )
                    pPass->InitiallyValid = FALSE;
                if( pPass->BackingStore.pHullShaderBlock != NULL && !pPass->BackingStore.pHullShaderBlock->IsValid )
                    pPass->InitiallyValid = FALSE;
                if( pPass->BackingStore.pDomainShaderBlock != NULL && !pPass->BackingStore.pDomainShaderBlock->IsValid )
                    pPass->InitiallyValid = FALSE;
                if( pPass->BackingStore.pComputeShaderBlock != NULL && !pPass->BackingStore.pComputeShaderBlock->IsValid )
                    pPass->InitiallyValid = FALSE;

                pTechnique->InitiallyValid &= pPass->InitiallyValid;
            }
            pGroup->InitiallyValid &= pTechnique->InitiallyValid;
        }
    }

lExit:
    return hr;
}

// FindVariableByName, plus an understanding of literal indices
// This code handles A[i].
// It does not handle anything else, like A.B, A[B[i]], A[B]
SVariable * CEffect::FindVariableByNameWithParsing(LPCSTR pName)
{
    SGlobalVariable *pVariable;
    const UINT MAX_PARSABLE_NAME_LENGTH = 256;
    char pScratchString[MAX_PARSABLE_NAME_LENGTH];

    const char* pSource = pName;
    char* pDest = pScratchString;
    char* pEnd = pScratchString + MAX_PARSABLE_NAME_LENGTH;

    pVariable = NULL;

    while( *pSource != 0 )
    {
        if( pDest == pEnd )
        {
            pVariable = FindLocalVariableByName(pName);
            if( pVariable == NULL )
            {
                DPF( 0, "Name %s is too long to parse", &pName );
            }
            return pVariable;
        }

        if( *pSource == '[' )
        {
            // parse previous variable name
            *pDest = 0;
            D3DXASSERT( pVariable == NULL );
            pVariable = FindLocalVariableByName(pScratchString);
            if( pVariable == NULL )
            {
                return NULL;
            }
            pDest = pScratchString;
        }
        else if( *pSource == ']' )
        {
            // parse integer
            *pDest = 0;
            UINT index = atoi(pScratchString);
            D3DXASSERT( pVariable != NULL );
            pVariable = (SGlobalVariable*)pVariable->GetElement(index);
            if( pVariable && !pVariable->IsValid() )
            {
                pVariable = NULL;
            }
            return pVariable;
        }
        else
        {
            // add character
            *pDest = *pSource;
            pDest++;
        }
        pSource++;
    }

    if( pDest != pScratchString )
    {
        // parse the variable name (there was no [i])
        *pDest = 0;
        D3DXASSERT( pVariable == NULL );
        pVariable = FindLocalVariableByName(pScratchString);
    }

    return pVariable;
}

SGlobalVariable * CEffect::FindVariableByName(LPCSTR pName)
{
    SGlobalVariable *pVariable;

    pVariable = FindLocalVariableByName(pName);

    return pVariable;
}

SGlobalVariable * CEffect::FindLocalVariableByName(LPCSTR pName)
{
    SGlobalVariable *pVariable, *pVariableEnd;

    pVariableEnd = m_pVariables + m_VariableCount;
    for (pVariable = m_pVariables; pVariable != pVariableEnd; pVariable++)
    {
        if (strcmp( pVariable->pName, pName) == 0)
        {
            return pVariable;
        }
    }

    return NULL;
}


//
// Checks to see if two types are equivalent (either at runtime
// or during the type-pooling load process)
//
// Major assumption: if both types are structures, then their
// member types & names should already have been added to the pool,
// in which case their member type & name pointers should be equal.
//
// This is true because complex data types (structures) have all
// sub-types translated before the containing type is translated,
// which means that simple sub-types (numeric types) have already
// been pooled.
//
BOOL SType::IsEqual(SType *pOtherType) CONST
{
    if (VarType != pOtherType->VarType || Elements != pOtherType->Elements
        || strcmp(pTypeName, pOtherType->pTypeName) != 0)
    {
        return FALSE;
    }

    switch (VarType)
    {
    case EVT_Struct:
        {
            if (StructType.Members != pOtherType->StructType.Members)
            {
                return FALSE;
            }
            D3DXASSERT(StructType.pMembers != NULL && pOtherType->StructType.pMembers != NULL);

            UINT  i;
            for (i = 0; i < StructType.Members; ++ i)
            {
                // names for types must exist (not true for semantics)
                D3DXASSERT(StructType.pMembers[i].pName != NULL && pOtherType->StructType.pMembers[i].pName != NULL);

                if (StructType.pMembers[i].pType != pOtherType->StructType.pMembers[i].pType ||
                    StructType.pMembers[i].Data.Offset != pOtherType->StructType.pMembers[i].Data.Offset ||
                    StructType.pMembers[i].pName != pOtherType->StructType.pMembers[i].pName ||
                    StructType.pMembers[i].pSemantic != pOtherType->StructType.pMembers[i].pSemantic)
                {
                    return FALSE;
                }
            }
        }
        break;

    case EVT_Object:
        {
            if (ObjectType != pOtherType->ObjectType)
            {
                return FALSE;
            }
        }
        break;

    case EVT_Numeric:
        {
            if (NumericType.Rows != pOtherType->NumericType.Rows ||
                NumericType.Columns != pOtherType->NumericType.Columns ||
                NumericType.ScalarType != pOtherType->NumericType.ScalarType ||
                NumericType.NumericLayout != pOtherType->NumericType.NumericLayout ||
                NumericType.IsColumnMajor != pOtherType->NumericType.IsColumnMajor ||
                NumericType.IsPackedArray != pOtherType->NumericType.IsPackedArray)
            {
                return FALSE;
            }
        }
        break;

    case EVT_Interface:
        {
            // VarType and pTypeName handled above
        }
        break;

    default:
        {
            D3DXASSERT(0);
            return FALSE;
        }
        break;
    }

    D3DXASSERT(TotalSize == pOtherType->TotalSize && Stride == pOtherType->Stride && PackedSize == pOtherType->PackedSize);

    return TRUE;
}

UINT SType::GetTotalUnpackedSize(BOOL IsSingleElement) CONST
{
    if (VarType == EVT_Object)
    {
        return 0;
    }
    else if (VarType == EVT_Interface)
    {
        return 0;
    }
    else if (Elements > 0 && IsSingleElement)
    {
        D3DXASSERT( ( TotalSize == 0 && Stride == 0 ) ||
                    ( (TotalSize > (Stride * (Elements - 1))) && (TotalSize <= (Stride * Elements)) ) );
        return TotalSize - Stride * (Elements - 1);
    }
    else
    {
        return TotalSize;
    }
}

UINT SType::GetTotalPackedSize(BOOL IsSingleElement) CONST
{
    if (Elements > 0 && IsSingleElement)
    {
        D3DXASSERT(PackedSize % Elements == 0);
        return PackedSize / Elements;
    }
    else
    {
        return PackedSize;
    }
}

SConstantBuffer *CEffect::FindCB(LPCSTR pName)
{
    UINT  i;

    for (i=0; i<m_CBCount; i++)
    {
        if (!strcmp(m_pCBs[i].pName, pName))
        {
            return &m_pCBs[i];
        }
    }

    return NULL;
}

inline UINT  PtrToDword(void *pPtr)
{
    return (UINT)(UINT_PTR) pPtr;
}

BOOL CEffect::IsOptimized()
{
    if ((m_Flags & D3DX11_EFFECT_OPTIMIZED) != 0)
    {
        D3DXASSERT(NULL == m_pReflection);
        return TRUE;
    }
    else
    {
        D3DXASSERT(NULL != m_pReflection);
        return FALSE;
    }
}

// Replace *ppType with the corresponding value in pMappingTable
// pMappingTable table describes how to map old type pointers to new type pointers
static HRESULT RemapType(SType **ppType, CPointerMappingTable *pMappingTable)
{
    HRESULT hr = S_OK;

    SPointerMapping ptrMapping;
    CPointerMappingTable::CIterator iter;
    ptrMapping.pOld = *ppType;
    VH( pMappingTable->FindValueWithHash(ptrMapping, ptrMapping.Hash(), &iter) );
    *ppType = (SType *) iter.GetData().pNew;

lExit:
    return hr;
}

// Replace *ppString with the corresponding value in pMappingTable
// pMappingTable table describes how to map old string pointers to new string pointers
static HRESULT RemapString(__in char **ppString, CPointerMappingTable *pMappingTable)
{
    HRESULT hr = S_OK;

    SPointerMapping ptrMapping;
    CPointerMappingTable::CIterator iter;
    ptrMapping.pOld = *ppString;
    VH( pMappingTable->FindValueWithHash(ptrMapping, ptrMapping.Hash(), &iter) );
    *ppString = (char *) iter.GetData().pNew;

lExit:
    return hr;
}

// Used in cloning, copy m_pMemberInterfaces from pEffectSource to this
HRESULT CEffect::CopyMemberInterfaces( CEffect* pEffectSource )
{
    HRESULT hr = S_OK;
    UINT i; // after a failure, this holds the failing index

    UINT Members = pEffectSource->m_pMemberInterfaces.GetSize();
    m_pMemberInterfaces.AddRange(Members);
    for( i=0; i < Members; i++ )
    {
        SMember* pOldMember = pEffectSource->m_pMemberInterfaces[i];
        if( pOldMember == NULL )
        {
            // During Optimization, m_pMemberInterfaces[i] was set to NULL because it was an annotation
            m_pMemberInterfaces[i] = NULL;
            continue;
        }

        SMember *pNewMember;
        D3DXASSERT( pOldMember->pTopLevelEntity != NULL );

        if (NULL == (pNewMember = CreateNewMember((SType*)pOldMember->pType, FALSE)))
        {
            DPF(0, "ID3DX11Effect: Out of memory while trying to create new member variable interface");
            VN( pNewMember );
        }

        pNewMember->pType = pOldMember->pType;
        pNewMember->pName = pOldMember->pName;
        pNewMember->pSemantic = pOldMember->pSemantic;
        pNewMember->Data.pGeneric = pOldMember->Data.pGeneric;
        pNewMember->IsSingleElement = pOldMember->IsSingleElement;
        pNewMember->pTopLevelEntity = pOldMember->pTopLevelEntity;
        pNewMember->pMemberData = pOldMember->pMemberData;

        m_pMemberInterfaces[i] = pNewMember;
    }

lExit:
    if( FAILED(hr) )
    {
        D3DXASSERT( i < Members );
        ZeroMemory( &m_pMemberInterfaces[i], sizeof(SMember) * ( Members - i ) );
    }
    return hr;
}

// Used in cloning, copy the string pool from pEffectSource to this and build mappingTable
// for use in RemapString
HRESULT CEffect::CopyStringPool( CEffect* pEffectSource, CPointerMappingTable& mappingTable )
{
    HRESULT hr = S_OK;
    D3DXASSERT( m_pPooledHeap != NULL );
    VN( m_pStringPool = NEW CEffect::CStringHashTable );
    m_pStringPool->SetPrivateHeap(m_pPooledHeap);
    VH( m_pStringPool->AutoGrow() );

    CStringHashTable::CIterator stringIter;

    // move strings over, build mapping table
    for (pEffectSource->m_pStringPool->GetFirstEntry(&stringIter); !pEffectSource->m_pStringPool->PastEnd(&stringIter); pEffectSource->m_pStringPool->GetNextEntry(&stringIter))
    {
        SPointerMapping ptrMapping;
        char *pString;

        const char* pOldString = stringIter.GetData();
        ptrMapping.pOld = (void*)pOldString;
        UINT len = (UINT)strlen(pOldString);
        UINT hash = ptrMapping.Hash();
        VN( pString = new(*m_pPooledHeap) char[len + 1] );
        ptrMapping.pNew = (void*)pString;
        memcpy(ptrMapping.pNew, ptrMapping.pOld, len + 1);
        VH( m_pStringPool->AddValueWithHash(pString, hash) );

        VH( mappingTable.AddValueWithHash(ptrMapping, hash) );
    }

    // Uncomment to print string mapping
    /*
    CPointerMappingTable::CIterator mapIter;
    for (mappingTable.GetFirstEntry(&mapIter); !mappingTable.PastEnd(&mapIter); mappingTable.GetNextEntry(&mapIter))
    {
    SPointerMapping ptrMapping = mapIter.GetData();
    DPF(0, "string: 0x%x : 0x%x  %s", (UINT_PTR)ptrMapping.pOld, (UINT_PTR)ptrMapping.pNew, (char*)ptrMapping.pNew );
    }*/

lExit:
    return hr;
}

// Used in cloning, copy the unoptimized type pool from pEffectSource to this and build mappingTableTypes
// for use in RemapType.  mappingTableStrings is the mapping table previously filled when copying strings.
HRESULT CEffect::CopyTypePool( CEffect* pEffectSource, CPointerMappingTable& mappingTableTypes, CPointerMappingTable& mappingTableStrings )
{
    HRESULT hr = S_OK;
    D3DXASSERT( m_pPooledHeap != NULL );
    VN( m_pTypePool = NEW CEffect::CTypeHashTable );
    m_pTypePool->SetPrivateHeap(m_pPooledHeap);
    VH( m_pTypePool->AutoGrow() );

    CTypeHashTable::CIterator typeIter;
    CPointerMappingTable::CIterator mapIter;

    // first pass: move types over, build mapping table
    for (pEffectSource->m_pTypePool->GetFirstEntry(&typeIter); !pEffectSource->m_pTypePool->PastEnd(&typeIter); pEffectSource->m_pTypePool->GetNextEntry(&typeIter))
    {
        SPointerMapping ptrMapping;
        SType *pType;

        ptrMapping.pOld = typeIter.GetData();
        UINT hash = ptrMapping.Hash();
        VN( (ptrMapping.pNew) = new(*m_pPooledHeap) SType );
        memcpy(ptrMapping.pNew, ptrMapping.pOld, sizeof(SType));

        pType = (SType *) ptrMapping.pNew;

        // if this is a struct, move its members to the newly allocated space
        if (EVT_Struct == pType->VarType)
        {
            SVariable* pOldMembers = pType->StructType.pMembers;
            VN( pType->StructType.pMembers = new(*m_pPooledHeap) SVariable[pType->StructType.Members] );
            memcpy(pType->StructType.pMembers, pOldMembers, pType->StructType.Members * sizeof(SVariable));
        }

        VH( m_pTypePool->AddValueWithHash(pType, hash) );
        VH( mappingTableTypes.AddValueWithHash(ptrMapping, hash) );
    }

    // second pass: fixup structure member & name pointers
    for (mappingTableTypes.GetFirstEntry(&mapIter); !mappingTableTypes.PastEnd(&mapIter); mappingTableTypes.GetNextEntry(&mapIter))
    {
        SPointerMapping ptrMapping = mapIter.GetData();

        // Uncomment to print type mapping
        //DPF(0, "type: 0x%x : 0x%x", (UINT_PTR)ptrMapping.pOld, (UINT_PTR)ptrMapping.pNew );

        SType *pType = (SType *) ptrMapping.pNew;

        if( pType->pTypeName )
        {
            VH( RemapString(&pType->pTypeName, &mappingTableStrings) );
        }

        // if this is a struct, fix up its members' pointers
        if (EVT_Struct == pType->VarType)
        {
            for (UINT i = 0; i < pType->StructType.Members; ++ i)
            {
                VH( RemapType((SType**)&pType->StructType.pMembers[i].pType, &mappingTableTypes) );
                if( pType->StructType.pMembers[i].pName )
                {
                    VH( RemapString(&pType->StructType.pMembers[i].pName, &mappingTableStrings) );
                }
                if( pType->StructType.pMembers[i].pSemantic )
                {
                    VH( RemapString(&pType->StructType.pMembers[i].pSemantic, &mappingTableStrings) );
                }
            }
        }
    } 

lExit:
    return hr;
}

// Used in cloning, copy the unoptimized type pool from pEffectSource to this and build mappingTableTypes
// for use in RemapType.  mappingTableStrings is the mapping table previously filled when copying strings.
HRESULT CEffect::CopyOptimizedTypePool( CEffect* pEffectSource, CPointerMappingTable& mappingTableTypes )
{
    HRESULT hr = S_OK;
    CEffectHeap* pOptimizedTypeHeap = NULL;

    D3DXASSERT( pEffectSource->m_pOptimizedTypeHeap != NULL );
    D3DXASSERT( m_pTypePool == NULL );
    D3DXASSERT( m_pStringPool == NULL );
    D3DXASSERT( m_pPooledHeap == NULL );

    VN( pOptimizedTypeHeap = NEW CEffectHeap );
    VH( pOptimizedTypeHeap->ReserveMemory( pEffectSource->m_pOptimizedTypeHeap->GetSize() ) );
    CPointerMappingTable::CIterator mapIter;

    // first pass: move types over, build mapping table
    BYTE* pReadTypes = pEffectSource->m_pOptimizedTypeHeap->GetDataStart();
    while( pEffectSource->m_pOptimizedTypeHeap->IsInHeap( pReadTypes ) )
    {
        SPointerMapping ptrMapping;
        SType *pType;
        UINT moveSize;

        ptrMapping.pOld = ptrMapping.pNew = pReadTypes;
        moveSize = sizeof(SType);
        VH( pOptimizedTypeHeap->MoveData(&ptrMapping.pNew, moveSize) );
        pReadTypes += moveSize;

        pType = (SType *) ptrMapping.pNew;

        // if this is a struct, move its members to the newly allocated space
        if (EVT_Struct == pType->VarType)
        {
            moveSize = pType->StructType.Members * sizeof(SVariable);
            VH( pOptimizedTypeHeap->MoveData((void **)&pType->StructType.pMembers, moveSize) );
            pReadTypes += moveSize;
        }

        VH( mappingTableTypes.AddValueWithHash(ptrMapping, ptrMapping.Hash()) );
    }

    // second pass: fixup structure member & name pointers
    for (mappingTableTypes.GetFirstEntry(&mapIter); !mappingTableTypes.PastEnd(&mapIter); mappingTableTypes.GetNextEntry(&mapIter))
    {
        SPointerMapping ptrMapping = mapIter.GetData();

        // Uncomment to print type mapping
        //DPF(0, "type: 0x%x : 0x%x", (UINT_PTR)ptrMapping.pOld, (UINT_PTR)ptrMapping.pNew );

        SType *pType = (SType *) ptrMapping.pNew;

        // if this is a struct, fix up its members' pointers
        if (EVT_Struct == pType->VarType)
        {
            for (UINT i = 0; i < pType->StructType.Members; ++ i)
            {
                VH( RemapType((SType**)&pType->StructType.pMembers[i].pType, &mappingTableTypes) );
            }
        }
    }  

lExit:
    return hr;
}

// Used in cloning, create new ID3D11ConstantBuffers for each non-single CB
HRESULT CEffect::RecreateCBs()
{
    HRESULT hr = S_OK;
    UINT i; // after a failure, this holds the failing index

    for (i = 0; i < m_CBCount; ++ i)
    {
        SConstantBuffer* pCB = &m_pCBs[i];

        pCB->IsNonUpdatable = pCB->IsUserManaged || pCB->ClonedSingle();

        if( pCB->Size > 0 && !pCB->ClonedSingle() )
        {
            ID3D11Buffer** ppOriginalBuffer;
            ID3D11ShaderResourceView** ppOriginalTBufferView;

            if( pCB->IsUserManaged )
            {
                ppOriginalBuffer = &pCB->pMemberData[0].Data.pD3DEffectsManagedConstantBuffer;
                ppOriginalTBufferView = &pCB->pMemberData[1].Data.pD3DEffectsManagedTextureBuffer;
            }
            else
            {
                ppOriginalBuffer = &pCB->pD3DObject;
                ppOriginalTBufferView = &pCB->TBuffer.pShaderResource;
            }

            VN( *ppOriginalBuffer );
            D3D11_BUFFER_DESC bufDesc;
            (*ppOriginalBuffer)->GetDesc( &bufDesc );
            ID3D11Buffer* pNewBuffer = NULL;
            VH( m_pDevice->CreateBuffer( &bufDesc, NULL, &pNewBuffer ) );
            (*ppOriginalBuffer)->Release();
            (*ppOriginalBuffer) = pNewBuffer;
            pNewBuffer = NULL;

            if( pCB->IsTBuffer )
            {
                VN( *ppOriginalTBufferView );
                D3D11_SHADER_RESOURCE_VIEW_DESC viewDesc;
                (*ppOriginalTBufferView)->GetDesc( &viewDesc );
                ID3D11ShaderResourceView* pNewView = NULL;
                VH( m_pDevice->CreateShaderResourceView( (*ppOriginalBuffer), &viewDesc, &pNewView) );
                (*ppOriginalTBufferView)->Release();
                (*ppOriginalTBufferView) = pNewView;
                pNewView = NULL;
            }
            else
            {
                D3DXASSERT( *ppOriginalTBufferView == NULL );
                ReplaceCBReference( pCB, (*ppOriginalBuffer) );
            }

            pCB->IsDirty = TRUE;
        }
    }

lExit:
    return hr;
}

// Move Name and Semantic strings using mappingTableStrings
HRESULT CEffect::FixupMemberInterface( SMember* pMember, CEffect* pEffectSource, CPointerMappingTable& mappingTableStrings )
{
    HRESULT hr = S_OK;

    if( pMember->pName )
    {
        if( pEffectSource->m_pReflection && pEffectSource->m_pReflection->m_Heap.IsInHeap(pMember->pName) )
        {
            pMember->pName = (char*)((UINT_PTR)pMember->pName - (UINT_PTR)pEffectSource->m_pReflection->m_Heap.GetDataStart() + (UINT_PTR)m_pReflection->m_Heap.GetDataStart());
        }
        else
        {
            VH( RemapString(&pMember->pName, &mappingTableStrings) );
        }
    }
    if( pMember->pSemantic )
    {
        if( pEffectSource->m_pReflection && pEffectSource->m_pReflection->m_Heap.IsInHeap(pMember->pSemantic) )
        {
            pMember->pSemantic = (char*)((UINT_PTR)pMember->pSemantic - (UINT_PTR)pEffectSource->m_pReflection->m_Heap.GetDataStart() + (UINT_PTR)m_pReflection->m_Heap.GetDataStart());
        }
        else
        {
            VH( RemapString(&pMember->pSemantic, &mappingTableStrings) );
        }
    }

lExit:
    return hr;
}

//////////////////////////////////////////////////////////////////////////
// Public API to create a copy of this effect
HRESULT CEffect::CloneEffect(UINT Flags, ID3DX11Effect** ppClonedEffect )
{
    HRESULT hr = S_OK;
    CPointerMappingTable mappingTableTypes;
    CPointerMappingTable mappingTableStrings;

    CEffectLoader loader;
    CEffect* pNewEffect = NULL;    
    CDataBlockStore* pTempHeap = NULL;


    VN( pNewEffect = NEW CEffect( m_Flags ) );
    if( Flags & D3DX11_EFFECT_CLONE_FORCE_NONSINGLE )
    {
        // The effect is cloned as if there was no original, so don't mark it as cloned
        pNewEffect->m_Flags &= ~(UINT)D3DX11_EFFECT_CLONE;
    }
    else
    {
        pNewEffect->m_Flags |= D3DX11_EFFECT_CLONE;
    }

    pNewEffect->m_VariableCount = m_VariableCount;
    pNewEffect->m_pVariables = m_pVariables;
    pNewEffect->m_AnonymousShaderCount = m_AnonymousShaderCount;
    pNewEffect->m_pAnonymousShaders = m_pAnonymousShaders;
    pNewEffect->m_TechniqueCount = m_TechniqueCount;
    pNewEffect->m_GroupCount = m_GroupCount;
    pNewEffect->m_pGroups = m_pGroups;
    pNewEffect->m_pNullGroup = m_pNullGroup;
    pNewEffect->m_ShaderBlockCount = m_ShaderBlockCount;
    pNewEffect->m_pShaderBlocks = m_pShaderBlocks;
    pNewEffect->m_DepthStencilBlockCount = m_DepthStencilBlockCount;
    pNewEffect->m_pDepthStencilBlocks = m_pDepthStencilBlocks;
    pNewEffect->m_BlendBlockCount = m_BlendBlockCount;
    pNewEffect->m_pBlendBlocks = m_pBlendBlocks;
    pNewEffect->m_RasterizerBlockCount = m_RasterizerBlockCount;
    pNewEffect->m_pRasterizerBlocks = m_pRasterizerBlocks;
    pNewEffect->m_SamplerBlockCount = m_SamplerBlockCount;
    pNewEffect->m_pSamplerBlocks = m_pSamplerBlocks;
    pNewEffect->m_MemberDataCount = m_MemberDataCount;
    pNewEffect->m_pMemberDataBlocks = m_pMemberDataBlocks;
    pNewEffect->m_InterfaceCount = m_InterfaceCount;
    pNewEffect->m_pInterfaces = m_pInterfaces;
    pNewEffect->m_CBCount = m_CBCount;
    pNewEffect->m_pCBs = m_pCBs;
    pNewEffect->m_StringCount = m_StringCount;
    pNewEffect->m_pStrings = m_pStrings;
    pNewEffect->m_ShaderResourceCount = m_ShaderResourceCount;
    pNewEffect->m_pShaderResources = m_pShaderResources;
    pNewEffect->m_UnorderedAccessViewCount = m_UnorderedAccessViewCount;
    pNewEffect->m_pUnorderedAccessViews = m_pUnorderedAccessViews;
    pNewEffect->m_RenderTargetViewCount = m_RenderTargetViewCount;
    pNewEffect->m_pRenderTargetViews = m_pRenderTargetViews;
    pNewEffect->m_DepthStencilViewCount = m_DepthStencilViewCount;
    pNewEffect->m_pDepthStencilViews = m_pDepthStencilViews; 
    pNewEffect->m_LocalTimer = m_LocalTimer;
    pNewEffect->m_FXLIndex = m_FXLIndex;
    pNewEffect->m_pDevice = m_pDevice;
    pNewEffect->m_pClassLinkage = m_pClassLinkage;

    pNewEffect->AddRefAllForCloning( this );


    // m_pMemberInterfaces is a vector of cbuffer members that were created when the user called GetMemberBy* or GetElement
    // or during Effect loading when an interface is initialized to a global class variable elment.
    VH( pNewEffect->CopyMemberInterfaces( this ) );

    loader.m_pvOldMemberInterfaces = &m_pMemberInterfaces;
    loader.m_pEffect = pNewEffect;
    loader.m_EffectMemory = loader.m_ReflectionMemory = 0;


    // Move data from current effect to new effect
    if( !IsOptimized() )
    {
        VN( pNewEffect->m_pReflection = NEW CEffectReflection() );
        loader.m_pReflection = pNewEffect->m_pReflection;

        // make sure strings are moved before ReallocateEffectData
        VH( loader.InitializeReflectionDataAndMoveStrings( m_pReflection->m_Heap.GetSize() ) );
    }
    VH( loader.ReallocateEffectData( true ) );
    if( !IsOptimized() )
    {
        VH( loader.ReallocateReflectionData( true ) );
    }


    // Data structures for remapping type pointers and string pointers
    VN( pTempHeap = NEW CDataBlockStore );
    pTempHeap->EnableAlignment();
    mappingTableTypes.SetPrivateHeap(pTempHeap);
    mappingTableStrings.SetPrivateHeap(pTempHeap);
    VH( mappingTableTypes.AutoGrow() );
    VH( mappingTableStrings.AutoGrow() );

    if( !IsOptimized() )
    {
        // Let's re-create the type pool and string pool
        VN( pNewEffect->m_pPooledHeap = NEW CDataBlockStore );
        pNewEffect->m_pPooledHeap->EnableAlignment();

        VH( pNewEffect->CopyStringPool( this, mappingTableStrings ) );
        VH( pNewEffect->CopyTypePool( this, mappingTableTypes, mappingTableStrings ) );
    }
    else
    {
        // There's no string pool after optimizing.  Let's re-create the type pool
        VH( pNewEffect->CopyOptimizedTypePool( this, mappingTableTypes ) );
    }

    // fixup this effect's variable's types
    VH( pNewEffect->OptimizeTypes(&mappingTableTypes, true) );
    VH( pNewEffect->RecreateCBs() );


    for (UINT i = 0; i < pNewEffect->m_pMemberInterfaces.GetSize(); ++ i)
    {
        SMember* pMember = pNewEffect->m_pMemberInterfaces[i];
        VH( pNewEffect->FixupMemberInterface( pMember, this, mappingTableStrings ) );
    }


lExit:
    SAFE_DELETE( pTempHeap );
    if( FAILED( hr ) )
    {
        SAFE_DELETE( pNewEffect );
    }
    *ppClonedEffect = pNewEffect;
    return hr;
}

// Move all type pointers using pMappingTable.
// This is called after creating the optimized type pool or during cloning.
HRESULT CEffect::OptimizeTypes(CPointerMappingTable *pMappingTable, bool Cloning)
{
    HRESULT hr = S_OK;
    UINT  i;

    // find all child types, point them to the new location
    for (i = 0; i < m_VariableCount; ++ i)
    {
        VH( RemapType((SType**)&m_pVariables[i].pType, pMappingTable) );
    }

    UINT Members = m_pMemberInterfaces.GetSize();
    for( i=0; i < Members; i++ )
    {
        if( m_pMemberInterfaces[i] != NULL )
        {
            VH( RemapType((SType**)&m_pMemberInterfaces[i]->pType, pMappingTable) );
        }
    }

    // when cloning, there may be annotations
    if( Cloning )
    {
        for (UINT iVar = 0; iVar < m_VariableCount; ++ iVar)
        {
            for(i = 0; i < m_pVariables[iVar].AnnotationCount; ++ i )
            {
                VH( RemapType((SType**)&m_pVariables[iVar].pAnnotations[i].pType, pMappingTable) );
            }
        }
        for (UINT iCB = 0; iCB < m_CBCount; ++ iCB)
        {
            for(i = 0; i < m_pCBs[iCB].AnnotationCount; ++ i )
            {
                VH( RemapType((SType**)&m_pCBs[iCB].pAnnotations[i].pType, pMappingTable) );
            }
        }
        for (UINT iGroup = 0; iGroup < m_GroupCount; ++ iGroup)
        {
            for(i = 0; i < m_pGroups[iGroup].AnnotationCount; ++ i )
            {
                VH( RemapType((SType**)&m_pGroups[iGroup].pAnnotations[i].pType, pMappingTable) );
            }
            for(UINT iTech = 0; iTech < m_pGroups[iGroup].TechniqueCount; ++ iTech )
            {
                for(i = 0; i < m_pGroups[iGroup].pTechniques[iTech].AnnotationCount; ++ i )
                {
                    VH( RemapType((SType**)&m_pGroups[iGroup].pTechniques[iTech].pAnnotations[i].pType, pMappingTable) );
                }
                for(UINT iPass = 0; iPass < m_pGroups[iGroup].pTechniques[iTech].PassCount; ++ iPass )
                {
                    for(i = 0; i < m_pGroups[iGroup].pTechniques[iTech].pPasses[iPass].AnnotationCount; ++ i )
                    {
                        VH( RemapType((SType**)&m_pGroups[iGroup].pTechniques[iTech].pPasses[iPass].pAnnotations[i].pType, pMappingTable) );
                    }
                }
            }
        }
    }
lExit:
    return hr;
}

//////////////////////////////////////////////////////////////////////////
// Public API to shed this effect of its reflection data
HRESULT CEffect::Optimize()
{
    HRESULT hr = S_OK;
    UINT  i, j, k;
    CEffectHeap *pOptimizedTypeHeap = NULL;
    
    if (IsOptimized())
    {
        DPF(0, "ID3DX11Effect::Optimize: Effect has already been Optimize()'ed");
        return S_OK;
    }

    // Delete annotations, names, semantics, and string data on variables
    
    for (i = 0; i < m_VariableCount; ++ i)
    {
        m_pVariables[i].AnnotationCount = 0;
        m_pVariables[i].pAnnotations = NULL;
        m_pVariables[i].pName = NULL;
        m_pVariables[i].pSemantic = NULL;

        // 2) Point string variables to NULL
        if (m_pVariables[i].pType->IsObjectType(EOT_String))
        {
            D3DXASSERT(NULL != m_pVariables[i].Data.pString);
            m_pVariables[i].Data.pString = NULL;
        }
    }

    // Delete annotations and names on CBs

    for (i = 0; i < m_CBCount; ++ i)
    {
        m_pCBs[i].AnnotationCount = 0;
        m_pCBs[i].pAnnotations = NULL;
        m_pCBs[i].pName = NULL;
        m_pCBs[i].IsEffectOptimized = TRUE;
    }

    // Delete annotations and names on techniques and passes

    for (i = 0; i < m_GroupCount; ++ i)
    {
        m_pGroups[i].AnnotationCount = 0;
        m_pGroups[i].pAnnotations = NULL;
        m_pGroups[i].pName = NULL;

        for (j = 0; j < m_pGroups[i].TechniqueCount; ++ j)
        {
            m_pGroups[i].pTechniques[j].AnnotationCount = 0;
            m_pGroups[i].pTechniques[j].pAnnotations = NULL;
            m_pGroups[i].pTechniques[j].pName = NULL;

            for (k = 0; k < m_pGroups[i].pTechniques[j].PassCount; ++ k)
            {
                m_pGroups[i].pTechniques[j].pPasses[k].AnnotationCount = 0;
                m_pGroups[i].pTechniques[j].pPasses[k].pAnnotations = NULL;
                m_pGroups[i].pTechniques[j].pPasses[k].pName = NULL;
            }
        }
    };

    // 2) Remove shader bytecode & stream out decls
    //    (all are contained within pReflectionData)

    for (i = 0; i < m_ShaderBlockCount; ++ i)
    {
        if( m_pShaderBlocks[i].pReflectionData )
        {
            // pReflection was not created with PRIVATENEW
            SAFE_RELEASE( m_pShaderBlocks[i].pReflectionData->pReflection );

            m_pShaderBlocks[i].pReflectionData = NULL;
        }
    }

    UINT Members = m_pMemberInterfaces.GetSize();
    for( i=0; i < Members; i++ )
    {
        D3DXASSERT( m_pMemberInterfaces[i] != NULL );
        if( IsReflectionData(m_pMemberInterfaces[i]->pTopLevelEntity) )
        {
            D3DXASSERT( IsReflectionData(m_pMemberInterfaces[i]->Data.pGeneric) );

            // This is checked when cloning (so we don't clone Optimized-out member variables)
            m_pMemberInterfaces[i] = NULL;
        }
        else
        {
            m_pMemberInterfaces[i]->pName = NULL;
            m_pMemberInterfaces[i]->pSemantic = NULL;
        }
    }



    // get rid of the name/type hash tables and string data, 
    // then reallocate the type data and fix up this effect
    CPointerMappingTable mappingTable;
    CTypeHashTable::CIterator typeIter;
    CPointerMappingTable::CIterator mapIter;
    CCheckedDword chkSpaceNeeded = 0;
    UINT  spaceNeeded;

    // first pass: compute needed space
    for (m_pTypePool->GetFirstEntry(&typeIter); !m_pTypePool->PastEnd(&typeIter); m_pTypePool->GetNextEntry(&typeIter))
    {
        SType *pType = typeIter.GetData();
        
        chkSpaceNeeded += AlignToPowerOf2(sizeof(SType), c_DataAlignment);

        // if this is a struct, allocate room for its members
        if (EVT_Struct == pType->VarType)
        {
            chkSpaceNeeded += AlignToPowerOf2(pType->StructType.Members * sizeof(SVariable), c_DataAlignment);
        }
    }

    VH( chkSpaceNeeded.GetValue(&spaceNeeded) );

    D3DXASSERT(NULL == m_pOptimizedTypeHeap);
    VN( pOptimizedTypeHeap = NEW CEffectHeap );
    VH( pOptimizedTypeHeap->ReserveMemory(spaceNeeded));

    // use the private heap that we're about to destroy as scratch space for the mapping table
    mappingTable.SetPrivateHeap(m_pPooledHeap);
    VH( mappingTable.AutoGrow() );

    // second pass: move types over, build mapping table
    for (m_pTypePool->GetFirstEntry(&typeIter); !m_pTypePool->PastEnd(&typeIter); m_pTypePool->GetNextEntry(&typeIter))
    {
        SPointerMapping ptrMapping;
        SType *pType;

        ptrMapping.pOld = ptrMapping.pNew = typeIter.GetData();
        VH( pOptimizedTypeHeap->MoveData(&ptrMapping.pNew, sizeof(SType)) );

        pType = (SType *) ptrMapping.pNew;

        // if this is a struct, move its members to the newly allocated space
        if (EVT_Struct == pType->VarType)
        {
            VH( pOptimizedTypeHeap->MoveData((void **)&pType->StructType.pMembers, pType->StructType.Members * sizeof(SVariable)) );
        }

        VH( mappingTable.AddValueWithHash(ptrMapping, ptrMapping.Hash()) );
    }
    
    // third pass: fixup structure member & name pointers
    for (mappingTable.GetFirstEntry(&mapIter); !mappingTable.PastEnd(&mapIter); mappingTable.GetNextEntry(&mapIter))
    {
        SPointerMapping ptrMapping = mapIter.GetData();
        SType *pType = (SType *) ptrMapping.pNew;

        pType->pTypeName = NULL;

        // if this is a struct, fix up its members' pointers
        if (EVT_Struct == pType->VarType)
        {
            for (i = 0; i < pType->StructType.Members; ++ i)
            {
                VH( RemapType((SType**)&pType->StructType.pMembers[i].pType, &mappingTable) );
                pType->StructType.pMembers[i].pName = NULL;
                pType->StructType.pMembers[i].pSemantic = NULL;
            }
        }
    }        

    // fixup this effect's variable's types
    VH( OptimizeTypes(&mappingTable) );

    m_pOptimizedTypeHeap = pOptimizedTypeHeap;
    pOptimizedTypeHeap = NULL;

#ifdef D3DX11_FX_PRINT_HASH_STATS
    DPF(0, "Compiler string pool hash table statistics:");
    m_pTypePool->PrintHashTableStats();
    DPF(0, "Compiler type pool hash table statistics:");
    m_pStringPool->PrintHashTableStats();
#endif // D3DX11_FX_PRINT_HASH_STATS

    SAFE_DELETE(m_pTypePool);
    SAFE_DELETE(m_pStringPool);
    SAFE_DELETE(m_pPooledHeap);

    DPF(0, "ID3DX11Effect::Optimize: %d bytes of reflection data freed.", m_pReflection->m_Heap.GetSize());
    SAFE_DELETE(m_pReflection);
    m_Flags |= D3DX11_EFFECT_OPTIMIZED;

lExit:
    SAFE_DELETE(pOptimizedTypeHeap);
    return hr;
}

SMember * CreateNewMember(SType *pType, BOOL IsAnnotation)
{
    switch (pType->VarType)
    {
    case EVT_Struct:
        if (IsAnnotation)
        {
            D3DXASSERT(sizeof(SNumericAnnotationMember) == sizeof(SMember));
            return (SMember*) NEW SNumericAnnotationMember;
        }
        else if (pType->StructType.ImplementsInterface)
        {
            D3DXASSERT(sizeof(SClassInstanceGlobalVariableMember) == sizeof(SMember));
            return (SMember*) NEW SClassInstanceGlobalVariableMember;
        }
        else
        {
            D3DXASSERT(sizeof(SNumericGlobalVariableMember) == sizeof(SMember));
            return (SMember*) NEW SNumericGlobalVariableMember;
        }
        break;
    case EVT_Interface:
        D3DXASSERT(sizeof(SInterfaceGlobalVariableMember) == sizeof(SMember));
        return (SMember*) NEW SInterfaceGlobalVariableMember;
        break;
    case EVT_Object:
        switch (pType->ObjectType)
        {
        case EOT_String:
            if (IsAnnotation)
            {
                D3DXASSERT(sizeof(SStringAnnotationMember) == sizeof(SMember));
                return (SMember*) NEW SStringAnnotationMember;
            }
            else
            {
                D3DXASSERT(sizeof(SStringGlobalVariableMember) == sizeof(SMember));
                return (SMember*) NEW SStringGlobalVariableMember;
            }

            break;
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
        case EOT_Buffer:
        case EOT_ByteAddressBuffer:
        case EOT_StructuredBuffer:
            D3DXASSERT(!IsAnnotation);
            D3DXASSERT(sizeof(SShaderResourceGlobalVariableMember) == sizeof(SMember));
            return (SMember*) NEW SShaderResourceGlobalVariableMember;
            break;
        case EOT_RWTexture1D:
        case EOT_RWTexture1DArray:
        case EOT_RWTexture2D:
        case EOT_RWTexture2DArray:
        case EOT_RWTexture3D:
        case EOT_RWBuffer:
        case EOT_RWByteAddressBuffer:
        case EOT_RWStructuredBuffer:
        case EOT_RWStructuredBufferAlloc:
        case EOT_RWStructuredBufferConsume:
        case EOT_AppendStructuredBuffer:
        case EOT_ConsumeStructuredBuffer:
            D3DXASSERT(!IsAnnotation);
            D3DXASSERT(sizeof(SUnorderedAccessViewGlobalVariableMember) == sizeof(SMember));
            return (SMember*) NEW SUnorderedAccessViewGlobalVariableMember;
            break;
        case EOT_VertexShader:
        case EOT_VertexShader5:
        case EOT_GeometryShader:
        case EOT_GeometryShaderSO:
        case EOT_GeometryShader5:
        case EOT_PixelShader:
        case EOT_PixelShader5:
        case EOT_HullShader5:
        case EOT_DomainShader5:
        case EOT_ComputeShader5:
            D3DXASSERT(!IsAnnotation);
            D3DXASSERT(sizeof(SShaderGlobalVariableMember) == sizeof(SMember));
            return (SMember*) NEW SShaderGlobalVariableMember;
            break;
        case EOT_Blend:
            D3DXASSERT(!IsAnnotation);
            D3DXASSERT(sizeof(SBlendGlobalVariableMember) == sizeof(SMember));
            return (SMember*) NEW SBlendGlobalVariableMember;
            break;
        case EOT_Rasterizer:
            D3DXASSERT(!IsAnnotation);
            D3DXASSERT(sizeof(SRasterizerGlobalVariableMember) == sizeof(SMember));
            return (SMember*) NEW SRasterizerGlobalVariableMember;
            break;
        case EOT_DepthStencil:
            D3DXASSERT(!IsAnnotation);
            D3DXASSERT(sizeof(SDepthStencilGlobalVariableMember) == sizeof(SMember));
            return (SMember*) NEW SDepthStencilGlobalVariableMember;
            break;
        case EOT_Sampler:
            D3DXASSERT(!IsAnnotation);
            D3DXASSERT(sizeof(SSamplerGlobalVariableMember) == sizeof(SMember));
            return (SMember*) NEW SSamplerGlobalVariableMember;
            break;
        case EOT_DepthStencilView:
            D3DXASSERT(!IsAnnotation);
            D3DXASSERT(sizeof(SDepthStencilViewGlobalVariableMember) == sizeof(SMember));
            return (SMember*) NEW SDepthStencilViewGlobalVariableMember;
            break;
        case EOT_RenderTargetView:
            D3DXASSERT(!IsAnnotation);
            D3DXASSERT(sizeof(SRenderTargetViewGlobalVariableMember) == sizeof(SMember));
            return (SMember*) NEW SRenderTargetViewGlobalVariableMember;
            break;
        default:
            D3DXASSERT(0);
            DPF( 0, "Internal error: invalid object type." );
            return NULL;
            break;
        }
        break;
    case EVT_Numeric:
        switch (pType->NumericType.NumericLayout)
        {
        case ENL_Matrix:
            if (IsAnnotation)
            {
                D3DXASSERT(sizeof(SMatrixAnnotationMember) == sizeof(SMember));
                return (SMember*) NEW SMatrixAnnotationMember;
            }
            else
            {
                D3DXASSERT(sizeof(SMatrixGlobalVariableMember) == sizeof(SMember));
                D3DXASSERT(sizeof(SMatrix4x4ColumnMajorGlobalVariableMember) == sizeof(SMember));
                D3DXASSERT(sizeof(SMatrix4x4RowMajorGlobalVariableMember) == sizeof(SMember));

                if (pType->NumericType.Rows == 4 && pType->NumericType.Columns == 4)
                {
                    if (pType->NumericType.IsColumnMajor)
                    {
                        return (SMember*) NEW SMatrix4x4ColumnMajorGlobalVariableMember;
                    }
                    else
                    {
                        return (SMember*) NEW SMatrix4x4RowMajorGlobalVariableMember;
                    }
                }
                else
                {
                    return (SMember*) NEW SMatrixGlobalVariableMember;
                }
            }
            break;
        case ENL_Vector:
            switch (pType->NumericType.ScalarType)
            {
            case EST_Float:
                if (IsAnnotation)
                {
                    D3DXASSERT(sizeof(SFloatVectorAnnotationMember) == sizeof(SMember));
                    return (SMember*) NEW SFloatVectorAnnotationMember;
                }
                else
                {
                    D3DXASSERT(sizeof(SFloatVectorGlobalVariableMember) == sizeof(SMember));
                    D3DXASSERT(sizeof(SFloatVector4GlobalVariableMember) == sizeof(SMember));

                    if (pType->NumericType.Columns == 4)
                    {
                        return (SMember*) NEW SFloatVector4GlobalVariableMember;
                    }
                    else
                    {
                        return (SMember*) NEW SFloatVectorGlobalVariableMember;
                    }
                }
                break;
            case EST_Bool:
                if (IsAnnotation)
                {
                    D3DXASSERT(sizeof(SBoolVectorAnnotationMember) == sizeof(SMember));
                    return (SMember*) NEW SBoolVectorAnnotationMember;
                }
                else
                {
                    D3DXASSERT(sizeof(SBoolVectorGlobalVariableMember) == sizeof(SMember));
                    return (SMember*) NEW SBoolVectorGlobalVariableMember;
                }
                break;
            case EST_UInt:
            case EST_Int:
                if (IsAnnotation)
                {
                    D3DXASSERT(sizeof(SIntVectorAnnotationMember) == sizeof(SMember));
                    return (SMember*) NEW SIntVectorAnnotationMember;
                }
                else
                {
                    D3DXASSERT(sizeof(SIntVectorGlobalVariableMember) == sizeof(SMember));
                    return (SMember*) NEW SIntVectorGlobalVariableMember;
                }
                break;
            default:
                D3DXASSERT(0);
                DPF( 0, "Internal loading error: invalid vector type." );
                break;
            }
            break;
        case ENL_Scalar:
            switch (pType->NumericType.ScalarType)
            {
            case EST_Float:
                if (IsAnnotation)
                {
                    D3DXASSERT(sizeof(SFloatScalarAnnotationMember) == sizeof(SMember));
                    return (SMember*) NEW SFloatScalarAnnotationMember;
                }
                else
                {
                    D3DXASSERT(sizeof(SFloatScalarGlobalVariableMember) == sizeof(SMember));
                    return (SMember*) NEW SFloatScalarGlobalVariableMember;
                }
                break;
            case EST_UInt:
            case EST_Int:
                if (IsAnnotation)
                {
                    D3DXASSERT(sizeof(SIntScalarAnnotationMember) == sizeof(SMember));
                    return (SMember*) NEW SIntScalarAnnotationMember;
                }
                else
                {
                    D3DXASSERT(sizeof(SIntScalarGlobalVariableMember) == sizeof(SMember));
                    return (SMember*) NEW SIntScalarGlobalVariableMember;
                }
                break;
            case EST_Bool:
                if (IsAnnotation)
                {
                    D3DXASSERT(sizeof(SBoolScalarAnnotationMember) == sizeof(SMember));
                    return (SMember*) NEW SBoolScalarAnnotationMember;
                }
                else
                {
                    D3DXASSERT(sizeof(SBoolScalarGlobalVariableMember) == sizeof(SMember));
                    return (SMember*) NEW SBoolScalarGlobalVariableMember;
                }
                break;
            default:
                DPF( 0, "Internal loading error: invalid scalar type." );
                D3DXASSERT(0);
                break;
            }            
            break;
        default:
            D3DXASSERT(0);
            DPF( 0, "Internal loading error: invalid numeric type." );
            break;
        }
        break;
    default:
        D3DXASSERT(0);
        DPF( 0, "Internal loading error: invalid variable type." );
        break;
    }
    return NULL;
}

// Global variables are created in place because storage for them was allocated during LoadEffect
HRESULT PlacementNewVariable(void *pVar, SType *pType, BOOL IsAnnotation)
{
    switch (pType->VarType)
    {
    case EVT_Struct:
        if (IsAnnotation)
        {
            D3DXASSERT(sizeof(SNumericAnnotation) == sizeof(SAnnotation));
            new(pVar) SNumericAnnotation();
        }
        else if (pType->StructType.ImplementsInterface)
        {
            D3DXASSERT(sizeof(SClassInstanceGlobalVariable) == sizeof(SGlobalVariable));
            new(pVar) SClassInstanceGlobalVariable;
        }
        else 
        {
            D3DXASSERT(sizeof(SNumericGlobalVariable) == sizeof(SGlobalVariable));
            new(pVar) SNumericGlobalVariable;
        }
        break;
    case EVT_Interface:
        D3DXASSERT(sizeof(SInterfaceGlobalVariable) == sizeof(SGlobalVariable));
        new(pVar) SInterfaceGlobalVariable;
        break;
    case EVT_Object:
        switch (pType->ObjectType)
        {
        case EOT_String:
            if (IsAnnotation)
            {
                D3DXASSERT(sizeof(SStringAnnotation) == sizeof(SAnnotation));
                new(pVar) SStringAnnotation;
            }
            else
            {
                D3DXASSERT(sizeof(SStringGlobalVariable) == sizeof(SGlobalVariable));
                new(pVar) SStringGlobalVariable;
            }
            
            break;
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
        case EOT_Buffer:
        case EOT_ByteAddressBuffer:
        case EOT_StructuredBuffer:
            D3DXASSERT(!IsAnnotation);
            D3DXASSERT(sizeof(SShaderResourceGlobalVariable) == sizeof(SGlobalVariable));
            new(pVar) SShaderResourceGlobalVariable;
            break;
        case EOT_RWTexture1D:
        case EOT_RWTexture1DArray:
        case EOT_RWTexture2D:
        case EOT_RWTexture2DArray:
        case EOT_RWTexture3D:
        case EOT_RWBuffer:
        case EOT_RWByteAddressBuffer:
        case EOT_RWStructuredBuffer:
        case EOT_RWStructuredBufferAlloc:
        case EOT_RWStructuredBufferConsume:
        case EOT_AppendStructuredBuffer:
        case EOT_ConsumeStructuredBuffer:
            D3DXASSERT(!IsAnnotation);
            D3DXASSERT(sizeof(SUnorderedAccessViewGlobalVariable) == sizeof(SGlobalVariable));
            new(pVar) SUnorderedAccessViewGlobalVariable;
            break;
        case EOT_VertexShader:
        case EOT_VertexShader5:
        case EOT_GeometryShader:
        case EOT_GeometryShaderSO:
        case EOT_GeometryShader5:
        case EOT_PixelShader:
        case EOT_PixelShader5:
        case EOT_HullShader5:
        case EOT_DomainShader5:
        case EOT_ComputeShader5:
            D3DXASSERT(!IsAnnotation);
            D3DXASSERT(sizeof(SShaderGlobalVariable) == sizeof(SGlobalVariable));
            new(pVar) SShaderGlobalVariable;
            break;
        case EOT_Blend:
            D3DXASSERT(!IsAnnotation);
            D3DXASSERT(sizeof(SBlendGlobalVariable) == sizeof(SGlobalVariable));
            new(pVar) SBlendGlobalVariable;
            break;
        case EOT_Rasterizer:
            D3DXASSERT(!IsAnnotation);
            D3DXASSERT(sizeof(SRasterizerGlobalVariable) == sizeof(SGlobalVariable));
            new(pVar) SRasterizerGlobalVariable;
            break;
        case EOT_DepthStencil:
            D3DXASSERT(!IsAnnotation);
            D3DXASSERT(sizeof(SDepthStencilGlobalVariable) == sizeof(SGlobalVariable));
            new(pVar) SDepthStencilGlobalVariable;
            break;
        case EOT_Sampler:
            D3DXASSERT(!IsAnnotation);
            D3DXASSERT(sizeof(SSamplerGlobalVariable) == sizeof(SGlobalVariable));
            new(pVar) SSamplerGlobalVariable;
            break;
        case EOT_RenderTargetView:
            D3DXASSERT(!IsAnnotation);
            D3DXASSERT(sizeof(SRenderTargetViewGlobalVariable) == sizeof(SGlobalVariable));
            new(pVar) SRenderTargetViewGlobalVariable;
            break;
        case EOT_DepthStencilView:
            D3DXASSERT(!IsAnnotation);
            D3DXASSERT(sizeof(SDepthStencilViewGlobalVariable) == sizeof(SGlobalVariable));
            new(pVar) SDepthStencilViewGlobalVariable;
            break;
        default:
            D3DXASSERT(0);
            DPF( 0, "Internal loading error: invalid object type." );
            return E_FAIL;
            break;
        }
        break;
    case EVT_Numeric:
        switch (pType->NumericType.NumericLayout)
        {
        case ENL_Matrix:
            if (IsAnnotation)
            {
                D3DXASSERT(sizeof(SMatrixAnnotation) == sizeof(SAnnotation));
                new(pVar) SMatrixAnnotation;
            }
            else
            {
                D3DXASSERT(sizeof(SMatrixGlobalVariable) == sizeof(SGlobalVariable));
                D3DXASSERT(sizeof(SMatrix4x4ColumnMajorGlobalVariable) == sizeof(SGlobalVariable));
                D3DXASSERT(sizeof(SMatrix4x4RowMajorGlobalVariable) == sizeof(SGlobalVariable));
                
                if (pType->NumericType.Rows == 4 && pType->NumericType.Columns == 4)
                {
                    if (pType->NumericType.IsColumnMajor)
                    {
                        new(pVar) SMatrix4x4ColumnMajorGlobalVariable;
                    }
                    else
                    {
                        new(pVar) SMatrix4x4RowMajorGlobalVariable;
                    }
                }
                else
                {
                    new(pVar) SMatrixGlobalVariable;
                }
            }
            break;
        case ENL_Vector:
            switch (pType->NumericType.ScalarType)
            {
            case EST_Float:
                if (IsAnnotation)
                {
                    D3DXASSERT(sizeof(SFloatVectorAnnotation) == sizeof(SAnnotation));
                    new(pVar) SFloatVectorAnnotation;
                }
                else
                {
                    D3DXASSERT(sizeof(SFloatVectorGlobalVariable) == sizeof(SGlobalVariable));
                    D3DXASSERT(sizeof(SFloatVector4GlobalVariable) == sizeof(SGlobalVariable));

                    if (pType->NumericType.Columns == 4)
                    {
                        new(pVar) SFloatVector4GlobalVariable;
                    }
                    else
                    {
                        new(pVar) SFloatVectorGlobalVariable;
                    }
                }
                break;
            case EST_Bool:
                if (IsAnnotation)
                {
                    D3DXASSERT(sizeof(SBoolVectorAnnotation) == sizeof(SAnnotation));
                    new(pVar) SBoolVectorAnnotation;
                }
                else
                {
                    D3DXASSERT(sizeof(SBoolVectorGlobalVariable) == sizeof(SGlobalVariable));
                    new(pVar) SBoolVectorGlobalVariable;
                }
                break;
            case EST_UInt:
            case EST_Int:
                if (IsAnnotation)
                {
                    D3DXASSERT(sizeof(SIntVectorAnnotation) == sizeof(SAnnotation));
                    new(pVar) SIntVectorAnnotation;
                }
                else
                {
                    D3DXASSERT(sizeof(SIntVectorGlobalVariable) == sizeof(SGlobalVariable));
                    new(pVar) SIntVectorGlobalVariable;
                }
                break;
            }
            break;
        case ENL_Scalar:
            switch (pType->NumericType.ScalarType)
            {
            case EST_Float:
                if (IsAnnotation)
                {
                    D3DXASSERT(sizeof(SFloatScalarAnnotation) == sizeof(SAnnotation));
                    new(pVar) SFloatScalarAnnotation;
                }
                else
                {
                    D3DXASSERT(sizeof(SFloatScalarGlobalVariable) == sizeof(SGlobalVariable));
                    new(pVar) SFloatScalarGlobalVariable;
                }
                break;
            case EST_UInt:
            case EST_Int:
                if (IsAnnotation)
                {
                    D3DXASSERT(sizeof(SIntScalarAnnotation) == sizeof(SAnnotation));
                    new(pVar) SIntScalarAnnotation;
                }
                else
                {
                    D3DXASSERT(sizeof(SIntScalarGlobalVariable) == sizeof(SGlobalVariable));
                    new(pVar) SIntScalarGlobalVariable;
                }
                break;
            case EST_Bool:
                if (IsAnnotation)
                {
                    D3DXASSERT(sizeof(SBoolScalarAnnotation) == sizeof(SAnnotation));
                    new(pVar) SBoolScalarAnnotation;
                }
                else
                {
                    D3DXASSERT(sizeof(SBoolScalarGlobalVariable) == sizeof(SGlobalVariable));
                    new(pVar) SBoolScalarGlobalVariable;
                }
                break;
            default:
                D3DXASSERT(0);
                DPF( 0, "Internal loading error: invalid scalar type." );
                return E_FAIL;
                break;
            }            
            break;
        default:
            D3DXASSERT(0);
            DPF( 0, "Internal loading error: invalid numeric type." );
            return E_FAIL;
            break;
        }
        break;
    default:
        D3DXASSERT(0);
        DPF( 0, "Internal loading error: invalid variable type." );
        return E_FAIL;
        break;
    }
    return S_OK;
}

}
