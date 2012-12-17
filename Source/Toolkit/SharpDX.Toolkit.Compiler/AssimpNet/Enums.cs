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

namespace Assimp {

    /// <summary>
    /// Post processing flag options, specifying a number of steps
    /// that can be run on the data to either generate additional vertex
    /// data or optimize the imported data.
    /// </summary>
    [Flags]
    internal enum PostProcessSteps {
        /// <summary>
        /// No flags enabled.
        /// </summary>
        None = 0x0,

        /// <summary>
        /// Calculates the tangents and binormals (bitangents)
        /// for the imported meshes.
        /// <para>
        /// This does nothing if a mesh does not have normals. You might
        /// want this post processing step to be executed if you plan
        /// to use tangent space calculations such as normal mapping. There is a
        /// config setting AI_CONFIG_PP_CT_MAX_SMOOTHING_ANGLE which
        /// allows you to specify a maximimum smoothing angle for the algorithm.
        /// However, usually you'll want to leave it at the default value.
        /// </para>
        /// </summary>
        CalculateTangentSpace = 0x1,

        /// <summary>
        /// Identifies and joins identical vertex data sets within all
        /// imported meshes.
        /// <para>
        /// After this step is run each mesh does contain only unique vertices
        /// anymore, so a vertex is possibly used by multiple faces. You usually
        /// want to use this post processing step. If your application deals with
        /// indexed geometry, this step is compulsory or you'll just waste rendering
        /// time.</para>
        /// <para>If this flag is not specified, no vertices are referenced by more than one
        /// face and no index buffer is required for rendering.</para>
        /// </summary>
        JoinIdenticalVertices = 0x2,

        /// <summary>
        /// Converts all imported data to a left handed coordinate space.
        /// 
        /// <para>By default the data is returned in a right-handed coordinate space,
        /// where +X points to the right, +Z towards the viewer, and +Y upwards.</para>
        /// </summary>
        MakeLeftHanded = 0x4,

        /// <summary>
        /// Triangulates all faces of all meshes.
        /// <para>
        /// By default the imported mesh data might contain faces with more than 
        /// three indices. For rendering you'll usually want all faces to
        /// be triangles. This post processing step splits up all
        /// higher faces to triangles. Line and point primitives are *not*
        /// modified. If you want 'triangles only' with no other kinds of primitives,
        /// try the following:
        /// </para>
        /// <list type="number">
        /// <item>
        /// <description>Specify both <see cref="PostProcessSteps.Triangulate"/> and <see cref="PostProcessSteps.SortByPrimitiveType"/>.</description>
        /// </item>
        /// <item>
        /// <description>Ignore all point and line meshes when you process Assimp's output</description>
        /// </item>
        /// </list>
        /// </summary>
        Triangulate = 0x8,

        /// <summary>
        /// Removes some parts of the data structure (animations, materials,
        /// light sources, cameras, textures, vertex components).
        /// <para>
        /// The components to be removed are specified in a separate configuration
        /// option, AI_CONFIG_PP_RVC_FLAGS. This is quite useful if you don't
        /// need all parts of the output structure. Especially vertex colors are rarely used today...calling this step to remove
        /// unrequired stuff from the pipeline as early as possible results in an increased
        /// performance and a better optimized output data structure.
        /// </para>
        /// <para>
        /// This step is also useful if you want to force Assimp to recompute normals
        /// or tangents. the corresponding steps don't recompute them if they're already
        /// there (loaded from the source asset). By using this step you can make sure
        /// they are NOT there.</para>
        /// </summary>
        RemoveComponent = 0x10,

        /// <summary>
        /// Generates normals for all faces of all meshes. It may not be
        /// specified together with <see cref="PostProcessSteps.GenerateSmoothNormals"/>.
        /// <para>
        /// This is ignored if normals are already there at the time where this
        /// flag is evaluated. Model importers try to load them from the source file,
        /// so they're usually already there. Face normals are shared between all
        /// points of a single face, so a single point can have multiple normals,
        /// which in other words, forces the library to duplicate vertices in
        /// some cases. This makes <see cref="PostProcessSteps.JoinIdenticalVertices"/> senseless then.
        /// </para>
        /// </summary>
        GenerateNormals = 0x20,

        /// <summary>
        /// Generates smooth normals for all vertices of all meshes. It
        /// may not be specified together with <see cref="PostProcessSteps.GenerateNormals"/>.
        /// <para>
        /// This is ignored if normals are already there at the time where
        /// this flag is evaluated. Model importers try to load them from the
        /// source file, so they're usually already there.
        /// </para>
        /// <para>The configuration option AI_CONFIG_PP_GSN_MAX_SMOOTHING_ANGLE
        /// allows you to specify an angle maximum for the normal smoothing algorithm.
        /// Normals exceeding this limit are not smoothed, resulting in a 'hard' seam
        /// between two faces. using a decent angle here (e.g. 80 degrees) results in a very good visual
        /// appearance.</para>
        /// </summary>
        GenerateSmoothNormals = 0x40,

