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
using System;
using System.Collections.Generic;
using System.Text;

namespace SharpDX.Direct2D1.Effects
{
    /// <summary>
    /// Built in ColorManagement effect.
    /// </summary>
    public class ColorManagement : Effect
    {
        private ColorContext sourceContext;
        private ColorContext destinationContext;


        /// <summary>
        /// Initializes a new instance of <see cref="ColorManagement"/> effect.
        /// </summary>
        /// <param name="context"></param>
        public ColorManagement(DeviceContext context) : base(context, Effect.ColorManagement)
        {
        }

        /// <summary>
        /// The source color context. Default null
        /// </summary>
        public ColorContext SourceContext
        {
            get
            {
                if (sourceContext == null)
                    sourceContext = GetComObjectValue<ColorContext>((int)ColorManagementProperties.SourceColorContext);
                return sourceContext;
            }
            set
            {
                sourceContext = value;
                SetValue((int)ColorManagementProperties.SourceColorContext, sourceContext);
            }
        }

        /// <summary>
        /// The rendering intent for the source context.
        /// </summary>
        public ColorManagementRenderingIntent SourceIntent
        {
            get
            {
                return GetEnumValue<ColorManagementRenderingIntent>((int)ColorManagementProperties.SourceRenderingIntent);
            }
            set
            {
                SetEnumValue((int)ColorManagementProperties.SourceRenderingIntent, value);
            }
        }

        /// <summary>
        /// The destination color context. Default null
        /// </summary>
        public ColorContext DestinationContext
        {
            get
            {
                if (destinationContext == null)
                    destinationContext = GetComObjectValue<ColorContext>((int)ColorManagementProperties.DestinationColorContext);
                return destinationContext;
            }
            set
            {
                destinationContext = value;
                SetValue((int)ColorManagementProperties.DestinationColorContext, destinationContext);
            }
        }


        /// <summary>
        /// The rendering intent for the destination context.
        /// </summary>
        public ColorManagementRenderingIntent DestinationIntent
        {
            get
            {
                return GetEnumValue<ColorManagementRenderingIntent>((int)ColorManagementProperties.DestinationRenderingIntent);
            }
            set
            {
                SetEnumValue((int)ColorManagementProperties.DestinationRenderingIntent, value);
            }
        }

        /// <summary>
        /// The alpha mode of this color management.
        /// </summary>
        public ColorManagementAlphaMode AlphaMode
        {
            get
            {
                return GetEnumValue<ColorManagementAlphaMode>((int)ColorManagementProperties.AlphaMode);
            }
            set
            {
                SetEnumValue((int)ColorManagementProperties.AlphaMode, value);
            }
        }
    }
}