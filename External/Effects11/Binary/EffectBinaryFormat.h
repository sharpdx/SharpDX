//////////////////////////////////////////////////////////////////////////////
//
//  Copyright (C) Microsoft Corporation.  All Rights Reserved.
//
//  File:       Effect.h
//  Content:    D3DX11 Effects Binary Format
//              This is the binary file interface shared between the Effects 
//              compiler and runtime.
//
//////////////////////////////////////////////////////////////////////////////

#pragma once

namespace D3DX11Effects
{


//////////////////////////////////////////////////////////////////////////
// Version Control
//////////////////////////////////////////////////////////////////////////

#define D3D10_FXL_VERSION(_Major,_Minor) (('F' << 24) | ('X' << 16) | ((_Major) << 8) | (_Minor))

struct EVersionTag
{
    const char* m_pName;
    DWORD       m_Version;
    UINT        m_Tag;
};

// versions must be listed in ascending order
static CONST EVersionTag g_EffectVersions[] = 
{
    { "fx_4_0", D3D10_FXL_VERSION(4,0),   0xFEFF1001 },
    { "fx_4_1", D3D10_FXL_VERSION(4,1),   0xFEFF1011 },
    { "fx_5_0", D3D10_FXL_VERSION(5,0),   0xFEFF2001 },
};
#define NUM_EFFECT10_VERSIONS ( sizeof(g_EffectVersions) / sizeof(EVersionTag) )


//////////////////////////////////////////////////////////////////////////
// Reflection & Type structures
//////////////////////////////////////////////////////////////////////////

// Enumeration of the possible left-hand side values of an assignment,
// divided up categorically by the type of block they may appear in
enum ELhsType
{
    ELHS_Invalid,

    // Pass block assignment types

    ELHS_PixelShaderBlock,          // SBlock *pValue points to the block to apply
    ELHS_VertexShaderBlock,
    ELHS_GeometryShaderBlock,
    ELHS_RenderTargetView,
    ELHS_DepthStencilView,

    ELHS_RasterizerBlock,
    ELHS_DepthStencilBlock,
    ELHS_BlendBlock,

    ELHS_GenerateMips,              // This is really a call to D3D::GenerateMips

    // Various SAssignment.Value.*

    ELHS_DS_StencilRef,             // SAssignment.Value.pdValue
    ELHS_B_BlendFactor,             // D3D10_BLEND_CONFIG.BlendFactor, points to a float4
    ELHS_B_SampleMask,              // D3D10_BLEND_CONFIG.SampleMask

    ELHS_GeometryShaderSO,          // When setting SO assignments, GeometryShaderSO precedes the actual GeometryShader assn

    ELHS_ComputeShaderBlock,   
    ELHS_HullShaderBlock,
    ELHS_DomainShaderBlock,

    // Rasterizer

    ELHS_FillMode = 0x20000,
    ELHS_CullMode,
    ELHS_FrontCC,
    ELHS_DepthBias,
    ELHS_DepthBiasClamp,
    ELHS_SlopeScaledDepthBias,
    ELHS_DepthClipEnable,
    ELHS_ScissorEnable,
    ELHS_MultisampleEnable,
    ELHS_AntialiasedLineEnable,

    // Sampler

    ELHS_Filter = 0x30000,
    ELHS_AddressU,
    ELHS_AddressV,
    ELHS_AddressW,
    ELHS_MipLODBias,
    ELHS_MaxAnisotropy,
    ELHS_ComparisonFunc,
    ELHS_BorderColor,
    ELHS_MinLOD,
    ELHS_MaxLOD,
    ELHS_Texture,

    // DepthStencil

