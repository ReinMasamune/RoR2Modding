using UnityEngine;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;

namespace WispSurvivor.Components
{
    public class WispSurvivorPodPanelController : MonoBehaviour
    {
        public void Start()
        {
            Transform par = transform;
            while (par.parent != null) par = par.parent;

            Debug.Log(par.name);
            var control = par.GetComponent<WispSurvivorPodController>();
            if( control )
            {
                var skin = control.skin;

                transform.Find("EscapePodMesh.002").GetComponent<MeshRenderer>().material = Modules.WispMaterialModule.armorMaterials[skin];
            }

            
        }
    }
}
