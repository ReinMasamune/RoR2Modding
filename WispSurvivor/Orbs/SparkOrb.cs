using RoR2;
using UnityEngine;
using System.Collections.Generic;
using RoR2.Orbs;

namespace WispSurvivor.Orbs
{
    class SparkOrb : RoR2.Orbs.Orb
    {
        public float speed = 200f;
        public float damage = 1f;
        public float scale = 1f;
        public float procCoef = 1f;
        public float radius = 1f;
        public float stepHeight = 1f;
        public float stepDist = 1f;
        public float maxFall = 5f;
        public float minDistThreshold = 1f;
        public float height = 10f;
        public float vOffset = 5f;
        public float innerRadScale = 0.67f;
        public float edgePenaltyMult = 0.5f;

        public int stepsLeft = 0;
        public uint skin = 0;
        
        public Vector3 direction;

        public bool isFirst = false;
        public bool crit = false;


        public TeamIndex team;
        public DamageColorIndex damageColor;

        public GameObject attacker;
        public ProcChainMask procMask;

        private Vector3 dest;

        private bool last = false;

        public override void Begin()
        {
            Vector3 intermediate = origin + new Vector3(0f, stepHeight, 0f);
            intermediate += direction * stepDist * ( isFirst ? 0.5f : 1.0f );
            Vector3 dir = intermediate - origin;
            float dist = dir.magnitude;
            dir = Vector3.Normalize(dir);

            Ray r1 = new Ray
            {
                origin = origin,
                direction = dir
            };

            RaycastHit rh1;

            Vector3 top;

            if (Physics.SphereCast(r1, 0.25f, out rh1, dist, LayerIndex.world.mask, QueryTriggerInteraction.UseGlobal))
            {
                top = rh1.point + rh1.normal;
            }
            else
            {
                top = r1.GetPoint(dist);
            }

            Ray r2 = new Ray
            {
                origin = top,
                direction = Vector3.down
            };

            RaycastHit rh2;

            if (Physics.SphereCast(r2, 0.25f,  out rh2 , maxFall, LayerIndex.world.mask , QueryTriggerInteraction.UseGlobal ) )
            {
                dest = rh2.point + rh2.normal;
            }
            else
            {
                dest = r2.GetPoint(maxFall);
            }

            float distance = Vector3.Distance(dest, origin);

            duration = distance / speed;

            if (!isFirst && distance < minDistThreshold) last = true;

            //Effect is created here
        }

        public override void OnArrival()
        {
            //Explosion effect
            EffectData effect = new EffectData
            {
                origin = dest
            };

            EffectManager.instance.SpawnEffect(Modules.WispEffectModule.secondaryExplosions[skin], effect, true);

            if (attacker)
            {
                Dictionary<HealthComponent, bool> mask = new Dictionary<HealthComponent, bool>();
                HurtBox box;

                Collider[] cols = Physics.OverlapCapsule(dest, dest + new Vector3(0f, 20f, 0f), radius * innerRadScale, LayerIndex.entityPrecise.mask, QueryTriggerInteraction.UseGlobal);

                foreach (Collider col in cols)
                {
                    if (!col) continue;
                    box = col.GetComponent<HurtBox>();
                    if (!box) continue;
                    HealthComponent hcomp = box.healthComponent;
                    if (!hcomp || mask.ContainsKey(hcomp) || TeamComponent.GetObjectTeam(hcomp.gameObject) == team) continue;

                    DamageInfo dmg = new DamageInfo();
                    dmg.damage = damage;
                    dmg.attacker = attacker;
                    dmg.crit = crit;
                    dmg.damageColorIndex = damageColor;
                    dmg.damageType = DamageType.Generic;
                    dmg.force = (direction + Vector3.up) * 50f;
                    dmg.inflictor = attacker;
                    dmg.position = col.transform.position;
                    dmg.procChainMask = procMask;
                    dmg.procCoefficient = procCoef;

                    hcomp.TakeDamage(dmg);
                    GlobalEventManager.instance.OnHitEnemy(dmg, hcomp.gameObject);
                    GlobalEventManager.instance.OnHitAll(dmg, hcomp.gameObject);
                    mask.Add(hcomp, true);
                }

                cols = Physics.OverlapCapsule(dest, dest + new Vector3(0f, 20f, 0f), radius, LayerIndex.entityPrecise.mask, QueryTriggerInteraction.UseGlobal);

                foreach (Collider col in cols)
                {
                    if (!col) continue;
                    box = col.GetComponent<HurtBox>();
                    if (!box) continue;
                    HealthComponent hcomp = box.healthComponent;
                    if (!hcomp || mask.ContainsKey(hcomp) || TeamComponent.GetObjectTeam(hcomp.gameObject) == team) continue;

                    DamageInfo dmg = new DamageInfo();
                    dmg.damage = damage * edgePenaltyMult;
                    dmg.attacker = attacker;
                    dmg.crit = crit;
                    dmg.damageColorIndex = damageColor;
                    dmg.damageType = DamageType.Generic;
                    dmg.force = (direction + Vector3.up) * 10f;
                    dmg.inflictor = attacker;
                    dmg.position = col.transform.position;
                    dmg.procChainMask = procMask;
                    dmg.procCoefficient = procCoef * edgePenaltyMult;

                    hcomp.TakeDamage(dmg);
                    GlobalEventManager.instance.OnHitEnemy(dmg, hcomp.gameObject);
                    GlobalEventManager.instance.OnHitAll(dmg, hcomp.gameObject);
                    mask.Add(hcomp, true);
                }
            }

            if( stepsLeft > 0 && !last)
            {
                SparkOrb nextOrb = new SparkOrb();
                nextOrb.maxFall = maxFall;
                nextOrb.attacker = attacker;
                nextOrb.crit = crit;
                nextOrb.damage = damage;
                nextOrb.damageColor = damageColor;
                nextOrb.direction = direction;
                nextOrb.origin = dest;
                nextOrb.procCoef = procCoef;
                nextOrb.procMask = procMask;
                nextOrb.isFirst = false;
                nextOrb.radius = radius;
                nextOrb.scale = scale;
                nextOrb.speed = speed;
                nextOrb.stepDist = stepDist;
                nextOrb.stepHeight = stepHeight;
                nextOrb.stepsLeft = stepsLeft - 1;
                nextOrb.target = target;
                nextOrb.team = team;
                nextOrb.skin = skin;

                OrbManager.instance.AddOrb(nextOrb);
            }
        }


    }
}
