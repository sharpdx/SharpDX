
//+---------------------------------------------------------------------------
//
//  Microsoft Windows
//  Copyright (c) Microsoft Corporation. All rights reserved.
//
//  File:       objbase.h
//
//  Contents:   Component object model defintions.
//
//----------------------------------------------------------------------------

#include <rpc.h>
#include <rpcndr.h>


#if(NTDDI_VERSION >= NTDDI_VISTA && !defined(_WIN32_WINNT))
#define _WIN32_WINNT 0x0600
#endif

#if(NTDDI_VERSION >= NTDDI_WS03 && !defined(_WIN32_WINNT))
#define _WIN32_WINNT 0x0502
#endif

#if(NTDDI_VERSION >= NTDDI_WINXP && !defined(_WIN32_WINNT))
#define _WIN32_WINNT 0x0501
#endif

#if(NTDDI_VERSION >= NTDDI_WIN2K && !defined(_WIN32_WINNT))
#define _WIN32_WINNT 0x0500
#endif


#if !defined( _OBJBASE_H_ )
#define _OBJBASE_H_

#if _MSC_VER > 1000
#pragma once
#endif

#include <pshpack8.h>

#ifdef _MAC
#ifndef _WLM_NOFORCE_LIBS

#ifdef _WLMDLL
        #ifdef _DEBUG
                #pragma comment(lib, "oledlgd.lib")
                #pragma comment(lib, "msvcoled.lib")
        #else
                #pragma comment(lib, "oledlg.lib")
                #pragma comment(lib, "msvcole.lib")
        #endif
#else
        #ifdef _DEBUG
                #pragma comment(lib, "wlmoled.lib")
                #pragma comment(lib, "ole2uid.lib")
        #else
                #pragma comment(lib, "wlmole.lib")
                #pragma comment(lib, "ole2ui.lib")
        #endif
        #pragma data_seg(".drectve")
        static char _gszWlmOLEUIResourceDirective[] = "/macres:ole2ui.rsc";
        #pragma data_seg()
#endif

#pragma comment(lib, "uuid.lib")

#ifdef _DEBUG
    #pragma comment(lib, "ole2d.lib")
    #pragma comment(lib, "ole2autd.lib")
#else
    #pragma comment(lib, "ole2.lib")
    #pragma comment(lib, "ole2auto.lib")
#endif

#endif // !_WLM_NOFORCE_LIBS
#endif // _MAC

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
#if defined(_MPPC_)  && \
    ( (defined(_MSC_VER) || defined(__SC__) || defined(__MWERKS__)) && \
    !defined(NO_NULL_VTABLE_ENTRY) )
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


#ifdef _MAC
#if !defined(OLE2ANSI)
#define OLE2ANSI
#endif
#endif

#include <stdlib.h>

#define LISet32(li, v) ((li).HighPart = ((LONG) (v)) < 0 ? -1 : 0, (li).LowPart = (v))

#define ULISet32(li, v) ((li).HighPart = 0, (li).LowPart = (v))






#define CLSCTX_INPROC           (CLSCTX_INPROC_SERVER|CLSCTX_INPROC_HANDLER)

// With DCOM, CLSCTX_REMOTE_SERVER should be included
#if (_WIN32_WINNT >= 0x0400 ) || defined(_WIN32_DCOM) // DCOM
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

// interface marshaling definitions
#define MARSHALINTERFACE_MIN 500 // minimum number of bytes for interface marshl


//
// Common typedefs for paramaters used in Storage API's, gleamed from storage.h
// Also contains Storage error codes, which should be moved into the storage
// idl files.
//


#define CWCSTORAGENAME 32

/* Storage instantiation modes */
#define STGM_DIRECT             0x00000000L
#define STGM_TRANSACTED         0x00010000L
#define STGM_SIMPLE             0x08000000L

#define STGM_READ               0x00000000L
#define STGM_WRITE              0x00000001L
#define STGM_READWRITE          0x00000002L

#define STGM_SHARE_DENY_NONE    0x00000040L
#define STGM_SHARE_DENY_READ    0x00000030L
#define STGM_SHARE_DENY_WRITE   0x00000020L
#define STGM_SHARE_EXCLUSIVE    0x00000010L

