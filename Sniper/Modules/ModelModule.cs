using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BepInEx.Logging;
using ReinCore;
using RoR2;
using UnityEngine;
using Sniper.Properties;
using Sniper.Data;

namespace Sniper.Modules
{
    internal static class ModelModule
    {
        internal static GameObject GetModel()
        {
            var defaultIcon = new SkinTextureJob( )


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
                TextureModule.GetRailDefaultTextures(),
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
                TextureModule.GetRailAlt1Textures(),
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

            


            defaultSkin.CreateAndAddSkin( charModel, Properties.Tokens.SNIPER_SKIN_DEFAULT_NAME, "", null );
            alt1.CreateAndAddSkin( charModel, Properties.Tokens.SNIPER_SKIN_ALT1_NAME, "", null );
            alt2.CreateAndAddSkin( charModel, Properties.Tokens.SNIPER_SKIN_ALT2_NAME, "", null );
            alt3.CreateAndAddSkin( charModel, Properties.Tokens.SNIPER_SKIN_ALT3_NAME, "", null );
            alt4.CreateAndAddSkin( charModel, Properties.Tokens.SNIPER_SKIN_ALT4_NAME, "", null );

            var defaultSkinRenderers = charModel.GetComponent<ModelSkinController>().skins[0].rendererInfos;
            for( Int32 i = 0; i < defaultSkinRenderers.Length; ++i )
            {
                var info = charModel.baseRendererInfos[i];
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
                mat.mainColor = new Color( 0.7f, 0.7f, 0.7f, 1f );
                mat.emissionColor = Color.white;
                mat.emissionPower = 1f;
                mat.smoothness = 0.5f;
                mat.rampChoice = MaterialBase.RampInfo.SmoothedTwoTone;
                mat.specularStrength = 0.05f;
                mat.specularExponent = 5f;
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
                mat.mainColor = new Color( 0.62f, 0.62f, 0.62f, 1f );
                mat.rampChoice = MaterialBase.RampInfo.SmoothedTwoTone;
                mat.cull = MaterialBase.CullMode.Off;
                mat.emissionPower = 0f;
                mat.smoothness = 0.5f;
                mat.specularStrength = 0.1f;
                mat.normalStrength = 0.2f;
                mat.specularExponent = 5f;
            } );

            alt1.AddMaterialModifier( SniperSkin.SniperMaterial.Body, ( mat ) =>
            {
                mat.mainColor = Color.white;
                mat.emissionPower = 0f;
                mat.normalStrength = 0.05f;
                mat.smoothness = 0.5f;
                mat.specularStrength = 0.1f;
                mat.rampChoice = MaterialBase.RampInfo.SmoothedTwoTone;
                mat.specularExponent = 5f;
            } );

            alt1.AddMaterialModifier( SniperSkin.SniperMaterial.Emissive, ( mat ) =>
            {
                mat.rampChoice = MaterialBase.RampInfo.Unlitish;
                mat.normalStrength = 0f;
                mat.emissionColor = Color.white;
                mat.specularExponent = 1f;
                mat.specularStrength = 0f;
                mat.emissionPower = 3f;
            } );

            alt1.AddMaterialModifier( SniperSkin.SniperMaterial.Rail, ( mat ) =>
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
                mat.mainColor = new Color( 0.55f, 0.55f, 0.55f, 1f );
                mat.emissionColor = Color.white;
                mat.emissionPower = 2f;
                mat.smoothness = 0.5f;
                mat.rampChoice = MaterialBase.RampInfo.TwoTone;
                mat.specularStrength = 0.1f;
                mat.specularExponent = 5f;
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
                mat.mainColor = new Color( 0.47f, 0.47f, 0.47f, 1f );
                mat.rampChoice = MaterialBase.RampInfo.SmoothedTwoTone;
                mat.cull = MaterialBase.CullMode.Off;
                mat.emissionPower = 0f;
                mat.smoothness = 0.5f;
                mat.specularStrength = 0.15f;
                mat.normalStrength = 0.2f;
                mat.specularExponent = 5f;
            } );

            alt3.AddMaterialModifier( SniperSkin.SniperMaterial.Body, ( mat ) =>
            {
                mat.mainColor = new Color( 0.78f, 0.78f, 0.78f, 1f );
                mat.emissionPower = 0f;
                mat.normalStrength = 1f;
                mat.smoothness = 0.5f;
                mat.specularStrength = 0.5f;
                mat.rampChoice = MaterialBase.RampInfo.SmoothedTwoTone;
                mat.specularExponent = 5f;
            } );

            alt3.AddMaterialModifier( SniperSkin.SniperMaterial.Emissive, ( mat ) =>
            {
                mat.rampChoice = MaterialBase.RampInfo.Unlitish;
                mat.normalStrength = 0f;
                mat.emissionColor = Color.white;
                mat.specularExponent = 1f;
                mat.specularStrength = 0f;
                mat.emissionPower = 3f;
            } );

            alt3.AddMaterialModifier( SniperSkin.SniperMaterial.Rail, ( mat ) =>
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

        private static void ApplyAlt4SkinModifiers( this SniperSkin alt4 )
        {
            alt4.AddMaterialModifier( SniperSkin.SniperMaterial.All, ( mat ) =>
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

            alt4.AddMaterialModifier( SniperSkin.SniperMaterial.Armor, ( mat ) =>
            {
                mat.cull = MaterialBase.CullMode.Off;
                mat.emissionColor = new Color( 0.85f, 0.85f, 0.85f, 1f );
                mat.smoothness = 0.25f;
                mat.normalStrength = 1f;
                mat.specularStrength = 0.015f;
                mat.specularExponent = 5f;
            } );

            alt4.AddMaterialModifier( SniperSkin.SniperMaterial.Ammo, ( mat ) =>
            {
                mat.rampChoice = MaterialBase.RampInfo.TwoTone;
                mat.smoothness = 0.5f;
                mat.normalStrength = 0.05f;
                mat.emissionColor = new Color( 0f, 1f, 0f, 1f );
                mat.emissionPower = 5f;
                mat.specularStrength = 0.75f;
                mat.specularExponent = 2f;
            } );

            alt4.AddMaterialModifier( SniperSkin.SniperMaterial.Cloak, ( mat ) =>
            {
                mat.rampChoice = MaterialBase.RampInfo.SmoothedTwoTone;
                mat.cull = MaterialBase.CullMode.Off;
                mat.emissionPower = 0f;
                mat.specularStrength = 0.01f;
                mat.specularExponent = 5f;
                mat.normalStrength = 0.6f;
                mat.smoothness = 0.25f;

            } );

            alt4.AddMaterialModifier( SniperSkin.SniperMaterial.Body, ( mat ) =>
            {
                mat.mainColor = new Color( 0.9f, 0.77f, 0.63f, 1f );
                mat.emissionPower = 0f;
                mat.normalStrength = 0.8f;
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
                mat.specularStrength = 0f;
            } );

            alt4.AddMaterialModifier( SniperSkin.SniperMaterial.Rail, ( mat ) =>
            {
                mat.normalStrength = 1f;
                mat.emissionColor = Color.white;
                mat.smoothness = 0.5f;
                mat.specularStrength = 0.15f;
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