        /// <summary>
        /// Splits large meshes into smaller submeshes.
        /// <para>
        /// This is useful for realtime rendering where the number
        /// of triangles which can be maximally processed in a single draw call is
        /// usually limited by the video driver/hardware. The maximum vertex buffer
        /// is usually limited, too. Both requirements can be met with this step:
        /// you may specify both a triangle and a vertex limit for a single mesh.
        /// </para>
        /// <para>The split limits can be set through the AI_CONFIG_PP_SLM_VERTEX_LIMIT
        /// and AI_CONFIG_PP_SLM_TRIANGLE_LIMIT config settings. The default
        /// values are 1,000,000.</para>
        /// 
        /// <para>Warning: This can be a time consuming task.</para>
        /// </summary>
        SplitLargeMeshes = 0x80,

        /// <summary>
        /// Removes the node graph and "bakes" (pre-transforms) all
        /// vertices with the local transformation matrices of their nodes.
        /// The output scene does still contain nodes, however, there is only
        /// a root node with children, each one referencing only one mesh. 
        /// Each mesh referencing one material. For rendering, you can simply render
        /// all meshes in order, you don't need to pay attention to local transformations
        /// and the node hierarchy.
        /// 
        /// <para>Warning: Animations are removed during this step.</para>
        /// </summary>
        PreTransformVertices = 0x100,

        /// <summary>
        /// Limits the number of bones simultaneously affecting a single
        /// vertex to a maximum value.
        /// <para>
        /// If any vertex is affected by more than that number of bones,
        /// the least important vertex weights are removed and the remaining vertex
        /// weights are re-normalized so that the weights still sum up to 1.
        /// </para>
        /// <para>The default bone weight limit is 4 and uses the
        /// AI_LMW_MAX_WEIGHTS config. If you intend to perform the skinning in hardware, this post processing
        /// step might be of interest for you.</para>
        /// </summary>
        LimitBoneWeights = 0x200,

        /// <summary>
        /// Validates the imported scene data structure.
        /// <para>
        /// This makes sure that all indices are valid, all animations
        /// and bones are linked correctly, all material references are
        /// correct, etc.
        /// </para>
        /// It is recommended to capture Assimp's log output if you use this flag,
        /// so you can easily find out what's actually wrong if a file fails the
        /// validation. The validator is quite rude and will find *all* inconsistencies
        /// in the data structure. There are two types of failures:
        /// <list type="bullet">
        /// <item>
        /// <description>Error: There's something wrong with the imported data. Further
        /// postprocessing is not possible and the data is not usable at all. The import
        /// fails.</description>
        /// </item>
        /// <item>
        /// <description>Warning: There are some minor issues (e.g. 1000000 animation keyframes
        /// with the same time), but further postprocessing and use of the data structure is still
        /// safe. Warning details are written to the log file.</description>
        /// </item>
        /// </list>
        /// </summary>
        ValidateDataStructure = 0x400,

        /// <summary>
        /// Re-orders triangles for better vertex cache locality.
        /// 
        /// <para>This step tries to improve the ACMR (average post-transform vertex cache
        /// miss ratio) for all meshes. The implementation runs in O(n) time 
        /// and is roughly based on the <a href="http://www.cs.princeton.edu/gfx/pubs/Sander_2007_%3ETR/tipsy.pdf">'tipsify' algorithm</a>.</para>
        /// 
        /// <para>If you intend to render huge models in hardware, this step might be of interest for you.
        /// The AI_CONFIG_PP_ICL_PTCACHE_SIZE config setting can be used to fine tune
        /// the cache optimization.</para>
        /// </summary>
        ImproveCacheLocality = 0x800,

        /// <summary>
        /// Searches for redundant/unreferenced materials and removes them.
        /// <para>
        /// This is especially useful in combination with the  PreTransformVertices
        /// and OptimizeMeshes flags. Both join small meshes with equal characteristics, but
        /// they can't do their work if two meshes have different materials. Because several
        /// material settings are always lost during Assimp's import filders and because many
        /// exporters don't check for redundant materials, huge models often have materials which
        /// are defined several times with exactly the same settings.
        /// </para>
        /// <para>Several material settings not contributing to the final appearance of a surface
        /// are ignored in all comparisons ... the material name is one of them. So, if you're passing
        /// additional information through the content pipeline (probably using *magic* material names),
        /// don't specify this flag. Alternatively, take a look at the AI_CONFIG_PP_RRM_EXCLUDE_LIST
        /// setting.</para>
        /// </summary>
        RemoveRedundantMaterials = 0x1000,

