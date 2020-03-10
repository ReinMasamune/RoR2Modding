#if MATEDITOR
using System;
using UnityEngine;

namespace RogueWispPlugin.Helpers
{
    /*
    internal class TexSelect : MonoBehaviour
    {
        const Int32 windowID = 2;
        const Single width = 600f;
        const Single height = 600f;

        private Boolean shouldDraw = false;

        private MaterialBase.TextureData data;
        private String title;
        private TextureDataMenu.TextureDataContext context;
        private Boolean isScaleOffset;

        private MaterialBase.ScaleOffsetTextureData soData;

        internal void Init( TextureDataMenu.TextureDataContext context, MaterialBase.TextureData data, String name = "Texture" )
        {
            this.data = data;
            this.context = context;
            this.title = name;
            this.isScaleOffset = this.context is ScaleOffsetTextureDataMenu.ScaleOffsetTextureDataContext;

            if( this.isScaleOffset )
            {
                this.soData = this.data as MaterialBase.ScaleOffsetTextureData;
            }
        }

        internal void On()
        {
            this.shouldDraw = true;
        }
        internal void Off()
        {
            this.shouldDraw = false;
        }

        private void OnGUI()
        {
            if( this.shouldDraw )
            {
                var rect = GUILayout.Window( windowID, new Rect( Screen.width - width - MaterialEditor.width - 50f, Screen.height - height, width, height ), this.WindowFunc, "Texture Editor", MaterialEditor.windowStyle );
            }
        }

        private void WindowFunc( Int32 windowID )
        {
            GUILayout.BeginVertical();
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.FlexibleSpace();
                    GUILayout.Label( this.title );
                    GUILayout.FlexibleSpace();
                    if( GUILayout.Button( "Close" ) )
                    {
                        this.context.enabled = false;
                    }
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                {
                    GUILayout.BeginVertical();
                    {
                        this.scroll1 = GUILayout.BeginScrollView( this.scroll1, GUILayout.Width( 250f ) );
                        {
                            if( textureNames == null )
                            {
                                textureNames = Enum.GetNames( typeof(TextureIndex) );
                                var count = textureNames.Length;
                                textureValues = new TextureIndex[count];
                                for( Int32 i = 0; i < count; ++i )
                                {
                                    var name = textureNames[i];
                                    TextureIndex val;
                                    if( Enum.TryParse<TextureIndex>( name, out val ) )
                                    {
                                        textureValues[i] = val;
                                    }
                                }
                            }

                            this.selectionIndex = GUILayout.SelectionGrid( this.selectionIndex, textureNames, 1 );

                            try
                            {
                                var tex = AssetLibrary<Texture>.i[textureValues[this.selectionIndex]];
                                this.data.texture = tex;
                            } catch (Exception e )
                            {
                                Main.LogE( e );
                            }

                        }
                        GUILayout.EndScrollView();

                    }
                    GUILayout.EndVertical();

                    GUILayout.BeginVertical();
                    {
                        GUILayout.BeginHorizontal( GUILayout.Height( 200f ) );
                        {
                            GUILayout.BeginVertical();
                            {
                                if( this.isScaleOffset )
                                {
                                    GUILayout.BeginHorizontal();
                                    {
                                        if( this.shouldResetTile )
                                        {
                                            this.ResetTileValues();
                                        }
                                        GUILayout.Label( "Tiling", GUILayout.Width( 6f * MenuStructure.widthPerChar ) );
                                        GUILayout.FlexibleSpace();

                                        GUILayout.Label( "X:", GUILayout.Width( MenuStructure.widthPerChar * 2f ) );
                                        this.textTileX = GUILayout.TextField( this.textTileX, GUILayout.Width( MenuStructure.widthPerChar * 9f ) );

                                        GUILayout.Label( "Y", GUILayout.Width( MenuStructure.widthPerChar ) );
                                        this.textTileY = GUILayout.TextField( this.textTileY, GUILayout.Width( MenuStructure.widthPerChar * 9f ) );

                                        if( GUILayout.Button( "SET", GUILayout.Width( 6f * MenuStructure.widthPerChar ) ) )
                                        {
                                            Single tempX;
                                            Single tempY;
                                            var successX = Single.TryParse( this.textTileX, out tempX );
                                            var successY = Single.TryParse( this.textTileY, out tempY );
                                            if( successX && successY )
                                            {
                                                this.shouldResetTile = true;
                                                this.soData.tiling = new Vector2( tempX, tempY );
                                            } else
                                            {
                                                Main.LogE( "Invalid string" );
                                                this.shouldResetTile = true;
                                            }
                                        }
                                    }
                                    GUILayout.EndHorizontal();

                                    GUILayout.BeginHorizontal();
                                    {
                                        if( this.shouldResetOff )
                                        {
                                            this.ResetOffValues();
                                        }
                                        GUILayout.Label( "Offset", GUILayout.Width( 6f * MenuStructure.widthPerChar ) );
                                        GUILayout.FlexibleSpace();

                                        GUILayout.Label( "X:", GUILayout.Width( MenuStructure.widthPerChar * 2f ) );
                                        this.textOffX = GUILayout.TextField( this.textOffX, GUILayout.Width( MenuStructure.widthPerChar * 9f ) );

                                        GUILayout.Label( "Y", GUILayout.Width( MenuStructure.widthPerChar ) );
                                        this.textOffY = GUILayout.TextField( this.textOffY, GUILayout.Width( MenuStructure.widthPerChar * 9f ) );

                                        if( GUILayout.Button( "SET", GUILayout.Width( 6f * MenuStructure.widthPerChar ) ) )
                                        {
                                            Single tempX;
                                            Single tempY;
                                            var successX = Single.TryParse( this.textOffX, out tempX );
                                            var successY = Single.TryParse( this.textOffY, out tempY );
                                            if( successX && successY )
                                            {
                                                this.shouldResetOff = true;
                                                this.soData.offset = new Vector2( tempX, tempY );
                                            } else
                                            {
                                                Main.LogE( "Invalid string" );
                                                this.shouldResetOff = true;
                                            }
                                        }
                                    }
                                    GUILayout.EndHorizontal();
                                }
                            }
                            GUILayout.EndVertical();
                        }
                        GUILayout.EndHorizontal();

                        this.scroll2 = GUILayout.BeginScrollView( this.scroll2 );
                        {
                            GUILayout.Box( this.data.texture );
                        }
                        GUILayout.EndScrollView();
                        
                    }
                    GUILayout.EndVertical();
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }

        private Vector2 scroll1;
        private Vector2 scroll2;

        private String textTileX;
        private String textTileY;
        private String textOffX;
        private String textOffY;


        private Boolean shouldResetTile = true;
        private Boolean shouldResetOff = true;

        private void ResetTileValues()
        {
            this.shouldResetTile = false;
            this.textTileX = this.soData.tiling.x.ToString();
            this.textTileY = this.soData.tiling.y.ToString();
        }
        private void ResetOffValues()
        {
            this.shouldResetOff = false;
            this.textOffX = this.soData.offset.x.ToString();
            this.textOffY = this.soData.offset.y.ToString();
        }

        private Int32 selectionIndex = 0;

        private static String[] textureNames;
        private static TextureIndex[] textureValues;
    }
    */
}
#endif