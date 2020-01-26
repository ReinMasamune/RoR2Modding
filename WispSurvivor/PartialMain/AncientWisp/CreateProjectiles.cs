#if ANCIENTWISP
using R2API;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace RogueWispPlugin
{
    internal partial class Main
    {
        partial void AW_CreateProjectiles()
        {
            this.Load += this.AW_CreatePrimaryProjectile;
            this.Load += this.AW_CreateSpecialProjectile;
            this.Load += this.AW_CreateSpecialZoneProjectile;
        }

        private void AW_CreateSpecialZoneProjectile()
        {

        }

        private void AW_CreateSpecialProjectile()
        {
            var obj1 = Resources.Load<GameObject>("Prefabs/Projectiles/EvisProjectile" ).InstantiateClone("AWZoneLaunch" );
            var obj2 = Resources.Load<GameObject>("Prefabs/Projectiles/EvisOverlapProjectile").InstantiateClone("AWZone" );

            var impactExplosion1 = obj1.GetComponent<ProjectileImpactExplosion>();
            impactExplosion1.childrenProjectilePrefab = obj2;

            var healComp = obj2.AddComponent<ProjectileUniversalHealOrbOnDamage>();
            healComp.healType = UniversalHealOrb.HealType.PercentMax;
            healComp.healTarget = UniversalHealOrb.HealTarget.Barrier;
            healComp.value = 0.05f;
            healComp.effectPrefab = Resources.Load<GameObject>( "Prefabs/Effects/OrbEffects/HealthOrbEffect" );

            this.AW_utilProj = obj1;
            this.AW_utilZoneProj = obj2;

            ProjectileCatalog.getAdditionalEntries += ( list ) => list.Add( this.AW_utilProj );
            ProjectileCatalog.getAdditionalEntries += ( list ) => list.Add( this.AW_utilZoneProj );
        }

        private void AW_CreatePrimaryProjectile()
        {
            this.AW_primaryProj = Resources.Load<GameObject>( "Prefabs/Projectiles/WispCannon" ).InstantiateClone( "AncientWispCannon" );
            ProjectileCatalog.getAdditionalEntries += ( list ) => list.Add( this.AW_primaryProj );
        }
    }
}
#endif
