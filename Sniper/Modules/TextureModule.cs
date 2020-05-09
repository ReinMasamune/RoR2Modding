using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using System.Runtime.CompilerServices;
using BepInEx.Logging;
using ReinCore;
using RoR2;
using Sniper.Data;
using Sniper.ScriptableObjects.Custom;
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
                        new GradientColorKey(new Color( 0.3f, 0.5f, 0.3f), 0.4f ),
                        new GradientColorKey(new Color( 0.6f, 0.8f, 0.6f ), 0.95f ),
                        new GradientColorKey(Color.white,1f),
                    },
                } );
            }

            return standardAmmoRamp;
        }
        private static Texture2D standardAmmoRamp;

        internal static Texture2D GetExplosiveAmmoRamp()
        {
            if( explosiveAmmoRamp == null )
            {
                explosiveAmmoRamp = TexturesCore.GenerateRampTexture( new Gradient
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
                        new GradientColorKey(new Color( 0.5f, 0.4f, 0.3f), 0.4f ),
                        new GradientColorKey(new Color( 0.8f, 0.65f, 0.4f ), 0.95f ),
                        new GradientColorKey(Color.white,1f),
                    },
                } );
            }

            return explosiveAmmoRamp;
        }
        private static Texture2D explosiveAmmoRamp;



        private static SniperTextureSet masterSet;
        
        private static void ScanAndLoadPacks()
        {
            var embeddedBundle = AssetModule.GetSniperAssetBundle();
            masterSet = embeddedBundle.LoadAllAssets<SniperTextureSet>()[0];

            var path = Assembly.GetExecutingAssembly().Location;
            var dir = new FileInfo( path ).Directory;

            foreach( var file in dir.EnumerateFiles("*-tex") )
            {
                try
                {
                    var bundle = AssetBundle.LoadFromFile(file.FullName);
                    foreach( var set in bundle.LoadAllAssets<SniperTextureSet>() )
                    {
                        masterSet.MergeAndReplace( set );
                    }
                } catch( Exception e ) 
                {
                    Log.Error( e );
                }
            }

        }


        private static SniperTextureSet GetMasterSet()
        {
            if( masterSet == null ) ScanAndLoadPacks();

            return masterSet;
        }


        internal static TextureSet GetSniperDefaultTextures()
        {
            if( _sniperDefaultTextures == null )
            {
                var set = GetMasterSet();
                var tex1 = set[Properties.Resources.SniperDefault_Diffuse];
                var tex2 = set[Properties.Resources.SniperDefault_Normal];
                var tex3 = set[Properties.Resources.SniperDefault_Emissive];

                _sniperDefaultTextures = new TextureSet( tex1, tex2, tex3 );
            }

            return _sniperDefaultTextures;
        }
        private static TextureSet _sniperDefaultTextures;

        internal static TextureSet GetSniperSkin1Textures()
        {
            if( _sniperSkin1Textures == null )
            {
                var set = GetMasterSet();
                var tex1 = set[Properties.Resources.SniperSkin1_Diffuse];
                var tex2 = set[Properties.Resources.SniperSkin1_Normal];
                var tex3 = set[Properties.Resources.SniperSkin1_Emissive];

                _sniperSkin1Textures = new TextureSet( tex1, tex2, tex3 );
            }

            return _sniperSkin1Textures;
        }
        private static TextureSet _sniperSkin1Textures;

        internal static TextureSet GetSniperSkin2Textures()
        {
            if( _sniperSkin2Textures == null )
            {
                var set = GetMasterSet();
                var tex1 = set[Properties.Resources.SniperSkin2_Diffuse];
                var tex2 = set[Properties.Resources.SniperSkin2_Normal];
                var tex3 = set[Properties.Resources.SniperSkin2_Emissive];

                _sniperSkin2Textures = new TextureSet( tex1, tex2, tex3 );
            }

            return _sniperSkin2Textures;
        }
        private static TextureSet _sniperSkin2Textures;

        internal static TextureSet GetSniperSkin3Textures()
        {
            if( _sniperSkin3Textures == null )
            {
                var set = GetMasterSet();
                var tex1 = set[Properties.Resources.SniperSkin3_Diffuse];
                var tex2 = set[Properties.Resources.SniperSkin3_Normal];
                var tex3 = set[Properties.Resources.SniperSkin3_Emissive];

                _sniperSkin3Textures = new TextureSet( tex1, tex2, tex3 );
            }

            return _sniperSkin3Textures;
        }
        private static TextureSet _sniperSkin3Textures;


        internal static TextureSet GetSniperSkin4Textures()
        {
            if( _sniperSkin4Textures == null )
            {
                var set = GetMasterSet();
                var tex1 = set[Properties.Resources.SniperSkin4_Diffuse];
                var tex2 = set[Properties.Resources.SniperSkin4_Normal];
                var tex3 = set[Properties.Resources.SniperSkin4_Emissive];

                _sniperSkin4Textures = new TextureSet( tex1, tex2, tex3 );
            }

            return _sniperSkin4Textures;
        }
        private static TextureSet _sniperSkin4Textures;

        internal static TextureSet GetSniperSkin5Textures()
        {
            if( _sniperSkin5Textures == null )
            {
                var set = GetMasterSet();
                var tex1 = set[Properties.Resources.SniperSkin5_Diffuse];
                var tex2 = set[Properties.Resources.SniperSkin5_Normal];
                var tex3 = set[Properties.Resources.SniperSkin5_Emissive];

                _sniperSkin5Textures = new TextureSet( tex1, tex2, tex3 );
            }

            return _sniperSkin5Textures;
        }
        private static TextureSet _sniperSkin5Textures;




        internal static TextureSet GetRailDefaultTextures()
        {
            if( _railDefaultTextures == null )
            {
                var set = GetMasterSet();
                var tex1 = set[Properties.Resources.RailgunDefault_Diffuse];
                var tex2 = set[Properties.Resources.RailgunDefault_Normal];
                var tex3 = set[Properties.Resources.RailgunDefault_Emissive];

                _railDefaultTextures = new TextureSet( tex1, tex2, tex3 );
            }

            return _railDefaultTextures;
        }
        private static TextureSet _railDefaultTextures;

        internal static TextureSet GetRailAlt1Textures()
        {
            if( _railAlt1Textures == null )
            {
                var set = GetMasterSet();
                var tex1 = set[Properties.Resources.RailgunAlt1_Diffuse];
                var tex2 = set[Properties.Resources.RailgunAlt1_Normal];
                var tex3 = set[Properties.Resources.RailgunAlt1_Emissive];

                _railAlt1Textures = new TextureSet( tex1, tex2, tex3 );
            }

            return _railAlt1Textures;
        }
        private static TextureSet _railAlt1Textures;

        internal static TextureSet GetRailAlt2Textures()
        {
            if( _railAlt2Textures == null )
            {
                var set = GetMasterSet();
                var tex1 = set[Properties.Resources.RailgunAlt2_Diffuse];
                var tex2 = set[Properties.Resources.RailgunAlt2_Normal];
                var tex3 = set[Properties.Resources.RailgunAlt2_Emissive];

                _railAlt2Textures = new TextureSet( tex1, tex2, tex3 );
            }

            return _railAlt2Textures;
        }
        private static TextureSet _railAlt2Textures;

        internal static TextureSet GetRailAlt3Textures()
        {
            if( _railAlt3Textures == null )
            {
                var set = GetMasterSet();
                var tex1 = set[Properties.Resources.RailgunAlt3_Diffuse];
                var tex2 = set[Properties.Resources.RailgunAlt3_Normal];
                var tex3 = set[Properties.Resources.RailgunAlt3_Emissive];

                _railAlt3Textures = new TextureSet( tex1, tex2, tex3 );
            }

            return _railAlt3Textures;
        }
        private static TextureSet _railAlt3Textures;




        internal static TextureSet GetThrowKnifeDefaultTextures()
        {
            if( _throwKnifeDefaultTextures == null )
            {
                var set = GetMasterSet();
                var tex1 = set[Properties.Resources.ThrowKnifeDefault_Diffuse];
                var tex2 = set[Properties.Resources.ThrowKnifeDefault_Normal];
                var tex3 = set[Properties.Resources.ThrowKnifeDefault_Emissive];

                _throwKnifeDefaultTextures = new TextureSet( tex1, tex2, tex3 );
            }

            return _throwKnifeDefaultTextures;
        }
        private static TextureSet _throwKnifeDefaultTextures;
    }

}
