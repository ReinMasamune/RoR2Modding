using R2API;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RogueWispPlugin.Helpers
{
    [SerializeField]
    [System.Serializable]
    public class SoundEventObject : ScriptableObject
    {
        public Single time;
        public String soundName;
        public Boolean scaleOn;
        public Boolean rtpcOn;
        public String rtpcName;
        public Single rtpcValue;


        public void Play( GameObject position )
        {
            if( this.rtpcOn )
            {
                Util.PlaySound( this.soundName, position, this.rtpcName, this.rtpcValue );
            } else
            {
                if( this.scaleOn )
                {
                    Util.PlayScaledSound( this.soundName, position, this.rtpcValue );
                } else
                {
                    Util.PlaySound( this.soundName, position );
                }

            }
        }
    }
}