    ELHS_DepthEnable = 0x40000,
    ELHS_DepthWriteMask,
    ELHS_DepthFunc,
    ELHS_StencilEnable,
    ELHS_StencilReadMask,
    ELHS_StencilWriteMask,
    ELHS_FrontFaceStencilFailOp,
    ELHS_FrontFaceStencilDepthFailOp,
    ELHS_FrontFaceStencilPassOp,
    ELHS_FrontFaceStencilFunc,
    ELHS_BackFaceStencilFailOp,
    ELHS_BackFaceStencilDepthFailOp,
    ELHS_BackFaceStencilPassOp,
    ELHS_BackFaceStencilFunc,

    // BlendState

    ELHS_AlphaToCoverage = 0x50000,
    ELHS_BlendEnable,
    ELHS_SrcBlend,
    ELHS_DestBlend,
    ELHS_BlendOp,
    ELHS_SrcBlendAlpha,
    ELHS_DestBlendAlpha,
    ELHS_BlendOpAlpha,
    ELHS_RenderTargetWriteMask,
};



enum EBlockType
{
    EBT_Invalid,
    EBT_DepthStencil,
    EBT_Blend,
    EBT_Rasterizer,
    EBT_Sampler,
    EBT_Pass
};

enum EVarType
{
    EVT_Invalid,
    EVT_Numeric,
    EVT_Object,
    EVT_Struct,
    EVT_Interface,
};

enum EScalarType
{
    EST_Invalid,
    EST_Float,
    EST_Int,
    EST_UInt,
    EST_Bool,
    EST_Count
};

enum ENumericLayout
{
    ENL_Invalid,
    ENL_Scalar,
    ENL_Vector,
    ENL_Matrix,
    ENL_Count
};

enum EObjectType
{
    EOT_Invalid,
    EOT_String,
    EOT_Blend,
    EOT_DepthStencil,
    EOT_Rasterizer,
    EOT_PixelShader,
    EOT_VertexShader,
    EOT_GeometryShader,              // Regular geometry shader
    EOT_GeometryShaderSO,            // Geometry shader with a attached StreamOut decl
    EOT_Texture,
    EOT_Texture1D,
    EOT_Texture1DArray,
    EOT_Texture2D,
    EOT_Texture2DArray,
    EOT_Texture2DMS,
    EOT_Texture2DMSArray,
    EOT_Texture3D,
    EOT_TextureCube,
    EOT_ConstantBuffer,
    EOT_RenderTargetView,
    EOT_DepthStencilView,
    EOT_Sampler,
    EOT_Buffer,
    EOT_TextureCubeArray,
    EOT_Count,
    EOT_PixelShader5,
    EOT_VertexShader5,
    EOT_GeometryShader5,
    EOT_ComputeShader5,
    EOT_HullShader5,
    EOT_DomainShader5,
    EOT_RWTexture1D,
    EOT_RWTexture1DArray,
    EOT_RWTexture2D,
    EOT_RWTexture2DArray,
    EOT_RWTexture3D,
    EOT_RWBuffer,
    EOT_ByteAddressBuffer,
    EOT_RWByteAddressBuffer,
    EOT_StructuredBuffer,
    EOT_RWStructuredBuffer,
    EOT_RWStructuredBufferAlloc,
    EOT_RWStructuredBufferConsume,
    EOT_AppendStructuredBuffer,
    EOT_ConsumeStructuredBuffer,
};

D3DX11INLINE BOOL IsObjectTypeHelper(EVarType InVarType,
                                     EObjectType InObjType,
                                     EObjectType TargetObjType)
{
    return (InVarType == EVT_Object) && (InObjType == TargetObjType);
}

D3DX11INLINE BOOL IsSamplerHelper(EVarType InVarType,
                                  EObjectType InObjType)
{
    return (InVarType == EVT_Object) && (InObjType == EOT_Sampler);
}

D3DX11INLINE BOOL IsStateBlockObjectHelper(EVarType InVarType,
                                           EObjectType InObjType)
{
    return (InVarType == EVT_Object) && ((InObjType == EOT_Blend) || (InObjType == EOT_DepthStencil) || (InObjType == EOT_Rasterizer) || IsSamplerHelper(InVarType, InObjType));
}

D3DX11INLINE BOOL IsShaderHelper(EVarType InVarType,
                                 EObjectType InObjType)
{
    return (InVarType == EVT_Object) && ((InObjType == EOT_VertexShader) ||
                                         (InObjType == EOT_VertexShader5) ||
                                         (InObjType == EOT_HullShader5) ||
                                         (InObjType == EOT_DomainShader5) ||
                                         (InObjType == EOT_ComputeShader5) ||
                                         (InObjType == EOT_GeometryShader) ||
                                         (InObjType == EOT_GeometryShaderSO) ||
                                         (InObjType == EOT_GeometryShader5) ||
                                         (InObjType == EOT_PixelShader) ||
                                         (InObjType == EOT_PixelShader5));
}

D3DX11INLINE BOOL IsShader5Helper(EVarType InVarType,
                                  EObjectType InObjType)
{
    return (InVarType == EVT_Object) && ((InObjType == EOT_VertexShader5) ||
                                         (InObjType == EOT_HullShader5) ||
                                         (InObjType == EOT_DomainShader5) ||
                                         (InObjType == EOT_ComputeShader5) ||
                                         (InObjType == EOT_GeometryShader5) ||
                                         (InObjType == EOT_PixelShader5));
}

D3DX11INLINE BOOL IsInterfaceHelper(EVarType InVarType,
                                         EObjectType InObjType)
{
    return (InVarType == EVT_Interface);
}

D3DX11INLINE BOOL IsShaderResourceHelper(EVarType InVarType,
                                         EObjectType InObjType)
{
    return (InVarType == EVT_Object) && ((InObjType == EOT_Texture) ||
                                         (InObjType == EOT_Texture1D) || 
                                         (InObjType == EOT_Texture1DArray) ||
                                         (InObjType == EOT_Texture2D) || 
                                         (InObjType == EOT_Texture2DArray) ||
                                         (InObjType == EOT_Texture2DMS) || 
                                         (InObjType == EOT_Texture2DMSArray) ||
                                         (InObjType == EOT_Texture3D) || 
                                         (InObjType == EOT_TextureCube) ||
                                         (InObjType == EOT_TextureCubeArray) || 
                                         (InObjType == EOT_Buffer) ||
                                         (InObjType == EOT_StructuredBuffer) ||
                                         (InObjType == EOT_ByteAddressBuffer));
}

D3DX11INLINE BOOL IsUnorderedAccessViewHelper(EVarType InVarType,
                                              EObjectType InObjType)
{
    return (InVarType == EVT_Object) &&
        ((InObjType == EOT_RWTexture1D) ||
         (InObjType == EOT_RWTexture1DArray) ||
         (InObjType == EOT_RWTexture2D) ||
         (InObjType == EOT_RWTexture2DArray) ||
         (InObjType == EOT_RWTexture3D) ||
         (InObjType == EOT_RWBuffer) ||
         (InObjType == EOT_RWByteAddressBuffer) ||
         (InObjType == EOT_RWStructuredBuffer) ||
         (InObjType == EOT_RWStructuredBufferAlloc) ||
         (InObjType == EOT_RWStructuredBufferConsume) ||
         (InObjType == EOT_AppendStructuredBuffer) ||
         (InObjType == EOT_ConsumeStructuredBuffer));
}

D3DX11INLINE BOOL IsRenderTargetViewHelper(EVarType InVarType,
                                           EObjectType InObjType)
{
    return (InVarType == EVT_Object) && (InObjType == EOT_RenderTargetView);
}

D3DX11INLINE BOOL IsDepthStencilViewHelper(EVarType InVarType,
                                           EObjectType InObjType)
{
    return (InVarType == EVT_Object) && (InObjType == EOT_DepthStencilView);
}

D3DX11INLINE BOOL IsObjectAssignmentHelper(ELhsType LhsType)
{
    switch(LhsType)
    {
    case ELHS_VertexShaderBlock:
    case ELHS_HullShaderBlock:
    case ELHS_DepthStencilView:
    case ELHS_GeometryShaderBlock:
    case ELHS_PixelShaderBlock:
    case ELHS_ComputeShaderBlock:
    case ELHS_DepthStencilBlock:
    case ELHS_RasterizerBlock:
    case ELHS_BlendBlock:
    case ELHS_Texture:
    case ELHS_RenderTargetView:
    case ELHS_DomainShaderBlock:
        return TRUE;
    }
    return FALSE;
}




// Effect file format structures /////////////////////////////////////////////
// File format:
//   File header (SBinaryHeader Header)
//   Unstructured data block (BYTE[Header.cbUnstructured))
//   Structured data block
//     ConstantBuffer (SBinaryConstantBuffer CB) * Header.Effect.cCBs
//       UINT  NumAnnotations
//       Annotation data (SBinaryAnnotation) * (NumAnnotations) *this structure is variable sized
//       Variable data (SBinaryNumericVariable Var) * (CB.cVariables)
//         UINT  NumAnnotations
//         Annotation data (SBinaryAnnotation) * (NumAnnotations) *this structure is variable sized
//     Object variables (SBinaryObjectVariable Var) * (Header.cObjectVariables) *this structure is variable sized
//       UINT  NumAnnotations
//       Annotation data (SBinaryAnnotation) * (NumAnnotations) *this structure is variable sized
//     Interface variables (SBinaryInterfaceVariable Var) * (Header.cInterfaceVariables) *this structure is variable sized
//       UINT  NumAnnotations
//       Annotation data (SBinaryAnnotation) * (NumAnnotations) *this structure is variable sized
//     Groups (SBinaryGroup Group) * Header.cGroups
//       UINT  NumAnnotations
//       Annotation data (SBinaryAnnotation) * (NumAnnotations) *this structure is variable sized
//       Techniques (SBinaryTechnique Technique) * Group.cTechniques
//         UINT  NumAnnotations
//         Annotation data (SBinaryAnnotation) * (NumAnnotations) *this structure is variable sized
//         Pass (SBinaryPass Pass) * Technique.cPasses
//           UINT  NumAnnotations
//           Annotation data (SBinaryAnnotation) * (NumAnnotations) *this structure is variable sized
//           Pass assignments (SBinaryAssignment) * Pass.cAssignments

struct SBinaryHeader
{
    struct SVarCounts
    {
        UINT  cCBs;
        UINT  cNumericVariables;
        UINT  cObjectVariables;
    };

