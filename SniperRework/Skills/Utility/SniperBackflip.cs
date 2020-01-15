using BepInEx;
using RoR2;
using UnityEngine;
using R2API;
using R2API.Utils;
using System.Reflection;
using EntityStates;
using RoR2.Skills;
using System;
using UnityEngine.Networking;

namespace ReinSniperRework
{
    internal partial class Main
    {
        internal class SniperBackflip : BaseState
        {
            const Single duration = 0.35f;
            const Single initSpeedCoef = 10f;
            const Single endSpeedCoef = 2.5f;
            const Single damageCoef = 1.0f;
            const Single procCoef = 1.0f;
            const Single baseForce = 5f;
            const Single downForce = 10f;

            private Single rollSpeed;

            private Vector3 previousPosition;
            private Vector3 forwardDirection;
            private Animator animator;
            private HurtBoxGroup boxGroup;

            public override void OnEnter()
            {
                base.OnEnter();
                Util.PlaySound( EntityStates.Commando.DodgeState.dodgeSoundString, base.gameObject );

                this.animator = base.GetModelAnimator();
                base.characterBody.SetAimTimer( 2f );

                this.boxGroup = base.GetModelTransform().GetComponent<HurtBoxGroup>();

                ChildLocator childLoc = this.animator.GetComponent<ChildLocator>();
                if( base.isAuthority )
                {
                    //this.forwardDirection = -base.characterDirection.forward.normalized;
                    this.forwardDirection = base.GetAimRay().direction * -1;
                    this.forwardDirection.y = 0;
                    this.forwardDirection = Vector3.Normalize( this.forwardDirection );
                    if( this.forwardDirection == Vector3.zero ) this.forwardDirection = -base.characterDirection.forward.normalized;
                }


                this.animator.SetFloat( "rightSpeed", 0f, 0.1f, Time.fixedDeltaTime );
                this.animator.SetFloat( "forwardSpeed", -1f, 0.1f, Time.fixedDeltaTime );
                base.PlayAnimation( "Body", "DodgeBackward", "Dodge.playbackRate", duration );

                //Debuff thing here
                this.CalcSpeed();
                if( base.characterMotor && base.characterDirection )
                {
                    base.characterMotor.velocity.y = 0f;
                    base.characterMotor.velocity = this.forwardDirection * this.rollSpeed;
                }
                Vector3 b = base.characterMotor ? base.characterMotor.velocity : Vector3.zero;
                this.previousPosition = base.transform.position - b;

                if( base.isAuthority )
                {
                    new BlastAttack
                    {
                        attacker = base.gameObject,
                        baseDamage = base.damageStat * damageCoef,
                        baseForce = baseForce,
                        canHurtAttacker = false,
                        bonusForce = Vector3.down * downForce,
                        crit = base.RollCrit(),
                        damageColorIndex = DamageColorIndex.Default,
                        damageType = DamageType.Stun1s,
                        falloffModel = BlastAttack.FalloffModel.None,
                        losType = BlastAttack.LoSType.None,
                        position = Util.GetCorePosition( base.gameObject ),
                        procChainMask = default,
                        procCoefficient = procCoef,
                        radius = 8f,
                        teamIndex = base.teamComponent.teamIndex
                    }.Fire();
                }

                if( this.boxGroup )
                {
                    var boxTemp = this.boxGroup;
                    var deactivCounter = boxTemp.hurtBoxesDeactivatorCounter + 1;
                    boxTemp.hurtBoxesDeactivatorCounter = deactivCounter;
                }
            }

            public override void FixedUpdate()
            {
                base.FixedUpdate();
                this.CalcSpeed();

                if( base.cameraTargetParams )
                {
                    //FOV stuff happens here
                }

                Vector3 normal = (base.transform.position - this.previousPosition ).normalized;
                if( base.characterMotor && base.characterDirection && normal != Vector3.zero )
                {
                    Vector3 vec = normal * this.rollSpeed;
                    Single y = vec.y;
                    vec.y = 0;
                    Single d = Mathf.Max( Vector3.Dot( vec, this.forwardDirection ), 0f );
                    vec = this.forwardDirection * d;
                    vec.y += Mathf.Max( y, 0f );
                    base.characterMotor.velocity = vec;
                }
                this.previousPosition = base.transform.position;

                if( base.fixedAge >= duration && base.isAuthority )
                {
                    base.outer.SetNextStateToMain();
                    return;
                }
            }

            public override void OnExit()
            {
                if( this.boxGroup )
                {
                    var boxTemp = this.boxGroup;
                    var deactivCounter = boxTemp.hurtBoxesDeactivatorCounter - 1;
                    boxTemp.hurtBoxesDeactivatorCounter = deactivCounter;
                }

                if( base.cameraTargetParams )
                {
                    // FOV stuff
                }
                base.OnExit();
            }

            public override void OnSerialize( NetworkWriter writer )
            {
                base.OnSerialize( writer );
                writer.Write( this.forwardDirection );
            }

            public override void OnDeserialize( NetworkReader reader )
            {
                base.OnDeserialize( reader );
                this.forwardDirection = reader.ReadVector3();
            }

            private void CalcSpeed()
            {
                this.rollSpeed = base.moveSpeedStat * Mathf.Lerp( initSpeedCoef, endSpeedCoef, base.fixedAge / duration );
            }
        }
    }
}


