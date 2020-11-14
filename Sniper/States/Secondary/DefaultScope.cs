namespace Rein.Sniper.States.Secondary
{
    using System;
    using System.Runtime.CompilerServices;

    using EntityStates.BeetleQueenMonster;

    using RoR2;

    using Rein.Sniper.Data;
    using Rein.Sniper.States.Bases;

    using UnityEngine;
    using UnityEngine.Networking;

    internal class DefaultScope : ScopeBaseState
    {
        private const Single maxCharge = 1f;
        private const Single baseStartDelay = 0.5f;
        private const Single chargePerSecond = 0.3f;
        private const Single minModifier = 0f;
        private const Single maxModifier = 3f;
        private const Single speedScalar = 0.25f;

        private static readonly AnimationCurve damageCurve = new AnimationCurve( new[]
        {
            new Keyframe( 0f, minModifier, 0f, 1.5f, 0f, 0.25f ),
            new Keyframe( 1f, maxModifier, 0f, 0f, 0.2f, 0f ),
        });

        private Single startDelay;
        internal override Single currentCharge { get => base.shouldRunDelay ? (this.GetDamageMultiplier() - minModifier) / (maxModifier - minModifier) : 0f; }
        internal override Boolean isReady { get => this.delayTimer >= this.startDelay; }
        internal override Single readyFrac { get => Mathf.Clamp01(this.delayTimer / this.startDelay); }

        internal Single charge = 0f;


        private Single delayTimer = 0f;

        internal override Boolean OnFired()
        {
            if(this.isReady)
            {
                this.delayTimer = 0f;
                return true;
            }
            return false;
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
                //this.characterBody.AddBuff(BuffIndex.Slow50);
            }
            this.charge = base.startingCharge;

        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            base.characterBody.SetAimTimer(2f);
            if(!base.isAuthority) return;
            if(base.shouldRunDelay)
            {
                var dt = Time.fixedDeltaTime;
                if(this.delayTimer >= this.startDelay)
                {
                    switch(this.charge)
                    {
                        case Single s when s == maxCharge:
                            break;
                        case Single s when s < maxCharge:
                            this.charge += dt * chargePerSecond * base.characterBody.attackSpeed;
                            break;
                        default:
                            this.charge = maxCharge;
                            break;
                    }
                } else
                {
                    this.delayTimer += dt;
                }
            } else
            {
                this.delayTimer = 0f;
            }
        }

        public override void OnExit()
        {
            if(NetworkServer.active)
            {
                //base.characterBody.RemoveBuff(BuffIndex.Slow50);
            }
            base.OnExit();
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Single GetDamageMultiplier() => damageCurve.Evaluate(this.charge / maxCharge);

        internal override void ResetCharge()
        {
            this.charge = 0f;
        }
    }
}
