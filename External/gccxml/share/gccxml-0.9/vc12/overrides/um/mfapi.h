#include <winapifamily.h>

//*@@@+++@@@@******************************************************************
//
// Microsoft Windows Media Foundation
// Copyright (C) Microsoft Corporation. All rights reserved.
//
//*@@@---@@@@******************************************************************
//

//
// MFAPI.h is the header containing the APIs for using the MF platform.
//

#pragma once
#if !defined(__MFAPI_H__)
#define __MFAPI_H__

#pragma pack(push, mfhrds)
#include <mfobjects.h>
#pragma pack(pop, mfhrds)

#include "mmreg.h"

#include <avrt.h>
#ifndef AVRT_DATA
#define AVRT_DATA
#endif
#ifndef AVRT_BSS
#define AVRT_BSS
#endif

#if !defined(MF_VERSION)

#if (WINVER >= _WIN32_WINNT_WIN7)

#define MF_SDK_VERSION 0x0002

#else // Vista

#define MF_SDK_VERSION 0x0001

#endif // (WINVER >= _WIN32_WINNT_WIN7)

#define MF_API_VERSION 0x0070 // This value is unused in the Win7 release and left at its Vista release value
#define MF_VERSION (MF_SDK_VERSION << 16 | MF_API_VERSION)

#endif //!defined(MF_VERSION)


#define MFSTARTUP_NOSOCKET 0x1
#define MFSTARTUP_LITE (MFSTARTUP_NOSOCKET)
#define MFSTARTUP_FULL 0

#if defined(__cplusplus)
extern "C" {
#endif

////////////////////////////////////////////////////////////////////////////////
///////////////////////////////   Startup/Shutdown  ////////////////////////////
////////////////////////////////////////////////////////////////////////////////

//
// Initializes the platform object.
// Must be called before using Media Foundation.
// A matching MFShutdown call must be made when the application is done using
// Media Foundation.
// The "Version" parameter should be set to MF_API_VERSION.
// Application should not call MFStartup / MFShutdown from workqueue threads
//
#if defined(__cplusplus)

#pragma region Application Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP)

STDAPI MFStartup( ULONG Version, DWORD dwFlags = MFSTARTUP_FULL );

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP) */
#pragma endregion

#else

#pragma region Application Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP)

STDAPI MFStartup( ULONG Version, DWORD dwFlags );

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP) */
#pragma endregion

#endif

#pragma region Application Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP)

//
// Shuts down the platform object.
// Releases all resources including threads.
// Application should call MFShutdown the same number of times as MFStartup
// Application should not call MFStartup / MFShutdown from workqueue threads
//
STDAPI MFShutdown();


////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////    Platform    ///////////////////////////////
////////////////////////////////////////////////////////////////////////////////

//
// These functions can be used to keep the MF platform object in place.
// Every call to MFLockPlatform should have a matching call to MFUnlockPlatform
//
STDAPI MFLockPlatform();
STDAPI MFUnlockPlatform();

///////////////////////////////////////////////////////////////////////////////

//
// MF workitem functions
//
typedef unsigned __int64 MFWORKITEM_KEY;

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP) */
#pragma endregion

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

STDAPI MFPutWorkItem(
            DWORD dwQueue,
            IMFAsyncCallback * pCallback,
            IUnknown * pState);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#pragma region Application Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP)

STDAPI MFPutWorkItem2(
            DWORD dwQueue,
            LONG Priority,
            _In_ IMFAsyncCallback * pCallback,
            _In_opt_ IUnknown * pState);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP) */
#pragma endregion

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

STDAPI MFPutWorkItemEx(
            DWORD dwQueue,
            IMFAsyncResult * pResult);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#pragma region Application Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP)

STDAPI MFPutWorkItemEx2(
            DWORD dwQueue,
            LONG Priority,
            _In_ IMFAsyncResult * pResult);

STDAPI MFPutWaitingWorkItem (
            HANDLE hEvent,
            LONG Priority,
            _In_ IMFAsyncResult * pResult,
            _Out_opt_ MFWORKITEM_KEY * pKey
            );

STDAPI MFAllocateSerialWorkQueue (
            _In_ DWORD dwWorkQueue,
            _Out_ OUT DWORD * pdwWorkQueue);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP) */
#pragma endregion

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

STDAPI MFScheduleWorkItem(
            IMFAsyncCallback * pCallback,
            IUnknown * pState,
            INT64 Timeout,
            _Out_opt_ MFWORKITEM_KEY * pKey);

STDAPI MFScheduleWorkItemEx(
            IMFAsyncResult * pResult,
            INT64 Timeout,
            _Out_opt_ MFWORKITEM_KEY * pKey);

//
//   The CancelWorkItem method is used by objects to cancel scheduled operation
//   Due to asynchronous nature of timers, application might still get a
//   timer callback after MFCancelWorkItem has returned.
//
#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#pragma region Application Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP)

STDAPI MFCancelWorkItem(
            MFWORKITEM_KEY Key);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP) */
#pragma endregion

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

///////////////////////////////////////////////////////////////////////////////

//
// MF periodic callbacks
//
STDAPI MFGetTimerPeriodicity(
            _Out_ DWORD * Periodicity);

typedef void (*MFPERIODICCALLBACK)(IUnknown* pContext);

STDAPI MFAddPeriodicCallback(
            MFPERIODICCALLBACK Callback,
            IUnknown * pContext,
            _Out_opt_ DWORD * pdwKey);

STDAPI MFRemovePeriodicCallback(
            DWORD dwKey);

///////////////////////////////////////////////////////////////////////////////

//
// MF work queues
//

#if (WINVER >= _WIN32_WINNT_WIN7)
//
// MFASYNC_WORKQUEUE_TYPE: types of work queue used by MFAllocateWorkQueueEx
//
typedef enum
{
    // MF_STANDARD_WORKQUEUE: Work queue in a thread without Window 
    // message loop.
    MF_STANDARD_WORKQUEUE = 0,

    // MF_WINDOW_WORKQUEUE: Work queue in a thread running Window 
    // Message loop that calls PeekMessage() / DispatchMessage()..
    MF_WINDOW_WORKQUEUE = 1,

    //
    //
    MF_MULTITHREADED_WORKQUEUE = 2, // common MT threadpool
}   MFASYNC_WORKQUEUE_TYPE;

STDAPI MFAllocateWorkQueueEx(
            _In_ MFASYNC_WORKQUEUE_TYPE WorkQueueType,
            _Out_ OUT DWORD * pdwWorkQueue);
#endif // (WINVER >= _WIN32_WINNT_WIN7)

//
// Allocate a standard work queue. the behaviour is the same with:
// MFAllocateWorkQueueEx( MF_STANDARD_WORKQUEUE, pdwWorkQueue )
//
STDAPI MFAllocateWorkQueue(
            _Out_ OUT DWORD * pdwWorkQueue);


#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#pragma region Application Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP)

STDAPI MFLockWorkQueue(
            _In_ DWORD dwWorkQueue);

STDAPI MFUnlockWorkQueue(
            _In_ DWORD dwWorkQueue);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP) */
#pragma endregion

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

STDAPI MFBeginRegisterWorkQueueWithMMCSS(
            DWORD dwWorkQueueId,
            _In_ LPCWSTR wszClass,
            DWORD dwTaskId,
            _In_ IMFAsyncCallback * pDoneCallback,
            _In_ IUnknown * pDoneState );

STDAPI MFBeginRegisterWorkQueueWithMMCSSEx(
            DWORD dwWorkQueueId,
            _In_ LPCWSTR wszClass,
            DWORD dwTaskId,
            LONG lPriority,
            _In_ IMFAsyncCallback * pDoneCallback,
            _In_ IUnknown * pDoneState );

STDAPI MFEndRegisterWorkQueueWithMMCSS(
            _In_ IMFAsyncResult * pResult,
            _Out_ DWORD * pdwTaskId );

STDAPI MFBeginUnregisterWorkQueueWithMMCSS(
            DWORD dwWorkQueueId,
            _In_ IMFAsyncCallback * pDoneCallback,
            _In_ IUnknown * pDoneState );

STDAPI MFEndUnregisterWorkQueueWithMMCSS(
            _In_ IMFAsyncResult * pResult );

STDAPI MFGetWorkQueueMMCSSClass(
            DWORD dwWorkQueueId,
            _Out_writes_to_opt_(*pcchClass,*pcchClass)  LPWSTR pwszClass,
            _Inout_  DWORD *pcchClass );

STDAPI MFGetWorkQueueMMCSSTaskId(
            DWORD dwWorkQueueId,
            _Out_ LPDWORD pdwTaskId );

STDAPI MFRegisterPlatformWithMMCSS(
    _In_ PCWSTR wszClass,
    _Inout_ DWORD* pdwTaskId,
    _In_ LONG lPriority );

STDAPI MFUnregisterPlatformFromMMCSS();

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#pragma region Application Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP)

STDAPI MFLockSharedWorkQueue(
    _In_ PCWSTR wszClass,
    _In_ LONG BasePriority,
    _Inout_ DWORD* pdwTaskId,
    _Out_ DWORD* pID );

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP) */
#pragma endregion

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

STDAPI MFGetWorkQueueMMCSSPriority(
            DWORD dwWorkQueueId,
            _Out_ LONG* lPriority );


///////////////////////////////////////////////////////////////////////////////
/////////////////////////////////    Async Model //////////////////////////////
///////////////////////////////////////////////////////////////////////////////

//
// Instantiates the MF-provided Async Result implementation
//
#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#pragma region Application Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP)

STDAPI MFCreateAsyncResult(
    IUnknown * punkObject,
    IMFAsyncCallback * pCallback,
    IUnknown * punkState,
    _Out_ IMFAsyncResult ** ppAsyncResult );

//
// Helper for calling IMFAsyncCallback::Invoke
//
STDAPI MFInvokeCallback(
    IMFAsyncResult * pAsyncResult );

//
// MFASYNCRESULT struct.
// Any implementation of IMFAsyncResult must inherit from this struct;
// the Media Foundation workqueue implementation depends on this.
//
#if defined(__cplusplus) && !defined(CINTERFACE)
typedef struct tagMFASYNCRESULT : public IMFAsyncResult
{
    OVERLAPPED overlapped;
    IMFAsyncCallback * pCallback;
    HRESULT hrStatusResult;
    DWORD dwBytesTransferred;
    HANDLE hEvent;
}   MFASYNCRESULT;
#else /* C style interface */
typedef struct tagMFASYNCRESULT
{
    IMFAsyncResult AsyncResult;
    OVERLAPPED overlapped;
    IMFAsyncCallback * pCallback;
    HRESULT hrStatusResult;
    DWORD dwBytesTransferred;
    HANDLE hEvent;
}   MFASYNCRESULT;
#endif /* C style interface */

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP) */
#pragma endregion

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

///////////////////////////////////////////////////////////////////////////////
/////////////////////////////////    Files       //////////////////////////////
///////////////////////////////////////////////////////////////////////////////

//
// Regardless of the access mode with which the file is opened, the sharing
// permissions will allow shared reading and deleting.
//
STDAPI MFCreateFile(
    MF_FILE_ACCESSMODE  AccessMode,
    MF_FILE_OPENMODE    OpenMode,
    MF_FILE_FLAGS       fFlags,
    LPCWSTR             pwszFileURL,
    _Out_ IMFByteStream       **ppIByteStream );

STDAPI MFCreateTempFile(
    MF_FILE_ACCESSMODE  AccessMode,
    MF_FILE_OPENMODE    OpenMode,
    MF_FILE_FLAGS       fFlags,
    _Out_ IMFByteStream       **ppIByteStream );

STDAPI MFBeginCreateFile(
    MF_FILE_ACCESSMODE  AccessMode,
    MF_FILE_OPENMODE    OpenMode,
    MF_FILE_FLAGS       fFlags,
    LPCWSTR             pwszFilePath,
    IMFAsyncCallback *  pCallback,
    IUnknown *          pState,
    _Out_ IUnknown ** ppCancelCookie);

STDAPI MFEndCreateFile(
    IMFAsyncResult * pResult,
    _Out_ IMFByteStream **ppFile );

STDAPI MFCancelCreateFile(
    IUnknown * pCancelCookie);


///////////////////////////////////////////////////////////////////////////////
/////////////////////////////////    Buffers     //////////////////////////////
///////////////////////////////////////////////////////////////////////////////

//
// Creates an IMFMediaBuffer in memory
//
#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#pragma region Application Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP)

STDAPI MFCreateMemoryBuffer(
    _In_ DWORD                      cbMaxLength,
    _Out_ IMFMediaBuffer **         ppBuffer );

//
// Creates an IMFMediaBuffer wrapper at the given offset and length
// within an existing IMFMediaBuffer
//
STDAPI MFCreateMediaBufferWrapper(
    _In_ IMFMediaBuffer *           pBuffer,
    _In_ DWORD                      cbOffset,
    _In_ DWORD                      dwLength,
    _Out_ IMFMediaBuffer **         ppBuffer );

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP) */
#pragma endregion

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

//
// Creates a legacy buffer (IMediaBuffer) wrapper at the given offset within
// an existing IMFMediaBuffer.
// pSample is optional.  It can point to the original IMFSample from which this
// IMFMediaBuffer came.  If provided, then *ppMediaBuffer will succeed
// QueryInterface for IID_IMFSample, from which the original sample's attributes
// can be obtained
//
STDAPI MFCreateLegacyMediaBufferOnMFMediaBuffer(
    _In_opt_ IMFSample *            pSample,
    _In_ IMFMediaBuffer *           pMFMediaBuffer,
    _In_ DWORD                      cbOffset,
    _Outptr_ IMediaBuffer **     ppMediaBuffer );

//
// Create a DirectX surface buffer
//
#include <dxgiformat.h>
STDAPI_(DXGI_FORMAT) MFMapDX9FormatToDXGIFormat( _In_ DWORD dx9 );
STDAPI_(DWORD) MFMapDXGIFormatToDX9Format( _In_ DXGI_FORMAT dx11 );

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#pragma region Application Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP)

STDAPI MFLockDXGIDeviceManager(
    _Out_opt_ UINT* pResetToken,
    _Outptr_ IMFDXGIDeviceManager** ppManager
    );

STDAPI MFUnlockDXGIDeviceManager();
#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP) */
#pragma endregion

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

STDAPI MFCreateDXSurfaceBuffer(
    _In_ REFIID                     riid,
    _In_ IUnknown *                 punkSurface,
    _In_ BOOL                       fBottomUpWhenLinear,
    _Outptr_ IMFMediaBuffer **   ppBuffer );

STDAPI MFCreateWICBitmapBuffer(
    _In_ REFIID                     riid,
    _In_ IUnknown *                 punkSurface,
    _Outptr_ IMFMediaBuffer **   ppBuffer
    );

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#pragma region Application Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP)

STDAPI
MFCreateDXGISurfaceBuffer(
    _In_ REFIID riid,
    _In_ IUnknown* punkSurface,
    _In_ UINT uSubresourceIndex,
    _In_ BOOL fBottomUpWhenLinear,
    _Outptr_ IMFMediaBuffer** ppBuffer
    );

STDAPI MFCreateVideoSampleAllocatorEx(
    _In_   REFIID riid,
    _Outptr_  void** ppSampleAllocator
    );

STDAPI
MFCreateDXGIDeviceManager(
    _Out_ UINT* resetToken,
    _Outptr_ IMFDXGIDeviceManager** ppDeviceManager
    );

#define MF_E_DXGI_DEVICE_NOT_INITIALIZED ((HRESULT)0x80041000L)  // DXVA2_E_NOT_INITIALIZED     
#define MF_E_DXGI_NEW_VIDEO_DEVICE       ((HRESULT)0x80041001L)  // DXVA2_E_NEW_VIDEO_DEVICE    
#define MF_E_DXGI_VIDEO_DEVICE_LOCKED    ((HRESULT)0x80041002L)  // DXVA2_E_VIDEO_DEVICE_LOCKED 

//
// Create an aligned memory buffer.
// The following constants were chosen for parity with the alignment constants
// in ntioapi.h
// 
#define MF_1_BYTE_ALIGNMENT       0x00000000 
#define MF_2_BYTE_ALIGNMENT       0x00000001
#define MF_4_BYTE_ALIGNMENT       0x00000003
#define MF_8_BYTE_ALIGNMENT       0x00000007 
#define MF_16_BYTE_ALIGNMENT      0x0000000f
#define MF_32_BYTE_ALIGNMENT      0x0000001f
#define MF_64_BYTE_ALIGNMENT      0x0000003f
#define MF_128_BYTE_ALIGNMENT     0x0000007f
#define MF_256_BYTE_ALIGNMENT     0x000000ff
#define MF_512_BYTE_ALIGNMENT     0x000001ff

STDAPI MFCreateAlignedMemoryBuffer(
    _In_ DWORD                      cbMaxLength,
    _In_ DWORD                      cbAligment, 
    _Out_ IMFMediaBuffer **         ppBuffer );

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP) */
#pragma endregion

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

//
// This GUID is used in IMFGetService::GetService calls to retrieve 
// interfaces from the buffer.  Its value is defined in evr.h
// 
EXTERN_C const GUID MR_BUFFER_SERVICE;

///////////////////////////////////////////////////////////////////////////////
/////////////////////////////////    Events      //////////////////////////////
///////////////////////////////////////////////////////////////////////////////

//
// Instantiates the MF-provided Media Event implementation.
//

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#pragma region Application Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP)

STDAPI MFCreateMediaEvent(
    _In_ MediaEventType met,
    _In_ REFGUID guidExtendedType,
    _In_ HRESULT hrStatus,
    _In_opt_ const PROPVARIANT * pvValue,
    _Out_ IMFMediaEvent ** ppEvent );

//
// Instantiates an object that implements IMFMediaEventQueue.
// Components that provide an IMFMediaEventGenerator can use this object
// internally to do their Media Event Generator work for them.
// IMFMediaEventGenerator calls should be forwarded to the similar call
// on this object's IMFMediaEventQueue interface (e.g. BeginGetEvent,
// EndGetEvent), and the various IMFMediaEventQueue::QueueEventXXX methods
// can be used to queue events that the caller will consume.
//
STDAPI MFCreateEventQueue(
    _Out_ IMFMediaEventQueue **ppMediaEventQueue );

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP) */
#pragma endregion

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

