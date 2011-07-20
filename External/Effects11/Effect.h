//////////////////////////////////////////////////////////////////////////////
//
//  Copyright (C) Microsoft Corporation.  All Rights Reserved.
//
//  File:       Effect.h
//  Content:    D3DX11 Effects Header for ID3DX11Effect Implementation
//
//////////////////////////////////////////////////////////////////////////////

#pragma once

#include "EffectBinaryFormat.h"

using namespace D3DX11Core;

namespace D3DX11Effects
{

//////////////////////////////////////////////////////////////////////////
// Forward defines
//////////////////////////////////////////////////////////////////////////

struct SBaseBlock;
struct SShaderBlock;
struct SPassBlock;
struct SClassInstance;
struct SInterface;
struct SShaderResource;
struct SUnorderedAccessView;
struct SRenderTargetView;
struct SDepthStencilView;
struct SSamplerBlock;
struct SDepthStencilBlock;
struct SBlendBlock;
struct SRasterizerBlock;
struct SString;
struct SD3DShaderVTable;
struct SClassInstanceGlobalVariable;

struct SAssignment;
struct SVariable;
struct SGlobalVariable;
struct SAnnotation;
struct SConstantBuffer;

class CEffect;
class CEffectLoader;

enum ELhsType;

// Allows the use of 32-bit and 64-bit timers depending on platform type
typedef SIZE_T Timer;

//////////////////////////////////////////////////////////////////////////
// Reflection & Type structures
//////////////////////////////////////////////////////////////////////////

// CEffectMatrix is used internally instead of float arrays
struct CEffectMatrix 
{
    union 
    {
        struct 
        {
            float        _11, _12, _13, _14;
            float        _21, _22, _23, _24;
            float        _31, _32, _33, _34;
            float        _41, _42, _43, _44;

        };
        float m[4][4];
    };
};

struct CEffectVector4 
{
    float x;
    float y;
    float z;
    float w;
};

union UDataPointer
{
    void                    *pGeneric;
    BYTE                    *pNumeric; 
    float                   *pNumericFloat;
    UINT                    *pNumericDword;
    int                     *pNumericInt;
    BOOL                    *pNumericBool;
    SString                 *pString;
    SShaderBlock            *pShader;
    SBaseBlock              *pBlock;
    SBlendBlock             *pBlend;
    SDepthStencilBlock      *pDepthStencil;
    SRasterizerBlock        *pRasterizer;
    SInterface              *pInterface;
    SShaderResource         *pShaderResource;
    SUnorderedAccessView    *pUnorderedAccessView;
    SRenderTargetView       *pRenderTargetView;
    SDepthStencilView       *pDepthStencilView;
    SSamplerBlock           *pSampler;
    CEffectVector4          *pVector;
    CEffectMatrix           *pMatrix;
    UINT_PTR                Offset;
};

enum EMemberDataType
{
    MDT_ClassInstance,
    MDT_BlendState,
    MDT_DepthStencilState,
    MDT_RasterizerState,
    MDT_SamplerState,
    MDT_Buffer,
    MDT_ShaderResourceView,
};

struct SMemberDataPointer
{
    EMemberDataType             Type;
    union
    {
        IUnknown                *pGeneric;
        ID3D11ClassInstance     *pD3DClassInstance;
        ID3D11BlendState        *pD3DEffectsManagedBlendState;
        ID3D11DepthStencilState *pD3DEffectsManagedDepthStencilState;
        ID3D11RasterizerState   *pD3DEffectsManagedRasterizerState;
        ID3D11SamplerState      *pD3DEffectsManagedSamplerState;
        ID3D11Buffer            *pD3DEffectsManagedConstantBuffer;
        ID3D11ShaderResourceView*pD3DEffectsManagedTextureBuffer;
    } Data;
};

struct SType : public ID3DX11EffectType
{   
    static const UINT_PTR c_InvalidIndex = (UINT) -1;
    static const UINT c_ScalarSize = sizeof(UINT);

    // packing rule constants
    static const UINT c_ScalarsPerRegister = 4;
    static const UINT c_RegisterSize = c_ScalarsPerRegister * c_ScalarSize; // must be a power of 2!!    
    
    EVarType    VarType;        // numeric, object, struct
    UINT        Elements;       // # of array elements (0 for non-arrays)
    char        *pTypeName;     // friendly name of the type: "VS_OUTPUT", "float4", etc.

    // *Size and stride values are always 0 for object types
    // *Annotations adhere to packing rules (even though they do not reside in constant buffers)
    //      for consistency's sake
    //
    // Packing rules:
    // *Structures and array elements are always register aligned
    // *Single-row values (or, for column major matrices, single-column) are greedily
    //  packed unless doing so would span a register boundary, in which case they are
    //  register aligned

    UINT        TotalSize;      // Total size of this data type in a constant buffer from
                                // start to finish (padding in between elements is included,
                                // but padding at the end is not since that would require
                                // knowledge of the following data type).

    UINT        Stride;         // Number of bytes to advance between elements.
                                // Typically a multiple of 16 for arrays, vectors, matrices.
                                // For scalars and small vectors/matrices, this can be 4 or 8.    

    UINT        PackedSize;     // Size, in bytes, of this data typed when fully packed

    union
    {        
        SBinaryNumericType  NumericType;
        EObjectType         ObjectType;         // not all values of EObjectType are valid here (e.g. constant buffer)
        struct
        {
            SVariable   *pMembers;              // array of type instances describing structure members
            UINT        Members;
            bool        ImplementsInterface;    // true if this type implements an interface
            bool        HasSuperClass;          // true if this type has a parent class
        }                   StructType;
        void*               InterfaceType;      // nothing for interfaces
    };


    SType() :
       VarType(EVT_Invalid),
       Elements(0),
       pTypeName(NULL),
       TotalSize(0),
       Stride(0),
       PackedSize(0)
    {
        C_ASSERT( sizeof(NumericType) <= sizeof(StructType) );
        C_ASSERT( sizeof(ObjectType) <= sizeof(StructType) );
        C_ASSERT( sizeof(InterfaceType) <= sizeof(StructType) );
        ZeroMemory( &StructType, sizeof(StructType) );
    }

