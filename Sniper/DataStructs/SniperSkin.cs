using System;
using System.Collections.Generic;
using System.Text;
using RoR2;
using ReinCore;
using UnityEngine;
using Sniper.Modules;

namespace Sniper.Data
{
    internal class SniperSkin
    {
        [Flags]
        internal enum SniperMaterial
        {
            Body = 1,
            Armor = 2,
            Cloak = 4,
            Ammo = 8,
            Emissive = 16,
            Rail = 32,
            ThrowKnife = 64,
            Knife = 128,
            All = 255,
        }

        internal delegate void MaterialModifier( StandardMaterial material );


        internal SniperSkin( String name, TextureSet sniper, TextureSet rail, TextureSet throwKnife, TextureSet knife )
        {
            this.name = name;
            this.sniperTextures = sniper;
            this.railTextures = rail;
            this.throwKnifeTextures = throwKnife;
            this.knifeTextures = knife;

            this.materialModifiers = new Dictionary<SniperMaterial, List<MaterialModifier>>
            {
                [SniperMaterial.Ammo] = new List<MaterialModifier>(),
                [SniperMaterial.Armor] = new List<MaterialModifier>(),
                [SniperMaterial.Body] = new List<MaterialModifier>(),
                [SniperMaterial.Cloak] = new List<MaterialModifier>(),
                [SniperMaterial.Emissive] = new List<MaterialModifier>(),
                [SniperMaterial.Knife] = new List<MaterialModifier>(),
                [SniperMaterial.Rail] = new List<MaterialModifier>(),
                [SniperMaterial.ThrowKnife] = new List<MaterialModifier>()
            };
        }

        internal void AddMaterialModifier( SniperMaterial materialMask, MaterialModifier modifier )
        {
            foreach( KeyValuePair<SniperMaterial, List<MaterialModifier>> kv in this.materialModifiers ) if( materialMask.Flag( kv.Key ) ) kv.Value.Add( modifier );
        }







        internal void CreateAndAddSkin( CharacterModel model, String nameToken, String unlockableName, Sprite icon )
        {
            using( var skin = Skin.Create( model ) )
            {
                var rendInfos = skin.rendererInfos;         
                for( Int32 i = 0; i < rendInfos.Length; ++i )
                {
                    var info = rendInfos[i];
                    var smr = info.renderer;
                    switch( smr.name )
                    {
                        default:
                        Log.Warning( String.Format( "{0} is not a handled renderer name", smr.name ) );
                        info.defaultMaterial = null;
                        break;

                        case "SniperMesh":
                        info.defaultMaterial = null;
                        break;

                        case "ThrowKnife":
                        info.defaultMaterial = this.GetThrowKnifeMaterial();
                        break;

                        case "Knife":
                        info.defaultMaterial = this.GetKnifeMaterial();
                        break;

                        case "RailGun":
                        info.defaultMaterial = this.GetRailMaterial();
                        break;

                        case "GaussGun":
                        info.defaultMaterial = null;
                        break;

                        case "AmmoMesh":
                        info.defaultMaterial = this.GetAmmoMaterial();
                        break;

                        case "ArmorMesh":
                        info.defaultMaterial = this.GetArmorMaterial();
                        break;

                        case "BodyMesh":
                        info.defaultMaterial = this.GetBodyMaterial();
                        break;

                        case "CloakMesh":
                        info.defaultMaterial = this.GetCloakMaterial();
                        break;

                        case "EmissionMesh":
                        info.defaultMaterial = this.GetEmissiveMaterial();
                        break;
                    }
                    rendInfos[i] = info;
                }
                skin.rendererInfos = rendInfos;


                skin.icon = icon;
                skin.nameToken = nameToken;
                skin.unlockableName = unlockableName;
            }
        }

        private readonly String name;

        private readonly Dictionary<SniperMaterial, List<MaterialModifier>> materialModifiers;

        private readonly TextureSet sniperTextures;
        private readonly TextureSet knifeTextures;
        private readonly TextureSet throwKnifeTextures;
        private readonly TextureSet railTextures;


        private Material GetBodyMaterial()
        {
            var mat = MaterialModule.GetSniperBase().Clone();
            this.sniperTextures.Apply( mat );
            foreach( var mod in this.materialModifiers[SniperMaterial.Body] ) mod( mat );

            SniperMain.AddMaterial( mat, String.Format( "{0} {1}", name, "Body" ) );
            return mat.material;
        }
        private Material GetArmorMaterial()
        {
            var mat = MaterialModule.GetSniperBase().Clone();
            this.sniperTextures.Apply( mat );
            foreach( var mod in this.materialModifiers[SniperMaterial.Armor] ) mod( mat );

            SniperMain.AddMaterial( mat, String.Format( "{0} {1}", name, "Armor" ) );
            return mat.material;
        }
        private Material GetCloakMaterial()
        {
            var mat = MaterialModule.GetSniperBase().Clone();
            this.sniperTextures.Apply( mat );
            foreach( var mod in this.materialModifiers[SniperMaterial.Cloak] ) mod( mat );

            SniperMain.AddMaterial( mat, String.Format( "{0} {1}", name, "Cloak" ) );
            return mat.material;
        }
        private Material GetAmmoMaterial()
        {
            var mat = MaterialModule.GetSniperBase().Clone();
            this.sniperTextures.Apply( mat );
            foreach( var mod in this.materialModifiers[SniperMaterial.Ammo] ) mod( mat );

            SniperMain.AddMaterial( mat, String.Format( "{0} {1}", name, "Ammo" ) );
            return mat.material;
        }
        private Material GetEmissiveMaterial()
        {
            var mat = MaterialModule.GetSniperBase().Clone();
            this.sniperTextures.Apply( mat );
            foreach( var mod in this.materialModifiers[SniperMaterial.Emissive] ) mod( mat );

            SniperMain.AddMaterial( mat, String.Format( "{0} {1}", name, "Emissive" ) );
            return mat.material;
        }
        private Material GetRailMaterial()
        {
            var mat = MaterialModule.GetRailBase().Clone();
            this.railTextures.Apply( mat );
            foreach( var mod in this.materialModifiers[SniperMaterial.Rail] ) mod( mat );

            SniperMain.AddMaterial( mat, String.Format( "{0} {1}", name, "Railgun" ) );
            return mat.material;
        }
        private Material GetThrowKnifeMaterial()
        {
            var mat = MaterialModule.GetThrowKnifeBase().Clone();
            this.throwKnifeTextures.Apply( mat );
            foreach( var mod in this.materialModifiers[SniperMaterial.ThrowKnife] ) mod( mat );

            SniperMain.AddMaterial( mat, String.Format( "{0} {1}", name, "ThrowKnife" ) );
            return mat.material;
        }
        private Material GetKnifeMaterial()
        {
            // TODO: GetKnifeBase
            var mat = MaterialModule.GetSniperBase().Clone();
            this.knifeTextures.Apply( mat );
            foreach( var mod in this.materialModifiers[SniperMaterial.Knife] ) mod( mat );

            SniperMain.AddMaterial( mat, String.Format( "{0} {1}", name, "Knife" ) );
            return mat.material;
        }
    }
}
