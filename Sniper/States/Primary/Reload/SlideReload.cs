namespace Sniper.States.Primary.Reload
{
    using System;

    using EntityStates;

    using ReinCore;

    using RoR2;

    using Sniper.Enums;
    using Sniper.Modules;
    using Sniper.States.Bases;

    using UnityEngine;
    using UnityEngine.Networking;

    internal class SlideReload : GenericCharacterMain, ISniperReloadState
    {
        private static readonly AnimationCurve speedCurve = AnimationCurve.EaseInOut( 0f, 1f, 1f, 0.25f );
        private static readonly Single[] reloadMults = new[]
        {
            0.2f,
            0.4f,
            0.65f,
            1f
        };

        private enum Direction { Forward, Back, Left, Right }

        private const Single baseDuration = 0.3f;
        private const Single slideDurationMult = 2f;
        private const Single baseSpeedMultiplier = 5f;
        private const Single midairUpSpeedBoost = 15f;

        public ReloadTier reloadTier { get; set; }

        private Vector3 slideDirection;
        private Single speedMultiplier;
        private Single duration;
        private Boolean isSliding;

        private Direction animDir;

        private Transform gunTransform;

        private Single currentSpeed
        {
            get => this.speedMultiplier * base.moveSpeedStat * speedCurve.Evaluate( base.fixedAge / this.duration );
        }

        public override void OnEnter()
        {
            base.OnEnter();
            base.StartAimMode( 1f, false );
            base.modelAnimator.SetBool( "shouldAim", true );
            if( base.isAuthority )
            {
                if( base.inputBank )
                {
                    this.slideDirection = base.inputBank.moveVector;
                    this.slideDirection.y = 0f;
                    this.slideDirection = this.slideDirection.normalized;

                    CharacterDirection dir = base.characterDirection;
                    Vector3 forward = dir.forward;
                    Vector3 up = Vector3.up;
                    var right = Vector3.Cross( up, forward );

                    Single angX = Vector3.Dot( this.slideDirection, forward );
                    Single angY = Vector3.Dot( this.slideDirection, right );

                    this.animDir = Mathf.Abs( angX ) > Mathf.Abs( angY )
                        ? angX > 0f ? Direction.Forward : Direction.Back
                        : angY > 0f ? Direction.Right : Direction.Left;
                }
            }


            SoundModule.PlayLoad( base.gameObject, this.reloadTier );

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


                String animStr = "";

                if( base.characterMotor.isGrounded )
                {
                    switch( this.animDir )
                    {
                        case Direction.Forward:
                        animStr = "ReloadDash FG";
                        break;

                        case Direction.Back:
                        animStr = "ReloadDash BG";
                        break;

                        case Direction.Right:
                        animStr = "ReloadDash RG";
                        break;

                        case Direction.Left:
                        animStr = "ReloadDash LG";
                        break;
                    }
                } else
                {
                    switch( this.animDir )
                    {
                        case Direction.Forward:
                        animStr = "ReloadDash FA";
                        break;

                        case Direction.Back:
                        animStr = "ReloadDash BA";
                        break;

                        case Direction.Right:
                        animStr = "ReloadDash RA";
                        break;

                        case Direction.Left:
                        animStr = "ReloadDash LA";
                        break;
                    }
                }

                base.PlayAnimation( "Gesture, Override", animStr, "rateReloadDash", this.duration );


                // TODO: Slide sounds
                // TODO: Slide VFX
            } else
            {
                this.isSliding = false;
                base.PlayAnimation( "Gesture, Additive", "Reload", "rateReload", this.duration );
                this.gunTransform = base.FindModelChild( "RailgunBone" );
                this.gunTransform.SetParent( base.FindModelChild( "LeftWeapon" ), true );
                // TODO: No Slide VFX
            }

            base.StartAimMode( this.duration * 2f, false );

        }

        public override void OnSerialize( NetworkWriter writer )
        {
            base.OnSerialize( writer );
            writer.Write( new PackedUnitVector3( this.slideDirection ) );
            writer.Write( (Byte)this.reloadTier );
            writer.Write( (Byte)this.animDir );
        }

        public override void OnDeserialize( NetworkReader reader )
        {
            base.OnDeserialize( reader );
            this.slideDirection = reader.ReadPackedUnitVector3().Unpack();
            this.reloadTier = (ReloadTier)reader.ReadByte();
            this.animDir = (Direction)reader.ReadByte();
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

        public override void OnExit()
        {
            base.OnExit();
            if( this.gunTransform )
            {
                this.gunTransform.SetParent( base.FindModelChild( "RailgunDefaultPosition" ), true );
                this.gunTransform.localPosition = Vector3.zero;
                this.gunTransform.localRotation = Quaternion.identity;
                this.gunTransform.localScale = Vector3.one;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority() => InterruptPriority.PrioritySkill;
    }
}
