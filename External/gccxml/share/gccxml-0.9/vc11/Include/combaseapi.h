/*#!perl
MapHeaderToDll("combaseapi.h", "combaseapi.dll");
ActivateAroundFunctionCall("combaseapi.dll");
IgnoreFunction("CoGetCurrentProcess"); # never fails => hard to wrap well
IgnoreFunction("CoAddRefServerProcess"); # never fails => hard to wrap well
IgnoreFunction("CoReleaseServerProcess"); # never fails => hard to wrap well
IgnoreFunction("wIsEqualGUID");
DeclareFunctionErrorValue("StringFromGUID2" , "0");
DeclareFunctionErrorValue("CoTaskMemAlloc", "NULL");
DeclareFunctionErrorValue("CoTaskMemRealloc", "NULL");
IgnoreFunction("DllGetClassObject"); # client function prototyped (like WinMain)
IgnoreFunction("DllCanUnloadNow"); # client function prototyped (like WinMain)
*/

//+---------------------------------------------------------------------------
//
//  Microsoft Windows
//  Copyright (c) Microsoft Corporation. All rights reserved.
//
//  File:       combaseapi.h
//
//  Contents:   Base Component Object Model defintions.
//
//----------------------------------------------------------------------------

#ifdef _CONTRACT_GEN
#include <nt.h>
#include <ntrtl.h>
#include <nturtl.h>
#endif
#include <apiset.h>
#include <apisetcconv.h>

/* APISET_NAME: api-ms-win-core-com-l1 */

#ifndef _APISET_COM_VER
#ifdef _APISET_MINCORE_VERSION
#if _APISET_MINCORE_VERSION >= 0x0100
#define _APISET_COM_VER 0x0100
#endif // _APISET_MINCORE_VERSION >= 0x0100
#endif // _APISET_MINCORE_VERSION
#endif // _APISET_COM_VER

#include <rpc.h>
#include <rpcndr.h>


#if (NTDDI_VERSION >= NTDDI_VISTA && !defined(_WIN32_WINNT))
#define _WIN32_WINNT 0x0600
#endif


#if (NTDDI_VERSION >= NTDDI_WS03 && !defined(_WIN32_WINNT))
#define _WIN32_WINNT 0x0502
#endif


#if (NTDDI_VERSION >= NTDDI_WINXP && !defined(_WIN32_WINNT))
#define _WIN32_WINNT 0x0501
#endif


#if (NTDDI_VERSION >= NTDDI_WIN2K && !defined(_WIN32_WINNT))
#define _WIN32_WINNT 0x0500
#endif



#if !defined(_COMBASEAPI_H_)
#define _COMBASEAPI_H_

#ifdef _MSC_VER
#pragma once
#endif  // _MSC_VER

#include <pshpack8.h>

//TODO change _OLE32_ to _COMBASEAPI_
#ifdef _OLE32_
#define WINOLEAPI        STDAPI
#define WINOLEAPI_(type) STDAPI_(type)
#else

#ifdef _68K_
#ifndef REQUIRESAPPLEPASCAL
#define WINOLEAPI        EXTERN_C DECLSPEC_IMPORT HRESULT PASCAL
#define WINOLEAPI_(type) EXTERN_C DECLSPEC_IMPORT type PASCAL
#else
#define WINOLEAPI        EXTERN_C DECLSPEC_IMPORT PASCAL HRESULT
#define WINOLEAPI_(type) EXTERN_C DECLSPEC_IMPORT PASCAL type
#endif
#else
#define WINOLEAPI        EXTERN_C DECLSPEC_IMPORT HRESULT STDAPICALLTYPE
#define WINOLEAPI_(type) EXTERN_C DECLSPEC_IMPORT type STDAPICALLTYPE
#endif

#endif

/****** Interface Declaration ***********************************************/

