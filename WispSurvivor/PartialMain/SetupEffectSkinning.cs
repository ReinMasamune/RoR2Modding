using System;
using System.Collections.Generic;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using R2API;
using RogueWispPlugin.Helpers;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace RogueWispPlugin
{
    internal partial class Main
    {
        private static HashSet<GameObject> skinnedEffects = new HashSet<GameObject>();
        private static Dictionary<EffectIndex,Dictionary<UInt32,EffectDef>> skinnedEffectCache = new Dictionary<EffectIndex, Dictionary<UInt32, EffectDef>>();
        partial void SetupEffectSkinning()
        {
            this.Enable += this.Main_Enable;
            this.Disable += this.Main_Disable;
            this.FirstFrame += this.Main_FirstFrame1;
        }

        internal static void RegisterEffect( GameObject effect )
        {
            var bitSkin = effect.GetComponent<BitSkinController>();
            if( bitSkin != null )
            {
                skinnedEffects.Add( effect );
            }

            EffectAPI.AddEffect( effect );
        }

        internal static void RegisterProjectile( GameObject projectile )
        {
            var controller = projectile.GetComponent<ProjectileController>();
            if( controller != null )
            {
                var ghost = controller.ghostPrefab;
                if( ghost != null )
                {
                    var bitSkin = ghost.GetComponent<BitSkinController>();
                    if( bitSkin != null )
                    {
                        skinnedProjectiles.Add( projectile );
                    }
                }

                ProjectileCatalog.getAdditionalEntries += ( list ) => list.Add( projectile );
            } else
            {
                Main.LogE( "No ProjectileController on: " + projectile.name );
            }
        }

        private void Main_FirstFrame1()
        {
            foreach( var effect in skinnedEffects )
            {
                var ind = EffectCatalog.FindEffectIndexFromPrefab(effect);
                if( ind == EffectIndex.Invalid )
                {
                    Main.LogE( "Invalid effect: " + effect.name );
                    continue;
                }

                skinnedEffectCache[ind] = new Dictionary<UInt32, EffectDef>();
            }

            foreach( var projectile in skinnedProjectiles )
            {
                var ind = ProjectileCatalog.GetProjectileIndex( projectile );
                if( ind == -1 )
                {
                    Main.LogE( "Invalid projectile: " + projectile.name );
                    continue;
                }

                skinnedGhostCache[ind] = new Dictionary<UInt32, GameObject>();
            }
        }

        private void Main_Disable()
        {
            On.RoR2.Projectile.ProjectileController.Start -= this.ProjectileController_Start1;
        }
        private void Main_Enable()
        {
            On.RoR2.Projectile.ProjectileController.Start += this.ProjectileController_Start1;
            IL.RoR2.EffectManager.SpawnEffect_EffectIndex_EffectData_bool += this.EffectManager_SpawnEffect_EffectIndex_EffectData_bool1;
        }

        private void EffectManager_SpawnEffect_EffectIndex_EffectData_bool1( MonoMod.Cil.ILContext il )
        {
            ILCursor c = new ILCursor( il );

            c.GotoNext( MoveType.Before, x => x.MatchCall( typeof( RoR2.EffectCatalog ), "GetEffectDef" ) );
            c.Remove();
            c.Emit( OpCodes.Ldarg_1 );
            c.EmitDelegate<Func<EffectIndex, EffectData, EffectDef>>( ( index, data ) =>
            {
                if( skinnedEffectCache.ContainsKey( index ) )
                {
                    var cache = skinnedEffectCache[index];
                    var skinInd = data.genericUInt;
                    if( cache.ContainsKey( skinInd ) )
                    {
                        return cache[skinInd];
                    } else
                    {
                        var origDef = EffectCatalog.GetEffectDef( index );
                        var skinnedPrefab = origDef.prefab.InstantiateClone( "CachedEffect" );
                        var skinController = skinnedPrefab.GetComponent<BitSkinController>();
                        skinController.Apply( WispBitSkin.GetWispSkin( skinInd ) );
                        Destroy( skinController );
                        var newDef = new EffectDef()
                        {
                            cullMethod = origDef.cullMethod,
                            index = index,
                            prefab = skinnedPrefab,
                            prefabEffectComponent = skinnedPrefab.GetComponent<EffectComponent>(),
                            prefabName = origDef.prefabName,
                            prefabVfxAttributes = skinnedPrefab.GetComponent<VFXAttributes>(),
                            spawnSoundEventName = origDef.spawnSoundEventName
                        };

                        cache[skinInd] = newDef;
                        return newDef;
                    }
                } else
                {
                    return EffectCatalog.GetEffectDef( index );
                }
            } );
        }

        private static HashSet<GameObject> skinnedProjectiles = new HashSet<GameObject>();
        private static Dictionary<Int32,Dictionary<UInt32,GameObject>> skinnedGhostCache = new Dictionary<Int32, Dictionary<UInt32, GameObject>>();

        private void ProjectileController_Start1( On.RoR2.Projectile.ProjectileController.orig_Start orig, RoR2.Projectile.ProjectileController self )
        {
            if( skinnedGhostCache.ContainsKey( self.catalogIndex ) && self.owner != null )
            {
                var skinInd = self.owner.GetComponent<CharacterBody>().skinIndex;
                var cache = skinnedGhostCache[self.catalogIndex];
                if( cache.ContainsKey( skinInd ) )
                {
                    self.ghostPrefab = cache[skinInd];
                } else
                {
                    var origGhost = self.ghostPrefab;
                    var newGhost = origGhost.InstantiateClone( "CachedProjectileGhost", false );
                    var controller = newGhost.GetComponent<BitSkinController>();
                    controller.Apply(WispBitSkin.GetWispSkin(skinInd));
                    UnityEngine.Object.Destroy( controller );
                    cache[skinInd] = newGhost;
                    self.ghostPrefab = newGhost;
                }
            }

            orig( self );
        }
    }
}
