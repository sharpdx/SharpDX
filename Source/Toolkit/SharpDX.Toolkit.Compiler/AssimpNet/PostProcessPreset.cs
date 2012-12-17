
namespace Assimp {
    /// <summary>
    /// Static class containing preset properties for post processing options.
    /// </summary>
    internal static class PostProcessPreset {

        /// <summary>
        /// PostProcess configuration for (some) Direct3D conventions,
        /// left handed geometry, upper left origin for UV coordinates,
        /// and clockwise face order, suitable for CCW culling.
        /// </summary>
        public static PostProcessSteps ConvertToLeftHanded {
            get {
                return PostProcessSteps.MakeLeftHanded |
                    PostProcessSteps.FlipUVs |
                    PostProcessSteps.FlipWindingOrder;
            }
        }

        /// <summary>
        /// PostProcess configuration for optimizing data for real-time.
        /// Does the following steps:
        /// 
        /// <see cref="PostProcessSteps.CalculateTangentSpace"/>, <see cref="PostProcessSteps.GenerateNormals"/>, 
        /// <see cref="PostProcessSteps.JoinIdenticalVertices"/>, <see cref="PostProcessSteps.Triangulate"/>,
        /// <see cref="PostProcessSteps.GenerateUVCoords"/>, and <see cref="PostProcessSteps.SortByPrimitiveType"/>
        /// </summary>
        public static PostProcessSteps TargetRealTimeFast {
            get {
                return PostProcessSteps.CalculateTangentSpace |
                    PostProcessSteps.GenerateNormals |
                    PostProcessSteps.JoinIdenticalVertices |
                    PostProcessSteps.Triangulate |
                    PostProcessSteps.GenerateUVCoords |
                    PostProcessSteps.SortByPrimitiveType;
            }
        }

        /// <summary>
        /// PostProcess configuration for optimizing
        /// data for real-time rendering. Does the following steps:
        /// 
        /// <see cref="PostProcessSteps.CalculateTangentSpace"/>, <see cref="PostProcessSteps.GenerateSmoothNormals"/>, 
        /// <see cref="PostProcessSteps.JoinIdenticalVertices"/>, <see cref="PostProcessSteps.Triangulate"/>,
        /// <see cref="PostProcessSteps.GenerateUVCoords"/>, <see cref="PostProcessSteps.SortByPrimitiveType"/>
        /// <see cref="PostProcessSteps.LimitBoneWeights"/>, <see cref="PostProcessSteps.RemoveRedundantMaterials"/>,
        /// <see cref="PostProcessSteps.SplitLargeMeshes"/>, <see cref="PostProcessSteps.FindDegenerates"/>, and
        /// <see cref="PostProcessSteps.FindInvalidData"/>
        /// </summary>
        public static PostProcessSteps TargetRealTimeQuality {
            get {
                return PostProcessSteps.CalculateTangentSpace |
                    PostProcessSteps.GenerateSmoothNormals |
                    PostProcessSteps.JoinIdenticalVertices |
                    PostProcessSteps.LimitBoneWeights |
                    PostProcessSteps.RemoveRedundantMaterials |
                    PostProcessSteps.SplitLargeMeshes |
                    PostProcessSteps.Triangulate |
                    PostProcessSteps.GenerateUVCoords |
                    PostProcessSteps.SortByPrimitiveType |
                    PostProcessSteps.FindDegenerates |
                    PostProcessSteps.FindInvalidData;
            }
        }

        /// <summary>
        /// PostProcess configuration for heavily optimizing the data
        /// for real-time rendering. Includes all flags in
        /// <see cref="PostProcessPreset.TargetRealTimeQuality"/> as well as 
        /// <see cref="PostProcessSteps.FindInstances"/>, <see cref="PostProcessSteps.ValidateDataStructure"/>, and
        /// <see cref="PostProcessSteps.OptimizeMeshes"/>
        /// </summary>
        public static PostProcessSteps TargetRealTimeMaximumQuality {
            get {
                return TargetRealTimeQuality |
                    PostProcessSteps.FindInstances |
                    PostProcessSteps.ValidateDataStructure |
                    PostProcessSteps.OptimizeMeshes;
            }
        }
    }
}