    UINT        Tag;    // should be equal to c_EffectFileTag
                        // this is used to identify ASCII vs Binary files

    SVarCounts  Effect;
    SVarCounts  Pool;
    
    UINT        cTechniques;
    UINT        cbUnstructured;

    UINT        cStrings;
    UINT        cShaderResources;

    UINT        cDepthStencilBlocks;
    UINT        cBlendStateBlocks;
    UINT        cRasterizerStateBlocks;
    UINT        cSamplers;
    UINT        cRenderTargetViews;
    UINT        cDepthStencilViews;

    UINT        cTotalShaders;
    UINT        cInlineShaders; // of the aforementioned shaders, the number that are defined inline within pass blocks

    D3DX11INLINE bool RequiresPool() const
    {
        return (Pool.cCBs != 0) ||
               (Pool.cNumericVariables != 0) ||
               (Pool.cObjectVariables != 0);
    }
};

struct SBinaryHeader5 : public SBinaryHeader
{
    UINT  cGroups;
    UINT  cUnorderedAccessViews;
    UINT  cInterfaceVariables;
    UINT  cInterfaceVariableElements;
    UINT  cClassInstanceElements;
};

// Constant buffer definition
struct SBinaryConstantBuffer
{
    // private flags
    static const UINT   c_IsTBuffer = (1 << 0);
    static const UINT   c_IsSingle = (1 << 1);

