namespace BakeTest.Mod
{
    using System;

    using UnityEngine;
    using UnityEngine.Assertions.Must;

    internal static class BoundsDraw
    {
        private enum Corner : Byte { A, B, C, D, E, F, G, H }

        private const Byte pathLength = 18;

        private static unsafe void GetCornerOrder(Corner* corners)
        {
            corners[0] = Corner.A;
            corners[1] = Corner.B;
            corners[2] = Corner.D;
            corners[3] = Corner.C;
            corners[4] = Corner.A;
            corners[5] = Corner.D;
            corners[6] = Corner.H;
            corners[7] = Corner.B;
            corners[8] = Corner.F;
            corners[9] = Corner.H;
            corners[10] = Corner.G;
            corners[11] = Corner.E;
            corners[12] = Corner.F;
            corners[13] = Corner.A;
            corners[14] = Corner.E;
            corners[15] = Corner.H;
            corners[16] = Corner.C;
            corners[17] = Corner.G;
        }
        private static unsafe void GetCornerPositions(Vector3* positions)
        {
            Corner* corners = stackalloc Corner[pathLength];
            GetCornerOrder(corners);
            for(Int32 i = 0; i < pathLength; ++i) positions[i] = corners[i].ToPosition();
        }

        private static Vector3 ToPosition(this Corner corner) => corner switch
        {
            Corner.A => new(1f, 1f, 1f),
            Corner.B => new(1f, 1f, -1f),
            Corner.C => new(1f, -1f, 1f),
            Corner.D => new(1f, -1f, -1f),
            Corner.E => new(-1f, 1f, 1f),
            Corner.F => new(-1f, 1f, -1f),
            Corner.G => new(-1f, -1f, 1f),
            Corner.H => new(-1f, -1f, -1f),
            _ => default,
        };

        private static Bounds _currentBounds;
        internal static Bounds currentBounds
        {
            get => _currentBounds;
            set => UpdateBounds(value);
        }

        private unsafe static void UpdateBounds(Bounds next)
        {
            if(next == _currentBounds) return;

            Vector3* positionMults = stackalloc Vector3[pathLength];
            GetCornerPositions(positionMults);

            var extents = next.extents;
            renderer.endWidth = renderer.startWidth = extents.magnitude * 0.025f;
            for(Int32 i = 0; i < pathLength; ++i)
            {
                renderer.SetPosition(i, extents.ScaleBy(positionMults[i]) + next.center);
            }
        }


        private static LineRenderer _renderer;
        private static LineRenderer renderer => _renderer ??= CreateRenderer();

        private static LineRenderer CreateRenderer()
        {
            var obj = new GameObject("BoundsDrawObj");
            UnityEngine.Object.DontDestroyOnLoad(obj);
            var rend = obj.AddComponent<LineRenderer>();

            rend.endColor = rend.startColor = new(1f, 0.2f, 0.2f, 1f);
            rend.endWidth = rend.startWidth = 5f;
            rend.material = new Material(Shader.Find("Unlit/Color"))
            {
                doubleSidedGI = true
            };
            rend.loop = true;
            rend.numCornerVertices = rend.numCapVertices = 64;

            rend.positionCount = pathLength;

            return rend;
        }
    }


}