        /// <summary>
        /// This step tries to determine which meshes have normal vectors
        /// that are facing inwards. 
        /// <para>
        /// The algorithm is simple but effective:
        /// </para>
        /// <para>The bounding box of all vertices and their normals are compared
        /// against the volume of the bounding box of all vertices without their normals.
        /// This works well for most objects, problems might occur with planar surfaces. However,
        /// the step tries to filter such cases. The step inverts all in-facing normals.
        /// Generally, it is recommended to enable this step, although the result is not
        /// always correct.</para>
        /// </summary>
        FixInFacingNormals = 0x2000,

        /// <summary>
        /// This step splits meshes with more than one primitive type in homogeneous submeshes.
        /// <para>
        /// This step is executed after triangulation and after it returns, just one
        /// bit is set in aiMesh:mPrimitiveTypes. This is especially useful for real-time
        /// rendering where point and line primitives are often ignored or rendered separately.
        /// </para>
        /// <para>
        /// You can use AI_CONFIG_PP_SBP_REMOVE option to specify which primitive types you need.
        /// This can be used to easily exclude lines and points, which are rarely used,
        /// from the import.
        /// </para>
        /// </summary>
        SortByPrimitiveType = 0x8000,

        /// <summary>
        /// This step searches all meshes for degenerated primitives and
        /// converts them to proper lines or points. A face is 'degenerated' if one or more of its points are identical.
        /// <para>
        /// To have degenerated primitives removed, specify the <see cref="PostProcessSteps.FindDegenerates"/> flag
        /// try one of the following procedures:
        /// </para>
        /// <list type="numbers">
        /// <item>
        /// <description>To support lines and points: Set the
        /// AI_CONFIG_PP_FD_REMOVE option to one. This will cause the step to remove degenerated triangles as
        /// soon as they are detected. They won't pass any further pipeline steps.</description>
        /// </item>
        /// <item>
        /// <description>If you don't support lines and points: Specify <see cref="PostProcessSteps.SortByPrimitiveType"/> flag, which
        /// will move line and point primitives to separate meshes.  Then set the AI_CONFIG_PP_SBP_REMOVE
        /// option to <see cref="PrimitiveType.Point"/> and <see cref="PrimitiveType.Line"/> to cause <see cref="PostProcessSteps.SortByPrimitiveType"/> step
        /// to reject point and line meshes from the scene.</description>
        /// </item>
        /// </list>
        /// <para>
        /// Degenerated polygons are not necessarily evil and that's why they are not removed by default. There are several
        /// file formats which do not support lines or points where exporters bypass the format specification and write
        /// them as degenerated triangles instead.
        /// </para>
        /// </summary>
        FindDegenerates = 0x10000,

        /// <summary>
        /// This step searches all meshes for invalid data, such as zeroed
        /// normal vectors or invalid UV coordinates and removes or fixes them.
        /// This is intended to get rid of some common exporter rrors.
        /// <para>
        /// This is especially useful for normals. If they are invalid,
        /// and the step recognizes this, they will be removed and can later
        /// be recomputed, e.g. by the GenerateSmoothNormals flag. The step
        /// will also remove meshes that are infinitely small and reduce animation
        /// tracks consisting of hundreds of redundant keys to a single key. The
        /// AI_CONFIG_PP_FID_ANIM_ACCURACY config property decides the accuracy of the check
        /// for duplicate animation tracks.</para>
        /// </summary>
        FindInvalidData = 0x20000,

        /// <summary>
        /// This step converts non-UV mappings (such as spherical or
        /// cylindrical mapping) to proper texture coordinate channels.
        /// 
        /// <para>Most applications will support UV mapping only, so you will
        /// probably want to specify this step in every case. Note that Assimp
        /// is not always able to match the original mapping implementation of the 3D
        /// app which produced a model perfectly. It's always better
        /// to let the father app compute the UV channels, at least 3DS max, maya, blender,
        /// lightwave, modo, .... are able to achieve this.</para>
        /// 
        /// <para>If this step is not requested, you'll need to process the MATKEY_MAPPING
        /// material property in order to display all assets properly.</para>
        /// </summary>
        GenerateUVCoords = 0x40000,

