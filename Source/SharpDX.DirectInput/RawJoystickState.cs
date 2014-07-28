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

using System.Runtime.InteropServices;

namespace SharpDX.DirectInput
{
    [StructLayout(LayoutKind.Sequential, Pack = 0 )]
    [DataFormat(DataFormatFlag.AbsoluteAxis)]
    public unsafe partial struct RawJoystickState
    {
        private const DeviceObjectTypeFlags TypeRelativeAxisOpt = DeviceObjectTypeFlags.RelativeAxis | DeviceObjectTypeFlags.AbsoluteAxis | DeviceObjectTypeFlags.AnyInstance | DeviceObjectTypeFlags.Optional;
        private const DeviceObjectTypeFlags TypePovOpt = DeviceObjectTypeFlags.PointOfViewController | DeviceObjectTypeFlags.AnyInstance | DeviceObjectTypeFlags.Optional;
        private const DeviceObjectTypeFlags TypeButtonOpt = DeviceObjectTypeFlags.PushButton | DeviceObjectTypeFlags.ToggleButton | DeviceObjectTypeFlags.AnyInstance | DeviceObjectTypeFlags.Optional;

        [DataObjectFormat(ObjectGuid.XAxisStr, TypeRelativeAxisOpt, ObjectDataFormatFlags.Position)]
        public int X;

        [DataObjectFormat(ObjectGuid.YAxisStr, TypeRelativeAxisOpt, ObjectDataFormatFlags.Position)]
        public int Y;

        [DataObjectFormat(ObjectGuid.ZAxisStr, TypeRelativeAxisOpt, ObjectDataFormatFlags.Position)]
        public int Z;

        [DataObjectFormat(ObjectGuid.RxAxisStr, TypeRelativeAxisOpt, ObjectDataFormatFlags.Position)]
        public int RotationX;

        [DataObjectFormat(ObjectGuid.RyAxisStr, TypeRelativeAxisOpt, ObjectDataFormatFlags.Position)]
        public int RotationY;

        [DataObjectFormat(ObjectGuid.RzAxisStr, TypeRelativeAxisOpt, ObjectDataFormatFlags.Position)]
        public int RotationZ;

        [DataObjectFormat(ObjectGuid.SliderStr, 2, TypeRelativeAxisOpt, ObjectDataFormatFlags.Position)]
        public fixed int Sliders[2];

        [DataObjectFormat(ObjectGuid.PovControllerStr, 4, TypePovOpt)]
        public fixed int PointOfViewControllers[4];

        [DataObjectFormat(128, TypeButtonOpt)]
        public fixed byte Buttons[128];

        [DataObjectFormat(ObjectGuid.XAxisStr, TypeRelativeAxisOpt, ObjectDataFormatFlags.Velocity)]
        public int VelocityX;
        [DataObjectFormat(ObjectGuid.YAxisStr, TypeRelativeAxisOpt, ObjectDataFormatFlags.Velocity)]
        public int VelocityY;
        [DataObjectFormat(ObjectGuid.ZAxisStr, TypeRelativeAxisOpt, ObjectDataFormatFlags.Velocity)]
        public int VelocityZ;
        [DataObjectFormat(ObjectGuid.RxAxisStr, TypeRelativeAxisOpt, ObjectDataFormatFlags.Velocity)]
        public int AngularVelocityX;
        [DataObjectFormat(ObjectGuid.RyAxisStr, TypeRelativeAxisOpt, ObjectDataFormatFlags.Velocity)]
        public int AngularVelocityY;
        [DataObjectFormat(ObjectGuid.RzAxisStr, TypeRelativeAxisOpt, ObjectDataFormatFlags.Velocity)]
        public int AngularVelocityZ;

        [DataObjectFormat(ObjectGuid.SliderStr, 2, TypeRelativeAxisOpt, ObjectDataFormatFlags.Velocity)]
        public fixed int VelocitySliders[2];

        [DataObjectFormat(ObjectGuid.XAxisStr, TypeRelativeAxisOpt, ObjectDataFormatFlags.Acceleration)]
        public int AccelerationX;

        [DataObjectFormat(ObjectGuid.YAxisStr, TypeRelativeAxisOpt, ObjectDataFormatFlags.Acceleration)]
        public int AccelerationY;

        [DataObjectFormat(ObjectGuid.ZAxisStr, TypeRelativeAxisOpt, ObjectDataFormatFlags.Acceleration)]
        public int AccelerationZ;

