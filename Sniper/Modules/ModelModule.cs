using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BepInEx.Logging;
using ReinCore;
using RoR2;
using UnityEngine;
using Sniper.Properties;
using Sniper.Data;
using Unity.Jobs;

namespace Sniper.Modules
{
    internal static class ModelModule
    {
        private static readonly Color[] iconColors = new[]
        {
            new Color( 0.2452f, 0.2452f, 0.2452f ),
            new Color( 0.1195f, 1f, 0f ),
            new Color( 0.1037f, 0.1037f, 0.1037f ),
            new Color( 0.6851f, 1f, 0.6273f ),

            new Color( 0.1645f, 0f, 0.3018f ),
            new Color( 0.6174f, 0f, 1f ),
            new Color( 0.0660f, 0.0660f, 0.0660f ),
            new Color( 0.8773f, 0.7122f, 1f ),

            new Color( 0.1509f, 0.1509f, 0.1509f ),
            new Color( 1f, 0.3397f, 0f ),
            new Color( 0.3373f, 0.3373f, 0.3373f ),
            new Color( 0.5566f, 0.5566f, 0.5566f ),

            new Color( 0.1266f, 0.1266f, 0.1266f ),
            new Color( 0.2984f, 1f, 0f ),
            new Color( 0.1981f, 0.1118f, 0f ),
            new Color( 0.0508f, 0.1603f, 0f ),

            new Color( 0.0943f, 0.0943f, 0.0943f ),
            new Color( 1f, 0f, 0f ),
            new Color( 0.2547f, 0.2547f, 0.2547f ),
            new Color( 0.2542f, 0f, 0f ),
        };

