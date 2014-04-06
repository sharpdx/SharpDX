﻿// Copyright (c) 2010-2013 SharpDX - Alexandre Mutel
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

using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;

[assembly:AssemblyCompany("Alexandre Mutel")]
[assembly:AssemblyCopyright("Copyright © 2010-2013 Alexandre Mutel")]

[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

[assembly:AssemblyVersion("2.5.1")]
[assembly:AssemblyFileVersion("2.5.1")]

[assembly: NeutralResourcesLanguage("en-us")]

#if DEBUG
[assembly:AssemblyConfiguration("Debug")]
#else
[assembly:AssemblyConfiguration("Release")]
#endif

[assembly:ComVisible(false)]

[assembly: Obfuscation(Feature = "Apply to type SharpDX.* when public and interface: renaming", Exclude = false, ApplyToMembers = true)]
[assembly: Obfuscation(Feature = "Apply to type SharpDX.* when struct: renaming", Exclude = false, ApplyToMembers = true)]
[assembly: Obfuscation(Feature = "Apply to type SharpDX.*: INotifyPropertyChanged heuristics", Exclude = true)]
[assembly: Obfuscation(Feature = "Apply to type SharpDX.* when enum: forced rename", Exclude = false)]
[assembly: Obfuscation(Feature = "Apply to type SharpDX.* when enum: enum values pruning", Exclude = false)]
[assembly: Obfuscation(Feature = "legacy xml serialization heuristics", Exclude = true)]
[assembly: Obfuscation(Feature = "ignore InternalsVisibleToAttribute", Exclude = false)]
