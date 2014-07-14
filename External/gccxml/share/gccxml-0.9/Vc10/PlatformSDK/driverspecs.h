/*****************************************************************************\
*                                                                             *
* DriverSpecs.h - markers for documenting the semantics of driver APIs        *
*                 See also <SpecStrings.h>                                    *
*                                                                             *
* Version 1.2.10                                                              *
*                                                                             *
* Copyright (c) Microsoft Corporation. All rights reserved.                   *
*                                                                             *
\*****************************************************************************/

/*****************************************************************************\
* NOTE																		  *
* NOTE																		  *
* NOTE																		  *
*   The macro bodies in this file are subject to change without notice.       *
*   Attempting to use the annotations in the macro bodies directly is not     *
*   supported.																  *
* NOTE																		  *
* NOTE																		  *
* NOTE																		  *
\*****************************************************************************/

/*****************************************************************************\
* The annotations described by KernelSpecs.h and DriverSpecs.h, taken together,
* are used to annotate drivers.  Many of the annotations are applicable to
* user space code (including subsystems) as well as to drivers.
*
* DriverSpecs.h contains those annotations which are appropriate to userspace
* code, or which might appear in headers that are shared between user space
* and kernel space.  In the case of annotations which might appear in such a
* shared header, but which are meaningless in user space, the annotations are
* #defined to nothing in DriverSpecs.h.
*
* KernelSpecs.h contains those annotations which either will only appear in
* kernel code or headers; or which might appear in shared headers.  In the
* latter case, it is assumed that DriverSpecs.h has been #included, and
* the anntoations are re-defined (using #undef) to give them a meaningful
* value.  In general, documentation for the shared-header annotations appears
* in DriverSpecs.h.
*
* Many annotations are context dependent.  They only apply to certain versions
* of Windows, or only to certain classes of driver.  These rules can be written
* using something like __drv_when(NTDDI_VERSION >= NTDDI_WINXP, ...)
* which causes the rule only to apply to Windows XP and later.  Many of these
* symbols are already defined in various Windows headers.
*
* To facilitate using this sort of conditional rule, we collect here the
* various known symbols that are (or reasonably might) be used in such
* a conditional annotation.  Some are speculative in that the symbol has
* not yet been defined because there are no known uses of it yet.
*
* Where the symbol already exists its relevant header is
* noted below (excluding the "really well known" ones).
*
* Each symbol is listed with the currently known possible values.
*
* Some symbols are marked as #define symbols -- they are used with #ifdef
* operators only.  To use them in __drv_when, use something like
* __drv_when(__drv_defined(NT), ...).
*
* WDK Version (copied for convenience from sdkddkver.h)
*     NTDDI_VERSION: NTDDI_WIN2K NTDDI_WIN2KSP1 NTDDI_WIN2KSP2 NTDDI_WIN2KSP3 
*                    NTDDI_WIN2KSP4 NTDDI_WINXP NTDDI_WINXPSP1 NTDDI_WINXPSP2 
*					 NTDDI_WS03 NTDDI_WS03SP1 NTDDI_VISTA
*     The WDK version is taken as the WDM version as well.
*
* OS Version: (copied for convenience from sdkddkver.h)
*     _WIN32_WINNT: _WIN32_WINNT_NT4 _WIN32_WINNT_WIN2K _WIN32_WINNT_WINXP 
*                   _WIN32_WINNT_WS03 _WIN32_WINNT_LONGHORN
*     WINVER: 0x030B 0x0400 0x0500 0x0600
*     NT (#define symbol)
* (sdkddkver.h also defines symbols for IE versions should they be needed.)
*
* Compiler Version:
*	  _MSC_VER: too many to list.
*	  _MSC_FULL_VER: too many to list.
*
* KMDF Version:  (Currently defined/used only in makefiles.)
*     KMDF_VERSION_MAJOR: 1     
*
* UMDF Version:  (Currently defined/used only in makefiles.)
*     UMDF_VERSION_MAJOR: 1     
*
* Architecture kinds:
*     __WIN64 (#define symbols)
*     _X86_
*     _AMD64_
*     _IA64_ 
*
* Machine Architectures:
*     _M_IX86
*     _M_AMD64
*     _M_IA64
*
* Driver Kind (NYI: "not yet implemented")
*   Typically these will be defined in the most-common header for a 
*   particular driver (or in individual source files if appropriate).
*   These are not intended to necessarily be orthogonal: more than one might 
*   apply to a particular driver.
*     _DRIVER_TYPE_BUS: 1                // NYI
*     _DRIVER_TYPE_FUNCTIONAL: 1         // NYI
*     _DRIVER_TYPE_MINIPORT: 1           // NYI
*     _DRIVER_TYPE_STORAGE: 1            // NYI
*     _DRIVER_TYPE_DISPLAY: 1            // NYI
*     _DRIVER_TYPE_FILESYSTEM: 1
*     _DRIVER_TYPE_FILESYSTEM_FILTER: 1
*
* NDIS driver version: (see ndis.h for much more detail.)
*   These can be used to both identify an NDIS driver and to check the version.
*     NDIS40 NDIS50 NDIS51 NDIS60 (#defined symbols)
*     NDIS_PROTOCOL_MAJOR_VERSION.NDIS_PROTOCOL_MINOR_VERSION: 4.0 5.0 5.1 6.0
*     And many others in ndis.h (including MINIPORT)
*
\*****************************************************************************/

