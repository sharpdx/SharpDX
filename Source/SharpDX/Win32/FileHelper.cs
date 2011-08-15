// Copyright (c) 2011 Silicon Studio

using System;
using System.IO;
using System.Runtime.InteropServices;

namespace SharpDX.Win32
{
    [Flags]
    public enum NativeFileAccess : uint
    {
        /// <summary>
        ///
        /// </summary>
        GenericRead = 0x80000000,
        /// <summary>
        ///
        /// </summary>
        GenericWrite = 0x40000000,
        /// <summary>
        ///
        /// </summary>
        GenericExecute = 0x20000000,
        /// <summary>
        ///
        /// </summary>
        GenericAll = 0x10000000
    }

    [Flags]
    public enum NativeFileShare : uint
    {
        None = 0x00000000,
        /// <summary>
        /// Enables subsequent open operations on an object to request read access.
        /// Otherwise, other processes cannot open the object if they request read access.
        /// If this flag is not specified, but the object has been opened for read access, the function fails.
        /// </summary>
        Read = 0x00000001,
        /// <summary>
        /// Enables subsequent open operations on an object to request write access.
        /// Otherwise, other processes cannot open the object if they request write access.
        /// If this flag is not specified, but the object has been opened for write access, the function fails.
        /// </summary>
        Write = 0x00000002,
        /// <summary>
        /// Enables subsequent open operations on an object to request delete access.
        /// Otherwise, other processes cannot open the object if they request delete access.
        /// If this flag is not specified, but the object has been opened for delete access, the function fails.
        /// </summary>
        Delete = 0x00000004
    }

    public enum NativeFileCreationDisposition : uint
    {
        /// <summary>
        /// Creates a new file. The function fails if a specified file exists.
        /// </summary>
        New = 1,
        /// <summary>
        /// Creates a new file, always.
        /// If a file exists, the function overwrites the file, clears the existing attributes, combines the specified file attributes,
        /// and flags with FILE_ATTRIBUTE_ARCHIVE, but does not set the security descriptor that the SECURITY_ATTRIBUTES structure specifies.
        /// </summary>
        CreateAlways = 2,
        /// <summary>
        /// Opens a file. The function fails if the file does not exist.
        /// </summary>
        OpenExisting = 3,
        /// <summary>
        /// Opens a file, always.
        /// If a file does not exist, the function creates a file as if dwCreationDisposition is CREATE_NEW.
        /// </summary>
        OpenAlways = 4,
        /// <summary>
        /// Opens a file and truncates it so that its size is 0 (zero) bytes. The function fails if the file does not exist.
        /// The calling process must open the file with the GENERIC_WRITE access right.
        /// </summary>
        TruncateExisting = 5
    }

    [Flags]
    public enum NativeFileAttributes : uint
    {
        Readonly = 0x00000001,
        Hidden = 0x00000002,
        System = 0x00000004,
        Directory = 0x00000010,
        Archive = 0x00000020,
        Device = 0x00000040,
        Normal = 0x00000080,
        Temporary = 0x00000100,
        SparseFile = 0x00000200,
        ReparsePoint = 0x00000400,
        Compressed = 0x00000800,
        Offline = 0x00001000,
        NotContentIndexed = 0x00002000,
        Encrypted = 0x00004000,
        Write_Through = 0x80000000,
        Overlapped = 0x40000000,
        NoBuffering = 0x20000000,
        RandomAccess = 0x10000000,
        SequentialScan = 0x08000000,
        DeleteOnClose = 0x04000000,
        BackupSemantics = 0x02000000,
        PosixSemantics = 0x01000000,
        OpenReparsePoint = 0x00200000,
        OpenNoRecall = 0x00100000,
        FirstPipeInstance = 0x00080000
    }

    /// <summary>
    /// Windows File Helper.
    /// </summary>
    public class FileHelper
    {
        /// <summary>
        /// Creates the file.
        /// </summary>
        /// <param name="lpFileName">Name of the lp file.</param>
        /// <param name="dwDesiredAccess">The dw desired access.</param>
        /// <param name="dwShareMode">The dw share mode.</param>
        /// <param name="lpSecurityAttributes">The lp security attributes.</param>
        /// <param name="dwCreationDisposition">The dw creation disposition.</param>
        /// <param name="dwFlagsAndAttributes">The dw flags and attributes.</param>
        /// <param name="hTemplateFile">The h template file.</param>
        /// <returns></returns>
        [DllImport("kernel32.dll", EntryPoint = "CreateFile", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr CreateFile(
           string lpFileName,
           NativeFileAccess dwDesiredAccess,
           NativeFileShare dwShareMode,
           IntPtr lpSecurityAttributes,
           NativeFileCreationDisposition dwCreationDisposition,
           NativeFileAttributes dwFlagsAndAttributes,
           IntPtr hTemplateFile);

        /// <summary>
        /// Converts the specified file access to a native file access.
        /// </summary>
        /// <param name="fileAccess">The file access.</param>
        /// <returns></returns>
        public static NativeFileAccess Convert(FileAccess fileAccess)
        {
            NativeFileAccess nativeFileAccess;
            switch (fileAccess)
            {
                case FileAccess.Read:
                    nativeFileAccess = NativeFileAccess.GenericRead;
                    break;
                case FileAccess.Write:
                    nativeFileAccess = NativeFileAccess.GenericWrite;
                    break;
                default:
                    nativeFileAccess = NativeFileAccess.GenericRead | NativeFileAccess.GenericWrite;
                    break;
            }
            return nativeFileAccess;
        }
    }
}