//
// Event attributes
// Some of the common Media Foundation events have associated attributes
// that go in their IMFAttributes stores
//

//
// MESessionCapabilitiesChanged attributes
//

// MF_EVENT_SESSIONCAPS {7E5EBCD0-11B8-4abe-AFAD-10F6599A7F42}
// Type: UINT32
DEFINE_GUID(MF_EVENT_SESSIONCAPS,
0x7e5ebcd0, 0x11b8, 0x4abe, 0xaf, 0xad, 0x10, 0xf6, 0x59, 0x9a, 0x7f, 0x42);

// MF_EVENT_SESSIONCAPS_DELTA {7E5EBCD1-11B8-4abe-AFAD-10F6599A7F42}
// Type: UINT32
DEFINE_GUID(MF_EVENT_SESSIONCAPS_DELTA,
0x7e5ebcd1, 0x11b8, 0x4abe, 0xaf, 0xad, 0x10, 0xf6, 0x59, 0x9a, 0x7f, 0x42);

// Session capabilities bitflags
#define MFSESSIONCAP_START                  0x00000001
#define MFSESSIONCAP_SEEK                   0x00000002
#define MFSESSIONCAP_PAUSE                  0x00000004
#define MFSESSIONCAP_RATE_FORWARD           0x00000010
#define MFSESSIONCAP_RATE_REVERSE           0x00000020
#define MFSESSIONCAP_DOES_NOT_USE_NETWORK   0x00000040

//
// MESessionTopologyStatus attributes
//

// Possible values for MF_EVENT_TOPOLOGY_STATUS attribute.
//
// For a given topology, these status values will arrive via
// MESessionTopologyStatus in the order below.
//
// However, there are no guarantees about how these status values will be
// ordered between two consecutive topologies.  For example,
// MF_TOPOSTATUS_READY could arrive for topology n+1 before
// MF_TOPOSTATUS_ENDED arrives for topology n if the application called
// IMFMediaSession::SetTopology for topology n+1 well enough in advance of the
// end of topology n.  Conversely, if topology n ends before the application
// calls IMFMediaSession::SetTopology for topology n+1, then
// MF_TOPOSTATUS_ENDED will arrive for topology n before MF_TOPOSTATUS_READY
// arrives for topology n+1.
typedef enum
{
    // MF_TOPOSTATUS_INVALID: Invalid value; will not be sent
    MF_TOPOSTATUS_INVALID = 0,

    // MF_TOPOSTATUS_READY: The topology has been put in place and is
    // ready to start.  All GetService calls to the Media Session will use
    // this topology.
    MF_TOPOSTATUS_READY     = 100,

    // MF_TOPOSTATUS_STARTED_SOURCE: The Media Session has started to read
    // and process data from the Media Source(s) in this topology.
    MF_TOPOSTATUS_STARTED_SOURCE = 200,


#if (WINVER >= _WIN32_WINNT_WIN7)
    // MF_TOPOSTATUS_DYNAMIC_CHANGED: The topology has been dynamic changed
    // due to the format change.
    MF_TOPOSTATUS_DYNAMIC_CHANGED = 210,
#endif // (WINVER >= _WIN32_WINNT_WIN7) 

    // MF_TOPOSTATUS_SINK_SWITCHED: The Media Sinks in the pipeline have
    // switched from a previous topology to this topology.
    // Note that this status does not get sent for the first topology;
    // applications can assume that the sinks are playing the first
    // topology when they receive MESessionStarted.
    MF_TOPOSTATUS_SINK_SWITCHED = 300,
    
    // MF_TOPOSTATUS_ENDED: Playback of this topology is complete.
    // Before deleting this topology, however, the application should wait
    // for either MESessionEnded or the MF_TOPOSTATUS_STARTED_SOURCE status
    // on the next topology to ensure that the Media Session is no longer
    // using this topology.
    MF_TOPOSTATUS_ENDED = 400,

}   MF_TOPOSTATUS;

// MF_EVENT_TOPOLOGY_STATUS {30C5018D-9A53-454b-AD9E-6D5F8FA7C43B}
// Type: UINT32 {MF_TOPOLOGY_STATUS}
DEFINE_GUID(MF_EVENT_TOPOLOGY_STATUS,
0x30c5018d, 0x9a53, 0x454b, 0xad, 0x9e, 0x6d, 0x5f, 0x8f, 0xa7, 0xc4, 0x3b);

//
// MESessionNotifyPresentationTime attributes
//

// MF_EVENT_START_PRESENTATION_TIME {5AD914D0-9B45-4a8d-A2C0-81D1E50BFB07}
// Type: UINT64
DEFINE_GUID(MF_EVENT_START_PRESENTATION_TIME,
0x5ad914d0, 0x9b45, 0x4a8d, 0xa2, 0xc0, 0x81, 0xd1, 0xe5, 0xb, 0xfb, 0x7);

// MF_EVENT_PRESENTATION_TIME_OFFSET {5AD914D1-9B45-4a8d-A2C0-81D1E50BFB07}
// Type: UINT64
DEFINE_GUID(MF_EVENT_PRESENTATION_TIME_OFFSET,
0x5ad914d1, 0x9b45, 0x4a8d, 0xa2, 0xc0, 0x81, 0xd1, 0xe5, 0xb, 0xfb, 0x7);

// MF_EVENT_START_PRESENTATION_TIME_AT_OUTPUT {5AD914D2-9B45-4a8d-A2C0-81D1E50BFB07}
// Type: UINT64
DEFINE_GUID(MF_EVENT_START_PRESENTATION_TIME_AT_OUTPUT,
0x5ad914d2, 0x9b45, 0x4a8d, 0xa2, 0xc0, 0x81, 0xd1, 0xe5, 0xb, 0xfb, 0x7);

//

//
// MESourceStarted attributes
//

// MF_EVENT_SOURCE_FAKE_START {a8cc55a7-6b31-419f-845d-ffb351a2434b}
// Type: UINT32
DEFINE_GUID(MF_EVENT_SOURCE_FAKE_START,
0xa8cc55a7, 0x6b31, 0x419f, 0x84, 0x5d, 0xff, 0xb3, 0x51, 0xa2, 0x43, 0x4b);

// MF_EVENT_SOURCE_PROJECTSTART {a8cc55a8-6b31-419f-845d-ffb351a2434b}
// Type: UINT64
DEFINE_GUID(MF_EVENT_SOURCE_PROJECTSTART,
0xa8cc55a8, 0x6b31, 0x419f, 0x84, 0x5d, 0xff, 0xb3, 0x51, 0xa2, 0x43, 0x4b);

// MF_EVENT_SOURCE_ACTUAL_START {a8cc55a9-6b31-419f-845d-ffb351a2434b}
// Type: UINT64
DEFINE_GUID(MF_EVENT_SOURCE_ACTUAL_START,
0xa8cc55a9, 0x6b31, 0x419f, 0x84, 0x5d, 0xff, 0xb3, 0x51, 0xa2, 0x43, 0x4b);

//
// MEEndOfPresentationSegment attributes
//

// MF_EVENT_SOURCE_TOPOLOGY_CANCELED {DB62F650-9A5E-4704-ACF3-563BC6A73364}
// Type: UINT32
DEFINE_GUID(MF_EVENT_SOURCE_TOPOLOGY_CANCELED,
0xdb62f650, 0x9a5e, 0x4704, 0xac, 0xf3, 0x56, 0x3b, 0xc6, 0xa7, 0x33, 0x64);

//
// MESourceCharacteristicsChanged attributes
//

// MF_EVENT_SOURCE_CHARACTERISTICS {47DB8490-8B22-4f52-AFDA-9CE1B2D3CFA8}
// Type: UINT32
DEFINE_GUID(MF_EVENT_SOURCE_CHARACTERISTICS,
0x47db8490, 0x8b22, 0x4f52, 0xaf, 0xda, 0x9c, 0xe1, 0xb2, 0xd3, 0xcf, 0xa8);

// MF_EVENT_SOURCE_CHARACTERISTICS_OLD {47DB8491-8B22-4f52-AFDA-9CE1B2D3CFA8}
// Type: UINT32
DEFINE_GUID(MF_EVENT_SOURCE_CHARACTERISTICS_OLD,
0x47db8491, 0x8b22, 0x4f52, 0xaf, 0xda, 0x9c, 0xe1, 0xb2, 0xd3, 0xcf, 0xa8);

//
// MESourceRateChangeRequested attributes
//

// MF_EVENT_DO_THINNING {321EA6FB-DAD9-46e4-B31D-D2EAE7090E30}
// Type: UINT32
DEFINE_GUID(MF_EVENT_DO_THINNING,
0x321ea6fb, 0xdad9, 0x46e4, 0xb3, 0x1d, 0xd2, 0xea, 0xe7, 0x9, 0xe, 0x30);

//
// MEStreamSinkScrubSampleComplete attributes
//

// MF_EVENT_SCRUBSAMPLE_TIME {9AC712B3-DCB8-44d5-8D0C-37455A2782E3}
// Type: UINT64
DEFINE_GUID(MF_EVENT_SCRUBSAMPLE_TIME,
0x9ac712b3, 0xdcb8, 0x44d5, 0x8d, 0xc, 0x37, 0x45, 0x5a, 0x27, 0x82, 0xe3);

//
// MESinkInvalidated and MESessionStreamSinkFormatChanged attributes
//

// MF_EVENT_OUTPUT_NODE {830f1a8b-c060-46dd-a801-1c95dec9b107}
// Type: UINT64
DEFINE_GUID(MF_EVENT_OUTPUT_NODE,
0x830f1a8b, 0xc060, 0x46dd, 0xa8, 0x01, 0x1c, 0x95, 0xde, 0xc9, 0xb1, 0x07);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#pragma region Application Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP)

#if (WINVER >= _WIN32_WINNT_WIN7)
//
// METransformNeedInput attributes
// 

// MF_EVENT_MFT_INPUT_STREAM_ID {F29C2CCA-7AE6-42d2-B284-BF837CC874E2}
// Type: UINT32
DEFINE_GUID(MF_EVENT_MFT_INPUT_STREAM_ID, 
0xf29c2cca, 0x7ae6, 0x42d2, 0xb2, 0x84, 0xbf, 0x83, 0x7c, 0xc8, 0x74, 0xe2);

//
// METransformDrainComplete and METransformMarker attributes
//

// MF_EVENT_MFT_CONTEXT {B7CD31F1-899E-4b41-80C9-26A896D32977}
// Type: UINT64
DEFINE_GUID(MF_EVENT_MFT_CONTEXT, 
0xb7cd31f1, 0x899e, 0x4b41, 0x80, 0xc9, 0x26, 0xa8, 0x96, 0xd3, 0x29, 0x77);

#endif // (WINVER >= _WIN32_WINNT_WIN7)

#if (WINVER >= _WIN32_WINNT_WINBLUE)
//
// MEContentProtectionMetadata attributes
// 

// MF_EVENT_STREAM_METADATA_KEYDATA {CD59A4A1-4A3B-4BBD-8665-72A40FBEA776}
// Type: BLOB
DEFINE_GUID(MF_EVENT_STREAM_METADATA_KEYDATA, 
0xcd59a4a1, 0x4a3b, 0x4bbd, 0x86, 0x65, 0x72, 0xa4, 0xf, 0xbe, 0xa7, 0x76);

// MF_EVENT_STREAM_METADATA_CONTENT_KEYIDS {5063449D-CC29-4FC6-A75A-D247B35AF85C}
// Type: BLOB
DEFINE_GUID(MF_EVENT_STREAM_METADATA_CONTENT_KEYIDS, 
0x5063449d, 0xcc29, 0x4fc6, 0xa7, 0x5a, 0xd2, 0x47, 0xb3, 0x5a, 0xf8, 0x5c);

// MF_EVENT_STREAM_METADATA_SYSTEMID {1EA2EF64-BA16-4A36-8719-FE7560BA32AD}
// Type: BLOB
DEFINE_GUID(MF_EVENT_STREAM_METADATA_SYSTEMID, 
0x1ea2ef64, 0xba16, 0x4a36, 0x87, 0x19, 0xfe, 0x75, 0x60, 0xba, 0x32, 0xad);

#endif // (WINVER >= _WIN32_WINNT_WINBLUE)

////////////////////////////////////////////////////////////////////////////////
///////////////////////////////  Samples  //////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

//
// Creates an instance of the Media Foundation implementation of IMFSample
//



STDAPI MFCreateSample( _Out_ IMFSample **ppIMFSample );


//
// Sample attributes
// These are the well-known attributes that can be present on an MF Sample's
// IMFAttributes store
//

// MFSampleExtension_CleanPoint {9cdf01d8-a0f0-43ba-b077-eaa06cbd728a}
// Type: UINT32
// If present and nonzero, indicates that the sample is a clean point (key
// frame), and decoding can begin at this sample.
DEFINE_GUID(MFSampleExtension_CleanPoint,
0x9cdf01d8, 0xa0f0, 0x43ba, 0xb0, 0x77, 0xea, 0xa0, 0x6c, 0xbd, 0x72, 0x8a);

// MFSampleExtension_Discontinuity {9cdf01d9-a0f0-43ba-b077-eaa06cbd728a}
// Type: UINT32
// If present and nonzero, indicates that the sample data represents the first
// sample following a discontinuity (gap) in the stream of samples.
// This can happen, for instance, if the previous sample was lost in
// transmission.
DEFINE_GUID(MFSampleExtension_Discontinuity,
0x9cdf01d9, 0xa0f0, 0x43ba, 0xb0, 0x77, 0xea, 0xa0, 0x6c, 0xbd, 0x72, 0x8a);

// MFSampleExtension_Token {8294da66-f328-4805-b551-00deb4c57a61}
// Type: IUNKNOWN
// When an IMFMediaStream delivers a sample via MEMediaStream, this attribute
// should be set to the IUnknown *pToken argument that was passed with the
// IMFMediaStream::RequestSample call to which this sample corresponds.
DEFINE_GUID(MFSampleExtension_Token,
0x8294da66, 0xf328, 0x4805, 0xb5, 0x51, 0x00, 0xde, 0xb4, 0xc5, 0x7a, 0x61);

// MFSampleExtension_DecodeTimestamp {73A954D4-09E2-4861-BEFC-94BD97C08E6E}
// Type : UINT64
// If present, contains the DTS (Decoding Time Stamp) of the sample.
DEFINE_GUID(MFSampleExtension_DecodeTimestamp,
0x73a954d4, 0x9e2, 0x4861, 0xbe, 0xfc, 0x94, 0xbd, 0x97, 0xc0, 0x8e, 0x6e);

// MFSampleExtension_VideoEncodeQP {B2EFE478-F979-4C66-B95E-EE2B82C82F36}
// Type: UINT64
// Used by video encoders to specify the QP used to encode the output sample.
DEFINE_GUID(MFSampleExtension_VideoEncodeQP,
0xb2efe478, 0xf979, 0x4c66, 0xb9, 0x5e, 0xee, 0x2b, 0x82, 0xc8, 0x2f, 0x36);

// MFSampleExtension_VideoEncPictureType {973704E6-CD14-483C-8F20-C9FC0928BAD5}
// Type: UINT32
// Used by video encoders to specify the output sample's picture type.
DEFINE_GUID(MFSampleExtension_VideoEncodePictureType,
0x973704e6, 0xcd14, 0x483c, 0x8f, 0x20, 0xc9, 0xfc, 0x9, 0x28, 0xba, 0xd5);

// MFSampleExtension_FrameCorruption {B4DD4A8C-0BEB-44C4-8B75-B02B913B04F0}
// Type: UINT32
// Indicates whether the frame in the sample has corruption or not
// value 0 indicates that there is no corruption, or it is unknown
// Value 1 indicates that some corruption was detected e.g, during decoding
DEFINE_GUID(MFSampleExtension_FrameCorruption,
0xb4dd4a8c, 0xbeb, 0x44c4, 0x8b, 0x75, 0xb0, 0x2b, 0x91, 0x3b, 0x4, 0xf0);

/////////////////////////////////////////////////////////////////////////////
//
// The following sample attributes are used for encrypted samples
//
/////////////////////////////////////////////////////////////////////////////

// MFSampleExtension_DescrambleData {43483BE6-4903-4314-B032-2951365936FC}
// Type: UINT64
DEFINE_GUID(MFSampleExtension_DescrambleData,
0x43483be6, 0x4903, 0x4314, 0xb0, 0x32, 0x29, 0x51, 0x36, 0x59, 0x36, 0xfc);

// MFSampleExtension_SampleKeyID {9ED713C8-9B87-4B26-8297-A93B0C5A8ACC}
// Type: UINT32
DEFINE_GUID(MFSampleExtension_SampleKeyID,
0x9ed713c8, 0x9b87, 0x4b26, 0x82, 0x97, 0xa9, 0x3b, 0x0c, 0x5a, 0x8a, 0xcc);

// MFSampleExtension_GenKeyFunc {441CA1EE-6B1F-4501-903A-DE87DF42F6ED}
// Type: UINT64
DEFINE_GUID(MFSampleExtension_GenKeyFunc,
0x441ca1ee, 0x6b1f, 0x4501, 0x90, 0x3a, 0xde, 0x87, 0xdf, 0x42, 0xf6, 0xed);

// MFSampleExtension_GenKeyCtx {188120CB-D7DA-4B59-9B3E-9252FD37301C}
// Type: UINT64
DEFINE_GUID(MFSampleExtension_GenKeyCtx,
0x188120cb, 0xd7da, 0x4b59, 0x9b, 0x3e, 0x92, 0x52, 0xfd, 0x37, 0x30, 0x1c);

// MFSampleExtension_PacketCrossOffsets {2789671D-389F-40BB-90D9-C282F77F9ABD}
// Type: BLOB
DEFINE_GUID(MFSampleExtension_PacketCrossOffsets,
0x2789671d, 0x389f, 0x40bb, 0x90, 0xd9, 0xc2, 0x82, 0xf7, 0x7f, 0x9a, 0xbd);

// MFSampleExtension_Encryption_SampleID {6698B84E-0AFA-4330-AEB2-1C0A98D7A44D}
// Type: UINT64
DEFINE_GUID(MFSampleExtension_Encryption_SampleID,
0x6698b84e, 0x0afa, 0x4330, 0xae, 0xb2, 0x1c, 0x0a, 0x98, 0xd7, 0xa4, 0x4d);

