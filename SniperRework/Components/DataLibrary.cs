using RoR2;
using UnityEngine;
using RoR2.UI;
using RoR2.Projectile;

namespace ReinSniperRework
{
    public class ReinDataLibrary : MonoBehaviour
    {
                        //General use values
        //floats
        public readonly float g_baseDamage = 16.0f;
        public readonly float g_baseHealth = 100.0f;
        public readonly float g_baseRegen = 0.6f;
        public float g_shotCharge;
        //ints
        public int g_reloadTier = 2;
        public int g_chargeTier = 0;
        //bools
        public bool g_zoomed = false;
        //strings
        public readonly string g_crosshairString = "prefabs/crosshair/banditcrosshairrevolver";
        //other
        public SniperUIController g_ui;

        
                        //UI Controller values
        //floats
        //ints
        public readonly int ui_bar1Width = 200;
        public readonly int ui_bar1Height = 12;
        public readonly int ui_bar1HOffset = 0;
        public readonly int ui_bar1VOffset = -100;
        public readonly int ui_bar1Slider1Width = 3;
        public readonly int ui_bar1Slider1Height = 18;
        public readonly int ui_bar2Width = 200;
        public readonly int ui_bar2Height = 12;
        public readonly int ui_bar2HOffset = 0;
        public readonly int ui_bar2VOffset = 103;
        public readonly int ui_bar2Slider1Width = 3;
        public readonly int ui_bar2Slider1Height = 18;
        //strings
        //other
        public readonly Color ui_bar1BorderColor = new Color(0.0f, 0.0f, 0.0f, 1.0f);
        public readonly Color ui_bar1BaseColor = new Color(0.0f, 0.0f, 0.0f, 0.5f);
        public readonly Color ui_bar1FillColor1 = new Color(0.5f, 0.5f, 0.5f, 0.75f);
        public readonly Color ui_bar1FillColor2 = new Color(0.75f, 0.75f, 0.75f, 0.75f);
        public readonly Color ui_bar1SliderColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        public readonly Color ui_bar2BorderColor = new Color(0.0f, 0.0f, 0.0f, 1.0f);
        public readonly Color ui_bar2BaseColor = new Color(0.0f, 0.0f, 0.0f, 0.5f);
        public readonly Color ui_bar2FillColor1 = new Color(0.25f, 0.75f, 0.75f, 0.75f);
        public readonly Color ui_bar2FillColor2 = new Color(0.75f, 0.25f, 0.25f, 0.75f);
        public readonly Color ui_bar2SliderColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);


