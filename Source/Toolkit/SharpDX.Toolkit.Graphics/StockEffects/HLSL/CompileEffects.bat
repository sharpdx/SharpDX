REM Generate bytecode. This script is a temporary solution before having an integrated plugin inside VS2012
..\..\..\..\..\Bin\Standard-Net40\tkfxc.exe /Ti /To..\..\obj\Net40Debug /nodis /DSM4 /FoAlphaTestEffect.tkfxo /FcAlphaTestEffect.ByteCode.cs /OCAlphaTestEffect AlphaTestEffect.fx
..\..\..\..\..\Bin\Standard-Net40\tkfxc.exe /Ti /To..\..\obj\Net40Debug /nodis /DSM4 /FoBasicEffect.tkfxo /FcBasicEffect.ByteCode.cs /OCBasicEffect BasicEffect.fx
..\..\..\..\..\Bin\Standard-Net40\tkfxc.exe /Ti /To..\..\obj\Net40Debug /nodis /DSM4 /FoDualTextureEffect.tkfxo /FcDualTextureEffect.ByteCode.cs /OCDualTextureEffect DualTextureEffect.fx
..\..\..\..\..\Bin\Standard-Net40\tkfxc.exe /Ti /To..\..\obj\Net40Debug /nodis /DSM4 /FoEnvironmentMapEffect.tkfxo /FcEnvironmentMapEffect.ByteCode.cs /OCEnvironmentMapEffect EnvironmentMapEffect.fx
..\..\..\..\..\Bin\Standard-Net40\tkfxc.exe /Ti /To..\..\obj\Net40Debug /nodis /DSM4 /FoSkinnedEffect.tkfxo /FcSkinnedEffect.ByteCode.cs /OCSkinnedEffect SkinnedEffect.fx
..\..\..\..\..\Bin\Standard-Net40\tkfxc.exe /Ti /To..\..\obj\Net40Debug /nodis /DSM4 /FoSpriteBatch.tkfxo /FcSpriteBatch.ByteCode.cs /OCSpriteBatch SpriteEffect.fx
..\..\..\..\..\Bin\Standard-Net40\tkfxc.exe /Ti /To..\..\obj\Net40Debug /nodis /DSM4 /FoPrimitiveQuad.tkfxo /FcPrimitiveQuad.cs /OCPrimitiveQuad PrimitiveQuad.fx
