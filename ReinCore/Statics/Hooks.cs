using System;
using System.ComponentModel;
using System.Reflection;
using BepInEx;
using MonoMod.Cil;
using MonoMod.RuntimeDetour.HookGen;
using RoR2;
using RoR2.Projectile;
using RoR2.UI;
using UnityEngine;

namespace ReinCore
{
    // TODO: Docs for HooksCore
    // TODO: Fix formatting in HooksCore
    // TODO: Add more hooks to HooksCore


    /*
private static MethodBase method = HookableHelpers.GetBase( typeof(thing) );
public delegate void Orig( global::thing self );
public delegate void Hook( Orig orig, global::thing self );
public static event ILContext.Manipulator Il
{
    add => HookEndpointManager.Modify<Hook>( method, value );
    remove => HookEndpointManager.Unmodify<Hook>( method, value );
}
public static event Hook On
{
    add => HookEndpointManager.Add<Hook>( method, value );
    remove => HookEndpointManager.Remove<Hook>( method, value );
}
    */




    /// <summary>
    /// 
    /// </summary>
    public static class HooksCore
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable IDE1006 // Naming Styles
        public struct EntityStates
        {
            public struct Commando
            {
                public static class DodgeState
                {
                    public struct OnEnter
                    {
                        private static MethodBase method = HookableHelpers.GetBase( typeof(OnEnter) );
                        public delegate void Orig(global::EntityStates.Commando.DodgeState self );
                        public delegate void Hook( Orig orig, global::EntityStates.Commando.DodgeState self );
                        public static event ILContext.Manipulator Il
                        {
                            add => HookEndpointManager.Modify<Hook>( method, value );
                            remove => HookEndpointManager.Unmodify<Hook>( method, value );
                        }
                        public static event Hook On
                        {
                            add => HookEndpointManager.Add<Hook>( method, value );
                            remove => HookEndpointManager.Remove<Hook>( method, value );
                        }
                    }
                }
            }

            public struct GoldGat
            {
                public static class BaseGoldGatState
                {
                    public struct FixedUpdate
                    {
                        private static MethodBase method = HookableHelpers.GetBase( typeof(FixedUpdate) );
                        public delegate void Orig(global::EntityStates.GoldGat.BaseGoldGatState self );
                        public delegate void Hook( Orig orig, global::EntityStates.GoldGat.BaseGoldGatState self );
                        public static event ILContext.Manipulator Il
                        {
                            add => HookEndpointManager.Modify<Hook>( method, value );
                            remove => HookEndpointManager.Unmodify<Hook>( method, value );
                        }
                        public static event Hook On
                        {
                            add => HookEndpointManager.Add<Hook>( method, value );
                            remove => HookEndpointManager.Remove<Hook>( method, value );
                        }
                    }
                }
            }

            public struct Mage
            {
                public struct Weapon
                {
                    public static class ChargeNovabomb
                    {
                        public struct OnEnter
                        {
                            private static MethodBase method = HookableHelpers.GetBase( typeof(OnEnter) );
                            public delegate void Orig( global::EntityStates.Mage.Weapon.ChargeNovabomb self );
                            public delegate void Hook( Orig orig, global::EntityStates.Mage.Weapon.ChargeNovabomb self );
                            public static event ILContext.Manipulator Il
                            {
                                add => HookEndpointManager.Modify<Hook>( method, value );
                                remove => HookEndpointManager.Unmodify<Hook>( method, value );
                            }
                            public static event Hook On
                            {
                                add => HookEndpointManager.Add<Hook>( method, value );
                                remove => HookEndpointManager.Remove<Hook>( method, value );
                            }
                        }

                        public struct FixedUpdate
                        {
                            private static MethodBase method = HookableHelpers.GetBase( typeof(FixedUpdate) );
                            public delegate void Orig( global::EntityStates.Mage.Weapon.ChargeNovabomb self );
                            public delegate void Hook( Orig orig, global::EntityStates.Mage.Weapon.ChargeNovabomb self );
                            public static event ILContext.Manipulator Il
                            {
                                add => HookEndpointManager.Modify<Hook>( method, value );
                                remove => HookEndpointManager.Unmodify<Hook>( method, value );
                            }
                            public static event Hook On
                            {
                                add => HookEndpointManager.Add<Hook>( method, value );
                                remove => HookEndpointManager.Remove<Hook>( method, value );
                            }
                        }

                        public struct FireNovaBomb
                        {
                            private static MethodBase method = HookableHelpers.GetBase( typeof(FireNovaBomb) );
                            public delegate void Orig( global::EntityStates.Mage.Weapon.ChargeNovabomb self );
                            public delegate void Hook( Orig orig, global::EntityStates.Mage.Weapon.ChargeNovabomb self );
                            public static event ILContext.Manipulator Il
                            {
                                add => HookEndpointManager.Modify<Hook>( method, value );
                                remove => HookEndpointManager.Unmodify<Hook>( method, value );
                            }
                            public static event Hook On
                            {
                                add => HookEndpointManager.Add<Hook>( method, value );
                                remove => HookEndpointManager.Remove<Hook>( method, value );
                            }
                        }

