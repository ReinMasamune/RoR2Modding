using RoR2;
using System;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;

namespace RogueWispPlugin.Helpers
{
    public struct Triangle
    {
        public readonly Int32 index;

        public Triangle( Int32 index, Int32 vert1, Int32 vert2, Int32 vert3, Int32 link12, Int32 link23, Int32 link31 )
        {
            this.index = index;
            this.vert1 = vert1;
            this.vert2 = vert2;
            this.vert3 = vert3;

            this.link12 = link12;
            this.link23 = link23;
            this.link31 = link31;
        }





        private readonly Int32 vert1;
        private readonly Int32 vert2;
        private readonly Int32 vert3;

        private readonly Int32 link12;
        private readonly Int32 link23;
        private readonly Int32 link31;
    }
}
