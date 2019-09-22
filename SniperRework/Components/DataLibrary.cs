using RoR2;
using UnityEngine;
using RoR2.UI;
using RoR2.Projectile;
using R2API.Utils;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

namespace ReinSniperRework
{
    public class ReinDataLibrary : MonoBehaviour
    {
        public AssetBundle bundle;
        //General use values
        //floats
        public readonly float g_baseDamage = 16.0f;
        public readonly float g_baseHealth = 110.0f;
        public readonly float g_baseRegen = 1.0f;
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
        private Texture2D newTex;
        private Texture2D newTex2;

        //Enumerator config
        //floats
        private float en_checkTimer = 0.5f;
        private float en_readDelay = 0.2f;


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
        public readonly float p_shotForce = 750.0f;
        public readonly float p_shotCoef = 1.0f;
        public readonly float p_recoilAmplitude = 10.0f;
        public readonly float p_baseDuration = 0.01f;
        public readonly float p_interruptInterval = 0.01f;
        public readonly float p_recoilForceMult = 600.0f;
        public readonly float p_reloadForceMult = 600.0f;
        public readonly float p_ntShotRadius = 0.025f;
        public readonly float p_t0ShotRadius = 0.05f;
        public readonly float p_t1ShotRadius = 0.1f;
        public readonly float p_t2ShotRadius = 0.5f;
        public readonly float p_maxRange = 1500.0f;
        public readonly float p_reloadT0Mod = 0.75f;
        public readonly float p_reloadT1Mod = 1.45f;
        public readonly float p_reloadT2Mod = 2.0f;
        public readonly float p_chargeT0Mod = 0.0f;
        public readonly float p_chargeT1Mod = -1.0f;
        public readonly float p_chargeT2Mod = -1.0f;
        public readonly float p_chargeT0Scale = 0.0f;
        public readonly float p_chargeT1Scale = 5.25f;
        public readonly float p_chargeT2Scale = 10.0f;
        public readonly float p_chargeT1Coef = 1.05f;
        public readonly float p_chargeT2Coef = 1.25f;
        public readonly float p_reloadStartDelay = 0.5f;
        public readonly float p_loadTime = 2.0f;
        public readonly float p_attackSpeedSoft = 2.5f;
        public readonly float p_softLoadStart = 0.39f;
        public readonly float p_softLoadEnd = 0.6f;
        public readonly float p_sweetLoadStart = 0.25f;
        public readonly float p_sweetLoadEnd = 0.4f;
        public readonly float p_hitLIBase = 100.0f;
        public readonly float p_hitLRBase = 3.0f;
        public readonly float p_hitLIScale = 5.0f;
        public readonly float p_hitLRScale = 5.0f;
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
        public readonly string p_attackSound = "Play_MULT_m1_snipe_shoot";
        public readonly string p_muzzleName = "MuzzleShotgun";
        public readonly string p_baseLoadSound = "Play_bandit_M1_pump";
        public readonly string p_softLoadSound = "";
        public readonly string p_sweetLoadSound = "Play_item_proc_crit_cooldown";
        //other
        public GameObject p_hitEffectPrefab;
        public GameObject p_tracerEffectPrefab;
        public Tracer p_tracer;
        public ParticleSystemRenderer p_tracerPSR;
        public ParticleSystemRenderer p_subPSR;
        public ParticleSystem p_tracerPS;
        public ParticleSystem p_subPS;
        public Light p_tracerHitL;
        public List<Material> p_matsToEdit = new List<Material>();
        //load requests
        private ResourceRequest req_p_hitEffectPrefab;
        private ResourceRequest req_p_tracerEffectPrefab;
        private IEnumerator en_p_hitEffectPrefab;
        private IEnumerator en_p_tracerEffectPrefab;