                        public struct OnExit
                        {
                            private static MethodBase method = HookableHelpers.GetBase( typeof(OnExit) );
                            public delegate void Orig( global::EntityStates.Mage.Weapon.ChargeNovabomb self );
                            public delegate void Hook( Orig orig, global::EntityStates.Mage.Weapon.ChargeNovabomb self );
                            public static event ILContext.Manipulator Il
                            {
                                add => HookEndpointManager.Modify<Hook>( method, value );
                                remove => HookEndpointManager.Unmodify<Hook>( method, value );
                            }
                            public static event Hook On
                            {
                                add => HookEndpointManager.Add<Hook>( method, value );
                                remove => HookEndpointManager.Remove<Hook>( method, value );
                            }
                        }
                    }

                    public static class FireFireBolt
                    {
                        public struct FireGauntlet
                        {
                            private static MethodBase method = HookableHelpers.GetBase( typeof(FireGauntlet) );
                            public delegate void Orig( global::EntityStates.Mage.Weapon.ChargeNovabomb self );
                            public delegate void Hook( Orig orig, global::EntityStates.Mage.Weapon.ChargeNovabomb self );
                            public static event ILContext.Manipulator Il
                            {
                                add => HookEndpointManager.Modify<Hook>( method, value );
                                remove => HookEndpointManager.Unmodify<Hook>( method, value );
                            }
                            public static event Hook On
                            {
                                add => HookEndpointManager.Add<Hook>( method, value );
                                remove => HookEndpointManager.Remove<Hook>( method, value );
                            }
                        }
                    }

                    public static class Flamethrower
                    {
                        public struct OnEnter
                        {
                            private static MethodBase method = HookableHelpers.GetBase( typeof(OnEnter) );
                            public delegate void Orig( global::EntityStates.Mage.Weapon.Flamethrower self );
                            public delegate void Hook( Orig orig, global::EntityStates.Mage.Weapon.Flamethrower self );
                            public static event ILContext.Manipulator Il
                            {
                                add => HookEndpointManager.Modify<Hook>( method, value );
                                remove => HookEndpointManager.Unmodify<Hook>( method, value );
                            }
                            public static event Hook On
                            {
                                add => HookEndpointManager.Add<Hook>( method, value );
                                remove => HookEndpointManager.Remove<Hook>( method, value );
                            }
                        }

                        public struct FixedUpdate
                        {
                            private static MethodBase method = HookableHelpers.GetBase( typeof(FixedUpdate) );
                            public delegate void Orig( global::EntityStates.Mage.Weapon.Flamethrower self );
                            public delegate void Hook( Orig orig, global::EntityStates.Mage.Weapon.Flamethrower self );
                            public static event ILContext.Manipulator Il
                            {
                                add => HookEndpointManager.Modify<Hook>( method, value );
                                remove => HookEndpointManager.Unmodify<Hook>( method, value );
                            }
                            public static event Hook On
                            {
                                add => HookEndpointManager.Add<Hook>( method, value );
                                remove => HookEndpointManager.Remove<Hook>( method, value );
                            }
                        }

                        public struct OnExit
                        {
                            private static MethodBase method = HookableHelpers.GetBase( typeof(OnExit) );
                            public delegate void Orig( global::EntityStates.Mage.Weapon.Flamethrower self );
                            public delegate void Hook( Orig orig, global::EntityStates.Mage.Weapon.Flamethrower self );
                            public static event ILContext.Manipulator Il
                            {
                                add => HookEndpointManager.Modify<Hook>( method, value );
                                remove => HookEndpointManager.Unmodify<Hook>( method, value );
                            }
                            public static event Hook On
                            {
                                add => HookEndpointManager.Add<Hook>( method, value );
                                remove => HookEndpointManager.Remove<Hook>( method, value );
                            }
                        }
                    }

                    public static class PrepWall
                    {
                        public struct OnEnter
                        {
                            private static MethodBase method = HookableHelpers.GetBase( typeof(OnEnter) );
                            public delegate void Orig( global::EntityStates.Mage.Weapon.PrepWall self );
                            public delegate void Hook( Orig orig, global::EntityStates.Mage.Weapon.PrepWall self );
                            public static event ILContext.Manipulator Il
                            {
                                add => HookEndpointManager.Modify<Hook>( method, value );
                                remove => HookEndpointManager.Unmodify<Hook>( method, value );
                            }
                            public static event Hook On
                            {
                                add => HookEndpointManager.Add<Hook>( method, value );
                                remove => HookEndpointManager.Remove<Hook>( method, value );
                            }
                        }

                        public struct OnExit
                        {
                            private static MethodBase method = HookableHelpers.GetBase( typeof(OnExit) );
                            public delegate void Orig( global::EntityStates.Mage.Weapon.PrepWall self );
                            public delegate void Hook( Orig orig, global::EntityStates.Mage.Weapon.PrepWall self );
                            public static event ILContext.Manipulator Il
                            {
                                add => HookEndpointManager.Modify<Hook>( method, value );
                                remove => HookEndpointManager.Unmodify<Hook>( method, value );
                            }
                            public static event Hook On
                            {
                                add => HookEndpointManager.Add<Hook>( method, value );
                                remove => HookEndpointManager.Remove<Hook>( method, value );
                            }
                        }
                    }
                }
            }
        }

