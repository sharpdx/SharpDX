/*
* Copyright (c) 2012 Nicholas Woodfield
* 
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
* 
* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
* 
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
* THE SOFTWARE.
*/

using System;
using System.Text;
using Assimp.Unmanaged;

namespace Assimp.Configs {
    /// <summary>
    /// Base property config.
    /// </summary>
    internal abstract class PropertyConfig {
        private String m_name;

        /// <summary>
        /// Gets the property name.
        /// </summary>
        public String Name {
            get {
                return m_name;
            }
        }

        /// <summary>
        /// Creates a new property config that has no active Assimp property store.
        /// </summary>
        /// <param name="name">Name of the property.</param>
        protected PropertyConfig(String name) {
            m_name = name;
        }

        /// <summary>
        /// Sets the current value to the default value.
        /// </summary>
        public abstract void SetDefaultValue();

        /// <summary>
        /// Applies the property value to the given Assimp property store.
        /// </summary>
        /// <param name="propStore">Assimp property store</param>
        internal void ApplyValue(IntPtr propStore) {
            OnApplyValue(propStore);
        }

        /// <summary>
        /// Applies the property value to the given Assimp property store.
        /// </summary>
        /// <param name="propStore">Assimp property store</param>
        protected abstract void OnApplyValue(IntPtr propStore);
    }

    /// <summary>
    /// Describes an integer configuration property.
    /// </summary>
    internal class IntegerPropertyConfig : PropertyConfig {
        private int m_value;
        private int m_defaultValue;

        /// <summary>
        /// Gets the property value.
        /// </summary>
        public int Value {
            get {
                return m_value;
            }
            set {
                m_value = value;
            }
        }

        /// <summary>
        /// Gets the default property value.
        /// </summary>
        public int DefaultValue {
            get {
                return m_defaultValue;
            }
        }

        /// <summary>
        /// Constructs a new IntengerPropertyConfig.
        /// </summary>
        /// <param name="name">Name of the property</param>
        /// <param name="value">Property value</param>
        public IntegerPropertyConfig(String name, int value)
            : this(name, value, 0) { }

        /// <summary>
        /// constructs a new IntegerPropertyConfig with a default value.
        /// </summary>
        /// <param name="name">Name of the property</param>
        /// <param name="value">Property value</param>
        /// <param name="defaultValue">The default property value</param>
        public IntegerPropertyConfig(String name, int value, int defaultValue)
            : base(name) {
            m_value = value;
            m_defaultValue = defaultValue;
        }

        /// <summary>
        /// Sets the current value to the default value.
        /// </summary>
        public override void  SetDefaultValue() {
            m_value = m_defaultValue;
        }

        /// <summary>
        /// Applies the property value to the given Assimp property store.
        /// </summary>
        /// <param name="propStore">Assimp property store</param>
        protected override void OnApplyValue(IntPtr propStore) {
            if(propStore != IntPtr.Zero) {
                AssimpMethods.SetImportPropertyInteger(propStore, Name, m_value);
            }
        }
    }

    /// <summary>
    /// Describes a float configuration property.
    /// </summary>
    internal class FloatPropertyConfig : PropertyConfig {
        private float m_value;
        private float m_defaultValue;

        /// <summary>
        /// Gets the property value.
        /// </summary>
        public float Value {
            get {
                return m_value;
            }
            set {
                m_value = value;
            }
        }

        /// <summary>
        /// Gets the default property value.
        /// </summary>
        public float DefaultValue {
            get {
                return m_defaultValue;
            }
        }

        /// <summary>
        /// Constructs a new FloatPropertyConfig.
        /// </summary>
        /// <param name="name">Name of the property</param>
        /// <param name="value">Property value</param>
        public FloatPropertyConfig(String name, float value)
            : this(name, value, 0.0f) { }

        /// <summary>
        /// Constructs a new FloatPropertyConfig with a default value.
        /// </summary>
        /// <param name="name">Name of the property</param>
        /// <param name="value">Property value</param>
        /// <param name="defaultValue">The default property value</param>
        public FloatPropertyConfig(String name, float value, float defaultValue)
            : base(name) {
            m_value = value;
            m_defaultValue = defaultValue;
        }

        /// <summary>
        /// Sets the current value to the default value.
        /// </summary>
        public override void  SetDefaultValue() {
            m_value = m_defaultValue;
        }

        /// <summary>
        /// Applies the property value to the given Assimp property store.
        /// </summary>
        /// <param name="propStore">Assimp property store</param>
        protected override void OnApplyValue(IntPtr propStore) {
            if(propStore != IntPtr.Zero) {
                AssimpMethods.SetImportPropertyFloat(propStore, Name, m_value);
            }
        }
    }

    /// <summary>
    /// Describes a boolean configuration property.
    /// </summary>
    internal class BooleanPropertyConfig : PropertyConfig {
        private bool m_value;
        private bool m_defaultValue;

        /// <summary>
        /// Gets the property value.
        /// </summary>
        public bool Value {
            get {
                return m_value;
            }
            set {
                m_value = value;
            }
        }

        /// <summary>
        /// Gets the default property value.
        /// </summary>
        public bool DefaultValue {
            get {
                return m_defaultValue;
            }
        }

        /// <summary>
        /// Constructs a new BooleanPropertyConfig.
        /// </summary>
        /// <param name="name">Name of the property</param>
        /// <param name="value">Property value</param>
        public BooleanPropertyConfig(String name, bool value)
            : this(name, value, false) { }

        /// <summary>
        /// Constructs a new BooleanPropertyConfig with a default value.
        /// </summary>
        /// <param name="name">Name of the property</param>
        /// <param name="value">Property value</param>
        /// <param name="defaultValue">The default property value</param>
        public BooleanPropertyConfig(String name, bool value, bool defaultValue)
            : base(name) {
            m_value = value;
            m_defaultValue = defaultValue;
        }

        /// <summary>
        /// Sets the current value to the default value.
        /// </summary>
        public override void  SetDefaultValue() {
            m_value = m_defaultValue;
        }

        /// <summary>
        /// Applies the property value to the given Assimp property store.
        /// </summary>
        /// <param name="propStore">Assimp property store</param>
        protected override void OnApplyValue(IntPtr propStore) {
            if(propStore != IntPtr.Zero) {
                int aiBool = (m_value) ? 1 : 0;
                AssimpMethods.SetImportPropertyInteger(propStore, Name, aiBool);
            }
        }
    }

    /// <summary>
    /// Describes a string configuration property.
    /// </summary>
    internal class StringPropertyConfig : PropertyConfig {
        private String m_value;
        private String m_defaultValue;

        /// <summary>
        /// Gets the property value.
        /// </summary>
        public String Value {
            get {
                return m_value;
            }
            set {
                m_value = value;
            }
        }

        /// <summary>
        /// Gets the default property value.
        /// </summary>
        public String DefaultValue {
            get {
                return m_defaultValue;
            }
        }

        /// <summary>
        /// Constructs a new StringPropertyConfig.
        /// </summary>
        /// <param name="name">Name of the property</param>
        /// <param name="value">Property value</param>
        public StringPropertyConfig(String name, String value)
            : this(name, value, String.Empty) { }