        //SniperSecondary Values
        //floats
        public readonly float s_rechargeInterval = 30.0f;
        public readonly float s_shootDelay = 0.1f;
        public readonly float s_startZoom = 2.0f;
        public readonly float s_minZoom = 2.0f;
        public readonly float s_maxZoom = 32.0f;
        public readonly float s_baseFOV = 60.0f;
        public readonly float s_scrollScale = 1.5f;
        public readonly float s_chargeTime = 5.0f;
        public readonly float s_boost1Start = 0.33f;
        public readonly float s_boost2Start = 0.9f;
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
        private ResourceRequest req_s_crosshairPrefab;
        private IEnumerator en_s_crosshairPrefab;


        //SniperUtility values
        //floats
        public readonly float u_rechargeInterval = 8.0f;
        public readonly float u_shootDelay = 0.25f;
        public readonly float u_duration = 0.15f;
        public readonly float u_speedCoef = 12.5f;
        //ints
        public readonly int u_baseMaxStock = 1;
        public readonly int u_rechargeStock = 1;
        public readonly int u_requiredStock = 1;
        public readonly int u_stockToConsume = 1;
        //bools
        public readonly bool u_isBullets = false;
        public readonly bool u_beginCDOnEnd = true;
        public readonly bool u_isCombatSkill = false;
        public readonly bool u_noSprint = false;
        public readonly bool u_mustKeyPress = true;
        //strings
        public readonly string u_beginSoundString = "";
        public readonly string u_endSoundString = "";
        //other
        public GameObject u_blinkPrefab;
        public Material u_blinkMat1;
        public Material u_blinkMat2;
        //load requests
        private ResourceRequest req_u_blinkPrefab;
        private ResourceRequest req_u_blinkMat1;
        private ResourceRequest req_u_blinkMat2;
        private IEnumerator en_u_blinkPrefab;
        private IEnumerator en_u_blinkMat1;
        private IEnumerator en_u_blinkMat2;


        //SniperSpecial values
        //floats
        public readonly float r_rechargeInterval = 20.0f;
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
        private int i_mineHookTicks = 10;
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
        private ResourceRequest req_r_mineProj;
        private ResourceRequest req_i_mineWard;
        private ResourceRequest req_i_mineForceTetherPrefab;
        private IEnumerator en_r_mineProj;

        public void Awake()
        {
            newTex = CreateRampTexture(new Vector3(1f, 1f, 1f), 0.5f);
            newTex2 = new Texture2D(1, 1);
            newTex2.SetPixel(0, 0, new Color(1f, 1f, 1f, 1f));
            newTex2.Apply();
            //newTex = bundle.LoadAsset<Texture2D>("Assets/Texture2D/texRampDefault.png");
            p_matsToEdit.Clear();

            req_p_hitEffectPrefab = Resources.LoadAsync<GameObject>("prefabs/effects/impacteffects/LightningStrikeImpact");//Need to adjust this
            req_p_tracerEffectPrefab = Resources.LoadAsync<GameObject>("prefabs/effects/tracers/tracergolem");
            req_s_crosshairPrefab = Resources.LoadAsync<GameObject>("prefabs/crosshair/snipercrosshair");
            req_u_blinkPrefab = Resources.LoadAsync<GameObject>("prefabs/effects/impblinkeffect");
            req_u_blinkMat1 = Resources.LoadAsync<Material>("Materials/mattpinout");
            req_u_blinkMat2 = Resources.LoadAsync<Material>("Materials/mathuntressflashexpanded");
            req_r_mineProj = Resources.LoadAsync<GameObject>("Prefabs/projectiles/engimine");//Need to adjust this
            req_i_mineWard = Resources.LoadAsync<GameObject>("prefabs/networkedobjects/engimineward");//Need to adjust this
            req_i_mineForceTetherPrefab = Resources.LoadAsync<GameObject>("prefabs/effects/gravspheretether");//Maybe adjust this?

            en_p_hitEffectPrefab = co_p_hitEffectPrefab();
            StartCoroutine(en_p_hitEffectPrefab);

            en_p_tracerEffectPrefab = co_p_tracerEffectPrefab();
            StartCoroutine(en_p_tracerEffectPrefab);

            en_s_crosshairPrefab = co_s_crosshairPrefab();
            StartCoroutine(en_s_crosshairPrefab);

            en_u_blinkPrefab = co_u_blinkPrefab();
            StartCoroutine(en_u_blinkPrefab);

            en_u_blinkMat1 = co_u_blinkMat1();
            StartCoroutine(en_u_blinkMat1);

            en_u_blinkMat2 = co_u_blinkMat2();
            StartCoroutine(en_u_blinkMat2);

            en_r_mineProj = co_r_mineProj();
            StartCoroutine(en_r_mineProj);
        }

