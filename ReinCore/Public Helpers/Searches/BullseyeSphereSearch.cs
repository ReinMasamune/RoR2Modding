namespace ReinCore
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Linq;

    using Facepunch.Steamworks.Callbacks;

    using RoR2;

    using UnityEngine;
    using System.Runtime.CompilerServices;


    //TODO: Add pooling to kill allocations later
    //TODO: Remove linq after writing better impl
    public unsafe ref struct BullseyeSphereSearch
    {
        //private delegate*<Candidate, Boolean> filter;
        //private delegate*<Candidate, Single> ordering;

        public Vector3 origin { get; set; }

        private Single sqRadius;
        public Single radius
        {
            get => Mathf.Sqrt(this.sqRadius);
            set => this.sqRadius = (value * value);
        }
        //public FFMode mode { get; set; }

        //public enum FFMode { }

        public Result[] PerformSearch()
        {
            static Candidate GetCandidate(HurtBox box) => new Candidate(box);
            static Boolean IsValid(Candidate c) => c.valid;
            static Single GetScore(Candidate c) => c.score;
            static Result ToResult(Candidate c) => new(c);

            var orig = this.origin;
            var sqdist = this.sqRadius;
            //var ffMode = this.mode;

            Boolean IsInRange(Candidate candidate) => (candidate.hb.transform.position - orig).sqrMagnitude <= sqdist;
            var candidates = HurtBox.readOnlyBullseyesList.Select(GetCandidate).Where(IsInRange).ToArray();

            //for(Int32 i = 0; i < candidates.Length; ++i)
            //{
            //    ref var c = ref candidates[i];
            //    if(filter != null)
            //    {
            //        c.valid = filter(c);
            //    }
            //    if(ordering != null)
            //    {
            //        c.score = ordering(c);
            //    }
            //}

            return candidates.Where(IsValid).OrderByDescending(GetScore).Select(ToResult).ToArray();
        }
    }

    public struct Candidate
    {
        public HurtBox hb { get; }
        internal Boolean valid;
        internal Single score;
        internal Candidate(HurtBox hb)
        {
            this.hb = hb;
            this.valid = true;
            this.score = 0f;
        }
    }

    public struct Result
    {
        public HurtBox hb { get; }

        internal Result(Candidate candidate)
        {
            this.hb = candidate.hb;
        }
    }
}
