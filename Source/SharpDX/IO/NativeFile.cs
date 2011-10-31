// Copyright (c) 2010-2011 SharpDX - Alexandre Mutel
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace SharpDX.IO
{
    /// <summary>
    /// Windows File Helper.
    /// </summary>
    public class NativeFile 
    {

#if WIN8
        /// <summary>
        /// Creates the specified lp file name.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="desiredAccess">The desired access.</param>
        /// <param name="shareMode">The share mode.</param>
        /// <param name="mode">The creation disposition.</param>
        /// <param name="extendedParameters">The extended parameters.</param>
        /// <returns>A handle to the created file. IntPtr.Zero if failed.</returns>
        /// <unmanaged>CreateFile2</unmanaged>
        [DllImport("kernel32.dll", EntryPoint = "CreateFile2", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern IntPtr Create(
            string fileName,
            NativeFileAccess desiredAccess,
            NativeFileShare shareMode,
            NativeFileMode mode,
            IntPtr extendedParameters);
#else
        /// <summary>
        /// Creates the file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="desiredAccess">The desired access.</param>
        /// <param name="shareMode">The share mode.</param>
        /// <param name="securityAttributes">The security attributes.</param>
        /// <param name="mode">The creation disposition.</param>
        /// <param name="flagsAndOptions">The flags and attributes.</param>
        /// <param name="templateFile">The template file.</param>
        /// <returns>A handle to the created file. IntPtr.Zero if failed.</returns>
        /// <unmanaged>CreateFile</unmanaged>
        [DllImport("kernel32.dll", EntryPoint = "CreateFile", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern IntPtr Create(
            string fileName,
            NativeFileAccess desiredAccess,
            NativeFileShare shareMode,
            IntPtr securityAttributes,
            NativeFileMode mode,
            NativeFileOptions flagsAndOptions,
            IntPtr templateFile);
#endif

        /// <summary>
        /// Reads to a file.
        /// </summary>
        /// <param name="fileHandle">The file handle.</param>
        /// <param name="buffer">The buffer.</param>
        /// <param name="numberOfBytesToRead">The number of bytes to read.</param>
        /// <param name="numberOfBytesRead">The number of bytes read.</param>
        /// <param name="overlapped">The overlapped.</param>
        /// <returns>A Result</returns>
        /// <unmanaged>ReadFile</unmanaged>
        [DllImport("kernel32.dll", EntryPoint = "ReadFile", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern bool ReadFile(IntPtr fileHandle, IntPtr buffer, int numberOfBytesToRead, out int numberOfBytesRead, IntPtr overlapped);

        /// <summary>
        /// Writes to a file.
        /// </summary>
        /// <param name="fileHandle">The file handle.</param>
        /// <param name="buffer">The buffer.</param>
        /// <param name="numberOfBytesToRead">The number of bytes to read.</param>
        /// <param name="numberOfBytesRead">The number of bytes read.</param>
        /// <param name="overlapped">The overlapped.</param>
        /// <returns>A Result</returns>
        /// <unmanaged>WriteFile</unmanaged>
        [DllImport("kernel32.dll", EntryPoint = "WriteFile", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern bool WriteFile(IntPtr fileHandle, IntPtr buffer, int numberOfBytesToRead, out int numberOfBytesRead, IntPtr overlapped);

        /// <summary>
        /// Sets the file pointer.
        /// </summary>
        /// <param name="handle">The handle.</param>
        /// <param name="distanceToMove">The distance to move.</param>
        /// <param name="distanceToMoveHigh">The distance to move high.</param>
        /// <param name="seekOrigin">The seek origin.</param>
        /// <returns></returns>
        /// <unmanaged>SetFilePointerEx</unmanaged>
        [DllImport("kernel32.dll", EntryPoint = "SetFilePointerEx", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern bool SetFilePointerEx(IntPtr handle, long distanceToMove, out long distanceToMoveHigh, SeekOrigin seekOrigin);

        /// <summary>
        /// Sets the end of file.
        /// </summary>
        /// <param name="handle">The handle.</param>
        /// <returns></returns>
        /// <unmanaged>SetEndOfFile</unmanaged>
        [DllImport("kernel32.dll", EntryPoint = "SetEndOfFile", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern bool SetEndOfFile(IntPtr handle);

#if WIN8

        private enum FILE_INFO_BY_HANDLE_CLASS : int
        {
            FileBasicInfo = 0,

            FileStandardInfo = 1,

            FileNameInfo = 2,

            FileRenameInfo = 3,

            FileDispositionInfo = 4,

            FileAllocationInfo = 5,

            FileEndOfFileInfo = 6,

            FileStreamInfo = 7,

            FileCompressionInfo = 8,

            FileAttributeTagInfo = 9,

            FileIdBothDirectoryInfo = 10, // 0xA
            FileIdBothDirectoryRestartInfo = 11, // 0xB
            FileIoPriorityHintInfo = 12, // 0xC
            FileRemoteProtocolInfo = 13, // 0xD
            FileFullDirectoryInfo = 14, // 0xE
            FileFullDirectoryRestartInfo = 15, // 0xF
            FileStorageInfo = 16, // 0x10
            FileAlignmentInfo = 17, // 0x11
            MaximumFileInfoByHandlesClass
        };

        [StructLayout(LayoutKind.Sequential)]
        private struct FILE_STANDARD_INFO
        {
            public long AllocationSize;

            public long EndOfFile;

            public int NumberOfLinks;

            public int DeletePending;

            public int Directory;
        };

        [DllImport("kernel32.dll", EntryPoint = "GetFileInformationByHandleEx", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool GetFileInformationByHandleEx(IntPtr handle, FILE_INFO_BY_HANDLE_CLASS FileInformationClass, IntPtr lpFileInformation, int dwBufferSize);

        /// <summary>
        /// Gets the size of the file.
        /// </summary>
        /// <param name="handle">The handle.</param>
        /// <param name="fileSize">Size of the file.</param>
        /// <returns></returns>
        /// <unmanaged>GetFileSizeEx</unmanaged>
        internal static bool GetFileSizeEx(IntPtr handle, out long fileSize)
        {
            FILE_STANDARD_INFO info;
            unsafe
            {
                var result = GetFileInformationByHandleEx(handle, FILE_INFO_BY_HANDLE_CLASS.FileStandardInfo, new IntPtr(&info), Utilities.SizeOf<FILE_STANDARD_INFO>());
                fileSize = info.EndOfFile;
                return result;
            }
        }

#else
        /// <summary>
        /// Gets the size of the file.
        /// </summary>
        /// <param name="handle">The handle.</param>
        /// <param name="fileSize">Size of the file.</param>
        /// <returns></returns>
        /// <unmanaged>GetFileSizeEx</unmanaged>
        [DllImport("kernel32.dll", EntryPoint = "GetFileSizeEx", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern bool GetFileSizeEx(IntPtr handle, out long fileSize);
#endif

    }
}
