﻿using RoR2;
using R2API.Utils;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace RoR2Plugin
{
    /// <summary>
    /// An API for interfacing with the Director system for spawning enemies and items.
    /// </summary>
    public static partial class DirectorAPI
    {
        /// <summary>
        /// This subclass contains helper methods for use with DirectorAPI.
        /// Note that there is much more flexibility by working with the API directly through its event system.
        /// The primary purpose of these helpers is to serve as example code, and to assist with very simple tasks.
        /// They are NOT intended to be, or ever will be, a comprehensive way to use the DirectorAPI.
        /// </summary>
        public static class Helpers
        {
            /// <summary>
            /// This class contains static strings for each characterspawncard in the base game.
            /// These can be used for matching names.
            /// </summary>
            public static class MonsterNames
            {
                public static readonly string StoneTitanDistantRoost = "csctitanblackbeach";
                public static readonly string StoneTitanAbyssalDepths = "csctitandampcaves";
                public static readonly string StoneTitanTitanicPlains = "csctitangolemplains";
                public static readonly string StoneTitanAbandonedAqueduct = "csctitangoolake";
                public static readonly string ArchaicWisp = "cscarchwisp";
                public static readonly string StrikeDrone = "cscbackupdrone";
                public static readonly string Beetle = "cscbeetle";
                public static readonly string BeetleGuard = "cscbeetleguard";
                public static readonly string BeetleGuardFriendly = "cscbeetleguardally";
                public static readonly string BeetleQueen = "cscbeetlequeen";
                public static readonly string BrassContraption = "cscbell";
                public static readonly string BighornBison = "cscbison";
                public static readonly string ClayDunestrider = "cscclayboss";
                public static readonly string ClayTemplar = "cscclaybruiser";
                public static readonly string OverloadingWorm = "cscelectricworm";
                public static readonly string StoneGolem = "cscgolem";
                public static readonly string Grovetender = "cscgravekeeper";
                public static readonly string GreaterWisp = "cscgreaterwisp";
                public static readonly string HermitCrab = "cschermitcrab";
                public static readonly string Imp = "cscimp";
                public static readonly string ImpOverlord = "cscimpboss";
                public static readonly string Jellyfish = "cscjellyfish";
                public static readonly string Lemurian = "csclemurian";
                public static readonly string ElderLemurian = "csclemurianbruiser";
                public static readonly string LesserWisp = "csclesserwisp";
                public static readonly string MagmaWorm = "cscmagmaworm";
                public static readonly string SolusControlUnit = "cscroboballboss";
                public static readonly string SolusProbe = "cscroboballmini";
                public static readonly string AlloyWorshipUnit = "cscsuperroboballboss";
                public static readonly string AlliedWarshipUnit = "cscsuperroboballboss";
                public static readonly string FriendlyBoatUnit = "cscsuperroboballboss";
                public static readonly string Aurelionite = "csctitangold";
                public static readonly string AurelioniteAlly = "csctitangoldally";
                public static readonly string WanderingVagrant = "cscvagrant";
                public static readonly string AlloyVulture = "cscvulture";
            }

            /// <summary>
            /// This class contains static strings for each interactablespawncard in the base game.
            /// These can be used for matching names.
            /// </summary>
            public static class InteractableNames
            {
                public static readonly string Barrel = "iscbarrel1";
                public static readonly string GunnerDrone = "iscbrokendrone1";
                public static readonly string HealingDrone = "iscbrokendrone2";
                public static readonly string EquipmentDrone = "iscbrokenequipmentdrone";
                public static readonly string IncineratorDrone = "iscbrokenflamedrone";
                public static readonly string TC280 = "iscbrokenmegadrone";
                public static readonly string MissileDrone = "iscbrokenmissiledrone";
                public static readonly string GunnerTurret = "iscbrokenturret1";
                public static readonly string DamageChest = "isccategorychestdamage";
                public static readonly string HealingChest = "isccategorychesthealing";
                public static readonly string UtilityChest = "isccategorychestutility";
                public static readonly string BasicChest = "iscchest1";
                public static readonly string CloakedChest = "iscchest1stealthed";
                public static readonly string LargeChest = "iscchest2";
                public static readonly string PrinterCommon = "iscduplicator";
                public static readonly string PrinterUncommon = "iscduplicatorlarge";
                public static readonly string PrinterLegendary = "iscduplicatormilitary";
                public static readonly string EquipmentBarrel = "iscequipmentbarrel";
                public static readonly string LegendaryChest = "iscgoldchest";
                public static readonly string HalcyonBacon = "iscgoldshoresbracon";
                public static readonly string GoldPortal = "iscgoldshoresportal";
                public static readonly string Lockbox = "isclockbox";
                public static readonly string LunarBud = "isclunarchest";
                public static readonly string CelestialPortal = "iscmsportal";
                public static readonly string RadioScanner = "iscradartower";
                public static readonly string BluePortal = "iscshopportal";
                public static readonly string BloodShrine = "iscshrineblood";
                public static readonly string MountainShrine = "iscshrineboss";
                public static readonly string ChanceShrine = "iscshrinechance";
                public static readonly string CombatShrine = "iscshrinecombat";
                public static readonly string GoldShrine = "iscshrinegoldshoresaccess";
                public static readonly string WoodsShrine = "iscshrinehealing";
                public static readonly string OrderShrine = "iscshrinerestack";
                public static readonly string Teleporter = "iscteleporter";
                public static readonly string MultiShopCommon = "isctripleshop";
                public static readonly string MultiShopUncommon = "isctripleshoplarge";
            }


            /// <summary>
            /// Enables or disables elite spawns for a specific monster.
            /// </summary>
            /// <param name="monsterName">The name of the monster to edit</param>
            /// <param name="elitesAllowed">Should elites be allowed?</param>
            public static void PreventElites( string monsterName, bool elitesAllowed )
            {
                DirectorAPI.AddHook();
                DirectorAPI.monsterActions += ( monsters, currentStage ) =>
                {
                    foreach( DirectorCardHolder holder in monsters )
                    {
                        if( holder.card.spawnCard.name.ToLower() == monsterName.ToLower() )
                        {
                            ((CharacterSpawnCard)holder.card.spawnCard).noElites = elitesAllowed;
                        }
                    }
                };
            }

            /// <summary>
            /// Adds a new monster to all stages.
            /// </summary>
            /// <param name="monsterCard">The DirectorCard for the monster</param>
            /// <param name="category">The category to add the monster to</param>
            public static void AddNewMonster( DirectorCard monsterCard, MonsterCategory category )
            {
                DirectorAPI.AddHook();
                DirectorCardHolder card = new DirectorCardHolder
                {
                    card = monsterCard,
                    interactableCategory = InteractableCategory.None,
                    monsterCategory = category
                };
                DirectorAPI.monsterActions += ( monsters, currentStage ) =>
                {
                    monsters.Add( card );
                };
            }

            /// <summary>
            /// Adds a new monster to a specific stage.
            /// For custom stages use Stage.Custom and enter the name of the stage in customStageName.
            /// </summary>
            /// <param name="monsterCard">The DirectorCard of the monster to add</param>
            /// <param name="category">The category to add the monster to</param>
            /// <param name="stage">The stage to add the monster to</param>
            /// <param name="customStageName">The name of the custom stage</param>
            public static void AddNewMonsterToStage( DirectorCard monsterCard, MonsterCategory category, Stage stage, string customStageName = "" )
            {
                DirectorAPI.AddHook();
                DirectorCardHolder card = new DirectorCardHolder
                {
                    card = monsterCard,
                    interactableCategory = InteractableCategory.None,
                    monsterCategory = category
                };
                DirectorAPI.monsterActions += ( monsters, currentStage ) =>
                {
                    if( currentStage.stage == stage )
                    {
                        if(currentStage.CheckStage(stage,customStageName))
                        {
                            monsters.Add( card );
                        }
                    }
                };
            }

            /// <summary>
            /// Adds a new interactable to all stages.
            /// </summary>
            /// <param name="interactableCard">The DirectorCard for the interactable</param>
            /// <param name="category">The category of the interactable</param>
            public static void AddNewInteractable( DirectorCard interactableCard, InteractableCategory category )
            {
                DirectorAPI.AddHook();
                DirectorCardHolder card = new DirectorCardHolder
                {
                    card = interactableCard,
                    interactableCategory = category,
                    monsterCategory = MonsterCategory.None
                };
                DirectorAPI.interactableActions += ( interactables, currentStage ) =>
                {
                    interactables.Add( card );
                };
            }

            /// <summary>
            /// Adds a new interactable to a specific stage.
            /// For custom stages use Stage.Custom and enter the name of the stage in customStageName.
            /// </summary>
            /// <param name="interactableCard">The DirectorCard of the interactable</param>
            /// <param name="category">The category of the interactable</param>
            /// <param name="stage">The stage to add the interactable to</param>
            /// <param name="customStageName">The name of the custom stage</param>
            public static void AddNewInteractableToStage( DirectorCard interactableCard, InteractableCategory category, Stage stage, string customStageName = "" )
            {
                DirectorAPI.AddHook();
                DirectorCardHolder card = new DirectorCardHolder
                {
                    card = interactableCard,
                    interactableCategory = category,
                    monsterCategory = MonsterCategory.None
                };
                DirectorAPI.interactableActions += ( interactables, currentStage ) =>
                {
                    if( currentStage.stage == stage )
                    {
                        if(currentStage.CheckStage(stage,customStageName))
                        {
                            interactables.Add( card );
                        }
                    }
                };
            }

            /// <summary>
            /// Removes a monster from spawns on all stages.
            /// </summary>
            /// <param name="monsterName">The name of the monster card to remove</param>
            public static void RemoveExistingMonster( string monsterName )
            {
                DirectorAPI.AddHook();
                DirectorAPI.monsterActions += ( monsters, currentStage ) =>
                {
                    monsters.RemoveAll( ( card ) => (card.card.spawnCard.name.ToLower() == monsterName.ToLower()) );
                };
            }

            /// <summary>
            /// Removes a monster from spawns on a specific stage.
            /// For custom stages use Stage.Custom and enter the name of the stage in customStageName.
            /// </summary>
            /// <param name="monsterName">The name of the monster card to remove</param>
            /// <param name="stage">The stage to remove on</param>
            /// <param name="customStageName">The name of the custom stage</param>
            public static void RemoveExistingMonsterFromStage( string monsterName, Stage stage, string customStageName = "" )
            {
                DirectorAPI.AddHook();
                DirectorAPI.monsterActions += ( monsters, currentStage ) =>
                {
                    if( currentStage.stage == stage )
                    {
                        if( ( stage != Stage.Custom ) ^ ( currentStage.customStageName == customStageName ) )
                        {
                            monsters.RemoveAll( ( card ) => (card.card.spawnCard.name.ToLower() == monsterName.ToLower()) );
                        }
                    }
                };
            }

            /// <summary>
            /// Remove an interactable from spawns on all stages.
            /// </summary>
            /// <param name="interactableName">Name of the interactable to remove</param>
            public static void RemoveExistingInteractable( string interactableName )
            {
                DirectorAPI.AddHook();
                DirectorAPI.interactableActions += ( interactables, currentStage ) =>
                {
                    interactables.RemoveAll( ( card ) => (card.card.spawnCard.name.ToLower() == interactableName.ToLower()) );
                };
            }

            /// <summary>
            /// Remove an interactable from spawns on a specific stage.
            /// For custom stages use Stage.Custom and enter the name of the stage in customStageName.
            /// </summary>
            /// <param name="interactableName">The name of the interactable to remove</param>
            /// <param name="stage">The stage to remove on</param>
            /// <param name="customStageName">The name of the custom stage</param>
            public static void RemoveExistingInteractableFromStage( string interactableName, Stage stage, string customStageName = "" )
            {
                DirectorAPI.AddHook();
                DirectorAPI.interactableActions += ( interactables, currentStage ) =>
                {
                    if( currentStage.stage == stage )
                    {
                        if(currentStage.CheckStage(stage,customStageName))
                        {
                            interactables.RemoveAll( ( card ) => (card.card.spawnCard.name.ToLower() == interactableName.ToLower()) );
                        }
                    }
                };
            }

            /// <summary>
            /// Adds a flat amount of monster credits to the scene director on a specific stage.
            /// For custom stages use Stage.Custom and enter the name of the stage in customStageName.
            /// </summary>
            /// <param name="increase">The quantity to add</param>
            /// <param name="stage">The stage to add on</param>
            /// <param name="customStageName">The name of the custom stage</param>
            public static void AddSceneMonsterCredits( int increase, Stage stage, string customStageName = "" )
            {
                DirectorAPI.AddHook();
                DirectorAPI.stageSettingsActions += ( settings, currentStage ) =>
                {
                    if( currentStage.stage == stage )
                    {
                        if(currentStage.CheckStage(stage,customStageName))
                        {
                            settings.sceneDirectorMonsterCredits += increase;
                        }
                    }
                };
            }

            /// <summary>
            /// Adds a flat amount of interactable credits to the scene director on a specific stage.
            /// For custom stages use Stage.Custom and enter the name of the stage in customStageName.
            /// </summary>
            /// <param name="increase">The quantity to add</param>
            /// <param name="stage">The stage to add on</param>
            /// <param name="customStageName">The name of the custom stage</param>
            public static void AddSceneInteractableCredits( int increase, Stage stage, string customStageName = "" )
            {
                DirectorAPI.AddHook();
                DirectorAPI.stageSettingsActions += ( settings, currentStage ) =>
                {
                    if( currentStage.stage == stage )
                    {
                        if(currentStage.CheckStage(stage,customStageName))
                        {
                            settings.sceneDirectorInteractableCredits += increase;
                        }
                    }
                };
            }

            /// <summary>
            /// Multiplies the scene director monster credits on a specific stage.
            /// For custom stages use Stage.Custom and enter the name of the stage in customStageName.
            /// </summary>
            /// <param name="multiplier">The number to multiply by</param>
            /// <param name="stage">The stage to multiply on</param>
            /// <param name="customStageName">The name of the custom stage</param>
            public static void MultiplySceneMonsterCredits( int multiplier, Stage stage, string customStageName = "" )
            {
                DirectorAPI.AddHook();
                DirectorAPI.stageSettingsActions += ( settings, currentStage ) =>
                {
                    if( currentStage.stage == stage )
                    {
                        if(currentStage.CheckStage(stage,customStageName))
                        {
                            settings.sceneDirectorMonsterCredits *= multiplier;
                        }
                    }
                };
            }

            /// <summary>
            /// Multiplies the scene director interactable credits on a specific stage.
            /// For custom stages use Stage.Custom and enter the name of the stage in customStageName.
            /// </summary>
            /// <param name="multiplier">The number to multiply by</param>
            /// <param name="stage">The stage to multiply on</param>
            /// <param name="customStageName">The name of the custom stage</param>
            public static void MultiplySceneInteractableCredits( int multiplier, Stage stage, string customStageName = "" )
            {
                DirectorAPI.AddHook();
                DirectorAPI.stageSettingsActions += ( settings, currentStage ) =>
                {
                    if( currentStage.stage == stage )
                    {
                        if(currentStage.CheckStage(stage,customStageName))
                        {
                            settings.sceneDirectorInteractableCredits *= multiplier;
                        }
                    }
                };
            }

            /// <summary>
            /// Divides the scene director monster credits on a specific stage.
            /// For custom stages use Stage.Custom and enter the name of the stage in customStageName.
            /// </summary>
            /// <param name="divisor">The number to divide by</param>
            /// <param name="stage">The stage to divide on</param>
            /// <param name="customStageName">The name of the custom stage</param>
            public static void ReduceSceneMonsterCredits( int divisor, Stage stage, string customStageName = "" )
            {
                DirectorAPI.AddHook();
                DirectorAPI.stageSettingsActions += ( settings, currentStage ) =>
                {
                    if( currentStage.stage == stage )
                    {
                        if(currentStage.CheckStage(stage,customStageName))
                        {
                            settings.sceneDirectorMonsterCredits /= divisor;
                        }
                    }
                };
            }

            /// <summary>
            /// Divides the scene director interactable credits on a specific stage.
            /// For custom stages use Stage.Custom and enter the name of the stage in customStageName.
            /// </summary>
            /// <param name="divisor">The number to divide by</param>
            /// <param name="stage">The stage to divide on</param>
            /// <param name="customStageName">The name of the custom stage</param>
            public static void ReduceSceneInteractableCredits( int divisor, Stage stage, string customStageName = "" )
            {
                DirectorAPI.AddHook();
                DirectorAPI.stageSettingsActions += ( settings, currentStage ) =>
                {
                    if( currentStage.stage == stage )
                    {
                        if(currentStage.CheckStage(stage,customStageName))
                        {
                            settings.sceneDirectorInteractableCredits /= divisor;
                        }
                    }
                };
            }
        }
    }
}