#define STGM_PRIORITY           0x00040000L
#define STGM_DELETEONRELEASE    0x04000000L
#if (WINVER >= 400)
#define STGM_NOSCRATCH          0x00100000L
#endif /* WINVER */

#define STGM_CREATE             0x00001000L
#define STGM_CONVERT            0x00020000L
#define STGM_FAILIFTHERE        0x00000000L

#define STGM_NOSNAPSHOT         0x00200000L
#if (_WIN32_WINNT >= 0x0500)
#define STGM_DIRECT_SWMR        0x00400000L
#endif

/*  flags for internet asyncronous and layout docfile */
#define ASYNC_MODE_COMPATIBILITY    0x00000001L
#define ASYNC_MODE_DEFAULT          0x00000000L

#define STGTY_REPEAT                0x00000100L
#define STG_TOEND                   0xFFFFFFFFL

#define STG_LAYOUT_SEQUENTIAL       0x00000000L
#define STG_LAYOUT_INTERLEAVED      0x00000001L

typedef DWORD STGFMT;

#define STGFMT_STORAGE          0
#define STGFMT_NATIVE           1
#define STGFMT_FILE             3
#define STGFMT_ANY              4
#define STGFMT_DOCFILE          5

// This is a legacy define to allow old component to builds
#define STGFMT_DOCUMENT         0

/* here is where we pull in the MIDL generated headers for the interfaces */
typedef interface    IRpcStubBuffer     IRpcStubBuffer;
typedef interface    IRpcChannelBuffer  IRpcChannelBuffer;

#include <wtypes.h>
#include <unknwn.h>
#include <objidl.h>

#ifdef _OLE32_
#ifdef _OLE32PRIV_
BOOL _fastcall wIsEqualGUID(REFGUID rguid1, REFGUID rguid2);
#define IsEqualGUID(rguid1, rguid2) wIsEqualGUID(rguid1, rguid2)
#else
#define __INLINE_ISEQUAL_GUID
#endif  // _OLE32PRIV_
#endif  // _OLE32_

#include <guiddef.h>

#ifndef INITGUID
#include <cguid.h>
#endif

// COM initialization flags; passed to CoInitialize.
typedef enum tagCOINIT
{
  COINIT_APARTMENTTHREADED  = 0x2,      // Apartment model

#if  (_WIN32_WINNT >= 0x0400 ) || defined(_WIN32_DCOM) // DCOM
  // These constants are only valid on Windows NT 4.0
  COINIT_MULTITHREADED      = 0x0,      // OLE calls objects on any thread.
  COINIT_DISABLE_OLE1DDE    = 0x4,      // Don't use DDE for Ole1 support.
  COINIT_SPEED_OVER_MEMORY  = 0x8,      // Trade memory for speed.
#endif // DCOM
} COINIT;





/****** STD Object API Prototypes *****************************************/

WINOLEAPI_(DWORD) CoBuildVersion( VOID );

/* init/uninit */

__checkReturn WINOLEAPI  CoInitialize(__in_opt LPVOID pvReserved);
WINOLEAPI_(void)  CoUninitialize(void);
__checkReturn WINOLEAPI  CoGetMalloc(__in DWORD dwMemContext, __deref_out LPMALLOC FAR* ppMalloc);
WINOLEAPI_(DWORD) CoGetCurrentProcess(void);
WINOLEAPI  CoRegisterMallocSpy(__in LPMALLOCSPY pMallocSpy);
WINOLEAPI  CoRevokeMallocSpy(void);
WINOLEAPI  CoCreateStandardMalloc(__in DWORD memctx, __deref_out IMalloc FAR* FAR* ppMalloc);

#if (_WIN32_WINNT >= 0x0400 ) || defined(_WIN32_DCOM) // DCOM

__checkReturn WINOLEAPI  CoInitializeEx(__in_opt LPVOID pvReserved, __in DWORD dwCoInit);


WINOLEAPI CoGetCallerTID( __out LPDWORD lpdwTID );


WINOLEAPI CoGetCurrentLogicalThreadId(__out GUID *pguid);
#endif // DCOM

#if (_WIN32_WINNT >= 0x0501)

__checkReturn WINOLEAPI  CoRegisterInitializeSpy(__in LPINITIALIZESPY pSpy, __out ULARGE_INTEGER *puliCookie);
__checkReturn WINOLEAPI  CoRevokeInitializeSpy(__in ULARGE_INTEGER uliCookie);

