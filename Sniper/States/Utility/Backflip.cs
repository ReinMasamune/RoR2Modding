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
using Sniper.States.Bases;
using UnityEngine.Networking;

namespace Sniper.States.Utility
{
    internal class Backflip : GenericCharacterMain
    {
        private static readonly AnimationCurve backflipSpeedCurve = new AnimationCurve(
            new Keyframe( 0f, 0f ),
            new Keyframe( 0.05f, 0f ),
            new Keyframe( 0.15f, 1f ),
            new Keyframe( 0.3f, 0.9f ),
            new Keyframe( 1f, 0.3f )
        );

        const Single baseDuration = 0.75f;
        const Single speedMultiplier = 8f;
        const Single upwardsBoost = 15f;

        private Single duration;

        private Vector3 direction;

        private Single currentSpeed
        {
            get =>  speedMultiplier * base.moveSpeedStat / base.characterBody.sprintingSpeedMultiplier * backflipSpeedCurve.Evaluate( base.fixedAge / this.duration );
        }

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = baseDuration;
            base.characterBody.isSprinting = true;

            if( base.isAuthority )
            {
                this.direction = base.GetAimRay().direction * -1f;
                this.direction.y = 0f;
                this.direction = this.direction.normalized;
            }

            base.PlayAnimation( "Gesture, Override", "Backflip", "rateBackflip", this.duration );
            // TODO: Play Sound
            // TODO: VFX
            // TODO: Damage?

            base.characterMotor.Motor.ForceUnground();
            Single speed = this.currentSpeed;
            Vector3 boost = speed * this.direction;
            boost += new Vector3( 0f, upwardsBoost, 0f );
            base.characterMotor.velocity = boost;

            base.StartAimMode( 2f );
        }

        public override void OnSerialize( NetworkWriter writer )
        {
            base.OnSerialize( writer );
            writer.Write( new PackedUnitVector3( this.direction ) );
        }

        public override void OnDeserialize( NetworkReader reader )
        {
            base.OnDeserialize( reader );
            this.direction = reader.ReadPackedUnitVector3().Unpack();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            Single speed = this.currentSpeed;
            Single y = base.characterMotor.velocity.y;
            Vector3 boost = speed * this.direction;
            boost.y = y;
            base.characterMotor.velocity = boost;

            if( base.isAuthority && base.fixedAge > this.duration )
            {
                base.outer.SetNextStateToMain();
            }
        }
    }
}