/*
 *      These are macros for declaring interfaces.  They exist so that
 *      a single definition of the interface is simulataneously a proper
 *      declaration of the interface structures (C++ abstract classes)
 *      for both C and C++.
 *
 *      DECLARE_INTERFACE(iface) is used to declare an interface that does
 *      not derive from a base interface.
 *      DECLARE_INTERFACE_(iface, baseiface) is used to declare an interface
 *      that does derive from a base interface.
 *
 *      By default if the source file has a .c extension the C version of
 *      the interface declaratations will be expanded; if it has a .cpp
 *      extension the C++ version will be expanded. if you want to force
 *      the C version expansion even though the source file has a .cpp
 *      extension, then define the macro "CINTERFACE".
 *      eg.     cl -DCINTERFACE file.cpp
 *
 *      Example Interface declaration:
 *
 *          #undef  INTERFACE
 *          #define INTERFACE   IClassFactory
 *
 *          DECLARE_INTERFACE_(IClassFactory, IUnknown)
 *          {
 *              // *** IUnknown methods ***
 *              STDMETHOD(QueryInterface) (THIS_
 *                                        REFIID riid,
 *                                        LPVOID FAR* ppvObj) PURE;
 *              STDMETHOD_(ULONG,AddRef) (THIS) PURE;
 *              STDMETHOD_(ULONG,Release) (THIS) PURE;
 *
 *              // *** IClassFactory methods ***
 *              STDMETHOD(CreateInstance) (THIS_
 *                                        LPUNKNOWN pUnkOuter,
 *                                        REFIID riid,
 *                                        LPVOID FAR* ppvObject) PURE;
 *          };
 *
 *      Example C++ expansion:
 *
 *          struct FAR IClassFactory : public IUnknown
 *          {
 *              virtual HRESULT STDMETHODCALLTYPE QueryInterface(
 *                                                  IID FAR& riid,
 *                                                  LPVOID FAR* ppvObj) = 0;
 *              virtual HRESULT STDMETHODCALLTYPE AddRef(void) = 0;
 *              virtual HRESULT STDMETHODCALLTYPE Release(void) = 0;
 *              virtual HRESULT STDMETHODCALLTYPE CreateInstance(
 *                                              LPUNKNOWN pUnkOuter,
 *                                              IID FAR& riid,
 *                                              LPVOID FAR* ppvObject) = 0;
 *          };
 *
 *          NOTE: Our documentation says '#define interface class' but we use
 *          'struct' instead of 'class' to keep a lot of 'public:' lines
 *          out of the interfaces.  The 'FAR' forces the 'this' pointers to
 *          be far, which is what we need.
 *
 *      Example C expansion:
 *
 *          typedef struct IClassFactory
 *          {
 *              const struct IClassFactoryVtbl FAR* lpVtbl;
 *          } IClassFactory;
 *
 *          typedef struct IClassFactoryVtbl IClassFactoryVtbl;
 *
 *          struct IClassFactoryVtbl
 *          {
 *              HRESULT (STDMETHODCALLTYPE * QueryInterface) (
 *                                                  IClassFactory FAR* This,
 *                                                  IID FAR* riid,
 *                                                  LPVOID FAR* ppvObj) ;
 *              HRESULT (STDMETHODCALLTYPE * AddRef) (IClassFactory FAR* This) ;
 *              HRESULT (STDMETHODCALLTYPE * Release) (IClassFactory FAR* This) ;
 *              HRESULT (STDMETHODCALLTYPE * CreateInstance) (
 *                                                  IClassFactory FAR* This,
 *                                                  LPUNKNOWN pUnkOuter,
 *                                                  IID FAR* riid,
 *                                                  LPVOID FAR* ppvObject);
 *              HRESULT (STDMETHODCALLTYPE * LockServer) (
 *                                                  IClassFactory FAR* This,
 *                                                  BOOL fLock);
 *          };
 */


#if defined(__cplusplus) && !defined(CINTERFACE)
//#define interface               struct FAR

#ifdef COM_STDMETHOD_CAN_THROW
#define COM_DECLSPEC_NOTHROW
#else
#define COM_DECLSPEC_NOTHROW DECLSPEC_NOTHROW
#endif

#define __STRUCT__ struct
#define interface __STRUCT__
#define STDMETHOD(method)        virtual COM_DECLSPEC_NOTHROW HRESULT STDMETHODCALLTYPE method
#define STDMETHOD_(type,method)  virtual COM_DECLSPEC_NOTHROW type STDMETHODCALLTYPE method
#define STDMETHODV(method)       virtual COM_DECLSPEC_NOTHROW HRESULT STDMETHODVCALLTYPE method
#define STDMETHODV_(type,method) virtual COM_DECLSPEC_NOTHROW type STDMETHODVCALLTYPE method
#define PURE                    = 0
#define THIS_
#define THIS                    void
#define DECLARE_INTERFACE(iface)                        interface DECLSPEC_NOVTABLE iface
#define DECLARE_INTERFACE_(iface, baseiface)            interface DECLSPEC_NOVTABLE iface : public baseiface
#define DECLARE_INTERFACE_IID(iface, iid)               interface DECLSPEC_UUID(iid) DECLSPEC_NOVTABLE iface
#define DECLARE_INTERFACE_IID_(iface, baseiface, iid)   interface DECLSPEC_UUID(iid) DECLSPEC_NOVTABLE iface : public baseiface

#define IFACEMETHOD(method)         __override STDMETHOD(method)
#define IFACEMETHOD_(type,method)   __override STDMETHOD_(type,method)
#define IFACEMETHODV(method)        __override STDMETHODV(method)
#define IFACEMETHODV_(type,method)  __override STDMETHODV_(type,method)


#if !defined(BEGIN_INTERFACE)

#if defined(_MPPC_) && ((defined(_MSC_VER) || defined(__SC__) || defined(__MWERKS__)) && !defined(NO_NULL_VTABLE_ENTRY))
   #define BEGIN_INTERFACE virtual void a() {}
   #define END_INTERFACE
#else
   #define BEGIN_INTERFACE
   #define END_INTERFACE
#endif
#endif

//  IID_PPV_ARGS(ppType)
//      ppType is the variable of type IType that will be filled
//
//      RESULTS in:  IID_IType, ppvType
//      will create a compiler error if wrong level of indirection is used.
//
extern "C++"
{
    template<typename T> void** IID_PPV_ARGS_Helper(T** pp) 
    {
        //static_cast<IUnknown*>(*pp);    // make sure everyone derives from IUnknown
        return reinterpret_cast<void**>(pp);
    }
}

#define IID_PPV_ARGS(ppType) __uuidof(**(ppType)), IID_PPV_ARGS_Helper(ppType)

#else

#define interface               struct

#define STDMETHOD(method)       HRESULT (STDMETHODCALLTYPE * method)
#define STDMETHOD_(type,method) type (STDMETHODCALLTYPE * method)
#define STDMETHODV(method)       HRESULT (STDMETHODVCALLTYPE * method)
#define STDMETHODV_(type,method) type (STDMETHODVCALLTYPE * method)

#define IFACEMETHOD(method)         __override STDMETHOD(method)
#define IFACEMETHOD_(type,method)   __override STDMETHOD_(type,method)
#define IFACEMETHODV(method)        __override STDMETHODV(method)
#define IFACEMETHODV_(type,method)  __override STDMETHODV_(type,method)


#if !defined(BEGIN_INTERFACE)

#if defined(_MPPC_)
    #define BEGIN_INTERFACE       void    *b;
    #define END_INTERFACE
