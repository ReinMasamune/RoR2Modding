using RoR2;
using System;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Networking;

namespace ReinSniperRework
{
    internal partial class Main
    {
        public class SniperUIController : NetworkBehaviour
        {
            #region Indicator
            const Single indicatorWFrac = 0.005f;
            const Single indicatorHFrac = 0.03f;
            const Int32 indicatorBorderThickness = 2;
            private static readonly Color32 indicatorCenterColor = new Color32( 255, 255, 255, 255 );
            private static readonly Color32 indicatorBorderColor = new Color32( 0, 0, 0, 255 );
            #endregion
            #region Reload
            const Single reloadBarWFrac = 0.13f;
            const Single reloadBarHFrac = 0.02f;
            const Single reloadBarWPosFrac = 0.5f;
            const Single reloadBarHPosFrac = 0.65f;
            const Int32 reloadBorderThickness = 2;
            private static readonly Color32 reloadBorderColor = new Color32( 0, 0, 0, 255 );
            private static readonly Color32 reloadBGColor = new Color32( 5, 5, 5, 255 );
            private static readonly Color32 reloadGoodColor = new Color32( 150, 150, 150, 255 );
            private static readonly Color32 reloadPerfectColor = new Color32( 255, 255, 255, 255 );
            #endregion
            #region Charge
            const Single chargeBarWFrac = 0.13f;
            const Single chargeBarHFrac = 0.02f;
            const Single chargeBarWPosFrac = 0.5f;
            const Single chargeBarHPosFrac = 0.35f;
            const Int32 chargeBorderThickness = 2;
            private static readonly Color32 chargeBorderColor = new Color32( 0, 0, 0, 255 );
            private static readonly Color32 chargeBGColor = new Color32( 5, 5, 5, 255 );
            private static readonly Color32 chargeT1Color = new Color32( 150, 150, 150, 255 );
            private static readonly Color32 chargeT2Color = new Color32( 255, 255, 255, 255 );
            #endregion
            private Texture2D indicator;
            private Texture2D reloadBar;
            private Texture2D chargeBar;
            private JobHandle indicatorHandle;
            private JobHandle reloadHandle;
            private JobHandle chargeHandle;
            private Rect reloadBarPos;
            private Rect chargeBarPos;
            private Vector2 indicatorSize;
            private Single reloadIndicatorHeight;
            private Single chargeIndicatorHeight;
            private Single reloadIndicatorStartW;
            private Single chargeIndicatorStartW;
            private Single reloadIndicatorWMult;
            private Single chargeIndicatorWMult;


            private SniperReload reloadInstance;
            private SniperCharging chargeInstance;

            public void SetReloading( SniperReload reloadInstance )
            {
                this.reloadInstance = reloadInstance;
            }

            public void SetCharging( SniperCharging chargeInstance )
            {
                this.chargeInstance = chargeInstance;
            }

            public Single GetCharge()
            {
                if( this.chargeInstance != null && this.chargeInstance.isActive )
                {
                    return this.chargeInstance.damageMult;
                } 
                return 1f;
            }