        public struct RoR2
        {
            public static class BuffCatalog
            {
                public struct Init
                {
                    private static MethodBase method = HookableHelpers.GetBase( typeof(Init) );
                    public delegate void Orig();
                    public delegate void Hook( Orig orig );
                    public static event ILContext.Manipulator Il
                    {
                        add => HookEndpointManager.Modify<Hook>( method, value );
                        remove => HookEndpointManager.Unmodify<Hook>( method, value );
                    }
                    public static event Hook On
                    {
                        add => HookEndpointManager.Add<Hook>( method, value );
                        remove => HookEndpointManager.Remove<Hook>( method, value );
                    }
                }
            }

            public static class CameraRigController
            {
                public struct Start
                {
                    private static MethodBase method = HookableHelpers.GetBase( typeof(Start) );
                    public delegate void Orig( global::RoR2.CameraRigController self );
                    public delegate void Hook( Orig orig, global::RoR2.CameraRigController self );
                    public static event ILContext.Manipulator Il
                    {
                        add => HookEndpointManager.Modify<Hook>( method, value );
                        remove => HookEndpointManager.Unmodify<Hook>( method, value );
                    }
                    public static event Hook On
                    {
                        add => HookEndpointManager.Add<Hook>( method, value );
                        remove => HookEndpointManager.Remove<Hook>( method, value );
                    }
                }

                public struct Update
                {
                    private static MethodBase method = HookableHelpers.GetBase( typeof(Update) );
                    public delegate void Orig( global::RoR2.CameraRigController self );
                    public delegate void Hook( Orig orig, global::RoR2.CameraRigController self );
                    public static event ILContext.Manipulator Il
                    {
                        add => HookEndpointManager.Modify<Hook>( method, value );
                        remove => HookEndpointManager.Unmodify<Hook>( method, value );
                    }
                    public static event Hook On
                    {
                        add => HookEndpointManager.Add<Hook>( method, value );
                        remove => HookEndpointManager.Remove<Hook>( method, value );
                    }
                }
            }

            public static class CharacterBody
            {
                public struct FixedUpdate
                {
                    private static MethodBase method = HookableHelpers.GetBase( typeof(FixedUpdate) );
                    public delegate void Orig( global::RoR2.CharacterBody self );
                    public delegate void Hook( Orig orig, global::RoR2.CharacterBody self );
                    public static event ILContext.Manipulator Il
                    {
                        add => HookEndpointManager.Modify<Hook>( method, value );
                        remove => HookEndpointManager.Unmodify<Hook>( method, value );
                    }
                    public static event Hook On
                    {
                        add => HookEndpointManager.Add<Hook>( method, value );
                        remove => HookEndpointManager.Remove<Hook>( method, value );
                    }
                }

                public struct RecalculateStats
                {
                    private static MethodBase method = HookableHelpers.GetBase( typeof(RecalculateStats) );
                    public delegate void Orig( global::RoR2.CharacterBody self );
                    public delegate void Hook( Orig orig, global::RoR2.CharacterBody self );
                    public static event ILContext.Manipulator Il
                    {
                        add => HookEndpointManager.Modify<Hook>( method, value );
                        remove => HookEndpointManager.Unmodify<Hook>( method, value );
                    }
                    public static event Hook On
                    {
                        add => HookEndpointManager.Add<Hook>( method, value );
                        remove => HookEndpointManager.Remove<Hook>( method, value );
                    }
                }

                public struct Start
                {
                    private static MethodBase method = HookableHelpers.GetBase( typeof(Start) );
                    public delegate void Orig( global::RoR2.CharacterBody self );
                    public delegate void Hook( Orig orig, global::RoR2.CharacterBody self );
                    public static event ILContext.Manipulator Il
                    {
                        add => HookEndpointManager.Modify<Hook>( method, value );
                        remove => HookEndpointManager.Unmodify<Hook>( method, value );
                    }
                    public static event Hook On
                    {
                        add => HookEndpointManager.Add<Hook>( method, value );
                        remove => HookEndpointManager.Remove<Hook>( method, value );
                    }
                }
            }

            public static class CharacterSpawnCard
            {
                public struct Awake
                {
                    private static MethodBase method = HookableHelpers.GetBase( typeof(Awake) );
                    public delegate void Orig( global::RoR2.CharacterSpawnCard self );
                    public delegate void Hook( Orig orig, global::RoR2.CharacterSpawnCard self );
                    public static event ILContext.Manipulator Il
                    {
                        add => HookEndpointManager.Modify<Hook>( method, value );
                        remove => HookEndpointManager.Unmodify<Hook>( method, value );
                    }
                    public static event Hook On
                    {
                        add => HookEndpointManager.Add<Hook>( method, value );
                        remove => HookEndpointManager.Remove<Hook>( method, value );
                    }
                }
            }

