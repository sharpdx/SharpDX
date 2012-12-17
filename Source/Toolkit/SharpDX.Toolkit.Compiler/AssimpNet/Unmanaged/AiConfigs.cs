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

namespace Assimp.Unmanaged {
    /// <summary>
    /// Defines configurable properties for importing models. All properties
    /// have default values. Setting config properties are done via the SetProperty*
    /// methods in AssimpMethods.
    /// </summary>
    internal static class AiConfigs {
        #region Library Settings - General/Global settings
        
        /// <summary>
        /// Enables time measurements. If enabled the time needed for each
        /// part of the loading process is timed and logged.
        /// <para>Type: bool. Default: false</para>
        /// </summary>
        public const String AI_CONFIG_GLOB_MEASURE_TIME = "GLOB_MEASURE_TIME";

        /// <summary>
        /// Sets Assimp's multithreading policy. This is ignored if Assimp is
        /// built without boost.thread support. Possible values are: -1 to
        /// let Assimp decide, 0 to disable multithreading, and nay number larger than 0
        /// to force a specific number of threads. This is only a hint and may be 
        /// ignored by Assimp.
        /// <para>Type: integer. Default: -1</para>
        /// </summary>
        public const String AI_CONFIG_GLOB_MULTITHREADING = "GLOB_MULTITHREADING";
        
        #endregion

        #region Post Processing Settings

        /// <summary>
        /// Specifies the maximum angle that may be between two vertex tangents that their tangents
        /// and bitangents are smoothed during the step to calculate the tangent basis. The angle specified 
        /// is in degrees. The maximum value is 175 degrees.
        /// <para>Type: float. Default: 45 degrees</para>
        /// </summary>
        public const String AI_CONFIG_PP_CT_MAX_SMOOTHING_ANGLE = "PP_CT_MAX_SMOOTHING_ANGLE";

        /// <summary>
        /// Specifies the maximum angle that may be between two face normals at the same vertex position that
        /// their normals will be smoothed together during the calculate smooth normals step. This is commonly
        /// called the "crease angle". The angle is specified in degrees. Maximum value is 175 degrees (all vertices
        /// smoothed).
        /// <para>Type: float. Default: 175 degrees</para>
        /// </summary>
        public const String AI_CONFIG_PP_GSN_MAX_SMOOTHING_ANGLE = "PP_GSN_MAX_SMOOTHING_ANGLE";

        /// <summary>
        /// Sets the colormap(= palette) to be used to decode embedded textures in MDL (Quake or 3DG5) files.
        /// This must be a valid path to a file. The file is 768 (256 * 3) bytes large and contains
        /// RGB triplets for each of the 256 palette entries. If the file is not found, a default
        /// palette (from Quake 1) is used.
        /// <para>Type: string. Default: "colormap.lmp"</para>
        /// </summary>
        public const String AI_CONFIG_IMPORT_MDL_COLORMAP = "IMPORT_MDL_COLORMAP";

        /// <summary>
        /// Configures the <see cref="PostProcessSteps.RemoveRedundantMaterials"/> step to
        /// keep materials matching a name in a given list. This is a list of
        /// 1 to n strings where whitespace ' ' serves as a delimiter character. Identifiers
        /// containing whitespaces must be enclosed in *single* quotation marks. Tabs or
        /// carriage returns are treated as whitespace.
        /// <para>If a material matches one of these names, it will not be modified
        /// or removed by the post processing step nor will other materials be replaced
        /// by a reference to it.</para>
        /// <para>Default: string. Default: ""</para>
        /// </summary>
        public const String AI_CONFIG_PP_RRM_EXCLUDE_LIST = "PP_RRM_EXCLUDE_LIST";