            private void Awake()
            {
                this.indicator = new Texture2D( (Int32)(Screen.width * indicatorWFrac), (Int32)(Screen.height * indicatorHFrac), TextureFormat.RGBA32, false );
                this.indicatorHandle = new DrawTexBar
                {
                    borderColor = indicatorBorderColor,
                    borderThickness = indicatorBorderThickness,
                    perfectColor = indicatorCenterColor,
                    perfectStart = 0,
                    perfectEnd = this.indicator.width,
                    height = this.indicator.height,
                    width = this.indicator.width,
                    tex = this.indicator.GetRawTextureData<Color32>()
                }.Schedule( this.indicator.width * this.indicator.height, 1 );

                this.reloadBar = new Texture2D( (Int32)(Screen.width * reloadBarWFrac), (Int32)(Screen.height * reloadBarHFrac), TextureFormat.RGBA32, false );
                this.reloadHandle = new DrawTexBar
                {
                    normalColor = reloadBGColor,
                    goodColor = reloadGoodColor,
                    perfectColor = reloadPerfectColor,
                    tex = this.reloadBar.GetRawTextureData<Color32>(),
                    width = this.reloadBar.width,
                    height = this.reloadBar.height,
                    borderColor = reloadBorderColor,
                    borderThickness = reloadBorderThickness,
                    goodStart = (Int32)(SniperReload.gFracStart * this.reloadBar.width),
                    goodEnd = (Int32)(SniperReload.gFracEnd * this.reloadBar.width),
                    perfectStart = (Int32)(SniperReload.pFracStart * this.reloadBar.width),
                    perfectEnd = (Int32)(SniperReload.pFracEnd * this.reloadBar.width)
                }.Schedule( this.reloadBar.width * this.reloadBar.height, 1 );
                
                this.chargeBar = new Texture2D( (Int32)(Screen.width * chargeBarWFrac), (Int32)(Screen.height * chargeBarHFrac), TextureFormat.RGBA32, false );                
                this.chargeHandle = new DrawTexBar
                {
                    normalColor = chargeBGColor,
                    goodColor = chargeT1Color,
                    perfectColor = chargeT2Color,
                    tex = this.chargeBar.GetRawTextureData<Color32>(),
                    width = this.chargeBar.width,
                    height = this.chargeBar.height,
                    borderColor = chargeBorderColor,
                    borderThickness = chargeBorderThickness,
                    goodStart = (Int32)(SniperCharging.t1StartFrac * this.chargeBar.width),
                    goodEnd = this.chargeBar.width,
                    perfectStart = (Int32)(SniperCharging.t2StartFrac * this.chargeBar.width),
                    perfectEnd = this.chargeBar.width
                }.Schedule( this.chargeBar.width * this.chargeBar.height, 1 );

                this.indicatorSize = new Vector2( this.indicator.width, this.indicator.height );

                this.reloadBarPos = new Rect( (Screen.width * reloadBarWPosFrac) - (this.reloadBar.width / 2f), (Screen.height * reloadBarHPosFrac) - (this.reloadBar.height / 2f), this.reloadBar.width, this.reloadBar.height );
                this.reloadIndicatorHeight = (Screen.height * reloadBarHPosFrac) - (this.indicator.height / 2f);
                this.reloadIndicatorStartW = (Screen.width * reloadBarWPosFrac) - (this.indicator.width / 2f) - (this.reloadBar.width / 2f);
                this.reloadIndicatorWMult = this.reloadBar.width;



                this.chargeBarPos = new Rect( (Screen.width * chargeBarWPosFrac) - (this.chargeBar.width / 2f), (Screen.height * chargeBarHPosFrac) - (this.chargeBar.height / 2f), this.chargeBar.width, this.chargeBar.height );
                this.chargeIndicatorHeight = (Screen.height * chargeBarHPosFrac) - (this.indicator.height / 2f);
                this.chargeIndicatorStartW = (Screen.width * chargeBarWPosFrac) - (this.indicator.width / 2f) - (this.chargeBar.width / 2f);
                this.chargeIndicatorWMult = this.chargeBar.width;
            }

            private void Start()
            {
                this.indicatorHandle.Complete();
                this.indicator.Apply();
                this.reloadHandle.Complete();
                this.reloadBar.Apply();
                this.chargeHandle.Complete();
                this.chargeBar.Apply();
            }

            private void OnGUI()
            {
                if( this.reloadInstance != null && this.reloadInstance.isActive ) this.DrawReloadBar( this.reloadInstance.reloadFrac );
                //if( this.chargeInstance != null && this.chargeInstance.isActive ) this.DrawChargeBar( this.chargeInstance.charge );
            }





