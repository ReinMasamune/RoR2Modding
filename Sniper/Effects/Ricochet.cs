namespace Rein.Sniper.Effects
{
    using ReinCore;

    using RoR2;

    using Rein.Sniper.Components;

    using UnityEngine;

    internal static partial class EffectCreator
	{
        internal static GameObject CreateRicochetEffect()
        {
            GameObject obj = PrefabsCore.CreatePrefab( "SniperRicochetEffect", false );

            var effectComp = obj.AddComponent<EffectComponent>();
            var vfxAtrib = obj.AddComponent<VFXAttributes>();
            obj.AddComponent<RicochetEffectController>().destroyDelay = 2f;

            EffectsCore.AddEffect( obj );

            return obj;
        }

	}
}
