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

        // TODO: Implement Decoy Activation
        internal override DecoySkillData CreateSkillData()
        {
            base.data = new DecoySkillData();
            return base.data;
        }

        private Vector3 position;
        private Quaternion rotation;

        public override void OnEnter()
        {
            base.OnEnter();

            if( base.isAuthority )
            {
                this.position = base.transform.position;
                this.rotation = base.transform.rotation;
            }

            if( NetworkServer.active )
            {
                base.characterBody.AddTimedBuff( BuffIndex.Cloak, 8f );
                base.characterBody.SummonDecoy( this.position, this.rotation );
            }
        }

        public override void OnSerialize( NetworkWriter writer )
        {
            base.OnSerialize( writer );
            writer.Write( this.position );
            writer.Write( this.rotation );
        }

        public override void OnDeserialize( NetworkReader reader )
        {
            base.OnDeserialize( reader );
            this.position = reader.ReadVector3();
            this.rotation = reader.ReadQuaternion();
        }

    }
}