            public static class ClassicStageInfo
            {
                public struct Awake
                {
                    private static MethodBase method = HookableHelpers.GetBase( typeof(Awake) );
                    public delegate void Orig( global::RoR2.ClassicStageInfo self );
                    public delegate void Hook( Orig orig, global::RoR2.ClassicStageInfo self );
                    public static event ILContext.Manipulator Il
                    {
                        add => HookEndpointManager.Modify<Hook>( method, value );
                        remove => HookEndpointManager.Unmodify<Hook>( method, value );
                    }
                    public static event Hook On
                    {
                        add => HookEndpointManager.Add<Hook>( method, value );
                        remove => HookEndpointManager.Remove<Hook>( method, value );
                    }
                }
            }

            public static class EffectCatalog
            {
                public struct GetDefaultEffectDefs
                {
                    private static MethodBase method = HookableHelpers.GetBase( typeof(GetDefaultEffectDefs) );
                    public delegate global::RoR2.EffectDef[] Orig();
                    public delegate global::RoR2.EffectDef[] Hook( Orig orig );
                    public static event ILContext.Manipulator Il
                    {
                        add => HookEndpointManager.Modify<Hook>( method, value );
                        remove => HookEndpointManager.Unmodify<Hook>( method, value );
                    }
                    public static event Hook On
                    {
                        add => HookEndpointManager.Add<Hook>( method, value );
                        remove => HookEndpointManager.Remove<Hook>( method, value );
                    }
                }
            }

            public static class EffectManager
            {
                public struct SpawnEffect___void_EffectIndex_EffectData_Boolean
                {
                    private static MethodBase method = HookableHelpers.GetBase( typeof(SpawnEffect___void_EffectIndex_EffectData_Boolean), true, CallingConventions.Standard,  typeof(global::RoR2.EffectIndex), typeof(global::RoR2.EffectData), typeof(global::System.Boolean) );
                    public delegate void Orig( global::RoR2.EffectIndex index, global::RoR2.EffectData data, global::System.Boolean transmit );
                    public delegate void Hook( Orig orig, global::RoR2.EffectIndex index, global::RoR2.EffectData data, global::System.Boolean transmit );

                    public static event ILContext.Manipulator Il
                    {
                        add => HookEndpointManager.Modify<Hook>( method, value );
                        remove => HookEndpointManager.Unmodify<Hook>( method, value );
                    }
                    public static event Hook On
                    {
                        add => HookEndpointManager.Add<Hook>( method, value );
                        remove => HookEndpointManager.Remove<Hook>( method, value );
                    }
                }
            }

            public static class EquipmentSlot
            {
                public struct FixedUpdate
                {
                    private static MethodBase method = HookableHelpers.GetBase( typeof(FixedUpdate) );
                    public delegate void Orig(global::RoR2.EquipmentSlot self );
                    public delegate void Hook( Orig orig, global::RoR2.EquipmentSlot self );
                    public static event ILContext.Manipulator Il
                    {
                        add => HookEndpointManager.Modify<Hook>( method, value );
                        remove => HookEndpointManager.Unmodify<Hook>( method, value );
                    }
                    public static event Hook On
                    {
                        add => HookEndpointManager.Add<Hook>( method, value );
                        remove => HookEndpointManager.Remove<Hook>( method, value );
                    }
                }
            }

            public static class GenericPickupController
            {
                public struct GrantEquipment
                {
                    private static MethodBase method = HookableHelpers.GetBase( typeof(GrantEquipment) );
                    public delegate void Orig(global::RoR2.GenericPickupController self, global::RoR2.CharacterBody body, global::RoR2.Inventory inventory);
                    public delegate void Hook( Orig orig, global::RoR2.GenericPickupController self, global::RoR2.CharacterBody body, global::RoR2.Inventory inventory );
                    public static event ILContext.Manipulator Il
                    {
                        add => HookEndpointManager.Modify<Hook>( method, value );
                        remove => HookEndpointManager.Unmodify<Hook>( method, value );
                    }
                    public static event Hook On
                    {
                        add => HookEndpointManager.Add<Hook>( method, value );
                        remove => HookEndpointManager.Remove<Hook>( method, value );
                    }
                }
            }

            public static class GlobalEventManager
            {
                public struct OnHitEnemy
                {
                    private static MethodBase method = HookableHelpers.GetBase( typeof(OnHitEnemy) );
                    public delegate void Orig( global::RoR2.GlobalEventManager self, global::RoR2.DamageInfo damageInfo, global::UnityEngine.GameObject victim );
                    public delegate void Hook( Orig orig, global::RoR2.GlobalEventManager self, global::RoR2.DamageInfo damageInfo, global::UnityEngine.GameObject victim );
                    public static event ILContext.Manipulator Il
                    {
                        add => HookEndpointManager.Modify<Hook>( method, value );
                        remove => HookEndpointManager.Unmodify<Hook>( method, value );
                    }
                    public static event Hook On
                    {
                        add => HookEndpointManager.Add<Hook>( method, value );
                        remove => HookEndpointManager.Remove<Hook>( method, value );
                    }
                }
            }

