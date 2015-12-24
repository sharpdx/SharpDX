using System;

namespace SharpDX.DirectInput
{
    /// <summary>	
    /// No documentation.	
    /// </summary>	
    /// <unmanaged>DIJOYSTATE2</unmanaged>
    public class JoystickState : IDeviceState<RawJoystickState, JoystickUpdate>
    {
        public JoystickState()
        {
            Sliders = new int[2];
            PointOfViewControllers = new int[4];
            Buttons = new bool[128];
            VelocitySliders = new int[2];
            AccelerationSliders = new int[2];
            ForceSliders = new int[2];
        }

        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <unmanaged>int lX</unmanaged>
        public int X { get; set; }
        
        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <unmanaged>int lY</unmanaged>
        public int Y { get; set; }
        
        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <unmanaged>int lZ</unmanaged>
        public int Z { get; set; }
        
        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <unmanaged>int lRx</unmanaged>
        public int RotationX { get; set; }
        
        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <unmanaged>int lRy</unmanaged>
        public int RotationY { get; set; }
        
        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <unmanaged>int lRz</unmanaged>
        public int RotationZ { get; set; }

        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <unmanaged>int rglSlider[2]</unmanaged>
        public int[] Sliders { get; internal set; }

        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <unmanaged>unsigned int rgdwPOV[4]</unmanaged>
        public int[] PointOfViewControllers { get; internal set; }

        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <unmanaged>unsigned char rgbButtons[128]</unmanaged>
        public bool[] Buttons { get; internal set; }

        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <unmanaged>int lVX</unmanaged>
        public int VelocityX { get; set; }
        
        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <unmanaged>int lVY</unmanaged>
        public int VelocityY { get; set; }
        
        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <unmanaged>int lVZ</unmanaged>
        public int VelocityZ { get; set; }
        
        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <unmanaged>int lVRx</unmanaged>
        public int AngularVelocityX { get; set; }
        
        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <unmanaged>int lVRy</unmanaged>
        public int AngularVelocityY { get; set; }
        
        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <unmanaged>int lVRz</unmanaged>
        public int AngularVelocityZ { get; set; }

        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <unmanaged>int rglVSlider[2]</unmanaged>
        public int[] VelocitySliders { get; internal set; }

        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <unmanaged>int lAX</unmanaged>
        public int AccelerationX { get; set; }
        
        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <unmanaged>int lAY</unmanaged>
        public int AccelerationY { get; set; }
        
        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <unmanaged>int lAZ</unmanaged>
        public int AccelerationZ { get; set; }
        
        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <unmanaged>int lARx</unmanaged>
        public int AngularAccelerationX { get; set; }
        
        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <unmanaged>int lARy</unmanaged>
        public int AngularAccelerationY { get; set; }
        
        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <unmanaged>int lARz</unmanaged>
        public int AngularAccelerationZ { get; set; }

        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <unmanaged>int rglASlider[2]</unmanaged>
        public int[] AccelerationSliders { get; internal set; }

        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <unmanaged>int lFX</unmanaged>
        public int ForceX { get; set; }
        
        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <unmanaged>int lFY</unmanaged>
        public int ForceY { get; set; }
        
        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <unmanaged>int lFZ</unmanaged>
        public int ForceZ { get; set; }
        
        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <unmanaged>int lFRx</unmanaged>
        public int TorqueX { get; set; }
        
        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <unmanaged>int lFRy</unmanaged>
        public int TorqueY { get; set; }
        
        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <unmanaged>int lFRz</unmanaged>
        public int TorqueZ { get; set; }

        /// <summary>	
        /// No documentation.	
        /// </summary>	
        /// <unmanaged>int rglFSlider[2]</unmanaged>
        public int[] ForceSliders { get; internal set; }

        // Internal native struct used for marshalling

        public void Update(JoystickUpdate update)
        {
            int value = update.Value;
            switch (update.Offset)
            {
                case JoystickOffset.X:
                    X = value;
                    break;
                case JoystickOffset.Y:
                    Y = value;
                    break;
                case JoystickOffset.Z:
                    Z = value;
                    break;
                case JoystickOffset.RotationX:
                    RotationX = value;
                    break;
                case JoystickOffset.RotationY:
                    RotationY = value;
                    break;
                case JoystickOffset.RotationZ:
                    RotationZ = value;
                    break;
                case JoystickOffset.Sliders0:
                    Sliders[0] = value;
                    break;
                case JoystickOffset.Sliders1:
                    Sliders[1] = value;
                    break;
                case JoystickOffset.PointOfViewControllers0:
                    PointOfViewControllers[0] = value;
                    break;
                case JoystickOffset.PointOfViewControllers1:
                    PointOfViewControllers[1] = value;
                    break;
                case JoystickOffset.PointOfViewControllers2:
                    PointOfViewControllers[2] = value;
                    break;
                case JoystickOffset.PointOfViewControllers3:
                    PointOfViewControllers[3] = value;
                    break;
                case JoystickOffset.VelocityX:
                    VelocityX = value;
                    break;
                case JoystickOffset.VelocityY:
                    VelocityY = value;
                    break;
                case JoystickOffset.VelocityZ:
                    VelocityZ = value;
                    break;
                case JoystickOffset.AngularVelocityX:
                    AngularVelocityX = value;
                    break;
                case JoystickOffset.AngularVelocityY:
                    AngularVelocityY = value;
                    break;
                case JoystickOffset.AngularVelocityZ:
                    AngularVelocityZ = value;
                    break;
                case JoystickOffset.VelocitySliders0:
                    VelocitySliders[0] = value;
                    break;
                case JoystickOffset.VelocitySliders1:
                    VelocitySliders[1] = value;
                    break;
                case JoystickOffset.AccelerationX:
                    AccelerationX = value;
                    break;
                case JoystickOffset.AccelerationY:
                    AccelerationY = value;
                    break;
                case JoystickOffset.AccelerationZ:
                    AccelerationZ = value;
                    break;
                case JoystickOffset.AngularAccelerationX:
                    AngularAccelerationX = value;
                    break;
                case JoystickOffset.AngularAccelerationY:
                    AngularAccelerationY = value;
                    break;
                case JoystickOffset.AngularAccelerationZ:
                    AngularAccelerationZ = value;
                    break;
                case JoystickOffset.AccelerationSliders0:
                    AccelerationSliders[0] = value;
                    break;
                case JoystickOffset.AccelerationSliders1:
                    AccelerationSliders[1] = value;
                    break;
                case JoystickOffset.ForceX:
                    ForceX = value;
                    break;
                case JoystickOffset.ForceY:
                    ForceY = value;
                    break;
                case JoystickOffset.ForceZ:
                    ForceZ = value;
                    break;
                case JoystickOffset.TorqueX:
                    TorqueX = value;
                    break;
                case JoystickOffset.TorqueY:
                    TorqueY = value;
                    break;
                case JoystickOffset.TorqueZ:
                    TorqueZ = value;
                    break;
                case JoystickOffset.ForceSliders0:
                    ForceSliders[0] = value;
                    break;
                case JoystickOffset.ForceSliders1:
                    ForceSliders[1] = value;
                    break;
                default:
                    int buttonIndex = update.Offset - JoystickOffset.Buttons0;
                    if (buttonIndex >= 0 && buttonIndex <128)
                        Buttons[buttonIndex] = (value & 0x80) != 0;
                    break;
            }            
        }

