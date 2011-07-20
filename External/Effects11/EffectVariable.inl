//////////////////////////////////////////////////////////////////////////////
//
//  Copyright (C) Microsoft Corporation.  All Rights Reserved.
//
//  File:       EffectVariable.inl
//  Content:    D3DX11 Effects Variable reflection template
//              These templates define the many Effect variable types.
//
//////////////////////////////////////////////////////////////////////////////

//////////////////////////////////////////////////////////////////////////
// Invalid variable forward defines
//////////////////////////////////////////////////////////////////////////

struct SEffectInvalidScalarVariable;
struct SEffectInvalidVectorVariable;
struct SEffectInvalidMatrixVariable;
struct SEffectInvalidStringVariable;
struct SEffectInvalidClassInstanceVariable;
struct SEffectInvalidInterfaceVariable;
struct SEffectInvalidShaderResourceVariable;
struct SEffectInvalidUnorderedAccessViewVariable;
struct SEffectInvalidRenderTargetViewVariable;
struct SEffectInvalidDepthStencilViewVariable;
struct SEffectInvalidConstantBuffer;
struct SEffectInvalidShaderVariable;
struct SEffectInvalidBlendVariable;
struct SEffectInvalidDepthStencilVariable;
struct SEffectInvalidRasterizerVariable;
struct SEffectInvalidSamplerVariable;
struct SEffectInvalidTechnique;
struct SEffectInvalidPass;
struct SEffectInvalidType;

extern SEffectInvalidScalarVariable g_InvalidScalarVariable;
extern SEffectInvalidVectorVariable g_InvalidVectorVariable;
extern SEffectInvalidMatrixVariable g_InvalidMatrixVariable;
extern SEffectInvalidStringVariable g_InvalidStringVariable;
extern SEffectInvalidClassInstanceVariable g_InvalidClassInstanceVariable;
extern SEffectInvalidInterfaceVariable g_InvalidInterfaceVariable;
extern SEffectInvalidShaderResourceVariable g_InvalidShaderResourceVariable;
extern SEffectInvalidUnorderedAccessViewVariable g_InvalidUnorderedAccessViewVariable;
extern SEffectInvalidRenderTargetViewVariable g_InvalidRenderTargetViewVariable;
extern SEffectInvalidDepthStencilViewVariable g_InvalidDepthStencilViewVariable;
extern SEffectInvalidConstantBuffer g_InvalidConstantBuffer;
extern SEffectInvalidShaderVariable g_InvalidShaderVariable;
extern SEffectInvalidBlendVariable g_InvalidBlendVariable;
extern SEffectInvalidDepthStencilVariable g_InvalidDepthStencilVariable;
extern SEffectInvalidRasterizerVariable g_InvalidRasterizerVariable;
extern SEffectInvalidSamplerVariable g_InvalidSamplerVariable;
extern SEffectInvalidTechnique g_InvalidTechnique;
extern SEffectInvalidPass g_InvalidPass;
extern SEffectInvalidType g_InvalidType;

enum ETemplateVarType
{
    ETVT_Bool,
    ETVT_Int,
    ETVT_Float
};

//////////////////////////////////////////////////////////////////////////
// Invalid effect variable struct definitions
//////////////////////////////////////////////////////////////////////////

struct SEffectInvalidType : public ID3DX11EffectType
{
    STDMETHOD_(BOOL, IsValid)() { return FALSE; }
    STDMETHOD(GetDesc)(D3DX11_EFFECT_TYPE_DESC *pDesc) { return E_FAIL; }
    STDMETHOD_(ID3DX11EffectType*, GetMemberTypeByIndex)(UINT Index) { return &g_InvalidType; }
    STDMETHOD_(ID3DX11EffectType*, GetMemberTypeByName)(LPCSTR Name) { return &g_InvalidType; }
    STDMETHOD_(ID3DX11EffectType*, GetMemberTypeBySemantic)(LPCSTR Semanti) { return &g_InvalidType; }
    STDMETHOD_(LPCSTR, GetMemberName)(UINT Index) { return NULL; }
    STDMETHOD_(LPCSTR, GetMemberSemantic)(UINT Index) { return NULL; }
};

template<typename IBaseInterface>
struct TEffectInvalidVariable : public IBaseInterface
{
public:
    STDMETHOD_(BOOL, IsValid)() { return FALSE; }
    STDMETHOD_(ID3DX11EffectType*, GetType)() { return &g_InvalidType; }
    STDMETHOD(GetDesc)(D3DX11_EFFECT_VARIABLE_DESC *pDesc) { return E_FAIL; }

    STDMETHOD_(ID3DX11EffectVariable*, GetAnnotationByIndex)(UINT Index) { return &g_InvalidScalarVariable; }
    STDMETHOD_(ID3DX11EffectVariable*, GetAnnotationByName)(LPCSTR Name) { return &g_InvalidScalarVariable; }

    STDMETHOD_(ID3DX11EffectVariable*, GetMemberByIndex)(UINT Index) { return &g_InvalidScalarVariable; }
    STDMETHOD_(ID3DX11EffectVariable*, GetMemberByName)(LPCSTR Name) { return &g_InvalidScalarVariable; }
    STDMETHOD_(ID3DX11EffectVariable*, GetMemberBySemantic)(LPCSTR Semantic) { return &g_InvalidScalarVariable; }

    STDMETHOD_(ID3DX11EffectVariable*, GetElement)(UINT Index) { return &g_InvalidScalarVariable; }

    STDMETHOD_(ID3DX11EffectConstantBuffer*, GetParentConstantBuffer)() { return &g_InvalidConstantBuffer; }

    STDMETHOD_(ID3DX11EffectScalarVariable*, AsScalar)() { return &g_InvalidScalarVariable; }
    STDMETHOD_(ID3DX11EffectVectorVariable*, AsVector)() { return &g_InvalidVectorVariable; }
    STDMETHOD_(ID3DX11EffectMatrixVariable*, AsMatrix)() { return &g_InvalidMatrixVariable; }
    STDMETHOD_(ID3DX11EffectStringVariable*, AsString)() { return &g_InvalidStringVariable; }
    STDMETHOD_(ID3DX11EffectClassInstanceVariable*, AsClassInstance)() { return &g_InvalidClassInstanceVariable; }
    STDMETHOD_(ID3DX11EffectInterfaceVariable*, AsInterface)() { return &g_InvalidInterfaceVariable; }
    STDMETHOD_(ID3DX11EffectShaderResourceVariable*, AsShaderResource)() { return &g_InvalidShaderResourceVariable; }
    STDMETHOD_(ID3DX11EffectUnorderedAccessViewVariable*, AsUnorderedAccessView)() { return &g_InvalidUnorderedAccessViewVariable; }
    STDMETHOD_(ID3DX11EffectRenderTargetViewVariable*, AsRenderTargetView)() { return &g_InvalidRenderTargetViewVariable; }
    STDMETHOD_(ID3DX11EffectDepthStencilViewVariable*, AsDepthStencilView)() { return &g_InvalidDepthStencilViewVariable; }
    STDMETHOD_(ID3DX11EffectConstantBuffer*, AsConstantBuffer)() { return &g_InvalidConstantBuffer; }
    STDMETHOD_(ID3DX11EffectShaderVariable*, AsShader)() { return &g_InvalidShaderVariable; }
    STDMETHOD_(ID3DX11EffectBlendVariable*, AsBlend)() { return &g_InvalidBlendVariable; }
    STDMETHOD_(ID3DX11EffectDepthStencilVariable*, AsDepthStencil)() { return &g_InvalidDepthStencilVariable; }
    STDMETHOD_(ID3DX11EffectRasterizerVariable*, AsRasterizer)() { return &g_InvalidRasterizerVariable; }
    STDMETHOD_(ID3DX11EffectSamplerVariable*, AsSampler)() { return &g_InvalidSamplerVariable; }

    STDMETHOD(SetRawValue)(CONST void *pData, UINT  Offset, UINT  Count) { return E_FAIL; }
    STDMETHOD(GetRawValue)(void *pData, UINT  Offset, UINT  Count) { return E_FAIL; }
};

struct SEffectInvalidScalarVariable : public TEffectInvalidVariable<ID3DX11EffectScalarVariable>
{
public:

    STDMETHOD(SetFloat)(CONST float Value) { return E_FAIL; }
    STDMETHOD(GetFloat)(float *pValue) { return E_FAIL; }

    STDMETHOD(SetFloatArray)(CONST float *pData, UINT  Offset, UINT  Count) { return E_FAIL; }
    STDMETHOD(GetFloatArray)(float *pData, UINT  Offset, UINT  Count) { return E_FAIL; }

    STDMETHOD(SetInt)(CONST int Value) { return E_FAIL; }
    STDMETHOD(GetInt)(int *pValue) { return E_FAIL; }

    STDMETHOD(SetIntArray)(CONST int *pData, UINT  Offset, UINT  Count) { return E_FAIL; }
    STDMETHOD(GetIntArray)(int *pData, UINT  Offset, UINT  Count) { return E_FAIL; }

    STDMETHOD(SetBool)(CONST BOOL Value) { return E_FAIL; }
    STDMETHOD(GetBool)(BOOL *pValue) { return E_FAIL; }

    STDMETHOD(SetBoolArray)(CONST BOOL *pData, UINT  Offset, UINT  Count) { return E_FAIL; }
    STDMETHOD(GetBoolArray)(BOOL *pData, UINT  Offset, UINT  Count) { return E_FAIL; }
};


struct SEffectInvalidVectorVariable : public TEffectInvalidVariable<ID3DX11EffectVectorVariable>
{
public:
    STDMETHOD(SetFloatVector)(CONST float *pData) { return E_FAIL; };
    STDMETHOD(SetIntVector)(CONST int *pData) { return E_FAIL; };
    STDMETHOD(SetBoolVector)(CONST BOOL *pData) { return E_FAIL; };

    STDMETHOD(GetFloatVector)(float *pData) { return E_FAIL; };
    STDMETHOD(GetIntVector)(int *pData) { return E_FAIL; };
    STDMETHOD(GetBoolVector)(BOOL *pData) { return E_FAIL; };

    STDMETHOD(SetBoolVectorArray) (CONST BOOL *pData, UINT Offset, UINT Count) { return E_FAIL; };
    STDMETHOD(SetIntVectorArray)  (CONST int *pData, UINT Offset, UINT Count) { return E_FAIL; };
    STDMETHOD(SetFloatVectorArray)(CONST float *pData, UINT Offset, UINT Count) { return E_FAIL; };

    STDMETHOD(GetBoolVectorArray) (BOOL *pData, UINT Offset, UINT Count) { return E_FAIL; };
    STDMETHOD(GetIntVectorArray)  (int *pData, UINT Offset, UINT Count) { return E_FAIL; };
    STDMETHOD(GetFloatVectorArray)(float *pData, UINT Offset, UINT Count) { return E_FAIL; };

};

struct SEffectInvalidMatrixVariable : public TEffectInvalidVariable<ID3DX11EffectMatrixVariable>
{
public:

    STDMETHOD(SetMatrix)(CONST float *pData) { return E_FAIL; }
    STDMETHOD(GetMatrix)(float *pData) { return E_FAIL; }

    STDMETHOD(SetMatrixArray)(CONST float *pData, UINT  Offset, UINT  Count) { return E_FAIL; }
    STDMETHOD(GetMatrixArray)(float *pData, UINT  Offset, UINT  Count) { return E_FAIL; }

    STDMETHOD(SetMatrixPointerArray)(CONST float **ppData, UINT  Offset, UINT  Count) { return E_FAIL; }
    STDMETHOD(GetMatrixPointerArray)(float **ppData, UINT  Offset, UINT  Count) { return E_FAIL; }

    STDMETHOD(SetMatrixTranspose)(CONST float *pData) { return E_FAIL; }
    STDMETHOD(GetMatrixTranspose)(float *pData) { return E_FAIL; }

    STDMETHOD(SetMatrixTransposeArray)(CONST float *pData, UINT  Offset, UINT  Count) { return E_FAIL; }
    STDMETHOD(GetMatrixTransposeArray)(float *pData, UINT  Offset, UINT  Count) { return E_FAIL; }

    STDMETHOD(SetMatrixTransposePointerArray)(CONST float **ppData, UINT  Offset, UINT  Count) { return E_FAIL; }
    STDMETHOD(GetMatrixTransposePointerArray)(float **ppData, UINT  Offset, UINT  Count) { return E_FAIL; }
};

struct SEffectInvalidStringVariable : public TEffectInvalidVariable<ID3DX11EffectStringVariable>
{
public:

    STDMETHOD(GetString)(LPCSTR *ppString) { return E_FAIL; }
    STDMETHOD(GetStringArray)(LPCSTR *ppStrings, UINT  Offset, UINT  Count) { return E_FAIL; }
};

struct SEffectInvalidClassInstanceVariable : public TEffectInvalidVariable<ID3DX11EffectClassInstanceVariable>
{
public:

    STDMETHOD(GetClassInstance)(ID3D11ClassInstance **ppClassInstance) { return E_FAIL; }
};


struct SEffectInvalidInterfaceVariable : public TEffectInvalidVariable<ID3DX11EffectInterfaceVariable>
{
public:

    STDMETHOD(SetClassInstance)(ID3DX11EffectClassInstanceVariable *pEffectClassInstance) { return E_FAIL; }
    STDMETHOD(GetClassInstance)(ID3DX11EffectClassInstanceVariable **ppEffectClassInstance) { return E_FAIL; }
};


struct SEffectInvalidShaderResourceVariable : public TEffectInvalidVariable<ID3DX11EffectShaderResourceVariable>
{
public:

    STDMETHOD(SetResource)(ID3D11ShaderResourceView *pResource) { return E_FAIL; }
    STDMETHOD(GetResource)(ID3D11ShaderResourceView **ppResource) { return E_FAIL; }

    STDMETHOD(SetResourceArray)(ID3D11ShaderResourceView **ppResources, UINT  Offset, UINT  Count) { return E_FAIL; }
    STDMETHOD(GetResourceArray)(ID3D11ShaderResourceView **ppResources, UINT  Offset, UINT  Count) { return E_FAIL; }
};


struct SEffectInvalidUnorderedAccessViewVariable : public TEffectInvalidVariable<ID3DX11EffectUnorderedAccessViewVariable>
{
public:

    STDMETHOD(SetUnorderedAccessView)(ID3D11UnorderedAccessView *pResource) { return E_FAIL; }
    STDMETHOD(GetUnorderedAccessView)(ID3D11UnorderedAccessView **ppResource) { return E_FAIL; }

    STDMETHOD(SetUnorderedAccessViewArray)(ID3D11UnorderedAccessView **ppResources, UINT  Offset, UINT  Count) { return E_FAIL; }
    STDMETHOD(GetUnorderedAccessViewArray)(ID3D11UnorderedAccessView **ppResources, UINT  Offset, UINT  Count) { return E_FAIL; }
};


struct SEffectInvalidRenderTargetViewVariable : public TEffectInvalidVariable<ID3DX11EffectRenderTargetViewVariable>
{
public:

    STDMETHOD(SetRenderTarget)(ID3D11RenderTargetView *pResource) { return E_FAIL; }
    STDMETHOD(GetRenderTarget)(ID3D11RenderTargetView **ppResource) { return E_FAIL; }

    STDMETHOD(SetRenderTargetArray)(ID3D11RenderTargetView **ppResources, UINT  Offset, UINT  Count) { return E_FAIL; }
    STDMETHOD(GetRenderTargetArray)(ID3D11RenderTargetView **ppResources, UINT  Offset, UINT  Count) { return E_FAIL; }
};


struct SEffectInvalidDepthStencilViewVariable : public TEffectInvalidVariable<ID3DX11EffectDepthStencilViewVariable>
{
public:

    STDMETHOD(SetDepthStencil)(ID3D11DepthStencilView *pResource) { return E_FAIL; }
    STDMETHOD(GetDepthStencil)(ID3D11DepthStencilView **ppResource) { return E_FAIL; }

    STDMETHOD(SetDepthStencilArray)(ID3D11DepthStencilView **ppResources, UINT  Offset, UINT  Count) { return E_FAIL; }
    STDMETHOD(GetDepthStencilArray)(ID3D11DepthStencilView **ppResources, UINT  Offset, UINT  Count) { return E_FAIL; }
};


struct SEffectInvalidConstantBuffer : public TEffectInvalidVariable<ID3DX11EffectConstantBuffer>
{
public:

    STDMETHOD(SetConstantBuffer)(ID3D11Buffer *pConstantBuffer) { return E_FAIL; }
    STDMETHOD(GetConstantBuffer)(ID3D11Buffer **ppConstantBuffer) { return E_FAIL; }
    STDMETHOD(UndoSetConstantBuffer)() { return E_FAIL; }

    STDMETHOD(SetTextureBuffer)(ID3D11ShaderResourceView *pTextureBuffer) { return E_FAIL; }
    STDMETHOD(GetTextureBuffer)(ID3D11ShaderResourceView **ppTextureBuffer) { return E_FAIL; }
    STDMETHOD(UndoSetTextureBuffer)() { return E_FAIL; }
};

struct SEffectInvalidShaderVariable : public TEffectInvalidVariable<ID3DX11EffectShaderVariable>
{
public:

    STDMETHOD(GetShaderDesc)(UINT ShaderIndex, D3DX11_EFFECT_SHADER_DESC *pDesc) { return E_FAIL; }

    STDMETHOD(GetVertexShader)(UINT ShaderIndex, ID3D11VertexShader **ppVS) { return E_FAIL; }
    STDMETHOD(GetGeometryShader)(UINT ShaderIndex, ID3D11GeometryShader **ppGS) { return E_FAIL; }
    STDMETHOD(GetPixelShader)(UINT ShaderIndex, ID3D11PixelShader **ppPS) { return E_FAIL; }
    STDMETHOD(GetHullShader)(UINT ShaderIndex, ID3D11HullShader **ppPS) { return E_FAIL; }
    STDMETHOD(GetDomainShader)(UINT ShaderIndex, ID3D11DomainShader **ppPS) { return E_FAIL; }
    STDMETHOD(GetComputeShader)(UINT ShaderIndex, ID3D11ComputeShader **ppPS) { return E_FAIL; }

    STDMETHOD(GetInputSignatureElementDesc)(UINT ShaderIndex, UINT Element, D3D11_SIGNATURE_PARAMETER_DESC *pDesc) { return E_FAIL; }
    STDMETHOD(GetOutputSignatureElementDesc)(UINT ShaderIndex, UINT Element, D3D11_SIGNATURE_PARAMETER_DESC *pDesc) { return E_FAIL; }
    STDMETHOD(GetPatchConstantSignatureElementDesc)(UINT ShaderIndex, UINT Element, D3D11_SIGNATURE_PARAMETER_DESC *pDesc) { return E_FAIL; }
};

struct SEffectInvalidBlendVariable : public TEffectInvalidVariable<ID3DX11EffectBlendVariable>
{
public:

    STDMETHOD(GetBlendState)(UINT Index, ID3D11BlendState **ppBlendState) { return E_FAIL; }
    STDMETHOD(SetBlendState)(UINT Index, ID3D11BlendState *pBlendState) { return E_FAIL; }
    STDMETHOD(UndoSetBlendState)(UINT Index) { return E_FAIL; }
    STDMETHOD(GetBackingStore)(UINT Index, D3D11_BLEND_DESC *pBlendDesc) { return E_FAIL; }
};

struct SEffectInvalidDepthStencilVariable : public TEffectInvalidVariable<ID3DX11EffectDepthStencilVariable>
{
public:

    STDMETHOD(GetDepthStencilState)(UINT Index, ID3D11DepthStencilState **ppDepthStencilState) { return E_FAIL; }
    STDMETHOD(SetDepthStencilState)(UINT Index, ID3D11DepthStencilState *pDepthStencilState) { return E_FAIL; }
    STDMETHOD(UndoSetDepthStencilState)(UINT Index) { return E_FAIL; }
    STDMETHOD(GetBackingStore)(UINT Index, D3D11_DEPTH_STENCIL_DESC *pDepthStencilDesc) { return E_FAIL; }
};

struct SEffectInvalidRasterizerVariable : public TEffectInvalidVariable<ID3DX11EffectRasterizerVariable>
{
public:

    STDMETHOD(GetRasterizerState)(UINT Index, ID3D11RasterizerState **ppRasterizerState) { return E_FAIL; }
    STDMETHOD(SetRasterizerState)(UINT Index, ID3D11RasterizerState *pRasterizerState) { return E_FAIL; }
    STDMETHOD(UndoSetRasterizerState)(UINT Index) { return E_FAIL; }
    STDMETHOD(GetBackingStore)(UINT Index, D3D11_RASTERIZER_DESC *pRasterizerDesc) { return E_FAIL; }
};

struct SEffectInvalidSamplerVariable : public TEffectInvalidVariable<ID3DX11EffectSamplerVariable>
{
public:

    STDMETHOD(GetSampler)(UINT Index, ID3D11SamplerState **ppSampler) { return E_FAIL; }
    STDMETHOD(SetSampler)(UINT Index, ID3D11SamplerState *pSampler) { return E_FAIL; }
    STDMETHOD(UndoSetSampler)(UINT Index) { return E_FAIL; }
    STDMETHOD(GetBackingStore)(UINT Index, D3D11_SAMPLER_DESC *pSamplerDesc) { return E_FAIL; }
};

struct SEffectInvalidPass : public ID3DX11EffectPass
{
public:
    STDMETHOD_(BOOL, IsValid)() { return FALSE; }
    STDMETHOD(GetDesc)(D3DX11_PASS_DESC *pDesc) { return E_FAIL; }

    STDMETHOD(GetVertexShaderDesc)(D3DX11_PASS_SHADER_DESC *pDesc) { return E_FAIL; }
    STDMETHOD(GetGeometryShaderDesc)(D3DX11_PASS_SHADER_DESC *pDesc) { return E_FAIL; }
    STDMETHOD(GetPixelShaderDesc)(D3DX11_PASS_SHADER_DESC *pDesc) { return E_FAIL; }
    STDMETHOD(GetHullShaderDesc)(D3DX11_PASS_SHADER_DESC *pDesc) { return E_FAIL; }
    STDMETHOD(GetDomainShaderDesc)(D3DX11_PASS_SHADER_DESC *pDesc) { return E_FAIL; }
    STDMETHOD(GetComputeShaderDesc)(D3DX11_PASS_SHADER_DESC *pDesc) { return E_FAIL; }

    STDMETHOD_(ID3DX11EffectVariable*, GetAnnotationByIndex)(UINT Index) { return &g_InvalidScalarVariable; }
    STDMETHOD_(ID3DX11EffectVariable*, GetAnnotationByName)(LPCSTR Name) { return &g_InvalidScalarVariable; }

    STDMETHOD(Apply)(UINT Flags, ID3D11DeviceContext* pContext) { return E_FAIL; }
    STDMETHOD(ComputeStateBlockMask)(D3DX11_STATE_BLOCK_MASK *pStateBlockMask) { return E_FAIL; }
};

struct SEffectInvalidTechnique : public ID3DX11EffectTechnique
{
public:
    STDMETHOD_(BOOL, IsValid)() { return FALSE; }
    STDMETHOD(GetDesc)(D3DX11_TECHNIQUE_DESC *pDesc) { return E_FAIL; }

    STDMETHOD_(ID3DX11EffectVariable*, GetAnnotationByIndex)(UINT Index) { return &g_InvalidScalarVariable; }
    STDMETHOD_(ID3DX11EffectVariable*, GetAnnotationByName)(LPCSTR Name) { return &g_InvalidScalarVariable; }

    STDMETHOD_(ID3DX11EffectPass*, GetPassByIndex)(UINT Index) { return &g_InvalidPass; }
    STDMETHOD_(ID3DX11EffectPass*, GetPassByName)(LPCSTR Name) { return &g_InvalidPass; }

    STDMETHOD(ComputeStateBlockMask)(D3DX11_STATE_BLOCK_MASK *pStateBlockMask) { return E_FAIL; }
};

struct SEffectInvalidGroup : public ID3DX11EffectGroup
{
public:
    STDMETHOD_(BOOL, IsValid)() { return FALSE; }
    STDMETHOD(GetDesc)(D3DX11_GROUP_DESC *pDesc) { return E_FAIL; }

    STDMETHOD_(ID3DX11EffectVariable*, GetAnnotationByIndex)(UINT Index) { return &g_InvalidScalarVariable; }
    STDMETHOD_(ID3DX11EffectVariable*, GetAnnotationByName)(LPCSTR Name) { return &g_InvalidScalarVariable; }

    STDMETHOD_(ID3DX11EffectTechnique*, GetTechniqueByIndex)(UINT Index) { return &g_InvalidTechnique; }
    STDMETHOD_(ID3DX11EffectTechnique*, GetTechniqueByName)(LPCSTR Name) { return &g_InvalidTechnique; }
};

//////////////////////////////////////////////////////////////////////////
// Helper routines
//////////////////////////////////////////////////////////////////////////

