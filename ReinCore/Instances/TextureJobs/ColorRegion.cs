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
    }
}
