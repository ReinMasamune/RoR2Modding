namespace Sniper.Modules
{
    using System;

    using ReinCore;
    using Sniper.Properties;
    using Sniper.Enums;

    using UnityEngine;

    using Resources = UnityEngine.Resources;
    using Object = System.Object;

    internal static class SoundModule
    {
        internal static UInt32 bankIndex { get; private set; }
        internal static void LoadBank() => SoundsCore.LoadSoundbank( Properties.Resources.Rein_Sniper_Bank, ( ind ) => bankIndex = ind );

        internal static Single sfxVolume { private get; set; } = 1f;
        internal static Single masterVolume { private get; set; } = 1f;



        internal enum FireType
        {
            Normal,
            Scatter,
            Plasma
        }

        private static UInt32 GetFireID( FireType fireType )
        {
            switch( fireType )
            {
                case FireType.Normal:
                return Sounds.Bolt_Normal_Shot.ID();

                case FireType.Scatter:
                return Sounds.Bolt_Scatter_shot.ID();

                case FireType.Plasma:
                return Sounds.Bolt_Plasma_Shot.ID();

                default:
                return Sounds.Bolt_Normal_Shot.ID();
            }
        }

        internal static void PlayFire( GameObject source, Single chargeLevel, FireType fireType )
        {
            UInt32 id = AkSoundEngine.PostEvent( GetFireID( fireType ), source );
            //Log.WarningT( String.Format( "Charge level: {0}", chargeLevel ));
            _ = AkSoundEngine.SetRTPCValueByPlayingID( Sounds.Sniper_Charge_Amount.ID(), chargeLevel * 100f, id );
        }

        private const UInt32 bolt_open_chamber = 1389001356u;
        internal static void PlayOpenReload( GameObject source ) => _ = AkSoundEngine.PostEvent( bolt_open_chamber, source );

        private const UInt32 bolt_ricochet = 3672228492u;
        internal static void PlayRicochet( GameObject source ) => _ = AkSoundEngine.PostEvent( bolt_ricochet, source );

        internal static void PlayLoad( GameObject source, ReloadTier reloadTier ) => _ = AkSoundEngine.PostEvent( reloadTier.GetSound(), source );

        private static UInt32 GetSound( this ReloadTier tier )
        {
            switch( tier )
            {
                case ReloadTier.Bad:
                return Sounds.Bolt_New_Bullet_Trash.ID();

                case ReloadTier.Good:
                return Sounds.Bolt_New_Bullet_Good.ID();

                case ReloadTier.Perfect:
                return Sounds.Bolt_New_Bullet_Best.ID();

                default:
                return Sounds.Bolt_New_Bullet_Trash.ID();
            }
        }


        internal static void PlayKnifeHit( GameObject source, KnifeHitMaterial mat  )
        {
            _ = AkSoundEngine.PostEvent( mat.GetSound(), source );
        }

        private static UInt32 GetSound( this KnifeHitMaterial mat )
        {
            switch( mat )
            {
                case KnifeHitMaterial.Metallic:
                return Sounds.Knife_Projectile_Metallic_hit.ID();
                case KnifeHitMaterial.Organic:
                return Sounds.Knife_Projectile_Organic_hit.ID();
                default:
                return Sounds.Knife_Projectile_Metallic_hit.ID();
            }

        }
        internal enum KnifeHitMaterial
        {
            Organic,
            Metallic,
        }

    }

}
