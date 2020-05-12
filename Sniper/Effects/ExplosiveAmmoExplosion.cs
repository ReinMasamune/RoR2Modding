namespace Sniper.Effects
{
    using System;
    using System.Collections.Generic;
    using System.Text;
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
