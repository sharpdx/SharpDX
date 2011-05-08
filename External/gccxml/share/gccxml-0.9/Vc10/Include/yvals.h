/* yvals.h values header for Microsoft C/C++ */
#pragma once
#ifndef _YVALS
#define _YVALS

#include <crtdefs.h>

#pragma pack(push,_CRT_PACKING)

#define _CPPLIB_VER	520

#if 1 // gccxml cannot provide these builtins until we use the 4.3 parser
#define __is_union(X) gccxml_does_not_yet_support_tr1_type_traits
#define __is_class(X) gccxml_does_not_yet_support_tr1_type_traits
#define __is_enum(X) gccxml_does_not_yet_support_tr1_type_traits
#define __is_pod(X) gccxml_does_not_yet_support_tr1_type_traits
#define __is_empty(X) gccxml_does_not_yet_support_tr1_type_traits
#define __is_polymorphic(X) gccxml_does_not_yet_support_tr1_type_traits
#define __is_abstract(X) gccxml_does_not_yet_support_tr1_type_traits
#define __is_convertible_to(X,Y) gccxml_does_not_yet_support_tr1_type_traits
#define __has_trivial_constructor(X) gccxml_does_not_yet_support_tr1_type_traits
#define __has_trivial_copy(X) gccxml_does_not_yet_support_tr1_type_traits
#define __has_trivial_assign(X) gccxml_does_not_yet_support_tr1_type_traits
#define __has_trivial_destructor(X) gccxml_does_not_yet_support_tr1_type_traits
#define __has_nothrow_constructor(X) gccxml_does_not_yet_support_tr1_type_traits
#define __has_nothrow_copy(X) gccxml_does_not_yet_support_tr1_type_traits
#define __has_nothrow_assign(X) gccxml_does_not_yet_support_tr1_type_traits
#define __has_virtual_destructor(X) gccxml_does_not_yet_support_tr1_type_traits
#endif

#define _ALLOW_KEYWORD_MACROS	// TRANSITION
#ifndef _ALLOW_KEYWORD_MACROS
 #include <xkeycheck.h>
#endif /* _ALLOW_KEYWORD_MACROS */

#ifndef _HAS_CPP0X
 #define _HAS_CPP0X     1
#endif

#define _HAS_TR1	1	/* enable TR1 extensions */

/* Note on use of "deprecate":
 * Various places in this header and other headers use __declspec(deprecate) or macros that have the term DEPRECATE in them.
 * We use deprecate here ONLY to signal the compiler to emit a warning about these items. The use of deprecate
 * should NOT be taken to imply that any standard committee has deprecated these functions from the relevant standards.
 * In fact, these functions are NOT deprecated from the standard.
 *
 * Full details can be found in our documentation by searching for "Checked Iterators".
*/

#define __PURE_APPDOMAIN_GLOBAL

#ifndef __CRTDECL
#if defined(_M_CEE_PURE) || defined(MRTDLL)
#define __CRTDECL   __clrcall
#else
#define __CRTDECL   __cdecl
#endif
#endif

#ifndef __CLR_OR_THIS_CALL
#if defined(MRTDLL) || defined(_M_CEE_PURE)
#define __CLR_OR_THIS_CALL  __clrcall
#else
#define __CLR_OR_THIS_CALL
#endif
#endif

#ifndef __CLRCALL_OR_CDECL
#if defined(MRTDLL) || defined(_M_CEE_PURE)
#define __CLRCALL_OR_CDECL __clrcall
#else
#define __CLRCALL_OR_CDECL __cdecl
#endif
#endif

#ifndef __CLRCALL_PURE_OR_CDECL
#if defined(_M_CEE_PURE)
#define __CLRCALL_PURE_OR_CDECL __clrcall
#else
#define __CLRCALL_PURE_OR_CDECL __cdecl
#endif
#endif

		/* CURRENT DLL NAMES */
#ifndef _CRT_MSVCP_CURRENT
#ifdef _DEBUG
#define _CRT_MSVCP_CURRENT "MSVCP100D.dll"
#else
#define _CRT_MSVCP_CURRENT "MSVCP100.dll"
#endif
#endif

		/* NAMING PROPERTIES */
#define _WIN32_C_LIB	1

		/* THREAD AND LOCALE CONTROL */
#define _MULTI_THREAD	1	/* nontrivial locks if multithreaded */
#define _IOSTREAM_OP_LOCKS	1	/* lock iostream operations */
#define _GLOBAL_LOCALE	0	/* 0 for per-thread locales, 1 for shared */

		/* THREAD-LOCAL STORAGE */