#else
    #define BEGIN_INTERFACE
    #define END_INTERFACE
#endif
#endif


#define PURE
#define THIS_                   INTERFACE FAR* This,
#define THIS                    INTERFACE FAR* This
#ifdef CONST_VTABLE
#undef CONST_VTBL
#define CONST_VTBL const
#define DECLARE_INTERFACE(iface)    typedef interface iface { \
                                    const struct iface##Vtbl FAR* lpVtbl; \
                                } iface; \
                                typedef const struct iface##Vtbl iface##Vtbl; \
                                const struct iface##Vtbl
#else
#undef CONST_VTBL
#define CONST_VTBL
#define DECLARE_INTERFACE(iface)    typedef interface iface { \
                                    struct iface##Vtbl FAR* lpVtbl; \
                                } iface; \
                                typedef struct iface##Vtbl iface##Vtbl; \
                                struct iface##Vtbl
#endif
#define DECLARE_INTERFACE_(iface, baseiface)    DECLARE_INTERFACE(iface)
#define DECLARE_INTERFACE_IID(iface, iid)               DECLARE_INTERFACE(iface)
#define DECLARE_INTERFACE_IID_(iface, baseiface, iid)   DECLARE_INTERFACE_(iface, baseiface)
#endif




/****** Additional basic types **********************************************/


#ifndef FARSTRUCT
#ifdef __cplusplus
#define FARSTRUCT   FAR
#else
#define FARSTRUCT
#endif  // __cplusplus
#endif  // FARSTRUCT



#ifndef HUGEP

#if defined(_WIN32) || defined(_MPPC_)
#define HUGEP
#else
#define HUGEP __huge
#endif // WIN32
#endif // HUGEP

#include <stdlib.h>

#define LISet32(li, v) ((li).HighPart = ((LONG) (v)) < 0 ? -1 : 0, (li).LowPart = (v))

#define ULISet32(li, v) ((li).HighPart = 0, (li).LowPart = (v))






#define CLSCTX_INPROC           (CLSCTX_INPROC_SERVER|CLSCTX_INPROC_HANDLER)

// With DCOM, CLSCTX_REMOTE_SERVER should be included
// DCOM
#if (_WIN32_WINNT >= 0x0400) || defined(_WIN32_DCOM)
#define CLSCTX_ALL              (CLSCTX_INPROC_SERVER| \
                                 CLSCTX_INPROC_HANDLER| \
                                 CLSCTX_LOCAL_SERVER| \
                                 CLSCTX_REMOTE_SERVER)

#define CLSCTX_SERVER           (CLSCTX_INPROC_SERVER|CLSCTX_LOCAL_SERVER|CLSCTX_REMOTE_SERVER)
#else
#define CLSCTX_ALL              (CLSCTX_INPROC_SERVER| \
                                 CLSCTX_INPROC_HANDLER| \
                                 CLSCTX_LOCAL_SERVER )

#define CLSCTX_SERVER           (CLSCTX_INPROC_SERVER|CLSCTX_LOCAL_SERVER)
#endif


// class registration flags; passed to CoRegisterClassObject
typedef enum tagREGCLS
{
    REGCLS_SINGLEUSE = 0,       // class object only generates one instance
    REGCLS_MULTIPLEUSE = 1,     // same class object genereates multiple inst.
                                // and local automatically goes into inproc tbl.
    REGCLS_MULTI_SEPARATE = 2,  // multiple use, but separate control over each
                                // context.
    REGCLS_SUSPENDED      = 4,  // register is as suspended, will be activated
                                // when app calls CoResumeClassObjects
    REGCLS_SURROGATE      = 8   // must be used when a surrogate process
                                // is registering a class object that will be
                                // loaded in the surrogate
} REGCLS;

 
/* here is where we pull in the MIDL generated headers for the interfaces */
typedef interface    IRpcStubBuffer     IRpcStubBuffer;
typedef interface    IRpcChannelBuffer  IRpcChannelBuffer;

// COM initialization flags; passed to CoInitialize.
typedef enum tagCOINITBASE
{
// DCOM
#if (_WIN32_WINNT >= 0x0400) || defined(_WIN32_DCOM)
  // These constants are only valid on Windows NT 4.0
  COINITBASE_MULTITHREADED      = 0x0,      // OLE calls objects on any thread.
#endif // DCOM
} COINITBASE;

#include <wtypesbase.h>
#include <unknwnbase.h>
#include <objidlbase.h>

#include <guiddef.h>

#ifndef INITGUID
// TODO change to cguidbase.h
#include <cguid.h>
#endif


/****** STD Object API Prototypes *****************************************/

_Check_return_ WINOLEAPI
CoGetMalloc(
    _In_ DWORD dwMemContext,
    _Outptr_ LPMALLOC FAR * ppMalloc
    );


_Check_return_ WINOLEAPI
CreateStreamOnHGlobal(
    HGLOBAL hGlobal,
    _In_ BOOL fDeleteOnRelease,
    _Outptr_ LPSTREAM FAR * ppstm
    );

                    
_Check_return_ WINOLEAPI
GetHGlobalFromStream(
    _In_ LPSTREAM pstm,
    _Out_ HGLOBAL FAR * phglobal
    );


/* init/uninit */

WINOLEAPI_(void)
CoUninitialize(
    void
    );

WINOLEAPI_(DWORD)
CoGetCurrentProcess(
    void
    );


// DCOM
#if (_WIN32_WINNT >= 0x0400) || defined(_WIN32_DCOM)
/* #!perl PoundIf("CoInitializeEx", "(_WIN32_WINNT >= 0x0400 ) || defined(_WIN32_DCOM)");
*/
_Check_return_ WINOLEAPI
CoInitializeEx(
    _In_opt_ LPVOID pvReserved,
    _In_ DWORD dwCoInit
    );