    BOOL IsEqual(SType *pOtherType) const;
    
    BOOL IsObjectType(EObjectType ObjType) const
    {
        return IsObjectTypeHelper(VarType, ObjectType, ObjType);
    }
    BOOL IsShader() const
    {
        return IsShaderHelper(VarType, ObjectType);
    }
    BOOL BelongsInConstantBuffer() const
    {
        return (VarType == EVT_Numeric) || (VarType == EVT_Struct);
    }
    BOOL IsStateBlockObject() const
    {
        return IsStateBlockObjectHelper(VarType, ObjectType);
    }
    BOOL IsClassInstance() const
    {
        return (VarType == EVT_Struct) && StructType.ImplementsInterface;
    }
    BOOL IsInterface() const
    {
        return IsInterfaceHelper(VarType, ObjectType);
    }
    BOOL IsShaderResource() const
    {
        return IsShaderResourceHelper(VarType, ObjectType);
    }
    BOOL IsUnorderedAccessView() const
    {
        return IsUnorderedAccessViewHelper(VarType, ObjectType);
    }
    BOOL IsSampler() const
    {
        return IsSamplerHelper(VarType, ObjectType);
    }
    BOOL IsRenderTargetView() const
    {
        return IsRenderTargetViewHelper(VarType, ObjectType);
    }
    BOOL IsDepthStencilView() const
    {
        return IsDepthStencilViewHelper(VarType, ObjectType);
    }

    UINT GetTotalUnpackedSize(BOOL IsSingleElement) const; 
    UINT GetTotalPackedSize(BOOL IsSingleElement) const; 
    HRESULT GetDescHelper(D3DX11_EFFECT_TYPE_DESC *pDesc, BOOL IsSingleElement) const;

    STDMETHOD_(BOOL, IsValid)() { return TRUE; }
    STDMETHOD(GetDesc)(D3DX11_EFFECT_TYPE_DESC *pDesc) { return GetDescHelper(pDesc, FALSE); }
    STDMETHOD_(ID3DX11EffectType*, GetMemberTypeByIndex)(UINT Index);
    STDMETHOD_(ID3DX11EffectType*, GetMemberTypeByName)(LPCSTR Name);
    STDMETHOD_(ID3DX11EffectType*, GetMemberTypeBySemantic)(LPCSTR Semantic);
    STDMETHOD_(LPCSTR, GetMemberName)(UINT Index);
    STDMETHOD_(LPCSTR, GetMemberSemantic)(UINT Index);
};

// Represents a type structure for a single element.
// It seems pretty trivial, but it has a different virtual table which enables
// us to accurately represent a type that consists of a single element
struct SSingleElementType : public ID3DX11EffectType
{
    SType *pType;

    STDMETHOD_(BOOL, IsValid)() { return TRUE; }
    STDMETHOD(GetDesc)(D3DX11_EFFECT_TYPE_DESC *pDesc) { return ((SType*)pType)->GetDescHelper(pDesc, TRUE); }
    STDMETHOD_(ID3DX11EffectType*, GetMemberTypeByIndex)(UINT Index) { return ((SType*)pType)->GetMemberTypeByIndex(Index); }
    STDMETHOD_(ID3DX11EffectType*, GetMemberTypeByName)(LPCSTR Name) { return ((SType*)pType)->GetMemberTypeByName(Name); }
    STDMETHOD_(ID3DX11EffectType*, GetMemberTypeBySemantic)(LPCSTR Semantic) { return ((SType*)pType)->GetMemberTypeBySemantic(Semantic); }
    STDMETHOD_(LPCSTR, GetMemberName)(UINT Index) { return ((SType*)pType)->GetMemberName(Index); }
    STDMETHOD_(LPCSTR, GetMemberSemantic)(UINT Index) { return ((SType*)pType)->GetMemberSemantic(Index); }
};

//////////////////////////////////////////////////////////////////////////
// Block definitions
//////////////////////////////////////////////////////////////////////////

void * GetBlockByIndex(EVarType VarType, EObjectType ObjectType, void *pBaseBlock, UINT Index);

struct SBaseBlock
{
    EBlockType      BlockType;

    BOOL            IsUserManaged:1;

    UINT            AssignmentCount;
    SAssignment     *pAssignments;

    SBaseBlock();

    BOOL ApplyAssignments(CEffect *pEffect);

    D3DX11INLINE SSamplerBlock *AsSampler() const
    {
        D3DXASSERT( BlockType == EBT_Sampler );
        return (SSamplerBlock*) this;
    }

    D3DX11INLINE SDepthStencilBlock *AsDepthStencil() const
    {
        D3DXASSERT( BlockType == EBT_DepthStencil );
        return (SDepthStencilBlock*) this;
    }

    D3DX11INLINE SBlendBlock *AsBlend() const
    {
        D3DXASSERT( BlockType == EBT_Blend );
        return (SBlendBlock*) this;
    }

    D3DX11INLINE SRasterizerBlock *AsRasterizer() const
    {
        D3DXASSERT( BlockType == EBT_Rasterizer );
        return (SRasterizerBlock*) this;
    }

    D3DX11INLINE SPassBlock *AsPass() const
    {
        D3DXASSERT( BlockType == EBT_Pass );
        return (SPassBlock*) this;
    }
};

struct STechnique : public ID3DX11EffectTechnique
{
    char        *pName;

    UINT        PassCount;
    SPassBlock  *pPasses;

    UINT        AnnotationCount;
    SAnnotation *pAnnotations;

    BOOL        InitiallyValid;
    BOOL        HasDependencies;

    STechnique();

    STDMETHOD_(BOOL, IsValid)();
    STDMETHOD(GetDesc)(D3DX11_TECHNIQUE_DESC *pDesc);

    STDMETHOD_(ID3DX11EffectVariable*, GetAnnotationByIndex)(UINT Index);
    STDMETHOD_(ID3DX11EffectVariable*, GetAnnotationByName)(LPCSTR Name);

