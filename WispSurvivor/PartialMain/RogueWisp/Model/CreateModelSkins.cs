using BepInEx;
using R2API.Utils;
using RoR2;
using RoR2.Networking;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using RogueWispPlugin.Helpers;
using RogueWispPlugin.Modules;
using static RogueWispPlugin.Helpers.APIInterface;

namespace RogueWispPlugin
{
    internal partial class Main
    {
        partial void RW_CreateModelSkins()
        {
            this.Load += this.RW_DoSkinCreation;
        }

        private void RW_DoSkinCreation()
        {
            GameObject bodyModel = this.RW_body.GetComponent<ModelLocator>().modelTransform.gameObject;
            CharacterModel bodyCharModel = bodyModel.GetComponent<CharacterModel>();
            ModelSkinController bodySkins = bodyModel.AddOrGetComponent<ModelSkinController>();

            Renderer armorRenderer = bodyCharModel.baseRendererInfos[0].renderer;
            bodyCharModel.baseRendererInfos[0].defaultMaterial = Main.armorMaterials[0];

            bodyCharModel.baseRendererInfos[0].ignoreOverlays = false;



            CharacterModel.ParticleSystemInfo[] particles = bodyCharModel.baseParticleSystemInfos;


            for( Int32 i = 0; i < particles.Length; i++ )
            {
                particles[i].renderer.material = Main.fireMaterials[0][0];
                particles[i].defaultMaterial = Main.fireMaterials[0][0];
            }

            armorRenderer.material = Main.armorMaterials[0];

            CharacterModel.RendererInfo[][] rendererInfos = new CharacterModel.RendererInfo[8][];
            for( Int32 i = 0; i < 8; i++ )
            {
                rendererInfos[i] = new CharacterModel.RendererInfo[particles.Length + 1];
                for( Int32 j = 0; j < particles.Length; j++ )
                {
                    rendererInfos[i][j] = CreateFlameRendererInfo( particles[j].renderer, Main.fireMaterials[i][0] );
                }
                // TODO: Array of armor mats should be reffed here and used
                rendererInfos[i][particles.Length] = new CharacterModel.RendererInfo
                {
                    renderer = armorRenderer,
                    defaultMaterial = Main.armorMaterials[i],
                    defaultShadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On,
                    ignoreOverlays = false
                };
            }

            String[] skinNames = new String[8];
            skinNames[0] = "WISP_SURVIVOR_SKIN_1";
            skinNames[1] = "WISP_SURVIVOR_SKIN_2";
            skinNames[2] = "WISP_SURVIVOR_SKIN_3";
            skinNames[3] = "WISP_SURVIVOR_SKIN_4";
            skinNames[4] = "WISP_SURVIVOR_SKIN_5";
            skinNames[5] = "WISP_SURVIVOR_SKIN_6";
            skinNames[6] = "WISP_SURVIVOR_SKIN_7";
            skinNames[7] = "WISP_SURVIVOR_SKIN_8";

            SkinDef[] skins = new SkinDef[8];

            for( Int32 i = 0; i < 8; i++ )
            {
                R2API.SkinAPI.SkinDefInfo skinInfo = new R2API.SkinAPI.SkinDefInfo
                {
                    baseSkins = Array.Empty<SkinDef>(),
                    icon = Resources.Load<Sprite>("NotAPath"),
                    nameToken = skinNames[i],
                    name = skinNames[i],
                    unlockableName = "",
                    rootObject = bodyModel,
                    rendererInfos = rendererInfos[i]
                };
                skins[i] = CreateNewSkinDef( skinInfo );
            }

            bodySkins.skins = skins;
        }

        private static CharacterModel.RendererInfo CreateFlameRendererInfo( Renderer r, Material m ) => CreateRendererInfo( r, m, true, UnityEngine.Rendering.ShadowCastingMode.On );

    }

}
