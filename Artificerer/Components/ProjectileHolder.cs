using RoR2;
using UnityEngine;
using RoR2.UI;
using RoR2.Projectile;
using R2API.Utils;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using EntityStates;
using System;

namespace ReinArtificerer
{
    public static class ReinProjectileHolder
    {
        #region Fields
        #region Misc Fields
        //=========================================================================
        //General Config
        //=========================================================================
        //floats
        private const float requestCheckDelay = 0.02f;


        //=========================================================================
        //Generic data
        //=========================================================================
        public ReinElementTracker element;
        public ReinLightningBuffTracker lightning;
        public EntityStateMachine bodyState;
        public EntityStateMachine weaponState;
        public AssetBundle bundle;
        //=========================================================================
        //Element tracker settings
        //=========================================================================
        //floats
        //ints
        public readonly int el_maxElementLevel = 5;
        public readonly int el_bonusFromAffixBuff = 1;
        public readonly int el_bonusFromAffixItem = 1;
        #endregion
        #region Primary Fields
        //=========================================================================
        //Primary
        //=========================================================================
        //floats
        public readonly float p_blinkCastValue = 1.0f;
        public readonly float p_baseRadius = 1.0f;
        public readonly float p_attackSpeedAnimationSwitch = 1.0f;
        public readonly float p_baseDuration = 0.25f;
        public readonly float p_fireSoundPitch = 1.3f;
        public readonly float p_bloom = 0.2f;
        public readonly float p_damageCoef = 2.0f;
        public readonly float p_procCoef = 1.0f;
        public readonly float p_hitForce = 300.0f;
        public readonly float p_f_radMod = 0.15f;
        public readonly float p_i_baseDur = 3.0f;
        public readonly float p_i_durMod = 0.15f;
        public readonly float p_l_proxRange = 20.0f;
        public readonly float p_l_proxRangeMod = 0.15f;
        public readonly float p_l_proxAttackInt = 1.0f;
        public readonly float p_l_proxAttackIntMod = 0.15f;
        public readonly float p_l_proxProcCoef = 0.1f;
        public readonly float p_l_proxDamageCoef = 1.0f;
        public readonly float p_l_proxListClear = 1.0f;
        public readonly float p_l_proxListClearMod = 0.15f;
        //ints
        //bools
        //strings
        public readonly string p_fireSound = "Play_mage_m1_shoot";
        //loadpaths
        private const string path_p_projectile = "Prefabs/Projectiles/MageFireboltBasic";
        private const string path_p_f_projectile = "Prefabs/ProjectileGhosts/MageFireboltGhost";
        private const string path_p_f_muzzle = "Prefabs/Effects/MuzzleFlashes/MuzzleFlashMageFire";
        private const string path_p_f_impact = "";
        private const string path_p_f_trail = "Prefabs/MageFireTrail";
        private const string path_p_i_projectile = "Prefabs/ProjectileGhosts/MageIceboltGhost";
        private const string path_p_i_muzzle = "Prefabs/Effects/MuzzleFlashes/MuzzleFlashMageIce";
        private const string path_p_i_impact = "";
        private const string path_p_i_frostProj = "Prefabs/Projectiles/MageIceBoltExpanded";
        private const string path_p_i_frostEffect = "";
        private const string path_p_l_projectile = "Prefabs/ProjectileGhosts/MageLightningboltGhost";
        private const string path_p_l_muzzle = "Prefabs/Effects/MuzzleFlashes/MuzzleFlashMageLightning";
        private const string path_p_l_impact = "";
        //load routines
        private ResourceRequest req_p_projectile;
        private ResourceRequest req_p_f_projectile;
        private ResourceRequest req_p_f_muzzle;
        private ResourceRequest req_p_f_impact;
        private ResourceRequest req_p_f_trail;
        private ResourceRequest req_p_i_projectile;
        private ResourceRequest req_p_i_muzzle;
        private ResourceRequest req_p_i_impact;
        private ResourceRequest req_p_i_frostProj;
        private ResourceRequest req_p_i_frostEffect;
        private ResourceRequest req_p_l_projectile;
        private ResourceRequest req_p_l_muzzle;
        private ResourceRequest req_p_l_impact;
        private IEnumerator en_p_projectile;
        private IEnumerator en_p_f_projectile;
        private IEnumerator en_p_f_muzzle;
        private IEnumerator en_p_f_impact;
        private IEnumerator en_p_f_trail;
        private IEnumerator en_p_i_projectile;
        private IEnumerator en_p_i_muzzle;
        private IEnumerator en_p_i_impact;
        private IEnumerator en_p_i_frostProj;
        private IEnumerator en_p_i_frostEffect;
        private IEnumerator en_p_l_projectile;
        private IEnumerator en_p_l_muzzle;
        private IEnumerator en_p_l_impact;
        //other
        public GameObject p_projectile;
        public GameObject p_f_projectile;
        public GameObject p_f_muzzle;
        public GameObject p_f_impact;
        public GameObject p_f_trail;
        public GameObject p_i_projectile;
        public GameObject p_i_muzzle;
        public GameObject p_i_impact;
        public GameObject p_i_frostProj;
        public GameObject p_i_frostEffect;
        public GameObject p_l_projectile;
        public GameObject p_l_muzzle;
        public GameObject p_l_impact;
        //components
        public ProjectileDamage p_dmg;
        public ProjectileController p_control;
        public ProjectileSimple p_simple;
        public ProjectileImpactExplosion p_explode;
        public ProjectileDamageTrail p_trailProj;
        public ProjectileProximityBeamController p_proxBeams;
        public ProjectileSimple p_i_frostSimp;
        #endregion
        #region Secondary Fields
        //=========================================================================
        //Secondary
        //=========================================================================
        //floats
        public readonly float s_blinkCastValue = 1.0f;
        public readonly float s_chargeDuration = 2.0f;
        public readonly float s_minChargeDuration = 0.5f;
        public readonly float s_winddownDuration = 0.4f;
        public readonly float s_minRadius = 0.1f;
        public readonly float s_maxRadius = 0.5f;
        public readonly float s_minDamageCoef = 4.0f;
        public readonly float s_maxDamageCoef = 12.0f;
        