/* #!perl PoundIf("CoGetCallerTID", "(_WIN32_WINNT >= 0x0400 ) || defined(_WIN32_DCOM)");
*/
WINOLEAPI
CoGetCallerTID(
    _Out_ LPDWORD lpdwTID
    );


 
/* #!perl PoundIf("CoGetCurrentLogicalThreadId", "(_WIN32_WINNT >= 0x0400 ) || defined(_WIN32_DCOM)");
*/
WINOLEAPI
CoGetCurrentLogicalThreadId(
    _Out_ GUID * pguid
    );

#endif // DCOM


#if (_WIN32_WINNT >= 0x0501)
 
_Check_return_ WINOLEAPI
CoGetContextToken(
    _Out_ ULONG_PTR * pToken
    );


_Check_return_ WINOLEAPI
CoGetDefaultContext(
    _In_ APTTYPE aptType,
    _In_ REFIID riid,
    _Outptr_ void ** ppv
    );

#endif

// definition for Win7 new APIs


#if (NTDDI_VERSION >= NTDDI_WIN7)

_Check_return_ WINOLEAPI
CoGetApartmentType(
    _Out_ APTTYPE * pAptType,
    _Out_ APTTYPEQUALIFIER * pAptQualifier
    );


#endif

// definition for Win8 new APIs


#if (NTDDI_VERSION >= NTDDI_WIN8)

DECLARE_HANDLE(CO_MTA_USAGE_COOKIE);

_Check_return_ WINOLEAPI
CoIncrementMTAUsage(
    _Out_ CO_MTA_USAGE_COOKIE * pCookie
    );

               WINOLEAPI
CoDecrementMTAUsage(
    _In_ CO_MTA_USAGE_COOKIE Cookie
    );



#if (!defined(_KRPCENV_))

_Check_return_ WINOLEAPI
CoCreateMTAWorker(
    _In_ LPTHREAD_START_ROUTINE Function,
    _In_ PVOID Context,
    _In_ DWORD dwFlags
    );


#endif
               WINOLEAPI
CoWaitMTACompletion(
    VOID
    );


WINOLEAPI_(void)
CoEnterApplicationThreadLifetimeLoop(
    );


WINOLEAPI
CoGetApplicationThreadReference(
    _Outptr_ IUnknown ** ppThreadReference
    );


WINOLEAPI
CoWaitForMultipleObjects(
    _In_ DWORD dwFlags,
    _In_ DWORD dwTimeout,
    _In_ ULONG cHandles,
    _In_reads_(cHandles) LPHANDLE pHandles,
    _Out_ LPDWORD lpdwindex
    );


#endif 
 
_Check_return_ WINOLEAPI
CoGetObjectContext(
    _In_ REFIID riid,
    _Outptr_ LPVOID FAR * ppv
    );


/* register/revoke/get class objects */

#pragma region Desktop Family

#if ((WINAPI_FAMILY & WINAPI_PARTITION_DESKTOP) == WINAPI_PARTITION_DESKTOP)
_Check_return_ WINOLEAPI
CoGetClassObject(
    _In_ REFCLSID rclsid,
    _In_ DWORD dwClsContext,
    _In_opt_ LPVOID pvReserved,
    _In_ REFIID riid,
    _Outptr_ LPVOID FAR * ppv
    );

#endif
#pragma endregion

_Check_return_ WINOLEAPI
CoRegisterClassObject(
    _In_ REFCLSID rclsid,
    _In_ LPUNKNOWN pUnk,
    _In_ DWORD dwClsContext,
    _In_ DWORD flags,
    _Out_ LPDWORD lpdwRegister
    );

_Check_return_ WINOLEAPI
CoRevokeClassObject(
    _In_ DWORD dwRegister
    );

_Check_return_ WINOLEAPI
CoResumeClassObjects(
    void
    );

_Check_return_ WINOLEAPI
CoSuspendClassObjects(
    void
    );

WINOLEAPI_(ULONG)
CoAddRefServerProcess(
    void
    );

WINOLEAPI_(ULONG)
CoReleaseServerProcess(
    void
    );

_Check_return_ WINOLEAPI
CoGetPSClsid(
    _In_ REFIID riid,
    _Out_ CLSID * pClsid
    );

_Check_return_ WINOLEAPI
CoRegisterPSClsid(
    _In_ REFIID riid,
    _In_ REFCLSID rclsid
    );


// Registering surrogate processes
_Check_return_ WINOLEAPI
CoRegisterSurrogate(
    _In_ LPSURROGATE pSurrogate
    );


/* marshaling interface pointers */

_Check_return_ WINOLEAPI
CoGetMarshalSizeMax(
    _Out_ ULONG * pulSize,
    _In_ REFIID riid,
    _In_ LPUNKNOWN pUnk,
    _In_ DWORD dwDestContext,
    _In_opt_ LPVOID pvDestContext,
    _In_ DWORD mshlflags
    );

_Check_return_ WINOLEAPI
CoMarshalInterface(
    _In_ LPSTREAM pStm,
    _In_ REFIID riid,
    _In_ LPUNKNOWN pUnk,
    _In_ DWORD dwDestContext,
    _In_opt_ LPVOID pvDestContext,
    _In_ DWORD mshlflags
    );

_Check_return_ WINOLEAPI
CoUnmarshalInterface(
    _In_ LPSTREAM pStm,
    _In_ REFIID riid,
    _Outptr_ LPVOID FAR * ppv
    );

WINOLEAPI
CoMarshalHresult(
    _In_ LPSTREAM pstm,
    _In_ HRESULT hresult
    );

