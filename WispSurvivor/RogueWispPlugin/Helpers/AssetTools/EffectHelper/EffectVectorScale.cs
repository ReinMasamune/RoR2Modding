using RoR2;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Rein.RogueWispPlugin.Helpers
{
    internal class EffectVectorScale : MonoBehaviour
    {
        public EffectComponent effectComp;
        public Boolean applyX;
        public Boolean applyY;
        public Boolean applyZ;
        public Boolean scaleOverTime;
        public Single durationFrac;
        public Single duration;
        public Boolean useEffectComponent = true;
        public Boolean reverse = false;


        private Boolean scaling = false;
        private Single scaleTimer = 0f;
        public Vector3 startScale;
        public Vector3 endScale;

        private void Start()
        {
            if( !this.useEffectComponent )
            {
                if( this.scaleOverTime )
                {
                    base.transform.localScale = this.startScale;
                    this.scaleTimer = this.duration * this.durationFrac;
                    this.scaling = true;
                } else
                {
                    base.transform.localScale = this.endScale;
                }
            } else
            {
                if( this.effectComp == null )
                {
                    Main.LogE( "No effect component found, effect will not be scaled." );
                    return;
                }
                var data = this.effectComp.effectData;
                if( data == null )
                {
                    Main.LogE( "No effect data recieved, effect will not be scaled" );
                    return;
                }

                var scale = data.start;
                this.startScale = new Vector3( this.applyX ? 0f : 1f, this.applyY ? 0f : 1f, this.applyZ ? 0f : 1f );
                this.endScale = new Vector3( this.applyX ? scale.x : 1f, this.applyY ? scale.y : 1f, this.applyZ ? scale.z : 1f );

                if( this.scaleOverTime )
                {
                    this.scaling = true;
                    this.scaleTimer = data.genericFloat * this.durationFrac;
                    base.transform.localScale = this.startScale;
                } else
                {
                    base.transform.localScale = this.endScale;
                }
            }

            if( this.reverse )
            {
                var temp = this.startScale;
                this.startScale = this.endScale;
                this.endScale = temp;

                base.transform.localScale = this.startScale;
            }
        }

        private Single age = 0f;
        private void Update()
        {
            if( this.scaling )
            {
                this.age += Time.deltaTime;
                if( this.age <= this.scaleTimer )
                {
                    var frac = this.age / this.scaleTimer;
                    base.transform.localScale = Vector3.Lerp( this.startScale, this.endScale, frac );
                } else
                {
                    this.scaling = false;
                    base.transform.localScale = this.endScale;
                }
            } else
            {
                base.transform.localScale = this.endScale;
                Destroy( this );
            }

        }


    }
}
