using System;

using UnityEngine;

namespace Rein.RogueWispPlugin.Helpers
{
    [SerializeField]
    [System.Serializable]
    public struct SoundEvent
    {
        public Single time
        {
            get => this.intTime;
        }

        [SerializeField]
        public Single intTime;

        [SerializeField]
        public String soundName;
        [SerializeField]
        public Boolean scaleOn;
        [SerializeField]
        public Boolean rtpcOn;
        [SerializeField]
        public String rtpcName;
        [SerializeField]
        public Single rtpcValue;

        internal SoundEvent( Single time, String soundName )
        {
            this.intTime = time;

            this.soundName = soundName;
            this.rtpcOn = false;
            this.rtpcName = default;
            this.rtpcValue = default;
            this.scaleOn = false;
        }

        internal SoundEvent( Single time, String soundName, Single scale )
        {
            this.intTime = time;

            this.soundName = soundName;
            this.rtpcOn = false;
            this.rtpcName = default;
            this.rtpcValue = scale;
            this.scaleOn = true;

        }

        internal SoundEvent( Single time, String soundName, String rtpcName, Single rtpcValue )
        {
            this.intTime = time;

            this.soundName = soundName;
            this.rtpcOn = true;
            this.rtpcName = default;
            this.rtpcValue = default;
            this.scaleOn = false;
        }

        public static implicit operator SoundEventObject( SoundEvent sound )
        {
            return sound.Convert();
        }

        public SoundEventObject Convert()
        {
            var obj = ScriptableObject.CreateInstance<SoundEventObject>();
            obj.time = this.time;
            obj.rtpcName = this.rtpcName;
            obj.rtpcOn = this.rtpcOn;
            obj.rtpcValue = this.rtpcValue;
            obj.scaleOn = this.scaleOn;
            obj.soundName = this.soundName;
            return obj;
        }


    }
}