        private IEnumerator co_p_hitEffectPrefab()
        {
            if (!p_hitEffectPrefab)
            {
                while (!req_p_hitEffectPrefab.isDone)
                {
                    yield return new WaitForSeconds(en_checkTimer);
                }
                yield return new WaitForSeconds(en_readDelay);

                GameObject g = (GameObject)req_p_hitEffectPrefab.asset;
                //g = Instantiate(g);
                //DontDestroyOnLoad(g);
                //g.SetActive(false);

                foreach (ParticleSystemRenderer psr in g.GetComponentsInChildren<ParticleSystemRenderer>())
                {
                    //Debug.Log(psr.gameObject.name);
                    //Debug.Log("Material");
                    //DebugMaterialInfo(psr.material);
                    //Debug.Log("Trail Material");
                    //DebugMaterialInfo(psr.material);

                    switch (psr.gameObject.name)
                    {
                        case "LightningRibbon":
                            Destroy(psr);
                            break;
                        case "Ring":
                            psr.material.SetTexture("_RemapTex", newTex);
                            p_matsToEdit.Add(psr.material);
                            break;
                        case "Flash":
                            Destroy(psr);
                            break;
                        case "Distortion":
                            break;
                        case "Flash Lines":
                            //psr.material.SetTexture("_RemapTex", newTex);
                            //p_matsToEdit.Add(psr.material);
                            Destroy(psr);
                            break;
                        case "Sphere":
                            psr.material.SetTexture("_RemapTex", newTex);
                            p_matsToEdit.Add(psr.material);
                            break;
                        default:
                            Debug.Log(psr.gameObject.name + " not registered");
                            break;
                    }
                }

                foreach (ParticleSystem ps in g.GetComponentsInChildren<ParticleSystem>())
                {
                    switch (ps.gameObject.name)
                    {
                        case "LightningRibbon":
                            Destroy(ps);
                            break;
                        case "Ring":
                            var ringSize = ps.sizeOverLifetime;
                            ringSize.sizeMultiplier = 1f;
                            break;
                        case "Flash":
                            Destroy(ps);
                            break;
                        case "Distortion":
                            var distSize = ps.sizeOverLifetime;
                            distSize.sizeMultiplier = 1.25f;
                            break;
                        case "Flash Lines":
                            Destroy(ps);
                            break;
                        case "Sphere":
                            var size = ps.sizeOverLifetime;
                            size.sizeMultiplier = 0.75f;
                            break;
                        default:
                            Debug.Log(ps.gameObject.name + " not registered");
                            break;
                    }
                }


                p_tracerHitL = g.GetComponent<Light>();
                if (!p_tracerHitL)
                {
                    p_tracerHitL = g.AddComponent<Light>();
                }
                p_tracerHitL.type = LightType.Point;
                FadeLight fl = g.GetComponent<FadeLight>();
                if (!fl)
                {
                    fl = g.AddComponent<FadeLight>();
                }
                fl.light = p_tracerHitL;
                fl.time = 1.0f;

                p_hitEffectPrefab = g;
            }
        }

