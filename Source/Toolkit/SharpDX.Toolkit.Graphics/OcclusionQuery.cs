using System;
using SharpDX.Direct3D11;

namespace SharpDX.Toolkit.Graphics
{
    /// <summary>
    /// A D3D query used to determine the number of visible pixels of a drawn object(s)
    /// </summary>
    public class OcclusionQuery : Component
    {
        private Query d3dQuery;

        private bool isBeginCalled = false;
        private bool isQueryPerformed = false;
        private bool isCompleteCalled = false;
        private ulong pixelCount = 0;

        /// <summary>
        /// Gets a value indicating whether or not the query is complete.
        /// </summary>
        public bool IsComplete
        {
            get
            {
                if (!isQueryPerformed)
                {
                    throw new InvalidOperationException("The query was not performed; use the Begin/End pair to perform a query.");
                }

                if (isBeginCalled)
                {
                    throw new InvalidOperationException("IsComplete can only be called after End.");
                }

                isCompleteCalled = true;

                return d3dQuery.Device.ImmediateContext.GetData(d3dQuery, out pixelCount);
            }
        }

        /// <summary>
        /// Gets the number of unoccluded (visible) pixels.
        /// </summary>
        /// <remarks>
        /// A value of 0 is only valid if the query was successfully completed.
        /// </remarks>
        public int PixelCount
        {
            get
            {
                if (!isCompleteCalled)
                {
                    throw new Exception("You must use IsComplete to determine if data is available.");
                }

                return (int)pixelCount;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OcclusionQuery" /> class.
        /// </summary>
        public OcclusionQuery(GraphicsDevice graphicsDevice)
        {
            if (graphicsDevice.Features.Level == SharpDX.Direct3D.FeatureLevel.Level_9_1)
            {
                // Occlusion queries are not supported on Level_9_1: http://msdn.microsoft.com/en-us/library/windows/desktop/ff476150(v=vs.85).aspx
                throw new NotSupportedException(graphicsDevice.Features.Level + " does not support occlusion querying");
            }

            try
            {
                d3dQuery = ToDispose(
                    new Query(
                        graphicsDevice,
                        new QueryDescription
                        {
                            Type = QueryType.Occlusion,
                            Flags = QueryFlags.None
                        }));
            }
            catch
            {
                throw new NotSupportedException("This device does not support occlusion querying");
            }
        }

        /// <summary>
        /// Begins the query.
        /// </summary>
        public void Begin()
        {
            if (isBeginCalled)
            {
                throw new InvalidOperationException("End must be called before begin");
            }

            d3dQuery.Device.ImmediateContext.Begin(d3dQuery);

            isBeginCalled = true;
            isCompleteCalled = false;
        }

        /// <summary>
        /// Ends the query.
        /// </summary>
        public void End()
        {
            if (!isBeginCalled)
            {
                throw new InvalidOperationException("Begin must be called before End");
            }

            d3dQuery.Device.ImmediateContext.End(d3dQuery);

            isBeginCalled = false;
            isQueryPerformed = true;
        }
    }
}
