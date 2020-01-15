using BepInEx;
using RoR2;
using UnityEngine;
using R2API;
using R2API.Utils;
using System.Reflection;
using EntityStates;
using RoR2.Skills;
using System;
using System.Collections.Generic;

namespace ReinSniperRework
{
    internal partial class Main
    {
        partial void Mesh()
        {
            this.Load += this.GetKnifeMesh1;
            this.Load += this.GetKnifeMesh2;
            this.Load += this.GetKnifeMesh3;
            this.Load += this.GetBodyMesh;
            this.Load += this.GetRifleMesh;
            
        }

        internal Mesh knifeMesh1;
        internal Mesh knifeMesh2;
        internal Mesh knifeMesh3;


        private void GetRifleMesh()
        {
            Mesh mesh = UnityEngine.Object.Instantiate<Mesh>(this.sniperBody.GetComponent<ModelLocator>().modelTransform.GetComponent<CharacterModel>().baseRendererInfos[1].renderer.GetComponent<MeshFilter>().sharedMesh );

            this.sniperRifleMesh = mesh;
        }

        private void GetBodyMesh()
        {
            Mesh mesh = UnityEngine.Object.Instantiate<Mesh>(this.sniperBody.GetComponent<ModelLocator>().modelTransform.GetComponent<ModelSkinController>().skins[1].meshReplacements[0].mesh);

            this.sniperBodyMesh = mesh;
        }


