
using System;

using RoR2;

using UnityEngine;

namespace Rein.RogueWispPlugin.Helpers
{
    internal class WispSkinSelectionUI : MonoBehaviour
    {
        private const Single colorSelectionFracW = 0.25f;   //0.75 left
        private const Single armorFlameGradSelectionFracW = 0.25f;      //0.5 left

        private static Texture2D colorPreview;
        //private Vector3[] corners = new Vector3[4];
        private RectTransform rectTransform;
        private Rect areaRect;

        internal void SetData( Int32 bodyIndex, UserProfile profile )
        {
            this.bodyIndex = bodyIndex;
            this.profile = profile;
            var temp = new Loadout();
            profile.CopyLoadout( temp );
            this.currentSkin = temp.bodyLoadoutManager.GetSkinIndex( bodyIndex );
            this.InitValues();
            this.dataSet = true;
        }

        private void OnEnable()
        {
            //Main.LogW( "Selection UI On" );
            this.rectTransform = base.transform as RectTransform;

            //this.rectTransform.GetWorldCorners( this.corners );
            //Vector2 topLeft = this.corners[1];
            //Vector2 bottomRight = this.corners[3];
            var screenVec = new Vector2( Screen.width, Screen.height );
            Vector2 topLeft = screenVec * new Vector2( 0.05f, 0.68f );
            Vector2 bottomRight = screenVec * new Vector2( 0.37f, 0.15f );


            var size = topLeft - bottomRight;
            topLeft.y = Screen.height - topLeft.y;
            size.x = Mathf.Abs( size.x );
            size.y = Mathf.Abs( size.y );
            this.areaRect = new Rect( topLeft, size );

            if( colorPreview == null )
            {
                colorPreview = new Texture2D( Mathf.FloorToInt( this.areaRect.width * 0.5f ), Mathf.FloorToInt( this.areaRect.height * 0.15f ) );
                for( Int32 x = 0; x < colorPreview.width; ++x )
                {
                    for( Int32 y = 0; y < colorPreview.height; ++y )
                    {
                        colorPreview.SetPixel( x, y, Color.white );
                    }
                }
                colorPreview.wrapMode = TextureWrapMode.Repeat;
                colorPreview.Apply();
            }
        }
        private void OnDisable()
        {
            //Main.LogW( "Selection UI Off" );

        }
        private Boolean dataSet = false;
        internal UserProfile profile;
        internal UInt32 currentSkin;
        internal Int32 bodyIndex;
        private void ApplySkin()
        {
            if( !this.dataSet ) return;
            var newSkin = this.GenerateSkinIndex();

            if( newSkin == this.currentSkin ) return;
            this.currentSkin = newSkin;

            Loadout loadout = new Loadout();
            this.profile.CopyLoadout( loadout );
            loadout.bodyLoadoutManager.SetSkinIndex( this.bodyIndex, this.currentSkin );
            this.profile.SetLoadout( loadout );
        }

        private UInt32 GenerateSkinIndex()
        {
            var ind = 0b0000_0000_0000_0000_0000_0000_00000_0000u;

            ind |= ( this.armorCracks ? 1u : 0u ) << 31;
            ind |= ( this.isIridescent ? 1u : 0u ) << 29;
            ind |= ( this.isCustomColor ? 1u : 0u ) << 28;

            if( this.isCustomColor )
            {
                var r = (UInt32)Mathf.RoundToInt(this.currentColor.r * 63 );
                var g = (UInt32)Mathf.RoundToInt(this.currentColor.g * 63 );
                var b = (UInt32)Mathf.RoundToInt(this.currentColor.b * 63 );

                r <<= 12;
                g <<= 6;

                ind |= r;
                ind |= g;
                ind |= b;
            } else
            {
                ind |= (UInt32)this.colorSelected;
            }

            ind |= ( (UInt32)this.flameGradSelected ) << 18;
            ind |= ( (UInt32)this.armorSelected ) << 25;

            return ind;
        }
        private void InitValues()
        {
            this.armorCracks = 1u == ( this.currentSkin & ( 1u << 31 ) ) >> 31;
            this.isIridescent = 1u == ( this.currentSkin & ( 1u << 29 ) ) >> 29;
            this.isCustomColor = 1u == ( this.currentSkin & ( 1u << 28 ) ) >> 28;
            this.flameGradSelected = (Int32)( this.currentSkin & ( 0b111u << 18 ) ) >> 18;
            this.armorSelected = (Int32)( this.currentSkin & ( 0b111u << 25 ) ) >> 25;

            if( this.isCustomColor )
            {
                this.colorSelected = 0;
                var r = ((this.currentSkin & (0b11_1111u << 12 ) ) >> 12) / 63f;
                var g = ((this.currentSkin & (0b11_1111u << 6 ) ) >> 6) / 63f;
                var b = (this.currentSkin & (0b11_1111u )) / 63f;
                this.currentColor = new Color( r, g, b, 1f );
                this.inputColor = this.currentColor;
            } else
            {
                this.currentColor = Color.black;
                this.inputColor = Color.black;
                this.colorSelected = (Int32)( this.currentSkin & 0b1111_1111u );
            }
        }