            public static class HealthComponent
            {
                public static class RepeatHealComponent
                {
                    public struct FixedUpdate
                    {
                        private static MethodBase method = HookableHelpers.GetBase( typeof(FixedUpdate) );
                        public delegate void Orig( System.Object self );
                        public delegate void Hook( Orig orig, System.Object self );
                        public static event ILContext.Manipulator Il
                        {
                            add => HookEndpointManager.Modify<Hook>( method, value );
                            remove => HookEndpointManager.Unmodify<Hook>( method, value );
                        }
                        public static event Hook On
                        {
                            add => HookEndpointManager.Add<Hook>( method, value );
                            remove => HookEndpointManager.Remove<Hook>( method, value );
                        }
                    }
                }


                public struct Heal
                {
                    private static MethodBase method = HookableHelpers.GetBase( typeof(Heal) );
                    public delegate void Orig( global::RoR2.HealthComponent self, global::RoR2.ProcChainMask procChainMask, Boolean nonRegen );
                    public delegate void Hook( Orig orig, global::RoR2.HealthComponent self, global::RoR2.ProcChainMask procChainMask, Boolean nonRegen );
                    public static event ILContext.Manipulator Il
                    {
                        add => HookEndpointManager.Modify<Hook>( method, value );
                        remove => HookEndpointManager.Unmodify<Hook>( method, value );
                    }
                    public static event Hook On
                    {
                        add => HookEndpointManager.Add<Hook>( method, value );
                        remove => HookEndpointManager.Remove<Hook>( method, value );
                    }
                }
                public struct TakeDamage
                {
                    private static MethodBase method = HookableHelpers.GetBase( typeof(TakeDamage) );
                    public delegate void Orig( global::RoR2.HealthComponent self, global::RoR2.DamageInfo damageInfo );
                    public delegate void Hook( Orig orig, global::RoR2.HealthComponent self, global::RoR2.DamageInfo damageInfo );
                    public static event ILContext.Manipulator Il
                    {
                        add => HookEndpointManager.Modify<Hook>( method, value );
                        remove => HookEndpointManager.Unmodify<Hook>( method, value );
                    }
                    public static event Hook On
                    {
                        add => HookEndpointManager.Add<Hook>( method, value );
                        remove => HookEndpointManager.Remove<Hook>( method, value );
                    }
                }
            }

            public static class Inventory
            {
                public struct CalculateEquipmentCooldownScale
                {
                    private static MethodBase method = HookableHelpers.GetBase( typeof(CalculateEquipmentCooldownScale) );
                    public delegate Single Orig( global::RoR2.Inventory self );
                    public delegate Single Hook( Orig orig, global::RoR2.Inventory self );
                    public static event ILContext.Manipulator Il
                    {
                        add => HookEndpointManager.Modify<Hook>( method, value );
                        remove => HookEndpointManager.Unmodify<Hook>( method, value );
                    }
                    public static event Hook On
                    {
                        add => HookEndpointManager.Add<Hook>( method, value );
                        remove => HookEndpointManager.Remove<Hook>( method, value );
                    }
                }

                public struct SetActiveEquipmentSlot
                {
                    private static MethodBase method = HookableHelpers.GetBase( typeof(SetActiveEquipmentSlot) );
                    public delegate void Orig( global::RoR2.Inventory self, Byte slotIndex );
                    public delegate void Hook( Orig orig, global::RoR2.Inventory self, Byte slotIndex );
                    public static event ILContext.Manipulator Il
                    {
                        add => HookEndpointManager.Modify<Hook>( method, value );
                        remove => HookEndpointManager.Unmodify<Hook>( method, value );
                    }
                    public static event Hook On
                    {
                        add => HookEndpointManager.Add<Hook>( method, value );
                        remove => HookEndpointManager.Remove<Hook>( method, value );
                    }
                }

                public struct UpdateEquipment
                {
                    private static MethodBase method = HookableHelpers.GetBase( typeof(UpdateEquipment) );
                    public delegate void Orig( global::RoR2.Inventory self );
                    public delegate void Hook( Orig orig, global::RoR2.Inventory self );
                    public static event ILContext.Manipulator Il
                    {
                        add => HookEndpointManager.Modify<Hook>( method, value );
                        remove => HookEndpointManager.Unmodify<Hook>( method, value );
                    }
                    public static event Hook On
                    {
                        add => HookEndpointManager.Add<Hook>( method, value );
                        remove => HookEndpointManager.Remove<Hook>( method, value );
                    }
                }
            }

            public static class RoR2Application
            {
                public static class UnitySystemConsoleRedirector
                {
                    public struct Redirect
                    {
                        private static MethodBase method = HookableHelpers.GetBase( typeof(Redirect) );
                        public delegate void Orig();
                        public delegate void Hook( Orig orig );
                        public static event ILContext.Manipulator Il
                        {
                            add => HookEndpointManager.Modify<Hook>( method, value );
                            remove => HookEndpointManager.Unmodify<Hook>( method, value );
                        }
                        public static event Hook On
                        {
                            add => HookEndpointManager.Add<Hook>( method, value );
                            remove => HookEndpointManager.Remove<Hook>( method, value );
                        }
                    }
                }
            }

