using RoR2;
using RoR2.Orbs;
using UnityEngine;
using System;

namespace WispSurvivor.Orbs
{
    class IgnitionOrb : RoR2.Orbs.Orb, IOrbFixedUpdateBehavior
    {
        //Ignite settings
        public float igniteTime = 1f;
        public float igniteTickDmg = 1f;
        public float igniteTickFreq = 1f;
        public float igniteProcCoef = 1f;
        public float igniteDebuffTimeMult = 1f;
        public float igniteBaseStacksOnDeath = 0f;
        public float igniteStacksPerSecOnDeath = 1f;
        public float igniteExpireStacksMult = 1f;

        public DamageColorIndex igniteDamageColor = DamageColorIndex.Default;

        public BlazeOrb parent;

        //Global settings
        public bool crit = false;
        public bool isActive = true;

        public uint skin = 0;

        public TeamIndex team;

        public Vector3 normal;

        public GameObject attacker;

        //Private values
        private float damageInterval;
        private float damageTimer;
        private float durationTimer;

        private bool dead = false;
        private bool wasDead = false;
        private bool boosted = false;

        public override void Begin()
        {
            duration = igniteTime;
            isActive = true;

            Vector3 tangent = Vector3.forward;
            Vector3.OrthoNormalize(ref normal, ref tangent);

            EffectData effectData = new EffectData
            {
                origin = origin,
                genericFloat = duration,
                rotation = Quaternion.LookRotation(tangent, normal)
            };
            effectData.SetHurtBoxReference(target);

            EffectManager.instance.SpawnEffect(Modules.WispEffectModule.utilityBurns[skin], effectData, true);

            damageInterval = 1f / igniteTickFreq;
            damageTimer = damageInterval;
        }

        public void FixedUpdate()
        {
            try
            {
                boosted = parent.isActive && parent.isOwnerInside;
            } catch( NullReferenceException e )
            {
                boosted = false;
            }
            
            durationTimer += Time.fixedDeltaTime;

            if (!target) dead = true;
            if (!target.healthComponent) dead = true;
            if (!target.healthComponent.alive) dead = true;
            if (dead && !wasDead) OnDead();
            if (dead) return;

            damageTimer += Time.fixedDeltaTime * (boosted ? 1f : 0.5f);

            while (damageTimer >= damageInterval)
            {
                TickDamage(target);
                damageTimer -= damageInterval;
            }
        }

        public override void OnArrival()
        {
            if( isActive )
            {
                OnDead(igniteExpireStacksMult);
            }
        }

        private void TickDamage(HurtBox enemy)
        {
            if (!enemy) return;
            if (!enemy.healthComponent) return;
            if (!enemy.healthComponent.gameObject) return;
            if (!attacker) return;
            //Damage info stuff here
            DamageInfo d = new DamageInfo();
            d.damage = igniteTickDmg;
            d.attacker = attacker;
            d.inflictor = null;
            d.force = Vector3.zero;
            d.crit = crit;
            d.procChainMask = new ProcChainMask();
            d.procCoefficient = boosted ? igniteProcCoef : 0f;
            d.position = enemy.transform.position;
            d.damageColorIndex = igniteDamageColor;


            enemy.healthComponent.TakeDamage(d);
            GlobalEventManager.instance.OnHitEnemy(d, enemy.healthComponent.gameObject);
            GlobalEventManager.instance.OnHitAll(d, enemy.healthComponent.gameObject);
            parent.AddStacks(igniteDebuffTimeMult * (enemy.healthComponent.gameObject.GetComponent<CharacterBody>().hullClassification != HullClassification.Human ? 1f : 0.5f ));
        }

        private void OnDead( float mult = 1f)
        {
            wasDead = true;

            isActive = false;

            float value = mult * (igniteBaseStacksOnDeath + (durationTimer * igniteStacksPerSecOnDeath));

            if ( parent.isActive )
            {
                parent.AddStacks(value);
                parent.children.Remove(this);
            } else
            {
                //Do a cool thing here based on value (explosion I guess? modified will o wisp explosion could be fun...)
            }
        }

        private void Detonate()
        {
            //Implement this?
        }
    }
}