    STDMETHOD_(ID3DX11EffectPass*, GetPassByIndex)(UINT Index);
    STDMETHOD_(ID3DX11EffectPass*, GetPassByName)(LPCSTR Name);

    STDMETHOD(ComputeStateBlockMask)(D3DX11_STATE_BLOCK_MASK *pStateBlockMask);
};

struct SGroup : public ID3DX11EffectGroup
{
    char        *pName;

    UINT        TechniqueCount;
    STechnique  *pTechniques;

    UINT        AnnotationCount;
    SAnnotation *pAnnotations;

    BOOL        InitiallyValid;
    BOOL        HasDependencies;

    SGroup();

    STDMETHOD_(BOOL, IsValid)();
    STDMETHOD(GetDesc)(D3DX11_GROUP_DESC *pDesc);

    STDMETHOD_(ID3DX11EffectVariable*, GetAnnotationByIndex)(UINT Index);
    STDMETHOD_(ID3DX11EffectVariable*, GetAnnotationByName)(LPCSTR Name);

    STDMETHOD_(ID3DX11EffectTechnique*, GetTechniqueByIndex)(UINT Index);
    STDMETHOD_(ID3DX11EffectTechnique*, GetTechniqueByName)(LPCSTR Name);
};

struct SPassBlock : SBaseBlock, public ID3DX11EffectPass
{
    struct
    {
        ID3D11BlendState*       pBlendState;
        FLOAT                   BlendFactor[4];
        UINT                    SampleMask;
        ID3D11DepthStencilState *pDepthStencilState;
        UINT                    StencilRef;
        union
        {
            D3D11_SO_DECLARATION_ENTRY  *pEntry;
            char                        *pEntryDesc;
        }                       GSSODesc;

        // Pass assignments can write directly into these
        SBlendBlock             *pBlendBlock;
        SDepthStencilBlock      *pDepthStencilBlock;
        SRasterizerBlock        *pRasterizerBlock;
        UINT                    RenderTargetViewCount;
        SRenderTargetView       *pRenderTargetViews[D3D11_SIMULTANEOUS_RENDER_TARGET_COUNT];
        SDepthStencilView       *pDepthStencilView;
        SShaderBlock            *pVertexShaderBlock;
        SShaderBlock            *pPixelShaderBlock;
        SShaderBlock            *pGeometryShaderBlock;
        SShaderBlock            *pComputeShaderBlock;
        SShaderBlock            *pDomainShaderBlock;
        SShaderBlock            *pHullShaderBlock;
    }           BackingStore;

    char        *pName;

    UINT        AnnotationCount;
    SAnnotation *pAnnotations;

    CEffect     *pEffect;

    BOOL        InitiallyValid;         // validity of all state objects and shaders in pass upon BindToDevice
    BOOL        HasDependencies;        // if pass expressions or pass state blocks have dependencies on variables (if true, IsValid != InitiallyValid possibly)

    SPassBlock();

    void ApplyPassAssignments();
    BOOL CheckShaderDependencies( SShaderBlock* pBlock );
    BOOL CheckDependencies();

    template<EObjectType EShaderType>
    HRESULT GetShaderDescHelper(D3DX11_PASS_SHADER_DESC *pDesc);

    STDMETHOD_(BOOL, IsValid)();
    STDMETHOD(GetDesc)(D3DX11_PASS_DESC *pDesc);

    STDMETHOD(GetVertexShaderDesc)(D3DX11_PASS_SHADER_DESC *pDesc);
    STDMETHOD(GetGeometryShaderDesc)(D3DX11_PASS_SHADER_DESC *pDesc);
    STDMETHOD(GetPixelShaderDesc)(D3DX11_PASS_SHADER_DESC *pDesc);
    STDMETHOD(GetHullShaderDesc)(D3DX11_PASS_SHADER_DESC *pDesc);
    STDMETHOD(GetDomainShaderDesc)(D3DX11_PASS_SHADER_DESC *pDesc);
    STDMETHOD(GetComputeShaderDesc)(D3DX11_PASS_SHADER_DESC *pDesc);

    STDMETHOD_(ID3DX11EffectVariable*, GetAnnotationByIndex)(UINT Index);
    STDMETHOD_(ID3DX11EffectVariable*, GetAnnotationByName)(LPCSTR Name);

    STDMETHOD(Apply)(UINT Flags, ID3D11DeviceContext* pContext);
    
    STDMETHOD(ComputeStateBlockMask)(D3DX11_STATE_BLOCK_MASK *pStateBlockMask);
};

struct SDepthStencilBlock : SBaseBlock
{
    ID3D11DepthStencilState *pDSObject;
    D3D11_DEPTH_STENCIL_DESC BackingStore;
    BOOL                     IsValid;

    SDepthStencilBlock();
};

struct SBlendBlock : SBaseBlock
{
    ID3D11BlendState        *pBlendObject;
    D3D11_BLEND_DESC        BackingStore;
    BOOL                    IsValid;

    SBlendBlock();
};

struct SRasterizerBlock : SBaseBlock
{
    ID3D11RasterizerState   *pRasterizerObject;
    D3D11_RASTERIZER_DESC   BackingStore;
    BOOL                    IsValid;

    SRasterizerBlock();
};

struct SSamplerBlock : SBaseBlock
{
    ID3D11SamplerState      *pD3DObject;
    struct
    {
        D3D11_SAMPLER_DESC  SamplerDesc;
        // Sampler "TEXTURE" assignments can write directly into this
        SShaderResource     *pTexture;
    } BackingStore;

    SSamplerBlock();
};

struct SInterface
{
    SClassInstanceGlobalVariable* pClassInstance;

    SInterface()
    {
        pClassInstance = NULL;
    }
};

struct SShaderResource
{
    ID3D11ShaderResourceView *pShaderResource;

    SShaderResource() 
    {
        pShaderResource = NULL;
    }

};

struct SUnorderedAccessView
{
    ID3D11UnorderedAccessView *pUnorderedAccessView;