// This is an annoying warning that pops up in retail builds because 
// the code that jumps to "lExit" is conditionally not compiled.
// The only alternative is more #ifdefs in every function
#pragma warning( disable : 4102 ) // 'label' : unreferenced label

#define VERIFYPARAMETER(x) \
{ if (!(x)) { DPF(0, "%s: Parameter " #x " was NULL.", pFuncName); \
    __BREAK_ON_FAIL; hr = E_INVALIDARG; goto lExit; } }

static HRESULT AnnotationInvalidSetCall(LPCSTR pFuncName)
{
    DPF(0, "%s: Annotations are readonly", pFuncName);
    return D3DERR_INVALIDCALL;
}

static HRESULT ObjectSetRawValue()
{
    DPF(0, "ID3DX11EffectVariable::SetRawValue: Objects do not support ths call; please use the specific object accessors instead.");
    return D3DERR_INVALIDCALL;
}

static HRESULT ObjectGetRawValue()
{
    DPF(0, "ID3DX11EffectVariable::GetRawValue: Objects do not support ths call; please use the specific object accessors instead.");
    return D3DERR_INVALIDCALL;
}

ID3DX11EffectConstantBuffer * NoParentCB();

ID3DX11EffectVariable * GetAnnotationByIndexHelper(const char *pClassName, UINT Index, UINT  AnnotationCount, SAnnotation *pAnnotations);

ID3DX11EffectVariable * GetAnnotationByNameHelper(const char *pClassName, LPCSTR Name, UINT  AnnotationCount, SAnnotation *pAnnotations);

template<typename SVarType>
BOOL GetVariableByIndexHelper(UINT Index, UINT  VariableCount, SVarType *pVariables, 
                              BYTE *pBaseAddress, SVarType **ppMember, void **ppDataPtr)
{
    LPCSTR pFuncName = "ID3DX11EffectVariable::GetMemberByIndex";

    if (Index >= VariableCount)
    {
        DPF(0, "%s: Invalid index (%d, total: %d)", pFuncName, Index, VariableCount);
        return FALSE;
    }

    *ppMember = pVariables + Index;
    *ppDataPtr = pBaseAddress + (*ppMember)->Data.Offset;
    return TRUE;
}

template<typename SVarType>
BOOL GetVariableByNameHelper(LPCSTR Name, UINT  VariableCount, SVarType *pVariables, 
                             BYTE *pBaseAddress, SVarType **ppMember, void **ppDataPtr, UINT* pIndex)
{
    LPCSTR pFuncName = "ID3DX11EffectVariable::GetMemberByName";

    if (NULL == Name)
    {
        DPF(0, "%s: Parameter Name was NULL.", pFuncName);
        return FALSE;
    }

    UINT i;
    bool bHasSuper = false;

    for (i = 0; i < VariableCount; ++ i)
    {
        *ppMember = pVariables + i;
        D3DXASSERT(NULL != (*ppMember)->pName);
        if (strcmp((*ppMember)->pName, Name) == 0)
        {
            *ppDataPtr = pBaseAddress + (*ppMember)->Data.Offset;
            *pIndex = i;
            return TRUE;
        }
	else if (i == 0 &&
                 (*ppMember)->pName[0] == '$' &&
                 strcmp((*ppMember)->pName, "$super") == 0)
        {
            bHasSuper = true;
        }
    }

    if (bHasSuper)
    {
        SVarType* pSuper = pVariables;

        return GetVariableByNameHelper<SVarType>(Name,
                                                 pSuper->pType->StructType.Members,
                                                 (SVarType*)pSuper->pType->StructType.pMembers,
                                                 pBaseAddress + pSuper->Data.Offset,
                                                 ppMember,
                                                 ppDataPtr,
                                                 pIndex);
    }

    DPF(0, "%s: Variable [%s] not found", pFuncName, Name);
    return FALSE;
}

template<typename SVarType>
BOOL GetVariableBySemanticHelper(LPCSTR Semantic, UINT  VariableCount, SVarType *pVariables, 
                                 BYTE *pBaseAddress, SVarType **ppMember, void **ppDataPtr, UINT* pIndex)
{
    LPCSTR pFuncName = "ID3DX11EffectVariable::GetMemberBySemantic";

    if (NULL == Semantic)
    {
        DPF(0, "%s: Parameter Semantic was NULL.", pFuncName);
        return FALSE;
    }

    UINT i;

    for (i = 0; i < VariableCount; ++ i)
    {
        *ppMember = pVariables + i;
        if (NULL != (*ppMember)->pSemantic &&
            _stricmp((*ppMember)->pSemantic, Semantic) == 0)
        {
            *ppDataPtr = pBaseAddress + (*ppMember)->Data.Offset;
            *pIndex = i;
            return TRUE;
        }
    }

    DPF(0, "%s: Variable with semantic [%s] not found", pFuncName, Semantic);
    return FALSE;
}

D3DX11INLINE BOOL AreBoundsValid(UINT  Offset, UINT  Count, CONST void *pData, CONST SType *pType, UINT  TotalUnpackedSize)
{
    if (Count == 0) return TRUE;
    UINT  singleElementSize = pType->GetTotalUnpackedSize(TRUE);
    D3DXASSERT(singleElementSize <= pType->Stride);

    return ((Offset + Count >= Offset) &&
        ((Offset + Count) < ((UINT)-1) / pType->Stride) &&
        (Count * pType->Stride + (BYTE*)pData >= (BYTE*)pData) &&
        ((Offset + Count - 1) * pType->Stride + singleElementSize <= TotalUnpackedSize));
}

// Note that the branches in this code is based on template parameters and will be compiled out
template<ETemplateVarType SourceType, ETemplateVarType DestType, typename SRC_TYPE, BOOL ValidatePtr>
__forceinline HRESULT CopyScalarValue(SRC_TYPE SrcValue, void *pDest, const char *pFuncName)
{
    HRESULT hr = S_OK;
#ifdef _DEBUG
    if (ValidatePtr)
        VERIFYPARAMETER(pDest);
#endif

    switch (SourceType)
    {
    case ETVT_Bool:
        switch (DestType)
        {
        case ETVT_Bool:
            *(int*)pDest = (SrcValue != 0) ? -1 : 0;
            break;

        case ETVT_Int:
            *(int*)pDest = SrcValue ? 1 : 0;
            break;

        case ETVT_Float:
            *(float*)pDest = SrcValue ? 1.0f : 0.0f;
            break;

        default:
            D3DXASSERT(0);
        }
        break;


    case ETVT_Int:
        switch (DestType)
        {
        case ETVT_Bool:
            *(int*)pDest = (SrcValue != 0) ? -1 : 0;
            break;

        case ETVT_Int:
            *(int*)pDest = (int) SrcValue;
            break;

        case ETVT_Float:
            *(float*)pDest = (float)(SrcValue);
            break;

        default:
            D3DXASSERT(0);
        }
        break;

    case ETVT_Float:
        switch (DestType)
        {
        case ETVT_Bool:
            *(int*)pDest = (SrcValue != 0.0f) ? -1 : 0;
            break;

        case ETVT_Int:
            *(int*)pDest = (int) (SrcValue);
            break;

        case ETVT_Float:
            *(float*)pDest = (float) SrcValue;
            break;

        default:
            D3DXASSERT(0);
        }
        break;

    default:
        D3DXASSERT(0);
    }

lExit:
    return S_OK;
}

template<ETemplateVarType SourceType, ETemplateVarType DestType, typename SRC_TYPE, typename DEST_TYPE>
D3DX11INLINE HRESULT SetScalarArray(CONST SRC_TYPE *pSrcValues, DEST_TYPE *pDestValues, UINT  Offset, UINT  Count, 
                                    SType *pType, UINT  TotalUnpackedSize, const char *pFuncName)
{
    HRESULT hr = S_OK;

#ifdef _DEBUG    
    VERIFYPARAMETER(pSrcValues);

    if (!AreBoundsValid(Offset, Count, pSrcValues, pType, TotalUnpackedSize))
    {
        DPF(0, "%s: Invalid range specified", pFuncName);
        VH(E_INVALIDARG);
    }
#endif

    UINT i, j, delta = pType->NumericType.IsPackedArray ? 1 : SType::c_ScalarsPerRegister;
    pDestValues += Offset * delta;
    for (i = 0, j = 0; j < Count; i += delta, ++ j)
    {
        // pDestValues[i] = (DEST_TYPE)pSrcValues[j];
        CopyScalarValue<SourceType, DestType, SRC_TYPE, FALSE>(pSrcValues[j], &pDestValues[i], "SetScalarArray");
    }

lExit:
    return hr;
}

template<ETemplateVarType SourceType, ETemplateVarType DestType, typename SRC_TYPE, typename DEST_TYPE>
D3DX11INLINE HRESULT GetScalarArray(SRC_TYPE *pSrcValues, DEST_TYPE *pDestValues, UINT  Offset, UINT  Count, 
                                    SType *pType, UINT  TotalUnpackedSize, const char *pFuncName)
{
    HRESULT hr = S_OK;

#ifdef _DEBUG    
    VERIFYPARAMETER(pDestValues);

    if (!AreBoundsValid(Offset, Count, pDestValues, pType, TotalUnpackedSize))
    {
        DPF(0, "%s: Invalid range specified", pFuncName);
        VH(E_INVALIDARG);
    }
#endif

    UINT i, j, delta = pType->NumericType.IsPackedArray ? 1 : SType::c_ScalarsPerRegister;
    pSrcValues += Offset * delta;
    for (i = 0, j = 0; j < Count; i += delta, ++ j)
    {
        // pDestValues[j] = (DEST_TYPE)pSrcValues[i];
        CopyScalarValue<SourceType, DestType, SRC_TYPE, FALSE>(pSrcValues[i], &pDestValues[j], "GetScalarArray");
    }

lExit:
    return hr;
}


//////////////////////////////////////////////////////////////////////////
// TVariable - implements type casting and member/element retrieval
//////////////////////////////////////////////////////////////////////////

// requires that IBaseInterface contain SVariable's fields and support ID3DX11EffectVariable
template<typename IBaseInterface>
struct TVariable : public IBaseInterface
{
    STDMETHOD_(BOOL, IsValid)() { return TRUE; }

    STDMETHOD_(ID3DX11EffectVariable*, GetMemberByIndex)(UINT Index)
    {
        SVariable *pMember;
        UDataPointer dataPtr;
        TTopLevelVariable<ID3DX11EffectVariable> *pTopLevelEntity = GetTopLevelEntity();

        if (((ID3DX11Effect*)pTopLevelEntity->pEffect)->IsOptimized())
        {
            DPF(0, "ID3DX11EffectVariable::GetMemberByIndex: Cannot get members; effect has been Optimize()'ed");
            return &g_InvalidScalarVariable;
        }

        if (pType->VarType != EVT_Struct)
        {
            DPF(0, "ID3DX11EffectVariable::GetMemberByIndex: Variable is not a structure");
            return &g_InvalidScalarVariable;
        }

        if (!GetVariableByIndexHelper<SVariable>(Index, pType->StructType.Members, pType->StructType.pMembers, 
            Data.pNumeric, &pMember, &dataPtr.pGeneric))
        {
            return &g_InvalidScalarVariable;
        }

        return pTopLevelEntity->pEffect->CreatePooledVariableMemberInterface(pTopLevelEntity, pMember, dataPtr, FALSE, Index);
    }

    STDMETHOD_(ID3DX11EffectVariable*, GetMemberByName)(LPCSTR Name)
    {
        SVariable *pMember;
        UDataPointer dataPtr;
        UINT index;
        TTopLevelVariable<ID3DX11EffectVariable> *pTopLevelEntity = GetTopLevelEntity();

        if (pTopLevelEntity->pEffect->IsOptimized())
        {
            DPF(0, "ID3DX11EffectVariable::GetMemberByName: Cannot get members; effect has been Optimize()'ed");
            return &g_InvalidScalarVariable;
        }

        if (pType->VarType != EVT_Struct)
        {
            DPF(0, "ID3DX11EffectVariable::GetMemberByName: Variable is not a structure");
            return &g_InvalidScalarVariable;
        }

        if (!GetVariableByNameHelper<SVariable>(Name, pType->StructType.Members, pType->StructType.pMembers, 
            Data.pNumeric, &pMember, &dataPtr.pGeneric, &index))
        {
            return &g_InvalidScalarVariable;

        }

        return pTopLevelEntity->pEffect->CreatePooledVariableMemberInterface(pTopLevelEntity, pMember, dataPtr, FALSE, index);
    }

    STDMETHOD_(ID3DX11EffectVariable*, GetMemberBySemantic)(LPCSTR Semantic)
    {
        SVariable *pMember;
        UDataPointer dataPtr;
        UINT index;
        TTopLevelVariable<ID3DX11EffectVariable> *pTopLevelEntity = GetTopLevelEntity();

        if (pTopLevelEntity->pEffect->IsOptimized())
        {
            DPF(0, "ID3DX11EffectVariable::GetMemberBySemantic: Cannot get members; effect has been Optimize()'ed");
            return &g_InvalidScalarVariable;
        }

        if (pType->VarType != EVT_Struct)
        {
            DPF(0, "ID3DX11EffectVariable::GetMemberBySemantic: Variable is not a structure");
            return &g_InvalidScalarVariable;
        }

        if (!GetVariableBySemanticHelper<SVariable>(Semantic, pType->StructType.Members, pType->StructType.pMembers, 
            Data.pNumeric, &pMember, &dataPtr.pGeneric, &index))
        {
            return &g_InvalidScalarVariable;

        }

        return pTopLevelEntity->pEffect->CreatePooledVariableMemberInterface(pTopLevelEntity, pMember, dataPtr, FALSE, index);
    }

    STDMETHOD_(ID3DX11EffectVariable*, GetElement)(UINT Index)
    {
        LPCSTR pFuncName = "ID3DX11EffectVariable::GetElement";
        TTopLevelVariable<ID3DX11EffectVariable> *pTopLevelEntity = GetTopLevelEntity();
        UDataPointer dataPtr;

        if (pTopLevelEntity->pEffect->IsOptimized())
        {
            DPF(0, "ID3DX11EffectVariable::GetElement: Cannot get element; effect has been Optimize()'ed");
            return &g_InvalidScalarVariable;
        }

        if (!IsArray())
        {
            DPF(0, "%s: This interface does not refer to an array", pFuncName);
            return &g_InvalidScalarVariable;
        }

        if (Index >= pType->Elements)
        {
            DPF(0, "%s: Invalid element index (%d, total: %d)", pFuncName, Index, pType->Elements);
            return &g_InvalidScalarVariable;
        }

        if (pType->BelongsInConstantBuffer())
        {
            dataPtr.pGeneric = Data.pNumeric + pType->Stride * Index;
        }
        else
        {
            dataPtr.pGeneric = GetBlockByIndex(pType->VarType, pType->ObjectType, Data.pGeneric, Index);
            if (NULL == dataPtr.pGeneric)
            {
                DPF(0, "%s: Internal error", pFuncName);
                return &g_InvalidScalarVariable;
            }
        }

        return pTopLevelEntity->pEffect->CreatePooledVariableMemberInterface(pTopLevelEntity, (SVariable *) this, dataPtr, TRUE, Index);
    }

    STDMETHOD_(ID3DX11EffectScalarVariable*, AsScalar)()
    {
        LPCSTR pFuncName = "ID3DX11EffectVariable::AsScalar";

        if (pType->VarType != EVT_Numeric || 
            pType->NumericType.NumericLayout != ENL_Scalar)
        {
            DPF(0, "%s: Invalid typecast", pFuncName);
            return &g_InvalidScalarVariable;
        }

        return (ID3DX11EffectScalarVariable *) this;
    }

    STDMETHOD_(ID3DX11EffectVectorVariable*, AsVector)()
    {
        LPCSTR pFuncName = "ID3DX11EffectVariable::AsVector";

        if (pType->VarType != EVT_Numeric || 
            pType->NumericType.NumericLayout != ENL_Vector)
        {
            DPF(0, "%s: Invalid typecast", pFuncName);
            return &g_InvalidVectorVariable;
        }

        return (ID3DX11EffectVectorVariable *) this;
    }

    STDMETHOD_(ID3DX11EffectMatrixVariable*, AsMatrix)()
    {
        LPCSTR pFuncName = "ID3DX11EffectVariable::AsMatrix";

        if (pType->VarType != EVT_Numeric || 
            pType->NumericType.NumericLayout != ENL_Matrix)
        {
            DPF(0, "%s: Invalid typecast", pFuncName);
            return &g_InvalidMatrixVariable;
        }

        return (ID3DX11EffectMatrixVariable *) this;
    }

    STDMETHOD_(ID3DX11EffectStringVariable*, AsString)()
    {
        LPCSTR pFuncName = "ID3DX11EffectVariable::AsString";

        if (!pType->IsObjectType(EOT_String))
        {
            DPF(0, "%s: Invalid typecast", pFuncName);
            return &g_InvalidStringVariable;
        }

        return (ID3DX11EffectStringVariable *) this;
    }

    STDMETHOD_(ID3DX11EffectClassInstanceVariable*, AsClassInstance)()
    {
        LPCSTR pFuncName = "ID3DX11EffectVariable::AsClassInstance";

        if (!pType->IsClassInstance() )
        {
            DPF(0, "%s: Invalid typecast", pFuncName);
            return &g_InvalidClassInstanceVariable;
        }
        else if( pMemberData == NULL )
        {
            DPF(0, "%s: Non-global class instance variables (members of structs or classes) and class instances "
                   "inside tbuffers are not supported.", pFuncName );
            return &g_InvalidClassInstanceVariable;
        }

        return (ID3DX11EffectClassInstanceVariable *) this;
    }

    STDMETHOD_(ID3DX11EffectInterfaceVariable*, AsInterface)()
    {
        LPCSTR pFuncName = "ID3DX11EffectVariable::AsInterface";

        if (!pType->IsInterface())
        {
            DPF(0, "%s: Invalid typecast", pFuncName);
            return &g_InvalidInterfaceVariable;
        }

        return (ID3DX11EffectInterfaceVariable *) this;
    }

    STDMETHOD_(ID3DX11EffectShaderResourceVariable*, AsShaderResource)()
    {
        LPCSTR pFuncName = "ID3DX11EffectVariable::AsShaderResource";

        if (!pType->IsShaderResource())
        {
            DPF(0, "%s: Invalid typecast", pFuncName);
            return &g_InvalidShaderResourceVariable;
        }

        return (ID3DX11EffectShaderResourceVariable *) this;
    }

    STDMETHOD_(ID3DX11EffectUnorderedAccessViewVariable*, AsUnorderedAccessView)()
    {
        LPCSTR pFuncName = "ID3DX11EffectVariable::AsUnorderedAccessView";

        if (!pType->IsUnorderedAccessView())
        {
            DPF(0, "%s: Invalid typecast", pFuncName);
            return &g_InvalidUnorderedAccessViewVariable;
        }

        return (ID3DX11EffectUnorderedAccessViewVariable *) this;
    }

    STDMETHOD_(ID3DX11EffectRenderTargetViewVariable*, AsRenderTargetView)()
    {
        LPCSTR pFuncName = "ID3DX11EffectVariable::AsRenderTargetView";

        if (!pType->IsRenderTargetView())
        {
            DPF(0, "%s: Invalid typecast", pFuncName);
            return &g_InvalidRenderTargetViewVariable;
        }

        return (ID3DX11EffectRenderTargetViewVariable *) this;
    }

    STDMETHOD_(ID3DX11EffectDepthStencilViewVariable*, AsDepthStencilView)()
    {
        LPCSTR pFuncName = "ID3DX11EffectVariable::AsDepthStencilView";

        if (!pType->IsDepthStencilView())
        {
            DPF(0, "%s: Invalid typecast", pFuncName);
            return &g_InvalidDepthStencilViewVariable;
        }

        return (ID3DX11EffectDepthStencilViewVariable *) this;
    }

    STDMETHOD_(ID3DX11EffectConstantBuffer*, AsConstantBuffer)()
    {
        LPCSTR pFuncName = "ID3DX11EffectVariable::AsConstantBuffer";
        DPF(0, "%s: Invalid typecast", pFuncName);
        return &g_InvalidConstantBuffer;
    }

    STDMETHOD_(ID3DX11EffectShaderVariable*, AsShader)()
    {
        LPCSTR pFuncName = "ID3DX11EffectVariable::AsShader";

        if (!pType->IsShader())
        {
            DPF(0, "%s: Invalid typecast", pFuncName);
            return &g_InvalidShaderVariable;
        }

        return (ID3DX11EffectShaderVariable *) this;
    }

    STDMETHOD_(ID3DX11EffectBlendVariable*, AsBlend)()
    {
        LPCSTR pFuncName = "ID3DX11EffectVariable::AsBlend";

        if (!pType->IsObjectType(EOT_Blend))
        {
            DPF(0, "%s: Invalid typecast", pFuncName);
            return &g_InvalidBlendVariable;
        }

        return (ID3DX11EffectBlendVariable *) this;
    }

    STDMETHOD_(ID3DX11EffectDepthStencilVariable*, AsDepthStencil)()
    {
        LPCSTR pFuncName = "ID3DX11EffectVariable::AsDepthStencil";

        if (!pType->IsObjectType(EOT_DepthStencil))
        {
            DPF(0, "%s: Invalid typecast", pFuncName);
            return &g_InvalidDepthStencilVariable;
        }

        return (ID3DX11EffectDepthStencilVariable *) this;
    }

    STDMETHOD_(ID3DX11EffectRasterizerVariable*, AsRasterizer)()
    {
        LPCSTR pFuncName = "ID3DX11EffectVariable::AsRasterizer";

        if (!pType->IsObjectType(EOT_Rasterizer))
        {
            DPF(0, "%s: Invalid typecast", pFuncName);
            return &g_InvalidRasterizerVariable;
        }

        return (ID3DX11EffectRasterizerVariable *) this;
    }

    STDMETHOD_(ID3DX11EffectSamplerVariable*, AsSampler)()
    {
        LPCSTR pFuncName = "ID3DX11EffectVariable::AsSampler";

        if (!pType->IsSampler())
        {
            DPF(0, "%s: Invalid typecast", pFuncName);
            return &g_InvalidSamplerVariable;
        }

        return (ID3DX11EffectSamplerVariable *) this;
    }

    // Numeric variables should override this
    STDMETHOD(SetRawValue)(CONST void *pData, UINT  Offset, UINT  Count) { return ObjectSetRawValue(); }
    STDMETHOD(GetRawValue)(void *pData, UINT  Offset, UINT  Count) { return ObjectGetRawValue(); }
};

//////////////////////////////////////////////////////////////////////////
// TTopLevelVariable - functionality for annotations and global variables
//////////////////////////////////////////////////////////////////////////

template<typename IBaseInterface>
struct TTopLevelVariable : public SVariable, public IBaseInterface
{
    // Required to create member/element variable interfaces
    CEffect *pEffect;

    CEffect* GetEffect()
    {
        return pEffect;
    }

    TTopLevelVariable()
    {
        pEffect = NULL;
    }

    UINT  GetTotalUnpackedSize()
    {
        return ((SType*)pType)->GetTotalUnpackedSize(FALSE);
    }

    STDMETHOD_(ID3DX11EffectType*, GetType)()
    {
        return (ID3DX11EffectType*)(SType*)pType;
    }

    TTopLevelVariable<ID3DX11EffectVariable> * GetTopLevelEntity()
    {
        return (TTopLevelVariable<ID3DX11EffectVariable> *)this;
    }

    BOOL IsArray()
    {
        return (pType->Elements > 0);
    }

};

//////////////////////////////////////////////////////////////////////////
// TMember - functionality for structure/array members of other variables
//////////////////////////////////////////////////////////////////////////

template<typename IBaseInterface>
struct TMember : public SVariable, public IBaseInterface
{
    // Indicates that this is a single element of a containing array
    UINT                                        IsSingleElement : 1;

    // Required to create member/element variable interfaces
    TTopLevelVariable<ID3DX11EffectVariable>    *pTopLevelEntity;

    TMember()
    {
        IsSingleElement = FALSE;
        pTopLevelEntity = NULL;
    }

    CEffect* GetEffect()
    {
        return pTopLevelEntity->pEffect;
    }

    UINT  GetTotalUnpackedSize()
    {
        return pType->GetTotalUnpackedSize(IsSingleElement);
    }

    STDMETHOD_(ID3DX11EffectType*, GetType)()
    {
        if (IsSingleElement)
        {
            return pTopLevelEntity->pEffect->CreatePooledSingleElementTypeInterface( pType );
        }
        else
        {
            return (ID3DX11EffectType*) pType;
        }
    }

    STDMETHOD(GetDesc)(D3DX11_EFFECT_VARIABLE_DESC *pDesc)
    {
        HRESULT hr = S_OK;
        LPCSTR pFuncName = "ID3DX11EffectVariable::GetDesc";

        VERIFYPARAMETER(pDesc != NULL);

        pDesc->Name = pName;
        pDesc->Semantic = pSemantic;
        pDesc->Flags = 0;

        if (pTopLevelEntity->pEffect->IsReflectionData(pTopLevelEntity))
        {
            // Is part of an annotation
            D3DXASSERT(pTopLevelEntity->pEffect->IsReflectionData(Data.pGeneric));
            pDesc->Annotations = 0;
            pDesc->BufferOffset = 0;
            pDesc->Flags |= D3DX11_EFFECT_VARIABLE_ANNOTATION;
        }
        else
        {
            // Is part of a global variable
            D3DXASSERT(pTopLevelEntity->pEffect->IsRuntimeData(pTopLevelEntity));
            if (!pTopLevelEntity->pType->IsObjectType(EOT_String))
            {
                // strings are funny; their data is reflection data, so ignore those
                D3DXASSERT(pTopLevelEntity->pEffect->IsRuntimeData(Data.pGeneric));
            }
            
            pDesc->Annotations = ((TGlobalVariable<ID3DX11Effect>*)pTopLevelEntity)->AnnotationCount;

            SConstantBuffer *pCB = ((TGlobalVariable<ID3DX11Effect>*)pTopLevelEntity)->pCB;

            if (pType->BelongsInConstantBuffer())
            {   
                D3DXASSERT(pCB != NULL);
                UINT_PTR offset = Data.pNumeric - pCB->pBackingStore;
                D3DXASSERT(offset == (UINT)offset);
                pDesc->BufferOffset = (UINT)offset;
                D3DXASSERT(pDesc->BufferOffset >= 0 && pDesc->BufferOffset + GetTotalUnpackedSize() <= pCB->Size);
            }
            else
            {
                D3DXASSERT(pCB == NULL);
                pDesc->BufferOffset = 0;
            }
        }

lExit:
        return hr;
    }

    TTopLevelVariable<ID3DX11EffectVariable> * GetTopLevelEntity()
    {
        return pTopLevelEntity;
    }

    BOOL IsArray()
    {
        return (pType->Elements > 0 && !IsSingleElement);
    }

    STDMETHOD_(ID3DX11EffectVariable*, GetAnnotationByIndex)(UINT Index)
    { return pTopLevelEntity->GetAnnotationByIndex(Index); }
    STDMETHOD_(ID3DX11EffectVariable*, GetAnnotationByName)(LPCSTR Name)
    { return pTopLevelEntity->GetAnnotationByName(Name); }

    STDMETHOD_(ID3DX11EffectConstantBuffer*, GetParentConstantBuffer)()
    { return pTopLevelEntity->GetParentConstantBuffer(); }

    // Annotations should never be able to go down this codepath
    void DirtyVariable()
    {
        // make sure to call the global variable's version of dirty variable
        ((TGlobalVariable<ID3DX11EffectVariable>*)pTopLevelEntity)->DirtyVariable();
    }
};

//////////////////////////////////////////////////////////////////////////
// TAnnotation - functionality for top level annotations
//////////////////////////////////////////////////////////////////////////

template<typename IBaseInterface>
struct TAnnotation : public TVariable<TTopLevelVariable<IBaseInterface> >
{
    STDMETHOD(GetDesc)(D3DX11_EFFECT_VARIABLE_DESC *pDesc)
    {
        HRESULT hr = S_OK;
        LPCSTR pFuncName = "ID3DX11EffectVariable::GetDesc";

        VERIFYPARAMETER(pDesc != NULL);

        pDesc->Name = pName;
        pDesc->Semantic = pSemantic;
        pDesc->Flags = D3DX11_EFFECT_VARIABLE_ANNOTATION;
        pDesc->Annotations = 0;
        pDesc->BufferOffset = 0;
        pDesc->ExplicitBindPoint = 0;

lExit:
        return hr;

    }

    STDMETHOD_(ID3DX11EffectVariable*, GetAnnotationByIndex)(UINT Index)
    {
        LPCSTR pFuncName = "ID3DX11EffectVariable::GetAnnotationByIndex";
        DPF(0, "%s: Only variables may have annotations", pFuncName);
        return &g_InvalidScalarVariable;
    }

    STDMETHOD_(ID3DX11EffectVariable*, GetAnnotationByName)(LPCSTR Name)
    {
        LPCSTR pFuncName = "ID3DX11EffectVariable::GetAnnotationByName";
        DPF(0, "%s: Only variables may have annotations", pFuncName);
        return &g_InvalidScalarVariable;
    }

    STDMETHOD_(ID3DX11EffectConstantBuffer*, GetParentConstantBuffer)()
    { return NoParentCB(); }

    void DirtyVariable()
    {
        D3DXASSERT(0);
    }
};

//////////////////////////////////////////////////////////////////////////
// TGlobalVariable - functionality for top level global variables
//////////////////////////////////////////////////////////////////////////

template<typename IBaseInterface>
struct TGlobalVariable : public TVariable<TTopLevelVariable<IBaseInterface> >
{
    Timer           LastModifiedTime;

    // if numeric, pointer to the constant buffer where this variable lives
    SConstantBuffer *pCB;

    UINT            AnnotationCount;
    SAnnotation     *pAnnotations;

    TGlobalVariable()
    {
        LastModifiedTime = 0;
        pCB = NULL;
        AnnotationCount = 0;
        pAnnotations = NULL;
    }

    STDMETHOD(GetDesc)(D3DX11_EFFECT_VARIABLE_DESC *pDesc)
    {
        HRESULT hr = S_OK;
        LPCSTR pFuncName = "ID3DX11EffectVariable::GetDesc";

        VERIFYPARAMETER(pDesc != NULL);

        pDesc->Name = pName;
        pDesc->Semantic = pSemantic;
        pDesc->Flags = 0;
        pDesc->Annotations = AnnotationCount;

        if (pType->BelongsInConstantBuffer())
        {
            D3DXASSERT(pCB != NULL);
            UINT_PTR offset = Data.pNumeric - pCB->pBackingStore;
            D3DXASSERT(offset == (UINT)offset);
            pDesc->BufferOffset = (UINT)offset;
            D3DXASSERT(pDesc->BufferOffset >= 0 && pDesc->BufferOffset + GetTotalUnpackedSize() <= pCB->Size );
        }
        else
        {
            D3DXASSERT(pCB == NULL);
            pDesc->BufferOffset = 0;
        }

        if (ExplicitBindPoint != -1)
        {
            pDesc->ExplicitBindPoint = ExplicitBindPoint;
            pDesc->Flags |= D3DX11_EFFECT_VARIABLE_EXPLICIT_BIND_POINT;
        }
        else
        {
            pDesc->ExplicitBindPoint = 0;
        }

lExit:
        return hr;
    }

    // these are all well defined for global vars
    STDMETHOD_(ID3DX11EffectVariable*, GetAnnotationByIndex)(UINT Index)
    {
        return GetAnnotationByIndexHelper("ID3DX11EffectVariable", Index, AnnotationCount, pAnnotations);
    }

    STDMETHOD_(ID3DX11EffectVariable*, GetAnnotationByName)(LPCSTR Name)
    {
        return GetAnnotationByNameHelper("ID3DX11EffectVariable", Name, AnnotationCount, pAnnotations);
    }

    STDMETHOD_(ID3DX11EffectConstantBuffer*, GetParentConstantBuffer)()
    { 
        if (NULL != pCB)
        {
            D3DXASSERT(pType->BelongsInConstantBuffer());
            return (ID3DX11EffectConstantBuffer*)pCB; 
        }
        else
        {
            D3DXASSERT(!pType->BelongsInConstantBuffer());
            return &g_InvalidConstantBuffer;
        }
    }

    D3DX11INLINE void DirtyVariable()
    {
        D3DXASSERT(NULL != pCB);
        pCB->IsDirty = TRUE;
        LastModifiedTime = pEffect->GetCurrentTime();
    }

};

//////////////////////////////////////////////////////////////////////////
// TNumericVariable - implements raw set/get functionality
//////////////////////////////////////////////////////////////////////////

// IMPORTANT NOTE: All of these numeric & object aspect classes MUST NOT
// add data members to the base variable classes.  Otherwise type sizes 
// will disagree between object & numeric variables and we cannot eaily 
// create arrays of global variables using SGlobalVariable

// Requires that IBaseInterface have SVariable's members, GetTotalUnpackedSize() and DirtyVariable()
template<typename IBaseInterface, BOOL IsAnnotation>
struct TNumericVariable : public IBaseInterface
{
    STDMETHOD(SetRawValue)(CONST void *pData, UINT  ByteOffset, UINT  ByteCount) 
    {
        if (IsAnnotation)
        {
            return AnnotationInvalidSetCall("ID3DX11EffectVariable::SetRawValue");
        }
        else
        {
            HRESULT hr = S_OK;    

#ifdef _DEBUG
            LPCSTR pFuncName = "ID3DX11EffectVariable::SetRawValue";

            VERIFYPARAMETER(pData);

            if ((ByteOffset + ByteCount < ByteOffset) ||
                (ByteCount + (BYTE*)pData < (BYTE*)pData) ||
                ((ByteOffset + ByteCount) > GetTotalUnpackedSize()))
            {
                // overflow of some kind
                DPF(0, "%s: Invalid range specified", pFuncName);
                VH(E_INVALIDARG);
            }
#endif

            DirtyVariable();
            memcpy(Data.pNumeric + ByteOffset, pData, ByteCount);

lExit:
            return hr;
        }
    }

    STDMETHOD(GetRawValue)(__out_bcount(ByteCount) void *pData, UINT  ByteOffset, UINT  ByteCount)
    {
        HRESULT hr = S_OK;    

#ifdef _DEBUG
        LPCSTR pFuncName = "ID3DX11EffectVariable::GetRawValue";

        VERIFYPARAMETER(pData);

        if ((ByteOffset + ByteCount < ByteOffset) ||
            (ByteCount + (BYTE*)pData < (BYTE*)pData) ||
            ((ByteOffset + ByteCount) > GetTotalUnpackedSize()))
        {
            // overflow of some kind
            DPF(0, "%s: Invalid range specified", pFuncName);
            VH(E_INVALIDARG);
        }
#endif

        memcpy(pData, Data.pNumeric + ByteOffset, ByteCount);

lExit:
        return hr;
    }
};

//////////////////////////////////////////////////////////////////////////
// ID3DX11EffectScalarVariable (TFloatScalarVariable implementation)
//////////////////////////////////////////////////////////////////////////

template<typename IBaseInterface, BOOL IsAnnotation>
struct TFloatScalarVariable : public TNumericVariable<IBaseInterface, IsAnnotation>
{
    STDMETHOD(SetFloat)(CONST float Value);
    STDMETHOD(GetFloat)(float *pValue);

    STDMETHOD(SetFloatArray)(CONST float *pData, UINT  Offset, UINT  Count);
    STDMETHOD(GetFloatArray)(float *pData, UINT  Offset, UINT  Count);

    STDMETHOD(SetInt)(CONST int Value);
    STDMETHOD(GetInt)(int *pValue);

    STDMETHOD(SetIntArray)(CONST int *pData, UINT  Offset, UINT  Count);
    STDMETHOD(GetIntArray)(int *pData, UINT  Offset, UINT  Count);

    STDMETHOD(SetBool)(CONST BOOL Value);
    STDMETHOD(GetBool)(BOOL *pValue);

    STDMETHOD(SetBoolArray)(CONST BOOL *pData, UINT  Offset, UINT  Count);
    STDMETHOD(GetBoolArray)(BOOL *pData, UINT  Offset, UINT  Count);
};

template<typename IBaseInterface, BOOL IsAnnotation>
HRESULT TFloatScalarVariable<IBaseInterface, IsAnnotation>::SetFloat(float Value)
{
    LPCSTR pFuncName = "ID3DX11EffectScalarVariable::SetFloat";
    if (IsAnnotation) return AnnotationInvalidSetCall(pFuncName);
    DirtyVariable();
    return CopyScalarValue<ETVT_Float, ETVT_Float, float, FALSE>(Value, Data.pNumericFloat, pFuncName);
}

template<typename IBaseInterface, BOOL IsAnnotation>
HRESULT TFloatScalarVariable<IBaseInterface, IsAnnotation>::GetFloat(float *pValue)
{
    return CopyScalarValue<ETVT_Float, ETVT_Float, float, TRUE>(*Data.pNumericFloat, pValue, "ID3DX11EffectScalarVariable::GetFloat");
}

template<typename IBaseInterface, BOOL IsAnnotation>
HRESULT TFloatScalarVariable<IBaseInterface, IsAnnotation>::SetFloatArray(CONST float *pData, UINT  Offset, UINT  Count)
{
    LPCSTR pFuncName = "ID3DX11EffectScalarVariable::SetFloatArray";
    if (IsAnnotation) return AnnotationInvalidSetCall(pFuncName);
    DirtyVariable();
    return SetScalarArray<ETVT_Float, ETVT_Float, float, float>(pData, Data.pNumericFloat, Offset, Count, 
        pType, GetTotalUnpackedSize(), pFuncName);
}

template<typename IBaseInterface, BOOL IsAnnotation>
HRESULT TFloatScalarVariable<IBaseInterface, IsAnnotation>::GetFloatArray(float *pData, UINT  Offset, UINT  Count)
{
    return GetScalarArray<ETVT_Float, ETVT_Float, float, float>(Data.pNumericFloat, pData, Offset, Count, 
        pType, GetTotalUnpackedSize(), "ID3DX11EffectScalarVariable::GetFloatArray");
}

template<typename IBaseInterface, BOOL IsAnnotation>
HRESULT TFloatScalarVariable<IBaseInterface, IsAnnotation>::SetInt(CONST int Value)
{
    LPCSTR pFuncName = "ID3DX11EffectScalarVariable::SetInt";
    if (IsAnnotation) return AnnotationInvalidSetCall(pFuncName);
    DirtyVariable();
    return CopyScalarValue<ETVT_Int, ETVT_Float, int, FALSE>(Value, Data.pNumericFloat, pFuncName);
}

template<typename IBaseInterface, BOOL IsAnnotation>
HRESULT TFloatScalarVariable<IBaseInterface, IsAnnotation>::GetInt(int *pValue)
{
    return CopyScalarValue<ETVT_Float, ETVT_Int, float, TRUE>(*Data.pNumericFloat, pValue, "ID3DX11EffectScalarVariable::GetInt");
}

template<typename IBaseInterface, BOOL IsAnnotation>
HRESULT TFloatScalarVariable<IBaseInterface, IsAnnotation>::SetIntArray(CONST int *pData, UINT  Offset, UINT  Count)
{
    LPCSTR pFuncName = "ID3DX11EffectScalarVariable::SetIntArray";
    if (IsAnnotation) return AnnotationInvalidSetCall(pFuncName);
    DirtyVariable();
    return SetScalarArray<ETVT_Int, ETVT_Float, int, float>(pData, Data.pNumericFloat, Offset, Count, 
        pType, GetTotalUnpackedSize(), pFuncName);
}

template<typename IBaseInterface, BOOL IsAnnotation>
HRESULT TFloatScalarVariable<IBaseInterface, IsAnnotation>::GetIntArray(int *pData, UINT  Offset, UINT  Count)
{
    return GetScalarArray<ETVT_Float, ETVT_Int, float, int>(Data.pNumericFloat, pData, Offset, Count, 
        pType, GetTotalUnpackedSize(), "ID3DX11EffectScalarVariable::GetIntArray");
}

template<typename IBaseInterface, BOOL IsAnnotation>
HRESULT TFloatScalarVariable<IBaseInterface, IsAnnotation>::SetBool(CONST BOOL Value)
{
    LPCSTR pFuncName = "ID3DX11EffectScalarVariable::SetBool";
    if (IsAnnotation) return AnnotationInvalidSetCall(pFuncName);
    DirtyVariable();
    return CopyScalarValue<ETVT_Bool, ETVT_Float, BOOL, FALSE>(Value, Data.pNumericFloat, pFuncName);
}

template<typename IBaseInterface, BOOL IsAnnotation>
HRESULT TFloatScalarVariable<IBaseInterface, IsAnnotation>::GetBool(BOOL *pValue)
{
    return CopyScalarValue<ETVT_Float, ETVT_Bool, float, TRUE>(*Data.pNumericFloat, pValue, "ID3DX11EffectScalarVariable::GetBool");
}

template<typename IBaseInterface, BOOL IsAnnotation>
HRESULT TFloatScalarVariable<IBaseInterface, IsAnnotation>::SetBoolArray(CONST BOOL *pData, UINT  Offset, UINT  Count)
{
    LPCSTR pFuncName = "ID3DX11EffectScalarVariable::SetBoolArray";
    if (IsAnnotation) return AnnotationInvalidSetCall(pFuncName);
    DirtyVariable();
    return SetScalarArray<ETVT_Bool, ETVT_Float, BOOL, float>(pData, Data.pNumericFloat, Offset, Count, 
        pType, GetTotalUnpackedSize(), pFuncName);
}

template<typename IBaseInterface, BOOL IsAnnotation>
HRESULT TFloatScalarVariable<IBaseInterface, IsAnnotation>::GetBoolArray(BOOL *pData, UINT  Offset, UINT  Count)
{
    return GetScalarArray<ETVT_Float, ETVT_Bool, float, BOOL>(Data.pNumericFloat, pData, Offset, Count, 
        pType, GetTotalUnpackedSize(), "ID3DX11EffectScalarVariable::GetBoolArray");
}

//////////////////////////////////////////////////////////////////////////
// ID3DX11EffectScalarVariable (TIntScalarVariable implementation)
//////////////////////////////////////////////////////////////////////////

template<typename IBaseInterface, BOOL IsAnnotation>
struct TIntScalarVariable : public TNumericVariable<IBaseInterface, IsAnnotation>
{
    STDMETHOD(SetFloat)(CONST float Value);
    STDMETHOD(GetFloat)(float *pValue);

    STDMETHOD(SetFloatArray)(CONST float *pData, UINT  Offset, UINT  Count);
    STDMETHOD(GetFloatArray)(float *pData, UINT  Offset, UINT  Count);

    STDMETHOD(SetInt)(CONST int Value);
    STDMETHOD(GetInt)(int *pValue);

    STDMETHOD(SetIntArray)(CONST int *pData, UINT  Offset, UINT  Count);
    STDMETHOD(GetIntArray)(int *pData, UINT  Offset, UINT  Count);

    STDMETHOD(SetBool)(CONST BOOL Value);
    STDMETHOD(GetBool)(BOOL *pValue);

    STDMETHOD(SetBoolArray)(CONST BOOL *pData, UINT  Offset, UINT  Count);
    STDMETHOD(GetBoolArray)(BOOL *pData, UINT  Offset, UINT  Count);
};

template<typename IBaseInterface, BOOL IsAnnotation>
HRESULT TIntScalarVariable<IBaseInterface, IsAnnotation>::SetFloat(float Value)
{
    LPCSTR pFuncName = "ID3DX11EffectScalarVariable::SetFloat";
    if (IsAnnotation) return AnnotationInvalidSetCall(pFuncName);
    DirtyVariable();
    return CopyScalarValue<ETVT_Float, ETVT_Int, float, FALSE>(Value, Data.pNumericInt, pFuncName);
}

template<typename IBaseInterface, BOOL IsAnnotation>
HRESULT TIntScalarVariable<IBaseInterface, IsAnnotation>::GetFloat(float *pValue)
{
    return CopyScalarValue<ETVT_Int, ETVT_Float, int, TRUE>(*Data.pNumericInt, pValue, "ID3DX11EffectScalarVariable::GetFloat");
}

template<typename IBaseInterface, BOOL IsAnnotation>
HRESULT TIntScalarVariable<IBaseInterface, IsAnnotation>::SetFloatArray(CONST float *pData, UINT  Offset, UINT  Count)
{
    LPCSTR pFuncName = "ID3DX11EffectScalarVariable::SetFloatArray";
    if (IsAnnotation) return AnnotationInvalidSetCall(pFuncName);
    DirtyVariable();
    return SetScalarArray<ETVT_Float, ETVT_Int, float, int>(pData, Data.pNumericInt, Offset, Count, 
        pType, GetTotalUnpackedSize(), pFuncName);
}

template<typename IBaseInterface, BOOL IsAnnotation>
HRESULT TIntScalarVariable<IBaseInterface, IsAnnotation>::GetFloatArray(float *pData, UINT  Offset, UINT  Count)
{
    return GetScalarArray<ETVT_Int, ETVT_Float, int, float>(Data.pNumericInt, pData, Offset, Count, 
        pType, GetTotalUnpackedSize(), "ID3DX11EffectScalarVariable::GetFloatArray");
}

template<typename IBaseInterface, BOOL IsAnnotation>
HRESULT TIntScalarVariable<IBaseInterface, IsAnnotation>::SetInt(CONST int Value)
{
    LPCSTR pFuncName = "ID3DX11EffectScalarVariable::SetInt";
    if (IsAnnotation) return AnnotationInvalidSetCall(pFuncName);
    DirtyVariable();
    return CopyScalarValue<ETVT_Int, ETVT_Int, int, FALSE>(Value, Data.pNumericInt, pFuncName);
}

template<typename IBaseInterface, BOOL IsAnnotation>
HRESULT TIntScalarVariable<IBaseInterface, IsAnnotation>::GetInt(int *pValue)
{
    return CopyScalarValue<ETVT_Int, ETVT_Int, int, TRUE>(*Data.pNumericInt, pValue, "ID3DX11EffectScalarVariable::GetInt");
}

template<typename IBaseInterface, BOOL IsAnnotation>
HRESULT TIntScalarVariable<IBaseInterface, IsAnnotation>::SetIntArray(CONST int *pData, UINT  Offset, UINT  Count)
{
    LPCSTR pFuncName = "ID3DX11EffectScalarVariable::SetIntArray";
    if (IsAnnotation) return AnnotationInvalidSetCall(pFuncName);
    DirtyVariable();
    return SetScalarArray<ETVT_Int, ETVT_Int, int, int>(pData, Data.pNumericInt, Offset, Count, 
        pType, GetTotalUnpackedSize(), pFuncName);
}

template<typename IBaseInterface, BOOL IsAnnotation>
HRESULT TIntScalarVariable<IBaseInterface, IsAnnotation>::GetIntArray(int *pData, UINT  Offset, UINT  Count)
{
    return GetScalarArray<ETVT_Int, ETVT_Int, int, int>(Data.pNumericInt, pData, Offset, Count, 
        pType, GetTotalUnpackedSize(), "ID3DX11EffectScalarVariable::GetIntArray");
}

template<typename IBaseInterface, BOOL IsAnnotation>
HRESULT TIntScalarVariable<IBaseInterface, IsAnnotation>::SetBool(CONST BOOL Value)
{
    LPCSTR pFuncName = "ID3DX11EffectScalarVariable::SetBool";
    if (IsAnnotation) return AnnotationInvalidSetCall(pFuncName);
    DirtyVariable();
    return CopyScalarValue<ETVT_Bool, ETVT_Int, BOOL, FALSE>(Value, Data.pNumericInt, pFuncName);
}

template<typename IBaseInterface, BOOL IsAnnotation>
HRESULT TIntScalarVariable<IBaseInterface, IsAnnotation>::GetBool(BOOL *pValue)
{
    return CopyScalarValue<ETVT_Int, ETVT_Bool, int, TRUE>(*Data.pNumericInt, pValue, "ID3DX11EffectScalarVariable::GetBool");
}

template<typename IBaseInterface, BOOL IsAnnotation>
HRESULT TIntScalarVariable<IBaseInterface, IsAnnotation>::SetBoolArray(CONST BOOL *pData, UINT  Offset, UINT  Count)
{
    LPCSTR pFuncName = "ID3DX11EffectScalarVariable::SetBoolArray";
    if (IsAnnotation) return AnnotationInvalidSetCall(pFuncName);
    DirtyVariable();
    return SetScalarArray<ETVT_Bool, ETVT_Int, BOOL, int>(pData, Data.pNumericInt, Offset, Count, 
        pType, GetTotalUnpackedSize(), pFuncName);
}

template<typename IBaseInterface, BOOL IsAnnotation>
HRESULT TIntScalarVariable<IBaseInterface, IsAnnotation>::GetBoolArray(BOOL *pData, UINT  Offset, UINT  Count)
{
    return GetScalarArray<ETVT_Int, ETVT_Bool, int, BOOL>(Data.pNumericInt, pData, Offset, Count, 
        pType, GetTotalUnpackedSize(), "ID3DX11EffectScalarVariable::GetBoolArray");
}

//////////////////////////////////////////////////////////////////////////
// ID3DX11EffectScalarVariable (TBoolScalarVariable implementation)
//////////////////////////////////////////////////////////////////////////

template<typename IBaseInterface, BOOL IsAnnotation>
struct TBoolScalarVariable : public TNumericVariable<IBaseInterface, IsAnnotation>
{
    STDMETHOD(SetFloat)(CONST float Value);
    STDMETHOD(GetFloat)(float *pValue);

    STDMETHOD(SetFloatArray)(CONST float *pData, UINT  Offset, UINT  Count);
    STDMETHOD(GetFloatArray)(float *pData, UINT  Offset, UINT  Count);

    STDMETHOD(SetInt)(CONST int Value);
    STDMETHOD(GetInt)(int *pValue);

    STDMETHOD(SetIntArray)(CONST int *pData, UINT  Offset, UINT  Count);
    STDMETHOD(GetIntArray)(int *pData, UINT  Offset, UINT  Count);

    STDMETHOD(SetBool)(CONST BOOL Value);
    STDMETHOD(GetBool)(BOOL *pValue);

    STDMETHOD(SetBoolArray)(CONST BOOL *pData, UINT  Offset, UINT  Count);
    STDMETHOD(GetBoolArray)(BOOL *pData, UINT  Offset, UINT  Count);
};

template<typename IBaseInterface, BOOL IsAnnotation>
HRESULT TBoolScalarVariable<IBaseInterface, IsAnnotation>::SetFloat(float Value)
{
    LPCSTR pFuncName = "ID3DX11EffectScalarVariable::SetFloat";
    if (IsAnnotation) return AnnotationInvalidSetCall(pFuncName);
    DirtyVariable();
    return CopyScalarValue<ETVT_Float, ETVT_Bool, float, FALSE>(Value, Data.pNumericBool, pFuncName);
}

template<typename IBaseInterface, BOOL IsAnnotation>
HRESULT TBoolScalarVariable<IBaseInterface, IsAnnotation>::GetFloat(float *pValue)
{
    return CopyScalarValue<ETVT_Bool, ETVT_Float, BOOL, TRUE>(*Data.pNumericBool, pValue, "ID3DX11EffectScalarVariable::GetFloat");
}

template<typename IBaseInterface, BOOL IsAnnotation>
HRESULT TBoolScalarVariable<IBaseInterface, IsAnnotation>::SetFloatArray(CONST float *pData, UINT  Offset, UINT  Count)
{
    LPCSTR pFuncName = "ID3DX11EffectScalarVariable::SetFloatArray";
    if (IsAnnotation) return AnnotationInvalidSetCall(pFuncName);
    DirtyVariable();
    return SetScalarArray<ETVT_Float, ETVT_Bool, float, BOOL>(pData, Data.pNumericBool, Offset, Count, 
        pType, GetTotalUnpackedSize(), pFuncName);
}

template<typename IBaseInterface, BOOL IsAnnotation>
HRESULT TBoolScalarVariable<IBaseInterface, IsAnnotation>::GetFloatArray(float *pData, UINT  Offset, UINT  Count)
{
    return GetScalarArray<ETVT_Bool, ETVT_Float, BOOL, float>(Data.pNumericBool, pData, Offset, Count, 
        pType, GetTotalUnpackedSize(), "ID3DX11EffectScalarVariable::GetFloatArray");
}

template<typename IBaseInterface, BOOL IsAnnotation>
HRESULT TBoolScalarVariable<IBaseInterface, IsAnnotation>::SetInt(CONST int Value)
{
    LPCSTR pFuncName = "ID3DX11EffectScalarVariable::SetInt";
    if (IsAnnotation) return AnnotationInvalidSetCall(pFuncName);
    DirtyVariable();
    return CopyScalarValue<ETVT_Int, ETVT_Bool, int, FALSE>(Value, Data.pNumericBool, pFuncName);
}

template<typename IBaseInterface, BOOL IsAnnotation>
HRESULT TBoolScalarVariable<IBaseInterface, IsAnnotation>::GetInt(int *pValue)
{
    return CopyScalarValue<ETVT_Bool, ETVT_Int, BOOL, TRUE>(*Data.pNumericBool, pValue, "ID3DX11EffectScalarVariable::GetInt");
}

template<typename IBaseInterface, BOOL IsAnnotation>
HRESULT TBoolScalarVariable<IBaseInterface, IsAnnotation>::SetIntArray(CONST int *pData, UINT  Offset, UINT  Count)
{
    LPCSTR pFuncName = "ID3DX11EffectScalarVariable::SetIntArray";
    if (IsAnnotation) return AnnotationInvalidSetCall(pFuncName);
    DirtyVariable();
    return SetScalarArray<ETVT_Int, ETVT_Bool, int, BOOL>(pData, Data.pNumericBool, Offset, Count, 
        pType, GetTotalUnpackedSize(), pFuncName);
}

template<typename IBaseInterface, BOOL IsAnnotation>
HRESULT TBoolScalarVariable<IBaseInterface, IsAnnotation>::GetIntArray(int *pData, UINT  Offset, UINT  Count)
{
    return GetScalarArray<ETVT_Bool, ETVT_Int, BOOL, int>(Data.pNumericBool, pData, Offset, Count, 
        pType, GetTotalUnpackedSize(), "ID3DX11EffectScalarVariable::GetIntArray");
}

template<typename IBaseInterface, BOOL IsAnnotation>
HRESULT TBoolScalarVariable<IBaseInterface, IsAnnotation>::SetBool(CONST BOOL Value)
{
    LPCSTR pFuncName = "ID3DX11EffectScalarVariable::SetBool";
    if (IsAnnotation) return AnnotationInvalidSetCall(pFuncName);
    DirtyVariable();
    return CopyScalarValue<ETVT_Bool, ETVT_Bool, BOOL, FALSE>(Value, Data.pNumericBool, pFuncName);
}

template<typename IBaseInterface, BOOL IsAnnotation>
HRESULT TBoolScalarVariable<IBaseInterface, IsAnnotation>::GetBool(BOOL *pValue)
{
    return CopyScalarValue<ETVT_Bool, ETVT_Bool, BOOL, TRUE>(*Data.pNumericBool, pValue, "ID3DX11EffectScalarVariable::GetBool");
}

template<typename IBaseInterface, BOOL IsAnnotation>
HRESULT TBoolScalarVariable<IBaseInterface, IsAnnotation>::SetBoolArray(CONST BOOL *pData, UINT  Offset, UINT  Count)
{
    LPCSTR pFuncName = "ID3DX11EffectScalarVariable::SetBoolArray";
    if (IsAnnotation) return AnnotationInvalidSetCall(pFuncName);
    DirtyVariable();
    return SetScalarArray<ETVT_Bool, ETVT_Bool, BOOL, BOOL>(pData, Data.pNumericBool, Offset, Count, 
        pType, GetTotalUnpackedSize(), pFuncName);
}

template<typename IBaseInterface, BOOL IsAnnotation>
HRESULT TBoolScalarVariable<IBaseInterface, IsAnnotation>::GetBoolArray(BOOL *pData, UINT  Offset, UINT  Count)
{
    return GetScalarArray<ETVT_Bool, ETVT_Bool, BOOL, BOOL>(Data.pNumericBool, pData, Offset, Count, 
        pType, GetTotalUnpackedSize(), "ID3DX11EffectScalarVariable::GetBoolArray");
}

//////////////////////////////////////////////////////////////////////////
// ID3DX11EffectVectorVariable (TVectorVariable implementation)
//////////////////////////////////////////////////////////////////////////

template<typename IBaseInterface, BOOL IsAnnotation, ETemplateVarType BaseType >
struct TVectorVariable : public TNumericVariable<IBaseInterface, IsAnnotation>
{
    STDMETHOD(SetBoolVector) (CONST BOOL *pData); 
    STDMETHOD(SetIntVector)  (CONST int *pData);
    STDMETHOD(SetFloatVector)(CONST float *pData);

    STDMETHOD(GetBoolVector) (BOOL *pData); 
    STDMETHOD(GetIntVector)  (int *pData);
    STDMETHOD(GetFloatVector)(float *pData);


    STDMETHOD(SetBoolVectorArray) (CONST BOOL *pData, UINT Offset, UINT Count); 
    STDMETHOD(SetIntVectorArray)  (CONST int *pData, UINT Offset, UINT Count);
    STDMETHOD(SetFloatVectorArray)(CONST float *pData, UINT Offset, UINT Count);

    STDMETHOD(GetBoolVectorArray) (BOOL *pData, UINT Offset, UINT Count); 
    STDMETHOD(GetIntVectorArray)  (int *pData, UINT Offset, UINT Count);
    STDMETHOD(GetFloatVectorArray)(float *pData, UINT Offset, UINT Count);
};

// Note that branches in this code is based on template parameters and will be compiled out
template <ETemplateVarType DestType, ETemplateVarType SourceType>
void __forceinline CopyDataWithTypeConversion(__out_bcount(vecCount * dstVecSize * sizeof(UINT)) void *pDest, CONST void *pSource, UINT dstVecSize, UINT srcVecSize, UINT elementCount, UINT vecCount)
{
    UINT i, j;

    switch (SourceType)
    {
    case ETVT_Bool:
        switch (DestType)
        {
        case ETVT_Bool:
            for (j=0; j<vecCount; j++)
            {
                dwordMemcpy(pDest, pSource, elementCount * SType::c_ScalarSize);

                pDest = ((float*) pDest) + dstVecSize;
                pSource = ((float*) pSource) + srcVecSize;
            }
            break;

        case ETVT_Int:
            for (j=0; j<vecCount; j++)
            {
                for (i=0; i<elementCount; i++)
                    ((int*)pDest)[i] = ((BOOL*)pSource)[i] ? -1 : 0;

                pDest = ((float*) pDest) + dstVecSize;
                pSource = ((float*) pSource) + srcVecSize;
            }
            break;

        case ETVT_Float:
            for (j=0; j<vecCount; j++)
            {
                for (i=0; i<elementCount; i++)
                    ((float*)pDest)[i] = ((BOOL*)pSource)[i] ? -1.0f : 0.0f;

                pDest = ((float*) pDest) + dstVecSize;
                pSource = ((float*) pSource) + srcVecSize;
            }
            break;

        default:
            D3DXASSERT(0);
        }
        break;


    case ETVT_Int:
        switch (DestType)
        {
        case ETVT_Bool:
            for (j=0; j<vecCount; j++)
            {
                for (i=0; i<elementCount; i++)
                    ((int*)pDest)[i] = (((int*)pSource)[i] != 0) ? -1 : 0;

                pDest = ((float*) pDest) + dstVecSize;
                pSource = ((float*) pSource) + srcVecSize;
            }
            break;

        case ETVT_Int:
            for (j=0; j<vecCount; j++)
            {
                dwordMemcpy(pDest, pSource, elementCount * SType::c_ScalarSize);

                pDest = ((float*) pDest) + dstVecSize;
                pSource = ((float*) pSource) + srcVecSize;
            }
            break;

        case ETVT_Float:
            for (j=0; j<vecCount; j++)
            {
                for (i=0; i<elementCount; i++)
                    ((float*)pDest)[i] = (float)(((int*)pSource)[i]);

                pDest = ((float*) pDest) + dstVecSize;
                pSource = ((float*) pSource) + srcVecSize;
            }
            break;

        default:
            D3DXASSERT(0);
        }
        break;

    case ETVT_Float:
        switch (DestType)
        {
        case ETVT_Bool:
            for (j=0; j<vecCount; j++)
            {
                for (i=0; i<elementCount; i++)
                    ((int*)pDest)[i] = (((float*)pSource)[i] != 0.0f) ? -1 : 0;

                pDest = ((float*) pDest) + dstVecSize;
                pSource = ((float*) pSource) + srcVecSize;
            }
            break;

        case ETVT_Int:
            for (j=0; j<vecCount; j++)
            {
                for (i=0; i<elementCount; i++)
                    ((int*)pDest)[i] = (int)((float*)pSource)[i];

                pDest = ((float*) pDest) + dstVecSize;
                pSource = ((float*) pSource) + srcVecSize;
            }
            break;

        case ETVT_Float:
            for (i=0; i<vecCount; i++)
            {
                dwordMemcpy( pDest, pSource, elementCount * SType::c_ScalarSize);

                pDest = ((float*) pDest) + dstVecSize;
                pSource = ((float*) pSource) + srcVecSize;
            }
            break;

        default:
            D3DXASSERT(0);
        }
        break;

    default:
        D3DXASSERT(0);
    }
}

// Float Vector

template<typename IBaseInterface, BOOL IsAnnotation, ETemplateVarType BaseType>
HRESULT TVectorVariable<IBaseInterface, IsAnnotation, BaseType >::SetFloatVector(CONST float *pData)
{
    HRESULT hr = S_OK;
    LPCSTR pFuncName = "ID3DX11EffectVectorVariable::SetFloatVector";

#ifdef _DEBUG
    VERIFYPARAMETER(pData);
#endif

    if (IsAnnotation) return AnnotationInvalidSetCall(pFuncName);
    DirtyVariable();
    CopyDataWithTypeConversion<BaseType, ETVT_Float>(Data.pVector, pData, 4, pType->NumericType.Columns, pType->NumericType.Columns, 1);

lExit:
    return hr;
}

template<typename IBaseInterface, BOOL IsAnnotation, ETemplateVarType BaseType>
HRESULT TVectorVariable<IBaseInterface, IsAnnotation, BaseType>::GetFloatVector(float *pData)
{
    HRESULT hr = S_OK;
    LPCSTR pFuncName = "ID3DX11EffectVectorVariable::GetFloatVector";

#ifdef _DEBUG
    VERIFYPARAMETER(pData);
#endif

    CopyDataWithTypeConversion<ETVT_Float, BaseType>(pData, Data.pVector, pType->NumericType.Columns, 4, pType->NumericType.Columns, 1);

lExit:
    return hr;
}

// Int Vector

template<typename IBaseInterface, BOOL IsAnnotation, ETemplateVarType BaseType>
HRESULT TVectorVariable<IBaseInterface, IsAnnotation, BaseType >::SetIntVector(CONST int *pData)
{
    HRESULT hr = S_OK;
    LPCSTR pFuncName = "ID3DX11EffectVectorVariable::SetIntVector";

#ifdef _DEBUG
    VERIFYPARAMETER(pData);
#endif

    if (IsAnnotation) return AnnotationInvalidSetCall(pFuncName);
    DirtyVariable();
    CopyDataWithTypeConversion<BaseType, ETVT_Int>(Data.pVector, pData, 4, pType->NumericType.Columns, pType->NumericType.Columns, 1);

lExit:
    return hr;
}

template<typename IBaseInterface, BOOL IsAnnotation, ETemplateVarType BaseType>
HRESULT TVectorVariable<IBaseInterface, IsAnnotation, BaseType>::GetIntVector(int *pData)
{
    HRESULT hr = S_OK;
    LPCSTR pFuncName = "ID3DX11EffectVectorVariable::GetIntVector";

#ifdef _DEBUG
    VERIFYPARAMETER(pData);
#endif

    CopyDataWithTypeConversion<ETVT_Int, BaseType>(pData, Data.pVector, pType->NumericType.Columns, 4, pType->NumericType.Columns, 1);

lExit:
    return hr;
}

// Bool Vector

template<typename IBaseInterface, BOOL IsAnnotation, ETemplateVarType BaseType>
HRESULT TVectorVariable<IBaseInterface, IsAnnotation, BaseType >::SetBoolVector(CONST BOOL *pData)
{
    HRESULT hr = S_OK;
    LPCSTR pFuncName = "ID3DX11EffectVectorVariable::SetBoolVector";

#ifdef _DEBUG
    VERIFYPARAMETER(pData);
#endif

    if (IsAnnotation) return AnnotationInvalidSetCall(pFuncName);
    DirtyVariable();
    CopyDataWithTypeConversion<BaseType, ETVT_Bool>(Data.pVector, pData, 4, pType->NumericType.Columns, pType->NumericType.Columns, 1);

lExit:
    return hr;
}

template<typename IBaseInterface, BOOL IsAnnotation, ETemplateVarType BaseType>
HRESULT TVectorVariable<IBaseInterface, IsAnnotation, BaseType>::GetBoolVector(BOOL *pData)
{
    HRESULT hr = S_OK;
    LPCSTR pFuncName = "ID3DX11EffectVectorVariable::GetBoolVector";

#ifdef _DEBUG
    VERIFYPARAMETER(pData);
#endif

    CopyDataWithTypeConversion<ETVT_Bool, BaseType>(pData, Data.pVector, pType->NumericType.Columns, 4, pType->NumericType.Columns, 1);

lExit:
    return hr;
}

// Vector Arrays /////////////////////////////////////////////////////////

template<typename IBaseInterface, BOOL IsAnnotation, ETemplateVarType BaseType>
HRESULT TVectorVariable<IBaseInterface, IsAnnotation, BaseType>::SetFloatVectorArray(CONST float *pData, UINT  Offset, UINT  Count)
{
    HRESULT hr = S_OK;
    LPCSTR pFuncName = "ID3DX11EffectVectorVariable::SetFloatVectorArray";

#ifdef _DEBUG
    if (!AreBoundsValid(Offset, Count, pData, pType, GetTotalUnpackedSize()))
    {
        DPF(0, "%s: Invalid range specified", pFuncName);
        VH(E_INVALIDARG);
    }
#endif

    if (IsAnnotation) return AnnotationInvalidSetCall(pFuncName);
    DirtyVariable();
    // ensure we don't write over the padding at the end of the vector array
    CopyDataWithTypeConversion<BaseType, ETVT_Float>(Data.pVector + Offset, pData, 4, pType->NumericType.Columns, pType->NumericType.Columns, max(min((int)Count, (int)pType->Elements - (int)Offset), 0));

lExit:
    return hr;
}

template<typename IBaseInterface, BOOL IsAnnotation, ETemplateVarType BaseType>
HRESULT TVectorVariable<IBaseInterface, IsAnnotation, BaseType>::GetFloatVectorArray(float *pData, UINT  Offset, UINT  Count)
{
    HRESULT hr = S_OK;
    LPCSTR pFuncName = "ID3DX11EffectVectorVariable::GetFloatVectorArray";

#ifdef _DEBUG
    if (!AreBoundsValid(Offset, Count, pData, pType, GetTotalUnpackedSize()))
    {
        DPF(0, "%s: Invalid range specified", pFuncName);
        VH(E_INVALIDARG);
    }
#endif

    // ensure we don't read past the end of the vector array
    CopyDataWithTypeConversion<ETVT_Float, BaseType>(pData, Data.pVector + Offset, pType->NumericType.Columns, 4, pType->NumericType.Columns, max(min((int)Count, (int)pType->Elements - (int)Offset), 0));

lExit:
    return hr;
}

// int

template<typename IBaseInterface, BOOL IsAnnotation, ETemplateVarType BaseType>
HRESULT TVectorVariable<IBaseInterface, IsAnnotation, BaseType>::SetIntVectorArray(CONST int *pData, UINT  Offset, UINT  Count)
{
    HRESULT hr = S_OK;
    LPCSTR pFuncName = "ID3DX11EffectVectorVariable::SetIntVectorArray";

#ifdef _DEBUG
    if (!AreBoundsValid(Offset, Count, pData, pType, GetTotalUnpackedSize()))
    {
        DPF(0, "%s: Invalid range specified", pFuncName);
        VH(E_INVALIDARG);
    }
#endif

    if (IsAnnotation) return AnnotationInvalidSetCall(pFuncName);
    DirtyVariable();
    // ensure we don't write over the padding at the end of the vector array
    CopyDataWithTypeConversion<BaseType, ETVT_Int>(Data.pVector + Offset, pData, 4, pType->NumericType.Columns, pType->NumericType.Columns, max(min((int)Count, (int)pType->Elements - (int)Offset), 0));

lExit:
    return hr;
}

template<typename IBaseInterface, BOOL IsAnnotation, ETemplateVarType BaseType>
HRESULT TVectorVariable<IBaseInterface, IsAnnotation, BaseType>::GetIntVectorArray(int *pData, UINT  Offset, UINT  Count)
{
    HRESULT hr = S_OK;
    LPCSTR pFuncName = "ID3DX11EffectVectorVariable::GetIntVectorArray";

#ifdef _DEBUG
    if (!AreBoundsValid(Offset, Count, pData, pType, GetTotalUnpackedSize()))
    {
        DPF(0, "%s: Invalid range specified", pFuncName);
        VH(E_INVALIDARG);
    }
#endif

    // ensure we don't read past the end of the vector array
    CopyDataWithTypeConversion<ETVT_Int, BaseType>(pData, Data.pVector + Offset, pType->NumericType.Columns, 4, pType->NumericType.Columns, max(min((int)Count, (int)pType->Elements - (int)Offset), 0));

lExit:
    return hr;
}

// bool

template<typename IBaseInterface, BOOL IsAnnotation, ETemplateVarType BaseType>
HRESULT TVectorVariable<IBaseInterface, IsAnnotation, BaseType>::SetBoolVectorArray(CONST BOOL *pData, UINT  Offset, UINT  Count)
{
    HRESULT hr = S_OK;
    LPCSTR pFuncName = "ID3DX11EffectVectorVariable::SetBoolVectorArray";

#ifdef _DEBUG
    if (!AreBoundsValid(Offset, Count, pData, pType, GetTotalUnpackedSize()))
    {
        DPF(0, "%s: Invalid range specified", pFuncName);
        VH(E_INVALIDARG);
    }
#endif

    if (IsAnnotation) return AnnotationInvalidSetCall(pFuncName);
    DirtyVariable();
    // ensure we don't write over the padding at the end of the vector array
    CopyDataWithTypeConversion<BaseType, ETVT_Bool>(Data.pVector + Offset, pData, 4, pType->NumericType.Columns, pType->NumericType.Columns, max(min((int)Count, (int)pType->Elements - (int)Offset), 0));

lExit:
    return hr;
}

template<typename IBaseInterface, BOOL IsAnnotation, ETemplateVarType BaseType>
HRESULT TVectorVariable<IBaseInterface, IsAnnotation, BaseType>::GetBoolVectorArray(BOOL *pData, UINT  Offset, UINT  Count)
{
    HRESULT hr = S_OK;
    LPCSTR pFuncName = "ID3DX11EffectVectorVariable::GetBoolVectorArray";

#ifdef _DEBUG
    if (!AreBoundsValid(Offset, Count, pData, pType, GetTotalUnpackedSize()))
    {
        DPF(0, "%s: Invalid range specified", pFuncName);
        VH(E_INVALIDARG);
    }
#endif

    // ensure we don't read past the end of the vector array
    CopyDataWithTypeConversion<ETVT_Bool, BaseType>(pData, Data.pVector + Offset, pType->NumericType.Columns, 4, pType->NumericType.Columns, max(min((int)Count, (int)pType->Elements - (int)Offset), 0));

lExit:
    return hr;
}

//////////////////////////////////////////////////////////////////////////
// ID3DX11EffectVector4Variable (TVectorVariable implementation) [OPTIMIZED]
//////////////////////////////////////////////////////////////////////////

template<typename IBaseInterface>
struct TVector4Variable : public TVectorVariable<IBaseInterface, FALSE, ETVT_Float>
{
    STDMETHOD(SetFloatVector)(CONST float *pData);
    STDMETHOD(GetFloatVector)(float *pData);

    STDMETHOD(SetFloatVectorArray)(CONST float *pData, UINT Offset, UINT  Count);
    STDMETHOD(GetFloatVectorArray)(float *pData, UINT Offset, UINT  Count);
};

template<typename IBaseInterface>
HRESULT TVector4Variable<IBaseInterface>::SetFloatVector(CONST float *pData)
{
    HRESULT hr = S_OK;
    LPCSTR pFuncName = "ID3DX11EffectVectorVariable::SetFloatVector";

#ifdef _DEBUG
    VERIFYPARAMETER(pData);
#endif

    DirtyVariable();
    Data.pVector[0] = ((CEffectVector4*) pData)[0];

lExit:
    return hr;
}

template<typename IBaseInterface>
HRESULT TVector4Variable<IBaseInterface>::GetFloatVector(float *pData)
{
    HRESULT hr = S_OK;
    LPCSTR pFuncName = "ID3DX11EffectVectorVariable::GetFloatVector";

#ifdef _DEBUG
    VERIFYPARAMETER(pData);
#endif

    dwordMemcpy(pData, Data.pVector, pType->NumericType.Columns * SType::c_ScalarSize);

lExit:
    return hr;
}

template<typename IBaseInterface>
HRESULT TVector4Variable<IBaseInterface>::SetFloatVectorArray(CONST float *pData, UINT  Offset, UINT  Count)
{
    HRESULT hr = S_OK;
    LPCSTR pFuncName = "ID3DX11EffectVectorVariable::SetFloatVectorArray";

#ifdef _DEBUG
    if (!AreBoundsValid(Offset, Count, pData, pType, GetTotalUnpackedSize()))
    {
        DPF(0, "%s: Invalid range specified", pFuncName);
        VH(E_INVALIDARG);
    }
#endif

    DirtyVariable();
    // ensure we don't write over the padding at the end of the vector array
    dwordMemcpy(Data.pVector + Offset, pData, min((Offset + Count) * sizeof(CEffectVector4), pType->TotalSize));

lExit:
    return hr;
}

template<typename IBaseInterface>
HRESULT TVector4Variable<IBaseInterface>::GetFloatVectorArray(float *pData, UINT  Offset, UINT  Count)
{
    HRESULT hr = S_OK;
    LPCSTR pFuncName = "ID3DX11EffectVectorVariable::GetFloatVectorArray";

#ifdef _DEBUG
    if (!AreBoundsValid(Offset, Count, pData, pType, GetTotalUnpackedSize()))
    {
        DPF(0, "%s: Invalid range specified", pFuncName);
        VH(E_INVALIDARG);
    }
#endif

    // ensure we don't read past the end of the vector array
    dwordMemcpy(pData, Data.pVector + Offset, min((Offset + Count) * sizeof(CEffectVector4), pType->TotalSize));

lExit:
    return hr;
}


//////////////////////////////////////////////////////////////////////////
// ID3DX11EffectMatrixVariable (TMatrixVariable implementation)
//////////////////////////////////////////////////////////////////////////

template<typename IBaseInterface, BOOL IsAnnotation>
struct TMatrixVariable : public TNumericVariable<IBaseInterface, IsAnnotation>
{
    STDMETHOD(SetMatrix)(CONST float *pData);
    STDMETHOD(GetMatrix)(float *pData);

    STDMETHOD(SetMatrixArray)(CONST float *pData, UINT  Offset, UINT  Count);
    STDMETHOD(GetMatrixArray)(float *pData, UINT  Offset, UINT  Count);

    STDMETHOD(SetMatrixPointerArray)(CONST float **ppData, UINT  Offset, UINT  Count);
    STDMETHOD(GetMatrixPointerArray)(float **ppData, UINT  Offset, UINT  Count);

    STDMETHOD(SetMatrixTranspose)(CONST float *pData);
    STDMETHOD(GetMatrixTranspose)(float *pData);

    STDMETHOD(SetMatrixTransposeArray)(CONST float *pData, UINT  Offset, UINT  Count);
    STDMETHOD(GetMatrixTransposeArray)(float *pData, UINT  Offset, UINT  Count);

    STDMETHOD(SetMatrixTransposePointerArray)(CONST float **ppData, UINT  Offset, UINT  Count);
    STDMETHOD(GetMatrixTransposePointerArray)(float **ppData, UINT  Offset, UINT  Count);
};

template<BOOL Transpose>
static void SetMatrixTransposeHelper(SType *pType, __out_bcount(64) BYTE *pDestData, CONST float* pMatrix)
{
    UINT i, j;
    UINT registers, entries;
    
    if (Transpose)
    {
        // row major
        registers = pType->NumericType.Rows;
        entries = pType->NumericType.Columns;
    }
    else
    {
        // column major
        registers = pType->NumericType.Columns;
        entries = pType->NumericType.Rows;
    }
    __analysis_assume( registers <= 4 );
    __analysis_assume( entries <= 4 );

    for (i = 0; i < registers; ++ i)
    {
        for (j = 0; j < entries; ++ j)
        {
#pragma prefast(suppress:__WARNING_UNRELATED_LOOP_TERMINATION, "regs / entries <= 4")
            ((float*)pDestData)[j] = ((float*)pMatrix)[j * 4 + i];
        }
        pDestData += SType::c_RegisterSize;
    }
}

template<BOOL Transpose>
static void GetMatrixTransposeHelper(SType *pType, __in_bcount(64) BYTE *pSrcData, __out_ecount(16) float* pMatrix)
{
    UINT i, j;
    UINT registers, entries;

    if (Transpose)
    {
        // row major
        registers = pType->NumericType.Rows;
        entries = pType->NumericType.Columns;
    }
    else
    {
        // column major
        registers = pType->NumericType.Columns;
        entries = pType->NumericType.Rows;
    }
    __analysis_assume( registers <= 4 );
    __analysis_assume( entries <= 4 );

    for (i = 0; i < registers; ++ i)
    {
        for (j = 0; j < entries; ++ j)
        {
            ((float*)pMatrix)[j * 4 + i] = ((float*)pSrcData)[j];
        }
        pSrcData += SType::c_RegisterSize;
    }
}

template<BOOL Transpose, BOOL IsSetting, BOOL ExtraIndirection>
HRESULT DoMatrixArrayInternal(SType *pType, UINT  TotalUnpackedSize, BYTE *pEffectData, void *pMatrixData, UINT  Offset, UINT  Count, LPCSTR pFuncName)
{    
    HRESULT hr = S_OK;

#ifdef _DEBUG
    if (!AreBoundsValid(Offset, Count, pMatrixData, pType, TotalUnpackedSize))
    {
        DPF(0, "%s: Invalid range specified", pFuncName);
        VH(E_INVALIDARG);
    }
#endif

    UINT i;

    if ((pType->NumericType.IsColumnMajor && Transpose) || (!pType->NumericType.IsColumnMajor && !Transpose))
    {
        // fast path
        UINT  dataSize;
        if (Transpose)
        {
            dataSize = ((pType->NumericType.Columns - 1) * 4 + pType->NumericType.Rows) * SType::c_ScalarSize;
        }
        else
        {
            dataSize = ((pType->NumericType.Rows - 1) * 4 + pType->NumericType.Columns) * SType::c_ScalarSize;
        }

        for (i = 0; i < Count; ++ i)
        {
            CEffectMatrix *pMatrix;
            if (ExtraIndirection)
            {
                pMatrix = ((CEffectMatrix **)pMatrixData)[i];
                if (!pMatrix)
                {
                    continue;
                }
            }
            else
            {
                pMatrix = ((CEffectMatrix *)pMatrixData) + i;
            }

            if (IsSetting)
            {
                dwordMemcpy(pEffectData + pType->Stride * (i + Offset), pMatrix, dataSize);
            }
            else
            {
                dwordMemcpy(pMatrix, pEffectData + pType->Stride * (i + Offset), dataSize);
            }
        }
    }
    else
    {
        // slow path
        for (i = 0; i < Count; ++ i)
        {
            CEffectMatrix *pMatrix;
            if (ExtraIndirection)
            {
                pMatrix = ((CEffectMatrix **)pMatrixData)[i];
                if (!pMatrix)
                {
                    continue;
                }
            }
            else
            {
                pMatrix = ((CEffectMatrix *)pMatrixData) + i;
            }

            if (IsSetting)
            {
                SetMatrixTransposeHelper<Transpose>(pType, pEffectData + pType->Stride * (i + Offset), (float*) pMatrix);
            }
            else
            {
                GetMatrixTransposeHelper<Transpose>(pType, pEffectData + pType->Stride * (i + Offset), (float*) pMatrix);
            }
        }
    }

lExit:
    return hr;
}

template<typename IBaseInterface, BOOL IsAnnotation>
HRESULT TMatrixVariable<IBaseInterface, IsAnnotation>::SetMatrix(CONST float *pData)
{
    LPCSTR pFuncName = "ID3DX11EffectMatrixVariable::SetMatrix";
    if (IsAnnotation) return AnnotationInvalidSetCall(pFuncName);
    DirtyVariable();
    return DoMatrixArrayInternal<FALSE, TRUE, FALSE>(pType, GetTotalUnpackedSize(), 
        Data.pNumeric, const_cast<float*>(pData), 0, 1, pFuncName);
}

template<typename IBaseInterface, BOOL IsAnnotation>
HRESULT TMatrixVariable<IBaseInterface, IsAnnotation>::GetMatrix(float *pData)
{
    return DoMatrixArrayInternal<FALSE, FALSE, FALSE>(pType, GetTotalUnpackedSize(), 
        Data.pNumeric, pData, 0, 1, "ID3DX11EffectMatrixVariable::GetMatrix");
}

template<typename IBaseInterface, BOOL IsAnnotation>
HRESULT TMatrixVariable<IBaseInterface, IsAnnotation>::SetMatrixArray(CONST float *pData, UINT  Offset, UINT  Count)
{
    LPCSTR pFuncName = "ID3DX11EffectMatrixVariable::SetMatrixArray";
    if (IsAnnotation) return AnnotationInvalidSetCall(pFuncName);
    DirtyVariable();
    return DoMatrixArrayInternal<FALSE, TRUE, FALSE>(pType, GetTotalUnpackedSize(), 
        Data.pNumeric, const_cast<float*>(pData), Offset, Count, "ID3DX11EffectMatrixVariable::SetMatrixArray");
}

template<typename IBaseInterface, BOOL IsAnnotation>
HRESULT TMatrixVariable<IBaseInterface, IsAnnotation>::GetMatrixArray(float *pData, UINT  Offset, UINT  Count)
{
    return DoMatrixArrayInternal<FALSE, FALSE, FALSE>(pType, GetTotalUnpackedSize(), 
        Data.pNumeric, pData, Offset, Count, "ID3DX11EffectMatrixVariable::GetMatrixArray");
}

template<typename IBaseInterface, BOOL IsAnnotation>
HRESULT TMatrixVariable<IBaseInterface, IsAnnotation>::SetMatrixPointerArray(CONST float **ppData, UINT  Offset, UINT  Count)
{
    LPCSTR pFuncName = "ID3DX11EffectMatrixVariable::SetMatrixPointerArray";
    if (IsAnnotation) return AnnotationInvalidSetCall(pFuncName);
    DirtyVariable();
    return DoMatrixArrayInternal<FALSE, TRUE, TRUE>(pType, GetTotalUnpackedSize(), 
        Data.pNumeric, const_cast<float**>(ppData), Offset, Count, "ID3DX11EffectMatrixVariable::SetMatrixPointerArray");
}

template<typename IBaseInterface, BOOL IsAnnotation>
HRESULT TMatrixVariable<IBaseInterface, IsAnnotation>::GetMatrixPointerArray(float **ppData, UINT  Offset, UINT  Count)
{
    return DoMatrixArrayInternal<FALSE, FALSE, TRUE>(pType, GetTotalUnpackedSize(), 
        Data.pNumeric, ppData, Offset, Count, "ID3DX11EffectMatrixVariable::GetMatrixPointerArray");
}

template<typename IBaseInterface, BOOL IsAnnotation>
HRESULT TMatrixVariable<IBaseInterface, IsAnnotation>::SetMatrixTranspose(CONST float *pData)
{
    LPCSTR pFuncName = "ID3DX11EffectMatrixVariable::SetMatrixTranspose";
    if (IsAnnotation) return AnnotationInvalidSetCall(pFuncName);
    DirtyVariable();
    return DoMatrixArrayInternal<TRUE, TRUE, FALSE>(pType, GetTotalUnpackedSize(), 
        Data.pNumeric, const_cast<float*>(pData), 0, 1, "ID3DX11EffectMatrixVariable::SetMatrixTranspose");
}

template<typename IBaseInterface, BOOL IsAnnotation>
HRESULT TMatrixVariable<IBaseInterface, IsAnnotation>::GetMatrixTranspose(float *pData)
{
    return DoMatrixArrayInternal<TRUE, FALSE, FALSE>(pType, GetTotalUnpackedSize(), 
        Data.pNumeric, pData, 0, 1, "ID3DX11EffectMatrixVariable::GetMatrixTranspose");
}

template<typename IBaseInterface, BOOL IsAnnotation>
HRESULT TMatrixVariable<IBaseInterface, IsAnnotation>::SetMatrixTransposeArray(CONST float *pData, UINT  Offset, UINT  Count)
{
    LPCSTR pFuncName = "ID3DX11EffectMatrixVariable::SetMatrixTransposeArray";
    if (IsAnnotation) return AnnotationInvalidSetCall(pFuncName);
    DirtyVariable();
    return DoMatrixArrayInternal<TRUE, TRUE, FALSE>(pType, GetTotalUnpackedSize(), 
        Data.pNumeric, const_cast<float*>(pData), Offset, Count, "ID3DX11EffectMatrixVariable::SetMatrixTransposeArray");
}

template<typename IBaseInterface, BOOL IsAnnotation>
HRESULT TMatrixVariable<IBaseInterface, IsAnnotation>::GetMatrixTransposeArray(float *pData, UINT  Offset, UINT  Count)
{
    return DoMatrixArrayInternal<TRUE, FALSE, FALSE>(pType, GetTotalUnpackedSize(), 
        Data.pNumeric, pData, Offset, Count, "ID3DX11EffectMatrixVariable::GetMatrixTransposeArray");
}

template<typename IBaseInterface, BOOL IsAnnotation>
HRESULT TMatrixVariable<IBaseInterface, IsAnnotation>::SetMatrixTransposePointerArray(CONST float **ppData, UINT  Offset, UINT  Count)
{
    LPCSTR pFuncName = "ID3DX11EffectMatrixVariable::SetMatrixTransposePointerArray";
    if (IsAnnotation) return AnnotationInvalidSetCall(pFuncName);
    DirtyVariable();
    return DoMatrixArrayInternal<TRUE, TRUE, TRUE>(pType, GetTotalUnpackedSize(), 
        Data.pNumeric, const_cast<float**>(ppData), Offset, Count, "ID3DX11EffectMatrixVariable::SetMatrixTransposePointerArray");
}

template<typename IBaseInterface, BOOL IsAnnotation>
HRESULT TMatrixVariable<IBaseInterface, IsAnnotation>::GetMatrixTransposePointerArray(float **ppData, UINT  Offset, UINT  Count)
{
    return DoMatrixArrayInternal<TRUE, FALSE, TRUE>(pType, GetTotalUnpackedSize(), 
        Data.pNumeric, ppData, Offset, Count, "ID3DX11EffectMatrixVariable::GetMatrixTransposePointerArray");
}

// Optimize commonly used fast paths
// (non-annotations only!)
template<typename IBaseInterface, BOOL IsColumnMajor>
struct TMatrix4x4Variable : public TMatrixVariable<IBaseInterface, FALSE>
{
    STDMETHOD(SetMatrix)(CONST float *pData);
    STDMETHOD(GetMatrix)(float *pData);

    STDMETHOD(SetMatrixArray)(CONST float *pData, UINT  Offset, UINT  Count);
    STDMETHOD(GetMatrixArray)(float *pData, UINT  Offset, UINT  Count);

    STDMETHOD(SetMatrixTranspose)(CONST float *pData);
    STDMETHOD(GetMatrixTranspose)(float *pData);

    STDMETHOD(SetMatrixTransposeArray)(CONST float *pData, UINT  Offset, UINT  Count);
    STDMETHOD(GetMatrixTransposeArray)(float *pData, UINT  Offset, UINT  Count);
};

D3DX11INLINE static void Matrix4x4TransposeHelper(CONST void *pSrc, void *pDst)
{
    BYTE *pDestData = (BYTE*)pDst;
    UINT *pMatrix = (UINT*)pSrc;

    ((UINT*)pDestData)[0 * 4 + 0] = pMatrix[0 * 4 + 0];
    ((UINT*)pDestData)[0 * 4 + 1] = pMatrix[1 * 4 + 0];
    ((UINT*)pDestData)[0 * 4 + 2] = pMatrix[2 * 4 + 0];
    ((UINT*)pDestData)[0 * 4 + 3] = pMatrix[3 * 4 + 0];

    ((UINT*)pDestData)[1 * 4 + 0] = pMatrix[0 * 4 + 1];
    ((UINT*)pDestData)[1 * 4 + 1] = pMatrix[1 * 4 + 1];
    ((UINT*)pDestData)[1 * 4 + 2] = pMatrix[2 * 4 + 1];
    ((UINT*)pDestData)[1 * 4 + 3] = pMatrix[3 * 4 + 1];

    ((UINT*)pDestData)[2 * 4 + 0] = pMatrix[0 * 4 + 2];
    ((UINT*)pDestData)[2 * 4 + 1] = pMatrix[1 * 4 + 2];
    ((UINT*)pDestData)[2 * 4 + 2] = pMatrix[2 * 4 + 2];
    ((UINT*)pDestData)[2 * 4 + 3] = pMatrix[3 * 4 + 2];

    ((UINT*)pDestData)[3 * 4 + 0] = pMatrix[0 * 4 + 3];
    ((UINT*)pDestData)[3 * 4 + 1] = pMatrix[1 * 4 + 3];
    ((UINT*)pDestData)[3 * 4 + 2] = pMatrix[2 * 4 + 3];
    ((UINT*)pDestData)[3 * 4 + 3] = pMatrix[3 * 4 + 3];
}

D3DX11INLINE static void Matrix4x4Copy(CONST void *pSrc, void *pDst)
{
#if 1
    // In tests, this path ended up generating faster code both on x86 and x64
    // T1 - Matrix4x4Copy - this path
    // T2 - Matrix4x4Transpose
    // T1: 1.88 T2: 1.92 - with 32 bit copies
    // T1: 1.85 T2: 1.80 - with 64 bit copies

    UINT64 *pDestData = (UINT64*)pDst;
    UINT64 *pMatrix = (UINT64*)pSrc;

    pDestData[0 * 4 + 0] = pMatrix[0 * 4 + 0];
    pDestData[0 * 4 + 1] = pMatrix[0 * 4 + 1];
    pDestData[0 * 4 + 2] = pMatrix[0 * 4 + 2];
    pDestData[0 * 4 + 3] = pMatrix[0 * 4 + 3];

    pDestData[1 * 4 + 0] = pMatrix[1 * 4 + 0];
    pDestData[1 * 4 + 1] = pMatrix[1 * 4 + 1];
    pDestData[1 * 4 + 2] = pMatrix[1 * 4 + 2];
    pDestData[1 * 4 + 3] = pMatrix[1 * 4 + 3];
#else
    UINT *pDestData = (UINT*)pDst;
    UINT *pMatrix = (UINT*)pSrc;

    pDestData[0 * 4 + 0] = pMatrix[0 * 4 + 0];
    pDestData[0 * 4 + 1] = pMatrix[0 * 4 + 1];
    pDestData[0 * 4 + 2] = pMatrix[0 * 4 + 2];
    pDestData[0 * 4 + 3] = pMatrix[0 * 4 + 3];

    pDestData[1 * 4 + 0] = pMatrix[1 * 4 + 0];
    pDestData[1 * 4 + 1] = pMatrix[1 * 4 + 1];
    pDestData[1 * 4 + 2] = pMatrix[1 * 4 + 2];
    pDestData[1 * 4 + 3] = pMatrix[1 * 4 + 3];

    pDestData[2 * 4 + 0] = pMatrix[2 * 4 + 0];
    pDestData[2 * 4 + 1] = pMatrix[2 * 4 + 1];
    pDestData[2 * 4 + 2] = pMatrix[2 * 4 + 2];
    pDestData[2 * 4 + 3] = pMatrix[2 * 4 + 3];

    pDestData[3 * 4 + 0] = pMatrix[3 * 4 + 0];
    pDestData[3 * 4 + 1] = pMatrix[3 * 4 + 1];
    pDestData[3 * 4 + 2] = pMatrix[3 * 4 + 2];
    pDestData[3 * 4 + 3] = pMatrix[3 * 4 + 3];
#endif
}


// Note that branches in this code is based on template parameters and will be compiled out
template<BOOL IsColumnMajor, BOOL Transpose, BOOL IsSetting>
D3DX11INLINE HRESULT DoMatrix4x4ArrayInternal(BYTE *pEffectData, void *pMatrixData, UINT  Offset, UINT  Count

#ifdef _DEBUG
                                              , SType *pType, UINT  TotalUnpackedSize, LPCSTR pFuncName)
#else
                                              )
