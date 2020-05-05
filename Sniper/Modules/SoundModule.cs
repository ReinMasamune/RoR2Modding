using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BepInEx.Logging;
using ReinCore;
using RoR2;
using Sniper.Enums;
using UnityEngine;

namespace Sniper.Modules
{
    internal static class SoundModule
    {
        internal static UInt32 bankIndex { get; private set; }
        internal static void LoadBank()
        {
            SoundsCore.LoadSoundbank( Properties.Resources.Rein_Sniper_Bank, ( ind ) => bankIndex = ind );
        }

        internal static Single sfxVolume { private get; set; } = 1f;
        internal static Single masterVolume { private get; set; } = 1f;

        private const UInt32 sniper_charge_amount = 135031646u;
        private const UInt32 volume_sfx = 3673881719u;
        private const UInt32 volume_master = 3695994288u;

        private const UInt32 bolt_normal_shot = 763788813u;
        private const UInt32 bolt_quickscope = 800730984u;
        internal static void PlayFire( GameObject source, Single chargeLevel )
        {
            UInt32 id = AkSoundEngine.PostEvent( bolt_normal_shot, source );
            _ = AkSoundEngine.SetRTPCValueByPlayingID( sniper_charge_amount, 0.25f + (chargeLevel * 0.75f), id );
            //_ = AkSoundEngine.SetRTPCValueByPlayingID( volume_master, 100f, id );
            //id = AkSoundEngine.PostEvent( bolt_normal_shot, source );
            //_ = AkSoundEngine.SetRTPCValueByPlayingID( sniper_charge_amount, chargeLevel, id );
            //id = AkSoundEngine.PostEvent( bolt_normal_shot, source );
            //_ = AkSoundEngine.SetRTPCValueByPlayingID( sniper_charge_amount, chargeLevel, id );
        }

        private const UInt32 bolt_open_chamber = 1389001356u;
        internal static void PlayOpenReload( GameObject source ) => _ = AkSoundEngine.PostEvent( bolt_open_chamber, source );

        private const UInt32 bolt_ricochet = 3672228492u;
        internal static void PlayRicochet( GameObject source )
        {
            _ = AkSoundEngine.PostEvent( bolt_ricochet, source );
        }

        internal static void PlayLoad( GameObject source, ReloadTier reloadTier )
        {
            _ = AkSoundEngine.PostEvent( reloadTier.GetSound(), source );
        }

        private const UInt32 bolt_reload_bad = 339887885u;
        private const UInt32 bolt_reload_good = 2811185582u;
        private const UInt32 bolt_reload_perfect = 1939097407u;
        private static UInt32 GetSound( this ReloadTier tier )
        {
            switch( tier )
            {
                case ReloadTier.Bad:
                return bolt_reload_bad;
                case ReloadTier.Good:
                return bolt_reload_good;
                case ReloadTier.Perfect:
                return bolt_reload_perfect;
                default:
                return bolt_reload_bad;
            }
        }
    }

}