__checkReturn WINOLEAPI  CoGetContextToken(__out ULONG_PTR* pToken);

// COM System Security Descriptors (used when the corresponding registry
// entries are absent)
typedef enum tagCOMSD
{
    SD_LAUNCHPERMISSIONS = 0,       // Machine wide launch permissions
    SD_ACCESSPERMISSIONS = 1,       // Machine wide acesss permissions
    SD_LAUNCHRESTRICTIONS = 2,      // Machine wide launch limits
    SD_ACCESSRESTRICTIONS = 3       // Machine wide access limits

} COMSD;
__checkReturn WINOLEAPI  CoGetSystemSecurityPermissions(COMSD comSDType, PSECURITY_DESCRIPTOR *ppSD);

#endif

// definition for Win7 new APIs
#if (_WIN32_WINNT >= _NT_TARGET_VERSION_WIN7)

__checkReturn WINOLEAPI CoGetApartmentType(__out APTTYPE * pAptType, __out APTTYPEQUALIFIER * pAptQualifier);

#endif

#if DBG == 1
WINOLEAPI_(ULONG) DebugCoGetRpcFault( void );
WINOLEAPI_(void) DebugCoSetRpcFault( ULONG );
#endif

#if (_WIN32_WINNT >= 0x0500)

typedef struct tagSOleTlsDataPublic
{
    void *pvReserved0[2];
    DWORD dwReserved0[3];
    void *pvReserved1[1];
    DWORD dwReserved1[3];
    void *pvReserved2[4];
    DWORD dwReserved2[1];
    void *pCurrentCtx;
} SOleTlsDataPublic;

#endif

/* COM+ APIs */

__checkReturn WINOLEAPI     CoGetObjectContext(__in REFIID riid, __deref_out LPVOID FAR* ppv);

/* register/revoke/get class objects */

__checkReturn WINOLEAPI  CoGetClassObject(__in REFCLSID rclsid, __in DWORD dwClsContext, __in_opt LPVOID pvReserved,
                    __in REFIID riid, __deref_out LPVOID FAR* ppv);
__checkReturn WINOLEAPI  CoRegisterClassObject(__in REFCLSID rclsid, __in LPUNKNOWN pUnk,
                    __in DWORD dwClsContext, __in DWORD flags, __out LPDWORD lpdwRegister);
__checkReturn WINOLEAPI  CoRevokeClassObject(__in DWORD dwRegister);
__checkReturn WINOLEAPI  CoResumeClassObjects(void);
__checkReturn WINOLEAPI  CoSuspendClassObjects(void);
WINOLEAPI_(ULONG) CoAddRefServerProcess(void);
WINOLEAPI_(ULONG) CoReleaseServerProcess(void);
__checkReturn WINOLEAPI  CoGetPSClsid(__in REFIID riid, __out CLSID *pClsid);
__checkReturn WINOLEAPI  CoRegisterPSClsid(__in REFIID riid, __in REFCLSID rclsid);

// Registering surrogate processes
__checkReturn WINOLEAPI  CoRegisterSurrogate(__in LPSURROGATE pSurrogate);

/* marshaling interface pointers */

__checkReturn WINOLEAPI CoGetMarshalSizeMax(__out ULONG *pulSize, __in REFIID riid, __in LPUNKNOWN pUnk,
                    __in DWORD dwDestContext, __in_opt LPVOID pvDestContext, __in DWORD mshlflags);
__checkReturn WINOLEAPI CoMarshalInterface(__in LPSTREAM pStm, __in REFIID riid, __in LPUNKNOWN pUnk,
                    __in DWORD dwDestContext, __in_opt LPVOID pvDestContext, __in DWORD mshlflags);
__checkReturn WINOLEAPI CoUnmarshalInterface(__in LPSTREAM pStm, __in REFIID riid, __deref_out LPVOID FAR* ppv);
WINOLEAPI CoMarshalHresult(__in LPSTREAM pstm, __in HRESULT hresult);
WINOLEAPI CoUnmarshalHresult(__in LPSTREAM pstm, __out HRESULT FAR * phresult);
__checkReturn WINOLEAPI CoReleaseMarshalData(__in LPSTREAM pStm);
__checkReturn WINOLEAPI CoDisconnectObject(__in LPUNKNOWN pUnk, __in DWORD dwReserved);
__checkReturn WINOLEAPI CoLockObjectExternal(__in LPUNKNOWN pUnk, __in BOOL fLock, __in BOOL fLastUnlockReleases);
__checkReturn WINOLEAPI CoGetStandardMarshal(__in REFIID riid, __in LPUNKNOWN pUnk,
                    __in DWORD dwDestContext, __in_opt LPVOID pvDestContext, __in DWORD mshlflags,
                    __deref_out LPMARSHAL FAR* ppMarshal);


