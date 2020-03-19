#if MATEDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using RogueWispPlugin.Helpers.IMGUI;
using ReinCore;

namespace RogueWispPlugin.Helpers
{
    internal class StandardTextureHiddenSelection : IGUIMenuDrawer<MaterialBase.TextureData>
    {
        private String name;
        private MaterialBase.TextureData data;
        private Boolean showSelection;
        private ScrollableEnumSelection<Texture2DIndex> textureSelection;
        private NamedVector2TextEntry tilingEntry;
        private NamedVector2TextEntry offsetEntry;
        private Texture startTexture;
        private Vector2 scroll;

        internal StandardTextureHiddenSelection( MaterialBase.TextureData data, String name, Boolean scaleOffset )
        {
            this.name = name;
            this.data = data;
            this.showSelection = false;
            this.startTexture = this.data.texture;
            this.textureSelection = new ScrollableEnumSelection<Texture2DIndex>( Texture2DIndex.None, "Texture", ( texInd ) =>
            {
                Texture tex = null;
                if( texInd == Texture2DIndex.None )
                {
                    tex = this.startTexture;
                } else
                {
                    tex = AssetsCore.LoadAsset<Texture2D>( texInd );
                }
                this.data.texture = tex;
            } );

            if( scaleOffset && data is MaterialBase.ScaleOffsetTextureData )
            {
                var soData = data as MaterialBase.ScaleOffsetTextureData;
                var curScale = soData.tiling;
                var curOffset = soData.offset;
                this.tilingEntry = new NamedVector2TextEntry( curScale, "Tiling", ( vec ) => soData.tiling = vec );
                this.offsetEntry = new NamedVector2TextEntry( curOffset, "Offset", ( vec ) => soData.offset = vec );
            }
        }
        public void ChangeValue( MaterialBase.TextureData newValue )
        {
            this.data = newValue;
        }
        public void ChangeName( String name )
        {
            this.name = name;
        }


        public void Draw()
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label( this.name, GUILayout.Width( this.name.Length * Settings.widthPerChar ) );
                GUILayout.FlexibleSpace();

                var texName = this.data.texture?.name;
                if( String.IsNullOrEmpty( texName ) ) texName = "No Texture";
                GUILayout.Label( texName, GUILayout.Width( texName.Length * Settings.widthPerChar ) );
                GUILayout.Space( Settings.defaultMinSpace );
                this.showSelection = GUILayout.Toggle( this.showSelection, "Show Selection", GUILayout.Width( 14 * Settings.widthPerChar ) );
            }
            GUILayout.EndHorizontal();

            if( this.showSelection )
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.BeginVertical( GUILayout.Height( 300f ) );
                    {
                        GUILayout.BeginHorizontal( GUILayout.Width( 300f ) );
                        {
                            this.textureSelection.Draw();
                        }
                        GUILayout.EndHorizontal();
                    }
                    GUILayout.EndVertical();

                    GUILayout.BeginVertical();
                    {
                        this.tilingEntry?.Draw();
                        this.offsetEntry?.Draw();

                        this.scroll = GUILayout.BeginScrollView( this.scroll );
                        {
                            GUILayout.Box( this.data.texture );
                        }
                        GUILayout.EndScrollView();
                    }
                    GUILayout.EndVertical();
                }
                GUILayout.EndHorizontal();
            }
        }

        public void ChangeAction( Action<MaterialBase.TextureData> onChanged ) { }
    }
}
#endif