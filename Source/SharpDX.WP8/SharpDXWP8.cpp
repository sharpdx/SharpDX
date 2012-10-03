// SharpDXWP8.cpp
#include "pch.h"
#include "SharpDXWP8.h"

using namespace SharpDX::WP8;
using namespace Platform;

Interop::Interop()
{
}

// Win32
IntPtr32 Interop::CoCreateInstanceFromApp() { return (IntPtr32)::CoCreateInstanceFromApp;}
IntPtr32 Interop::LoadPackagedLibrary() { return (IntPtr32)::LoadPackagedLibrary;}
IntPtr32 Interop::GetProcAddress() { return (IntPtr32)::GetProcAddress;}
IntPtr32 Interop::CreateFile2() { return (IntPtr32)::CreateFile2;}
IntPtr32 Interop::ReadFile() { return (IntPtr32)::ReadFile;}
IntPtr32 Interop::FlushFileBuffers() { return (IntPtr32)::FlushFileBuffers;}
IntPtr32 Interop::WriteFile() { return (IntPtr32)::WriteFile;}
IntPtr32 Interop::SetFilePointerEx() { return (IntPtr32)::SetFilePointerEx;}
IntPtr32 Interop::GetFileInformationByHandleEx() { return (IntPtr32)::GetFileInformationByHandleEx;}
IntPtr32 Interop::FormatMessageW() { return (IntPtr32)::FormatMessageW;}
IntPtr32 Interop::CloseHandle() { return (IntPtr32)::CloseHandle;}
IntPtr32 Interop::SetEndOfFile() { return (IntPtr32)::SetEndOfFile;}

// D3D11
IntPtr32 Interop::D3D11CreateDevice() { return (IntPtr32)::D3D11CreateDevice;}
// DXGI
IntPtr32 Interop::CreateDXGIFactory1() { return (IntPtr32)::CreateDXGIFactory1;}
// XAudio2
IntPtr32 Interop::XAudio2Create() { return (IntPtr32)::XAudio2Create;}
IntPtr32 Interop::CreateFX() { return (IntPtr32)::CreateFX;}
IntPtr32 Interop::X3DAudioCalculate() { return (IntPtr32)::X3DAudioCalculate;}
IntPtr32 Interop::X3DAudioInitialize() { return (IntPtr32)::X3DAudioInitialize;}
// Media Engine
IntPtr32 Interop::MFCreate2DMediaBuffer 					 () { return (IntPtr32)::MFCreate2DMediaBuffer 					 ;}
IntPtr32 Interop::MFCreateAlignedMemoryBuffer 			 () { return (IntPtr32)::MFCreateAlignedMemoryBuffer 			 ;}
IntPtr32 Interop::MFCreateAsyncResult 					 () { return (IntPtr32)::MFCreateAsyncResult 					 ;}
IntPtr32 Interop::MFCreateAttributes 						 () { return (IntPtr32)::MFCreateAttributes 						 ;}
IntPtr32 Interop::MFCreateCollection 						 () { return (IntPtr32)::MFCreateCollection 						 ;}
IntPtr32 Interop::MFCreateDXGIDeviceManager 				 () { return (IntPtr32)::MFCreateDXGIDeviceManager 				 ;}
IntPtr32 Interop::MFCreateDXGISurfaceBuffer 				 () { return (IntPtr32)::MFCreateDXGISurfaceBuffer 				 ;}
IntPtr32 Interop::MFCreateMFByteStreamOnStreamEx 			 () { return (IntPtr32)::MFCreateMFByteStreamOnStreamEx 			 ;}
IntPtr32 Interop::MFCreateMediaBufferFromMediaType 		 () { return (IntPtr32)::MFCreateMediaBufferFromMediaType 		 ;}
IntPtr32 Interop::MFCreateMediaBufferWrapper 				 () { return (IntPtr32)::MFCreateMediaBufferWrapper 				 ;}
IntPtr32 Interop::MFCreateMediaTypeFromProperties 		 () { return (IntPtr32)::MFCreateMediaTypeFromProperties 		 ;}
IntPtr32 Interop::MFCreateMemoryBuffer 					 () { return (IntPtr32)::MFCreateMemoryBuffer 					 ;}
IntPtr32 Interop::MFCreatePropertiesFromMediaType 		 () { return (IntPtr32)::MFCreatePropertiesFromMediaType 		 ;}
IntPtr32 Interop::MFCreateSample 							 () { return (IntPtr32)::MFCreateSample 							 ;}
IntPtr32 Interop::MFCreateStreamOnMFByteStreamEx 			 () { return (IntPtr32)::MFCreateStreamOnMFByteStreamEx 			 ;}
IntPtr32 Interop::MFCreateVideoSampleAllocatorEx 			 () { return (IntPtr32)::MFCreateVideoSampleAllocatorEx 			 ;}
IntPtr32 Interop::MFDeserializeAttributesFromStream 		 () { return (IntPtr32)::MFDeserializeAttributesFromStream 		 ;}
IntPtr32 Interop::MFGetAttributesAsBlob 					 () { return (IntPtr32)::MFGetAttributesAsBlob 					 ;}
IntPtr32 Interop::MFGetAttributesAsBlobSize 				 () { return (IntPtr32)::MFGetAttributesAsBlobSize 				 ;}
IntPtr32 Interop::MFInvokeCallback 						 () { return (IntPtr32)::MFInvokeCallback 						 ;}
IntPtr32 Interop::MFLockDXGIDeviceManager 				 () { return (IntPtr32)::MFLockDXGIDeviceManager 				 ;}
IntPtr32 Interop::MFLockPlatform 							 () { return (IntPtr32)::MFLockPlatform 							 ;}
IntPtr32 Interop::MFSerializeAttributesToStream 			 () { return (IntPtr32)::MFSerializeAttributesToStream 			 ;}
IntPtr32 Interop::MFShutdown 								 () { return (IntPtr32)::MFShutdown 								 ;}
IntPtr32 Interop::MFStartup 								 () { return (IntPtr32)::MFStartup 								 ;}
IntPtr32 Interop::MFUnlockDXGIDeviceManager 				 () { return (IntPtr32)::MFUnlockDXGIDeviceManager 				 ;}
IntPtr32 Interop::MFUnlockPlatform 						 () { return (IntPtr32)::MFUnlockPlatform 						 ;}
IntPtr32 Interop::MFllMulDiv 								 () { return (IntPtr32)::MFllMulDiv 								 ;}


