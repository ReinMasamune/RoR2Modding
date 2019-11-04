using RoR2;
using EntityStates;
using UnityEngine;
using System;
using RoR2.Orbs;
using UnityEngine.Networking;
using System.Linq;

namespace WispSurvivor.Skills.Primary
{
    public class FireHeatwave : BaseState
    {
        public static float baseFireDelay = 0.02125f;
        public static float baseDamageMult = 2.5f;
        public static float chargeScaler = 0.5f;
        public static float explosionRadius = 5f;

        public float initAS;

        public Vector3 targetVec;
        public HurtBox target;

        public bool crit;

        public uint skin = 0;


        private float baseFireDuration = PrepHeatwave.baseFireDuration;
        private float fireDuration;
        private float fireDelay;
        private float damageValue;

        private bool fired = false;

        private Components.WispPassiveController passive;
        private ChildLocator childLoc;

        public override void OnEnter()
        {
            base.OnEnter();
            passive = gameObject.GetComponent<Components.WispPassiveController>();
            skin = characterBody.skinIndex;
            childLoc = GetModelTransform().GetComponent<ChildLocator>();

            fireDuration = baseFireDuration / initAS;
            fireDelay = baseFireDelay / initAS;

            damageValue = damageStat * baseDamageMult * ((1f - chargeScaler) + chargeScaler * (float)((passive.ReadCharge() - 100.0) / 100.0));
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if( !fired )
            {
                characterBody.isSprinting = false;
            }
            if( !fired && fixedAge >= fireDelay )
            {
                FireOrb();
            }
            if( fixedAge >= fireDuration )
            {
                if( isAuthority )
                {
                    outer.SetNextStateToMain();
                }
            }
        }

        public override void OnExit()
        {
            base.OnExit();

        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }

        public override void OnSerialize(NetworkWriter writer)
        {
            base.OnSerialize(writer);
            writer.Write(HurtBoxReference.FromHurtBox(target));
            writer.Write(targetVec);
            writer.Write(initAS);
        }

        public override void OnDeserialize(NetworkReader reader)
        {
            base.OnDeserialize(reader);
            target = reader.ReadHurtBoxReference().ResolveHurtBox();
            targetVec = reader.ReadVector3();
            initAS = reader.ReadSingle();
        }

        private void FireOrb()
        {
            if (fired) return;
            fired = true;
            if (!NetworkServer.active) return;

            Orbs.SnapOrb snap = new Orbs.SnapOrb();
            snap.damage = damageValue;
            snap.crit = RollCrit();
            snap.team = TeamComponent.GetObjectTeam(gameObject);
            snap.attacker = gameObject;
            snap.procCoef = 1.0f;
            snap.radius = explosionRadius;
            snap.skin = skin;
            Transform trans = childLoc.FindChild("MuzzleRight");
            snap.origin = trans.position;
            snap.speed = 150f;
            if( target )
            {
                snap.target = target;
                snap.useTarget = true;
                targetVec = target.transform.position;
            }
            else
            {
                snap.useTarget = false;
            }
            snap.targetPos = targetVec;
            OrbManager.instance.AddOrb(snap);
        }
    }
}