    SUnorderedAccessView() 
    {
        pUnorderedAccessView = NULL;
    }

};

struct SRenderTargetView
{
    ID3D11RenderTargetView *pRenderTargetView;

    SRenderTargetView();
};

struct SDepthStencilView
{
    ID3D11DepthStencilView *pDepthStencilView;

    SDepthStencilView();
};


template<class T, class D3DTYPE> struct SShaderDependency
{
    UINT    StartIndex;
    UINT    Count;

    T       *ppFXPointers;              // Array of ptrs to FX objects (CBs, SShaderResources, etc)
    D3DTYPE *ppD3DObjects;              // Array of ptrs to matching D3D objects

    SShaderDependency()
    {
        StartIndex = Count = 0;

        ppD3DObjects = NULL;
        ppFXPointers = NULL;
    }
};

typedef SShaderDependency<SConstantBuffer*, ID3D11Buffer*> SShaderCBDependency;
typedef SShaderDependency<SSamplerBlock*, ID3D11SamplerState*> SShaderSamplerDependency;
typedef SShaderDependency<SShaderResource*, ID3D11ShaderResourceView*> SShaderResourceDependency;
typedef SShaderDependency<SUnorderedAccessView*, ID3D11UnorderedAccessView*> SUnorderedAccessViewDependency;
typedef SShaderDependency<SInterface*, ID3D11ClassInstance*> SInterfaceDependency;

// Shader VTables are used to eliminate branching in ApplyShaderBlock.
// The effect owns three D3DShaderVTables, one for PS, one for VS, and one for GS.
struct SD3DShaderVTable
{
    void ( __stdcall ID3D11DeviceContext::*pSetShader)(ID3D11DeviceChild* pShader, ID3D11ClassInstance*const* ppClassInstances, UINT NumClassInstances);
    void ( __stdcall ID3D11DeviceContext::*pSetConstantBuffers)(UINT StartConstantSlot, UINT NumBuffers, ID3D11Buffer *const *pBuffers);
    void ( __stdcall ID3D11DeviceContext::*pSetSamplers)(UINT Offset, UINT NumSamplers, ID3D11SamplerState*const* pSamplers);
    void ( __stdcall ID3D11DeviceContext::*pSetShaderResources)(UINT Offset, UINT NumResources, ID3D11ShaderResourceView *const *pResources);
    HRESULT ( __stdcall ID3D11Device::*pCreateShader)(const void *pShaderBlob, SIZE_T ShaderBlobSize, ID3D11ClassLinkage* pClassLinkage, ID3D11DeviceChild **ppShader);
};


struct SShaderBlock
{
    enum ESigType
    {
        ST_Input,
        ST_Output,
        ST_PatchConstant,
    };

    struct SInterfaceParameter
    {
        char                        *pName;
        UINT                        Index;
    };

    // this data is classified as reflection-only and will all be discarded at runtime
    struct SReflectionData
    {
        BYTE                        *pBytecode;
        UINT                        BytecodeLength;
        char                        *pStreamOutDecls[4];        // set with ConstructGSWithSO
        UINT                        RasterizedStream;           // set with ConstructGSWithSO
        BOOL                        IsNullGS;
        ID3D11ShaderReflection      *pReflection;
        UINT                        InterfaceParameterCount;    // set with BindInterfaces (used for function interface parameters)
        SInterfaceParameter         *pInterfaceParameters;      // set with BindInterfaces (used for function interface parameters)
    };

    BOOL                            IsValid;
    SD3DShaderVTable                *pVT;                

    // This value is NULL if the shader is NULL or was never initialized
    SReflectionData                 *pReflectionData;

    ID3D11DeviceChild               *pD3DObject;

    UINT                            CBDepCount;
    SShaderCBDependency             *pCBDeps;

    UINT                            SampDepCount;
    SShaderSamplerDependency        *pSampDeps;

    UINT                            InterfaceDepCount;
    SInterfaceDependency            *pInterfaceDeps;

    UINT                            ResourceDepCount;
    SShaderResourceDependency       *pResourceDeps;

    UINT                            UAVDepCount;
    SUnorderedAccessViewDependency  *pUAVDeps;

    UINT                            TBufferDepCount;
    SConstantBuffer                 **ppTbufDeps;

    ID3DBlob                        *pInputSignatureBlob;   // The input signature is separated from the bytecode because it 
                                                            // is always available, even after Optimize() has been called.

    SShaderBlock(SD3DShaderVTable *pVirtualTable = NULL);

    EObjectType GetShaderType();

    HRESULT OnDeviceBind();

    // Public API helpers
    HRESULT ComputeStateBlockMask(D3DX11_STATE_BLOCK_MASK *pStateBlockMask);

    HRESULT GetShaderDesc(D3DX11_EFFECT_SHADER_DESC *pDesc, BOOL IsInline);

    HRESULT GetVertexShader(ID3D11VertexShader **ppVS);
    HRESULT GetGeometryShader(ID3D11GeometryShader **ppGS);
    HRESULT GetPixelShader(ID3D11PixelShader **ppPS);
    HRESULT GetHullShader(ID3D11HullShader **ppPS);
    HRESULT GetDomainShader(ID3D11DomainShader **ppPS);
    HRESULT GetComputeShader(ID3D11ComputeShader **ppPS);

    HRESULT GetSignatureElementDesc(ESigType SigType, UINT Element, D3D11_SIGNATURE_PARAMETER_DESC *pDesc);
};



struct SString
{
    char *pString;

