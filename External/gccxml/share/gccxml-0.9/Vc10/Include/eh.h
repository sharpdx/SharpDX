/***
*eh.h - User include file for exception handling.
*
*       Copyright (c) Microsoft Corporation. All rights reserved.
*
*Purpose:
*       User include file for exception handling.
*
*       [Public]
*
****/

#pragma once

#include <crtdefs.h>

#ifndef _INC_EH
#define _INC_EH
#ifndef RC_INVOKED

/* Currently, all MS C compilers for Win32 platforms default to 8 byte 
 * alignment.
 */
#pragma pack(push,_CRT_PACKING)

#ifndef __cplusplus
#error "eh.h is only for C++!"
#endif

namespace std { class type_info; }
using std::type_info;

/* terminate_handler is the standard name; terminate_function is supported for historical reasons */
#ifndef _M_CEE_PURE
typedef void (__cdecl *terminate_function)();
typedef void (__cdecl *terminate_handler)();
typedef void (__cdecl *unexpected_function)();
typedef void (__cdecl *unexpected_handler)();
#else
typedef void (__clrcall *terminate_function)();
typedef void (__clrcall *terminate_handler)();
typedef void (__clrcall *unexpected_function)();
typedef void (__clrcall *unexpected_handler)();
#endif

#ifdef _M_CEE
typedef void (__clrcall *__terminate_function_m)();
typedef void (__clrcall *__terminate_handler_m)();
typedef void (__clrcall *__unexpected_function_m)();
typedef void (__clrcall *__unexpected_handler_m)();
#endif

struct _EXCEPTION_POINTERS;
#ifndef _M_CEE_PURE
typedef void (__cdecl *_se_translator_function)(unsigned int, struct _EXCEPTION_POINTERS*);
#endif

_CRTIMP __declspec(noreturn) void __cdecl terminate(void);
_CRTIMP void __cdecl unexpected(void);

_CRTIMP int __cdecl _is_exception_typeof(_In_ const type_info &_Type, _In_ struct _EXCEPTION_POINTERS * _ExceptionPtr);

#ifndef _M_CEE_PURE
/* only __clrcall versions provided by the MRT exist in pure */
_CRTIMP terminate_function __cdecl set_terminate(_In_opt_ terminate_function _NewPtFunc);
extern "C" _CRTIMP terminate_function __cdecl _get_terminate(void);
_CRTIMP unexpected_function __cdecl set_unexpected(_In_opt_ unexpected_function _NewPtFunc);
extern "C" _CRTIMP unexpected_function __cdecl _get_unexpected(void);
#endif

#ifndef _M_CEE_PURE
/* set_se_translator cannot be a managed implementation, and so cannot be called from _M_CEE_PURE code */
_CRTIMP _se_translator_function __cdecl _set_se_translator(_In_opt_ _se_translator_function _NewPtFunc);
#endif
_CRTIMP bool __cdecl __uncaught_exception();

/*
 * These overload helps in resolving NULL
 */
#ifdef _M_CEE
_CRTIMP terminate_function __cdecl set_terminate(_In_ int _Zero);
_CRTIMP unexpected_function __cdecl set_unexpected(_In_ int _Zero);
#endif

#pragma pack(pop)
#endif /* RC_INVOKED */
#endif  /* _INC_EH */