        /// <summary>
        /// Configures the <see cref="PostProcessSteps.PreTransformVertices"/> step
        /// to keep the scene hierarchy. Meshes are moved to worldspace, but no optimization
        /// is performed where meshes with the same materials are not joined.
        /// <para>This option could be of used if you have a scene hierarchy that contains
        /// important additional information which you intend to parse.</para>
        /// <para>Type: bool. Default: false</para>
        /// </summary>
        public const String AI_CONFIG_PP_PTV_KEEP_HIERARCHY = "PP_PTV_KEEP_HIERARCHY";

        /// <summary>
        /// Configures the <see cref="PostProcessSteps.PreTransformVertices"/> step
        /// to normalize all vertex components into the -1...1 range. That is, a bounding
        /// box for the whole scene is computed where the maximum component is taken
        /// and all meshes are scaled uniformly. This is useful if you don't know the spatial dimension
        /// of the input data.
        /// <para>Type: bool. Default: false</para>
        /// </summary>
        public const String AI_CONFIG_PP_PTV_NORMALIZE = "PP_PTV_NORMALIZE";

        /// <summary>
        /// Configures the <see cref="PostProcessSteps.FindDegenerates"/> step
        /// to remove degenerated primitives from the import immediately.
        /// <para>The default behavior converts degenerated triangles to lines and
        /// degenerated lines to points.</para>
        /// <para>Type: bool. Default: false</para>
        /// </summary>
        public const String AI_CONFIG_PP_FD_REMOVE = "PP_FD_REMOVE";

        /// <summary>
        /// Configures the <see cref="PostProcessSteps.OptimizeGraph"/> step
        /// to preserve nodes matching a name in a given list. This is a list of 1 to n strings, whitespace ' ' serves as a delimter character.
        /// Identifiers containing whitespaces must be enclosed in *single* quotation marks. Carriage returns
        /// and tabs are treated as white space.
        /// <para>If a node matches one of these names, it will not be modified or removed by the
        /// postprocessing step.</para>
        /// <para>Type: string. Default: ""</para>
        /// </summary>
        public const String AI_CONFIG_PP_OG_EXCLUDE_LIST = "PP_OG_EXCLUDE_LIST";

        /// <summary>
        /// Sets the maximum number of triangles a mesh can contain. This is used by the
        /// <see cref="PostProcessSteps.SplitLargeMeshes"/> step to determine
        /// whether a mesh must be split or not.
        /// <para>Type: int. Default: AiDefines.AI_SLM_DEFAULT_MAX_TRIANGLES</para>
        /// </summary>
        public const String AI_CONFIG_PP_SLM_TRIANGLE_LIMIT = "PP_SLM_TRIANGLE_LIMIT";

        /// <summary>
        /// Sets the maximum number of vertices in a mesh. This is used by the
        /// <see cref="PostProcessSteps.SplitLargeMeshes"/> step to determine
        /// whether a mesh must be split or not.
        /// <para>Type: integer. Default: AiDefines.AI_SLM_DEFAULT_MAX_VERTICES</para>
        /// </summary>
        public const String AI_CONFIG_PP_SLM_VERTEX_LIMIT = "PP_SLM_VERTEX_LIMIT";

        /// <summary>
        /// Sets the maximum number of bones that can affect a single vertex. This is used
        /// by the <see cref="PostProcessSteps.LimitBoneWeights"/> step.
        /// <para>Type: integer. Default: AiDefines.AI_LBW_MAX_WEIGHTS</para>
        /// </summary>
        public const String AI_CONFIG_PP_LBW_MAX_WEIGHTS = "PP_LBW_MAX_WEIGHTS";

        /// <summary>
        /// Sets the size of the post-transform vertex cache to optimize vertices for. This is
        /// for the <see cref="PostProcessSteps.ImproveCacheLocality"/> step. The size
        /// is given in vertices. Of course you can't know how the vertex format will exactly look
        /// like after the import returns, but you can still guess what your meshes will
        /// probably have. The default value *has* resulted in slight performance improvements
        /// for most Nvidia/AMD cards since 2002.
        /// <para>Type: integer. Default: AiDefines.PP_ICL_PTCACHE_SIZE</para>
        /// </summary>
        public const String AI_CONFIG_PP_ICL_PTCACHE_SIZE = "PP_ICL_PTCACHE_SIZE";