// MFSampleExtension_Encryption_KeyID {76376591-795F-4DA1-86ED-9D46ECA109A9}
// Type: BLOB
DEFINE_GUID(MFSampleExtension_Encryption_KeyID,
0x76376591, 0x795f, 0x4da1, 0x86, 0xed, 0x9d, 0x46, 0xec, 0xa1, 0x09, 0xa9);

// MFSampleExtension_Content_KeyID {C6C7F5B0-ACCA-415B-87D9-10441469EFC6}
// Type: GUID
DEFINE_GUID(MFSampleExtension_Content_KeyID,
0xc6c7f5b0, 0xacca, 0x415b, 0x87, 0xd9, 0x10, 0x44, 0x14, 0x69, 0xef, 0xc6);

// MFSampleExtension_Encryption_SubSampleMappingSplit {FE0254B9-2AA5-4EDC-99F7-17E89DBF9174}
// Type: BLOB
DEFINE_GUID(MFSampleExtension_Encryption_SubSampleMappingSplit,
0xfe0254b9, 0x2aa5, 0x4edc, 0x99, 0xf7, 0x17, 0xe8, 0x9d, 0xbf, 0x91, 0x74);

/////////////////////////////////////////////////////////////////////////////
//
// MFSample STANDARD EXTENSION ATTRIBUTE GUIDs
//
/////////////////////////////////////////////////////////////////////////////

// {b1d5830a-deb8-40e3-90fa-389943716461}   MFSampleExtension_Interlaced                {UINT32 (BOOL)}
DEFINE_GUID(MFSampleExtension_Interlaced,
0xb1d5830a, 0xdeb8, 0x40e3, 0x90, 0xfa, 0x38, 0x99, 0x43, 0x71, 0x64, 0x61);

// {941ce0a3-6ae3-4dda-9a08-a64298340617}   MFSampleExtension_BottomFieldFirst          {UINT32 (BOOL)}
DEFINE_GUID(MFSampleExtension_BottomFieldFirst,
0x941ce0a3, 0x6ae3, 0x4dda, 0x9a, 0x08, 0xa6, 0x42, 0x98, 0x34, 0x06, 0x17);

// {304d257c-7493-4fbd-b149-9228de8d9a99}   MFSampleExtension_RepeatFirstField          {UINT32 (BOOL)}
DEFINE_GUID(MFSampleExtension_RepeatFirstField,
0x304d257c, 0x7493, 0x4fbd, 0xb1, 0x49, 0x92, 0x28, 0xde, 0x8d, 0x9a, 0x99);

// {9d85f816-658b-455a-bde0-9fa7e15ab8f9}   MFSampleExtension_SingleField               {UINT32 (BOOL)}
DEFINE_GUID(MFSampleExtension_SingleField,
0x9d85f816, 0x658b, 0x455a, 0xbd, 0xe0, 0x9f, 0xa7, 0xe1, 0x5a, 0xb8, 0xf9);

// {6852465a-ae1c-4553-8e9b-c3420fcb1637}   MFSampleExtension_DerivedFromTopField       {UINT32 (BOOL)}
DEFINE_GUID(MFSampleExtension_DerivedFromTopField,
0x6852465a, 0xae1c, 0x4553, 0x8e, 0x9b, 0xc3, 0x42, 0x0f, 0xcb, 0x16, 0x37);

// MFSampleExtension_MeanAbsoluteDifference {1cdbde11-08b4-4311-a6dd-0f9f371907aa}
// Type: UINT32
DEFINE_GUID(MFSampleExtension_MeanAbsoluteDifference,
0x1cdbde11, 0x08b4, 0x4311, 0xa6, 0xdd, 0x0f, 0x9f, 0x37, 0x19, 0x07, 0xaa);

// MFSampleExtension_LongTermReferenceFrameInfo {9154733f-e1bd-41bf-81d3-fcd918f71332}
// Type: UINT32
DEFINE_GUID(MFSampleExtension_LongTermReferenceFrameInfo,
0x9154733f, 0xe1bd, 0x41bf, 0x81, 0xd3, 0xfc, 0xd9, 0x18, 0xf7, 0x13, 0x32);

// MFSampleExtension_ROIRectangle {3414a438-4998-4d2c-be82-be3ca0b24d43}
// Type: BLOB
DEFINE_GUID(MFSampleExtension_ROIRectangle,
0x3414a438, 0x4998, 0x4d2c, 0xbe, 0x82, 0xbe, 0x3c, 0xa0, 0xb2, 0x4d, 0x43);

typedef struct _ROI_AREA {
  RECT rect;
  INT32 QPDelta;
} ROI_AREA, *PROI_AREA;


///////////////////////////////////////////////////////////////////////////////
/// These are the attribute GUIDs that need to be used by MFT0 to provide
/// thumbnail support.  We are declaring these in our internal idl first and
/// once we pass API spec review, we can move it to the public header.
///////////////////////////////////////////////////////////////////////////////
// MFSampleExtension_PhotoThumbnail
// {74BBC85C-C8BB-42DC-B586DA17FFD35DCC}
// Type: IUnknown
// If this attribute is set on the IMFSample provided by the MFT0, this will contain the IMFMediaBuffer which contains
// the Photo Thumbnail as configured using the KSPROPERTYSETID_ExtendedCameraControl.
DEFINE_GUID(MFSampleExtension_PhotoThumbnail, 
0x74BBC85C, 0xC8BB, 0x42DC, 0xB5, 0x86, 0xDA, 0x17, 0xFF, 0xD3, 0x5D, 0xCC);

// MFSampleExtension_PhotoThumbnailMediaType
// {61AD5420-EBF8-4143-89AF6BF25F672DEF}
// Type: IUnknown
// This attribute will contain the IMFMediaType which describes the image format type contained in the 
// MFSampleExtension_PhotoThumbnail attribute.  If the MFSampleExtension_PhotoThumbnail attribute
// is present on the photo sample, the MFSampleExtension_PhotoThumbnailMediaType is required.
DEFINE_GUID(MFSampleExtension_PhotoThumbnailMediaType, 
0x61AD5420, 0xEBF8, 0x4143, 0x89, 0xAF, 0x6B, 0xF2, 0x5F, 0x67, 0x2D, 0xEF);

// MFSampleExtension_CaptureMetadata
// Type: IUnknown (IMFAttributes)
// This is the IMFAttributes store for all the metadata related to the capture
// pipeline.  It can be potentially present on any IMFSample.
DEFINE_GUID(MFSampleExtension_CaptureMetadata,
0x2EBE23A8, 0xFAF5, 0x444A, 0xA6, 0xA2, 0xEB, 0x81, 0x08, 0x80, 0xAB, 0x5D);

// Put all MF_CAPTURE_METADATA_* here.
// {0F9DD6C6-6003-45D8-BD59-F1F53E3D04E8}   MF_CAPTURE_METADATA_PHOTO_FRAME_FLASH       {UINT32}
// 0 - No flash triggered on this frame.
// non-0 - Flash triggered on this frame.
// Do not explicitly check for a value of 1 here, we may overload this to
// indicate special types of flash going forward (applications should only
// check for != 0 to indicate flash took place).
DEFINE_GUID(MF_CAPTURE_METADATA_PHOTO_FRAME_FLASH,  
0x0F9DD6C6, 0x6003, 0x45D8, 0xBD, 0x59, 0xF1, 0xF5, 0x3E, 0x3D, 0x04, 0xE8);  


///////////////////////////////////////////////////////////////////////////////////////////////////////////////  Attributes ////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////


STDAPI
MFCreateAttributes(
    _Out_   IMFAttributes** ppMFAttributes,
    _In_    UINT32          cInitialSize
    );

STDAPI
MFInitAttributesFromBlob(
    _In_                    IMFAttributes*  pAttributes,
    _In_reads_bytes_(cbBufSize)  const UINT8*    pBuf,
    _In_                    UINT            cbBufSize
    );

STDAPI
MFGetAttributesAsBlobSize(
    _In_    IMFAttributes*  pAttributes,
    _Out_   UINT32*         pcbBufSize
    );

STDAPI
MFGetAttributesAsBlob(
    _In_                    IMFAttributes*  pAttributes,
    _Out_writes_bytes_(cbBufSize) UINT8*          pBuf,
    _In_                    UINT            cbBufSize
    );


///////////////////////////////////////////////////////////////////////////////////////////////////////////////  MFT Register & Enum ////////////////////////////
////////////////////////////////////////////////////////////////////////////////

//
// MFT Registry categories
//

#ifdef MF_INIT_GUIDS
#include <initguid.h>
#endif

// {d6c02d4b-6833-45b4-971a-05a4b04bab91}   MFT_CATEGORY_VIDEO_DECODER
DEFINE_GUID(MFT_CATEGORY_VIDEO_DECODER,
0xd6c02d4b, 0x6833, 0x45b4, 0x97, 0x1a, 0x05, 0xa4, 0xb0, 0x4b, 0xab, 0x91);

// {f79eac7d-e545-4387-bdee-d647d7bde42a}   MFT_CATEGORY_VIDEO_ENCODER
DEFINE_GUID(MFT_CATEGORY_VIDEO_ENCODER,
0xf79eac7d, 0xe545, 0x4387, 0xbd, 0xee, 0xd6, 0x47, 0xd7, 0xbd, 0xe4, 0x2a);

// {12e17c21-532c-4a6e-8a1c-40825a736397}   MFT_CATEGORY_VIDEO_EFFECT
DEFINE_GUID(MFT_CATEGORY_VIDEO_EFFECT,
0x12e17c21, 0x532c, 0x4a6e, 0x8a, 0x1c, 0x40, 0x82, 0x5a, 0x73, 0x63, 0x97);

// {059c561e-05ae-4b61-b69d-55b61ee54a7b}   MFT_CATEGORY_MULTIPLEXER
DEFINE_GUID(MFT_CATEGORY_MULTIPLEXER,
0x059c561e, 0x05ae, 0x4b61, 0xb6, 0x9d, 0x55, 0xb6, 0x1e, 0xe5, 0x4a, 0x7b);

// {a8700a7a-939b-44c5-99d7-76226b23b3f1}   MFT_CATEGORY_DEMULTIPLEXER
DEFINE_GUID(MFT_CATEGORY_DEMULTIPLEXER,
0xa8700a7a, 0x939b, 0x44c5, 0x99, 0xd7, 0x76, 0x22, 0x6b, 0x23, 0xb3, 0xf1);

// {9ea73fb4-ef7a-4559-8d5d-719d8f0426c7}   MFT_CATEGORY_AUDIO_DECODER
DEFINE_GUID(MFT_CATEGORY_AUDIO_DECODER,
0x9ea73fb4, 0xef7a, 0x4559, 0x8d, 0x5d, 0x71, 0x9d, 0x8f, 0x04, 0x26, 0xc7);

// {91c64bd0-f91e-4d8c-9276-db248279d975}   MFT_CATEGORY_AUDIO_ENCODER
DEFINE_GUID(MFT_CATEGORY_AUDIO_ENCODER,
0x91c64bd0, 0xf91e, 0x4d8c, 0x92, 0x76, 0xdb, 0x24, 0x82, 0x79, 0xd9, 0x75);

// {11064c48-3648-4ed0-932e-05ce8ac811b7}   MFT_CATEGORY_AUDIO_EFFECT
DEFINE_GUID(MFT_CATEGORY_AUDIO_EFFECT,
0x11064c48, 0x3648, 0x4ed0, 0x93, 0x2e, 0x05, 0xce, 0x8a, 0xc8, 0x11, 0xb7);

#if (WINVER >= _WIN32_WINNT_WIN7)
// {302EA3FC-AA5F-47f9-9F7A-C2188BB163021}...MFT_CATEGORY_VIDEO_PROCESSOR
DEFINE_GUID(MFT_CATEGORY_VIDEO_PROCESSOR, 
0x302ea3fc, 0xaa5f, 0x47f9, 0x9f, 0x7a, 0xc2, 0x18, 0x8b, 0xb1, 0x63, 0x2);
#endif // (WINVER >= _WIN32_WINNT_WIN7)

// {90175d57-b7ea-4901-aeb3-933a8747756f}   MFT_CATEGORY_OTHER
DEFINE_GUID(MFT_CATEGORY_OTHER,
0x90175d57, 0xb7ea, 0x4901, 0xae, 0xb3, 0x93, 0x3a, 0x87, 0x47, 0x75, 0x6f);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP) */
#pragma endregion

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

//
// "Flags" is for future expansion - for now must be 0
//
STDAPI
MFTRegister(
    _In_                            CLSID                   clsidMFT,
    _In_                            GUID                    guidCategory,
    _In_                            LPWSTR                  pszName,
    _In_                            UINT32                  Flags,
    _In_                            UINT32                  cInputTypes,
    _In_reads_opt_(cInputTypes)    MFT_REGISTER_TYPE_INFO* pInputTypes,
    _In_                            UINT32                  cOutputTypes,
    _In_reads_opt_(cOutputTypes)   MFT_REGISTER_TYPE_INFO* pOutputTypes,
    _In_opt_                        IMFAttributes*          pAttributes
    );

STDAPI
MFTUnregister(
    _In_    CLSID   clsidMFT
    );

#if (WINVER >= _WIN32_WINNT_WIN7)
//  Register an MFT class in-process
STDAPI
MFTRegisterLocal(
   _In_                        IClassFactory*          pClassFactory,
   _In_                        REFGUID                 guidCategory,
   _In_                        LPCWSTR                 pszName,
   _In_                        UINT32                  Flags,
   _In_                        UINT32                  cInputTypes,
   _In_reads_opt_(cInputTypes)const MFT_REGISTER_TYPE_INFO* pInputTypes,
   _In_                        UINT32                  cOutputTypes,
   _In_reads_opt_(cOutputTypes)const MFT_REGISTER_TYPE_INFO* pOutputTypes
    );

//  Unregister locally registered MFT
//  If pClassFactory is NULL all local MFTs are unregistered
STDAPI
MFTUnregisterLocal(
    _In_opt_    IClassFactory *   pClassFactory
    );

// Register an MFT class in-process, by CLSID
STDAPI
MFTRegisterLocalByCLSID(
   _In_                        REFCLSID                clisdMFT,
   _In_                        REFGUID                 guidCategory,
   _In_                        LPCWSTR                 pszName,
   _In_                        UINT32                  Flags,
   _In_                        UINT32                  cInputTypes,
   _In_reads_opt_(cInputTypes)const MFT_REGISTER_TYPE_INFO* pInputTypes,
   _In_                        UINT32                  cOutputTypes,
   _In_reads_opt_(cOutputTypes)const MFT_REGISTER_TYPE_INFO* pOutputTypes
    );

// Unregister locally registered MFT by CLSID
STDAPI
MFTUnregisterLocalByCLSID(
    _In_    CLSID   clsidMFT
    );
#endif // (WINVER >= _WIN32_WINNT_WIN7)

//
// result *ppclsidMFT must be freed with CoTaskMemFree.
//
STDAPI
MFTEnum(
    _In_                    GUID                    guidCategory,
    _In_                    UINT32                  Flags,
    _In_opt_                MFT_REGISTER_TYPE_INFO* pInputType,
    _In_opt_                MFT_REGISTER_TYPE_INFO* pOutputType,
    _In_opt_                IMFAttributes*          pAttributes,
    _Outptr_result_buffer_(*pcMFTs)   CLSID**           ppclsidMFT, // must be freed with CoTaskMemFree
    _Out_                   UINT32*                 pcMFTs
    );

#if (WINVER >= _WIN32_WINNT_WIN7)

enum _MFT_ENUM_FLAG
{
    MFT_ENUM_FLAG_SYNCMFT                       = 0x00000001,   // Enumerates V1 MFTs. This is default.
    MFT_ENUM_FLAG_ASYNCMFT                      = 0x00000002,   // Enumerates only software async MFTs also known as V2 MFTs
    MFT_ENUM_FLAG_HARDWARE                      = 0x00000004,   // Enumerates V2 hardware async MFTs
    MFT_ENUM_FLAG_FIELDOFUSE                    = 0x00000008,   // Enumerates MFTs that require unlocking
    MFT_ENUM_FLAG_LOCALMFT                      = 0x00000010,   // Enumerates Locally (in-process) registered MFTs
    MFT_ENUM_FLAG_TRANSCODE_ONLY                = 0x00000020,   // Enumerates decoder MFTs used by transcode only    
    MFT_ENUM_FLAG_SORTANDFILTER                 = 0x00000040,   // Apply system local, do not use and preferred sorting and filtering
    MFT_ENUM_FLAG_SORTANDFILTER_APPROVED_ONLY   = 0x000000C0,   // Similar to MFT_ENUM_FLAG_SORTANDFILTER, but apply a local policy of: MF_PLUGIN_CONTROL_POLICY_USE_APPROVED_PLUGINS
    MFT_ENUM_FLAG_SORTANDFILTER_WEB_ONLY        = 0x00000140,   // Similar to MFT_ENUM_FLAG_SORTANDFILTER, but apply a local policy of: MF_PLUGIN_CONTROL_POLICY_USE_WEB_PLUGINS
    MFT_ENUM_FLAG_ALL                           = 0x0000003F    // Enumerates all MFTs including SW and HW MFTs and applies filtering
};

//
// result *pppMFTActivate must be freed with CoTaskMemFree. Each IMFActivate pointer inside this
// buffer should be released.
//

STDAPI
MFTEnumEx(
    _In_                                GUID                            guidCategory,
    _In_                                UINT32                          Flags,
    _In_opt_                            const MFT_REGISTER_TYPE_INFO*   pInputType,
    _In_opt_                            const MFT_REGISTER_TYPE_INFO*   pOutputType,
    _Outptr_result_buffer_(*pnumMFTActivate) IMFActivate***                 pppMFTActivate,
    _Out_                               UINT32*                         pnumMFTActivate
);
#endif // (WINVER >= _WIN32_WINNT_WIN7)

//
// results *pszName, *ppInputTypes, and *ppOutputTypes must be freed with CoTaskMemFree.
// *ppAttributes must be released.
//
STDAPI
MFTGetInfo(
    _In_                                   CLSID                       clsidMFT,
    _Out_opt_                              LPWSTR*                     pszName,
    _Outptr_opt_result_buffer_(*pcInputTypes)  MFT_REGISTER_TYPE_INFO**    ppInputTypes,
    _Out_opt_                              UINT32*                     pcInputTypes,
    _Outptr_opt_result_buffer_(*pcOutputTypes) MFT_REGISTER_TYPE_INFO**    ppOutputTypes,
    _Out_opt_                              UINT32*                     pcOutputTypes,
    _Outptr_opt_result_maybenull_                    IMFAttributes**             ppAttributes
    );