        public readonly float s_f_hitForce = 1300.0f;
        public readonly float s_f_selfForce = 1000.0f;
        
        public readonly float s_i_hitForce = 1300.0f;
        public readonly float s_i_selfForce = 1000.0f;
       
        public readonly float s_l_hitForce = 1300.0f;
        public readonly float s_l_selfForce = 1000.0f;
        //ints
        //bools
        //strings
        public readonly string s_fireSound = "Play_mage_m2_shoot";
        public readonly string s_chargeSound = "Play_mage_m2_charge";
        //loadpaths
        private const string path_s_projectile = "Prefabs/Projectiles/MageLightningBombProjectile";
        private const string path_s_f_projectile = "Prefabs/ProjectileGhosts/RedAffixMissileGhost";
        private const string path_s_f_muzzle = "Prefabs/Effects/Muzzleflashes/MuzzleflashMageFireLarge";
        private const string path_s_f_charge = "Prefabs/Effects/ChargeMageFireBomb";
        private const string path_s_f_child = "Prefabs/Projectiles/MageFireBoltExpanded";
        private const string path_s_f_childGhost = "Prefabs/ProjectileGhosts/MagmaOrbGhost";
        private const string path_s_i_projectile = "Prefabs/ProjectileGhosts/MageIceBombGhost";
        private const string path_s_i_muzzle = "Prefabs/Effects/Muzzleflashes/MuzzleflashMageIceLarge";
        private const string path_s_i_charge = "Prefabs/Effects/ChargeMageIceBomb";
        private const string path_s_i_freezeEffect = "Prefabs/Effects/ImpactEffects/AffixWhiteExplosion";
        private const string path_s_l_projectile = "Prefabs/ProjectileGhosts/MageLightningBombGhost";
        private const string path_s_l_muzzle = "Prefabs/Effects/Muzzleflashes/MuzzleflashMageLightningLarge";
        private const string path_s_l_charge = "Prefabs/Effects/ChargeMageLightningBomb";
        //load routines
        private ResourceRequest req_s_projectile;
        private ResourceRequest req_s_f_projectile;
        private ResourceRequest req_s_f_muzzle;
        private ResourceRequest req_s_f_charge;
        private ResourceRequest req_s_f_child;
        private ResourceRequest req_s_f_childGhost;
        private ResourceRequest req_s_i_projectile;
        private ResourceRequest req_s_i_muzzle;
        private ResourceRequest req_s_i_charge;
        private ResourceRequest req_s_l_projectile;
        private ResourceRequest req_s_l_muzzle;
        private ResourceRequest req_s_l_charge;
        private IEnumerator en_s_projectile;
        private IEnumerator en_s_f_projectile;
        private IEnumerator en_s_f_muzzle;
        private IEnumerator en_s_f_charge;
        private IEnumerator en_s_f_child;
        private IEnumerator en_s_i_projectile;
        private IEnumerator en_s_i_muzzle;
        private IEnumerator en_s_i_charge;
        private IEnumerator en_s_l_projectile;
        private IEnumerator en_s_l_muzzle;
        private IEnumerator en_s_l_charge;
        //other
        public GameObject s_crosshairOverridePrefab;
        public GameObject s_projectile;
        public GameObject s_f_projectile;
        public GameObject s_f_muzzle;
        public GameObject s_f_charge;
        public GameObject s_f_child;
        public GameObject s_f_childGhost;
        public GameObject s_i_projectile;
        public GameObject s_i_muzzle;
        public GameObject s_i_charge;
        public GameObject s_l_projectile;
        public GameObject s_l_muzzle;
        public GameObject s_l_charge;
        //components
        public ProjectileDamage s_dmg;
        public ProjectileController s_control;
        public ProjectileSimple s_simple;
        public ProjectileImpactExplosion s_explode;
        public ProjectileDamageTrail s_trailProj;
        public ProjectileProximityBeamController s_proxBeams;
        public IcicleAuraController s_icicleControl;
        #endregion
        #region Utility Fields
        //=========================================================================
        //Utility
        //=========================================================================
        //floats
        public readonly float u_blinkCastValue = 1.0f;
        public readonly float u_baseDuration = 0.3f;
        public readonly float u_maxRange = 600.0f;
        public readonly float u_maxSlope = 70.0f;
        public readonly float u_f_damageCoef = 1.0f;
        public readonly float u_f_procCoef = 1.0f;
        public readonly float u_i_damageCoef = 1.0f;
        public readonly float u_i_procCoef = 1.0f;
        public readonly float u_l_damageCoef = 1.0f;
        public readonly float u_l_procCoef = 1.0f;
        //ints
        //bools
        //strings
        public readonly string u_prepSound = "";
        public readonly string u_fireSound = "";
        //loadpaths
        private const string path_u_seedProjectile = "Prefabs/Projectiles/MageIceWallSeedProjectile";
        private const string path_u_walkerProjectile = "Prefabs/Projectiles/MageIceWallWalkerProjectile";
        private const string path_u_f_pillar = "Prefabs/Projectiles/MageFireWallPillarProjectile";
        private const string path_u_f_muzzle = "Prefabs/Effects/Muzzleflashes/MuzzleflashMageFire";
        private const string path_u_i_pillar = "Prefabs/Projectiles/MageIceWallPillarProjectile";
        private const string path_u_i_muzzle = "Prefabs/Effects/Muzzleflashes/MuzzleflashMageIce";
        private const string path_u_l_pillar = "Prefabs/Projectiles/MageLightningWallSeedProjectile";
        private const string path_u_l_muzzle = "Prefabs/Effects/Muzzleflashes/MuzzleflashMageLightning";
        private const string path_u_l_pillarGhost = "Prefabs/ProjectileGhosts/ElectricOrbGhost";
        private const string path_u_l_impactEffect = "Prefabs/Effects/LightningStakeNova";
        private const string path_u_l_childProj = "Prefabs/Projectiles/ElectricWormSeekerProjectile";
        //load routines
        private ResourceRequest req_u_seedProjectile;
        private ResourceRequest req_u_walkerProjectile;
        private ResourceRequest req_u_f_pillar;
        private ResourceRequest req_u_f_muzzle;
        private ResourceRequest req_u_i_pillar;
        private ResourceRequest req_u_i_muzzle;
        private ResourceRequest req_u_l_pillar;
        private ResourceRequest req_u_l_muzzle;
        private ResourceRequest req_u_l_pillarGhost;
        private ResourceRequest req_u_l_impactEffect;
        private ResourceRequest req_u_l_childProj;
        private IEnumerator en_u_seedProjectile;
        private IEnumerator en_u_f_pillar;
        private IEnumerator en_u_f_muzzle;
        private IEnumerator en_u_i_pillar;
        private IEnumerator en_u_i_muzzle;
        private IEnumerator en_u_l_pillar;
        private IEnumerator en_u_l_muzzle;
        //other
        public GameObject u_areaIndicator;
        public GameObject u_goodCrosshair;
        public GameObject u_badCrosshair;
        public GameObject u_seedProjectile;
        public GameObject u_walkerProjectile;
        public GameObject u_f_pillar;
        public GameObject u_f_muzzle;
        public GameObject u_i_pillar;
        public GameObject u_i_muzzle;
        public GameObject u_l_pillar;
        public GameObject u_l_muzzle;
        public GameObject u_l_pillarGhost;
        public GameObject u_l_impactEffect;
        public GameObject u_l_childProj;
        //components
        public ProjectileMageFirewallWalkerController u_walkerController;
        #endregion
        #region Special Fields
        //=========================================================================
        //Special
        //=========================================================================
        //floats
        public readonly float r_blinkCastValue = 1.0f;
        public readonly float r_f_maxDistance = 20.0f;
        public readonly float r_f_radius = 2.0f;
        public readonly float r_f_entryDur = 0.6f;
        public readonly float r_f_totalDur = 3.0f;
        public readonly float r_f_totalDamage = 20.0f;
        public readonly float r_f_procCoef = 1.0f;
        public readonly float r_f_tickFreq = 7.0f;
        public readonly float r_f_hitForce = 100.0f;
        public readonly float r_f_igniteChance = 50.0f;
        public readonly float r_f_recoil = 0.0f;
        //ints
        //bools
        //strings
        public readonly string r_f_startSound = "Play_mage_R_start";
        public readonly string r_f_endSound = "Play_mage_R_end";
        //loadpaths
        private const string path_r_f_mainEffect = "";
        private const string path_r_f_impactEffect = "";
        //load routines
        private ResourceRequest req_r_f_mainEffect;
        private ResourceRequest req_r_f_impactEffect;
        //other
        public GameObject r_f_mainEffect;
        public GameObject r_f_tracerEffect;
        public GameObject r_f_impactEffect;
        public GameObject r_i_areaIndicator;
        #endregion
        #region Blink Fields
        //=========================================================================
        //Blink
        //=========================================================================
        //floats
        public readonly float b_speedCoef = 15.0f;
        public readonly float b_baseDuration = 0.5f;
        //strings
        public readonly string b_sound1 = "";
        public readonly string b_sound2 = "";
        //loadpaths
        private const string path_b_mat1 = "";
        private const string path_b_mat2 = "";
        private const string path_b_prefab = "";
        //load routines
        private ResourceRequest req_b_mat1;
        private ResourceRequest req_b_mat2;
        private ResourceRequest req_b_prefab;
        private IEnumerator en_b_mat1;
        private IEnumerator en_b_mat2;
        private IEnumerator en_b_prefab;
        //other
        public Material b_mat1;
        public Material b_mat2;
        public GameObject b_prefab;
        #endregion