        /// <summary>
        /// Applies per-texture UV transformations and bakes them to stand-alone vtexture
        /// coordinate channels.
        /// 
        /// <para>UV Transformations are specified per-texture - see the MATKEY_UVTRANSFORM material
        /// key for more information. This step processes all textures with transformed input UV coordinates
        /// and generates new (pretransformed) UV channel transformations, so you will probably
        /// want to specify this step.</para>
        /// 
        /// <para>UV transformations are usually implemented in realtime apps by
        /// transforming texture coordinates in a vertex shader stage with a 3x3 (homogenous)
        /// transformation matrix.</para>
        /// </summary>
        TransformUVCoords = 0x80000,

        /// <summary>
        /// Searches for duplicated meshes and replaces them with a reference
        /// to the first mesh.
        /// <para>
        /// This is time consuming, so don't use it if you have no time. Its
        /// main purpose is to work around the limitation with some
        /// file formats that don't support instanced meshes, so exporters
        /// duplicate meshes.
        /// </para>
        /// </summary>
        FindInstances = 0x100000,

        /// <summary>
        /// Attempts to reduce the number of meshes (and draw calls). 
        /// <para>
        /// This is recommended to be used together with <see cref="PostProcessSteps.OptimizeGraph"/>
        /// and is fully compatible with both <see cref="PostProcessSteps.SplitLargeMeshes"/> and <see cref="PostProcessSteps.SortByPrimitiveType"/>.
        /// </para>
        /// </summary>
        OptimizeMeshes = 0x200000,

        /// <summary>
        /// Optimizes scene hierarchy. Nodes with no animations, bones,
        /// lights, or cameras assigned are collapsed and joined.
        /// 
        /// <para>Node names can be lost during this step, you can specify
        /// names of nodes that should'nt be touched or modified
        /// with AI_CONFIG_PP_OG_EXCLUDE_LIST.</para>
        /// 
        /// <para>Use this flag with caution. Most simple files will be collapsed to a 
        /// single node, complex hierarchies are usually completely lost. That's not
        /// the right choice for editor environments, but probably a very effective
        /// optimization if you just want to get the model data, convert it to your
        /// own format and render it as fast as possible. </para>
        /// 
        /// <para>This flag is designed to be used with <see cref="PostProcessSteps.OptimizeMeshes"/> for best
        /// results.</para>
        /// 
        /// <para>Scenes with thousands of extremely small meshes packed
        /// in deeply nested nodes exist for almost all file formats.
        /// Usage of this and <see cref="PostProcessSteps.OptimizeMeshes"/> usually fixes them all and
        /// makes them renderable.</para>
        /// </summary>
        OptimizeGraph = 0x400000,

        /// <summary>
        /// Flips all UV coordinates along the y-axis
        /// and adjusts material settings/bitangents accordingly.
        /// </summary>
        FlipUVs = 0x800000,

        /// <summary>
        /// Flips face winding order from CCW (default) to CW.
        /// </summary>
        FlipWindingOrder = 0x1000000,

        /// <summary>
        /// Splits meshes with many bones into submeshes so that each submesh has fewer or as many bones as a given limit.
        /// </summary>
        SplitByBoneCount = 0x2000000,

        /// <summary>
        /// <para>Removes bones losslessly or according to some threshold. In some cases (e.g. formats that require it) exporters
        /// are faced to assign dummy bone weights to otherwise static meshes assigned to animated meshes. Full, weight-based skinning is expensive while
        /// animating nodes is extremely cheap, so this step is offered to clean up the data in that regard. 
        /// </para>
        /// <para>Usage of the configuration AI_CONFIG_PP_DB_THRESHOLD to control the threshold and AI_CONFIG_PP_DB_ALL_OR_NONE if you want bones
        /// removed if and only if all bones within the scene qualify for removal.</para>
        /// </summary>
        Debone = 0x4000000
    }

    /// <summary>
    /// Enumerates components of the scene or mesh data that
    /// can be excluded from the import using the post process step
    /// RemoveComponent.
    /// </summary>
    [Flags]
    internal enum ExcludeComponent {
        /// <summary>
        /// No components to be excluded.
        /// </summary>
        None = 0x0,

        /// <summary>
        /// Removes normal vectors
        /// </summary>
        Normals = 0x2,

        /// <summary>
        /// Removes tangents/binormals
        /// </summary>
        TangentBasis = 0x4,

        /// <summary>
        /// Removes all color sets.
        /// </summary>
        Colors = 0x8,

        /// <summary>
        /// Removes all texture UV sets.
        /// </summary>
        TexCoords = 0x10,

        /// <summary>
        /// Remove all boneweights from all meshes. Scenegraph
        /// nodes corresponding to the bones are NOT removed.
        /// Use OptimizeGraph step to remove them.
        /// </summary>
        Boneweights = 0x20,

