
using ReinCore;

[assembly: Catalog(typeof(DotCatalog))]

namespace ReinCore
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using RoR2;

    using UnityEngine;
    using UnityEngine.Networking;

    public sealed class DotCatalog : Catalog<DotCatalog, IDotDef>
    {
        public override String guid => "Rein.DotCatalog";
        protected internal override Int32 order => 0;

        protected override void FirstInitSetup()
        {
            base.FirstInitSetup();
            NetworkCore.RegisterMessageType<DotMessage>();
            NetworkCore.RegisterMessageType<CleanseMessage>();
        }
    }

    public interface IDotDef : DotCatalog.ICatalogDef
    {
        void Apply(CharacterBody target, Byte[] bytes);

        Byte[] tempStackSizedArray { get; set; }
    }




}
