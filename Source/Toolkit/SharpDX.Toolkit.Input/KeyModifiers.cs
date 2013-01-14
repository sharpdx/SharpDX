namespace SharpDX.Toolkit.Input
{
    using System;

    [Flags]
    public enum KeyModifiers
    {
        None = 0x00,
        Control = 0x01,
        Shift = 0x02,
        Menu = 0x04,
        Windows = 0x08
    }
}