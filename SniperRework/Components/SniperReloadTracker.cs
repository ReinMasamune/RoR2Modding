using BepInEx;
using System;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Reflection;
using ReinSniperRework;
using RoR2.UI;

namespace ReinSniperRework
{
    public class SniperReloadTracker : MonoBehaviour
    {
        //Move all the config into the datalibrary
        private bool canShoot = true;
        private bool isReloading = false;
        private bool pastMinTime = false;
        private float reloadTimer;
        private float attackSpeed = 1.0f;
        private float scaledAttackSpeed = 1.0f;
        private float curMinReload;
        private float curMaxReload;
        private int reloadTier;

        private static int totalBarWidth = 200;
        private static int totalBarHeight = 12;
        private static int barHOffset = 0;
        private static int barVOffset = -100;

        private static int sliderWidth = 3;
        private static int sliderHeight = 18;


        public float baseMinReloadTime = 0.5f;
        public float baseMaxReloadTime = 2f;
        public float baseSoftSpotStart = 0.39f;
        public float baseSoftSpotEnd = 0.6f;
        public float baseSweetSpotStart = 0.25f;
        public float baseSweetSpotEnd = 0.40f;

        private static Color borderColor = new Color(0f, 0f, 0f, 1f);
        private static Color baseColor = new Color(0f, 0f, 0f, 0.5f);
        private static Color bar1Color = new Color(0.5f, 0.5f, 0.5f, 0.75f);
        private static Color bar2Color = new Color(0.75f, 0.75f, 0.75f, 0.75f);
        private static Color sliderColor = new Color(1f, 1f, 1f, 1f);

        Texture2D barTexture;
        Texture2D sliderTexture;
        Rect barPos;
        int sliderY;
        int sliderX1;
        int sliderX2;

        int width;
        int height;

        float frac;

        public void Awake()
        {
            width = Screen.width;
            height = Screen.height;
            GenerateBar();
        }

        public void FixedUpdate()
        {
            if (isReloading)
            {
                reloadTimer += Time.fixedDeltaTime;

                if (reloadTimer > curMinReload & !pastMinTime)
                {
                    reloadTimer -= curMinReload;
                    pastMinTime = true;
                }

                if (reloadTimer > curMaxReload)
                {
                    reloadTimer -= curMaxReload;
                }

                if (pastMinTime)
                {
                    frac = reloadTimer / curMaxReload;
                }
            }
        }

        public void OnGUI()
        {
            if (isReloading && pastMinTime)
            {
                GUI.Label(barPos, barTexture);
                int curSliderX = (int)((sliderX2 - sliderX1) * frac + sliderX1);
                GUI.Label(new Rect(curSliderX, sliderY, width, width), sliderTexture);
            }
        }

        public void Shot()
        {
            canShoot = false;
            reloadTimer = 0.0f;
            pastMinTime = false;
            isReloading = true;
        }

        public bool Reload()
        {
            if (isReloading)
            {
                if (pastMinTime)
                {
                    canShoot = true;
                    isReloading = false;
                    reloadTier = 0;

                    string reloadFeedback = "";

                    if (reloadTimer > baseSoftSpotStart * curMaxReload && reloadTimer < baseSoftSpotEnd * curMaxReload)
                    {
                        reloadTier = 1;
                    }
                    if (reloadTimer > baseSweetSpotStart * curMaxReload && reloadTimer < baseSweetSpotEnd * curMaxReload)
                    {
                        reloadTier = 2;
                        reloadFeedback = "Play_item_proc_crit_cooldown";
                    }


                    Util.PlaySound(reloadFeedback, gameObject);
                    //Chat.AddMessage(reloadFeedback);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                Debug.Log("Something tried to reload while not reloading... good luck");
                return false;
            }
        }

        public bool CanReload()
        {
            return pastMinTime && isReloading;
        }

        public bool FireReady()
        {
            return canShoot;
        }

        public int GetReloadTier()
        {
            return reloadTier;
        }

        public void UpdateAttackSpeed(float AS)
        {
            attackSpeed = AS;

            curMinReload = baseMinReloadTime / attackSpeed;

            scaledAttackSpeed = (1f + 3f * (1f - 1f / AS));

            curMaxReload = baseMaxReloadTime / Mathf.Min(AS, scaledAttackSpeed);
        }

        private void GenerateBar()
        {
            int bar1Start = (int)(baseSoftSpotStart * totalBarWidth);
            int bar1End = (int)(baseSoftSpotEnd * totalBarWidth);

            int bar2Start = (int)(baseSweetSpotStart * totalBarWidth);
            int bar2End = (int)(baseSweetSpotEnd * totalBarWidth);

            barTexture = new Texture2D(totalBarWidth + 2, totalBarHeight + 2, TextureFormat.ARGB32, false);

            for (int x = 0; x < barTexture.width; x++)
            {
                for (int y = 0; y < barTexture.height; y++)
                {
                    if (x == 0 || y == 0 || x == barTexture.width - 1 || y == barTexture.height - 1)
                    {
                        barTexture.SetPixel(x, y, borderColor);
                    }
                    else
                    {
                        if (x > bar2Start + 2 && x < bar2End)
                        {
                            barTexture.SetPixel(x, y, bar2Color);
                        }
                        else
                        {
                            if (x > bar1Start + 2 && x < bar1End)
                            {
                                barTexture.SetPixel(x, y, bar1Color);
                            }
                            else
                            {
                                barTexture.SetPixel(x, y, baseColor);
                            }
                        }
                    }
                }
            }

            barTexture.Apply();

            sliderX1 = (int)((width - totalBarWidth) / 2f + barHOffset);
            sliderX2 = (int)((width + totalBarWidth) / 2f + barHOffset);
            sliderY = (int)((height - totalBarHeight) / 2f - barVOffset);

            barPos = new Rect(sliderX1 - 1, sliderY - 1, width, width);

            sliderTexture = new Texture2D(sliderWidth, sliderHeight, TextureFormat.ARGB32, false);

            for (int x = 0; x < sliderTexture.width; x++)
            {
                for (int y = 0; y < sliderTexture.height; y++)
                {
                    sliderTexture.SetPixel(x, y, sliderColor);
                }
            }

            sliderX1 -= Mathf.FloorToInt(sliderWidth / 2f);
            sliderX2 -= Mathf.FloorToInt(sliderWidth / 2f);
            sliderY -= Mathf.FloorToInt((sliderHeight - totalBarHeight) / 2f);

            sliderTexture.Apply();
        }
    }
}