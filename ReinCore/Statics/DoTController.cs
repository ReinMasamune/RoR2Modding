namespace ReinCore
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Security.Cryptography;
    using System.Threading;

    using Mono.Cecil;
    using Mono.Cecil.Cil;
    using Mono.Security.Authenticode;

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
        internal static Boolean loaded { get; private set; }
        static CleanseReciever()
        {
            HooksCore.RoR2.DotController.RemoveAllDots.On += RemoveAllDots_On;
            loaded = true;
        }

        private static void RemoveAllDots_On(HooksCore.RoR2.DotController.RemoveAllDots.Orig orig, GameObject target)
        {
            orig(target);
            var body = target?.GetComponent<CharacterBody>();
            Invoke(body);
        }


        internal static void Invoke(CharacterBody body)
        {
            InvokeRecieved(body);
            new CleanseMessage(body).Send(NetworkDestination.Clients | NetworkDestination.Server);
        }

        internal static void InvokeRecieved(CharacterBody body)
        {
            if(body is null) return;
            onCleanseRecieved?.Invoke(body);
        }

        internal static event Action<CharacterBody> onCleanseRecieved;
    }

    internal struct CleanseMessage : INetMessage
    {
        private CharacterBody target;

        internal CleanseMessage(CharacterBody target)
        {
            this.target = target;
        }

        public void Deserialize(NetworkReader reader)
        {
            this.target = reader.ReadNetworkIdentity().GetComponent<CharacterBody>();
        }

        public void OnRecieved()
        {
            CleanseReciever.InvokeRecieved(this.target);
        }

        public void Serialize(NetworkWriter writer)
        {
            writer.Write(this.target.networkIdentity);
        }
    }


    internal unsafe struct DotMessage : INetMessage
    {
        internal DotMessage(CharacterBody target, DotCatalog.Index dotIndex, Byte[] bytes)
        {
            this.target = target;
            this.dotIndex = dotIndex;
            this.bytes = bytes;
        }
        private CharacterBody target;
        private DotCatalog.Index dotIndex;

        internal Byte[] bytes;

        public void Serialize(NetworkWriter writer)
        {
            writer.Write(this.target.networkIdentity);
            writer.Write((UInt64)this.dotIndex);
            writer.WriteBytesFull(this.bytes);
        }

        public void Deserialize(NetworkReader reader)
        {
            var id = reader.ReadNetworkIdentity();
            this.dotIndex = (DotCatalog.Index)reader.ReadUInt64();
            if(!DotCatalog.TryGetDef(this.dotIndex, out var def))
            {
                Log.Fatal($"Unable to find dot with index {this.dotIndex}, this indicates that you are experiencing serious desync and need to stop playing. Crashes are likely");
                return;
            }
            this.bytes = def.tempStackSizedArray;
            var l = reader.ReadUInt16();

            if(!id || id.GetComponent<CharacterBody>() is not CharacterBody body)
            {
                Log.Fatal("No matching networkidentity found. This indicates some kind of desync is going on and you should maybe end your run now.");
                return;
            }
            this.target = body;
    
            if(l != this.bytes.Length)
            {
                Log.Fatal("Mismatched buffer sizes for sent dot info. Ensure everyone is on the same version. This is a fatal error, meaning that there is zero way to recover internally, you should close your game.");
                return;
            }

            if(this.bytes is null)
            {
                Log.Fatal("Unregistered dot type recieved in message. This is going to cause desync and you should maybe just close the game now?");
                return;
            }
            for(Int32 i = 0; i < this.bytes.Length; ++i)
            {
                this.bytes[i] = reader.ReadByte();
            }
        }

        public void OnRecieved()
        {
            var def = DotCatalog.GetDef(this.dotIndex);
            if(def is null)
            {
                Log.Fatal("Unregistered dot type recieved in message. This is going to cause desync and you should maybe just close the game now?");
                return;
            }
            def.Apply(this.target, this.bytes);
        }


    }


    public static class DotController<TDot, TStackData, TUpdateContext, TPersistContext>
        where TDot : struct, IDot<TDot, TStackData, TUpdateContext, TPersistContext>
        where TStackData : struct, IDotStackData<TDot, TStackData, TUpdateContext, TPersistContext>, IByteSerializableObject
        where TUpdateContext : struct, IDotUpdateContext<TDot, TStackData, TUpdateContext, TPersistContext>
        where TPersistContext : struct, IDotPersistContext<TDot, TStackData, TUpdateContext, TPersistContext>
    {
        private static readonly String dotGuid = typeof(TDot).AssemblyQualifiedName;
        private static readonly Boolean processOnClients = new TDot().processOnClients;
        private static readonly UInt32 stackSize = new TStackData().size;
        private static DotCatalog.Entry? catalogEntry;
        private static DotCatalog.RegistrationToken? token;

        public struct ArraySizeDef : IArraySizeDef
        {
            public UInt32 size => stackSize;
        }

        private static Byte[] stackSizedByteArray
        {
            get => ArrayPool<Byte, ArraySizeDef>.item;
            set => ArrayPool<Byte, ArraySizeDef>.item = value;
        }



        public static void Register()
        {
            if(token is not null)
            {
                Log.Error($"Attempted to register Dot {typeof(TDot).FullName} when it was already registered.");
                return;
            }
            token = DotCatalog.Add(new Def());

            if(!CleanseReciever.loaded)
            {
                Log.Error($"{nameof(CleanseReciever)} is not loaded.");
            }
        }

        internal struct Def : IDotDef
        {
            public String guid => dotGuid;
            public DotCatalog.Entry? entry { get => catalogEntry; set => catalogEntry = value; }

            public Byte[] tempStackSizedArray
            {
                get => stackSizedByteArray;
                set => stackSizedByteArray = value;
            }

            public void Apply(CharacterBody target, Byte[] bytes)
            {
                TStackData stack = new();
                RecieveInflictDot(target, stack.Deserialize(bytes));
            }
        }

        public static unsafe void InflictDot(CharacterBody target, TStackData stackInfo)
        {
            //RecieveInflictDot(target, stackInfo);
            var ind = catalogEntry?.index ?? throw new ArgumentException("Unregistered dot type");
            var data = stackSizedByteArray;
            stackInfo.Serialize(data);
            var msg = new DotMessage(target, ind, data);
            msg.Send(NetworkDestination.Clients | NetworkDestination.Server);
            msg.bytes = null;
            stackSizedByteArray = data;
        }

        internal static void RecieveInflictDot(CharacterBody target, TStackData stackInfo)
        {
            if(!NetworkServer.active && !processOnClients) return;

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
        Boolean processOnClients { get; }
    }
    public interface IDot<TDot, TStackData, TUpdateContext, TPersistContext> : IDot
        where TDot : struct, IDot<TDot, TStackData, TUpdateContext, TPersistContext>
        where TStackData : struct, IDotStackData<TDot, TStackData, TUpdateContext, TPersistContext>, IByteSerializableObject
        where TUpdateContext : struct, IDotUpdateContext<TDot, TStackData, TUpdateContext, TPersistContext>
        where TPersistContext : struct, IDotPersistContext<TDot, TStackData, TUpdateContext, TPersistContext>
    {
    }

    public interface IDotStackData : ISerializableObject
    {
        Boolean shouldRemove { get; }
        void OnCleanseRecieved();
    }
    public interface IDotStackData<TDot, TStackData, TUpdateContext, TPersistContext> : IDotStackData, IByteSerializableObject
        where TDot : struct, IDot<TDot, TStackData, TUpdateContext, TPersistContext>
        where TStackData : struct, IDotStackData<TDot, TStackData, TUpdateContext, TPersistContext>, IByteSerializableObject
        where TUpdateContext : struct, IDotUpdateContext<TDot, TStackData, TUpdateContext, TPersistContext>
        where TPersistContext : struct, IDotPersistContext<TDot, TStackData, TUpdateContext, TPersistContext>
    {
        void OnApplied(ref TPersistContext ctx);
        void OnExpired(ref TPersistContext ctx);

        void Process(Single deltaTime, ref TUpdateContext updateContext);      
    }
    public interface IDotUpdateContext
    {
        
    }
    public interface IDotUpdateContext<TDot, TStackData, TUpdateContext, TPersistContext> : IDotUpdateContext
        where TDot : struct, IDot<TDot, TStackData, TUpdateContext, TPersistContext>
        where TStackData : struct, IDotStackData<TDot, TStackData, TUpdateContext, TPersistContext>, IByteSerializableObject
        where TUpdateContext : struct, IDotUpdateContext<TDot, TStackData, TUpdateContext, TPersistContext>
        where TPersistContext : struct, IDotPersistContext<TDot, TStackData, TUpdateContext, TPersistContext>
    {
    }
    public interface IDotPersistContext
    {
        UnityRef<CharacterBody> targetBody { get; set; }
        Boolean shouldRemove { get; }
        void OnFirstStackApplied();
        void OnLastStackRemoved();
        void AllExpired();
    }
    public interface IDotPersistContext<TDot, TStackData, TUpdateContext, TPersistContext> : IDotPersistContext
        where TDot : struct, IDot<TDot, TStackData, TUpdateContext, TPersistContext>
        where TStackData : struct, IDotStackData<TDot, TStackData, TUpdateContext, TPersistContext>, IByteSerializableObject
        where TUpdateContext : struct, IDotUpdateContext<TDot, TStackData, TUpdateContext, TPersistContext>
        where TPersistContext : struct, IDotPersistContext<TDot, TStackData, TUpdateContext, TPersistContext>
    {
        TUpdateContext InitUpdateContext();
        void HandleUpdateContext(in TUpdateContext context);
    }
}