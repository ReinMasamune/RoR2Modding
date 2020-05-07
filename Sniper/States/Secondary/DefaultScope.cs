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
using Sniper.Data;
using UnityEngine.Networking;
using Sniper.States.Bases;

namespace Sniper.States.Secondary
{
    internal class DefaultScope : ScopeBaseState
    {
        const Single maxCharge = 1f;
        const Single chargePerSecond = 0.2f;
        const Single minModifier = 1f;
        const Single maxModifier = 10f;
        const Single speedScalar = 0.25f;

        private static AnimationCurve damageCurve = new AnimationCurve
        (
            new Keyframe(0f, minModifier, 0f, 0f ),
            new Keyframe(0.2f, minModifier, 0f, 0f ),
            new Keyframe(1f, maxModifier, 0.5f, 0f )
        );



        internal override Boolean usesCharge { get; } = true;
        internal override Boolean usesStock { get; } = false;
        internal override Single currentCharge { get => this.charge; }
        internal override UInt32 currentStock { get; } = 0u;
        internal Single charge = 0f;

        internal override void OnFired()
        {
            this.charge = 0f;
        }
        internal override BulletModifier ReadModifier()
        {
            var mod = BulletModifier.identity;
            mod.damageMultiplier = this.GetDamageMultiplier();
            mod.charge = this.charge;

            return mod;
        }


        public override void OnEnter()
        {
            base.OnEnter();

            base.StartAimMode( 2f );
            if( NetworkServer.active ) characterBody.AddBuff( BuffIndex.Slow80 );

        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            base.characterBody.SetAimTimer( 2f );

            if( this.charge < maxCharge ) this.charge += Time.fixedDeltaTime * chargePerSecond * characterBody.attackSpeed / ( 1f + (base.characterMotor.velocity.magnitude * speedScalar ));

            else
            {
                this.charge = maxCharge;
            }
        }

        public override void OnExit()
        {
            base.OnExit();

            if( NetworkServer.active ) characterBody.RemoveBuff( BuffIndex.Slow80 ); 

        }


        private Single GetDamageMultiplier() => damageCurve.Evaluate( this.charge / maxCharge );
    }
}
