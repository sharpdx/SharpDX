using System;

namespace SharpDX.DirectInput
{
    /// <summary>	
    /// No documentation.	
    /// </summary>	
    /// <unmanaged>DIMOUSESTATE2</unmanaged>
    public class MouseState : IDeviceState<RawMouseState, MouseUpdate> {	
        
        public MouseState()
        {
            Buttons = new bool[8];
        }

        public int X { get; set; }

        public int Y { get; set; }

        public int Z { get; set; }

        public bool[] Buttons { get; private set; }

        public void Update(MouseUpdate update)
        {
            int value = update.Value;
            switch (update.Offset)
            {
                case MouseOffset.X:
                    X = value;
                    break;
                case MouseOffset.Y:
                    Y = value;
                    break;
                case MouseOffset.Z:
                    Z = value;
                    break;
                default:
                    int buttonIndex = update.Offset - MouseOffset.Buttons0;
                    if (buttonIndex >= 0 && buttonIndex < 8)
                        Buttons[buttonIndex] = (value & 0x80) != 0;
                    break;
            }
        }

        public void MarshalFrom(ref RawMouseState value)
        {
            unsafe
            {
                X = value.X;
                Y = value.Y;
                Z = value.Z;

                // Copy buttons states
                fixed(void* __from = &value.Buttons0) {
                    for(int i = 0; i < 8; i++) {
                        Buttons[i] = (((byte*)__from)[i] & 0x80) != 0;
                    }
                }
            }
        }

        public override string ToString()
        {
            return string.Format(System.Globalization.CultureInfo.InvariantCulture, "X: {0}, Y: {1}, Z: {2}, Buttons: {3}", X, Y, Z, Utilities.Join(";",Buttons));
        }
    }
}