WINOLEAPI
CoUnmarshalHresult(
    _In_ LPSTREAM pstm,
    _Out_ HRESULT FAR * phresult
    );

_Check_return_ WINOLEAPI
CoReleaseMarshalData(
    _In_ LPSTREAM pStm
    );

_Check_return_ WINOLEAPI
CoDisconnectObject(
    _In_ LPUNKNOWN pUnk,
    _In_ DWORD dwReserved
    );

_Check_return_ WINOLEAPI
CoLockObjectExternal(
    _In_ LPUNKNOWN pUnk,
    _In_ BOOL fLock,
    _In_ BOOL fLastUnlockReleases
    );

_Check_return_ WINOLEAPI
CoGetStandardMarshal(
    _In_ REFIID riid,
    _In_ LPUNKNOWN pUnk,
    _In_ DWORD dwDestContext,
    _In_opt_ LPVOID pvDestContext,
    _In_ DWORD mshlflags,
    _Outptr_ LPMARSHAL FAR * ppMarshal
    );



_Check_return_ WINOLEAPI
CoGetStdMarshalEx(
    _In_ LPUNKNOWN pUnkOuter,
    _In_ DWORD smexflags,
    _Outptr_ LPUNKNOWN FAR * ppUnkInner
    );


/* flags for CoGetStdMarshalEx */
typedef enum tagSTDMSHLFLAGS
{
    SMEXF_SERVER     = 0x01,       // server side aggregated std marshaler
    SMEXF_HANDLER    = 0x02        // client side (handler) agg std marshaler
} STDMSHLFLAGS;


WINOLEAPI_(BOOL)
CoIsHandlerConnected(
    _In_ LPUNKNOWN pUnk
    );



// Apartment model inter-thread interface passing helpers
_Check_return_ WINOLEAPI
CoMarshalInterThreadInterfaceInStream(
    _In_ REFIID riid,
    _In_ LPUNKNOWN pUnk,
    _Outptr_ LPSTREAM * ppStm
    );


_Check_return_ WINOLEAPI
CoGetInterfaceAndReleaseStream(
    _In_ LPSTREAM pStm,
    _In_ REFIID iid,
    _Outptr_ LPVOID FAR * ppv
    );


_Check_return_ WINOLEAPI
CoCreateFreeThreadedMarshaler(
    _In_opt_ LPUNKNOWN punkOuter,
    _Outptr_ LPUNKNOWN * ppunkMarshal
    );


WINOLEAPI_(void)
CoFreeUnusedLibraries(
    void
    );


#if (_WIN32_WINNT >= 0x0501)
/* #!perl PoundIf("CoFreeUnusedLibrariesEx", "(_WIN32_WINNT >= 0x0501)");
*/
WINOLEAPI_(void)
CoFreeUnusedLibrariesEx(
    _In_ DWORD dwUnloadDelay,
    _In_ DWORD dwReserved
    );

#endif


#if (_WIN32_WINNT >= 0x0600)
_Check_return_ WINOLEAPI
CoDisconnectContext(
    DWORD dwTimeout
    );

#endif

// DCOM
#if (_WIN32_WINNT >= 0x0400) || defined(_WIN32_DCOM)

/* Call Security. */

/* #!perl PoundIf("CoInitializeSecurity", "(_WIN32_WINNT >= 0x0400 ) || defined(_WIN32_DCOM)");
*/
_Check_return_ WINOLEAPI
CoInitializeSecurity(
    _In_opt_ PSECURITY_DESCRIPTOR pSecDesc,
    _In_ LONG cAuthSvc,
    _In_reads_opt_(cAuthSvc) SOLE_AUTHENTICATION_SERVICE * asAuthSvc,
    _In_opt_ void * pReserved1,
    _In_ DWORD dwAuthnLevel,
    _In_ DWORD dwImpLevel,
    _In_opt_ void * pAuthList,
    _In_ DWORD dwCapabilities,
    _In_opt_ void * pReserved3
    );


/* #!perl PoundIf("CoGetCallContext", "(_WIN32_WINNT >= 0x0400 ) || defined(_WIN32_DCOM)");
*/
_Check_return_ WINOLEAPI
CoGetCallContext(
    _In_ REFIID riid,
    _Outptr_ void ** ppInterface
    );


/* #!perl PoundIf("CoQueryProxyBlanket", "(_WIN32_WINNT >= 0x0400 ) || defined(_WIN32_DCOM)");
*/
_Check_return_ WINOLEAPI
CoQueryProxyBlanket(
    _In_ IUnknown * pProxy,
    _Out_opt_ DWORD * pwAuthnSvc,
    _Out_opt_ DWORD * pAuthzSvc,
    _Outptr_opt_ LPOLESTR * pServerPrincName,
    _Out_opt_ DWORD * pAuthnLevel,
    _Out_opt_ DWORD * pImpLevel,
    _Out_opt_ RPC_AUTH_IDENTITY_HANDLE * pAuthInfo,
    _Out_opt_ DWORD * pCapabilites
    );


/* #!perl PoundIf("CoSetProxyBlanket", "(_WIN32_WINNT >= 0x0400 ) || defined(_WIN32_DCOM)");
*/
_Check_return_ WINOLEAPI
CoSetProxyBlanket(
    _In_ IUnknown * pProxy,
    _In_ DWORD dwAuthnSvc,
    _In_ DWORD dwAuthzSvc,
    _In_opt_ OLECHAR * pServerPrincName,
    _In_ DWORD dwAuthnLevel,
    _In_ DWORD dwImpLevel,
    _In_opt_ RPC_AUTH_IDENTITY_HANDLE pAuthInfo,
    _In_ DWORD dwCapabilities
    );