                        //SniperPrimary Values
        //floats
        public readonly float p_rechargeInterval = 0.001f;
        public readonly float p_shootDelay = 0f;
        public readonly float p_shotDamage = 2.5f;
        public readonly float p_shotForce = 500.0f;
        public readonly float p_shotCoef = 1.0f;
        public readonly float p_recoilAmplitude = 10.0f;
        public readonly float p_baseDuration = 0.01f;
        public readonly float p_interruptInterval = 0.01f;
        public readonly float p_recoilForceMult = 500.0f;
        public readonly float p_reloadForceMult = 500.0f;
        public readonly float p_ntShotRadius = 0.025f;
        public readonly float p_t0ShotRadius = 0.5f;
        public readonly float p_t1ShotRadius = 0.75f;
        public readonly float p_t2ShotRadius = 1.5f;
        public readonly float p_maxRange = 1500.0f;
        public readonly float p_reloadT0Mod = 0.75f;
        public readonly float p_reloadT1Mod = 1.45f;
        public readonly float p_reloadT2Mod = 2.0f;
        public readonly float p_chargeT0Mod = 1.0f;
        public readonly float p_chargeT1Mod = 1.05f;
        public readonly float p_chargeT2Mod = 2.0f;
        public readonly float p_chargeT0Scale = 0.0f;
        public readonly float p_chargeT1Scale = 1.25f;
        public readonly float p_chargeT2Scale = 5.0f;
        public readonly float p_chargeT1Coef = 1.0f;
        public readonly float p_chargeT2Coef = 2.0f;
        public readonly float p_reloadStartDelay = 0.5f;
        public readonly float p_loadTime = 2.0f;
        public readonly float p_attackSpeedSoft = 2.5f;
        public readonly float p_softLoadStart = 0.39f;
        public readonly float p_softLoadEnd = 0.6f;
        public readonly float p_sweetLoadStart = 0.25f;
        public readonly float p_sweetLoadEnd = 0.4f;
        //ints
        public readonly int p_baseMaxStock = 1;
        public readonly int p_rechargeStock = 1;
        public readonly int p_requiredStock = 1;
        public readonly int p_stockToConsome = 1;
        //bools
        public readonly bool p_isBullets = false;
        public readonly bool p_beginCDOnEnd = false;
        public readonly bool p_isCombatSkill = true;
        public readonly bool p_noSprint = true;
        public readonly bool p_mustKeyPress = true;
        public readonly bool p_shotSmartCollision = true;
        //strings
        public readonly string p_attackSoundString = "Play_bandit_M2_shot";
        public readonly string p_muzzleName = "MuzzleShotgun";
        public readonly string p_baseLoadSound = "Play_bandit_M2_load";
        public readonly string p_softLoadSound = "";
        public readonly string p_sweetLoadSound = "Play_item_proc_crit_cooldown";
        //other
        public GameObject p_effectPrefab;
        public GameObject p_tracerEffectPrefab;
        public GameObject p_hitEffectPrefab;
        //load requests
        private ResourceRequest req_p_effectPrefab = Resources.LoadAsync<GameObject>("prefabs/effects/muzzleflashes/muzzleflashmagelightning");
        private ResourceRequest req_p_tracerEffectPrefab = Resources.LoadAsync<GameObject>("prefabs/effects/tracers/tracerclaybruiserminigun");
        private ResourceRequest req_p_hitEffectPrefab = Resources.LoadAsync<GameObject>("prefabs/effects/impacteffects/impactspear");


                        //SniperSecondary Values
        //floats
        public readonly float s_rechargeInterval = 30.0f;
        public readonly float s_shootDelay = 0.1f;
        public readonly float s_startZoom = 2.0f;
        public readonly float s_minZoom = 2.0f;
        public readonly float s_maxZoom = 32.0f;
        public readonly float s_baseFOV = 60.0f;
        public readonly float s_scrollScale = 1.5f;
        public readonly float s_chargeTime = 10.0f;
        public readonly float s_boost1Start = 0.25f;
        public readonly float s_boost2Start = 0.75f;
        //ints
        public readonly int s_baseMaxStock = 1;
        public readonly int s_rechargeStock = 1;
        public readonly int s_requiredStock = 0;
        public readonly int s_stockToConsume = 0;
        //bools
        public readonly bool s_isBullets = false;
        public readonly bool s_beginCDOnEnd = false;
        public readonly bool s_isCombatSkill = false;
        public readonly bool s_noSprint = true;
        public readonly bool s_mustKeyPress = false;
        //strings
        //other
        public GameObject s_crosshairPrefab;
        //load requests
        private ResourceRequest req_s_crosshairPrefab = Resources.LoadAsync<GameObject>("prefabs/crosshair/snipercrosshair");


