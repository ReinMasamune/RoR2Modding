using RoR2;
using UnityEngine;
using RoR2.Orbs;

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
            durationTimer += Time.fixedDeltaTime;

            if (!target) dead = true;
            if (!target.healthComponent) dead = true;
            if (!target.healthComponent.alive) dead = true;
            if (dead && !wasDead) OnDead();
            if (dead) return;

            damageTimer += Time.fixedDeltaTime;

            while (damageTimer >= damageInterval)
            {
                TickDamage(target);
                damageTimer -= damageInterval;
            }
        }

        public void OnArrival()
        {
            OnDead(igniteExpireStacksMult);
        }

        private void TickDamage(HurtBox enemy)
        {
            if (!enemy) return;
            if (!enemy.healthComponent) return;
            if (!enemy.healthComponent.gameObject) return;
            if (!attacker) return;
            BuffIndex b = BuffCatalog.FindBuffIndex("WispCurseBurn");
            enemy.healthComponent.gameObject.GetComponent<CharacterBody>().AddTimedBuff(b, (igniteTime - durationTimer ) * igniteDebuffTimeMult);
            //Damage info stuff here
            DamageInfo d = new DamageInfo();
            d.damage = igniteTickDmg;
            d.attacker = attacker;
            d.inflictor = null;
            d.force = Vector3.zero;
            d.crit = crit;
            d.procChainMask = new ProcChainMask();
            d.procCoefficient = igniteProcCoef;
            d.position = enemy.transform.position;
            d.damageColorIndex = igniteDamageColor;

            enemy.healthComponent.TakeDamage(d);
            GlobalEventManager.instance.OnHitEnemy(d, enemy.healthComponent.gameObject);
            GlobalEventManager.instance.OnHitAll(d, enemy.healthComponent.gameObject);
        }

        private void OnDead( float mult = 1f)
        {
            wasDead = true;

            isActive = false;

            if( parent.isActive )
            {
                parent.AddStacks(mult * (igniteBaseStacksOnDeath + (durationTimer * igniteStacksPerSecOnDeath)));
            }

            parent.AddStacks(1);
            //call back to the parent orb
        }
    }
}
