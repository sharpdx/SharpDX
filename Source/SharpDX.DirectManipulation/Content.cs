using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpDX.Mathematics.Interop;

namespace SharpDX.DirectManipulation
{
	partial class Content
	{
		/// <summary>
		/// Retrieves the transform applied to the content.
		/// </summary>
		public RawMatrix3x2 ContentTransform
		{
			get
			{
				float[] values = new float[6];
				GetContentTransform(values, 6);
				return new RawMatrix3x2(values);
			}
		}

		/// <summary>
		/// Gets the final transform applied to the content.
		/// </summary>
		public RawMatrix3x2 OutputTransform
		{
			get
			{
				float[] values = new float[6];
				GetOutputTransform(values, 6);
				return new RawMatrix3x2(values);
			}
		}

		/// <summary>
		/// Modifies the content transform while maintaining the output transform.
		/// </summary>
		/// <param name="transform"></param>
		public void SyncContentTransform(RawMatrix3x2 transform)
		{
			float[] values = transform.ToArray();
			SyncContentTransform(values, 6);
		}
	}
}
