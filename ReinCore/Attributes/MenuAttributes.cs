using System;
using UnityEngine;

namespace ReinCore
{
    [AttributeUsage( AttributeTargets.Property, AllowMultiple = false, Inherited = false )]
    public class MenuAttribute : Attribute
    {
        public String name { get; set; }
        public String sectionName { get; set; }
        public Int32 sectionOrder { get; set; }
        public Int32 orderInSection { get; set; }
        public Boolean isRampTexture { get; set; }
    }


}