#if (WINVER >= _WIN32_WINNT_WIN7)

//
//  Get the plugin control API
//
STDAPI
MFGetPluginControl(
    _Out_ IMFPluginControl **ppPluginControl
    );

//
//  Get MFT's merit - checking that is has a valid certificate
//
STDAPI
MFGetMFTMerit(
    _Inout_ IUnknown *pMFT,
    _In_    UINT32   cbVerifier,
    _In_reads_bytes_(cbVerifier) const BYTE * verifier,
    _Out_   DWORD   *merit
    );

#endif // (WINVER >= _WIN32_WINNT_WIN7)

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#if (WINVER >= _WIN32_WINNT_WIN8)

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

STDAPI
MFRegisterLocalSchemeHandler(
    _In_        PCWSTR          szScheme,
    _In_        IMFActivate*    pActivate
    );

STDAPI
MFRegisterLocalByteStreamHandler(
    _In_        PCWSTR          szFileExtension,
    _In_        PCWSTR          szMimeType,
    _In_        IMFActivate*    pActivate
    );

//
// Wrap a bytestream so that calling Close() on the wrapper
// closes the wrapper but not the original bytestream. The
// original bytestream can then be passed to another
// media source for instance.
//
STDAPI
MFCreateMFByteStreamWrapper(
    _In_        IMFByteStream*  pStream,
    _Out_       IMFByteStream** ppStreamWrapper
    );

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#pragma region Application Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP)

//
// Create a MF activate object that can instantiate media extension objects.
// The activate object supports both IMFActivate and IClassFactory.
//
STDAPI
MFCreateMediaExtensionActivate(
    _In_        PCWSTR          szActivatableClassId,
    _In_opt_    IUnknown*       pConfiguration,
    _In_        REFIID          riid,
    _Outptr_    LPVOID*         ppvObject
    );

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP) */
#pragma endregion

#endif // (WINVER >= _WIN32_WINNT_WIN8)

#pragma region Application Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP)


///////////////////////////////////////////////////////////////////////////////////////////////////////////////  MFT  Attributes GUIDs ////////////////////////////
// {53476A11-3F13-49fb-AC42-EE2733C96741} MFT_SUPPORT_DYNAMIC_FORMAT_CHANGE {UINT32 (BOOL)}
DEFINE_GUID(MFT_SUPPORT_DYNAMIC_FORMAT_CHANGE,
0x53476a11, 0x3f13, 0x49fb, 0xac, 0x42, 0xee, 0x27, 0x33, 0xc9, 0x67, 0x41);
///////////////////////////////////////////////////////////////////////////////////////////////////////////////  Media Type GUIDs ////////////////////////////
////////////////////////////////////////////////////////////////////////////////

//
// GUIDs for media types
//

//
// In MF, media types for uncompressed video formats MUST be composed from a FourCC or D3DFORMAT combined with
// the "base GUID" {00000000-0000-0010-8000-00AA00389B71} by replacing the initial 32 bits with the FourCC/D3DFORMAT
//
// Audio media types for types which already have a defined wFormatTag value can be constructed similarly, by
// putting the wFormatTag (zero-extended to 32 bits) into the first 32 bits of the base GUID.
//
// Compressed video or audio can also use any well-known GUID that exists, or can create a new GUID.
//
// GUIDs for common media types are defined below.
//

// needed for the GUID definition macros below
#ifndef FCC
#define FCC(ch4) ((((DWORD)(ch4) & 0xFF) << 24) |     \
                  (((DWORD)(ch4) & 0xFF00) << 8) |    \
                  (((DWORD)(ch4) & 0xFF0000) >> 8) |  \
                  (((DWORD)(ch4) & 0xFF000000) >> 24))
#endif




//
// this macro creates a media type GUID from a FourCC, D3DFMT, or WAVE_FORMAT
//
#ifndef DEFINE_MEDIATYPE_GUID
#define DEFINE_MEDIATYPE_GUID(name, format) \
    DEFINE_GUID(name,                       \
    format, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);
#endif

//
// video media types
//

//
// If no D3D headers have been included yet, define local versions of D3DFMT constants we use.
// We can't include D3D headers from this header because we need it to be compatible with all versions
// of D3D.
//
#ifndef DIRECT3D_VERSION
#define D3DFMT_R8G8B8       20
#define D3DFMT_A8R8G8B8     21
#define D3DFMT_X8R8G8B8     22
#define D3DFMT_R5G6B5       23
#define D3DFMT_X1R5G5B5     24
#define D3DFMT_P8           41
#define LOCAL_D3DFMT_DEFINES 1
#endif

DEFINE_MEDIATYPE_GUID( MFVideoFormat_Base,      0x00000000 );
DEFINE_MEDIATYPE_GUID( MFVideoFormat_RGB32,     D3DFMT_X8R8G8B8 );
DEFINE_MEDIATYPE_GUID( MFVideoFormat_ARGB32,    D3DFMT_A8R8G8B8 );
DEFINE_MEDIATYPE_GUID( MFVideoFormat_RGB24,     D3DFMT_R8G8B8 );
DEFINE_MEDIATYPE_GUID( MFVideoFormat_RGB555,    D3DFMT_X1R5G5B5 );
DEFINE_MEDIATYPE_GUID( MFVideoFormat_RGB565,    D3DFMT_R5G6B5 );
DEFINE_MEDIATYPE_GUID( MFVideoFormat_RGB8,      D3DFMT_P8 );
DEFINE_MEDIATYPE_GUID( MFVideoFormat_AI44,      FCC('AI44') );
DEFINE_MEDIATYPE_GUID( MFVideoFormat_AYUV,      FCC('AYUV') );
DEFINE_MEDIATYPE_GUID( MFVideoFormat_YUY2,      FCC('YUY2') );
DEFINE_MEDIATYPE_GUID( MFVideoFormat_YVYU,      FCC('YVYU') );
DEFINE_MEDIATYPE_GUID( MFVideoFormat_YVU9,      FCC('YVU9') );
DEFINE_MEDIATYPE_GUID( MFVideoFormat_UYVY,      FCC('UYVY') );
DEFINE_MEDIATYPE_GUID( MFVideoFormat_NV11,      FCC('NV11') );
DEFINE_MEDIATYPE_GUID( MFVideoFormat_NV12,      FCC('NV12') );
DEFINE_MEDIATYPE_GUID( MFVideoFormat_YV12,      FCC('YV12') );
DEFINE_MEDIATYPE_GUID( MFVideoFormat_I420,      FCC('I420') );
DEFINE_MEDIATYPE_GUID( MFVideoFormat_IYUV,      FCC('IYUV') );
DEFINE_MEDIATYPE_GUID( MFVideoFormat_Y210,      FCC('Y210') );
DEFINE_MEDIATYPE_GUID( MFVideoFormat_Y216,      FCC('Y216') );
DEFINE_MEDIATYPE_GUID( MFVideoFormat_Y410,      FCC('Y410') );
DEFINE_MEDIATYPE_GUID( MFVideoFormat_Y416,      FCC('Y416') );
DEFINE_MEDIATYPE_GUID( MFVideoFormat_Y41P,      FCC('Y41P') );
DEFINE_MEDIATYPE_GUID( MFVideoFormat_Y41T,      FCC('Y41T') );
DEFINE_MEDIATYPE_GUID( MFVideoFormat_Y42T,      FCC('Y42T') );
DEFINE_MEDIATYPE_GUID( MFVideoFormat_P210,      FCC('P210') );
DEFINE_MEDIATYPE_GUID( MFVideoFormat_P216,      FCC('P216') );
DEFINE_MEDIATYPE_GUID( MFVideoFormat_P010,      FCC('P010') );
DEFINE_MEDIATYPE_GUID( MFVideoFormat_P016,      FCC('P016') );
DEFINE_MEDIATYPE_GUID( MFVideoFormat_v210,      FCC('v210') );
DEFINE_MEDIATYPE_GUID( MFVideoFormat_v216,      FCC('v216') );
DEFINE_MEDIATYPE_GUID( MFVideoFormat_v410,      FCC('v410') );
DEFINE_MEDIATYPE_GUID( MFVideoFormat_MP43,      FCC('MP43') );
DEFINE_MEDIATYPE_GUID( MFVideoFormat_MP4S,      FCC('MP4S') );
DEFINE_MEDIATYPE_GUID( MFVideoFormat_M4S2,      FCC('M4S2') );
DEFINE_MEDIATYPE_GUID( MFVideoFormat_MP4V,      FCC('MP4V') );
DEFINE_MEDIATYPE_GUID( MFVideoFormat_WMV1,      FCC('WMV1') );
DEFINE_MEDIATYPE_GUID( MFVideoFormat_WMV2,      FCC('WMV2') );
DEFINE_MEDIATYPE_GUID( MFVideoFormat_WMV3,      FCC('WMV3') );
DEFINE_MEDIATYPE_GUID( MFVideoFormat_WVC1,      FCC('WVC1') );
DEFINE_MEDIATYPE_GUID( MFVideoFormat_MSS1,      FCC('MSS1') );
DEFINE_MEDIATYPE_GUID( MFVideoFormat_MSS2,      FCC('MSS2') );
DEFINE_MEDIATYPE_GUID( MFVideoFormat_MPG1,      FCC('MPG1') );
DEFINE_MEDIATYPE_GUID( MFVideoFormat_DVSL,      FCC('dvsl') );
DEFINE_MEDIATYPE_GUID( MFVideoFormat_DVSD,      FCC('dvsd') );
DEFINE_MEDIATYPE_GUID( MFVideoFormat_DVHD,      FCC('dvhd') );
DEFINE_MEDIATYPE_GUID( MFVideoFormat_DV25,      FCC('dv25') );
DEFINE_MEDIATYPE_GUID( MFVideoFormat_DV50,      FCC('dv50') );
DEFINE_MEDIATYPE_GUID( MFVideoFormat_DVH1,      FCC('dvh1') );
DEFINE_MEDIATYPE_GUID( MFVideoFormat_DVC,       FCC('dvc ') );
DEFINE_MEDIATYPE_GUID( MFVideoFormat_H264,      FCC('H264') );  // assume MFVideoFormat_H264 is frame aligned. that is, each input sample has one complete compressed frame (one frame picture, two field pictures or a single unpaired field picture)
DEFINE_MEDIATYPE_GUID( MFVideoFormat_MJPG,      FCC('MJPG') );
DEFINE_MEDIATYPE_GUID( MFVideoFormat_420O,      FCC('420O') );
DEFINE_MEDIATYPE_GUID( MFVideoFormat_HEVC,      FCC('HEVC') );
DEFINE_MEDIATYPE_GUID( MFVideoFormat_HEVC_ES,   FCC('HEVS') );

#if (WINVER >= _WIN32_WINNT_WIN8)
DEFINE_MEDIATYPE_GUID( MFVideoFormat_H263,      FCC('H263') );
#endif // (WINVER >= _WIN32_WINNT_WIN8)

//
// undef the local D3DFMT definitions to avoid later clashes with D3D headers
//
#ifdef LOCAL_D3DFMT_DEFINES
#undef D3DFMT_R8G8B8
#undef D3DFMT_A8R8G8B8
#undef D3DFMT_X8R8G8B8
#undef D3DFMT_R5G6B5
#undef D3DFMT_X1R5G5B5
#undef D3DFMT_P8
#undef LOCAL_D3DFMT_DEFINES
#endif


// assume MFVideoFormat_H264_ES may not be frame aligned. that is, each input sample may have one partial frame, 
// multiple frames, some frames plus some partial frame 
// or more general, N.M frames, N is the integer part and M is the fractional part.
//
// {3F40F4F0-5622-4FF8-B6D8-A17A584BEE5E}       MFVideoFormat_H264_ES
DEFINE_GUID(MFVideoFormat_H264_ES, 
0x3f40f4f0, 0x5622, 0x4ff8, 0xb6, 0xd8, 0xa1, 0x7a, 0x58, 0x4b, 0xee, 0x5e);


//
// some legacy formats that don't fit the common pattern
//

// {e06d8026-db46-11cf-b4d1-00805f6cbbea}       MFVideoFormat_MPEG2
DEFINE_GUID(MFVideoFormat_MPEG2,
0xe06d8026, 0xdb46, 0x11cf, 0xb4, 0xd1, 0x00, 0x80, 0x5f, 0x6c, 0xbb, 0xea);

#define MFVideoFormat_MPG2 MFVideoFormat_MPEG2



//
// audio media types
//
DEFINE_MEDIATYPE_GUID( MFAudioFormat_Base,              0x00000000 );
DEFINE_MEDIATYPE_GUID( MFAudioFormat_PCM,               WAVE_FORMAT_PCM );
DEFINE_MEDIATYPE_GUID( MFAudioFormat_Float,             WAVE_FORMAT_IEEE_FLOAT );
DEFINE_MEDIATYPE_GUID( MFAudioFormat_DTS,               WAVE_FORMAT_DTS );
DEFINE_MEDIATYPE_GUID( MFAudioFormat_Dolby_AC3_SPDIF,   WAVE_FORMAT_DOLBY_AC3_SPDIF );
DEFINE_MEDIATYPE_GUID( MFAudioFormat_DRM,               WAVE_FORMAT_DRM );
DEFINE_MEDIATYPE_GUID( MFAudioFormat_WMAudioV8,         WAVE_FORMAT_WMAUDIO2 );
DEFINE_MEDIATYPE_GUID( MFAudioFormat_WMAudioV9,         WAVE_FORMAT_WMAUDIO3 );
DEFINE_MEDIATYPE_GUID( MFAudioFormat_WMAudio_Lossless,  WAVE_FORMAT_WMAUDIO_LOSSLESS );
DEFINE_MEDIATYPE_GUID( MFAudioFormat_WMASPDIF,          WAVE_FORMAT_WMASPDIF );
DEFINE_MEDIATYPE_GUID( MFAudioFormat_MSP1,              WAVE_FORMAT_WMAVOICE9 );
DEFINE_MEDIATYPE_GUID( MFAudioFormat_MP3,               WAVE_FORMAT_MPEGLAYER3 );
DEFINE_MEDIATYPE_GUID( MFAudioFormat_MPEG,              WAVE_FORMAT_MPEG );
DEFINE_MEDIATYPE_GUID( MFAudioFormat_AAC,               WAVE_FORMAT_MPEG_HEAAC );
DEFINE_MEDIATYPE_GUID( MFAudioFormat_ADTS,              WAVE_FORMAT_MPEG_ADTS_AAC );
//DEFINE_MEDIATYPE_GUID( MFAudioFormat_AMR_NB,            WAVE_FORMAT_AMR_NB );
//DEFINE_MEDIATYPE_GUID( MFAudioFormat_AMR_WB,            WAVE_FORMAT_AMR_WB );
//DEFINE_MEDIATYPE_GUID( MFAudioFormat_AMR_WP,            WAVE_FORMAT_AMR_WP );

// These audio types are not derived from an existing wFormatTag 
DEFINE_GUID(MFAudioFormat_Dolby_AC3, // == MEDIASUBTYPE_DOLBY_AC3 defined in ksuuids.h
0xe06d802c, 0xdb46, 0x11cf, 0xb4, 0xd1, 0x00, 0x80, 0x05f, 0x6c, 0xbb, 0xea);
DEFINE_GUID(MFAudioFormat_Dolby_DDPlus, // == MEDIASUBTYPE_DOLBY_DDPLUS defined in wmcodecdsp.h
0xa7fb87af, 0x2d02, 0x42fb, 0xa4, 0xd4, 0x5, 0xcd, 0x93, 0x84, 0x3b, 0xdd);

//
// MPEG-4 media types
//

// {00000000-767a-494d-b478-f29d25dc9037}       MFMPEG4Format_Base
DEFINE_GUID(MFMPEG4Format_Base,
0x00000000, 0x767a, 0x494d, 0xb4, 0x78, 0xf2, 0x9d, 0x25, 0xdc, 0x90, 0x37);

///////////////////////////////////////////////////////////////////////////////////////////////////////////////  Media Type Attributes GUIDs ////////////////////////////
////////////////////////////////////////////////////////////////////////////////

//
// GUIDs for IMFMediaType properties - prefix 'MF_MT_' - basic prop type in {},
// with type to cast to in ().
//


//
// core info for all types
//
// {48eba18e-f8c9-4687-bf11-0a74c9f96a8f}   MF_MT_MAJOR_TYPE                {GUID}
DEFINE_GUID(MF_MT_MAJOR_TYPE,
0x48eba18e, 0xf8c9, 0x4687, 0xbf, 0x11, 0x0a, 0x74, 0xc9, 0xf9, 0x6a, 0x8f);

// {f7e34c9a-42e8-4714-b74b-cb29d72c35e5}   MF_MT_SUBTYPE                   {GUID}
DEFINE_GUID(MF_MT_SUBTYPE,
0xf7e34c9a, 0x42e8, 0x4714, 0xb7, 0x4b, 0xcb, 0x29, 0xd7, 0x2c, 0x35, 0xe5);

// {c9173739-5e56-461c-b713-46fb995cb95f}   MF_MT_ALL_SAMPLES_INDEPENDENT   {UINT32 (BOOL)}
DEFINE_GUID(MF_MT_ALL_SAMPLES_INDEPENDENT,
0xc9173739, 0x5e56, 0x461c, 0xb7, 0x13, 0x46, 0xfb, 0x99, 0x5c, 0xb9, 0x5f);

// {b8ebefaf-b718-4e04-b0a9-116775e3321b}   MF_MT_FIXED_SIZE_SAMPLES        {UINT32 (BOOL)}
DEFINE_GUID(MF_MT_FIXED_SIZE_SAMPLES,
0xb8ebefaf, 0xb718, 0x4e04, 0xb0, 0xa9, 0x11, 0x67, 0x75, 0xe3, 0x32, 0x1b);

// {3afd0cee-18f2-4ba5-a110-8bea502e1f92}   MF_MT_COMPRESSED                {UINT32 (BOOL)}
DEFINE_GUID(MF_MT_COMPRESSED,
0x3afd0cee, 0x18f2, 0x4ba5, 0xa1, 0x10, 0x8b, 0xea, 0x50, 0x2e, 0x1f, 0x92);

