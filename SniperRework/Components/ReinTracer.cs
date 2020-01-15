using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ReinSniperRework
{
    internal partial class Main
    {
        class ReinTracer : MonoBehaviour
        {
            private Vector3 start;
            private Vector3 end;
            private Vector3 dir;

            public Transform effect;
            public Single speed;
            public Single trailPersistTime;


            public void Start()
            {
                var effectComp = base.gameObject.GetComponent<EffectComponent>();
                this.start = effectComp.effectData.origin;
                this.end = effectComp.effectData.start;
                this.dir = (this.end - this.start).normalized;
                this.effect.position = this.start;
            }

            public void Update()
            {
                var curDist = ( this.end - this.effect.position ).magnitude;
                var distToTravel = Mathf.Min( this.speed * Time.deltaTime, curDist );
                this.effect.position += distToTravel * this.dir;

                if( distToTravel == curDist )
                {
                    this.End();
                }

            }

            private void End()
            {
                var timer = this.effect.gameObject.AddComponent<DestroyOnTimer>();
                timer.duration = this.trailPersistTime;

                this.effect.parent = null;
                this.effect.position = this.end;

                Destroy( base.gameObject );
            }
        }
    }

}
