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

#if CORECLR

using System;

namespace SharpDX.RawInput
{
	/// <summary>
	/// Replacement of the Message class from Winforms when compiling against CoreCLR.
	/// </summary>
    public struct Message
    {
        IntPtr hWnd;
        int msg;
        IntPtr wparam;
        IntPtr lparam;
        IntPtr result;

        public IntPtr HWnd
        {
            get { return hWnd; }
            set { hWnd = value; }
        }

        public int Msg
        {
            get { return msg; }
            set { msg = value; }
        }

        public IntPtr WParam
        {
            get { return wparam; }
            set { wparam = value; }
        }

        public IntPtr LParam
        {
            get { return lparam; }
            set { lparam = value; }
        }

        public IntPtr Result
        {
            get { return result; }
            set { result = value; }
        }

        public static Message Create(IntPtr hWnd, int msg, IntPtr wparam, IntPtr lparam)
        {
            Message m = new Message();
            m.hWnd = hWnd;
            m.msg = msg;
            m.wparam = wparam;
            m.lparam = lparam;
            m.result = IntPtr.Zero;

            return m;
        }

        public override bool Equals(object o)
        {
            if (!(o is Message))
            {
                return false;
            }

            Message m = (Message)o;
            return hWnd == m.hWnd &&
                   msg == m.msg &&
                   wparam == m.wparam &&
                   lparam == m.lparam &&
                   result == m.result;
        }

        public static bool operator !=(Message a, Message b)
        {
            return !a.Equals(b);
        }

        public static bool operator ==(Message a, Message b)
        {
            return a.Equals(b);
        }

        public override int GetHashCode()
        {
            return (int)hWnd << 4 | msg;
        }
    }
}

#endif