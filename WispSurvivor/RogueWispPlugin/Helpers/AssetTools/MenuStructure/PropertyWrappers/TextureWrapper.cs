#if MATEDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace RogueWispPlugin.Helpers
{
    internal class TextureWrapper<TMenu> : PropertyWrapper<TMenu>
    {
        private Dictionary<TMenu, TextureContext> contextLookup = new Dictionary<TMenu,TextureContext>();
        private Func<TMenu,MaterialBase.TextureData> getter;
        private MenuAttribute settings;
        private String name;
        private TMenu lastInstance;
        private TextureContext lastContext;

        internal TextureWrapper( PropertyInfo property, MenuAttribute settings )
        {
            var getMethod = property.GetGetMethod(true);
            if( getMethod == null )
            {
                throw new ArgumentException( property.Name + " is missing get method" );
            }
            this.getter = base.CompileGetter<MaterialBase.TextureData>( getMethod );
            this.settings = settings;
            if( String.IsNullOrEmpty( this.settings.name ) )
            {
                this.name = property.Name;
            } else
            {
                this.name = this.settings.name;
            }
        }

        internal override void Draw( TMenu instance )
        {
            if( this.lastInstance == null || this.lastContext == null || !this.lastInstance.Equals( instance ) )
            {
                this.lastContext = this.GetContext( instance );
            }

            this.lastContext.Draw();
        }

        private TextureContext GetContext( TMenu instance )
        {
            if( !this.contextLookup.ContainsKey(instance) || this.contextLookup[instance] == null )
            {
                this.contextLookup[instance] = this.ChooseContext(instance);
            }

            return this.contextLookup[instance];
        }

        private TextureContext ChooseContext( TMenu instance )
        {
            var inValue = this.getter(instance);
            if( this.settings.isRampTexture )
            {
                return new RampHiddenSelection( inValue, this.name );
            } else
            {
                return new StandardHiddenSelection( inValue, this.name, inValue is MaterialBase.ScaleOffsetTextureData );
            }
        }



        private abstract class TextureContext
        {
            internal abstract void Draw();
        }

        private class StandardHiddenSelection : TextureContext
        {
            private StandardTextureHiddenSelection elem;
            internal StandardHiddenSelection( MaterialBase.TextureData data, String name, Boolean scaleOffset ) 
            {
                this.elem = new StandardTextureHiddenSelection( data, name, scaleOffset );
            }

            internal override void Draw()
            {
                this.elem.Draw();
            }
        }

        private class RampHiddenSelection : TextureContext
        {
            private RampTextureHiddenSelection elem;
            internal RampHiddenSelection( MaterialBase.TextureData data, String name )
            {
                this.elem = new RampTextureHiddenSelection( data, name );
            }

            internal override void Draw()
            {
                this.elem.Draw();
            }
        }
    }
}
#endif