                        //SniperUtility values
        //floats
        public readonly float u_rechargeInterval = 8.0f;
        public readonly float u_shootDelay = 0.25f;
        public readonly float u_duration = 0.15f;
        public readonly float u_speedCoef = 12.5f;
        public readonly float u_smallHopStrength = 50.0f;
        public readonly float u_procCoef = 1.0f;
        public readonly float u_orbRange = 1000.0f;
        public readonly float u_orbPrefireDuration = 0.2f;
        public readonly float u_orbFreq = 1000.0f;
        public readonly float u_initSpeedCoef = 5.0f;
        public readonly float u_endSpeedCoef = 2.5f;
        public readonly float u_damageMult = 1.0f;
        //ints
        public readonly int u_baseMaxStock = 1;
        public readonly int u_rechargeStock = 1;
        public readonly int u_requiredStock = 1;
        public readonly int u_stockToConsume = 1;
        public readonly int u_orbMax = 1000;
        //bools
        public readonly bool u_isBullets = false;
        public readonly bool u_beginCDOnEnd = true;
        public readonly bool u_isCombatSkill = false;
        public readonly bool u_noSprint = false;
        public readonly bool u_mustKeyPress = true;
        //strings
        public readonly string u_beginSoundString = "";
        public readonly string u_endSoundString = "";
        public readonly string u_muzzle = "";
        public readonly string u_dodgeSound = "";
        //other
        public GameObject u_blinkPrefab;
        public GameObject u_muzzleFlashPrefab;
        public Material u_blinkMat1;
        public Material u_blinkMat2;
        //load requests
        private ResourceRequest req_u_blinkPrefab = Resources.LoadAsync<GameObject>("prefabs/effects/impblinkeffect");
        private ResourceRequest req_u_muzzleFlashPrefab = Resources.LoadAsync<GameObject>("prefabs/effects/muzzleflashes/muzzleflashmagelightning");
        private ResourceRequest req_u_blinkMat1 = Resources.LoadAsync<Material>("Materials/mattpinout");
        private ResourceRequest req_u_blinkMat2 = Resources.LoadAsync<Material>("Materials/mathuntressflashexpanded");


                        //SniperSpecial values
        //floats
        public readonly float r_rechargeInterval = 30.0f;
        public readonly float r_shootDelay = 0.1f;
        public readonly float r_baseDuration = 0.25f;
        private float i_mineThrowVelocity = 15f;
        private float i_minePrimeDelay = 2.5f;
        private float i_wardRadius = 7.5f;
        private float i_wardBuffDuration = 1.0f;
        private float i_wardInterval = 1.0f;
        private float i_wardDuration = 10.0f;
        private float i_triggerRadiusMod = 1.0f;
        private float i_mineHookInterval = 1.25f;
        private float i_mineHookDuration = 10.0f;
        private float i_mineHookRadiusMod = 3.5f;
        private float i_mineForceRadiusMod = 1.24f;
        private float i_mineForceDamping = 0.05f;
        private float i_mineForceStrength = 2.0f;
        private float i_mineForceFalloff = 0.0f;
        //ints
        public readonly int r_baseMaxStock = 1;
        public readonly int r_rechargeStock = 1;
        public readonly int r_requiredStock = 1;
        public readonly int r_stocktoConsume = 1;
        private int i_mineHooksPerTick = 4;
        //bools
        public readonly bool r_isBullets = false;
        public readonly bool r_beginCDOnEnd = false;
        public readonly bool r_isCombatSkill = false;
        public readonly bool r_noSprint = true;
        public readonly bool r_mustKeyPress = true;
        private bool i_wardFloorWard = true;
        private bool i_wardExpires = true;
        private bool i_wardInvertTeam = true;
        //strings
        public readonly string r_fireSound = "";
        public readonly string r_muzzleName = "MuzzleCenter";
        //other
        public GameObject r_mineProj;
        private BuffIndex i_wardBuff = BuffIndex.Cripple;
        //load requests
        private ResourceRequest req_r_mineProj = Resources.LoadAsync<GameObject>("Prefabs/projectiles/engimine");
        private ResourceRequest req_i_mineWard = Resources.LoadAsync<GameObject>("prefabs/networkedobjects/engimineward");
        private ResourceRequest req_i_mineForceTetherPrefab = Resources.LoadAsync<GameObject>("prefabs/effects/gravspheretether");

        public void FixedUpdate()
        {
            if (!p_effectPrefab && req_p_effectPrefab.isDone)
            {
                p_effectPrefab = Get_p_effectPrefab(req_p_effectPrefab);
            }
            if (!p_tracerEffectPrefab && req_p_tracerEffectPrefab.isDone)
            {
                p_tracerEffectPrefab = Get_p_tracereffectPrefab(req_p_tracerEffectPrefab);
            }
            if (!p_hitEffectPrefab && req_p_hitEffectPrefab.isDone)
            {
                p_hitEffectPrefab = Get_p_hitEffectPrefab(req_p_hitEffectPrefab);
            }
            if (!s_crosshairPrefab && req_s_crosshairPrefab.isDone)
            {
                s_crosshairPrefab = Get_s_crosshairPrefab(req_s_crosshairPrefab);
            }
            if (!u_blinkPrefab && req_u_blinkPrefab.isDone)
            {
                u_blinkPrefab = Get_u_blinkPrefab(req_u_blinkPrefab);
            }
            if( !u_muzzleFlashPrefab && req_u_muzzleFlashPrefab.isDone )
            {
                u_muzzleFlashPrefab = Get_u_muzzleFlashPrefab(req_u_muzzleFlashPrefab);
            }
            if (!r_mineProj && req_r_mineProj.isDone && req_i_mineWard.isDone && req_i_mineForceTetherPrefab.isDone )
            {
                r_mineProj = Get_r_mineProj(req_r_mineProj, req_i_mineWard, req_i_mineForceTetherPrefab);
            }
        }