__checkReturn WINOLEAPI CoGetStdMarshalEx(__in LPUNKNOWN pUnkOuter, __in DWORD smexflags,
                            __deref_out LPUNKNOWN FAR* ppUnkInner);

/* flags for CoGetStdMarshalEx */
typedef enum tagSTDMSHLFLAGS
{
    SMEXF_SERVER     = 0x01,       // server side aggregated std marshaler
    SMEXF_HANDLER    = 0x02        // client side (handler) agg std marshaler
} STDMSHLFLAGS;


WINOLEAPI_(BOOL) CoIsHandlerConnected(__in LPUNKNOWN pUnk);

// Apartment model inter-thread interface passing helpers
__checkReturn WINOLEAPI CoMarshalInterThreadInterfaceInStream(__in REFIID riid, __in LPUNKNOWN pUnk,
                    __deref_out LPSTREAM *ppStm);

__checkReturn WINOLEAPI CoGetInterfaceAndReleaseStream(__in LPSTREAM pStm, __in REFIID iid,
                    __deref_out LPVOID FAR* ppv);

__checkReturn WINOLEAPI CoCreateFreeThreadedMarshaler(__in_opt LPUNKNOWN  punkOuter,
                    __deref_out LPUNKNOWN *ppunkMarshal);

/* dll loading helpers; keeps track of ref counts and unloads all on exit */

WINOLEAPI_(HINSTANCE) CoLoadLibrary(__in LPOLESTR lpszLibName, __in BOOL bAutoFree);
WINOLEAPI_(void) CoFreeLibrary(__in HINSTANCE hInst);
WINOLEAPI_(void) CoFreeAllLibraries(void);
WINOLEAPI_(void) CoFreeUnusedLibraries(void);
#if  (_WIN32_WINNT >= 0x0501)

WINOLEAPI_(void) CoFreeUnusedLibrariesEx(__in DWORD dwUnloadDelay, __in DWORD dwReserved);
#endif

#if (_WIN32_WINNT >= 0x0600)
__checkReturn WINOLEAPI CoDisconnectContext(DWORD dwTimeout);
#endif

#if (_WIN32_WINNT >= 0x0400 ) || defined(_WIN32_DCOM) // DCOM

/* Call Security. */


__checkReturn WINOLEAPI CoInitializeSecurity(
                    __in_opt PSECURITY_DESCRIPTOR    pSecDesc,
                    __in LONG                        cAuthSvc,
                    __in_ecount_opt(cAuthSvc) 
                         SOLE_AUTHENTICATION_SERVICE *asAuthSvc,
                    __in_opt void                    *pReserved1,
                    __in DWORD                        dwAuthnLevel,
                    __in DWORD                        dwImpLevel,
                    __in_opt void                    *pAuthList,
                    __in DWORD                        dwCapabilities,
                    __in_opt void                    *pReserved3 );


__checkReturn WINOLEAPI CoGetCallContext( __in REFIID riid, __deref_out void **ppInterface );


__checkReturn WINOLEAPI CoQueryProxyBlanket(
    __in IUnknown                       *pProxy,
    __out_opt DWORD                     *pwAuthnSvc,
    __out_opt DWORD                     *pAuthzSvc,
    __deref_opt_out OLECHAR            **pServerPrincName,
    __out_opt DWORD                     *pAuthnLevel,
    __out_opt DWORD                     *pImpLevel,
    __out_opt RPC_AUTH_IDENTITY_HANDLE  *pAuthInfo,
    __out_opt DWORD                     *pCapabilites );


__checkReturn WINOLEAPI CoSetProxyBlanket(
    __in IUnknown                     *pProxy,
    __in DWORD                         dwAuthnSvc,
    __in DWORD                         dwAuthzSvc,
    __in_opt OLECHAR                  *pServerPrincName,
    __in DWORD                         dwAuthnLevel,
    __in DWORD                         dwImpLevel,
    __in_opt RPC_AUTH_IDENTITY_HANDLE  pAuthInfo,
    __in DWORD                         dwCapabilities );


