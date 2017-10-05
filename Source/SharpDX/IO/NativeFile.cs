// Copyright (c) 2010-2014 SharpDX - Alexandre Mutel
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
using System.Text;

namespace SharpDX.IO
{
    /// <summary>
    /// Windows File Helper.
    /// </summary>
    public static class NativeFile 
    {
#if WINDOWS_UWP
        private const string KERNEL_FILE = "api-ms-win-core-file-l1-2-0.dll";
#else
        private const string KERNEL_FILE = "kernel32.dll";
#endif

        /// <summary>
        /// Checks if the specified file path exists.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns><c>true</c> if the specified file path exists, <c>false</c> otherwise</returns>
        public static bool Exists(string filePath)
        {
            try
            {
                WIN32_FILE_ATTRIBUTE_DATA data;
                if (GetFileAttributesEx(filePath, 0, out data))
                {
                    return true;
                }
            } catch {}
            return false;
        }

        /// <summary>
        /// Opens a binary file, reads the contents of the file into a byte array, and then closes the file.
        /// </summary>
        /// <param name="path">The file to open for reading. </param>
        /// <returns>A byte array containing the contents of the file.</returns>
        public static byte[] ReadAllBytes(string path)
        {
            byte[] buffer;
            using (var stream = new NativeFileStream(path, NativeFileMode.Open, NativeFileAccess.Read))
            {
                int offset = 0;
                long length = stream.Length;
                if (length > 0x7fffffffL)
                {
                    throw new IOException("File too long");
                }

                int count = (int)length;
                buffer = new byte[count];

                while (count > 0)
                {
                    int num4 = stream.Read(buffer, offset, count);
                    if (num4 == 0)
                    {
                        throw new EndOfStreamException();
                    }
                    offset += num4;
                    count -= num4;
                }
            }
            return buffer;
        }

        /// <summary>
        /// Opens a text file, reads all lines of the file, and then closes the file.
        /// </summary>
        /// <param name="path">The file to open for reading. </param>
        /// <returns>A string containing all lines of the file.</returns>
        public static string ReadAllText(string path)
        {
            return ReadAllText(path, Encoding.UTF8);
        }

        /// <summary>
        /// Opens a text file, reads all lines of the file, and then closes the file.
        /// </summary>
        /// <param name="path">The file to open for reading.</param>
        /// <param name="encoding">The encoding.</param>
        /// <param name="sharing">The sharing.</param>
        /// <returns>A string containing all lines of the file.</returns>
        public static string ReadAllText(string path, Encoding encoding, NativeFileShare sharing = NativeFileShare.Read)
        {
            using (var stream = new NativeFileStream(path, NativeFileMode.Open, NativeFileAccess.Read, sharing))
            {
                using (StreamReader reader = new StreamReader(stream, encoding, true, 0x400))
                {
                    return reader.ReadToEnd();
                }
            }
        }
#if WINDOWS_UWP
        internal struct CREATEFILE2_EXTENDED_PARAMETERS
        {
            public uint dwSize;
            public uint dwFileAttributes;
            public uint dwFileFlags;
            public uint dwSecurityQosFlags;
            public IntPtr lpSecurityAttributes;
            public IntPtr hTemplateFile;
        };


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
#endif

        [StructLayout(LayoutKind.Sequential)]
        internal struct FILETIME
        {
            public uint DateTimeLow;
            public uint DateTimeHigh;

            public DateTime ToDateTime()
            {
                return DateTime.FromFileTimeUtc((((long)DateTimeHigh) << 32) | ((uint)DateTimeLow));
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct WIN32_FILE_ATTRIBUTE_DATA
        {
            public uint FileAttributes;
            public FILETIME CreationTime;
            public FILETIME LastAccessTime;
            public FILETIME LastWriteTime;
            public uint FileSizeHigh;
            public uint FileSizeLow;
        }

        /// <summary>
        /// Gets the last write time access for the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>The last write time access</returns>
        public static DateTime GetLastWriteTime(string path)
        {
            WIN32_FILE_ATTRIBUTE_DATA data;
            if (GetFileAttributesEx(path, 0, out data))
            {
                return data.LastWriteTime.ToDateTime().ToLocalTime();
            }
            return new DateTime(0);
        }

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
        [DllImport(KERNEL_FILE, EntryPoint = "ReadFile", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern bool ReadFile(IntPtr fileHandle, IntPtr buffer, int numberOfBytesToRead, out int numberOfBytesRead, IntPtr overlapped);


        [DllImport(KERNEL_FILE, EntryPoint = "FlushFileBuffers", SetLastError = true)]
        internal static extern bool FlushFileBuffers(IntPtr hFile);

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
        [DllImport(KERNEL_FILE, EntryPoint = "WriteFile", SetLastError = true, CharSet = CharSet.Unicode)]
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
        [DllImport(KERNEL_FILE, EntryPoint = "SetFilePointerEx", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern bool SetFilePointerEx(IntPtr handle, long distanceToMove, out long distanceToMoveHigh, SeekOrigin seekOrigin);

        /// <summary>
        /// Sets the end of file.
        /// </summary>
        /// <param name="handle">The handle.</param>
        /// <returns></returns>
        /// <unmanaged>SetEndOfFile</unmanaged>
        [DllImport(KERNEL_FILE, EntryPoint = "SetEndOfFile", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern bool SetEndOfFile(IntPtr handle);

        [DllImport(KERNEL_FILE, EntryPoint = "GetFileAttributesExW", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool GetFileAttributesEx(string name, int fileInfoLevel, out WIN32_FILE_ATTRIBUTE_DATA lpFileInformation);

#if WINDOWS_UWP

        /// <summary>
        /// Creates the file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="desiredAccess">The desired access.</param>
        /// <param name="shareMode">The share mode.</param>
        /// <param name="mode">The creation disposition.</param>
        /// <param name="extendedParameters">The extended parameters.</param>
        /// <returns>A handle to the created file. IntPtr.Zero if failed.</returns>
        /// <unmanaged>CreateFile2</unmanaged>
        [DllImport(KERNEL_FILE, EntryPoint = "CreateFile2", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern IntPtr Create(
            string fileName,
            NativeFileAccess desiredAccess,
            NativeFileShare shareMode,
            NativeFileMode mode,
            IntPtr extendedParameters);


        [DllImport("api-ms-win-core-file-l2-1-0.dll", EntryPoint = "GetFileInformationByHandleEx", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern bool GetFileInformationByHandleEx(IntPtr handle, FILE_INFO_BY_HANDLE_CLASS FileInformationClass, IntPtr lpFileInformation, int dwBufferSize);
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
        [DllImport("kernel32.dll", EntryPoint = "CreateFile", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern IntPtr Create(
            string fileName,
            NativeFileAccess desiredAccess,
            NativeFileShare shareMode,
            IntPtr securityAttributes,
            NativeFileMode mode,
            NativeFileOptions flagsAndOptions,
            IntPtr templateFile);

        /// <summary>
        /// Gets the size of the file.
        /// </summary>
        /// <param name="handle">The handle.</param>
        /// <param name="fileSize">Size of the file.</param>
        /// <returns></returns>
        /// <unmanaged>GetFileSizeEx</unmanaged>
        [DllImport("kernel32.dll", EntryPoint = "GetFileSizeEx", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern bool GetFileSizeEx(IntPtr handle, out long fileSize);

#endif
    }
}
