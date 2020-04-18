using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ReinCore
{
    [Serializable]
    public struct ColorRegion
    {
        [SerializeField]
        public Single start;
        [SerializeField]
        public Single end;
        [SerializeField]
        public Color color;

        public static implicit operator ColorRegion( (Single start, Single end, Color color) tuple )
        {
            return new ColorRegion
            {
                start = tuple.start,
                end = tuple.end,
                color = tuple.color,
            };
        }
    }
}