        private GameObject Get_p_effectPrefab(ResourceRequest r)
        {
            return (GameObject)r.asset;
        }
        private GameObject Get_p_tracereffectPrefab(ResourceRequest r)
        {
            return (GameObject)r.asset;
        }
        private GameObject Get_p_hitEffectPrefab(ResourceRequest r)
        {
            return (GameObject)r.asset;
        }
        private GameObject Get_s_crosshairPrefab(ResourceRequest r)
        {
            GameObject go = (GameObject)r.asset;
            go.GetComponent<DisplayStock>().skillSlot = SkillSlot.Secondary;
            return go;
        }
        private GameObject Get_u_blinkPrefab(ResourceRequest r)
        {
            return (GameObject)r.asset;
        }
        private GameObject Get_u_muzzleFlashPrefab(ResourceRequest r )
        {
            return (GameObject)r.asset;
        }
        private Material Get_u_blinkMat1(ResourceRequest r)
        {
            return (Material)r.asset;
        }
        private Material Get_u_blinkMat2(ResourceRequest r)
        {
            return (Material)r.asset;
        }
        private GameObject Get_r_mineProj(ResourceRequest r1, ResourceRequest r2, ResourceRequest r3 )
        {
            GameObject mine = (GameObject)r1.asset;
            GameObject mineWard = (GameObject)r2.asset;
            GameObject tetherPrefab = (GameObject)r3.asset;

            BuffWard ward = mineWard.GetComponent<BuffWard>();

            ward.radius = i_wardRadius;
            ward.interval = i_wardInterval;
            ward.buffType = i_wardBuff;
            ward.buffDuration = i_wardBuffDuration;
            ward.floorWard = i_wardFloorWard;
            ward.expires = i_wardExpires;
            ward.invertTeamFilter = i_wardInvertTeam;
            ward.expireDuration = i_wardDuration;

            RadialForce force = mineWard.GetComponent<RadialForce>();
            if( !force )
            {
                force = mineWard.AddComponent<RadialForce>();
            }
            

            force.tetherPrefab = tetherPrefab;
            force.radius = i_wardRadius * i_mineForceRadiusMod;
            force.damping = i_mineForceDamping;
            force.forceMagnitude = i_mineForceStrength;
            force.forceCoefficientAtEdge = i_mineForceFalloff;

            Collider col = mine.AddComponent<SphereCollider>();
            ((SphereCollider)col).radius = i_triggerRadiusMod * i_wardRadius;
            col.isTrigger = true;

            HookMineController hookControl = mine.GetComponent<HookMineController>();
            if( !hookControl )
            {
                hookControl = mine.AddComponent<HookMineController>();
            }

            hookControl.wardPrefab = mineWard;
            hookControl.primingDelay = i_minePrimeDelay;
            hookControl.hookDuration = i_mineHookDuration;
            hookControl.hookInterval = i_mineHookInterval;
            hookControl.hooksPerTick = i_mineHooksPerTick;
            hookControl.hookRadius = i_mineHookRadiusMod * i_wardRadius;
            hookControl.teamHostile = TeamIndex.Monster;
            hookControl.teamFriendly = TeamIndex.Player;

            mine.GetComponent<ProjectileSimple>().velocity = i_mineThrowVelocity;

            Destroy(mine.GetComponent<ProjectileController>().ghostPrefab.GetComponent<EngiMineAnimator>());
            Destroy(mine.GetComponent<EngiMineController>());

            return mine;
        }
    }
}