    SString();
};



//////////////////////////////////////////////////////////////////////////
// Global Variable & Annotation structure/interface definitions
//////////////////////////////////////////////////////////////////////////

//
// This is a general structure that can describe
// annotations, variables, and structure members
//
struct SVariable
{
    // For annotations/variables/variable members:
    // 1) If numeric, pointer to data (for variables: points into backing store,
    //      for annotations, points into reflection heap)
    // OR
    // 2) If object, pointer to the block. If object array, subsequent array elements are found in
    //      contiguous blocks; the Nth block is found by ((<SpecificBlockType> *) pBlock) + N
    //      (this is because variables that are arrays of objects have their blocks allocated contiguously)
    //
    // For structure members:
    //    Offset of this member (in bytes) from parent structure (structure members must be numeric/struct)
    UDataPointer            Data;
    union
    {
        UINT                MemberDataOffsetPlus4;  // 4 added so that 0 == NULL can represent "unused"
        SMemberDataPointer  *pMemberData;
    };

    SType                   *pType;
    char                    *pName;
    char                    *pSemantic;
    UINT                    ExplicitBindPoint;

    SVariable()
    {
        ZeroMemory(this, sizeof(*this));
        ExplicitBindPoint = -1;
    }
};

// Template definitions for all of the various ID3DX11EffectVariable specializations
#include "EffectVariable.inl"


////////////////////////////////////////////////////////////////////////////////
// ID3DX11EffectShaderVariable (SAnonymousShader implementation)
////////////////////////////////////////////////////////////////////////////////

struct SAnonymousShader : public TUncastableVariable<ID3DX11EffectShaderVariable>, public ID3DX11EffectType
{
    SShaderBlock    *pShaderBlock;

    SAnonymousShader(SShaderBlock *pBlock = NULL);

    // ID3DX11EffectShaderVariable interface
    STDMETHOD_(BOOL, IsValid)();
    STDMETHOD_(ID3DX11EffectType*, GetType)();
    STDMETHOD(GetDesc)(D3DX11_EFFECT_VARIABLE_DESC *pDesc);

    STDMETHOD_(ID3DX11EffectVariable*, GetAnnotationByIndex)(UINT Index);
    STDMETHOD_(ID3DX11EffectVariable*, GetAnnotationByName)(LPCSTR Name);

    STDMETHOD_(ID3DX11EffectVariable*, GetMemberByIndex)(UINT Index);
    STDMETHOD_(ID3DX11EffectVariable*, GetMemberByName)(LPCSTR Name);
    STDMETHOD_(ID3DX11EffectVariable*, GetMemberBySemantic)(LPCSTR Semantic);

    STDMETHOD_(ID3DX11EffectVariable*, GetElement)(UINT Index);

    STDMETHOD_(ID3DX11EffectConstantBuffer*, GetParentConstantBuffer)();

    // other casts are handled by TUncastableVariable
    STDMETHOD_(ID3DX11EffectShaderVariable*, AsShader)();

    STDMETHOD(SetRawValue)(CONST void *pData, UINT Offset, UINT Count);
    STDMETHOD(GetRawValue)(__out_bcount(Count) void *pData, UINT Offset, UINT Count);

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

    // ID3DX11EffectType interface
    STDMETHOD(GetDesc)(D3DX11_EFFECT_TYPE_DESC *pDesc);
    STDMETHOD_(ID3DX11EffectType*, GetMemberTypeByIndex)(UINT Index);
    STDMETHOD_(ID3DX11EffectType*, GetMemberTypeByName)(LPCSTR Name);
    STDMETHOD_(ID3DX11EffectType*, GetMemberTypeBySemantic)(LPCSTR Semantic);

    STDMETHOD_(LPCSTR, GetMemberName)(UINT Index);
    STDMETHOD_(LPCSTR, GetMemberSemantic)(UINT Index);
};

////////////////////////////////////////////////////////////////////////////////
// ID3DX11EffectConstantBuffer (SConstantBuffer implementation)
////////////////////////////////////////////////////////////////////////////////

struct SConstantBuffer : public TUncastableVariable<ID3DX11EffectConstantBuffer>, public ID3DX11EffectType
{
    ID3D11Buffer            *pD3DObject;
    SShaderResource         TBuffer;            // NULL iff IsTbuffer == FALSE

    BYTE                    *pBackingStore;
    UINT                    Size;               // in bytes

    char                    *pName;

    UINT                    AnnotationCount;
    SAnnotation             *pAnnotations;

    UINT                    VariableCount;      // # of variables contained in this cbuffer
    SGlobalVariable         *pVariables;        // array of size [VariableCount], points into effect's contiguous variable list
    UINT                    ExplicitBindPoint;  // Used when a CB has been explicitly bound (register(bXX)). -1 if not

    BOOL                    IsDirty:1;          // Set when any member is updated; cleared on CB apply    
    BOOL                    IsTBuffer:1;        // TRUE iff TBuffer.pShaderResource != NULL
    BOOL                    IsUserManaged:1;    // Set if you don't want effects to update this buffer
    BOOL                    IsEffectOptimized:1;// Set if the effect has been optimized
    BOOL                    IsUsedByExpression:1;// Set if used by any expressions
    BOOL                    IsUserPacked:1;     // Set if the elements have user-specified offsets
    BOOL                    IsSingle:1;         // Set to true if you want to share this CB with cloned Effects
    BOOL                    IsNonUpdatable:1;   // Set to true if you want to share this CB with cloned Effects

    union
    {
        // These are used to store the original ID3D11Buffer* for use in UndoSetConstantBuffer
        UINT                MemberDataOffsetPlus4;  // 4 added so that 0 == NULL can represent "unused"
        SMemberDataPointer  *pMemberData;
    };

    CEffect                 *pEffect;

    SConstantBuffer()
    {
        pD3DObject = NULL;
        ZeroMemory(&TBuffer, sizeof(TBuffer));
        ExplicitBindPoint = -1;
        pBackingStore = NULL;
        Size = 0;
        pName = NULL;
        VariableCount = 0;
        pVariables = NULL;
        AnnotationCount = 0;
        pAnnotations = NULL;
        IsDirty = FALSE;
        IsTBuffer = FALSE;
        IsUserManaged = FALSE;
        IsEffectOptimized = FALSE;
        IsUsedByExpression = FALSE;
        IsUserPacked = FALSE;
        IsSingle = FALSE;
        IsNonUpdatable = FALSE;
        pEffect = NULL;
    }

    bool ClonedSingle() const;

