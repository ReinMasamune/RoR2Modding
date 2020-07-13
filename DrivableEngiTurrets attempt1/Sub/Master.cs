using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Engi133769OMEGA
{
    public partial class OmegaTurretMain
    {
        private void EditMaster()
        {
            var charMaster = master.GetComponent<CharacterMaster>();

            charMaster.bodyPrefab = body;


        }
    }
}
