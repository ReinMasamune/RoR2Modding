using RoR2;
using UnityEngine;
using System.Collections.Generic;
using RoR2.Orbs;

namespace WispSurvivor.Orbs
{
    class BlazeOrb : RoR2.Orbs.Orb, IOrbFixedUpdateBehavior
    {
        //Blaze settings
        public float blazeTime = 1f;
        public float blazeFreq = 1f;
        public float blazeRadius = 1f;
        public float blazeMinDurToContinue = 1f;
        public float blazeDurationPerStack = 1f;

        //Ignite settings
        public float igniteDuration = 1f;
        public float igniteDebuffTimeMult = 1f;
        public float igniteTickDamage = 1f;
        public float igniteProcCoef = 1f;
        public float igniteTickFreq = 1f;
        public float igniteBaseStacksOnDeath = 1f;
        public float igniteStacksPerSecOnDeath = 1f;
        public float igniteExpireStacksMult = 1f;

        public DamageColorIndex igniteDamageColor = DamageColorIndex.Default;

        //Global settings
        public bool crit = false;

        public uint skin = 0;

        public bool isActive = true;

        public TeamIndex team;
        public Vector3 normal;

        public GameObject attacker;

        public List<IgnitionOrb> children;

        //Private stuff
        private float blazeResetInt;
        private float stacks = 0f;

        private Dictionary<HealthComponent, float> mask = new Dictionary<HealthComponent, float>();

        public override void Begin()
        {
            foreach (IgnitionOrb i in children)
            {
                if (i.isActive)
                {
                    i.parent = this;
                } else
                {
                    children.Remove(i);
                }
            }

            duration = blazeTime;

            isActive = true;

            Vector3 tangent = Vector3.forward;
            Vector3.OrthoNormalize(ref normal, ref tangent);

            EffectData effectData = new EffectData
            {
                origin = origin,
                genericFloat = duration,
                rotation = Quaternion.LookRotation(tangent, normal),
                scale = blazeRadius
            };
            EffectManager.instance.SpawnEffect(Modules.WispEffectModule.utilityFlames[skin], effectData, true);

            blazeResetInt = 1f / blazeFreq;
        }

        public void FixedUpdate()
        {
            Collider[] cols = Physics.OverlapSphere(origin, blazeRadius, LayerIndex.entityPrecise.mask, QueryTriggerInteraction.UseGlobal);

            HurtBox box;

            float curTime = Time.fixedTime;
            foreach (Collider col in cols)
            {
                if (!col) continue;
                box = col.GetComponent<HurtBox>();
                if (!box) continue;
                HealthComponent hcomp = box.healthComponent;
                if (!hcomp || !hcomp.alive || TeamComponent.GetObjectTeam(hcomp.gameObject) == team) continue;

                if (mask.ContainsKey(hcomp))
                {
                    if (mask[hcomp] > curTime) continue;
                    mask[hcomp] = curTime + blazeResetInt;
                    TickDamage(box);
                }else
                {
                    mask.Add(hcomp, curTime + blazeResetInt);
                    TickDamage(box);
                }
            }
        }

        public void OnArrival()
        {
            Debug.Log("Blaze orb had " + stacks.ToString() + " stacks.");

            float nextDuration = stacks * blazeDurationPerStack;

            if (nextDuration >= blazeMinDurToContinue)
            {
                BlazeOrb next = new BlazeOrb();
                next.attacker = attacker;
                next.blazeDurationPerStack = blazeDurationPerStack;
                next.blazeFreq = blazeFreq;
                next.blazeMinDurToContinue = blazeMinDurToContinue;
                next.blazeRadius = blazeRadius;
                next.blazeResetInt = blazeResetInt;
                next.blazeTime = nextDuration;
                next.children = children;
                next.crit = crit;
                next.igniteBaseStacksOnDeath = igniteBaseStacksOnDeath;
                next.igniteDamageColor = igniteDamageColor;
                next.igniteDebuffTimeMult = igniteDebuffTimeMult;
                next.igniteDuration = igniteDuration;
                next.igniteProcCoef = igniteProcCoef;
                next.igniteStacksPerSecOnDeath = igniteStacksPerSecOnDeath;
                next.igniteTickDamage = igniteTickDamage;
                next.igniteTickFreq = igniteTickFreq;
                next.igniteExpireStacksMult = igniteExpireStacksMult;
                next.normal = normal;
                next.origin = origin;
                next.skin = skin;
                next.stacks = 0;
                next.team = team;

                OrbManager.instance.AddOrb(next);
            }

            isActive = false;

        }

        public void AddStacks( float stacks )
        {
            stacks += stacks;
        }

        private void TickDamage(HurtBox enemy )
        {
            IgnitionOrb orb = new IgnitionOrb();

            orb.attacker = attacker;
            orb.crit = crit;
            orb.normal = Vector3.Normalize(enemy.transform.position - origin);
            orb.origin = enemy.transform.position;
            orb.skin = skin;
            orb.target = enemy;
            orb.team = team;
            orb.igniteDebuffTimeMult = igniteDebuffTimeMult;
            orb.igniteProcCoef = igniteProcCoef;
            orb.igniteTickDmg = igniteTickDamage;
            orb.igniteTickFreq = igniteTickFreq;
            orb.igniteTime = igniteDuration;
            orb.igniteDamageColor = igniteDamageColor;
            orb.igniteStacksPerSecOnDeath = igniteStacksPerSecOnDeath;
            orb.igniteBaseStacksOnDeath = igniteBaseStacksOnDeath;
            orb.igniteExpireStacksMult = igniteExpireStacksMult;
            orb.parent = this;
            


            RoR2.Orbs.OrbManager.instance.AddOrb(orb);
            children.Add(orb);
        }
    }
}