#ifndef DRIVERSPECS_H
#define DRIVERSPECS_H

// In case driverspecs.h is included directly (and w/o specstrings.h)
#ifndef SPECSTRINGS_H
#include <specstrings.h>
#endif

#include "sdv_driverspecs.h"

#if _MSC_VER > 1000 // [
#pragma once
#endif // ]

#ifdef  __cplusplus // [
extern "C" {
#endif  // ]

#if (_MSC_VER >= 1000) && !defined(__midl) && defined(_PREFAST_) // [
#define __drv_declspec(x) __declspec(x)
#define __$drv_group(annotes)												\
	  __drv_declspec("SAL_begin") annotes __drv_declspec("SAL_end")
#define __drv_nop(x) x
#else // ][
#define __drv_declspec(x)
#define __$drv_group(x)
#endif // ]

#if (_MSC_VER >= 1000) && !defined(__midl) && defined(_PREFAST_) && defined(_MSC_EXTENSIONS)// [


	// Synthesize a unique symbol.
	#define $$MKID(x, y) x ## y
	#define $MKID(x, y) $$MKID(x, y)
	#define $GENSYM(x) $MKID(x, __COUNTER__)


	// ---------------------------------------------------------------------
	// Processing mode selection:
	//
	// __internal_kernel_driver
	//
	// Flag for headers that indicates a probable driver.
	// This should only be coded in headers that are normally used 
	// as the "primary" header for a class of drivers.  It sets the
	// default to kernel mode driver.
	//
	// ';' inside the parens to keep MIDL happy
	__ANNOTATION(SAL_internal_kernel_driver();)
	#define __internal_kernel_driver 										\
		typedef int __drv_declspec("SAL_internal_kernel_driver") 			\
					$GENSYM(__prefast_flag_kernel_driver_mode);

	//
	// __kernel_code
	// __kernel_driver
	// __user_driver
	// __user_code
	//
	// Flags for compilation units that indicated specifically what kind of 
	// code it is.
	// These should be coded as early as possible in any compilation unit 
	// (.c/.cpp file) that doesn't get the correct default.  Whether before
	// or after __internal_kernel_driver
	// 
	// Indicate that the code is kernel, but not driver, code.

	__ANNOTATION(SAL_kernel();)
	__ANNOTATION(SAL_nokernel();)
	__ANNOTATION(SAL_driver();)
	__ANNOTATION(SAL_nodriver();)

	#define __kernel_code 													\
		typedef int __drv_declspec("SAL_kernel")  							\
					__drv_declspec("SAL_nodriver")							\
					$GENSYM(__prefast_flag_kernel_driver_mode);

	// Indicate that the code is kernel, driver, code.
	#define __kernel_driver 												\
		typedef int __drv_declspec("SAL_kernel")   							\
					__drv_declspec("SAL_driver")  							\
					$GENSYM(__prefast_flag_kernel_driver_mode);

	// Indicate that the code is a user mode driver.
	#define __user_driver 													\
		typedef int __drv_declspec("SAL_nokernel")							\
					__drv_declspec("SAL_driver")   							\
					$GENSYM(__prefast_flag_kernel_driver_mode);

	// Indicate that the code is ordinary user mode code.
	#define __user_code 													\
		typedef int __drv_declspec("SAL_nokernel") 							\
					__drv_declspec("SAL_nodriver") 							\
					$GENSYM(__prefast_flag_kernel_driver_mode);

	// "landmark" function definition to pass information to the
	// analysis tools, as needed.

    __ANNOTATION(SAL_landmark(__in char *);)

    #define __drv_Mode_impl(x)												\
      __declspec("SAL_landmark(\"" #x "\")")								\
      __inline void $GENSYM(__SAL_dummy_)(void){}

    // Macros to declare a function to be a particular class
    // of driver.

	#define __drv_WDM __drv_Mode_impl(WDM)
	#define __drv_KMDF __drv_Mode_impl(KMDF)
	#define __drv_NDIS __drv_Mode_impl(NDIS)

    // Inform PREfast that operator new does [not] throw.
    // Be sure you really know which is actually in use before using one of
    // these.  The default is throwing (and cannot return NULL) which is
    // standard conformant, but much kernel code links with a non-throwing
    // operator new.
    //
    // Header <new> will set the default to throwing, so be sure to place
    // this after that header is included.
    //
    // Be sure to use these macros for this purpose as the implementation 
    // could change.

    #define __prefast_operator_new_throws                                 \
        void* __cdecl operator new(size_t size) throw(std::bad_alloc);    \
        void* __cdecl operator new[](size_t size) throw(std::bad_alloc);

    #define __prefast_operator_new_null                                   \
        void* __cdecl operator new(size_t size) throw();                  \
        void* __cdecl operator new[](size_t size) throw();


#else // ][

	#define __internal_kernel_driver
	#define __kernel_code 
	#define __kernel_driver
	#define __user_driver
	#define __user_code
	#define __drv_Mode_impl(x)
	#define __drv_WDM
	#define __drv_KMDF
	#define __drv_NDIS 
	#define __prefast_operator_new_throws
	#define __prefast_operator_new_null


#endif // ]

	// core macros: these provide syntatic wrappers to make other uses
	// simpler.
	// (Note: right now we can't safely use the ellipsis (...) macro
	// syntax.  If we could then '##__drv_nop(annotes)' below could be 
	// simply 'annotes', and we could code __$drv_group as __$drv_group(...) 
	// in the "expands to nothing" case.)
	//
	// For example:
	//	 __drv_in(__drv_nonconstant __setsIRQL)
	
	#define __drv_deref(annotes) __deref __$drv_group(##__drv_nop(annotes))
	#define __drv_in(annotes) __pre __$drv_group(##__drv_nop(annotes))
	#define __drv_in_deref(annotes) __pre __deref __$drv_group(##__drv_nop(annotes))
	#define __drv_out(annotes) __post __$drv_group(##__drv_nop(annotes))
	#define __drv_out_deref(annotes) __post __deref __$drv_group(##__drv_nop(annotes))
	#define __drv_when(cond, annotes)
	  //__drv_declspec("SAL_when(" SPECSTRINGIZE(cond) ")") __$drv_group(##__drv_nop(annotes))
	#define __drv_at(expr,annotes)\
	  __drv_declspec("SAL_at(" SPECSTRINGIZE(expr) ")") __$drv_group(##__drv_nop(annotes))

	#define __drv_fun(annotes) __drv_at(return,##__drv_nop(annotes))
	#define __drv_ret(annotes) __drv_at(return,##__drv_nop(annotes))
	#define __drv_arg(expr,annotes) __drv_at(expr,##__drv_nop(annotes))
	#define __drv_unit(p)                                                   \
	  typedef int __$drv_unit_##p                                           \
                $GENSYM(__prefast_flag_kernel_driver_mode);

	// Internal macros for convenience
	#define __$drv_unit_internal_kernel_driver								\
		 __drv_declspec("SAL_internal_kernel_driver")

	//
	// __drv_unit
	//
	// Flags for compilation units that indicated specifically what kind of
	// code it is.
	// These should be coded as early as possible in any compilation unit
	// (.c/.cpp file) that doesn't get the correct default.	 Whether before
	// or after __internal_kernel_driver is immaterial as long as it will
	// successfully parse.
	//
	// Indicate that the code is kernel, but not driver, code.
	#define __$drv_unit_kernel_code											\
			__drv_declspec("SAL_kernel")  __drv_declspec("SAL_nodriver")

	// Indicate that the code is kernel, driver, code.
	#define __$drv_unit_kernel_driver										\
			__drv_declspec("SAL_kernel") __drv_declspec("SAL_driver")

	// Indicate that the code is a user mode driver.
	#define __$drv_unit_user_driver											\
			__drv_declspec("SAL_nokernel") __drv_declspec("SAL_driver")

	// Indicate that the code is ordinary user mode code.
	#define __$drv_unit_user_code											\
			__drv_declspec("SAL_nokernel")	__drv_declspec("SAL_nodriver")


	// These are needed for backwards compatability.
	#ifndef __internal_kernel_driver

	#define __internal_kernel_driver   __drv_unit(internal_kernel_driver)
	#define __kernel_code			   __drv_unit(kernel_code)
	#define __kernel_driver			   __drv_unit(kernel_driver)
	#define __user_driver			   __drv_unit(user_driver)
	#define __user_code				   __drv_unit(user_code)

	#endif

	// ---------------------------------------------------------------------
	// Syntatic utilities:
	// 
	// Needed to make the annotations convenient to use.
	//
	// So we can use a macro name that might be used in #ifdef context,
	// where it's defined with no value.  
	// This should only be used inside a __drv_when condition.
	//
	#define __drv_defined(x) macroDefined$( #x )

	// ---------------------------------------------------------------------
	// Callback properties:
	//
	// __drv_functionClass(x)
	//
	// Flag that the  the annotated function
	// is a member of that function class.	Some class names are recognized
	// by PREfast itself for special treatment.
	// This can be tested by the condition function inFunctionClass$()
	//
	__ANNOTATION(SAL_functionClass(__in char *);)
	#define __drv_functionClass(x)
		//__drv_out(__drv_declspec("SAL_functionClass(\""#x"\")"))

	// ---------------------------------------------------------------------
	// Resources:
	// 
	// __drv_acquiresResource(kind)
	// __drv_releasesResource(kind)
	// __drv_acquiresResourceGlobal(kind,param)
	// __drv_releasesResourceGlobal(kind,param)
	// __drv_mustHold(kind)
	// __drv_neverHold(kind)
	// __drv_mustHoldGlobal(kind,param)
	// __drv_neverHoldGlobal(kind,param)
	//
	// Flag that the annotated parameter acquires a resource of type kind.
	//
	__ANNOTATION(SAL_acquire(__in char *);)
	#define __drv_acquiresResource(kind)									\
		__post __drv_declspec("SAL_acquire(\"" #kind "\")")

	//
	// Flag that the annotated parameter releases a resource of type kind.
	//
	__ANNOTATION(SAL_release(__in char *);)
	#define __drv_releasesResource(kind)									\
		__post __drv_declspec("SAL_release(\"" #kind "\")")

	//
	// Flag that the annotated object acquires a global (otherwise anonymous)
	// resource of type kind named by param.
	//
	__ANNOTATION(SAL_acquireGlobal(__in char *, ...);)
	#define __drv_innerAcquiresGlobal(kind, param)							\
		__post __drv_declspec("SAL_acquireGlobal(\"" #kind "\","			\
												 SPECSTRINGIZE(param\t)")")
	#define __drv_acquiresResourceGlobal(kind,param)						\
		__drv_innerAcquiresGlobal(kind, param)
	//
	// Flag that the annotated object acquires a global (otherwise anonymous)
	// resource of type kind named by param.
	//
	__ANNOTATION(SAL_releaseGlobal(__in char *, ...);)
	#define __drv_innerReleasesGlobal(kind, param)							\
		__post __drv_declspec("SAL_releaseGlobal(\"" #kind "\","			\
												 SPECSTRINGIZE(param\t)")")
	#define __drv_releasesResourceGlobal(kind, param)						\
		__drv_innerReleasesGlobal(kind, param)
											  
	//
	// Flag that the annotated parameter must hold a resource of type kind
	//
	__ANNOTATION(SAL_mustHold(__in char *);)
	#define __drv_mustHold(kind)											\
		__pre __drv_declspec("SAL_mustHold(\""#kind"\")")

	//
	// Flag that the annotated object must hold a global resource
	// of type kind named by param.
	//
	__ANNOTATION(SAL_mustHoldGlobal(__in char *, ...);)
	#define __drv_innerMustHoldGlobal(kind, param)							\
		__pre __drv_declspec("SAL_mustHoldGlobal(\"" #kind "\","			\
												 SPECSTRINGIZE(param\t)")")
	#define __drv_mustHoldGlobal(kind,param)								\
		__drv_innerMustHoldGlobal(kind, param)

	//
	// Flag that the annotated parameter must never hold a resource of type kind
	//
	__ANNOTATION(SAL_neverHold(__in char *);)
	#define __drv_neverHold(kind)											\
		__pre __drv_declspec("SAL_neverHold(\"" #kind "\")")

	//
	// Flag that the annotated object must never hold a global resource
	// of type kind named by param.
	//
	__ANNOTATION(SAL_neverHoldGlobal(__in char *, ...);)
	#define __drv_innerNeverHoldGlobal(kind, param)							\
		__pre __drv_declspec("SAL_neverHoldGlobal(\"" #kind "\","			\
												 SPECSTRINGIZE(param\t)")")
	#define __drv_neverHoldGlobal(kind,param)								\
		__drv_innerNeverHoldGlobal(kind, param)

	// Predicates to determine if a resource is held
	__PRIMOP(int, holdsResource$(__in __deferTypecheck char *,__in char *);)
	__PRIMOP(int, holdsResourceGlobal$(__in char *, ...);)

	// ---------------------------------------------------------------------
	// Maintenance of IRQL values
	//
	// __drv_setsIRQL(irql)
	// __drv_raisesIRQL(irql)
	// __drv_requiresIRQL(irql)
	// __drv_maxIRQL(irql)
	// __drv_minIRQL(irql)
	// __drv_savesIRQL
	// __drv_restoresIRQL
	// __drv_savesIRQLGlobal(kind,param)
	// __drv_restoresIRQLGlobal(kind,param)
	// __drv_minFunctionIRQL(irql)
	// __drv_maxFunctionIRQL(irql)
	// __drv_useCancelIRQL
	// __drv_sameIRQL

	// 
	// The funciton exits at IRQL irql
	//
	#define __drv_setsIRQL(irql)  /* see kernelspecs.h */

	// 
	// The funciton exits at IRQL irql, but this may only raise the irql.
	//
	#define __drv_raisesIRQL(irql)  /* see kernelspecs.h */

	// 
	// The called function must be entered at IRQL level
	//
	#define __drv_requiresIRQL(irql)  /* see kernelspecs.h */


	// 
	// The maximum IRQL at which the function may be called.
	//
	#define __drv_maxIRQL(irql)  /* see kernelspecs.h */

	// 
	// The minimum IRQL at which the function may be called.
	//
	#define __drv_minIRQL(irql)  /* see kernelspecs.h */

	// 
	// The current IRQL is saved in the annotated parameter
	//
	#define __drv_savesIRQL  /* see kernelspecs.h */

	// 
	// The current IRQL is saved in the (otherwise anonymous) global object
	// identified by kind and further refined by param.
	//
	#define __drv_savesIRQLGlobal(kind,param)  /* see kernelspecs.h */

	// 
	// The current IRQL is restored from the annotated parameter
	//
	#define __drv_restoresIRQL  /* see kernelspecs.h */

	// 
	// The current IRQL is restored from the (otherwise anonymous) global object
	// identified by kind and further refined by param.
	//
	#define __drv_restoresIRQLGlobal(kind,param)  /* see kernelspecs.h */

	// The minimum IRQL to which the function can lower itself.	 The IRQL
	// at entry is assumed to be that value unless overridden.
	#define __drv_minFunctionIRQL(irql)  /* see kernelspecs.h */

	// The maximum IRQL to which the function can raise itself.
	#define __drv_maxFunctionIRQL(irql)  /* see kernelspecs.h */

	// The function must exit with the same IRQL it was entered with.
	// (It may change it but it must restore it.)
	#define __drv_sameIRQL  /* see kernelspecs.h */

	// The annotated parameter contains the cancelIRQL, which will be restored
	// by the called function.

	#define __drv_useCancelIRQL  /* see kernelspecs.h */

	// ---------------------------------------------------------------------
	// Specific function behaviors

    // The annotated function clears the requirement that DoInitializeing
	// is cleared (or not).
	__ANNOTATION(SAL_clearDoInit(enum __SAL_YesNo);)
	#define __drv_clearDoInit(yesNo)										\
		__post __drv_declspec("SAL_clearDoInit(" SPECSTRINGIZE(yesNo) ")") 

	// This is (or is like) IoGetDmaAdapter: look for misuse of DMA pointers
	__ANNOTATION(SAL_IoGetDmaAdapter(void);)
	#define __drv_IoGetDmaAdapter										\
		__post __drv_declspec("SAL_IoGetDmaAdapter") 

	// ---------------------------------------------------------------------
	// Function and out parameter return values.
	//
	// __drv_valueIs(<list>)
	//
	// The function being annotated will return each of the specified values
	// during simulation.  The items in the list are <relational op><constant>,
	// e.g. ==0 or <0.
	// This is a ; separated list of values.  The internal parser will accept
	// a comma-separated list.  In the future __VA_ARGS__ could be used.
	// See the documentation for use of this.
	//

	__ANNOTATION(SAL_return(__in __AuToQuOtE char *);)
	#define __drv_valueIs(arglist)											\
			__post __drv_declspec("SAL_return("SPECSTRINGIZE(arglist)")")

	// ---------------------------------------------------------------------
	// Additional parameter checking.
	//
	// __drv_constant
	// __drv_nonConstant
	// __drv_strictTypeMatch(mode)
	// __drv_strictType(type,mode)
	//
	// The actual parameter must evaluate to a constant (not a const).
	//
	__ANNOTATION(SAL_constant(enum __SAL_YesNo);)
	#define __drv_constant __pre __drv_declspec("SAL_constant(__yes)")

	//
	// The actual parameter may never evaluate to a numeric constant 
	// (exclusive of a const symbol).
	//
	#define __drv_nonConstant __pre __drv_declspec("SAL_constant(__no)")

	//
	// The actual parameter must match the type of the annotated formal
	// within the specifications set by mode.
	//
	__ANNOTATION(SAL_strictTypeMatch(__int64);)
	#define __drv_strictTypeMatch(mode)										\
		__pre __drv_declspec("SAL_strictTypeMatch("SPECSTRINGIZE(mode)")")

	//
	// The actual parameter must match the type of typename (below) 
	// within the specifications set by mode.
	//
	__ANNOTATION(SAL_strictType(__in __AuToQuOtE char *);) // currently 1/2 args
	#define __drv_strictType(typename,mode)									\
		__pre __drv_declspec("SAL_strictType("SPECSTRINGIZE(typename)","\
											  SPECSTRINGIZE(mode)")")
	//
	//    The following modes are defined:
		#define __drv_typeConst   0    // constants of that type
		#define __drv_typeCond    1    // plus ?:
		#define __drv_typeBitset  2    // plus all operators
		#define __drv_typeExpr    3    // plus literal constants
	// 
	// The actual parameter must be data (not a pointer).  Used to
	// prevent passing pointers to pointers when pointers to structures
	// are needed (because &pXXX is a common error when pXXX is 
	// intended).
	__ANNOTATION(SAL_mayBePointer(enum __SAL_YesNo);)
	#define __drv_notPointer  __pre __drv_declspec("SAL_mayBePointer(__no)")
	//
	// Convenience for the most common form of the above.
	#define __drv_isObjectPointer __drv_deref(__drv_notPointer)

	// ---------------------------------------------------------------------
	// Memory management
	//
	// __drv_aliasesMem
	// __drv_allocatesMem
	// __drv_freesMem
	//
	// The annotated parameter is "kept" by the function, creating an
	// alias, and relieving any obligation to free the object.
	//
	__ANNOTATION(SAL_IsAliased(void);)
	#define __drv_aliasesMem __post __drv_declspec("SAL_IsAliased")

	//
	// Allocate/release memory-like objects.
	// Kind is unused, but should be "mem" for malloc/free
	// and "object" for new/delete.
	__ANNOTATION(SAL_NeedsRelease(enum __SAL_YesNo);)
	#define __drv_allocatesMem(kind) __post __drv_declspec("SAL_NeedsRelease(__yes)")

	#define __drv_freesMem(kind)	 __post __drv_declspec("SAL_NeedsRelease(__no)")

	// ---------------------------------------------------------------------
	// Additional diagnostics
	//
	// __drv_preferredFunction
	// __drv_reportError
	//
	//
	// Function 'func' should be used for reason 'why'.	 Often used
	// conditionally.
	//
	__ANNOTATION(SAL_preferredFunction(__in __AuToQuOtE char *, __in __AuToQuOtE char *);)
	#define __drv_preferredFunction(func,why)								\
		__pre __drv_declspec(												\
			"SAL_preferredFunction(" SPECSTRINGIZE(func) ","				\
											 SPECSTRINGIZE(why) ")")

	//
	// The error given by 'why' was detected.  Used conditionally.
	//
	__ANNOTATION(SAL_error(__in __AuToQuOtE char *);)
	#define __drv_reportError(why)											\
		//__pre __drv_declspec("SAL_error(" SPECSTRINGIZE(why) ")")

	// ---------------------------------------------------------------------
	// Floating point save/restore:
	//
	// __drv_floatSaved
	// __drv_floatRestored
	// __drv_floatUsed
	//
	// The floating point hardware was saved (available to kernel)
	__ANNOTATION(SAL_floatSaved(void);)
	#define __drv_floatSaved __post __drv_declspec("SAL_floatSaved")

	//
	// The floating point hardware was restored (no longer available)
	__ANNOTATION(SAL_floatRestored(void);)
	#define __drv_floatRestored __post __drv_declspec("SAL_floatRestored")

	//
	// The function uses floating point.  Functions with floating point
	// in their type signature get this automatically.
	__ANNOTATION(SAL_floatUsed(void);)
	#define __drv_floatUsed __post __drv_declspec("SAL_floatUsed")

	// ---------------------------------------------------------------------
	// Usage:
	// 
	// __drv_interlocked
	// __drv_inTry
	// __drv_notInTry
	//
	// The parameter is used for interlocked instructions.
	__ANNOTATION(SAL_interlocked(void);)
	#define __drv_interlocked __pre __drv_declspec("SAL_interlocked")

	// The function must be called inside a try block
	__ANNOTATION(SAL_inTry(enum __SAL_YesNo);)
	#define __drv_inTry __pre __drv_declspec("SAL_inTry(__yes)")

	// The function must not be called inside a try block
	#define __drv_notInTry __pre __drv_declspec("SAL_inTry(__no)")

	// ---------------------------------------------------------------------
	// FormatString:
	//
	// kind can be "printf", "scanf", "strftime" or "FormatMessage".
	__ANNOTATION(SAL_IsFormatString(__in char *);)
	#define __drv_formatString(kind)\
		__drv_declspec("SAL_IsFormatString(\"" #kind "\")")
	
	// ---------------------------------------------------------------------
	// SDV support: see the SDV documentation for details

	// Identify dispatch callback types
	__ANNOTATION(SAL_dispatchType(__in __int64);)
	#define __drv_dispatchType(kindlist)\
		__pre __drv_declspec("SAL_dispatchType("\
							SPECSTRINGIZE(kindlist) ")" )

	// Identify dispatch callback types - special case
	#define __drv_dispatchType_other\
		__drv_dispatchType(-1)

	// Identify completion callback types
		__ANNOTATION(SAL_completionType(__in __AuToQuOtE char *);)
	#define __drv_completionType(kindlist)\
		__drv_declspec("SAL_completionType("\
							#kindlist ")" )

	__ANNOTATION(SAL_callbackType(__in __AuToQuOtE char *);)
	// Identify callback types (FDO or PDO)
	#define __drv_callbackType(kind)\
		__drv_declspec("SAL_callbackType("\
							#kind ")" )
	// ---------------------------------------------------------------------
	// Composite:

#ifdef _PREFAST_  // [ expand to nothing immediately to avoid RC problem
	//
	// Exclusive Resources
	#define __drv_acquiresExclusiveResource(kind)				\
		__$drv_group(											\
		__drv_neverHold(kind)									\
		__drv_acquiresResource(kind))

	#define __drv_releasesExclusiveResource(kind)				\
		__$drv_group(											\
		__drv_mustHold(kind)									\
		__drv_releasesResource(kind))

	#define __drv_acquiresExclusiveResourceGlobal(kind, param)	\
		__drv_neverHoldGlobal(kind, param)						\
		__drv_acquiresResourceGlobal(kind, param)

	#define __drv_releasesExclusiveResourceGlobal(kind, param)	\
		__drv_mustHoldGlobal(kind, param)						\
		__drv_releasesResourceGlobal(kind, param)

	//
	// CancelSpinLock
	#define __drv_acquiresCancelSpinLock						\
		__drv_innerNeverHoldGlobal(CancelSpinLock,)				\
		__drv_innerAcquiresGlobal(CancelSpinLock,)

	#define __drv_releasesCancelSpinLock						\
		__drv_innerMustHoldGlobal(CancelSpinLock,)				\
		__drv_innerReleasesGlobal(CancelSpinLock,)

	#define __drv_mustHoldCancelSpinLock						\
		__drv_innerMustHoldGlobal(CancelSpinLock,)

	#define __drv_neverHoldCancelSpinLock						\
		__drv_innerNeverHoldGlobal(CancelSpinLock,)

	#define __drv_holdsCancelSpinLock()							\
		holdsResourceGlobal$("CancelSpinLock",)

	//
	// CriticalRegion
	#define __drv_acquiresCriticalRegion						\
		__drv_innerNeverHoldGlobal(CriticalRegion,)				\
		__drv_innerAcquiresGlobal(CriticalRegion,)

	#define __drv_releasesCriticalRegion						\
		__drv_innerMustHoldGlobal(CriticalRegion,)				\
		__drv_innerReleasesGlobal(CriticalRegion,)

	#define __drv_mustHoldCriticalRegion						\
		__drv_innerMustHoldGlobal(CriticalRegion,)

	#define __drv_neverHoldCriticalRegion						\
		__drv_innerNeverHoldGlobal(CriticalRegion,)

	#define __drv_holdsCriticalRegion()							\
		holdsResourceGlobal$("CriticalRegion",)


    //
    // PriorityRegion
    #define __drv_acquiresPriorityRegion                        \
        __drv_innerNeverHoldGlobal(PriorityRegion,)             \
        __drv_innerAcquiresGlobal(PriorityRegion,)

    #define __drv_releasesPriorityRegion                        \
        __drv_innerMustHoldGlobal(PriorityRegion,)              \
        __drv_innerReleasesGlobal(PriorityRegion,)

    #define __drv_mustHoldPriorityRegion                        \
        __drv_innerMustHoldGlobal(PriorityRegion,)

    #define __drv_neverHoldPriorityRegion                       \
        __drv_innerNeverHoldGlobal(PriorityRegion,)

	#define __drv_holdsPriorityRegion()							\
		holdsResourceGlobal$("PriorityRegion",)

#else // ][

	#define __drv_acquiresExclusiveResource(kind)
	#define __drv_releasesExclusiveResource(kind)
	#define __drv_acquiresExclusiveResourceGlobal(kind, param)
	#define __drv_releasesExclusiveResourceGlobal(kind, param)
	#define __drv_acquiresCancelSpinLock
	#define __drv_releasesCancelSpinLock
	#define __drv_mustHoldCancelSpinLock
	#define __drv_holdsCancelSpinLock()
	#define __drv_neverHoldCancelSpinLock
	#define __drv_acquiresCriticalRegion
	#define __drv_releasesCriticalRegion
	#define __drv_mustHoldCriticalRegion
	#define __drv_neverHoldCriticalRegion
	#define __drv_holdsCriticalRegion()
	#define __drv_acquiresPriorityRegion
	#define __drv_releasesPriorityRegion
	#define __drv_mustHoldPriorityRegion
	#define __drv_neverHoldPriorityRegion
	#define __drv_holdsPriorityRegion()

#endif // ]

	// Passing the cancel Irql to a utility function
	#define __drv_isCancelIRQL  /* see kernelspecs.h */

    __PRIMOP(int, inFunctionClass$(__in char *);)

	// Check if this is kernel or driver code
	__PRIMOP(int, isKernel$(void);)
	__PRIMOP(int, isDriver$(void);)

#ifdef	__cplusplus
}
#endif

#endif // DRIVERSPECS_H