        /// <summary>
        /// Constructs a new StringPropertyConfig with a default value.
        /// </summary>
        /// <param name="name">Name of the property</param>
        /// <param name="value">Property value</param>
        /// <param name="defaultValue">The default property value</param>
        public StringPropertyConfig(String name, String value, String defaultValue)
            : base(name) {
            m_value = value;
            m_defaultValue = defaultValue;
        }

        /// <summary>
        /// Sets the current value to the default value.
        /// </summary>
        public override void  SetDefaultValue() {
            m_value = m_defaultValue;
        }

        /// <summary>
        /// Applies the property value to the given Assimp property store.
        /// </summary>
        /// <param name="propStore">Assimp property store</param>
        protected override void OnApplyValue(IntPtr propStore) {
            if(propStore != IntPtr.Zero) {
                AssimpMethods.SetImportPropertyString(propStore, Name, m_value);
            }
        }

        /// <summary>
        /// Convience method for constructing a whitespace delimited name list.
        /// </summary>
        /// <param name="names">Array of names</param>
        /// <returns>White-space delimited list as a string</returns>
        protected static String ProcessNames(String[] names) {
            if(names == null || names.Length == 0) {
                return String.Empty;
            }

            StringBuilder builder = new StringBuilder();
            foreach(String name in names) {
                if(!String.IsNullOrEmpty(name)) {
                    builder.Append(name);
                    builder.Append(' ');
                }
            }
            return builder.ToString();
        }
    }

    #region Library settings

    /// <summary>
    /// Configuration to enable time measurements. If enabled, each
    /// part of the loading process is timed and logged.
    /// </summary>
    internal sealed class MeasureTimeConfig : BooleanPropertyConfig {

        /// <summary>
        /// Gets the string name used by MeasureTimeConfig.
        /// </summary>
        public static String MeasureTimeConfigName {
            get {
                return AiConfigs.AI_CONFIG_GLOB_MEASURE_TIME;
            }
        }

        /// <summary>
        /// Constructs a new MeasureTimeConfig.
        /// </summary>
        /// <param name="measureTime">True if the loading process should be timed or not.</param>
        public MeasureTimeConfig(bool measureTime) 
            : base(MeasureTimeConfigName, measureTime, false) { }
    }

    /// <summary>
    /// Configuration to set Assimp's multithreading policy. Possible
    /// values are -1 to let Assimp decide, 0 to disable multithreading, or
    /// any number larger than zero to force a specific number of threads. This
    /// is only a hint and may be ignored by Assimp.
    /// </summary>
    internal sealed class MultithreadingConfig : IntegerPropertyConfig {

        /// <summary>
        /// Gets the string name used by MultithreadingConfig.
        /// </summary>
        public static String MultithreadingConfigName {
            get {
                return AiConfigs.AI_CONFIG_GLOB_MULTITHREADING;
            }
        }
        
        /// <summary>
        /// Constructs a new MultithreadingConfig.
        /// </summary>
        /// <param name="value">A value of -1 will let Assimp decide,
        /// a value of zero to disable multithreading, and a value greater than zero
        /// to force a specific number of threads.</param>
        public MultithreadingConfig(int value) 
            : base(MultithreadingConfigName, value, -1) { }
    }

    #endregion

    #region Post Processing Settings

    /// <summary>
    /// Configuration to set the maximum angle that may be between two vertex tangents/bitangents
    /// when they are smoothed during the step to calculate the tangent basis. The default
    /// value is 45 degrees.
    /// </summary>
    internal sealed class TangentSmoothingAngleConfig : FloatPropertyConfig {

        /// <summary>
        /// Gets the string name used by TangentSmoothingAngleConfig.
        /// </summary>
        public static String TangentSmoothingAngleConfigName {
            get {
                return AiConfigs.AI_CONFIG_PP_CT_MAX_SMOOTHING_ANGLE;
            }
        }

        /// <summary>
        /// Constructs a new TangentSmoothingAngleConfig.
        /// </summary>
        /// <param name="angle">Smoothing angle, in degrees.</param>
        public TangentSmoothingAngleConfig(float angle)
            : base(TangentSmoothingAngleConfigName, Math.Min(angle, 175.0f), 45.0f) { }
    }

    /// <summary>
    /// Configuration to set the maximum angle between two face normals at a vertex when
    /// they are smoothed during the step to calculate smooth normals. This is frequently
    /// called the "crease angle". The maximum and default value is 175 degrees.
    /// </summary>
    internal sealed class NormalSmoothingAngleConfig : FloatPropertyConfig {

        /// <summary>
        /// Gets the string name used by NormalSmoothingAngleConfig.
        /// </summary>
        public static String NormalSmoothingAngleConfigName {
            get {
                return AiConfigs.AI_CONFIG_PP_GSN_MAX_SMOOTHING_ANGLE;
            }
        }
        /// <summary>
        /// Constructs a new NormalSmoothingAngleConfig.
        /// </summary>
        /// <param name="angle">Smoothing angle, in degrees.</param>
        public NormalSmoothingAngleConfig(float angle)
            : base(NormalSmoothingAngleConfigName, Math.Min(angle, 175.0f), 175.0f) { }
    }

    /// <summary>
    /// Configuration to set the colormap (palette) to be used to decode embedded textures in MDL (Quake or 3DG5)
    /// files. This must be a valid path to a file. The file is 768 (256 * 3) bytes alrge and contains
    /// RGB triplets for each of the 256 palette entries. If the file is not found, a
    /// default palette (from Quake 1) is used. The default value is "colormap.lmp".
    /// </summary>
    internal sealed class MDLColorMapConfig : StringPropertyConfig {

        /// <summary>
        /// Gets the string name used by MDLColorMapConfig.
        /// </summary>
        public static String MDLColorMapConfigName {
            get {
                return AiConfigs.AI_CONFIG_IMPORT_MDL_COLORMAP;
            }
        }

        /// <summary>
        /// Constructs a new MDLColorMapConfig.
        /// </summary>
        /// <param name="fileName">Colormap filename</param>
        public MDLColorMapConfig(String fileName) 
            : base(MDLColorMapConfigName, (String.IsNullOrEmpty(fileName)) ? "colormap.lmp" : fileName, "colormap.lmp") { }
    }

    /// <summary>
    /// Configuration for the the <see cref="PostProcessSteps.RemoveRedundantMaterials"/> step
    /// to determine what materials to keep. If a material matches one of these names it will not
    /// be modified or removed by the post processing step. Default is an empty string.
    /// </summary>
    internal sealed class MaterialExcludeListConfig : StringPropertyConfig {

        /// <summary>
        /// Gets the string name used by MaterialExcludeListConfig.
        /// </summary>
        public static String MaterialExcludeListConfigName {
            get {
                return AiConfigs.AI_CONFIG_PP_RRM_EXCLUDE_LIST;
            }
        }

