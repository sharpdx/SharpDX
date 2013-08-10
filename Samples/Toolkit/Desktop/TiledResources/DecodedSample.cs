namespace TiledResources
{
    /// <summary>
    /// Represents a sample that has been decoded from the preview pass. Used in computation of which tiles to load.
    /// </summary>
    internal struct DecodedSample
    {
        /// <summary>
        /// U address in the face texture
        /// </summary>
        public readonly float U;

        /// <summary>
        /// V address in the face texture
        /// </summary>
        public readonly float V;

        /// <summary>
        /// Mip level of the face texture
        /// </summary>
        public readonly short Mip;

        /// <summary>
        /// The face index in the cube texture
        /// </summary>
        public readonly short Face;

        public DecodedSample(float u, float v, short mip, short face)
        {
            U = u;
            V = v;
            Mip = mip;
            Face = face;
        }
    }
}