#endif
{    
    HRESULT hr = S_OK;

#ifdef _DEBUG
    if (!AreBoundsValid(Offset, Count, pMatrixData, pType, TotalUnpackedSize))
    {
        DPF(0, "%s: Invalid range specified", pFuncName);
        VH(E_INVALIDARG);
    }

    D3DXASSERT(pType->NumericType.IsColumnMajor == IsColumnMajor && pType->Stride == (4 * SType::c_RegisterSize));
#endif

    UINT i;

    if ((IsColumnMajor && Transpose) || (!IsColumnMajor && !Transpose))
    {
        // fast path
        for (i = 0; i < Count; ++ i)
        {
            CEffectMatrix *pMatrix = ((CEffectMatrix *)pMatrixData) + i;

            if (IsSetting)
            {
                Matrix4x4Copy(pMatrix, pEffectData + 4 * SType::c_RegisterSize * (i + Offset));
            }
            else
            {
                Matrix4x4Copy(pEffectData + 4 * SType::c_RegisterSize * (i + Offset), pMatrix);
            }
        }
    }
    else
    {
        // slow path
        for (i = 0; i < Count; ++ i)
        {
            CEffectMatrix *pMatrix = ((CEffectMatrix *)pMatrixData) + i;

            if (IsSetting)
            {
                Matrix4x4TransposeHelper((float*) pMatrix, pEffectData + 4 * SType::c_RegisterSize * (i + Offset));
            }
            else
            {
                Matrix4x4TransposeHelper(pEffectData + 4 * SType::c_RegisterSize * (i + Offset), (float*) pMatrix);
            }
        }
    }

lExit:
    return hr;
}

