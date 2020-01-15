using BepInEx;
using RoR2;
using UnityEngine;
using R2API;
using R2API.Utils;
using System.Reflection;
using EntityStates;
using RoR2.Skills;
using System;

namespace ReinSniperRework
{
    internal partial class Main
    {
        partial void Model()
        {
            this.Load += this.EditModelSkins;
        }

        private void EditModelSkins()
        {
            var model = this.sniperBody.GetComponent<ModelLocator>().modelTransform;
            var charModel = model.GetComponent<CharacterModel>();
            var modelSkins = model.GetComponent<ModelSkinController>();

            var baseSkin = modelSkins.skins[0];

            var skinDef = SkinAPI.CreateNewSkinDef( new SkinAPI.SkinDefInfo() );

            skinDef.baseSkins = Array.Empty<SkinDef>();
            skinDef.gameObjectActivations = Array.Empty<SkinDef.GameObjectActivation>();
            skinDef.icon = Resources.Load<Sprite>( "NotAPath" ); // TODO: Grab skin icon generator and implement
            skinDef.rendererInfos = new CharacterModel.RendererInfo[3]
            {
                new CharacterModel.RendererInfo
                {
                    defaultMaterial = this.sniperPistolMaterial,
                    defaultShadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On,
                    ignoreOverlays = false,
                    renderer = baseSkin.rendererInfos[0].renderer
                },
                new CharacterModel.RendererInfo
                {
                    defaultMaterial = this.sniperRifleMaterial,
                    defaultShadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On,
                    ignoreOverlays = false,
                    renderer = baseSkin.rendererInfos[1].renderer
                },
                new CharacterModel.RendererInfo
                {
                    defaultMaterial = this.sniperBodyMaterial,
                    defaultShadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On,
                    ignoreOverlays = false,
                    renderer = baseSkin.rendererInfos[2].renderer
                }
            };
            skinDef.meshReplacements = new SkinDef.MeshReplacement[3]
            {
                new SkinDef.MeshReplacement
                {
                    mesh = this.sniperBodyMesh,
                    renderer = baseSkin.meshReplacements[0].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = this.sniperRifleMesh,
                    renderer = baseSkin.rendererInfos[0].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = this.knifeMesh3,
                    renderer = baseSkin.rendererInfos[1].renderer
                }
            };
            skinDef.rootObject = baseSkin.rootObject;
            skinDef.unlockableName = "";

            modelSkins.skins = new SkinDef[1]
            {
                skinDef
            };
        }
    }
}


