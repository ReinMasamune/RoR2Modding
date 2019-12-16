namespace AlternateArtificer.Components
{
    using RoR2;
    using RoR2.Projectile;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;
    using UnityEngine;
    using UnityEngine.Networking;

    public class PassiveController : MonoBehaviour
    {
        private Boolean local;

        private Power firePower;
        private Power icePower;
        private Power lightningPower;

        private CharacterBody body;

        private GameObject lightningProjectile;
        private GameObject iceProjectile;
        private GameObject fireProjectile;

        private enum Power
        {
            None = 0,
            Low = 1,
            Medium = 2,
            High = 3,
            Extreme = 4
        }

        public void SkillCast( GenericSkill skill )
        {
            Debug.Log( "Skill: " + skill.baseSkill.skillNameToken + " cast." );

            for( Int32 i = 1; i <= (Int32)lightningPower; i++ )
            {
                StartCoroutine( FireLightningProjectile( 0.3f * i ) );
            }
        }

        
        public void Awake()
        {
            body = base.GetComponent<CharacterBody>();
            lightningProjectile = Resources.Load<GameObject>( "Prefabs/Projectiles/ElectricWormSeekerProjectile" );
            lightningProjectile.GetComponent<Rigidbody>().useGravity = false;

            var finder = lightningProjectile.GetComponent<ProjectileDirectionalTargetFinder>();
            finder.lookCone = 60f;
            finder.lookRange = 50f;
            finder.targetSearchInterval = 0.05f;
            finder.onlySearchIfNoTarget = true;
            finder.allowTargetLoss = true;
            finder.testLoS = false;
            finder.ignoreAir = false;

            var homing = lightningProjectile.GetComponent<ProjectileSteerTowardTarget>();
            homing.rotationSpeed = 180f;

            var simple = lightningProjectile.GetComponent<ProjectileSimple>();
            simple.velocity = 100f;
            simple.updateAfterFiring = true;

            var target = lightningProjectile.GetComponent<ProjectileTargetComponent>();
        }

        public void Start()
        {
            GetPowers();
            //local = Util.HasEffectiveAuthority( base.GetComponent<NetworkIdentity>() );
            local = true;
        }

        public void FixedUpdate()
        {
            SkillListen();
        }

        public void Update()
        {
            SkillListen();
        }

        private void SkillListen()
        {

        }



        

        private void GetPowers()
        {
            SkillLocator loc = base.GetComponent<SkillLocator>();

            firePower = Power.None;
            icePower = Power.None;
            lightningPower = Power.None;

            GetSkillPower( loc.primary );
            GetSkillPower( loc.secondary );
            GetSkillPower( loc.utility );
            GetSkillPower( loc.special );

            Debug.Log( "Fire: " + firePower.ToString() );
            Debug.Log( "Ice: " + icePower.ToString() );
            Debug.Log( "Lightning: " + lightningPower.ToString() );
        }

        private void GetSkillPower( GenericSkill skill )
        {
            String name = skill.baseSkill.skillNameToken.Split('_')[2].ToLower();
            switch( name )
            {
                default:
                    Debug.Log( "Element: " + name + " is not handled" );
                    break;
                case "fire":
                    firePower++;
                    break;
                case "ice":
                    icePower++;
                    break;
                case "lightning":
                    lightningPower++;
                    break;
            }

            skill.onSkillChanged += ( s ) => GetPowers();
        }

        private IEnumerator FireLightningProjectile( Single delay )
        {
            yield return new WaitForSeconds( delay );

            FireProjectileInfo info = new FireProjectileInfo
            {
                crit = Util.CheckRoll(body.crit, body.master ),
                damage = body.damage * 0.5f,
                damageColorIndex = DamageColorIndex.Default,
                force = 1.0f,
                owner = gameObject,
                position = gameObject.transform.position + new Vector3( 0f, 2.5f, 0f ),
                procChainMask = default(ProcChainMask),
                projectilePrefab = lightningProjectile,
                rotation = Util.QuaternionSafeLookRotation(body.inputBank.aimDirection),
                useFuseOverride = false,
                useSpeedOverride = false
            };

            if( local ) ProjectileManager.instance.FireProjectile( info );
        }
    }
}