template<typename IBaseInterface, BOOL IsColumnMajor>
HRESULT TMatrix4x4Variable<IBaseInterface, IsColumnMajor>::SetMatrix(CONST float *pData)
{
    DirtyVariable();
    return DoMatrix4x4ArrayInternal<IsColumnMajor, FALSE, TRUE>(Data.pNumeric, const_cast<float*>(pData), 0, 1
#ifdef _DEBUG 
        , pType, GetTotalUnpackedSize(), "ID3DX11EffectMatrixVariable::SetMatrix");
#else
        );
#endif
}

template<typename IBaseInterface, BOOL IsColumnMajor>
HRESULT TMatrix4x4Variable<IBaseInterface, IsColumnMajor>::GetMatrix(float *pData)
{
    return DoMatrix4x4ArrayInternal<IsColumnMajor, FALSE, FALSE>(Data.pNumeric, pData, 0, 1
#ifdef _DEBUG 
        , pType, GetTotalUnpackedSize(), "ID3DX11EffectMatrixVariable::GetMatrix");
#else
        );
#endif
}

template<typename IBaseInterface, BOOL IsColumnMajor>
HRESULT TMatrix4x4Variable<IBaseInterface, IsColumnMajor>::SetMatrixArray(CONST float *pData, UINT  Offset, UINT  Count)
{
    DirtyVariable();
    return DoMatrix4x4ArrayInternal<IsColumnMajor, FALSE, TRUE>(Data.pNumeric, const_cast<float*>(pData), Offset, Count
#ifdef _DEBUG 
        , pType, GetTotalUnpackedSize(), "ID3DX11EffectMatrixVariable::SetMatrixArray");
#else
        );