    // ID3DX11EffectConstantBuffer interface
    STDMETHOD_(BOOL, IsValid)();
    STDMETHOD_(ID3DX11EffectType*, GetType)();
    STDMETHOD(GetDesc)(D3DX11_EFFECT_VARIABLE_DESC *pDesc);

    STDMETHOD_(ID3DX11EffectVariable*, GetAnnotationByIndex)(UINT Index);
    STDMETHOD_(ID3DX11EffectVariable*, GetAnnotationByName)(LPCSTR Name);

    STDMETHOD_(ID3DX11EffectVariable*, GetMemberByIndex)(UINT Index);
    STDMETHOD_(ID3DX11EffectVariable*, GetMemberByName)(LPCSTR Name);
    STDMETHOD_(ID3DX11EffectVariable*, GetMemberBySemantic)(LPCSTR Semantic);

    STDMETHOD_(ID3DX11EffectVariable*, GetElement)(UINT Index);

    STDMETHOD_(ID3DX11EffectConstantBuffer*, GetParentConstantBuffer)();

    // other casts are handled by TUncastableVariable
    STDMETHOD_(ID3DX11EffectConstantBuffer*, AsConstantBuffer)();

    STDMETHOD(SetRawValue)(CONST void *pData, UINT Offset, UINT Count);
    STDMETHOD(GetRawValue)(__out_bcount(Count) void *pData, UINT Offset, UINT Count);

    STDMETHOD(SetConstantBuffer)(ID3D11Buffer *pConstantBuffer);
    STDMETHOD(GetConstantBuffer)(ID3D11Buffer **ppConstantBuffer);
    STDMETHOD(UndoSetConstantBuffer)();

    STDMETHOD(SetTextureBuffer)(ID3D11ShaderResourceView *pTextureBuffer);
    STDMETHOD(GetTextureBuffer)(ID3D11ShaderResourceView **ppTextureBuffer);
    STDMETHOD(UndoSetTextureBuffer)();

    // ID3DX11EffectType interface
    STDMETHOD(GetDesc)(D3DX11_EFFECT_TYPE_DESC *pDesc);
    STDMETHOD_(ID3DX11EffectType*, GetMemberTypeByIndex)(UINT Index);
    STDMETHOD_(ID3DX11EffectType*, GetMemberTypeByName)(LPCSTR Name);
    STDMETHOD_(ID3DX11EffectType*, GetMemberTypeBySemantic)(LPCSTR Semantic);

    STDMETHOD_(LPCSTR, GetMemberName)(UINT Index);
    STDMETHOD_(LPCSTR, GetMemberSemantic)(UINT Index);
};


//////////////////////////////////////////////////////////////////////////
// Assignments
//////////////////////////////////////////////////////////////////////////

enum ERuntimeAssignmentType
{
    ERAT_Invalid,
    // [Destination] refers to the destination location, which is always the backing store of the pass/state block. 
    // [Source] refers to the current source of data, always coming from either a constant buffer's 
    //  backing store (for numeric assignments), an object variable's block array, or an anonymous (unowned) block

    // Numeric variables:
    ERAT_Constant,                  // Source is unused.
                                    // No dependencies; this assignment can be safely removed after load.
    ERAT_NumericVariable,           // Source points to the CB's backing store where the value lives.
                                    // 1 dependency: the variable itself.
    ERAT_NumericConstIndex,         // Source points to the CB's backing store where the value lives, offset by N.
                                    // 1 dependency: the variable array being indexed.
    ERAT_NumericVariableIndex,      // Source points to the last used element of the variable in the CB's backing store.
                                    // 2 dependencies: the index variable followed by the array variable.

    // Object variables:
    ERAT_ObjectInlineShader,        // An anonymous, immutable shader block pointer is copied to the destination immediately.
                                    // No dependencies; this assignment can be safely removed after load.
    ERAT_ObjectVariable,            // A pointer to the block owned by the object variable is copied to the destination immediately.
                                    // No dependencies; this assignment can be safely removed after load.
    ERAT_ObjectConstIndex,          // A pointer to the Nth block owned by an object variable is copied to the destination immediately.
                                    // No dependencies; this assignment can be safely removed after load.
    ERAT_ObjectVariableIndex,       // Source points to the first block owned by an object variable array
                                    // (the offset from this, N, is taken from another variable).
                                    // 1 dependency: the variable being used to index the array.
};

struct SAssignment
{
    struct SDependency
    {
        SGlobalVariable *pVariable;

        SDependency()
        {
            pVariable = NULL;
        }
    };

    ELhsType                LhsType;            // PS, VS, DepthStencil etc.

    // The value of SAssignment.AssignmentType determines how the other fields behave
    // (DependencyCount, pDependencies, Destination, and Source)
    ERuntimeAssignmentType  AssignmentType;      

    Timer                   LastRecomputedTime;

    // see comments in ERuntimeAssignmentType for how dependencies and data pointers are handled
    UINT                    DependencyCount;
    SDependency             *pDependencies;

    UDataPointer            Destination;        // This value never changes after load, and always refers to the backing store
    UDataPointer            Source;             // This value, on the other hand, can change if variable- or expression- driven

    UINT                    DataSize : 16;      // Size of the data element to be copied in bytes (if numeric) or
                                                // stride of the block type (if object)
    UINT                    MaxElements : 16;   // Max allowable index (needed because we don't store object arrays as dependencies,
                                                // and therefore have no way of getting their Element count)

    BOOL IsObjectAssignment()                   // True for Shader and RObject assignments (the type that appear in pass blocks)
    {
        return IsObjectAssignmentHelper(LhsType);
    }

    SAssignment()
    {
        LhsType = ELHS_Invalid;
        AssignmentType = ERAT_Invalid;

        Destination.pGeneric = NULL;
        Source.pGeneric = NULL;

        LastRecomputedTime = 0;
        DependencyCount = 0;
        pDependencies = NULL;

        DataSize = 0;
    }
};

//////////////////////////////////////////////////////////////////////////
// Private effect heaps
//////////////////////////////////////////////////////////////////////////

// Used to efficiently reallocate data
// 1) For every piece of data that needs reallocation, move it to its new location
// and add an entry into the table
// 2) For everyone that references one of these data blocks, do a quick table lookup
// to find the old pointer and then replace it with the new one
struct SPointerMapping
{
    void *pOld;
    void *pNew;

