using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BepInEx.Logging;
using ReinCore;
using RoR2;
using UnityEngine;
using Sniper.Properties;

namespace Sniper.Modules
{
    internal static class ModelModule
    {
        internal static GameObject GetModel()
        {
            GameObject model = AssetModule.GetSniperAssetBundle().LoadAsset<GameObject>( Properties.Resources.SniperPrefabPath );
            var charModel = model.GetComponent<CharacterModel>();

            for( Int32 i = 0; i < charModel.baseRendererInfos.Length; ++i )
            {
                var info = charModel.baseRendererInfos[i];
                var smr = info.renderer;
                Material mat = null;
                switch( smr.name )
                {
                    default:
                    Log.Warning( String.Format( "{0} is not a handled renderer name", smr.name ) );
                    break;

                    case "SniperMesh":
                    //
                    break;

                    case "ThrowKnife":
                    mat = MaterialModule.GenerateArmorMaterial( MaterialModule.GetSniperClassicBase() );
                    break;

                    case "Knife":
                    mat = MaterialModule.GenerateArmorMaterial( MaterialModule.GetSniperClassicBase() );
                    break;

                    case "RailGun":
                    mat = MaterialModule.GetRailDefault();
                    break;

                    case "GaussGun":
                    mat = null;
                    break;

                    case "AmmoMesh":
                    mat = MaterialModule.GenerateAmmoMaterial( MaterialModule.GetSniperClassicBase() );
                    break;

                    case "ArmorMesh":
                    mat = MaterialModule.GenerateArmorMaterial( MaterialModule.GetSniperClassicBase() );
                    break;

                    case "BodyMesh":
                    mat = MaterialModule.GenerateBodyMaterial( MaterialModule.GetSniperClassicBase() );
                    break;

                    case "CloakMesh":
                    mat = MaterialModule.GenerateCloakMaterial( MaterialModule.GetSniperClassicBase() );
                    break;

                    case "EmissionMesh":
                    mat = MaterialModule.GenerateEmissionMaterial( MaterialModule.GetSniperClassicBase() );
                    break;
                }

                info.defaultMaterial = mat;
                charModel.baseRendererInfos[i] = info;
            }

            foreach( SkinnedMeshRenderer smr in model.GetComponentsInChildren<SkinnedMeshRenderer>() )
            {
                switch( smr.name )
                {
                    default:
                    Log.Warning( String.Format( "{0} is not a handled renderer name", smr.name ) );
                    break;

                    case "SniperMesh":
                    smr.sharedMaterial = null;
                    break;

                    case "ThrowKnife":
                    break;

                    case "Knife":
                    break;

                    case "RailGun":
                    smr.sharedMaterial = MaterialModule.GetRailDefault();
                    break;

                    case "GaussGun":
                    smr.sharedMaterial = null;
                    break;

                    case "AmmoMesh":
                    smr.sharedMaterial = MaterialModule.GenerateAmmoMaterial( MaterialModule.GetSniperClassicBase() );
                    break;

                    case "ArmorMesh":
                    smr.sharedMaterial = MaterialModule.GenerateArmorMaterial( MaterialModule.GetSniperClassicBase() );
                    break;

                    case "BodyMesh":
                    smr.sharedMaterial = MaterialModule.GenerateBodyMaterial( MaterialModule.GetSniperClassicBase() );
                    break;

                    case "CloakMesh":
                    smr.sharedMaterial = MaterialModule.GenerateCloakMaterial( MaterialModule.GetSniperClassicBase() );
                    break;

                    case "EmissionMesh":
                    smr.sharedMaterial = MaterialModule.GenerateEmissionMaterial( MaterialModule.GetSniperClassicBase() );
                    break;
                }
            }

            return model;
        }
    }
}