        /// <summary>
        /// Constructs a new MaterialExcludeListConfig. Material names containing whitespace
        /// <c>must</c> be enclosed in single quotation marks.
        /// </summary>
        /// <param name="materialNames">List of material names that will not be modified or replaced by the remove redundant materials post process step.</param>
        public MaterialExcludeListConfig(String[] materialNames)
            : base(MaterialExcludeListConfigName, ProcessNames(materialNames), String.Empty) { }
    }

    /// <summary>
    /// Configuration for the <see cref="PostProcessSteps.PreTransformVertices"/> step
    /// to keep the scene hierarchy. Meshes are moved to worldspace, but no optimization is performed
    /// where meshes with the same materials are not joined. This option can be useful
    /// if you have a scene hierarchy that contains important additional information
    /// which you intend to parse. The default value is false.
    /// </summary>
    internal sealed class KeepSceneHierarchyConfig : BooleanPropertyConfig {

        /// <summary>
        /// Gets the string name used by KeepSceneHierarchyConfig.
        /// </summary>
        public static String KeepSceneHierarchyConfigName {
            get {
                return AiConfigs.AI_CONFIG_PP_PTV_KEEP_HIERARCHY;
            }
        }

        /// <summary>
        /// Constructs a new KeepHierarchyConfig. 
        /// </summary>
        /// <param name="keepHierarchy">True to keep the hierarchy, false otherwise.</param>
        public KeepSceneHierarchyConfig(bool keepHierarchy)
            : base(KeepSceneHierarchyConfigName, keepHierarchy, false) { }
    }

    /// <summary>
    /// Configuration for the <see cref="PostProcessSteps.PreTransformVertices"/> step
    /// to normalize all vertex components into the -1...1 range. The default value is
    /// false.
    /// </summary>
    internal sealed class NormalizeVertexComponentsConfig : BooleanPropertyConfig {

        /// <summary>
        /// Gets the string name used by NormalizeVertexComponentsConfig.
        /// </summary>
        public static String NormalizeVertexComponentsConfigName {
            get {
                return AiConfigs.AI_CONFIG_PP_PTV_NORMALIZE;
            }
        }

        /// <summary>
        /// Constructs a new NormalizeVertexComponentsConfig.
        /// </summary>
        /// <param name="normalizeVertexComponents">True if the post process step should normalize vertex components, false otherwise.</param>
        public NormalizeVertexComponentsConfig(bool normalizeVertexComponents) 
            : base(NormalizeVertexComponentsConfigName, normalizeVertexComponents, false) { }
    }

    /// <summary>
    /// Configuration for the <see cref="PostProcessSteps.FindDegenerates"/> step to
    /// remove degenerted primitives from the import immediately. The default value is false,
    /// where degenerated triangles are converted to lines, and degenerated lines to points.
    /// </summary>
    internal sealed class RemoveDegeneratePrimitivesConfig : BooleanPropertyConfig {

        /// <summary>
        /// Gets the string name used by RemoveDegeneratePrimitivesConfig.
        /// </summary>
        public static String RemoveDegeneratePrimitivesConfigName {
            get {
                return AiConfigs.AI_CONFIG_PP_FD_REMOVE;
            }
        }

        /// <summary>
        /// Constructs a new RemoveDegeneratePrimitivesConfig.
        /// </summary>
        /// <param name="removeDegenerates">True if the post process step should remove degenerate primitives, false otherwise.</param>
        public RemoveDegeneratePrimitivesConfig(bool removeDegenerates) 
            : base (RemoveDegeneratePrimitivesConfigName, removeDegenerates, false) { }
    }

    /// <summary>
    /// Configuration for the <see cref="PostProcessSteps.OptimizeGraph"/> step
    /// to preserve nodes matching a name in the given list. Nodes that match the names in the list
    /// will not be modified or removed. Identifiers containing whitespaces
    /// <c>must</c> be enclosed in single quotation marks. The default value is an
    /// empty string.
    /// </summary>
    internal sealed class NodeExcludeListConfig : StringPropertyConfig {

        /// <summary>
        /// Gets the string name used by NodeExcludeListConfig.
        /// </summary>
        public static String NodeExcludeListConfigName {
            get {
                return AiConfigs.AI_CONFIG_PP_OG_EXCLUDE_LIST;
            }
        }

        /// <summary>
        /// Constructs a new NodeExcludeListConfig.
        /// </summary>
        /// <param name="nodeNames">List of node names</param>
        public NodeExcludeListConfig(params String[] nodeNames) 
            : base(NodeExcludeListConfigName, ProcessNames(nodeNames), String.Empty) { }
    }

    /// <summary>
    /// Configuration for the <see cref="PostProcessSteps.SplitLargeMeshes"/> step 
    /// that specifies the maximum number of triangles a mesh can contain. The
    /// default value is MeshTriangleLimitConfigDefaultValue.
    /// </summary>
    internal sealed class MeshTriangleLimitConfig : IntegerPropertyConfig {

        /// <summary>
        /// Gets the string name used by MeshTriangleLimitConfig.
        /// </summary>
        public static String MeshTriangleLimitConfigName {
            get {
                return AiConfigs.AI_CONFIG_PP_SLM_TRIANGLE_LIMIT;
            }
        }

        /// <summary>
        /// Gets the defined default limit value, this corresponds to the
        /// <see cref="AiDefines.AI_SLM_DEFAULT_MAX_TRIANGLES"/> constant.
        /// </summary>
        public static int MeshTriangleLimitConfigDefaultValue {
            get {
                return AiDefines.AI_SLM_DEFAULT_MAX_TRIANGLES;
            }
        }

        /// <summary>
        /// Constructs a new MeshTriangleLimitConfig.
        /// </summary>
        /// <param name="maxTriangleLimit">Max number of triangles a mesh can contain.</param>
        public MeshTriangleLimitConfig(int maxTriangleLimit) 
            : base(MeshTriangleLimitConfigName, maxTriangleLimit, MeshTriangleLimitConfigDefaultValue) { }
    }

    /// <summary>
    /// Configuration for the <see cref="PostProcessSteps.SplitLargeMeshes"/> step
    /// that specifies the maximum number of vertices a mesh can contain. The
    /// default value is MeshVertexLimitConfigDefaultValue.
    /// </summary>
    internal sealed class MeshVertexLimitConfig : IntegerPropertyConfig {

        /// <summary>
        /// Gets the string name used by MeshVertexLimitConfig.
        /// </summary>
        public static String MeshVertexLimitConfigName {
            get {
                return AiConfigs.AI_CONFIG_PP_SLM_VERTEX_LIMIT;
            }
        }

        /// <summary>
        /// Gets the defined default limit value, this corresponds to the
        /// <see cref="AiDefines.AI_SLM_DEFAULT_MAX_VERTICES"/> constant.
        /// </summary>
        public static int MeshVertexLimitConfigDefaultValue {
            get {
                return AiDefines.AI_SLM_DEFAULT_MAX_VERTICES;
            }
        }