//
// MF_MT_SAMPLE_SIZE is only valid if MF_MT_FIXED_SIZED_SAMPLES is TRUE
//
// {dad3ab78-1990-408b-bce2-eba673dacc10}   MF_MT_SAMPLE_SIZE               {UINT32}
DEFINE_GUID(MF_MT_SAMPLE_SIZE,
0xdad3ab78, 0x1990, 0x408b, 0xbc, 0xe2, 0xeb, 0xa6, 0x73, 0xda, 0xcc, 0x10);

// 4d3f7b23-d02f-4e6c-9bee-e4bf2c6c695d     MF_MT_WRAPPED_TYPE              {Blob}
DEFINE_GUID(MF_MT_WRAPPED_TYPE,
0x4d3f7b23, 0xd02f, 0x4e6c, 0x9b, 0xee, 0xe4, 0xbf, 0x2c, 0x6c, 0x69, 0x5d);

#if (WINVER >= _WIN32_WINNT_WIN8)

//
// Media Type & Sample attributes for 3D Video
//

// {CB5E88CF-7B5B-476b-85AA-1CA5AE187555}        MF_MT_VIDEO_3D                 {UINT32 (BOOL)}
DEFINE_GUID( MF_MT_VIDEO_3D, 
0xcb5e88cf, 0x7b5b, 0x476b, 0x85, 0xaa, 0x1c, 0xa5, 0xae, 0x18, 0x75, 0x55);

// Enum describing the packing for 3D video frames
typedef enum _MFVideo3DFormat {
    MFVideo3DSampleFormat_BaseView              = 0,
    MFVideo3DSampleFormat_MultiView             = 1,
    MFVideo3DSampleFormat_Packed_LeftRight      = 2,
    MFVideo3DSampleFormat_Packed_TopBottom      = 3,
} MFVideo3DFormat;

// {5315d8a0-87c5-4697-b793-666c67c49b}         MF_MT_VIDEO_3D_FORMAT           {UINT32 (anyof MFVideo3DFormat)}
DEFINE_GUID(MF_MT_VIDEO_3D_FORMAT, 
0x5315d8a0, 0x87c5, 0x4697, 0xb7, 0x93, 0x66, 0x6, 0xc6, 0x7c, 0x4, 0x9b);

// {BB077E8A-DCBF-42eb-AF60-418DF98AA495}       MF_MT_VIDEO_3D_NUM_VIEW         {UINT32}
DEFINE_GUID( MF_MT_VIDEO_3D_NUM_VIEWS, 
0xbb077e8a, 0xdcbf, 0x42eb, 0xaf, 0x60, 0x41, 0x8d, 0xf9, 0x8a, 0xa4, 0x95);

// {6D4B7BFF-5629-4404-948C-C634F4CE26D4}       MF_MT_VIDEO_3D_LEFT_IS_BASE     {UINT32}
DEFINE_GUID( MF_MT_VIDEO_3D_LEFT_IS_BASE,
0x6d4b7bff, 0x5629, 0x4404, 0x94, 0x8c, 0xc6, 0x34, 0xf4, 0xce, 0x26, 0xd4);

// {EC298493-0ADA-4ea1-A4FE-CBBD36CE9331}       MF_MT_VIDEO_3D_FIRST_IS_LEFT    {UINT32 (BOOL)}
DEFINE_GUID( MF_MT_VIDEO_3D_FIRST_IS_LEFT, 
0xec298493, 0xada, 0x4ea1, 0xa4, 0xfe, 0xcb, 0xbd, 0x36, 0xce, 0x93, 0x31);


// MFSampleExtension_3DVideo                    {F86F97A4-DD54-4e2e-9A5E-55FC2D74A005}
// Type: UINT32
// If present and nonzero, indicates that the sample contains 3D Video data
DEFINE_GUID( MFSampleExtension_3DVideo, 
0xf86f97a4, 0xdd54, 0x4e2e, 0x9a, 0x5e, 0x55, 0xfc, 0x2d, 0x74, 0xa0, 0x05);

// Enum describing the packing for 3D video frames in a sample
typedef enum _MFVideo3DSampleFormat {
    MFSampleExtension_3DVideo_MultiView         = 1,
    MFSampleExtension_3DVideo_Packed            = 0,
} MFVideo3DSampleFormat;

// MFSampleExtension_3DVideo_SampleFormat       {08671772-E36F-4cff-97B3-D72E20987A48}
// Type: UINT32
// The value of this attribute is a member of the MFVideo3DSampleFormat enumeration.
// MFVideo3DSampleFormat enumeration identifies how 3D views are stored in the sample
//      - in a packed representation, all views are stored in a single buffer
//      - in a multiview representation, each view is stored in its own buffer
DEFINE_GUID( MFSampleExtension_3DVideo_SampleFormat, 
0x8671772, 0xe36f, 0x4cff, 0x97, 0xb3, 0xd7, 0x2e, 0x20, 0x98, 0x7a, 0x48);

// Enum describing the video rotation formats
// Only the values of 0, 90, 180, and 270 are valid.
typedef enum _MFVideoRotationFormat {
    MFVideoRotationFormat_0        = 0,
    MFVideoRotationFormat_90       = 90,
    MFVideoRotationFormat_180      = 180,
    MFVideoRotationFormat_270      = 270,
} MFVideoRotationFormat;

// MF_MT_VIDEO_ROTATION      {C380465D-2271-428C-9B83-ECEA3B4A85C1}
// Type: UINT32
// Description: MF_MT_VIDEO_ROTATION attribute means the degree that the content
// has already been rotated in the counter clockwise direction.
// Currently, only the values of 0, 90, 180, and 270 are valid for MF_MT_VIDEO_ROTATION.
// For convenience, these currently supported values are enumerated in MFVideoRotationFormat.
// Example: if the media type has MF_MT_VIDEO_ROTATION set as MFVideoRotationFormat_90,
// it means the content has been rotated 90 degree in the counter clockwise direction.
// If the content was actually rotated 90 degree in the clockwise direction, 90 degree in
// clockwise should be converted into 270 degree in the counter clockwise direction and set
// the attribute MF_MT_VIDEO_ROTATION as MFVideoRotationFormat_270 accordingly.
DEFINE_GUID(MF_MT_VIDEO_ROTATION,
0xc380465d, 0x2271, 0x428c, 0x9b, 0x83, 0xec, 0xea, 0x3b, 0x4a, 0x85, 0xc1);

#endif // (WINVER >= _WIN32_WINNT_WIN8)

//
// AUDIO data
//

// {37e48bf5-645e-4c5b-89de-ada9e29b696a}   MF_MT_AUDIO_NUM_CHANNELS            {UINT32}
DEFINE_GUID(MF_MT_AUDIO_NUM_CHANNELS,
0x37e48bf5, 0x645e, 0x4c5b, 0x89, 0xde, 0xad, 0xa9, 0xe2, 0x9b, 0x69, 0x6a);

// {5faeeae7-0290-4c31-9e8a-c534f68d9dba}   MF_MT_AUDIO_SAMPLES_PER_SECOND      {UINT32}
DEFINE_GUID(MF_MT_AUDIO_SAMPLES_PER_SECOND,
0x5faeeae7, 0x0290, 0x4c31, 0x9e, 0x8a, 0xc5, 0x34, 0xf6, 0x8d, 0x9d, 0xba);

// {fb3b724a-cfb5-4319-aefe-6e42b2406132}   MF_MT_AUDIO_FLOAT_SAMPLES_PER_SECOND {double}
DEFINE_GUID(MF_MT_AUDIO_FLOAT_SAMPLES_PER_SECOND,
0xfb3b724a, 0xcfb5, 0x4319, 0xae, 0xfe, 0x6e, 0x42, 0xb2, 0x40, 0x61, 0x32);

// {1aab75c8-cfef-451c-ab95-ac034b8e1731}   MF_MT_AUDIO_AVG_BYTES_PER_SECOND    {UINT32}
DEFINE_GUID(MF_MT_AUDIO_AVG_BYTES_PER_SECOND,
0x1aab75c8, 0xcfef, 0x451c, 0xab, 0x95, 0xac, 0x03, 0x4b, 0x8e, 0x17, 0x31);

// {322de230-9eeb-43bd-ab7a-ff412251541d}   MF_MT_AUDIO_BLOCK_ALIGNMENT         {UINT32}
DEFINE_GUID(MF_MT_AUDIO_BLOCK_ALIGNMENT,
0x322de230, 0x9eeb, 0x43bd, 0xab, 0x7a, 0xff, 0x41, 0x22, 0x51, 0x54, 0x1d);

// {f2deb57f-40fa-4764-aa33-ed4f2d1ff669}   MF_MT_AUDIO_BITS_PER_SAMPLE         {UINT32}
DEFINE_GUID(MF_MT_AUDIO_BITS_PER_SAMPLE,
0xf2deb57f, 0x40fa, 0x4764, 0xaa, 0x33, 0xed, 0x4f, 0x2d, 0x1f, 0xf6, 0x69);

// {d9bf8d6a-9530-4b7c-9ddf-ff6fd58bbd06}   MF_MT_AUDIO_VALID_BITS_PER_SAMPLE   {UINT32}
DEFINE_GUID(MF_MT_AUDIO_VALID_BITS_PER_SAMPLE,
0xd9bf8d6a, 0x9530, 0x4b7c, 0x9d, 0xdf, 0xff, 0x6f, 0xd5, 0x8b, 0xbd, 0x06);

// {aab15aac-e13a-4995-9222-501ea15c6877}   MF_MT_AUDIO_SAMPLES_PER_BLOCK       {UINT32}
DEFINE_GUID(MF_MT_AUDIO_SAMPLES_PER_BLOCK,
0xaab15aac, 0xe13a, 0x4995, 0x92, 0x22, 0x50, 0x1e, 0xa1, 0x5c, 0x68, 0x77);

// {55fb5765-644a-4caf-8479-938983bb1588}`  MF_MT_AUDIO_CHANNEL_MASK            {UINT32}
DEFINE_GUID(MF_MT_AUDIO_CHANNEL_MASK,
0x55fb5765, 0x644a, 0x4caf, 0x84, 0x79, 0x93, 0x89, 0x83, 0xbb, 0x15, 0x88);

//
// MF_MT_AUDIO_FOLDDOWN_MATRIX stores folddown structure from multichannel to stereo
//
typedef struct _MFFOLDDOWN_MATRIX
{
    UINT32 cbSize;
    UINT32 cSrcChannels; // number of source channels
    UINT32 cDstChannels; // number of destination channels
    UINT32 dwChannelMask; // mask
    LONG Coeff[64];
} MFFOLDDOWN_MATRIX;

// {9d62927c-36be-4cf2-b5c4-a3926e3e8711}`  MF_MT_AUDIO_FOLDDOWN_MATRIX         {BLOB, MFFOLDDOWN_MATRIX}
DEFINE_GUID(MF_MT_AUDIO_FOLDDOWN_MATRIX,
0x9d62927c, 0x36be, 0x4cf2, 0xb5, 0xc4, 0xa3, 0x92, 0x6e, 0x3e, 0x87, 0x11);

// {0x9d62927d-36be-4cf2-b5c4-a3926e3e8711}`  MF_MT_AUDIO_WMADRC_PEAKREF         {UINT32}
DEFINE_GUID(MF_MT_AUDIO_WMADRC_PEAKREF,
0x9d62927d, 0x36be, 0x4cf2, 0xb5, 0xc4, 0xa3, 0x92, 0x6e, 0x3e, 0x87, 0x11);

// {0x9d62927e-36be-4cf2-b5c4-a3926e3e8711}`  MF_MT_AUDIO_WMADRC_PEAKTARGET        {UINT32}
DEFINE_GUID(MF_MT_AUDIO_WMADRC_PEAKTARGET,
0x9d62927e, 0x36be, 0x4cf2, 0xb5, 0xc4, 0xa3, 0x92, 0x6e, 0x3e, 0x87, 0x11);


// {0x9d62927f-36be-4cf2-b5c4-a3926e3e8711}`  MF_MT_AUDIO_WMADRC_AVGREF         {UINT32}
DEFINE_GUID(MF_MT_AUDIO_WMADRC_AVGREF,
0x9d62927f, 0x36be, 0x4cf2, 0xb5, 0xc4, 0xa3, 0x92, 0x6e, 0x3e, 0x87, 0x11);

// {0x9d629280-36be-4cf2-b5c4-a3926e3e8711}`  MF_MT_AUDIO_WMADRC_AVGTARGET      {UINT32}
DEFINE_GUID(MF_MT_AUDIO_WMADRC_AVGTARGET,
0x9d629280, 0x36be, 0x4cf2, 0xb5, 0xc4, 0xa3, 0x92, 0x6e, 0x3e, 0x87, 0x11);

//
// MF_MT_AUDIO_PREFER_WAVEFORMATEX tells the converter to prefer a plain WAVEFORMATEX rather than
// a WAVEFORMATEXTENSIBLE when converting to a legacy type. It is set by the WAVEFORMATEX->IMFMediaType
// conversion routines when the original format block is a non-extensible WAVEFORMATEX.
//
// This preference can be overridden and does not guarantee that the type can be correctly expressed
// by a non-extensible type.
//
// {a901aaba-e037-458a-bdf6-545be2074042}   MF_MT_AUDIO_PREFER_WAVEFORMATEX     {UINT32 (BOOL)}
DEFINE_GUID(MF_MT_AUDIO_PREFER_WAVEFORMATEX,
0xa901aaba, 0xe037, 0x458a, 0xbd, 0xf6, 0x54, 0x5b, 0xe2, 0x07, 0x40, 0x42);

#if (WINVER >= _WIN32_WINNT_WIN7)
//
// AUDIO - AAC extra data
//

// {BFBABE79-7434-4d1c-94F0-72A3B9E17188} MF_MT_AAC_PAYLOAD_TYPE       {UINT32}
DEFINE_GUID(MF_MT_AAC_PAYLOAD_TYPE,
0xbfbabe79, 0x7434, 0x4d1c, 0x94, 0xf0, 0x72, 0xa3, 0xb9, 0xe1, 0x71, 0x88);

// {7632F0E6-9538-4d61-ACDA-EA29C8C14456} MF_MT_AAC_AUDIO_PROFILE_LEVEL_INDICATION       {UINT32}
DEFINE_GUID(MF_MT_AAC_AUDIO_PROFILE_LEVEL_INDICATION,
0x7632f0e6, 0x9538, 0x4d61, 0xac, 0xda, 0xea, 0x29, 0xc8, 0xc1, 0x44, 0x56);

#endif // (WINVER >= _WIN32_WINNT_WIN7)

//
// VIDEO core data
//

// {1652c33d-d6b2-4012-b834-72030849a37d}   MF_MT_FRAME_SIZE                {UINT64 (HI32(Width),LO32(Height))}
DEFINE_GUID(MF_MT_FRAME_SIZE,
0x1652c33d, 0xd6b2, 0x4012, 0xb8, 0x34, 0x72, 0x03, 0x08, 0x49, 0xa3, 0x7d);

// {c459a2e8-3d2c-4e44-b132-fee5156c7bb0}   MF_MT_FRAME_RATE                {UINT64 (HI32(Numerator),LO32(Denominator))}
DEFINE_GUID(MF_MT_FRAME_RATE,
0xc459a2e8, 0x3d2c, 0x4e44, 0xb1, 0x32, 0xfe, 0xe5, 0x15, 0x6c, 0x7b, 0xb0);

// {c6376a1e-8d0a-4027-be45-6d9a0ad39bb6}   MF_MT_PIXEL_ASPECT_RATIO        {UINT64 (HI32(Numerator),LO32(Denominator))}
DEFINE_GUID(MF_MT_PIXEL_ASPECT_RATIO,
0xc6376a1e, 0x8d0a, 0x4027, 0xbe, 0x45, 0x6d, 0x9a, 0x0a, 0xd3, 0x9b, 0xb6);

// {8772f323-355a-4cc7-bb78-6d61a048ae82}   MF_MT_DRM_FLAGS                 {UINT32 (anyof MFVideoDRMFlags)}
DEFINE_GUID(MF_MT_DRM_FLAGS,
0x8772f323, 0x355a, 0x4cc7, 0xbb, 0x78, 0x6d, 0x61, 0xa0, 0x48, 0xae, 0x82);

#if (WINVER >= _WIN32_WINNT_WIN8)

// {24974215-1B7B-41e4-8625-AC469F2DEDAA}   MF_MT_TIMESTAMP_CAN_BE_DTS      {UINT32 (BOOL)}
DEFINE_GUID(MF_MT_TIMESTAMP_CAN_BE_DTS, 
0x24974215, 0x1b7b, 0x41e4, 0x86, 0x25, 0xac, 0x46, 0x9f, 0x2d, 0xed, 0xaa);

#endif // (WINVER >= _WIN32_WINNT_WIN8)

typedef enum _MFVideoDRMFlags {
    MFVideoDRMFlag_None                 = 0,
    MFVideoDRMFlag_AnalogProtected      = 1,
    MFVideoDRMFlag_DigitallyProtected   = 2,
} MFVideoDRMFlags;


// {4d0e73e5-80ea-4354-a9d0-1176ceb028ea}   MF_MT_PAD_CONTROL_FLAGS         {UINT32 (oneof MFVideoPadFlags)}
DEFINE_GUID(MF_MT_PAD_CONTROL_FLAGS,
0x4d0e73e5, 0x80ea, 0x4354, 0xa9, 0xd0, 0x11, 0x76, 0xce, 0xb0, 0x28, 0xea);

typedef enum _MFVideoPadFlags {
    MFVideoPadFlag_PAD_TO_None  = 0,
    MFVideoPadFlag_PAD_TO_4x3   = 1,
    MFVideoPadFlag_PAD_TO_16x9  = 2
} MFVideoPadFlags;

// {68aca3cc-22d0-44e6-85f8-28167197fa38}   MF_MT_SOURCE_CONTENT_HINT       {UINT32 (oneof MFVideoSrcContentHintFlags)}
DEFINE_GUID(MF_MT_SOURCE_CONTENT_HINT,
0x68aca3cc, 0x22d0, 0x44e6, 0x85, 0xf8, 0x28, 0x16, 0x71, 0x97, 0xfa, 0x38);