        /// <summary>
        /// Removes all node animations.  Coressponding scenegraph
        /// nodes are NOT removed. Use OptimizeGraph step to 
        /// remove them.
        /// </summary>
        Animations = 0x40,

        /// <summary>
        /// Removes all embedded textures.
        /// </summary>
        Textures = 0x80,

        /// <summary>
        /// Removes all light sources. The corresponding scenegraph nodes are
        /// NOT removed. Use the OptimizeGraph step to do this.
        /// </summary>
        Lights = 0x100,

        /// <summary>
        /// Removes all cameras. The corresponding scenegraph
        /// nodes are NOT removed. Use the OptimizeGraph step
        /// to do this.
        /// </summary>
        Cameras = 0x200,

        /// <summary>
        /// Removes all meshes.
        /// </summary>
        Meshes = 0x400,

        /// <summary>
        /// Removes all materials. One default material will be generated.
        /// </summary>
        Materials = 0x800
    }

    /// <summary>
    /// Enumerates geometric primitive types.
    /// </summary>
    [Flags]
    internal enum PrimitiveType : uint {
        /// <summary>
        /// Point primitive. This is just a single vertex
        /// in the virtual world. A face has one index for such a primitive.
        /// </summary>
        Point = 0x1,

        /// <summary>
        /// Line primitive. This is  a line defined through a start and an
        /// end position. A face contains exactly two indices for such a primitive.
        /// </summary>
        Line = 0x2,

        /// <summary>
        /// Triangle primitive, consisting of three indices.
        /// </summary>
        Triangle = 0x4,

        /// <summary>
        /// A n-Gon that has more than three edges (thus is not a triangle).
        /// </summary>
        Polygon = 0x8
    }

    /// <summary>
    /// Defines an animation channel behaves outside the defined
    /// time range. This corresponds to the prestate and poststates
    /// of the animation node.
    /// </summary>
    internal enum AnimationBehaviour {
        /// <summary>
        /// The value from the default node transformation is taken.
        /// </summary>
        Default = 0x0,

        /// <summary>
        /// The nearest key value is used without interpolation.
        /// </summary>
        Constant = 0x1,

        /// <summary>
        /// The value of the nearest two keys is linearly extrapolated for the current
        /// time value.
        /// </summary>
        Linear = 0x2,

        /// <summary>
        /// The animation is repeated. If the animation key goes from n to m
        /// and the current time is t, use the value at (t - n ) % (|m-n|).
        /// </summary>
        Repeat = 0x3
    }

    /// <summary>
    /// Enumerates all supported light sources.
    /// </summary>
    internal enum LightSourceType {
        /// <summary>
        /// Unknown light.
        /// </summary>
        Undefined = 0x0,

        /// <summary>
        /// Directional light source that has a well-defined
        /// direction but is infinitely far away, e.g. the sun.
        /// </summary>
        Directional = 0x1,

        /// <summary>
        /// Point light source that has a well-defined position in
        /// space but is omni-directional, e.g. a light bulb.
        /// </summary>
        Point = 0x2,

        /// <summary>
        /// Spot light source emits light from a position in space,
        /// in a certain direction that is limited by an angle, like
        /// a cone.
        /// </summary>
        Spot = 0x3
    }

    /// <summary>
    /// Defines alpha blending flags, how the final
    /// color value of a pixel is computed, based on the following equation:
    /// <para>
    /// sourceColor * sourceBlend + destColor * destBlend
    /// </para>
    /// <para>
    /// Where the destColor is the previous color in the frame buffer
    /// and sourceColor is the material color before the
    /// transparency calculation. This corresponds to the AI_MATKEY_BLEND_FUNC property.</para>
    /// </summary>
    internal enum BlendMode {
        /// <summary>
        /// Default blending: sourceColor * sourceAlpha + destColor * (1 - sourceAlpha)
        /// </summary>
        Default = 0x0,

        /// <summary>
        /// Additive blending: sourcecolor * 1 + destColor * 1.
        /// </summary>
        Additive = 0x1,
    }

    /// <summary>
    /// Defines all shading models supported by the library.
    /// <para>
    /// The list of shading modes has been taken from Blender. See Blender
    /// documentation for more information.
    /// </para>
    /// </summary>
    internal enum ShadingMode {
        /// <summary>
        /// No shading mode defined.
        /// </summary>
        None = 0x0,

        /// <summary>
        /// Flat shading. Shading is done on a per-face basis and is diffuse only. Also known
        /// as 'faceted shading'.
        /// </summary>
        Flat = 0x1,
        