#define _COMPILER_TLS	1	/* 1 if compiler supports TLS directly */
 #if _MULTI_THREAD
  #define _TLS_QUAL	__declspec(thread)	/* TLS qualifier, if any */

 #else /* _MULTI_THREAD */
  #define _TLS_QUAL
 #endif /* _MULTI_THREAD */

 #ifndef _HAS_EXCEPTIONS
  #define  _HAS_EXCEPTIONS  1	/* predefine as 0 to disable exceptions */
 #endif /* _HAS_EXCEPTIONS */

#ifndef _HAS_STRICT_CONFORMANCE
 #define _HAS_STRICT_CONFORMANCE 0
#endif /* _HAS_STRICT_CONFORMANCE */

 #ifndef _HAS_TR1_IMPORTS
  #define _HAS_TR1_IMPORTS	_HAS_CPP0X
 #endif

#define _GLOBAL_USING	1


#ifdef _ITERATOR_DEBUG_LEVEL /* A. _ITERATOR_DEBUG_LEVEL is already defined. */

	/* A1. Validate _ITERATOR_DEBUG_LEVEL. */
	#if _ITERATOR_DEBUG_LEVEL > 2 && defined(_DEBUG)
		#error _ITERATOR_DEBUG_LEVEL > 2 is not supported in debug mode.
	#elif _ITERATOR_DEBUG_LEVEL > 1 && !defined(_DEBUG)
		#error _ITERATOR_DEBUG_LEVEL > 1 is not supported in release mode.
	#endif

	/* A2. Inspect _HAS_ITERATOR_DEBUGGING. */
	#ifdef _HAS_ITERATOR_DEBUGGING /* A2i. _HAS_ITERATOR_DEBUGGING is already defined, validate it. */
		#if _ITERATOR_DEBUG_LEVEL == 2 && _HAS_ITERATOR_DEBUGGING != 1
			#error _ITERATOR_DEBUG_LEVEL == 2 must imply _HAS_ITERATOR_DEBUGGING == 1 .
		#elif _ITERATOR_DEBUG_LEVEL < 2 && _HAS_ITERATOR_DEBUGGING != 0
			#error _ITERATOR_DEBUG_LEVEL < 2 must imply _HAS_ITERATOR_DEBUGGING == 0 .
		#endif
	#else /* A2ii. _HAS_ITERATOR_DEBUGGING is not yet defined, derive it. */
		#if _ITERATOR_DEBUG_LEVEL == 2
			#define _HAS_ITERATOR_DEBUGGING 1
		#else
			#define _HAS_ITERATOR_DEBUGGING 0
		#endif
	#endif /* _HAS_ITERATOR_DEBUGGING */

	/* A3. Inspect _SECURE_SCL. */
	#ifdef _SECURE_SCL /* A3i. _SECURE_SCL is already defined, validate it. */
		#if _ITERATOR_DEBUG_LEVEL > 0 && _SECURE_SCL != 1
			#error _ITERATOR_DEBUG_LEVEL > 0 must imply _SECURE_SCL == 1 .
		#elif _ITERATOR_DEBUG_LEVEL == 0 && _SECURE_SCL != 0
			#error _ITERATOR_DEBUG_LEVEL == 0 must imply _SECURE_SCL == 0 .
		#endif
	#else /* A3ii. _SECURE_SCL is not yet defined, derive it. */
		#if _ITERATOR_DEBUG_LEVEL > 0
			#define _SECURE_SCL 1
		#else
			#define _SECURE_SCL 0
		#endif
	#endif /* _SECURE_SCL */