            private void DrawReloadBar( Single frac )
            {
                UnityEngine.GUI.DrawTexture( this.reloadBarPos, this.reloadBar );
                UnityEngine.GUI.DrawTexture( new Rect( this.reloadIndicatorStartW + this.reloadIndicatorWMult * frac, this.reloadIndicatorHeight, this.indicatorSize.x, this.indicatorSize.y ), this.indicator ); 
            }

            private void DrawChargeBar( Single frac )
            {
                UnityEngine.GUI.DrawTexture( this.chargeBarPos, this.chargeBar );
                UnityEngine.GUI.DrawTexture( new Rect( this.chargeIndicatorStartW + this.chargeIndicatorWMult * frac, this.chargeIndicatorHeight, this.indicatorSize.x, this.indicatorSize.y ), this.indicator );
            }


            private struct DrawTexBar : IJobParallelFor
            {
                public NativeArray<Color32> tex;
                public Int32 width;
                public Int32 height;
                public Int32 goodStart;
                public Int32 goodEnd;
                public Int32 perfectStart;
                public Int32 perfectEnd;
                public Int32 borderThickness;
                public Color32 normalColor;
                public Color32 goodColor;
                public Color32 perfectColor;
                public Color32 borderColor;

                public void Execute( Int32 index )
                {
                    Int32 x = index % this.width;
                    Int32 y = index / this.width;

                    Color32 target = this.normalColor;
                    if( x >= this.goodStart && x <= this.goodEnd ) target = this.goodColor;
                    if( x >= this.perfectStart && x <= this.perfectEnd ) target = this.perfectColor;
                    if( x <= this.borderThickness || y <= this.borderThickness || x >= this.width - this.borderThickness - 1 || y >= this.height - this.borderThickness - 1 ) target = this.borderColor;
                    this.tex[index] = target;
                }
            }
            /*
            //public ReinDataLibrary data;

            //public bool showReloadBar = false;
            //public bool showChargeBar = false;

            
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
                if( Time.timeScale != 0f )
                {
                    if( showReloadBar )
                    {
                        UnityEngine.GUI.Label( bar1Pos, bar1Texture );
                        int curSliderX = (int)((slider1X2 - slider1X1) * reloadFrac + slider1X1);
                        UnityEngine.GUI.Label( new Rect( curSliderX, slider1Y, width, width ), slider1Texture );
                    }
                    if( showChargeBar )
                    {
                        UnityEngine.GUI.Label( bar2Pos, bar2Texture );
                        int curSliderX = (int)((slider2X2 - slider2X1) * data.g_shotCharge + slider2X1);
                        UnityEngine.GUI.Label( new Rect( curSliderX, slider2Y, width, width ), slider2Texture );
                    }
                }
            }

            private void GenerateBar1()
            {
                int bar1Start = (int)(data.p_softLoadStart * data.ui_bar1Width);
                int bar1End = (int)(data.p_softLoadEnd * data.ui_bar1Width);

                int bar2Start = (int)(data.p_sweetLoadStart * data.ui_bar1Width);
                int bar2End = (int)(data.p_sweetLoadEnd * data.ui_bar1Width);

                bar1Texture = new Texture2D( data.ui_bar1Width + 2, data.ui_bar1Height + 2, TextureFormat.ARGB32, false );

                for( int x = 0; x < bar1Texture.width; x++ )
                {
                    for( int y = 0; y < bar1Texture.height; y++ )
                    {
                        if( x == 0 || y == 0 || x == bar1Texture.width - 1 || y == bar1Texture.height - 1 )
                        {
                            bar1Texture.SetPixel( x, y, data.ui_bar1BorderColor );
                        } else
                        {
                            if( x > bar2Start + 1 && x < bar2End + 1 )
                            {
                                bar1Texture.SetPixel( x, y, data.ui_bar1FillColor2 );
                            } else
                            {
                                if( x > bar1Start + 1 && x < bar1End + 1 )
                                {
                                    bar1Texture.SetPixel( x, y, data.ui_bar1FillColor1 );
                                } else
                                {
                                    bar1Texture.SetPixel( x, y, data.ui_bar1BaseColor );
                                }
                            }
                        }
                    }
                }

                bar1Texture.Apply();

                slider1X1 = (int)((width - data.ui_bar1Width) / 2f + data.ui_bar1HOffset);
                slider1X2 = (int)((width + data.ui_bar1Width) / 2f + data.ui_bar1HOffset);
                slider1Y = (int)((height - data.ui_bar1Height) / 2f - data.ui_bar1VOffset);

                bar1Pos = new Rect( slider1X1 - 1, slider1Y - 1, width, width );

                slider1Texture = new Texture2D( data.ui_bar1Slider1Width, data.ui_bar1Slider1Height, TextureFormat.ARGB32, false );

                for( int x = 0; x < slider1Texture.width; x++ )
                {
                    for( int y = 0; y < slider1Texture.height; y++ )
                    {
                        slider1Texture.SetPixel( x, y, data.ui_bar1SliderColor );
                    }
                }

                slider1X1 -= Mathf.FloorToInt( data.ui_bar1Slider1Width / 2f );
                slider1X2 -= Mathf.FloorToInt( data.ui_bar1Slider1Width / 2f );
                slider1Y -= Mathf.FloorToInt( (data.ui_bar1Slider1Height - data.ui_bar1Height) / 2f );

                slider1Texture.Apply();
            }

            private void GenerateBar2()
            {
                int bar1Start = (int)(data.s_boost1Start * data.ui_bar2Width);
                int bar1End = (int)(data.s_boost2Start * data.ui_bar2Width);

                int bar2Start = (int)(data.s_boost2Start * data.ui_bar2Width);
                int bar2End = data.ui_bar2Width;

                bar2Texture = new Texture2D( data.ui_bar2Width + 2, data.ui_bar2Height + 2, TextureFormat.ARGB32, false );

                for( int x = 0; x < bar2Texture.width; x++ )
                {
                    for( int y = 0; y < bar2Texture.height; y++ )
                    {
                        if( x == 0 || y == 0 || x == bar2Texture.width - 1 || y == bar2Texture.height - 1 )
                        {
                            bar2Texture.SetPixel( x, y, data.ui_bar2BorderColor );
                        } else
                        {
                            if( x > bar2Start + 1 && x < bar2End + 1 )
                            {
                                bar2Texture.SetPixel( x, y, data.ui_bar2FillColor2 );
                            } else
                            {
                                if( x > bar1Start + 1 && x < bar1End + 2 )
                                {
                                    bar2Texture.SetPixel( x, y, data.ui_bar2FillColor1 );
                                } else
                                {
                                    bar2Texture.SetPixel( x, y, data.ui_bar2BaseColor );
                                }
                            }
                        }
                    }
                }

                bar2Texture.Apply();

                slider2X1 = (int)((width - data.ui_bar2Width) / 2f + data.ui_bar2HOffset);
                slider2X2 = (int)((width + data.ui_bar2Width) / 2f + data.ui_bar2HOffset);
                slider2Y = (int)((height - data.ui_bar2Height) / 2f - data.ui_bar2VOffset);

                bar2Pos = new Rect( slider2X1 - 1, slider2Y - 1, width, width );

                slider2Texture = new Texture2D( data.ui_bar2Slider1Width, data.ui_bar2Slider1Height, TextureFormat.ARGB32, false );

                for( int x = 0; x < slider2Texture.width; x++ )
                {
                    for( int y = 0; y < slider2Texture.height; y++ )
                    {
                        slider2Texture.SetPixel( x, y, data.ui_bar2SliderColor );
                    }
                }

                slider2X1 -= Mathf.FloorToInt( data.ui_bar2Slider1Width / 2f );
                slider2X2 -= Mathf.FloorToInt( data.ui_bar2Slider1Width / 2f );
                slider2Y -= Mathf.FloorToInt( (data.ui_bar2Slider1Height - data.ui_bar2Height) / 2f );

                slider2Texture.Apply();
            }
        }
        */
        }
    }
}