__checkReturn WINOLEAPI CoCopyProxy(
    __in IUnknown           *pProxy,
    __deref_out IUnknown   **ppCopy );


__checkReturn WINOLEAPI CoQueryClientBlanket(
    __out_opt DWORD             *pAuthnSvc,
    __out_opt DWORD             *pAuthzSvc,
    __out_opt OLECHAR           **pServerPrincName,
    __out_opt DWORD             *pAuthnLevel,
    __out_opt DWORD             *pImpLevel,
    __out_opt RPC_AUTHZ_HANDLE  *pPrivs,
    __inout_opt DWORD           *pCapabilities );


__checkReturn WINOLEAPI CoImpersonateClient(void);


__checkReturn WINOLEAPI CoRevertToSelf(void);


__checkReturn WINOLEAPI CoQueryAuthenticationServices(
    __out DWORD *pcAuthSvc,
    __deref_out_ecount(*pcAuthSvc) SOLE_AUTHENTICATION_SERVICE **asAuthSvc );


__checkReturn WINOLEAPI CoSwitchCallContext( __in_opt IUnknown *pNewObject, __deref_out IUnknown **ppOldObject );

#define COM_RIGHTS_EXECUTE 1
#define COM_RIGHTS_EXECUTE_LOCAL 2
#define COM_RIGHTS_EXECUTE_REMOTE 4
#define COM_RIGHTS_ACTIVATE_LOCAL 8
#define COM_RIGHTS_ACTIVATE_REMOTE 16



#endif // DCOM

/* helper for creating instances */

__checkReturn WINOLEAPI CoCreateInstance(__in     REFCLSID rclsid, 
                           __in_opt LPUNKNOWN pUnkOuter,
                           __in     DWORD dwClsContext, 
                           __in     REFIID riid, 
                           __deref_out LPVOID FAR* ppv);


#if (_WIN32_WINNT >= 0x0400 ) || defined(_WIN32_DCOM) // DCOM


__checkReturn WINOLEAPI CoGetInstanceFromFile(
    __in_opt COSERVERINFO *            pServerInfo,
    __in_opt CLSID        *            pClsid,
    __in_opt IUnknown     *            punkOuter, // only relevant locally
    __in DWORD                         dwClsCtx,
    __in DWORD                         grfMode,
    __in OLECHAR          *            pwszName,
    __in DWORD                         dwCount,
    __inout_ecount(dwCount) MULTI_QI * pResults );


__checkReturn WINOLEAPI CoGetInstanceFromIStorage(
    __in_opt COSERVERINFO *            pServerInfo,
    __in_opt CLSID        *            pClsid,
    __in_opt IUnknown     *            punkOuter, // only relevant locally
    __in DWORD                         dwClsCtx,
    __in struct IStorage  *            pstg,
    __in DWORD                         dwCount,
    __inout_ecount(dwCount) MULTI_QI * pResults );


__checkReturn WINOLEAPI CoCreateInstanceEx(
    __in REFCLSID                      Clsid,
    __in_opt IUnknown     *            punkOuter, // only relevant locally
    __in DWORD                         dwClsCtx,
    __in_opt COSERVERINFO *            pServerInfo,
    __in DWORD                         dwCount,
    __inout_ecount(dwCount) MULTI_QI * pResults );

#endif // DCOM

/* Call related APIs */
#if (_WIN32_WINNT >= 0x0500 ) || defined(_WIN32_DCOM) // DCOM


__checkReturn WINOLEAPI CoGetCancelObject(__in DWORD dwThreadId, __in REFIID iid, __deref_out void **ppUnk);


__checkReturn WINOLEAPI CoSetCancelObject(__in_opt IUnknown *pUnk);


__checkReturn WINOLEAPI CoCancelCall(__in DWORD dwThreadId, __in ULONG ulTimeout);


__checkReturn WINOLEAPI CoTestCancel(void);


__checkReturn WINOLEAPI CoEnableCallCancellation(__in_opt LPVOID pReserved);


__checkReturn WINOLEAPI CoDisableCallCancellation(__in_opt LPVOID pReserved);


