//using System;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using BepInEx;
//using ReinCore;
//using RoR2;
//using UnityEngine;

//namespace Rein.AlternateArtificer
//{
//    internal partial class Main
//    {
//        partial void Materials()
//        {
//            base.awake += this.Main_awake5;
//        }

//        private void Main_awake5()
//        {
//            var mat = new CloudMaterial("SwordGlow");
//            mat.sourceBlend = UnityEngine.Rendering.BlendMode.One;
//            mat.destinationBlend = UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha;
//            mat.internalSimpleBlendMode = 0f;
//            mat.tintColor = Color.white;
//            mat.disableRemapping = false;
//            mat.mainTexture.texture = AssetsCore.LoadAsset<Texture2D>( Texture2DIndex.refTexParticleDust1Mask );
//            mat.remapTexture.texture = swordRamp;
//            mat.softFactor = 1f;
//            mat.brightnessBoost = 4f;
//            mat.alphaBoost = 5.01f;
//            mat.alphaBias = 0f;
//            mat.useUV1 = false;
//            mat.fadeClose = false;
//            mat.fadeCloseDistance = 0.5f;
//            mat.cull = MaterialBase.CullMode.Off;
//            mat.cloudRemappingOn = false;
//            mat.cloudDistortionOn = false;
//            mat.distortionStrength = 0.1f;
//            mat.cloudTexture1.texture = null;
//            mat.cloudTexture2.texture = null;
//            mat.cutoffScrollSpeed = Vector4.zero;
//            mat.vertexColorOn = true;
//            mat.vertexAlphaOn = false;
//            mat.luminanceForTextureAlpha = false;
//            mat.vertexOffset = false;
//            mat.fresnelFade = false;
//            mat.fresnelPower = 0f;
//            mat.externalAlpha = 1f;

//            swordMaterial = mat.material;
//        }
//    }
//}

//// TODO: Sword glow material