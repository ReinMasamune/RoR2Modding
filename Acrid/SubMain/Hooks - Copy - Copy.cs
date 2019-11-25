using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Acrid
{
    public partial class AcridMain
    {
        public void EditStats()
        {
            CharacterBody charBody = body.GetComponent<CharacterBody>();

            charBody.baseArmor = 25f;
            charBody.levelArmor = 2f;
            charBody.baseMaxHealth = 150f;
            charBody.levelMaxHealth = 45f;
            charBody.baseDamage = 12f;
            charBody.levelDamage = 2.4f;
            charBody.baseMoveSpeed = 7f;
            charBody.levelMoveSpeed = 0f;
            charBody.baseJumpPower = 15f;
            charBody.levelJumpPower = 0f;
        }
    }
}
