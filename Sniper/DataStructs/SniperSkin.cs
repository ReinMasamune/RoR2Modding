namespace Rein.Sniper.Data
{
    using System;
    using System.Collections.Generic;

    using ReinCore;

    using RoR2;

    using Rein.Sniper.Modules;

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


        internal SniperSkin( String name, TextureSet sniper, TextureSet rail, TextureSet throwKnife, TextureSet knife, ITextureJob knifeTrail, Boolean isTrash = false )
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
            this.isTrash = isTrash;
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
            var rampTex = this.knifeTrail.OutputTextureAndDispose();
            using( var skin = Skin.Create( model ) )
            {
                var snipermeshInd = -1;
                var throwkinfeInd = -1;
                var knifeInd = -1;
                var railgunInd = -1;
                var gaussgunInd = -1;
                var ammomeshInd = -1;
                var armormeshInd = -1;
                var bodymeshInd = -1;
                var cloakmeshInd = -1;
                var emismeshInd = -1;
                var classicRifleInd = -1;
                var classicMeshInd = -1;

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
                            snipermeshInd = i;
                            break;

                        case "ThrowKnife":
                            info.defaultMaterial = this.throwknifeMat;
                            throwkinfeInd = i;
                            break;

                        case "Knife":
                            info.defaultMaterial = this.isTrash ? null : this.knifeMat;
                            knifeInd = i;
                            break;

                        case "RailGun":
                            info.defaultMaterial = this.isTrash ? null : this.railMat;
                            railgunInd = i;
                            break;

                        case "GaussGun":
                            info.defaultMaterial = null;
                            gaussgunInd = i;
                            break;

                        case "AmmoMesh":
                            info.defaultMaterial = this.isTrash ? null : this.ammoMat;
                            ammomeshInd = i;
                            break;

                        case "ArmorMesh":
                            info.defaultMaterial = this.isTrash ? null : this.armorMat;
                            armormeshInd = i;
                            break;

                        case "BodyMesh":
                            info.defaultMaterial = this.isTrash ? null : this.bodyMat;
                            bodymeshInd = i;
                            break;

                        case "CloakMesh":
                            info.defaultMaterial = this.isTrash ? null : this.cloakMat;
                            cloakmeshInd = i;
                            break;

                        case "EmissionMesh":
                            info.defaultMaterial = this.isTrash ? null : this.emisMat;
                            emismeshInd = i;
                            break;
                        case "ClassicRifle":
                            info.defaultMaterial = !this.isTrash ? null : this.railMat;
                            classicRifleInd = i;
                            break;
                        case "ClassicMesh":
                            info.defaultMaterial = !this.isTrash ? null : this.armorMat;
                            classicMeshInd = i;
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
                        projectileGhostReplacementPrefab = ProjectileGhostModule.GetKnifeGhost( this.throwknifeMat, MaterialModule.GetKnifeTrailMaterial( rampTex ).material ),
                    },
                };

                if(this.isTrash)
                {
                    skin.gameObjectActivations = new[]
                    {
                        new SkinDef.GameObjectActivation
                        {
                            shouldActivate = false,
                            gameObject = rendInfos[armormeshInd].renderer.gameObject,
                        },
                        new SkinDef.GameObjectActivation
                        {
                            shouldActivate = false,
                            gameObject = rendInfos[ammomeshInd].renderer.gameObject,
                        },
                        new SkinDef.GameObjectActivation
                        {
                            shouldActivate = false,
                            gameObject = rendInfos[bodymeshInd].renderer.gameObject,
                        },
                        new SkinDef.GameObjectActivation
                        {
                            shouldActivate = false,
                            gameObject = rendInfos[cloakmeshInd].renderer.gameObject,
                        },
                        new SkinDef.GameObjectActivation
                        {
                            shouldActivate = false,
                            gameObject = rendInfos[emismeshInd].renderer.gameObject,
                        },
                        new SkinDef.GameObjectActivation
                        {
                            shouldActivate = false,
                            gameObject = rendInfos[railgunInd].renderer.gameObject,
                        },
                        new SkinDef.GameObjectActivation
                        {
                            shouldActivate = false,
                            gameObject = rendInfos[knifeInd].renderer.gameObject,
                        },
                        new SkinDef.GameObjectActivation
                        {
                            shouldActivate = true,
                            gameObject = rendInfos[classicMeshInd].renderer.gameObject,
                        },
                        new SkinDef.GameObjectActivation
                        {
                            shouldActivate = true,
                            gameObject = rendInfos[classicRifleInd].renderer.gameObject,
                        }
                    };
                } else
                {
                    skin.gameObjectActivations = new[]
                    {
                        new SkinDef.GameObjectActivation
                        {
                            shouldActivate = true,
                            gameObject = rendInfos[armormeshInd].renderer.gameObject,
                        },
                        new SkinDef.GameObjectActivation
                        {
                            shouldActivate = true,
                            gameObject = rendInfos[ammomeshInd].renderer.gameObject,
                        },
                        new SkinDef.GameObjectActivation
                        {
                            shouldActivate = true,
                            gameObject = rendInfos[bodymeshInd].renderer.gameObject,
                        },
                        new SkinDef.GameObjectActivation
                        {
                            shouldActivate = true,
                            gameObject = rendInfos[cloakmeshInd].renderer.gameObject,
                        },
                        new SkinDef.GameObjectActivation
                        {
                            shouldActivate = true,
                            gameObject = rendInfos[emismeshInd].renderer.gameObject,
                        },
                        new SkinDef.GameObjectActivation
                        {
                            shouldActivate = true,
                            gameObject = rendInfos[railgunInd].renderer.gameObject,
                        },
                        new SkinDef.GameObjectActivation
                        {
                            shouldActivate = true,
                            gameObject = rendInfos[knifeInd].renderer.gameObject,
                        },
                        new SkinDef.GameObjectActivation
                        {
                            shouldActivate = false,
                            gameObject = rendInfos[classicMeshInd].renderer.gameObject,
                        },
                        new SkinDef.GameObjectActivation
                        {
                            shouldActivate = false,
                            gameObject = rendInfos[classicRifleInd].renderer.gameObject,
                        }
                    };
                }

                skin.icon = icon;
                skin.nameToken = nameToken;
                skin.unlockableName = unlockableName;
            }

            var index = model.GetComponent<ModelSkinController>().skins.Length - 1;
            var material = MaterialModule.GetSlashMaterial(rampTex);

            VFXModule.AddKnifePickupSlash((UInt32)index, material);

        }

        private readonly String name;

        private readonly Dictionary<SniperMaterial, List<MaterialModifier>> materialModifiers;

        private readonly TextureSet sniperTextures;
        private readonly TextureSet knifeTextures;
        private readonly TextureSet throwKnifeTextures;
        private readonly TextureSet railTextures;
        private readonly ITextureJob knifeTrail;
        private readonly Boolean isTrash;

        private Material _bodyMat;
        private Material bodyMat => this._bodyMat ??= GetBodyMaterial();
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


        private Material _armorMat;
        private Material armorMat => this._armorMat ??= GetArmorMaterial();
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

        private Material _cloakMat;
        private Material cloakMat => this._cloakMat ??= GetCloakMaterial();
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

        private Material _ammoMat;
        private Material ammoMat => this._ammoMat ??= GetAmmoMaterial();
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

        private Material _emisMat;
        private Material emisMat => this._emisMat ??= GetEmissiveMaterial();
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

        private Material _railMat;
        private Material railMat => this._railMat ??= GetRailMaterial();
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


        private Material _throwknifeMat;
        private Material throwknifeMat => this._throwknifeMat ??= GetThrowKnifeMaterial();
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


        private Material _knifeMat;
        private Material knifeMat => this._knifeMat ??= GetKnifeMaterial();
        private Material GetKnifeMaterial()
        {
            // FUTURE: Knife base material
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