        /// <summary>
        /// Constructs a new MeshVertexLimitConfig.
        /// </summary>
        /// <param name="maxVertexLimit">Max number of vertices a mesh can contain.</param>
        public MeshVertexLimitConfig(int maxVertexLimit) 
            : base(MeshVertexLimitConfigName, maxVertexLimit, MeshVertexLimitConfigDefaultValue) { }
    }

    /// <summary>
    /// Configuration for the <see cref="PostProcessSteps.LimitBoneWeights"/> step
    /// that specifies the maximum number of bone weights per vertex. The default
    /// value is VertexBoneWeightLimitConfigDefaultValue.
    /// </summary>
    internal sealed class VertexBoneWeightLimitConfig : IntegerPropertyConfig {

        /// <summary>
        /// gets the string name used by VertexBoneWeightLimitConfig.
        /// </summary>
        public static String VertexBoneWeightLimitConfigName {
            get {
                return AiConfigs.AI_CONFIG_PP_LBW_MAX_WEIGHTS;
            }
        }

        /// <summary>
        /// Gets the defined default limit value, this corresponds to the
        /// <see cref="AiDefines.AI_LBW_MAX_WEIGHTS"/> constant.
        /// </summary>
        public static int VertexBoneWeightLimitConfigDefaultValue {
            get {
                return AiDefines.AI_LBW_MAX_WEIGHTS;
            }
        }

        /// <summary>
        /// Constructs a new VertexBoneWeightLimitConfig.
        /// </summary>
        /// <param name="maxBoneWeights">Max number of bone weights per vertex.</param>
        public VertexBoneWeightLimitConfig(int maxBoneWeights) 
            : base(VertexBoneWeightLimitConfigName, maxBoneWeights, VertexBoneWeightLimitConfigDefaultValue) { }
    }

    /// <summary>
    /// Configuration for the <see cref="PostProcessSteps.ImproveCacheLocality"/> step
    /// that specifies the size of the post-transform vertex cache. The size is
    /// given in number of vertices and the default value is VertexCacheSizeConfigDefaultValue.
    /// </summary>
    internal sealed class VertexCacheSizeConfig : IntegerPropertyConfig {
        
        /// <summary>
        /// Gets the string name used by VertexCacheConfig.
        /// </summary>
        public static String VertexCacheSizeConfigName {
            get {
                return AiConfigs.AI_CONFIG_PP_ICL_PTCACHE_SIZE;
            }
        }

        /// <summary>
        /// Gets the defined default vertex cache size, this corresponds to 
        /// the <see cref="AiDefines.PP_ICL_PTCACHE_SIZE"/>.
        /// </summary>
        public static int VertexCacheSizeConfigDefaultValue {
            get {
                return AiDefines.PP_ICL_PTCACHE_SIZE;
            }
        }

        /// <summary>
        /// Constructs a new VertexCacheSizeConfig.
        /// </summary>
        /// <param name="vertexCacheSize">Size of the post-transform vertex cache, in number of vertices.</param>
        public VertexCacheSizeConfig(int vertexCacheSize) 
            : base(VertexCacheSizeConfigName, vertexCacheSize, VertexCacheSizeConfigDefaultValue) { }
    }

    /// <summary>
    /// Configuration for the <see cref="PostProcessSteps.RemoveComponent"/> step that
    /// specifies which parts of the data structure is to be removed. If no valid mesh
    /// remains after the step, the import fails. The default value i <see cref="ExcludeComponent.None"/>.
    /// </summary>
    internal sealed class RemoveComponentConfig : IntegerPropertyConfig {

        /// <summary>
        /// Gets the string name used by RemoveComponentConfig.
        /// </summary>
        public static String RemoveComponentConfigName {
            get {
                return AiConfigs.AI_CONFIG_PP_RVC_FLAGS;
            }
        }

        /// <summary>
        /// Constructs a new RemoveComponentConfig.
        /// </summary>
        /// <param name="componentsToExclude">Bit-wise combination of components to exclude.</param>
        public RemoveComponentConfig(ExcludeComponent componentsToExclude) 
            : base(RemoveComponentConfigName, (int) componentsToExclude, (int) ExcludeComponent.None) { }
    }

    /// <summary>
    /// Configuration for the <see cref="PostProcessSteps.SortByPrimitiveType"/> step that
    /// specifies which primitive types are to be removed by the step. Specifying all
    /// primitive types is illegal. The default value is zero specifying none.
    /// </summary>
    internal sealed class SortByPrimitiveTypeConfig : IntegerPropertyConfig {

        /// <summary>
        /// Gets the string name used by SortByPrimitiveTypeConfig.
        /// </summary>
        public static String SortByPrimitiveTypeConfigName {
            get {
                return AiConfigs.AI_CONFIG_PP_SBP_REMOVE;
            }
        }

        /// <summary>
        /// Constructs a new SortByPrimitiveTypeConfig.
        /// </summary>
        /// <param name="typesToRemove">Bit-wise combination of primitive types to remove</param>
        public SortByPrimitiveTypeConfig(PrimitiveType typesToRemove)
            : base(SortByPrimitiveTypeConfigName, (int) typesToRemove, 0) { }
    }

    /// <summary>
    /// Configuration for the <see cref="PostProcessSteps.FindInvalidData"/> step that
    /// specifies the floating point accuracy for animation values, specifically
    /// the episilon during comparisons. The default value is 0.0f.
    /// </summary>
    internal sealed class AnimationAccuracyConfig : FloatPropertyConfig {

        /// <summary>
        /// Gets the string name used by AnimationAccuracyConfig.
        /// </summary>
        public static String AnimationAccuracyConfigName {
            get {
                return AiConfigs.AI_CONFIG_PP_FID_ANIM_ACCURACY;
            }
        }

        /// <summary>
        /// Constructs a new AnimationAccuracyConfig.
        /// </summary>
        /// <param name="episilon">Episilon for animation value comparisons.</param>
        public AnimationAccuracyConfig(float episilon) 
            : base(AnimationAccuracyConfigName, episilon, 0.0f) { }
    }

    /// <summary>
    /// Configuration for the <see cref="PostProcessSteps.TransformUVCoords"/> step that
    /// specifies which UV transformations are to be evaluated. The default value
    /// is for all combinations (scaling, rotation, translation).
    /// </summary>
    internal sealed class TransformUVConfig : IntegerPropertyConfig {

        /// <summary>
        /// Gets the string name used by TransformUVConfig.
        /// </summary>
        public static String TransformUVConfigName {
            get {
                return AiConfigs.AI_CONFIG_PP_TUV_EVALUATE;
            }
        }

        /// <summary>
        /// Constructs a new TransformUVConfig.
        /// </summary>
        /// <param name="transformFlags">Bit-wise combination specifying which UV transforms that should be evaluated.</param>
        public TransformUVConfig(UVTransformFlags transformFlags)
            : base(TransformUVConfigName, (int) transformFlags, (int) AiDefines.AI_UVTRAFO_ALL) { }
    }

    /// <summary>
    /// Configuration that is a hint to Assimp to favor speed against import quality. Enabling this
    /// option may result in faster loading, or it may not. It is just a hint to loaders
    /// and post-process steps to use faster code paths if possible. The default value is false.
    /// </summary>
    internal sealed class FavorSpeedConfig : BooleanPropertyConfig {

