namespace SharpDX.D3DCompiler
{
    using System;
    using Direct3D;

    /// <summary>
    /// Represents a profile of a shader stage - stage type and version
    /// </summary>
    public struct ShaderProfile
    {
        private const string shortStringFormat = "{0}_{1}_{2}";
        private const string longStringFormat = "{0}_{1}_{2}_level_{3}_{4}";

        /// <summary>
        /// Gets the shader version - that is the shader stage type
        /// </summary>
        /// <remarks>For example, it is the "vs" part from "vs_5_0".</remarks>
        public readonly ShaderVersion Version;

        // REMARK: used int instad of uint to make this more CLS-Compliant

        /// <summary>
        /// Gets the major version of the shader profile.
        /// </summary>
        /// <remarks>For example, it is the "5" part from "vs_5_0".</remarks>
        public readonly int Major;

        /// <summary>
        /// Gets the minor version of the shader profile.
        /// </summary>
        /// <remarks>For example, it is the "0" part from "vs_5_0".</remarks>
        public readonly int Minor;

        /// <summary>
        /// Gets the major version of the shader sub-profile.
        /// </summary>
        /// <remarks>For example, it is the "9" part from "vs_4_0_profile_9_3".</remarks>
        public readonly int ProfileMajor;

        /// <summary>
        /// Gets the minor version of the shader sub-profile.
        /// </summary>
        /// <remarks>For example, it is the "3" part from "vs_4_0_profile_9_3".</remarks>
        public readonly int ProfileMinor;

        /// <summary>
        /// Creates a new instance of the <see cref="ShaderProfile"/> struct.
        /// </summary>
        /// <param name="version">The shader stage type.</param>
        /// <param name="major">The major version.</param>
        /// <param name="minor">The minor version.</param>
        /// <param name="profileMajor">The profile major version.</param>
        /// <param name="profileMinor">The profile minor version.</param>
        public ShaderProfile(ShaderVersion version, int major, int minor, int profileMajor, int profileMinor)
        {
            Version = version;
            Major = major;
            Minor = minor;
            ProfileMajor = profileMajor;
            ProfileMinor = profileMinor;
        }

        /// <summary>
        /// Returns the string representation of the current profile.
        /// </summary>
        /// <returns>The string representation, for example "ps_5_0" or "vs_4_0_profile_9_3".</returns>
        public override string ToString()
        {
            // get the textual representation of stage type
            var versionText = GetTypePrefix();

            // if we don't have a major profile - then it is the short version
            return ProfileMajor != 0
                       ? string.Format(longStringFormat, versionText, Major, Minor, ProfileMajor, ProfileMinor)
                       : string.Format(shortStringFormat, versionText, Major, Minor);
        }

        /// <summary>
        /// Gets the shader type prefix.
        /// </summary>
        /// <returns>Type prefix, for example "vs" or "ps".</returns>
        public string GetTypePrefix()
        {
            switch (Version)
            {
                case ShaderVersion.PixelShader:
                    return "ps";
                case ShaderVersion.VertexShader:
                    return "vs";
                case ShaderVersion.GeometryShader:
                    return "gs";
                case ShaderVersion.HullShader:
                    return "hs";
                case ShaderVersion.DomainShader:
                    return "ds";
                case ShaderVersion.ComputeShader:
                    return "cs";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Gets the corresponding <see cref="FeatureLevel"/> for this <see cref="ShaderProfile"/>.
        /// </summary>
        /// <returns>The minimum <see cref="FeatureLevel"/> that corresponds to current profile.</returns>
        public FeatureLevel GetFeatureLevel()
        {
            if (Major == 5 && Minor == 0)
                return FeatureLevel.Level_11_0;

            if (Major == 4 && Minor == 1)
                return FeatureLevel.Level_10_1;

            if (Major == 4 && Minor == 0)
            {
                if (ProfileMajor == 9 && ProfileMinor == 3) return FeatureLevel.Level_9_3;
                if (ProfileMajor == 9 && ProfileMinor == 1) return FeatureLevel.Level_9_1;
                if (ProfileMajor == 0 && ProfileMinor == 0) return FeatureLevel.Level_10_0;
            }

            throw new InvalidOperationException("Cannot convert profile to feature level.");
        }
    }
}