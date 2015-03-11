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
using System.Collections.Generic;
using System.Text;
using SharpGen.Generator;

namespace SharpGen.Model
{
    public class InteropMethodSignature
    {
        public InteropMethodSignature()
        {
            ParameterTypes = new List<InteropType>();
        }

        public int Index;
        public InteropType ReturnType;
        public List<InteropType> ParameterTypes;
        public bool IsLocal;
        public bool IsFunction;

        public string Name
        {
            get
            {
                string returnTypeName = ReturnType.TypeName;
                returnTypeName = returnTypeName.Replace("*", "Ptr");
                returnTypeName = returnTypeName.Replace(".", "");
                return "Calli" + ((IsFunction)?"Func":"") + returnTypeName + ((IsLocal) ? ""+ Index : "");
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            InteropMethodSignature against = obj as InteropMethodSignature;
            if (against == null)
                return false;
            if (this.ReturnType != against.ReturnType)
                return false;
            if (this.IsFunction != against.IsFunction)
                return false;
            if (this.ParameterTypes.Count != against.ParameterTypes.Count)
                return false;
            if (this.IsLocal != against.IsLocal)
                return false;

            for (int i = 0; i < ParameterTypes.Count; i++)
            {
                if (ParameterTypes[i] != against.ParameterTypes[i])
                    return false;
            }
            return true;
        }


        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(ReturnType.TypeName);
            builder.Append(" Calli" + ReturnType.TypeName + "(");
            for (int i = 0; i < ParameterTypes.Count; i++)
            {
                builder.Append(ParameterTypes[i].TypeName);
                if ((i + 1) < ParameterTypes.Count)
                    builder.Append(",");
            }
            builder.Append(")");
            return builder.ToString();
        }

		public override int GetHashCode()
		{
			return ReturnType.GetHashCode();
		} 
    }
}