        /// <summary>
        /// Simple Gouraud shading.
        /// </summary>
        Gouraud = 0x2,

        /// <summary>
        /// Phong Shading.
        /// </summary>
        Phong = 0x3,

        /// <summary>
        /// Phong-Blinn Shading.
        /// </summary>
        Blinn = 0x4,

        /// <summary>
        /// Toon-shading, also known as a 'comic' shader.
        /// </summary>
        Toon = 0x5,

        /// <summary>
        /// OrenNayer shading model. Extension to standard Lambertian shading, taking the roughness
        /// of the material into account.
        /// </summary>
        OrenNayar = 0x6,

        /// <summary>
        /// Minnaert shading model. Extension to standard Lambertian shading, taking the "darkness" of
        /// the material into account.
        /// </summary>
        Minnaert = 0x7,

        /// <summary>
        /// CookTorrance shading model. Special shader for metallic surfaces.
        /// </summary>
        CookTorrance = 0x8,

        /// <summary>
        /// No shading at all. Constant light influence of 1.0.
        /// </summary>
        NoShading = 0x9,

        /// <summary>
        /// Fresnel shading.
        /// </summary>
        Fresnel = 0xa
    }

    /// <summary>
    /// Defines some mixed flags for a particular texture. This corresponds
    /// to the AI_MAT_KEY_TEXFLAGS property.
    /// </summary>
    [Flags]
    internal enum TextureFlags {
        /// <summary>
        /// The texture's color values have to be inverted (componentwise 1-n).
        /// </summary>
        Invert = 0x1,

        /// <summary>
        /// Explicit request to the application to process the alpha channel of the texture. This is mutually
        /// exclusive with <see cref="TextureFlags.IgnoreAlpha"/>. These flags are
        /// set if the library can say for sure that the alpha channel is used/is not used.
        /// If the model format does not define this, iti s left to the application to decide
        /// whether the texture alpha channel - if any - is evaluated or not.
        /// </summary>
        UseAlpha = 0x2,

        /// <summary>
        /// Explicit request to the application to ignore the alpha channel of the texture. This is mutually
        /// exclusive with <see cref="TextureFlags.UseAlpha"/>.
        /// </summary>
        IgnoreAlpha = 0x4
    }

    /// <summary>
    /// Defines how UV coordinates outside the [0..1] range are handled. Commonly
    /// referred to as the 'wrapping mode'
    /// </summary>
    internal enum TextureWrapMode {
        /// <summary>
        /// A texture coordinate u|v is translated to u % 1| v % 1.
        /// </summary>
        Wrap = 0x0,

        /// <summary>
        /// Texture coordinates outside [0...1] are clamped to the nearest valid value.
        /// </summary>
        Clamp = 0x1,

        /// <summary>
        /// A texture coordinate u|v becomes u1|v1 if (u - (u % 1)) % 2 is zero
        /// and 1 - (u % 1) | 1 - (v % 1) otherwise.
        /// </summary>
        Mirror = 0x2,

        /// <summary>
        /// If the texture coordinates for a pixel are outside [0...1] the texture is not
        /// applied to that pixel.
        /// </summary>
        Decal = 0x3,
    }

    /// <summary>
    /// Defines how texture coordinates are generated
    /// <para>
    /// Real-time applications typically require full UV coordinates. So the use
    /// of <see cref="PostProcessSteps.GenerateUVCoords"/> step is highly recommended.
    /// It generates proper UV channels for non-UV mapped objects, as long as an accurate
    /// description of how the mapping should look like is given.
    /// </para>
    /// </summary>
    internal enum TextureMapping {
        /// <summary>
        /// Coordinates are taken from the an existing UV channel.
        /// <para>
        /// The AI_MATKEY_UVWSRC key specifies from the UV channel the texture coordinates
        /// are to be taken from since meshes can have more than one UV channel.
        /// </para>
        /// </summary>
        FromUV = 0x0,

        /// <summary>
        /// Spherical mapping
        /// </summary>
        Sphere = 0x1,

        /// <summary>
        /// Cylinder mapping
        /// </summary>
        Cylinder = 0x2,

        /// <summary>
        /// Cubic mapping
        /// </summary>
        Box = 0x3,

        /// <summary>
        /// Planar mapping
        /// </summary>
        Plane = 0x4,

        /// <summary>
        /// Unknown mapping that is not recognied.
        /// </summary>
        Unknown = 0x5
    }