#else /* B. _ITERATOR_DEBUG_LEVEL is not yet defined. */

	/* B1. Inspect _HAS_ITERATOR_DEBUGGING. */
	#ifdef _HAS_ITERATOR_DEBUGGING /* B1i. _HAS_ITERATOR_DEBUGGING is already defined, validate it. */
		#if _HAS_ITERATOR_DEBUGGING > 1
			#error _HAS_ITERATOR_DEBUGGING must be either 0 or 1 .
		#elif _HAS_ITERATOR_DEBUGGING == 1 && !defined(_DEBUG)
			#error _HAS_ITERATOR_DEBUGGING == 1 is not supported in release mode.
		#endif
	#else /* B1ii. _HAS_ITERATOR_DEBUGGING is not yet defined, default it. */
		#ifdef _DEBUG
			#define _HAS_ITERATOR_DEBUGGING 1
		#else
			#define _HAS_ITERATOR_DEBUGGING 0
		#endif
	#endif /* _HAS_ITERATOR_DEBUGGING */

	/* B2. Inspect _SECURE_SCL. */
	#ifdef _SECURE_SCL /* B2i. _SECURE_SCL is already defined, validate it. */
		#if _SECURE_SCL > 1
			#error _SECURE_SCL must be either 0 or 1 .
		#endif
	#else /* B2ii. _SECURE_SCL is not yet defined, default it. */
		#if _HAS_ITERATOR_DEBUGGING == 1
			#define _SECURE_SCL 1
		#else
			#define _SECURE_SCL 0
		#endif
	#endif /* _SECURE_SCL */

	/* B3. Derive _ITERATOR_DEBUG_LEVEL. */
	#if _HAS_ITERATOR_DEBUGGING
		#define _ITERATOR_DEBUG_LEVEL 2
	#elif _SECURE_SCL
		#define _ITERATOR_DEBUG_LEVEL 1
	#else
		#define _ITERATOR_DEBUG_LEVEL 0
	#endif

#endif /* _ITERATOR_DEBUG_LEVEL */


#if defined(_CRTBLD) && defined(_DLL)
	#define _ALLOW_ITERATOR_DEBUG_LEVEL_MISMATCH
#endif

#ifdef __cplusplus
	#ifndef _ALLOW_MSC_VER_MISMATCH
		#pragma detect_mismatch("_MSC_VER", "1600")
	#endif /* _ALLOW_MSC_VER_MISMATCH */

	#ifndef _ALLOW_ITERATOR_DEBUG_LEVEL_MISMATCH
		#if _ITERATOR_DEBUG_LEVEL == 0
		     #pragma detect_mismatch("_ITERATOR_DEBUG_LEVEL", "0")
		#elif _ITERATOR_DEBUG_LEVEL == 1
		     #pragma detect_mismatch("_ITERATOR_DEBUG_LEVEL", "1")
		#elif _ITERATOR_DEBUG_LEVEL == 2
		     #pragma detect_mismatch("_ITERATOR_DEBUG_LEVEL", "2")
		#else
		     #error Unrecognized _ITERATOR_DEBUG_LEVEL value.
		#endif
	#endif /* _ALLOW_ITERATOR_DEBUG_LEVEL_MISMATCH */
#endif /* __cplusplus */


/* See note on use of deprecate at the top of this file */
#if !defined(_SCL_SECURE_NO_WARNINGS) && defined(_SCL_SECURE_NO_DEPRECATE)
#define _SCL_SECURE_NO_WARNINGS
#endif

#if !defined (_SECURE_SCL_DEPRECATE)
#if defined(_SCL_SECURE_NO_WARNINGS)
#define _SECURE_SCL_DEPRECATE 0
#else
#define _SECURE_SCL_DEPRECATE 1
#endif
#endif

#if !defined (_SECURE_SCL_THROWS)
#define _SECURE_SCL_THROWS 0
#endif

/* _SECURE_SCL switches: helper macros */
/* See note on use of deprecate at the top of this file */

#if _ITERATOR_DEBUG_LEVEL > 0 && _SECURE_SCL_DEPRECATE
#define _SCL_INSECURE_DEPRECATE \
	_CRT_DEPRECATE_TEXT( \
		"Function call with parameters that may be unsafe - this call relies on the caller to check that the passed values are correct. " \
		"To disable this warning, use -D_SCL_SECURE_NO_WARNINGS. See documentation on how to use Visual C++ 'Checked Iterators'")
#else
#define _SCL_INSECURE_DEPRECATE
#endif


#ifndef _SCL_SECURE_INVALID_PARAMETER
 #define _SCL_SECURE_INVALID_PARAMETER(expr) _CRT_SECURE_INVALID_PARAMETER(expr)
#endif


/* ------------------------------------------------------------------------ */
/* Forward declare these now because they are used as non-dependent names.  */
#ifdef _DEBUG

#if !defined(_NATIVE_WCHAR_T_DEFINED) && defined(_M_CEE_PURE)
extern "C++"
#else
extern "C"
#endif
_CRTIMP void __cdecl _invalid_parameter(_In_opt_z_ const wchar_t *, _In_opt_z_ const wchar_t *, _In_opt_z_ const wchar_t *, unsigned int, uintptr_t);

#else /* _DEBUG */

extern "C"
_CRTIMP void __cdecl _invalid_parameter_noinfo(void);

