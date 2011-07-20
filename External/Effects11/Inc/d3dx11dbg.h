//////////////////////////////////////////////////////////////////////////////
//
//  Copyright (C) Microsoft Corporation.  All Rights Reserved.
//
//  File:       d3dx11dbg.h
//  Content:    D3DX11 debugging functions
//
//////////////////////////////////////////////////////////////////////////////


#ifndef __D3DX11DBG_H__
#define __D3DX11DBG_H__

#ifndef _PREFAST_

#pragma warning( disable: 4068 )

#endif


#include <strsafe.h>
#include <new>

#undef NEW
#undef DELETE

#define NEW new (std::nothrow)
#define NEW_PROTO_ARGS size_t s, const std::nothrow_t& t



typedef signed char    INT8;
typedef signed short   INT16;
typedef unsigned char  UINT8;
typedef unsigned short UINT16;


//----------------------------------------------------------------------------
// DPF
//----------------------------------------------------------------------------

#ifdef FXDPF
void cdecl D3DXDebugPrintf(UINT lvl, LPCSTR szFormat, ...);
#define DPF D3DXDebugPrintf
#else // !FXDPF
#pragma warning(disable:4002)
#define DPF() 0
#endif // !FXDPF


//----------------------------------------------------------------------------
// D3DXASSERT
//----------------------------------------------------------------------------

#if _DEBUG
int WINAPI D3DXDebugAssert(LPCSTR szFile, int nLine, LPCSTR szCondition);
#define D3DXASSERT(condition) \
    do { if(!(condition)) D3DXDebugAssert(__FILE__, __LINE__, #condition); } while(0)
#else // !_DEBUG
#define D3DXASSERT(condition) 0
#endif // !_DEBUG



#endif // __D3DX11DBG_H__