        /// <summary>
        /// Gets the string name used by FavorSpeedConfig.
        /// </summary>
        public static String FavorSpeedConfigName {
            get {
                return AiConfigs.AI_CONFIG_FAVOUR_SPEED;
            }
        }

        /// <summary>
        /// Constructs a new FavorSpeedConfig.
        /// </summary>
        /// <param name="favorSpeed">True if Assimp should favor speed at the expense of quality, false otherwise.</param>
        public FavorSpeedConfig(bool favorSpeed) 
            : base(FavorSpeedConfigName, favorSpeed, false) { }
    }

    /// <summary>
    /// Configures the maximum bone count per mesh for the <see cref="PostProcessSteps.SplitByBoneCount"/> step. Meshes are
    /// split until the maximum number of bones is reached.
    /// </summary>
    internal sealed class MaxBoneCountConfig : IntegerPropertyConfig {

        /// <summary>
        /// Gets the string name used by MaxBoneCountConfig.
        /// </summary>
        public static String MaxBoneCountConfigName {
            get {
                return AiConfigs.AI_CONFIG_PP_SBBC_MAX_BONES;
            }
        }

        /// <summary>
        /// Constructs a new MaxBoneCountConfig.
        /// </summary>
        /// <param name="maxBones">The maximum bone count.</param>
        public MaxBoneCountConfig(int maxBones)
            : base(MaxBoneCountConfigName, maxBones, AiDefines.AI_SBBC_DEFAULT_MAX_BONES) { }
    }

    /// <summary>
    /// Configures which texture channel is used for tangent space computations. The channel must exist or an error will be raised.
    /// </summary>
    internal sealed class TangentTextureChannelIndexConfig : IntegerPropertyConfig {

        /// <summary>
        /// Gets the string name used by TangentTextureChannelIndexConfig.
        /// </summary>
        public static String TangentTextureChannelIndexConfigName {
            get {
                return AiConfigs.AI_CONFIG_PP_CT_TEXTURE_CHANNEL_INDEX;
            }
        }

        /// <summary>
        /// Constructs a new TangentTextureChannelIndexConfig.
        /// </summary>
        /// <param name="textureChannelIndex">The zero-based texture channel index.</param>
        public TangentTextureChannelIndexConfig(int textureChannelIndex)
            : base(TangentTextureChannelIndexConfigName, textureChannelIndex, 0) { }
    }

    /// <summary>
    /// Configures the <see cref="PostProcessSteps.Debone"/> threshold that is used to determine what bones are removed.
    /// </summary>
    internal sealed class DeboneThresholdConfig : FloatPropertyConfig {

        /// <summary>
        /// Gets the string name used by DeboneThresholdConfig.
        /// </summary>
        public static String DeboneThresholdConfigName {
            get {
                return AiConfigs.AI_CONFIG_PP_DB_THRESHOLD;
            }
        }

        /// <summary>
        /// Constructs a new DeboneThresholdConfig.
        /// </summary>
        /// <param name="threshold">The debone threshold.</param>
        public DeboneThresholdConfig(float threshold)
            : base(DeboneThresholdConfigName, threshold, 1.0f) { }
    }


    /// <summary>
    /// Configuration that requires all bones to qualify for deboning before any are removed.
    /// </summary>
    internal sealed class DeboneAllOrNoneConfig : BooleanPropertyConfig {

        /// <summary>
        /// Gets the string name used by DeboneAllOrNoneConfig.
        /// </summary>
        public static String DeboneAllOrNoneConfigName {
            get {
                return AiConfigs.AI_CONFIG_PP_DB_ALL_OR_NONE;
            }
        }

        /// <summary>
        /// Constructs a new DeboneAllOrNoneConfig.
        /// </summary>
        /// <param name="allOrNone">True if all are required, false if none need to qualify.</param>
        public DeboneAllOrNoneConfig(bool allOrNone)
            : base(DeboneAllOrNoneConfigName, allOrNone, false) { }
    }

    #endregion

    #region Importer Settings

    /// <summary>
    /// Sets the vertex animation keyframe to be imported. Assimp does not support vertex keyframes (only
    /// bone animation is supported). the library reads only one keyframe with vertex animations. By default this is the
    /// first frame. This config sets the "global" keyframe that will be imported. There are other configs
    /// for specific importers that will override the global setting.
    /// </summary>
    internal sealed class GlobalKeyFrameImportConfig : IntegerPropertyConfig {
        
        /// <summary>
        /// Gets the string name used by GlobalKeyFrameImportConfig.
        /// </summary>
        public static String GlobalKeyFrameImportConfigName {
            get {
                return AiConfigs.AI_CONFIG_IMPORT_GLOBAL_KEYFRAME;
            }
        }

        /// <summary>
        /// Constructs a new GlobalKeyFrameImportConfig.
        /// </summary>
        /// <param name="keyFrame">Keyframe index</param>
        public GlobalKeyFrameImportConfig(int keyFrame)
            : base(GlobalKeyFrameImportConfigName, keyFrame, 0) { }
    }

    /// <summary>
    /// Sets the vertex animation keyframe to be imported. Assimp does not support vertex keyframes (only
    /// bone animation is supported). the library reads only one keyframe with vertex animations. By default this is the
    /// first frame. This config sets the global override for the MD3 format.
    /// </summary>
    internal sealed class MD3KeyFrameImportConfig : IntegerPropertyConfig {

        /// <summary>
        /// Gets the string name used by MD3KeyFrameImportConfig.
        /// </summary>
        public static String MD3KeyFrameImportConfigName {
            get {
                return AiConfigs.AI_CONFIG_IMPORT_MD3_KEYFRAME;
            }
        }

        /// <summary>
        /// Constructs a new MD3KeyFrameImportConfig.
        /// </summary>
        /// <param name="keyFrame">Keyframe index</param>
        public MD3KeyFrameImportConfig(int keyFrame)
            : base(MD3KeyFrameImportConfigName, keyFrame, 0) { }
    }

    /// <summary>
    /// Sets the vertex animation keyframe to be imported. Assimp does not support vertex keyframes (only
    /// bone animation is supported). the library reads only one keyframe with vertex animations. By default this is the
    /// first frame. This config sets the global override for the MD2 format.
    /// </summary>
    internal sealed class MD2KeyFrameImportConfig : IntegerPropertyConfig {

        /// <summary>
        /// Gets the string name used by MD2KeyFrameImportConfig.
        /// </summary>
        public static String MD2KeyFrameImportConfigName {
            get {
                return AiConfigs.AI_CONFIG_IMPORT_MD2_KEYFRAME;
            }
        }

        /// <summary>
        /// Constructs a new MD2KeyFrameImportConfig.
        /// </summary>
        /// <param name="keyFrame">Keyframe index</param>
        public MD2KeyFrameImportConfig(int keyFrame)
            : base(MD2KeyFrameImportConfigName, keyFrame, 0) { }
    }

