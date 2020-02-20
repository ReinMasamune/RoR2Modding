using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RogueWispPlugin.Helpers
{
    internal struct SoundEvent
    {
        internal Single time { get; private set; }

        private String soundName;
        private Boolean rtpcOn;
        private String rtpcName;
        private Single rtpcValue;
        
        internal SoundEvent( Single time, String soundName )
        {
            this.time = time;

            this.soundName = soundName;
            this.rtpcOn = false;
            this.rtpcName = default;
            this.rtpcValue = default;
        }

        internal SoundEvent( Single time, String soundName, String rtpcName, Single rtpcValue )
        {
            this.time = time;

            this.soundName = soundName;
            this.rtpcOn = true;
            this.rtpcName = default;
            this.rtpcValue = default;
        }


        internal void Play( GameObject position )
        {
            if( this.rtpcOn )
            {
                Util.PlaySound( this.soundName, position, this.rtpcName, this.rtpcValue );
            } else
            {
                Util.PlaySound( this.soundName, position );
            }
        }

        
    }
}