        internal static GameObject GetModel()
        {
            if( _model == null )
            {
                _model = CreateModel();
            }

            return _model;
        }

#pragma warning disable IDE1006 // Naming Styles
        private static GameObject _model;
#pragma warning restore IDE1006 // Naming Styles
        private static GameObject CreateModel()
        {
            Int32 ind = 0;
            ITextureJob defaultIconJob = TexturesCore.GenerateCrossTextureBatch( 512, 512, 100, 20, 3, new Color( 0.9f, 0.9f, 0.9f ), new Color( 0.4f, 0.4f, 0.4f ),
                iconColors[ind++], iconColors[ind++], iconColors[ind++], iconColors[ind++] );

            ITextureJob alt1IconJob = TexturesCore.GenerateCrossTextureBatch( 512, 512, 100, 20, 3, new Color( 0.9f, 0.9f, 0.9f ), new Color( 0.4f, 0.4f, 0.4f ),
                iconColors[ind++], iconColors[ind++], iconColors[ind++], iconColors[ind++] );

            ITextureJob alt2IconJob = TexturesCore.GenerateCrossTextureBatch( 512, 512, 100, 20, 3, new Color( 0.9f, 0.9f, 0.9f ), new Color( 0.4f, 0.4f, 0.4f ),
                iconColors[ind++], iconColors[ind++], iconColors[ind++], iconColors[ind++] );

            ITextureJob alt3IconJob = TexturesCore.GenerateCrossTextureBatch( 512, 512, 100, 20, 3, new Color( 0.9f, 0.9f, 0.9f ), new Color( 0.4f, 0.4f, 0.4f ),
                iconColors[ind++], iconColors[ind++], iconColors[ind++], iconColors[ind++] );

            ITextureJob alt4IconJob = TexturesCore.GenerateCrossTextureBatch( 512, 512, 100, 20, 3, new Color( 0.9f, 0.9f, 0.9f ), new Color( 0.4f, 0.4f, 0.4f ),
                iconColors[ind++], iconColors[ind++], iconColors[ind++], iconColors[ind++] );

            JobHandle.ScheduleBatchedJobs();


            GameObject model = AssetModule.GetSniperAssetBundle().LoadAsset<GameObject>( Properties.Resources.SniperPrefabPath );
            CharacterModel charModel = model.GetComponent<CharacterModel>();

            for( Int32 i = 0; i < charModel.baseRendererInfos.Length; ++i )
            {
                CharacterModel.RendererInfo info = charModel.baseRendererInfos[i];
                charModel.baseRendererInfos[i] = info;
            }

            var defaultSkin = new SniperSkin
            (
                "Default",
                TextureModule.GetSniperDefaultTextures(),
                TextureModule.GetRailDefaultTextures(),
                TextureModule.GetThrowKnifeDefaultTextures(),
                TextureModule.GetSniperDefaultTextures()
            );
            defaultSkin.ApplyDefaultSkinModifiers();

            var alt1 = new SniperSkin
            (
                "Alt1",
                TextureModule.GetSniperSkin1Textures(),
                TextureModule.GetRailAlt1Textures(),
                TextureModule.GetThrowKnifeDefaultTextures(),
                TextureModule.GetSniperSkin1Textures()
            );
            alt1.ApplyAlt1SkinModifiers();

            var alt2 = new SniperSkin
            (
                "Alt2",
                TextureModule.GetSniperSkin2Textures(),
                TextureModule.GetRailAlt2Textures(),
                TextureModule.GetThrowKnifeDefaultTextures(),
                TextureModule.GetSniperSkin2Textures()
            );
            alt2.ApplyAlt2SkinModifiers();


            var alt3 = new SniperSkin
            (
                "Alt3",
                TextureModule.GetSniperSkin3Textures(),
                TextureModule.GetRailAlt3Textures(),
                TextureModule.GetThrowKnifeDefaultTextures(),
                TextureModule.GetSniperSkin3Textures()
            );
            alt3.ApplyAlt3SkinModifiers();

            var alt4 = new SniperSkin
            (
                "Alt4",
                TextureModule.GetSniperSkin4Textures(),
                TextureModule.GetRailAlt2Textures(),
                TextureModule.GetThrowKnifeDefaultTextures(),
                TextureModule.GetSniperSkin4Textures()

            );
            alt4.ApplyAlt4SkinModifiers();





            Texture2D defaultTex = defaultIconJob.OutputTextureAndDispose();
            defaultSkin.CreateAndAddSkin( charModel, Properties.Tokens.SNIPER_SKIN_DEFAULT_NAME, "", 
                Sprite.Create( defaultTex, new Rect( 0f, 0f, defaultTex.width, defaultTex.height ), new Vector2( 0.5f, 0.5f ) ) );

            Texture2D alt1Tex = alt1IconJob.OutputTextureAndDispose();
            alt1.CreateAndAddSkin( charModel, Properties.Tokens.SNIPER_SKIN_ALT1_NAME, "",
                Sprite.Create( alt1Tex, new Rect( 0f, 0f, alt1Tex.width, alt1Tex.height ), new Vector2( 0.5f, 0.5f ) ) );

            Texture2D alt2Tex = alt2IconJob.OutputTextureAndDispose();
            alt2.CreateAndAddSkin( charModel, Properties.Tokens.SNIPER_SKIN_ALT2_NAME, "",
                Sprite.Create( alt2Tex, new Rect( 0f, 0f, alt2Tex.width, alt2Tex.height ), new Vector2( 0.5f, 0.5f ) ) );

            Texture2D alt3Tex = alt3IconJob.OutputTextureAndDispose();
            alt3.CreateAndAddSkin( charModel, Properties.Tokens.SNIPER_SKIN_ALT3_NAME, "",
                Sprite.Create( alt3Tex, new Rect( 0f, 0f, alt3Tex.width, alt3Tex.height ), new Vector2( 0.5f, 0.5f ) ) );

            Texture2D alt4Tex = alt4IconJob.OutputTextureAndDispose();
            alt4.CreateAndAddSkin( charModel, Properties.Tokens.SNIPER_SKIN_ALT4_NAME, "",
                Sprite.Create( alt4Tex, new Rect( 0f, 0f, alt4Tex.width, alt4Tex.height ), new Vector2( 0.5f, 0.5f ) ) );


            var skinsArray = charModel.GetComponent<ModelSkinController>().skins;
            CharacterModel.RendererInfo[] defaultSkinRenderers = skinsArray[0].rendererInfos;
            for( Int32 i = 0; i < defaultSkinRenderers.Length; ++i )
            {
                CharacterModel.RendererInfo info = charModel.baseRendererInfos[i];
                info.defaultMaterial = defaultSkinRenderers[i].defaultMaterial;
                charModel.baseRendererInfos[i] = info;
            }


            
            return model;
        }

