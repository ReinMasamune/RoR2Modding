using ReinCore;
[assembly: Catalog(typeof(OrbSerializerCatalog))]
namespace ReinCore
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using RoR2;
    using RoR2.Orbs;
    using UnityEngine;
    using UnityEngine.Networking;
    using Object = System.Object;
    using UnityObject = UnityEngine.Object;
    public sealed class OrbSerializerCatalog : Catalog<OrbSerializerCatalog, IOrbSerializer>
    {
        public override String guid => "Rein.OrbSerializerCatalog";
        protected internal override Int32 order => 0;
        protected override void FirstInitSetup()
        {
            OrbCatalog.indexToType = Array.Empty<Type>();
            OrbCatalog.typeToIndex = new();
            OrbCatalog.GenerateCatalog();

            VanillaOrbSerializers.AddAll();
            NetworkCore.RegisterMessageType<OrbMessage>();
            base.FirstInitSetup();
        }
        internal static IOrbSerializer OrbTypeToDefaultSerializer(Type t)
        {
            if(t.GetInterfaces().Contains(typeof(ISerializableObject)) && t.GetConstructor(Type.EmptyTypes) is not null)
            {
                try
                {
                    return (IOrbSerializer)Activator.CreateInstance(typeof(SerializableOrbSerializer<>).MakeGenericType(t));
                } catch { }
            }
            return new NoSerializer(t.FullName, t);
        }
        protected override IEnumerable<IOrbSerializer?> GetBaseEntries() => base.GetBaseEntries().Concat(OrbCatalog.indexToType.Select(OrbTypeToDefaultSerializer));
        protected override void ProcessAllDefinitions(IOrbSerializer[] definitions)
        {
            base.ProcessAllDefinitions(definitions);
            foreach(var def in definitions) typeof(OrbIndex<>).MakeGenericType(def.targetType).GetProperty(nameof(OrbIndex<Orb>.index)).GetSetMethod(true).Invoke(null, new Object[] { def.entry.index, });
        }
    }
    public static class OrbIndex<TOrb>
        where TOrb : Orb
    {
        public static OrbSerializerCatalog.Index index { get; internal set; } = OrbSerializerCatalog.Index.Invalid;
    }
    public interface IOrbSerializer : OrbSerializerCatalog.ICatalogDef, ISerializer<Orb>
    {
        Orb CreateInstance();
        Type targetType { get; }
    }
    internal struct OrbMessage : INetMessage
    {
        internal OrbMessage(Orb orb, IOrbSerializer serializer)
        {
            this.orb = orb;
            this.serializer = serializer;
        }
        private Orb orb;
        private IOrbSerializer serializer;
        public void OnRecieved() => OrbManager.instance.AddOrb(this.orb);
        public void Deserialize(NetworkReader reader)
        {
            this.serializer = OrbSerializerCatalog.GetDef(reader.ReadBits<OrbSerializerCatalog.Index>())!;
            this.orb = this.serializer.CreateInstance();
            reader.Read(ref this.orb, this.serializer);
        }
        public void Serialize(NetworkWriter writer)
        {
            writer.WriteBits((OrbSerializerCatalog.Index)this.serializer.entry?.index!);
            writer.Write(this.orb, this.serializer);
        }
    }
    internal struct NoSerializer : IOrbSerializer
    {
        internal NoSerializer(String guid, Type target)
        {
            this.guid = guid;
            this.entry = null;
            this.targetType = target;
        }
        public String guid { get; }
        public OrbSerializerCatalog.Entry? entry { get; set; }
        public Type targetType { get; }
        public Orb CreateInstance() => throw new InvalidOperationException("Cannot create instance with the default orb serializer");
        public void Serialize(NetworkWriter writer, Orb target) => throw new InvalidOperationException("Cannot serialize using the default orb serializer");
        public void Deserialize(NetworkReader reader, Orb target) => throw new InvalidOperationException("Cannot deserialize using the default orb deserializer");
    }
    internal struct SerializableOrbSerializer<TOrb> : IOrbSerializer
        where TOrb : Orb, ISerializableObject, new()
    {
        public Type targetType => typeof(TOrb);
        public String guid => typeof(TOrb).FullName;
        public OrbSerializerCatalog.Entry? entry { get; set; }
        public Orb CreateInstance() => new TOrb();
        public void Deserialize(NetworkReader reader, Orb target) => _ = target is TOrb orb ? reader.Read(ref orb) : throw new InvalidOperationException($"Invalid orb type, must be {this.guid}");
        public void Serialize(NetworkWriter writer, Orb target) => _ = target is TOrb orb ? writer.Write(orb) : throw new InvalidOperationException($"Invalid orb type, must be {this.guid}");
    }
}