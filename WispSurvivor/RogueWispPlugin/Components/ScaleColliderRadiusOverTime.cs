#if ANCIENTWISP
using System;

using UnityEngine;

namespace Rein.RogueWispPlugin
{

    internal partial class Main
    {
        [RequireComponent( typeof( SphereCollider ) )]
        internal class ScaleColliderRadiusOverTime : MonoBehaviour
        {
            public Single startRadius;
            public Single endRadius;
            public Single duration;
            public Single durationFrac;

            private Boolean scaling = false;
            private Single scaleTimer = 0f;
            private SphereCollider collider;

            private void Awake()
            {
                this.collider = base.GetComponent<SphereCollider>();
                if( this.collider == null )
                {
                    this.scaling = false;
                    return;
                }

                this.collider.radius = this.startRadius;
                this.scaleTimer = this.duration * this.durationFrac;
                this.scaling = true;
            }

            private Single age = 0f;
            private void FixedUpdate()
            {
                if( this.scaling )
                {
                    this.age += Time.fixedDeltaTime;
                    if( this.age < this.scaleTimer )
                    {
                        this.collider.radius = Mathf.Lerp( this.startRadius, this.endRadius, this.age / this.scaleTimer );
                    } else
                    {
                        this.collider.radius = this.endRadius;
                        this.scaling = false;
                    }
                } else
                {
                    Destroy( this );
                }
            }
        }
    }
}
#endif