        [DataObjectFormat(ObjectGuid.RxAxisStr, TypeRelativeAxisOpt, ObjectDataFormatFlags.Acceleration)]
        public int AngularAccelerationX;

        [DataObjectFormat(ObjectGuid.RyAxisStr, TypeRelativeAxisOpt, ObjectDataFormatFlags.Acceleration)]
        public int AngularAccelerationY;

        [DataObjectFormat(ObjectGuid.RzAxisStr, TypeRelativeAxisOpt, ObjectDataFormatFlags.Acceleration)]
        public int AngularAccelerationZ;

        [DataObjectFormat(ObjectGuid.SliderStr, 2, TypeRelativeAxisOpt, ObjectDataFormatFlags.Acceleration)]
        public fixed int AccelerationSliders[2];

        [DataObjectFormat(ObjectGuid.XAxisStr, TypeRelativeAxisOpt, ObjectDataFormatFlags.Force)]
        public int ForceX;
        [DataObjectFormat(ObjectGuid.YAxisStr, TypeRelativeAxisOpt, ObjectDataFormatFlags.Force)]
        public int ForceY;
        [DataObjectFormat(ObjectGuid.ZAxisStr, TypeRelativeAxisOpt, ObjectDataFormatFlags.Force)]
        public int ForceZ;

        [DataObjectFormat(ObjectGuid.RxAxisStr, TypeRelativeAxisOpt, ObjectDataFormatFlags.Force)]
        public int TorqueX;
        [DataObjectFormat(ObjectGuid.RyAxisStr, TypeRelativeAxisOpt, ObjectDataFormatFlags.Force)]
        public int TorqueY;
        [DataObjectFormat(ObjectGuid.RzAxisStr, TypeRelativeAxisOpt, ObjectDataFormatFlags.Force)]
        public int TorqueZ;

        [DataObjectFormat(ObjectGuid.SliderStr, 2, TypeRelativeAxisOpt, ObjectDataFormatFlags.Force)]
        public fixed int ForceSliders[2];


