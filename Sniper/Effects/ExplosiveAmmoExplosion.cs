namespace Rein.Sniper.Effects
{
    using UnityEngine;

    internal static partial class EffectCreator
    {
        internal static GameObject CreateExplosiveAmmoExplosion()
        {
            // FUTURE: Create explosive ammo explosion
            return Resources.Load<GameObject>( "Prefabs/Effects/OmniEffect/OmniExplosionVFXToolbotQuick" );
        }
    }
}