        private static void ApplyDefaultSkinModifiers( this SniperSkin defaultSkin )
        {
            defaultSkin.AddMaterialModifier( SniperSkin.SniperMaterial.All, ( mat ) =>
            {
                mat.mainColor = Color.white;
                mat.cull = MaterialBase.CullMode.Back;
                mat.ignoreDiffuseAlphaForSpecular = true;
                mat.rampChoice = MaterialBase.RampInfo.SmoothedTwoTone;
                mat.specularExponent = 3f;
                mat.emissionPower = 1f;
                mat.smoothness = 0f;
                mat.emissionColor = Color.clear;
            } );

            defaultSkin.AddMaterialModifier( SniperSkin.SniperMaterial.Armor, ( mat ) =>
            {
                mat.cull = MaterialBase.CullMode.Off;
                mat.emissionColor = Color.white;
                mat.smoothness = 1f;
                mat.specularStrength = 0.1f;
                mat.specularExponent = 1f;
            } );

            defaultSkin.AddMaterialModifier( SniperSkin.SniperMaterial.Ammo, ( mat ) =>
            {
                mat.rampChoice = MaterialBase.RampInfo.TwoTone;
                mat.smoothness = 0.5f;
                mat.normalStrength = 0.05f;
                mat.emissionColor = new Color( 0f, 1f, 0f, 1f );
                mat.emissionPower = 5f;
                mat.specularStrength = 0.75f;
                mat.specularExponent = 2f;
            } );

            defaultSkin.AddMaterialModifier( SniperSkin.SniperMaterial.Cloak, ( mat ) =>
            {
                mat.rampChoice = MaterialBase.RampInfo.SubSurface;
                mat.cull = MaterialBase.CullMode.Off;
                mat.emissionPower = 0f;
                mat.specularStrength = 0.05f;
                mat.normalStrength = 0.6f;
            } );

            defaultSkin.AddMaterialModifier( SniperSkin.SniperMaterial.Body, ( mat ) =>
            {
                mat.emissionPower = 0f;
                mat.normalStrength = 0.8f;
                mat.smoothness = 0.25f;
                mat.specularStrength = 0.5f;
            } );

            defaultSkin.AddMaterialModifier( SniperSkin.SniperMaterial.Emissive, ( mat ) =>
            {
                mat.rampChoice = MaterialBase.RampInfo.Unlitish;
                mat.normalStrength = 0f;
                mat.emissionColor = Color.white;
                mat.specularExponent = 1f;
                mat.specularStrength = 0f;
            } );

            defaultSkin.AddMaterialModifier( SniperSkin.SniperMaterial.Rail, ( mat ) =>
            {
                mat.normalStrength = 1f;
                mat.emissionColor = new Color( 0f, 1f, 0.48f, 1f );
                mat.smoothness = 0.5f;
                mat.specularStrength = 0.3f;
            } );
        }

