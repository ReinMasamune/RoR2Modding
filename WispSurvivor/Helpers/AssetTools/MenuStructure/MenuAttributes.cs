using System;
using UnityEngine;

namespace RogueWispPlugin.Helpers
{
    [AttributeUsage( AttributeTargets.Property, AllowMultiple = false, Inherited = false )]
    internal class MenuAttribute : Attribute
    {
        internal MenuAttribute( String section )
        {

        }

        internal String name { get; set; }
    }


}