        private IEnumerator co_p_tracerEffectPrefab()
        {
            if (!p_tracerEffectPrefab)
            {
                while (!req_p_tracerEffectPrefab.isDone)
                {
                    yield return new WaitForSeconds(en_checkTimer);
                }
                yield return new WaitForSeconds(en_readDelay);

                GameObject refGO = (GameObject)req_p_tracerEffectPrefab.asset;

                GameObject g = Instantiate(refGO);
                g.SetActive(false);
                DontDestroyOnLoad(g);

                p_tracer = g.GetComponent<Tracer>();

                p_tracerPS = g.GetComponentInChildren<ParticleSystem>();
                p_tracerPSR = g.GetComponentInChildren<ParticleSystemRenderer>();

                var main = p_tracerPS.main;
                var colL = p_tracerPS.colorOverLifetime;
                main.startSizeX = 3f;
                main.startSizeY = 0.25f;
                main.startSizeZ = 3f;
                main.simulationSpace = ParticleSystemSimulationSpace.World;

                colL.enabled = false;

                Material mat1 = Instantiate(refGO.GetComponentInChildren<ParticleSystemRenderer>().material);

                mat1.SetTexture("_RemapTex", newTex);

                mat1.SetTexture("_Cloud1Tex", bundle.LoadAsset<Texture2D>("Assets/Texture2D/texCloudWhiteNoise.png"));

                mat1.SetTexture("_Cloud2Tex", bundle.LoadAsset<Texture2D>("Assets/Texture2D/texCloudSkulls.png"));

                mat1.SetTexture("_MainTex", bundle.LoadAsset<Texture2D>("Assets/Texture2D/texAlphaGradient2Mask.png"));



                p_tracerPSR.material = mat1;

                p_matsToEdit.Add(mat1);

                p_tracerEffectPrefab = g;
            }
        }

        private IEnumerator co_s_crosshairPrefab()
        {
            if (!s_crosshairPrefab)
            {
                while (!req_s_crosshairPrefab.isDone)
                {
                    yield return new WaitForSeconds(en_checkTimer);
                }
                yield return new WaitForSeconds(en_readDelay);

                GameObject g = (GameObject)req_s_crosshairPrefab.asset;

                g.GetComponent<DisplayStock>().skillSlot = SkillSlot.Secondary;

                s_crosshairPrefab = g;
            }
        }

        private IEnumerator co_u_blinkPrefab()
        {
            if (!u_blinkPrefab)
            {
                while (!req_u_blinkPrefab.isDone)
                {
                    yield return new WaitForSeconds(en_checkTimer);
                }
                yield return new WaitForSeconds(en_readDelay);

                GameObject g = (GameObject)req_u_blinkPrefab.asset;

                u_blinkPrefab = g;
            }
        }

        private IEnumerator co_u_blinkMat1()
        {
            if (!u_blinkMat1)
            {
                while (!req_u_blinkMat1.isDone)
                {
                    yield return new WaitForSeconds(en_checkTimer);
                }
                yield return new WaitForSeconds(en_readDelay);

                Material m = (Material)req_u_blinkMat1.asset;

                u_blinkMat1 = m;
            }
        }

        private IEnumerator co_u_blinkMat2()
        {
            if (!u_blinkMat2)
            {
                while (!req_u_blinkMat2.isDone)
                {
                    yield return new WaitForSeconds(en_checkTimer);
                }
                yield return new WaitForSeconds(en_readDelay);

                Material m = (Material)req_u_blinkMat2.asset;

                u_blinkMat2 = m;
            }
        }