#endif
}

template<typename IBaseInterface, BOOL IsColumnMajor>
HRESULT TMatrix4x4Variable<IBaseInterface, IsColumnMajor>::GetMatrixArray(float *pData, UINT  Offset, UINT  Count)
{
    return DoMatrix4x4ArrayInternal<IsColumnMajor, FALSE, FALSE>(Data.pNumeric, pData, Offset, Count
#ifdef _DEBUG 
        , pType, GetTotalUnpackedSize(), "ID3DX11EffectMatrixVariable::GetMatrixArray");
#else
        );
#endif
}

template<typename IBaseInterface, BOOL IsColumnMajor>
HRESULT TMatrix4x4Variable<IBaseInterface, IsColumnMajor>::SetMatrixTranspose(CONST float *pData)
{
    DirtyVariable();
    return DoMatrix4x4ArrayInternal<IsColumnMajor, TRUE, TRUE>(Data.pNumeric, const_cast<float*>(pData), 0, 1
#ifdef _DEBUG 
        , pType, GetTotalUnpackedSize(), "ID3DX11EffectMatrixVariable::SetMatrixTranspose");
#else
        );
#endif
}

template<typename IBaseInterface, BOOL IsColumnMajor>
HRESULT TMatrix4x4Variable<IBaseInterface, IsColumnMajor>::GetMatrixTranspose(float *pData)
{
    return DoMatrix4x4ArrayInternal<IsColumnMajor, TRUE, FALSE>(Data.pNumeric, pData, 0, 1
#ifdef _DEBUG 
        , pType, GetTotalUnpackedSize(), "ID3DX11EffectMatrixVariable::GetMatrixTranspose");
#else
        );
#endif
}

template<typename IBaseInterface, BOOL IsColumnMajor>
HRESULT TMatrix4x4Variable<IBaseInterface, IsColumnMajor>::SetMatrixTransposeArray(CONST float *pData, UINT  Offset, UINT  Count)
{
    DirtyVariable();
    return DoMatrix4x4ArrayInternal<IsColumnMajor, TRUE, TRUE>(Data.pNumeric, const_cast<float*>(pData), Offset, Count
#ifdef _DEBUG 
        , pType, GetTotalUnpackedSize(), "ID3DX11EffectMatrixVariable::SetMatrixTransposeArray");
#else
        );
#endif
}

template<typename IBaseInterface, BOOL IsColumnMajor>
HRESULT TMatrix4x4Variable<IBaseInterface, IsColumnMajor>::GetMatrixTransposeArray(float *pData, UINT  Offset, UINT  Count)
{
    return DoMatrix4x4ArrayInternal<IsColumnMajor, TRUE, FALSE>(Data.pNumeric, pData, Offset, Count
#ifdef _DEBUG 
        , pType, GetTotalUnpackedSize(), "ID3DX11EffectMatrixVariable::GetMatrixTransposeArray");
#else
        );
#endif
}

