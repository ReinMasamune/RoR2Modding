
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
        internal Color activeLightColor;

        private CharacterModel modelRef;
        private WispBitSkin skin
        {
            get => this._skin;
            set
            {
                if( value != this._skin )
                {
                    this._skin = value;
                    this.OnSkinChanged( value );
                }
            }
        }
        private WispBitSkin _skin;


        internal override void Apply( IBitSkin skin )
        {
            Main.LogI( "Skin applied" );
            if( !(skin is WispBitSkin) )
            {
                throw new ArgumentException( "Provided skin was not a WispBitSkin" );
            }

            this.skin = (WispBitSkin)skin;
            Main.LogI( "Skin code: " + this.skin.EncodeToSkinIndex() );
        }

        private void OnSkinChanged( WispBitSkin newSkin )
        {
            this.activeFlameMaterial = newSkin.flameMainMaterial;
            this.activeArmorMaterial = newSkin.armorMainMaterial;
            this.activeLightColor = newSkin.mainColor;
            this.ApplyMaterials();
        }

        private void Awake()
        {
            this.modelRef = base.GetComponent<CharacterModel>();
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
            }
        }
    }
}