/* #!perl PoundIf("CoCopyProxy", "(_WIN32_WINNT >= 0x0400 ) || defined(_WIN32_DCOM)");
*/
_Check_return_ WINOLEAPI
CoCopyProxy(
    _In_ IUnknown * pProxy,
    _Outptr_ IUnknown ** ppCopy
    );


/* #!perl PoundIf("CoQueryClientBlanket", "(_WIN32_WINNT >= 0x0400 ) || defined(_WIN32_DCOM)");
*/
_Check_return_ WINOLEAPI
CoQueryClientBlanket(
    _Out_opt_ DWORD * pAuthnSvc,
    _Out_opt_ DWORD * pAuthzSvc,
    _Outptr_opt_ LPOLESTR * pServerPrincName,
    _Out_opt_ DWORD * pAuthnLevel,
    _Out_opt_ DWORD * pImpLevel,
    _Outptr_opt_result_buffer_(_Inexpressible_("depends on pAuthnSvc")) RPC_AUTHZ_HANDLE * pPrivs,
    _Inout_opt_ DWORD * pCapabilities
    );


/* #!perl PoundIf("CoImpersonateClient", "(_WIN32_WINNT >= 0x0400 ) || defined(_WIN32_DCOM)");
*/
_Check_return_ WINOLEAPI
CoImpersonateClient(
    void
    );


/* #!perl PoundIf("CoRevertToSelf", "(_WIN32_WINNT >= 0x0400 ) || defined(_WIN32_DCOM)");
*/
_Check_return_ WINOLEAPI
CoRevertToSelf(
    void
    );


/* #!perl PoundIf("CoQueryAuthenticationServices", "(_WIN32_WINNT >= 0x0400 ) || defined(_WIN32_DCOM)");
*/
_Check_return_ WINOLEAPI
CoQueryAuthenticationServices(
    _Out_ DWORD * pcAuthSvc,
    _Outptr_result_buffer_(*pcAuthSvc) SOLE_AUTHENTICATION_SERVICE ** asAuthSvc
    );


/* #!perl PoundIf("CoSwitchCallContext", "(_WIN32_WINNT >= 0x0400 ) || defined(_WIN32_DCOM)");
*/
_Check_return_ WINOLEAPI
CoSwitchCallContext(
    _In_opt_ IUnknown * pNewObject,
    _Outptr_ IUnknown ** ppOldObject
    );


#define COM_RIGHTS_EXECUTE 1
#define COM_RIGHTS_EXECUTE_LOCAL 2
#define COM_RIGHTS_EXECUTE_REMOTE 4
#define COM_RIGHTS_ACTIVATE_LOCAL 8
#define COM_RIGHTS_ACTIVATE_REMOTE 16



#endif // DCOM

#pragma region Desktop Family

#if ((WINAPI_FAMILY & WINAPI_PARTITION_DESKTOP) == WINAPI_PARTITION_DESKTOP)

/* helper for creating instances */

_Check_return_ WINOLEAPI
CoCreateInstance(
    _In_ REFCLSID rclsid,
    _In_opt_ LPUNKNOWN pUnkOuter,
    _In_ DWORD dwClsContext,
    _In_ REFIID riid,
    _COM_Outptr_ _At_(*ppv, _Post_readable_size_(_Inexpressible_(varies))) LPVOID FAR * ppv
    );



// DCOM
#if (_WIN32_WINNT >= 0x0400) || defined(_WIN32_DCOM)

  
/* #!perl PoundIf("CoCreateInstanceEx", "(_WIN32_WINNT >= 0x0400 ) || defined(_WIN32_DCOM)");
*/
_Check_return_ WINOLEAPI
CoCreateInstanceEx(
    _In_ REFCLSID Clsid,
    _In_opt_ IUnknown * punkOuter,
    _In_ DWORD dwClsCtx,
    _In_opt_ COSERVERINFO * pServerInfo,
    _In_ DWORD dwCount,
    _Inout_updates_(dwCount) MULTI_QI * pResults
    );


#endif // DCOM

#endif // ( (WINAPI_FAMILY & WINAPI_PARTITION_DESKTOP) == WINAPI_PARTITION_DESKTOP )
#pragma endregion


#if (_WIN32_WINNT >= 0x0602)
  
#pragma region Application Family

#if ((WINAPI_FAMILY & WINAPI_PARTITION_APP) == WINAPI_PARTITION_APP)

/* #!perl PoundIf("CoCreateInstanceFromApp", "(_WIN32_WINNT >= 0x0602)");
*/
_Check_return_ WINOLEAPI
CoCreateInstanceFromApp(
    _In_ REFCLSID Clsid,
    _In_opt_ IUnknown * punkOuter,
    _In_ DWORD dwClsCtx,
    _In_opt_ PVOID reserved,
    _In_ DWORD dwCount,
    _Inout_updates_(dwCount) MULTI_QI * pResults
    );


#endif // ( (WINAPI_FAMILY & WINAPI_PARTITION_APP) == WINAPI_PARTITION_APP )
#pragma endregion

#endif


#pragma region Application Family

#if (WINAPI_FAMILY == WINAPI_FAMILY_APP)

__inline _Check_return_ HRESULT CoCreateInstance(
    _In_     REFCLSID rclsid, 
    _In_opt_ LPUNKNOWN pUnkOuter,
    _In_     DWORD dwClsContext, 
    _In_     REFIID riid, 
    _COM_Outptr_ _At_(*ppv, _Post_readable_size_(_Inexpressible_(varies))) LPVOID FAR* ppv)
{
    MULTI_QI    OneQI;
    HRESULT     hr;

    OneQI.pItf = NULL;

#ifdef __cplusplus
    OneQI.pIID = &riid;
#else
    OneQI.pIID = riid;
#endif

    hr = CoCreateInstanceFromApp( rclsid, pUnkOuter, dwClsContext, NULL, 1, &OneQI );

    *ppv = OneQI.pItf;
    return FAILED(hr) ? hr : OneQI.hr;
}

