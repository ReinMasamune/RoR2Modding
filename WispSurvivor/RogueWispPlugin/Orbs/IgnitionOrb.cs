using System;

using RoR2;
using RoR2.Orbs;

using UnityEngine;

namespace Rein.RogueWispPlugin
{
    internal partial class Main
    {
        internal class IgnitionOrb : RoR2.Orbs.Orb, IOrbFixedUpdateBehavior
        {
            //Ignite settings
            public Single igniteTime = 1f;
            public Single igniteTickDmg = 1f;
            public Single igniteTickFreq = 1f;
            public Single igniteProcCoef = 1f;
            public Single igniteStacksPerTick = 1f;
            public Single igniteBaseStacksOnDeath = 0f;
            public Single igniteDeathStacksMult = 1f;
            public Single igniteExpireStacksMult = 1f;

            public DamageColorIndex igniteDamageColor = DamageColorIndex.Default;

            public BlazeOrb parent;

            //Global settings
            public Boolean crit = false;
            public Boolean isActive = true;

            public UInt32 skin = 0;

            public TeamIndex team;

            public Vector3 normal;
            public Vector3 targetPos;

            public GameObject attacker;

            //Private values
            private Single damageInterval;
            private Single damageTimer;
            private Single durationTimer;
            private Single chargeMult;

            private Boolean dead = false;
            private Boolean wasDead = false;
            private Boolean boosted = false;
            private Boolean firstTick = true;

            public override void Begin()
            {
                this.duration = this.igniteTime;
                this.isActive = true;

                Vector3 tangent = Vector3.forward;
                Vector3.OrthoNormalize(ref this.normal, ref tangent);

                this.damageInterval = 1f / this.igniteTickFreq;
                this.damageTimer = this.damageInterval;

                this.chargeMult = Main.WispPassiveController.GetMultiplierForBody(target?.healthComponent?.body);
            }

            public void FixedUpdate()
            {
                try
                {
                    this.boosted = this.parent.isActive && this.parent.isOwnerInside;
                } catch(NullReferenceException)
                {
                    this.boosted = false;
                }

                this.durationTimer += Time.fixedDeltaTime;

                if(!this.target) this.dead = true;
                if(!this.target.healthComponent) this.dead = true;
                if(!this.target.healthComponent.alive) this.dead = true;
                if(this.dead && !this.wasDead) this.OnDead();
                if(this.dead) return;

                this.targetPos = this.target.transform.position;

                this.damageTimer += Time.fixedDeltaTime * (this.boosted ? 1f : 0.5f);

                while(this.damageTimer >= this.damageInterval)
                {
                    this.TickDamage(this.target);
                    this.damageTimer -= this.damageInterval;
                }
            }

            public override void OnArrival()
            {
                if(this.isActive)
                {
                    this.OnDead(this.igniteExpireStacksMult);
                }
            }

            private void TickDamage(HurtBox enemy)
            {
                if(!enemy) return;
                if(!enemy.healthComponent) return;
                if(!enemy.healthComponent.gameObject) return;
                if(!this.attacker) return;
                //Damage info stuff here
                DamageInfo d = new DamageInfo();
                d.damage = this.igniteTickDmg;
                d.attacker = this.attacker;
                d.inflictor = null;
                d.force = Vector3.zero;
                d.crit = this.crit;
                d.procChainMask = new ProcChainMask();
                d.procCoefficient = (this.boosted ? this.igniteProcCoef : 0f) * (this.firstTick ? 1f : 1f);
                d.position = enemy.transform.position;
                d.damageColorIndex = this.igniteDamageColor;

                this.firstTick = false;

                enemy.healthComponent.TakeDamage(d);
                GlobalEventManager.instance.OnHitEnemy(d, enemy.healthComponent.gameObject);
                GlobalEventManager.instance.OnHitAll(d, enemy.healthComponent.gameObject);
                this.parent.AddStacks(this.igniteStacksPerTick * this.chargeMult, this);
            }

            private void OnDead(Single mult = 1f)
            {
                this.wasDead = true;

                this.isActive = false;

                Single value = mult * (this.igniteBaseStacksOnDeath + ((this.igniteTime - this.durationTimer) * this.igniteTickFreq * this.igniteStacksPerTick * this.igniteDeathStacksMult));

                if(this.parent.isActive)
                {
                    this.parent.AddStacks(value * this.chargeMult, this);
                    this.parent.children.Remove(this);
                } else
                {
                    //Do a cool thing here based on value (explosion I guess? modified will o wisp explosion could be fun...)
                }
            }
        }
    }
}