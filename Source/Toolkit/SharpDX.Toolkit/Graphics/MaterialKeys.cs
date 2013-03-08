// Copyright (c) 2010-2012 SharpDX - Alexandre Mutel
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

namespace SharpDX.Toolkit.Graphics
{
    public class MaterialKeys
    {
        public readonly static MaterialKey<string> Name = new MaterialKey<string>("Name");
        public readonly static MaterialKey<bool> TwoSided = new MaterialKey<bool>("TwoSided");
        public readonly static MaterialKey<MaterialShadingMode> ShadingMode = new MaterialKey<MaterialShadingMode>("ShadingMode");
        public readonly static MaterialKey<bool> Wireframe = new MaterialKey<bool>("Wireframe");
        public readonly static MaterialKey<MaterialBlendMode> BlendMode = new MaterialKey<MaterialBlendMode>("BlendMode");
        public readonly static MaterialKey<float> Opacity = new MaterialKey<float>("Opacity");
        public readonly static MaterialKey<float> BumpScaling = new MaterialKey<float>("BumpScaling");
        public readonly static MaterialKey<float> Shininess = new MaterialKey<float>("Shininess");
        public readonly static MaterialKey<float> Reflectivity = new MaterialKey<float>("Reflectivity");
        public readonly static MaterialKey<float> ShininessStrength = new MaterialKey<float>("ShininessStrength");
        public readonly static MaterialKey<float> Refractivity = new MaterialKey<float>("Refractivity");
        public readonly static MaterialKey<Color4> ColorDiffuse = new MaterialKey<Color4>("ColorDiffuse");
        public readonly static MaterialKey<Color4> ColorAmbient = new MaterialKey<Color4>("ColorAmbient");
        public readonly static MaterialKey<Color4> ColorSpecular = new MaterialKey<Color4>("ColorSpecular");
        public readonly static MaterialKey<Color4> ColorEmissive = new MaterialKey<Color4>("ColorEmissive");
        public readonly static MaterialKey<Color4> ColorTransparent = new MaterialKey<Color4>("ColorTransparent");
        public readonly static MaterialKey<Color4> ColorReflective = new MaterialKey<Color4>("ColorReflective");
    }
}