    UINT                oName;                // Offset to constant buffer name
    UINT                Size;                 // Size, in bytes
    UINT                Flags;
    UINT                cVariables;           // # of variables inside this buffer
    UINT                ExplicitBindPoint;    // Defined if the effect file specifies a bind point using the register keyword
                                              // otherwise, -1
};

struct SBinaryAnnotation
{
    UINT  oName;                // Offset to variable name
    UINT  oType;                // Offset to type information (SBinaryType)

    // For numeric annotations:
    // UINT  oDefaultValue;     // Offset to default initializer value
    //
    // For string annotations:
    // UINT  oStringOffsets[Elements]; // Elements comes from the type data at oType
};

struct SBinaryNumericVariable
{
    UINT  oName;                // Offset to variable name
    UINT  oType;                // Offset to type information (SBinaryType)
    UINT  oSemantic;            // Offset to semantic information
    UINT  Offset;               // Offset in parent constant buffer
    UINT  oDefaultValue;        // Offset to default initializer value
    UINT  Flags;                // Explicit bind point
};

struct SBinaryInterfaceVariable
{
    UINT  oName;                // Offset to variable name
    UINT  oType;                // Offset to type information (SBinaryType)
    UINT  oDefaultValue;        // Offset to default initializer array (SBinaryInterfaceInitializer[Elements])
    UINT  Flags;
};

struct SBinaryInterfaceInitializer
{
    UINT  oInstanceName;
    UINT  ArrayIndex;
};

struct SBinaryObjectVariable
{
    UINT  oName;                // Offset to variable name
    UINT  oType;                // Offset to type information (SBinaryType)
    UINT  oSemantic;            // Offset to semantic information
    UINT  ExplicitBindPoint;    // Used when a variable has been explicitly bound (register(XX)). -1 if not

