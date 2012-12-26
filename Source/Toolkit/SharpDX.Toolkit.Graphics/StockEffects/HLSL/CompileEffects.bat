REM Generate bytecode. This script is a temporary solution before having an integrated plugin inside VS2012
..\..\..\tkfxc\bin\Net20Debug\tkfxc.exe /Ti /nodis /DSM4 /FoAlphaTestEffect.tkfxo /FcAlphaTestEffect.ByteCode.cs /OCAlphaTestEffect AlphaTestEffect.fx
..\..\..\tkfxc\bin\Net20Debug\tkfxc.exe /Ti /nodis /DSM4 /FoBasicEffect.tkfxo /FcBasicEffect.ByteCode.cs /OCBasicEffect BasicEffect.fx
..\..\..\tkfxc\bin\Net20Debug\tkfxc.exe /Ti /nodis /DSM4 /FoDualTextureEffect.tkfxo /FcDualTextureEffect.ByteCode.cs /OCDualTextureEffect DualTextureEffect.fx
..\..\..\tkfxc\bin\Net20Debug\tkfxc.exe /Ti /nodis /DSM4 /FoEnvironmentMapEffect.tkfxo /FcEnvironmentMapEffect.ByteCode.cs /OCEnvironmentMapEffect EnvironmentMapEffect.fx
..\..\..\tkfxc\bin\Net20Debug\tkfxc.exe /Ti /nodis /DSM4 /FoSkinnedEffect.tkfxo /FcSkinnedEffect.ByteCode.cs /OCSkinnedEffect SkinnedEffect.fx
..\..\..\tkfxc\bin\Net20Debug\tkfxc.exe /Ti /nodis /DSM4 /FoSpriteBatch.tkfxo /FcSpriteBatch.ByteCode.cs /OCSpriteBatch SpriteEffect.fx
..\..\..\tkfxc\bin\Net20Debug\tkfxc.exe /Ti /nodis /DSM4 /FoPrimitiveQuad.tkfxo /FcPrimitiveQuad.cs /OCPrimitiveQuad PrimitiveQuad.fx
