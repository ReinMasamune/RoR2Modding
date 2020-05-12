namespace Sniper.Modules
{
    using ReinCore;

    using RoR2;
    using RoR2.Projectile;

    using UnityEngine;

    internal static class ProjectileGhostModule
    {
#pragma warning disable IDE1006 // Naming Styles
        private static GameObject _baseKnifeGhost;
#pragma warning restore IDE1006 // Naming Styles
        private static GameObject GetBaseKnifeGhost()
        {
            if( _baseKnifeGhost == null )
            {
                _baseKnifeGhost = CreateBaseKnifeGhost();
            }

            return _baseKnifeGhost;
        }
        private static GameObject CreateBaseKnifeGhost()
        {
            GameObject obj = AssetModule.GetSniperAssetBundle().LoadAsset<GameObject>( Properties.Resources.KnifeGhostPrefabPath );

            _ = obj.AddOrGetComponent<ProjectileGhostController>();

            VFXAttributes vfx = obj.AddOrGetComponent<VFXAttributes>();
            vfx.vfxPriority = VFXAttributes.VFXPriority.Always;
            vfx.vfxIntensity = VFXAttributes.VFXIntensity.Low;
            vfx.optionalLights = null;
            vfx.secondaryParticleSystem = null;

            return obj;
        }
    }
}
