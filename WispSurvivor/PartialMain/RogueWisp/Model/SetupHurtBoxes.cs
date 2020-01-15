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
        partial void RW_SetupHurtBoxes()
        {
            //this.Load += this.RW_DoHurtBoxSetup;
        }

        private void RW_DoHurtBoxSetup()
        {
            Transform model = this.RW_body.GetComponent<ModelLocator>().modelTransform;
            Transform mesh = model.Find("AncientWispMesh");
            Transform refHb = model.Find("Hurtbox");

            MeshCollider meshCol = refHb.gameObject.AddComponent<MeshCollider>();

            MonoBehaviour.DestroyImmediate( refHb.GetComponent<Collider>() );

            meshCol.sharedMesh = mesh.GetComponent<SkinnedMeshRenderer>().sharedMesh;
            meshCol.isTrigger = false;
        }
    }
#endif
}
