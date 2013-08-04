// Guids.cs
// MUST match guids.h
using System;

namespace SharpDX.VisualStudio.Package
{
    static class GuidList
    {
        public const string guidSharpDX_VisualStudio_PackagePkgString = "c9f3eae3-5b73-4a71-a3b7-da6e1fc935b5";
        public const string guidSharpDX_VisualStudio_PackageCmdSetString = "c39b9a6a-a214-496c-9a2d-de3c0a2de2da";

        public static readonly Guid guidSharpDX_VisualStudio_PackageCmdSet = new Guid(guidSharpDX_VisualStudio_PackageCmdSetString);
    };
}