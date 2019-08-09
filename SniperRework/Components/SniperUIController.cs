using RoR2;
using UnityEngine;

namespace ReinSniperRework
{
    public class SniperUIController : MonoBehaviour
    {
        public ReinDataLibrary data;

        public bool showReloadBar = false;
        public bool showChargeBar = false;

        public float reloadFrac = 0.0f;
        public float chargeFrab = 0.0f;

        Texture2D bar1Texture;
        Texture2D bar2Texture;
        Texture2D slider1Texture;
        Texture2D slider2Texture;
        Rect bar1Pos;
        Rect bar2Pos;
        int slider1Y;
        int slider1X1;
        int slider1X2;
        int slider2Y;
        int slider2X1;
        int slider2X2;

        int width;
        int height;

        public void Awake()
        {
            width = Screen.width;
            height = Screen.height;
            GenerateBar1();
            GenerateBar2();
        }

        public void OnGUI()
        {
            if (Time.timeScale != 0f )
            {
                if (showReloadBar)
                {
                    GUI.Label(bar1Pos, bar1Texture);
                    int curSliderX = (int)((slider1X2 - slider1X1) * reloadFrac + slider1X1);
                    GUI.Label(new Rect(curSliderX, slider1Y, width, width), slider1Texture);
                }
                if (showChargeBar)
                {
                    GUI.Label(bar2Pos, bar2Texture);
                    int curSliderX = (int)((slider2X2 - slider2X1) * data.g_shotCharge + slider2X1);
                    GUI.Label(new Rect(curSliderX, slider2Y, width, width), slider2Texture);
                }
            }
        }

        private void GenerateBar1()
        {
            int bar1Start = (int)(data.p_softLoadStart * data.ui_bar1Width);
            int bar1End = (int)(data.p_softLoadEnd * data.ui_bar1Width);

            int bar2Start = (int)(data.p_sweetLoadStart * data.ui_bar1Width);
            int bar2End = (int)(data.p_sweetLoadEnd * data.ui_bar1Width);

            bar1Texture = new Texture2D(data.ui_bar1Width + 2, data.ui_bar1Height + 2, TextureFormat.ARGB32, false);

            for (int x = 0; x < bar1Texture.width; x++)
            {
                for (int y = 0; y < bar1Texture.height; y++)
                {
                    if (x == 0 || y == 0 || x == bar1Texture.width - 1 || y == bar1Texture.height - 1)
                    {
                        bar1Texture.SetPixel(x, y, data.ui_bar1BorderColor);
                    }
                    else
                    {
                        if (x > bar2Start + 1 && x < bar2End + 1)
                        {
                            bar1Texture.SetPixel(x, y, data.ui_bar1FillColor2);
                        }
                        else
                        {
                            if (x > bar1Start + 1 && x < bar1End + 1)
                            {
                                bar1Texture.SetPixel(x, y, data.ui_bar1FillColor1);
                            }
                            else
                            {
                                bar1Texture.SetPixel(x, y, data.ui_bar1BaseColor);
                            }
                        }
                    }
                }
            }

            bar1Texture.Apply();

            slider1X1 = (int)((width - data.ui_bar1Width) / 2f + data.ui_bar1HOffset);
            slider1X2 = (int)((width + data.ui_bar1Width) / 2f + data.ui_bar1HOffset);
            slider1Y = (int)((height - data.ui_bar1Height) / 2f - data.ui_bar1VOffset);

            bar1Pos = new Rect(slider1X1 - 1, slider1Y - 1, width, width);

            slider1Texture = new Texture2D(data.ui_bar1Slider1Width, data.ui_bar1Slider1Height, TextureFormat.ARGB32, false);

            for (int x = 0; x < slider1Texture.width; x++)
            {
                for (int y = 0; y < slider1Texture.height; y++)
                {
                    slider1Texture.SetPixel(x, y, data.ui_bar1SliderColor);
                }
            }

            slider1X1 -= Mathf.FloorToInt(data.ui_bar1Slider1Width / 2f);
            slider1X2 -= Mathf.FloorToInt(data.ui_bar1Slider1Width / 2f);
            slider1Y -= Mathf.FloorToInt((data.ui_bar1Slider1Height - data.ui_bar1Height) / 2f);

            slider1Texture.Apply();
        }

        private void GenerateBar2()
        {
            int bar1Start = (int)(data.s_boost1Start * data.ui_bar2Width);
            int bar1End = (int)(data.s_boost2Start * data.ui_bar2Width);

            int bar2Start = (int)(data.s_boost2Start * data.ui_bar2Width);
            int bar2End = data.ui_bar2Width;

            bar2Texture = new Texture2D(data.ui_bar2Width + 2, data.ui_bar2Height + 2, TextureFormat.ARGB32, false);

            for (int x = 0; x < bar2Texture.width; x++)
            {
                for (int y = 0; y < bar2Texture.height; y++)
                {
                    if (x == 0 || y == 0 || x == bar2Texture.width - 1 || y == bar2Texture.height - 1)
                    {
                        bar2Texture.SetPixel(x, y, data.ui_bar2BorderColor);
                    }
                    else
                    {
                        if (x > bar2Start + 1 && x < bar2End + 1)
                        {
                            bar2Texture.SetPixel(x, y, data.ui_bar2FillColor2);
                        }
                        else
                        {
                            if (x > bar1Start + 1 && x < bar1End + 2)
                            {
                                bar2Texture.SetPixel(x, y, data.ui_bar2FillColor1);
                            }
                            else
                            {
                                bar2Texture.SetPixel(x, y, data.ui_bar2BaseColor);
                            }
                        }
                    }
                }
            }

            bar2Texture.Apply();

            slider2X1 = (int)((width - data.ui_bar2Width) / 2f + data.ui_bar2HOffset);
            slider2X2 = (int)((width + data.ui_bar2Width) / 2f + data.ui_bar2HOffset);
            slider2Y = (int)((height - data.ui_bar2Height) / 2f - data.ui_bar2VOffset);

            bar2Pos = new Rect(slider2X1 - 1, slider2Y - 1, width, width);

            slider2Texture = new Texture2D(data.ui_bar2Slider1Width, data.ui_bar2Slider1Height, TextureFormat.ARGB32, false);

            for (int x = 0; x < slider2Texture.width; x++)
            {
                for (int y = 0; y < slider2Texture.height; y++)
                {
                    slider2Texture.SetPixel(x, y, data.ui_bar2SliderColor);
                }
            }

            slider2X1 -= Mathf.FloorToInt(data.ui_bar2Slider1Width / 2f);
            slider2X2 -= Mathf.FloorToInt(data.ui_bar2Slider1Width / 2f);
            slider2Y -= Mathf.FloorToInt((data.ui_bar2Slider1Height - data.ui_bar2Height) / 2f);

            slider2Texture.Apply();
        }
    }
}