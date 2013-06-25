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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.IO;
using SharpDX.WIC;
using SharpDX.Windows;

namespace SharpDX.Toolkit.Graphics.Tests
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //var test = new TestBuffer();
            //test.AllTest();

            //var testTexture2D = new TestTexture2D();
            //testTexture2D.TestConstructors();

            //// Test Image
            //var testImage = new TestImage();
            //testImage.Initialize();
            //testImage.TestLoadAndSave();

            // Test Image
            //var testImage = new TestTexture();
            //testImage.Initialize();
            //testImage.TestLoadSave();

            //var testTexture = new TestTexture();
            //testTexture.Initialize();
            //testTexture.Test1DArrayAndMipmaps();

            //var testCompiler = new TestEffect();
            //testCompiler.TestMatrix();

            var testSpriteFont = new TestSpriteFont();
            testSpriteFont.TestSpriteFontDataLoad();

        }
    }
}