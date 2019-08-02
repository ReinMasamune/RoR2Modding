using UnityEngine;

namespace ReinSniperRework
{
    public class SniperChargeTracker : MonoBehaviour
    {
        public ReinDataLibrary data;

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
                    chargeLevel += chg / data.sc_baseChargeDuration;
                }
                else
                {
                    chargeLevel = 1f;
                }

                if (chargeLevel > data.sc_specChargeThreshold)
                {
                    chargeTier = 2;
                }
                else
                {
                    if (chargeLevel > data.sc_anyChargeThreshold)
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
            int bar1Start = (int)(data.sc_anyChargeThreshold * data.sc_totalBarWidth);
            int bar1End = (int)(data.sc_specChargeThreshold * data.sc_totalBarWidth);

            int bar2Start = (int)(data.sc_specChargeThreshold * data.sc_totalBarWidth);
            int bar2End = (int)(data.sc_totalBarWidth);

            barTexture = new Texture2D(data.sc_totalBarWidth + 2, data.sc_totalBarHeight + 2, TextureFormat.ARGB32, false);

            for (int x = 0; x < barTexture.width; x++)
            {
                for (int y = 0; y < barTexture.height; y++)
                {
                    if (x == 0 || y == 0 || x == barTexture.width - 1 || y == barTexture.height - 1)
                    {
                        barTexture.SetPixel(x, y, data.sc_borderColor);
                    }
                    else
                    {
                        if (x > bar2Start + 2 && x < bar2End)
                        {
                            barTexture.SetPixel(x, y, data.sc_bar2Color);
                        }
                        else
                        {
                            if (x > bar1Start + 2 && x < bar1End)
                            {
                                barTexture.SetPixel(x, y, data.sc_bar1Color);
                            }
                            else
                            {
                                barTexture.SetPixel(x, y, data.sc_baseColor);
                            }
                        }
                    }
                }
            }

            barTexture.Apply();

            sliderX1 = (int)((width - data.sc_totalBarWidth) / 2f + data.sc_barHOffset);
            sliderX2 = (int)((width + data.sc_totalBarWidth) / 2f + data.sc_barHOffset);
            sliderY = (int)((height - data.sc_totalBarHeight) / 2f - data.sc_barVOffset);

            barPos = new Rect(sliderX1 - 1, sliderY - 1, width, width);

            sliderTexture = new Texture2D(data.sc_sliderWidth, data.sc_sliderHeight, TextureFormat.ARGB32, false);

            for (int x = 0; x < sliderTexture.width; x++)
            {
                for (int y = 0; y < sliderTexture.height; y++)
                {
                    sliderTexture.SetPixel(x, y, data.sc_sliderColor);
                }
            }

            sliderX1 -= Mathf.FloorToInt(data.sc_sliderWidth / 2f);
            sliderX2 -= Mathf.FloorToInt(data.sc_sliderWidth / 2f);
            sliderY -= Mathf.FloorToInt((data.sc_sliderHeight - data.sc_totalBarHeight) / 2f);

            sliderTexture.Apply();
        }
    }
}