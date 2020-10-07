namespace Rein.Sniper.States.Special
{
    using System;
    using EntityStates;
    using RoR2;

    using Rein.Sniper.SkillDefs;
    using Rein.Sniper.States.Bases;

    using UnityEngine;
    using UnityEngine.Networking;

    internal class BeingADecoy : GenericCharacterMain
    {
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            base.characterBody.isSprinting = true;
        }
    }
}
