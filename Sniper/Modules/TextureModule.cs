using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BepInEx.Logging;
using ReinCore;
using RoR2;
using Sniper.Data;
using UnityEngine;

namespace Sniper.Modules
{
    internal static class TextureModule
    {
        internal static Texture2D GetStandardAmmoRamp()
        {
            if( standardAmmoRamp == null )
            {
                standardAmmoRamp = TexturesCore.GenerateRampTexture( new Gradient
                {
                    mode = GradientMode.Blend,
                    alphaKeys = new[]
                    {
                        new GradientAlphaKey(0f, 0f),
                        new GradientAlphaKey(1f, 1f),
                    },
                    colorKeys = new[]
                    {
                        new GradientColorKey(Color.black,0f),
                        new GradientColorKey(new Color( 0.5f, 0.3f, 0.6f), 0.4f ),
                        new GradientColorKey(new Color( 0.8f, 0.6f, 0.8f ), 0.95f ),
                        new GradientColorKey(Color.white,1f),
                    },
                } );
            }

            return standardAmmoRamp;
        }
        private static Texture2D standardAmmoRamp;



        internal static TextureSet GetSniperClassicTextures()
        {
            if( _sniperClassicTextures == null )
            {
                var bundle = AssetModule.GetSniperAssetBundle();
                var tex1 = bundle.LoadAsset<Texture2D>("Assets/Textures/Classic_D.png");
                var tex2 = bundle.LoadAsset<Texture2D>("Assets/Textures/Classic_N.png");
                var tex3 = bundle.LoadAsset<Texture2D>("Assets/Textures/Classic_E.png");

                _sniperClassicTextures = new TextureSet( tex1, tex2, tex3 );
            }

            return _sniperClassicTextures;
        }
        private static TextureSet _sniperClassicTextures;

        internal static TextureSet GetSniperRedTextures()
        {
            if( _sniperRedTextures == null )
            {
                var bundle = AssetModule.GetSniperAssetBundle();
                var tex1 = bundle.LoadAsset<Texture2D>("Assets/Textures/Red_D.png");
                var tex2 = bundle.LoadAsset<Texture2D>("Assets/Textures/Red_N.png");
                var tex3 = bundle.LoadAsset<Texture2D>("Assets/Textures/Red_E.png");

                _sniperRedTextures = new TextureSet( tex1, tex2, tex3 );
            }

            return _sniperRedTextures;
        }
        private static TextureSet _sniperRedTextures;

        internal static TextureSet GetSniperRedBlackTextures()
        {
            if( _sniperRedBlackTextures == null )
            {
                var bundle = AssetModule.GetSniperAssetBundle();
                var tex1 = bundle.LoadAsset<Texture2D>("Assets/Textures/RedBlack_D.png");
                var tex2 = bundle.LoadAsset<Texture2D>("Assets/Textures/RedBlack_N.png");
                var tex3 = bundle.LoadAsset<Texture2D>("Assets/Textures/RedBlack_E.png");

                _sniperRedBlackTextures = new TextureSet( tex1, tex2, tex3 );
            }

            return _sniperRedBlackTextures;
        }
        private static TextureSet _sniperRedBlackTextures;

        internal static TextureSet GetSniperRailTextures()
        {
            if( _sniperRailTextures == null )
            {
                var bundle = AssetModule.GetSniperAssetBundle();
                var tex1 = bundle.LoadAsset<Texture2D>("Assets/Textures/RailGun_AlbedoTransparency.png");
                var tex2 = bundle.LoadAsset<Texture2D>("Assets/Textures/RailGun_Normal.png");
                var tex3 = bundle.LoadAsset<Texture2D>("Assets/Textures/RailGun_Emission.png");

                _sniperRailTextures = new TextureSet( tex1, tex2, tex3 );
            }

            return _sniperRailTextures;
        }
        private static TextureSet _sniperRailTextures;
    }

}