WINOLEAPI CoAllowSetForegroundWindow(__in IUnknown *pUnk, __in_opt LPVOID lpvReserved);


WINOLEAPI DcomChannelSetHResult(__in_opt LPVOID pvReserved, __in_opt ULONG* pulReserved, __in HRESULT appsHR);

#endif

/* other helpers */

__checkReturn WINOLEAPI StringFromCLSID(__in REFCLSID rclsid, __deref_out LPOLESTR FAR* lplpsz);
__checkReturn WINOLEAPI CLSIDFromString(__in LPCOLESTR lpsz, __out LPCLSID pclsid);
__checkReturn WINOLEAPI StringFromIID(__in REFIID rclsid, __deref_out LPOLESTR FAR* lplpsz);
__checkReturn WINOLEAPI IIDFromString(__in LPCOLESTR lpsz, __out LPIID lpiid);
WINOLEAPI_(BOOL) CoIsOle1Class(__in REFCLSID rclsid);
__checkReturn WINOLEAPI ProgIDFromCLSID (__in REFCLSID clsid, __deref_out LPOLESTR FAR* lplpszProgID);
__checkReturn WINOLEAPI CLSIDFromProgID (__in LPCOLESTR lpszProgID, __out LPCLSID lpclsid);
__checkReturn WINOLEAPI CLSIDFromProgIDEx (__in LPCOLESTR lpszProgID, __out LPCLSID lpclsid);
__checkReturn WINOLEAPI_(int) StringFromGUID2(__in REFGUID rguid, __out_ecount(cchMax) LPOLESTR lpsz, __in int cchMax);

__checkReturn WINOLEAPI CoCreateGuid(__out GUID FAR *pguid);

WINOLEAPI_(BOOL) CoFileTimeToDosDateTime(
                 __in FILETIME FAR* lpFileTime, __out LPWORD lpDosDate, __out LPWORD lpDosTime);
WINOLEAPI_(BOOL) CoDosDateTimeToFileTime(
                       __in WORD nDosDate, __in WORD nDosTime, __out FILETIME FAR* lpFileTime);
WINOLEAPI  CoFileTimeNow( __out FILETIME FAR* lpFileTime );


__checkReturn WINOLEAPI CoRegisterMessageFilter( __in_opt LPMESSAGEFILTER lpMessageFilter,
                                __deref_opt_out_opt LPMESSAGEFILTER FAR* lplpMessageFilter );

#if (_WIN32_WINNT >= 0x0400 ) || defined(_WIN32_DCOM) // DCOM

WINOLEAPI CoRegisterChannelHook( __in REFGUID ExtensionUuid, __in IChannelHook *pChannelHook );
#endif // DCOM

#if (_WIN32_WINNT >= 0x0400 ) || defined(_WIN32_DCOM) // DCOM
/* Synchronization API */


__checkReturn WINOLEAPI CoWaitForMultipleHandles (__in DWORD dwFlags,
                                    __in DWORD dwTimeout,
                                    __in ULONG cHandles,
                                    __in_ecount(cHandles) LPHANDLE pHandles,
                                    __out LPDWORD  lpdwindex);

/* Flags for Synchronization API and Classes */

typedef enum tagCOWAIT_FLAGS
{
  COWAIT_WAITALL = 1,
  COWAIT_ALERTABLE = 2,
  COWAIT_INPUTAVAILABLE = 4
}COWAIT_FLAGS;

#endif // DCOM

/* for flushing OLESCM remote binding handles */

#if  (_WIN32_WINNT >= 0x0501)

__checkReturn WINOLEAPI CoInvalidateRemoteMachineBindings(__in LPOLESTR pszMachineName);
#endif

/* TreatAs APIS */

__checkReturn WINOLEAPI CoGetTreatAsClass(__in REFCLSID clsidOld, __out LPCLSID pClsidNew);
__checkReturn WINOLEAPI CoTreatAsClass(__in REFCLSID clsidOld, __in REFCLSID clsidNew);


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

STDAPI  DllGetClassObject(__in REFCLSID rclsid, __in REFIID riid, __deref_out LPVOID FAR* ppv);

STDAPI  DllCanUnloadNow(void);


/****** Default Memory Allocation ******************************************/
WINOLEAPI_(__bcount_opt(cb) __allocator __drv_allocatesMem(Mem) __checkReturn LPVOID) CoTaskMemAlloc(
	__in SIZE_T cb);