    /// <summary>
    /// Sets the vertex animation keyframe to be imported. Assimp does not support vertex keyframes (only
    /// bone animation is supported). the library reads only one keyframe with vertex animations. By default this is the
    /// first frame. This config sets the global override for the MDL format.
    /// </summary>
    internal sealed class MDLKeyFrameImportConfig : IntegerPropertyConfig {

        /// <summary>
        /// Gets the string name used by MDLKeyFrameImportConfig.
        /// </summary>
        public static String MDLKeyFrameImportConfigName {
            get {
                return AiConfigs.AI_CONFIG_IMPORT_MDL_KEYFRAME;
            }
        }

        /// <summary>
        /// Constructs a new MDLKeyFrameImportConfig.
        /// </summary>
        /// <param name="keyFrame">Keyframe index</param>
        public MDLKeyFrameImportConfig(int keyFrame)
            : base(MDLKeyFrameImportConfigName, keyFrame, 0) { }
    }

    /// <summary>
    /// Sets the vertex animation keyframe to be imported. Assimp does not support vertex keyframes (only
    /// bone animation is supported). the library reads only one keyframe with vertex animations. By default this is the
    /// first frame. This config sets the global override for the SMD format.
    /// </summary>
    internal sealed class SMDKeyFrameImportConfig : IntegerPropertyConfig {

        /// <summary>
        /// Gets the string name used by SMDKeyFrameImportConfig.
        /// </summary>
        public static String SMDKeyFrameImportConfigName {
            get {
                return AiConfigs.AI_CONFIG_IMPORT_SMD_KEYFRAME;
            }
        }

        /// <summary>
        /// Constructs a new SMDKeyFrameImportConfig.
        /// </summary>
        /// <param name="keyFrame">Keyframe index</param>
        public SMDKeyFrameImportConfig(int keyFrame)
            : base(SMDKeyFrameImportConfigName, keyFrame, 0) { }
    }

    /// <summary>
    /// Sets the vertex animation keyframe to be imported. Assimp does not support vertex keyframes (only
    /// bone animation is supported). the library reads only one keyframe with vertex animations. By default this is the
    /// first frame. This config sets the global override for the Unreal format.
    /// </summary>
    internal sealed class UnrealKeyFrameImportConfig : IntegerPropertyConfig {

        /// <summary>
        /// Gets the string name used by UnrealKeyFrameImportConfig.
        /// </summary>
        public static String UnrealKeyFrameImportConfigName {
            get {
                return AiConfigs.AI_CONFIG_IMPORT_UNREAL_KEYFRAME;
            }
        }

        /// <summary>
        /// Constructs a new UnrealKeyFrameImportConfig.
        /// </summary>
        /// <param name="keyFrame">Keyframe index</param>
        public UnrealKeyFrameImportConfig(int keyFrame)
            : base(UnrealKeyFrameImportConfigName, keyFrame, 0) { }
    }

    /// <summary>
    /// Configures the AC loader to collect all surfaces which have the "Backface cull" flag set in separate
    /// meshes. The default value is true.
    /// </summary>
    internal sealed class ACSeparateBackfaceCullConfig : BooleanPropertyConfig {

        /// <summary>
        /// Gets the string name used by ACSeparateBackfaceCullConfig.
        /// </summary>
        public static String ACSeparateBackfaceCullConfigName {
            get {
                return AiConfigs.AI_CONFIG_IMPORT_AC_SEPARATE_BFCULL;
            }
        }

        /// <summary>
        /// Constructs a new ACSeparateBackfaceCullConfig.
        /// </summary>
        /// <param name="separateBackfaces">True if all surfaces that have the "backface cull" flag set should be collected in separate meshes, false otherwise.</param>
        public ACSeparateBackfaceCullConfig(bool separateBackfaces)
            : base(ACSeparateBackfaceCullConfigName, separateBackfaces, true) { }
    }

    /// <summary>
    /// Configures whether the AC loader evaluates subdivision surfaces (indicated by the presence
    /// of the 'subdiv' attribute in the file). By default, Assimp performs
    /// the subdivision using the standard Catmull-Clark algorithm. The default value is true.
    /// </summary>
    internal sealed class ACEvaluateSubdivisionConfig : BooleanPropertyConfig {

        /// <summary>
        /// Gets the string name used by ACEvaluateSubdivisionConfig.
        /// </summary>
        public static String ACEvaluateSubdivisionConfigName {
            get {
                return AiConfigs.AI_CONFIG_IMPORT_AC_EVAL_SUBDIVISION;
            }
        }

        /// <summary>
        /// Constructs a new ACEvaluateSubdivisionConfig.
        /// </summary>
        /// <param name="evaluateSubdivision">True if the AC loader should evaluate subdivisions, false otherwise.</param>
        public ACEvaluateSubdivisionConfig(bool evaluateSubdivision) 
            : base(ACEvaluateSubdivisionConfigName, evaluateSubdivision, true) { }
    }

    /// <summary>
    /// Configures the UNREAL 3D loader to separate faces with different surface flags (e.g. two-sided vs single-sided).
    /// The default value is true.
    /// </summary>
    internal sealed class UnrealHandleFlagsConfig : BooleanPropertyConfig {

        /// <summary>
        /// Gets the string name used by UnrealHandleFlagsConfig.
        /// </summary>
        public static String UnrealHandleFlagsConfigName {
            get {
                return AiConfigs.AI_CONFIG_IMPORT_UNREAL_HANDLE_FLAGS;
            }
        }

        /// <summary>
        /// Constructs a new UnrealHandleFlagsConfig.
        /// </summary>
        /// <param name="handleFlags">True if the unreal loader should separate faces with different surface flags, false otherwise.</param>
        public UnrealHandleFlagsConfig(bool handleFlags) 
            : base(UnrealHandleFlagsConfigName, handleFlags, true) { }
    }

    /// <summary>
    /// Configures the terragen import plugin to compute UV's for terrains, if
    /// they are not given. Furthermore, a default texture is assigned. The default value is false.
    /// <para>UV coordinates for terrains are so simple to compute that you'll usually 
    /// want to compute them on your own, if you need them. This option is intended for model viewers which
    /// want to offer an easy way to apply textures to terrains.</para>
    /// </summary>
    internal sealed class TerragenComputeTexCoordsConfig : BooleanPropertyConfig {

        /// <summary>
        /// Gets the string name used by TerragenComputeTexCoordsConfig.
        /// </summary>
        public static String TerragenComputeTexCoordsConfigName {
            get {
                return AiConfigs.AI_CONFIG_IMPORT_TER_MAKE_UVS;
            }
        }

        /// <summary>
        /// Constructs a new TerragenComputeTexCoordsConfig.
        /// </summary>
        /// <param name="computeTexCoords">True if terran UV coordinates should be computed, false otherwise.</param>
        public TerragenComputeTexCoordsConfig(bool computeTexCoords) 
            : base(TerragenComputeTexCoordsConfigName, computeTexCoords, false) { }
    }

    /// <summary>
    /// Configures the ASE loader to always reconstruct normal vectors basing on the smoothing groups
    /// loaded from the file. Some ASE files carry invalid normals, others don't. The default value is true.
    /// </summary>
    internal sealed class ASEReconstructNormalsConfig : BooleanPropertyConfig {

