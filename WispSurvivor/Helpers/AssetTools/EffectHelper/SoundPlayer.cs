using R2API;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.Linq;

namespace RogueWispPlugin.Helpers
{
    internal class EffectSoundPlayer : MonoBehaviour
    {
        public SoundEvent[] soundEvents = Array.Empty<SoundEvent>();

        internal void AddSound( SoundEvent sound )
        {
            if( this.postAwake ) throw new Exception( "Cannot add sounds after object has been instantiated" );
            var ind = this.soundEvents.Length;
            Array.Resize<SoundEvent>( ref this.soundEvents, ind + 1 );
            this.soundEvents[ind] = sound;
        }


        private Queue<SoundEvent> remainingEvents;
        private Single timer = 0f;
        private Boolean postAwake = false;

        private void Awake()
        {
            this.postAwake = true;
            this.remainingEvents = new Queue<SoundEvent>( this.soundEvents.OrderBy<SoundEvent,Single>( (val) => val.time ) );

        }

        private void Update()
        {
            this.timer += Time.deltaTime;
            this.DoSounds();
        }

        private void DoSounds()
        {
            if( this.remainingEvents.Count == 0 )
            {
                this.enabled = false;
            } else
            {
                var nextSound = this.remainingEvents.Peek();
                if( this.remainingEvents.Peek().time <= this.timer )
                {
                    this.remainingEvents.Dequeue().Play( base.gameObject );
                    this.DoSounds();
                }
            }
        }
    }
}
