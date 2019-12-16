using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Engi133769OMEGA
{
    public partial class OmegaTurretMain
    {
        private void EditBodyStats()
        {
            var charBody = body.GetComponent<CharacterBody>();
            charBody.baseMaxHealth *= 10f;
            charBody.levelMaxHealth *= 10f;
            charBody.baseRegen *= 10f;
            charBody.levelRegen *= 10f;
            charBody.baseDamage *= 3f;
            charBody.levelDamage *= 3f;

            charBody.baseMoveSpeed = 5f;
        }
    }
}