        private IEnumerator co_r_mineProj()
        {
            if (!r_mineProj)
            {
                while (!req_r_mineProj.isDone || !req_i_mineForceTetherPrefab.isDone || !req_i_mineWard.isDone)
                {
                    yield return new WaitForSeconds(en_checkTimer);
                }
                yield return new WaitForSeconds(en_readDelay);

                GameObject mine = (GameObject)req_r_mineProj.asset;
                GameObject mineWard = (GameObject)req_i_mineWard.asset;
                GameObject tetherPrefab = (GameObject)req_i_mineForceTetherPrefab.asset;

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
                if (!force)
                {
                    force = mineWard.AddComponent<RadialForce>();
                }

                force.tetherPrefab = tetherPrefab;
                force.radius = i_wardRadius * i_mineForceRadiusMod;
                force.damping = i_mineForceDamping;
                force.forceMagnitude = i_mineForceStrength;
                force.forceCoefficientAtEdge = i_mineForceFalloff;

                HookMineHooking hooks = mineWard.GetComponent<HookMineHooking>();
                if( !hooks )
                {
                    hooks = mineWard.AddComponent<HookMineHooking>();
                }

                hooks.hookDuration = i_mineHookDuration;
                hooks.hookInterval = i_mineHookInterval;
                hooks.hooksPerTick = i_mineHooksPerTick;
                hooks.hookRadius = i_mineHookRadiusMod * i_wardRadius;
                hooks.hookTicks = i_mineHookTicks;
                hooks.teamHostile = TeamIndex.Monster;
                hooks.teamFriendly = TeamIndex.Player;

                //Collider col = mine.AddComponent<SphereCollider>();
                //((SphereCollider)col).radius = i_triggerRadiusMod * i_wardRadius;
                //col.isTrigger = true;

                //HookMineController hookControl = mine.GetComponent<HookMineController>();
                //if (!hookControl)
                //{
                //    hookControl = mine.AddComponent<HookMineController>();
                //}

                HookMineInvader hookInv = mine.GetComponent<HookMineInvader>();
                if( !hookInv )
                {
                    hookInv = mine.AddComponent<HookMineInvader>();
                }

                //hookControl.wardPrefab = mineWard;
                //hookControl.primingDelay = i_minePrimeDelay;
                //hookControl.hookDuration = i_mineHookDuration;
                //hookControl.hookInterval = i_mineHookInterval;
                //hookControl.hooksPerTick = i_mineHooksPerTick;
                //hookControl.hookRadius = i_mineHookRadiusMod * i_wardRadius;
                //hookControl.teamHostile = TeamIndex.Monster;
                //hookControl.teamFriendly = TeamIndex.Player;

                hookInv.enabled = false;
                hookInv.wardPrefab = mineWard;
                //hookInv.SelfRef = mine;


                //mine.GetComponent<ProjectileSimple>().velocity = i_mineThrowVelocity;

                //Destroy(mine.GetComponent<ProjectileController>().ghostPrefab.GetComponent<EngiMineAnimator>());
                //Destroy(mine.GetComponent<EngiMineController>());

                r_mineProj = mine;
                //hookInv.SelfRef = r_mineProj;
            }
        }

        public Texture2D CreateRampTexture(Vector3 vec, float startGrad)
        {
            Texture2D tex = new Texture2D(256, 16, TextureFormat.RGBA32, false);

            int start = Mathf.CeilToInt(startGrad * 255);
            int gradLength = 255 - start;
            Color32 back = new Color32(0, 0, 0, 0);
            Color32 temp = new Color32(0, 0, 0, 0);
            for (int i = 0; i <= 255; i++)
            {
                if (i > start)
                {
                    float frac = ((float)i - (float)start) / (float)gradLength;
                    temp.r = (byte)Mathf.RoundToInt(255 * frac * vec.x);
                    temp.g = (byte)Mathf.RoundToInt(255 * frac * vec.y);
                    temp.b = (byte)Mathf.RoundToInt(255 * frac * vec.z);
                    temp.a = (byte)Mathf.RoundToInt(255 * frac);
                }
                else
                {
                    temp = back;
                }
                for (int y = 0; y < 16; y++)
                {
                    tex.SetPixel(i, y, temp);
                }
            }

            tex.wrapMode = TextureWrapMode.MirrorOnce;
            tex.Apply();

            return tex;
        }

        private void DebugMaterialInfo(Material m)
        {
            Debug.Log("Material name: " + m.name);
            string[] s = m.shaderKeywords;
            Debug.Log("Shader keywords");
            for (int i = 0; i < s.Length; i++)
            {
                Debug.Log(s[i]);
            }

            Debug.Log("Shader name: " + m.shader.name);

            Debug.Log("Texture Properties");
            string[] s2 = m.GetTexturePropertyNames();
            for (int i = 0; i < s2.Length; i++)
            {
                Debug.Log(s2[i] + " : " + m.GetTexture(s2[i]));
            }
        }
    }
}

