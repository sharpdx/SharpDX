//////////////////////////////////////////////////////////////////////////////
//
//  Copyright (C) Microsoft Corporation.  All Rights Reserved.
//
//  File:       EffectLoad.h
//  Content:    D3DX11 Effects header for the FX file loader 
//              A CEffectLoader is created at load time to facilitate loading
//
//////////////////////////////////////////////////////////////////////////////

#pragma once

namespace D3DX11Effects
{

// Ranges are used for dependency checking during load

enum ERanges
{
    ER_CBuffer = 0,
    ER_Texture,     // Includes TBuffers
    ER_Sampler,
    ER_UnorderedAccessView,
    ER_Interfaces,
    ER_Count        // This should be the size of the enum
};

struct SRange
{
    UINT                    start;
    UINT                    last;
    CEffectVector<void *>   vResources; // should be (last - start) in length, resource type depends on the range type
};

// Used during load to validate assignments
D3D10_SHADER_VARIABLE_TYPE GetSimpleParameterTypeFromObjectType(EObjectType ObjectType);


// A class to facilitate loading an Effect.  This class is a friend of CEffect.
class CEffectLoader
{
    friend HRESULT CEffect::CloneEffect(UINT Flags, ID3DX11Effect** ppClonedEffect );

protected:
    // Load-time allocations that eventually get moved happen out of the TempHeap. This heap will grow as needed
    CDataBlockStore             m_BulkHeap;

    BYTE                        *m_pData;
    SBinaryHeader5              *m_pHeader;
    DWORD                       m_Version;

    CEffect                     *m_pEffect;
    CEffectReflection           *m_pReflection;

    D3DX11Core::CMemoryStream   m_msStructured;
    D3DX11Core::CMemoryStream   m_msUnstructured;
    
    // used to avoid repeated hash buffer allocations in LoadTypeAndAddToPool
    CEffectVector<BYTE>         m_HashBuffer;

    UINT                        m_dwBufferSize;     // Size of data buffer in bytes

    // List of SInterface blocks created to back class instances bound to shaders
    CEffectVector<SInterface*>  m_BackgroundInterfaces;

    // Pointers to pre-reallocation data
    SGlobalVariable             *m_pOldVars;
    SShaderBlock                *m_pOldShaders;
    SDepthStencilBlock          *m_pOldDS;
    SBlendBlock                 *m_pOldAB;
    SRasterizerBlock            *m_pOldRS;
    SConstantBuffer             *m_pOldCBs;
    SSamplerBlock               *m_pOldSamplers;
    UINT                        m_OldInterfaceCount;
    SInterface                  *m_pOldInterfaces;
    SShaderResource             *m_pOldShaderResources;
    SUnorderedAccessView        *m_pOldUnorderedAccessViews;
    SRenderTargetView           *m_pOldRenderTargetViews;
    SDepthStencilView           *m_pOldDepthStencilViews;
    SString                     *m_pOldStrings;
    SMemberDataPointer          *m_pOldMemberDataBlocks;
    CEffectVectorOwner<SMember> *m_pvOldMemberInterfaces;
    SGroup                      *m_pOldGroups;

    UINT                        m_EffectMemory;     // Effect private heap
    UINT                        m_ReflectionMemory; // Reflection private heap

    // Loader helpers
    HRESULT LoadCBs();
    HRESULT LoadNumericVariable(SConstantBuffer *pParentCB);
    HRESULT LoadObjectVariables();
    HRESULT LoadInterfaceVariables();

    HRESULT LoadTypeAndAddToPool(SType **ppType, UINT  dwOffset);
    HRESULT LoadStringAndAddToPool(__out_ecount_full(1) char **ppString, UINT  dwOffset);
    HRESULT LoadAssignments( UINT Assignments, SAssignment **pAssignments, BYTE *pBackingStore, UINT *pRTVAssignments, UINT *pFinalAssignments );
    HRESULT LoadGroups();
    HRESULT LoadTechnique( STechnique* pTech );
    HRESULT LoadAnnotations(UINT  *pcAnnotations, SAnnotation **ppAnnotations);

    HRESULT ExecuteConstantAssignment(SBinaryConstant *pConstant, void *pLHS, D3D10_SHADER_VARIABLE_TYPE lhsType);
    UINT    UnpackData(BYTE *pDestData, BYTE *pSrcData, UINT  PackedDataSize, SType *pType, UINT  *pBytesRead);

    // Build shader blocks
    HRESULT ConvertRangesToBindings(SShaderBlock *pShaderBlock, CEffectVector<SRange> *pvRanges );
    HRESULT GrabShaderData(SShaderBlock *pShaderBlock);
    HRESULT BuildShaderBlock(SShaderBlock *pShaderBlock);

    // Memory compactors
    HRESULT InitializeReflectionDataAndMoveStrings( UINT KnownSize = 0 );
    HRESULT ReallocateReflectionData( bool Cloning = false );
    HRESULT ReallocateEffectData( bool Cloning = false );
    HRESULT ReallocateShaderBlocks();
    template<class T> HRESULT ReallocateBlockAssignments(T* &pBlocks, UINT  cBlocks, T* pOldBlocks = NULL);
    HRESULT ReallocateAnnotationData(UINT  cAnnotations, SAnnotation **ppAnnotations);

    HRESULT CalculateAnnotationSize(UINT  cAnnotations, SAnnotation *pAnnotations);
    UINT  CalculateShaderBlockSize();
    template<class T> UINT  CalculateBlockAssignmentSize(T* &pBlocks, UINT  cBlocks);

    HRESULT FixupCBPointer(SConstantBuffer **ppCB);
    HRESULT FixupShaderPointer(SShaderBlock **ppShaderBlock);
    HRESULT FixupDSPointer(SDepthStencilBlock **ppDSBlock);
    HRESULT FixupABPointer(SBlendBlock **ppABBlock);
    HRESULT FixupRSPointer(SRasterizerBlock **ppRSBlock);
    HRESULT FixupInterfacePointer(SInterface **ppInterface, bool CheckBackgroundInterfaces);
    HRESULT FixupShaderResourcePointer(SShaderResource **ppResource);
    HRESULT FixupUnorderedAccessViewPointer(SUnorderedAccessView **ppResource);
    HRESULT FixupRenderTargetViewPointer(SRenderTargetView **ppRenderTargetView);
    HRESULT FixupDepthStencilViewPointer(SDepthStencilView **ppDepthStencilView);
    HRESULT FixupSamplerPointer(SSamplerBlock **ppSampler);
    HRESULT FixupVariablePointer(SGlobalVariable **ppVar);
    HRESULT FixupStringPointer(SString **ppString);
    HRESULT FixupMemberDataPointer(SMemberDataPointer **ppMemberData);
    HRESULT FixupGroupPointer(SGroup **ppGroup);

    // Methods to retrieve data from the unstructured block
    // (these do not make copies; they simply return pointers into the block)
    HRESULT GetStringAndAddToReflection(UINT  offset, __out_ecount_full(1) char **ppPointer);  // Returns a string from the file string block, updates m_EffectMemory
    HRESULT GetUnstructuredDataBlock(UINT  offset, UINT  *pdwSize, void **ppData);
    // This function makes a copy of the array of SInterfaceParameters, but not a copy of the strings
    HRESULT GetInterfaceParametersAndAddToReflection( UINT InterfaceCount, UINT offset, __out_ecount_full(1) SShaderBlock::SInterfaceParameter **ppInterfaces );
public:

    HRESULT LoadEffect(CEffect *pEffect, CONST void *pEffectBuffer, UINT  cbEffectBuffer);
};


}