        #endregion
        #region Prefab loading
        //=========================================================================
        //Loading
        //=========================================================================
        private void Awake()
        {
            var skill1 = EntityState.Instantiate(125);
            var skill2 = EntityState.Instantiate(59);
            var skill3 = EntityState.Instantiate(279);
            var skill4 = EntityState.Instantiate(190);
            var skill5 = EntityState.Instantiate(58);

            #region Primary prefab loading
            //Primary prefabs
            Debug.Log("p_projectile");
            if( !p_projectile || !p_i_frostProj || !p_i_frostEffect )
            {
                req_p_projectile = Resources.LoadAsync<GameObject>(path_p_projectile);
                req_p_i_frostProj = Resources.LoadAsync<GameObject>(path_p_i_frostProj);
                req_p_i_frostEffect = Resources.LoadAsync<GameObject>(path_p_i_frostEffect);
                en_p_projectile = Co_p_projectile(req_p_projectile , req_p_i_frostProj , req_p_i_frostEffect );
                StartCoroutine(en_p_projectile);
            }

            Debug.Log("p_f_projectile");
            if( !p_f_projectile )
            {
                req_p_f_projectile = Resources.LoadAsync<GameObject>(path_p_f_projectile);
                en_p_f_projectile = Co_p_f_projectile(req_p_f_projectile);
                StartCoroutine(en_p_f_projectile);
            }

            Debug.Log("p_f_muzzle");
            if (!p_f_muzzle)
            {
                req_p_f_muzzle = Resources.LoadAsync<GameObject>(path_p_f_muzzle);
                en_p_f_muzzle = Co_p_f_muzzle(req_p_f_muzzle);
                StartCoroutine(en_p_f_muzzle);
            }

            /*
            Debug.Log("p_f_impact");
            if( !p_f_impact )
            {
                req_p_f_impact = Resources.LoadAsync<GameObject>(path_p_f_impact);
                en_p_f_impact = Co_p_f_impact(req_p_f_impact);
                StartCoroutine(en_p_f_impact);
            }
            */

            Debug.Log("p_f_trail");
            if (!p_f_trail)
            {
                req_p_f_trail = Resources.LoadAsync<GameObject>(path_p_f_trail);
                en_p_f_trail = Co_p_f_trail(req_p_f_trail);
                StartCoroutine(en_p_f_trail);
            }

            Debug.Log("p_i_projectile");
            if (!p_i_projectile)
            {
                req_p_i_projectile = Resources.LoadAsync<GameObject>(path_p_i_projectile);
                en_p_i_projectile = Co_p_i_projectile(req_p_i_projectile);
                StartCoroutine(en_p_i_projectile);
            }

            Debug.Log("p_i_muzzle");
            if (!p_i_muzzle)
            {
                req_p_i_muzzle = Resources.LoadAsync<GameObject>(path_p_i_muzzle);
                en_p_i_muzzle = Co_p_i_muzzle(req_p_i_muzzle);
                StartCoroutine(en_p_i_muzzle);
            }

            /*
            Debug.Log("p_i_impact");
            if (!p_i_impact)
            {
                req_p_i_impact = Resources.LoadAsync<GameObject>(path_p_i_impact);
                en_p_i_impact = Co_p_i_impact(req_p_i_impact);
                StartCoroutine(en_p_i_impact);
            }
            */

            Debug.Log("p_l_projectile");
            if (!p_l_projectile)
            {
                req_p_l_projectile = Resources.LoadAsync<GameObject>(path_p_l_projectile);
                en_p_l_projectile = Co_p_l_projectile(req_p_l_projectile);
                StartCoroutine(en_p_l_projectile);
            }

            Debug.Log("p_l_muzzle");
            if (!p_l_muzzle)
            {
                req_p_l_muzzle = Resources.LoadAsync<GameObject>(path_p_l_muzzle);
                en_p_l_muzzle = Co_p_l_muzzle(req_p_l_muzzle);
                StartCoroutine(en_p_l_muzzle);
            }

            /*
            Debug.Log("p_l_impact");
            if (!p_l_impact)
            {
                req_p_l_impact = Resources.LoadAsync<GameObject>(path_p_l_impact);
                en_p_l_impact = Co_p_l_impact(req_p_l_impact);
                StartCoroutine(en_p_l_impact);
            }
            */
            #endregion
            #region Secondary prefab loading
            //Secondary prefabs
            Debug.Log("s_projectile");
            if( !s_projectile )
            {
                req_s_projectile = Resources.LoadAsync<GameObject>(path_s_projectile);
                en_s_projectile = Co_s_projectile(req_s_projectile);
                StartCoroutine(en_s_projectile);
            }

            Debug.Log("s_f_projectile + s_f_charge");
            if( !s_f_projectile || !s_f_charge )
            {
                req_s_f_projectile = Resources.LoadAsync<GameObject>(path_s_f_projectile);
                req_s_f_charge = Resources.LoadAsync<GameObject>(path_s_f_charge);
                en_s_f_projectile = Co_s_f_projectile(req_s_f_projectile, req_s_f_charge);
                StartCoroutine(en_s_f_projectile);
            }

            Debug.Log("s_f_child + s_f_childGhost");
            if( !s_f_child | !s_f_childGhost  )
            {
                req_s_f_child = Resources.LoadAsync<GameObject>(path_s_f_child);
                req_s_f_childGhost = Resources.LoadAsync<GameObject>(path_s_f_childGhost);
                en_s_f_child = Co_s_f_child(req_s_f_child, req_s_f_childGhost);
                StartCoroutine(en_s_f_child);
            }

            Debug.Log("s_f_muzzle");
            if (!s_f_muzzle)
            {
                req_s_f_muzzle = Resources.LoadAsync<GameObject>(path_s_f_muzzle);
                en_s_f_muzzle = Co_s_f_muzzle(req_s_f_muzzle);
                StartCoroutine(en_s_f_muzzle);
            }

            Debug.Log("s_i_projectile");
            if (!s_i_projectile)
            {
                req_s_i_projectile = Resources.LoadAsync<GameObject>(path_s_i_projectile);
                en_s_i_projectile = Co_s_i_projectile(req_s_i_projectile);
                StartCoroutine(en_s_i_projectile);
            }

            Debug.Log("s_i_muzzle");
            if (!s_i_muzzle)
            {
                req_s_i_muzzle = Resources.LoadAsync<GameObject>(path_s_i_muzzle);
                en_s_i_muzzle = Co_s_i_muzzle(req_s_i_muzzle);
                StartCoroutine(en_s_i_muzzle);
            }

            Debug.Log("s_i_charge");
            if (!s_i_charge)
            {
                req_s_i_charge = Resources.LoadAsync<GameObject>(path_s_i_charge);
                en_s_i_charge = Co_s_i_charge(req_s_i_charge);
                StartCoroutine(en_s_i_charge);
            }

            Debug.Log("s_l_projectile");
            if (!s_l_projectile)
            {
                req_s_l_projectile = Resources.LoadAsync<GameObject>(path_s_l_projectile);
                en_s_l_projectile = Co_s_l_projectile(req_s_l_projectile);
                StartCoroutine(en_s_l_projectile);
            }

            Debug.Log("s_l_muzzle");
            if (!s_l_muzzle)
            {
                req_s_l_muzzle = Resources.LoadAsync<GameObject>(path_s_l_muzzle);
                en_s_l_muzzle = Co_s_l_muzzle(req_s_l_muzzle);
                StartCoroutine(en_s_l_muzzle);
            }

            Debug.Log("s_l_charge");
            if (!s_l_charge)
            {
                req_s_l_charge = Resources.LoadAsync<GameObject>(path_s_l_charge);
                en_s_l_charge = Co_s_l_charge(req_s_l_charge);
                StartCoroutine(en_s_l_charge);
            }

            Debug.Log("s_crosshairOverride");
            s_crosshairOverridePrefab = skill2.GetFieldValue<GameObject>("crosshairOverridePrefab");
            DontDestroyOnLoad(s_crosshairOverridePrefab);
            #endregion
            #region Utility prefab loading
            //Utility prefabs
            Debug.Log("u_seed + u_walker");
            if( !u_seedProjectile || !u_walkerProjectile )
            {
                req_u_seedProjectile = Resources.LoadAsync<GameObject>(path_u_seedProjectile);
                req_u_walkerProjectile = Resources.LoadAsync<GameObject>(path_u_walkerProjectile);
                en_u_seedProjectile = Co_u_seedProjectile(req_u_seedProjectile, req_u_walkerProjectile);
                StartCoroutine(en_u_seedProjectile);
            }

            Debug.Log("u_f_pillar");
            if( !u_f_pillar )
            {
                req_u_f_pillar = Resources.LoadAsync<GameObject>(path_u_f_pillar);
                en_u_f_pillar = Co_u_f_pillar(req_u_f_pillar);
                StartCoroutine(en_u_f_pillar);
            }

            Debug.Log("u_f_muzzle");
            if (!u_f_muzzle)
            {
                req_u_f_muzzle = Resources.LoadAsync<GameObject>(path_u_f_muzzle);
                en_u_f_muzzle = Co_u_f_muzzle(req_u_f_muzzle);
                StartCoroutine(en_u_f_muzzle);
            }

            Debug.Log("u_i_pillar");
            if (!u_i_pillar)
            {
                req_u_i_pillar = Resources.LoadAsync<GameObject>(path_u_i_pillar);
                en_u_i_pillar = Co_u_i_pillar(req_u_i_pillar);
                StartCoroutine(en_u_i_pillar);
            }

            Debug.Log("u_i_muzzle");
            if (!u_i_muzzle)
            {
                req_u_i_muzzle = Resources.LoadAsync<GameObject>(path_u_i_muzzle);
                en_u_i_muzzle = Co_u_i_muzzle(req_u_i_muzzle);
                StartCoroutine(en_u_i_muzzle);
            }

            Debug.Log("u_l_pillar + u_l_pillarGhost + u_l_impactEffect + u_l_childProj");
            if (!u_l_pillar)
            {
                req_u_l_pillar = Resources.LoadAsync<GameObject>(path_u_l_pillar);
                req_u_l_pillarGhost = Resources.LoadAsync<GameObject>(path_u_l_pillarGhost);
                req_u_l_impactEffect = Resources.LoadAsync<GameObject>(path_u_l_impactEffect);
                req_u_l_childProj = Resources.LoadAsync<GameObject>(path_u_l_childProj);
                en_u_l_pillar = Co_u_l_pillar(req_u_l_pillar, req_u_l_pillarGhost, req_u_l_impactEffect, req_u_l_childProj);
                StartCoroutine(en_u_l_pillar);
            }

            Debug.Log("u_l_muzzle");
            if (!u_l_muzzle)
            {
                req_u_l_muzzle = Resources.LoadAsync<GameObject>(path_u_l_muzzle);
                en_u_l_muzzle = Co_u_l_muzzle(req_u_l_muzzle);
                StartCoroutine(en_u_l_muzzle);
            }

            //Debug.Log("u_i_projectile");
            //u_i_projectile = skill3.GetFieldValue<GameObject>("projectilePrefab");
            //DontDestroyOnLoad(u_i_projectile);

            //Debug.Log("u_i_muzzle");
            //u_i_muzzle = skill3.GetFieldValue<GameObject>("muzzleflashEffect");
            //DontDestroyOnLoad(u_i_muzzle);

            Debug.Log("u_areaIndicator");
            u_areaIndicator = skill3.GetFieldValue<GameObject>("areaIndicatorPrefab");
            DontDestroyOnLoad(u_areaIndicator);

            Debug.Log("u_goodCrosshair");
            u_goodCrosshair = skill3.GetFieldValue<GameObject>("goodCrosshairPrefab");
            DontDestroyOnLoad(u_goodCrosshair);

            Debug.Log("u_badCrosshair");
            u_badCrosshair = skill3.GetFieldValue<GameObject>("badCrosshairPrefab");
            DontDestroyOnLoad(u_badCrosshair);

            #endregion
            #region Special prefab loading
            //Special prefabs
            Debug.Log("r_f_mainEffect");
            r_f_mainEffect = skill4.GetFieldValue<GameObject>("flamethrowerEffectPrefab");
            DontDestroyOnLoad(r_f_mainEffect);

            Debug.Log("r_f_impacteffect");
            r_f_impactEffect = skill4.GetFieldValue<GameObject>("impactEffectPrefab");
            DontDestroyOnLoad(r_f_impactEffect);


            r_i_areaIndicator = skill5.GetFieldValue<GameObject>("areaIndicatorPrefab");
            //Debug.Log("r_f_tracerEffect");
            //r_f_tracerEffect = Instantiate(skill4.GetFieldValue<GameObject>("tracerEffectPrefab"));
            //DontDestroyOnLoad(r_f_tracerEffect);
            #endregion
        }
        #endregion
        #region Prefab setup
        //=========================================================================
        //IEnumerators
        //=========================================================================
        #region Primary
        private IEnumerator Co_p_projectile( ResourceRequest r1 , ResourceRequest r2 , ResourceRequest r3)
        {
            while( !r1.isDone )
            {
                yield return new WaitForSeconds(requestCheckDelay);
            }
            p_projectile = (GameObject)r1.asset;
            p_i_frostProj = (GameObject)r2.asset;
            p_i_frostEffect = (GameObject)r3.asset;

            p_explode = p_projectile.GetComponent<ProjectileImpactExplosion>();
            p_dmg = p_projectile.GetComponent<ProjectileDamage>();
            p_control = p_projectile.GetComponent<ProjectileController>();
            p_simple = p_projectile.GetComponent<ProjectileSimple>();

            p_i_frostSimp = p_i_frostProj.GetComponent<ProjectileSimple>();

            p_explode.fireChildren = false;
            p_explode.childrenCount = 1;
            p_explode.childrenDamageCoefficient = 1.0f;
            p_explode.childrenProjectilePrefab = p_i_frostProj;

            p_proxBeams = p_projectile.GetComponent<ProjectileProximityBeamController>();
            if( !p_proxBeams )
            {
                p_proxBeams = p_projectile.AddComponent<ProjectileProximityBeamController>();
                p_proxBeams.lightningType = RoR2.Orbs.LightningOrb.LightningType.Ukulele;
                p_proxBeams.attackInterval = p_l_proxAttackInt;
                p_proxBeams.listClearInterval = p_l_proxListClear;
                p_proxBeams.attackRange = p_l_proxRange;
                p_proxBeams.procCoefficient = p_l_proxProcCoef;
                p_proxBeams.damageCoefficient = p_l_proxDamageCoef;
            }
        }
        #region Fire
        private IEnumerator Co_p_f_projectile(ResourceRequest r1)
        {
            while (!r1.isDone)
            {
                yield return new WaitForSeconds(requestCheckDelay);
            }
            p_f_projectile = (GameObject)r1.asset;
        }
        private IEnumerator Co_p_f_muzzle(ResourceRequest r1)
        {
            while (!r1.isDone)
            {
                yield return new WaitForSeconds(requestCheckDelay);
            }
            p_f_muzzle = (GameObject)r1.asset;
        }
        private IEnumerator Co_p_f_impact(ResourceRequest r1)
        {
            while (!r1.isDone)
            {
                yield return new WaitForSeconds(requestCheckDelay);
            }
            p_f_impact = (GameObject)r1.asset;
        }
        private IEnumerator Co_p_f_trail(ResourceRequest r1)
        {
            while (!r1.isDone)
            {
                yield return new WaitForSeconds(requestCheckDelay);
            }
            p_f_trail = (GameObject)r1.asset;
        }
        #endregion
        #region Ice
        private IEnumerator Co_p_i_projectile(ResourceRequest r1)
        {
            while (!r1.isDone)
            {
                yield return new WaitForSeconds(requestCheckDelay);
            }
            p_i_projectile = (GameObject)r1.asset;
        }
        private IEnumerator Co_p_i_muzzle(ResourceRequest r1)
        {
            while (!r1.isDone)
            {
                yield return new WaitForSeconds(requestCheckDelay);
            }
            p_i_muzzle = (GameObject)r1.asset;
        }
        private IEnumerator Co_p_i_impact(ResourceRequest r1)
        {
            while (!r1.isDone)
            {
                yield return new WaitForSeconds(requestCheckDelay);
            }
            p_i_impact = (GameObject)r1.asset;
        }
        #endregion
        #region Lightning
        private IEnumerator Co_p_l_projectile(ResourceRequest r1)
        {
            while (!r1.isDone)
            {
                yield return new WaitForSeconds(requestCheckDelay);
            }
            p_l_projectile = (GameObject)r1.asset;
        }
        private IEnumerator Co_p_l_muzzle(ResourceRequest r1)
        {
            while (!r1.isDone)
            {
                yield return new WaitForSeconds(requestCheckDelay);
            }
            p_l_muzzle = (GameObject)r1.asset;
        }
        private IEnumerator Co_p_l_impact(ResourceRequest r1)
        {
            while (!r1.isDone)
            {
                yield return new WaitForSeconds(requestCheckDelay);
            }
            p_l_impact = (GameObject)r1.asset;
        }
        #endregion
        #endregion
        #region Secondary
        private IEnumerator Co_s_projectile( ResourceRequest r1 )
        {
            while(!r1.isDone)
            {
                yield return new WaitForSeconds(requestCheckDelay);
            }
            s_projectile = (GameObject)r1.asset;

            s_control = s_projectile.GetComponent<ProjectileController>();
            s_simple = s_projectile.GetComponent<ProjectileSimple>();
            s_explode = s_projectile.GetComponent<ProjectileImpactExplosion>();
            s_dmg = s_projectile.GetComponent<ProjectileDamage>();


            s_proxBeams = s_projectile.GetComponent<ProjectileProximityBeamController>();
            s_proxBeams.enabled = false;

            s_trailProj = s_projectile.GetComponent<ProjectileDamageTrail>();
            if( !s_trailProj )
            {
                s_trailProj = s_projectile.AddComponent<ProjectileDamageTrail>();
            }
            s_trailProj.enabled = false;

            // TODO: Secondary Projectile; Icicle Setup

            // TODO: Secondary projectile; Other effects
        }

