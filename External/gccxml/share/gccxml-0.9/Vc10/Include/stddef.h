/***
*stddef.h - definitions/declarations for common constants, types, variables
*
*       Copyright (c) Microsoft Corporation. All rights reserved.
*
*Purpose:
*       This file contains definitions and declarations for some commonly
*       used constants, types, and variables.
*       [ANSI]
*
*       [Public]
*
****/

#pragma once

#ifndef _INC_STDDEF
#define _INC_STDDEF

#include <crtdefs.h>

#ifdef  __cplusplus
extern "C" {
#endif

/* Define NULL pointer value */
#ifndef NULL
#ifdef __cplusplus
#define NULL    0
#else
#define NULL    ((void *)0)
#endif
#endif


#ifdef __cplusplus
#ifdef _NATIVE_NULLPTR_SUPPORTED
namespace std { typedef decltype(__nullptr) nullptr_t; }
using ::std::nullptr_t;
#endif
#endif


/* Declare reference to errno */
#ifndef _CRT_ERRNO_DEFINED
#define _CRT_ERRNO_DEFINED
_CRTIMP extern int * __cdecl _errno(void);
#define errno   (*_errno())

errno_t __cdecl _set_errno(_In_ int _Value);
errno_t __cdecl _get_errno(_Out_ int * _Value);
#endif

/* Define offsetof macro */
#define offsetof(s,m) ((size_t)__builtin_offsetof(s,m))
#if 0 /* replaced for gccxml */
#ifdef __cplusplus

#ifdef  _WIN64
#define offsetof(s,m)   (size_t)( (ptrdiff_t)&reinterpret_cast<const volatile char&>((((s *)0)->m)) )
#else
#define offsetof(s,m)   (size_t)&reinterpret_cast<const volatile char&>((((s *)0)->m))
#endif

#else

#ifdef  _WIN64
#define offsetof(s,m)   (size_t)( (ptrdiff_t)&(((s *)0)->m) )
#else
#define offsetof(s,m)   (size_t)&(((s *)0)->m)
#endif

#endif	/* __cplusplus */
#endif  /* 0 */

_CRTIMP extern unsigned long  __cdecl __threadid(void);
#define _threadid       (__threadid())
_CRTIMP extern uintptr_t __cdecl __threadhandle(void);

#ifdef  __cplusplus
}
#endif

#endif  /* _INC_STDDEF */