        private void GetKnifeMesh3()
        {
            var obj = Resources.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponent<ModelLocator>().modelTransform;
            var mesh = Instantiate<Mesh>(obj.GetComponent<ModelSkinController>().skins[1].meshReplacements[0].mesh);

            base.Logger.LogWarning( "bindposes " + mesh.bindposes.Length );
            base.Logger.LogWarning( "boneweights " + mesh.boneWeights.Length );
            base.Logger.LogWarning( "colors " + mesh.colors.Length );
            base.Logger.LogWarning( "colors32 " + mesh.colors32.Length );
            base.Logger.LogWarning( "colors32 " + mesh.normals.Length );
            base.Logger.LogWarning( "colors32 " + mesh.tangents.Length );

            base.Logger.LogWarning( "UV1 " + mesh.uv.Length );
            base.Logger.LogWarning( "UV2 " + mesh.uv2.Length );
            base.Logger.LogWarning( "UV3 " + mesh.uv3.Length );
            base.Logger.LogWarning( "UV4 " + mesh.uv4.Length );
            base.Logger.LogWarning( "UV5 " + mesh.uv5.Length );
            base.Logger.LogWarning( "UV6 " + mesh.uv6.Length );
            base.Logger.LogWarning( "UV7 " + mesh.uv7.Length );
            base.Logger.LogWarning( "UV8 " + mesh.uv8.Length );


            var tris = mesh.triangles;
            var verts = mesh.vertices;
            var boneWeights = mesh.boneWeights;
            var colors = mesh.colors;
            var colors32 = mesh.colors32;
            var normals = mesh.normals;
            var tangents = mesh.tangents;
            var uv1 = mesh.uv;
            var uv2 = mesh.uv2;
            var bones = mesh.bindposes;
           

            var count = tris.Length;
            base.Logger.LogWarning( tris.Length );
            HashSet<Int32> usedVerts = new HashSet<Int32>();
            HashSet<Int32> usedBones = new HashSet<Int32>();
            var newTris = new Int32[642];
            Int32 tempCounter = 0;
            for( Int32 i = 0; i < count; i+=3 )
            {
                if( i <= 24825 )
                {
                    verts[tris[i]] += new Vector3( 0f, 100f, 0f );
                    verts[tris[i + 1]] += new Vector3( 0f, 100f, 0f );
                    verts[tris[i + 2]] += new Vector3( 0f, 100f, 0f );
                } else
                {
                    usedVerts.Add( tris[i] );
                    usedVerts.Add( tris[i + 1] );
                    usedVerts.Add( tris[i + 2] );

                    var boneWeight1 = boneWeights[tris[i]];
                    var boneWeight2 = boneWeights[tris[i+1]];
                    var boneWeight3 = boneWeights[tris[i+2]];

                    usedBones.Add( boneWeight1.boneIndex0 );
                    usedBones.Add( boneWeight1.boneIndex1 );
                    usedBones.Add( boneWeight1.boneIndex2 );
                    usedBones.Add( boneWeight1.boneIndex3 );
                    usedBones.Add( boneWeight2.boneIndex0 );
                    usedBones.Add( boneWeight2.boneIndex1 );
                    usedBones.Add( boneWeight2.boneIndex2 );
                    usedBones.Add( boneWeight2.boneIndex3 );
                    usedBones.Add( boneWeight3.boneIndex0 );
                    usedBones.Add( boneWeight3.boneIndex1 );
                    usedBones.Add( boneWeight3.boneIndex2 );
                    usedBones.Add( boneWeight3.boneIndex3 );

                    //newTris[tempCounter++] = tris[i];
                    //newTris[tempCounter++] = tris[i + 1];
                    //newTris[tempCounter++] = tris[i + 2];
                }
            }

            var newVerts = new Vector3[usedVerts.Count];
            var newUV1s = new Vector2[usedVerts.Count];
            var newUV2s = new Vector2[usedVerts.Count];
            var newColors = new Color[usedVerts.Count];
            var newColors32 = new Color32[usedVerts.Count];
            var newNormals = new Vector3[usedVerts.Count];
            var newTangents = new Vector4[usedVerts.Count];
            var newBoneWeights = new BoneWeight[usedVerts.Count];
            var newBones = new Matrix4x4[usedBones.Count];
            //var newUVs 
            //var newWeights = new BoneWeight[usedVerts.Count];

            var newerTris = new Int32[newTris.Length];

            Int32 vertCounter = 0;
            foreach( Int32 i in usedVerts )
            {
                newVerts[vertCounter] = verts[i];
                newColors[vertCounter] = colors[i];
                newColors32[vertCounter] = colors32[i];
                newNormals[vertCounter] = normals[i];
                newTangents[vertCounter] = tangents[i];
                newBoneWeights[vertCounter] = boneWeights[i];
                newUV1s[vertCounter] = uv1[i];
                newUV2s[vertCounter] = uv2[i];

                for( Int32 j = 24828; j < count; j++ )
                {
                    if( j >= tris.Length )
                    {
                        base.Logger.LogError( "Out of range j" );
                        continue;
                    }
                    if( (j - 24828) >= newerTris.Length )
                    {
                        base.Logger.LogError( "Out of range vert j newTris" );
                        base.Logger.LogError( "Length: " + newerTris.Length );
                        base.Logger.LogError( "Index: " + (j - 24828) );
                        continue;
                    }
                    if( tris[j] == i )
                    {
                        newerTris[j - 24828] = vertCounter;
                    }
                }

                vertCounter += 1;
            }
            Int32 boneCounter = 0;
            foreach( Int32 i in usedBones )
            {
                newBones[boneCounter] = bones[i];

                for( Int32 j = 0; j < newBoneWeights.Length; ++j )
                {
                    var weight = newBoneWeights[j];
                    if( newBoneWeights[j].boneIndex0 == i ) newBoneWeights[j].boneIndex0 = boneCounter;
                    if( newBoneWeights[j].boneIndex1 == i ) newBoneWeights[j].boneIndex1 = boneCounter;
                    if( newBoneWeights[j].boneIndex2 == i ) newBoneWeights[j].boneIndex2 = boneCounter;
                    if( newBoneWeights[j].boneIndex3 == i ) newBoneWeights[j].boneIndex3 = boneCounter;
                }

                ++boneCounter;
            }

            mesh.triangles = newerTris;
            mesh.vertices = newVerts;
            mesh.normals = newNormals;
            mesh.tangents = newTangents;
            mesh.colors = newColors;
            mesh.colors32 = newColors32;
            mesh.boneWeights = newBoneWeights;
            mesh.uv = newUV1s;
            mesh.uv2 = newUV2s;
            mesh.bindposes = newBones;
            this.knifeMesh3 = mesh;
        }
        private void GetKnifeMesh2()
        {
            var obj = Resources.Load<GameObject>("Prefabs/CharacterDisplays/MercDisplay").transform.Find("mdlMerc");
            var mesh = Instantiate<Mesh>( ((SkinnedMeshRenderer)obj.GetComponent<CharacterModel>().baseRendererInfos[1].renderer).sharedMesh );

            this.knifeMesh2 = mesh;
        }
        private void GetKnifeMesh1()
        {
            var obj = Resources.Load<GameObject>("Prefabs/PickupModels/PickupDagger").transform.Find("mdlDagger");
            var mesh = Instantiate<Mesh>(obj.GetComponent<MeshFilter>().sharedMesh);

            this.knifeMesh1 = mesh;
        }
    }
}


