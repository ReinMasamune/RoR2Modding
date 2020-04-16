//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using BepInEx;
//using ReinCore;
//using RoR2;
//using Unity.Collections;
//using Unity.Jobs;
//using UnityEngine;

//namespace Rein.AlternateArtificer
//{
//    internal partial class Main
//    {
//        partial void Model()
//        {
//            this.awake += this.Main_awake1;
//        }

//        private void Main_awake1()
//        {
//            Transform model = this.artiBodyPrefab.GetComponent<ModelLocator>().modelTransform;

//            #region Remove jets
//            var display = Resources.Load<GameObject>("Prefabs/CharacterDisplays/MageDisplay").transform.Find("mdlMage").GetComponent<CharacterModel>().baseRendererInfos;
//            display[0].renderer.gameObject.SetActive( false );
//            display[1].renderer.gameObject.SetActive( false );
//            #endregion

//            #region Edit Mesh
//            SkinnedMeshRenderer meshRenderer = model.Find("MageMesh").GetComponent<SkinnedMeshRenderer>();
//            artiDefaultMesh = meshRenderer.sharedMesh;
//            artiModifiedMesh = Instantiate<Mesh>( artiDefaultMesh );

//            Int32[] tris = artiModifiedMesh.triangles;
//            Int32 size = 1902;
//            Int32 start1 = 4916;
//            Int32 start2 = 10054;
//            SortedSet<Int32> inds = new SortedSet<Int32>();
//            for( Int32 i = 0; i < size; i++ )
//            {
//                Int32 indOff1 = (i + start1) * 3;
//                inds.Add( tris[indOff1] );
//                inds.Add( tris[indOff1 + 1] );
//                inds.Add( tris[indOff1 + 2] );
//                Int32 indOff2 = (i + start2) * 3;
//                inds.Add( tris[indOff2] );
//                inds.Add( tris[indOff2 + 1] );
//                inds.Add( tris[indOff2 + 2] );
//            }
//            var boneWL = artiModifiedMesh.boneWeights.ToList();
//            var colorL = artiModifiedMesh.colors.ToList();
//            var color32L = artiModifiedMesh.colors32.ToList();
//            var normalL = artiModifiedMesh.normals.ToList();
//            var tanL = artiModifiedMesh.tangents.ToList();
//            var uv1L = artiModifiedMesh.uv.ToList();
//            var vertL = artiModifiedMesh.vertices.ToList();
//            var trisL = artiModifiedMesh.triangles.ToList();
//            foreach( Int32 i in inds.Reverse() )
//            {
//                if( boneWL.Count >= i ) boneWL.RemoveAt( i );
//                if( colorL.Count >= i ) colorL.RemoveAt( i );
//                if( color32L.Count >= i ) color32L.RemoveAt( i );
//                if( normalL.Count >= i ) normalL.RemoveAt( i );
//                if( tanL.Count >= i ) tanL.RemoveAt( i );
//                if( uv1L.Count >= i ) uv1L.RemoveAt( i );
//                if( vertL.Count >= i ) vertL.RemoveAt( i );
//                Int32 offset = 0;
//                for( Int32 j = 0; j < trisL.Count + offset; j += 3 )
//                {
//                    var ind1 = trisL[j - offset];
//                    var ind2 = trisL[j + 1 - offset];
//                    var ind3 = trisL[j + 2 - offset];
//                    if( ind1 == i || ind2 == i || ind3 == i )
//                    {
//                        trisL.RemoveRange( j - offset, 3 );
//                        offset += 3;
//                        continue;
//                    }
//                    if( ind1 > i ) trisL[j - offset] -= 1;
//                    if( ind2 > i ) trisL[j + 1 - offset] -= 1;
//                    if( ind3 > i ) trisL[j + 2 - offset] -= 1;
//                }
//            }
//            artiModifiedMesh.triangles = trisL.ToArray();
//            artiModifiedMesh.vertices = vertL.ToArray();
//            artiModifiedMesh.boneWeights = boneWL.ToArray();
//            artiModifiedMesh.colors = colorL.ToArray();
//            artiModifiedMesh.colors32 = color32L.ToArray();
//            artiModifiedMesh.normals = normalL.ToArray();
//            artiModifiedMesh.tangents = tanL.ToArray();
//            artiModifiedMesh.uv = uv1L.ToArray();
//            #endregion

//            #region Fix Skirt
//            model.gameObject.AddComponent<Components.SkirtFix>();
//            Resources.Load<GameObject>( "Prefabs/CharacterDisplays/MageDisplay" ).transform.Find( "mdlMage" ).gameObject.AddComponent<Components.SkirtFix>();
//            Resources.Load<GameObject>( "Prefabs/NetworkedObjects/LockedMage" ).transform.Find( "ModelBase" ).Find( "mdlMage" ).gameObject.AddComponent<Components.SkirtFix>();
//            #endregion

//            #region Add Rotator
//            model.Find( "MageArmature" ).gameObject.AddComponent<Components.Rotator>();
//            #endregion

//            model.GetComponent<CharacterModel>().itemDisplayRuleSet = Resources.Load<GameObject>( "Prefabs/CharacterBodies/AncientWispBody" ).GetComponent<ModelLocator>().modelTransform.GetComponent<CharacterModel>().itemDisplayRuleSet;
//            //var idrs = model.GetComponent<CharacterModel>().itemDisplayRuleSet;
//            //idrs.


//            // TODO: Fix+Enable IDRS
//            // ATG
//            // Infusion
//            // Crowbar
//            // Cautious Slug
//            // Fuel cell
//            // Tougher Times
//            // Wings
//            // Dio (consumed?)
//            // Warbanner
//            // Blast Shower
//            // Harvester Scythe
//            // Fireworks
//            // Ghors Tome
//            // Shattering Justice
//        }
//    }
//}

//// TODO: Model skin
//// TODO: Jetpack-removed mesh
//// TODO: Adjust body scales and stuff
//// TODO: Make skin icon generator for Core
