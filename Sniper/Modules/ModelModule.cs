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
            CharacterModel charModel = model.GetComponent<CharacterModel>();

            for( Int32 i = 0; i < charModel.baseRendererInfos.Length; ++i )
            {
                CharacterModel.RendererInfo info = charModel.baseRendererInfos[i];
                Renderer smr = info.renderer;
                Material defaultMat;
                Material skin1Mat;
                Material skin2Mat;
                switch( smr.name )
                {
                    default:
                    Log.Warning( String.Format( "{0} is not a handled renderer name", smr.name ) );
                    defaultMat = null;
                    break;

                    case "SniperMesh":
                    defaultMat = null;
                    break;

                    case "ThrowKnife":
                    defaultMat = MaterialModule.GetThrowKnifeDefault();
                    break;

                    case "Knife":
                    defaultMat = MaterialModule.GenerateArmorMaterial( MaterialModule.GetSniperDefaultBase() );
                    break;

                    case "RailGun":
                    defaultMat = MaterialModule.GetRailDefault();
                    break;

                    case "GaussGun":
                    defaultMat = null;
                    break;

                    case "AmmoMesh":
                    defaultMat = MaterialModule.GenerateAmmoMaterial( MaterialModule.GetSniperDefaultBase() );
                    break;

                    case "ArmorMesh":
                    defaultMat = MaterialModule.GenerateArmorMaterial( MaterialModule.GetSniperDefaultBase() );
                    break;

                    case "BodyMesh":
                    defaultMat = MaterialModule.GenerateBodyMaterial( MaterialModule.GetSniperDefaultBase() );
                    break;

                    case "CloakMesh":
                    defaultMat = MaterialModule.GenerateCloakMaterial( MaterialModule.GetSniperDefaultBase() );
                    break;

                    case "EmissionMesh":
                    defaultMat = MaterialModule.GenerateEmissionMaterial( MaterialModule.GetSniperDefaultBase() );
                    break;
                }
                smr.material = defaultMat;
                info.defaultMaterial = defaultMat;
                charModel.baseRendererInfos[i] = info;
                if( defaultMat != null )
                {
                    var holder1 = smr.gameObject.AddComponent<StandardMaterialHolder>();
                    //var holder2 = smr.gameObject.AddComponent<BaseMaterialHolder<StandardMaterial>>();
                    holder1.standardMaterial = new StandardMaterial( defaultMat );
                }
            }

            return model;
        }
    }
}