            public static class Run
            {
                public struct Awake
                {
                    private static MethodBase method = HookableHelpers.GetBase( typeof(Awake) );
                    public delegate void Orig( global::RoR2.Run self );
                    public delegate void Hook( Orig orig, global::RoR2.Run self );
                    public static event ILContext.Manipulator Il
                    {
                        add => HookEndpointManager.Modify<Hook>( method, value );
                        remove => HookEndpointManager.Unmodify<Hook>( method, value );
                    }
                    public static event Hook On
                    {
                        add => HookEndpointManager.Add<Hook>( method, value );
                        remove => HookEndpointManager.Remove<Hook>( method, value );
                    }
                }
            }

            public static class SetStateOnHurt
            {
                public struct OnTakeDamageServer
                {
                    private static MethodBase method = HookableHelpers.GetBase( typeof(OnTakeDamageServer) );
                    public delegate void Orig( global::RoR2.SetStateOnHurt self, global::RoR2.DamageReport damageReport );
                    public delegate void Hook( Orig orig, global::RoR2.SetStateOnHurt self, global::RoR2.DamageReport damageReport );
                    public static event ILContext.Manipulator Il
                    {
                        add => HookEndpointManager.Modify<Hook>( method, value );
                        remove => HookEndpointManager.Unmodify<Hook>( method, value );
                    }
                    public static event Hook On
                    {
                        add => HookEndpointManager.Add<Hook>( method, value );
                        remove => HookEndpointManager.Remove<Hook>( method, value );
                    }
                }
            }

            public static class SkillLocator
            {
                public struct ApplyAmmoPack
                {
                    private static MethodBase method = HookableHelpers.GetBase( typeof(ApplyAmmoPack) );
                    public delegate void Orig( global::RoR2.SkillLocator self );
                    public delegate void Hook( Orig orig, global::RoR2.SkillLocator self );
                    public static event ILContext.Manipulator Il
                    {
                        add => HookEndpointManager.Modify<Hook>( method, value );
                        remove => HookEndpointManager.Unmodify<Hook>( method, value );
                    }
                    public static event Hook On
                    {
                        add => HookEndpointManager.Add<Hook>( method, value );
                        remove => HookEndpointManager.Remove<Hook>( method, value );
                    }
                }
            }

            public static class Stage
            {
                public struct OnEnable
                {
                    private static MethodBase method = HookableHelpers.GetBase( typeof(OnDisable) );
                    public delegate void Orig( global::RoR2.Stage self );
                    public delegate void Hook( Orig orig, global::RoR2.Stage self );
                    public static event ILContext.Manipulator Il
                    {
                        add => HookEndpointManager.Modify<Hook>( method, value );
                        remove => HookEndpointManager.Unmodify<Hook>( method, value );
                    }
                    public static event Hook On
                    {
                        add => HookEndpointManager.Add<Hook>( method, value );
                        remove => HookEndpointManager.Remove<Hook>( method, value );
                    }
                }

                public struct OnDisable
                {
                    private static MethodBase method = HookableHelpers.GetBase( typeof(OnDisable) );
                    public delegate void Orig( global::RoR2.Stage self );
                    public delegate void Hook( Orig orig, global::RoR2.Stage self );
                    public static event ILContext.Manipulator Il
                    {
                        add => HookEndpointManager.Modify<Hook>( method, value );
                        remove => HookEndpointManager.Unmodify<Hook>( method, value );
                    }
                    public static event Hook On
                    {
                        add => HookEndpointManager.Add<Hook>( method, value );
                        remove => HookEndpointManager.Remove<Hook>( method, value );
                    }
                }
            }

            public static class SurvivorCatalog
            {
                public struct Init
                {
                    private static MethodBase method = HookableHelpers.GetBase( typeof(Init) );
                    public delegate void Orig();
                    public delegate void Hook( Orig orig );
                    public static event ILContext.Manipulator Il
                    {
                        add => HookEndpointManager.Modify<Hook>( method, value );
                        remove => HookEndpointManager.Unmodify<Hook>( method, value );
                    }
                    public static event Hook On
                    {
                        add => HookEndpointManager.Add<Hook>( method, value );
                        remove => HookEndpointManager.Remove<Hook>( method, value );
                    }
                }
            }

            public static class Util
            {
                public struct IsPrefab
                {
                    private static MethodBase method = HookableHelpers.GetBase( typeof(IsPrefab) );
                    public delegate Boolean Orig( global::UnityEngine.GameObject gameObject );
                    public delegate Boolean Hook( Orig orig, global::UnityEngine.GameObject gameObject );
                    public static event ILContext.Manipulator Il
                    {
                        add => HookEndpointManager.Modify<Hook>( method, value );
                        remove => HookEndpointManager.Unmodify<Hook>( method, value );
                    }
                    public static event Hook On
                    {
                        add => HookEndpointManager.Add<Hook>( method, value );
                        remove => HookEndpointManager.Remove<Hook>( method, value );
                    }
                }
            }

