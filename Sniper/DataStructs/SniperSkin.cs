namespace Sniper.Data
{
    using System;
    using System.Collections.Generic;

    using ReinCore;

    using RoR2;

    using Sniper.Modules;

    using UnityEngine;

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


        internal SniperSkin( String name, TextureSet sniper, TextureSet rail, TextureSet throwKnife, TextureSet knife, ITextureJob knifeTrail )
        {
            this.name = name;
            this.sniperTextures = sniper;
            this.railTextures = rail;
            this.throwKnifeTextures = throwKnife;
            this.knifeTextures = knife;
            this.knifeTrail = knifeTrail;

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
            foreach( KeyValuePair<SniperMaterial, List<MaterialModifier>> kv in this.materialModifiers )
            {
                if( materialMask.Flag( kv.Key ) )
                {
                    kv.Value.Add( modifier );
                }
            }
        }







        internal void CreateAndAddSkin( CharacterModel model, String nameToken, String unlockableName, Sprite icon )
        {
            using( var skin = Skin.Create( model ) )
            {
                CharacterModel.RendererInfo[] rendInfos = skin.rendererInfos;
                for( Int32 i = 0; i < rendInfos.Length; ++i )
                {
                    CharacterModel.RendererInfo info = rendInfos[i];
                    Renderer smr = info.renderer;
                    switch( smr.name )
                    {
                        default:
#if ASSERT
                        Log.Warning( String.Format( "{0} is not a handled renderer name", smr.name ) );
#endif
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

                skin.projectileGhostReplacements = new[]
                {
                    new SkinDef.ProjectileGhostReplacement
                    {
                        projectilePrefab = ProjectileModule.GetKnifeProjectile(),
                        projectileGhostReplacementPrefab = ProjectileGhostModule.GetKnifeGhost( this.GetThrowKnifeMaterial(), this.knifeTrail ),
                    },
                };


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
        private readonly ITextureJob knifeTrail;


        private Material GetBodyMaterial()
        {
            StandardMaterial mat = MaterialModule.CreateSniperBase().Clone();
            this.sniperTextures.Apply( mat );
            foreach( MaterialModifier mod in this.materialModifiers[SniperMaterial.Body] )
            {
                mod( mat );
            }

            SniperMain.AddMaterial( mat, String.Format( "{0} {1}", this.name, "Body" ) );
            return mat.material;
        }
        private Material GetArmorMaterial()
        {
            StandardMaterial mat = MaterialModule.CreateSniperBase().Clone();
            this.sniperTextures.Apply( mat );
            foreach( MaterialModifier mod in this.materialModifiers[SniperMaterial.Armor] )
            {
                mod( mat );
            }

            SniperMain.AddMaterial( mat, String.Format( "{0} {1}", this.name, "Armor" ) );
            return mat.material;
        }
        private Material GetCloakMaterial()
        {
            StandardMaterial mat = MaterialModule.CreateSniperBase().Clone();
            this.sniperTextures.Apply( mat );
            foreach( MaterialModifier mod in this.materialModifiers[SniperMaterial.Cloak] )
            {
                mod( mat );
            }

            SniperMain.AddMaterial( mat, String.Format( "{0} {1}", this.name, "Cloak" ) );
            return mat.material;
        }
        private Material GetAmmoMaterial()
        {
            StandardMaterial mat = MaterialModule.CreateSniperBase().Clone();
            this.sniperTextures.Apply( mat );
            foreach( MaterialModifier mod in this.materialModifiers[SniperMaterial.Ammo] )
            {
                mod( mat );
            }

            SniperMain.AddMaterial( mat, String.Format( "{0} {1}", this.name, "Ammo" ) );
            return mat.material;
        }
        private Material GetEmissiveMaterial()
        {
            StandardMaterial mat = MaterialModule.CreateSniperBase().Clone();
            this.sniperTextures.Apply( mat );
            foreach( MaterialModifier mod in this.materialModifiers[SniperMaterial.Emissive] )
            {
                mod( mat );
            }

            SniperMain.AddMaterial( mat, String.Format( "{0} {1}", this.name, "Emissive" ) );
            return mat.material;
        }
        private Material GetRailMaterial()
        {
            StandardMaterial mat = MaterialModule.GetRailBase().Clone();
            this.railTextures.Apply( mat );
            foreach( MaterialModifier mod in this.materialModifiers[SniperMaterial.Rail] )
            {
                mod( mat );
            }

            SniperMain.AddMaterial( mat, String.Format( "{0} {1}", this.name, "Railgun" ) );
            return mat.material;
        }
        private Material GetThrowKnifeMaterial()
        {
            StandardMaterial mat = MaterialModule.GetThrowKnifeBase().Clone();
            this.throwKnifeTextures.Apply( mat );
            foreach( MaterialModifier mod in this.materialModifiers[SniperMaterial.ThrowKnife] )
            {
                mod( mat );
            }

            SniperMain.AddMaterial( mat, String.Format( "{0} {1}", this.name, "ThrowKnife" ) );
            return mat.material;
        }
        private Material GetKnifeMaterial()
        {
            // TODO: Knife base material
            StandardMaterial mat = MaterialModule.CreateSniperBase().Clone();
            this.knifeTextures.Apply( mat );
            foreach( MaterialModifier mod in this.materialModifiers[SniperMaterial.Knife] )
            {
                mod( mat );
            }

            SniperMain.AddMaterial( mat, String.Format( "{0} {1}", this.name, "Knife" ) );
            return mat.material;
        }
    }
}
