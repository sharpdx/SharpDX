namespace SharpDX.Tests
{
    using System;
    using D3DCompiler;
    using Direct3D;
    using NUnit.Framework;

    [TestFixture]
    public class ShaderBytecodeProfileTests
    {
        [TestCase("ps_4_0_level_9_1")]
        [TestCase("ps_4_0_level_9_3")]
        [TestCase("ps_4_0")]
        [TestCase("ps_4_1")]
        [TestCase("ps_5_0")]
        public void WillReturnCorrectProfileVersionForPixelShader(string profile)
        {
            const string pixelShaderSourceCode = @"float4 ps() : SV_Target { return float4(0,0,0,0); }";

            var bytecode = ShaderBytecode.Compile(pixelShaderSourceCode, "ps", profile).Bytecode;

            var profileStruct = bytecode.GetVersion();

            Assert.AreEqual(profile, profileStruct.ToString());
        }

        [TestCase("vs_4_0_level_9_1")]
        [TestCase("vs_4_0_level_9_3")]
        [TestCase("vs_4_0")]
        [TestCase("vs_4_1")]
        [TestCase("vs_5_0")]
        public void WillReturnCorrectProfileVersionForVertexShader(string profile)
        {
            const string vertexShaderSourceCode = @"float4 vs() : SV_Position { return float4(0,0,0,0); }";

            var bytecode = ShaderBytecode.Compile(vertexShaderSourceCode, "vs", profile).Bytecode;

            var profileStruct = bytecode.GetVersion();

            Assert.AreEqual(profile, profileStruct.ToString());
        }

        [TestCase(5, 0, 0, 0, FeatureLevel.Level_11_0)]
        [TestCase(4, 1, 0, 0, FeatureLevel.Level_10_1)]
        [TestCase(4, 0, 0, 0, FeatureLevel.Level_10_0)]
        [TestCase(4, 0, 9, 3, FeatureLevel.Level_9_3)]
        [TestCase(4, 0, 9, 1, FeatureLevel.Level_9_1)]
        [TestCase(4, 0, 9, 2, FeatureLevel.Level_9_1, ExpectedException = typeof(InvalidOperationException))]
        public void WillReturnCorrectFeatureLevelFromProfile(int major, int minor, int profileMajor, int profileMinor, FeatureLevel expected)
        {
            var profile = new ShaderProfile(ShaderVersion.VertexShader, major, minor, profileMajor, profileMinor);

            var featureLevel = profile.GetFeatureLevel();

            Assert.AreEqual(expected, featureLevel);
        }
    }
}