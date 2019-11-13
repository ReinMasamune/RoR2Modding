using RoR2;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Reflection;
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
        public float blazeOrbBundleSendFreq = 1f;
        public float blazeStackExchangeRate = 1f;

        public uint blazeOrbStackBundleSize = 1;

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
        public bool isOwnerInside = false;

        public TeamIndex team;
        public Vector3 normal;

        public GameObject attacker;

        public List<IgnitionOrb> children;

        //Private stuff
        private float blazeResetInt;
        private float bundleSendInt;
        private float curTime;
        private float bundleSendTimer = 0f;
        private float stacks = 0f;

        private HurtBox attackerBox;

        private CharacterBody attackerBody;

        private BuffIndex b;

        private Dictionary<HealthComponent, float> mask = new Dictionary<HealthComponent, float>();

        public override void Begin()
        {
            b = BuffCatalog.FindBuffIndex("WispCurseBurn");

            foreach (IgnitionOrb i in children)
            {
                if (i.isActive)
                {
                    i.parent = this;
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
            bundleSendInt = 1f / blazeOrbBundleSendFreq;

            attackerBody = attacker.GetComponent<CharacterBody>();
            attackerBox = attackerBody.mainHurtBox;
        }

        public void FixedUpdate()
        {
            isOwnerInside = attacker && Vector3.Distance(attacker.transform.position, origin) <= blazeRadius;

            if (isOwnerInside) bundleSendTimer += Time.fixedDeltaTime;

            if (isOwnerInside && stacks >= blazeOrbStackBundleSize && bundleSendTimer >= bundleSendInt) SendStackBundle();

            if (isOwnerInside && !attackerBody.HasBuff(BuffIndex.EnrageAncientWisp)) attackerBody.AddBuff(BuffIndex.EnrageAncientWisp);
            if (!isOwnerInside && attackerBody.HasBuff(BuffIndex.EnrageAncientWisp)) attackerBody.RemoveBuff(BuffIndex.EnrageAncientWisp);

            Collider[] cols = Physics.OverlapSphere(origin, blazeRadius, LayerIndex.entityPrecise.mask, QueryTriggerInteraction.UseGlobal);

            HurtBox box;

            curTime = Time.fixedTime;
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

        public override void OnArrival()
        {
            base.OnArrival();
            //Ceil stacks to int and send if owner is in radius

            if( isOwnerInside && stacks > 0 )
            {
                RestoreOrb orb = new RestoreOrb();
                orb.stacks = (uint)Mathf.CeilToInt((stacks) / blazeMinDurToContinue);
                orb.length = blazeDurationPerStack;
                orb.origin = origin;
                orb.skin = skin;
                orb.target = attackerBox;

                OrbManager.instance.AddOrb(orb);
            }

            if (attackerBody.HasBuff(BuffIndex.EnrageAncientWisp)) attackerBody.RemoveBuff(BuffIndex.EnrageAncientWisp);

            isActive = false;
         
        }

        public void AddStacks( float stacks )
        {
            this.stacks += stacks;
        }

        private void SendStackBundle()
        {
            bundleSendTimer -= bundleSendInt;
            stacks -= blazeOrbStackBundleSize;
            var bonus = Mathf.FloorToInt(stacks * blazeStackExchangeRate);
            stacks -= bonus;

            if (bundleSendTimer > 0f) bundleSendTimer *= 0.25f;

            RestoreOrb orb = new RestoreOrb();
            orb.stacks = (uint)Mathf.CeilToInt(blazeOrbStackBundleSize + bonus);
            orb.length = blazeDurationPerStack;
            orb.origin = origin;
            orb.skin = skin;
            orb.target = attackerBox;

            OrbManager.instance.AddOrb(orb);
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

            enemy.healthComponent.gameObject.GetComponent<CharacterBody>().AddTimedBuff(b, igniteDuration);
        }
    }
}