            public struct Orbs
            {
                public static class OrbCatalog
                {
                    public struct GenerateCatalog
                    {
                        private static MethodBase method = HookableHelpers.GetBase( typeof(GenerateCatalog) );
                        public delegate void Orig();
                        public delegate void Hook( Orig orig );
                        public static event ILContext.Manipulator Il
                        {
                            add => HookEndpointManager.Modify<Hook>( method, value );
                            remove => HookEndpointManager.Unmodify<Hook>( method, value );
                        }
                        public static event Hook On
                        {
                            add => HookEndpointManager.Add<Hook>( method, value );
                            remove => HookEndpointManager.Remove<Hook>( method, value );
                        }
                    }
                }
            }

            public struct Projectile
            {
                public static class ProjectileController
                {
                    public struct Start
                    {
                        private static MethodBase method = HookableHelpers.GetBase( typeof(Start) );
                        public delegate void Orig( global::RoR2.Projectile.ProjectileController self );
                        public delegate void Hook( Orig orig, global::RoR2.Projectile.ProjectileController self );
                        public static event ILContext.Manipulator Il
                        {
                            add => HookEndpointManager.Modify<Hook>( method, value );
                            remove => HookEndpointManager.Unmodify<Hook>( method, value );
                        }
                        public static event Hook On
                        {
                            add => HookEndpointManager.Add<Hook>( method, value );
                            remove => HookEndpointManager.Remove<Hook>( method, value );
                        }
                    }
                }

                public static class ProjectileImpactExplosion
                {
                    public struct FixedUpdate
                    {
                        private static MethodBase method = HookableHelpers.GetBase( typeof(FixedUpdate) );
                        public delegate void Orig( global::RoR2.Projectile.ProjectileImpactExplosion self );
                        public delegate void Hook( Orig orig, global::RoR2.Projectile.ProjectileImpactExplosion self );
                        public static event ILContext.Manipulator Il
                        {
                            add => HookEndpointManager.Modify<Hook>( method, value );
                            remove => HookEndpointManager.Unmodify<Hook>( method, value );
                        }
                        public static event Hook On
                        {
                            add => HookEndpointManager.Add<Hook>( method, value );
                            remove => HookEndpointManager.Remove<Hook>( method, value );
                        }
                    }
                }
            }

            public struct Skills
            {
                public static class LunarPrimaryReplacementSkill
                {
                    public struct OnAssigned
                    {
                        private static MethodBase method = HookableHelpers.GetBase( typeof(OnAssigned) );
                        public delegate global::RoR2.Skills.SkillDef.BaseSkillInstanceData Orig(global::RoR2.Skills.LunarPrimaryReplacementSkill self, global::RoR2.GenericSkill skillSlot );
                        public delegate global::RoR2.Skills.SkillDef.BaseSkillInstanceData Hook( Orig orig, global::RoR2.Skills.LunarPrimaryReplacementSkill self, global::RoR2.GenericSkill skillSlot );
                        public static event ILContext.Manipulator Il
                        {
                            add => HookEndpointManager.Modify<Hook>( method, value );
                            remove => HookEndpointManager.Unmodify<Hook>( method, value );
                        }
                        public static event Hook On
                        {
                            add => HookEndpointManager.Add<Hook>( method, value );
                            remove => HookEndpointManager.Remove<Hook>( method, value );
                        }
                    }

                    public struct OnUnassigned
                    {
                        private static MethodBase method = HookableHelpers.GetBase( typeof(OnUnassigned) );
                        public delegate void Orig( global::RoR2.Skills.LunarPrimaryReplacementSkill self, global::RoR2.GenericSkill skillSlot );
                        public delegate void Hook( Orig orig, global::RoR2.Skills.LunarPrimaryReplacementSkill self, global::RoR2.GenericSkill skillSlot );
                        public static event ILContext.Manipulator Il
                        {
                            add => HookEndpointManager.Modify<Hook>( method, value );
                            remove => HookEndpointManager.Unmodify<Hook>( method, value );
                        }
                        public static event Hook On
                        {
                            add => HookEndpointManager.Add<Hook>( method, value );
                            remove => HookEndpointManager.Remove<Hook>( method, value );
                        }
                    }
                }
            }

            public struct UI
            {
                public static class CharacterSelectController
                {
                    public struct Awake
                    {
                        private static MethodBase method = HookableHelpers.GetBase( typeof(Awake) );
                        public delegate void Orig( global::RoR2.UI.CharacterSelectController self );
                        public delegate void Hook( Orig orig, global::RoR2.UI.CharacterSelectController self );
                        public static event ILContext.Manipulator Il
                        {
                            add => HookEndpointManager.Modify<Hook>( method, value );
                            remove => HookEndpointManager.Unmodify<Hook>( method, value );
                        }
                        public static event Hook On
                        {
                            add => HookEndpointManager.Add<Hook>( method, value );
                            remove => HookEndpointManager.Remove<Hook>( method, value );
                        }
                    }

                    public struct OnNetworkUserLoadoutChanged
                    {
                        private static MethodBase method = HookableHelpers.GetBase( typeof(OnNetworkUserLoadoutChanged) );
                        public delegate void Orig( global::RoR2.UI.CharacterSelectController self, global::RoR2.NetworkUser networkUser );
                        public delegate void Hook( Orig orig, global::RoR2.UI.CharacterSelectController self, global::RoR2.NetworkUser networkUser );
                        public static event ILContext.Manipulator Il
                        {
                            add => HookEndpointManager.Modify<Hook>( method, value );
                            remove => HookEndpointManager.Unmodify<Hook>( method, value );
                        }
                        public static event Hook On
                        {
                            add => HookEndpointManager.Add<Hook>( method, value );
                            remove => HookEndpointManager.Remove<Hook>( method, value );
                        }
                    }