WINOLEAPI_(__bcount_opt(cb) __allocator __drv_when(cb > 0, __drv_allocatesMem(Mem) __checkReturn) LPVOID) CoTaskMemRealloc(
	__in_opt __drv_freesMem(Mem) __post __notvalid LPVOID pv, 
	__in SIZE_T cb);
WINOLEAPI_(void)   CoTaskMemFree(
	__in_opt __drv_freesMem(Mem) __post __notvalid LPVOID pv
	); 


/****** DV APIs ***********************************************************/

/* This function is declared in objbase.h and ole2.h */
WINOLEAPI CreateDataAdviseHolder(__deref_out LPDATAADVISEHOLDER FAR* ppDAHolder);

WINOLEAPI CreateDataCache(__in_opt LPUNKNOWN pUnkOuter, __in REFCLSID rclsid,
                          __in REFIID iid, __out LPVOID FAR* ppv);


/****** Storage API Prototypes ********************************************/


__checkReturn WINOLEAPI StgCreateDocfile(__in_opt __nullterminated const WCHAR* pwcsName,
            __in DWORD grfMode,
            __reserved DWORD reserved,
            __deref_out IStorage** ppstgOpen);

__checkReturn WINOLEAPI StgCreateDocfileOnILockBytes(__in ILockBytes* plkbyt,
                    __in DWORD grfMode,
                    __in DWORD reserved,
                    __deref_out IStorage** ppstgOpen);

__checkReturn WINOLEAPI StgOpenStorage(__in_opt __nullterminated const WCHAR* pwcsName,
              __in_opt IStorage* pstgPriority,
              __in DWORD grfMode,
              __in_opt SNB snbExclude,
              __in DWORD reserved,
              __deref_out IStorage** ppstgOpen);
__checkReturn WINOLEAPI StgOpenStorageOnILockBytes(__in ILockBytes* plkbyt,
                  __in_opt IStorage* pstgPriority,
                  __in DWORD grfMode,
                  __in_opt SNB snbExclude,
                  __reserved DWORD reserved,
                  __deref_out IStorage** ppstgOpen);

__checkReturn WINOLEAPI StgIsStorageFile(__in __nullterminated const WCHAR* pwcsName);
__checkReturn WINOLEAPI StgIsStorageILockBytes(__in ILockBytes* plkbyt);

__checkReturn WINOLEAPI StgSetTimes(__in __nullterminated const WCHAR* lpszName,
                   __in_opt const FILETIME* pctime,
                   __in_opt const FILETIME* patime,
                   __in_opt const FILETIME* pmtime);

__checkReturn WINOLEAPI StgOpenAsyncDocfileOnIFillLockBytes( __in IFillLockBytes *pflb,
             __in DWORD grfMode,
             __in DWORD asyncFlags,
             __deref_out IStorage** ppstgOpen);

__checkReturn WINOLEAPI StgGetIFillLockBytesOnILockBytes( __in ILockBytes *pilb,
             __deref_out IFillLockBytes** ppflb);

__checkReturn WINOLEAPI StgGetIFillLockBytesOnFile(__in __nullterminated OLECHAR const *pwcsName,
             __deref_out IFillLockBytes** ppflb);


__checkReturn WINOLEAPI StgOpenLayoutDocfile(__in __nullterminated OLECHAR const *pwcsDfName,
             __in DWORD grfMode,
             __in DWORD reserved,
             __deref_out IStorage** ppstgOpen);

// STG initialization options for StgCreateStorageEx and StgOpenStorageEx
#if _WIN32_WINNT == 0x500
#define STGOPTIONS_VERSION 1
#elif _WIN32_WINNT > 0x500
#define STGOPTIONS_VERSION 2
#else
#define STGOPTIONS_VERSION 0
#endif

typedef struct tagSTGOPTIONS
{
    USHORT usVersion;            // Versions 1 and 2 supported
    USHORT reserved;             // must be 0 for padding
    ULONG ulSectorSize;          // docfile header sector size (512)
#if STGOPTIONS_VERSION >= 2
    const WCHAR *pwcsTemplateFile;  // version 2 or above
#endif
} STGOPTIONS;

