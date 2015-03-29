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
namespace SharpDX.Direct3D11
{
    public partial class EffectUnorderedAccessViewVariable
    {
        /// <summary>
        /// Sets the specified view.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <returns>A <see cref = "T:SharpDX.Result" /> object describing the result of the operation.</returns>
        /// <unmanaged>HRESULT ID3DX11EffectUnorderedAccessViewVariable::SetUnorderedAccessViewArray([In, Buffer] ID3D11UnorderedAccessView** ppResources,[In] unsigned int Offset,[In] unsigned int Count)</unmanaged>
        public void Set(UnorderedAccessView[] view)
        {
            Set(view, 0);
        }

        /// <summary>
        /// Sets the specified data ref.
        /// </summary>
        /// <param name="dataRef">The data ref.</param>
        /// <param name="offset">The offset.</param>
        /// <returns>A <see cref = "T:SharpDX.Result" /> object describing the result of the operation.</returns>
        /// <unmanaged>HRESULT ID3DX11EffectUnorderedAccessViewVariable::SetUnorderedAccessViewArray([In, Buffer] ID3D11UnorderedAccessView** ppResources,[In] unsigned int Offset,[In] unsigned int Count)</unmanaged>
        public void Set(UnorderedAccessView[] dataRef, int offset)
        {
            Set(dataRef, offset, dataRef.Length);
        }

        /// <summary>
        /// Sets the specified data ref.
        /// </summary>
        /// <param name="dataRef">The data ref.</param>
        /// <returns>A <see cref = "T:SharpDX.Result" /> object describing the result of the operation.</returns>
        /// <unmanaged>HRESULT ID3DX11EffectUnorderedAccessViewVariable::SetUnorderedAccessViewArray([In, Buffer] ID3D11UnorderedAccessView** ppResources,[In] unsigned int Offset,[In] unsigned int Count)</unmanaged>
        public void Set(SharpDX.ComArray<UnorderedAccessView> dataRef)
        {
            Set(dataRef, 0, dataRef.Length);
        }

        /// <summary>
        /// Sets the specified data ref.
        /// </summary>
        /// <param name="dataRef">The data ref.</param>
        /// <param name="offset">The offset.</param>
        /// <returns>A <see cref = "T:SharpDX.Result" /> object describing the result of the operation.</returns>
        /// <unmanaged>HRESULT ID3DX11EffectUnorderedAccessViewVariable::SetUnorderedAccessViewArray([In, Buffer] ID3D11UnorderedAccessView** ppResources,[In] unsigned int Offset,[In] unsigned int Count)</unmanaged>
        public void Set(SharpDX.ComArray<UnorderedAccessView> dataRef, int offset)
        {
            Set(dataRef, offset, dataRef.Length);
        }

        /// <summary>
        /// Gets the unordered access view array.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <returns>A <see cref = "T:SharpDX.Result" /> object describing the result of the operation.</returns>
        /// <unmanaged>HRESULT ID3DX11EffectUnorderedAccessViewVariable::GetUnorderedAccessViewArray([Out, Buffer] ID3D11UnorderedAccessView** ppResources,[In] unsigned int Offset,[In] unsigned int Count)</unmanaged>
        public UnorderedAccessView[] GetUnorderedAccessViewArray(int count)
        {
            return GetUnorderedAccessViewArray(0, count);
        }

        /// <summary>
        /// Gets the unordered access view array.
        /// </summary>
        /// <param name="offset">The offset.</param>
        /// <param name="count">The count.</param>
        /// <returns>A <see cref = "T:SharpDX.Result" /> object describing the result of the operation.</returns>
        /// <unmanaged>HRESULT ID3DX11EffectUnorderedAccessViewVariable::GetUnorderedAccessViewArray([Out, Buffer] ID3D11UnorderedAccessView** ppResources,[In] unsigned int Offset,[In] unsigned int Count)</unmanaged>
        public UnorderedAccessView[] GetUnorderedAccessViewArray(int offset, int count)
        {
            var temp = new UnorderedAccessView[count];
            this.GetUnorderedAccessViewArray(temp, offset, count);
            return temp;
        }
    }
}