namespace AlternativeArtificer.Components
{
    using System;

    using RoR2;

    using UnityEngine;

    public class SoundOnAwake : MonoBehaviour
    {
        public String sound;
        public void Awake() => Util.PlaySound( this.sound, base.gameObject );
    }
}
