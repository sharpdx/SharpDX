//////////////////////////////////////////////////////////////////////////////
//
//  Copyright (C) Microsoft Corporation.  All Rights Reserved.
//
//  File:       EffectReflection.cpp
//  Content:    D3DX11 Effects public reflection APIs
//
//////////////////////////////////////////////////////////////////////////////

#include "pchfx.h"

namespace D3DX11Effects
{

SEffectInvalidType g_InvalidType;

SEffectInvalidScalarVariable g_InvalidScalarVariable;
SEffectInvalidVectorVariable g_InvalidVectorVariable;
SEffectInvalidMatrixVariable g_InvalidMatrixVariable;
SEffectInvalidStringVariable g_InvalidStringVariable;
SEffectInvalidClassInstanceVariable g_InvalidClassInstanceVariable;
SEffectInvalidInterfaceVariable g_InvalidInterfaceVariable;
SEffectInvalidShaderResourceVariable g_InvalidShaderResourceVariable;
SEffectInvalidUnorderedAccessViewVariable g_InvalidUnorderedAccessViewVariable;
SEffectInvalidRenderTargetViewVariable g_InvalidRenderTargetViewVariable;
SEffectInvalidDepthStencilViewVariable g_InvalidDepthStencilViewVariable;
SEffectInvalidConstantBuffer g_InvalidConstantBuffer;
SEffectInvalidShaderVariable g_InvalidShaderVariable;
SEffectInvalidBlendVariable g_InvalidBlendVariable;
SEffectInvalidDepthStencilVariable g_InvalidDepthStencilVariable;
SEffectInvalidRasterizerVariable g_InvalidRasterizerVariable;
SEffectInvalidSamplerVariable g_InvalidSamplerVariable;

SEffectInvalidPass g_InvalidPass;
SEffectInvalidTechnique g_InvalidTechnique;
SEffectInvalidGroup g_InvalidGroup;


//////////////////////////////////////////////////////////////////////////
// Helper routine implementations
//////////////////////////////////////////////////////////////////////////

ID3DX11EffectConstantBuffer * NoParentCB()
{
    DPF(0, "ID3DX11EffectVariable::GetParentConstantBuffer: Variable does not have a parent constant buffer");
    // have to typecast because the type of g_InvalidScalarVariable has not been declared yet
    return &g_InvalidConstantBuffer;
}

ID3DX11EffectVariable * GetAnnotationByIndexHelper(const char *pClassName, UINT  Index, UINT  AnnotationCount, SAnnotation *pAnnotations)
{
    if (Index >= AnnotationCount)
    {
        DPF(0, "%s::GetAnnotationByIndex: Invalid index (%d, total: %d)", pClassName, Index, AnnotationCount);
        return &g_InvalidScalarVariable;
    }

    return pAnnotations + Index;
}

ID3DX11EffectVariable * GetAnnotationByNameHelper(const char *pClassName, LPCSTR Name, UINT  AnnotationCount, SAnnotation *pAnnotations)
{
    UINT  i;
    for (i = 0; i < AnnotationCount; ++ i)
    {
        if (strcmp(pAnnotations[i].pName, Name) == 0)
        {
            return pAnnotations + i;
        }
    }

    DPF(0, "%s::GetAnnotationByName: Annotation [%s] not found", pClassName, Name);
    return &g_InvalidScalarVariable;
}

//////////////////////////////////////////////////////////////////////////
// Effect routines to pool interfaces
//////////////////////////////////////////////////////////////////////////

ID3DX11EffectType * CEffect::CreatePooledSingleElementTypeInterface(SType *pType)
{
    UINT  i;

    if (IsOptimized())
    {
        DPF(0, "ID3DX11Effect: Cannot create new type interfaces since the effect has been Optimize()'ed");
        return &g_InvalidType;
    }

    for (i = 0; i < m_pTypeInterfaces.GetSize(); ++ i)
    {
        if (m_pTypeInterfaces[i]->pType == pType)
        {
            return (SSingleElementType*)m_pTypeInterfaces[i];
        }
    }
    SSingleElementType *pNewType;
    if (NULL == (pNewType = NEW SSingleElementType))
    {
        DPF(0, "ID3DX11Effect: Out of memory while trying to create new type interface");
        return &g_InvalidType;
    }

    pNewType->pType = pType;
    m_pTypeInterfaces.Add(pNewType);

    return pNewType;
}

// Create a member variable (via GetMemberBy* or GetElement)
ID3DX11EffectVariable * CEffect::CreatePooledVariableMemberInterface(TTopLevelVariable<ID3DX11EffectVariable> *pTopLevelEntity, SVariable *pMember, UDataPointer Data, BOOL IsSingleElement, UINT Index)
{
    BOOL IsAnnotation;
    UINT  i;

    if (IsOptimized())
    {
        DPF(0, "ID3DX11Effect: Cannot create new variable interfaces since the effect has been Optimize()'ed");
        return &g_InvalidScalarVariable;
    }

    for (i = 0; i < m_pMemberInterfaces.GetSize(); ++ i)
    {
        if (m_pMemberInterfaces[i]->pType == pMember->pType && 
            m_pMemberInterfaces[i]->pName == pMember->pName &&
            m_pMemberInterfaces[i]->pSemantic == pMember->pSemantic &&
            m_pMemberInterfaces[i]->Data.pGeneric == Data.pGeneric &&
            m_pMemberInterfaces[i]->IsSingleElement == IsSingleElement &&
            ((SMember*)m_pMemberInterfaces[i])->pTopLevelEntity == pTopLevelEntity)
        {
            return (ID3DX11EffectVariable *) m_pMemberInterfaces[i];
        }
    }

    // is this annotation or runtime data?
    if( pTopLevelEntity->pEffect->IsReflectionData(pTopLevelEntity) )
    {
        D3DXASSERT( pTopLevelEntity->pEffect->IsReflectionData(Data.pGeneric) );
        IsAnnotation = TRUE;
    }
    else
    {
        // if the heap is empty, we are still loading the Effect, and thus creating a member for a variable initializer
        // ex. Interface myInt = myClassArray[2];
        if( pTopLevelEntity->pEffect->m_Heap.GetSize() > 0 )
        {
            D3DXASSERT( pTopLevelEntity->pEffect->IsRuntimeData(pTopLevelEntity) );
            if (!pTopLevelEntity->pType->IsObjectType(EOT_String))
            {
                // strings are funny; their data is reflection data, so ignore those
                D3DXASSERT( pTopLevelEntity->pEffect->IsRuntimeData(Data.pGeneric) );
            }
        }
        IsAnnotation = FALSE;
    }

    SMember *pNewMember;

    if (NULL == (pNewMember = CreateNewMember((SType*)pMember->pType, IsAnnotation)))
    {
        DPF(0, "ID3DX11Effect: Out of memory while trying to create new member variable interface");
        return &g_InvalidScalarVariable;
    }
    
    pNewMember->pType = pMember->pType;
    pNewMember->pName = pMember->pName;
    pNewMember->pSemantic = pMember->pSemantic;
    pNewMember->Data.pGeneric = Data.pGeneric;
    pNewMember->IsSingleElement = IsSingleElement;
    pNewMember->pTopLevelEntity = pTopLevelEntity;

    if( IsSingleElement && pMember->pMemberData )
    {
        D3DXASSERT( !IsAnnotation );
        // This is an element of a global variable array
        pNewMember->pMemberData = pMember->pMemberData + Index;
    }

    if (FAILED(m_pMemberInterfaces.Add(pNewMember)))
    {
        SAFE_DELETE(pNewMember);
        DPF(0, "ID3DX11Effect: Out of memory while trying to create new member variable interface");
        return &g_InvalidScalarVariable;
    }

    return (ID3DX11EffectVariable *) pNewMember;
}

//////////////////////////////////////////////////////////////////////////
// ID3DX11EffectType (SType, SSingleElementType implementations)
//////////////////////////////////////////////////////////////////////////

static ID3DX11EffectType * GetTypeByIndexHelper(UINT Index, UINT  VariableCount, 
                                               SVariable *pVariables, UINT  SizeOfVariableType)
{
    LPCSTR pFuncName = "ID3DX11EffectType::GetMemberTypeByIndex";

    if (Index >= VariableCount)
    {
        DPF(0, "%s: Invalid index (%d, total: %d)", pFuncName, Index, VariableCount);
        return &g_InvalidType;
    }

    SVariable *pVariable = (SVariable *)((BYTE *)pVariables + Index * SizeOfVariableType);
    if (NULL == pVariable->pName)
    {
        DPF(0, "%s: Cannot get member types; Effect has been Optimize()'ed", pFuncName);
        return &g_InvalidType;
    }

    return (ID3DX11EffectType *) pVariable->pType;
}

static ID3DX11EffectType * GetTypeByNameHelper(LPCSTR Name, UINT  VariableCount, 
                                              SVariable *pVariables, UINT  SizeOfVariableType)
{
    LPCSTR pFuncName = "ID3DX11EffectType::GetMemberTypeByName";

    if (NULL == Name)
    {
        DPF(0, "%s: Parameter Name was NULL.", pFuncName);
        return &g_InvalidType;
    }

    UINT  i;
    SVariable *pVariable;

    for (i = 0; i < VariableCount; ++ i)
    {
        pVariable = (SVariable *)((BYTE *)pVariables + i * SizeOfVariableType);
        if (NULL == pVariable->pName)
        {
            DPF(0, "%s: Cannot get member types; Effect has been Optimize()'ed", pFuncName);
            return &g_InvalidType;
        }
        if (strcmp(pVariable->pName, Name) == 0)
        {
            return (ID3DX11EffectType *) pVariable->pType;
        }
    }

    DPF(0, "%s: Member type [%s] not found", pFuncName, Name);
    return &g_InvalidType;
}


static ID3DX11EffectType * GetTypeBySemanticHelper(LPCSTR Semantic, UINT  VariableCount, 
                                                  SVariable *pVariables, UINT  SizeOfVariableType)
{
    LPCSTR pFuncName = "ID3DX11EffectType::GetMemberTypeBySemantic";

    if (NULL == Semantic)
    {
        DPF(0, "%s: Parameter Semantic was NULL.", pFuncName);
        return &g_InvalidType;
    }

    UINT  i;
    SVariable *pVariable;

    for (i = 0; i < VariableCount; ++ i)
    {
        pVariable = (SVariable *)((BYTE *)pVariables + i * SizeOfVariableType);
        if (NULL == pVariable->pName)
        {
            DPF(0, "%s: Cannot get member types; Effect has been Optimize()'ed", pFuncName);
            return &g_InvalidType;
        }
        if (NULL != pVariable->pSemantic &&
            _stricmp(pVariable->pSemantic, Semantic) == 0)
        {
            return (ID3DX11EffectType *) pVariable->pType;
        }
    }

    DPF(0, "%s: Member type with semantic [%s] not found", pFuncName, Semantic);
    return &g_InvalidType;
}

ID3DX11EffectType * SType::GetMemberTypeByIndex(UINT Index)
{
    if (VarType != EVT_Struct)
    {
        DPF(0, "ID3DX11EffectType::GetMemberTypeByIndex: This interface does not refer to a structure");
        return &g_InvalidType;
    }

    return GetTypeByIndexHelper(Index, StructType.Members, StructType.pMembers, sizeof(SVariable));
}

ID3DX11EffectType * SType::GetMemberTypeByName(LPCSTR Name)
{
    if (VarType != EVT_Struct)
    {
        DPF(0, "ID3DX11EffectType::GetMemberTypeByName: This interface does not refer to a structure");
        return &g_InvalidType;
    }

    return GetTypeByNameHelper(Name, StructType.Members, StructType.pMembers, sizeof(SVariable));
}

ID3DX11EffectType * SType::GetMemberTypeBySemantic(LPCSTR Semantic)
{
    if (VarType != EVT_Struct)
    {
        DPF(0, "ID3DX11EffectType::GetMemberTypeBySemantic: This interface does not refer to a structure");
        return &g_InvalidType;
    }

    return GetTypeBySemanticHelper(Semantic, StructType.Members, StructType.pMembers, sizeof(SVariable));
}

LPCSTR SType::GetMemberName(UINT Index)
{
    LPCSTR pFuncName = "ID3DX11EffectType::GetMemberName";

    if (VarType != EVT_Struct)
    {
        DPF(0, "%s: This interface does not refer to a structure", pFuncName);
        return NULL;
    }

    if (Index >= StructType.Members)
    {
        DPF(0, "%s: Invalid index (%d, total: %d)", pFuncName, Index, StructType.Members);
        return NULL;
    }

    SVariable *pVariable = StructType.pMembers + Index;

    if (NULL == pVariable->pName)
    {
        DPF(0, "%s: Cannot get member names; Effect has been Optimize()'ed", pFuncName);
        return NULL;
    }

    return pVariable->pName;
}

LPCSTR SType::GetMemberSemantic(UINT Index)
{
    LPCSTR pFuncName = "ID3DX11EffectType::GetMemberSemantic";

    if (VarType != EVT_Struct)
    {
        DPF(0, "%s: This interface does not refer to a structure", pFuncName);
        return NULL;
    }

    if (Index >= StructType.Members)
    {
        DPF(0, "%s: Invalid index (%d, total: %d)", pFuncName, Index, StructType.Members);
        return NULL;
    }

    SVariable *pVariable = StructType.pMembers + Index;

    if (NULL == pVariable->pName)
    {
        DPF(0, "%s: Cannot get member semantics; Effect has been Optimize()'ed", pFuncName);
        return NULL;
    }

    return pVariable->pSemantic;
}

HRESULT SType::GetDescHelper(D3DX11_EFFECT_TYPE_DESC *pDesc, BOOL IsSingleElement) const
{
    HRESULT hr = S_OK;
    LPCSTR pFuncName = "ID3DX11EffectType::GetDesc";

    VERIFYPARAMETER(pDesc);
    
    pDesc->TypeName = pTypeName;

    // intentionally return 0 so they know it's not a single element array
    pDesc->Elements = IsSingleElement ? 0 : Elements;
    pDesc->PackedSize = GetTotalPackedSize(IsSingleElement);
    pDesc->UnpackedSize = GetTotalUnpackedSize(IsSingleElement);
    pDesc->Stride = Stride;

    switch (VarType)
    {
    case EVT_Numeric:
        switch (NumericType.NumericLayout)
        {
        case ENL_Matrix:
            if (NumericType.IsColumnMajor)
            {
                pDesc->Class = D3D10_SVC_MATRIX_COLUMNS;
            }
            else
            {
                pDesc->Class = D3D10_SVC_MATRIX_ROWS;
            }
            break;
        case ENL_Vector:
            pDesc->Class = D3D10_SVC_VECTOR;
            break;
        case ENL_Scalar:
            pDesc->Class = D3D10_SVC_SCALAR;
            break;
        default:
            D3DXASSERT(0);
        }

        switch (NumericType.ScalarType)
        {
        case EST_Bool:
            pDesc->Type = D3D10_SVT_BOOL;
            break;
        case EST_Int:
            pDesc->Type = D3D10_SVT_INT;
            break;
        case EST_UInt:
            pDesc->Type = D3D10_SVT_UINT;
            break;
        case EST_Float:
            pDesc->Type = D3D10_SVT_FLOAT;
            break;
        default:
            D3DXASSERT(0);
        }

        pDesc->Rows = NumericType.Rows;
        pDesc->Columns = NumericType.Columns;
        pDesc->Members = 0;

        break;

    case EVT_Struct:
        pDesc->Rows = 0;
        pDesc->Columns = 0;
        pDesc->Members = StructType.Members;
        if( StructType.ImplementsInterface )
        {
            pDesc->Class = D3D11_SVC_INTERFACE_CLASS;
        }
        else
        {
            pDesc->Class = D3D10_SVC_STRUCT;
        }
        pDesc->Type = D3D10_SVT_VOID;
        break;

    case EVT_Interface:
        pDesc->Rows = 0;
        pDesc->Columns = 0;
        pDesc->Members = 0;
        pDesc->Class = D3D11_SVC_INTERFACE_POINTER;
        pDesc->Type = D3D11_SVT_INTERFACE_POINTER;
        break;

    case EVT_Object:
        pDesc->Rows = 0;
        pDesc->Columns = 0;
        pDesc->Members = 0;
        pDesc->Class = D3D10_SVC_OBJECT;            

        switch (ObjectType)
        {
        case EOT_String:
            pDesc->Type = D3D10_SVT_STRING;
            break;
        case EOT_Blend:
            pDesc->Type = D3D10_SVT_BLEND; 
            break;
        case EOT_DepthStencil:
            pDesc->Type = D3D10_SVT_DEPTHSTENCIL;
            break;
        case EOT_Rasterizer:
            pDesc->Type = D3D10_SVT_RASTERIZER;
            break;
        case EOT_PixelShader:
        case EOT_PixelShader5:
            pDesc->Type = D3D10_SVT_PIXELSHADER;
            break;
        case EOT_VertexShader:
        case EOT_VertexShader5:
            pDesc->Type = D3D10_SVT_VERTEXSHADER;
            break;
        case EOT_GeometryShader:
        case EOT_GeometryShaderSO:
        case EOT_GeometryShader5:
            pDesc->Type = D3D10_SVT_GEOMETRYSHADER;
            break;
        case EOT_HullShader5:
            pDesc->Type = D3D11_SVT_HULLSHADER;
            break;
        case EOT_DomainShader5:
            pDesc->Type = D3D11_SVT_DOMAINSHADER;
            break;
        case EOT_ComputeShader5:
            pDesc->Type = D3D11_SVT_COMPUTESHADER;
            break;
        case EOT_Texture:
            pDesc->Type = D3D10_SVT_TEXTURE;
            break;
        case EOT_Texture1D:
            pDesc->Type = D3D10_SVT_TEXTURE1D;
            break;
        case EOT_Texture1DArray:
            pDesc->Type = D3D10_SVT_TEXTURE1DARRAY;
            break;
        case EOT_Texture2D:
            pDesc->Type = D3D10_SVT_TEXTURE2D;
            break;
        case EOT_Texture2DArray:
            pDesc->Type = D3D10_SVT_TEXTURE2DARRAY;
            break;
        case EOT_Texture2DMS:
            pDesc->Type = D3D10_SVT_TEXTURE2DMS;
            break;
        case EOT_Texture2DMSArray:
            pDesc->Type = D3D10_SVT_TEXTURE2DMSARRAY;
            break;
        case EOT_Texture3D:
            pDesc->Type = D3D10_SVT_TEXTURE3D;
            break;
        case EOT_TextureCube:
            pDesc->Type = D3D10_SVT_TEXTURECUBE;
            break;
        case EOT_TextureCubeArray:
            pDesc->Type = D3D10_SVT_TEXTURECUBEARRAY;
            break;
        case EOT_Buffer:
            pDesc->Type = D3D10_SVT_BUFFER;
            break;
        case EOT_Sampler:
            pDesc->Type = D3D10_SVT_SAMPLER;
            break;
        case EOT_RenderTargetView:
            pDesc->Type = D3D10_SVT_RENDERTARGETVIEW;
            break;
        case EOT_DepthStencilView:
            pDesc->Type = D3D10_SVT_DEPTHSTENCILVIEW;
            break;
        case EOT_RWTexture1D:
            pDesc->Type = D3D11_SVT_RWTEXTURE1D;
            break;
        case EOT_RWTexture1DArray:
            pDesc->Type = D3D11_SVT_RWTEXTURE1DARRAY;
            break;
        case EOT_RWTexture2D:
            pDesc->Type = D3D11_SVT_RWTEXTURE2D;
            break;
        case EOT_RWTexture2DArray:
            pDesc->Type = D3D11_SVT_RWTEXTURE2DARRAY;
            break;
        case EOT_RWTexture3D:
            pDesc->Type = D3D11_SVT_RWTEXTURE3D;
            break;
        case EOT_RWBuffer:
            pDesc->Type = D3D11_SVT_RWBUFFER;
            break;
        case EOT_ByteAddressBuffer:
            pDesc->Type = D3D11_SVT_BYTEADDRESS_BUFFER;
            break;
        case EOT_RWByteAddressBuffer:
            pDesc->Type = D3D11_SVT_RWBYTEADDRESS_BUFFER;
            break;
        case EOT_StructuredBuffer:
            pDesc->Type = D3D11_SVT_STRUCTURED_BUFFER;
            break;
        case EOT_RWStructuredBuffer:
        case EOT_RWStructuredBufferAlloc:
        case EOT_RWStructuredBufferConsume:
            pDesc->Type = D3D11_SVT_RWSTRUCTURED_BUFFER;
            break;
        case EOT_AppendStructuredBuffer:
            pDesc->Type = D3D11_SVT_APPEND_STRUCTURED_BUFFER;
            break;
        case EOT_ConsumeStructuredBuffer:
            pDesc->Type = D3D11_SVT_CONSUME_STRUCTURED_BUFFER;
            break;

        default:
            D3DXASSERT(0);
        }
        break;

    default:
        D3DXASSERT(0);
    }

lExit:
    return hr;

}

////////////////////////////////////////////////////////////////////////////////
// ID3DX11EffectShaderVariable (SAnonymousShader implementation)
////////////////////////////////////////////////////////////////////////////////

SAnonymousShader::SAnonymousShader(SShaderBlock *pBlock)
{
    pShaderBlock = pBlock;
}

BOOL SAnonymousShader::IsValid()
{
    return pShaderBlock && pShaderBlock->IsValid;
}

ID3DX11EffectType * SAnonymousShader::GetType()
{
    return (ID3DX11EffectType *) this;
}

HRESULT SAnonymousShader::GetDesc(D3DX11_EFFECT_VARIABLE_DESC *pDesc)
{
    pDesc->Annotations = 0;
    pDesc->Flags = 0;

    pDesc->Name = "$Anonymous";
    pDesc->Semantic = NULL;
    pDesc->BufferOffset = 0;

    return S_OK;
}

ID3DX11EffectVariable * SAnonymousShader::GetAnnotationByIndex(UINT Index)
{
    DPF(0, "ID3DX11EffectVariable::GetAnnotationByIndex: Anonymous shaders cannot have annotations");
    return &g_InvalidScalarVariable;
}

ID3DX11EffectVariable * SAnonymousShader::GetAnnotationByName(LPCSTR Name)
{
    DPF(0, "ID3DX11EffectVariable::GetAnnotationByName: Anonymous shaders cannot have annotations");
    return &g_InvalidScalarVariable;
}

ID3DX11EffectVariable * SAnonymousShader::GetMemberByIndex(UINT  Index)
{
    DPF(0, "ID3DX11EffectVariable::GetMemberByIndex: Variable is not a structure");
    return &g_InvalidScalarVariable;
}

ID3DX11EffectVariable * SAnonymousShader::GetMemberByName(LPCSTR Name)
{
    DPF(0, "ID3DX11EffectVariable::GetMemberByName: Variable is not a structure");
    return &g_InvalidScalarVariable;
}

ID3DX11EffectVariable * SAnonymousShader::GetMemberBySemantic(LPCSTR Semantic)
{
    DPF(0, "ID3DX11EffectVariable::GetMemberBySemantic: Variable is not a structure");
    return &g_InvalidScalarVariable;
}

ID3DX11EffectVariable * SAnonymousShader::GetElement(UINT Index)
{
    DPF(0, "ID3DX11EffectVariable::GetElement: Anonymous shaders cannot have elements");
    return &g_InvalidScalarVariable;
}

ID3DX11EffectConstantBuffer * SAnonymousShader::GetParentConstantBuffer()
{
    return NoParentCB();
}

ID3DX11EffectShaderVariable * SAnonymousShader::AsShader()
{
    return (ID3DX11EffectShaderVariable *) this;
}

HRESULT SAnonymousShader::SetRawValue(CONST void *pData, UINT Offset, UINT Count) 
{ 
    return ObjectSetRawValue(); 
}

HRESULT SAnonymousShader::GetRawValue(__out_bcount(Count) void *pData, UINT Offset, UINT Count) 
{ 
    return ObjectGetRawValue(); 
}

#define ANONYMOUS_SHADER_INDEX_CHECK() \
    HRESULT hr = S_OK; \
    if (0 != ShaderIndex) \
    { \
        DPF(0, "%s: Invalid index specified", pFuncName); \
        VH(E_INVALIDARG); \
    } \

HRESULT SAnonymousShader::GetShaderDesc(UINT ShaderIndex, D3DX11_EFFECT_SHADER_DESC *pDesc)
{
    LPCSTR pFuncName = "ID3DX11EffectShaderVariable::GetShaderDesc";

    ANONYMOUS_SHADER_INDEX_CHECK();

    pShaderBlock->GetShaderDesc(pDesc, TRUE);

lExit:
    return hr;
}

HRESULT SAnonymousShader::GetVertexShader(UINT ShaderIndex, ID3D11VertexShader **ppVS)
{
    LPCSTR pFuncName = "ID3DX11EffectShaderVariable::GetVertexShader";

    ANONYMOUS_SHADER_INDEX_CHECK();

    VH( pShaderBlock->GetVertexShader(ppVS) );

lExit:
    return hr;
}

HRESULT SAnonymousShader::GetGeometryShader(UINT ShaderIndex, ID3D11GeometryShader **ppGS)
{
    LPCSTR pFuncName = "ID3DX11EffectShaderVariable::GetGeometryShader";

    ANONYMOUS_SHADER_INDEX_CHECK();

    VH( pShaderBlock->GetGeometryShader(ppGS) );

lExit:
    return hr;
}

HRESULT SAnonymousShader::GetPixelShader(UINT ShaderIndex, ID3D11PixelShader **ppPS)
{
    LPCSTR pFuncName = "ID3DX11EffectShaderVariable::GetPixelShader";

    ANONYMOUS_SHADER_INDEX_CHECK();

    VH( pShaderBlock->GetPixelShader(ppPS) );

lExit:
    return hr;
}

HRESULT SAnonymousShader::GetHullShader(UINT ShaderIndex, ID3D11HullShader **ppHS)
{
    LPCSTR pFuncName = "ID3DX11EffectShaderVariable::GetHullShader";

    ANONYMOUS_SHADER_INDEX_CHECK();

    VH( pShaderBlock->GetHullShader(ppHS) );

lExit:
    return hr;
}

HRESULT SAnonymousShader::GetDomainShader(UINT ShaderIndex, ID3D11DomainShader **ppCS)
{
    LPCSTR pFuncName = "ID3DX11EffectShaderVariable::GetDomainShader";

    ANONYMOUS_SHADER_INDEX_CHECK();

    VH( pShaderBlock->GetDomainShader(ppCS) );

lExit:
    return hr;
}

HRESULT SAnonymousShader::GetComputeShader(UINT ShaderIndex, ID3D11ComputeShader **ppCS)
{
    LPCSTR pFuncName = "ID3DX11EffectShaderVariable::GetComputeShader";

    ANONYMOUS_SHADER_INDEX_CHECK();

    VH( pShaderBlock->GetComputeShader(ppCS) );

lExit:
    return hr;
}

HRESULT SAnonymousShader::GetInputSignatureElementDesc(UINT ShaderIndex, UINT Element, D3D11_SIGNATURE_PARAMETER_DESC *pDesc)
{
    LPCSTR pFuncName = "ID3DX11EffectShaderVariable::GetInputSignatureElementDesc";

    ANONYMOUS_SHADER_INDEX_CHECK();

    VH( pShaderBlock->GetSignatureElementDesc(SShaderBlock::ST_Input, Element, pDesc) );

lExit:
    return hr;
}

HRESULT SAnonymousShader::GetOutputSignatureElementDesc(UINT ShaderIndex, UINT Element, D3D11_SIGNATURE_PARAMETER_DESC *pDesc)
{
    LPCSTR pFuncName = "ID3DX11EffectShaderVariable::GetOutputSignatureElementDesc";

    ANONYMOUS_SHADER_INDEX_CHECK();

    VH( pShaderBlock->GetSignatureElementDesc(SShaderBlock::ST_Output, Element, pDesc) );

lExit:
    return hr;
}

HRESULT SAnonymousShader::GetPatchConstantSignatureElementDesc(UINT ShaderIndex, UINT Element, D3D11_SIGNATURE_PARAMETER_DESC *pDesc)
{
    LPCSTR pFuncName = "ID3DX11EffectShaderVariable::GetPatchConstantSignatureElementDesc";

    ANONYMOUS_SHADER_INDEX_CHECK();

    VH( pShaderBlock->GetSignatureElementDesc(SShaderBlock::ST_PatchConstant, Element, pDesc) );

lExit:
    return hr;
}

HRESULT SAnonymousShader::GetDesc(D3DX11_EFFECT_TYPE_DESC *pDesc)
{
    pDesc->Class = D3D10_SVC_OBJECT;

    switch (pShaderBlock->GetShaderType())
    {
    case EOT_VertexShader:
    case EOT_VertexShader5:
        pDesc->TypeName = "vertexshader";
        pDesc->Type = D3D10_SVT_VERTEXSHADER;
        break;
    case EOT_GeometryShader:
    case EOT_GeometryShader5:
        pDesc->TypeName = "geometryshader";
        pDesc->Type = D3D10_SVT_GEOMETRYSHADER;
        break;
    case EOT_PixelShader:
    case EOT_PixelShader5:
        pDesc->TypeName = "pixelshader";
        pDesc->Type = D3D10_SVT_PIXELSHADER;
        break;
    case EOT_HullShader5:
        pDesc->TypeName = "Hullshader";
        pDesc->Type = D3D11_SVT_HULLSHADER;
        break;
    case EOT_DomainShader5:
        pDesc->TypeName = "Domainshader";
        pDesc->Type = D3D11_SVT_DOMAINSHADER;
        break;
    case EOT_ComputeShader5:
        pDesc->TypeName = "Computeshader";
        pDesc->Type = D3D11_SVT_COMPUTESHADER;
        break;
    }

    pDesc->Elements = 0;
    pDesc->Members = 0;
    pDesc->Rows = 0;
    pDesc->Columns = 0;
    pDesc->PackedSize = 0;
    pDesc->UnpackedSize = 0;
    pDesc->Stride = 0;

    return S_OK;
}

ID3DX11EffectType * SAnonymousShader::GetMemberTypeByIndex(UINT  Index)
{
    DPF(0, "ID3DX11EffectType::GetMemberTypeByIndex: This interface does not refer to a structure");
    return &g_InvalidType;
}

ID3DX11EffectType * SAnonymousShader::GetMemberTypeByName(LPCSTR Name)
{
    DPF(0, "ID3DX11EffectType::GetMemberTypeByName: This interface does not refer to a structure");
    return &g_InvalidType;
}

ID3DX11EffectType * SAnonymousShader::GetMemberTypeBySemantic(LPCSTR Semantic)
{
    DPF(0, "ID3DX11EffectType::GetMemberTypeBySemantic: This interface does not refer to a structure");
    return &g_InvalidType;
}

LPCSTR SAnonymousShader::GetMemberName(UINT Index)
{
    DPF(0, "ID3DX11EffectType::GetMemberName: This interface does not refer to a structure");
    return NULL;
}

LPCSTR SAnonymousShader::GetMemberSemantic(UINT Index)
{
    DPF(0, "ID3DX11EffectType::GetMemberSemantic: This interface does not refer to a structure");
    return NULL;
}

//////////////////////////////////////////////////////////////////////////
// ID3DX11EffectConstantBuffer (SConstantBuffer implementation)
//////////////////////////////////////////////////////////////////////////

BOOL SConstantBuffer::IsValid()
{
    return TRUE;
}

ID3DX11EffectType * SConstantBuffer::GetType()
{
    return (ID3DX11EffectType *) this;
}

HRESULT SConstantBuffer::GetDesc(D3DX11_EFFECT_VARIABLE_DESC *pDesc)
{
    pDesc->Annotations = AnnotationCount;
    pDesc->Flags = 0;

    pDesc->Name = pName;
    pDesc->Semantic = NULL;
    pDesc->BufferOffset = 0;

    if (ExplicitBindPoint != -1)
    {
        pDesc->ExplicitBindPoint = ExplicitBindPoint;
        pDesc->Flags |= D3DX11_EFFECT_VARIABLE_EXPLICIT_BIND_POINT;
    }
    else
    {
        pDesc->ExplicitBindPoint = 0;
    }

    return S_OK;
}

ID3DX11EffectVariable * SConstantBuffer::GetAnnotationByIndex(UINT  Index)
{
    return GetAnnotationByIndexHelper("ID3DX11EffectVariable", Index, AnnotationCount, pAnnotations);
}

ID3DX11EffectVariable * SConstantBuffer::GetAnnotationByName(LPCSTR Name)
{
    return GetAnnotationByNameHelper("ID3DX11EffectVariable", Name, AnnotationCount, pAnnotations);
}

ID3DX11EffectVariable * SConstantBuffer::GetMemberByIndex(UINT  Index)
{
    SGlobalVariable *pMember;
    UDataPointer dataPtr;

    if (IsEffectOptimized)
    {
        DPF(0, "ID3DX11EffectVariable::GetMemberByIndex: Cannot get members; effect has been Optimize()'ed");
        return &g_InvalidScalarVariable;
    }

    if (!GetVariableByIndexHelper<SGlobalVariable>(Index, VariableCount, (SGlobalVariable*)pVariables, 
        NULL, &pMember, &dataPtr.pGeneric))
    {
        return &g_InvalidScalarVariable;
    }

    return (ID3DX11EffectVariable *) pMember;
}

ID3DX11EffectVariable * SConstantBuffer::GetMemberByName(LPCSTR Name)
{
    SGlobalVariable *pMember;
    UDataPointer dataPtr;
    UINT index;

    if (IsEffectOptimized)
    {
        DPF(0, "ID3DX11EffectVariable::GetMemberByName: Cannot get members; effect has been Optimize()'ed");
        return &g_InvalidScalarVariable;
    }

    if (!GetVariableByNameHelper<SGlobalVariable>(Name, VariableCount, (SGlobalVariable*)pVariables, 
        NULL, &pMember, &dataPtr.pGeneric, &index))
    {
        return &g_InvalidScalarVariable;
    }

    return (ID3DX11EffectVariable *) pMember;
}

ID3DX11EffectVariable * SConstantBuffer::GetMemberBySemantic(LPCSTR Semantic)
{
    SGlobalVariable *pMember;
    UDataPointer dataPtr;
    UINT index;

    if (IsEffectOptimized)
    {
        DPF(0, "ID3DX11EffectVariable::GetMemberBySemantic: Cannot get members; effect has been Optimize()'ed");
        return &g_InvalidScalarVariable;
    }

    if (!GetVariableBySemanticHelper<SGlobalVariable>(Semantic, VariableCount, (SGlobalVariable*)pVariables, 
        NULL, &pMember, &dataPtr.pGeneric, &index))
    {
        return &g_InvalidScalarVariable;
    }

    return (ID3DX11EffectVariable *) pMember;
}

ID3DX11EffectVariable * SConstantBuffer::GetElement(UINT  Index)
{
    LPCSTR pFuncName = "ID3DX11EffectVariable::GetElement";
    DPF(0, "%s: This interface does not refer to an array", pFuncName);
    return &g_InvalidScalarVariable;
}

ID3DX11EffectConstantBuffer * SConstantBuffer::GetParentConstantBuffer()
{
    LPCSTR pFuncName = "ID3DX11EffectVariable::GetParentConstantBuffer";
    DPF(0, "%s: Constant buffers do not have parent constant buffers", pFuncName);
    return &g_InvalidConstantBuffer;
}

ID3DX11EffectConstantBuffer * SConstantBuffer::AsConstantBuffer()
{
    return (ID3DX11EffectConstantBuffer *) this;
}

HRESULT SConstantBuffer::GetDesc(D3DX11_EFFECT_TYPE_DESC *pDesc)
{
    pDesc->TypeName = IsTBuffer ? "tbuffer" : "cbuffer";
    pDesc->Class = D3D10_SVC_OBJECT;
    pDesc->Type = IsTBuffer ? D3D10_SVT_TBUFFER : D3D10_SVT_CBUFFER;

    pDesc->Elements = 0;
    pDesc->Members = VariableCount;
    pDesc->Rows = 0;
    pDesc->Columns = 0;

    UINT  i;
    pDesc->PackedSize = 0;
    for (i = 0; i < VariableCount; ++ i)
    {
        pDesc->PackedSize += pVariables[i].pType->PackedSize;
    }

    pDesc->UnpackedSize = Size;
    D3DXASSERT(pDesc->UnpackedSize >= pDesc->PackedSize);

    pDesc->Stride = AlignToPowerOf2(pDesc->UnpackedSize, SType::c_RegisterSize);

    return S_OK;
}

ID3DX11EffectType * SConstantBuffer::GetMemberTypeByIndex(UINT  Index)
{
    return GetTypeByIndexHelper(Index, VariableCount, pVariables, sizeof (SGlobalVariable));
}

ID3DX11EffectType * SConstantBuffer::GetMemberTypeByName(LPCSTR Name)
{
    return GetTypeByNameHelper(Name, VariableCount, pVariables, sizeof (SGlobalVariable));
}

ID3DX11EffectType * SConstantBuffer::GetMemberTypeBySemantic(LPCSTR Semantic)
{
    return GetTypeBySemanticHelper(Semantic, VariableCount, pVariables, sizeof (SGlobalVariable));
}

LPCSTR SConstantBuffer::GetMemberName(UINT Index)
{
    LPCSTR pFuncName = "ID3DX11EffectType::GetMemberName";

    if (IsEffectOptimized)
    {
        DPF(0, "%s: Cannot get member names; Effect has been Optimize()'ed", pFuncName);
        return NULL;
    }

    if (Index >= VariableCount)
    {
        DPF(0, "%s: Invalid index (%d, total: %d)", pFuncName, Index, VariableCount);
        return NULL;
    }

    return pVariables[Index].pName;
}

LPCSTR SConstantBuffer::GetMemberSemantic(UINT Index)
{
    LPCSTR pFuncName = "ID3DX11EffectType::GetMemberSemantic";

    if (IsEffectOptimized)
    {
        DPF(0, "%s: Cannot get member semantics; Effect has been Optimize()'ed", pFuncName);
        return NULL;
    }

    if (Index >= VariableCount)
    {
        DPF(0, "%s: Invalid index (%d, total: %d)", pFuncName, Index, VariableCount);
        return NULL;
    }

    return pVariables[Index].pSemantic;
}

HRESULT SConstantBuffer::SetRawValue(CONST void *pData, UINT  Offset, UINT  Count)
{
    HRESULT hr = S_OK;    

#ifdef _DEBUG
    LPCSTR pFuncName = "ID3DX11EffectVariable::SetRawValue";

    VERIFYPARAMETER(pData);

    if ((Offset + Count < Offset) ||
        (Count + (BYTE*)pData < (BYTE*)pData) ||
        ((Offset + Count) > Size))
    {
        // overflow of some kind
        DPF(0, "%s: Invalid range specified", pFuncName);
        VH(E_INVALIDARG);
    }
#endif

    if (IsUsedByExpression)
    {
        UINT  i;
        for (i = 0; i < VariableCount; ++ i)
        {
            ((SGlobalVariable*)pVariables)[i].DirtyVariable();
        }
    }
    else
    {
        IsDirty = TRUE;
    }

    memcpy(pBackingStore + Offset, pData, Count);

lExit:
    return hr;
}

HRESULT SConstantBuffer::GetRawValue(__out_bcount(Count) void *pData, UINT  Offset, UINT  Count)
{
    HRESULT hr = S_OK;    

#ifdef _DEBUG
    LPCSTR pFuncName = "ID3DX11EffectVariable::GetRawValue";

    VERIFYPARAMETER(pData);

    if ((Offset + Count < Offset) ||
        (Count + (BYTE*)pData < (BYTE*)pData) ||
        ((Offset + Count) > Size))
    {
        // overflow of some kind
        DPF(0, "%s: Invalid range specified", pFuncName);
        VH(E_INVALIDARG);
    }
#endif

    memcpy(pData, pBackingStore + Offset, Count);

lExit:
    return hr;
}

bool SConstantBuffer::ClonedSingle() const
{
    return IsSingle && ( pEffect->m_Flags & D3DX11_EFFECT_CLONE );
}

HRESULT SConstantBuffer::SetConstantBuffer(ID3D11Buffer *pConstantBuffer)
{
    HRESULT hr = S_OK;
    LPCSTR pFuncName = "ID3DX11EffectConstantBuffer::SetConstantBuffer";

    if (IsTBuffer)
    {
        DPF(0, "%s: This is a texture buffer; use SetTextureBuffer instead", pFuncName);
        VH(D3DERR_INVALIDCALL);
    }

    // Replace all references to the old shader block with this one
    pEffect->ReplaceCBReference(this, pConstantBuffer);

    if( !IsUserManaged )
    {
        // Save original cbuffer in case we UndoSet
        D3DXASSERT( pMemberData[0].Type == MDT_Buffer );
        VB( pMemberData[0].Data.pD3DEffectsManagedConstantBuffer == NULL );
        pMemberData[0].Data.pD3DEffectsManagedConstantBuffer = pD3DObject;
        pD3DObject = NULL;
        IsUserManaged = TRUE;
        IsNonUpdatable = TRUE;
    }

    SAFE_ADDREF( pConstantBuffer );
    SAFE_RELEASE( pD3DObject );
    pD3DObject = pConstantBuffer;

lExit:
    return hr;
}

HRESULT SConstantBuffer::GetConstantBuffer(ID3D11Buffer **ppConstantBuffer)
{
    HRESULT hr = S_OK;
    LPCSTR pFuncName = "ID3DX11EffectConstantBuffer::GetConstantBuffer";

    VERIFYPARAMETER(ppConstantBuffer);

    if (IsTBuffer)
    {
        DPF(0, "%s: This is a texture buffer; use GetTextureBuffer instead", pFuncName);
        VH(D3DERR_INVALIDCALL);
    }

    *ppConstantBuffer = pD3DObject;
    SAFE_ADDREF(*ppConstantBuffer);

lExit:
    return hr;
}

HRESULT SConstantBuffer::UndoSetConstantBuffer() 
{
    HRESULT hr = S_OK;
    LPCSTR pFuncName = "ID3DX11EffectConstantBuffer::UndoSetConstantBuffer";

    if (IsTBuffer)
    {
        DPF(0, "%s: This is a texture buffer; use UndoSetTextureBuffer instead", pFuncName);
        VH(D3DERR_INVALIDCALL);
    }

    if( !IsUserManaged )
    {
        return S_FALSE;
    }

    // Replace all references to the old shader block with this one
    pEffect->ReplaceCBReference(this, pMemberData[0].Data.pD3DEffectsManagedConstantBuffer);

    // Revert to original cbuffer
    SAFE_RELEASE( pD3DObject );
    pD3DObject = pMemberData[0].Data.pD3DEffectsManagedConstantBuffer;
    pMemberData[0].Data.pD3DEffectsManagedConstantBuffer = NULL;
    IsUserManaged = FALSE;
    IsNonUpdatable = ClonedSingle();

lExit:
    return hr;
}

HRESULT SConstantBuffer::SetTextureBuffer(ID3D11ShaderResourceView *pTextureBuffer)
{
    HRESULT hr = S_OK;
    LPCSTR pFuncName = "ID3DX11EffectConstantBuffer::SetTextureBuffer";

    if (!IsTBuffer)
    {
        DPF(0, "%s: This is a constant buffer; use SetConstantBuffer instead", pFuncName);
        VH(D3DERR_INVALIDCALL);
    }

    if( !IsUserManaged )
    {
        // Save original cbuffer and tbuffer in case we UndoSet
        D3DXASSERT( pMemberData[0].Type == MDT_Buffer );
        VB( pMemberData[0].Data.pD3DEffectsManagedConstantBuffer == NULL );
        pMemberData[0].Data.pD3DEffectsManagedConstantBuffer = pD3DObject;
        pD3DObject = NULL;
        D3DXASSERT( pMemberData[1].Type == MDT_ShaderResourceView );
        VB( pMemberData[1].Data.pD3DEffectsManagedTextureBuffer == NULL );
        pMemberData[1].Data.pD3DEffectsManagedTextureBuffer = TBuffer.pShaderResource;
        TBuffer.pShaderResource = NULL;
        IsUserManaged = TRUE;
        IsNonUpdatable = TRUE;
    }

    SAFE_ADDREF( pTextureBuffer );
    SAFE_RELEASE(pD3DObject); // won't be needing this anymore...
    SAFE_RELEASE( TBuffer.pShaderResource );
    TBuffer.pShaderResource = pTextureBuffer;

lExit:
    return hr;
}

HRESULT SConstantBuffer::GetTextureBuffer(ID3D11ShaderResourceView **ppTextureBuffer)
{
    HRESULT hr = S_OK;
    LPCSTR pFuncName = "ID3DX11EffectConstantBuffer::GetTextureBuffer";

    VERIFYPARAMETER(ppTextureBuffer);

    if (!IsTBuffer)
    {
        DPF(0, "%s: This is a constant buffer; use GetConstantBuffer instead", pFuncName);
        VH(D3DERR_INVALIDCALL);
    }

    *ppTextureBuffer = TBuffer.pShaderResource;
    SAFE_ADDREF(*ppTextureBuffer);

lExit:
    return hr;
}

HRESULT SConstantBuffer::UndoSetTextureBuffer()
{
    HRESULT hr = S_OK;
    LPCSTR pFuncName = "ID3DX11EffectConstantBuffer::UndoSetTextureBuffer";

    if (!IsTBuffer)
    {
        DPF(0, "%s: This is a texture buffer; use UndoSetConstantBuffer instead", pFuncName);
        VH(D3DERR_INVALIDCALL);
    }

    if( !IsUserManaged )
    {
        return S_FALSE;
    }

    // Revert to original cbuffer
    SAFE_RELEASE( pD3DObject );
    pD3DObject = pMemberData[0].Data.pD3DEffectsManagedConstantBuffer;
    pMemberData[0].Data.pD3DEffectsManagedConstantBuffer = NULL;
    SAFE_RELEASE( TBuffer.pShaderResource );
    TBuffer.pShaderResource = pMemberData[1].Data.pD3DEffectsManagedTextureBuffer;
    pMemberData[1].Data.pD3DEffectsManagedTextureBuffer = NULL;
    IsUserManaged = FALSE;
    IsNonUpdatable = ClonedSingle();

lExit:
    return hr;
}

//////////////////////////////////////////////////////////////////////////
// ID3DX11EffectPass (CEffectPass implementation)
//////////////////////////////////////////////////////////////////////////

BOOL SPassBlock::IsValid()
{
    if( HasDependencies )
        return pEffect->ValidatePassBlock( this );
    return InitiallyValid;
}

HRESULT SPassBlock::GetDesc(D3DX11_PASS_DESC *pDesc)
{
    HRESULT hr = S_OK;
    LPCSTR pFuncName = "ID3DX11EffectPass::GetDesc";

    VERIFYPARAMETER(pDesc);

    ZeroMemory(pDesc, sizeof(*pDesc));

    pDesc->Name = pName;
    pDesc->Annotations = AnnotationCount;
    
    SAssignment *pAssignment;
    SAssignment *pLastAssn;

    pEffect->IncrementTimer();

    pAssignment = pAssignments;
    pLastAssn = pAssignments + AssignmentCount;

    for(; pAssignment < pLastAssn; pAssignment++)
    {
        pEffect->EvaluateAssignment(pAssignment);
    }

    if( BackingStore.pVertexShaderBlock && BackingStore.pVertexShaderBlock->pInputSignatureBlob )
    {
        // pInputSignatureBlob can be null if we're setting a NULL VS "SetVertexShader( NULL )"
        pDesc->pIAInputSignature = (BYTE*)BackingStore.pVertexShaderBlock->pInputSignatureBlob->GetBufferPointer();
        pDesc->IAInputSignatureSize = BackingStore.pVertexShaderBlock->pInputSignatureBlob->GetBufferSize();
    }

    pDesc->StencilRef = BackingStore.StencilRef;
    pDesc->SampleMask = BackingStore.SampleMask;
    pDesc->BlendFactor[0] = BackingStore.BlendFactor[0];
    pDesc->BlendFactor[1] = BackingStore.BlendFactor[1];
    pDesc->BlendFactor[2] = BackingStore.BlendFactor[2];
    pDesc->BlendFactor[3] = BackingStore.BlendFactor[3];

lExit:
    return hr;
}

extern SShaderBlock g_NullVS;
extern SShaderBlock g_NullGS;
extern SShaderBlock g_NullPS;
extern SShaderBlock g_NullHS;
extern SShaderBlock g_NullDS;
extern SShaderBlock g_NullCS;

SAnonymousShader g_AnonymousNullVS(&g_NullVS);
SAnonymousShader g_AnonymousNullGS(&g_NullGS);
SAnonymousShader g_AnonymousNullPS(&g_NullPS);
SAnonymousShader g_AnonymousNullHS(&g_NullHS);
SAnonymousShader g_AnonymousNullDS(&g_NullDS);
SAnonymousShader g_AnonymousNullCS(&g_NullCS);

template<EObjectType EShaderType>
HRESULT SPassBlock::GetShaderDescHelper(D3DX11_PASS_SHADER_DESC *pDesc)
{
    HRESULT hr = S_OK;
    UINT  i;
    LPCSTR pFuncName = NULL;
    SShaderBlock *pShaderBlock = NULL;

    ApplyPassAssignments();

    switch (EShaderType)
    {
    case EOT_VertexShader:
    case EOT_VertexShader5:
#pragma prefast(suppress:__WARNING_UNUSED_POINTER_ASSIGNMENT, "pFuncName used in DPF")
        pFuncName = "ID3DX11EffectPass::GetVertexShaderDesc";
        pShaderBlock = BackingStore.pVertexShaderBlock;
        break;
    case EOT_PixelShader:
    case EOT_PixelShader5:
#pragma prefast(suppress:__WARNING_UNUSED_POINTER_ASSIGNMENT, "pFuncName used in DPF")
        pFuncName = "ID3DX11EffectPass::GetPixelShaderDesc";
        pShaderBlock = BackingStore.pPixelShaderBlock;
        break;
    case EOT_GeometryShader:
    case EOT_GeometryShader5:
#pragma prefast(suppress:__WARNING_UNUSED_POINTER_ASSIGNMENT, "pFuncName used in DPF")
        pFuncName = "ID3DX11EffectPass::GetGeometryShaderDesc";
        pShaderBlock = BackingStore.pGeometryShaderBlock;
        break;
    case EOT_HullShader5:
#pragma prefast(suppress:__WARNING_UNUSED_POINTER_ASSIGNMENT, "pFuncName used in DPF")
        pFuncName = "ID3DX11EffectPass::GetHullShaderDesc";
        pShaderBlock = BackingStore.pHullShaderBlock;
        break;
    case EOT_DomainShader5:
#pragma prefast(suppress:__WARNING_UNUSED_POINTER_ASSIGNMENT, "pFuncName used in DPF")
        pFuncName = "ID3DX11EffectPass::GetDomainShaderDesc";
        pShaderBlock = BackingStore.pDomainShaderBlock;
        break;
    case EOT_ComputeShader5:
#pragma prefast(suppress:__WARNING_UNUSED_POINTER_ASSIGNMENT, "pFuncName used in DPF")
        pFuncName = "ID3DX11EffectPass::GetComputeShaderDesc";
        pShaderBlock = BackingStore.pComputeShaderBlock;
        break;
    default:
        D3DXASSERT(0);
    }

    VERIFYPARAMETER(pDesc);

    // in case of error (or in case the assignment doesn't exist), return something reasonable
    pDesc->pShaderVariable = &g_InvalidShaderVariable;
    pDesc->ShaderIndex = 0;

    if (NULL != pShaderBlock)
    {
        UINT elements, varCount, anonymousShaderCount;
        SGlobalVariable *pVariables;
        SAnonymousShader *pAnonymousShaders;

        if (pShaderBlock == &g_NullVS)
        {
            pDesc->pShaderVariable = &g_AnonymousNullVS;
            pDesc->ShaderIndex = 0;
            // we're done
            goto lExit;
        }
        else if (pShaderBlock == &g_NullGS)
        {
            pDesc->pShaderVariable = &g_AnonymousNullGS;
            pDesc->ShaderIndex = 0;
            // we're done
            goto lExit;
        }
        else if (pShaderBlock == &g_NullPS)
        {
            pDesc->pShaderVariable = &g_AnonymousNullPS;
            pDesc->ShaderIndex = 0;
            // we're done
            goto lExit;
        }
        else if (pShaderBlock == &g_NullHS)
        {
            pDesc->pShaderVariable = &g_AnonymousNullHS;
            pDesc->ShaderIndex = 0;
            // we're done
            goto lExit;
        }
        else if (pShaderBlock == &g_NullDS)
        {
            pDesc->pShaderVariable = &g_AnonymousNullDS;
            pDesc->ShaderIndex = 0;
            // we're done
            goto lExit;
        }
        else if (pShaderBlock == &g_NullCS)
        {
            pDesc->pShaderVariable = &g_AnonymousNullCS;
            pDesc->ShaderIndex = 0;
            // we're done
            goto lExit;
        }
        else 
        {
            VB( pEffect->IsRuntimeData(pShaderBlock) );
            varCount = pEffect->m_VariableCount;
            pVariables = pEffect->m_pVariables;
            anonymousShaderCount = pEffect->m_AnonymousShaderCount;
            pAnonymousShaders = pEffect->m_pAnonymousShaders;
        }

        for (i = 0; i < varCount; ++ i)
        {
            elements = max(1, pVariables[i].pType->Elements);
            // make sure the variable type matches, and don't forget about GeometryShaderSO's
            if (pVariables[i].pType->IsShader())
            {
                if (pShaderBlock >= pVariables[i].Data.pShader && pShaderBlock < pVariables[i].Data.pShader + elements)
                {
                    pDesc->pShaderVariable = (ID3DX11EffectShaderVariable *)(pVariables + i);
                    pDesc->ShaderIndex = (UINT)(UINT_PTR)(pShaderBlock - pVariables[i].Data.pShader);
                    // we're done
                    goto lExit;
                }
            }
        }

        for (i = 0; i < anonymousShaderCount; ++ i)
        {
            if (pShaderBlock == pAnonymousShaders[i].pShaderBlock)
            {
                VB(EShaderType == pAnonymousShaders[i].pShaderBlock->GetShaderType())
                pDesc->pShaderVariable = (pAnonymousShaders + i);
                pDesc->ShaderIndex = 0;
                // we're done
                goto lExit;
            }
        }

        DPF(0, "%s: Internal error; shader not found", pFuncName);
        VH( E_FAIL );
    }

lExit:
    return hr;
}

HRESULT SPassBlock::GetVertexShaderDesc(D3DX11_PASS_SHADER_DESC *pDesc)
{
    return GetShaderDescHelper<EOT_VertexShader>(pDesc);
}

HRESULT SPassBlock::GetPixelShaderDesc(D3DX11_PASS_SHADER_DESC *pDesc)
{
    return GetShaderDescHelper<EOT_PixelShader>(pDesc);
}

HRESULT SPassBlock::GetGeometryShaderDesc(D3DX11_PASS_SHADER_DESC *pDesc)
{
    return GetShaderDescHelper<EOT_GeometryShader>(pDesc);
}

HRESULT SPassBlock::GetHullShaderDesc(D3DX11_PASS_SHADER_DESC *pDesc)
{
    return GetShaderDescHelper<EOT_HullShader5>(pDesc);
}

HRESULT SPassBlock::GetDomainShaderDesc(D3DX11_PASS_SHADER_DESC *pDesc)
{
    return GetShaderDescHelper<EOT_DomainShader5>(pDesc);
}

HRESULT SPassBlock::GetComputeShaderDesc(D3DX11_PASS_SHADER_DESC *pDesc)
{
    return GetShaderDescHelper<EOT_ComputeShader5>(pDesc);
}

ID3DX11EffectVariable * SPassBlock::GetAnnotationByIndex(UINT  Index)
{
    return GetAnnotationByIndexHelper("ID3DX11EffectPass", Index, AnnotationCount, pAnnotations);
}

ID3DX11EffectVariable * SPassBlock::GetAnnotationByName(LPCSTR Name)
{
    return GetAnnotationByNameHelper("ID3DX11EffectPass", Name, AnnotationCount, pAnnotations);
}

HRESULT SPassBlock::Apply(UINT  Flags, ID3D11DeviceContext* pContext)
{
    HRESULT hr = S_OK;

    // TODO: Flags are not yet implemented    

    D3DXASSERT( pEffect->m_pContext == NULL );
    pEffect->m_pContext = pContext;
    pEffect->ApplyPassBlock(this);
    pEffect->m_pContext = NULL;

lExit:
    return hr;
}

HRESULT SPassBlock::ComputeStateBlockMask(D3DX11_STATE_BLOCK_MASK *pStateBlockMask)
{
    HRESULT hr = S_OK;
    UINT i, j;
    
    // flags indicating whether the following shader types were caught by assignment checks or not
    BOOL bVS = FALSE, bGS = FALSE, bPS = FALSE, bHS = FALSE, bDS = FALSE, bCS = FALSE;

    for (i = 0; i < AssignmentCount; ++ i)
    {
        BOOL bShader = FALSE;
        
        switch (pAssignments[i].LhsType)
        {
        case ELHS_VertexShaderBlock:
            bVS = TRUE;
            bShader = TRUE;
            break;
        case ELHS_GeometryShaderBlock:
            bGS = TRUE;
            bShader = TRUE;
            break;
        case ELHS_PixelShaderBlock:
            bPS = TRUE;
            bShader = TRUE;
            break;
        case ELHS_HullShaderBlock:
            bHS = TRUE;
            bShader = TRUE;
            break;
        case ELHS_DomainShaderBlock:
            bDS = TRUE;
            bShader = TRUE;
            break;
        case ELHS_ComputeShaderBlock:
            bCS = TRUE;
            bShader = TRUE;
            break;

        case ELHS_RasterizerBlock:
            pStateBlockMask->RSRasterizerState = 1;
            break;
        case ELHS_BlendBlock:
            pStateBlockMask->OMBlendState = 1;
            break;
        case ELHS_DepthStencilBlock:
            pStateBlockMask->OMDepthStencilState = 1;
            break;

        default:            
            // ignore this assignment (must be a scalar/vector assignment associated with a state object)
            break;
        }

        if (bShader)
        {
            for (j = 0; j < pAssignments[i].MaxElements; ++ j)
            {
                // compute state block mask for the union of ALL shaders
                VH( pAssignments[i].Source.pShader[j].ComputeStateBlockMask(pStateBlockMask) );
            }
        }
    }

    // go over the state block objects in case there was no corresponding assignment
    if (NULL != BackingStore.pRasterizerBlock)
    {
        pStateBlockMask->RSRasterizerState = 1;
    }
    if (NULL != BackingStore.pBlendBlock)
    {
        pStateBlockMask->OMBlendState = 1;
    }
    if (NULL != BackingStore.pDepthStencilBlock)
    {
        pStateBlockMask->OMDepthStencilState = 1;
    }

    // go over the shaders only if an assignment didn't already catch them
    if (FALSE == bVS && NULL != BackingStore.pVertexShaderBlock)
    {
        VH( BackingStore.pVertexShaderBlock->ComputeStateBlockMask(pStateBlockMask) );
    }
    if (FALSE == bGS && NULL != BackingStore.pGeometryShaderBlock)
    {
        VH( BackingStore.pGeometryShaderBlock->ComputeStateBlockMask(pStateBlockMask) );
    }
    if (FALSE == bPS && NULL != BackingStore.pPixelShaderBlock)
    {
        VH( BackingStore.pPixelShaderBlock->ComputeStateBlockMask(pStateBlockMask) );
    }
    if (FALSE == bHS && NULL != BackingStore.pHullShaderBlock)
    {
        VH( BackingStore.pHullShaderBlock->ComputeStateBlockMask(pStateBlockMask) );
    }
    if (FALSE == bDS && NULL != BackingStore.pDomainShaderBlock)
    {
        VH( BackingStore.pDomainShaderBlock->ComputeStateBlockMask(pStateBlockMask) );
    }
    if (FALSE == bCS && NULL != BackingStore.pComputeShaderBlock)
    {
        VH( BackingStore.pComputeShaderBlock->ComputeStateBlockMask(pStateBlockMask) );
    }
    
lExit:
    return hr;
}

//////////////////////////////////////////////////////////////////////////
// ID3DX11EffectTechnique (STechnique implementation)
//////////////////////////////////////////////////////////////////////////

BOOL STechnique::IsValid()
{ 
    if( HasDependencies )
    {
        for( UINT i = 0; i < PassCount; i++ )
        {
            if( !((SPassBlock*)pPasses)[i].IsValid() )
                return FALSE;
        }
        return TRUE;
    }
    return InitiallyValid;
}

HRESULT STechnique::GetDesc(D3DX11_TECHNIQUE_DESC *pDesc)
{
    HRESULT hr = S_OK;

    LPCSTR pFuncName = "ID3DX11EffectTechnique::GetDesc";

    VERIFYPARAMETER(pDesc);

    pDesc->Name = pName;
    pDesc->Annotations = AnnotationCount;
    pDesc->Passes = PassCount;

lExit:
    return hr;
}

ID3DX11EffectVariable * STechnique::GetAnnotationByIndex(UINT  Index)
{
    return GetAnnotationByIndexHelper("ID3DX11EffectTechnique", Index, AnnotationCount, pAnnotations);
}

ID3DX11EffectVariable * STechnique::GetAnnotationByName(LPCSTR Name)
{
    return GetAnnotationByNameHelper("ID3DX11EffectTechnique", Name, AnnotationCount, pAnnotations);
}

ID3DX11EffectPass * STechnique::GetPassByIndex(UINT  Index)
{
    LPCSTR pFuncName = "ID3DX11EffectTechnique::GetPassByIndex";

    if (Index >= PassCount)
    {
        DPF(0, "%s: Invalid pass index (%d, total: %d)", pFuncName, Index, PassCount);
        return &g_InvalidPass;
    }

    return (ID3DX11EffectPass *)(pPasses + Index);
}

ID3DX11EffectPass * STechnique::GetPassByName(LPCSTR Name)
{
    LPCSTR pFuncName = "ID3DX11EffectTechnique::GetPassByName";

    UINT  i;

    for (i = 0; i < PassCount; ++ i)
    {
        if (NULL != pPasses[i].pName &&
            strcmp(pPasses[i].pName, Name) == 0)
        {
            break;
        }
    }

    if (i == PassCount)
    {
        DPF(0, "%s: Pass [%s] not found", pFuncName, Name);
        return &g_InvalidPass;
    }

    return (ID3DX11EffectPass *)(pPasses + i);
}

HRESULT STechnique::ComputeStateBlockMask(D3DX11_STATE_BLOCK_MASK *pStateBlockMask)
{
    HRESULT hr = S_OK;
    UINT i;

    for (i = 0; i < PassCount; ++ i)
    {
        VH( ((SPassBlock*)pPasses)[i].ComputeStateBlockMask(pStateBlockMask) );
    }

lExit:
    return hr;
}

//////////////////////////////////////////////////////////////////////////
// ID3DX11EffectGroup (SGroup implementation)
//////////////////////////////////////////////////////////////////////////

BOOL SGroup::IsValid()
{ 
    if( HasDependencies )
    {
        for( UINT i = 0; i < TechniqueCount; i++ )
        {
            if( !((STechnique*)pTechniques)[i].IsValid() )
                return FALSE;
        }
        return TRUE;
    }
    return InitiallyValid;
}

HRESULT SGroup::GetDesc(D3DX11_GROUP_DESC *pDesc)
{
    HRESULT hr = S_OK;

    LPCSTR pFuncName = "ID3DX11EffectGroup::GetDesc";

    VERIFYPARAMETER(pDesc);

    pDesc->Name = pName;
    pDesc->Annotations = AnnotationCount;
    pDesc->Techniques = TechniqueCount;

lExit:
    return hr;
}

ID3DX11EffectVariable * SGroup::GetAnnotationByIndex(UINT  Index)
{
    return GetAnnotationByIndexHelper("ID3DX11EffectGroup", Index, AnnotationCount, pAnnotations);
}

ID3DX11EffectVariable * SGroup::GetAnnotationByName(LPCSTR Name)
{
    return GetAnnotationByNameHelper("ID3DX11EffectGroup", Name, AnnotationCount, pAnnotations);
}

ID3DX11EffectTechnique * SGroup::GetTechniqueByIndex(UINT  Index)
{
    LPCSTR pFuncName = "ID3DX11EffectGroup::GetTechniqueByIndex";

    if (Index >= TechniqueCount)
    {
        DPF(0, "%s: Invalid pass index (%d, total: %d)", pFuncName, Index, TechniqueCount);
        return &g_InvalidTechnique;
    }

    return (ID3DX11EffectTechnique *)(pTechniques + Index);
}

ID3DX11EffectTechnique * SGroup::GetTechniqueByName(LPCSTR Name)
{
    LPCSTR pFuncName = "ID3DX11EffectGroup::GetTechniqueByName";

    UINT  i;

    for (i = 0; i < TechniqueCount; ++ i)
    {
        if (NULL != pTechniques[i].pName &&
            strcmp(pTechniques[i].pName, Name) == 0)
        {
            break;
        }
    }

    if (i == TechniqueCount)
    {
        DPF(0, "%s: Technique [%s] not found", pFuncName, Name);
        return &g_InvalidTechnique;
    }

    return (ID3DX11EffectTechnique *)(pTechniques + i);
}

//////////////////////////////////////////////////////////////////////////
// ID3DX11Effect Public Reflection APIs (CEffect)
//////////////////////////////////////////////////////////////////////////

HRESULT CEffect::GetDevice(ID3D11Device **ppDevice)
{
    HRESULT hr = S_OK;
    LPCSTR pFuncName = "ID3DX11Effect::GetDevice";
    VERIFYPARAMETER(ppDevice);

    m_pDevice->AddRef();
    *ppDevice = m_pDevice;

lExit:
    return hr;
}

HRESULT CEffect::GetDesc(D3DX11_EFFECT_DESC *pDesc)
{
    HRESULT hr = S_OK;

    LPCSTR pFuncName = "ID3DX11Effect::GetDesc";

    VERIFYPARAMETER(pDesc);

    pDesc->ConstantBuffers = m_CBCount;
    pDesc->GlobalVariables = m_VariableCount;
    pDesc->Techniques = m_TechniqueCount;
    pDesc->Groups = m_GroupCount;
    pDesc->InterfaceVariables = m_InterfaceCount;

lExit:
    return hr;    
}

ID3DX11EffectConstantBuffer * CEffect::GetConstantBufferByIndex(UINT  Index)
{
    LPCSTR pFuncName = "ID3DX11Effect::GetConstantBufferByIndex";

    if (Index < m_CBCount)
    {
        return m_pCBs + Index;
    }

    DPF(0, "%s: Invalid constant buffer index", pFuncName);
    return &g_InvalidConstantBuffer;
}

ID3DX11EffectConstantBuffer * CEffect::GetConstantBufferByName(LPCSTR Name)
{
    HRESULT hr = S_OK;
    LPCSTR pFuncName = "ID3DX11Effect::GetConstantBufferByName";

    if (IsOptimized())
    {
        DPF(0, "%s: Cannot get constant buffer interfaces by name since the effect has been Optimize()'ed", pFuncName);
        return &g_InvalidConstantBuffer;
    }

    if (NULL == Name)
    {
        DPF(0, "%s: Parameter Name was NULL.", pFuncName);
        return &g_InvalidConstantBuffer;
    }

    UINT  i;

    for (i = 0; i < m_CBCount; ++ i)
    {
        if (strcmp(m_pCBs[i].pName, Name) == 0)
        {
            return m_pCBs + i;
        }
    }

    DPF(0, "%s: Constant Buffer [%s] not found", pFuncName, Name);
    return &g_InvalidConstantBuffer;
}

ID3DX11EffectVariable * CEffect::GetVariableByIndex(UINT  Index)
{
    LPCSTR pFuncName = "ID3DX11Effect::GetVariableByIndex";

    if (Index < m_VariableCount)
    {
        return m_pVariables + Index;
    }

    DPF(0, "%s: Invalid variable index", pFuncName);
    return &g_InvalidScalarVariable;
}

ID3DX11EffectVariable * CEffect::GetVariableByName(LPCSTR Name)
{
    HRESULT hr = S_OK;
    LPCSTR pFuncName = "ID3DX11Effect::GetVariableByName";

    if (IsOptimized())
    {
        DPF(0, "%s: Cannot get variable interfaces by name since the effect has been Optimize()'ed", pFuncName);
        return &g_InvalidScalarVariable;
    }

    if (NULL == Name)
    {
        DPF(0, "%s: Parameter Name was NULL.", pFuncName);
        return &g_InvalidScalarVariable;
    }

    UINT  i;

    for (i = 0; i < m_VariableCount; ++ i)
    {
        if (strcmp(m_pVariables[i].pName, Name) == 0)
        {
            return m_pVariables + i;
        }
    }

    DPF(0, "%s: Variable [%s] not found", pFuncName, Name);
    return &g_InvalidScalarVariable;
}

ID3DX11EffectVariable * CEffect::GetVariableBySemantic(LPCSTR Semantic)
{    
    LPCSTR pFuncName = "ID3DX11Effect::GetVariableBySemantic";

    if (IsOptimized())
    {
        DPF(0, "%s: Cannot get variable interfaces by semantic since the effect has been Optimize()'ed", pFuncName);
        return &g_InvalidScalarVariable;
    }

    if (NULL == Semantic)
    {
        DPF(0, "%s: Parameter Semantic was NULL.", pFuncName);
        return &g_InvalidScalarVariable;
    }

    UINT  i;

    for (i = 0; i < m_VariableCount; ++ i)
    {
        if (NULL != m_pVariables[i].pSemantic && 
            _stricmp(m_pVariables[i].pSemantic, Semantic) == 0)
        {
            return (ID3DX11EffectVariable *)(m_pVariables + i);
        }
    }

    DPF(0, "%s: Variable with semantic [%s] not found", pFuncName, Semantic);
    return &g_InvalidScalarVariable;
}

ID3DX11EffectTechnique * CEffect::GetTechniqueByIndex(UINT  Index)
{
    LPCSTR pFuncName = "ID3DX11Effect::GetTechniqueByIndex";

    if( Index < m_TechniqueCount )
    {
        UINT i;
        for( i=0; i < m_GroupCount; i++ )
        {
            if( Index < m_pGroups[i].TechniqueCount )
            {
                return (ID3DX11EffectTechnique *)(m_pGroups[i].pTechniques + Index);
            }
            Index -= m_pGroups[i].TechniqueCount;
        }
        D3DXASSERT( FALSE );
    }
    DPF(0, "%s: Invalid technique index (%d)", pFuncName, Index);
    return &g_InvalidTechnique;
}

ID3DX11EffectTechnique * CEffect::GetTechniqueByName(LPCSTR Name)
{
    HRESULT hr = S_OK;
    LPCSTR pFuncName = "ID3DX11Effect::GetTechniqueByName";
    const UINT MAX_GROUP_TECHNIQUE_SIZE = 256;
    char NameCopy[MAX_GROUP_TECHNIQUE_SIZE];

    if (IsOptimized())
    {
        DPF(0, "ID3DX11Effect::GetTechniqueByName: Cannot get technique interfaces by name since the effect has been Optimize()'ed");
        return &g_InvalidTechnique;
    }

    if (NULL == Name)
    {
        DPF(0, "%s: Parameter Name was NULL.", pFuncName);
        return &g_InvalidTechnique;
    }

    if( FAILED( StringCchCopyA( NameCopy, MAX_GROUP_TECHNIQUE_SIZE, Name ) ) )
    {
        DPF( 0, "Group|Technique name has a length greater than %d.", MAX_GROUP_TECHNIQUE_SIZE );
        return &g_InvalidTechnique;
    }

    char* pDelimiter = strchr( NameCopy, '|' );
    if( pDelimiter == NULL )
    {
        if ( m_pNullGroup == NULL )
        {
            DPF( 0, "The effect contains no default group." );
            return &g_InvalidTechnique;
        }

        return m_pNullGroup->GetTechniqueByName( Name );
    }

    // separate group name and technique name
    *pDelimiter = 0; 

    return GetGroupByName( NameCopy )->GetTechniqueByName( pDelimiter + 1 );
}

ID3D11ClassLinkage * CEffect::GetClassLinkage()
{
    SAFE_ADDREF( m_pClassLinkage );
    return m_pClassLinkage;
}

ID3DX11EffectGroup * CEffect::GetGroupByIndex(UINT  Index)
{
    LPCSTR pFuncName = "ID3DX11Effect::GetGroupByIndex";

    if( Index < m_GroupCount )
    {
        return (ID3DX11EffectGroup *)(m_pGroups + Index);
    }
    DPF(0, "%s: Invalid group index (%d)", pFuncName, Index);
    return &g_InvalidGroup;
}

ID3DX11EffectGroup * CEffect::GetGroupByName(LPCSTR Name)
{
    HRESULT hr = S_OK;
    LPCSTR pFuncName = "ID3DX11Effect::GetGroupByName";

    if (IsOptimized())
    {
        DPF(0, "ID3DX11Effect::GetGroupByName: Cannot get group interfaces by name since the effect has been Optimize()'ed");
        return &g_InvalidGroup;
    }

    if (NULL == Name || Name[0] == 0 )
    {
        return m_pNullGroup ? (ID3DX11EffectGroup *)m_pNullGroup : &g_InvalidGroup;
    }

    UINT  i;

    for (i = 0; i < m_GroupCount; ++ i)
    {
        if (NULL != m_pGroups[i].pName && 
            strcmp(m_pGroups[i].pName, Name) == 0)
        {
            break;
        }
    }

    if (i == m_GroupCount)
    {
        DPF(0, "%s: Group [%s] not found", pFuncName, Name);
        return &g_InvalidGroup;
    }

    return (ID3DX11EffectGroup *)(m_pGroups + i);
}

}
