using EntityStates;
using RoR2;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace RogueWispPlugin
{
#if ROGUEWISP
    internal partial class Main
    {
        public class FireHeatwave : BaseState
        {
            public static Single baseFireDelay = 0f;
            public static Single baseDamageMult = 3.0f;
            public static Single explosionRadius = 2f;
            public static Single falloffStart = 0.25f;
            public static Single endFalloffMult = 0.25f;

            public Single initAS;

            public Vector3 targetVec;
            public HurtBox target;

            public Boolean crit;

            public UInt32 skin = 0;


            private Single baseFireDuration = PrepHeatwave.baseFireDuration;
            private Single fireDuration;
            private Single fireDelay;
            private Single damageValue;

            private Boolean fired = false;

            private WispPassiveController passive;
            private ChildLocator childLoc;
            private ClientOrbController orbControl;

            public override void OnEnter()
            {
                base.OnEnter();
                this.orbControl = this.gameObject.GetComponent<ClientOrbController>();
                this.passive = this.gameObject.GetComponent<WispPassiveController>();
                this.skin = this.characterBody.skinIndex;
                this.childLoc = this.GetModelTransform().GetComponent<ChildLocator>();

                this.fireDuration = this.baseFireDuration / this.initAS;
                this.fireDelay = baseFireDelay / this.initAS;

                this.damageValue = this.damageStat * baseDamageMult;

                //RoR2.Util.PlaySound("Play_beetle_guard_attack2_initial", gameObject);
            }

            public override void FixedUpdate()
            {
                base.FixedUpdate();

                if( !this.fired )
                {
                    this.characterBody.isSprinting = false;
                }
                if( !this.fired && this.fixedAge >= this.fireDelay )
                {
                    this.FireOrb();
                }
                if( this.fixedAge >= this.fireDuration && this.isAuthority )
                {
                    if( this.inputBank && this.inputBank.skill1.down )
                    {
                        this.outer.SetNextState( new PrepHeatwave() );
                    } else
                    {
                        this.outer.SetNextState( new HeatwaveWindDown() );
                    }
                }
            }

            public override void OnExit() => base.OnExit();

            public override InterruptPriority GetMinimumInterruptPriority() => InterruptPriority.Skill;

            public override void OnSerialize( NetworkWriter writer )
            {
                base.OnSerialize( writer );
                writer.Write( HurtBoxReference.FromHurtBox( this.target ) );
                writer.Write( this.targetVec );
                writer.Write( this.initAS );
            }

            public override void OnDeserialize( NetworkReader reader )
            {
                base.OnDeserialize( reader );
                this.target = reader.ReadHurtBoxReference().ResolveHurtBox();
                this.targetVec = reader.ReadVector3();
                this.initAS = reader.ReadSingle();
            }

            private void FireOrb()
            {
                if( this.fired ) return;
                this.fired = true;
                if( !this.isAuthority ) return;

                HeatwaveClientOrb snap = new HeatwaveClientOrb();

                snap.damage = this.damageValue;
                snap.crit = this.RollCrit();
                snap.team = TeamComponent.GetObjectTeam( this.gameObject );
                snap.attacker = this.gameObject;
                snap.procCoef = 1.0f;
                snap.radius = explosionRadius;
                snap.skin = this.skin;
                Transform trans = this.childLoc.FindChild("MuzzleRight");
                snap.startPos = trans.position;
                snap.speed = 250f;
                snap.targetPos = this.targetVec;
                snap.chargeRestore = 5f;
                snap.force = 100f;
                snap.range = PrepHeatwave.maxRange;
                snap.falloffStart = falloffStart;
                snap.endFalloffMult = endFalloffMult;

                this.orbControl.AddClientOrb( snap );

                /*
                Orbs.SnapOrb snap = new Orbs.SnapOrb();
                snap.damage = this.damageValue;
                snap.crit = this.RollCrit();
                snap.team = TeamComponent.GetObjectTeam( this.gameObject );
                snap.attacker = this.gameObject;
                snap.procCoef = 1.0f;
                snap.radius = explosionRadius;
                snap.skin = this.skin;
                Transform trans = this.childLoc.FindChild("MuzzleRight");
                snap.origin = trans.position;
                snap.speed = 150f;
                if( this.target )
                {
                    snap.target = this.target;
                    snap.useTarget = true;
                    this.targetVec = this.target.transform.position;
                } else
                {
                    snap.useTarget = false;
                }
                snap.targetPos = this.targetVec;
                OrbManager.instance.AddOrb( snap );
                */
            }
        }
    }
#endif
}