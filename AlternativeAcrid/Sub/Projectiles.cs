namespace AlternativeAcrid
{
    using R2API;
    using RoR2;
    using RoR2.Projectile;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using UnityEngine;
    partial class Main
    {
        private void CreateProjectiles()
        {
            GameObject secondaryProj = Resources.Load<GameObject>("Prefabs/Projectiles/CrocoSpit").InstantiateClone("sprayProj");

            States.Secondary.FireSpray.projectilePrefab = secondaryProj;

            ProjectileCatalog.getAdditionalEntries += ( list ) =>
            {
                list.Add( secondaryProj );
            };

            ProjectileSimple spraySimp = secondaryProj.GetComponent<ProjectileSimple>();
            spraySimp.lifetime = 0.3f;

            ProjectileController controller = secondaryProj.GetComponent<ProjectileController>();
            controller.procCoefficient = 0.5f * ( ( 1f / numberOfPellets ) + Mathf.Sqrt( 1f / numberOfPellets ) );
        }
    }
}
