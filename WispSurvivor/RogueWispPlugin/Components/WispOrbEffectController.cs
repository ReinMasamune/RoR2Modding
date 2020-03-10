using RoR2;
using UnityEngine;

namespace RogueWispPlugin
{
    internal partial class Main
    {
        [RequireComponent( typeof( EffectComponent ) )]
        public class WispOrbEffectController : MonoBehaviour
        {
            public System.String startSound = "";
            public System.String endSound = "";

            public System.String explosionSound = "";

            private System.Single duration;
            private System.Single timeLeft;
            private System.Single startDist;
            private System.Single deathDelay = 3f;

            private System.Boolean useTarget;
            private System.Boolean dead = false;

            private Vector3 start;
            private Vector3 end;
            private Vector3 prevPos;

            private Transform target;

            private Rigidbody rb;


            public void Start()
            {
                EffectComponent effectComp = this.GetComponent<EffectComponent>();
                EffectData data = effectComp.effectData;

                this.useTarget = data.genericBool;
                this.duration = data.genericFloat;
                this.start = data.origin;
                this.end = data.start;

                if( this.useTarget )
                {
                    this.target = data.ResolveHurtBoxReference().transform;
                    this.startDist = Vector3.Distance( this.start, this.target.position );
                } else
                {
                    this.startDist = Vector3.Distance( this.start, this.end );
                }

                this.timeLeft = this.duration;
                this.prevPos = this.start;
                var startVelocity = ( this.end - this.start ) / this.duration;
                var direction = startVelocity.normalized;
                base.transform.forward = direction;

                this.transform.position = this.start;
                this.rb = base.GetComponent<Rigidbody>();
                this.rb.velocity = startVelocity;
                RoR2.Util.PlayScaledSound( this.startSound, this.gameObject, 2.0f );
            }

            public void Update()
            {
                if( !this.dead )
                {
                    if( this.useTarget )
                    {
                        if( !this.target )
                        {
                            this.useTarget = false;
                        }
                    }

                    //Vector3 dest = this.end;
                    //if( this.useTarget )
                    //{
                    //    dest = this.target.position;
                    //}

                    //This can be modified to make arcing effects
                    this.timeLeft -= Time.deltaTime;
                    //System.Single frac = 1f - this.timeLeft / this.duration;
                    //Vector3 desiredPos = Vector3.Lerp(this.start, dest, frac);

                    //this.transform.rotation = Quaternion.FromToRotation( Vector3.Normalize( desiredPos - this.prevPos ), this.transform.forward );
                    //this.transform.position = desiredPos;
                    //this.prevPos = desiredPos;

                    if( this.timeLeft < 0f )
                    {
                        this.rb.velocity = Vector3.zero;
                        this.rb.position = this.end;
                        RoR2.Util.PlaySound( this.endSound, this.gameObject );
                        RoR2.Util.PlaySound( this.explosionSound, this.gameObject );
                        this.dead = true;
                        var trail = base.gameObject.GetComponent<TrailRenderer>();
                        if( trail )
                        {
                            trail.emitting = false;
                        }
                    }
                } else
                {
                    this.deathDelay -= Time.deltaTime;
                    if( this.deathDelay < 0 )
                    {
                        Destroy( this.gameObject );
                    }
                }
            }
        }
    }
}
