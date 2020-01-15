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
//using static RogueWispPlugin.Helpers.APIInterface;

namespace RogueWispPlugin
{
#if ROGUEWISP
    internal partial class Main
    {
        partial void RW_EditModelMesh()
        {
            this.Load += this.RW_DoModelMeshEdits;
        }

        private void RW_DoModelMeshEdits()
        {
            Mesh m = this.RW_body.GetComponent<ModelLocator>().modelTransform.Find("AncientWispMesh").GetComponent<SkinnedMeshRenderer>().sharedMesh;
            Vector2[] newUvs = new Vector2[m.vertexCount];
            Vector3[] verts = m.vertices;
            Single tempu = 0f;
            Single tempv = 0f;
            Single tempp = 0f;
            Vector2 tempv2 = Vector2.zero;
            for( Int32 i = 0; i < m.vertexCount; i++ )
            {
                tempv = verts[i].z;
                tempu = Mathf.Atan2( verts[i].y, verts[i].x );

                newUvs[i] = new Vector2( tempu, tempv );
            }
            m.uv = newUvs;
        }
    }
#endif
}
