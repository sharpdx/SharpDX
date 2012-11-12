REM Generate bytecode. This script is a temporary solution before having an integrated plugin inside VS2012
..\..\..\tkfxc\bin\Net20Debug\tkfxc.exe /nodis /DSM4 /FcAlphaTestEffect.ByteCode.cs /OCAlphaTestEffect AlphaTestEffect.fx
..\..\..\tkfxc\bin\Net20Debug\tkfxc.exe /nodis /DSM4 /FcBasicEffect.ByteCode.cs /OCBasicEffect BasicEffect.fx
..\..\..\tkfxc\bin\Net20Debug\tkfxc.exe /nodis /DSM4 /FcDualTextureEffect.ByteCode.cs /OCDualTextureEffect DualTextureEffect.fx
..\..\..\tkfxc\bin\Net20Debug\tkfxc.exe /nodis /DSM4 /FcEnvironmentMapEffect.ByteCode.cs /OCEnvironmentMapEffect EnvironmentMapEffect.fx
..\..\..\tkfxc\bin\Net20Debug\tkfxc.exe /nodis /DSM4 /FcSkinnedEffect.ByteCode.cs /OCSkinnedEffect SkinnedEffect.fx
..\..\..\tkfxc\bin\Net20Debug\tkfxc.exe /nodis /DSM4 /FcSpriteBatch.ByteCode.cs /OCSpriteBatch SpriteEffect.fx
..\..\..\tkfxc\bin\Net20Debug\tkfxc.exe /nodis /DSM4 /FcPrimitiveQuad.cs /OCPrimitiveQuad PrimitiveQuad.fx
