using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using BepInEx;
using ReinCore;
using RoR2;
using UnityEngine;
using System.Reflection;

namespace SkinStuff
{
    [BepInPlugin( "com.rein.test", "test", "1.0.0" )]
    public class Class1 : BaseUnityPlugin
    {
        private AssetBundle bundle;
        private Mesh newMesh;
        private void Awake()
        {
            ReinCore.ReinCore.Init( false, true, true, true, true, true, true );

            this.bundle = AssetBundle.LoadFromMemory( Properties.Resources.crocomesh94b );
            var all = bundle.LoadAllAssets();

            var obj = all[0] as GameObject;

            var newSkinned = obj.GetComponent<SkinnedMeshRenderer>();
            if( newSkinned == null ) newSkinned = obj.GetComponentInChildren<SkinnedMeshRenderer>();
            var acridBody = Resources.Load<GameObject>( "Prefabs/CharacterBodies/CrocoBody" );
            var acridModel = acridBody.GetComponent<ModelLocator>().modelTransform.gameObject;
            var acridSkins = acridModel.GetComponent<ModelSkinController>();
            var oldSkinDef = acridSkins.skins[1];
            var old = oldSkinDef.rendererInfos[0].renderer as SkinnedMeshRenderer;
            var ogmesh = old.sharedMesh;

            base.Logger.LogWarning( String.Format( "new renderer pos {0}", newSkinned.transform.eulerAngles ) );
            base.Logger.LogWarning( String.Format( "old renderer pos {0}", old.transform.eulerAngles ) );

            var oldRootTr = old.transform;
            var newRootTr = newSkinned.transform;




            var newBones = newSkinned.bones;
            var oldBones = old.bones;

            var oldNameLookup = new Dictionary<String,Int32>();
            var newIndexMap = new Dictionary<Int32,Int32>();
            var matrixMap = new Dictionary<Int32,Matrix4x4>();

            for( Int32 i = 0; i < oldBones.Length; ++i )
            {
                oldNameLookup[oldBones[i].name.ToLowerInvariant()] = i;
            }
            for( Int32 i = 0; i < newBones.Length; ++i )
            {
                var newB = newBones[i];
                if( oldNameLookup.TryGetValue( newB.name.ToLowerInvariant(), out var oldInd ) )
                {
                    newIndexMap[i] = oldInd;
                    var newTR = newBones[i];
                    var oldTR = oldBones[oldInd];
                    var newPos = newTR.position;
                    var oldPos = oldTR.position;
                    newPos = newRootTr.InverseTransformPoint( newPos );
                    oldPos = oldRootTr.InverseTransformPoint( oldPos );



                } else
                {
                    base.Logger.LogWarning( String.Format( "Unhandled bone {0}", newB.name ) );
                }
            }

            this.newMesh = newSkinned.sharedMesh;
            var newVerts = newMesh.vertices;
            var newBoneWeights = this.newMesh.boneWeights;

            var newBoneCenters = new Dictionary<Int32,CenterFinder>();

            for( Int32 i = 0; i < newBoneWeights.Length; ++i )
            {
                var weight = newBoneWeights[i];
                //weight.boneIndex0 = GetNewBoneIndex( weight.boneIndex0, newIndexMap );
                //weight.boneIndex1 = GetNewBoneIndex( weight.boneIndex1, newIndexMap );
                //weight.boneIndex2 = GetNewBoneIndex( weight.boneIndex2, newIndexMap );
                //weight.boneIndex3 = GetNewBoneIndex( weight.boneIndex3, newIndexMap );

                if( weight.weight0 != Mathf.Max( weight.weight0, weight.weight1, weight.weight2, weight.weight3 ) )
                {
                    base.Logger.LogWarning( String.Format( "Index {0} bone 0 not max weight", i ) );
                } else
                {
                    if( !newBoneCenters.ContainsKey( weight.boneIndex0 ) )
                    {
                        newBoneCenters[weight.boneIndex0] = new CenterFinder();
                    }
                    newBoneCenters[weight.boneIndex0].AddVec( newVerts[i], weight.weight0 );

                    if( !newBoneCenters.ContainsKey( weight.boneIndex1 ) )
                    {
                        newBoneCenters[weight.boneIndex1] = new CenterFinder();
                    }
                    newBoneCenters[weight.boneIndex1].AddVec( newVerts[i], weight.weight1 );

                    if( !newBoneCenters.ContainsKey( weight.boneIndex2 ) )
                    {
                        newBoneCenters[weight.boneIndex2] = new CenterFinder();
                    }
                    newBoneCenters[weight.boneIndex2].AddVec( newVerts[i], weight.weight2 );

                    if( !newBoneCenters.ContainsKey( weight.boneIndex3 ) )
                    {
                        newBoneCenters[weight.boneIndex3] = new CenterFinder();
                    }
                    newBoneCenters[weight.boneIndex3].AddVec( newVerts[i], weight.weight3 );
                }

                //weight.boneIndex0 = 0;

                weight.weight0 = 1f;
                weight.weight1 = 0f;
                weight.weight2 = 0f;
                weight.weight3 = 0f;

                newBoneWeights[i] = weight;
            }


            for( Int32 i = 0; i < newVerts.Length; ++i )
            {
                //newVerts[i] = newBoneCenters[newBoneWeights[i].boneIndex0].center;
                var tempWeight = newBoneWeights[i];
                tempWeight.boneIndex0 = 0;
                newBoneWeights[i] = tempWeight;
            }
            this.newMesh.boneWeights = newBoneWeights;
            this.newMesh.vertices = newVerts;

            var poses = this.newMesh.bindposes;
            var temp = new List<Matrix4x4>(poses);
            var ogposes = ogmesh.bindposes;
            base.Logger.LogWarning( String.Format( "{0} bindposes", temp.Count ) );
            var count2 = poses.Length;
            base.Logger.LogWarning( String.Format( "{0} og bindposes", ogposes.Length ) );

            //for( Int32 i = 0; i < count2; ++i )
            //{
            //    var newPose = poses[i];
            //    var oldPose = ogposes[i];
            //    if( newPose != oldPose )
            //    {
            //        base.Logger.LogWarning( String.Format( "{0} has mismatched poses, adjusting", i ) );
            //    }


            //}
            var newPoses = new Matrix4x4[ogposes.Length];
            for( Int32 i = 0; i < ogposes.Length; ++i )
            {
                newPoses[i] = ogposes[GetNewBoneIndex( i, newIndexMap )];
            }
            this.newMesh.bindposes = newPoses;


            base.Logger.LogWarning( String.Format( "{0} new blendshape count", this.newMesh.blendShapeCount ) );
            base.Logger.LogWarning( String.Format( "{0} old blendshape count", ogmesh.blendShapeCount ) );



            var oldMesh = Instantiate<Mesh>(old.sharedMesh);
            var oldVerts = oldMesh.vertices;
            var oldWeights = oldMesh.boneWeights;
            var oldBoneCenters = new Dictionary<Int32, CenterFinder>();
            for( Int32 i = 0; i < oldWeights.Length; ++i )
            {
                var weight = oldWeights[i];

                if( weight.weight0 != Mathf.Max( weight.weight0, weight.weight1, weight.weight2, weight.weight3 ) )
                {
                    base.Logger.LogWarning( String.Format( "Index {0} bone 0 not max weight", i ) );
                } else
                {
                    if( !oldBoneCenters.ContainsKey( weight.boneIndex0 ) )
                    {
                        oldBoneCenters[weight.boneIndex0] = new CenterFinder();
                    }
                    oldBoneCenters[weight.boneIndex0].AddVec( oldVerts[i], weight.weight0 );

                    if( !oldBoneCenters.ContainsKey( weight.boneIndex1 ) )
                    {
                        oldBoneCenters[weight.boneIndex1] = new CenterFinder();
                    }
                    oldBoneCenters[weight.boneIndex1].AddVec( oldVerts[i], weight.weight1 );

                    if( !oldBoneCenters.ContainsKey( weight.boneIndex2 ) )
                    {
                        oldBoneCenters[weight.boneIndex2] = new CenterFinder();
                    }
                    oldBoneCenters[weight.boneIndex2].AddVec( oldVerts[i], weight.weight2 );

                    if( !oldBoneCenters.ContainsKey( weight.boneIndex3 ) )
                    {
                        oldBoneCenters[weight.boneIndex3] = new CenterFinder();
                    }
                    oldBoneCenters[weight.boneIndex3].AddVec( oldVerts[i], weight.weight3 );
                }

                //weight.boneIndex0 = 0;

                weight.weight0 = 1f;
                weight.weight1 = 0f;
                weight.weight2 = 0f;
                weight.weight3 = 0f;
                oldWeights[i] = weight;
            }
            

            for( Int32 i = 0; i < oldVerts.Length; ++i )
            {
                //oldVerts[i] = oldBoneCenters[oldWeights[i].boneIndex0].center;
                var tempWeight = oldWeights[i];
                tempWeight.boneIndex0 = 0;
                oldWeights[i] = tempWeight;
            }
            oldMesh.boneWeights = oldWeights;
            oldMesh.vertices = oldVerts;

            oldSkinDef.meshReplacements = new[]
            {
                new SkinDef.MeshReplacement
                {
                    mesh = oldMesh,
                    renderer = old,
                }
            };


            var newSkin = Instantiate<SkinDef>(acridSkins.skins[1] );
            newSkin.meshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    mesh = newMesh,
                    renderer = old,
                },
            };