__inline _Check_return_ HRESULT CoCreateInstanceEx(
    _In_ REFCLSID                      Clsid,
    _In_opt_ IUnknown     *            punkOuter,
    _In_ DWORD                         dwClsCtx,
    _In_opt_ COSERVERINFO *            pServerInfo,
    _In_ DWORD                         dwCount,
    _Inout_updates_(dwCount) MULTI_QI *pResults )
{
    return CoCreateInstanceFromApp(Clsid, punkOuter, dwClsCtx, pServerInfo, dwCount, pResults);
}

#endif // (WINAPI_FAMILY == WINAPI_FAMILY_APP)
#pragma endregion

/* Call related APIs */
// DCOM
#if (_WIN32_WINNT >= 0x0500) || defined(_WIN32_DCOM)

/* #!perl PoundIf("CoGetCancelObject", "(_WIN32_WINNT >= 0x0500 ) || defined(_WIN32_DCOM)");
*/
_Check_return_ WINOLEAPI
CoGetCancelObject(
    _In_ DWORD dwThreadId,
    _In_ REFIID iid,
    _Outptr_ void ** ppUnk
    );


/* #!perl PoundIf("CoSetCancelObject", "(_WIN32_WINNT >= 0x0500 ) || defined(_WIN32_DCOM)");
*/
_Check_return_ WINOLEAPI
CoSetCancelObject(
    _In_opt_ IUnknown * pUnk
    );


/* #!perl PoundIf("CoCancelCall", "(_WIN32_WINNT >= 0x0500 ) || defined(_WIN32_DCOM)");
*/
_Check_return_ WINOLEAPI
CoCancelCall(
    _In_ DWORD dwThreadId,
    _In_ ULONG ulTimeout
    );


/* #!perl PoundIf("CoTestCancel", "(_WIN32_WINNT >= 0x0500 ) || defined(_WIN32_DCOM)");
*/
_Check_return_ WINOLEAPI
CoTestCancel(
    void
    );


/* #!perl PoundIf("CoEnableCallCancellation", "(_WIN32_WINNT >= 0x0500 ) || defined(_WIN32_DCOM)");
*/
_Check_return_ WINOLEAPI
CoEnableCallCancellation(
    _In_opt_ LPVOID pReserved
    );


/* #!perl PoundIf("CoDisableCallCancellation", "(_WIN32_WINNT >= 0x0500 ) || defined(_WIN32_DCOM)");
*/
_Check_return_ WINOLEAPI
CoDisableCallCancellation(
    _In_opt_ LPVOID pReserved
    );


/* #!perl PoundIf("DcomChannelSetHResult", "(_WIN32_WINNT >= 0x0500 ) || defined(_WIN32_DCOM)");
*/
WINOLEAPI
DcomChannelSetHResult(
    _In_opt_ LPVOID pvReserved,
    _In_opt_ ULONG * pulReserved,
    _In_ HRESULT appsHR
    );


#endif

/* other helpers */

_Check_return_ WINOLEAPI
StringFromCLSID(
    _In_ REFCLSID rclsid,
    _Outptr_ LPOLESTR FAR * lplpsz
    );

_Check_return_ WINOLEAPI
CLSIDFromString(
    _In_ LPCOLESTR lpsz,
    _Out_ LPCLSID pclsid
    );

_Check_return_ WINOLEAPI
StringFromIID(
    _In_ REFIID rclsid,
    _Outptr_ LPOLESTR FAR * lplpsz
    );

_Check_return_ WINOLEAPI
IIDFromString(
    _In_ LPCOLESTR lpsz,
    _Out_ LPIID lpiid
    );

_Check_return_ WINOLEAPI
ProgIDFromCLSID(
    _In_ REFCLSID clsid,
    _Outptr_ LPOLESTR FAR * lplpszProgID
    );

_Check_return_ WINOLEAPI
CLSIDFromProgID(
    _In_ LPCOLESTR lpszProgID,
    _Out_ LPCLSID lpclsid
    );

_Check_return_ WINOLEAPI_(int)
StringFromGUID2(
    _In_ REFGUID rguid,
    _Out_writes_to_(cchMax, return) LPOLESTR lpsz,
    _In_ int cchMax
    );


_Check_return_ WINOLEAPI
CoCreateGuid(
    _Out_ GUID FAR * pguid
    );


/* Prop variant support */

typedef struct tagPROPVARIANT PROPVARIANT;

_Check_return_
WINOLEAPI
PropVariantCopy(
    _Out_ PROPVARIANT * pvarDest,
    _In_ const PROPVARIANT * pvarSrc
    );


WINOLEAPI
PropVariantClear(
    _Inout_ PROPVARIANT * pvar
    );


WINOLEAPI
FreePropVariantArray(
    _In_ ULONG cVariants,
    _Inout_updates_(cVariants) PROPVARIANT * rgvars
    );


// DCOM
#if (_WIN32_WINNT >= 0x0400) || defined(_WIN32_DCOM)
/* #!perl PoundIf("CoRegisterChannelHook", "(_WIN32_WINNT >= 0x0400 ) || defined(_WIN32_DCOM)");
*/
WINOLEAPI
CoRegisterChannelHook(
    _In_ REFGUID ExtensionUuid,
    _In_ IChannelHook * pChannelHook
    );

#endif // DCOM

// DCOM
#if (_WIN32_WINNT >= 0x0400) || defined(_WIN32_DCOM)
/* Synchronization API */

