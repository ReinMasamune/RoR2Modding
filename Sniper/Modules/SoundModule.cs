namespace Sniper.Modules
{
    using System;

    using ReinCore;
    using Sniper.Properties;
    using Sniper.Enums;

    using UnityEngine;

    using Resources = UnityEngine.Resources;
    using Object = System.Object;
    using BepInEx.Configuration;

    internal static class SoundModule
    {
        internal static UInt32 bankIndex { get; private set; }
        internal static void LoadBank() => SoundsCore.LoadSoundbank(Properties.Resources.Rein_Sniper_Bank, (ind) => bankIndex = ind);

        internal static void PlayFire(GameObject source, Single chargeLevel, FireType fireType)
        {
            UInt32 id = AkSoundEngine.PostEvent( fireType.GetSound(), source );
            _ = AkSoundEngine.SetRTPCValueByPlayingID(Sounds.Sniper_Charge_Amount.ID(), chargeLevel * 100f, id);
            _ = AkSoundEngine.SetRTPCValueByPlayingID(Sounds.Sniper_Volume_ALL.ID(), ConfigModule.sfxVolume, id);
            _ = AkSoundEngine.SetRTPCValueByPlayingID(Sounds.Sniper_Volume_SHOTS.ID(), ConfigModule.shotsVolume, id);
        }
        internal static void PlayOpenReload(GameObject source)
        {
            var id = AkSoundEngine.PostEvent(Sounds.Bolt_Open_Chamber.ID(), source);
            _ = AkSoundEngine.SetRTPCValueByPlayingID(Sounds.Sniper_Volume_ALL.ID(), ConfigModule.sfxVolume, id);
        }
        internal static void PlayRicochet(GameObject source)
        {
            var id = AkSoundEngine.PostEvent(Sounds.Bolt_Ricochet.ID(), source);
            _ = AkSoundEngine.SetRTPCValueByPlayingID(Sounds.Sniper_Volume_ALL.ID(), ConfigModule.sfxVolume, id);
        }
        internal static void PlayLoad(GameObject source, ReloadTier reloadTier)
        {
            var id = AkSoundEngine.PostEvent(reloadTier.GetSound(), source);
            _ = AkSoundEngine.SetRTPCValueByPlayingID(Sounds.Sniper_Volume_ALL.ID(), ConfigModule.sfxVolume, id);
        }
        internal static void PlayKnifeHit(GameObject source, KnifeHitMaterial mat)
        {
            var id = AkSoundEngine.PostEvent(mat.GetSound(), source);
            _ = AkSoundEngine.SetRTPCValueByPlayingID(Sounds.Sniper_Volume_ALL.ID(), ConfigModule.sfxVolume, id);
        }

        private static UInt32 GetSound(this FireType fireType) => fireType switch
        {
            FireType.Normal => Sounds.Bolt_Normal_Shot.ID(),
            FireType.Plasma => Sounds.Bolt_Plasma_Shot.ID(),
            FireType.Scatter => Sounds.Bolt_Scatter_shot.ID(),
            FireType.Burst => Sounds.Bolt_Burst.ID(),
            _ => Sounds.Bolt_Normal_Shot.ID(),
        };
        private static UInt32 GetSound(this ReloadTier tier) => tier switch
        {
            ReloadTier.Bad => Sounds.Bolt_New_Bullet_Trash.ID(),
            ReloadTier.Good => Sounds.Bolt_New_Bullet_Good.ID(),
            ReloadTier.Perfect => Sounds.Bolt_New_Bullet_Best.ID(),
            _ => Sounds.Bolt_New_Bullet_Trash.ID(),
        };
        private static UInt32 GetSound(this KnifeHitMaterial mat) => mat switch
        {
            KnifeHitMaterial.Metallic => Sounds.Knife_Projectile_Metallic_hit.ID(),
            KnifeHitMaterial.Organic => Sounds.Knife_Projectile_Organic_hit.ID(),
            _ => Sounds.Knife_Projectile_Metallic_hit.ID(),
        };
        internal enum FireType
        {
            Normal,
            Scatter,
            Plasma,
            Burst
        }
        internal enum KnifeHitMaterial
        {
            Organic,
            Metallic,
        }
    }
}
