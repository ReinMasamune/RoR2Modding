using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BepInEx.Logging;
using ReinCore;
using RoR2;
using RoR2.Networking;
using UnityEngine;
using KinematicCharacterController;
using EntityStates;
using RoR2.Skills;
using System.Reflection;
using Sniper.Expansions;
using Sniper.Enums;
using Sniper.SkillDefs;
using Sniper.States.Bases;
using UnityEngine.Networking;

namespace Sniper.States.Special
{
    internal class DecoyActivation : ActivationBaseState<DecoySkillData>
    {
        const Single cloakDuration = 8f;

        // TODO: Implement
        internal override DecoySkillData CreateSkillData()
        {
            base.data = new DecoySkillData();
            return base.data;
        }



        public override void OnEnter()
        {
            base.OnEnter();

            if( NetworkServer.active )
            {
                base.characterBody.AddTimedBuff( BuffIndex.Cloak, 8f );
            }
        }

        public override void OnSerialize( NetworkWriter writer )
        {

        }

        public override void OnDeserialize( NetworkReader reader )
        {

        }

    }
}
