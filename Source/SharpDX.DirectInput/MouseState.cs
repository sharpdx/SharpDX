using System;

namespace SharpDX.DirectInput
{
    /// <summary>The mouse state class.</summary>
    /// <unmanaged>DIMOUSESTATE2</unmanaged>
    public class MouseState : IDeviceState<RawMouseState, MouseUpdate> {

        /// <summary>Initializes a new instance of the <see cref="MouseState"/> class.</summary>
        public MouseState()
        {
            Buttons = new bool[8];
        }

        /// <summary>Gets or sets the X position.</summary>
        /// <value>The executable.</value>
        public int X { get; set; }

        /// <summary>Gets or sets the Y position.</summary>
        /// <value>The asynchronous.</value>
        public int Y { get; set; }

        /// <summary>Gets or sets the Z position.</summary>
        /// <value>The z.</value>
        public int Z { get; set; }

        /// <summary>Gets the buttons.</summary>
        /// <value>The buttons.</value>
        public bool[] Buttons { get; private set; }

        /// <summary>Updates the specified update.</summary>
        /// <param name="update">The update.</param>
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

        /// <summary>Marshals from.</summary>
        /// <param name="value">The value.</param>
        public void MarshalFrom(ref RawMouseState value)
        {
            unsafe
            {
                X = value.X;
                Y = value.Y;
                Z = value.Z;

                // Copy buttons states
                fixed (void* __to = &Buttons[0]) fixed (void* __from = &value.Buttons0) SharpDX.Utilities.CopyMemory((IntPtr)__to, (IntPtr)__from, Buttons.Length);
            }
        }

        /// <summary>Returns a <see cref="System.String" /> that represents this instance.</summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return string.Format(System.Globalization.CultureInfo.InvariantCulture, "X: {0}, Y: {1}, Z: {2}, Buttons: {3}", X, Y, Z, Utilities.Join(";",Buttons));
        }
    }
}