        /// <summary>
        /// Input parameter to the <see cref="PostProcessSteps.RemoveComponent"/> step. 
        /// It specifies the parts of the data structure to be removed.
        /// <para>This is a bitwise combination of the <see cref="ExcludeComponent"/> flag. If no valid mesh is remaining after
        /// the step is executed, the import FAILS.</para>
        /// <para>Type: integer. Default: 0</para>
        /// </summary>
        public const String AI_CONFIG_PP_RVC_FLAGS = "PP_RVC_FLAGS";

        /// <summary>
        /// Input parameter to the <see cref="PostProcessSteps.SortByPrimitiveType"/> step.
        /// It specifies which primitive types are to be removed by the step.
        /// <para>This is a bitwise combination of the <see cref="PrimitiveType"/> flag.
        /// Specifying ALL types is illegal.</para>
        /// <para>Type: integer. Default: 0</para>
        /// </summary>
        public const String AI_CONFIG_PP_SBP_REMOVE = "PP_SBP_REMOVE";

        /// <summary>
        /// Input parameter to the <see cref="PostProcessSteps.FindInvalidData"/> step.
        /// It specifies the floating point accuracy for animation values, specifically the epislon
        /// during the comparison. The step checks for animation tracks where all frame values are absolutely equal 
        /// and removes them. Two floats are considered equal if the invariant <c>abs(n0-n1) > epislon</c> holds
        /// true for all vector/quaternion components.
        /// <para>Type: float. Default: 0.0f (comparisons are exact)</para>
        /// </summary>
        public const String AI_CONFIG_PP_FID_ANIM_ACCURACY = "PP_FID_ANIM_ACCURACY";

        /// <summary>
        /// Input parameter to the <see cref="PostProcessSteps.TransformUVCoords"/> step.
        /// It specifies which UV transformations are to be evaluated.
        /// <para>This is bitwise combination of the <see cref="UVTransformFlags"/> flag.</para>
        /// <para>Type: integer. Default: AiDefines.AI_UV_TRAFO_ALL (All combinations)</para>
        /// </summary>
        public const String AI_CONFIG_PP_TUV_EVALUATE = "PP_TUV_EVALUATE";

        /// <summary>
        /// A hint to Assimp to favour speed against import quality. Enabling this option
        /// may result in faster loading, or it may not. It is just a hint to loaders and post-processing
        /// steps to use faster code paths if possible. A value not equal to zero stands
        /// for true.
        /// <para>Type: integer. Default: 0</para>
        /// </summary>
        public const String AI_CONFIG_FAVOUR_SPEED = "FAVOUR_SPEED";

        /// <summary>
        /// Maximum bone cone per mesh for the <see cref="PostProcessSteps.SplitByBoneCount"/> step. Meshes
        /// are split until the max number of bones is reached.
        /// <para>Type: integer. Default: 60</para>
        /// </summary>
        public const String AI_CONFIG_PP_SBBC_MAX_BONES = "PP_SBBC_MAX_BONES";

        /// <summary>
        /// Source UV channel for tangent space computation. The specified channel must exist or an error will be raised.
        /// <para>Type: integer. Default: 0</para>
        /// </summary>
        public const String AI_CONFIG_PP_CT_TEXTURE_CHANNEL_INDEX = "AI_CONFIG_PP_CT_TEXTURE_CHANNEL_INDEX";

        /// <summary>
        /// Threshold used to determine if a bone is kept or removed during the <see cref="PostProcessSteps.Debone"/> step.
        /// <para>Type: float. Default: 1.0f</para>
        /// </summary>
        public const String AI_CONFIG_PP_DB_THRESHOLD = "PP_DB_THRESHOLD";

