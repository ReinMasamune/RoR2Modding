namespace ReinCore
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using UnityEngine;

    public sealed class MeshCatalog : Catalog<MeshCatalog, MeshCatalog.Def>
    {
        public sealed override String guid { get; } = $"Rein.{nameof(MeshCatalog)}";

        protected internal sealed override Int32 order => 0;

        public class Def : ICatalogDef
        {
            public Def(Mesh mesh, String guid)
            {
                this.guid = guid;
            }
            public String guid { get; }

            public Entry? entry { get; set; }
        }
    }


}