        #region Fire
        private IEnumerator Co_s_f_projectile( ResourceRequest r1 , ResourceRequest r2 )
        {
            while( !r1.isDone || !r2.isDone )
            {
                yield return new WaitForSeconds(requestCheckDelay);
            }
            s_f_projectile = (GameObject)r1.asset;
            s_f_charge = (GameObject)r2.asset;
            Debug.Log("Step1");
            ParticleSystem ps1 = s_f_charge.GetComponentInChildren<ParticleSystem>();
            ParticleSystem ps2 = s_f_projectile.GetComponentInChildren<ParticleSystem>();
            Debug.Log("Step2");
            ParticleSystemRenderer psr1 = s_f_charge.GetComponentInChildren<ParticleSystemRenderer>();
            ParticleSystemRenderer psr2 = s_f_projectile.GetComponentInChildren<ParticleSystemRenderer>();
            Debug.Log("Step3");
            var ps2Main = ps2.main;
            ps2Main.prewarm = true;
            ps2Main.loop = true;
            var ps2SizeL = ps2.sizeOverLifetime;
            ps2SizeL.enabled = false;

            psr2.material = psr1.material;

            Material[] mat = new Material[] { s_f_charge.GetComponentInChildren<MeshRenderer>().material };
            s_f_projectile.GetComponentInChildren<MeshRenderer>().materials = mat;

        }
        private IEnumerator Co_s_f_muzzle(ResourceRequest r1)
        {
            while (!r1.isDone)
            {
                yield return new WaitForSeconds(requestCheckDelay);
            }
            s_f_muzzle = (GameObject)r1.asset;
        }