        private static void ApplyAlt1SkinModifiers( this SniperSkin alt1 )
        {
            alt1.AddMaterialModifier( SniperSkin.SniperMaterial.All, ( mat ) =>
            {
                mat.mainColor = Color.white;
                mat.cull = MaterialBase.CullMode.Back;
                mat.ignoreDiffuseAlphaForSpecular = true;
                mat.rampChoice = MaterialBase.RampInfo.SmoothedTwoTone;
                mat.specularExponent = 3f;
                mat.emissionPower = 1f;
                mat.smoothness = 0f;
                mat.emissionColor = Color.clear;
            } );

            alt1.AddMaterialModifier( SniperSkin.SniperMaterial.Armor, ( mat ) =>
            {
                mat.mainColor = new Color( 0.95f, 0.8f, 0.6f, 1f );
                mat.emissionColor = Color.white;
                mat.emissionPower = 2f;
                mat.smoothness = 0.5f;
                mat.rampChoice = MaterialBase.RampInfo.SmoothedTwoTone;
                mat.specularStrength = 0.15f;
                mat.specularExponent = 3f;
                mat.cull = MaterialBase.CullMode.Off;
            } );

            alt1.AddMaterialModifier( SniperSkin.SniperMaterial.Ammo, ( mat ) =>
            {
                mat.rampChoice = MaterialBase.RampInfo.TwoTone;
                mat.smoothness = 0.5f;
                mat.normalStrength = 0.05f;
                mat.emissionColor = Color.white;
                mat.emissionPower = 5f;
                mat.specularStrength = 0.75f;
                mat.specularExponent = 2f;
            } );

            alt1.AddMaterialModifier( SniperSkin.SniperMaterial.Cloak, ( mat ) =>
            {
                mat.mainColor = new Color( 0.95f, 0.8f, 0.6f, 1f );
                mat.rampChoice = MaterialBase.RampInfo.SmoothedTwoTone;
                mat.cull = MaterialBase.CullMode.Off;
                mat.emissionPower = 0f;
                mat.smoothness = 0.5f;
                mat.specularStrength = 0.15f;
                mat.normalStrength = 0.2f;
                mat.specularExponent = 3f;
            } );

            alt1.AddMaterialModifier( SniperSkin.SniperMaterial.Body, ( mat ) =>
            {
                mat.mainColor = Color.white;
                mat.emissionPower = 0f;
                mat.normalStrength = 0.05f;
                mat.smoothness = 0.5f;
                mat.specularStrength = 0.15f;
                mat.rampChoice = MaterialBase.RampInfo.SmoothedTwoTone;
                mat.specularExponent = 3f;
            } );

            alt1.AddMaterialModifier( SniperSkin.SniperMaterial.Emissive, ( mat ) =>
            {
                mat.rampChoice = MaterialBase.RampInfo.Unlitish;
                mat.normalStrength = 0f;
                mat.emissionColor = Color.white;
                mat.specularExponent = 1f;
                mat.specularStrength = 0f;
                mat.emissionPower = 3.5f;
            } );

            alt1.AddMaterialModifier( SniperSkin.SniperMaterial.Rail, ( mat ) =>
            {
                mat.mainColor = new Color( 0.76f, 0.76f, 0.76f, 1f );
                mat.normalStrength = 1f;
                mat.emissionColor = new Color( 0.95f, 0.4f, 1f, 1f );
                mat.emissionPower = 3f;
                mat.smoothness = 0.6f;
                mat.specularStrength = 0.5f;
                mat.specularExponent = 2f;
            } );
        }