#ifdef _DEBUG

// Useful object macro to check bounds and parameters
#define CHECK_OBJECT_ARRAY_BOUNDS(Offset, Count, Pointer) \
    HRESULT hr = S_OK; \
    VERIFYPARAMETER(Pointer) \
    UINT elements = IsArray() ? pType->Elements : 1; \
    \
    if ((Offset + Count < Offset) || (elements < Offset + Count)) \
    { \
        DPF(0, "%s: Invalid range specified", pFuncName); \
        VH(E_INVALIDARG); \
    } \

#define CHECK_OBJECT_SCALAR_BOUNDS(Index, Pointer) \
    HRESULT hr = S_OK; \
    VERIFYPARAMETER(Pointer) \
    UINT elements = IsArray() ? pType->Elements : 1; \
    \
    if (Index >= elements) \
    { \
        DPF(0, "%s: Invalid index specified", pFuncName); \
        VH(E_INVALIDARG); \
    } \

#define CHECK_SCALAR_BOUNDS(Index) \
    HRESULT hr = S_OK; \
    UINT elements = IsArray() ? pType->Elements : 1; \
    \
    if (Index >= elements) \
{ \
    DPF(0, "%s: Invalid index specified", pFuncName); \
    VH(E_INVALIDARG); \
} \

#else // _DEBUG

#define CHECK_OBJECT_ARRAY_BOUNDS(Offset, Count, Pointer) \
    HRESULT hr = S_OK; \

#define CHECK_OBJECT_SCALAR_BOUNDS(Index, Pointer) \
    HRESULT hr = S_OK; \

#define CHECK_SCALAR_BOUNDS(Index) \
    HRESULT hr = S_OK; \

#endif // _DEBUG

//////////////////////////////////////////////////////////////////////////
// ID3DX11EffectStringVariable (TStringVariable implementation)
//////////////////////////////////////////////////////////////////////////

template<typename IBaseInterface, BOOL IsAnnotation>
struct TStringVariable : public IBaseInterface
{
    STDMETHOD(GetString)(LPCSTR *ppString);
    STDMETHOD(GetStringArray)( __out_ecount(Count) LPCSTR *ppStrings, UINT  Offset, UINT  Count );
};

template<typename IBaseInterface, BOOL IsAnnotation>
HRESULT TStringVariable<IBaseInterface, IsAnnotation>::GetString(LPCSTR *ppString)
{
    HRESULT hr = S_OK;
    LPCSTR pFuncName = "ID3DX11EffectStringVariable::GetString";

    VERIFYPARAMETER(ppString);

    if (GetTopLevelEntity()->pEffect->IsOptimized())
    {
        DPF(0, "%s: Effect has been Optimize()'ed; all string/reflection data has been deleted", pFuncName);
        return D3DERR_INVALIDCALL;
    }

    D3DXASSERT(NULL != Data.pString);

    *ppString = Data.pString->pString;

lExit:
    return hr;
}

template<typename IBaseInterface, BOOL IsAnnotation>
HRESULT TStringVariable<IBaseInterface, IsAnnotation>::GetStringArray( __out_ecount(Count) LPCSTR *ppStrings, UINT  Offset, UINT  Count)
{
    LPCSTR pFuncName = "ID3DX11EffectStringVariable::GetStringArray";

    CHECK_OBJECT_ARRAY_BOUNDS(Offset, Count, ppStrings);

    if (GetTopLevelEntity()->pEffect->IsOptimized())
    {
        DPF(0, "%s: Effect has been Optimize()'ed; all string/reflection data has been deleted", pFuncName);
        return D3DERR_INVALIDCALL;
    }

    D3DXASSERT(NULL != Data.pString);

    UINT i;
    for (i = 0; i < Count; ++ i)
    {
        ppStrings[i] = (Data.pString + Offset + i)->pString;
    }

lExit:
    return hr;
}

//////////////////////////////////////////////////////////////////////////
// ID3DX11EffectClassInstanceVariable (TClassInstanceVariable implementation)
//////////////////////////////////////////////////////////////////////////

template<typename IBaseInterface>
struct TClassInstanceVariable : public IBaseInterface
{
    STDMETHOD(GetClassInstance)(ID3D11ClassInstance **ppClassInstance);
};

template<typename IBaseClassInstance>
HRESULT TClassInstanceVariable<IBaseClassInstance>::GetClassInstance(ID3D11ClassInstance** ppClassInstance)
{
    HRESULT hr = S_OK;
    LPCSTR pFuncName = "ID3DX11EffectClassInstanceVariable::GetClassInstance";

    D3DXASSERT( pMemberData != NULL );
    *ppClassInstance = pMemberData->Data.pD3DClassInstance;
    SAFE_ADDREF(*ppClassInstance);

lExit:
    return hr;
}

//////////////////////////////////////////////////////////////////////////
// ID3DX11EffectInterfaceeVariable (TInterfaceVariable implementation)
//////////////////////////////////////////////////////////////////////////

template<typename IBaseInterface>
struct TInterfaceVariable : public IBaseInterface
{
    STDMETHOD(SetClassInstance)(ID3DX11EffectClassInstanceVariable *pEffectClassInstance);
    STDMETHOD(GetClassInstance)(ID3DX11EffectClassInstanceVariable **ppEffectClassInstance);
};

template<typename IBaseInterface>
HRESULT TInterfaceVariable<IBaseInterface>::SetClassInstance(ID3DX11EffectClassInstanceVariable *pEffectClassInstance)
{
    HRESULT hr = S_OK;
    LPCSTR pFuncName = "ID3DX11EffectInterfaceVariable::SetClassInstance";

    // Note that we don't check if the types are compatible.  The debug layer will complain if it is.
    // IsValid() will not catch type mismatches.
    SClassInstanceGlobalVariable* pCI = (SClassInstanceGlobalVariable*)pEffectClassInstance;
    Data.pInterface->pClassInstance = pCI;

lExit:
    return hr;
}

template<typename IBaseInterface>
HRESULT TInterfaceVariable<IBaseInterface>::GetClassInstance(ID3DX11EffectClassInstanceVariable **ppEffectClassInstance)
{
    HRESULT hr = S_OK;
    LPCSTR pFuncName = "ID3DX11EffectInterfaceVariable::GetClassInstance";

#ifdef _DEBUG
    VERIFYPARAMETER(ppEffectClassInstance);
#endif

    *ppEffectClassInstance = Data.pInterface->pClassInstance;

lExit:
    return hr;
}

//////////////////////////////////////////////////////////////////////////
// ID3DX11EffectShaderResourceVariable (TShaderResourceVariable implementation)
//////////////////////////////////////////////////////////////////////////

template<typename IBaseInterface>
struct TShaderResourceVariable : public IBaseInterface
{
    STDMETHOD(SetResource)(ID3D11ShaderResourceView *pResource);
    STDMETHOD(GetResource)(ID3D11ShaderResourceView **ppResource);

    STDMETHOD(SetResourceArray)(ID3D11ShaderResourceView **ppResources, UINT  Offset, UINT  Count);
    STDMETHOD(GetResourceArray)(ID3D11ShaderResourceView **ppResources, UINT  Offset, UINT  Count);
};

static LPCSTR GetTextureTypeNameFromEnum(EObjectType ObjectType)
{
    switch (ObjectType)
    {
    case EOT_Buffer:
        return "Buffer";
    case EOT_Texture:
        return "texture";
    case EOT_Texture1D:
    case EOT_Texture1DArray:
        return "Texture1D";
    case EOT_Texture2DMS:
    case EOT_Texture2DMSArray:
        return "Texture2DMS";
    case EOT_Texture2D:
    case EOT_Texture2DArray:
        return "Texture2D";
    case EOT_Texture3D:
        return "Texture3D";
    case EOT_TextureCube:
        return "TextureCube";
    case EOT_TextureCubeArray:
        return "TextureCubeArray";
    case EOT_RWTexture1D:
    case EOT_RWTexture1DArray:
        return "RWTexture1D";
    case EOT_RWTexture2D:
    case EOT_RWTexture2DArray:
        return "RWTexture2D";
    case EOT_RWTexture3D:
        return "RWTexture3D";
    case EOT_RWBuffer:
        return "RWBuffer";
    case EOT_ByteAddressBuffer:
        return "ByteAddressBuffer";
    case EOT_RWByteAddressBuffer:
        return "RWByteAddressBuffer";
    case EOT_StructuredBuffer:
        return "StructuredBuffe";
    case EOT_RWStructuredBuffer:
        return "RWStructuredBuffer";
    case EOT_RWStructuredBufferAlloc:
        return "RWStructuredBufferAlloc";
    case EOT_RWStructuredBufferConsume:
        return "RWStructuredBufferConsume";
    case EOT_AppendStructuredBuffer:
        return "AppendStructuredBuffer";
    case EOT_ConsumeStructuredBuffer:
        return "ConsumeStructuredBuffer";
    }
    return "<unknown texture format>";
}

static LPCSTR GetResourceDimensionNameFromEnum(D3D11_RESOURCE_DIMENSION ResourceDimension)
{
    switch (ResourceDimension)
    {
    case D3D11_RESOURCE_DIMENSION_BUFFER:
        return "Buffer";
    case D3D11_RESOURCE_DIMENSION_TEXTURE1D:
        return "Texture1D";
    case D3D11_RESOURCE_DIMENSION_TEXTURE2D:
        return "Texture2D";
    case D3D11_RESOURCE_DIMENSION_TEXTURE3D:
        return "Texture3D";
    }
    return "<unknown texture format>";
}

static LPCSTR GetSRVDimensionNameFromEnum(D3D11_SRV_DIMENSION ViewDimension)
{
    switch (ViewDimension)
    {
    case D3D11_SRV_DIMENSION_BUFFER:
    case D3D11_SRV_DIMENSION_BUFFEREX:
        return "Buffer";
    case D3D11_SRV_DIMENSION_TEXTURE1D:
        return "Texture1D";
    case D3D11_SRV_DIMENSION_TEXTURE1DARRAY:
        return "Texture1DArray";
    case D3D11_SRV_DIMENSION_TEXTURE2D:
        return "Texture2D";
    case D3D11_SRV_DIMENSION_TEXTURE2DARRAY:
        return "Texture2DArray";
    case D3D11_SRV_DIMENSION_TEXTURE2DMS:
        return "Texture2DMS";
    case D3D11_SRV_DIMENSION_TEXTURE2DMSARRAY:
        return "Texture2DMSArray";
    case D3D11_SRV_DIMENSION_TEXTURE3D:
        return "Texture3D";
    case D3D11_SRV_DIMENSION_TEXTURECUBE:
        return "TextureCube";
    }
    return "<unknown texture format>";
}

static LPCSTR GetUAVDimensionNameFromEnum(D3D11_UAV_DIMENSION ViewDimension)
{
    switch (ViewDimension)
    {
    case D3D11_UAV_DIMENSION_BUFFER:
        return "Buffer";
    case D3D11_UAV_DIMENSION_TEXTURE1D:
        return "RWTexture1D";
    case D3D11_UAV_DIMENSION_TEXTURE1DARRAY:
        return "RWTexture1DArray";
    case D3D11_UAV_DIMENSION_TEXTURE2D:
        return "RWTexture2D";
    case D3D11_UAV_DIMENSION_TEXTURE2DARRAY:
        return "RWTexture2DArray";
    case D3D11_UAV_DIMENSION_TEXTURE3D:
        return "RWTexture3D";
    }
    return "<unknown texture format>";
}

static LPCSTR GetRTVDimensionNameFromEnum(D3D11_RTV_DIMENSION ViewDimension)
{
    switch (ViewDimension)
    {
    case D3D11_RTV_DIMENSION_BUFFER:
        return "Buffer";
    case D3D11_RTV_DIMENSION_TEXTURE1D:
        return "Texture1D";
    case D3D11_RTV_DIMENSION_TEXTURE1DARRAY:
        return "Texture1DArray";
    case D3D11_RTV_DIMENSION_TEXTURE2D:
        return "Texture2D";
    case D3D11_RTV_DIMENSION_TEXTURE2DARRAY:
        return "Texture2DArray";
    case D3D11_RTV_DIMENSION_TEXTURE2DMS:
        return "Texture2DMS";
    case D3D11_RTV_DIMENSION_TEXTURE2DMSARRAY:
        return "Texture2DMSArray";
    case D3D11_RTV_DIMENSION_TEXTURE3D:
        return "Texture3D";
    }
    return "<unknown texture format>";
}

static LPCSTR GetDSVDimensionNameFromEnum(D3D11_DSV_DIMENSION ViewDimension)
{
    switch (ViewDimension)
    {
    case D3D11_DSV_DIMENSION_TEXTURE1D:
        return "Texture1D";
    case D3D11_DSV_DIMENSION_TEXTURE1DARRAY:
        return "Texture1DArray";
    case D3D11_DSV_DIMENSION_TEXTURE2D:
        return "Texture2D";
    case D3D11_DSV_DIMENSION_TEXTURE2DARRAY:
        return "Texture2DArray";
    case D3D11_DSV_DIMENSION_TEXTURE2DMS:
        return "Texture2DMS";
    case D3D11_DSV_DIMENSION_TEXTURE2DMSARRAY:
        return "Texture2DMSArray";
    }
    return "<unknown texture format>";
}

static HRESULT ValidateTextureType(ID3D11ShaderResourceView *pView, EObjectType ObjectType, LPCSTR pFuncName)
{
    if (NULL != pView)
    {
        D3D11_SHADER_RESOURCE_VIEW_DESC desc;
        pView->GetDesc(&desc);
        switch (ObjectType)
        {
        case EOT_Texture:
            if (desc.ViewDimension != D3D11_SRV_DIMENSION_BUFFER && desc.ViewDimension != D3D11_SRV_DIMENSION_BUFFEREX)
                return S_OK;
            break;
        case EOT_Buffer:
            if (desc.ViewDimension != D3D11_SRV_DIMENSION_BUFFER && desc.ViewDimension != D3D11_SRV_DIMENSION_BUFFEREX)
                break;
            if (desc.ViewDimension == D3D11_SRV_DIMENSION_BUFFEREX && (desc.BufferEx.Flags & D3D11_BUFFEREX_SRV_FLAG_RAW))
            {
                DPF(0, "%s: Resource type mismatch; %s expected, ByteAddressBuffer provided.", pFuncName, GetTextureTypeNameFromEnum(ObjectType));
                return E_INVALIDARG;
            }
            else
            {
                ID3D11Buffer* pBuffer = NULL;
                pView->GetResource( (ID3D11Resource**)&pBuffer );
                D3DXASSERT( pBuffer != NULL );
                D3D11_BUFFER_DESC BufDesc;
                pBuffer->GetDesc( &BufDesc );
                SAFE_RELEASE( pBuffer );
                if( BufDesc.MiscFlags & D3D11_RESOURCE_MISC_BUFFER_STRUCTURED )
                {
                    DPF(0, "%s: Resource type mismatch; %s expected, StructuredBuffer provided.", pFuncName, GetTextureTypeNameFromEnum(ObjectType));
                    return E_INVALIDARG;
                }
                else
                {
                    return S_OK;
                }
            }
            break;
        case EOT_Texture1D:
        case EOT_Texture1DArray:
            if (desc.ViewDimension == D3D11_SRV_DIMENSION_TEXTURE1D || 
                desc.ViewDimension == D3D11_SRV_DIMENSION_TEXTURE1DARRAY)
                return S_OK;
            break;
        case EOT_Texture2D:
        case EOT_Texture2DArray:
            if (desc.ViewDimension == D3D11_SRV_DIMENSION_TEXTURE2D ||
                desc.ViewDimension == D3D11_SRV_DIMENSION_TEXTURE2DARRAY)
                return S_OK;
            break;
        case EOT_Texture2DMS:
        case EOT_Texture2DMSArray:
            if (desc.ViewDimension == D3D11_SRV_DIMENSION_TEXTURE2DMS ||
                desc.ViewDimension == D3D11_SRV_DIMENSION_TEXTURE2DMSARRAY)
                return S_OK;
            break;
        case EOT_Texture3D:
            if (desc.ViewDimension == D3D11_SRV_DIMENSION_TEXTURE3D)
                return S_OK;
            break;
        case EOT_TextureCube:
        case EOT_TextureCubeArray:
            if (desc.ViewDimension == D3D11_SRV_DIMENSION_TEXTURECUBE ||
                desc.ViewDimension == D3D11_SRV_DIMENSION_TEXTURECUBEARRAY)
                return S_OK;
            break;
        case EOT_ByteAddressBuffer:
            if (desc.ViewDimension == D3D11_SRV_DIMENSION_BUFFEREX && (desc.BufferEx.Flags & D3D11_BUFFEREX_SRV_FLAG_RAW))
                return S_OK;
            break;
        case EOT_StructuredBuffer:
            if (desc.ViewDimension == D3D11_SRV_DIMENSION_BUFFEREX || desc.ViewDimension == D3D11_SRV_DIMENSION_BUFFER)
            {
                ID3D11Buffer* pBuffer = NULL;
                pView->GetResource( (ID3D11Resource**)&pBuffer );
                D3DXASSERT( pBuffer != NULL );
                D3D11_BUFFER_DESC BufDesc;
                pBuffer->GetDesc( &BufDesc );
                SAFE_RELEASE( pBuffer );
                if( BufDesc.MiscFlags & D3D11_RESOURCE_MISC_BUFFER_STRUCTURED )
                {
                    return S_OK;
                }
                else
                {
                    DPF(0, "%s: Resource type mismatch; %s expected, non-structured Buffer provided.", pFuncName, GetTextureTypeNameFromEnum(ObjectType));
                    return E_INVALIDARG;
                }
            }
            break;
        default:
            D3DXASSERT(0); // internal error, should never get here
            return E_FAIL;
        }
        

        DPF(0, "%s: Resource type mismatch; %s expected, %s provided.", pFuncName, GetTextureTypeNameFromEnum(ObjectType), GetSRVDimensionNameFromEnum(desc.ViewDimension));
        return E_INVALIDARG;
    }
    return S_OK;
}

template<typename IBaseInterface>
HRESULT TShaderResourceVariable<IBaseInterface>::SetResource(ID3D11ShaderResourceView *pResource)
{
    HRESULT hr = S_OK;

#ifdef _DEBUG
    LPCSTR pFuncName = "ID3DX11EffectShaderResourceVariable::SetResource";

    VH(ValidateTextureType(pResource, pType->ObjectType, pFuncName));
#endif

    // Texture variables don't need to be dirtied.
    SAFE_ADDREF(pResource);
    SAFE_RELEASE(Data.pShaderResource->pShaderResource);
    Data.pShaderResource->pShaderResource = pResource;

lExit:
    return hr;
}

template<typename IBaseInterface>
HRESULT TShaderResourceVariable<IBaseInterface>::GetResource(ID3D11ShaderResourceView **ppResource)
{
    HRESULT hr = S_OK;

#ifdef _DEBUG
    LPCSTR pFuncName = "ID3DX11EffectShaderResourceVariable::GetResource";

    VERIFYPARAMETER(ppResource);
#endif

    *ppResource = Data.pShaderResource->pShaderResource;
    SAFE_ADDREF(*ppResource);

lExit:
    return hr;
}

template<typename IBaseInterface>
HRESULT TShaderResourceVariable<IBaseInterface>::SetResourceArray(ID3D11ShaderResourceView **ppResources, UINT  Offset, UINT  Count)
{
    LPCSTR pFuncName = "ID3DX11EffectShaderResourceVariable::SetResourceArray";
    UINT i;

    CHECK_OBJECT_ARRAY_BOUNDS(Offset, Count, ppResources);

#ifdef _DEBUG
    for (i = 0; i < Count; ++ i)
    {
        VH(ValidateTextureType(ppResources[i], pType->ObjectType, pFuncName));
    }
#endif

    // Texture variables don't need to be dirtied.
    for (i = 0; i < Count; ++ i)
    {
        SShaderResource *pResourceBlock = Data.pShaderResource + Offset + i;
        SAFE_ADDREF(ppResources[i]);
        SAFE_RELEASE(pResourceBlock->pShaderResource);
        pResourceBlock->pShaderResource = ppResources[i];
    }

lExit:
    return hr;
}

template<typename IBaseInterface>
HRESULT TShaderResourceVariable<IBaseInterface>::GetResourceArray(ID3D11ShaderResourceView **ppResources, UINT  Offset, UINT  Count)
{
    LPCSTR pFuncName = "ID3DX11EffectShaderResourceVariable::GetResourceArray";

    CHECK_OBJECT_ARRAY_BOUNDS(Offset, Count, ppResources);

    UINT i;
    for (i = 0; i < Count; ++ i)
    {
        ppResources[i] = (Data.pShaderResource + Offset + i)->pShaderResource;
        SAFE_ADDREF(ppResources[i]);
    }

lExit:
    return hr;
}

//////////////////////////////////////////////////////////////////////////
// ID3DX11EffectUnorderedAccessViewVariable (TUnorderedAccessViewVariable implementation)
//////////////////////////////////////////////////////////////////////////

template<typename IBaseInterface>
struct TUnorderedAccessViewVariable : public IBaseInterface
{
    STDMETHOD(SetUnorderedAccessView)(ID3D11UnorderedAccessView *pResource);
    STDMETHOD(GetUnorderedAccessView)(ID3D11UnorderedAccessView **ppResource);

    STDMETHOD(SetUnorderedAccessViewArray)(ID3D11UnorderedAccessView **ppResources, UINT  Offset, UINT  Count);
    STDMETHOD(GetUnorderedAccessViewArray)(ID3D11UnorderedAccessView **ppResources, UINT  Offset, UINT  Count);
};

static HRESULT ValidateTextureType(ID3D11UnorderedAccessView *pView, EObjectType ObjectType, LPCSTR pFuncName)
{
    if (NULL != pView)
    {
        D3D11_UNORDERED_ACCESS_VIEW_DESC desc;
        pView->GetDesc(&desc);
        switch (ObjectType)
        {
        case EOT_RWBuffer:
            if (desc.ViewDimension != D3D11_UAV_DIMENSION_BUFFER)
                break;
            if (desc.Buffer.Flags & D3D11_BUFFER_UAV_FLAG_RAW)
            {
                DPF(0, "%s: Resource type mismatch; %s expected, RWByteAddressBuffer provided.", pFuncName, GetTextureTypeNameFromEnum(ObjectType));
                return E_INVALIDARG;
            }
            else
            {
                ID3D11Buffer* pBuffer = NULL;
                pView->GetResource( (ID3D11Resource**)&pBuffer );
                D3DXASSERT( pBuffer != NULL );
                D3D11_BUFFER_DESC BufDesc;
                pBuffer->GetDesc( &BufDesc );
                SAFE_RELEASE( pBuffer );
                if( BufDesc.MiscFlags & D3D11_RESOURCE_MISC_BUFFER_STRUCTURED )
                {
                    DPF(0, "%s: Resource type mismatch; %s expected, an RWStructuredBuffer provided.", pFuncName, GetTextureTypeNameFromEnum(ObjectType));
                    return E_INVALIDARG;
                }
                else
                {
                    return S_OK;
                }
            }
            break;
        case EOT_RWTexture1D:
        case EOT_RWTexture1DArray:
            if (desc.ViewDimension == D3D11_UAV_DIMENSION_TEXTURE1D || 
                desc.ViewDimension == D3D11_UAV_DIMENSION_TEXTURE1DARRAY)
                return S_OK;
            break;
        case EOT_RWTexture2D:
        case EOT_RWTexture2DArray:
            if (desc.ViewDimension == D3D11_UAV_DIMENSION_TEXTURE2D ||
                desc.ViewDimension == D3D11_UAV_DIMENSION_TEXTURE2DARRAY)
                return S_OK;
            break;
        case EOT_RWTexture3D:
            if (desc.ViewDimension == D3D11_UAV_DIMENSION_TEXTURE3D)
                return S_OK;
            break;
        case EOT_RWByteAddressBuffer:
            if (desc.ViewDimension == D3D11_UAV_DIMENSION_BUFFER && (desc.Buffer.Flags & D3D11_BUFFER_UAV_FLAG_RAW))
                return S_OK;
            break;
        case EOT_RWStructuredBuffer:
            if (desc.ViewDimension == D3D11_UAV_DIMENSION_BUFFER)
            {
                ID3D11Buffer* pBuffer = NULL;
                pView->GetResource( (ID3D11Resource**)&pBuffer );
                D3DXASSERT( pBuffer != NULL );
                D3D11_BUFFER_DESC BufDesc;
                pBuffer->GetDesc( &BufDesc );
                SAFE_RELEASE( pBuffer );
                if( BufDesc.MiscFlags & D3D11_RESOURCE_MISC_BUFFER_STRUCTURED )
                {
                    return S_OK;
                }
                else
                {
                    DPF(0, "%s: Resource type mismatch; %s expected, non-structured Buffer provided.", pFuncName, GetTextureTypeNameFromEnum(ObjectType));
                    return E_INVALIDARG;
                }
            }
            break;
        case EOT_RWStructuredBufferAlloc:
        case EOT_RWStructuredBufferConsume:
            if (desc.ViewDimension != D3D11_UAV_DIMENSION_BUFFER)
                break;
            if (desc.Buffer.Flags & D3D11_BUFFER_UAV_FLAG_COUNTER)
            {
                return S_OK;
            }
            else
            {
                DPF(0, "%s: Resource type mismatch; %s expected, non-Counter buffer provided.", pFuncName, GetTextureTypeNameFromEnum(ObjectType));
                return E_INVALIDARG;
            }
            break;
        case EOT_AppendStructuredBuffer:
        case EOT_ConsumeStructuredBuffer:
            if (desc.ViewDimension != D3D11_UAV_DIMENSION_BUFFER)
                break;
            if (desc.Buffer.Flags & D3D11_BUFFER_UAV_FLAG_APPEND)
            {
                return S_OK;
            }
            else
            {
                DPF(0, "%s: Resource type mismatch; %s expected, non-Append buffer provided.", pFuncName, GetTextureTypeNameFromEnum(ObjectType));
                return E_INVALIDARG;
            }
            break;
        default:
            D3DXASSERT(0); // internal error, should never get here
            return E_FAIL;
        }


        DPF(0, "%s: Resource type mismatch; %s expected, %s provided.", pFuncName, GetTextureTypeNameFromEnum(ObjectType), GetUAVDimensionNameFromEnum(desc.ViewDimension));
        return E_INVALIDARG;
    }
    return S_OK;
}