            Array.Resize<SkinDef>( ref acridSkins.skins, acridSkins.skins.Length + 1 );

            var instanceParam = Expression.Parameter( typeof(SkinDef), "instance" );
            var objParam = Expression.Parameter( typeof(System.Object), "object" );
            var runtimeSkinType = typeof(SkinDef).GetNestedType( "RuntimeSkin", BindingFlags.NonPublic );
            var conv = Expression.Convert( objParam, runtimeSkinType );
            var field = Expression.Field( instanceParam, typeof(SkinDef), "runtimeSkin" );
            var assign = Expression.Assign( field, conv );
            var func = Expression.Lambda<Action<SkinDef,System.Object>>( assign, instanceParam, objParam ).Compile();

            func( newSkin, null );
            func( oldSkinDef, null );
            
            acridSkins.skins[acridSkins.skins.Length - 1] = newSkin;
        }

        private delegate void Bake(SkinDef skin);


        private static void AdjustBoneIndicies( SkinnedMeshRenderer newMesh, SkinnedMeshRenderer oldMesh )
        {
            var oldBones = oldMesh.bones;
            var newBones = newMesh.bones;

            var oldNameLookup = new Dictionary<String,Int32>();
            var newIndexMap = new Dictionary<Int32,Int32>();

            for( Int32 i = 0; i < oldBones.Length; ++i )
            {
                oldNameLookup[oldBones[i].name] = i;
            }
            for( Int32 i = 0; i < newBones.Length; ++i )
            {
                var newB = newBones[i];
                if( oldNameLookup.TryGetValue( newB.name, out var oldInd ) )
                {
                    newIndexMap[i] = oldInd;
                }
            }

            var mesh = newMesh.sharedMesh;
            var newBoneWeights = mesh.boneWeights;

            for( Int32 i = 0; i < newBoneWeights.Length; ++i )
            {
                var weight = newBoneWeights[i];
                weight.boneIndex0 = GetNewBoneIndex( weight.boneIndex0, newIndexMap );
                weight.boneIndex1 = GetNewBoneIndex( weight.boneIndex1, newIndexMap );
                weight.boneIndex2 = GetNewBoneIndex( weight.boneIndex2, newIndexMap );
                weight.boneIndex3 = GetNewBoneIndex( weight.boneIndex3, newIndexMap );

                newBoneWeights[i] = weight;
            }

            mesh.boneWeights = newBoneWeights;
        }

        private static Int32 GetNewBoneIndex( Int32 index, Dictionary<Int32,Int32> map )
        {
            if( map.TryGetValue( index, out var newInd ) )
            {
                if( newInd == index )
                {
                    Debug.LogWarningFormat( "ind {0} had identity bone map", index );
                }

                return newInd;
            }
            Debug.LogWarningFormat( "ind {0} was unmapped", index );
            return index;
        }

        private class CenterFinder
        {
            public Vector3 center
            {
                get => this.total / this.counter;
            }

            public void AddVec( Vector3 added, Single weight = 1f )
            {
                this.total += (added * weight);
                this.counter += weight;
            }

            private Single counter = 0;
            private Vector3 total = Vector3.zero;
        }
    }
}
