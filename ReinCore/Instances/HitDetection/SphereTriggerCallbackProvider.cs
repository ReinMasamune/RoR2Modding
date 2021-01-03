namespace Rein.Instances.HitDetection
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Text;

    using Mono.Cecil;

    using ReinCore;

    using RoR2;

    using UnityEngine;


    //Reimplement unsafe stuff........
    public class SphereTriggerCallbackProvider<TCb>
        where TCb : struct, ITriggerCallbacks<TCb>
    {
        private static unsafe readonly Int32 headerInNints = 16 / sizeof(nint);
        public TCb cb;

        private Collider[] cols;
        

        public void Start()
        {
            RoR2Application.onFixedUpdate += this.RoR2Application_onFixedUpdate;
            RoR2.Stage.onServerStageComplete += this.StageEnd;
            this.intervalTimer = 0f;
            this.cols = new Collider[this.cb.maxHits];
        }
        public void Stop()
        {
            RoR2Application.onFixedUpdate -= this.RoR2Application_onFixedUpdate;
            RoR2.Stage.onServerStageComplete -= this.StageEnd;
        }
        public void StageEnd(Stage _)
        {
            this.Stop();
        }


        private void RoR2Application_onFixedUpdate()
        {
            var dt = Time.fixedDeltaTime;
            this.intervalTimer += dt;
            this.cb.PassTime(dt);
            while(this.intervalTimer >= this.cb.interval && !this.cb.shouldEnd)
            {
                this.intervalTimer -= this.cb.interval;

                DoSphereCheck();
            }

            if(this.cb.shouldEnd)
            {
                this.Stop();
            }
        }

        private void DoSphereCheck()
        {
            this.cb.PreSphereCheck();
            //Need to setup unsafe stuff...
            //unsafe
            //{
            //    var ctr = this.cb.maxHits;
            //    var r = Array.Empty<Collider>();
            //    var templatePtr = (nint*)*(nint*)Unsafe.AsPointer(ref r);
            //    var ptr = stackalloc nint[ctr + headerInNints];
            //    ptr[0] = templatePtr[0];
            //    ptr[1] = (nint)ctr;
            //    cols = Unsafe.Read<Collider[]>(ptr);
            //}
            var c = Physics.OverlapSphereNonAlloc(this.cb.position, this.cb.radius, cols, this.cb.layerMask, this.cb.queryTriggerInteraction);
            for(Int32 i = 0; i < c; ++i)
            {
                this.cb.OnColliderInSphere(cols[i]);
            }
        }


        private Single intervalTimer;
    }

    public interface ITriggerCallbacks<TSelf>
        where TSelf : struct, ITriggerCallbacks<TSelf>
    {
        Boolean shouldEnd { get; }
        Vector3 position { get; }
        Single radius { get; }
        Single interval { get; }

        Int32 maxHits { get; }
        Int32 layerMask { get; }
        QueryTriggerInteraction queryTriggerInteraction { get; }

        void OnColliderInSphere(Collider col);
        void PreSphereCheck();
        void PassTime(Single delta);
    }
}