__checkReturn WINOLEAPI StgCreateStorageEx (__in_opt __nullterminated const WCHAR* pwcsName,
            __in DWORD grfMode,
            __in DWORD stgfmt,              // enum
            __in DWORD grfAttrs,
            __inout_opt STGOPTIONS* pStgOptions,
            __in_opt PSECURITY_DESCRIPTOR pSecurityDescriptor,
            __in REFIID riid,
            __deref_out void** ppObjectOpen);

__checkReturn WINOLEAPI StgOpenStorageEx (__in __nullterminated const WCHAR* pwcsName,
            __in DWORD grfMode,
            __in DWORD stgfmt,              // enum
            __in DWORD grfAttrs,
            __inout_opt STGOPTIONS* pStgOptions,
            __in_opt PSECURITY_DESCRIPTOR pSecurityDescriptor,
            __in REFIID riid,
            __deref_out void** ppObjectOpen);


//
//  Moniker APIs
//

__checkReturn WINOLEAPI  BindMoniker(__in LPMONIKER pmk, __in DWORD grfOpt, __in REFIID iidResult, __deref_out LPVOID FAR* ppvResult);

WINOLEAPI  CoInstall(
    __in IBindCtx     * pbc,
    __in DWORD          dwFlags,
    __in uCLSSPEC     * pClassSpec,
    __in QUERYCONTEXT * pQuery,
    __in LPWSTR         pszCodeBase);

__checkReturn WINOLEAPI  CoGetObject(__in LPCWSTR pszName, __in_opt BIND_OPTS *pBindOptions, __in REFIID riid, __deref_out void **ppv);
__checkReturn WINOLEAPI  MkParseDisplayName(__in LPBC pbc, __in LPCOLESTR szUserName,
                __out ULONG FAR * pchEaten, __deref_out LPMONIKER FAR * ppmk);
__checkReturn WINOLEAPI  MonikerRelativePathTo(__in LPMONIKER pmkSrc, __in LPMONIKER pmkDest, __deref_out LPMONIKER
                FAR* ppmkRelPath, __in BOOL dwReserved);
__checkReturn WINOLEAPI  MonikerCommonPrefixWith(__in LPMONIKER pmkThis, __in LPMONIKER pmkOther,
                __deref_out LPMONIKER FAR* ppmkCommon);
__checkReturn WINOLEAPI  CreateBindCtx(__in DWORD reserved, __deref_out LPBC FAR* ppbc);
__checkReturn WINOLEAPI  CreateGenericComposite(__in_opt LPMONIKER pmkFirst, __in_opt LPMONIKER pmkRest,
    __deref_out LPMONIKER FAR* ppmkComposite);
__checkReturn WINOLEAPI  GetClassFile (__in LPCOLESTR szFilename, __out CLSID FAR* pclsid);

__checkReturn WINOLEAPI  CreateClassMoniker(__in REFCLSID rclsid, __deref_out LPMONIKER FAR* ppmk);

__checkReturn WINOLEAPI  CreateFileMoniker(__in LPCOLESTR lpszPathName, __deref_out LPMONIKER FAR* ppmk);

__checkReturn WINOLEAPI  CreateItemMoniker(__in LPCOLESTR lpszDelim, __in LPCOLESTR lpszItem,
                          __deref_out LPMONIKER FAR* ppmk);
__checkReturn WINOLEAPI  CreateAntiMoniker(__deref_out LPMONIKER FAR* ppmk);
__checkReturn WINOLEAPI  CreatePointerMoniker(__in_opt LPUNKNOWN punk, __deref_out LPMONIKER FAR* ppmk);
__checkReturn WINOLEAPI  CreateObjrefMoniker(__in_opt LPUNKNOWN punk, __deref_out LPMONIKER FAR * ppmk);

__checkReturn WINOLEAPI  GetRunningObjectTable( __in DWORD reserved, __deref_out LPRUNNINGOBJECTTABLE FAR* pprot);

#include <urlmon.h>
#include <propidl.h>

//
// Standard Progress Indicator impolementation
//
WINOLEAPI CreateStdProgressIndicator(__in HWND hwndParent,
                                   __in LPCOLESTR pszTitle,
                                   __in IBindStatusCallback * pIbscCaller,
                                   __deref_out IBindStatusCallback ** ppIbsc);


#ifndef RC_INVOKED
#include <poppack.h>
#endif // RC_INVOKED

#endif     // __OBJBASE_H__