    /// <summary>
    /// Defines how the Nth texture of a specific type is combined
    /// with the result of all previous layers.
    /// <para>
    /// Example (left: key, right: value):
    /// <code>
    /// DiffColor0     - gray
    /// DiffTextureOp0 - TextureOperation.Multiply
    /// DiffTexture0   - tex1.png
    /// DiffTextureOp0 - TextureOperation.Add
    /// DiffTexture1   - tex2.png
    /// </code>
    /// <para>
    /// Written as an equation, the final diffuse term for a specific
    /// pixel would be:
    /// </para>
    /// <code>
    /// diffFinal = DiffColor0 * sampleTex(DiffTexture0, UV0) + sampleTex(DiffTexture1, UV0) * diffContrib;
    /// </code>
    /// </para>
    /// </summary>
    internal enum TextureOperation {
        /// <summary>
        /// T = T1 * T2
        /// </summary>
        Multiply = 0x0,

        /// <summary>
        /// T = T1 + T2
        /// </summary>
        Add = 0x1,

        /// <summary>
        /// T = T1 - T2
        /// </summary>
        Subtract = 0x2,

        /// <summary>
        /// T = T1 / T2
        /// </summary>
        Divide = 0x3,

        /// <summary>
        /// T = (T1 + T2) - (T1 * T2)
        /// </summary>
        SmoothAdd = 0x4,

        /// <summary>
        /// T = T1 + (T2 - 0.5)
        /// </summary>
        SignedAdd = 0x5
    }

    /// <summary>
    /// Defines the purpose of a texture.
    /// </summary>
    internal enum TextureType : uint {
        /// <summary>
        /// No texture, but the value can be used as a 'texture semantic'.
        /// </summary>
        None = 0x0,

        /// <summary>
        /// A diffuse texture that is combined with the result of the diffuse lighting equation.
        /// </summary>
        Diffuse = 0x1,

        /// <summary>
        /// A specular texture that is combined with the result of the specular lighting equation.
        /// </summary>
        Specular = 0x2,

        /// <summary>
        /// An ambient texture that is combined with the ambient lighting equation.
        /// </summary>
        Ambient = 0x3,

        /// <summary>
        /// An emissive texture that is added to the result of the lighting calculation. It is not influenced
        /// by incoming light, instead it represents the light that the object is naturally emitting.
        /// </summary>
        Emissive = 0x4,

        /// <summary>
        /// A height map texture. by convention, higher gray-scale values stand for
        /// higher elevations from some base height.
        /// </summary>
        Height = 0x5,

        /// <summary>
        /// A tangent-space normal map. There are several conventions for normal maps
        /// and Assimp does (intentionally) not distinguish here.
        /// </summary>
        Normals = 0x6,

        /// <summary>
        /// A texture that defines the glossiness of the material. This is the exponent of the specular (phong)
        /// lighting equation. Usually there is a conversion function defined to map the linear color values
        /// in the texture to a suitable exponent.
        /// </summary>
        Shininess = 0x7,

        /// <summary>
        /// The texture defines per-pixel opacity. usually 'white' means opaque and 'black' means 'transparency. Or quite
        /// the opposite.
        /// </summary>
        Opacity = 0x8,

        /// <summary>
        /// A displacement texture. The exact purpose and format is application-dependent. Higher color values stand for higher vertex displacements.
        /// </summary>
        Displacement = 0x9,

        /// <summary>
        /// A lightmap texture (aka Ambient occlusion). Both 'lightmaps' and dedicated 'ambient occlusion maps' are covered by this material property. The
        /// texture contains a scaling value for the final color value of a pixel. Its intensity is not affected by incoming light.
        /// </summary>
        Lightmap = 0xA,

        /// <summary>
        /// A reflection texture. Contains the color of a perfect mirror reflection. This is rarely used, almost never for real-time applications.
        /// </summary>
        Reflection = 0xB,

        /// <summary>
        /// An unknown texture that does not mention any of the defined texture type definitions. It is still imported, but is excluded from any
        /// further postprocessing.
        /// </summary>
        Unknown = 0xC
    }

    /// <summary>
    /// Defines the state of the imported scene data structure.
    /// </summary>
    [Flags]
    internal enum SceneFlags : uint {
        /// <summary>
        /// Default state of the scene, it imported successfully.
        /// </summary>
        None = 0x0,

        /// <summary>
        /// Specifies that the scene data structure that was imported is not complete.
        /// This flag bypasses some internal validations and allows the import
        /// of animation skeletons, material libaries, or camera animation paths
        /// using Assimp. Most applications won't support such data.
        /// </summary>
        Incomplete = 0x1,

        /// <summary>
        /// This flag is set by the <see cref="PostProcessSteps.ValidateDataStructure"/>
        /// post process step if validation is successful. In a validated scene you can be sure that any
        /// cross references in the data structure (e.g. vertex indices) are valid.
        /// </summary>
        Validated = 0x2,

