using RoR2;
using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace RogueWispPlugin.Helpers
{
    public struct Link
    {
        public readonly Int32 index;

        public Link( Int32 index, Int32 vert1, Int32 vert2, Int32 tri1, Int32 tri2 )
        {
            this.index = index;
            this.vert1 = vert1;
            this.vert2 = vert2;
            this.tri1 = tri1;
            this.tri2 = tri2;
        }


        private readonly Int32 vert1;
        private readonly Int32 vert2;

        private readonly Int32 tri1;
        private readonly Int32 tri2;
    }
}
