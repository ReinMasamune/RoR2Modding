namespace ReinCore
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Reflection;
    using System.Threading;

    using Mono.Cecil;
    using Mono.Cecil.Cil;

    using MonoMod.Cil;

    using RoR2;

    using UnityEngine;
    using UnityEngine.Networking;



    /* 
     * Network setup:
     * Inflict called anywhere
     * Message to server if needed
     * Server adds
     * Bool on dot type to require client sync, if true server sends message to all clients to add (eventually...)
     */

    internal static class CleanseReciever
    {
        static CleanseReciever()
        {
            HooksCore.RoR2.DotController.RemoveAllDots.On += RemoveAllDots_On;
        }

        private static void RemoveAllDots_On(HooksCore.RoR2.DotController.RemoveAllDots.Orig orig, CharacterBody target)
        {
            orig(target);
            onCleanseRecieved?.Invoke(target);
        }

        internal static event Action<CharacterBody> onCleanseRecieved;
    }


    internal static class DotController<TDot, TStackData, TUpdateContext, TPersistContext>
        where TDot : struct, IDot<TDot, TStackData, TUpdateContext, TPersistContext>
        where TStackData : struct, IDotStackData<TDot, TStackData, TUpdateContext, TPersistContext>
        where TUpdateContext : struct, IDotUpdateContext<TDot, TStackData, TUpdateContext, TPersistContext>
        where TPersistContext : struct, IDotPersistContext<TDot, TStackData, TUpdateContext, TPersistContext>
    {
        internal static void ServerInflictDot(CharacterBody target, TStackData stackInfo, CharacterBody attacker)
        {
            var id = target.netId;
            if(id == NetworkInstanceId.Invalid) throw new ArgumentException("Invalid target");
            if(!bodySpecific.TryGetValue(id, out var ctx))
            {
                var persistCtx = new TPersistContext();
                persistCtx.targetBody = target;
                persistCtx.OnFirstStackApplied();
                ctx = (persistCtx, LinkListPool<TStackData>.item);
            }
            var (persist, list) = ctx;
            stackInfo.OnApplied(ref persist);
            list.Append(stackInfo);
            bodySpecific[id] = (persist, list);
            if(!updateRegistered) RegisterUpdate();
        }

        private static readonly Dictionary<NetworkInstanceId,(TPersistContext,LinkList<TStackData>)> bodySpecific = new();
        private static Boolean updateRegistered = false;

        private static void RegisterUpdate()
        {
            updateRegistered = true;
            RoR2Application.onFixedUpdate += FixedUpdate;
            CleanseReciever.onCleanseRecieved += HandleCleanse;
        }
        private static void UnregisterUpdate()
        {
            updateRegistered = false;
            RoR2Application.onFixedUpdate -= FixedUpdate;
            CleanseReciever.onCleanseRecieved -= HandleCleanse;
        }

        private static void HandleCleanse(CharacterBody target)
        {
            if(bodySpecific.TryGetValue(target.netId, out var val))
            {
                var (persist, list) = val;
                var node = list.first;
                while(node is not null)
                {
                    node.item.OnCleanseRecieved();
                    node = node.next;
                }
            }
        }


        // ZERO ALLOCATIONS, HA
        private unsafe static void FixedUpdate()
        {
            var delta = Time.fixedDeltaTime;

            //Make a copy of the keys collection because we need to enumerate it multiple times, and also need to mutate the dictionary during the second loop
            var keyCount = bodySpecific.Count;
            NetworkInstanceId* keys = stackalloc NetworkInstanceId[keyCount];
            using(var enumerator = bodySpecific.Keys.GetEnumerator())
            {
                Int32 i = 0;
                while(enumerator.MoveNext())
                {
                    keys[i++] = enumerator.Current;
                }
            }
                
            //Primary enumeration, actually processes the dots
            for(Int32 i = 0; i < keyCount; ++i)
            {
                var key = keys[i];
                var (persist, list) = bodySpecific[key];

                var updateContext = persist.InitUpdateContext();

                //Process all stacks
                var curNode = list.first;
                while(curNode is not null)
                {
                    curNode.item.Process(delta, ref updateContext);
                    var old = curNode;
                    curNode = curNode.next;
                    if(old.item.shouldRemove)
                    {
                        old.item.OnExpired(ref persist);
                        old.Remove();
                    }
                }

                persist.HandleUpdateContext(in updateContext);

                if(list.count == 0) persist.AllExpired();

                //Need to commit any changes in the state of the persist context back into dictionary because persist is a value type
                bodySpecific[key] = (persist, list);
            }


            //Secondary enumeration, this removes and cleans up anything that is flagged for removal after processing.
            for(Int32 i = 0; i < keyCount; ++i)
            {
                var key = keys[i];
                var (persist, list) = bodySpecific[key];
                if(!persist.shouldRemove) continue;

                //Inform any leftover stacks that they are being removed and then remove them.
                var curNode = list.first;
                while(curNode is not null)
                {
                    curNode.item.OnExpired(ref persist);
                    var old = curNode;
                    curNode = curNode.next;
                    old.Remove();
                }
                //Returns the list to the pool
                LinkListPool<TStackData>.item = list;
                persist.OnLastStackRemoved();
                bodySpecific.Remove(key);
            }
        }
    }




    public interface IDot
    {
        // TODO: Implement
        Boolean sendToClients { get; }
    }
    public interface IDot<TDot, TStackData, TUpdateContext, TPersistContext> : IDot
        where TDot : struct, IDot<TDot, TStackData, TUpdateContext, TPersistContext>
        where TStackData : struct, IDotStackData<TDot, TStackData, TUpdateContext, TPersistContext>
        where TUpdateContext : struct, IDotUpdateContext<TDot, TStackData, TUpdateContext, TPersistContext>
        where TPersistContext : struct, IDotPersistContext<TDot, TStackData, TUpdateContext, TPersistContext>
    {
    }

    public interface IDotStackData
    {
        Boolean shouldRemove { get; }
    }
    public interface IDotStackData<TDot, TStackData, TUpdateContext, TPersistContext> : IDotStackData, ISerializableObject
        where TDot : struct, IDot<TDot, TStackData, TUpdateContext, TPersistContext>
        where TStackData : struct, IDotStackData<TDot, TStackData, TUpdateContext, TPersistContext>
        where TUpdateContext : struct, IDotUpdateContext<TDot, TStackData, TUpdateContext, TPersistContext>
        where TPersistContext : struct, IDotPersistContext<TDot, TStackData, TUpdateContext, TPersistContext>
    {
        void OnApplied(ref TPersistContext ctx);
        void OnExpired(ref TPersistContext ctx);

        void Process(Single deltaTime, ref TUpdateContext updateContext);
        void OnCleanseRecieved();
    }
    public interface IDotUpdateContext
    {
        
    }
    public interface IDotUpdateContext<TDot, TStackData, TUpdateContext, TPersistContext> : IDotUpdateContext
        where TDot : struct, IDot<TDot, TStackData, TUpdateContext, TPersistContext>
        where TStackData : struct, IDotStackData<TDot, TStackData, TUpdateContext, TPersistContext>
        where TUpdateContext : struct, IDotUpdateContext<TDot, TStackData, TUpdateContext, TPersistContext>
        where TPersistContext : struct, IDotPersistContext<TDot, TStackData, TUpdateContext, TPersistContext>
    {
    }
    public interface IDotPersistContext
    {
        CharacterBody targetBody { get; set; }
        Boolean shouldRemove { get; }

        void OnFirstStackApplied();
        void OnLastStackRemoved();
        void AllExpired();
    }
    public interface IDotPersistContext<TDot, TStackData, TUpdateContext, TPersistContext> : IDotPersistContext
        where TDot : struct, IDot<TDot, TStackData, TUpdateContext, TPersistContext>
        where TStackData : struct, IDotStackData<TDot, TStackData, TUpdateContext, TPersistContext>
        where TUpdateContext : struct, IDotUpdateContext<TDot, TStackData, TUpdateContext, TPersistContext>
        where TPersistContext : struct, IDotPersistContext<TDot, TStackData, TUpdateContext, TPersistContext>
    {


        TUpdateContext InitUpdateContext();
        void HandleUpdateContext(in TUpdateContext context);
    }



    internal static class DotInfo<TDot>
        where TDot : struct, IDot
    {
        internal static Boolean sendToClients => new TDot().sendToClients;
    }

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