//////////////////////////////////////////////////////////////////////////////
//
//  Copyright (C) Microsoft Corporation.  All Rights Reserved.
//
//  File:       EffectStateBase11.h
//  Content:    D3DX11 Effects States Header
//
//////////////////////////////////////////////////////////////////////////////

#pragma once

namespace D3DX11Effects
{

//////////////////////////////////////////////////////////////////////////
// Effect HLSL states and late resolve lists
//////////////////////////////////////////////////////////////////////////

struct RValue
{
    const char  *m_pName;
    UINT        m_Value;
};

#define RVALUE_END()    { NULL, 0U }
#define RVALUE_ENTRY(prefix, x)         { #x, (UINT)prefix##x }

enum ELhsType;

struct LValue
{
    const char      *m_pName;           // name of the LHS side of expression
    EBlockType      m_BlockType;        // type of block it can appear in
    D3D10_SHADER_VARIABLE_TYPE m_Type;  // data type allows
    UINT            m_Cols;             // number of [m_Type]'s required (1 for a scalar, 4 for a vector)
    UINT            m_Indices;          // max index allowable (if LHS is an array; otherwise this is 1)
    BOOL            m_VectorScalar;     // can be both vector and scalar (setting as a scalar sets all m_Indices values simultaneously)
    CONST RValue    *m_pRValue;         // pointer to table of allowable RHS "late resolve" values
    ELhsType        m_LhsType;          // ELHS_* enum value that corresponds to this entry
    UINT            m_Offset;           // offset into the given block type where this value should be written
    UINT            m_Stride;           // for vectors, byte stride between two consecutive values. if 0, m_Type's size is used
};

#define LVALUE_END()    { NULL, D3D10_SVT_UINT, 0, 0, 0, NULL }

extern CONST LValue g_lvGeneral[];
extern CONST UINT   g_lvGeneralCount;

} // end namespace D3DX11Effects
