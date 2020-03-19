#if MATEDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using RogueWispPlugin.Helpers.IMGUI;
using ReinCore;

namespace RogueWispPlugin.Helpers
{
    internal class RampTextureHiddenSelection : IGUIMenuDrawer<MaterialBase.TextureData>
    {
        private String name;
        private MaterialBase.TextureData data;
        private Boolean showSelection;
        private NamedVector2TextEntry tilingEntry;
        private NamedVector2TextEntry offsetEntry;
        private Texture startTexture;
        private GradientEditor gradEditor;
        private Gradient backingGradient;
        private Texture curTexture;
        private Texture selTexture;
        private Boolean pendingChanges;
        private Int32 select;
        private String[] selectionStrings;

        private Boolean useOrig
        {
            get
            {
                return (this.selTexture == this.startTexture);
            }
            set
            {
                var desiredTex = (value ? this.startTexture : this.curTexture );
                if( this.selTexture != desiredTex )
                {
                    this.selTexture = desiredTex;
                    this.data.texture = this.selTexture;
                }
            }
        }

        internal RampTextureHiddenSelection( MaterialBase.TextureData data, String name )
        {
            this.name = name;
            this.data = data;
            this.showSelection = false;
            this.startTexture = this.data.texture;
            this.curTexture = new Texture2D( 256, 16, TextureFormat.RGBAFloat, false );
            this.curTexture.wrapMode = TextureWrapMode.Clamp;
            this.selTexture = this.startTexture;
            this.select = 0;
            this.selectionStrings = new String[]
            {
                "Original",
                "Generated"
            };

            this.pendingChanges = false;

            this.backingGradient = new Gradient();
            this.backingGradient.mode = GradientMode.Blend;
            this.gradEditor = new GradientEditor( this.backingGradient, ( val ) =>
            {
                this.pendingChanges = true;
            } );

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
                if( String.IsNullOrEmpty( texName ) ) if( this.data.texture == null ) texName = "No texture"; else texName = "Unnamed texture";

                GUILayout.Label( texName, GUILayout.Width( texName.Length * Settings.widthPerChar ) );
                GUILayout.Space( Settings.defaultMinSpace );
                this.showSelection = GUILayout.Toggle( this.showSelection, "Show Selection", GUILayout.Width( 14 * Settings.widthPerChar ) );
            }
            GUILayout.EndHorizontal();

            if( this.showSelection )
            {
                GUILayout.BeginHorizontal();
                {
                    var selection = GUILayout.Toolbar( this.select, this.selectionStrings );
                    if( GUI.changed && selection != this.select )
                    {
                        this.select = selection;
                        this.useOrig = this.select == 0;
                    }
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                {
                    GUILayout.Box( this.selTexture );
                    GUILayout.FlexibleSpace();
                    var changesText = (this.pendingChanges ? "Pending Changes" : "No Pending Changes" );
                    GUILayout.Label( changesText, GUILayout.Width( 20f * Settings.widthPerChar ) );
                    GUILayout.Space( Settings.defaultMinSpace );

                    if( GUILayout.Button( "Apply", GUILayout.Width( 6f * Settings.widthPerChar ) ) && this.pendingChanges )
                    {
                        this.ApplyChanges();
                        this.pendingChanges = false;
                    }
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                {
                    this.gradEditor.Draw();
                }
                GUILayout.EndHorizontal();
            }
        }

        private void ApplyChanges()
        {
            // TODO: Convert ramp texture gen to jobs

            var tex = this.curTexture as Texture2D;
            if( tex == null )
            {
                Main.LogE( "Texture was not a Texture2D, aborting change application" );
                return;
            }

            TextureGenerator.ApplyRampTexture( tex, this.backingGradient );
        }

        public void ChangeAction( Action<MaterialBase.TextureData> onChanged ) { }
    }

}
#endif