namespace BakeTest.Mod
{
    using System;

    using RoR2;
    using RoR2.Navigation;

    internal struct Triangle
    {
        internal Triangle(Int32 a, Int32 b, Int32 c)
        {
            this._a = a;
            this._b = b;
            this._c = c;
        }
        private readonly Int32 _a;
        private readonly Int32 _b;
        private readonly Int32 _c;

        internal NodeGraph.Link ab => MakeLink(this._a, this._b);
        internal NodeGraph.Link ac => MakeLink(this._a, this._c);
        internal NodeGraph.Link ba => MakeLink(this._b, this._a);
        internal NodeGraph.Link bc => MakeLink(this._b, this._c);
        internal NodeGraph.Link ca => MakeLink(this._c, this._a);
        internal NodeGraph.Link cb => MakeLink(this._c, this._b);

        private static NodeGraph.Link MakeLink(Int32 first, Int32 second) => new NodeGraph.Link
        {
            nodeIndexA = new NodeGraph.NodeIndex(first),
            nodeIndexB = new NodeGraph.NodeIndex(second),
            distanceScore = 1f,
            maxSlope = 45f,
            minJumpHeight = 10f,
            hullMask = (Int32)(HullMask.Human | HullMask.Golem | HullMask.BeetleQueen),
            jumpHullMask = 0,
            
        };
    }
}