                    public struct RebuildLocal
                    {
                        private static MethodBase method = HookableHelpers.GetBase( typeof(RebuildLocal) );
                        public delegate void Orig( global::RoR2.UI.CharacterSelectController self );
                        public delegate void Hook( Orig orig, global::RoR2.UI.CharacterSelectController self );
                        public static event ILContext.Manipulator Il
                        {
                            add => HookEndpointManager.Modify<Hook>( method, value );
                            remove => HookEndpointManager.Unmodify<Hook>( method, value );
                        }
                        public static event Hook On
                        {
                            add => HookEndpointManager.Add<Hook>( method, value );
                            remove => HookEndpointManager.Remove<Hook>( method, value );
                        }
                    }
                }

                public static class CrosshairManager
                {
                    public struct UpdateCrosshair
                    {
                        private static MethodBase method = HookableHelpers.GetBase( typeof(UpdateCrosshair) );
                        public delegate void Orig( global::RoR2.UI.CrosshairManager self, global::RoR2.CharacterBody targetBody, global::UnityEngine.Vector3 crosshairWorldPosition, global::UnityEngine.Camera uiCamera );
                        public delegate void Hook( Orig orig, global::RoR2.UI.CrosshairManager self, global::RoR2.CharacterBody targetBody, global::UnityEngine.Vector3 crosshairWorldPosition, global::UnityEngine.Camera uiCamera );
                        public static event ILContext.Manipulator Il
                        {
                            add => HookEndpointManager.Modify<Hook>( method, value );
                            remove => HookEndpointManager.Unmodify<Hook>( method, value );
                        }
                        public static event Hook On
                        {
                            add => HookEndpointManager.Add<Hook>( method, value );
                            remove => HookEndpointManager.Remove<Hook>( method, value );
                        }
                    }
                }

                public static class LoadoutPanelController
                {
                    public struct Rebuild
                    {
                        private static MethodBase method = HookableHelpers.GetBase( typeof(Rebuild) );
                        public delegate void Orig( global::RoR2.UI.LoadoutPanelController self );
                        public delegate void Hook( Orig orig, global::RoR2.UI.LoadoutPanelController self );
                        public static event ILContext.Manipulator Il
                        {
                            add => HookEndpointManager.Modify<Hook>( method, value );
                            remove => HookEndpointManager.Unmodify<Hook>( method, value );
                        }
                        public static event Hook On
                        {
                            add => HookEndpointManager.Add<Hook>( method, value );
                            remove => HookEndpointManager.Remove<Hook>( method, value );
                        }
                    }
                }

                public static class PauseScreenController
                {
                    public struct OnEnable
                    {
                        private static MethodBase method = HookableHelpers.GetBase( typeof(OnEnable) );
                        public delegate void Orig( global::RoR2.UI.PauseScreenController self );
                        public delegate void Hook( Orig orig, global::RoR2.UI.PauseScreenController self );
                        public static event ILContext.Manipulator Il
                        {
                            add => HookEndpointManager.Modify<Hook>( method, value );
                            remove => HookEndpointManager.Unmodify<Hook>( method, value );
                        }
                        public static event Hook On
                        {
                            add => HookEndpointManager.Add<Hook>( method, value );
                            remove => HookEndpointManager.Remove<Hook>( method, value );
                        }
                    }

                    public struct OnDisable
                    {
                        private static MethodBase method = HookableHelpers.GetBase( typeof(OnDisable) );
                        public delegate void Orig(global::RoR2.UI.PauseScreenController self);
                        public delegate void Hook( Orig orig, global::RoR2.UI.PauseScreenController self );
                        public static event ILContext.Manipulator Il
                        {
                            add => HookEndpointManager.Modify<Hook>( method, value );
                            remove => HookEndpointManager.Unmodify<Hook>( method, value );
                        }
                        public static event Hook On
                        {
                            add => HookEndpointManager.Add<Hook>( method, value );
                            remove => HookEndpointManager.Remove<Hook>( method, value );
                        }
                    }
                }

                public static class QuickPlayButtonController
                {
                    public struct Start
                    {
                        private static MethodBase method = HookableHelpers.GetBase( typeof(Start) );
                        public delegate void Orig( global::RoR2.UI.QuickPlayButtonController self );
                        public delegate void Hook( Orig orig, global::RoR2.UI.QuickPlayButtonController self );
                        public static event ILContext.Manipulator Il
                        {
                            add => HookEndpointManager.Modify<Hook>( method, value );
                            remove => HookEndpointManager.Unmodify<Hook>( method, value );
                        }
                        public static event Hook On
                        {
                            add => HookEndpointManager.Add<Hook>( method, value );
                            remove => HookEndpointManager.Remove<Hook>( method, value );
                        }
                    }
                }
            }
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning restore IDE1006 // Naming Styles
    }
}