typedef enum _MFVideoSrcContentHintFlags {
    MFVideoSrcContentHintFlag_None  = 0,
    MFVideoSrcContentHintFlag_16x9  = 1,
    MFVideoSrcContentHintFlag_235_1 = 2
} MFVideoSrcContentHintFlags;

// {65df2370-c773-4c33-aa64-843e068efb0c}   MF_MT_CHROMA_SITING             {UINT32 (anyof MFVideoChromaSubsampling)}
DEFINE_GUID(MF_MT_VIDEO_CHROMA_SITING,
0x65df2370, 0xc773, 0x4c33, 0xaa, 0x64, 0x84, 0x3e, 0x06, 0x8e, 0xfb, 0x0c);

// {e2724bb8-e676-4806-b4b2-a8d6efb44ccd}   MF_MT_INTERLACE_MODE            {UINT32 (oneof MFVideoInterlaceMode)}
DEFINE_GUID(MF_MT_INTERLACE_MODE,
0xe2724bb8, 0xe676, 0x4806, 0xb4, 0xb2, 0xa8, 0xd6, 0xef, 0xb4, 0x4c, 0xcd);

// {5fb0fce9-be5c-4935-a811-ec838f8eed93}   MF_MT_TRANSFER_FUNCTION         {UINT32 (oneof MFVideoTransferFunction)}
DEFINE_GUID(MF_MT_TRANSFER_FUNCTION,
0x5fb0fce9, 0xbe5c, 0x4935, 0xa8, 0x11, 0xec, 0x83, 0x8f, 0x8e, 0xed, 0x93);

// {dbfbe4d7-0740-4ee0-8192-850ab0e21935}   MF_MT_VIDEO_PRIMARIES           {UINT32 (oneof MFVideoPrimaries)}
DEFINE_GUID(MF_MT_VIDEO_PRIMARIES,
0xdbfbe4d7, 0x0740, 0x4ee0, 0x81, 0x92, 0x85, 0x0a, 0xb0, 0xe2, 0x19, 0x35);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP) */
#pragma endregion

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

// {47537213-8cfb-4722-aa34-fbc9e24d77b8}   MF_MT_CUSTOM_VIDEO_PRIMARIES    {BLOB (MT_CUSTOM_VIDEO_PRIMARIES)}
DEFINE_GUID(MF_MT_CUSTOM_VIDEO_PRIMARIES,
0x47537213, 0x8cfb, 0x4722, 0xaa, 0x34, 0xfb, 0xc9, 0xe2, 0x4d, 0x77, 0xb8);

typedef struct _MT_CUSTOM_VIDEO_PRIMARIES {
    float fRx;
    float fRy;
    float fGx;
    float fGy;
    float fBx;
    float fBy;
    float fWx;
    float fWy;
} MT_CUSTOM_VIDEO_PRIMARIES;

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#pragma region Application Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP)

// {3e23d450-2c75-4d25-a00e-b91670d12327}   MF_MT_YUV_MATRIX                {UINT32 (oneof MFVideoTransferMatrix)}
DEFINE_GUID(MF_MT_YUV_MATRIX,
0x3e23d450, 0x2c75, 0x4d25, 0xa0, 0x0e, 0xb9, 0x16, 0x70, 0xd1, 0x23, 0x27);

// {53a0529c-890b-4216-8bf9-599367ad6d20}   MF_MT_VIDEO_LIGHTING            {UINT32 (oneof MFVideoLighting)}
DEFINE_GUID(MF_MT_VIDEO_LIGHTING,
0x53a0529c, 0x890b, 0x4216, 0x8b, 0xf9, 0x59, 0x93, 0x67, 0xad, 0x6d, 0x20);

// {c21b8ee5-b956-4071-8daf-325edf5cab11}   MF_MT_VIDEO_NOMINAL_RANGE       {UINT32 (oneof MFNominalRange)}
DEFINE_GUID(MF_MT_VIDEO_NOMINAL_RANGE,
0xc21b8ee5, 0xb956, 0x4071, 0x8d, 0xaf, 0x32, 0x5e, 0xdf, 0x5c, 0xab, 0x11);

// {66758743-7e5f-400d-980a-aa8596c85696}   MF_MT_GEOMETRIC_APERTURE        {BLOB (MFVideoArea)}
DEFINE_GUID(MF_MT_GEOMETRIC_APERTURE,
0x66758743, 0x7e5f, 0x400d, 0x98, 0x0a, 0xaa, 0x85, 0x96, 0xc8, 0x56, 0x96);

// {d7388766-18fe-48c6-a177-ee894867c8c4}   MF_MT_MINIMUM_DISPLAY_APERTURE  {BLOB (MFVideoArea)}
DEFINE_GUID(MF_MT_MINIMUM_DISPLAY_APERTURE,
0xd7388766, 0x18fe, 0x48c6, 0xa1, 0x77, 0xee, 0x89, 0x48, 0x67, 0xc8, 0xc4);

// {79614dde-9187-48fb-b8c7-4d52689de649}   MF_MT_PAN_SCAN_APERTURE         {BLOB (MFVideoArea)}
DEFINE_GUID(MF_MT_PAN_SCAN_APERTURE,
0x79614dde, 0x9187, 0x48fb, 0xb8, 0xc7, 0x4d, 0x52, 0x68, 0x9d, 0xe6, 0x49);

// {4b7f6bc3-8b13-40b2-a993-abf630b8204e}   MF_MT_PAN_SCAN_ENABLED          {UINT32 (BOOL)}
DEFINE_GUID(MF_MT_PAN_SCAN_ENABLED,
0x4b7f6bc3, 0x8b13, 0x40b2, 0xa9, 0x93, 0xab, 0xf6, 0x30, 0xb8, 0x20, 0x4e);

// {20332624-fb0d-4d9e-bd0d-cbf6786c102e}   MF_MT_AVG_BITRATE               {UINT32}
DEFINE_GUID(MF_MT_AVG_BITRATE,
0x20332624, 0xfb0d, 0x4d9e, 0xbd, 0x0d, 0xcb, 0xf6, 0x78, 0x6c, 0x10, 0x2e);

// {799cabd6-3508-4db4-a3c7-569cd533deb1}   MF_MT_AVG_BIT_ERROR_RATE        {UINT32}
DEFINE_GUID(MF_MT_AVG_BIT_ERROR_RATE,
0x799cabd6, 0x3508, 0x4db4, 0xa3, 0xc7, 0x56, 0x9c, 0xd5, 0x33, 0xde, 0xb1);

// {c16eb52b-73a1-476f-8d62-839d6a020652}   MF_MT_MAX_KEYFRAME_SPACING      {UINT32}
DEFINE_GUID(MF_MT_MAX_KEYFRAME_SPACING,
0xc16eb52b, 0x73a1, 0x476f, 0x8d, 0x62, 0x83, 0x9d, 0x6a, 0x02, 0x06, 0x52);

// {b6bc765f-4c3b-40a4-bd51-2535b66fe09d}   MF_MT_USER_DATA                 {BLOB}
DEFINE_GUID(MF_MT_USER_DATA,
0xb6bc765f, 0x4c3b, 0x40a4, 0xbd, 0x51, 0x25, 0x35, 0xb6, 0x6f, 0xe0, 0x9d);

//
// VIDEO - uncompressed format data
//

// {644b4e48-1e02-4516-b0eb-c01ca9d49ac6}   MF_MT_DEFAULT_STRIDE            {UINT32 (INT32)} // in bytes
DEFINE_GUID(MF_MT_DEFAULT_STRIDE,
0x644b4e48, 0x1e02, 0x4516, 0xb0, 0xeb, 0xc0, 0x1c, 0xa9, 0xd4, 0x9a, 0xc6);

// {6d283f42-9846-4410-afd9-654d503b1a54}   MF_MT_PALETTE                   {BLOB (array of MFPaletteEntry - usually 256)}
DEFINE_GUID(MF_MT_PALETTE,
0x6d283f42, 0x9846, 0x4410, 0xaf, 0xd9, 0x65, 0x4d, 0x50, 0x3b, 0x1a, 0x54);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP) */
#pragma endregion

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

//
// the following is only used for legacy data that was stuck at the end of the format block when the type
// was converted from a VIDEOINFOHEADER or VIDEOINFOHEADER2 block in an AM_MEDIA_TYPE.
//

// {73d1072d-1870-4174-a063-29ff4ff6c11e}
DEFINE_GUID(MF_MT_AM_FORMAT_TYPE,
0x73d1072d, 0x1870, 0x4174, 0xa0, 0x63, 0x29, 0xff, 0x4f, 0xf6, 0xc1, 0x1e);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#pragma region Application Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP)

//
// VIDEO - Generic compressed video extra data
//

// {ad76a80b-2d5c-4e0b-b375-64e520137036}   MF_MT_VIDEO_PROFILE             {UINT32}    This is an alias of  MF_MT_MPEG2_PROFILE       
DEFINE_GUID(MF_MT_VIDEO_PROFILE,
0xad76a80b, 0x2d5c, 0x4e0b, 0xb3, 0x75, 0x64, 0xe5, 0x20, 0x13, 0x70, 0x36);

// {96f66574-11c5-4015-8666-bff516436da7}   MF_MT_VIDEO_LEVEL               {UINT32}    This is an alias of  MF_MT_MPEG2_LEVEL        
DEFINE_GUID(MF_MT_VIDEO_LEVEL,
0x96f66574, 0x11c5, 0x4015, 0x86, 0x66, 0xbf, 0xf5, 0x16, 0x43, 0x6d, 0xa7);


//
// VIDEO - MPEG1/2 extra data
//

// {91f67885-4333-4280-97cd-bd5a6c03a06e}   MF_MT_MPEG_START_TIME_CODE      {UINT32}
DEFINE_GUID(MF_MT_MPEG_START_TIME_CODE,
0x91f67885, 0x4333, 0x4280, 0x97, 0xcd, 0xbd, 0x5a, 0x6c, 0x03, 0xa0, 0x6e);

// {ad76a80b-2d5c-4e0b-b375-64e520137036}   MF_MT_MPEG2_PROFILE             {UINT32 (oneof AM_MPEG2Profile)}
DEFINE_GUID(MF_MT_MPEG2_PROFILE,
0xad76a80b, 0x2d5c, 0x4e0b, 0xb3, 0x75, 0x64, 0xe5, 0x20, 0x13, 0x70, 0x36);

// {96f66574-11c5-4015-8666-bff516436da7}   MF_MT_MPEG2_LEVEL               {UINT32 (oneof AM_MPEG2Level)}
DEFINE_GUID(MF_MT_MPEG2_LEVEL,
0x96f66574, 0x11c5, 0x4015, 0x86, 0x66, 0xbf, 0xf5, 0x16, 0x43, 0x6d, 0xa7);

// {31e3991d-f701-4b2f-b426-8ae3bda9e04b}   MF_MT_MPEG2_FLAGS               {UINT32 (anyof AMMPEG2_xxx flags)}
DEFINE_GUID(MF_MT_MPEG2_FLAGS,
0x31e3991d, 0xf701, 0x4b2f, 0xb4, 0x26, 0x8a, 0xe3, 0xbd, 0xa9, 0xe0, 0x4b);

// {3c036de7-3ad0-4c9e-9216-ee6d6ac21cb3}   MF_MT_MPEG_SEQUENCE_HEADER      {BLOB}
DEFINE_GUID(MF_MT_MPEG_SEQUENCE_HEADER,
0x3c036de7, 0x3ad0, 0x4c9e, 0x92, 0x16, 0xee, 0x6d, 0x6a, 0xc2, 0x1c, 0xb3);

// {A20AF9E8-928A-4B26-AAA9-F05C74CAC47C}   MF_MT_MPEG2_STANDARD            {UINT32 (0 for default MPEG2, 1  to use ATSC standard, 2 to use DVB standard, 3 to use ARIB standard)}
DEFINE_GUID(MF_MT_MPEG2_STANDARD, 
0xa20af9e8, 0x928a, 0x4b26, 0xaa, 0xa9, 0xf0, 0x5c, 0x74, 0xca, 0xc4, 0x7c);

// {5229BA10-E29D-4F80-A59C-DF4F180207D2}   MF_MT_MPEG2_TIMECODE            {UINT32 (0 for no timecode, 1 to append an 4 byte timecode to the front of each transport packet)}
DEFINE_GUID(MF_MT_MPEG2_TIMECODE, 
0x5229ba10, 0xe29d, 0x4f80, 0xa5, 0x9c, 0xdf, 0x4f, 0x18, 0x2, 0x7, 0xd2);

// {825D55E4-4F12-4197-9EB3-59B6E4710F06}   MF_MT_MPEG2_CONTENT_PACKET      {UINT32 (0 for no content packet, 1 to append a 14 byte Content Packet header according to the ARIB specification to the beginning a transport packet at 200-1000 ms intervals.)}
DEFINE_GUID(MF_MT_MPEG2_CONTENT_PACKET, 
0x825d55e4, 0x4f12, 0x4197, 0x9e, 0xb3, 0x59, 0xb6, 0xe4, 0x71, 0xf, 0x6);

//
// VIDEO - H264 extra data
//

// {F5929986-4C45-4FBB-BB49-6CC534D05B9B}  {UINT32, UVC 1.5 H.264 format descriptor: bMaxCodecConfigDelay}
DEFINE_GUID(MF_MT_H264_MAX_CODEC_CONFIG_DELAY,
0xf5929986, 0x4c45, 0x4fbb, 0xbb, 0x49, 0x6c, 0xc5, 0x34, 0xd0, 0x5b, 0x9b);

// {C8BE1937-4D64-4549-8343-A8086C0BFDA5} {UINT32, UVC 1.5 H.264 format descriptor: bmSupportedSliceModes}
DEFINE_GUID(MF_MT_H264_SUPPORTED_SLICE_MODES,
0xc8be1937, 0x4d64, 0x4549, 0x83, 0x43, 0xa8, 0x8, 0x6c, 0xb, 0xfd, 0xa5);

// {89A52C01-F282-48D2-B522-22E6AE633199} {UINT32, UVC 1.5 H.264 format descriptor: bmSupportedSyncFrameTypes}
DEFINE_GUID(MF_MT_H264_SUPPORTED_SYNC_FRAME_TYPES,
0x89a52c01, 0xf282, 0x48d2, 0xb5, 0x22, 0x22, 0xe6, 0xae, 0x63, 0x31, 0x99);

// {E3854272-F715-4757-BA90-1B696C773457} {UINT32, UVC 1.5 H.264 format descriptor: bResolutionScaling}
DEFINE_GUID(MF_MT_H264_RESOLUTION_SCALING,
0xe3854272, 0xf715, 0x4757, 0xba, 0x90, 0x1b, 0x69, 0x6c, 0x77, 0x34, 0x57);

// {9EA2D63D-53F0-4A34-B94E-9DE49A078CB3} {UINT32, UVC 1.5 H.264 format descriptor: bSimulcastSupport}
DEFINE_GUID(MF_MT_H264_SIMULCAST_SUPPORT,
0x9ea2d63d, 0x53f0, 0x4a34, 0xb9, 0x4e, 0x9d, 0xe4, 0x9a, 0x7, 0x8c, 0xb3);

// {6A8AC47E-519C-4F18-9BB3-7EEAAEA5594D} {UINT32, UVC 1.5 H.264 format descriptor: bmSupportedRateControlModes}
DEFINE_GUID(MF_MT_H264_SUPPORTED_RATE_CONTROL_MODES,
0x6a8ac47e, 0x519c, 0x4f18, 0x9b, 0xb3, 0x7e, 0xea, 0xae, 0xa5, 0x59, 0x4d);

// {45256D30-7215-4576-9336-B0F1BCD59BB2}  {Blob of size 20 * sizeof(WORD), UVC 1.5 H.264 format descriptor: wMaxMBperSec*}
DEFINE_GUID(MF_MT_H264_MAX_MB_PER_SEC,
0x45256d30, 0x7215, 0x4576, 0x93, 0x36, 0xb0, 0xf1, 0xbc, 0xd5, 0x9b, 0xb2);

// {60B1A998-DC01-40CE-9736-ABA845A2DBDC}         {UINT32, UVC 1.5 H.264 frame descriptor: bmSupportedUsages}
DEFINE_GUID(MF_MT_H264_SUPPORTED_USAGES,
0x60b1a998, 0xdc01, 0x40ce, 0x97, 0x36, 0xab, 0xa8, 0x45, 0xa2, 0xdb, 0xdc);

// {BB3BD508-490A-11E0-99E4-1316DFD72085}         {UINT32, UVC 1.5 H.264 frame descriptor: bmCapabilities}
DEFINE_GUID(MF_MT_H264_CAPABILITIES,
0xbb3bd508, 0x490a, 0x11e0, 0x99, 0xe4, 0x13, 0x16, 0xdf, 0xd7, 0x20, 0x85);

// {F8993ABE-D937-4A8F-BBCA-6966FE9E1152}         {UINT32, UVC 1.5 H.264 frame descriptor: bmSVCCapabilities}
DEFINE_GUID(MF_MT_H264_SVC_CAPABILITIES,
0xf8993abe, 0xd937, 0x4a8f, 0xbb, 0xca, 0x69, 0x66, 0xfe, 0x9e, 0x11, 0x52);

// {359CE3A5-AF00-49CA-A2F4-2AC94CA82B61}         {UINT32, UVC 1.5 H.264 Probe/Commit Control: bUsage}
DEFINE_GUID(MF_MT_H264_USAGE,
0x359ce3a5, 0xaf00, 0x49ca, 0xa2, 0xf4, 0x2a, 0xc9, 0x4c, 0xa8, 0x2b, 0x61);

//{705177D8-45CB-11E0-AC7D-B91CE0D72085}          {UINT32, UVC 1.5 H.264 Probe/Commit Control: bmRateControlModes}
DEFINE_GUID(MF_MT_H264_RATE_CONTROL_MODES,
0x705177d8, 0x45cb, 0x11e0, 0xac, 0x7d, 0xb9, 0x1c, 0xe0, 0xd7, 0x20, 0x85);

//{85E299B2-90E3-4FE8-B2F5-C067E0BFE57A}          {UINT64, UVC 1.5 H.264 Probe/Commit Control: bmLayoutPerStream}
DEFINE_GUID(MF_MT_H264_LAYOUT_PER_STREAM,
0x85e299b2, 0x90e3, 0x4fe8, 0xb2, 0xf5, 0xc0, 0x67, 0xe0, 0xbf, 0xe5, 0x7a);

