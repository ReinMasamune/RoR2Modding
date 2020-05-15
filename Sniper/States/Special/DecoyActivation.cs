namespace Sniper.States.Special
{
    using System;
    using EntityStates;
    using RoR2;

    using Sniper.SkillDefs;
    using Sniper.States.Bases;

    using UnityEngine;
    using UnityEngine.Networking;

    internal class DecoyActivation : ActivationBaseState<DecoySkillData>
    {
        private const Single cloakDuration = 8f;

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

                base.outer.SetNextStateToMain();
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

        public override InterruptPriority GetMinimumInterruptPriority() => InterruptPriority.PrioritySkill;
    }
}
