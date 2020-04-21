using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Sniper.Effects
{
    internal static partial class EffectCreator
    {
        internal static GameObject CreateExplosiveAmmoExplosion()
        {
            // TODO: Explosive Ammo Explosion
            return Resources.Load<GameObject>( "Prefabs/Effects/OmniEffect/OmniExplosionVFXCommandoGrenade" );
        }
    }
}