template<typename IBaseInterface>
HRESULT TUnorderedAccessViewVariable<IBaseInterface>::SetUnorderedAccessView(ID3D11UnorderedAccessView *pResource)
{
    HRESULT hr = S_OK;

#ifdef _DEBUG
    LPCSTR pFuncName = "ID3DX11EffectUnorderedAccessViewVariable::SetUnorderedAccessView";

    VH(ValidateTextureType(pResource, pType->ObjectType, pFuncName));
#endif

    // UAV variables don't need to be dirtied.
    SAFE_ADDREF(pResource);
    SAFE_RELEASE(Data.pUnorderedAccessView->pUnorderedAccessView);
    Data.pUnorderedAccessView->pUnorderedAccessView = pResource;

lExit:
    return hr;
}

template<typename IBaseInterface>
HRESULT TUnorderedAccessViewVariable<IBaseInterface>::GetUnorderedAccessView(ID3D11UnorderedAccessView **ppResource)
{
    HRESULT hr = S_OK;

#ifdef _DEBUG
    LPCSTR pFuncName = "ID3DX11EffectUnorderedAccessViewVariable::GetUnorderedAccessView";

    VERIFYPARAMETER(ppResource);
#endif

    *ppResource = Data.pUnorderedAccessView->pUnorderedAccessView;
    SAFE_ADDREF(*ppResource);

lExit:
    return hr;
}

template<typename IBaseInterface>
HRESULT TUnorderedAccessViewVariable<IBaseInterface>::SetUnorderedAccessViewArray(ID3D11UnorderedAccessView **ppResources, UINT  Offset, UINT  Count)
{
    LPCSTR pFuncName = "ID3DX11EffectUnorderedAccessViewVariable::SetUnorderedAccessViewArray";
    UINT i;

    CHECK_OBJECT_ARRAY_BOUNDS(Offset, Count, ppResources);

#ifdef _DEBUG
    for (i = 0; i < Count; ++ i)
    {
        VH(ValidateTextureType(ppResources[i], pType->ObjectType, pFuncName));
    }
#endif

    // Texture variables don't need to be dirtied.
    for (i = 0; i < Count; ++ i)
    {
        SUnorderedAccessView *pResourceBlock = Data.pUnorderedAccessView + Offset + i;
        SAFE_ADDREF(ppResources[i]);
        SAFE_RELEASE(pResourceBlock->pUnorderedAccessView);
        pResourceBlock->pUnorderedAccessView = ppResources[i];
    }

lExit:
    return hr;
}

template<typename IBaseInterface>
HRESULT TUnorderedAccessViewVariable<IBaseInterface>::GetUnorderedAccessViewArray(ID3D11UnorderedAccessView **ppResources, UINT  Offset, UINT  Count)
{
    LPCSTR pFuncName = "ID3DX11EffectUnorderedAccessViewVariable::GetUnorderedAccessViewArray";

    CHECK_OBJECT_ARRAY_BOUNDS(Offset, Count, ppResources);

    UINT i;
    for (i = 0; i < Count; ++ i)
    {
        ppResources[i] = (Data.pUnorderedAccessView + Offset + i)->pUnorderedAccessView;
        SAFE_ADDREF(ppResources[i]);
    }

lExit:
    return hr;
}

//////////////////////////////////////////////////////////////////////////
// ID3DX11EffectRenderTargetViewVariable (TRenderTargetViewVariable implementation)
//////////////////////////////////////////////////////////////////////////

template<typename IBaseInterface>
struct TRenderTargetViewVariable : public IBaseInterface
{
    STDMETHOD(SetRenderTarget)(ID3D11RenderTargetView *pResource);
    STDMETHOD(GetRenderTarget)(ID3D11RenderTargetView **ppResource);

    STDMETHOD(SetRenderTargetArray)(ID3D11RenderTargetView **ppResources, UINT  Offset, UINT  Count);
    STDMETHOD(GetRenderTargetArray)(ID3D11RenderTargetView **ppResources, UINT  Offset, UINT  Count);
};


template<typename IBaseInterface>
HRESULT TRenderTargetViewVariable<IBaseInterface>::SetRenderTarget(ID3D11RenderTargetView *pResource)
{
    HRESULT hr = S_OK;

#ifdef _DEBUG
    LPCSTR pFuncName = "ID3DX11EffectRenderTargetVariable::SetRenderTarget";

#endif

    // Texture variables don't need to be dirtied.
    SAFE_ADDREF(pResource);
    SAFE_RELEASE(Data.pRenderTargetView->pRenderTargetView);
    Data.pRenderTargetView->pRenderTargetView = pResource;

lExit:
    return hr;
}

template<typename IBaseInterface>
HRESULT TRenderTargetViewVariable<IBaseInterface>::GetRenderTarget(ID3D11RenderTargetView **ppResource)
{
    HRESULT hr = S_OK;

    *ppResource = Data.pRenderTargetView->pRenderTargetView;
    SAFE_ADDREF(*ppResource);

lExit:
    return hr;
}

template<typename IBaseInterface>
HRESULT TRenderTargetViewVariable<IBaseInterface>::SetRenderTargetArray(ID3D11RenderTargetView **ppResources, UINT  Offset, UINT  Count)
{
    LPCSTR pFuncName = "ID3DX11EffectRenderTargetVariable::SetRenderTargetArray";
    UINT i;

    CHECK_OBJECT_ARRAY_BOUNDS(Offset, Count, ppResources);

    // Texture variables don't need to be dirtied.
    for (i = 0; i < Count; ++ i)
    {
        SRenderTargetView *pResourceBlock = Data.pRenderTargetView + Offset + i;
        SAFE_ADDREF(ppResources[i]);
        SAFE_RELEASE(pResourceBlock->pRenderTargetView);
        pResourceBlock->pRenderTargetView = ppResources[i];
    }

lExit:
    return hr;
}

template<typename IBaseInterface>
HRESULT TRenderTargetViewVariable<IBaseInterface>::GetRenderTargetArray(ID3D11RenderTargetView **ppResources, UINT  Offset, UINT  Count)
{
    LPCSTR pFuncName = "ID3DX11EffectRenderTargetVariable::GetRenderTargetArray";

    CHECK_OBJECT_ARRAY_BOUNDS(Offset, Count, ppResources);

    UINT i;
    for (i = 0; i < Count; ++ i)
    {
        ppResources[i] = (Data.pRenderTargetView + Offset + i)->pRenderTargetView;
        SAFE_ADDREF(ppResources[i]);
    }

lExit:
    return hr;
}

//////////////////////////////////////////////////////////////////////////
// ID3DX11EffectDepthStencilViewVariable (TDepthStencilViewVariable implementation)
//////////////////////////////////////////////////////////////////////////

template<typename IBaseInterface>
struct TDepthStencilViewVariable : public IBaseInterface
{
    STDMETHOD(SetDepthStencil)(ID3D11DepthStencilView *pResource);
    STDMETHOD(GetDepthStencil)(ID3D11DepthStencilView **ppResource);

    STDMETHOD(SetDepthStencilArray)(ID3D11DepthStencilView **ppResources, UINT  Offset, UINT  Count);
    STDMETHOD(GetDepthStencilArray)(ID3D11DepthStencilView **ppResources, UINT  Offset, UINT  Count);
};


template<typename IBaseInterface>
HRESULT TDepthStencilViewVariable<IBaseInterface>::SetDepthStencil(ID3D11DepthStencilView *pResource)
{
    HRESULT hr = S_OK;

#ifdef _DEBUG
    LPCSTR pFuncName = "ID3D11DepthStencilViewVariable::SetDepthStencil";

#endif

    // Texture variables don't need to be dirtied.
    SAFE_ADDREF(pResource);
    SAFE_RELEASE(Data.pDepthStencilView->pDepthStencilView);
    Data.pDepthStencilView->pDepthStencilView = pResource;

lExit:
    return hr;
}

template<typename IBaseInterface>
HRESULT TDepthStencilViewVariable<IBaseInterface>::GetDepthStencil(ID3D11DepthStencilView **ppResource)
{
    HRESULT hr = S_OK;

#ifdef _DEBUG
    LPCSTR pFuncName = "ID3D11DepthStencilViewVariable::GetDepthStencil";

    VERIFYPARAMETER(ppResource);
#endif

    *ppResource = Data.pDepthStencilView->pDepthStencilView;
    SAFE_ADDREF(*ppResource);

lExit:
    return hr;
}

template<typename IBaseInterface>
HRESULT TDepthStencilViewVariable<IBaseInterface>::SetDepthStencilArray(ID3D11DepthStencilView **ppResources, UINT  Offset, UINT  Count)
{
    LPCSTR pFuncName = "ID3D11DepthStencilViewVariable::SetDepthStencilArray";
    UINT i;

    CHECK_OBJECT_ARRAY_BOUNDS(Offset, Count, ppResources);

    // Texture variables don't need to be dirtied.
    for (i = 0; i < Count; ++ i)
    {
        SDepthStencilView *pResourceBlock = Data.pDepthStencilView + Offset + i;
        SAFE_ADDREF(ppResources[i]);
        SAFE_RELEASE(pResourceBlock->pDepthStencilView);
        pResourceBlock->pDepthStencilView = ppResources[i];
    }

lExit:
    return hr;
}

template<typename IBaseInterface>
HRESULT TDepthStencilViewVariable<IBaseInterface>::GetDepthStencilArray(ID3D11DepthStencilView **ppResources, UINT  Offset, UINT  Count)
{
    LPCSTR pFuncName = "ID3D11DepthStencilViewVariable::GetDepthStencilArray";

    CHECK_OBJECT_ARRAY_BOUNDS(Offset, Count, ppResources);

    UINT i;
    for (i = 0; i < Count; ++ i)
    {
        ppResources[i] = (Data.pDepthStencilView + Offset + i)->pDepthStencilView;
        SAFE_ADDREF(ppResources[i]);
    }

lExit:
    return hr;
}



////////////////////////////////////////////////////////////////////////////////
// ID3DX11EffectShaderVariable (TShaderVariable implementation)
////////////////////////////////////////////////////////////////////////////////

template<typename IBaseInterface>
struct TShaderVariable : public IBaseInterface
{
    STDMETHOD(GetShaderDesc)(UINT ShaderIndex, D3DX11_EFFECT_SHADER_DESC *pDesc);

    STDMETHOD(GetVertexShader)(UINT ShaderIndex, ID3D11VertexShader **ppVS);
    STDMETHOD(GetGeometryShader)(UINT ShaderIndex, ID3D11GeometryShader **ppGS);
    STDMETHOD(GetPixelShader)(UINT ShaderIndex, ID3D11PixelShader **ppPS);
    STDMETHOD(GetHullShader)(UINT ShaderIndex, ID3D11HullShader **ppPS);
    STDMETHOD(GetDomainShader)(UINT ShaderIndex, ID3D11DomainShader **ppPS);
    STDMETHOD(GetComputeShader)(UINT ShaderIndex, ID3D11ComputeShader **ppPS);

    STDMETHOD(GetInputSignatureElementDesc)(UINT ShaderIndex, UINT Element, D3D11_SIGNATURE_PARAMETER_DESC *pDesc);
    STDMETHOD(GetOutputSignatureElementDesc)(UINT ShaderIndex, UINT Element, D3D11_SIGNATURE_PARAMETER_DESC *pDesc);
    STDMETHOD(GetPatchConstantSignatureElementDesc)(UINT ShaderIndex, UINT Element, D3D11_SIGNATURE_PARAMETER_DESC *pDesc);

    STDMETHOD_(BOOL, IsValid)();
};

template<typename IBaseInterface>
HRESULT TShaderVariable<IBaseInterface>::GetShaderDesc(UINT ShaderIndex, D3DX11_EFFECT_SHADER_DESC *pDesc)
{
    LPCSTR pFuncName = "ID3DX11EffectShaderVariable::GetShaderDesc";

    CHECK_OBJECT_SCALAR_BOUNDS(ShaderIndex, pDesc);

    Data.pShader[ShaderIndex].GetShaderDesc(pDesc, FALSE);

lExit:
    return hr;
}

template<typename IBaseInterface>
HRESULT TShaderVariable<IBaseInterface>::GetVertexShader(UINT ShaderIndex, ID3D11VertexShader **ppVS)
{
    LPCSTR pFuncName = "ID3DX11EffectShaderVariable::GetVertexShader";

    CHECK_OBJECT_SCALAR_BOUNDS(ShaderIndex, ppVS);

    VH( Data.pShader[ShaderIndex].GetVertexShader(ppVS) );

lExit:
    return hr;
}

template<typename IBaseInterface>
HRESULT TShaderVariable<IBaseInterface>::GetGeometryShader(UINT ShaderIndex, ID3D11GeometryShader **ppGS)
{
    LPCSTR pFuncName = "ID3DX11EffectShaderVariable::GetGeometryShader";

    CHECK_OBJECT_SCALAR_BOUNDS(ShaderIndex, ppGS);

    VH( Data.pShader[ShaderIndex].GetGeometryShader(ppGS) );

lExit:
    return hr;
}

template<typename IBaseInterface>
HRESULT TShaderVariable<IBaseInterface>::GetPixelShader(UINT ShaderIndex, ID3D11PixelShader **ppPS)
{
    LPCSTR pFuncName = "ID3DX11EffectShaderVariable::GetPixelShader";

    CHECK_OBJECT_SCALAR_BOUNDS(ShaderIndex, ppPS);

    VH( Data.pShader[ShaderIndex].GetPixelShader(ppPS) );

lExit:
    return hr;
}

template<typename IBaseInterface>
HRESULT TShaderVariable<IBaseInterface>::GetHullShader(UINT ShaderIndex, ID3D11HullShader **ppHS)
{
    LPCSTR pFuncName = "ID3DX11EffectShaderVariable::GetHullShader";

    CHECK_OBJECT_SCALAR_BOUNDS(ShaderIndex, ppHS);

    VH( Data.pShader[ShaderIndex].GetHullShader(ppHS) );

lExit:
    return hr;
}

template<typename IBaseInterface>
HRESULT TShaderVariable<IBaseInterface>::GetDomainShader(UINT ShaderIndex, ID3D11DomainShader **ppDS)
{
    LPCSTR pFuncName = "ID3DX11EffectShaderVariable::GetDomainShader";

    CHECK_OBJECT_SCALAR_BOUNDS(ShaderIndex, ppDS);

    VH( Data.pShader[ShaderIndex].GetDomainShader(ppDS) );

lExit:
    return hr;
}

template<typename IBaseInterface>
HRESULT TShaderVariable<IBaseInterface>::GetComputeShader(UINT ShaderIndex, ID3D11ComputeShader **ppCS)
{
    LPCSTR pFuncName = "ID3DX11EffectShaderVariable::GetComputeShader";

    CHECK_OBJECT_SCALAR_BOUNDS(ShaderIndex, ppCS);

    VH( Data.pShader[ShaderIndex].GetComputeShader(ppCS) );

lExit:
    return hr;
}

template<typename IBaseInterface>
HRESULT TShaderVariable<IBaseInterface>::GetInputSignatureElementDesc(UINT ShaderIndex, UINT Element, D3D11_SIGNATURE_PARAMETER_DESC *pDesc)
{
    LPCSTR pFuncName = "ID3DX11EffectShaderVariable::GetInputSignatureElementDesc";

    CHECK_OBJECT_SCALAR_BOUNDS(ShaderIndex, pDesc);

    VH( Data.pShader[ShaderIndex].GetSignatureElementDesc(SShaderBlock::ST_Input, Element, pDesc) );

lExit:
    return hr;
}

template<typename IBaseInterface>
HRESULT TShaderVariable<IBaseInterface>::GetOutputSignatureElementDesc(UINT ShaderIndex, UINT Element, D3D11_SIGNATURE_PARAMETER_DESC *pDesc)
{
    LPCSTR pFuncName = "ID3DX11EffectShaderVariable::GetOutputSignatureElementDesc";

    CHECK_OBJECT_SCALAR_BOUNDS(ShaderIndex, pDesc);

    VH( Data.pShader[ShaderIndex].GetSignatureElementDesc(SShaderBlock::ST_Output, Element, pDesc) );

lExit:
    return hr;
}

template<typename IBaseInterface>
HRESULT TShaderVariable<IBaseInterface>::GetPatchConstantSignatureElementDesc(UINT ShaderIndex, UINT Element, D3D11_SIGNATURE_PARAMETER_DESC *pDesc)
{
    LPCSTR pFuncName = "ID3DX11EffectShaderVariable::GetPatchConstantSignatureElementDesc";

    CHECK_OBJECT_SCALAR_BOUNDS(ShaderIndex, pDesc);

    VH( Data.pShader[ShaderIndex].GetSignatureElementDesc(SShaderBlock::ST_PatchConstant, Element, pDesc) );

lExit:
    return hr;
}

template<typename IBaseInterface>
BOOL TShaderVariable<IBaseInterface>::IsValid()
{
    UINT numElements = IsArray()? pType->Elements : 1;
    BOOL valid = TRUE;
    while( numElements > 0 && ( valid = Data.pShader[ numElements-1 ].IsValid ) )
        numElements--;
    return valid;
}

////////////////////////////////////////////////////////////////////////////////
// ID3DX11EffectBlendVariable (TBlendVariable implementation)
////////////////////////////////////////////////////////////////////////////////

template<typename IBaseInterface>
struct TBlendVariable : public IBaseInterface
{
public:
    STDMETHOD(GetBlendState)(UINT Index, ID3D11BlendState **ppBlendState);
    STDMETHOD(SetBlendState)(UINT Index, ID3D11BlendState *pBlendState);
    STDMETHOD(UndoSetBlendState)(UINT Index);
    STDMETHOD(GetBackingStore)(UINT Index, D3D11_BLEND_DESC *pBlendDesc);
    STDMETHOD_(BOOL, IsValid)();
};

template<typename IBaseInterface>
HRESULT TBlendVariable<IBaseInterface>::GetBlendState(UINT Index, ID3D11BlendState **ppBlendState)
{
    LPCSTR pFuncName = "ID3DX11EffectBlendVariable::GetBlendState";

    CHECK_OBJECT_SCALAR_BOUNDS(Index, ppBlendState);

    *ppBlendState = Data.pBlend[Index].pBlendObject;
    SAFE_ADDREF(*ppBlendState);

lExit:
    return hr;
}

template<typename IBaseInterface>
HRESULT TBlendVariable<IBaseInterface>::SetBlendState(UINT Index, ID3D11BlendState *pBlendState)
{
    LPCSTR pFuncName = "ID3DX11EffectBlendState::SetBlendState";

    CHECK_SCALAR_BOUNDS(Index);

    if( !Data.pBlend[Index].IsUserManaged )
    {
        // Save original state object in case we UndoSet
        D3DXASSERT( pMemberData[Index].Type == MDT_BlendState );
        VB( pMemberData[Index].Data.pD3DEffectsManagedBlendState == NULL );
        pMemberData[Index].Data.pD3DEffectsManagedBlendState = Data.pBlend[Index].pBlendObject;
        Data.pBlend[Index].pBlendObject = NULL;
        Data.pBlend[Index].IsUserManaged = TRUE;
    }

    SAFE_ADDREF( pBlendState );
    SAFE_RELEASE( Data.pBlend[Index].pBlendObject );
    Data.pBlend[Index].pBlendObject = pBlendState;
    Data.pBlend[Index].IsValid = TRUE;
lExit:
    return hr;
}

template<typename IBaseInterface>
HRESULT TBlendVariable<IBaseInterface>::UndoSetBlendState(UINT Index)
{
    LPCSTR pFuncName = "ID3DX11EffectBlendState::UndoSetBlendState";

    CHECK_SCALAR_BOUNDS(Index);

    if( !Data.pBlend[Index].IsUserManaged )
    {
        return S_FALSE;
    }

    // Revert to original state object
    SAFE_RELEASE( Data.pBlend[Index].pBlendObject );
    Data.pBlend[Index].pBlendObject = pMemberData[Index].Data.pD3DEffectsManagedBlendState;
    pMemberData[Index].Data.pD3DEffectsManagedBlendState = NULL;
    Data.pBlend[Index].IsUserManaged = FALSE;

lExit:
    return hr;
}

template<typename IBaseInterface>
HRESULT TBlendVariable<IBaseInterface>::GetBackingStore(UINT Index, D3D11_BLEND_DESC *pBlendDesc)
{
    LPCSTR pFuncName = "ID3DX11EffectBlendVariable::GetBackingStore";

    CHECK_OBJECT_SCALAR_BOUNDS(Index, pBlendDesc);

    if( Data.pBlend[Index].IsUserManaged )
    {
        if( Data.pBlend[Index].pBlendObject )
        {
            Data.pBlend[Index].pBlendObject->GetDesc( pBlendDesc );
        }
        else
        {
            *pBlendDesc = CD3D11_BLEND_DESC( D3D11_DEFAULT );
        }
    }
    else
    {
        SBlendBlock *pBlock = Data.pBlend + Index;
        if (pBlock->ApplyAssignments(GetTopLevelEntity()->pEffect))
        {
            pBlock->pAssignments[0].LastRecomputedTime = 0; // Force a recreate of this block the next time ApplyRenderStateBlock is called
        }

        memcpy( pBlendDesc, &pBlock->BackingStore, sizeof(D3D11_BLEND_DESC) );
    }

lExit:
    return hr;
}

template<typename IBaseInterface>
BOOL TBlendVariable<IBaseInterface>::IsValid()
{
    UINT numElements = IsArray()? pType->Elements : 1;
    BOOL valid = TRUE;
    while( numElements > 0 && ( valid = Data.pBlend[ numElements-1 ].IsValid ) )
        numElements--;
    return valid;
}


////////////////////////////////////////////////////////////////////////////////
// ID3DX11EffectDepthStencilVariable (TDepthStencilVariable implementation)
////////////////////////////////////////////////////////////////////////////////

template<typename IBaseInterface>
struct TDepthStencilVariable : public IBaseInterface
{
public:
    STDMETHOD(GetDepthStencilState)(UINT Index, ID3D11DepthStencilState **ppDepthStencilState);
    STDMETHOD(SetDepthStencilState)(UINT Index, ID3D11DepthStencilState *pDepthStencilState);
    STDMETHOD(UndoSetDepthStencilState)(UINT Index);
    STDMETHOD(GetBackingStore)(UINT Index, D3D11_DEPTH_STENCIL_DESC *pDepthStencilDesc);
    STDMETHOD_(BOOL, IsValid)();
};

template<typename IBaseInterface>
HRESULT TDepthStencilVariable<IBaseInterface>::GetDepthStencilState(UINT Index, ID3D11DepthStencilState **ppDepthStencilState)
{
    LPCSTR pFuncName = "ID3DX11EffectDepthStencilVariable::GetDepthStencilState";

    CHECK_OBJECT_SCALAR_BOUNDS(Index, ppDepthStencilState);

    *ppDepthStencilState = Data.pDepthStencil[Index].pDSObject;
    SAFE_ADDREF(*ppDepthStencilState);

lExit:
    return hr;
}

