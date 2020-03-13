using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RogueWispPlugin.Helpers
{
    internal class ParticleElement : EffectElement
    {
        internal override Vector3 position
        {
            get
            {
                return base.obj.transform.localPosition;
            }
            set
            {
                base.obj.transform.localPosition = value;
            }
        }
        internal override Vector3 scale
        {
            get
            {
                return base.obj.transform.localScale;
            }
            set
            {
                base.obj.transform.localScale = value;
            }
        }
        internal override Quaternion rotation
        {
            get
            {
                return base.obj.transform.localRotation;
            }
            set
            {
                base.obj.transform.localRotation = value;
            }
        }

        internal ParticleSystem partSys { get; private set; }
        internal ParticleSystemRenderer partRend { get; private set; }

        internal Material material
        {
            get
            {
                return this.partRend.material;
            }
            set
            {
                this.partRend.material = value;
            }
        }


        internal override EffectElement Duplicate( GameObject newObj, EffectCategory newParent )
        {
            return new ParticleElement( this, newObj, newParent );
        }
        
        internal ParticleElement( String name, EffectCategory parent ) : base( name, parent )
        {
            this.partSys = base.obj.AddComponent<ParticleSystem>();
            this.partRend = base.obj.AddOrGetComponent<ParticleSystemRenderer>();
        }

        private ParticleElement( ParticleElement orig, GameObject newObj, EffectCategory newParent ) : base( orig, newObj, newParent )
        {
            this.partSys = newObj.GetComponent<ParticleSystem>();
            this.partRend = newObj.GetComponent<ParticleSystemRenderer>();
        }
    }
}
