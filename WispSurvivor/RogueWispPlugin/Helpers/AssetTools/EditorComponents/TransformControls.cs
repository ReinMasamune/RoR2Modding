#if MATEDITOR
using System;
using UnityEngine;
using ReinCore;

namespace Rein.RogueWispPlugin.Helpers
{
    internal class TransformControls
    {
        private Transform transform;
        internal TransformControls( Transform transform )
        {
            this.transform = transform;
        }

        [MenuAttribute( sectionName = "General", sectionOrder = 0)]
        internal Vector3 position
        {
            get => this.transform.localPosition;
            set => this.transform.localPosition = value;
        }

        [Menu( sectionName = "General" )]
        internal Vector3 rotation
        {
            get => this.transform.localEulerAngles;
            set => this.transform.localEulerAngles = value;
        }

        [Menu( sectionName = "General" )]
        internal Vector3 scale
        {
            get => this.transform.localScale;
            set => this.transform.localScale = value;
        }
        
    }
}
#endif