/* #!perl PoundIf("CoWaitForMultipleHandles", "(_WIN32_WINNT >= 0x0400 ) || defined(_WIN32_DCOM)");
*/
_Check_return_ WINOLEAPI
CoWaitForMultipleHandles(
    _In_ DWORD dwFlags,
    _In_ DWORD dwTimeout,
    _In_ ULONG cHandles,
    _In_reads_(cHandles) LPHANDLE pHandles,
    _Out_ LPDWORD lpdwindex
    );


/* Flags for Synchronization API and Classes */

typedef enum tagCOWAIT_FLAGS
{
  COWAIT_WAITALL = 1,
  COWAIT_ALERTABLE = 2,
  COWAIT_INPUTAVAILABLE = 4,
  COWAIT_DISPATCH_CALLS = 8,
  COWAIT_DISPATCH_WINDOW_MESSAGES = 0x10,    

  // REAL_COWAIT_ENABLECALLREENTRANCY, COWAIT_ENABLEWINDOWMESSAGES and COWAIT_ENABLECALLREENTRANCY will be removed. 
  // Use COWAIT_DISPATCH_CALLS and COWAIT_DISPATCH_WINDOW_MESSAGES.

  // REAL_COWAIT_ENABLECALLREENTRANCY can be used when ENABLEWINDOWMESSAGES is not intended
  REAL_COWAIT_ENABLECALLREENTRANCY = COWAIT_DISPATCH_CALLS, 
  COWAIT_ENABLEWINDOWMESSAGES = COWAIT_DISPATCH_WINDOW_MESSAGES,

  // Temporarily making COWAIT_ENABLECALLREENTRANCY include the new ENABLEWINDOWMESSAGES flag so that CoWaitForMultipleHandles replacing a
  // message loop will continue to get message loop parity behavior.
  COWAIT_ENABLECALLREENTRANCY = 0x18,                
}COWAIT_FLAGS;

typedef enum CWMO_FLAGS
{
  CWMO_DISPATCH_CALLS = 1,
  CWMO_DISPATCH_WINDOW_MESSAGES = 2,

  // CWMO_ENABLE_CALL_REENTRANCY, REAL_CWMO_ENABLE_CALL_REENTRANCY and CWMO_ENABLE_WINDOW_MESSAGES will be removed. 
  // Use CWMO_DISPATCH_CALLS and CWMO_DISPATCH_WINDOW_MESSAGES.

  // REAL_CWMO_ENABLE_CALL_REENTRANCY can be used when ENABLE_WINDOW_MESSAGES is not intended
  REAL_CWMO_ENABLE_CALL_REENTRANCY = CWMO_DISPATCH_CALLS,
  CWMO_ENABLE_WINDOW_MESSAGES = CWMO_DISPATCH_WINDOW_MESSAGES,

  // Temporarily making CWMO_ENABLE_CALL_REENTRANCY include the new ENABLE_WINDOW_MESSAGES flag so that CoWaitForMultipleObjects replacing a
  // message loop will continue to get message loop parity behavior.
  CWMO_ENABLE_CALL_REENTRANCY = 3,    
} CWMO_FLAGS;

static const ULONG CWMO_MAX_HANDLES = 56;

#endif // DCOM

_Check_return_ WINOLEAPI
CoGetTreatAsClass(
    _In_ REFCLSID clsidOld,
    _Out_ LPCLSID pClsidNew
    );


/* for flushing OLESCM remote binding handles */


#if (_WIN32_WINNT >= 0x0501)
/* #!perl
    PoundIf("CoInvalidateRemoteMachineBindings", "_WIN32_WINNT >= 0x0501");
*/
_Check_return_ WINOLEAPI
CoInvalidateRemoteMachineBindings(
    _In_ LPOLESTR pszMachineName
    );

#endif


 
/* the server dlls must define their DllGetClassObject and DllCanUnloadNow
 * to match these; the typedefs are located here to ensure all are changed at
 * the same time.
 */

//#ifdef _MAC
//typedef STDAPICALLTYPE HRESULT (* LPFNGETCLASSOBJECT) (REFCLSID, REFIID, LPVOID *);
//#else
typedef HRESULT (STDAPICALLTYPE * LPFNGETCLASSOBJECT) (REFCLSID, REFIID, LPVOID *);
//#endif

//#ifdef _MAC
//typedef STDAPICALLTYPE HRESULT (* LPFNCANUNLOADNOW)(void);
//#else
typedef HRESULT (STDAPICALLTYPE * LPFNCANUNLOADNOW)(void);
//#endif

_Check_return_
STDAPI  DllGetClassObject(_In_ REFCLSID rclsid, _In_ REFIID riid, _Outptr_ LPVOID FAR* ppv);

STDAPI  DllCanUnloadNow(void);


/****** Default Memory Allocation ******************************************/
WINOLEAPI_(_Ret_opt_ _Post_writable_byte_size_(cb)  __drv_allocatesMem(Mem) _Check_return_ LPVOID)
CoTaskMemAlloc(
    _In_ SIZE_T cb
    );

WINOLEAPI_(_Ret_opt_ _Post_writable_byte_size_(cb)  _When_(cb > 0, __drv_allocatesMem(Mem) _Check_return_) LPVOID)
CoTaskMemRealloc(
    _In_opt_ __drv_freesMem(Mem) _Post_invalid_ LPVOID pv,
    _In_ SIZE_T cb
    );

WINOLEAPI_(void)
CoTaskMemFree(
    _In_opt_ __drv_freesMem(Mem) _Post_invalid_ LPVOID pv
    );
 
 
#ifndef RC_INVOKED
#include <poppack.h>
#endif // RC_INVOKED

#endif     // __COMBASEAPI_H__

