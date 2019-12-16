using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Engi133769OMEGA
{
    public partial class OmegaTurretMain
    {
        private void EditModel()
        {
            Transform modelBase = body.GetComponent<ModelLocator>().modelBaseTransform;
            Transform model = body.GetComponent<ModelLocator>().modelTransform;

            modelBase.localScale = new Vector3( 3f, 3f, 3f );
        }
    }
}