        /*
        DataFormatFlag IDataFormatProvider.Flags
        {
            get { return DataFormatFlag.AbsoluteAxis; }
        }

        DataObjectFormat[] IDataFormatProvider.ObjectsFormat
        {
            get {  return _objectsFormat; }
        }

        private static DataObjectFormat[] _objectsFormat = new DataObjectFormat[164]
                                                               {
                                                                   new DataObjectFormat(ObjectGuid.XAxis, 0, TypeRelativeAxisOpt, ObjectDataFormatFlags.Position),
                                                                   new DataObjectFormat(ObjectGuid.YAxis, 4, TypeRelativeAxisOpt, ObjectDataFormatFlags.Position),
                                                                   new DataObjectFormat(ObjectGuid.ZAxis, 8, TypeRelativeAxisOpt, ObjectDataFormatFlags.Position),
                                                                   new DataObjectFormat(ObjectGuid.RxAxis, 12, TypeRelativeAxisOpt, ObjectDataFormatFlags.Position),
                                                                   new DataObjectFormat(ObjectGuid.RyAxis, 16, TypeRelativeAxisOpt, ObjectDataFormatFlags.Position),
                                                                   new DataObjectFormat(ObjectGuid.RzAxis, 20, TypeRelativeAxisOpt, ObjectDataFormatFlags.Position),
                                                                   new DataObjectFormat(ObjectGuid.Slider, 24, TypeRelativeAxisOpt, ObjectDataFormatFlags.Position),
                                                                   new DataObjectFormat(ObjectGuid.Slider, 28, TypeRelativeAxisOpt, ObjectDataFormatFlags.Position),
                                                                   new DataObjectFormat(ObjectGuid.PovController, 32, TypePovOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(ObjectGuid.PovController, 36, TypePovOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(ObjectGuid.PovController, 40, TypePovOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(ObjectGuid.PovController, 44, TypePovOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 48, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 49, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 50, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 51, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 52, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 53, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 54, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 55, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 56, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 57, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 58, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 59, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 60, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 61, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 62, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 63, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 64, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 65, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 66, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 67, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 68, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 69, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 70, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 71, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 72, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 73, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 74, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 75, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 76, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 77, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 78, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 79, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 80, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 81, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 82, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 83, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 84, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 85, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 86, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 87, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 88, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 89, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 90, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 91, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 92, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 93, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 94, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 95, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 96, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 97, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 98, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 99, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 100, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 101, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 102, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 103, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 104, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 105, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 106, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 107, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 108, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 109, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 110, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 111, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 112, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 113, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 114, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 115, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 116, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 117, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 118, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 119, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 120, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 121, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 122, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 123, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 124, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 125, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 126, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 127, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 128, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 129, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 130, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 131, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 132, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 133, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 134, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 135, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 136, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 137, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 138, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 139, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 140, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 141, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 142, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 143, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 144, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 145, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 146, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 147, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 148, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 149, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 150, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 151, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 152, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 153, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 154, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 155, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 156, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 157, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 158, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 159, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 160, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 161, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 162, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 163, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 164, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 165, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 166, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 167, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 168, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 169, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 170, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 171, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 172, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 173, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 174, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(Guid.Empty, 175, TypeButtonOpt, ObjectDataFormatFlags.None),
                                                                   new DataObjectFormat(ObjectGuid.XAxis, 176, TypeRelativeAxisOpt, ObjectDataFormatFlags.Velocity),
                                                                   new DataObjectFormat(ObjectGuid.YAxis, 180, TypeRelativeAxisOpt, ObjectDataFormatFlags.Velocity),
                                                                   new DataObjectFormat(ObjectGuid.ZAxis, 184, TypeRelativeAxisOpt, ObjectDataFormatFlags.Velocity),
                                                                   new DataObjectFormat(ObjectGuid.RxAxis, 188, TypeRelativeAxisOpt, ObjectDataFormatFlags.Velocity),
                                                                   new DataObjectFormat(ObjectGuid.RyAxis, 192, TypeRelativeAxisOpt, ObjectDataFormatFlags.Velocity),
                                                                   new DataObjectFormat(ObjectGuid.RzAxis, 196, TypeRelativeAxisOpt, ObjectDataFormatFlags.Velocity),
                                                                   new DataObjectFormat(ObjectGuid.Slider, 24, TypeRelativeAxisOpt, ObjectDataFormatFlags.Velocity),
                                                                   new DataObjectFormat(ObjectGuid.Slider, 28, TypeRelativeAxisOpt, ObjectDataFormatFlags.Velocity),
                                                                   new DataObjectFormat(ObjectGuid.XAxis, 208, TypeRelativeAxisOpt, ObjectDataFormatFlags.Acceleration),
                                                                   new DataObjectFormat(ObjectGuid.YAxis, 212, TypeRelativeAxisOpt, ObjectDataFormatFlags.Acceleration),
                                                                   new DataObjectFormat(ObjectGuid.ZAxis, 216, TypeRelativeAxisOpt, ObjectDataFormatFlags.Acceleration),
                                                                   new DataObjectFormat(ObjectGuid.RxAxis, 220, TypeRelativeAxisOpt, ObjectDataFormatFlags.Acceleration),
                                                                   new DataObjectFormat(ObjectGuid.RyAxis, 224, TypeRelativeAxisOpt, ObjectDataFormatFlags.Acceleration),
                                                                   new DataObjectFormat(ObjectGuid.RzAxis, 228, TypeRelativeAxisOpt, ObjectDataFormatFlags.Acceleration),
                                                                   new DataObjectFormat(ObjectGuid.Slider, 24, TypeRelativeAxisOpt, ObjectDataFormatFlags.Acceleration),
                                                                   new DataObjectFormat(ObjectGuid.Slider, 28, TypeRelativeAxisOpt, ObjectDataFormatFlags.Acceleration),
                                                                   new DataObjectFormat(ObjectGuid.XAxis, 240, TypeRelativeAxisOpt, ObjectDataFormatFlags.Force),
                                                                   new DataObjectFormat(ObjectGuid.YAxis, 244, TypeRelativeAxisOpt, ObjectDataFormatFlags.Force),
                                                                   new DataObjectFormat(ObjectGuid.ZAxis, 248, TypeRelativeAxisOpt, ObjectDataFormatFlags.Force),
                                                                   new DataObjectFormat(ObjectGuid.RxAxis, 252, TypeRelativeAxisOpt, ObjectDataFormatFlags.Force),
                                                                   new DataObjectFormat(ObjectGuid.RyAxis, 256, TypeRelativeAxisOpt, ObjectDataFormatFlags.Force),
                                                                   new DataObjectFormat(ObjectGuid.RzAxis, 260, TypeRelativeAxisOpt, ObjectDataFormatFlags.Force),
                                                                   new DataObjectFormat(ObjectGuid.Slider, 24, TypeRelativeAxisOpt, ObjectDataFormatFlags.Force),
                                                                   new DataObjectFormat(ObjectGuid.Slider, 28, TypeRelativeAxisOpt, ObjectDataFormatFlags.Force),
                                                               }; 
        */

        
    }
}