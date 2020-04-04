using System;
using System.Collections.Generic;
using BepInEx;
using R2API;
using R2API.Utils;
using RoR2;
using UnityEngine;
namespace SkinModTemplate
{
    [R2APISubmoduleDependency( nameof(LoadoutAPI))]
    [BepInDependency( "com.bepis.r2api" )]
    [BepInPlugin( "com.ProperGUIDHere", "ProperNameHere", "1.0.0.0" )]
    public class SkinTemplate : BaseUnityPlugin
    {
        private void Awake()
        {
            var acridPrefab = Resources.Load<GameObject>( "Prefabs/CharacterBodies/CrocoBody" );
            var acridModel = acridPrefab.GetComponent<ModelLocator>().modelTransform;
            var acridSkins = acridModel.GetComponent<ModelSkinController>();
            var skin = Instantiate<SkinDef>(acridSkins.skins[1]);
            skin = LoadoutAPI.CreateNewSkinDef( new LoadoutAPI.SkinDefInfo
            {
                BaseSkins = skin.baseSkins,
                GameObjectActivations = skin.gameObjectActivations,
                Icon = skin.icon,
                Name = skin.name,
                NameToken = skin.nameToken,
                RendererInfos = skin.rendererInfos,
                RootObject = skin.rootObject,
                UnlockableName = skin.unlockableName,
            } );
            skin.meshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    mesh = this.GetMesh(),
                    renderer = skin.rendererInfos[0].renderer,
                },
            };
            Array.Resize<SkinDef>( ref acridSkins.skins, acridSkins.skins.Length + 1 );
            acridSkins.skins[acridSkins.skins.Length - 1] = skin;
        }
        private Mesh GetMesh()
        {
            var bundle = LoadAssetBundle( /*BytesOfBundle*/ );
            var obj = bundle.LoadAllAssets()[0] as GameObject;
            var skinned = obj.GetComponent<SkinnedMeshRenderer>();
            if( skinned == null ) skinned = obj.GetComponentInChildren<SkinnedMeshRenderer>();
            var oldBody = UnityEngine.Resources.Load<GameObject>( "Prefabs/CharacterBodies/CrocoBody" );
            var oldSkinned = oldBody.GetComponent<ModelLocator>().modelTransform.GetComponent<ModelSkinController>().skins[0].rendererInfos[0].renderer as SkinnedMeshRenderer;

            AdjustBoneIndicies( skinned, oldSkinned );

            return skinned.sharedMesh;
        }
        public static AssetBundle LoadAssetBundle( Byte[] resourceBytes )
        {
            if( resourceBytes == null ) throw new ArgumentNullException( nameof( resourceBytes ) );
            return AssetBundle.LoadFromMemory( resourceBytes );
        }


        public static void AdjustBoneIndicies( Mesh mesh )
        {
            var weights = mesh.boneWeights;

            for( Int32 i = 0; i < weights.Length; ++i )
            {
                var weight = weights[i];

                weight.boneIndex0 = GetNewIndex( weight.boneIndex0 );
                weight.boneIndex1 = GetNewIndex( weight.boneIndex1 );
                weight.boneIndex2 = GetNewIndex( weight.boneIndex2 );
                weight.boneIndex3 = GetNewIndex( weight.boneIndex3 );
            }

            mesh.boneWeights = weights;
        }

        public static Int32 GetNewIndex( Int32 oldIndex )
        {
            switch( oldIndex )
            {
                default:
                    if( !loggedInds.Contains( oldIndex ) )
                    {
                        loggedInds.Add( oldIndex );
                        Debug.Log( String.Format( "Unhandled index: {0}", oldIndex ) );
                    }
                    return 0;
            }
        }

        private static HashSet<Int32> loggedInds = new HashSet<Int32>();
    }
}