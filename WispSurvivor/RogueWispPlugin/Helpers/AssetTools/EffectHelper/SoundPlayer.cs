using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.Linq;

namespace RogueWispPlugin.Helpers
{
    public class EffectSoundPlayer : MonoBehaviour
    {
        [SerializeField]
        public SoundEventObject[] soundEvents = Array.Empty<SoundEventObject>();

        internal void AddSound( SoundEventObject sound )
        {
            if( this.postAwake ) throw new Exception( "Cannot add sounds after object has been instantiated" );

            var ind = this.soundEvents.Length;
            Array.Resize<SoundEventObject>( ref this.soundEvents, ind + 1 );
            this.soundEvents[ind] = sound;
        }

        private Queue<SoundEventObject> remainingEvents;
        private Single timer = 0f;
        private Boolean postAwake = false;
        private Queue<SoundEventObject> endingSounds = new Queue<SoundEventObject>();

        private void OnEnable()
        {
            this.postAwake = true;
            this.endingSounds.Clear();
            this.remainingEvents = new Queue<SoundEventObject>( this.soundEvents.OrderBy<SoundEventObject, Single>( ( val ) => val.time ) );
            //Main.LogI( this.soundEvents.Length );
            //Main.LogI( this.remainingEvents.Count );
            while( this.remainingEvents.Count > 0 && this.remainingEvents.Peek().time < 0f )
            {
                this.endingSounds.Enqueue( this.remainingEvents.Dequeue() );
            }
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
                //var nextSound = this.remainingEvents.Peek();
                if( this.remainingEvents.Peek().time <= this.timer )
                {
                    this.remainingEvents.Dequeue().Play( base.gameObject );
                    this.DoSounds();
                }
            }
        }

        private void OnDisable()
        {
            this.postAwake = false;
            while( this.endingSounds.Count > 0 )
            {
                this.endingSounds.Dequeue().Play( base.gameObject );
            }
        }


    }
}