template<typename IBaseInterface>
HRESULT TDepthStencilVariable<IBaseInterface>::SetDepthStencilState(UINT Index, ID3D11DepthStencilState *pDepthStencilState)
{
    LPCSTR pFuncName = "ID3DX11EffectDepthStencilState::SetDepthStencilState";

    CHECK_SCALAR_BOUNDS(Index);

    if( !Data.pDepthStencil[Index].IsUserManaged )
    {
        // Save original state object in case we UndoSet
        D3DXASSERT( pMemberData[Index].Type == MDT_DepthStencilState );
        VB( pMemberData[Index].Data.pD3DEffectsManagedDepthStencilState == NULL );
        pMemberData[Index].Data.pD3DEffectsManagedDepthStencilState = Data.pDepthStencil[Index].pDSObject;
        Data.pDepthStencil[Index].pDSObject = NULL;
        Data.pDepthStencil[Index].IsUserManaged = TRUE;
    }

    SAFE_ADDREF( pDepthStencilState );
    SAFE_RELEASE( Data.pDepthStencil[Index].pDSObject );
    Data.pDepthStencil[Index].pDSObject = pDepthStencilState;
    Data.pDepthStencil[Index].IsValid = TRUE;
lExit:
    return hr;
}

template<typename IBaseInterface>
HRESULT TDepthStencilVariable<IBaseInterface>::UndoSetDepthStencilState(UINT Index)
{
    LPCSTR pFuncName = "ID3DX11EffectDepthStencilState::UndoSetDepthStencilState";

    CHECK_SCALAR_BOUNDS(Index);

    if( !Data.pDepthStencil[Index].IsUserManaged )
    {
        return S_FALSE;
    }

    // Revert to original state object
    SAFE_RELEASE( Data.pDepthStencil[Index].pDSObject );
    Data.pDepthStencil[Index].pDSObject = pMemberData[Index].Data.pD3DEffectsManagedDepthStencilState;
    pMemberData[Index].Data.pD3DEffectsManagedDepthStencilState = NULL;
    Data.pDepthStencil[Index].IsUserManaged = FALSE;

lExit:
    return hr;
}

template<typename IBaseInterface>
HRESULT TDepthStencilVariable<IBaseInterface>::GetBackingStore(UINT Index, D3D11_DEPTH_STENCIL_DESC *pDepthStencilDesc)
{
    LPCSTR pFuncName = "ID3DX11EffectDepthStencilVariable::GetBackingStore";

    CHECK_OBJECT_SCALAR_BOUNDS(Index, pDepthStencilDesc);

    if( Data.pDepthStencil[Index].IsUserManaged )
    {
        if( Data.pDepthStencil[Index].pDSObject )
        {
            Data.pDepthStencil[Index].pDSObject->GetDesc( pDepthStencilDesc );
        }
        else
        {
            *pDepthStencilDesc = CD3D11_DEPTH_STENCIL_DESC( D3D11_DEFAULT );
        }
    }
    else
    {
        SDepthStencilBlock *pBlock = Data.pDepthStencil + Index;
        if (pBlock->ApplyAssignments(GetTopLevelEntity()->pEffect))
        {
            pBlock->pAssignments[0].LastRecomputedTime = 0; // Force a recreate of this block the next time ApplyRenderStateBlock is called
        }

        memcpy(pDepthStencilDesc, &pBlock->BackingStore, sizeof(D3D11_DEPTH_STENCIL_DESC));
    }

lExit:
    return hr;
}

template<typename IBaseInterface>
BOOL TDepthStencilVariable<IBaseInterface>::IsValid()
{
    UINT numElements = IsArray()? pType->Elements : 1;
    BOOL valid = TRUE;
    while( numElements > 0 && ( valid = Data.pDepthStencil[ numElements-1 ].IsValid ) )
        numElements--;
    return valid;
}

////////////////////////////////////////////////////////////////////////////////
// ID3DX11EffectRasterizerVariable (TRasterizerVariable implementation)
////////////////////////////////////////////////////////////////////////////////

template<typename IBaseInterface>
struct TRasterizerVariable : public IBaseInterface
{
public:

    STDMETHOD(GetRasterizerState)(UINT Index, ID3D11RasterizerState **ppRasterizerState);
    STDMETHOD(SetRasterizerState)(UINT Index, ID3D11RasterizerState *pRasterizerState);
    STDMETHOD(UndoSetRasterizerState)(UINT Index);
    STDMETHOD(GetBackingStore)(UINT Index, D3D11_RASTERIZER_DESC *pRasterizerDesc);
    STDMETHOD_(BOOL, IsValid)();
};

template<typename IBaseInterface>
HRESULT TRasterizerVariable<IBaseInterface>::GetRasterizerState(UINT Index, ID3D11RasterizerState **ppRasterizerState)
{
    LPCSTR pFuncName = "ID3DX11EffectRasterizerVariable::GetRasterizerState";

    CHECK_OBJECT_SCALAR_BOUNDS(Index, ppRasterizerState);

    *ppRasterizerState = Data.pRasterizer[Index].pRasterizerObject;
    SAFE_ADDREF(*ppRasterizerState);

lExit:
    return hr;
}

template<typename IBaseInterface>
HRESULT TRasterizerVariable<IBaseInterface>::SetRasterizerState(UINT Index, ID3D11RasterizerState *pRasterizerState)
{
    LPCSTR pFuncName = "ID3DX11EffectRasterizerState::SetRasterizerState";

    CHECK_SCALAR_BOUNDS(Index);

    if( !Data.pRasterizer[Index].IsUserManaged )
    {
        // Save original state object in case we UndoSet
        D3DXASSERT( pMemberData[Index].Type == MDT_RasterizerState );
        VB( pMemberData[Index].Data.pD3DEffectsManagedRasterizerState == NULL );
        pMemberData[Index].Data.pD3DEffectsManagedRasterizerState = Data.pRasterizer[Index].pRasterizerObject;
        Data.pRasterizer[Index].pRasterizerObject = NULL;
        Data.pRasterizer[Index].IsUserManaged = TRUE;
    }

    SAFE_ADDREF( pRasterizerState );
    SAFE_RELEASE( Data.pRasterizer[Index].pRasterizerObject );
    Data.pRasterizer[Index].pRasterizerObject = pRasterizerState;
    Data.pRasterizer[Index].IsValid = TRUE;
lExit:
    return hr;
}

template<typename IBaseInterface>
HRESULT TRasterizerVariable<IBaseInterface>::UndoSetRasterizerState(UINT Index)
{
    LPCSTR pFuncName = "ID3DX11EffectRasterizerState::UndoSetRasterizerState";

    CHECK_SCALAR_BOUNDS(Index);

    if( !Data.pRasterizer[Index].IsUserManaged )
    {
        return S_FALSE;
    }

    // Revert to original state object
    SAFE_RELEASE( Data.pRasterizer[Index].pRasterizerObject );
    Data.pRasterizer[Index].pRasterizerObject = pMemberData[Index].Data.pD3DEffectsManagedRasterizerState;
    pMemberData[Index].Data.pD3DEffectsManagedRasterizerState = NULL;
    Data.pRasterizer[Index].IsUserManaged = FALSE;

lExit:
    return hr;
}

template<typename IBaseInterface>
HRESULT TRasterizerVariable<IBaseInterface>::GetBackingStore(UINT Index, D3D11_RASTERIZER_DESC *pRasterizerDesc)
{
    LPCSTR pFuncName = "ID3DX11EffectRasterizerVariable::GetBackingStore";

    CHECK_OBJECT_SCALAR_BOUNDS(Index, pRasterizerDesc);

    if( Data.pRasterizer[Index].IsUserManaged )
    {
        if( Data.pRasterizer[Index].pRasterizerObject )
        {
            Data.pRasterizer[Index].pRasterizerObject->GetDesc( pRasterizerDesc );
        }
        else
        {
            *pRasterizerDesc = CD3D11_RASTERIZER_DESC( D3D11_DEFAULT );
        }
    }
    else
    {
        SRasterizerBlock *pBlock = Data.pRasterizer + Index;
        if (pBlock->ApplyAssignments(GetTopLevelEntity()->pEffect))
        {
            pBlock->pAssignments[0].LastRecomputedTime = 0; // Force a recreate of this block the next time ApplyRenderStateBlock is called
        }

        memcpy(pRasterizerDesc, &pBlock->BackingStore, sizeof(D3D11_RASTERIZER_DESC));
    }

lExit:
    return hr;
}

template<typename IBaseInterface>
BOOL TRasterizerVariable<IBaseInterface>::IsValid()
{
    UINT numElements = IsArray()? pType->Elements : 1;
    BOOL valid = TRUE;
    while( numElements > 0 && ( valid = Data.pRasterizer[ numElements-1 ].IsValid ) )
        numElements--;
    return valid;
}

////////////////////////////////////////////////////////////////////////////////
// ID3DX11EffectSamplerVariable (TSamplerVariable implementation)
////////////////////////////////////////////////////////////////////////////////

template<typename IBaseInterface>
struct TSamplerVariable : public IBaseInterface
{
public:

    STDMETHOD(GetSampler)(UINT Index, ID3D11SamplerState **ppSampler);
    STDMETHOD(SetSampler)(UINT Index, ID3D11SamplerState *pSampler);
    STDMETHOD(UndoSetSampler)(UINT Index);
    STDMETHOD(GetBackingStore)(UINT Index, D3D11_SAMPLER_DESC *pSamplerDesc);
};

template<typename IBaseInterface>
HRESULT TSamplerVariable<IBaseInterface>::GetSampler(UINT Index, ID3D11SamplerState **ppSampler)
{
    LPCSTR pFuncName = "ID3DX11EffectSamplerVariable::GetSampler";

    CHECK_OBJECT_SCALAR_BOUNDS(Index, ppSampler);

    *ppSampler = Data.pSampler[Index].pD3DObject;
    SAFE_ADDREF(*ppSampler);

lExit:
    return hr;
}

template<typename IBaseInterface>
HRESULT TSamplerVariable<IBaseInterface>::SetSampler(UINT Index, ID3D11SamplerState *pSampler)
{
    LPCSTR pFuncName = "ID3DX11EffectSamplerState::SetSampler";

    CHECK_SCALAR_BOUNDS(Index);

    // Replace all references to the old shader block with this one
    GetEffect()->ReplaceSamplerReference(&Data.pSampler[Index], pSampler);

    if( !Data.pSampler[Index].IsUserManaged )
    {
        // Save original state object in case we UndoSet
        D3DXASSERT( pMemberData[Index].Type == MDT_SamplerState );
        VB( pMemberData[Index].Data.pD3DEffectsManagedSamplerState == NULL );
        pMemberData[Index].Data.pD3DEffectsManagedSamplerState = Data.pSampler[Index].pD3DObject;
        Data.pSampler[Index].pD3DObject = NULL;
        Data.pSampler[Index].IsUserManaged = TRUE;
    }

    SAFE_ADDREF( pSampler );
    SAFE_RELEASE( Data.pSampler[Index].pD3DObject );
    Data.pSampler[Index].pD3DObject = pSampler;
lExit:
    return hr;
}

template<typename IBaseInterface>
HRESULT TSamplerVariable<IBaseInterface>::UndoSetSampler(UINT Index)
{
    LPCSTR pFuncName = "ID3DX11EffectSamplerState::UndoSetSampler";

    CHECK_SCALAR_BOUNDS(Index);

    if( !Data.pSampler[Index].IsUserManaged )
    {
        return S_FALSE;
    }

    // Replace all references to the old shader block with this one
    GetEffect()->ReplaceSamplerReference(&Data.pSampler[Index], pMemberData[Index].Data.pD3DEffectsManagedSamplerState);

    // Revert to original state object
    SAFE_RELEASE( Data.pSampler[Index].pD3DObject );
    Data.pSampler[Index].pD3DObject = pMemberData[Index].Data.pD3DEffectsManagedSamplerState;
    pMemberData[Index].Data.pD3DEffectsManagedSamplerState = NULL;
    Data.pSampler[Index].IsUserManaged = FALSE;

lExit:
    return hr;
}

template<typename IBaseInterface>
HRESULT TSamplerVariable<IBaseInterface>::GetBackingStore(UINT Index, D3D11_SAMPLER_DESC *pSamplerDesc)
{
    LPCSTR pFuncName = "ID3DX11EffectSamplerVariable::GetBackingStore";

    CHECK_OBJECT_SCALAR_BOUNDS(Index, pSamplerDesc);

    if( Data.pSampler[Index].IsUserManaged )
    {
        if( Data.pSampler[Index].pD3DObject )
        {
            Data.pSampler[Index].pD3DObject->GetDesc( pSamplerDesc );
        }
        else
        {
            *pSamplerDesc = CD3D11_SAMPLER_DESC( D3D11_DEFAULT );
        }
    }
    else
    {
        SSamplerBlock *pBlock = Data.pSampler + Index;
        if (pBlock->ApplyAssignments(GetTopLevelEntity()->pEffect))
        {
            pBlock->pAssignments[0].LastRecomputedTime = 0; // Force a recreate of this block the next time ApplyRenderStateBlock is called
        }

        memcpy(pSamplerDesc, &pBlock->BackingStore.SamplerDesc, sizeof(D3D11_SAMPLER_DESC));
    }

lExit:
    return hr;
}

////////////////////////////////////////////////////////////////////////////////
// TUncastableVariable
////////////////////////////////////////////////////////////////////////////////

template<typename IBaseInterface>
struct TUncastableVariable : public IBaseInterface
{
    STDMETHOD_(ID3DX11EffectScalarVariable*, AsScalar)();
    STDMETHOD_(ID3DX11EffectVectorVariable*, AsVector)();
    STDMETHOD_(ID3DX11EffectMatrixVariable*, AsMatrix)();
    STDMETHOD_(ID3DX11EffectStringVariable*, AsString)();
    STDMETHOD_(ID3DX11EffectClassInstanceVariable*, AsClassInstance)();
    STDMETHOD_(ID3DX11EffectInterfaceVariable*, AsInterface)();
    STDMETHOD_(ID3DX11EffectShaderResourceVariable*, AsShaderResource)();
    STDMETHOD_(ID3DX11EffectUnorderedAccessViewVariable*, AsUnorderedAccessView)();
    STDMETHOD_(ID3DX11EffectRenderTargetViewVariable*, AsRenderTargetView)();
    STDMETHOD_(ID3DX11EffectDepthStencilViewVariable*, AsDepthStencilView)();
    STDMETHOD_(ID3DX11EffectConstantBuffer*, AsConstantBuffer)();
    STDMETHOD_(ID3DX11EffectShaderVariable*, AsShader)();
    STDMETHOD_(ID3DX11EffectBlendVariable*, AsBlend)();
    STDMETHOD_(ID3DX11EffectDepthStencilVariable*, AsDepthStencil)();
    STDMETHOD_(ID3DX11EffectRasterizerVariable*, AsRasterizer)();
    STDMETHOD_(ID3DX11EffectSamplerVariable*, AsSampler)();
};

template<typename IBaseInterface>
ID3DX11EffectScalarVariable * TUncastableVariable<IBaseInterface>::AsScalar()
{
    LPCSTR pFuncName = "ID3DX11EffectVariable::AsScalar";
    DPF(0, "%s: Invalid typecast", pFuncName);
    return &g_InvalidScalarVariable;
}

template<typename IBaseInterface>
ID3DX11EffectVectorVariable * TUncastableVariable<IBaseInterface>::AsVector()
{
    LPCSTR pFuncName = "ID3DX11EffectVariable::AsVector";
    DPF(0, "%s: Invalid typecast", pFuncName);
    return &g_InvalidVectorVariable;
}

template<typename IBaseInterface>
ID3DX11EffectMatrixVariable * TUncastableVariable<IBaseInterface>::AsMatrix()
{
    LPCSTR pFuncName = "ID3DX11EffectVariable::AsMatrix";
    DPF(0, "%s: Invalid typecast", pFuncName);
    return &g_InvalidMatrixVariable;
}

template<typename IBaseInterface>
ID3DX11EffectStringVariable * TUncastableVariable<IBaseInterface>::AsString()
{
    LPCSTR pFuncName = "ID3DX11EffectVariable::AsString";
    DPF(0, "%s: Invalid typecast", pFuncName);
    return &g_InvalidStringVariable;
}

template<typename IBaseClassInstance>
ID3DX11EffectClassInstanceVariable * TUncastableVariable<IBaseClassInstance>::AsClassInstance()
{
    LPCSTR pFuncName = "ID3DX11EffectVariable::AsClassInstance";
    DPF(0, "%s: Invalid typecast", pFuncName);
    return &g_InvalidClassInstanceVariable;
}

template<typename IBaseInterface>
ID3DX11EffectInterfaceVariable * TUncastableVariable<IBaseInterface>::AsInterface()
{
    LPCSTR pFuncName = "ID3DX11EffectVariable::AsInterface";
    DPF(0, "%s: Invalid typecast", pFuncName);
    return &g_InvalidInterfaceVariable;
}

template<typename IBaseInterface>
ID3DX11EffectShaderResourceVariable * TUncastableVariable<IBaseInterface>::AsShaderResource()
{
    LPCSTR pFuncName = "ID3DX11EffectVariable::AsShaderResource";
    DPF(0, "%s: Invalid typecast", pFuncName);
    return &g_InvalidShaderResourceVariable;
}

template<typename IBaseInterface>
ID3DX11EffectUnorderedAccessViewVariable * TUncastableVariable<IBaseInterface>::AsUnorderedAccessView()
{
    LPCSTR pFuncName = "ID3DX11EffectVariable::AsUnorderedAccessView";
    DPF(0, "%s: Invalid typecast", pFuncName);
    return &g_InvalidUnorderedAccessViewVariable;
}

template<typename IBaseInterface>
ID3DX11EffectRenderTargetViewVariable * TUncastableVariable<IBaseInterface>::AsRenderTargetView()
{
    LPCSTR pFuncName = "ID3DX11EffectVariable::AsRenderTargetView";
    DPF(0, "%s: Invalid typecast", pFuncName);
    return &g_InvalidRenderTargetViewVariable;
}

template<typename IBaseInterface>
ID3DX11EffectDepthStencilViewVariable * TUncastableVariable<IBaseInterface>::AsDepthStencilView()
{
    LPCSTR pFuncName = "ID3DX11EffectVariable::AsDepthStencilView";
    DPF(0, "%s: Invalid typecast", pFuncName);
    return &g_InvalidDepthStencilViewVariable;
}

template<typename IBaseInterface>
ID3DX11EffectConstantBuffer * TUncastableVariable<IBaseInterface>::AsConstantBuffer()
{
    LPCSTR pFuncName = "ID3DX11EffectVariable::AsConstantBuffer";
    DPF(0, "%s: Invalid typecast", pFuncName);
    return &g_InvalidConstantBuffer;
}

template<typename IBaseInterface>
ID3DX11EffectShaderVariable * TUncastableVariable<IBaseInterface>::AsShader()
{
    LPCSTR pFuncName = "ID3DX11EffectVariable::AsShader";
    DPF(0, "%s: Invalid typecast", pFuncName);
    return &g_InvalidShaderVariable;
}

template<typename IBaseInterface>
ID3DX11EffectBlendVariable * TUncastableVariable<IBaseInterface>::AsBlend()
{
    LPCSTR pFuncName = "ID3DX11EffectVariable::AsBlend";
    DPF(0, "%s: Invalid typecast", pFuncName);
    return &g_InvalidBlendVariable;
}

template<typename IBaseInterface>
ID3DX11EffectDepthStencilVariable * TUncastableVariable<IBaseInterface>::AsDepthStencil()
{
    LPCSTR pFuncName = "ID3DX11EffectVariable::AsDepthStencil";
    DPF(0, "%s: Invalid typecast", pFuncName);
    return &g_InvalidDepthStencilVariable;
}

template<typename IBaseInterface>
ID3DX11EffectRasterizerVariable * TUncastableVariable<IBaseInterface>::AsRasterizer()
{
    LPCSTR pFuncName = "ID3DX11EffectVariable::AsRasterizer";
    DPF(0, "%s: Invalid typecast", pFuncName);
    return &g_InvalidRasterizerVariable;
}

template<typename IBaseInterface>
ID3DX11EffectSamplerVariable * TUncastableVariable<IBaseInterface>::AsSampler()
{
    LPCSTR pFuncName = "ID3DX11EffectVariable::AsSampler";
    DPF(0, "%s: Invalid typecast", pFuncName);
    return &g_InvalidSamplerVariable;
}

////////////////////////////////////////////////////////////////////////////////
// Macros to instantiate the myriad templates
////////////////////////////////////////////////////////////////////////////////

// generates a global variable, annotation, global variable member, and annotation member of each struct type
#define GenerateReflectionClasses(Type, BaseInterface) \
struct S##Type##GlobalVariable : public T##Type##Variable<TGlobalVariable<BaseInterface>, FALSE> { }; \
struct S##Type##Annotation : public T##Type##Variable<TAnnotation<BaseInterface>, TRUE> { }; \
struct S##Type##GlobalVariableMember : public T##Type##Variable<TVariable<TMember<BaseInterface> >, FALSE> { }; \
struct S##Type##AnnotationMember : public T##Type##Variable<TVariable<TMember<BaseInterface> >, TRUE> { };

#define GenerateVectorReflectionClasses(Type, BaseType, BaseInterface) \
struct S##Type##GlobalVariable : public TVectorVariable<TGlobalVariable<BaseInterface>, FALSE, BaseType> { }; \
struct S##Type##Annotation : public TVectorVariable<TAnnotation<BaseInterface>, TRUE, BaseType> { }; \
struct S##Type##GlobalVariableMember : public TVectorVariable<TVariable<TMember<BaseInterface> >, FALSE, BaseType> { }; \
struct S##Type##AnnotationMember : public TVectorVariable<TVariable<TMember<BaseInterface> >, TRUE, BaseType> { };

#define GenerateReflectionGlobalOnlyClasses(Type) \
struct S##Type##GlobalVariable : public T##Type##Variable<TGlobalVariable<ID3DX11Effect##Type##Variable> > { }; \
struct S##Type##GlobalVariableMember : public T##Type##Variable<TVariable<TMember<ID3DX11Effect##Type##Variable> > > { }; \

GenerateReflectionClasses(Numeric, ID3DX11EffectVariable);
GenerateReflectionClasses(FloatScalar, ID3DX11EffectScalarVariable);
GenerateReflectionClasses(IntScalar, ID3DX11EffectScalarVariable);
GenerateReflectionClasses(BoolScalar, ID3DX11EffectScalarVariable);
GenerateVectorReflectionClasses(FloatVector, ETVT_Float, ID3DX11EffectVectorVariable);
GenerateVectorReflectionClasses(BoolVector, ETVT_Bool, ID3DX11EffectVectorVariable);
GenerateVectorReflectionClasses(IntVector, ETVT_Int, ID3DX11EffectVectorVariable);
GenerateReflectionClasses(Matrix, ID3DX11EffectMatrixVariable);
GenerateReflectionClasses(String, ID3DX11EffectStringVariable);
GenerateReflectionGlobalOnlyClasses(ClassInstance);
GenerateReflectionGlobalOnlyClasses(Interface);
GenerateReflectionGlobalOnlyClasses(ShaderResource);
GenerateReflectionGlobalOnlyClasses(UnorderedAccessView);
GenerateReflectionGlobalOnlyClasses(RenderTargetView);
GenerateReflectionGlobalOnlyClasses(DepthStencilView);
GenerateReflectionGlobalOnlyClasses(Shader);
GenerateReflectionGlobalOnlyClasses(Blend);
GenerateReflectionGlobalOnlyClasses(DepthStencil);
GenerateReflectionGlobalOnlyClasses(Rasterizer);
GenerateReflectionGlobalOnlyClasses(Sampler);

// Optimized matrix classes
struct SMatrix4x4ColumnMajorGlobalVariable : public TMatrix4x4Variable<TGlobalVariable<ID3DX11EffectMatrixVariable>, TRUE> { };
struct SMatrix4x4RowMajorGlobalVariable : public TMatrix4x4Variable<TGlobalVariable<ID3DX11EffectMatrixVariable>, FALSE> { };

struct SMatrix4x4ColumnMajorGlobalVariableMember : public TMatrix4x4Variable<TVariable<TMember<ID3DX11EffectMatrixVariable> >, TRUE> { };
struct SMatrix4x4RowMajorGlobalVariableMember : public TMatrix4x4Variable<TVariable<TMember<ID3DX11EffectMatrixVariable> >, FALSE> { };

// Optimized vector classes
struct SFloatVector4GlobalVariable : public TVector4Variable<TGlobalVariable<ID3DX11EffectVectorVariable> > { };
struct SFloatVector4GlobalVariableMember : public TVector4Variable<TVariable<TMember<ID3DX11EffectVectorVariable> > > { };

// These 3 classes should never be used directly

// The "base" global variable struct (all global variables should be the same size in bytes,
// but we pick this as the default).  
struct SGlobalVariable : public TGlobalVariable<ID3DX11EffectVariable>
{

};

// The "base" annotation struct (all annotations should be the same size in bytes,
// but we pick this as the default).
struct SAnnotation : public TAnnotation<ID3DX11EffectVariable>
{

};

// The "base" variable member struct (all annotation/global variable members should be the
// same size in bytes, but we pick this as the default).
struct SMember : public TVariable<TMember<ID3DX11EffectVariable> >
{

};

// creates a new variable of the appropriate polymorphic type where pVar was
HRESULT PlacementNewVariable(void *pVar, SType *pType, BOOL IsAnnotation);
SMember * CreateNewMember(SType *pType, BOOL IsAnnotation);