        private IEnumerator Co_s_f_child( ResourceRequest r1 , ResourceRequest r2 )
        {
            while( !r1.isDone || !r2.isDone )
            {
                yield return new WaitForSeconds(requestCheckDelay);
            }
            s_f_child = (GameObject)r1.asset;
            s_f_childGhost = (GameObject)r2.asset;

            s_f_child.GetComponent<ProjectileController>().ghostPrefab = s_f_childGhost;

            s_f_child.GetComponent<Rigidbody>().useGravity = true;

            var s_f_childSimple = s_f_child.GetComponent<ProjectileSimple>();
            s_f_childSimple.lifetime = 5f;
            s_f_childSimple.velocity = 25.0f;
            s_f_childSimple.updateAfterFiring = false;

            s_f_child.GetComponent<SphereCollider>().radius = 0.5f;

            s_f_child.GetComponent<ProjectileOverlapAttack>().enabled = false;
            s_f_child.GetComponent<HitBoxGroup>().enabled = false;

            var s_f_childExplode = s_f_child.GetComponent<ProjectileImpactExplosion>();
            if( !s_f_childExplode )
            {
                s_f_childExplode = s_f_child.AddComponent<ProjectileImpactExplosion>();
            }
            // TODO: Secondary Fire Child; load impact effect
            //s_f_childExplode.impactEffect
            // TODO: Secondary Fire Child; sound
            //s_f_childExplode.explosionSoundString = "";
            s_f_childExplode.destroyOnEnemy = true;
            s_f_childExplode.destroyOnWorld = true;
            s_f_childExplode.timerAfterImpact = false;
            s_f_childExplode.falloffModel = BlastAttack.FalloffModel.Linear;
            s_f_childExplode.lifetime = 5f;
            s_f_childExplode.lifetimeAfterImpact = 0f;
            s_f_childExplode.lifetimeRandomOffset = 0f;
            s_f_childExplode.blastRadius = 7.0f;
            s_f_childExplode.blastDamageCoefficient = 1.0f;
            s_f_childExplode.blastProcCoefficient = 1.0f;
            s_f_childExplode.bonusBlastForce = Vector3.zero;

            // TODO: Secondary Fire Child; Second child for ground burn?

        }
        #endregion
        #region Ice
        private IEnumerator Co_s_i_projectile(ResourceRequest r1)
        {
            while (!r1.isDone)
            {
                yield return new WaitForSeconds(requestCheckDelay);
            }
            s_i_projectile = (GameObject)r1.asset;
        }
        private IEnumerator Co_s_i_muzzle(ResourceRequest r1)
        {
            while (!r1.isDone)
            {
                yield return new WaitForSeconds(requestCheckDelay);
            }
            s_i_muzzle = (GameObject)r1.asset;
        }
        private IEnumerator Co_s_i_charge(ResourceRequest r1)
        {
            while (!r1.isDone)
            {
                yield return new WaitForSeconds(requestCheckDelay);
            }
            s_i_charge = (GameObject)r1.asset;
        }
        #endregion
        #region Lightning
        private IEnumerator Co_s_l_projectile(ResourceRequest r1)
        {
            while (!r1.isDone)
            {
                yield return new WaitForSeconds(requestCheckDelay);
            }
            s_l_projectile = (GameObject)r1.asset;
        }
        private IEnumerator Co_s_l_muzzle(ResourceRequest r1)
        {
            while (!r1.isDone)
            {
                yield return new WaitForSeconds(requestCheckDelay);
            }
            s_l_muzzle = (GameObject)r1.asset;
        }
        private IEnumerator Co_s_l_charge(ResourceRequest r1)
        {
            while (!r1.isDone)
            {
                yield return new WaitForSeconds(requestCheckDelay);
            }
            s_l_charge = (GameObject)r1.asset;
        }
        #endregion
        #endregion
        #region Utility
        private IEnumerator Co_u_seedProjectile( ResourceRequest r1 , ResourceRequest r2 )
        {
            while( !r1.isDone || !r2.isDone )
            {
                yield return new WaitForSeconds(requestCheckDelay);
            }
            u_seedProjectile = (GameObject)r1.asset;
            u_walkerProjectile = (GameObject)r2.asset;

            ProjectileMageFirewallController pmfc = u_seedProjectile.GetComponent<ProjectileMageFirewallController>();
            pmfc.walkerPrefab = u_walkerProjectile;

            u_walkerController = u_walkerProjectile.GetComponent<ProjectileMageFirewallWalkerController>();

        }
        #region Fire
        IEnumerator Co_u_f_pillar( ResourceRequest r1 )
        {
            while( !r1.isDone )
            {
                yield return new WaitForSeconds(requestCheckDelay);
            }
            u_f_pillar = (GameObject)r1.asset;
        }
        IEnumerator Co_u_f_muzzle(ResourceRequest r1)
        {
            while (!r1.isDone)
            {
                yield return new WaitForSeconds(requestCheckDelay);
            }
            u_f_muzzle = (GameObject)r1.asset;
        }
        #endregion
        #region Ice
        IEnumerator Co_u_i_pillar(ResourceRequest r1)
        {
            while (!r1.isDone)
            {
                yield return new WaitForSeconds(requestCheckDelay);
            }
            u_i_pillar = (GameObject)r1.asset;
        }
        IEnumerator Co_u_i_muzzle(ResourceRequest r1)
        {
            while (!r1.isDone)
            {
                yield return new WaitForSeconds(requestCheckDelay);
            }
            u_i_muzzle = (GameObject)r1.asset;
        }
        #endregion
        #region Lightning
        IEnumerator Co_u_l_pillar(ResourceRequest r1 , ResourceRequest r2 , ResourceRequest r3 , ResourceRequest r4)
        {
            while (!r1.isDone || !r2.isDone || !r3.isDone || !r4.isDone )
            {
                yield return new WaitForSeconds(requestCheckDelay);
            }
            u_l_pillar = (GameObject)r1.asset;
            u_l_pillarGhost = (GameObject)r2.asset;
            u_l_impactEffect = (GameObject)r3.asset;
            u_l_childProj = (GameObject)r4.asset;

            u_l_pillar.GetComponent<ProjectileParentTether>().enabled = false;
            var simpleProj = u_l_pillar.GetComponent<ProjectileSimple>();
            simpleProj.lifetime = 10f;
            simpleProj.velocity = 0f;

            u_l_pillar.GetComponent<ProjectileController>().ghostPrefab = u_l_pillarGhost;

            var explode = u_l_pillar.GetComponent<ProjectileImpactExplosion>();
            if( !explode )
            {
                explode = u_l_pillar.AddComponent<ProjectileImpactExplosion>();
            }
            explode.impactEffect = u_l_impactEffect;
            //explode.explosionSoundString = "";
            //explode.lifetimeExpiredSoundString = "";
            //explode.offsetForLifetimeExpiredSound = 0;
            explode.destroyOnEnemy = false;
            explode.destroyOnWorld = false;
            explode.timerAfterImpact = true;
            explode.falloffModel = BlastAttack.FalloffModel.None;
            explode.lifetime = 10.0f;
            explode.lifetimeAfterImpact = 8.0f;
            explode.lifetimeRandomOffset = 0.0f;
            explode.blastRadius = 4.0f;
            explode.blastDamageCoefficient = 1.0f;
            explode.blastProcCoefficient = 1.0f;
            explode.bonusBlastForce = Vector3.zero;
            explode.fireChildren = true;
            explode.childrenProjectilePrefab = u_l_childProj;
            explode.childrenCount = 4;
            explode.childrenDamageCoefficient = 0.25f;
            explode.minAngleOffset = new Vector3(-0.3f, -0.3f, -0.3f);
            explode.maxAngleOffset = new Vector3(0.3f, 0.3f, 0.3f);
            explode.transformSpace = ProjectileImpactExplosion.TransformSpace.Local;

            var stick = u_l_pillar.GetComponent<ProjectileStickOnImpact>();
            stick.ignoreWorld = true;

            var rb = u_l_pillar.GetComponent<Rigidbody>();
            rb.useGravity = false;
        }
        IEnumerator Co_u_l_muzzle(ResourceRequest r1)
        {
            while (!r1.isDone)
            {
                yield return new WaitForSeconds(requestCheckDelay);
            }
            u_l_muzzle = (GameObject)r1.asset;
        }
        #endregion
        #endregion
        #region Special

