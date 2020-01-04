namespace AlternativeArtificer.Components
{
    using RoR2;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using UnityEngine;

    public class SoundOnAwake : MonoBehaviour
    {
        public String sound;
        public void Awake()
        {
            Util.PlaySound( sound, base.gameObject );
        }
    }
}