        private Vector2 scroll;
        private void OnGUI()
        {
            //Main.LogW( "GUI" );
            GUILayout.BeginArea( this.areaRect );
            {
                this.scroll = GUILayout.BeginScrollView( this.scroll );
                {
                    this.DrawSkinSelection();
                }
                GUILayout.EndScrollView();
            }
            GUILayout.EndArea();
        }

        private void DrawSkinSelection()
        {
            GUILayout.BeginHorizontal();
            {
                this.DrawColorSelection();

                GUILayout.BeginVertical( GUILayout.MaxWidth( armorFlameGradSelectionFracW * this.areaRect.width ) );
                {
                    this.DrawArmorMaterialSelection();
                    this.DrawFlameGradientTypeSelection();
                }
                GUILayout.EndVertical();

                this.DrawCustomColorSelection();

            }
            GUILayout.EndHorizontal();
        }

        private Vector2 colorSelectionScroll;
        private Int32 colorSelected;
        private Boolean isIridescent;
        private Boolean isCustomColor;
        private String[] colorNames = Enum.GetNames( typeof(WispBitSkin.WispColorIndex) );
        private void DrawColorSelection()
        {
            GUILayout.BeginVertical( GUILayout.MaxWidth( this.areaRect.width * colorSelectionFracW ) );
            {
                CenteredLabel( "Color" );

                var tempVal = GUILayout.Toggle( this.isIridescent, "Iridescent" );
                if( tempVal != this.isIridescent )
                {
                    this.isIridescent = tempVal;
                    this.ApplySkin();
                }

                tempVal = GUILayout.Toggle( this.isCustomColor, "Custom Color" );
                if( tempVal != this.isCustomColor )
                {
                    this.isCustomColor = tempVal;
                    this.ApplySkin();
                }

                this.colorSelectionScroll = GUILayout.BeginScrollView( this.colorSelectionScroll );
                {
                    var selectionInd = GUILayout.SelectionGrid( this.colorSelected, this.colorNames, 1 );
                    if( selectionInd != this.colorSelected )
                    {
                        this.colorSelected = selectionInd;
                        this.ApplySkin();
                    }
                }
                GUILayout.EndScrollView();
            }
            GUILayout.EndVertical();
        }

        private Vector2 armorSelectionScroll;
        private Int32 armorSelected;
        private Boolean armorCracks;
        private String[] armorSelectionNames = Enum.GetNames( typeof(WispBitSkin.ArmorMaterialType));
        private void DrawArmorMaterialSelection()
        {
            GUILayout.BeginVertical( GUILayout.MaxHeight( this.areaRect.height * 0.45f ) );
            {
                CenteredLabel( "Armor" );
                var tempCracks = GUILayout.Toggle(this.armorCracks, "Glow Cracks" );
                if( tempCracks != this.armorCracks )
                {
                    this.armorCracks = tempCracks;
                    this.ApplySkin();
                }

                this.armorSelectionScroll = GUILayout.BeginScrollView( this.armorSelectionScroll );
                {
                    var selectionInd = GUILayout.SelectionGrid( this.armorSelected, this.armorSelectionNames, 1 );
                    if( selectionInd != this.armorSelected )
                    {
                        this.armorSelected = selectionInd;
                        this.ApplySkin();
                    }
                }
                GUILayout.EndScrollView();
            }
            GUILayout.EndVertical();
        }


        private Vector2 flameGradScroll;
        private Int32 flameGradSelected;
        private String[] flameGradNames = Enum.GetNames( typeof( WispBitSkin.FlameGradientType ) );

        private void DrawFlameGradientTypeSelection()
        {
            GUILayout.BeginVertical( GUILayout.MaxHeight( this.areaRect.height * 0.45f ) );
            {
                CenteredLabel( "Fire" );
                this.flameGradScroll = GUILayout.BeginScrollView( this.flameGradScroll );
                {
                    var selectionInd = GUILayout.SelectionGrid( this.flameGradSelected, this.flameGradNames, 1 );
                    if( selectionInd != this.flameGradSelected )
                    {
                        this.flameGradSelected = selectionInd;
                        this.ApplySkin();
                    }
                }
                GUILayout.EndScrollView();
            }
            GUILayout.EndVertical();
        }