        /// <summary>
        /// Gets the string name used by ASEReconstructNormalsConfig.
        /// </summary>
        public static String ASEReconstructNormalsConfigName {
            get {
                return AiConfigs.AI_CONFIG_IMPORT_ASE_RECONSTRUCT_NORMALS;
            }
        }

        /// <summary>
        /// Constructs a new ASEReconstructNormalsConfig.
        /// </summary>
        /// <param name="reconstructNormals">True if normals should be re-computed, false otherwise.</param>
        public ASEReconstructNormalsConfig(bool reconstructNormals) 
            : base(ASEReconstructNormalsConfigName, reconstructNormals, true) { }
    }

    /// <summary>
    /// Configures the M3D loader to detect and process multi-part Quake player models. These models
    /// usually consit of three files, lower.md3, upper.md3 and head.md3. If this propery is
    /// set to true, Assimp will try to load and combine all three files if one of them is loaded. The
    /// default value is true.
    /// </summary>
    internal sealed class MD3HandleMultiPartConfig : BooleanPropertyConfig {

        /// <summary>
        /// Gets the string name used by MD3HandleMultiPartConfig.
        /// </summary>
        public static String MD3HandleMultiPartConfigName {
            get {
                return AiConfigs.AI_CONFIG_IMPORT_MD3_HANDLE_MULTIPART;
            }
        }

        /// <summary>
        /// Constructs a new MD3HandleMultiPartConfig.
        /// </summary>
        /// <param name="handleMultiParts">True if the split files should be loaded and combined, false otherwise.</param>
        public MD3HandleMultiPartConfig(bool handleMultiParts) 
            : base(MD3HandleMultiPartConfigName, handleMultiParts, true) { }
    }

    /// <summary>
    /// Tells the MD3 loader which skin files to load. When loading MD3 files, Assimp checks
    /// whether a file named "md3_file_name"_"skin_name".skin exists. These files are used by
    /// Quake III to be able to assign different skins (e.g. red and blue team) to models. 'default', 'red', 'blue'
    /// are typical skin names. The default string value is "default".
    /// </summary>
    internal sealed class MD3SkinNameConfig : StringPropertyConfig {

        /// <summary>
        /// Gets the string name used by MD3SkinNameConfig.
        /// </summary>
        public static String MD3SkinNameConfigName {
            get {
                return AiConfigs.AI_CONFIG_IMPORT_MD3_SKIN_NAME;
            }
        }

        /// <summary>
        /// Constructs a new MD3SkinNameConfig.
        /// </summary>
        /// <param name="skinName">The skin name.</param>
        public MD3SkinNameConfig(String skinName)
            : base(MD3SkinNameConfigName, skinName, "default") { }
    }

    /// <summary>
    /// Specifies the Quake 3 shader file to be used for a particular MD3 file. This can be a full path or
    /// relative to where all MD3 shaders reside. the default string value is an empty string.
    /// </summary>
    internal sealed class MD3ShaderSourceConfig : StringPropertyConfig {

        /// <summary>
        /// Gets the string name used by MD3ShaderSourceConfig.
        /// </summary>
        public static String MD3ShaderSourceConfigName {
            get {
                return AiConfigs.AI_CONFIG_IMPORT_MD3_SHADER_SRC;
            }
        }

        /// <summary>
        /// Constructs a new MD3ShaderSourceConfig.
        /// </summary>
        /// <param name="shaderFile">The shader file.</param>
        public MD3ShaderSourceConfig(String shaderFile)
            : base(MD3ShaderSourceConfigName, shaderFile, String.Empty) { }
    }

    /// <summary>
    /// Configures the LWO loader to load just one layer from the model.
    /// <para>LWO files consist of layers and in some cases it could be useful to load only one of them.
    /// This property can be either a string - which specifies the name of the layer - or an integer - the index
    /// of the layer. If the property is not set then the whole LWO model is loaded. Loading fails
    /// if the requested layer is not vailable. The layer index is zero-based and the layer name may not be empty</para>
    /// The default value is false (all layers are loaded).
    /// </summary>
    internal sealed class LWOImportOneLayerConfig : BooleanPropertyConfig {

        /// <summary>
        /// Gets the string name used by LWOImportOneLayerConfig.
        /// </summary>
        public static String LWOImportOneLayerConfigName {
            get {
                return AiConfigs.AI_CONFIG_IMPORT_LWO_ONE_LAYER_ONLY;
            }
        }

        /// <summary>
        /// Constructs a new LWOImportOneLayerConfig.
        /// </summary>
        /// <param name="importOneLayerOnly">True if only one layer should be imported, false if all layers should be imported.</param>
        public LWOImportOneLayerConfig(bool importOneLayerOnly) 
            : base(LWOImportOneLayerConfigName, importOneLayerOnly, false) { }
    }

    /// <summary>
    /// Configures the MD5 loader to not load the MD5ANIM file for a MD5MESH file automatically.
    /// The default value is false.
    /// <para>The default strategy is to look for a file with the same name but with the MD5ANIm extension
    /// in the same directory. If it is found it is loaded and combined with the MD5MESH file. This configuration
    /// option can be used to disable this behavior.</para>
    /// </summary>
    internal sealed class MD5NoAnimationAutoLoadConfig : BooleanPropertyConfig {

        /// <summary>
        /// Gets the string name used by MD5NoAnimationAutoLoadConfig.
        /// </summary>
        public static String MD5NoAnimationAutoLoadConfigName {
            get {
                return AiConfigs.AI_CONFIG_IMPORT_MD5_NO_ANIM_AUTOLOAD;
            }
        }

        /// <summary>
        /// Constructs a new MD5NoAnimationAutoLoadConfig.
        /// </summary>
        /// <param name="noAutoLoadAnim">True if animations should not be automatically loaded, false if they should be.</param>
        public MD5NoAnimationAutoLoadConfig(bool noAutoLoadAnim) 
            : base(MD5NoAnimationAutoLoadConfigName, noAutoLoadAnim, false) { }
    }

    /// <summary>
    /// Defines the beginning of the time range for which the LWS loader evaluates animations and computes
    /// AiNodeAnim's. The default value is the one taken from the file.
    /// <para>Assimp provides full conversion of Lightwave's envelope system, including pre and post
    /// conditions. The loader computes linearly subsampled animation channels with the frame rate
    /// given in the LWS file. This property defines the start time.</para>
    /// <para>Animation channels are only generated if a node has at least one envelope with more than one key
    /// assigned. This property is given in frames where '0' is the first. By default,
    /// if this property is not set, the importer takes the animation start from the input LWS
    /// file ('FirstFrame' line)</para>
    /// </summary>
    internal sealed class LWSAnimationStartConfig : IntegerPropertyConfig {

        /// <summary>
        /// Gets the string name used by LWSAnimationStartConfig.
        /// </summary>
        public static String LWSAnimationStartConfigName {
            get {
                return AiConfigs.AI_CONFIG_IMPORT_LWS_ANIM_START;
            }
        }

