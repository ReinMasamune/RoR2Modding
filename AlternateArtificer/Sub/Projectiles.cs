namespace AlternateArtificer
{
    using BepInEx;
    using RoR2.Projectile;
    using System;
    using UnityEngine;

    public partial class Main
    {
        private void EditProjectiles()
        {
            EditNovaBomb();
            EditPlasmaBolt();
        }

        private void CreateProjectiles()
        {
            CreateLightningSword();
        }

        private void EditNovaBomb()
        {
            var novaProj = Resources.Load<GameObject>("Prefabs/Projectiles/MageLightningBombProjectile");

            novaProj.GetComponent<Rigidbody>().useGravity = true;

            var novaImpact = novaProj.GetComponent<ProjectileImpactExplosion>();
            novaImpact.blastDamageCoefficient = 1.2f;
            //novaImpact.falloffModel = RoR2.BlastAttack.FalloffModel.Linear;
            novaImpact.blastRadius = 12f;
           
        }

        private void EditPlasmaBolt()
        {
            // TODO: implement
        }

        private void CreateLightningSword()
        {

        }
    }
}
