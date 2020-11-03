namespace ReinCore
{
    using System;

    [AttributeUsage(AttributeTargets.Assembly)]
    public sealed class CatalogAttribute : Attribute
    {
        public CatalogAttribute(Type catalogType)
        {
            type = catalogType;
        }

        internal Type type { get; }
    }
}