        /// <summary>
        /// Require all bones to qualify for deboning before any are removed.
        /// <para>Type: bool. Default: false</para>
        /// </summary>
        public const String AI_CONFIG_PP_DB_ALL_OR_NONE = "PP_DB_ALL_OR_NONE";

        #endregion

        #region Importer Settings

        /// <summary>
        /// Sets the vertex animation keyframe to be imported. Assimp does not support
        /// vertex keyframes (only bone animation is supported). The libary reads only one frame of models
        /// with vertex animations. By default this is the first frame.
        /// <para>The default value is 0. This option applies to all importers. However, it is
        /// also possible to override the global setting for a specific loader. You can use the
        /// AI_CONFIG_IMPORT_XXX_KEYFRAME options where XXX is a placeholder for the file format which
        /// you want to override the global setting.</para>
        /// <para>Type: integer. Default: 0</para>
        /// </summary>
        public const String AI_CONFIG_IMPORT_GLOBAL_KEYFRAME = "IMPORT_GLOBAL_KEYFRAME";

        /// <summary>
        /// See the documentation for <see cref="AiConfigs.AI_CONFIG_IMPORT_GLOBAL_KEYFRAME"/>.
        /// </summary>
        public const String AI_CONFIG_IMPORT_MD3_KEYFRAME = "IMPORT_MD3_KEYFRAME";

        /// <summary>
        /// See the documentation for <see cref="AiConfigs.AI_CONFIG_IMPORT_GLOBAL_KEYFRAME"/>.
        /// </summary>
        public const String AI_CONFIG_IMPORT_MD2_KEYFRAME = "IMPORT_MD3_KEYFRAME";

        /// <summary>
        /// See the documentation for <see cref="AiConfigs.AI_CONFIG_IMPORT_GLOBAL_KEYFRAME"/>.
        /// </summary>
        public const String AI_CONFIG_IMPORT_MDL_KEYFRAME = "IMPORT_MDL_KEYFRAME";

        /// <summary>
        /// See the documentation for <see cref="AiConfigs.AI_CONFIG_IMPORT_GLOBAL_KEYFRAME"/>.
        /// </summary>
        public const String AI_CONFIG_IMPORT_MDC_KEYFRAME = "IMPORT_MDC_KEYFRAME";

        /// <summary>
        /// See the documentation for <see cref="AiConfigs.AI_CONFIG_IMPORT_GLOBAL_KEYFRAME"/>.
        /// </summary>
        public const String AI_CONFIG_IMPORT_SMD_KEYFRAME = "IMPORT_SMD_KEYFRAME";

        /// <summary>
        /// See the documentation for <see cref="AiConfigs.AI_CONFIG_IMPORT_GLOBAL_KEYFRAME"/>.
        /// </summary>
        public const String AI_CONFIG_IMPORT_UNREAL_KEYFRAME = "IMPORT_UNREAL_KEYFRAME";

        /// <summary>
        /// Configures the AC loader to collect all surfaces which have the "Backface cull" flag set in separate
        /// meshes.
        /// <para>Type: bool. Default: true</para>
        /// </summary>
        public const String AI_CONFIG_IMPORT_AC_SEPARATE_BFCULL = "IMPORT_AC_SEPARATE_BFCULL";

        /// <summary>
        /// Configures whether the AC loader evaluates subdivision surfaces (indicated by the presence
        /// of the 'subdiv' attribute in the file). By default, Assimp performs
        /// the subdivision using the standard Catmull-Clark algorithm.
        /// <para>Type: bool. Default: true</para>
        /// </summary>
        public const String AI_CONFIG_IMPORT_AC_EVAL_SUBDIVISION = "IMPORT_AC_EVAL_SUBDIVISION";

        /// <summary>
        /// Configures the UNREAL 3D loader to separate faces with different surface flags (e.g. two-sided vs single-sided).
        /// <para>Type: bool. Default: true</para>
        /// </summary>
        public const String AI_CONFIG_IMPORT_UNREAL_HANDLE_FLAGS = "UNREAL_HANDLE_FLAGS";