//
// INTERLEAVED - DV extra data
//
// {84bd5d88-0fb8-4ac8-be4b-a8848bef98f3}   MF_MT_DV_AAUX_SRC_PACK_0        {UINT32}
DEFINE_GUID(MF_MT_DV_AAUX_SRC_PACK_0,
0x84bd5d88, 0x0fb8, 0x4ac8, 0xbe, 0x4b, 0xa8, 0x84, 0x8b, 0xef, 0x98, 0xf3);

// {f731004e-1dd1-4515-aabe-f0c06aa536ac}   MF_MT_DV_AAUX_CTRL_PACK_0       {UINT32}
DEFINE_GUID(MF_MT_DV_AAUX_CTRL_PACK_0,
0xf731004e, 0x1dd1, 0x4515, 0xaa, 0xbe, 0xf0, 0xc0, 0x6a, 0xa5, 0x36, 0xac);

// {720e6544-0225-4003-a651-0196563a958e}   MF_MT_DV_AAUX_SRC_PACK_1        {UINT32}
DEFINE_GUID(MF_MT_DV_AAUX_SRC_PACK_1,
0x720e6544, 0x0225, 0x4003, 0xa6, 0x51, 0x01, 0x96, 0x56, 0x3a, 0x95, 0x8e);

// {cd1f470d-1f04-4fe0-bfb9-d07ae0386ad8}   MF_MT_DV_AAUX_CTRL_PACK_1       {UINT32}
DEFINE_GUID(MF_MT_DV_AAUX_CTRL_PACK_1,
0xcd1f470d, 0x1f04, 0x4fe0, 0xbf, 0xb9, 0xd0, 0x7a, 0xe0, 0x38, 0x6a, 0xd8);

// {41402d9d-7b57-43c6-b129-2cb997f15009}   MF_MT_DV_VAUX_SRC_PACK          {UINT32}
DEFINE_GUID(MF_MT_DV_VAUX_SRC_PACK,
0x41402d9d, 0x7b57, 0x43c6, 0xb1, 0x29, 0x2c, 0xb9, 0x97, 0xf1, 0x50, 0x09);

// {2f84e1c4-0da1-4788-938e-0dfbfbb34b48}   MF_MT_DV_VAUX_CTRL_PACK         {UINT32}
DEFINE_GUID(MF_MT_DV_VAUX_CTRL_PACK,
0x2f84e1c4, 0x0da1, 0x4788, 0x93, 0x8e, 0x0d, 0xfb, 0xfb, 0xb3, 0x4b, 0x48);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP) */
#pragma endregion

#if (WINVER >= _WIN32_WINNT_WIN7)

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

//
// ARBITRARY
//

//
// MT_ARBITRARY_HEADER stores information about the format of an arbitrary media type
//
typedef struct _MT_ARBITRARY_HEADER
{
    GUID majortype;
    GUID subtype;
    BOOL bFixedSizeSamples;
    BOOL bTemporalCompression;
    ULONG lSampleSize;
    GUID formattype;
}
MT_ARBITRARY_HEADER;

// {9E6BD6F5-0109-4f95-84AC-9309153A19FC}   MF_MT_ARBITRARY_HEADER          {MT_ARBITRARY_HEADER}
DEFINE_GUID(MF_MT_ARBITRARY_HEADER,
0x9e6bd6f5, 0x109, 0x4f95, 0x84, 0xac, 0x93, 0x9, 0x15, 0x3a, 0x19, 0xfc );

// {5A75B249-0D7D-49a1-A1C3-E0D87F0CADE5}   MF_MT_ARBITRARY_FORMAT          {Blob}
DEFINE_GUID(MF_MT_ARBITRARY_FORMAT,
0x5a75b249, 0xd7d, 0x49a1, 0xa1, 0xc3, 0xe0, 0xd8, 0x7f, 0xc, 0xad, 0xe5);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#pragma region Application Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP)

//
// IMAGE
//
// {ED062CF4-E34E-4922-BE99-934032133D7C}   MF_MT_IMAGE_LOSS_TOLERANT       {UINT32 (BOOL)}
DEFINE_GUID(MF_MT_IMAGE_LOSS_TOLERANT, 
0xed062cf4, 0xe34e, 0x4922, 0xbe, 0x99, 0x93, 0x40, 0x32, 0x13, 0x3d, 0x7c);


//
// MPEG-4 Media Type Attributes
//
// {261E9D83-9529-4B8F-A111-8B9C950A81A9}   MF_MT_MPEG4_SAMPLE_DESCRIPTION   {BLOB}
DEFINE_GUID(MF_MT_MPEG4_SAMPLE_DESCRIPTION,
0x261e9d83, 0x9529, 0x4b8f, 0xa1, 0x11, 0x8b, 0x9c, 0x95, 0x0a, 0x81, 0xa9);

// {9aa7e155-b64a-4c1d-a500-455d600b6560}   MF_MT_MPEG4_CURRENT_SAMPLE_ENTRY {UINT32}
DEFINE_GUID(MF_MT_MPEG4_CURRENT_SAMPLE_ENTRY,
0x9aa7e155, 0xb64a, 0x4c1d, 0xa5, 0x00, 0x45, 0x5d, 0x60, 0x0b, 0x65, 0x60);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP) */
#pragma endregion

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

//
// Save original format information for AVI and WAV files
//
// {d7be3fe0-2bc7-492d-b843-61a1919b70c3}   MF_MT_ORIGINAL_4CC               (UINT32)
DEFINE_GUID(MF_MT_ORIGINAL_4CC, 
0xd7be3fe0, 0x2bc7, 0x492d, 0xb8, 0x43, 0x61, 0xa1, 0x91, 0x9b, 0x70, 0xc3);

// {8cbbc843-9fd9-49c2-882f-a72586c408ad}   MF_MT_ORIGINAL_WAVE_FORMAT_TAG   (UINT32)
DEFINE_GUID(MF_MT_ORIGINAL_WAVE_FORMAT_TAG,
0x8cbbc843, 0x9fd9, 0x49c2, 0x88, 0x2f, 0xa7, 0x25, 0x86, 0xc4, 0x08, 0xad);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#pragma region Application Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP)

//
// Video Capture Media Type Attributes
//

// {D2E7558C-DC1F-403f-9A72-D28BB1EB3B5E}   MF_MT_FRAME_RATE_RANGE_MIN      {UINT64 (HI32(Numerator),LO32(Denominator))}
DEFINE_GUID(MF_MT_FRAME_RATE_RANGE_MIN, 
0xd2e7558c, 0xdc1f, 0x403f, 0x9a, 0x72, 0xd2, 0x8b, 0xb1, 0xeb, 0x3b, 0x5e);

// {E3371D41-B4CF-4a05-BD4E-20B88BB2C4D6}   MF_MT_FRAME_RATE_RANGE_MAX      {UINT64 (HI32(Numerator),LO32(Denominator))}
DEFINE_GUID(MF_MT_FRAME_RATE_RANGE_MAX, 
0xe3371d41, 0xb4cf, 0x4a05, 0xbd, 0x4e, 0x20, 0xb8, 0x8b, 0xb2, 0xc4, 0xd6);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP) */
#pragma endregion

#endif // (WINVER >= _WIN32_WINNT_WIN7)

#pragma region Application Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP)

#if (WINVER >= _WIN32_WINNT_WIN8)
// {9C27891A-ED7A-40e1-88E8-B22727A024EE}   MF_LOW_LATENCY                  {UINT32 (BOOL)}
// Same GUID as CODECAPI_AVLowLatencyMode  
DEFINE_GUID(MF_LOW_LATENCY,
0x9c27891a, 0xed7a, 0x40e1, 0x88, 0xe8, 0xb2, 0x27, 0x27, 0xa0, 0x24, 0xee);

// {E3F2E203-D445-4B8C-9211-AE390D3BA017}  {UINT32} Maximum macroblocks per second that can be handled by MFT
DEFINE_GUID(MF_VIDEO_MAX_MB_PER_SEC,
0xe3f2e203, 0xd445, 0x4b8c, 0x92, 0x11, 0xae, 0x39, 0xd, 0x3b, 0xa0, 0x17);
#endif // (WINVER >= _WIN32_WINNT_WIN8)


////////////////////////////////////////////////////////////////////////////////
///////////////////////////////  Media Type GUIDs //////////////////////////////
////////////////////////////////////////////////////////////////////////////////


//
// Major types
//
DEFINE_GUID(MFMediaType_Default,
0x81A412E6, 0x8103, 0x4B06, 0x85, 0x7F, 0x18, 0x62, 0x78, 0x10, 0x24, 0xAC);
DEFINE_GUID(MFMediaType_Audio,
0x73647561, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xAA, 0x00, 0x38, 0x9B, 0x71);
DEFINE_GUID(MFMediaType_Video,
0x73646976, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xAA, 0x00, 0x38, 0x9B, 0x71);
DEFINE_GUID(MFMediaType_Protected,
0x7b4b6fe6, 0x9d04, 0x4494, 0xbe, 0x14, 0x7e, 0x0b, 0xd0, 0x76, 0xc8, 0xe4);
DEFINE_GUID(MFMediaType_SAMI,
0xe69669a0, 0x3dcd, 0x40cb, 0x9e, 0x2e, 0x37, 0x08, 0x38, 0x7c, 0x06, 0x16);
DEFINE_GUID(MFMediaType_Script,
0x72178C22, 0xE45B, 0x11D5, 0xBC, 0x2A, 0x00, 0xB0, 0xD0, 0xF3, 0xF4, 0xAB);
DEFINE_GUID(MFMediaType_Image,
0x72178C23, 0xE45B, 0x11D5, 0xBC, 0x2A, 0x00, 0xB0, 0xD0, 0xF3, 0xF4, 0xAB);
DEFINE_GUID(MFMediaType_HTML,
0x72178C24, 0xE45B, 0x11D5, 0xBC, 0x2A, 0x00, 0xB0, 0xD0, 0xF3, 0xF4, 0xAB);
DEFINE_GUID(MFMediaType_Binary,
0x72178C25, 0xE45B, 0x11D5, 0xBC, 0x2A, 0x00, 0xB0, 0xD0, 0xF3, 0xF4, 0xAB);
DEFINE_GUID(MFMediaType_FileTransfer,
0x72178C26, 0xE45B, 0x11D5, 0xBC, 0x2A, 0x00, 0xB0, 0xD0, 0xF3, 0xF4, 0xAB);
DEFINE_GUID(MFMediaType_Stream,
0xe436eb83, 0x524f, 0x11ce, 0x9f, 0x53, 0x00, 0x20, 0xaf, 0x0b, 0xa7, 0x70);

//
// Image subtypes (MFMediaType_Image major type)
//
// JPEG subtype: same as GUID_ContainerFormatJpeg 
DEFINE_GUID(MFImageFormat_JPEG,
0x19e4a5aa, 0x5662, 0x4fc5, 0xa0, 0xc0, 0x17, 0x58, 0x02, 0x8e, 0x10, 0x57);

// RGB32 subtype: same as MFVideoFormat_RGB32 
DEFINE_GUID(MFImageFormat_RGB32,
0x00000016, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71);

//
// MPEG2 Stream subtypes (MFMediaType_Stream major type)
//
DEFINE_GUID(MFStreamFormat_MPEG2Transport,
0xe06d8023, 0xdb46, 0x11cf, 0xb4, 0xd1, 0x00, 0x80, 0x5f, 0x6c, 0xbb, 0xea);
DEFINE_GUID(MFStreamFormat_MPEG2Program,
0x263067d1, 0xd330, 0x45dc, 0xb6, 0x69, 0x34, 0xd9, 0x86, 0xe4, 0xe3, 0xe1);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP) */
#pragma endregion

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)
//
// Representations
//
DEFINE_GUID(AM_MEDIA_TYPE_REPRESENTATION,
0xe2e42ad2, 0x132c, 0x491e, 0xa2, 0x68, 0x3c, 0x7c, 0x2d, 0xca, 0x18, 0x1f);
DEFINE_GUID(FORMAT_MFVideoFormat,
0xaed4ab2d, 0x7326, 0x43cb, 0x94, 0x64, 0xc8, 0x79, 0xca, 0xb9, 0xc4, 0x3d);


///////////////////////////////////////////////////////////////////////////////////////////////////////////////  Media Type functions //////////////////////////
////////////////////////////////////////////////////////////////////////////////

//
// Forward declaration
//
struct tagVIDEOINFOHEADER;
typedef struct tagVIDEOINFOHEADER VIDEOINFOHEADER;
struct tagVIDEOINFOHEADER2;
typedef struct tagVIDEOINFOHEADER2 VIDEOINFOHEADER2;
struct tagMPEG1VIDEOINFO;
typedef struct tagMPEG1VIDEOINFO MPEG1VIDEOINFO;
struct tagMPEG2VIDEOINFO;
typedef struct tagMPEG2VIDEOINFO MPEG2VIDEOINFO;
struct _AMMediaType;
typedef struct _AMMediaType AM_MEDIA_TYPE;

STDAPI
MFValidateMediaTypeSize(
    _In_                    GUID    FormatType,
    _In_reads_bytes_opt_(cbSize) UINT8*  pBlock,
    _In_                    UINT32  cbSize
    );

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#pragma region Application Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP)

STDAPI
MFCreateMediaType(
    _Outptr_ IMFMediaType**  ppMFType
    );

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP) */
#pragma endregion

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

STDAPI
MFCreateMFVideoFormatFromMFMediaType(
    _In_        IMFMediaType*           pMFType,
    _Out_       MFVIDEOFORMAT**         ppMFVF, // must be deleted with CoTaskMemFree
    _Out_opt_   UINT32*                 pcbSize
    );

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#pragma region Application Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP)

typedef enum _MFWaveFormatExConvertFlags {
    MFWaveFormatExConvertFlag_Normal            = 0,
    MFWaveFormatExConvertFlag_ForceExtensible   = 1
} MFWaveFormatExConvertFlags;
#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP) */
#pragma endregion

#ifdef __cplusplus

//
// declarations with default parameters
//

#pragma region Application Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP)
STDAPI
MFCreateWaveFormatExFromMFMediaType(
    _In_        IMFMediaType*   pMFType,
    _Out_       WAVEFORMATEX**  ppWF,
    _Out_opt_   UINT32*         pcbSize,
    _In_        UINT32          Flags = MFWaveFormatExConvertFlag_Normal
    );
#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP) */
#pragma endregion

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

STDAPI
MFInitMediaTypeFromVideoInfoHeader(
    _In_                    IMFMediaType*           pMFType,
    _In_reads_bytes_(cbBufSize)  const VIDEOINFOHEADER*  pVIH,
    _In_                    UINT32                  cbBufSize,
    _In_opt_                const GUID*             pSubtype = NULL
    );

STDAPI
MFInitMediaTypeFromVideoInfoHeader2(
    _In_                    IMFMediaType*           pMFType,
    _In_reads_bytes_(cbBufSize)  const VIDEOINFOHEADER2* pVIH2,
    _In_                    UINT32                  cbBufSize,
    _In_opt_                const GUID*             pSubtype = NULL
    );

STDAPI
MFInitMediaTypeFromMPEG1VideoInfo(
    _In_                    IMFMediaType*           pMFType,
    _In_reads_bytes_(cbBufSize)  const MPEG1VIDEOINFO*   pMP1VI,
    _In_                    UINT32                  cbBufSize,
    _In_opt_                const GUID*             pSubtype = NULL
    );

STDAPI
MFInitMediaTypeFromMPEG2VideoInfo(
    _In_                    IMFMediaType*           pMFType,
    _In_reads_bytes_(cbBufSize)  const MPEG2VIDEOINFO*   pMP2VI,
    _In_                    UINT32                  cbBufSize,
    _In_opt_                const GUID*             pSubtype = NULL
    );

STDAPI
MFCalculateBitmapImageSize(
    _In_reads_bytes_(cbBufSize)  const BITMAPINFOHEADER* pBMIH,
    _In_                    UINT32                  cbBufSize,
    _Out_                   UINT32*                 pcbImageSize,
    _Out_opt_               BOOL*                   pbKnown = NULL
    );
#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#else /* cplusplus */

//
// same declarations without default parameters
//

#pragma region Application Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP)

STDAPI
MFCreateWaveFormatExFromMFMediaType(
    _In_        IMFMediaType*   pMFType,
    _Out_       WAVEFORMATEX**  ppWF,
    _Out_opt_   UINT32*         pcbSize,
    _In_        UINT32          Flags
    );
#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP) */
#pragma endregion

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)
STDAPI
MFInitMediaTypeFromVideoInfoHeader(
    _In_                    IMFMediaType*           pMFType,
    _In_reads_bytes_(cbBufSize)  const VIDEOINFOHEADER*  pVIH,
    _In_                    UINT32                  cbBufSize,
    _In_opt_                const GUID*             pSubtype
    );

STDAPI
MFInitMediaTypeFromVideoInfoHeader2(
    _In_                    IMFMediaType*           pMFType,
    _In_reads_bytes_(cbBufSize)  const VIDEOINFOHEADER2* pVIH2,
    _In_                    UINT32                  cbBufSize,
    _In_opt_                const GUID*             pSubtype
    );

STDAPI
MFInitMediaTypeFromMPEG1VideoInfo(
    _In_                    IMFMediaType*           pMFType,
    _In_reads_bytes_(cbBufSize)  const MPEG1VIDEOINFO*   pMP1VI,
    _In_                    UINT32                  cbBufSize,
    _In_opt_                const GUID*             pSubtype
    );

STDAPI
MFInitMediaTypeFromMPEG2VideoInfo(
    _In_                    IMFMediaType*           pMFType,
    _In_reads_bytes_(cbBufSize)  const MPEG2VIDEOINFO*   pMP2VI,
    _In_                    UINT32                  cbBufSize,
    _In_opt_                const GUID*             pSubtype
    );

STDAPI
MFCalculateBitmapImageSize(
    _In_reads_bytes_(cbBufSize)  const BITMAPINFOHEADER* pBMIH,
    _In_                    UINT32                  cbBufSize,
    _Out_                   UINT32*                 pcbImageSize,
    _Out_opt_               BOOL*                   pbKnown
    );
#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#endif /* cplusplus */

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

