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
    public class SniperChargeTracker : MonoBehaviour
    {
        private static float baseChargeDuration = 10f;
        private static float anyChargeThreshold = 0.25f;
        private static float specChargeThreshold = 0.65f;

        private static int totalBarWidth = 200;
        private static int totalBarHeight = 12;
        private static int barHOffset = 0;
        private static int barVOffset = 103;

        private static int sliderWidth = 3;
        private static int sliderHeight = 18;

        private static Color borderColor = new Color(0f, 0f, 0f, 1f);
        private static Color baseColor = new Color(0f, 0f, 0f, 0.5f);
        private static Color bar1Color = new Color(0.25f, 0.75f, 0.75f, 0.75f);
        private static Color bar2Color = new Color(0.75f, 0.25f, 0.25f, 0.75f);
        private static Color sliderColor = new Color(1f, 1f, 1f, 1f);

        Texture2D barTexture;
        Texture2D sliderTexture;
        Rect barPos;
        int sliderY;
        int sliderX1;
        int sliderX2;

        private float chargeLevel = 0f;
        private bool showChargeBar = false;
        private int chargeTier = 0;
        public bool charging = false;


        int width;
        int height;


        public void Awake()
        {
            width = Screen.width;
            height = Screen.height;

            GenerateBar();
        }

        public void OnGUI()
        {
            if (showChargeBar)
            {
                GUI.Label(barPos, barTexture);
                int curSliderX = (int)((sliderX2 - sliderX1) * chargeLevel + sliderX1);
                GUI.Label(new Rect(curSliderX, sliderY, width, width), sliderTexture);
            }
        }

        public void AddCharge(float chg)
        {
            if (showChargeBar)
            {
                if (chargeLevel < 1f)
                {
                    chargeLevel += chg / baseChargeDuration;
                }
                else
                {
                    chargeLevel = 1f;
                }

                if (chargeLevel > specChargeThreshold)
                {
                    chargeTier = 2;
                }
                else
                {
                    if (chargeLevel > anyChargeThreshold)
                    {
                        chargeTier = 1;
                    }
                    else
                    {
                        chargeTier = 0;
                    }
                }
            }
            else
            {
                chargeTier = 0;
            }

        }

        public void UpdateCharge(float newCharge)
        {
            chargeLevel = newCharge;
        }

        public void ShowBar(bool show)
        {
            showChargeBar = show;
            charging = show;
        }

        public void UpdateChargeTier(int i)
        {
            chargeTier = i;
        }

        public int GetChargeTier()
        {
            return chargeTier;
        }

        public float GetCharge()
        {
            return chargeLevel;
        }

        private void GenerateBar()
        {
            int bar1Start = (int)(anyChargeThreshold * totalBarWidth);
            int bar1End = (int)(specChargeThreshold * totalBarWidth);

            int bar2Start = (int)(specChargeThreshold * totalBarWidth);
            int bar2End = (int)(totalBarWidth);

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