        /// <summary>
        /// Configures the terragen import plugin to compute UV's for terrains, if
        /// they are not given. Furthermore, a default texture is assigned.
        /// <para>UV coordinates for terrains are so simple to compute that you'll usually 
        /// want to compute them on your own, if you need them. This option is intended for model viewers which
        /// want to offer an easy way to apply textures to terrains.</para>
        /// <para>Type: bool. Default: false</para>
        /// </summary>
        public const String AI_CONFIG_IMPORT_TER_MAKE_UVS = "IMPORT_TER_MAKE_UVS";

        /// <summary>
        /// Configures the ASE loader to always reconstruct normal vectors basing on the smoothing groups
        /// loaded from the file. Some ASE files carry invalid normals, others don't.
        /// <para>Type: bool. Default: true</para>
        /// </summary>
        public const String AI_CONFIG_IMPORT_ASE_RECONSTRUCT_NORMALS = "IMPORT_ASE_RECONSTRUCT_NORMALS";

        /// <summary>
        /// Configures the M3D loader to detect and process multi-part Quake player models. These models
        /// usually consit of three files, lower.md3, upper.md3 and head.md3. If this propery is
        /// set to true, Assimp will try to load and combine all three files if one of them is loaded.
        /// <para>Type: bool. Default: true</para>
        /// </summary>
        public const String AI_CONFIG_IMPORT_MD3_HANDLE_MULTIPART = "IMPORT_MD3_HANDLE_MULTIPART";

        /// <summary>
        /// Tells the MD3 loader which skin files to load. When loading MD3 files, Assimp checks
        /// whether a file named "md3_file_name"_"skin_name".skin exists. These files are used by
        /// Quake III to be able to assign different skins (e.g. red and blue team) to models. 'default', 'red', 'blue'
        /// are typical skin names.
        /// <para>Type: string. Default: "default"</para>
        /// </summary>
        public const String AI_CONFIG_IMPORT_MD3_SKIN_NAME = "IMPORT_MD3_SKIN_NAME";

        /// <summary>
        /// Specifies the Quake 3 shader file to be used for a particular MD3 file. This can be a full path or
        /// relative to where all MD3 shaders reside.
        /// <para>Type: string. Default: ""</para>
        /// </summary>
        public const String AI_CONFIG_IMPORT_MD3_SHADER_SRC = "IMPORT_MD3_SHADER_SRC";

        /// <summary>
        /// Configures the LWO loader to load just one layer from the model.
        /// <para>LWO files consist of layers and in some cases it could be useful to load only one of them.
        /// This property can be either a string - which specifies the name of the layer - or an integer - the index
        /// of the layer. If the property is not set then the whole LWO model is loaded. Loading fails
        /// if the requested layer is not vailable. The layer index is zero-based and the layer name may not be empty</para>
        /// <para>Type: bool. Default: false (All layers are loaded)</para>
        /// </summary>
        public const String AI_CONFIG_IMPORT_LWO_ONE_LAYER_ONLY = "IMPORT_LWO_ONE_LAYER_ONLY";

        /// <summary>
        /// Configures the MD5 loader to not load the MD5ANIM file for a MD5MESH file automatically.
        /// <para>The default strategy is to look for a file with the same name but with the MD5ANIm extension
        /// in the same directory. If it is found it is loaded and combined with the MD5MESH file. This configuration
        /// option can be used to disable this behavior.</para>
        /// <para>Type: bool. Default: false</para>
        /// </summary>
        public const String AI_CONFIG_IMPORT_MD5_NO_ANIM_AUTOLOAD = "IMPORT_MD5_NO_ANIM_AUTOLOAD";

