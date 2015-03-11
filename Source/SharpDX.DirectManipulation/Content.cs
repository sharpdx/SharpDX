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
				var values = new float[6];
				GetContentTransform(values, 6);
				return ToMatrix(values);
			}
		}

		/// <summary>
		/// Gets the final transform applied to the content.
		/// </summary>
		public RawMatrix3x2 OutputTransform
		{
			get
			{
				var values = new float[6];
				GetOutputTransform(values, 6);
			    return ToMatrix(values);
			}
		}

		/// <summary>
		/// Modifies the content transform while maintaining the output transform.
		/// </summary>
		/// <param name="transform"></param>
		public void SyncContentTransform(RawMatrix3x2 transform)
		{
			var values = ToArray(transform);
			SyncContentTransform(values, 6);
		}

        /// <summary>
        /// Converts the float array to the equivalend <see cref="RawMatrix3x2"/>.
        /// </summary>
        /// <param name="values">The values to convert.</param>
        /// <returns>The converted result.</returns>
	    private static RawMatrix3x2 ToMatrix(float[] values)
	    {
	        return new RawMatrix3x2
	               {
	                   M11 = values[0],
	                   M12 = values[1],
	                   M21 = values[2],
	                   M22 = values[3],
	                   M31 = values[4],
	                   M32 = values[5]
	               };
	    }

        /// <summary>
        /// Converts the <see cref="RawMatrix3x2"/> to the equivalend float array.
        /// </summary>
        /// <param name="matrix">The matrix to convert.</param>
        /// <returns>The converted result array.</returns>
	    private static float[] ToArray(RawMatrix3x2 matrix)
	    {
	        return new[]
	               {
	                   matrix.M11, matrix.M12,
	                   matrix.M21, matrix.M22,
	                   matrix.M31, matrix.M32,
	               };
	    }
	}
}
