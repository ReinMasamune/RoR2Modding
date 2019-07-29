using BepInEx;
using System;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Reflection;
using ReinSniperRework;
using RoR2.UI;
using System.Collections.Generic;
using RoR2.Orbs;
using RoR2.Projectile;

namespace ReinSniperRework
{
    public class ReinDataLibrary : MonoBehaviour
    {
        //General use values
        //floats
        //ints
        //strings
        //other
        public SniperReloadTracker g_reload;
        public SniperChargeTracker g_charge;

        //SniperPrimary Values
        //floats
        public float p_shotDamage = 2.5f;
        public float p_shotForce = 250.0f;
        public float p_shotCoef = 1.0f;
        public float p_recoilAmplitude = 12.0f;
        public float p_baseDuration = 0.01f;
        public float p_interruptInterval = 0.01f;
        public float p_recoilForceMult = 750.0f;
        public float p_reloadForceMult = 500.0f;
        public float p_shotRadius = 0.025f;
        public float p_maxRange = 1000.0f;
        public float p_reloadT0Mod = 0.75f;
        public float p_reloadT1Mod = 1.45f;
        public float p_reloadT2Mod = 2.0f;
        public float p_chargeT0Mod = 1.0f;
        public float p_chargeT1Mod = 1.05f;
        public float p_chargeT2Mod = 2.0f;
        public float p_chargeT0Scale = 0.0f;
        public float p_chargeT1Scale = 1.25f;
        public float p_chargeT2Scale = 5.0f;
        //ints
        //bools
        public bool p_shotSmartCollision = true;
        //strings
        public string p_attackSoundString = "Play_bandit_M2_shot";
        public string p_reloadSoundString = "Play_bandit_M2_load";
        public string p_muzzleName = "MuzzleShotgun";
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
        public float s_startZoom = 2.0f;
        public float s_minZoom = 2.0f;
        public float s_maxZoom = 32.0f;
        public float s_baseFOV = 60.0f;
        public float s_scrollScale = 1.0f;
        //ints
        //bools
        //strings
        //other
        public GameObject s_crosshairPrefab;
        //load requests
        private ResourceRequest req_s_crosshairPrefab = Resources.LoadAsync<GameObject>("prefabs/crosshair/snipercrosshair");

        //SniperUtility values
        //floats
        public float u_duration = 0.1f;
        public float u_speedCoef = 10.0f;
        //ints
        //bools
        //strings
        public string u_beginSoundString = "";
        public string u_endSoundString = "";
        //other
        public GameObject u_blinkPrefab;
        public Material u_blinkMat1;
        public Material u_blinkMat2;
        //load requests
        private ResourceRequest req_u_blinkPrefab = Resources.LoadAsync<GameObject>("prefabs/effects/impblinkeffect");
        private ResourceRequest req_u_blinkMat1 = Resources.LoadAsync<Material>("Materials/mattpinout");
        private ResourceRequest req_u_blinkMat2 = Resources.LoadAsync<Material>("Materials/mathuntressflashexpanded");

        //SniperSpecial values
        //floats
        public float r_baseDuration = 1.0f;
        private float i_minePrimeDelay = 5.0f;
        private float i_wardRadius = 8.0f;
        private float i_wardBuffDuration = 1.0f;
        private float i_wardInterval = 0.5f;
        private float i_wardDuration = 10.0f;
        //ints
        //bools
        private bool i_wardFloorWard = true;
        private bool i_wardExpires = true;
        private bool i_wardInvertTeam = true;
        //strings
        public string r_fireSound = "";
        public string r_muzzleName = "MuzzleCenter";
        //other
        public GameObject r_mineProj;
        private BuffIndex i_wardBuff = BuffIndex.Cripple;
        //load requests
        private ResourceRequest req_r_mineProj = Resources.LoadAsync<GameObject>("Prefabs/projectiles/engimine");
        private ResourceRequest req_i_mineWard = Resources.LoadAsync<GameObject>("prefabs/networkedobjects/engimineward");





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
            if (!r_mineProj && req_r_mineProj.isDone && req_i_mineWard.isDone)
            {
                r_mineProj = Get_r_mineProj(req_r_mineProj, req_i_mineWard);
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
        private Material Get_u_blinkMat1(ResourceRequest r)
        {
            return (Material)r.asset;
        }
        private Material Get_u_blinkMat2(ResourceRequest r)
        {
            return (Material)r.asset;
        }
        private GameObject Get_r_mineProj(ResourceRequest r1, ResourceRequest r2)
        {
            GameObject mine = (GameObject)r1.asset;
            GameObject mineWard = (GameObject)r2.asset;

            BuffWard ward = mineWard.GetComponent<BuffWard>();

            ward.radius = i_wardRadius;
            ward.interval = i_wardInterval;
            //ward.RangeIndicator = ;
            ward.buffType = BuffIndex.Cripple;
            ward.buffDuration = i_wardBuffDuration;
            ward.floorWard = i_wardFloorWard;
            ward.expires = i_wardExpires;
            ward.invertTeamFilter = i_wardInvertTeam;
            ward.expireDuration = i_wardDuration;
            //ward.animateRadius = ;
            //ward.radiusCoefficientCurve = ;

            //EngiMineController control = mine.GetComponent<EngiMineController>();
            HookMineController hookControl = mine.AddComponent<HookMineController>();

            hookControl.wardPrefab = mineWard;
            hookControl.primingDelay = i_minePrimeDelay;

            return mine;
        }
    }
}