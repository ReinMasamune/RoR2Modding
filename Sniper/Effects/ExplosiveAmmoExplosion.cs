namespace Sniper.Effects
{
    using UnityEngine;

    internal static partial class EffectCreator
    {
        internal static GameObject CreateExplosiveAmmoExplosion()
        {
            // TODO: Create explosive ammo explosion
            return Resources.Load<GameObject>( "Prefabs/Effects/OmniEffect/OmniExplosionVFXToolbotQuick" );
        }
    }
}
