#if ROGUEWISP
using RoR2;
using UnityEngine;
//using static RogueWispPlugin.Helpers.APIInterface;

namespace Rein.RogueWispPlugin
{

    internal partial class Main
    {
        partial void RW_SetupHurtBoxes()
        {
            //this.Load += this.RW_DoHurtBoxSetup;
        }

        private void RW_DoHurtBoxSetup()
        {
            //Transform model = this.RW_body.GetComponent<ModelLocator>().modelTransform;
            //Transform mesh = model.Find("AncientWispMesh");
            //Transform refHb = model.Find("Hurtbox");

            //MeshCollider meshCol = refHb.gameObject.AddComponent<MeshCollider>();

            //MonoBehaviour.DestroyImmediate( refHb.GetComponent<Collider>() );

            //meshCol.sharedMesh = mesh.GetComponent<SkinnedMeshRenderer>().sharedMesh;
            //meshCol.isTrigger = false;
        }
    }

}
#endif