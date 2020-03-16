
using RoR2;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RogueWispPlugin.Helpers
{
    internal class WispModelBitSkinController : BitSkinController
    {
        internal Material activeFlameMaterial;
        internal Material activeArmorMaterial;
        internal Material activeTracerMaterial;
        internal Material activeFlamePillarMaterial;
        internal Material activeAreaIndicatorMaterial;
        internal Material activeExplosionMaterial;
        internal Material activeBeamMaterial;
        internal Material activeDistortionLightMaterial;
        internal Material activeDistortionMaterial;
        internal Material activeDistortionHeavyMaterial;
        internal Material activeArcaneCircleMaterial;
        internal Material activeFlameTornadoMaterial;
        internal Material activeBossAreaIndicatorMaterial;
        internal Material activeBossAreaExplosionMaterial;
        internal Color activeLightColor;

        private CharacterModel modelRef;
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
                if( value != this._skin || this._skin is null )
                {
                    this._skin = value;
                    this.OnSkinChanged( value );
                }
            }
        }
        private WispBitSkin _skin;


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
            //Main.LogI( String.Format( "Skin changed\n{0}\n{1}\n{2}", oldCode, newCode, appliedCode ) );
        }

        private void OnSkinChanged( WispBitSkin newSkin )
        {
            this.activeFlameMaterial = newSkin.flameMainMaterial;
            this.activeArmorMaterial = newSkin.armorMainMaterial;
            this.activeTracerMaterial = newSkin.tracerMaterial;
            this.activeFlamePillarMaterial = newSkin.flamePillarMaterial;
            this.activeAreaIndicatorMaterial = newSkin.areaIndicatorMaterial;
            this.activeExplosionMaterial = newSkin.explosionMaterial;
            this.activeBeamMaterial = newSkin.beamMaterial;
            this.activeDistortionLightMaterial = newSkin.distortionLightMaterial;
            this.activeDistortionMaterial = newSkin.distortionMaterial;
            this.activeDistortionHeavyMaterial = newSkin.distortionHeavyMaterial;
            this.activeArcaneCircleMaterial = newSkin.arcaneCircleMaterial;
            this.activeFlameTornadoMaterial = newSkin.flameTornadoMaterial;
            this.activeBossAreaIndicatorMaterial = newSkin.bossAreaIndicatorMaterial;
            this.activeBossAreaExplosionMaterial = newSkin.bossExplosionAreaMaterial;
            this.activeLightColor = newSkin.mainColor;
            this.ApplyMaterials();
        }

        private void Awake()
        {
            this.modelRef = base.GetComponent<CharacterModel>();
            if( this.modelRef.body != null )
            {
                this.Apply(WispBitSkin.GetWispSkin( this.modelRef.body.skinIndex ));
            } else
            {
                this.Apply( WispBitSkin.GetWispSkin( 0u ) );
            }
        }

        private void Start()
        {
            this.modelRef = base.GetComponent<CharacterModel>();
            if( this.modelRef.body != null )
            {
                this.Apply( WispBitSkin.GetWispSkin( this.modelRef.body.skinIndex ) );
            }
        }

        private void ApplyMaterials()
        {
            for( Int32 i = 0; i < this.modelRef.baseRendererInfos.Length; ++i )
            {
                this.modelRef.baseRendererInfos[i].defaultMaterial = this.activeArmorMaterial;
            }
            for( Int32 i = 0; i < this.modelRef.baseLightInfos.Length; ++i )
            {
                this.modelRef.baseLightInfos[i].defaultColor = this.activeLightColor;
            }
            for( Int32 i = 0; i < this.modelRef.baseParticleSystemInfos.Length; ++i )
            {
                this.modelRef.baseParticleSystemInfos[i].defaultMaterial = this.activeFlameMaterial;
                this.modelRef.baseParticleSystemInfos[i].renderer.material = this.activeFlameMaterial;
            }
        }
    }
}
