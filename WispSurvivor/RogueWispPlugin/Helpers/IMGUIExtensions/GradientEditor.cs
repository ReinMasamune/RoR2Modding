#if MATEDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Rein.RogueWispPlugin.Helpers.IMGUI;

namespace Rein.RogueWispPlugin.Helpers
{
    
    internal class GradientEditor
    {
        private Gradient grad;
        private Action<Gradient> onChanged;
        private Int32 curSelectedMode;
        private String[] modeNames;
        private Mode currentMode;
        private Boolean shouldRebuild;
        private ColorKeyRepresentation[] colorKeys = new ColorKeyRepresentation[8];
        private AlphaKeyRepresentation[] alphaKeys = new AlphaKeyRepresentation[8];
        private List<GradientColorKey> tempCKeys = new List<GradientColorKey>(8);
        private List<GradientAlphaKey> tempAKeys = new List<GradientAlphaKey>(8);



        internal GradientEditor( Gradient inValue, Action<Gradient> onChanged )
        {
            this.grad = inValue;
            this.onChanged = onChanged;
            this.shouldRebuild = false;
            this.modeNames = new String[]
            {
                "Color",
                "Alpha"
            };
            this.currentMode = Mode.ColorKeys;
            this.curSelectedMode = 0;
            var inCKeys = inValue.colorKeys;
            var inAKeys = inValue.alphaKeys;

            for( Int32 i = 0; i < 8; ++i )
            {
                if( i < inCKeys.Length )
                {
                    this.colorKeys[i] = new ColorKeyRepresentation( inCKeys[i] );
                } else
                {
                    this.colorKeys[i] = new ColorKeyRepresentation();
                }

                if( i < inAKeys.Length )
                {
                    this.alphaKeys[i] = new AlphaKeyRepresentation( inAKeys[i] );
                } else
                {
                    this.alphaKeys[i] = new AlphaKeyRepresentation();
                }
            }
        }

        internal void Draw()
        {
            GUILayout.BeginVertical();
            {
                var selected = GUILayout.Toolbar( this.curSelectedMode, this.modeNames );
                if( GUI.changed && selected != this.curSelectedMode )
                {
                    this.curSelectedMode = selected;
                    this.currentMode = (Mode)this.curSelectedMode;
                }

                if( this.currentMode == Mode.ColorKeys )
                {
                    foreach( var color in this.colorKeys )
                    {
                        if( color.Draw() )
                        {
                            this.shouldRebuild = true;
                        }
                    }
                } else if( this.currentMode == Mode.AlphaKeys )
                {
                    foreach( var alpha in this.alphaKeys )
                    {
                        if( alpha.Draw() )
                        {
                            this.shouldRebuild = true;
                        }
                    }
                }
            }
            GUILayout.EndVertical();

            if( this.shouldRebuild )
            {
                this.shouldRebuild = false;

                this.tempAKeys.Clear();
                this.tempCKeys.Clear();

                for( Int32 i = 0; i < 8; ++i )
                {
                    var color = this.colorKeys[i];
                    var alpha = this.alphaKeys[i];

                    if( color.isEnabled ) this.tempCKeys.Add( new GradientColorKey( color.value, color.time ) );
                    if( alpha.isEnabled ) this.tempAKeys.Add( new GradientAlphaKey( alpha.value, alpha.time ) );
                }

                this.grad.colorKeys = this.tempCKeys.ToArray();
                this.grad.alphaKeys = this.tempAKeys.ToArray();

                this.onChanged.Invoke( this.grad );
            }
        }


        private enum Mode
        {
            ColorKeys = 0,
            AlphaKeys = 1
        }

        private class ColorKeyRepresentation
        {
            internal Boolean isEnabled { get; private set; }
            internal Single time { get; private set; }
            internal Color value { get; private set; }

            private Boolean changed;
            private NamedCheckbox toggleBox;
            private RangedSingleSlider timeSlider;
            private NamedColorTextEntry colorEntry;


            internal ColorKeyRepresentation( GradientColorKey key )
            {
                this.isEnabled = true;
                this.time = key.time;
                this.value = key.color;
                this.changed = false;

                this.CreateElements();
            }
            internal ColorKeyRepresentation()
            {
                this.isEnabled = false;
                this.time = 0f;
                this.value = Color.black;
                this.changed = false;

                this.CreateElements();
            }
            internal Boolean Draw()
            {
                this.changed = false;
                GUILayout.BeginHorizontal();
                {
                    this.toggleBox.Draw();
                    this.timeSlider.Draw();
                    this.colorEntry.Draw();
                }
                GUILayout.EndHorizontal();
                return this.changed;
            }
            private void CreateElements()
            {
                this.toggleBox = new NamedCheckbox( this.isEnabled, null, ( val ) =>
                {
                    this.changed = true;
                    this.isEnabled = val;
                } );

                this.timeSlider = new RangedSingleSlider( this.time, 0f, 1f, "time", ( val ) =>
                {
                    this.changed = true;
                    this.time = val;
                } );

                this.colorEntry = new NamedColorTextEntry( this.value, null, ( val ) =>
                {
                    this.changed = true;
                    this.value = val;
                } );
            }
        }
        private class AlphaKeyRepresentation
        {
            internal Boolean isEnabled { get; private set; }
            internal Single time { get; private set; }
            internal Single value { get; private set; }

            private Boolean changed;
            private NamedCheckbox toggleBox;
            private RangedSingleSlider timeSlider;
            private RangedSingleSlider alphaSlider;

            internal AlphaKeyRepresentation( GradientAlphaKey key )
            {
                this.isEnabled = true;
                this.time = key.time;
                this.value = key.alpha;
                this.changed = false;

                this.CreateElements();
            }
            internal AlphaKeyRepresentation()
            {
                this.isEnabled = false;
                this.time = 0f;
                this.value = 0f;
                this.changed = false;

                this.CreateElements();
            }
            internal Boolean Draw()
            {
                this.changed = false;
                GUILayout.BeginHorizontal();
                {
                    this.toggleBox.Draw();
                    this.timeSlider.Draw();
                    GUILayout.Space( 4f );
                    this.alphaSlider.Draw();
                }
                GUILayout.EndHorizontal();
                return this.changed;
            }
            private void CreateElements()
            {
                this.toggleBox = new NamedCheckbox( this.isEnabled, null, ( val ) =>
                {
                    this.changed = true;
                    this.isEnabled = val;
                } );

                this.timeSlider = new RangedSingleSlider( this.time, 0f, 1f, "time", ( val ) =>
                {
                    this.changed = true;
                    this.time = val;
                } );

                this.alphaSlider = new RangedSingleSlider( this.value, 0f, 1f, "alpha", ( val ) =>
                {
                    this.changed = true;
                    this.value = val;
                } );
            }
        }
    }
    
}
#endif