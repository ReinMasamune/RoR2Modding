namespace Sniper.States.Secondary
{
    using System;
    using System.Runtime.CompilerServices;

    using EntityStates.BeetleQueenMonster;

    using RoR2;

    using Sniper.Data;
    using Sniper.States.Bases;

    using UnityEngine;
    using UnityEngine.Networking;

    internal class DefaultScope : ScopeBaseState
    {
        private const Single maxCharge = 1f;
        private const Single baseStartDelay = 0.5f;
        private const Single chargePerSecond = 0.4f;
        private const Single minModifier = 0f;
        private const Single maxModifier = 3f;
        private const Single speedScalar = 0.25f;

        private static readonly AnimationCurve damageCurve = new AnimationCurve( new[]
        {
            new Keyframe( 0f, minModifier, 0f, 1.5f, 0f, 0.25f ),
            new Keyframe( 1f, maxModifier, 0f, 0f, 0.2f, 0f ),
        });

        private Single startDelay;
        internal override Single currentCharge { get => (this.GetDamageMultiplier() - minModifier) / (maxModifier - minModifier); }
        internal override Boolean isReady { get => this.delayTimer >= this.startDelay; }
        internal override Single readyFrac { get => Mathf.Clamp01(this.delayTimer / this.startDelay); }

        internal Single charge = 0f;


        private Single delayTimer = 0f;

        internal override Boolean OnFired()
        {
            //this.charge = 0f;
            this.delayTimer = 0f;
            return base.fixedAge >= this.startDelay;
        }
        internal override BulletModifier ReadModifier()
        {
#pragma warning disable IDE0046 // Convert to conditional expression
#pragma warning disable IDE0011 // Add braces
            if(base.fixedAge >= this.startDelay)
            {
                return new BulletModifier
                {
                    damageMultiplier = this.GetDamageMultiplier(),
                    charge = this.charge,
                };
            } else return default;
#pragma warning restore IDE0011 // Add braces
#pragma warning restore IDE0046 // Convert to conditional expression
        }


        public override void OnEnter()
        {
            base.OnEnter();
            this.startDelay = baseStartDelay / base.attackSpeedStat;
            base.StartAimMode(2f);
            if(NetworkServer.active)
            {
                this.characterBody.AddBuff(BuffIndex.Slow50);
            }
            this.charge = base.startingCharge;

        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            base.characterBody.SetAimTimer(2f);
            //if(!base.isAuthority) return; // TODO: Uncomment after testing in multiplayer
            var dt = Time.fixedDeltaTime;
            if(this.delayTimer >= this.startDelay)
            {
                switch(this.charge)
                {
                    case Single s when s == maxCharge:
                        break;
                    case Single s when s < maxCharge:
                        var vel = base.characterMotor.velocity;
                        vel.y = vel.y * speedScalar;
                        vel.x *= vel.x * speedScalar;
                        vel.z *= vel.z * speedScalar;
                        this.charge += dt * chargePerSecond * base.characterBody.attackSpeed / (1f + vel.magnitude);
                        break;
                    default:
                        this.charge = maxCharge;
                        break;
                }
            } else
            {
                this.delayTimer += dt;
            }
        }

        public override void OnExit()
        {
            if(NetworkServer.active)
            {
                base.characterBody.RemoveBuff(BuffIndex.Slow50);
            }
            base.OnExit();
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Single GetDamageMultiplier() => damageCurve.Evaluate(this.charge / maxCharge);
    }
}
