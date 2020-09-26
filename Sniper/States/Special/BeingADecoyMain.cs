namespace Sniper.States.Special
{
    using System;
    using EntityStates;
    using RoR2;

    using Sniper.SkillDefs;
    using Sniper.States.Bases;

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
