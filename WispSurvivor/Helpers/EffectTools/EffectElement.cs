using R2API;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RogueWispPlugin.Helpers
{
    internal abstract class EffectElement : MonoBehaviour
    {
        internal String name
        {
            get
            {
                return this.obj.name;
            }
            set
            {
                this.obj.name = value;
            }
        }
        internal String path
        {
            get
            {
                return this.parent.path + "/" + this.name;
            }
        }
        internal GameObject obj { get; private set; }
        internal EffectCategory parent { get; private set; }


        internal abstract Vector3 scale { get; set; }
        internal abstract Vector3 position { get; set; }
        internal abstract Quaternion rotation { get; set; }


        internal abstract EffectElement Duplicate( GameObject newObj, EffectCategory newParent );


        internal EffectElement( String name, EffectCategory parent )
        {
            this.obj = new GameObject();
            this.name = name;
            this.parent = parent;
            parent.AddChild( this );

            this.obj.transform.parent = parent.obj.transform.parent;
            this.scale = Vector3.one;
            this.position = Vector3.zero;
            this.rotation = Quaternion.identity;
        }

        internal EffectElement( EffectElement orig, GameObject newObj, EffectCategory newParent )
        {
            this.obj = newObj;
            this.name = orig.name;
            this.parent = newParent;
        }
    }
}