        private static void ApplyAlt2SkinModifiers( this SniperSkin alt2 )
        {
            alt2.AddMaterialModifier( SniperSkin.SniperMaterial.All, ( mat ) =>
            {
                mat.mainColor = Color.white;
                mat.cull = MaterialBase.CullMode.Back;
                mat.ignoreDiffuseAlphaForSpecular = true;
                mat.rampChoice = MaterialBase.RampInfo.SmoothedTwoTone;
                mat.specularExponent = 3f;
                mat.emissionPower = 1f;
                mat.smoothness = 0f;
                mat.emissionColor = Color.clear;
            } );

            alt2.AddMaterialModifier( SniperSkin.SniperMaterial.Armor, ( mat ) =>
            {
                mat.mainColor = new Color( 0.68f, 0.68f, 0.68f, 1f );
                mat.emissionColor = Color.white;
                mat.emissionPower = 2f;
                mat.smoothness = 0.5f;
                mat.rampChoice = MaterialBase.RampInfo.TwoTone;
                mat.specularStrength = 0.1f;
                mat.specularExponent = 5f;
                mat.cull = MaterialBase.CullMode.Off;
            } );

            alt2.AddMaterialModifier( SniperSkin.SniperMaterial.Ammo, ( mat ) =>
            {
                mat.rampChoice = MaterialBase.RampInfo.TwoTone;
                mat.smoothness = 0.5f;
                mat.normalStrength = 0.05f;
                mat.emissionColor = Color.white;
                mat.emissionPower = 5f;
                mat.specularStrength = 0.75f;
                mat.specularExponent = 2f;
            } );

            alt2.AddMaterialModifier( SniperSkin.SniperMaterial.Cloak, ( mat ) =>
            {
                mat.mainColor = new Color( 0.47f, 0.47f, 0.47f, 1f );
                mat.rampChoice = MaterialBase.RampInfo.SmoothedTwoTone;
                mat.cull = MaterialBase.CullMode.Off;
                mat.emissionPower = 0f;
                mat.smoothness = 0.5f;
                mat.specularStrength = 0.15f;
                mat.normalStrength = 0.2f;
                mat.specularExponent = 5f;
            } );

            alt2.AddMaterialModifier( SniperSkin.SniperMaterial.Body, ( mat ) =>
            {
                mat.mainColor = new Color( 0.78f, 0.78f, 0.78f, 1f );
                mat.emissionPower = 0f;
                mat.normalStrength = 1f;
                mat.smoothness = 0.5f;
                mat.specularStrength = 0.5f;
                mat.rampChoice = MaterialBase.RampInfo.SmoothedTwoTone;
                mat.specularExponent = 5f;
            } );

            alt2.AddMaterialModifier( SniperSkin.SniperMaterial.Emissive, ( mat ) =>
            {
                mat.rampChoice = MaterialBase.RampInfo.Unlitish;
                mat.normalStrength = 0f;
                mat.emissionColor = Color.white;
                mat.specularExponent = 1f;
                mat.specularStrength = 0f;
                mat.emissionPower = 3f;
            } );

            alt2.AddMaterialModifier( SniperSkin.SniperMaterial.Rail, ( mat ) =>
            {
                mat.mainColor = new Color( 0.76f, 0.76f, 0.76f, 1f );
                mat.normalStrength = 1f;
                mat.emissionColor = Color.white;
                mat.emissionPower = 2f;
                mat.smoothness = 0.6f;
                mat.specularStrength = 0.15f;
                mat.specularExponent = 5f;
            } );
        }


        private static void ApplyAlt3SkinModifiers( this SniperSkin alt3 )
        {
            alt3.AddMaterialModifier( SniperSkin.SniperMaterial.All, ( mat ) =>
            {
                mat.mainColor = Color.white;
                mat.cull = MaterialBase.CullMode.Back;
                mat.ignoreDiffuseAlphaForSpecular = true;
                mat.rampChoice = MaterialBase.RampInfo.SmoothedTwoTone;
                mat.specularExponent = 3f;
                mat.emissionPower = 1f;
                mat.smoothness = 0f;
                mat.emissionColor = Color.clear;
            } );

            alt3.AddMaterialModifier( SniperSkin.SniperMaterial.Armor, ( mat ) =>
            {
                mat.mainColor = new Color( 0.85f, 0.85f, 0.85f, 1f );
                mat.emissionColor = Color.white;
                mat.emissionPower = 2f;
                mat.smoothness = 0.5f;
                mat.rampChoice = MaterialBase.RampInfo.SmoothedTwoTone;
                mat.specularStrength = 0.15f;
                mat.specularExponent = 3f;
                mat.cull = MaterialBase.CullMode.Off;
            } );

            alt3.AddMaterialModifier( SniperSkin.SniperMaterial.Ammo, ( mat ) =>
            {
                mat.rampChoice = MaterialBase.RampInfo.TwoTone;
                mat.smoothness = 0.5f;
                mat.normalStrength = 0.05f;
                mat.emissionColor = Color.white;
                mat.emissionPower = 5f;
                mat.specularStrength = 0.75f;
                mat.specularExponent = 2f;
            } );

            alt3.AddMaterialModifier( SniperSkin.SniperMaterial.Cloak, ( mat ) =>
            {
                mat.mainColor = Color.white;
                mat.rampChoice = MaterialBase.RampInfo.SmoothedTwoTone;
                mat.cull = MaterialBase.CullMode.Off;
                mat.emissionPower = 0f;
                mat.smoothness = 0.5f;
                mat.specularStrength = 0.15f;
                mat.normalStrength = 0.2f;
                mat.specularExponent = 3f;
            } );

            alt3.AddMaterialModifier( SniperSkin.SniperMaterial.Body, ( mat ) =>
            {
                mat.mainColor = new Color( 0.5377358f, 0.3714074f, 0.2764774f, 1f );
                mat.emissionPower = 0f;
                mat.normalStrength = 1f;
                mat.smoothness = 0.5f;
                mat.specularStrength = 0.15f;
                mat.rampChoice = MaterialBase.RampInfo.SmoothedTwoTone;
                mat.specularExponent = 3f;
            } );

            alt3.AddMaterialModifier( SniperSkin.SniperMaterial.Emissive, ( mat ) =>
            {
                mat.rampChoice = MaterialBase.RampInfo.Unlitish;
                mat.normalStrength = 0f;
                mat.emissionColor = Color.white;
                mat.specularExponent = 1f;
                mat.specularStrength = 0f;
                mat.emissionPower = 2f;
            } );

            alt3.AddMaterialModifier( SniperSkin.SniperMaterial.Rail, ( mat ) =>
            {
                mat.mainColor = Color.white;
                mat.normalStrength = 1f;
                mat.emissionColor = Color.white;
                mat.emissionPower = 2f;
                mat.smoothness = 0.6f;
                mat.specularStrength = 0.35f;
                mat.specularExponent = 3f;
            } );
        }

