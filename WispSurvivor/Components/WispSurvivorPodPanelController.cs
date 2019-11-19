using System;
using UnityEngine;

namespace WispSurvivor.Components
{
    public class WispSurvivorPodPanelController : MonoBehaviour
    {
        public void Start()
        {
            Transform par = this.transform;
            while( par.parent != null ) par = par.parent;

            Debug.Log( par.name );
            WispSurvivorPodController control = par.GetComponent<WispSurvivorPodController>();
            if( control )
            {
                UInt32 skin = control.skin;

                this.transform.Find( "EscapePodMesh.002" ).GetComponent<MeshRenderer>().material = Modules.WispMaterialModule.armorMaterials[skin];
            }


        }
    }
}