        #region Fire
        #endregion
        #region Ice
        #endregion
        #region Lightning
        #endregion
        #endregion

        #endregion
        #region Helper functions
        //=========================================================================
        //Helper funcs
        //=========================================================================
        private void CopyFields<T>(T dest , T source )
        {
            foreach( FieldInfo f in typeof(T).GetFields() )
            {
                f.SetValue(dest, f.GetValue(source));
            }
        }

        private void S_f_setupChildPS1( ParticleSystem ps , ParticleSystemRenderer psr , GameObject refPS )
        {
            ParticleSystem refPartSys = refPS.GetComponent<ParticleSystem>();
            ParticleSystemRenderer refPartSysR = refPS.GetComponent<ParticleSystemRenderer>();

            CopyFields<ParticleSystem.MainModule>(ps.main, refPartSys.main);
            CopyFields<ParticleSystem.EmissionModule>(ps.emission, refPartSys.emission);
            CopyFields<ParticleSystem.ColorOverLifetimeModule>(ps.colorOverLifetime, refPartSys.colorOverLifetime);
            CopyFields<ParticleSystem.SizeOverLifetimeModule>(ps.sizeOverLifetime, refPartSys.sizeOverLifetime);
            CopyFields<ParticleSystem.RotationOverLifetimeModule>(ps.rotationOverLifetime, refPartSys.rotationOverLifetime);

            CopyFields<ParticleSystemRenderer>(psr, refPartSysR);
        }

