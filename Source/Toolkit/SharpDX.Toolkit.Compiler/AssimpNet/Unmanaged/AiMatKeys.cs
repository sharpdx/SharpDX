using System;

namespace Assimp.Unmanaged {
    /// <summary>
    /// Static class containing material key constants. A fully qualified mat key
    /// name here means that it's a string that combines the mat key (base) name, its
    /// texture type semantic, and its texture index into a single string delimited by
    /// commas. For non-texture material properties, the texture type semantic and texture
    /// index are always zero.
    /// </summary>
    internal static class AiMatKeys {

        /// <summary>
        /// Material name (String)
        /// </summary>
        public const String NAME = "?mat.name,0,0";
        
        /// <summary>
        /// Two sided property (boolean)
        /// </summary>
        public const String TWOSIDED = "$mat.twosided,0,0";

        /// <summary>
        /// Shading mode property (ShadingMode)
        /// </summary>
        public const String SHADING_MODEL = "$mat.shadingm,0,0";

        /// <summary>
        /// Enable wireframe property (boolean)
        /// </summary>
        public const String ENABLE_WIREFRAME = "$mat.wireframe,0,0";

        /// <summary>
        /// Blending function (BlendMode)
        /// </summary>
        public const String BLEND_FUNC = "$mat.blend,0,0";

        /// <summary>
        /// Opacity (float)
        /// </summary>
        public const String OPACITY = "$mat.opacity,0,0";

        /// <summary>
        /// Bumpscaling (float)
        /// </summary>
        public const String BUMPSCALING = "$mat.bumpscaling,0,0";

        /// <summary>
        /// Shininess (float)
        /// </summary>
        public const String SHININESS = "$mat.shininess,0,0";

        /// <summary>
        /// Reflectivity (float)
        /// </summary>
        public const String REFLECTIVITY = "$mat.reflectivity,0,0";

        /// <summary>
        /// Shininess strength (float)
        /// </summary>
        public const String SHININESS_STRENGTH = "$mat.shinpercent,0,0";

        /// <summary>
        /// Refracti (float)
        /// </summary>
        public const String REFRACTI = "$mat.refracti,0,0";

        /// <summary>
        /// Diffuse color (Color4D)
        /// </summary>
        public const String COLOR_DIFFUSE = "$clr.diffuse,0,0";

        /// <summary>
        /// Ambient color (Color4D)
        /// </summary>
        public const String COLOR_AMBIENT = "$clr.ambient,0,0";

        /// <summary>
        /// Specular color (Color4D)
        /// </summary>
        public const String COLOR_SPECULAR = "$clr.specular,0,0";

        /// <summary>
        /// Emissive color (Color4D)
        /// </summary>
        public const String COLOR_EMISSIVE = "$clr.emissive,0,0";

        /// <summary>
        /// Transparent color (Color4D)
        /// </summary>
        public const String COLOR_TRANSPARENT = "$clr.transparent,0,0";

        /// <summary>
        /// Reflective color (Color4D)
        /// </summary>
        public const String COLOR_REFLECTIVE = "$clr.reflective,0,0";

        /// <summary>
        /// Background image (String)
        /// </summary>
        public const String GLOBAL_BACKGROUND_IMAGE = "?bg.global,0,0";

        /// <summary>
        /// Texture base name
        /// </summary>
        public const String TEXTURE_BASE = "$tex.file";

        /// <summary>
        /// UVWSRC base name
        /// </summary>
        public const String UVWSRC_BASE = "$tex.uvwsrc";

        /// <summary>
        /// Texture op base name
        /// </summary>
        public const String TEXOP_BASE = "$tex.op";

        /// <summary>
        /// Mapping base name
        /// </summary>
        public const String MAPPING_BASE = "$tex.mapping";

        /// <summary>
        /// Texture blend base name.
        /// </summary>
        public const String TEXBLEND_BASE = "$tex.blend";

        /// <summary>
        /// Mapping mode U base name
        /// </summary>
        public const String MAPPINGMODE_U_BASE = "$tex.mapmodeu";

        /// <summary>
        /// Mapping mode V base name
        /// </summary>
        public const String MAPPINGMODE_V_BASE = "$tex.mapmodev";

        /// <summary>
        /// Texture map axis base name
        /// </summary>
        public const String TEXMAP_AXIS_BASE = "$tex.mapaxis";

        /// <summary>
        /// UV transform base name
        /// </summary>
        public const String UVTRANSFORM_BASE = "$tex.uvtrafo";

        /// <summary>
        /// Texture flags base name
        /// </summary>
        public const String TEXFLAGS_BASE = "$tex.flags";

        /// <summary>
        /// Helper function to get the fully qualified name of a texture property type name. Takes
        /// in a base name constant, a texture type, and a texture index and outputs the name in the format:
        /// <para>"baseName.TextureType.texIndex"</para>
        /// </summary>
        /// <param name="baseName">Base name</param>
        /// <param name="texType">Texture type</param>
        /// <param name="texIndex">Texture index</param>
        /// <returns>Fully qualified texture name</returns>
        public static String GetFullTextureName(String baseName, TextureType texType, int texIndex) {
            return String.Format("{0},{1},{2}", baseName, (int) texType, texIndex);
        }
    }
}
