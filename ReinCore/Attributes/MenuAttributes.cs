namespace ReinCore
{
    using System;

    [AttributeUsage( AttributeTargets.Property, AllowMultiple = false, Inherited = false )]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class MenuAttribute : Attribute
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public String name { get; set; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public String sectionName { get; set; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Int32 sectionOrder { get; set; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Int32 orderInSection { get; set; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Boolean isRampTexture { get; set; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }


}