        /// <summary>
        /// This flag is set by the <see cref="PostProcessSteps.ValidateDataStructure"/>
        /// post process step if validation is successful, but some issues have been found. This can for example
        /// mean that a texture that does not exist is referenced by a material or that the bone weights for a vertex
        /// do not sum to 1.0. In most cases you should still be able to use the import. This flag can be useful
        /// for applications which do not capture Assimp's log output.
        /// </summary>
        ValidationWarning = 0x4,

        /// <summary>
        /// This flag is set by the <see cref="PostProcessSteps.JoinIdenticalVertices"/> post process step.
        /// It indicates that the vertices of the output mesh are not in the internal verbose format anymore. In the
        /// verbose format, all vertices are unique where no vertex is ever referenced by more than one face.
        /// </summary>
        NonVerboseFormat = 0x8,

        /// <summary>
        /// Denotes the scene is pure height-map terrain data. Pure terrains usually consist of quads, sometimes triangles,
        /// in a regular grid. The x,y coordinates of all vertex positions refer to the x,y coordinates on the terrain height map, the
        /// z-axis stores the elevation at a specific point.
        /// <para>
        /// TER (Terragen) and HMP (3D Game Studio) are height map formats.
        /// </para>
        /// </summary>
        Terrain = 0x10
    }

    /// <summary>
    /// Enumerates Assimp function result codes.
    /// </summary>
    internal enum ReturnCode {
        /// <summary>
        /// Function returned successfully.
        /// </summary>
        Success = 0x0,

        /// <summary>
        /// There was an error.
        /// </summary>
        Failure = -0x1,

        /// <summary>
        /// Assimp ran out of memory.
        /// </summary>
        OutOfMemory = -0x3,
    }

    /// <summary>
    /// Seek origins for Assimp's virtual file system API.
    /// </summary>
    internal enum Origin {
        /// <summary>
        /// Beginning of the file
        /// </summary>
        Set = 0x0,

        /// <summary>
        /// Current position of the file pointer.
        /// </summary>
        Current = 0x1,

        /// <summary>
        /// End of the file, offsets must be negative.
        /// </summary>
        End = 0x2
    }

    /// <summary>
    /// Enumerates predefined log streaming destinations.
    /// </summary>
    [Flags]
    internal enum DefaultLogStream {
        /// <summary>
        /// Stream log to a file
        /// </summary>
        File = 0x1,

        /// <summary>
        /// Stream log to the standard output
        /// </summary>
        StdOut = 0x2,

        /// <summary>
        /// Stream log to the standard error output.
        /// </summary>
        StdErr = 0x4,

        /// <summary>
        /// MSVC only: Stream the log to the debugger (this relies
        /// on OutputDebugString from the Win32 SDK).
        /// </summary>
        Debugger = 0x8
    }

    /// <summary>
    /// Defines material property types.
    /// </summary>
    internal enum PropertyType {
        /// <summary>
        /// Array of single-precision (32 bit) floats.
        /// </summary>
        Float = 0x1,

        /// <summary>
        /// Property is a string.
        /// </summary>
        String = 0x3,

        /// <summary>
        /// Array of 32 bit integers.
        /// </summary>
        Integer = 0x4,

        /// <summary>
        /// Byte buffer where the content is undefined.
        /// </summary>
        Buffer = 0x5
    }

    /// <summary>
    /// Enumerates how the native Assimp DLL was compiled
    /// </summary>
    internal enum CompileFlags : uint {
        /// <summary>
        /// Assimp compiled as a shared object (Windows: DLL);
        /// </summary>
        Shared = 0x1,

        /// <summary>
        /// Assimp was compiled against STLport
        /// </summary>
        STLport = 0x2,

        /// <summary>
        /// Assimp was compiled as a debug build
        /// </summary>
        Debug = 0x4,

        /// <summary>
        /// Assimp was compiled with the boost work around.
        /// </summary>
        NoBoost = 0x8,

        /// <summary>
        /// Assimp was compiled built to run single threaded.
        /// </summary>
        SingleThreaded = 0x10
    }

    /// <summary>
    /// Defines how UV coordinates should be transformed.
    /// </summary>
    [Flags]
    internal enum UVTransformFlags {
        /// <summary>
        /// Scaling is evaluated.
        /// </summary>
        Scaling = 0x1,

        /// <summary>
        /// Rotation is evaluated.
        /// </summary>
        Rotation = 0x2,

        /// <summary>
        /// Translation is evaluated.
        /// </summary>
        Translation = 0x4
    }
}