        private static void ApplyAlt4SkinModifiers( this SniperSkin alt4 )
        {
            alt4.AddMaterialModifier( SniperSkin.SniperMaterial.All, ( mat ) =>
            {
                mat.mainColor = new Color( 0.6f, 0.6f, 0.6f, 1f );
                mat.cull = MaterialBase.CullMode.Back;
                mat.ignoreDiffuseAlphaForSpecular = true;
                mat.rampChoice = MaterialBase.RampInfo.SmoothedTwoTone;
                mat.specularExponent = 3f;
                mat.emissionPower = 1f;
                mat.smoothness = 0f;
                mat.emissionColor = Color.clear;
            } );

            alt4.AddMaterialModifier( SniperSkin.SniperMaterial.Armor, ( mat ) =>
            {
                mat.cull = MaterialBase.CullMode.Off;
                mat.emissionColor = new Color( 0.85f, 0.85f, 0.85f, 1f );
                mat.emissionPower = 2f;
                mat.smoothness = 0.25f;
                mat.normalStrength = 3f;
                mat.specularStrength = 0.15f;
                mat.specularExponent = 3f;
            } );

            alt4.AddMaterialModifier( SniperSkin.SniperMaterial.Ammo, ( mat ) =>
            {
                mat.rampChoice = MaterialBase.RampInfo.TwoTone;
                mat.smoothness = 0.5f;
                mat.normalStrength = 0.05f;
                mat.emissionColor = Color.white;
                mat.emissionPower = 5f;
                mat.specularStrength = 0.75f;
                mat.specularExponent = 2f;
            } );

            alt4.AddMaterialModifier( SniperSkin.SniperMaterial.Cloak, ( mat ) =>
            {
                mat.rampChoice = MaterialBase.RampInfo.SmoothedTwoTone;
                mat.cull = MaterialBase.CullMode.Off;
                mat.emissionPower = 0f;
                mat.specularStrength = 0.3f;
                mat.specularExponent = 2f;
                mat.normalStrength = 0.1f;
                mat.smoothness = 0.25f;

            } );

            alt4.AddMaterialModifier( SniperSkin.SniperMaterial.Body, ( mat ) =>
            {
                mat.mainColor = Color.white;
                mat.emissionPower = 0f;
                mat.normalStrength = 1f;
                mat.smoothness = 0.1f;
                mat.specularStrength = 0.05f;
                mat.specularExponent = 3f;
                mat.rampChoice = MaterialBase.RampInfo.TwoTone;
            } );

            alt4.AddMaterialModifier( SniperSkin.SniperMaterial.Emissive, ( mat ) =>
            {
                mat.rampChoice = MaterialBase.RampInfo.Unlitish;
                mat.normalStrength = 0f;
                mat.emissionColor = Color.white;
                mat.specularExponent = 1f;
                mat.emissionPower = 3f;
                mat.specularStrength = 0f;
            } );

            alt4.AddMaterialModifier( SniperSkin.SniperMaterial.Rail, ( mat ) =>
            {
                mat.normalStrength = 1f;
                mat.emissionColor = Color.white;
                mat.smoothness = 0.5f;
                mat.specularStrength = 0.2f;
                mat.specularExponent = 2f;
                mat.emissionPower = 2f;
                mat.mainColor = new Color( 0.8f, 0.8f, 0.8f, 1f );
            } );
        }