        /// <summary>
        /// Constructs a new LWSAnimationStartConfig.
        /// </summary>
        /// <param name="animStart">Beginning of the time range</param>
        public LWSAnimationStartConfig(int animStart)
            : base(LWSAnimationStartConfigName, animStart, -1) { } //TODO: Verify the default value to tell the loader to use the value from the file
    }

    /// <summary>
    /// Defines the ending of the time range for which the LWS loader evaluates animations and computes
    /// AiNodeAnim's. The default value is the one taken from the file
    /// <para>Assimp provides full conversion of Lightwave's envelope system, including pre and post
    /// conditions. The loader computes linearly subsampled animation channels with the frame rate
    /// given in the LWS file. This property defines the end time.</para>
    /// <para>Animation channels are only generated if a node has at least one envelope with more than one key
    /// assigned. This property is given in frames where '0' is the first. By default,
    /// if this property is not set, the importer takes the animation end from the input LWS
    /// file.</para>
    /// </summary>
    internal sealed class LWSAnimationEndConfig : IntegerPropertyConfig {

        /// <summary>
        /// Gets the string name used by LWSAnimationEndConfig.
        /// </summary>
        public static String LWSAnimationEndConfigName {
            get {
                return AiConfigs.AI_CONFIG_IMPORT_LWS_ANIM_END;
            }
        }

        /// <summary>
        /// Constructs a new LWSAnimationEndConfig.
        /// </summary>
        /// <param name="animEnd">Ending of the time range</param>
        public LWSAnimationEndConfig(int animEnd)
            : base(LWSAnimationEndConfigName, animEnd, -1) { } //TODO: Verify the default value to tell the loader to use the value from the file.
    }

    /// <summary>
    /// Defines the output frame rate of the IRR loader.
    /// <para>IRR animations are difficult to convert for Assimp and there will always be
    /// a loss of quality. This setting defines how many keys per second are returned by the converter.</para>
    /// The default value is 100 frames per second.
    /// </summary>
    internal sealed class IRRAnimationFrameRateConfig : IntegerPropertyConfig {

        /// <summary>
        /// Gets the string name used by IRRAnimationFrameRateConfig.
        /// </summary>
        public static String IRRAnimationFrameRateConfigName {
            get {
                return AiConfigs.AI_CONFIG_IMPORT_IRR_ANIM_FPS;
            }
        }

        /// <summary>
        /// Constructs a new IRRAnimationFramerateConfig.
        /// </summary>
        /// <param name="frameRate">Number of frames per second to output.</param>
        public IRRAnimationFrameRateConfig(int frameRate) 
            : base(IRRAnimationFrameRateConfigName, frameRate, 100) { }
    }

    /// <summary>
    /// The Ogre importer will try to load this MaterialFile. If a material file does not
    /// exist with the same name as a material to load, the ogre importer will try to load this file
    /// and searches for the material in it. The default string value is an empty string.
    /// </summary>
    internal sealed class OgreMaterialFileConfig : StringPropertyConfig {

        /// <summary>
        /// Gets the string name used by OgreMaterialFileConfig.
        /// </summary>
        public static String OgreMaterialFileConfigName {
            get {
                return AiConfigs.AI_CONFIG_IMPORT_OGRE_MATERIAL_FILE;
            }
        }

        /// <summary>
        /// Constructs a new OgreMaterialFileConfig.
        /// </summary>
        /// <param name="materialFileName">Material file name to load.</param>
        public OgreMaterialFileConfig(String materialFileName)
            : base(OgreMaterialFileConfigName, materialFileName, String.Empty) { }
    }

    /// <summary>
    /// The Ogre importer will detect the texture usage from the filename. Normally a texture is loaded as a color map, if no target is specified
    /// in the material file. If this is enabled, texture names ending with _n, _l, _s are used as normal maps, light maps, or specular maps.
    /// </summary>
    internal sealed class OgreTextureTypeFromFilenameConfig : BooleanPropertyConfig {

        /// <summary>
        /// Gets the string name used by OgreTextureTypeFromFilenameConfig.
        /// </summary>
        public static String OgreTextureTypeFromFilenameConfigName {
            get {
                return AiConfigs.AI_CONFIG_IMPORT_OGRE_TEXTURETYPE_FROM_FILENAME;
            }
        }

        /// <summary>
        /// Constructs a new OgreTextureTypeFromFilenameConfig.
        /// </summary>
        /// <param name="fileNameDefinesTextureUsage">True if the filename defines texture usage, false otherwise.</param>
        public OgreTextureTypeFromFilenameConfig(bool fileNameDefinesTextureUsage)
            : base(OgreTextureTypeFromFilenameConfigName, fileNameDefinesTextureUsage, true) { }
    }

    /// <summary>
    /// Specifies whether the IFC loader skips over shape representations of type 'Curve2D'. A lot of files contain both a faceted mesh representation and a outline 
    /// with a presentation type of 'Curve2D'. Currently Assimp does not convert those, so turning this option off just clutters the log with errors.
    /// </summary>
    internal sealed class IFCSkipCurveShapesConfig : BooleanPropertyConfig {

        /// <summary>
        /// Gets the string name used by IFCSkipCurveShapesConfig.
        /// </summary>
        public static String IFCSkipCurveShapesConfigName {
            get {
                return AiConfigs.AI_CONFIG_IMPORT_IFC_SKIP_CURVE_REPRESENTATIONS;
            }
        }

        /// <summary>
        /// Constructs a new IFCSkipCurveShapesConfig.
        /// </summary>
        /// <param name="skipCurveShapes">True if the Curve2D shapes are skipped during import, false otherwise.</param>
        public IFCSkipCurveShapesConfig(bool skipCurveShapes)
            : base(IFCSkipCurveShapesConfigName, skipCurveShapes, true) { }
    }

    /// <summary>
    /// Specifies whether the IFC loader will use its own, custom triangulation algorithm to triangulate wall and floor meshes. If this is set to false,
    /// walls will be either triangulated by the post process triangulation or will be passed through as huge polygons with faked holes (e.g. holes that are connected
    /// with the outer boundary using a dummy edge). It is highly recommended to leave this property set to true as the default post process has some known
    /// issues with these kind of polygons.
    /// </summary>
    internal sealed class IFCUseCustomTriangulationConfig : BooleanPropertyConfig {

        /// <summary>
        /// Gets the string name used by IFCUseCustomTriangulationConfig.
        /// </summary>
        public static String IFCUseCustomTriangulationConfigName {
            get {
                return AiConfigs.AI_CONFIG_IMPORT_IFC_CUSTOM_TRIANGULATION;
            }
        }

        /// <summary>
        /// Constructs a new IFCUseCustomTriangulationConfig..
        /// </summary>
        /// <param name="useCustomTriangulation">True if the loader should use its own triangulation routine for walls/floors, false otherwise.</param>
        public IFCUseCustomTriangulationConfig(bool useCustomTriangulation)
            : base(IFCUseCustomTriangulationConfigName, useCustomTriangulation, true) { }
    }

    #endregion
}
