namespace AlternateArtificer
{
    using RoR2;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public partial class Main
    {
        private void EditModel()
        {
            Transform model = artiBody.GetComponent<ModelLocator>().modelTransform;

            #region Remove jets
            Transform jetsParent = model.Find("MageArmature").Find("ROOT").Find("base").Find("stomach").Find("chest");
            jetsParent.Find( "Jets, Right" ).gameObject.SetActive( false );
            jetsParent.Find( "Jets, Left" ).gameObject.SetActive( false );
            #endregion

            #region Edit Mesh
            SkinnedMeshRenderer meshRenderer = model.Find("MageMesh").GetComponent<SkinnedMeshRenderer>();
            Mesh mesh = meshRenderer.sharedMesh;
            Int32[] tris = mesh.triangles;
            Int32 size = 1902;
            Int32 start1 = 4916;
            Int32 start2 = 10054;
            SortedSet<Int32> inds = new SortedSet<Int32>();
            for( Int32 i = 0; i < size; i++ )
            {
                Int32 indOff1 = (i + start1) * 3;
                inds.Add(tris[indOff1]);
                inds.Add(tris[indOff1 + 1]);
                inds.Add(tris[indOff1 + 2]);
                Int32 indOff2 = (i + start2) * 3;
                inds.Add(tris[indOff2]);
                inds.Add(tris[indOff2 + 1]);
                inds.Add(tris[indOff2 + 2]);
            }
            var boneWL = mesh.boneWeights.ToList();
            var colorL = mesh.colors.ToList();
            var color32L = mesh.colors32.ToList();
            var normalL = mesh.normals.ToList();
            var tanL = mesh.tangents.ToList();
            var uv1L = mesh.uv.ToList();
            var vertL = mesh.vertices.ToList();
            var trisL = mesh.triangles.ToList();
            foreach( Int32 i in inds.Reverse() )
            {
                if( boneWL.Count >= i ) boneWL.RemoveAt( i );
                if( colorL.Count >= i ) colorL.RemoveAt( i );
                if( color32L.Count >= i ) color32L.RemoveAt( i );
                if( normalL.Count >= i ) normalL.RemoveAt( i );
                if( tanL.Count >= i ) tanL.RemoveAt( i );
                if( uv1L.Count >= i ) uv1L.RemoveAt( i );
                if( vertL.Count >= i ) vertL.RemoveAt( i );
                Int32 offset = 0;
                for( Int32 j = 0; j < trisL.Count + offset; j += 3 )
                {
                    var ind1 = trisL[j - offset];
                    var ind2 = trisL[j + 1 - offset];
                    var ind3 = trisL[j + 2 - offset];
                    if( ind1 == i || ind2 == i || ind3 == i )
                    {
                        trisL.RemoveRange( j - offset, 3 );
                        offset += 3;
                        continue;
                    }
                    if( ind1 > i ) trisL[j - offset] -= 1;
                    if( ind2 > i ) trisL[j + 1 - offset] -= 1;
                    if( ind3 > i ) trisL[j + 2 - offset] -= 1;
                }
            }
            mesh.triangles = trisL.ToArray();
            mesh.vertices = vertL.ToArray();
            mesh.boneWeights = boneWL.ToArray();
            mesh.colors = colorL.ToArray();
            mesh.colors32 = color32L.ToArray();
            mesh.normals = normalL.ToArray();
            mesh.tangents = tanL.ToArray();
            mesh.uv = uv1L.ToArray();
            #endregion

            #region Fix Skirt
            model.gameObject.AddComponent<Components.SkirtFix>();
            Resources.Load<GameObject>( "Prefabs/CharacterDisplays/MageDisplay" ).transform.Find( "mdlMage" ).gameObject.AddComponent<Components.SkirtFix>();
            Resources.Load<GameObject>( "Prefabs/NetworkedObjects/LockedMage" ).transform.Find( "ModelBase" ).Find( "mdlMage" ).gameObject.AddComponent<Components.SkirtFix>();
            #endregion

            #region Add Rotator
            model.Find( "MageArmature" ).gameObject.AddComponent<Components.Rotator>();
            #endregion

            // TODO: Disable IDRS
            // TODO: Fix+Enable IDRS
            // ATG
            // Infusion
            // Crowbar
            // Cautious Slug
            // Fuel cell
            // Tougher Times
        }
    }
}