        /// <summary>
        /// Defines the beginning of the time range for which the LWS loader evaluates animations and computes
        /// AiNodeAnim's.
        /// <para>Assimp provides full conversion of Lightwave's envelope system, including pre and post
        /// conditions. The loader computes linearly subsampled animation channels with the frame rate
        /// given in the LWS file. This property defines the start time.</para>
        /// <para>Animation channels are only generated if a node has at least one envelope with more than one key
        /// assigned. This property is given in frames where '0' is the first. By default,
        /// if this property is not set, the importer takes the animation start from the input LWS
        /// file ('FirstFrame' line)</para>
        /// <para>Type: integer. Default: taken from file</para>
        /// </summary>
        public const String AI_CONFIG_IMPORT_LWS_ANIM_START = "IMPORT_LWS_ANIM_START";

        /// <summary>
        /// Defines the ending of the time range for which the LWS loader evaluates animations and computes
        /// AiNodeAnim's.
        /// <para>Assimp provides full conversion of Lightwave's envelope system, including pre and post
        /// conditions. The loader computes linearly subsampled animation channels with the frame rate
        /// given in the LWS file. This property defines the end time.</para>
        /// <para>Animation channels are only generated if a node has at least one envelope with more than one key
        /// assigned. This property is given in frames where '0' is the first. By default,
        /// if this property is not set, the importer takes the animation end from the input LWS
        /// file.</para>
        /// <para>Type: integer. Default: taken from file</para>
        /// </summary>
        public const String AI_CONFIG_IMPORT_LWS_ANIM_END = "IMPORT_LWS_ANIM_END";

        /// <summary>
        /// Defines the output frame rate of the IRR loader.
        /// <para>IRR animations are difficult to convert for Assimp and there will always be
        /// a loss of quality. This setting defines how many keys per second are returned by the converter.</para>
        /// <para>Type: integer. Default: 100</para>
        /// </summary>
        public const String AI_CONFIG_IMPORT_IRR_ANIM_FPS = "IMPORT_IRR_ANIM_FPS";

        /// <summary>
        /// The Ogre importer will try to load this MaterialFile. If a material file does not
        /// exist with the same name as a material to load, the ogre importer will try to load this file
        /// and searches for the material in it.
        /// <para>Type: string. Default: ""</para>
        /// </summary>
        public const String AI_CONFIG_IMPORT_OGRE_MATERIAL_FILE = "IMPORT_OGRE_MATERIAL_FILE";

        /// <summary>
        /// The Ogre importer will detect the texture usage from the filename. Normally a texture is loaded as a color map, if no target is specified
        /// in the material file. If this is enabled, texture names ending with _n, _l, _s are used as normal maps, light maps, or specular maps.
        /// <para>Type: Bool. Default: true.</para>
        /// </summary>
        public const String AI_CONFIG_IMPORT_OGRE_TEXTURETYPE_FROM_FILENAME = "IMPORT_OGRE_TEXTURETYPE_FROM_FILENAME";

        /// <summary>
        /// Specifies whether the IFC loader skips over shape representations of type 'Curve2D'. A lot of files contain both a faceted mesh representation and a outline 
        /// with a presentation type of 'Curve2D'. Currently Assimp does not convert those, so turning this option off just clutters the log with errors.
        /// <para>Type: Bool. Default: true.</para>
        /// </summary>
        public const String AI_CONFIG_IMPORT_IFC_SKIP_CURVE_REPRESENTATIONS = "IMPORT_IFC_SKIP_CURVE_REPRESENTATIONS";

        /// <summary>
        /// Specifies whether the IFC loader will use its own, custom triangulation algorithm to triangulate wall and floor meshes. If this is set to false,
        /// walls will be either triangulated by the post process triangulation or will be passed through as huge polygons with faked holes (e.g. holes that are connected
        /// with the outer boundary using a dummy edge). It is highly recommended to leave this property set to true as the default post process has some known
        /// issues with these kind of polygons.
        /// <para>Type: Bool. Default: true.</para>
        /// </summary>
        public const String AI_CONFIG_IMPORT_IFC_CUSTOM_TRIANGULATION = "IMPORT_IFC_CUSTOM_TRIANGULATION";
        
        #endregion
    }
}
