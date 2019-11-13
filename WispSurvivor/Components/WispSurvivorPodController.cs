using UnityEngine;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;

namespace WispSurvivor.Components
{
    public class WispSurvivorPodController : MonoBehaviour
    {
        public Transform podMesh;
        public Transform doorMesh;
        public Transform doorMesh2;

        public uint skin;

        public void Start()
        {
            var body = GetComponent<VehicleSeat>().currentPassengerBody;
            skin = body.skinIndex;

            if( body.baseNameToken == "WISP_SURVIVOR_BODY_NAME" )
            {
                podMesh.GetComponent<MeshRenderer>().material = Modules.WispMaterialModule.armorMaterials[skin];
                doorMesh.GetComponent<MeshRenderer>().material = Modules.WispMaterialModule.armorMaterials[skin];
                doorMesh2.GetComponent<MeshRenderer>().material = Modules.WispMaterialModule.armorMaterials[skin];
            }
        }
    }
}
