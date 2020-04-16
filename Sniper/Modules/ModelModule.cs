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
            var bundle = Properties.Tools.LoadAssetBundle( Properties.Resources.sniper );
            var model = bundle.LoadAsset<GameObject>( Properties.Resources.SniperPrefabPath );

            var sniperMats = MaterialModule.GetSniperMaterials();
            var rifleMat = MaterialModule.GetRifleMaterial();
            var knifeMat = MaterialModule.GetKnifeMaterial();
            foreach( var smr in model.GetComponentsInChildren<SkinnedMeshRenderer>() )
            {
                if( smr.gameObject.name == "SniperMesh" )
                {
                    smr.sharedMaterials = sniperMats;
                } else if( smr.gameObject.name == "GuassGun" )
                {
                    smr.sharedMaterial = rifleMat;
                } else
                {
                    smr.sharedMaterial = knifeMat;
                }
            }

            return model;
        }
    }
}