    // Initializer data:
    //
    // The type structure pointed to by oType gives you Elements, 
    // VarType (must be EVT_Object), and ObjectType
    //
    // For ObjectType == EOT_Blend, EOT_DepthStencil, EOT_Rasterizer, EOT_Sampler
    // struct 
    // {
    //   UINT  cAssignments;
    //   SBinaryAssignment Assignments[cAssignments];
    // } Blocks[Elements]
    //
    // For EObjectType == EOT_Texture*, EOT_Buffer
    // <nothing>
    //
    // For EObjectType == EOT_*Shader, EOT_String
    // UINT  oData[Elements]; // offsets to a shader data block or a NULL-terminated string
    //
    // For EObjectType == EOT_GeometryShaderSO
    //   SBinaryGSSOInitializer[Elements]
    //
    // For EObjectType == EOT_*Shader5
    //   SBinaryShaderData5[Elements]
};

struct SBinaryGSSOInitializer
{
    UINT  oShader;              // Offset to shader bytecode data block
    UINT  oSODecl;              // Offset to StreamOutput decl string
};

struct SBinaryShaderData5
{
    UINT  oShader;              // Offset to shader bytecode data block
    UINT  oSODecls[4];          // Offset to StreamOutput decl strings
    UINT  cSODecls;             // Count of valid oSODecls entries.
    UINT  RasterizedStream;     // Which stream is used for rasterization
    UINT  cInterfaceBindings;   // Count of interface bindings.
    UINT  oInterfaceBindings;   // Offset to SBinaryInterfaceInitializer[cInterfaceBindings].
};

struct SBinaryType
{
    UINT        oTypeName;      // Offset to friendly type name ("float4", "VS_OUTPUT")
    EVarType    VarType;        // Numeric, Object, or Struct
    UINT        Elements;       // # of array elements (0 for non-arrays)
    UINT        TotalSize;      // Size in bytes; not necessarily Stride * Elements for arrays 
                                // because of possible gap left in final register
    UINT        Stride;         // If an array, this is the spacing between elements.
                                // For unpacked arrays, always divisible by 16-bytes (1 register).
                                // No support for packed arrays    
    UINT        PackedSize;     // Size, in bytes, of this data typed when fully packed