STDAPI
MFCalculateImageSize(
    _In_                    REFGUID                 guidSubtype,
    _In_                    UINT32                  unWidth,
    _In_                    UINT32                  unHeight,
    _Out_                   UINT32*                 pcbImageSize
    );

STDAPI
MFFrameRateToAverageTimePerFrame(
    _In_                    UINT32                  unNumerator,
    _In_                    UINT32                  unDenominator,
    _Out_                   UINT64*                 punAverageTimePerFrame
    );

STDAPI
MFAverageTimePerFrameToFrameRate(
    _In_                    UINT64                  unAverageTimePerFrame,
    _Out_                   UINT32*                 punNumerator,
    _Out_                   UINT32*                 punDenominator
    );

STDAPI
MFInitMediaTypeFromMFVideoFormat(
    _In_                    IMFMediaType*           pMFType,
    _In_reads_bytes_(cbBufSize)  const MFVIDEOFORMAT*    pMFVF,
    _In_                    UINT32                  cbBufSize
    );

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#pragma region Application Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP)
STDAPI
MFInitMediaTypeFromWaveFormatEx(
    _In_                    IMFMediaType*           pMFType,
    _In_reads_bytes_(cbBufSize)  const WAVEFORMATEX*     pWaveFormat,
    _In_                    UINT32                  cbBufSize
    );
#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP) */
#pragma endregion

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)
STDAPI
MFInitMediaTypeFromAMMediaType(
    _In_    IMFMediaType*           pMFType,
    _In_    const AM_MEDIA_TYPE*    pAMType
    );

STDAPI
MFInitAMMediaTypeFromMFMediaType(
    _In_    IMFMediaType*           pMFType,
    _In_    GUID                    guidFormatBlockType,
    _Inout_ AM_MEDIA_TYPE*          pAMType
    );

STDAPI
MFCreateAMMediaTypeFromMFMediaType(
    _In_    IMFMediaType*           pMFType,
    _In_    GUID                    guidFormatBlockType,
    _Inout_ AM_MEDIA_TYPE**         ppAMType // delete with DeleteMediaType
    );


//
// This function compares a full media type to a partial media type.
//
// A "partial" media type is one that is given out by a component as a possible
// media type it could accept. Many attributes may be unset, which represents
// a "don't care" status for that attribute.
//
// For example, a video effect may report that it supports YV12,
// but not want to specify a particular size. It simply creates a media type and sets
// the major type to MFMediaType_Video and the subtype to MEDIASUBTYPE_YV12.
//
// The comparison function succeeds if the partial type contains at least a major type,
// and all of the attributes in the partial type exist in the full type and are set to
// the same value.
//
STDAPI_(BOOL)
MFCompareFullToPartialMediaType(
    _In_    IMFMediaType*   pMFTypeFull,
    _In_    IMFMediaType*   pMFTypePartial
    );


#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#pragma region Application Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP)
STDAPI
MFWrapMediaType(
    _In_    IMFMediaType*    pOrig,
    _In_    REFGUID          MajorType,
    _In_    REFGUID          SubType,
    _Out_   IMFMediaType **  ppWrap
    );



STDAPI
MFUnwrapMediaType(
    _In_    IMFMediaType*    pWrap,
    _Out_   IMFMediaType **  ppOrig
    );

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP) */
#pragma endregion

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

//
// MFCreateVideoMediaType
//

#ifdef _KSMEDIA_
STDAPI MFCreateVideoMediaTypeFromVideoInfoHeader(
    _In_ const KS_VIDEOINFOHEADER* pVideoInfoHeader,
    DWORD cbVideoInfoHeader,
    DWORD dwPixelAspectRatioX,
    DWORD dwPixelAspectRatioY,
    MFVideoInterlaceMode InterlaceMode,
    QWORD VideoFlags,
    _In_opt_ const GUID * pSubtype,
    _Out_ IMFVideoMediaType** ppIVideoMediaType
    );

STDAPI MFCreateVideoMediaTypeFromVideoInfoHeader2(
    _In_ const KS_VIDEOINFOHEADER2* pVideoInfoHeader,
    DWORD cbVideoInfoHeader,
    QWORD AdditionalVideoFlags,
    _In_opt_ const GUID * pSubtype,
    _Out_ IMFVideoMediaType** ppIVideoMediaType
    );

#endif

STDAPI MFCreateVideoMediaType(
    _In_ const MFVIDEOFORMAT* pVideoFormat,
    _Out_ IMFVideoMediaType** ppIVideoMediaType
    );

STDAPI MFCreateVideoMediaTypeFromSubtype(
    _In_ const GUID * pAMSubtype,
    _Out_ IMFVideoMediaType  **ppIVideoMediaType
    );

STDAPI_(BOOL)
MFIsFormatYUV(
    DWORD Format
    );

//
//  These depend on BITMAPINFOHEADER being defined
//
STDAPI MFCreateVideoMediaTypeFromBitMapInfoHeader(
    _In_ const BITMAPINFOHEADER* pbmihBitMapInfoHeader,
    DWORD dwPixelAspectRatioX,
    DWORD dwPixelAspectRatioY,
    MFVideoInterlaceMode InterlaceMode,
    QWORD VideoFlags,
    QWORD qwFramesPerSecondNumerator,
    QWORD qwFramesPerSecondDenominator,
    DWORD dwMaxBitRate,
    _Out_ IMFVideoMediaType** ppIVideoMediaType
    );

STDAPI MFGetStrideForBitmapInfoHeader(
    DWORD format,
    DWORD dwWidth,
    _Out_ LONG* pStride
    );

STDAPI MFGetPlaneSize(
    DWORD format,
    DWORD dwWidth,
    DWORD dwHeight,
    _Out_ DWORD* pdwPlaneSize
    );

#if (WINVER >= _WIN32_WINNT_WIN7)
//
// MFCreateVideoMediaTypeFromBitMapInfoHeaderEx
//

STDAPI MFCreateVideoMediaTypeFromBitMapInfoHeaderEx(
    _In_reads_bytes_(cbBitMapInfoHeader) const BITMAPINFOHEADER* pbmihBitMapInfoHeader,
    _In_    UINT32                  cbBitMapInfoHeader,
    DWORD dwPixelAspectRatioX,
    DWORD dwPixelAspectRatioY,
    MFVideoInterlaceMode InterlaceMode,
    QWORD VideoFlags,
    DWORD dwFramesPerSecondNumerator,
    DWORD dwFramesPerSecondDenominator,
    DWORD dwMaxBitRate,
    _Out_ IMFVideoMediaType** ppIVideoMediaType
    );
#endif // (WINVER >= _WIN32_WINNT_WIN7)

//
// MFCreateMediaTypeFromRepresentation
//

STDAPI MFCreateMediaTypeFromRepresentation(
    GUID guidRepresentation,
    _In_ LPVOID pvRepresentation,
    _Out_ IMFMediaType** ppIMediaType
    );


//
// MFCreateAudioMediaType
//


STDAPI
MFCreateAudioMediaType(
    _In_    const WAVEFORMATEX* pAudioFormat,
    _Out_   IMFAudioMediaType** ppIAudioMediaType
    );

DWORD
STDMETHODCALLTYPE
MFGetUncompressedVideoFormat(
    _In_    const MFVIDEOFORMAT* pVideoFormat
    );

STDAPI 
MFInitVideoFormat(
    _In_    MFVIDEOFORMAT*          pVideoFormat,
    _In_    MFStandardVideoFormat   type
    );

STDAPI
MFInitVideoFormat_RGB(
    _In_    MFVIDEOFORMAT*  pVideoFormat,
    _In_    DWORD           dwWidth,
    _In_    DWORD           dwHeight,
    _In_    DWORD           D3Dfmt /* 0 indicates sRGB */
    );

STDAPI 
MFConvertColorInfoToDXVA(
    _Out_ DWORD* pdwToDXVA,
    _In_  const MFVIDEOFORMAT* pFromFormat
    );
STDAPI
MFConvertColorInfoFromDXVA(
    _Inout_ MFVIDEOFORMAT* pToFormat,
    _In_    DWORD dwFromDXVA
    );

//
// Optimized stride copy function
//
#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#pragma region Application Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP)

STDAPI MFCopyImage(
    _Out_writes_bytes_(_Inexpressible_(abs(lDestStride) *  dwLines)) BYTE* pDest,
    LONG    lDestStride,
    _In_reads_bytes_(_Inexpressible_(abs(lSrcStride) * dwLines)) const BYTE* pSrc,
    LONG    lSrcStride,
    _Out_range_(<=, _Inexpressible_(min(abs(lSrcStride), abs(lDestStride))))  DWORD dwWidthInBytes,
    DWORD   dwLines
    );

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP) */
#pragma endregion

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

STDAPI MFConvertFromFP16Array(
    _Out_writes_(dwCount) float* pDest,
    _In_reads_(dwCount) const WORD* pSrc,
    DWORD dwCount
    );

STDAPI MFConvertToFP16Array(
    _Out_writes_(dwCount) WORD* pDest,
    _In_reads_(dwCount) const float* pSrc,
    DWORD dwCount
    );

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#pragma region Application Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP)

STDAPI MFCreate2DMediaBuffer( 
    _In_ DWORD dwWidth,
    _In_ DWORD dwHeight,
    _In_ DWORD dwFourCC,
    _In_ BOOL fBottomUp,
    _Out_ IMFMediaBuffer**    ppBuffer
    );


//
// Creates an optimal system memory media buffer from a media type
//
STDAPI MFCreateMediaBufferFromMediaType(
    _In_ IMFMediaType* pMediaType,
    _In_ LONGLONG llDuration,   // Sample Duration, needed for audio
    _In_ DWORD dwMinLength,     // 0 means optimized default 
    _In_ DWORD dwMinAlignment,  // 0 means optimized default
    _Outptr_ IMFMediaBuffer** ppBuffer 
    );

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP) */
#pragma endregion

///////////////////////////////////////////////////////////////////////////////////////////////////////////////  Attributes Utility functions ////////////////////////////
////////////////////////////////////////////////////////////////////////////////


#ifdef __cplusplus

#pragma region Application Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP)

//
// IMFAttributes inline UTILITY FUNCTIONS - used for IMFMediaType as well
//
inline
UINT32
HI32(UINT64 unPacked)
{
    return (UINT32)(unPacked >> 32);
}

inline
UINT32
LO32(UINT64 unPacked)
{
    return (UINT32)unPacked;
}

inline
UINT64
Pack2UINT32AsUINT64(UINT32 unHigh, UINT32 unLow)
{
    return ((UINT64)unHigh << 32) | unLow;
}

inline
void
Unpack2UINT32AsUINT64(UINT64 unPacked, _Out_ UINT32* punHigh, _Out_ UINT32* punLow)
{
    *punHigh = HI32(unPacked);
    *punLow = LO32(unPacked);
}

inline
UINT64
PackSize(UINT32 unWidth, UINT32 unHeight)
{
    return Pack2UINT32AsUINT64(unWidth, unHeight);
}

inline
void
UnpackSize(UINT64 unPacked, _Out_ UINT32* punWidth, _Out_ UINT32* punHeight)
{
    Unpack2UINT32AsUINT64(unPacked, punWidth, punHeight);
}

inline
UINT64
PackRatio(INT32 nNumerator, UINT32 unDenominator)
{
    return Pack2UINT32AsUINT64((UINT32)nNumerator, unDenominator);
}

inline
void
UnpackRatio(UINT64 unPacked, _Out_ INT32* pnNumerator, _Out_ UINT32* punDenominator)
{
    Unpack2UINT32AsUINT64(unPacked, (UINT32*)pnNumerator, punDenominator);
}


//
// "failsafe" inline get methods - return the stored value or return a default
//
inline
UINT32
MFGetAttributeUINT32(
    IMFAttributes*  pAttributes,
    REFGUID         guidKey,
    UINT32          unDefault
    )
{
    UINT32 unRet;
    if (FAILED(pAttributes->GetUINT32(guidKey, &unRet))) {
        unRet = unDefault;
    }
    return unRet;
}

inline
UINT64
MFGetAttributeUINT64(
    IMFAttributes*  pAttributes,
    REFGUID         guidKey,
    UINT64          unDefault
    )
{
    UINT64 unRet;
    if (FAILED(pAttributes->GetUINT64(guidKey, &unRet))) {
        unRet = unDefault;
    }
    return unRet;
}

inline
double
MFGetAttributeDouble(
    IMFAttributes*  pAttributes,
    REFGUID         guidKey,
    double          fDefault
    )
{
    double fRet;
    if (FAILED(pAttributes->GetDouble(guidKey, &fRet))) {
        fRet = fDefault;
    }
    return fRet;
}

//
// helpers for getting/setting ratios and sizes
//

inline
HRESULT
MFGetAttribute2UINT32asUINT64(
    IMFAttributes*  pAttributes,
    REFGUID         guidKey,
    _Out_ UINT32*   punHigh32,
    _Out_ UINT32*   punLow32
    )
{
    UINT64 unPacked;
    HRESULT hr = S_OK;

    hr = pAttributes->GetUINT64(guidKey, &unPacked);
    if (FAILED(hr)) {
        return hr;
    }
    Unpack2UINT32AsUINT64(unPacked, punHigh32, punLow32);

    return hr;
}

inline
HRESULT
MFSetAttribute2UINT32asUINT64(
    IMFAttributes*  pAttributes,
    REFGUID         guidKey,
    UINT32          unHigh32,
    UINT32          unLow32
    )
{
    return pAttributes->SetUINT64(guidKey, Pack2UINT32AsUINT64(unHigh32, unLow32));
}

inline
HRESULT
MFGetAttributeRatio(
    IMFAttributes*  pAttributes,
    REFGUID         guidKey,
    _Out_ UINT32*   punNumerator,
    _Out_ UINT32*   punDenominator
    )
{
    return MFGetAttribute2UINT32asUINT64(pAttributes, guidKey, punNumerator, punDenominator);
}

inline
HRESULT
MFGetAttributeSize(
    IMFAttributes*  pAttributes,
    REFGUID         guidKey,
    _Out_ UINT32*   punWidth,
    _Out_ UINT32*   punHeight
    )
{
    return MFGetAttribute2UINT32asUINT64(pAttributes, guidKey, punWidth, punHeight);
}

inline
HRESULT
MFSetAttributeRatio(
    IMFAttributes*  pAttributes,
    REFGUID         guidKey,
    UINT32          unNumerator,
    UINT32          unDenominator
    )
{
    return MFSetAttribute2UINT32asUINT64(pAttributes, guidKey, unNumerator, unDenominator);
}

inline
HRESULT
MFSetAttributeSize(
    IMFAttributes*  pAttributes,
    REFGUID         guidKey,
    UINT32          unWidth,
    UINT32          unHeight
    )
{
    return MFSetAttribute2UINT32asUINT64(pAttributes, guidKey, unWidth, unHeight);
}

#ifdef _INTSAFE_H_INCLUDED_
inline
HRESULT
MFGetAttributeString(
    IMFAttributes*  pAttributes,
    REFGUID         guidKey,
    _Outptr_ PWSTR *ppsz
    )
{
    UINT32 length;
    PWSTR psz = NULL;
    *ppsz = NULL;
    HRESULT hr = pAttributes->GetStringLength(guidKey, &length);
    // add NULL to length
    if (SUCCEEDED(hr)) {
        hr = UIntAdd(length, 1, &length);
    }
    if (SUCCEEDED(hr)) {
        size_t cb;
        hr = SizeTMult(length, sizeof(WCHAR), &cb);
        if( SUCCEEDED( hr ) )
        {
            psz = PWSTR( CoTaskMemAlloc( cb ) );
            if( !psz ) 
            {
                hr = E_OUTOFMEMORY;
            }
        }
    }
    if (SUCCEEDED(hr)) {
        hr = pAttributes->GetString(guidKey, psz, length, &length);
    }
    if (SUCCEEDED(hr)) {
        *ppsz = psz;
    } else {
        CoTaskMemFree(psz);
    }
    return hr;
}

#endif // _INTSAFE_H_INCLUDED_

///////////////////////////////  Collection         ////////////////////////////
////////////////////////////////////////////////////////////////////////////////

//
// Instantiates the MF-provided IMFCollection implementation
//
STDAPI MFCreateCollection(
    _Out_ IMFCollection **ppIMFCollection );


#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP) */
#pragma endregion

#endif

////////////////////////////////////////////////////////////////////////////////
////////////////////////////////  Memory Management ////////////////////////////
////////////////////////////////////////////////////////////////////////////////

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

//
// Heap alloc/free
//
typedef enum _EAllocationType
{
    eAllocationTypeDynamic,
    eAllocationTypeRT,
    eAllocationTypePageable,
    eAllocationTypeIgnore
}   EAllocationType;

EXTERN_C void* WINAPI MFHeapAlloc( size_t nSize,
                            ULONG dwFlags,
                            _In_opt_ char *pszFile,
                            int line,
                            EAllocationType eat);
EXTERN_C void WINAPI MFHeapFree( void * pv );

//////////////////////////       SourceResolver     ////////////////////////////
////////////////////////////////////////////////////////////////////////////////
DEFINE_GUID(CLSID_MFSourceResolver,
    0x90eab60f,
    0xe43a,
    0x4188,
    0xbc, 0xc4, 0xe4, 0x7f, 0xdf, 0x04, 0x86, 0x8c);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#if (WINVER >= _WIN32_WINNT_WIN7)
//  Return (a * b + d) / c
//  Returns _I64_MAX or LLONG_MIN on failure or _I64_MAX if mplat.dll is not available

#pragma region Application Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP)

LONGLONG WINAPI MFllMulDiv(LONGLONG a, LONGLONG b, LONGLONG c, LONGLONG d);

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_APP) */
#pragma endregion

#endif // (WINVER >= _WIN32_WINNT_WIN7)


//////////////////////////    Content Protection    ////////////////////////////
////////////////////////////////////////////////////////////////////////////////

#pragma region Desktop Family
#if WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP)

STDAPI MFGetContentProtectionSystemCLSID(
    _In_ REFGUID guidProtectionSystemID,
    _Out_ CLSID *pclsid );

#endif /* WINAPI_FAMILY_PARTITION(WINAPI_PARTITION_DESKTOP) */
#pragma endregion

#if defined(__cplusplus)
}
#endif

#endif //#if !defined(__MFAPI_H__)

