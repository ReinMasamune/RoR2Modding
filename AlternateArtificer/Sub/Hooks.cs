﻿namespace AlternativeArtificer
{
    using System.Collections.Generic;
    using UnityEngine;
    using EntityStates.Mage;
    using EntityStates.Mage.Weapon;
    using States.Main;
    using System;
    using RoR2;
    using MonoMod.Cil;
    using Mono.Cecil.Cil;
    using UnityEngine.Networking;
    using System.Xml.Linq;
    using R2API.Utils;
    using RoR2.Skills;
    using System.Linq;
    using RoR2.UI;
    using AlternateArtificer.SelectablePassive;

    public partial class Main
    {
        private void RemoveHooks()
        {

        }

        private void AddHooks()
        {
            On.EntityStates.Mage.Weapon.FireFireBolt.FireGauntlet += this.FireFireBolt_FireGauntlet;
            On.EntityStates.Mage.Weapon.ChargeNovabomb.OnEnter += this.ChargeNovabomb_OnEnter;
            On.EntityStates.Mage.Weapon.ChargeNovabomb.FixedUpdate += this.ChargeNovabomb_FixedUpdate;
            On.EntityStates.Mage.Weapon.ChargeNovabomb.FireNovaBomb += this.ChargeNovabomb_FireNovaBomb;
            On.EntityStates.Mage.Weapon.ChargeNovabomb.OnExit += this.ChargeNovabomb_OnExit;
            On.EntityStates.Mage.Weapon.PrepWall.OnEnter += this.PrepWall_OnEnter;
            On.EntityStates.Mage.Weapon.PrepWall.OnExit += this.PrepWall_OnExit;
            On.EntityStates.Mage.Weapon.Flamethrower.OnEnter += this.Flamethrower_OnEnter;
            On.EntityStates.Mage.Weapon.Flamethrower.FixedUpdate += this.Flamethrower_FixedUpdate;
            On.EntityStates.Mage.Weapon.Flamethrower.OnExit += this.Flamethrower_OnExit;
            On.RoR2.HealthComponent.TakeDamage += this.HealthComponent_TakeDamage;
            GlobalEventManager.onCharacterDeathGlobal += this.GlobalEventManager_OnCharacterDeath;
            On.RoR2.Loadout.BodyLoadoutManager.BodyLoadout.ToXml += this.BodyLoadout_ToXml;
            IL.RoR2.UI.CharacterSelectController.OnNetworkUserLoadoutChanged += this.CharacterSelectController_OnNetworkUserLoadoutChanged;
        }

        private void CharacterSelectController_OnNetworkUserLoadoutChanged( ILContext il )
        {
            Action<CharacterModel,Loadout,Int32> emittedAction = ( model, loadout, body ) =>
            {
                var skills = BodyCatalog.GetBodyPrefabSkillSlots(body);
                for( Int32 i = 0; i < skills.Length; i++ )
                {
                    UInt32 selectedSkillIndex = loadout.bodyLoadoutManager.GetSkillVariant( body, i );
                    var slot = skills[i];

                    for( Int32 j = 0; j < slot.skillFamily.variants.Length; j++ )
                    {
                        var skillDef = slot.skillFamily.variants[j].skillDef;

                        if( skillDef != null && skillDef is PassiveSkillDef )
                        {
                            var passiveSkillDef = skillDef as PassiveSkillDef;
                            if( j == selectedSkillIndex )
                            {
                                passiveSkillDef.OnAssignDisplay( model );
                            } else
                            {
                                passiveSkillDef.OnUnassignDisplay( model );
                            }
                        }
                    }
                }

            };

            ILCursor c = new ILCursor( il );

            c.GotoNext( MoveType.After, x => x.MatchCallOrCallvirt<RoR2.SkinDef>( "Apply" ) );
            c.Emit( OpCodes.Ldloc, 6 );
            c.Emit( OpCodes.Ldloc, 2 );
            c.Emit( OpCodes.Ldloc, 3 );
            c.EmitDelegate<Action<CharacterModel, Loadout, Int32>>( emittedAction );
        }

        private System.Xml.Linq.XElement BodyLoadout_ToXml( On.RoR2.Loadout.BodyLoadoutManager.BodyLoadout.orig_ToXml orig, System.Object self, String elementName )
        {
            Int32 bodyIndex = self.GetFieldValue<Int32>("bodyIndex");
            var bodySkinController = BodyCatalog.GetBodyPrefab( bodyIndex ).GetComponent<ModelLocator>().modelTransform.GetComponent<ModelSkinController>();
            UInt32 skinPreference = self.GetFieldValue<UInt32>("skinPreference" );
            if( this.addedSkins.Contains(bodySkinController.skins[skinPreference] ) )
            {
                self.SetFieldValue<UInt32>( "skinPreference", 0u );
            }
            UInt32[] skillPreferences = self.GetFieldValue<UInt32[]>("skillPreferences" );
            var allBodyInfosObj = typeof( Loadout.BodyLoadoutManager ).GetFieldValue<object>( "allBodyInfos" );
            var allBodyInfos = ((Array)allBodyInfosObj).Cast<object>().ToArray();
            var currentInfo = allBodyInfos[bodyIndex];
            var prefabSkillSlotsObj = currentInfo.GetFieldValue<object>( "prefabSkillSlots" );
            var prefabSkillSlots = ((Array)prefabSkillSlotsObj).Cast<object>().ToArray();
            var skillFamilyIndices = currentInfo.GetFieldValue<Int32[]>( "skillFamilyIndices" );
            for( Int32 i = 0; i < prefabSkillSlots.Length; i++ )
            {
                var skillFamilyIndex = skillFamilyIndices[i];
                SkillFamily family = SkillCatalog.GetSkillFamily( skillFamilyIndex );
                SkillDef def = family.variants[skillPreferences[i]].skillDef;
                if( this.addedSkills.Contains( def ) )
                {
                    skillPreferences[i] = 0u;
                }
            }

            return orig( self, elementName );
        }



        #region IceStuff + FireStuff
        private struct FreezeInfo
        {
            public GameObject frozenBy;
            public Vector3 frozenAt;

            public FreezeInfo( GameObject frozenBy, Vector3 frozenAt )
            {
                this.frozenAt = frozenAt;
                this.frozenBy = frozenBy;
            }
        }

        private Dictionary<GameObject, GameObject> frozenBy = new Dictionary<GameObject, GameObject>();

        private void GlobalEventManager_OnCharacterDeath( DamageReport damageReport )
        {
            if( NetworkServer.active )
            {
                if( damageReport != null && damageReport.victimBody && damageReport.victimBody.healthComponent )
                {
                    if( damageReport.victimBody.healthComponent.isInFrozenState )
                    {
                        if( frozenBy.ContainsKey( damageReport.victim.gameObject ) )
                        {
                            var body = frozenBy[damageReport.victim.gameObject];
                            if( AltArtiPassive.instanceLookup.ContainsKey( body ) )
                            {
                                var passive = AltArtiPassive.instanceLookup[body];
                                passive.DoExecute( damageReport );
                            }
                        }
                    } else if( damageReport.damageInfo.damageType.HasFlag( DamageType.Freeze2s ) )
                    {
                        if( AltArtiPassive.instanceLookup.ContainsKey(damageReport.attacker))
                        {
                            AltArtiPassive.instanceLookup[damageReport.attacker].DoExecute( damageReport ); 
                        }
                    }
                }
            }
        }
        private void HealthComponent_TakeDamage( On.RoR2.HealthComponent.orig_TakeDamage orig, RoR2.HealthComponent self, RoR2.DamageInfo damageInfo )
        {
            if( damageInfo.damageType.HasFlag(DamageType.Freeze2s) )
            {
                frozenBy[self.gameObject] = damageInfo.attacker;
            }

            if( damageInfo.dotIndex == this.burnDot )
            {
                if( damageInfo.attacker )
                {
                    var attackerBody = damageInfo.attacker.GetComponent<CharacterBody>();
                    if( attackerBody )
                    {
                        Int32 buffCount = attackerBody.GetBuffCount( this.fireBuff );

                        if( buffCount >= 0 )
                        {
                            damageInfo.damage *= 1f + AltArtiPassive.burnDamageMult * buffCount;
                        }
                    }
                }
            }
            orig( self, damageInfo );
        }
        #endregion
        #region Flamethrower
        private class FlamethrowerContext
        {
            public AltArtiPassive passive;
            public Single timer;

            public FlamethrowerContext( AltArtiPassive passive )
            {
                this.passive = passive;
                this.timer = 0f;
            }
        }
        private Dictionary<Flamethrower, FlamethrowerContext> flamethrowerContext = new Dictionary<Flamethrower, FlamethrowerContext>();
        private void Flamethrower_OnEnter( On.EntityStates.Mage.Weapon.Flamethrower.orig_OnEnter orig, Flamethrower self )
        {
            orig( self );
            GameObject obj = self.outer.gameObject;
            if( AltArtiPassive.instanceLookup.ContainsKey( obj ) )
            {
                var passive = AltArtiPassive.instanceLookup[obj];
                var context = new FlamethrowerContext( passive );
                passive.SkillCast();
                flamethrowerContext[self] = context;
            }
        }
        private void Flamethrower_FixedUpdate( On.EntityStates.Mage.Weapon.Flamethrower.orig_FixedUpdate orig, Flamethrower self )
        {
            orig( self );
            if( flamethrowerContext.ContainsKey( self ) )
            {
                var context = flamethrowerContext[self];
                context.timer += Time.fixedDeltaTime * context.passive.ext_attackSpeedStat;
                Int32 count = 0;
                while( context.timer >= context.passive.ext_flamethrowerInterval && count <= context.passive.ext_flamethrowerMaxPerTick )
                {
                    context.passive.SkillCast();
                    count++;
                    context.timer -= context.passive.ext_flamethrowerInterval;
                }
            }
        }
        private void Flamethrower_OnExit( On.EntityStates.Mage.Weapon.Flamethrower.orig_OnExit orig, Flamethrower self )
        {
            orig( self );
            if( flamethrowerContext.ContainsKey( self ) )
            {
                var context = flamethrowerContext[self];
                context.passive.SkillCast();
                flamethrowerContext.Remove( self );
            }
        }
        #endregion
        #region Ice Wall
        private class PrepWallContext
        {
            public AltArtiPassive passive;
            public AltArtiPassive.BatchHandle handle;

            public PrepWallContext( AltArtiPassive passive, AltArtiPassive.BatchHandle handle )
            {
                this.passive = passive;
                this.handle = handle;
            }
        }
        private Dictionary<PrepWall,PrepWallContext> prepWallContext = new Dictionary<PrepWall, PrepWallContext>();
        private void PrepWall_OnEnter( On.EntityStates.Mage.Weapon.PrepWall.orig_OnEnter orig, PrepWall self )
        {
            orig( self );
            GameObject obj = self.outer.gameObject;
            if( AltArtiPassive.instanceLookup.ContainsKey( obj ) )
            {
                var passive = AltArtiPassive.instanceLookup[obj];
                var handle = new AltArtiPassive.BatchHandle();
                passive.SkillCast( handle );
                var context = new PrepWallContext( passive, handle );
                prepWallContext[self] = context;
            }
        }
        private void PrepWall_OnExit( On.EntityStates.Mage.Weapon.PrepWall.orig_OnExit orig, PrepWall self )
        {
            orig( self );
            if( prepWallContext.ContainsKey( self ) )
            {
                var context = prepWallContext[self];
                context.handle.Fire( context.passive.ext_prepWallMinDelay, context.passive.ext_prepWallMaxDelay );
                prepWallContext.Remove( self );
            }
        }
        #endregion
        #region Nano Bomb/Spear
        private class NanoBombContext
        {
            public AltArtiPassive passive;
            public AltArtiPassive.BatchHandle handle;
            public Single timer;
            public NanoBombContext( AltArtiPassive passive, AltArtiPassive.BatchHandle handle )
            {
                this.passive = passive;
                this.handle = handle;
                this.timer = 0f;
            }
        }
        private Dictionary<ChargeNovabomb, NanoBombContext> nanoBombContext = new Dictionary<ChargeNovabomb, NanoBombContext>();
        private void ChargeNovabomb_OnEnter( On.EntityStates.Mage.Weapon.ChargeNovabomb.orig_OnEnter orig, ChargeNovabomb self )
        {
            orig( self );
            GameObject obj = self.outer.gameObject;
            if( AltArtiPassive.instanceLookup.ContainsKey( obj ) )
            {
                var passive = AltArtiPassive.instanceLookup[obj];
                var handle = new AltArtiPassive.BatchHandle();
                var context = new NanoBombContext( passive, handle);
                nanoBombContext[self] = context;
                passive.SkillCast( handle );
            }
        }
        private void ChargeNovabomb_FixedUpdate( On.EntityStates.Mage.Weapon.ChargeNovabomb.orig_FixedUpdate orig, ChargeNovabomb self )
        {
            orig( self );
            if( nanoBombContext.ContainsKey( self ) )
            {
                var context = nanoBombContext[self];
                context.timer += Time.fixedDeltaTime * context.passive.ext_attackSpeedStat;
                Int32 count = 0;
                while( context.timer >= context.passive.ext_nanoBombInterval && count <= context.passive.ext_nanoBombMaxPerTick )
                {
                    count++;
                    context.passive.SkillCast( context.handle );
                    context.timer -= context.passive.ext_nanoBombInterval;
                }
            }
        }
        private void ChargeNovabomb_FireNovaBomb( On.EntityStates.Mage.Weapon.ChargeNovabomb.orig_FireNovaBomb orig, ChargeNovabomb self )
        {
            orig( self );
            if( nanoBombContext.ContainsKey( self ) )
            {
                var context = nanoBombContext[self];

                Int32 count = 0;
                while( context.timer >= context.passive.ext_nanoBombInterval && count <= context.passive.ext_nanoBombMaxPerTick )
                {
                    count++;
                    context.passive.SkillCast( context.handle );
                    context.timer -= context.passive.ext_nanoBombInterval;
                }

                context.handle.Fire( context.passive.ext_nanoBombMinDelay, context.passive.ext_nanoBombMaxDelay );
                nanoBombContext.Remove( self );
            }
        }
        private void ChargeNovabomb_OnExit( On.EntityStates.Mage.Weapon.ChargeNovabomb.orig_OnExit orig, ChargeNovabomb self )
        {
            orig( self );
            if( nanoBombContext.ContainsKey( self ) )
            {
                var context = nanoBombContext[self];

                Int32 count = 0;
                while( context.timer >= context.passive.ext_nanoBombInterval && count <= context.passive.ext_nanoBombMaxPerTick )
                {
                    count++;
                    context.passive.SkillCast( context.handle );
                    context.timer -= context.passive.ext_nanoBombInterval;
                }

                context.handle.Fire( context.passive.ext_nanoBombMinDelay, context.passive.ext_nanoBombMaxDelay );
                nanoBombContext.Remove( self );
            }
        }
        #endregion
        #region Fire/Lightning Bolt
        private void FireFireBolt_FireGauntlet( On.EntityStates.Mage.Weapon.FireFireBolt.orig_FireGauntlet orig, FireFireBolt self )
        {
            orig( self );
            GameObject obj = self.outer.gameObject;
            if( AltArtiPassive.instanceLookup.ContainsKey( obj ) )
            {
                AltArtiPassive.instanceLookup[obj].SkillCast();
            }
        }
        #endregion
    }
}