        private static void ApplyAlt5SkinModifiers( this SniperSkin alt5 )
        {
            alt5.AddMaterialModifier( SniperSkin.SniperMaterial.All, ( mat ) =>
            {
                mat.mainColor = Color.white;
                mat.cull = MaterialBase.CullMode.Back;
                mat.ignoreDiffuseAlphaForSpecular = true;
                mat.rampChoice = MaterialBase.RampInfo.SmoothedTwoTone;
                mat.specularExponent = 3f;
                mat.emissionPower = 1f;
                mat.smoothness = 0f;
                mat.emissionColor = Color.clear;
            } );

            alt5.AddMaterialModifier( SniperSkin.SniperMaterial.Armor, ( mat ) =>
            {
                mat.mainColor = new Color( 0.55f, 0.55f, 0.55f, 1f );
                mat.emissionColor = Color.white;
                mat.emissionPower = 2f;
                mat.smoothness = 0.5f;
                mat.rampChoice = MaterialBase.RampInfo.TwoTone;
                mat.specularStrength = 0.1f;
                mat.specularExponent = 5f;
                mat.cull = MaterialBase.CullMode.Off;
            } );

            alt5.AddMaterialModifier( SniperSkin.SniperMaterial.Ammo, ( mat ) =>
            {
                mat.rampChoice = MaterialBase.RampInfo.TwoTone;
                mat.smoothness = 0.5f;
                mat.normalStrength = 0.05f;
                mat.emissionColor = Color.white;
                mat.emissionPower = 5f;
                mat.specularStrength = 0.75f;
                mat.specularExponent = 2f;
            } );

            alt5.AddMaterialModifier( SniperSkin.SniperMaterial.Cloak, ( mat ) =>
            {
                mat.mainColor = new Color( 0.47f, 0.47f, 0.47f, 1f );
                mat.rampChoice = MaterialBase.RampInfo.SmoothedTwoTone;
                mat.cull = MaterialBase.CullMode.Off;
                mat.emissionPower = 0f;
                mat.smoothness = 0.5f;
                mat.specularStrength = 0.15f;
                mat.normalStrength = 0.2f;
                mat.specularExponent = 5f;
            } );

            alt5.AddMaterialModifier( SniperSkin.SniperMaterial.Body, ( mat ) =>
            {
                mat.mainColor = new Color( 0.78f, 0.78f, 0.78f, 1f );
                mat.emissionPower = 0f;
                mat.normalStrength = 1f;
                mat.smoothness = 0.5f;
                mat.specularStrength = 0.5f;
                mat.rampChoice = MaterialBase.RampInfo.SmoothedTwoTone;
                mat.specularExponent = 5f;
            } );

            alt5.AddMaterialModifier( SniperSkin.SniperMaterial.Emissive, ( mat ) =>
            {
                mat.rampChoice = MaterialBase.RampInfo.Unlitish;
                mat.normalStrength = 0f;
                mat.emissionColor = Color.white;
                mat.specularExponent = 1f;
                mat.specularStrength = 0f;
                mat.emissionPower = 3f;
            } );

            alt5.AddMaterialModifier( SniperSkin.SniperMaterial.Rail, ( mat ) =>
            {
                mat.mainColor = new Color( 0.76f, 0.76f, 0.76f, 1f );
                mat.normalStrength = 1f;
                mat.emissionColor = Color.white;
                mat.emissionPower = 2f;
                mat.smoothness = 0.6f;
                mat.specularStrength = 0.15f;
                mat.specularExponent = 5f;
            } );
        }


    }
}