        public void MarshalFrom(ref RawJoystickState value)
        {
            unsafe
            {
                this.X = value.X;
                this.Y = value.Y;
                this.Z = value.Z;
                this.RotationX = value.RotationX;
                this.RotationY = value.RotationY;
                this.RotationZ = value.RotationZ;
                fixed (int* __from = value.Sliders)
                {
                    Sliders[0] = __from[0]; Sliders[1] = __from[1];
                }
                    
                fixed (int* __from = value.PointOfViewControllers)
                {
                    PointOfViewControllers[0] = __from[0];
                    PointOfViewControllers[1] = __from[1];
                    PointOfViewControllers[2] = __from[2];
                    PointOfViewControllers[3] = __from[3];
                }

                fixed(void* __from = value.Buttons)
                {
                    for(int i = 0; i < 128; i++)
                        Buttons[i] = (((byte*)__from)[i] & 0x80) != 0;
                }

                this.VelocityX = value.VelocityX;
                this.VelocityY = value.VelocityY;
                this.VelocityZ = value.VelocityZ;
                this.AngularVelocityX = value.AngularVelocityX;
                this.AngularVelocityY = value.AngularVelocityY;
                this.AngularVelocityZ = value.AngularVelocityZ;

                fixed (int* __from = value.VelocitySliders)
                {
                    VelocitySliders[0] = __from[0]; VelocitySliders[1] = __from[1];
                }

                this.AccelerationX = value.AccelerationX;
                this.AccelerationY = value.AccelerationY;
                this.AccelerationZ = value.AccelerationZ;
                this.AngularAccelerationX = value.AngularAccelerationX;
                this.AngularAccelerationY = value.AngularAccelerationY;
                this.AngularAccelerationZ = value.AngularAccelerationZ;
                fixed (int* __from = value.AccelerationSliders)
                {
                    AccelerationSliders[0] = __from[0]; AccelerationSliders[1] = __from[1];
                }
                this.ForceX = value.ForceX;
                this.ForceY = value.ForceY;
                this.ForceZ = value.ForceZ;
                this.TorqueX = value.TorqueX;
                this.TorqueY = value.TorqueY;
                this.TorqueZ = value.TorqueZ;

                fixed (int* __from = value.ForceSliders)
                {
                    ForceSliders[0] = __from[0]; ForceSliders[1] = __from[1];
                }        
            }
        }

        public override string ToString()
        {
            return string.Format(System.Globalization.CultureInfo.InvariantCulture, "X: {0}, Y: {1}, Z: {2}, RotationX: {3}, RotationY: {4}, RotationZ: {5}, Sliders: {6}, PointOfViewControllers: {7}, Buttons: {8}, VelocityX: {9}, VelocityY: {10}, VelocityZ: {11}, AngularVelocityX: {12}, AngularVelocityY: {13}, AngularVelocityZ: {14}, VelocitySliders: {15}, AccelerationX: {16}, AccelerationY: {17}, AccelerationZ: {18}, AngularAccelerationX: {19}, AngularAccelerationY: {20}, AngularAccelerationZ: {21}, AccelerationSliders: {22}, ForceX: {23}, ForceY: {24}, ForceZ: {25}, TorqueX: {26}, TorqueY: {27}, TorqueZ: {28}, ForceSliders: {29}", X, Y, Z, RotationX, RotationY, RotationZ, Utilities.Join(";",Sliders), Utilities.Join(";",PointOfViewControllers), Utilities.Join(";",Buttons), VelocityX, VelocityY, VelocityZ, AngularVelocityX, AngularVelocityY, AngularVelocityZ, Utilities.Join(";",VelocitySliders), AccelerationX, AccelerationY, AccelerationZ, AngularAccelerationX, AngularAccelerationY, AngularAccelerationZ, Utilities.Join(";",AccelerationSliders), ForceX, ForceY, ForceZ, TorqueX, TorqueY, TorqueZ, Utilities.Join(";",ForceSliders));
        }
    }
}