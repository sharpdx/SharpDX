#pragma once

typedef int IntPtr32;

namespace SharpDX
{

   namespace WP8
	{
		public ref class Interop sealed
		{
		private:
			Interop();
		public:
			// COM
			static IntPtr32 CoCreateInstanceFromApp();
			static IntPtr32 LoadPackagedLibrary();
			static IntPtr32 GetProcAddress();
			static IntPtr32 CreateFile2();
			static IntPtr32 ReadFile();
			static IntPtr32 FlushFileBuffers();
			static IntPtr32 WriteFile();
			static IntPtr32 SetFilePointerEx();
			static IntPtr32 GetFileInformationByHandleEx();
			static IntPtr32 FormatMessageW();
			static IntPtr32 CloseHandle();
			static IntPtr32 SetEndOfFile();
			// D3D11
			static IntPtr32 D3D11CreateDevice();
			// DXGI
			static IntPtr32 CreateDXGIFactory1();
			// XAudio2
			static IntPtr32 XAudio2Create();
			static IntPtr32 CreateFX();
			static IntPtr32 CreateAudioReverb();
			static IntPtr32 CreateAudioVolumeMeter();
			static IntPtr32 X3DAudioCalculate();
			static IntPtr32 X3DAudioInitialize();
			// Media Engine
			static IntPtr32 MFCreate2DMediaBuffer 					 ();
			static IntPtr32 MFCreateAlignedMemoryBuffer 			 ();
			static IntPtr32 MFCreateAsyncResult 					 ();
			static IntPtr32 MFCreateAttributes 						 ();
			static IntPtr32 MFCreateCollection 						 ();
			static IntPtr32 MFCreateDXGIDeviceManager 				 ();
			static IntPtr32 MFCreateDXGISurfaceBuffer 				 ();
			static IntPtr32 MFCreateMFByteStreamOnStreamEx 			 ();
			static IntPtr32 MFCreateMediaBufferFromMediaType 		 ();
			static IntPtr32 MFCreateMediaBufferWrapper 				 ();
			static IntPtr32 MFCreateMediaTypeFromProperties 		 ();
			static IntPtr32 MFCreateMemoryBuffer 					 ();
			static IntPtr32 MFCreatePropertiesFromMediaType 		 ();
			static IntPtr32 MFCreateSample 							 ();
			static IntPtr32 MFCreateStreamOnMFByteStreamEx 			 ();
			static IntPtr32 MFCreateVideoSampleAllocatorEx 			 ();
			static IntPtr32 MFDeserializeAttributesFromStream 		 ();
			static IntPtr32 MFGetAttributesAsBlob 					 ();
			static IntPtr32 MFGetAttributesAsBlobSize 				 ();
			static IntPtr32 MFInvokeCallback 						 ();
			static IntPtr32 MFLockDXGIDeviceManager 				 ();
			static IntPtr32 MFLockPlatform 							 ();
			static IntPtr32 MFSerializeAttributesToStream 			 ();
			static IntPtr32 MFShutdown 								 ();
			static IntPtr32 MFStartup 								 ();
			static IntPtr32 MFUnlockDXGIDeviceManager 				 ();
			static IntPtr32 MFUnlockPlatform 						 ();
			static IntPtr32 MFllMulDiv 								 ();
		};
	}
}