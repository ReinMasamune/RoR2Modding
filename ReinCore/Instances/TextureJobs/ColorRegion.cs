namespace ReinCore
{
    using System;

    using UnityEngine;

    [Serializable]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public struct ColorRegion
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
        [SerializeField]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Single start;
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        [SerializeField]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Single end;
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        [SerializeField]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public Color color;
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static implicit operator ColorRegion( (Single start, Single end, Color color) tuple )
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
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
