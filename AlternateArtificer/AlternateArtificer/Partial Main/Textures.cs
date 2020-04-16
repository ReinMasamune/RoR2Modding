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
//        partial void Textures()
//        {
//            base.awake += this.Main_awake6;
//        }

//        private void Main_awake6()
//        {
//            swordRamp = TexturesCore.GenerateRampTexture( new Gradient
//            {
//                mode = GradientMode.Blend,
//                alphaKeys = new[]
//                {
//                    new GradientAlphaKey(0f, 0f),
//                    new GradientAlphaKey(1f, 1f),
//                },
//                colorKeys = new[]
//                {
//                    new GradientColorKey(Color.black, 0f),
//                    new GradientColorKey(new Color( 0f, 0.8f, 1f), 0.95f ),
//                    new GradientColorKey(Color.white, 1f ),
//                },
//            } );

//            iceRamp = TexturesCore.GenerateRampTexture( new Gradient
//            {
//                mode = GradientMode.Blend,
//                alphaKeys = new GradientAlphaKey[8]
//    {
//                    new GradientAlphaKey( 0f, 0f ),
//                    new GradientAlphaKey( 0f, 0.14f ),
//                    new GradientAlphaKey( 0.22f, 0.46f ),
//                    new GradientAlphaKey( 0.22f, 0.61f),
//                    new GradientAlphaKey( 0.72f, 0.63f ),
//                    new GradientAlphaKey( 0.72f, 0.8f ),
//                    new GradientAlphaKey( 0.87f, 0.81f ),
//                    new GradientAlphaKey( 0.87f, 1f )
//    },
//                colorKeys = new GradientColorKey[8]
//    {
//                    new GradientColorKey( new Color( 0f, 0f, 0f ), 0f ),
//                    new GradientColorKey( new Color( 0f, 0f, 0f ), 0.14f ),
//                    new GradientColorKey( new Color( 0.179f, 0.278f, 0.250f ), 0.46f ),
//                    new GradientColorKey( new Color( 0.179f, 0.278f, 0.250f ), 0.61f ),
//                    new GradientColorKey( new Color( 0.612f, 0.906f, 0.815f ), 0.63f ),
//                    new GradientColorKey( new Color( 0.612f, 0.906f, 0.815f ), 0.8f ),
//                    new GradientColorKey( new Color( 0.776f, 0.957f, 0.861f ), 0.81f ),
//                    new GradientColorKey( new Color( 0.776f, 0.957f, 0.861f ), 1f )
//    }
//            } );
//        }
//    }
//}