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
using Sniper.States.Bases;
using UnityEngine.Networking;
using Sniper.Enums;

namespace Sniper.States.Primary.Reload
{
    internal class SlideReload : GenericCharacterMain, ISniperReloadState
    {
        private static readonly AnimationCurve speedCurve = AnimationCurve.Linear( 0f, 1f, 1f, 0.25f );
        private static readonly Single[] reloadMults = new[]
        {
            0.3f,
            0.4f,
            0.65f,
            1f
        };

        const Single baseDuration = 0.3f;
        const Single slideDurationMult = 1.75f;
        const Single baseSpeedMultiplier = 5f;
        const Single midairUpSpeedBoost = 15f;

        public ReloadTier reloadTier { get; set; }

        private Vector3 slideDirection;
        private Single speedMultiplier;
        private Single duration;
        private Boolean isSliding;


        private Single currentSpeed
        {
            get => speedMultiplier * base.moveSpeedStat * speedCurve.Evaluate( base.fixedAge / this.duration );
        }

        public override void OnEnter()
        {
            base.OnEnter();

            if( base.isAuthority )
            {
                if( base.inputBank )
                {
                    this.slideDirection = base.inputBank.moveVector;
                    this.slideDirection.y = 0f;
                    this.slideDirection = slideDirection.normalized;
                }
            }


            this.duration = baseDuration / base.attackSpeedStat;

            if( this.slideDirection != Vector3.zero )
            {
                this.duration *= slideDurationMult;
                this.speedMultiplier = baseSpeedMultiplier * base.attackSpeedStat;
                this.speedMultiplier *= reloadMults[(Byte)this.reloadTier + 1];
                this.isSliding = true;

                Single speed = this.currentSpeed;
                Vector3 boost = this.slideDirection * speed;
                boost += base.characterMotor.isGrounded ? Vector3.zero : Vector3.up * midairUpSpeedBoost;
                base.characterMotor.velocity = boost;

                // TODO: Slide animation
                // TODO: Slide sounds
                // TODO: Slide VFX
            } else
            {
                this.isSliding = false;
                // TODO: No slide animation
                // TODO: No slide sounds
                // TODO: No Slide VFX
            }

        }

        public override void OnSerialize( NetworkWriter writer )
        {
            base.OnSerialize( writer );
            writer.Write( new PackedUnitVector3( this.slideDirection ) );
            writer.Write( (Byte)this.reloadTier );
        }

        public override void OnDeserialize( NetworkReader reader )
        {
            base.OnDeserialize( reader );
            this.slideDirection = reader.ReadPackedUnitVector3().Unpack();
            this.reloadTier = (ReloadTier)reader.ReadByte();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if( this.isSliding )
            {
                Single speed = this.currentSpeed;
                Single y = base.characterMotor.velocity.y;
                Vector3 boost = this.slideDirection * speed;
                boost.y = y;
                base.characterMotor.velocity = boost;
            }

            if( base.isAuthority && base.fixedAge >= this.duration )
            {
                base.outer.SetNextStateToMain();
            }
        }
    }
}