    static BOOL AreMappingsEqual(const SPointerMapping &pMap1, const SPointerMapping &pMap2)
    {
        return (pMap1.pOld == pMap2.pOld);
    }

    UINT Hash()
    {
        // hash the pointer itself 
        // (using the pointer as a hash would be very bad)
        return ComputeHash((BYTE*)&pOld, sizeof(pOld));
    }
};

typedef CEffectHashTableWithPrivateHeap<SPointerMapping, SPointerMapping::AreMappingsEqual> CPointerMappingTable;

// Assist adding data to a block of memory
class CEffectHeap
{
protected:
    BYTE    *m_pData;
    UINT    m_dwBufferSize;
    UINT    m_dwSize;

    template <bool bCopyData>
    HRESULT AddDataInternal(const void *pData, UINT dwSize, void **ppPointer);

public:
    HRESULT ReserveMemory(UINT dwSize);
    UINT GetSize();
    BYTE* GetDataStart() { return m_pData; }

    // AddData and AddString append existing data to the buffer - they change m_dwSize. Users are 
    //   not expected to modify the data pointed to by the return pointer
    HRESULT AddString(const char *pString, __deref_out_z char **ppPointer);
    HRESULT AddData(const void *pData, UINT  dwSize, void **ppPointer);

    // Allocate behaves like a standard new - it will allocate memory, move m_dwSize. The caller is 
    //   expected to use the returned pointer
    void* Allocate(UINT dwSize);

    // Move data from the general heap and optional free memory
    HRESULT MoveData(void **ppData, UINT size);
    HRESULT MoveString(__deref_inout_z char **ppStringData);
    HRESULT MoveInterfaceParameters(UINT InterfaceCount, __in_ecount(1) SShaderBlock::SInterfaceParameter **ppInterfaces);
    HRESULT MoveEmptyDataBlock(void **ppData, UINT size);

    BOOL IsInHeap(void *pData) const
    {
        return (pData >= m_pData && pData < (m_pData + m_dwBufferSize));
    }

    CEffectHeap();
    ~CEffectHeap();
};

class CEffectReflection
{
public:
    // Single memory block support
    CEffectHeap m_Heap;
};


class CEffect : public ID3DX11Effect
{
    friend struct SBaseBlock;
    friend struct SPassBlock;
    friend class CEffectLoader;
    friend struct SConstantBuffer;
    friend struct TSamplerVariable<TGlobalVariable<ID3DX11EffectSamplerVariable>>;
    friend struct TSamplerVariable<TVariable<TMember<ID3DX11EffectSamplerVariable>>>;
    
protected:

    UINT                    m_RefCount;
    UINT                    m_Flags;

    // Private heap - all pointers should point into here
    CEffectHeap             m_Heap;

    // Reflection object
    CEffectReflection       *m_pReflection;

    // global variables in the effect (aka parameters)
    UINT                    m_VariableCount;
    SGlobalVariable         *m_pVariables;

    // anonymous shader variables (one for every inline shader assignment)
    UINT                    m_AnonymousShaderCount;
    SAnonymousShader        *m_pAnonymousShaders;

    // techniques within this effect (the actual data is located in each group)
    UINT                    m_TechniqueCount;

    // groups within this effect
    UINT                    m_GroupCount;
    SGroup                  *m_pGroups;
    SGroup                  *m_pNullGroup;

    UINT                    m_ShaderBlockCount;
    SShaderBlock            *m_pShaderBlocks;

    UINT                    m_DepthStencilBlockCount;
    SDepthStencilBlock      *m_pDepthStencilBlocks;

    UINT                    m_BlendBlockCount;
    SBlendBlock             *m_pBlendBlocks;

    UINT                    m_RasterizerBlockCount;
    SRasterizerBlock        *m_pRasterizerBlocks;

    UINT                    m_SamplerBlockCount;
    SSamplerBlock           *m_pSamplerBlocks;

    UINT                    m_MemberDataCount;
    SMemberDataPointer      *m_pMemberDataBlocks;

    UINT                    m_InterfaceCount;
    SInterface              *m_pInterfaces;

    UINT                    m_CBCount;
    SConstantBuffer         *m_pCBs;

    UINT                    m_StringCount;
    SString                 *m_pStrings;

    UINT                    m_ShaderResourceCount;
    SShaderResource         *m_pShaderResources;

    UINT                    m_UnorderedAccessViewCount;
    SUnorderedAccessView    *m_pUnorderedAccessViews;

    UINT                    m_RenderTargetViewCount;
    SRenderTargetView       *m_pRenderTargetViews;

    UINT                    m_DepthStencilViewCount;
    SDepthStencilView       *m_pDepthStencilViews; 

    Timer                   m_LocalTimer;
    
    // temporary index variable for assignment evaluation
    UINT                    m_FXLIndex;

    ID3D11Device            *m_pDevice;
    ID3D11DeviceContext     *m_pContext;
    ID3D11ClassLinkage      *m_pClassLinkage;

    // Master lists of reflection interfaces
    CEffectVectorOwner<SSingleElementType> m_pTypeInterfaces;
    CEffectVectorOwner<SMember>            m_pMemberInterfaces;

    //////////////////////////////////////////////////////////////////////////    
    // String & Type pooling

    typedef SType *LPSRUNTIMETYPE;
    static BOOL AreTypesEqual(const LPSRUNTIMETYPE &pType1, const LPSRUNTIMETYPE &pType2) { return (pType1->IsEqual(pType2)); }
    static BOOL AreStringsEqual(__in const LPCSTR &pStr1, __in const LPCSTR &pStr2) { return strcmp(pStr1, pStr2) == 0; }

