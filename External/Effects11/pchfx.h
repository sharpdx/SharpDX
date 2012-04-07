//////////////////////////////////////////////////////////////////////////////
//
//  Copyright (C) Microsoft Corporation.  All Rights Reserved.
//
//  File:       pchfx.h
//  Content:    D3D shader effects precompiled header
//
//////////////////////////////////////////////////////////////////////////////

#ifndef __D3DX11_PCHFX_H__
#define __D3DX11_PCHFX_H__

#define D3DX11_DEFAULT            ((UINT) -1)
#define D3DX11_FROM_FILE          ((UINT) -3)
#define DXGI_FORMAT_FROM_FILE     ((DXGI_FORMAT) -3)

#ifndef D3DX11INLINE
#ifdef _MSC_VER
  #if (_MSC_VER >= 1200)
  #define D3DX11INLINE __forceinline
  #else
  #define D3DX11INLINE __inline
  #endif
#else
  #ifdef __cplusplus
  #define D3DX11INLINE inline
  #else
  #define D3DX11INLINE
  #endif
#endif
#endif

#define _FACD3D  0x876
#define MAKE_D3DHRESULT( code )  MAKE_HRESULT( 1, _FACD3D, code )
#define MAKE_D3DSTATUS( code )  MAKE_HRESULT( 0, _FACD3D, code )

#define D3DERR_INVALIDCALL                      MAKE_D3DHRESULT(2156)
#define D3DERR_WASSTILLDRAWING                  MAKE_D3DHRESULT(540)
#include <float.h>

#include "d3d11.h"
//#include "d3dx11.h"
#undef DEFINE_GUID
#include "INITGUID.h"
#include "d3dx11effect.h"

#define UNUSED -1

//////////////////////////////////////////////////////////////////////////

#define offsetof_fx( a, b ) (UINT)offsetof( a, b )

#include "d3dxGlobal.h"

#include <stddef.h>
#include <strsafe.h>

#include "Effect.h"
#include "EffectStateBase11.h"
#include "EffectLoad.h"

#include "D3DCompiler.h"

//////////////////////////////////////////////////////////////////////////

namespace D3DX11Effects
{
} // end namespace D3DX11Effects

#endif // __D3DX11_PCHFX_H__
