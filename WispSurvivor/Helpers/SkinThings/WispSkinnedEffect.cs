
using RoR2;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RogueWispPlugin.Helpers
{
    internal class WispSkinnedEffect : BitSkinController
    {
        public Renderer[] flameRenderers = Array.Empty<Renderer>();
        public Renderer[] armorRenderers = Array.Empty<Renderer>();
        public Renderer[] tracerRenderers = Array.Empty<Renderer>();
        public Renderer[] flamePillarRenderers = Array.Empty<Renderer>();
        public Renderer[] areaIndicatorRenderers = Array.Empty<Renderer>();
        public Renderer[] explosionRenderers = Array.Empty<Renderer>();
        public Renderer[] beamRenderers = Array.Empty<Renderer>();
        public Light[] lights = Array.Empty<Light>();
        public TrailRenderer[] trails = Array.Empty<TrailRenderer>();

        private Material activeFlameMaterial;
        private Material activeArmorMaterial;
        private Material activeTracerMaterial;
        private Material activeFlamePillarMaterial;
        private Material activeAreaIndicatorMaterial;
        private Material activeExplosionMaterial;
        private Material activeBeamMaterial;
        private Color activeColor;
        private Gradient activeColorGradient;

        private WispBitSkin skin
        {
            get
            {
                if( this._skin == null )
                {
                    this._skin = WispBitSkin.GetWispSkin( 0u );
                    this.OnSkinChanged( this._skin );
                }
                return this._skin;
            }
            set
            {
                if( value != this.skin )
                {
                    this._skin = value;
                    this.OnSkinChanged( value );
                }
            }
        }
        private WispBitSkin _skin;

        internal void AddRenderer( Renderer renderer, MaterialType matType )
        {
            Int32 ind = 0;
            switch( matType )
            {
                default:
                    Main.LogE( "Unhandled MaterialType: " + matType.ToString() );
                    break;
                case MaterialType.Armor:
                    ind = this.armorRenderers.Length;
                    Array.Resize<Renderer>( ref this.armorRenderers, ind + 1 );
                    this.armorRenderers[ind] = renderer;
                    break;
                case MaterialType.Flames:
                    ind = this.flameRenderers.Length;
                    Array.Resize<Renderer>( ref this.flameRenderers, ind + 1 );
                    this.flameRenderers[ind] = renderer;
                    break;
                case MaterialType.Tracer:
                    ind = this.tracerRenderers.Length;
                    Array.Resize<Renderer>( ref this.tracerRenderers, ind + 1 );
                    this.tracerRenderers[ind] = renderer;
                    break;
                case MaterialType.FlamePillar:
                    ind = this.flamePillarRenderers.Length;
                    Array.Resize<Renderer>( ref this.flamePillarRenderers, ind + 1 );
                    this.flamePillarRenderers[ind] = renderer;
                    break;
                case MaterialType.AreaIndicator:
                    ind = this.areaIndicatorRenderers.Length;
                    Array.Resize<Renderer>( ref this.areaIndicatorRenderers, ind + 1 );
                    this.areaIndicatorRenderers[ind] = renderer;
                    break;
                case MaterialType.Explosion:
                    ind = this.explosionRenderers.Length;
                    Array.Resize<Renderer>( ref this.explosionRenderers, ind + 1 );
                    this.explosionRenderers[ind] = renderer;
                    break;
                case MaterialType.Beam:
                    ind = this.beamRenderers.Length;
                    Array.Resize<Renderer>( ref this.beamRenderers, ind + 1 );
                    this.beamRenderers[ind] = renderer;
                    break;
                    
            }
        }

        internal void AddLight( Light light )
        {
            var ind = this.lights.Length;
            Array.Resize<Light>( ref this.lights, ind + 1 );
            this.lights[ind] = light;
        }

        internal void AddTrail( TrailRenderer renderer )
        {
            var ind = this.trails.Length;
            Array.Resize<TrailRenderer>( ref this.trails, ind + 1 );
            this.trails[ind] = renderer;
        }

        internal override void Apply( IBitSkin skin )
        {
            if( !(skin is WispBitSkin) )
            {
                throw new ArgumentException( "Provided skin was not a WispBitSkin" );
            }
            var tempSkin = skin as WispBitSkin;
            var oldCode = Convert.ToString( this.skin.EncodeToSkinIndex(), 2).PadLeft( 32, '0' );
            var newCode = Convert.ToString( tempSkin.EncodeToSkinIndex(), 2).PadLeft( 32, '0' );
            this.skin = tempSkin;
            var appliedCode = Convert.ToString( this.skin.EncodeToSkinIndex(), 2).PadLeft( 32, '0' );
            Main.LogI( String.Format( "Skin changed\n{0}\n{1}\n{2}", oldCode, newCode, appliedCode ) );
        }

        private void OnSkinChanged( WispBitSkin newSkin )
        {
            this.activeArmorMaterial = newSkin.armorMainMaterial;
            this.activeFlameMaterial = newSkin.flameMainMaterial;
            this.activeTracerMaterial = newSkin.tracerMaterial;
            this.activeFlamePillarMaterial = newSkin.flamePillarMaterial;
            this.activeAreaIndicatorMaterial = newSkin.areaIndicatorMaterial;
            this.activeExplosionMaterial = newSkin.explosionMaterial;
            this.activeBeamMaterial = newSkin.beamMaterial;
            this.activeColor = newSkin.mainColor;
            this.activeColorGradient = newSkin.flameGradient;
            this.ApplyMaterials();
        }

        private void ApplyMaterials()
        {
            foreach( var ren in this.armorRenderers )
            {
                ren.material = this.activeArmorMaterial;
            }
            foreach( var ren in this.flameRenderers )
            {
                ren.material = this.activeFlameMaterial;
            }
            foreach( var ren in this.tracerRenderers )
            {
                ren.material = this.activeTracerMaterial;
            }
            foreach( var ren in this.flamePillarRenderers )
            {
                ren.material = this.activeFlamePillarMaterial;
            }
            foreach( var ren in this.areaIndicatorRenderers )
            {
                ren.material = this.activeAreaIndicatorMaterial;
            }
            foreach( var ren in this.explosionRenderers )
            {
                ren.material = this.activeExplosionMaterial;
            }
            foreach( var ren in this.beamRenderers )
            {
                ren.material = this.activeBeamMaterial;
            }


            foreach( var light in this.lights )
            {
                light.color = this.activeColor;
            }
            foreach( var trail in this.trails )
            {
                trail.colorGradient = this.activeColorGradient;
            }
        }
    }
}