#endif /* def _DEBUG */
/* ------------------------------------------------------------------------ */


 #if _SECURE_SCL_THROWS

 #ifndef _SILENCE_DEPRECATION_OF_SECURE_SCL_THROWS
  #include <crtwrn.h>
  #pragma push_macro("_SECURE_SCL_THROWS")
  #undef _SECURE_SCL_THROWS
  #pragma _CRT_WARNING( _DEPRECATE_SECURE_SCL_THROWS )
  #pragma pop_macro("_SECURE_SCL_THROWS")
 #endif /* _SILENCE_DEPRECATION_OF_SECURE_SCL_THROWS */

 #define _SCL_SECURE_INVALID_ARGUMENT_NO_ASSERT		_STD _Xinvalid_argument("invalid argument")
 #define _SCL_SECURE_OUT_OF_RANGE_NO_ASSERT			_STD _Xout_of_range("out of range")

 #else /* _SECURE_SCL_THROWS */

 #define _SCL_SECURE_INVALID_ARGUMENT_NO_ASSERT		_SCL_SECURE_INVALID_PARAMETER("invalid argument")
 #define _SCL_SECURE_OUT_OF_RANGE_NO_ASSERT			_SCL_SECURE_INVALID_PARAMETER("out of range")

 #endif /* _SECURE_SCL_THROWS */

 #define _SCL_SECURE_ALWAYS_VALIDATE(cond)				\
	{													\
		if (!(cond))									\
		{												\
			_ASSERTE(#cond && 0);						\
			_SCL_SECURE_INVALID_ARGUMENT_NO_ASSERT;		\
		}												\
		__analysis_assume(cond);						\
	}

 #define _SCL_SECURE_ALWAYS_VALIDATE_RANGE(cond)		\
	{													\
		if (!(cond))									\
		{												\
			_ASSERTE(#cond && 0);						\
			_SCL_SECURE_OUT_OF_RANGE_NO_ASSERT;			\
		}												\
		__analysis_assume(cond);						\
	}

 #define _SCL_SECURE_CRT_VALIDATE(cond, retvalue)		\
	{													\
		if (!(cond))									\
		{												\
			_ASSERTE(#cond && 0);						\
			_SCL_SECURE_INVALID_PARAMETER(cond);		\
			return (retvalue);							\
		}												\
	}

 #if _ITERATOR_DEBUG_LEVEL > 0

 #define _SCL_SECURE_VALIDATE(cond)						\
	{													\
		if (!(cond))									\
		{												\
			_ASSERTE(#cond && 0);						\
			_SCL_SECURE_INVALID_ARGUMENT_NO_ASSERT;		\
		}												\
		__analysis_assume(cond);						\
	}

 #define _SCL_SECURE_VALIDATE_RANGE(cond)				\
	{													\
		if (!(cond))									\
		{												\
			_ASSERTE(#cond && 0);						\
			_SCL_SECURE_OUT_OF_RANGE_NO_ASSERT;			\
		}												\
		__analysis_assume(cond);						\
	}

 #define _SCL_SECURE_INVALID_ARGUMENT					\
	{													\
		_ASSERTE("Standard C++ Libraries Invalid Argument" && 0); \
		_SCL_SECURE_INVALID_ARGUMENT_NO_ASSERT;			\
	}
 #define _SCL_SECURE_OUT_OF_RANGE						\
	{													\
		_ASSERTE("Standard C++ Libraries Out of Range" && 0); \
		_SCL_SECURE_OUT_OF_RANGE_NO_ASSERT;				\
	}

 #else /* _ITERATOR_DEBUG_LEVEL > 0 */
 
/* when users disable _SECURE_SCL to get performance, we don't want analysis warnings from SCL headers */
#if _ITERATOR_DEBUG_LEVEL == 2
 #define _SCL_SECURE_VALIDATE(cond)			__analysis_assume(cond)
 #define _SCL_SECURE_VALIDATE_RANGE(cond)	__analysis_assume(cond)
#else
 #define _SCL_SECURE_VALIDATE(cond)
 #define _SCL_SECURE_VALIDATE_RANGE(cond)
#endif

 #define _SCL_SECURE_INVALID_ARGUMENT 
 #define _SCL_SECURE_OUT_OF_RANGE 

 #endif /* _ITERATOR_DEBUG_LEVEL > 0 */

#if __STDC_WANT_SECURE_LIB__
#define _CRT_SECURE_MEMCPY(dest, destsize, source, count) ::memcpy_s((dest), (destsize), (source), (count))
#define _CRT_SECURE_MEMMOVE(dest, destsize, source, count) ::memmove_s((dest), (destsize), (source), (count))
#define _CRT_SECURE_WMEMCPY(dest, destsize, source, count) ::wmemcpy_s((dest), (destsize), (source), (count))
#define _CRT_SECURE_WMEMMOVE(dest, destsize, source, count) ::wmemmove_s((dest), (destsize), (source), (count))
#else
#define _CRT_SECURE_MEMCPY(dest, destsize, source, count) ::memcpy((dest), (source), (count))
#define _CRT_SECURE_MEMMOVE(dest, destsize, source, count) ::memmove((dest), (source), (count))
#define _CRT_SECURE_WMEMCPY(dest, destsize, source, count) ::wmemcpy((dest), (source), (count))
#define _CRT_SECURE_WMEMMOVE(dest, destsize, source, count) ::wmemmove((dest), (source), (count))
#endif

#include <use_ansi.h>

#if defined(_M_CEE) && defined(_STATIC_CPPLIB)
#include <crtwrn.h>
#pragma push_macro("_STATIC_CPPLIB")
#undef _STATIC_CPPLIB
#pragma _CRT_WARNING( _CLR_AND_STATIC_CPPLIB )
#pragma pop_macro("_STATIC_CPPLIB")
#endif

#if defined(_DLL) && defined(_STATIC_CPPLIB) && !defined(_DISABLE_DEPRECATE_STATIC_CPPLIB)
#include <crtwrn.h>
#pragma push_macro("_STATIC_CPPLIB")
#undef _STATIC_CPPLIB
#pragma _CRT_WARNING( _DEPRECATE_STATIC_CPPLIB )
#pragma pop_macro("_STATIC_CPPLIB")
#endif

/* Define _CRTIMP2 */
 #ifndef _CRTIMP2

   #if defined(_DLL) && !defined(_STATIC_CPPLIB)
    #define _CRTIMP2	__declspec(dllimport)

   #else   /* ndef _DLL && !STATIC_CPPLIB */
    #define _CRTIMP2
   #endif  /* _DLL && !STATIC_CPPLIB */

 #endif  /* _CRTIMP2 */

/* Define _CRTIMP2_NCEEPURE */
 #ifndef _CRTIMP2_NCEEPURE
  #if defined(_M_CEE_PURE)
   #define _CRTIMP2_NCEEPURE
  #else
   #define _CRTIMP2_NCEEPURE _CRTIMP2
  #endif
 #endif

#ifndef _MRTIMP
    #define _MRTIMP __declspec(dllimport)
#endif  /* _MRTIMP */

/* Define _MRTIMP2 */
 #ifndef _MRTIMP2

   #if defined(_DLL) && !defined(_STATIC_CPPLIB)
    #define _MRTIMP2	__declspec(dllimport)

   #else   /* ndef _DLL && !STATIC_CPPLIB */
    #define _MRTIMP2
   #endif  /* _DLL && !STATIC_CPPLIB */

 #endif  /* _MRTIMP2 */

 #ifndef _MRTIMP2_PURE
  #if defined(_M_CEE_PURE)
   #define _MRTIMP2_PURE
  #else
   #define _MRTIMP2_PURE _MRTIMP2
  #endif
 #endif

 #ifndef _MRTIMP2_PURE_NPURE
  #if defined(_M_CEE_PURE)
   #define _MRTIMP2_PURE_NPURE
  #else
   #define _MRTIMP2_PURE_NPURE _MRTIMP2_NPURE
  #endif
 #endif

/* Define _MRTIMP2_NPURE */
 #ifndef _MRTIMP2_NPURE

   #if defined(_DLL) && defined(_M_CEE_PURE)
    #define _MRTIMP2_NPURE	__declspec(dllimport)

   #else
    #define _MRTIMP2_NPURE
   #endif

 #endif  /* _MRTIMP2_NPURE */

/* Define _MRTIMP2_NCEE */
 #ifndef _MRTIMP2_NCEE
  #if defined(_M_CEE)
   #define _MRTIMP2_NCEE
  #else
   #define _MRTIMP2_NCEE _MRTIMP2
  #endif
 #endif

/* Define _MRTIMP2_NCEEPURE */
 #ifndef _MRTIMP2_NCEEPURE
  #if defined(_M_CEE_PURE)
   #define _MRTIMP2_NCEEPURE
  #else
   #define _MRTIMP2_NCEEPURE _MRTIMP2
  #endif
 #endif

/* Define _MRTIMP2_NPURE_NCEEPURE */
 #ifndef _MRTIMP2_NPURE_NCEEPURE
  #if defined(_M_CEE_PURE)
   #define _MRTIMP2_NPURE_NCEEPURE
  #else
   #define _MRTIMP2_NPURE_NCEEPURE _MRTIMP2_NPURE
  #endif
 #endif

 #if defined(_DLL) && !defined(_STATIC_CPPLIB) && !defined(_M_CEE_PURE)
  #define _DLL_CPPLIB
 #endif

 #ifndef _CRTIMP2_PURE
   #ifdef  _M_CEE_PURE
     #define _CRTIMP2_PURE
   #else
     #define _CRTIMP2_PURE _CRTIMP2
   #endif
 #endif

#if !defined(_CRTDATA2)
    #define _CRTDATA2 _CRTIMP2
#endif



		/* NAMESPACE */

 #if defined(__cplusplus)
  #define _STD_BEGIN	namespace std {
  #define _STD_END		}
  #define _STD	::std::

/*
We use the stdext (standard extension) namespace to contain extensions that are not part of the current standard
*/
  #define _STDEXT_BEGIN	    namespace stdext {
  #define _STDEXT_END		}
  #define _STDEXT	        ::stdext::

  #ifdef _STD_USING
   #define _C_STD_BEGIN	namespace std {	/* only if *.c compiled as C++ */
   #define _C_STD_END	}
   #define _CSTD	::std::

  #else /* _STD_USING */
/* #define _GLOBAL_USING	*.h in global namespace, c* imports to std */

   #define _C_STD_BEGIN
   #define _C_STD_END
   #define _CSTD	::
  #endif /* _STD_USING */

  #define _C_LIB_DECL		extern "C" {	/* C has extern "C" linkage */
  #define _END_C_LIB_DECL	}
  #define _EXTERN_C			extern "C" {
  #define _END_EXTERN_C		}

 #else /* __cplusplus */
  #define _STD_BEGIN
  #define _STD_END
  #define _STD

  #define _C_STD_BEGIN
  #define _C_STD_END
  #define _CSTD

  #define _C_LIB_DECL
  #define _END_C_LIB_DECL
  #define _EXTERN_C
  #define _END_EXTERN_C
 #endif /* __cplusplus */

 #define _Restrict	restrict

 #ifdef __cplusplus
_STD_BEGIN
typedef bool _Bool;
_STD_END
 #endif /* __cplusplus */

		/* VC++ COMPILER PARAMETERS */
#define _LONGLONG	__int64
#define _ULONGLONG	unsigned __int64
#define _LLONG_MAX	0x7fffffffffffffffi64
#define _ULLONG_MAX	0xffffffffffffffffui64

		/* INTEGER PROPERTIES */
#define _C2			1	/* 0 if not 2's complement */

#define _MAX_EXP_DIG	8	/* for parsing numerics */
#define _MAX_INT_DIG	32
#define _MAX_SIG_DIG	36

typedef _LONGLONG _Longlong;
typedef _ULONGLONG _ULonglong;

		/* STDIO PROPERTIES */
#define _Filet _iobuf

 #ifndef _FPOS_T_DEFINED
  #define _FPOSOFF(fp)	((long)(fp))
 #endif /* _FPOS_T_DEFINED */

#define _IOBASE	_base
#define _IOPTR	_ptr
#define _IOCNT	_cnt

#ifndef _HAS_CHAR16_T_LANGUAGE_SUPPORT
 #define _HAS_CHAR16_T_LANGUAGE_SUPPORT 0
#endif /* _HAS_CHAR16_T_LANGUAGE_SUPPORT */

		/* uchar PROPERTIES */
 #if _HAS_CHAR16_T_LANGUAGE_SUPPORT
 #else /* _HAS_CHAR16_T_LANGUAGE_SUPPORT */
 #if !defined(_CHAR16T)
  #define _CHAR16T
typedef unsigned short char16_t;
typedef unsigned int char32_t;
 #endif /* !defined(_CHAR16T) */

 #endif /* _HAS_CHAR16_T_LANGUAGE_SUPPORT */

		/* MULTITHREAD PROPERTIES */
		/* LOCK MACROS */
#define _LOCK_LOCALE	0
#define _LOCK_MALLOC	1
#define _LOCK_STREAM	2
#define _LOCK_DEBUG		3
#define _MAX_LOCK		4	/* one more than highest lock number */

 #ifdef __cplusplus
_STD_BEGIN
enum _Uninitialized
	{	// tag for suppressing initialization
	_Noinit
	};

		// CLASS _Lockit
// warning 4412 is benign here
#pragma warning(push)
#pragma warning(disable:4412)
class _CRTIMP2_PURE _Lockit
	{	// lock while object in existence -- MUST NEST
public:
 #if _MULTI_THREAD

  #if defined(_M_CEE_PURE) || defined(MRTDLL)
	__CLR_OR_THIS_CALL _Lockit()
        : _Locktype(0)
	    {	// default construct
        _Lockit_ctor(this);
	    }

	explicit __CLR_OR_THIS_CALL _Lockit(int _Kind)
	    {	// set the lock
        _Lockit_ctor(this, _Kind);
	    }

	__CLR_OR_THIS_CALL ~_Lockit()
	    {	// clear the lock
        _Lockit_dtor(this);
	    }

  #else /* defined(_M_CEE_PURE) || defined(MRTDLL) */
	__thiscall _Lockit();	// default construct
	explicit __thiscall _Lockit(int);	// set the lock
	__thiscall ~_Lockit();	// clear the lock
  #endif /* defined(_M_CEE_PURE) || defined(MRTDLL) */

    static _MRTIMP2_NPURE void __cdecl _Lockit_ctor(int);
    static _MRTIMP2_NPURE void __cdecl _Lockit_dtor(int);

private:
    static _MRTIMP2_NPURE void __cdecl _Lockit_ctor(_Lockit *);
    static _MRTIMP2_NPURE void __cdecl _Lockit_ctor(_Lockit *, int);
    static _MRTIMP2_NPURE void __cdecl _Lockit_dtor(_Lockit *);
#if 0 /* Disabled for gccxml */
	__CLR_OR_THIS_CALL _Lockit(const _Lockit&);				// not defined
	_Lockit& __CLR_OR_THIS_CALL operator=(const _Lockit&);	// not defined
#endif /* Disabled for gccxml */
	int _Locktype;

  #else /* _MULTI_THREAD */
	_Lockit()
		{	// do nothing
		}

	explicit _Lockit(int)
		{	// do nothing
		}

	~_Lockit()
		{	// do nothing
		}
  #endif /* _MULTI_THREAD */
	};

 #ifdef _M_CEE
class _CRTIMP2_PURE _EmptyLockit
	{	// empty lock class used for bin compat
public:
  #if _MULTI_THREAD
private:
	int _Locktype;
  #endif /* _MULTI_THREAD */
	};

  #if defined(__cplusplus_cli)
   #define _M_CEE_FINALLY finally
  #else /* defined(__cplusplus_cli) */
   #define _M_CEE_FINALLY __finally
  #endif /* defined(__cplusplus_cli) */

  #define _BEGIN_LOCK(_Kind) \
	{ \
		typedef int _TmpTestType; \
		__if_exists(_TmpTestType::ToString) \
		{ \
		bool _MustReleaseLock = false; \
		int _LockKind = _Kind; \
		System::Runtime::CompilerServices::RuntimeHelpers::PrepareConstrainedRegions(); \
		try \
		} \
		{ \
			__if_exists(_TmpTestType::ToString) \
			{ \
			System::Runtime::CompilerServices::RuntimeHelpers::PrepareConstrainedRegions(); \
			try { } _M_CEE_FINALLY \
			{ \
				_STD _Lockit::_Lockit_ctor(_LockKind); \
				_MustReleaseLock = true; \
			} \
			} \
			__if_not_exists(_TmpTestType::ToString) \
			{ \
			_STD _Lockit _Lock(_Kind); \
			}

  #define _END_LOCK() \
		} \
		__if_exists(_TmpTestType::ToString) \
		{ \
		_M_CEE_FINALLY \
		{ \
			if (_MustReleaseLock) \
			{ \
				_STD _Lockit::_Lockit_dtor(_LockKind); \
			} \
		} \
		} \
	}

  #define _BEGIN_LOCINFO(_VarName) \
	_BEGIN_LOCK(_LOCK_LOCALE) \
	_Locinfo _VarName;

  #define _END_LOCINFO() \
	_END_LOCK() \

  #define _RELIABILITY_CONTRACT \
	[System::Runtime::ConstrainedExecution::ReliabilityContract( \
		System::Runtime::ConstrainedExecution::Consistency::WillNotCorruptState, \
		System::Runtime::ConstrainedExecution::Cer::Success)]

 #else /* _M_CEE */
  #define _BEGIN_LOCK(_Kind) \
	{ \
		_STD _Lockit _Lock(_Kind);

  #define _END_LOCK() \
	}

  #define _BEGIN_LOCINFO(_VarName) \
	{ \
		_Locinfo _VarName;

  #define _END_LOCINFO() \
	}

  #define _RELIABILITY_CONTRACT
 #endif /* _M_CEE */

class _CRTIMP2_PURE _Mutex
	{	// lock under program control
public:

 #if _MULTI_THREAD
  #if defined(_M_CEE_PURE) || defined(MRTDLL)
	__CLR_OR_THIS_CALL _Mutex(_Uninitialized)
	    {	// do nothing
	    }

	__CLR_OR_THIS_CALL _Mutex()
	    {	// default construct
        _Mutex_ctor(this);
	    }

	__CLR_OR_THIS_CALL ~_Mutex()
	    {	// destroy the object
        _Mutex_dtor(this);
	    }

	void __CLR_OR_THIS_CALL _Lock()
	    {	// lock the mutex
        _Mutex_Lock(this);
	    }

	void __CLR_OR_THIS_CALL _Unlock()
	    {	// unlock the mutex
        _Mutex_Unlock(this);
	    }

  #else /* defined(_M_CEE_PURE) || defined(MRTDLL) */
    __thiscall _Mutex(_Uninitialized)
		{	// do nothing
		}

    __thiscall _Mutex();
	__thiscall ~_Mutex();
	void __thiscall _Lock();
	void __thiscall _Unlock();
  #endif /* defined(_M_CEE_PURE) || defined(MRTDLL) */

private:
    static _MRTIMP2_NPURE_NCEEPURE void __CLRCALL_PURE_OR_CDECL _Mutex_ctor(_Mutex *);
    static _MRTIMP2_NPURE_NCEEPURE void __CLRCALL_PURE_OR_CDECL _Mutex_dtor(_Mutex *);
    static _MRTIMP2_NPURE_NCEEPURE void __CLRCALL_PURE_OR_CDECL _Mutex_Lock(_Mutex *);
    static _MRTIMP2_NPURE_NCEEPURE void __CLRCALL_PURE_OR_CDECL _Mutex_Unlock(_Mutex *);

	__CLR_OR_THIS_CALL _Mutex(const _Mutex&);				// not defined
	_Mutex& __CLR_OR_THIS_CALL operator=(const _Mutex&);	// not defined
	void *_Mtx;

  #else /* _MULTI_THREAD */
    void _Lock()
		{	// do nothing
		}

	void _Unlock()
		{	// do nothing
		}
  #endif /* _MULTI_THREAD */
	};

class _CRTIMP2_PURE _Init_locks
	{	// initialize mutexes
public:
 #if _MULTI_THREAD
      #if defined(_M_CEE_PURE) || defined(MRTDLL)
	__CLR_OR_THIS_CALL _Init_locks()
	    {	// default construct
        _Init_locks_ctor(this);
	    }

	__CLR_OR_THIS_CALL ~_Init_locks()
	    {	// destroy the object
        _Init_locks_dtor(this);
	    }

  #else /* defined(_M_CEE_PURE) || defined(MRTDLL) */
    __thiscall _Init_locks();
	__thiscall ~_Init_locks();
  #endif /* defined(_M_CEE_PURE) || defined(MRTDLL) */

private:
    static _MRTIMP2_NPURE void __cdecl _Init_locks_ctor(_Init_locks *);
    static _MRTIMP2_NPURE void __cdecl _Init_locks_dtor(_Init_locks *);

 #else /* _MULTI_THREAD */
	_Init_locks()
		{	// do nothing
		}

	~_Init_locks()
		{	// do nothing
		}
 #endif /* _MULTI_THREAD */
	};

#pragma warning(pop)
_STD_END
 #endif /* __cplusplus */

#ifndef _RELIABILITY_CONTRACT
 #define _RELIABILITY_CONTRACT
#endif /* _RELIABILITY_CONTRACT */

		/* MISCELLANEOUS MACROS AND TYPES */
_C_STD_BEGIN
_MRTIMP2 void __cdecl _Atexit(void (__cdecl *)(void));

typedef int _Mbstatet;
typedef unsigned long _Uint32t;

#define _ATEXIT_T	void
#define _Mbstinit(x)	mbstate_t x = {0}
_C_STD_END

 #define _THROW_BAD_ALLOC	_THROW1(...)

 #pragma pack(pop)

#endif /* _YVALS */


/*
 * Copyright (c) 1992-2009 by P.J. Plauger.  ALL RIGHTS RESERVED.
 * Consult your license regarding permissions and restrictions.
 V5.20:0009 */