    struct SBinaryMember
    {
        UINT    oName;          // Offset to structure member name ("m_pFoo")
        UINT    oSemantic;      // Offset to semantic ("POSITION0")
        UINT    Offset;         // Offset, in bytes, relative to start of parent structure
        UINT    oType;          // Offset to member's type descriptor
    };

    // the data that follows depends on the VarType:
    // Numeric: SType::SNumericType
    // Object:  EObjectType
    // Struct:  
    //   struct
    //   {
    //        UINT              cMembers;
    //        SBinaryMembers    Members[cMembers];
    //   } MemberInfo
    //   struct
    //   {
    //        UINT              oBaseClassType;  // Offset to type information (SBinaryType)
    //        UINT              cInterfaces;
    //        UINT              oInterfaceTypes[cInterfaces];
    //   } SBinaryTypeInheritance
    // Interface: (nothing)
};

struct SBinaryNumericType
{
    ENumericLayout  NumericLayout   : 3;    // scalar (1x1), vector (1xN), matrix (NxN)
    EScalarType     ScalarType      : 5;    // float32, int32, int8, etc.
    UINT            Rows            : 3;    // 1 <= Rows <= 4
    UINT            Columns         : 3;    // 1 <= Columns <= 4
    UINT            IsColumnMajor   : 1;    // applies only to matrices
    UINT            IsPackedArray   : 1;    // if this is an array, indicates whether elements should be greedily packed
};

struct SBinaryTypeInheritance
{
    UINT oBaseClass;            // Offset to base class type info or 0 if no base class.
    UINT cInterfaces;

    // Followed by UINT[cInterfaces] with offsets to the type
    // info of each interface.
};

struct SBinaryGroup
{
    UINT  oName;
    UINT  cTechniques;
};

struct SBinaryTechnique
{
    UINT  oName;
    UINT  cPasses;
};

struct SBinaryPass
{
    UINT  oName;
    UINT  cAssignments;
};

enum ECompilerAssignmentType
{
    ECAT_Invalid,                   // Assignment-specific data (always in the unstructured blob)
    ECAT_Constant,                  // -N SConstant structures
    ECAT_Variable,                  // -NULL terminated string with variable name ("foo")
    ECAT_ConstIndex,                // -SConstantIndex structure
    ECAT_VariableIndex,             // -SVariableIndex structure
    ECAT_ExpressionIndex,           // -SIndexedObjectExpression structure
    ECAT_Expression,                // -Data block containing FXLVM code
    ECAT_InlineShader,              // -Data block containing shader
    ECAT_InlineShader5,             // -Data block containing shader with extended 5.0 data (SBinaryShaderData5)
};

struct SBinaryAssignment
{
    UINT  iState;                   // index into g_lvGeneral
    UINT  Index;                    // the particular index to assign to (see g_lvGeneral to find the # of valid indices)
    ECompilerAssignmentType AssignmentType;
    UINT  oInitializer;             // Offset of assignment-specific data

    //struct SConstantAssignment
    //{
    //    UINT  NumConstants;         // number of constants to follow
    //    SCompilerConstant Constants[NumConstants];
    //};

    struct SConstantIndex
    {
        UINT  oArrayName;
        UINT  Index;
    };

    struct SVariableIndex
    {
        UINT  oArrayName;
        UINT  oIndexVarName;
    };

    struct SIndexedObjectExpression
    {   
        UINT  oArrayName;
        UINT  oCode;
    };

    struct SInlineShader
    {
        UINT  oShader;
        UINT  oSODecl;
    };

    //struct SExpression or SInlineShader
    //{
    //    UINT  DataSize;
    //    BYTE Data[DataSize];
    //}

};

struct SBinaryConstant
{
    EScalarType Type;
    union
    {
        BOOL    bValue;
        INT     iValue;
        float   fValue;
    };
};


} // end namespace D3DX11Effects