        private void S_f_setupChildPS2( ParticleSystem ps , ParticleSystemRenderer psr , GameObject refPS )
        {
            ParticleSystem refPartSys = refPS.GetComponent<ParticleSystem>();
            ParticleSystemRenderer refPartSysR = refPS.GetComponent<ParticleSystemRenderer>();

            CopyFields<ParticleSystem.MainModule>(ps.main, refPartSys.main);
            CopyFields<ParticleSystem.EmissionModule>(ps.emission, refPartSys.emission);
            CopyFields<ParticleSystem.ColorOverLifetimeModule>(ps.colorOverLifetime, refPartSys.colorOverLifetime);
            CopyFields<ParticleSystem.SizeOverLifetimeModule>(ps.sizeOverLifetime, refPartSys.sizeOverLifetime);
            CopyFields<ParticleSystem.RotationOverLifetimeModule>(ps.rotationOverLifetime, refPartSys.rotationOverLifetime);

            CopyFields<ParticleSystemRenderer>(psr, refPartSysR);
        }



        struct Line
        {
            int indents;
            string text;

            public Line(int indents, string text)
            {
                this.indents = indents;
                this.text = text ?? throw new ArgumentNullException(nameof(text));
            }
        }
        
        private void DebugChildren( GameObject g )
        {
            if (g != null)
            {
                DebugComponents(g);
                Transform t = g.transform;
                if (t != null)
                {
                    foreach( Transform child in t )
                    {
                        if (child.parent == t )
                        {
                            DebugComponents(child.gameObject);
                            DebugChildren(child.gameObject);
                        }
                    }
                }
            }
        }

        private void DebugComponents(GameObject g)
        {
            if( g != null )
            {
                foreach( Component c in g.GetComponents<Component>() )
                {
                    if( c != null )
                    {
                        DebugFields(c);
                    }
                }
            }
        }

        private void DebugFields(object thing)
        {
            if (thing != null)
            {
                foreach (FieldInfo f in thing.GetType().GetFields())
                {
                    if (f != null)
                    {
                        string name = f.Name;

                        var type = f.FieldType;
                        var value = f.GetValue(thing);
                        if (type.IsValueType || type == typeof(string))
                        {
                            if (value != null)
                            {
                            }
                            else
                            {
                            }
                        }
                        else if (type == typeof(GameObject))
                        {
                            DebugChildren(value as GameObject);
                        }
                        else
                        {
                            DebugFields(value);
                        }
                    }
                    else
                    {
                    }
                }
            }
            else
            {
            }
        }
        #endregion
    }
}