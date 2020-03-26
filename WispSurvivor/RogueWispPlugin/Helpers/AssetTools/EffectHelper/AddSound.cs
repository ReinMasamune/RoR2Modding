using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Rein.RogueWispPlugin.Helpers
{
    internal static partial class EffectHelper
    {
        internal static void AddSound( GameObject mainObj, String sound, Single delay )
        {
            var soundPlayer = mainObj.AddOrGetComponent<EffectSoundPlayer>();

            soundPlayer.AddSound( new SoundEvent( delay, sound ) );
        }

        internal static void AddSound( GameObject mainObj, String sound, Single delay, String rtpcName, Single rtpcValue )
        {
            var soundPlayer = mainObj.AddOrGetComponent<EffectSoundPlayer>();

            soundPlayer.AddSound( new SoundEvent( delay, sound, rtpcName, rtpcValue ) );
        }

        internal static void AddSound( GameObject mainObj, String startSound, String endSound, Single delay, Single duration )
        {
            var soundPlayer = mainObj.AddOrGetComponent<EffectSoundPlayer>();

            soundPlayer.AddSound( new SoundEvent( delay, startSound ) );
            soundPlayer.AddSound( new SoundEvent( delay + duration, endSound ) );
        }

        internal static void AddSound( GameObject mainObj, String startSound, String endSound, Single delay, Single duration, String rtpcName, Single rtpcValue )
        {
            var soundPlayer = mainObj.AddOrGetComponent<EffectSoundPlayer>();

            soundPlayer.AddSound( new SoundEvent( delay, startSound, rtpcName, rtpcValue ) );
            soundPlayer.AddSound( new SoundEvent( delay + duration, endSound, rtpcName, rtpcValue ) );
        }
    }
}
