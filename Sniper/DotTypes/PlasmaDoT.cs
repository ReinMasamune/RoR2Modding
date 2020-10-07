namespace Rein.Sniper.DotTypes
{
    using RoR2;

    using System;

    using ReinCore;

    using UnityEngine.Networking;

    //Test implementation
    public struct Plasma : IDot<Plasma, Plasma.StackData, Plasma.UpdateContext, Plasma.PersistContext>
    {
        public Boolean sendToClients => false;
        const Single ticksPerSecond = 4f;
        const Single tickInterval = 1f / ticksPerSecond;
        const Single baseDamagePerSecond = 100f;
        const Single baseDamageMultiplier = baseDamagePerSecond / ticksPerSecond;

        public static void Inflict(HurtBox target, CharacterBody attacker, Single duration, Single multiplier)
        {
            var stack = new StackData(attacker, target, duration, multiplier);
            //...
        }


        public struct StackData : IDotStackData<Plasma, StackData, UpdateContext, PersistContext>
        {
            public Boolean shouldRemove { get; private set; }

            internal StackData(CharacterBody attackerBody, HurtBox partHit, Single duration, Single multiplier)
            {
                this.attackerBody = attackerBody;
                this.partHit = partHit;
                this.damageMultiplier = multiplier;
                this.remainingTicks = (Int32)(duration * ticksPerSecond);
                this.shouldRemove = false;
                this.tickTimer = 0f;
            }

            private Int32 remainingTicks;
            private CharacterBody attackerBody;
            private HurtBox partHit;
            private Single damageMultiplier;
            private Single tickTimer;

            public void Process(Single deltaTime, ref UpdateContext updateContext)
            {
                this.tickTimer += deltaTime;

                while(this.tickTimer >= tickInterval && this.remainingTicks > 0)
                {
                    this.tickTimer -= tickInterval;
                    this.remainingTicks--;
                    //Do damage here
                }
            }
            public void OnCleanseRecieved()
            {
                this.remainingTicks = 0;
            }

            //Unneeded in this context
            public void OnApplied(ref PersistContext ctx) { }
            public void OnExpired(ref PersistContext ctx) { }
            public void Deserialize(NetworkReader reader) { }
            public void Serialize(NetworkWriter writer) { }
        }

        public struct UpdateContext : IDotUpdateContext<Plasma, StackData, UpdateContext, PersistContext>
        {
            internal UpdateContext(PersistContext persistCtx) => this.persistContext = persistCtx;
            private PersistContext persistContext;
            internal CharacterBody targetBody => this.persistContext.targetBody;
        }

        public struct PersistContext : IDotPersistContext<Plasma, StackData, UpdateContext, PersistContext>
        {
            public CharacterBody targetBody { get; set; }
            public Boolean shouldRemove { get; private set; }
            public UpdateContext InitUpdateContext() => new(this);
            public void AllExpired() => this.shouldRemove = true;
            public void OnFirstStackApplied() { } // Apply debuff
            public void OnLastStackRemoved() { } // Remove debuff

            //Unneeded in this context
            public void HandleUpdateContext(in UpdateContext context) { }
        }
    }
}