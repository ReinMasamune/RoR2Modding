using R2API;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RogueWispPlugin.Helpers
{
    internal class EffectCategory
    {
        public String name { get; private set; }
        public GameObject obj { get; private set; }

        public String path
        {
            get
            {
                if( this.parent == null )
                {
                    return "";
                } else
                {
                    return this.parent.path + "/" + this.name;
                }
            }
        }
        public IEnumerable<EffectElement> childLoop
        {
            get
            {
                return this.children.Values;
            }
        }
        public IEnumerable<EffectCategory> childCatsLoop
        {
            get
            {
                return this.childCats.Values;
            }
        }


        private EffectCategory parent;
        private Dictionary<String, EffectElement> children = new Dictionary<String, EffectElement>();
        private Dictionary<String, EffectCategory> childCats = new Dictionary<String, EffectCategory>();


        public void AddChild( EffectElement child )
        {
            var name = child.name;
            if( this.children.ContainsKey( name ) )
            {
                Main.LogE( name + " already exists as a child" );
                return;
            }
            this.children[name] = child;
        }


        public EffectCategory( GameObject obj )
        {
            this.name = EffectBuilder.rootName;
            this.obj = obj;
            this.parent = null;
        }
        public EffectCategory( String name, GameObject obj, EffectCategory parent )
        {
            this.name = name;
            this.obj = obj;
            this.parent = parent;

            this.parent.childCats[name] = this;
        }
    }
}
