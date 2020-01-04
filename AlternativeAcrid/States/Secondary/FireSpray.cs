namespace AlternativeAcrid.States.Secondary
{
    using EntityStates;
    using RoR2;
    using RoR2.Projectile;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using UnityEngine;

    public class FireSpray : BaseState
    {
        public static Single baseDuration = 0.2f;
        public static Single recoilAmplitude = 0.5f;
        public static Single bloom = 0.1f;
        public static Single damageRatio = 1.0f;
        public static Single force = 1f;
        public static Single spreadCone = 0.5f;

        public static Int32 pellets = 10;

        public static String attackString = "";

        public static GameObject projectilePrefab;
        public static GameObject effectPrefab;



        private Single duration;


        public override void OnEnter()
        {
            base.OnEnter();
            Ray aimRay = base.GetAimRay();
            this.duration = baseDuration / base.attackSpeedStat;
            base.StartAimMode( this.duration + 2f, false );
            base.PlayAnimation( "Gesture, Mouth", "FireSpit", "FireSpit.playbackRate", this.duration );
            Util.PlaySound( attackString, base.gameObject );
            base.AddRecoil( -1f * recoilAmplitude, -1.5f * recoilAmplitude, -0.25f * recoilAmplitude, -0.25f * recoilAmplitude );
            base.characterBody.AddSpreadBloom( bloom );

            String muzzleName = "MouthMuzzle";

            if( effectPrefab )
            {
                EffectManager.SimpleMuzzleFlash( effectPrefab, base.gameObject, muzzleName, false );
            }

            Boolean crit = RollCrit();
            Single damage = base.damageStat * damageRatio;

            if( base.isAuthority )
            {
                for( Int32 i = 0; i < pellets; i++ )
                {

                    Ray fire = this.GetFireRay( aimRay, spreadCone );
                    ProjectileManager.instance.FireProjectile( new FireProjectileInfo
                    {
                        crit = crit,
                        damage = damage,
                        damageColorIndex = DamageColorIndex.Default,
                        force = force,
                        owner = base.gameObject,
                        position = fire.origin,
                        procChainMask = default( ProcChainMask ),
                        projectilePrefab = projectilePrefab,
                        rotation = Util.QuaternionSafeLookRotation( fire.direction ),
                        target = null
                    } );
                }
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if( base.fixedAge >= this.duration && base.isAuthority )
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return base.fixedAge >= this.duration / 3f ? InterruptPriority.Any : InterruptPriority.Skill;
        }


        private Ray GetFireRay( Ray aim, Single cone )
        {
            Single zMin = Mathf.Cos( cone );
            Single z = UnityEngine.Random.Range( zMin, 1.0f );
            Single theta = UnityEngine.Random.Range( 0f, Mathf.PI * 2f );
            Single remMag = Mathf.Sqrt( 1f - (z*z) );

            Vector3 localDirection = new Vector3( remMag * Mathf.Cos( theta ) , remMag * Mathf.Sin( theta ), z );

            Quaternion rotation = Quaternion.FromToRotation( Vector3.forward, aim.direction );


            return new Ray( aim.origin, rotation * localDirection );
        }
    }
}
