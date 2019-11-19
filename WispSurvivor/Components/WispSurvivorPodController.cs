using RoR2;
using System;
using UnityEngine;

namespace WispSurvivor.Components
{
    public class WispSurvivorPodController : MonoBehaviour
    {
        public Transform podMesh;
        public Transform doorMesh;
        public Transform doorMesh2;

        public UInt32 skin;

        public void Start()
        {
            CharacterBody body = this.GetComponent<VehicleSeat>().currentPassengerBody;
            this.skin = body.skinIndex;

            if( body.baseNameToken == "WISP_SURVIVOR_BODY_NAME" )
            {
                this.podMesh.GetComponent<MeshRenderer>().material = Modules.WispMaterialModule.armorMaterials[this.skin];
                this.doorMesh.GetComponent<MeshRenderer>().material = Modules.WispMaterialModule.armorMaterials[this.skin];
                this.doorMesh2.GetComponent<MeshRenderer>().material = Modules.WispMaterialModule.armorMaterials[this.skin];
            }
        }
    }
}
