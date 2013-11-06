// Copyright (c) 2010-2013 SharpDX - Alexandre Mutel
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

namespace SharpDX.Toolkit.Input
{
    /// <summary>Keyboard buttons</summary>
    /// <remarks>At this time only 256 key codes are supported</remarks>
    public enum Keys : byte
    {
        // values are taken from XNA

        /// <summary>The none.</summary>
        None = 0,
        /// <summary>The back.</summary>
        Back = 8,
        /// <summary>The tab.</summary>
        Tab = 9,
        /// <summary>The enter.</summary>
        Enter = 13,
        /// <summary>The shift.</summary>
        Shift = 16,
        /// <summary>The control.</summary>
        Control = 17,
        /// <summary>The alt.</summary>
        Alt = 18,
        /// <summary>The pause.</summary>
        Pause = 19,
        /// <summary>The caps lock.</summary>
        CapsLock = 20,
        /// <summary>The kana.</summary>
        Kana = 21,
        /// <summary>The kanji.</summary>
        Kanji = 25,
        /// <summary>The escape.</summary>
        Escape = 27,
        /// <summary>The IME convert.</summary>
        ImeConvert = 28,
        /// <summary>The IME no convert.</summary>
        ImeNoConvert = 29,
        /// <summary>The space.</summary>
        Space = 32,
        /// <summary>The page up.</summary>
        PageUp = 33,
        /// <summary>The page down.</summary>
        PageDown = 34,
        /// <summary>The end.</summary>
        End = 35,
        /// <summary>The home.</summary>
        Home = 36,
        /// <summary>The left.</summary>
        Left = 37,
        /// <summary>The up.</summary>
        Up = 38,
        /// <summary>The right.</summary>
        Right = 39,
        /// <summary>The down.</summary>
        Down = 40,
        /// <summary>The select.</summary>
        Select = 41,
        /// <summary>The print.</summary>
        Print = 42,
        /// <summary>The execute.</summary>
        Execute = 43,
        /// <summary>The print screen.</summary>
        PrintScreen = 44,
        /// <summary>The insert.</summary>
        Insert = 45,
        /// <summary>The delete.</summary>
        Delete = 46,
        /// <summary>The help.</summary>
        Help = 47,
        /// <summary>The d0.</summary>
        D0 = 48,
        /// <summary>The d1.</summary>
        D1 = 49,
        /// <summary>The d2.</summary>
        D2 = 50,
        /// <summary>The d3.</summary>
        D3 = 51,
        /// <summary>The d4.</summary>
        D4 = 52,
        /// <summary>The d5.</summary>
        D5 = 53,
        /// <summary>The d6.</summary>
        D6 = 54,
        /// <summary>The d7.</summary>
        D7 = 55,
        /// <summary>The d8.</summary>
        D8 = 56,
        /// <summary>The d9.</summary>
        D9 = 57,
        /// <summary>The A.</summary>
        A = 65,
        /// <summary>The B.</summary>
        B = 66,
        /// <summary>The C.</summary>
        C = 67,
        /// <summary>The D.</summary>
        D = 68,
        /// <summary>The E.</summary>
        E = 69,
        /// <summary>The F.</summary>
        F = 70,
        /// <summary>The G.</summary>
        G = 71,
        /// <summary>The H.</summary>
        H = 72,
        /// <summary>The I.</summary>
        I = 73,
        /// <summary>The J.</summary>
        J = 74,
        /// <summary>The K.</summary>
        K = 75,
        /// <summary>The L.</summary>
        L = 76,
        /// <summary>The M.</summary>
        M = 77,
        /// <summary>The N.</summary>
        N = 78,
        /// <summary>The O.</summary>
        O = 79,
        /// <summary>The P.</summary>
        P = 80,
        /// <summary>The Q.</summary>
        Q = 81,
        /// <summary>The R.</summary>
        R = 82,
        /// <summary>The S.</summary>
        S = 83,
        /// <summary>The T.</summary>
        T = 84,
        /// <summary>The U.</summary>
        U = 85,
        /// <summary>The V.</summary>
        V = 86,
        /// <summary>The W.</summary>
        W = 87,
        /// <summary>The X.</summary>
        X = 88,
        /// <summary>The Y.</summary>
        Y = 89,
        /// <summary>The Z.</summary>
        Z = 90,
        /// <summary>The left windows.</summary>
        LeftWindows = 91,
        /// <summary>The right windows.</summary>
        RightWindows = 92,
        /// <summary>The apps.</summary>
        Apps = 93,
        /// <summary>The sleep.</summary>
        Sleep = 95,
        /// <summary>The number pad0.</summary>
        NumPad0 = 96,
        /// <summary>The number pad1.</summary>
        NumPad1 = 97,
        /// <summary>The number pad2.</summary>
        NumPad2 = 98,
        /// <summary>The number pad3.</summary>
        NumPad3 = 99,
        /// <summary>The number pad4.</summary>
        NumPad4 = 100,
        /// <summary>The number pad5.</summary>
        NumPad5 = 101,
        /// <summary>The number pad6.</summary>
        NumPad6 = 102,
        /// <summary>The number pad7.</summary>
        NumPad7 = 103,
        /// <summary>The number pad8.</summary>
        NumPad8 = 104,
        /// <summary>The number pad9.</summary>
        NumPad9 = 105,
        /// <summary>The multiply.</summary>
        Multiply = 106,
        /// <summary>The add.</summary>
        Add = 107,
        /// <summary>The separator.</summary>
        Separator = 108,
        /// <summary>The subtract.</summary>
        Subtract = 109,
        /// <summary>The decimal.</summary>
        Decimal = 110,
        /// <summary>The divide.</summary>
        Divide = 111,
        /// <summary>The f1.</summary>
        F1 = 112,
        /// <summary>The f2.</summary>
        F2 = 113,
        /// <summary>The f3.</summary>
        F3 = 114,
        /// <summary>The f4.</summary>
        F4 = 115,
        /// <summary>The f5.</summary>
        F5 = 116,
        /// <summary>The f6.</summary>
        F6 = 117,
        /// <summary>The f7.</summary>
        F7 = 118,
        /// <summary>The f8.</summary>
        F8 = 119,
        /// <summary>The f9.</summary>
        F9 = 120,
        /// <summary>The F10.</summary>
        F10 = 121,
        /// <summary>The F11.</summary>
        F11 = 122,
        /// <summary>The F12.</summary>
        F12 = 123,
        /// <summary>The F13.</summary>
        F13 = 124,
        /// <summary>The F14.</summary>
        F14 = 125,
        /// <summary>The F15.</summary>
        F15 = 126,
        /// <summary>The F16.</summary>
        F16 = 127,
        /// <summary>The F17.</summary>
        F17 = 128,
        /// <summary>The F18.</summary>
        F18 = 129,
        /// <summary>The F19.</summary>
        F19 = 130,
        /// <summary>The F20.</summary>
        F20 = 131,
        /// <summary>The F21.</summary>
        F21 = 132,
        /// <summary>The F22.</summary>
        F22 = 133,
        /// <summary>The F23.</summary>
        F23 = 134,
        /// <summary>The F24.</summary>
        F24 = 135,
        /// <summary>The number lock.</summary>
        NumLock = 144,
        /// <summary>The scroll.</summary>
        Scroll = 145,
        /// <summary>The left shift.</summary>
        LeftShift = 160,
        /// <summary>The right shift.</summary>
        RightShift = 161,
        /// <summary>The left control.</summary>
        LeftControl = 162,
        /// <summary>The right control.</summary>
        RightControl = 163,
        /// <summary>The left alt.</summary>
        LeftAlt = 164,
        /// <summary>The right alt.</summary>
        RightAlt = 165,
        /// <summary>The browser back.</summary>
        BrowserBack = 166,
        /// <summary>The browser forward.</summary>
        BrowserForward = 167,
        /// <summary>The browser refresh.</summary>
        BrowserRefresh = 168,
        /// <summary>The browser stop.</summary>
        BrowserStop = 169,
        /// <summary>The browser search.</summary>
        BrowserSearch = 170,
        /// <summary>The browser favorites.</summary>
        BrowserFavorites = 171,
        /// <summary>The browser home.</summary>
        BrowserHome = 172,
        /// <summary>The volume mute.</summary>
        VolumeMute = 173,
        /// <summary>The volume down.</summary>
        VolumeDown = 174,
        /// <summary>The volume up.</summary>
        VolumeUp = 175,
        /// <summary>The media next track.</summary>
        MediaNextTrack = 176,
        /// <summary>The media previous track.</summary>
        MediaPreviousTrack = 177,
        /// <summary>The media stop.</summary>
        MediaStop = 178,
        /// <summary>The media play pause.</summary>
        MediaPlayPause = 179,
        /// <summary>The launch mail.</summary>
        LaunchMail = 180,
        /// <summary>The select media.</summary>
        SelectMedia = 181,
        /// <summary>The launch application1.</summary>
        LaunchApplication1 = 182,
        /// <summary>The launch application2.</summary>
        LaunchApplication2 = 183,
        /// <summary>The oem semicolon.</summary>
        OemSemicolon = 186,
        /// <summary>The oem plus.</summary>
        OemPlus = 187,
        /// <summary>The oem comma.</summary>
        OemComma = 188,
        /// <summary>The oem minus.</summary>
        OemMinus = 189,
        /// <summary>The oem period.</summary>
        OemPeriod = 190,
        /// <summary>The oem question.</summary>
        OemQuestion = 191,
        /// <summary>The oem tilde.</summary>
        OemTilde = 192,
        /// <summary>The chat pad green.</summary>
        ChatPadGreen = 202,
        /// <summary>The chat pad orange.</summary>
        ChatPadOrange = 203,
        /// <summary>The oem open brackets.</summary>
        OemOpenBrackets = 219,
        /// <summary>The oem pipe.</summary>
        OemPipe = 220,
        /// <summary>The oem close brackets.</summary>
        OemCloseBrackets = 221,
        /// <summary>The oem quotes.</summary>
        OemQuotes = 222,
        /// <summary>The oem8.</summary>
        Oem8 = 223,
        /// <summary>The oem backslash.</summary>
        OemBackslash = 226,
        /// <summary>The process key.</summary>
        ProcessKey = 229,
        /// <summary>The oem copy.</summary>
        OemCopy = 242,
        /// <summary>The oem automatic.</summary>
        OemAuto = 243,
        /// <summary>The oem enl forward.</summary>
        OemEnlW = 244,
        /// <summary>The attn.</summary>
        Attn = 246,
        /// <summary>The crsel.</summary>
        Crsel = 247,
        /// <summary>The exsel.</summary>
        Exsel = 248,
        /// <summary>The erase EOF.</summary>
        EraseEof = 249,
        /// <summary>The play.</summary>
        Play = 250,
        /// <summary>The zoom.</summary>
        Zoom = 251,
        /// <summary>The pa1.</summary>
        Pa1 = 253,
        /// <summary>The oem clear.</summary>
        OemClear = 254
    }
}