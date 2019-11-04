using RoR2;
using UnityEngine;

namespace WispSurvivor.Orbs
{
    class SnapOrb : RoR2.Orbs.Orb
    {
        public float speed = 200f;
        public float damage = 1f;
        public float scale = 1f;
        public float procCoef = 1f;
        public float radius = 1f;
        public uint skin = 0;

        public Vector3 targetPos;

        public bool crit = false;
        public bool useTarget = false;

        public TeamIndex team;
        public DamageColorIndex damageColor;

        public GameObject attacker;
        public ProcChainMask procMask;

        private Vector3 lastPos;

        public override void Begin()
        {
            if( !target )
            {
                useTarget = false;
            }

            if( useTarget )
            {
                lastPos = target.transform.position;
            }
            else
            {
                lastPos = targetPos;
            }

            duration = Vector3.Distance(lastPos, origin) / speed + 0.15f;
            EffectData effectData = new EffectData
            {
                origin = origin,
                genericFloat = duration,
                genericBool = useTarget,
                start = lastPos
            };
            effectData.SetHurtBoxReference(target);
            EffectManager.instance.SpawnEffect(Modules.WispEffectModule.primaryOrbEffects[skin], effectData, true);
        }

        public override void OnArrival()
        {
            if (useTarget)
            {
                lastPos = targetPos;
            }
            else
            {
                lastPos = targetPos;
            }
            EffectData effect = new EffectData
            {
                origin = lastPos,
                scale = 0.5f
            };

            EffectManager.instance.SpawnEffect(Modules.WispEffectModule.primaryExplosionEffects[skin] , effect , true);

            if ( attacker )
            {
                new BlastAttack
                {
                    attacker = attacker,
                    baseDamage = damage,
                    baseForce = 0f,
                    bonusForce = Vector3.zero,
                    canHurtAttacker = false,
                    crit = crit,
                    damageColorIndex = damageColor,
                    damageType = DamageType.Generic,
                    falloffModel = BlastAttack.FalloffModel.None,
                    inflictor = null,
                    position = lastPos,
                    procChainMask = procMask,
                    procCoefficient = procCoef,
                    radius = radius,
                    teamIndex = team
                }.Fire();
            }
        }
    }
}