    typedef CEffectHashTableWithPrivateHeap<SType *, AreTypesEqual> CTypeHashTable;
    typedef CEffectHashTableWithPrivateHeap<LPCSTR, AreStringsEqual> CStringHashTable;

    // These are used to pool types & type-related strings
    // until Optimize() is called
    CTypeHashTable          *m_pTypePool;
    CStringHashTable        *m_pStringPool;
    CDataBlockStore         *m_pPooledHeap;
    // After Optimize() is called, the type/string pools should be deleted and all
    // remaining data should be migrated into the optimized type heap
    CEffectHeap             *m_pOptimizedTypeHeap;

    // Pools a string or type and modifies the pointer
    void AddStringToPool(const char **ppString);
    void AddTypeToPool(SType **ppType);

    HRESULT OptimizeTypes(CPointerMappingTable *pMappingTable, bool Cloning = false);


    //////////////////////////////////////////////////////////////////////////    
    // Runtime (performance critical)
    
    void ApplyShaderBlock(SShaderBlock *pBlock);
    BOOL ApplyRenderStateBlock(SBaseBlock *pBlock);
    BOOL ApplySamplerBlock(SSamplerBlock *pBlock);
    void ApplyPassBlock(SPassBlock *pBlock);
    BOOL EvaluateAssignment(SAssignment *pAssignment);
    BOOL ValidateShaderBlock( SShaderBlock* pBlock );
    BOOL ValidatePassBlock( SPassBlock* pBlock );
    
    //////////////////////////////////////////////////////////////////////////    
    // Non-runtime functions (not performance critical)    

    SGlobalVariable *FindLocalVariableByName(LPCSTR pVarName);      // Looks in the current effect only
    SGlobalVariable *FindVariableByName(LPCSTR pVarName);
    SVariable *FindVariableByNameWithParsing(LPCSTR pVarName);
    SConstantBuffer *FindCB(LPCSTR pName);
    void ReplaceCBReference(SConstantBuffer *pOldBufferBlock, ID3D11Buffer *pNewBuffer);            // Used by user-managed CBs
    void ReplaceSamplerReference(SSamplerBlock *pOldSamplerBlock, ID3D11SamplerState *pNewSampler);
    void AddRefAllForCloning( CEffect* pEffectSource );
    HRESULT CopyMemberInterfaces( CEffect* pEffectSource );
    HRESULT CopyStringPool( CEffect* pEffectSource, CPointerMappingTable& mappingTable );
    HRESULT CopyTypePool( CEffect* pEffectSource, CPointerMappingTable& mappingTableTypes, CPointerMappingTable& mappingTableStrings );
    HRESULT CopyOptimizedTypePool( CEffect* pEffectSource, CPointerMappingTable& mappingTableTypes );
    HRESULT RecreateCBs();
    HRESULT FixupMemberInterface( SMember* pMember, CEffect* pEffectSource, CPointerMappingTable& mappingTableStrings );

    void ValidateIndex(UINT Elements);

    void IncrementTimer();    
    void HandleLocalTimerRollover();

    friend struct SConstantBuffer;

public:
    CEffect( UINT Flags = 0 );
    virtual ~CEffect();
    void ReleaseShaderRefection();

    // Initialize must be called after the effect is created
    HRESULT LoadEffect(CONST void *pEffectBuffer, UINT cbEffectBuffer);

    // Once the effect is fully loaded, call BindToDevice to attach it to a device
    HRESULT BindToDevice(ID3D11Device *pDevice);

    Timer GetCurrentTime() const { return m_LocalTimer; }
    
    BOOL IsReflectionData(void *pData) const { return m_pReflection->m_Heap.IsInHeap(pData); }
    BOOL IsRuntimeData(void *pData) const { return m_Heap.IsInHeap(pData); }

    //////////////////////////////////////////////////////////////////////////    
    // Public interface

    // IUnknown
    STDMETHOD(QueryInterface)(REFIID iid, LPVOID *ppv);
    STDMETHOD_(ULONG, AddRef)();
    STDMETHOD_(ULONG, Release)();

    STDMETHOD_(BOOL, IsValid)() { return TRUE; }

    STDMETHOD(GetDevice)(ID3D11Device** ppDevice);    

    STDMETHOD(GetDesc)(D3DX11_EFFECT_DESC *pDesc);

    STDMETHOD_(ID3DX11EffectConstantBuffer*, GetConstantBufferByIndex)(UINT Index);
    STDMETHOD_(ID3DX11EffectConstantBuffer*, GetConstantBufferByName)(LPCSTR Name);

    STDMETHOD_(ID3DX11EffectVariable*, GetVariableByIndex)(UINT Index);
    STDMETHOD_(ID3DX11EffectVariable*, GetVariableByName)(LPCSTR Name);
    STDMETHOD_(ID3DX11EffectVariable*, GetVariableBySemantic)(LPCSTR Semantic);

    STDMETHOD_(ID3DX11EffectTechnique*, GetTechniqueByIndex)(UINT Index);
    STDMETHOD_(ID3DX11EffectTechnique*, GetTechniqueByName)(LPCSTR Name);

    STDMETHOD_(ID3DX11EffectGroup*, GetGroupByIndex)(UINT Index);
    STDMETHOD_(ID3DX11EffectGroup*, GetGroupByName)(LPCSTR Name);

    STDMETHOD_(ID3D11ClassLinkage*, GetClassLinkage)();

    STDMETHOD(CloneEffect)(UINT Flags, ID3DX11Effect** ppClonedEffect);
    STDMETHOD(Optimize)();
    STDMETHOD_(BOOL, IsOptimized)();

    //////////////////////////////////////////////////////////////////////////    
    // New reflection helpers

    ID3DX11EffectType * CreatePooledSingleElementTypeInterface(SType *pType);
    ID3DX11EffectVariable * CreatePooledVariableMemberInterface(TTopLevelVariable<ID3DX11EffectVariable> *pTopLevelEntity, SVariable *pMember, UDataPointer Data, BOOL IsSingleElement, UINT Index);

};

}