        private Color inputColor;
        private Single inputR
        {
            get => this.inputColor.r * 255;
            set
            {
                var temp = value + 1f;
                temp /= 4f;
                temp = Mathf.RoundToInt( temp );
                temp *= 4f;
                temp -= 1f;
                temp /= 255;
                temp = Mathf.Clamp01( temp );
                if( this.inputR != temp )
                {
                    this.inputColor.r = temp;
                    this.hasUnappliedChanges = true;
                }
            }
        }
        private Single inputG
        {
            get => this.inputColor.g * 255;
            set
            {
                var temp = value + 1f;
                temp /= 4f;
                temp = Mathf.RoundToInt( temp );
                temp *= 4f;
                temp -= 1f;
                temp /= 255;
                temp = Mathf.Clamp01( temp );
                if( this.inputG != temp )
                {
                    this.inputColor.g = temp;
                    this.hasUnappliedChanges = true;
                }
            }
        }

        private Single inputB
        {
            get => this.inputColor.b * 255;
            set
            {
                var temp = value + 1f;
                temp /= 4f;
                temp = Mathf.RoundToInt( temp );
                temp *= 4f;
                temp -= 1f;
                temp /= 255;
                temp = Mathf.Clamp01( temp );
                if( this.inputB != temp )
                {
                    this.inputColor.b = temp;
                    this.hasUnappliedChanges = true;
                }
            }
        }
        private Color currentColor;
        private Boolean hasUnappliedChanges = false;
        private void DrawCustomColorSelection()
        {
            GUILayout.BeginVertical();
            {
                CenteredLabel( "Custom Color" );

                var tempColor = GUI.color;
                GUI.color = this.currentColor;
                GUILayout.Label( colorPreview, GUILayout.Height( colorPreview.height ), GUILayout.Width( colorPreview.width ) );
                GUI.color = tempColor;


                if( GUILayout.Button( "Apply", GUILayout.Height( 25f ) ) && this.hasUnappliedChanges )
                {
                    this.hasUnappliedChanges = false;
                    this.currentColor = this.inputColor;
                    this.ApplySkin();
                }


                GUILayout.FlexibleSpace();

                var tempColor2 = GUI.color;
                GUI.color = this.inputColor;
                GUILayout.Label( colorPreview, GUILayout.Height( colorPreview.height ), GUILayout.Width( colorPreview.width ) );
                GUI.color = tempColor2;



                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label( "R", GUILayout.Width( 10f ) );
                    GUILayout.FlexibleSpace();
                    GUILayout.Label( ( (Int32)this.inputR ).ToString(), GUILayout.Width( 30f ) );
                    GUILayout.FlexibleSpace();
                    var tempSlider = GUILayout.HorizontalSlider( this.inputR, 0f, 255f, GUILayout.Width( this.areaRect.width * 0.5f * 0.5f ) );
                    if( tempSlider != this.inputR )
                    {
                        this.inputR = tempSlider;
                    }
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label( "G", GUILayout.Width( 10f ) );
                    GUILayout.FlexibleSpace();
                    GUILayout.Label( ( (Int32)this.inputG ).ToString(), GUILayout.Width( 30f ) );
                    GUILayout.FlexibleSpace();
                    var tempSlider = GUILayout.HorizontalSlider( this.inputG, 0f, 255f, GUILayout.Width( this.areaRect.width * 0.5f * 0.5f ) );
                    if( tempSlider != this.inputG )
                    {
                        this.inputG = tempSlider;
                    }
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label( "B", GUILayout.Width( 10f ) );
                    GUILayout.FlexibleSpace();
                    GUILayout.Label( ( (Int32)this.inputB ).ToString(), GUILayout.Width( 30f ) );
                    GUILayout.FlexibleSpace();
                    var tempSlider = GUILayout.HorizontalSlider( this.inputB, 0f, 255f, GUILayout.Width( this.areaRect.width * 0.5f * 0.5f ) );
                    if( tempSlider != this.inputB )
                    {
                        this.inputB = tempSlider;
                    }
                }
                GUILayout.EndHorizontal();


                GUILayout.Space( this.areaRect.height * 0.2f );
            }
            GUILayout.EndVertical();
        }


        private static void CenteredLabel( String text )
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.FlexibleSpace();
                GUILayout.Label( text );
                GUILayout.FlexibleSpace();
            }
            GUILayout.EndHorizontal();
        }
    }
}
