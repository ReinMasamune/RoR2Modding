namespace Rein.Sniper.Modules
{
    using System;

    using ReinCore;

    using RoR2;

    using Rein.Sniper.Data;

    using Unity.Jobs;

    using UnityEngine;
    using Rein.Sniper.Properties;

    internal static class ModelModule
    {
        private static readonly Color defaultEmisColor = new Color( 0f, 1f, 0.48f, 1f );
        private static readonly Color alt1EmisColor = new Color( 0.8f, 0.5f, 1f );
        private static readonly Color alt2EmisColor = new Color( 0.88f, 0.31f, 0.11f );
        private static readonly Color alt3EmisColor = new Color( 0.4f, 1f, 0f );
        private static readonly Color alt4EmisColor = new Color( 1f, 0f, 0f );
        private static readonly Color alt5EmisColor = new Color( 1f, 1f, 0f );
        private static readonly Color alt6EmisColor = new Color( 0.4f, 0.78f, 1f );
        private static readonly Color trashEmisColor = new(0.9f, 0.6f, 0.8f, 1f);

        private static Color Square( Color color )
        {
            return new Color( color.r * color.r, color.g * color.g, color.b * color.b, color.a * color.a );
        }

        private static Color Root( Color color )
        {
            return new Color( Mathf.Sqrt( color.r ), Mathf.Sqrt( color.g ), Mathf.Sqrt( color.b ), Mathf.Sqrt( color.a ) );
        }

        private static readonly Color[] iconColors = new[]
        {
            new Color( 0.2452f, 0.2452f, 0.2452f ),
            new Color( 0.04f, 0.04f, 0.04f ),
            new Color( 0.1037f, 0.1037f, 0.1037f ),
            Square(defaultEmisColor),

            new Color( 0.02f, 0.02f, 0.02f ),
            new Color( 0.08f, 0.05f, 0.15f ),
            new Color( 0.0660f, 0.0660f, 0.0660f ),
            Square(alt1EmisColor),

            new Color( 0.1509f, 0.1509f, 0.1509f ),
            new Color( 0.02f, 0.02f, 0.02f ),
            new Color( 0.3373f, 0.3373f, 0.3373f ),
            Square(alt2EmisColor),

            new Color( 0.1266f, 0.1266f, 0.1266f ),
            new Color( 0f, 0.25f, 0f ),
            new Color( 0.1981f, 0.1118f, 0f ),
            Square(alt3EmisColor),

            new Color( 0.02f, 0.02f, 0.02f ),
            new Color( 0.2f, 0f, 0f ),
            new Color( 0.08f, 0f, 0f ),
            Square(alt4EmisColor),

            new Color( 0.75f, 0.5f, 0.19f ),
            Square(new Color( 0.32f, 0.67f, 0.19f )),
            new Color( 0.1f, 0.1f, 0.1f ),
            Square(alt5EmisColor),

            new Color( 1f, 1f, 1f ),
            new Color( 0.6f, 0.6f, 0.6f ),
            new Color( 0.2f, 0.2f, 0.2f ),
            Square(alt6EmisColor),

            new Color( 0.02f, 0.02f, 0.02f ),
            new Color( 0.08f, 0.05f, 0.15f ),
            new Color( 0.0660f, 0.0660f, 0.0660f ),
            Square(trashEmisColor),
        };

        internal static GameObject GetModel()
        {
            if( _model == null )
            {
                _model = CreateModel();
            }

            return _model;
        }

        private static GameObject _model;
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

            ITextureJob alt5IconJob = TexturesCore.GenerateCrossTextureBatch( 512, 512, 100, 20, 3, new Color( 0.9f, 0.9f, 0.9f ), new Color( 0.4f, 0.4f, 0.4f ),
                iconColors[ind++], iconColors[ind++], iconColors[ind++], iconColors[ind++] );

            ITextureJob alt6IconJob = TexturesCore.GenerateCrossTextureBatch( 512, 512, 100, 20, 3, new Color( 0.9f, 0.9f, 0.9f ), new Color( 0.4f, 0.4f, 0.4f ),
                iconColors[ind++], iconColors[ind++], iconColors[ind++], iconColors[ind++] );

            ITextureJob trashIconJob = TexturesCore.GenerateCrossTextureBatch( 512, 512, 100, 20, 3, new Color( 0.9f, 0.9f, 0.9f ), new Color( 0.4f, 0.4f, 0.4f ),
                iconColors[ind++], iconColors[ind++], iconColors[ind++], iconColors[ind++] );


            ITextureJob defaultRampJob = TexturesCore.GenerateRampTextureBatch( new[]
            {
                new GradientAlphaKey(0f, 0f),
                new GradientAlphaKey(0.1f, 0.5f),
                new GradientAlphaKey(1f, 1f),
            },
            new[]
            {
                new GradientColorKey( Color.black, 0f ),
                new GradientColorKey( defaultEmisColor, 0.95f ),
                new GradientColorKey( Color.white, 1f ),
            } );

            ITextureJob alt1RampJob = TexturesCore.GenerateRampTextureBatch( new[]
            {
                new GradientAlphaKey(0f, 0f),
                new GradientAlphaKey(0.1f, 0.5f),
                new GradientAlphaKey(1f, 1f),
            },
            new[]
            {
                new GradientColorKey( Color.black, 0f ),
                new GradientColorKey( alt1EmisColor, 0.95f ),
                new GradientColorKey( Color.white, 1f ),
            } );

            ITextureJob alt2RampJob = TexturesCore.GenerateRampTextureBatch( new[]
            {
                new GradientAlphaKey(0f, 0f),
                new GradientAlphaKey(0.1f, 0.5f),
                new GradientAlphaKey(1f, 1f),
            },
            new[]
            {
                new GradientColorKey( Color.black, 0f ),
                new GradientColorKey( alt2EmisColor, 0.95f ),
                new GradientColorKey( Color.white, 1f ),
            } );

            ITextureJob alt3RampJob = TexturesCore.GenerateRampTextureBatch( new[]
            {
                new GradientAlphaKey(0f, 0f),
                new GradientAlphaKey(0.1f, 0.5f),
                new GradientAlphaKey(1f, 1f),
            },
            new[]
            {
                new GradientColorKey( Color.black, 0f ),
                new GradientColorKey( alt3EmisColor, 0.95f ),
                new GradientColorKey( Color.white, 1f ),
            } );

            ITextureJob alt4RampJob = TexturesCore.GenerateRampTextureBatch( new[]
            {
                new GradientAlphaKey(0f, 0f),
                new GradientAlphaKey(0.1f, 0.5f),
                new GradientAlphaKey(1f, 1f),
            },
            new[]
            {
                new GradientColorKey( Color.black, 0f ),
                new GradientColorKey( alt4EmisColor, 0.95f ),
                new GradientColorKey( Color.white, 1f ),
            } );

            ITextureJob alt5RampJob = TexturesCore.GenerateRampTextureBatch( new[]
            {
                new GradientAlphaKey(0f, 0f),
                new GradientAlphaKey(0.1f, 0.5f),
                new GradientAlphaKey(1f, 1f),
            },
            new[]
            {
                new GradientColorKey( Color.black, 0f ),
                new GradientColorKey( alt5EmisColor, 0.95f ),
                new GradientColorKey( Color.white, 1f ),
            } );

            ITextureJob alt6RampJob = TexturesCore.GenerateRampTextureBatch( new[]
            {
                new GradientAlphaKey(0f, 0f),
                new GradientAlphaKey(0.1f, 0.5f),
                new GradientAlphaKey(1f, 1f),
            },
            new[]
            {
                new GradientColorKey( Color.black, 0f ),
                new GradientColorKey( alt6EmisColor, 0.95f ),
                new GradientColorKey( Color.white, 1f ),
            } );

            ITextureJob trashRampJob = TexturesCore.GenerateRampTextureBatch( new[]
            {
                new GradientAlphaKey(0f, 0f),
                new GradientAlphaKey(0.1f, 0.5f),
                new GradientAlphaKey(1f, 1f),
            },
            new[]
            {
                new GradientColorKey( Color.black, 0f ),
                new GradientColorKey( trashEmisColor, 0.95f ),
                new GradientColorKey( Color.white, 1f ),
            } );

            //JobHandle.ScheduleBatchedJobs();

            GameObject model = AssetModule.GetSniperAssetBundle().LoadAsset<GameObject>( Properties.Resources.prefab__SniperPrefab );
            CharacterModel charModel = model.GetComponent<CharacterModel>();

            for( Int32 i = 0; i < charModel.baseRendererInfos.Length; ++i )
            {
                CharacterModel.RendererInfo info = charModel.baseRendererInfos[i];
                charModel.baseRendererInfos[i] = info;
            }

            var defaultSkin = new SniperSkin
            (
                "Default",
                TextureModule.GetSniperTextures(),
                TextureModule.GetRailTextures(),
                TextureModule.GetThrowKnifeTextures(),
                TextureModule.GetSniperTextures(),
                defaultRampJob
            );
            defaultSkin.ApplyDefaultSkinModifiers();
            Texture2D defaultTex = defaultIconJob.OutputTextureAndDispose();
            defaultSkin.CreateAndAddSkin(charModel, Properties.Tokens.SNIPER_SKIN_DEFAULT_NAME, "",
                Sprite.Create(defaultTex, new Rect(0f, 0f, defaultTex.width, defaultTex.height), new Vector2(0.5f, 0.5f)));

            var alt1 = new SniperSkin
            (
                "Alt1",
                TextureModule.GetSniperAlt1Textures(),
                TextureModule.GetRailAlt1Textures(),
                TextureModule.GetThrowKnifeAlt1Textures(),
                TextureModule.GetSniperAlt1Textures(),
                alt1RampJob
            );
            alt1.ApplyAlt1SkinModifiers();
            Texture2D alt1Tex = alt1IconJob.OutputTextureAndDispose();
            alt1.CreateAndAddSkin(charModel, Properties.Tokens.SNIPER_SKIN_ALT1_NAME, "",
                Sprite.Create(alt1Tex, new Rect(0f, 0f, alt1Tex.width, alt1Tex.height), new Vector2(0.5f, 0.5f)));

            var alt2 = new SniperSkin
            (
                "Alt2",
                TextureModule.GetSniperAlt2Textures(),
                TextureModule.GetRailAlt2Textures(),
                TextureModule.GetThrowKnifeAlt2Textures(),
                TextureModule.GetSniperAlt2Textures(),
                alt2RampJob
            );
            alt2.ApplyAlt2SkinModifiers();
            Texture2D alt2Tex = alt2IconJob.OutputTextureAndDispose();
            alt2.CreateAndAddSkin(charModel, Properties.Tokens.SNIPER_SKIN_ALT2_NAME, "",
                Sprite.Create(alt2Tex, new Rect(0f, 0f, alt2Tex.width, alt2Tex.height), new Vector2(0.5f, 0.5f)));

            var alt3 = new SniperSkin
            (
                "Alt3",
                TextureModule.GetSniperAlt3Textures(),
                TextureModule.GetRailAlt3Textures(),
                TextureModule.GetThrowKnifeAlt3Textures(),
                TextureModule.GetSniperAlt3Textures(),
                alt3RampJob
            );
            alt3.ApplyAlt3SkinModifiers();
            Texture2D alt3Tex = alt3IconJob.OutputTextureAndDispose();
            alt3.CreateAndAddSkin(charModel, Properties.Tokens.SNIPER_SKIN_ALT3_NAME, "",
                Sprite.Create(alt3Tex, new Rect(0f, 0f, alt3Tex.width, alt3Tex.height), new Vector2(0.5f, 0.5f)));

            var alt4 = new SniperSkin
            (
                "Alt4",
                TextureModule.GetSniperAlt4Textures(),
                TextureModule.GetRailAlt4Textures(),
                TextureModule.GetThrowKnifeAlt4Textures(),
                TextureModule.GetSniperAlt4Textures(),
                alt4RampJob
            );
            alt4.ApplyAlt4SkinModifiers();
            Texture2D alt4Tex = alt4IconJob.OutputTextureAndDispose();
            alt4.CreateAndAddSkin(charModel, Properties.Tokens.SNIPER_SKIN_ALT4_NAME, "",
                Sprite.Create(alt4Tex, new Rect(0f, 0f, alt4Tex.width, alt4Tex.height), new Vector2(0.5f, 0.5f)));

            var alt5 = new SniperSkin
            (
                "Alt5",
                TextureModule.GetSniperAlt5Textures(),
                TextureModule.GetRailAlt5Textures(),
                TextureModule.GetThrowKnifeAlt5Textures(),
                TextureModule.GetSniperAlt5Textures(),
                alt5RampJob
            );
            alt5.ApplyAlt5SkinModifiers();
            Texture2D alt5Tex = alt5IconJob.OutputTextureAndDispose();
            alt5.CreateAndAddSkin(charModel, Properties.Tokens.SNIPER_SKIN_ALT5_NAME, "",
                Sprite.Create(alt5Tex, new Rect(0f, 0f, alt5Tex.width, alt5Tex.height), new Vector2(0.5f, 0.5f)));

            var alt6 = new SniperSkin
            (
                "Alt6",
                TextureModule.GetSniperAlt6Textures(),
                TextureModule.GetRailAlt6Textures(),
                TextureModule.GetThrowKnifeAlt6Textures(),
                TextureModule.GetSniperAlt6Textures(),
                alt6RampJob
            );
            alt6.ApplyAlt6SkinModifiers();
            Texture2D alt6Tex = alt6IconJob.OutputTextureAndDispose();
            alt6.CreateAndAddSkin(charModel, Properties.Tokens.SNIPER_SKIN_ALT6_NAME, "",
                Sprite.Create(alt6Tex, new Rect(0f, 0f, alt6Tex.width, alt6Tex.height), new Vector2(0.5f, 0.5f)));

            var trash = new SniperSkin
            (
                "Trash",
                TextureModule.GetSniperTrashTextures(),
                TextureModule.GetRailTrashTextures(),
                TextureModule.GetThrowKnifeTrashTextures(),
                TextureModule.GetSniperTrashTextures(),
                trashRampJob,
                true
            );
            trash.ApplyTrashSkinModifiers();

            Texture2D trashTex = trashIconJob.OutputTextureAndDispose();
            trash.CreateAndAddSkin(charModel, Properties.Tokens.SNIPER_SKIN_TRASH_NAME, "",
                Sprite.Create(trashTex, new Rect(0f, 0f, trashTex.width, trashTex.height), new Vector2(0.5f, 0.5f)));



























            var skinController = charModel.GetComponent<ModelSkinController>();
            //Array.Resize( ref skinController.skins, skinController.skins.Length + 5 );
            //for( Int32 i = skinController.skins.Length - 6; i < skinController.skins.Length; ++i )
            //{
            //    skinController.skins[i] = skinController.skins[0];
            //}
            SkinDef[] skinsArray = skinController.skins;

            CharacterModel.RendererInfo[] defaultSkinRenderers = skinsArray[0].rendererInfos;
            for( Int32 i = 0; i < defaultSkinRenderers.Length; ++i )
            {
                CharacterModel.RendererInfo info = charModel.baseRendererInfos[i];
                info.defaultMaterial = defaultSkinRenderers[i].defaultMaterial;
                charModel.baseRendererInfos[i] = info;
            }

            return model;
        }

        private static void ApplyDefaultSkinModifiers( this SniperSkin skin )
        {
            skin.AddMaterialModifier( SniperSkin.SniperMaterial.All, ( mat ) =>
            {
                mat.mainColor = Color.white;
                mat.cull = MaterialBase.CullMode.Back;
                mat.ignoreDiffuseAlphaForSpecular = true;
                mat.rampChoice = MaterialBase.RampInfo.TwoTone;
                mat.specularExponent = 2f;
                mat.emissionPower = 1f;
                mat.smoothness = 0f;
                mat.emissionColor = Color.clear;
                mat.flashColor = Color.clear;
            } );

            skin.AddMaterialModifier( SniperSkin.SniperMaterial.Armor, ( mat ) =>
            {
                mat.cull = MaterialBase.CullMode.Off;
                mat.emissionColor = defaultEmisColor;
                mat.smoothness = 0.0f;
                mat.emissionPower = 1.5f;
                mat.specularStrength = 0.2f;
                mat.specularExponent = 2f;
                mat.normalStrength = 0.4f;
            } );

            skin.AddMaterialModifier( SniperSkin.SniperMaterial.Ammo, ( mat ) =>
            {
                mat.rampChoice = MaterialBase.RampInfo.TwoTone;
                mat.smoothness = 0.5f;
                mat.normalStrength = 0.05f;
                mat.emissionColor = new Color( 0f, 1f, 0f, 1f );
                mat.emissionPower = 5f;
                mat.specularStrength = 0.75f;
                mat.specularExponent = 2f;
            } );

            skin.AddMaterialModifier( SniperSkin.SniperMaterial.Cloak, ( mat ) =>
            {
                mat.rampChoice = MaterialBase.RampInfo.TwoTone;
                mat.cull = MaterialBase.CullMode.Off;
                mat.emissionPower = 0f;
                mat.specularStrength = 0.05f;
                mat.normalStrength = 0.1f;
                mat.smoothness = 0f;
                mat.specularExponent = 1f;
            } );

            skin.AddMaterialModifier( SniperSkin.SniperMaterial.Body, ( mat ) =>
            {
                mat.mainColor = new Color( 0.4f, 0.4f, 0.4f );
                mat.emissionPower = 0f;
                mat.normalStrength = 0.4f;
                mat.smoothness = 0.1f;
                mat.specularStrength = 0.35f;
                mat.specularExponent = 2f;
            } );

            skin.AddMaterialModifier( SniperSkin.SniperMaterial.Emissive, ( mat ) =>
            {
                mat.mainColor = Color.clear;
                mat.rampChoice = MaterialBase.RampInfo.Unlitish;
                mat.normalStrength = 0f;
                mat.emissionColor = defaultEmisColor;
                mat.specularExponent = 1f;
                mat.specularStrength = 0f;
            } );

            skin.AddMaterialModifier( SniperSkin.SniperMaterial.Rail, ( mat ) =>
            {
                mat.normalStrength = 1f;
                mat.emissionColor = defaultEmisColor;
                mat.smoothness = 0.5f;
                mat.specularStrength = 0.15f;
                mat.specularExponent = 2f;
            } );


            skin.AddMaterialModifier( SniperSkin.SniperMaterial.ThrowKnife, ( mat ) =>
            {
                mat.emissionColor = defaultEmisColor;
                mat.emissionPower = 5f;
            } );
        }

        private static void ApplyAlt1SkinModifiers( this SniperSkin skin )
        {
            skin.AddMaterialModifier( SniperSkin.SniperMaterial.All, ( mat ) =>
            {
                mat.mainColor = Color.white;
                mat.cull = MaterialBase.CullMode.Back;
                mat.ignoreDiffuseAlphaForSpecular = true;
                mat.rampChoice = MaterialBase.RampInfo.TwoTone;
                mat.specularExponent = 3f;
                mat.emissionPower = 1f;
                mat.smoothness = 0f;
                mat.emissionColor = Color.clear;
            } );

            skin.AddMaterialModifier( SniperSkin.SniperMaterial.Armor, ( mat ) =>
            {
                mat.mainColor = new Color( 0.2f, 0.4f, 0.53f, 1f );
                mat.emissionColor = Color.white;
                mat.emissionPower = 2f;
                mat.smoothness = 0.5f;
                mat.rampChoice = MaterialBase.RampInfo.TwoTone;
                mat.specularStrength = 0.3f;
                mat.specularExponent = 2f;
                mat.cull = MaterialBase.CullMode.Off;
                mat.normalStrength = 1f;
            } );

            skin.AddMaterialModifier( SniperSkin.SniperMaterial.Ammo, ( mat ) =>
            {
                mat.rampChoice = MaterialBase.RampInfo.TwoTone;
                mat.smoothness = 0.5f;
                mat.normalStrength = 0.05f;
                mat.emissionColor = Color.white;
                mat.emissionPower = 5f;
                mat.specularStrength = 0.75f;
                mat.specularExponent = 2f;
            } );

            skin.AddMaterialModifier( SniperSkin.SniperMaterial.Cloak, ( mat ) =>
            {
                mat.mainColor = new Color( 0.2f, 0.4f, 0.4f, 1f );
                mat.rampChoice = MaterialBase.RampInfo.TwoTone;
                mat.cull = MaterialBase.CullMode.Off;
                mat.emissionPower = 0f;
                mat.smoothness = 0.2f;
                mat.specularStrength = 0.2f;
                mat.normalStrength = 0.05f;
                mat.specularExponent = 2f;
            } );

            skin.AddMaterialModifier( SniperSkin.SniperMaterial.Body, ( mat ) =>
            {
                mat.mainColor = Color.black;
                mat.emissionPower = 0f;
                mat.normalStrength = 0.3f;
                mat.smoothness = 0.5f;
                mat.specularStrength = 0.2f;
                mat.rampChoice = MaterialBase.RampInfo.TwoTone;
                mat.specularExponent = 2f;
            } );

            skin.AddMaterialModifier( SniperSkin.SniperMaterial.Emissive, ( mat ) =>
            {
                mat.rampChoice = MaterialBase.RampInfo.Unlitish;
                mat.normalStrength = 0f;
                mat.emissionColor = Color.white;
                mat.specularExponent = 1f;
                mat.specularStrength = 0f;
                mat.emissionPower = 3.5f;
            } );

            skin.AddMaterialModifier( SniperSkin.SniperMaterial.Rail, ( mat ) =>
            {
                mat.mainColor = new Color( 0.55f, 0.55f, 0.55f, 1f );
                mat.normalStrength = 1f;
                mat.emissionColor = alt1EmisColor;
                mat.emissionPower = 3f;
                mat.smoothness = 0.3f;
                mat.specularStrength = 0.3f;
                mat.specularExponent = 2f;
            } );

            skin.AddMaterialModifier( SniperSkin.SniperMaterial.ThrowKnife, ( mat ) =>
            {
                mat.emissionColor = alt1EmisColor;
                mat.emissionPower = 5f;
                mat.normalStrength = 1f;
                mat.smoothness = 1f;
                mat.specularStrength = 0.35f;
                mat.specularExponent = 2f;
            } );

            skin.AddMaterialModifier( SniperSkin.SniperMaterial.Knife, ( mat ) =>
            {
                mat.mainColor = new Color( 0.55f, 0.55f, 0.55f );
                mat.normalStrength = 1f;
                mat.smoothness = 1f;
                mat.specularStrength = 0.35f;
                mat.specularExponent = 2f;
            } );
        }

        private static void ApplyAlt2SkinModifiers( this SniperSkin skin )
        {
            skin.AddMaterialModifier( SniperSkin.SniperMaterial.All, ( mat ) =>
            {
                mat.mainColor = Color.white;
                mat.cull = MaterialBase.CullMode.Back;
                mat.ignoreDiffuseAlphaForSpecular = true;
                mat.rampChoice = MaterialBase.RampInfo.TwoTone;
                mat.specularExponent = 3f;
                mat.emissionPower = 1f;
                mat.smoothness = 0f;
                mat.emissionColor = Color.clear;
            } );

            skin.AddMaterialModifier( SniperSkin.SniperMaterial.Armor, ( mat ) =>
            {
                mat.mainColor = new Color( 0.6f, 0.6f, 0.6f, 1f );
                mat.emissionColor = Color.white;
                mat.emissionPower = 5f;
                mat.smoothness = 0f;
                mat.rampChoice = MaterialBase.RampInfo.TwoTone;
                mat.specularStrength = 0.1f;
                mat.specularExponent = 1f;
                mat.cull = MaterialBase.CullMode.Off;
                mat.normalStrength = 1f;
            } );

            skin.AddMaterialModifier( SniperSkin.SniperMaterial.Ammo, ( mat ) =>
            {
                mat.rampChoice = MaterialBase.RampInfo.TwoTone;
                mat.smoothness = 0.5f;
                mat.normalStrength = 0.05f;
                mat.emissionColor = Color.white;
                mat.emissionPower = 5f;
                mat.specularStrength = 0.75f;
                mat.specularExponent = 2f;
            } );

            skin.AddMaterialModifier( SniperSkin.SniperMaterial.Cloak, ( mat ) =>
            {
                mat.mainColor = new Color( 0.6f, 0.6f, 0.6f, 1f );
                mat.rampChoice = MaterialBase.RampInfo.TwoTone;
                mat.cull = MaterialBase.CullMode.Off;
                mat.emissionPower = 0f;
                mat.smoothness = 0.15f;
                mat.specularStrength = 0f;
                mat.normalStrength = 0.1f;
                mat.specularExponent = 2f;
            } );

            skin.AddMaterialModifier( SniperSkin.SniperMaterial.Body, ( mat ) =>
            {
                mat.mainColor = new Color( 0.6f, 0.6f, 0.6f, 1f );
                mat.emissionPower = 0f;
                mat.normalStrength = 0.35f;
                mat.smoothness = 0.1f;
                mat.specularStrength = 0.1f;
                mat.rampChoice = MaterialBase.RampInfo.TwoTone;
                mat.specularExponent = 3f;
            } );

            skin.AddMaterialModifier( SniperSkin.SniperMaterial.Emissive, ( mat ) =>
            {
                mat.rampChoice = MaterialBase.RampInfo.Unlitish;
                mat.normalStrength = 0f;
                mat.emissionColor = Color.white;
                mat.specularExponent = 1f;
                mat.specularStrength = 0f;
                mat.emissionPower = 10f;
            } );

            skin.AddMaterialModifier( SniperSkin.SniperMaterial.Rail, ( mat ) =>
            {
                mat.mainColor = Color.white;
                mat.normalStrength = 1f;
                mat.emissionColor = alt2EmisColor;
                mat.emissionPower = 2f;
                mat.smoothness = 0f;
                mat.specularStrength = 0.3f;
                mat.specularExponent = 2f;
            } );

            skin.AddMaterialModifier( SniperSkin.SniperMaterial.ThrowKnife, ( mat ) =>
            {
                mat.emissionColor = alt2EmisColor;
                mat.emissionPower = 5f;
                mat.normalStrength = 1f;
                mat.smoothness = 1f;
                mat.specularStrength = 0.5f;
                mat.specularExponent = 2f;
            } );

            skin.AddMaterialModifier( SniperSkin.SniperMaterial.Knife, ( mat ) =>
            {
                mat.mainColor = Color.black;
                mat.normalStrength = 1f;
                mat.smoothness = 1f;
                mat.specularStrength = 0.5f;
                mat.specularExponent = 2f;
            } );
        }

        private static void ApplyAlt3SkinModifiers( this SniperSkin skin )
        {
            skin.AddMaterialModifier( SniperSkin.SniperMaterial.All, ( mat ) =>
            {
                mat.mainColor = Color.white;
                mat.cull = MaterialBase.CullMode.Back;
                mat.ignoreDiffuseAlphaForSpecular = true;
                mat.rampChoice = MaterialBase.RampInfo.TwoTone;
                mat.specularExponent = 3f;
                mat.emissionPower = 1f;
                mat.smoothness = 0f;
                mat.emissionColor = Color.clear;
            } );

            skin.AddMaterialModifier( SniperSkin.SniperMaterial.Armor, ( mat ) =>
            {
                mat.mainColor = Color.white;
                mat.emissionColor = alt3EmisColor;
                mat.emissionPower = 4f;
                mat.smoothness = 0f;
                mat.rampChoice = MaterialBase.RampInfo.TwoTone;
                mat.specularStrength = 0.2f;
                mat.specularExponent = 2f;
                mat.cull = MaterialBase.CullMode.Off;
                mat.normalStrength = 1f;
            } );

            skin.AddMaterialModifier( SniperSkin.SniperMaterial.Ammo, ( mat ) =>
            {
                mat.rampChoice = MaterialBase.RampInfo.TwoTone;
                mat.smoothness = 0.5f;
                mat.normalStrength = 0.05f;
                mat.emissionColor = Color.white;
                mat.emissionPower = 5f;
                mat.specularStrength = 0.75f;
                mat.specularExponent = 2f;
            } );

            skin.AddMaterialModifier( SniperSkin.SniperMaterial.Cloak, ( mat ) =>
            {
                mat.mainColor = Color.white;
                mat.rampChoice = MaterialBase.RampInfo.TwoTone;
                mat.cull = MaterialBase.CullMode.Off;
                mat.emissionPower = 0f;
                mat.smoothness = 0f;
                mat.specularStrength = 0.1f;
                mat.normalStrength = 0.05f;
                mat.specularExponent = 2f;
            } );

            skin.AddMaterialModifier( SniperSkin.SniperMaterial.Body, ( mat ) =>
            {
                mat.mainColor = new Color( 1f, 0.7f, 0.46f );
                mat.emissionPower = 0f;
                mat.normalStrength = 1f;
                mat.smoothness = 0.1f;
                mat.specularStrength = 0.1f;
                mat.rampChoice = MaterialBase.RampInfo.TwoTone;
                mat.specularExponent = 2f;
            } );

            skin.AddMaterialModifier( SniperSkin.SniperMaterial.Emissive, ( mat ) =>
            {
                mat.rampChoice = MaterialBase.RampInfo.Unlitish;
                mat.normalStrength = 0f;
                mat.emissionColor = alt3EmisColor;
                mat.specularExponent = 1f;
                mat.specularStrength = 0f;
                mat.emissionPower = 4f;
            } );

            skin.AddMaterialModifier( SniperSkin.SniperMaterial.Rail, ( mat ) =>
            {
                mat.mainColor = Color.white;
                mat.normalStrength = 1f;
                mat.emissionColor = alt3EmisColor;
                mat.emissionPower = 4f;
                mat.smoothness = 0.2f;
                mat.specularStrength = 0.2f;
                mat.specularExponent = 2f;
            } );

            skin.AddMaterialModifier( SniperSkin.SniperMaterial.ThrowKnife, ( mat ) =>
            {
                mat.emissionColor = alt3EmisColor;
                mat.emissionPower = 5f;
                mat.normalStrength = 1f;
                mat.smoothness = 1f;
                mat.specularStrength = 0.5f;
                mat.specularExponent = 2f;
            } );

            skin.AddMaterialModifier( SniperSkin.SniperMaterial.Knife, ( mat ) =>
            {
                mat.mainColor = Color.white;
                mat.normalStrength = 1f;
                mat.smoothness = 1f;
                mat.specularStrength = 0.3f;
                mat.specularExponent = 2f;
            } );
        }

        private static void ApplyAlt4SkinModifiers( this SniperSkin skin )
        {
            skin.AddMaterialModifier( SniperSkin.SniperMaterial.All, ( mat ) =>
            {
                mat.mainColor = Color.white;
                mat.cull = MaterialBase.CullMode.Back;
                mat.ignoreDiffuseAlphaForSpecular = true;
                mat.rampChoice = MaterialBase.RampInfo.TwoTone;
                mat.specularExponent = 3f;
                mat.emissionPower = 1f;
                mat.smoothness = 0f;
                mat.emissionColor = Color.clear;
            } );

            skin.AddMaterialModifier( SniperSkin.SniperMaterial.Armor, ( mat ) =>
            {
                mat.cull = MaterialBase.CullMode.Off;
                mat.mainColor = new Color( 0.6f, 0.32f, 0.32f );
                mat.emissionColor = Color.white;
                mat.emissionPower = 2f;
                mat.smoothness = 0f;
                mat.normalStrength = 1f;
                mat.specularStrength = 0.1f;
                mat.specularExponent = 1f;
            } );

            skin.AddMaterialModifier( SniperSkin.SniperMaterial.Ammo, ( mat ) =>
            {
                mat.rampChoice = MaterialBase.RampInfo.TwoTone;
                mat.smoothness = 0.5f;
                mat.normalStrength = 0.05f;
                mat.emissionColor = Color.white;
                mat.emissionPower = 5f;
                mat.specularStrength = 0.75f;
                mat.specularExponent = 2f;
            } );

            skin.AddMaterialModifier( SniperSkin.SniperMaterial.Cloak, ( mat ) =>
            {
                mat.rampChoice = MaterialBase.RampInfo.TwoTone;
                mat.cull = MaterialBase.CullMode.Off;
                mat.emissionPower = 0f;
                mat.specularStrength = 0.1f;
                mat.specularExponent = 2f;
                mat.normalStrength = 0.05f;
                mat.smoothness = 0f;
                mat.mainColor = new Color( 0.6f, 0.32f, 0.32f );

            } );

            skin.AddMaterialModifier( SniperSkin.SniperMaterial.Body, ( mat ) =>
            {
                mat.mainColor = new Color( 1f, 0f, 0f );
                mat.emissionPower = 0f;
                mat.normalStrength = 0.2f;
                mat.smoothness = 0f;
                mat.specularStrength = 0.1f;
                mat.specularExponent = 2f;
                mat.rampChoice = MaterialBase.RampInfo.TwoTone;
            } );

            skin.AddMaterialModifier( SniperSkin.SniperMaterial.Emissive, ( mat ) =>
            {
                mat.rampChoice = MaterialBase.RampInfo.Unlitish;
                mat.normalStrength = 0f;
                mat.emissionColor = Color.white;
                mat.specularExponent = 1f;
                mat.emissionPower = 10f;
                mat.specularStrength = 0f;
            } );

            skin.AddMaterialModifier( SniperSkin.SniperMaterial.Rail, ( mat ) =>
            {
                mat.normalStrength = 1f;
                mat.emissionColor = alt4EmisColor;
                mat.smoothness = 0.5f;
                mat.specularStrength = 0.2f;
                mat.specularExponent = 2f;
                mat.emissionPower = 2f;
                mat.mainColor = new Color( 0.8f, 0.8f, 0.8f, 1f );
            } );

            skin.AddMaterialModifier( SniperSkin.SniperMaterial.ThrowKnife, ( mat ) =>
            {
                mat.emissionColor = alt4EmisColor;
                mat.emissionPower = 5f;
                mat.normalStrength = 1f;
                mat.smoothness = 1f;
                mat.specularStrength = 0.5f;
                mat.specularExponent = 2f;
            } );

            skin.AddMaterialModifier( SniperSkin.SniperMaterial.Knife, ( mat ) =>
            {
                mat.mainColor = new Color( 0.4f, 0.4f, 0.4f );
                mat.normalStrength = 1f;
                mat.smoothness = 1f;
                mat.specularStrength = 0.1f;
                mat.specularExponent = 2f;
            } );
        }

        private static void ApplyAlt5SkinModifiers( this SniperSkin skin )
        {
            skin.AddMaterialModifier( SniperSkin.SniperMaterial.All, ( mat ) =>
            {
                mat.mainColor = Color.white;
                mat.cull = MaterialBase.CullMode.Back;
                mat.ignoreDiffuseAlphaForSpecular = true;
                mat.rampChoice = MaterialBase.RampInfo.TwoTone;
                mat.specularExponent = 3f;
                mat.emissionPower = 1f;
                mat.smoothness = 0f;
                mat.emissionColor = Color.clear;
            } );

            skin.AddMaterialModifier( SniperSkin.SniperMaterial.Armor, ( mat ) =>
            {
                mat.mainColor = Color.white;
                mat.emissionColor = alt5EmisColor;
                mat.emissionPower = 3f;
                mat.smoothness = 0f;
                mat.rampChoice = MaterialBase.RampInfo.TwoTone;
                mat.specularStrength = 0.1f;
                mat.specularExponent = 2f;
                mat.cull = MaterialBase.CullMode.Off;
                mat.normalStrength = 1f;
            } );

            skin.AddMaterialModifier( SniperSkin.SniperMaterial.Ammo, ( mat ) =>
            {
                
                mat.rampChoice = MaterialBase.RampInfo.TwoTone;
                mat.smoothness = 0.5f;
                mat.normalStrength = 0.05f;
                mat.emissionColor = Color.white;
                mat.emissionPower = 5f;
                mat.specularStrength = 0.75f;
                mat.specularExponent = 2f;
            } );

            skin.AddMaterialModifier( SniperSkin.SniperMaterial.Cloak, ( mat ) =>
            {
                mat.mainColor = Color.white;
                mat.rampChoice = MaterialBase.RampInfo.TwoTone;
                mat.cull = MaterialBase.CullMode.Off;
                mat.emissionPower = 0f;
                mat.smoothness = 0f;
                mat.specularStrength = 0.2f;
                mat.normalStrength = 0.05f;
                mat.specularExponent = 1.5f;
            } );

            skin.AddMaterialModifier( SniperSkin.SniperMaterial.Body, ( mat ) =>
            {
                mat.mainColor = new Color( 0.6f, 0.6f, 0.6f );
                mat.emissionPower = 0f;
                mat.normalStrength = 0.1f;
                mat.smoothness = 0f;
                mat.specularStrength = 0.2f;
                mat.rampChoice = MaterialBase.RampInfo.TwoTone;
                mat.specularExponent = 2f;
            } );

            skin.AddMaterialModifier( SniperSkin.SniperMaterial.Emissive, ( mat ) =>
            {
                mat.rampChoice = MaterialBase.RampInfo.Unlitish;
                mat.normalStrength = 0f;
                mat.emissionColor = alt5EmisColor;
                mat.specularExponent = 1f;
                mat.specularStrength = 0f;
                mat.emissionPower = 4f;
            } );

            skin.AddMaterialModifier( SniperSkin.SniperMaterial.Rail, ( mat ) =>
            {
                mat.mainColor = Color.white;
                mat.normalStrength = 1f;
                mat.emissionColor = alt5EmisColor;
                mat.emissionPower = 2f;
                mat.smoothness = 0.3f;
                mat.specularStrength = 0.2f;
                mat.specularExponent = 2f;
            } );

            skin.AddMaterialModifier( SniperSkin.SniperMaterial.ThrowKnife, ( mat ) =>
            {
                mat.emissionColor = alt5EmisColor;
                mat.emissionPower = 5f;
                mat.normalStrength = 1f;
                mat.smoothness = 1f;
                mat.specularStrength = 0.5f;
                mat.specularExponent = 2f;
            } );

            skin.AddMaterialModifier( SniperSkin.SniperMaterial.Knife, ( mat ) =>
            {
                mat.mainColor = Color.white;
                mat.normalStrength = 1f;
                mat.smoothness = 1f;
                mat.specularStrength = 0.35f;
                mat.specularExponent = 2f;
            } );
        }

        private static void ApplyAlt6SkinModifiers( this SniperSkin skin )
        {
            skin.AddMaterialModifier( SniperSkin.SniperMaterial.All, ( mat ) =>
            {
                mat.mainColor = Color.white;
                mat.cull = MaterialBase.CullMode.Back;
                mat.ignoreDiffuseAlphaForSpecular = true;
                mat.rampChoice = MaterialBase.RampInfo.TwoTone;
                mat.specularExponent = 3f;
                mat.emissionPower = 1f;
                mat.smoothness = 0f;
                mat.emissionColor = Color.clear;
            } );

            skin.AddMaterialModifier( SniperSkin.SniperMaterial.Armor, ( mat ) =>
            {
                mat.mainColor = new Color(0.86f, 0.9f, 1f, 1f);
                mat.emissionColor = Color.white;
                mat.emissionPower = 2.5f;
                mat.smoothness = 0f;
                mat.rampChoice = MaterialBase.RampInfo.TwoTone;
                mat.specularStrength = 0.15f;
                mat.specularExponent = 2f;
                mat.cull = MaterialBase.CullMode.Off;
                mat.normalStrength = 1f;
            } );

            skin.AddMaterialModifier( SniperSkin.SniperMaterial.Ammo, ( mat ) =>
            {
                mat.rampChoice = MaterialBase.RampInfo.TwoTone;
                mat.smoothness = 0.5f;
                mat.normalStrength = 0.05f;
                mat.emissionColor = Color.white;
                mat.emissionPower = 5f;
                mat.specularStrength = 0.75f;
                mat.specularExponent = 2f;
            } );

            skin.AddMaterialModifier( SniperSkin.SniperMaterial.Cloak, ( mat ) =>
            {
                mat.mainColor = new Color(0.86f, 0.9f, 1f, 1f);
                mat.rampChoice = MaterialBase.RampInfo.TwoTone;
                mat.cull = MaterialBase.CullMode.Off;
                mat.emissionPower = 0f;
                mat.smoothness = 0f;
                mat.specularStrength = 0.15f;
                mat.normalStrength = 1f;
                mat.specularExponent = 2f;
            } );

            skin.AddMaterialModifier( SniperSkin.SniperMaterial.Body, ( mat ) =>
            {
                mat.mainColor = Color.white;
                mat.emissionPower = 0f;
                mat.normalStrength = 0.5f;
                mat.smoothness = 0f;
                mat.specularStrength = 0.1f;
                mat.rampChoice = MaterialBase.RampInfo.TwoTone;
                mat.specularExponent = 1.25f;
            } );

            skin.AddMaterialModifier( SniperSkin.SniperMaterial.Emissive, ( mat ) =>
            {
                mat.rampChoice = MaterialBase.RampInfo.Unlitish;
                mat.normalStrength = 0f;
                mat.emissionColor = Color.white;
                mat.specularExponent = 1f;
                mat.specularStrength = 0f;
                mat.emissionPower = 4f;
            } );

            skin.AddMaterialModifier( SniperSkin.SniperMaterial.Rail, ( mat ) =>
            {
                mat.mainColor = Color.white;
                mat.normalStrength = 1f;
                mat.emissionColor = Color.white;
                mat.emissionPower = 2f;
                mat.smoothness = 0.2f;
                mat.specularStrength = 0.4f;
                mat.specularExponent = 2f;
            } );

            skin.AddMaterialModifier( SniperSkin.SniperMaterial.ThrowKnife, ( mat ) =>
            {
                mat.emissionColor = alt6EmisColor;
                mat.emissionPower = 5f;
                mat.normalStrength = 1f;
                mat.smoothness = 1f;
                mat.specularStrength = 0.5f;
                mat.specularExponent = 2f;
            } );

            skin.AddMaterialModifier( SniperSkin.SniperMaterial.Knife, ( mat ) =>
            {
                mat.mainColor = Color.white;
                mat.normalStrength = 1f;
                mat.smoothness = 1f;
                mat.specularStrength = 0.9f;
                mat.specularExponent = 2f;
            } );
        }

        private static void ApplyTrashSkinModifiers(this SniperSkin skin)
        {
            skin.AddMaterialModifier(SniperSkin.SniperMaterial.All, (mat) =>
            {
                mat.mainColor = Color.white;
                mat.cull = MaterialBase.CullMode.Back;
                mat.ignoreDiffuseAlphaForSpecular = true;
                mat.rampChoice = MaterialBase.RampInfo.TwoTone;
                mat.specularExponent = 3f;
                mat.emissionPower = 1f;
                mat.smoothness = 0f;
                mat.emissionColor = Color.clear;
            });

            skin.AddMaterialModifier(SniperSkin.SniperMaterial.Armor, (mat) =>
            {
                mat.mainColor = new Color(0.83529411764705882352941176470588f, 0.76078431372549019607843137254902f, 0.53725490196078431372549019607843f, 1f);
                mat.emissionColor = new Color(0.92f, 0.9f, 0.92f, 1f);
                mat.emissionPower = 3f;
                mat.smoothness = 0.6f;
                mat.rampChoice = MaterialBase.RampInfo.TwoTone;
                mat.specularStrength = 0.075f;
                mat.specularExponent = 0.1f;
                mat.cull = MaterialBase.CullMode.Off;
                mat.normalStrength = 1f;
            });

            skin.AddMaterialModifier(SniperSkin.SniperMaterial.Rail, (mat) =>
            {
                mat.mainColor = new Color(0.69411764705882352941176470588235f, 0.69411764705882352941176470588235f, 0.50980392156862745098039215686275f, 1f);
                mat.normalStrength = 1f;
                mat.emissionColor = new Color(0.65098039215686274509803921568627f, 1f, 0.96470588235294117647058823529412f, 1f);
                mat.emissionPower = 3f;
                mat.smoothness = 0.3f;
                mat.specularStrength = 0.15f;
                mat.specularExponent = 0.3f;
                mat.cull = MaterialBase.CullMode.Off;
            });

            skin.AddMaterialModifier(SniperSkin.SniperMaterial.ThrowKnife, (mat) =>
            {
                mat.mainColor = Color.white;
                mat.emissionColor = Color.white;
                mat.emissionPower = 2f;
                mat.normalStrength = 0.5f;
                mat.smoothness = 1f;
                mat.specularStrength = 0.35f;
                mat.specularExponent = 2f;
            });
        }
    }
}
