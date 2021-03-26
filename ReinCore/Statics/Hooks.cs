namespace ReinCore
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using MonoMod.Cil;
    using MonoMod.RuntimeDetour.HookGen;

    using RoR2;

    using UnityEngine;
    using UnityEngine.Networking;

    using UnityObject = UnityEngine.Object;
    using Object = System.Object;



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




    public static class HooksCore
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable IDE1006 // Naming Styles
        public struct EntityStates
        {
            public static class ShockState
            {
                public struct FixedUpdate
                {
                    private static readonly MethodBase method = HookHelpers.GetBase( typeof(FixedUpdate) );
                    public delegate void Orig(global::EntityStates.ShockState self);
                    public delegate void Hook(Orig orig, global::EntityStates.ShockState self);
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

                public struct OnEnter
                {
                    private static readonly MethodBase method = HookHelpers.GetBase( typeof(OnEnter) );
                    public delegate void Orig(global::EntityStates.ShockState self);
                    public delegate void Hook(Orig orig, global::EntityStates.ShockState self);
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
                    private static readonly MethodBase method = HookHelpers.GetBase( typeof(OnExit) );
                    public delegate void Orig(global::EntityStates.ShockState self);
                    public delegate void Hook(Orig orig, global::EntityStates.ShockState self);
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

            public struct Captain
            {
                public struct Weapon
                {
                    public static class FireCaptainShotgun
                    {
                        public struct ModifyBullet
                        {
                            private static readonly MethodBase method = HookHelpers.GetBase( typeof(ModifyBullet) );
                            public delegate void Orig(global::EntityStates.Captain.Weapon.FireCaptainShotgun self, global::RoR2.BulletAttack bullet);
                            public delegate void Hook(Orig orig, global::EntityStates.Captain.Weapon.FireCaptainShotgun self, global::RoR2.BulletAttack bullet);
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

                        public struct OnEnter
                        {
                            private static readonly MethodBase method = HookHelpers.GetBase( typeof(OnEnter) );
                            public delegate void Orig(global::EntityStates.Captain.Weapon.FireCaptainShotgun self);
                            public delegate void Hook(Orig orig, global::EntityStates.Captain.Weapon.FireCaptainShotgun self);
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

                    public static class ChargeCaptainShotgun
                    {
                        public struct OnEnter
                        {
                            private static readonly MethodBase method = HookHelpers.GetBase( typeof(OnEnter) );
                            public delegate void Orig(global::EntityStates.Captain.Weapon.ChargeCaptainShotgun self);
                            public delegate void Hook(Orig orig, global::EntityStates.Captain.Weapon.ChargeCaptainShotgun self);
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

                    public static class CallAirstrike1
                    {
                        public struct OnEnter
                        {
                            private static readonly MethodBase method = HookHelpers.GetBase( typeof(OnEnter) );
                            public delegate void Orig(global::EntityStates.Captain.Weapon.CallAirstrike1 self);
                            public delegate void Hook(Orig orig, global::EntityStates.Captain.Weapon.CallAirstrike1 self);
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
                    public static class CallAirstrike2
                    {
                        public struct OnEnter
                        {
                            private static readonly MethodBase method = HookHelpers.GetBase( typeof(OnEnter) );
                            public delegate void Orig(global::EntityStates.Captain.Weapon.CallAirstrike2 self);
                            public delegate void Hook(Orig orig, global::EntityStates.Captain.Weapon.CallAirstrike2 self);
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
                    public static class CallAirstrike3
                    {
                        public struct OnEnter
                        {
                            private static readonly MethodBase method = HookHelpers.GetBase( typeof(OnEnter) );
                            public delegate void Orig(global::EntityStates.Captain.Weapon.CallAirstrike3 self);
                            public delegate void Hook(Orig orig, global::EntityStates.Captain.Weapon.CallAirstrike3 self);
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

            public struct CaptainSupplyDrop
            {
                public static class ShockZoneMainState
                {
                    public struct Shock
                    {
                        private static readonly MethodBase method = HookHelpers.GetBase( typeof(Shock) );
                        public delegate void Orig(global::EntityStates.CaptainSupplyDrop.ShockZoneMainState self);
                        public delegate void Hook(Orig orig, global::EntityStates.CaptainSupplyDrop.ShockZoneMainState self);
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

            public struct Commando
            {
                public static class DodgeState
                {
                    public struct OnEnter
                    {
                        private static readonly MethodBase method = HookHelpers.GetBase( typeof(OnEnter) );
                        public delegate void Orig(global::EntityStates.Commando.DodgeState self);
                        public delegate void Hook(Orig orig, global::EntityStates.Commando.DodgeState self);
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
                        private static readonly MethodBase method = HookHelpers.GetBase( typeof(FixedUpdate) );
                        public delegate void Orig(global::EntityStates.GoldGat.BaseGoldGatState self);
                        public delegate void Hook(Orig orig, global::EntityStates.GoldGat.BaseGoldGatState self);
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

            public struct HermitCrab
            {
                public static class FireMortar
                {
                    public struct FixedUpdate
                    {
                        private static readonly MethodBase method = HookHelpers.GetBase( typeof(FixedUpdate) );
                        public delegate void Orig(global::EntityStates.HermitCrab.FireMortar self);
                        public delegate void Hook(Orig orig, global::EntityStates.HermitCrab.FireMortar self);
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

            public struct Huntress
            {
                public struct HuntressWeapon
                {
                    public static class FireFlurrySeekingArrow
                    {

                    }

                    public static class FireSeekingArrow
                    {
                        public struct FireOrbArrow
                        {
                            private static readonly MethodBase method = HookHelpers.GetBase( typeof(FireOrbArrow) );
                            public delegate void Orig(global::EntityStates.Huntress.HuntressWeapon.FireSeekingArrow self);
                            public delegate void Hook(Orig orig, global::EntityStates.Huntress.HuntressWeapon.FireSeekingArrow self);
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

                        public struct OnEnter
                        {
                            private static readonly MethodBase method = HookHelpers.GetBase( typeof(OnEnter) );
                            public delegate void Orig(global::EntityStates.Huntress.HuntressWeapon.FireSeekingArrow self);
                            public delegate void Hook(Orig orig, global::EntityStates.Huntress.HuntressWeapon.FireSeekingArrow self);
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

                    public static class ThrowGlaive
                    {
                        public struct OnEnter
                        {
                            private static readonly MethodBase method = HookHelpers.GetBase( typeof(OnEnter) );
                            public delegate void Orig(global::EntityStates.Huntress.HuntressWeapon.ThrowGlaive self);
                            public delegate void Hook(Orig orig, global::EntityStates.Huntress.HuntressWeapon.ThrowGlaive self);
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

            public struct Mage
            {
                public struct Weapon
                {
                    public static class ChargeNovabomb
                    {
                        public struct OnEnter
                        {
                            private static readonly MethodBase method = HookHelpers.GetBase( typeof(OnEnter) );
                            public delegate void Orig(global::EntityStates.Mage.Weapon.ChargeNovabomb self);
                            public delegate void Hook(Orig orig, global::EntityStates.Mage.Weapon.ChargeNovabomb self);
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
                            private static readonly MethodBase method = HookHelpers.GetBase( typeof(FixedUpdate) );
                            public delegate void Orig(global::EntityStates.Mage.Weapon.ChargeNovabomb self);
                            public delegate void Hook(Orig orig, global::EntityStates.Mage.Weapon.ChargeNovabomb self);
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
                            private static readonly MethodBase method = HookHelpers.GetBase( typeof(FireNovaBomb) );
                            public delegate void Orig(global::EntityStates.Mage.Weapon.ChargeNovabomb self);
                            public delegate void Hook(Orig orig, global::EntityStates.Mage.Weapon.ChargeNovabomb self);
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
                            private static readonly MethodBase method = HookHelpers.GetBase( typeof(OnExit) );
                            public delegate void Orig(global::EntityStates.Mage.Weapon.ChargeNovabomb self);
                            public delegate void Hook(Orig orig, global::EntityStates.Mage.Weapon.ChargeNovabomb self);
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
                            private static readonly MethodBase method = HookHelpers.GetBase( typeof(FireGauntlet) );
                            public delegate void Orig(global::EntityStates.Mage.Weapon.ChargeNovabomb self);
                            public delegate void Hook(Orig orig, global::EntityStates.Mage.Weapon.ChargeNovabomb self);
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
                            private static readonly MethodBase method = HookHelpers.GetBase( typeof(OnEnter) );
                            public delegate void Orig(global::EntityStates.Mage.Weapon.Flamethrower self);
                            public delegate void Hook(Orig orig, global::EntityStates.Mage.Weapon.Flamethrower self);
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
                            private static readonly MethodBase method = HookHelpers.GetBase( typeof(FixedUpdate) );
                            public delegate void Orig(global::EntityStates.Mage.Weapon.Flamethrower self);
                            public delegate void Hook(Orig orig, global::EntityStates.Mage.Weapon.Flamethrower self);
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
                            private static readonly MethodBase method = HookHelpers.GetBase( typeof(OnExit) );
                            public delegate void Orig(global::EntityStates.Mage.Weapon.Flamethrower self);
                            public delegate void Hook(Orig orig, global::EntityStates.Mage.Weapon.Flamethrower self);
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
                            private static readonly MethodBase method = HookHelpers.GetBase( typeof(OnEnter) );
                            public delegate void Orig(global::EntityStates.Mage.Weapon.PrepWall self);
                            public delegate void Hook(Orig orig, global::EntityStates.Mage.Weapon.PrepWall self);
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
                            private static readonly MethodBase method = HookHelpers.GetBase( typeof(OnExit) );
                            public delegate void Orig(global::EntityStates.Mage.Weapon.PrepWall self);
                            public delegate void Hook(Orig orig, global::EntityStates.Mage.Weapon.PrepWall self);
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
            public static class AchievementManager
            {
                public struct CollectAchievementDefs
                {
                    private static readonly MethodBase method = HookHelpers.GetBase( typeof(CollectAchievementDefs) );
                    public delegate void Orig(Dictionary<String, AchievementDef> map);
                    public delegate void Hook(Orig orig, Dictionary<String, AchievementDef> map);
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

            public static class BodyCatalog
            {
                public struct Init
                {
                    private static readonly MethodBase method = HookHelpers.GetBase( typeof(Init) );
                    public delegate void Orig();
                    public delegate void Hook(Orig orig);
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

                public struct SetBodyPrefabs
                {
                    private static readonly MethodBase method = HookHelpers.GetBase( typeof(SetBodyPrefabs) );
                    public delegate void Orig(global::UnityEngine.GameObject[] prefabs);
                    public delegate void Hook(Orig orig, global::UnityEngine.GameObject[] prefabs);
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

            public static class BuffCatalog
            {
                public struct Init
                {
                    private static readonly MethodBase method = HookHelpers.GetBase( typeof(Init) );
                    public delegate void Orig();
                    public delegate void Hook(Orig orig);
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

            public static class BulletAttack
            {
                public struct DefaultHitCallback
                {
                    private static readonly MethodBase method = HookHelpers.GetBase( typeof(DefaultHitCallback) );
                    public delegate Boolean Orig(global::RoR2.BulletAttack self, ref global::RoR2.BulletAttack.BulletHit hitInfo);
                    public delegate Boolean Hook(Orig orig, global::RoR2.BulletAttack self, ref global::RoR2.BulletAttack.BulletHit hitInfo);
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
                    private static readonly MethodBase method = HookHelpers.GetBase( typeof(Start) );
                    public delegate void Orig(global::RoR2.CameraRigController self);
                    public delegate void Hook(Orig orig, global::RoR2.CameraRigController self);
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
                    private static readonly MethodBase method = HookHelpers.GetBase( typeof(Update) );
                    public delegate void Orig(global::RoR2.CameraRigController self);
                    public delegate void Hook(Orig orig, global::RoR2.CameraRigController self);
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

            public static class CameraTargetParams
            {
                public struct Update
                {
                    private static readonly MethodBase method = HookHelpers.GetBase( typeof(Update) );
                    public delegate void Orig(global::RoR2.CameraTargetParams self);
                    public delegate void Hook(Orig orig, global::RoR2.CameraTargetParams self);
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

            public static class CaptainDefenseMatrixController
            {
                public struct Start
                {
                    private static readonly MethodBase method = HookHelpers.GetBase( typeof(Start) );
                    public delegate void Orig(global::RoR2.CaptainDefenseMatrixController self);
                    public delegate void Hook(Orig orig, global::RoR2.CaptainDefenseMatrixController self);
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
                    private static readonly MethodBase method = HookHelpers.GetBase( typeof(FixedUpdate) );
                    public delegate void Orig(global::RoR2.CharacterBody self);
                    public delegate void Hook(Orig orig, global::RoR2.CharacterBody self);
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

                public struct HandleConstructTurret
                {
                    private static readonly MethodBase method = HookHelpers.GetBase( typeof(HandleConstructTurret) );
                    public delegate void Orig(NetworkMessage netMsg);
                    public delegate void Hook(Orig orig, NetworkMessage netMsg);
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
                    private static readonly MethodBase method = HookHelpers.GetBase( typeof(RecalculateStats) );
                    public delegate void Orig(global::RoR2.CharacterBody self);
                    public delegate void Hook(Orig orig, global::RoR2.CharacterBody self);
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
                    private static readonly MethodBase method = HookHelpers.GetBase( typeof(Start) );
                    public delegate void Orig(global::RoR2.CharacterBody self);
                    public delegate void Hook(Orig orig, global::RoR2.CharacterBody self);
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
                public struct UpdatePowerWardSummon
                {
                    private static readonly MethodBase method = HookHelpers.GetBase( typeof(UpdatePowerWardSummon) );
                    public delegate void Orig(global::RoR2.CharacterBody self);
                    public delegate void Hook(Orig orig, global::RoR2.CharacterBody self);
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

            public static class CharacterMaster
            {
                public struct GetDeployableSameSlotLimit
                {
                    private static readonly MethodBase method = HookHelpers.GetBase( typeof(GetDeployableSameSlotLimit) );
                    public delegate Int32 Orig(global::RoR2.CharacterMaster self, global::RoR2.DeployableSlot slot);
                    public delegate Int32 Hook(Orig orig, global::RoR2.CharacterMaster self, global::RoR2.DeployableSlot slot);
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

                public struct OnBodyStart
                {
                    private static readonly MethodBase method = HookHelpers.GetBase( typeof(OnBodyStart) );
                    public delegate Int32 Orig(global::RoR2.CharacterMaster self, global::RoR2.CharacterBody body);
                    public delegate Int32 Hook(Orig orig, global::RoR2.CharacterMaster self, global::RoR2.CharacterBody body);
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

            public static class CharacterModel
            {
                public struct UpdateRendererMaterials
                {
                    private static readonly MethodBase method = HookHelpers.GetBase( typeof(UpdateRendererMaterials) );
                    public delegate Single Orig(global::RoR2.CharacterModel self, Renderer renderer, Material material, Boolean ignoreOverlays);
                    public delegate Single Hook(Orig orig, global::RoR2.CharacterModel self, Renderer renderer, Material material, Boolean ignoreOverlays);
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
                    private static readonly MethodBase method = HookHelpers.GetBase( typeof(Awake) );
                    public delegate void Orig(global::RoR2.CharacterSpawnCard self);
                    public delegate void Hook(Orig orig, global::RoR2.CharacterSpawnCard self);
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
                    private static readonly MethodBase method = HookHelpers.GetBase( typeof(Awake) );
                    public delegate void Orig(global::RoR2.ClassicStageInfo self);
                    public delegate void Hook(Orig orig, global::RoR2.ClassicStageInfo self);
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

            public static class DamageColor
            {
                public struct FindColor
                {
                    private static readonly MethodBase method = HookHelpers.GetBase( typeof(FindColor) );
                    public delegate void Orig(global::RoR2.DamageColorIndex colorIndex);
                    public delegate void Hook(Orig orig, global::RoR2.DamageColorIndex colorIndex);
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

            public static class DeathRewards
            {
                public struct OnKilledServer
                {
                    private static readonly MethodBase method = HookHelpers.GetBase( typeof(OnKilledServer) );
                    public delegate void Orig(global::RoR2.DeathRewards self, global::RoR2.DamageReport report);
                    public delegate void Hook(Orig orig, global::RoR2.DeathRewards self, global::RoR2.DamageReport report);
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

            public static class DisableIfGameModded
            {
                public struct OnEnable
                {
                    private static readonly MethodBase method = HookHelpers.GetBase( typeof(OnEnable) );
                    public delegate void Orig(global::RoR2.DisableIfGameModded self);
                    public delegate void Hook(Orig orig, global::RoR2.DisableIfGameModded self);
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

            public static class DotController
            {
                //public struct AddDot
                //{
                //    private static readonly MethodBase method = HookHelpers.GetBase( typeof(AddDot) );
                //    public delegate void Orig(global::RoR2.DotController self, global::UnityEngine.GameObject attacker, global::System.Single duration, global::RoR2.DotController.DotIndex dotIndex, global::System.Single damageMultiplier);
                //    public delegate void Hook(Orig orig, global::RoR2.DotController self, global::UnityEngine.GameObject attacker, global::System.Single duration, global::RoR2.DotController.DotIndex dotIndex, global::System.Single damageMultiplier);
                //    public static event ILContext.Manipulator Il
                //    {
                //        add => HookEndpointManager.Modify<Hook>( method, value );
                //        remove => HookEndpointManager.Unmodify<Hook>( method, value );
                //    }
                //    public static event Hook On
                //    {
                //        add => HookEndpointManager.Add<Hook>( method, value );
                //        remove => HookEndpointManager.Remove<Hook>( method, value );
                //    }
                //}

                public struct Awake
                {
                    private static readonly MethodBase method = HookHelpers.GetBase( typeof(Awake) );
                    public delegate void Orig(global::RoR2.DotController self);
                    public delegate void Hook(Orig orig, global::RoR2.DotController self);
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

                public struct EvaluateDotStacksForType
                {
                    private static readonly MethodBase method = HookHelpers.GetBase( typeof(EvaluateDotStacksForType) );
                    public delegate void Orig(global::RoR2.DotController self, global::RoR2.DotController.DotIndex dotIndex, global::System.Single dt, out global::System.Int32 remainingActive);
                    public delegate void Hook(Orig orig, global::RoR2.DotController self, global::RoR2.DotController.DotIndex dotIndex, global::System.Single dt, out global::System.Int32 remainingActive);
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
                    private static readonly MethodBase method = HookHelpers.GetBase( typeof(FixedUpdate) );
                    public delegate void Orig(global::RoR2.DotController self);
                    public delegate void Hook(Orig orig, global::RoR2.DotController self);
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

                public struct GetDotDef
                {
                    public static Boolean INLINED = true;
                    //private static readonly MethodBase method = HookableHelpers.GetBase( typeof(GetDotDef) );
                    //public delegate System.Object Orig( global::RoR2.DotController self, global::RoR2.DotController.DotIndex dotIndex );
                    //public delegate System.Object Hook( Orig orig, global::RoR2.DotController self, global::RoR2.DotController.DotIndex dotIndex );
                    //public static event ILContext.Manipulator Il
                    //{
                    //    add => HookEndpointManager.Modify<Hook>( method, value );
                    //    remove => HookEndpointManager.Unmodify<Hook>( method, value );
                    //}
                    //public static event Hook On
                    //{
                    //    add => HookEndpointManager.Add<Hook>( method, value );
                    //    remove => HookEndpointManager.Remove<Hook>( method, value );
                    //}
                }

                public struct RemoveAllDots
                {
                    private static readonly MethodBase method = HookHelpers.GetBase( typeof(RemoveAllDots) );
                    public delegate void Orig(global::UnityEngine.GameObject target);
                    public delegate void Hook(Orig orig, global::UnityEngine.GameObject target);
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

            public static class EclipseRun
            {
                public struct GetEclipseBaseUnlockableString
                {
                    private static readonly MethodBase method = HookHelpers.GetBase( typeof(GetEclipseBaseUnlockableString) );
                    public delegate String Orig();
                    public delegate String Hook(Orig orig);
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

                public struct OnClientGameOver
                {
                    private static readonly MethodBase method = HookHelpers.GetBase( typeof(OnClientGameOver) );
                    public delegate String Orig(global::RoR2.EclipseRun self, global::RoR2.RunReport report);
                    public delegate String Hook(Orig orig, global::RoR2.EclipseRun self, global::RoR2.RunReport report);
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

                public struct OverrideRuleChoices
                {
                    private static readonly MethodBase method = HookHelpers.GetBase( typeof(OverrideRuleChoices) );
                    public delegate void Orig(global::RoR2.EclipseRun self, global::RoR2.RuleChoiceMask mustInclude, global::RoR2.RuleChoiceMask mustExclude, UInt64 seed);
                    public delegate void Hook(Orig orig, global::RoR2.EclipseRun self, global::RoR2.RuleChoiceMask mustInclude, global::RoR2.RuleChoiceMask mustExclude, UInt64 seed);
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
                //Removed
                //public struct GetDefaultEffectDefs
                //{
                //    private static readonly MethodBase method = HookHelpers.GetBase( typeof(GetDefaultEffectDefs) );
                //    public delegate global::RoR2.EffectDef[] Orig();
                //    public delegate global::RoR2.EffectDef[] Hook(Orig orig);
                //    public static event ILContext.Manipulator Il
                //    {
                //        add => HookEndpointManager.Modify<Hook>( method, value );
                //        remove => HookEndpointManager.Unmodify<Hook>( method, value );
                //    }
                //    public static event Hook On
                //    {
                //        add => HookEndpointManager.Add<Hook>( method, value );
                //        remove => HookEndpointManager.Remove<Hook>( method, value );
                //    }
                //}
                public struct Init
                {
                    private static readonly MethodBase method = HookHelpers.GetBase( typeof(Init) );
                    public delegate void Orig();
                    public delegate void Hook(Orig orig);
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
                public struct SetEntries
                {
                    private static readonly MethodBase method = HookHelpers.GetBase( typeof(SetEntries) );
                    public delegate void Orig(global::RoR2.EffectDef[] effects);
                    public delegate void Hook(Orig orig, global::RoR2.EffectDef[] effects);
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
                    private static readonly MethodBase method = HookHelpers.GetBase( typeof(SpawnEffect___void_EffectIndex_EffectData_Boolean), true, CallingConventions.Standard,  typeof(global::RoR2.EffectIndex), typeof(global::RoR2.EffectData), typeof(global::System.Boolean) );
                    public delegate void Orig(global::RoR2.EffectIndex index, global::RoR2.EffectData data, global::System.Boolean transmit);
                    public delegate void Hook(Orig orig, global::RoR2.EffectIndex index, global::RoR2.EffectData data, global::System.Boolean transmit);

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

            public static class EntityStateCatalog
            {
                public struct Init
                {
                    private static readonly MethodBase method = HookHelpers.GetBase(typeof(Init));
                    public delegate void Orig();
                    public delegate void Hook(Orig orig);

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

                public struct SetElements
                {
                    private static readonly MethodBase method = HookHelpers.GetBase(typeof(SetElements));
                    public delegate void Orig(global::System.Type[] types, global::RoR2.EntityStateConfiguration[] configs);
                    public delegate void Hook(Orig orig, global::System.Type[] types, global::RoR2.EntityStateConfiguration[] configs);

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
                    private static readonly MethodBase method = HookHelpers.GetBase( typeof(FixedUpdate) );
                    public delegate void Orig(global::RoR2.EquipmentSlot self);
                    public delegate void Hook(Orig orig, global::RoR2.EquipmentSlot self);
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
                    private static readonly MethodBase method = HookHelpers.GetBase( typeof(GrantEquipment) );
                    public delegate void Orig(global::RoR2.GenericPickupController self, global::RoR2.CharacterBody body, global::RoR2.Inventory inventory);
                    public delegate void Hook(Orig orig, global::RoR2.GenericPickupController self, global::RoR2.CharacterBody body, global::RoR2.Inventory inventory);
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
                public struct OnCharacterDeath
                {
                    private static readonly MethodBase method = HookHelpers.GetBase( typeof(OnCharacterDeath) );
                    public delegate void Orig(global::RoR2.GlobalEventManager self, global::RoR2.DamageReport damageReport);
                    public delegate void Hook(Orig orig, global::RoR2.GlobalEventManager self, global::RoR2.DamageReport damageReport);
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

                public struct OnCharacterHitGround
                {
                    private static readonly MethodBase method = HookHelpers.GetBase( typeof(OnCharacterHitGround) );
                    public delegate void Orig(global::RoR2.GlobalEventManager self, global::RoR2.CharacterBody body, Vector3 impactVelocity);
                    public delegate void Hook(Orig orig, global::RoR2.GlobalEventManager self, global::RoR2.CharacterBody body, Vector3 impactVelocity);
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

                public struct OnHitEnemy
                {
                    private static readonly MethodBase method = HookHelpers.GetBase( typeof(OnHitEnemy) );
                    public delegate void Orig(global::RoR2.GlobalEventManager self, global::RoR2.DamageInfo damageInfo, global::UnityEngine.GameObject victim);
                    public delegate void Hook(Orig orig, global::RoR2.GlobalEventManager self, global::RoR2.DamageInfo damageInfo, global::UnityEngine.GameObject victim);
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

                public struct OnTeamLevelUp
                {
                    private static readonly MethodBase method = HookHelpers.GetBase( typeof(OnTeamLevelUp) );
                    public delegate void Orig(global::RoR2.TeamIndex team);
                    public delegate void Hook(Orig orig, global::RoR2.TeamIndex team);
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
                        private static readonly MethodBase method = HookHelpers.GetBase( typeof(FixedUpdate) );
                        public delegate void Orig(System.Object self);
                        public delegate void Hook(Orig orig, System.Object self);
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
                    private static readonly MethodBase method = HookHelpers.GetBase( typeof(Heal) );
                    public delegate void Orig(global::RoR2.HealthComponent self, global::RoR2.ProcChainMask procChainMask, Boolean nonRegen);
                    public delegate void Hook(Orig orig, global::RoR2.HealthComponent self, global::RoR2.ProcChainMask procChainMask, Boolean nonRegen);
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
                    private static readonly MethodBase method = HookHelpers.GetBase( typeof(TakeDamage) );
                    public delegate void Orig(global::RoR2.HealthComponent self, global::RoR2.DamageInfo damageInfo);
                    public delegate void Hook(Orig orig, global::RoR2.HealthComponent self, global::RoR2.DamageInfo damageInfo);
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

            public static class HoldoutZoneController
            {
                public struct FixedUpdate
                {
                    private static readonly MethodBase method = HookHelpers.GetBase( typeof(FixedUpdate) );
                    public delegate void Orig(global::RoR2.HoldoutZoneController self);
                    public delegate void Hook(Orig orig, global::RoR2.HoldoutZoneController self);
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
                public static class FocusConvergenceController
                {
                    public struct ApplyRadius
                    {
                        private static readonly MethodBase method = HookHelpers.GetBase( typeof(ApplyRadius) );
                        public delegate void Orig(System.Object self, ref Single radius);
                        public delegate void Hook(Orig orig, System.Object self, ref Single radius);
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

                    public struct ApplyRate
                    {

                        private static readonly MethodBase method = HookHelpers.GetBase( typeof(ApplyRate) );
                        public delegate void Orig(System.Object self, ref Single rate);
                        public delegate void Hook(Orig orig, System.Object self, ref Single rate);
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

            public static class HuntressTracker
            {
                public struct FixedUpdate
                {
                    private static readonly MethodBase method = HookHelpers.GetBase( typeof(FixedUpdate) );
                    public delegate void Orig(global::RoR2.HuntressTracker self);
                    public delegate void Hook(Orig orig, global::RoR2.HuntressTracker self);
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
                    private static readonly MethodBase method = HookHelpers.GetBase( typeof(CalculateEquipmentCooldownScale) );
                    public delegate Single Orig(global::RoR2.Inventory self);
                    public delegate Single Hook(Orig orig, global::RoR2.Inventory self);
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
                    private static readonly MethodBase method = HookHelpers.GetBase( typeof(SetActiveEquipmentSlot) );
                    public delegate void Orig(global::RoR2.Inventory self, Byte slotIndex);
                    public delegate void Hook(Orig orig, global::RoR2.Inventory self, Byte slotIndex);
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
                    private static readonly MethodBase method = HookHelpers.GetBase( typeof(UpdateEquipment) );
                    public delegate void Orig(global::RoR2.Inventory self);
                    public delegate void Hook(Orig orig, global::RoR2.Inventory self);
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

            public static class MasterCatalog
            {
                public struct Init
                {
                    private static readonly MethodBase method = HookHelpers.GetBase( typeof(Init) );
                    public delegate void Orig();
                    public delegate void Hook(Orig orig);
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

                public struct SetEntries
                {
                    private static readonly MethodBase method = HookHelpers.GetBase( typeof(SetEntries) );
                    public delegate void Orig(global::UnityEngine.GameObject[] prefabs);
                    public delegate void Hook(Orig orig, global::UnityEngine.GameObject[] prefabs);
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

            public static class PlayerCharacterMasterController
            {
                public struct FixedUpdate
                {
                    private static readonly MethodBase method = HookHelpers.GetBase( typeof(FixedUpdate) );
                    public delegate void Orig(global::RoR2.PlayerCharacterMasterController self);
                    public delegate void Hook(Orig orig, global::RoR2.PlayerCharacterMasterController self);
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

            public static class ProjectileCatalog
            {
                public struct Init
                {
                    private static readonly MethodBase method = HookHelpers.GetBase( typeof(Init) );
                    public delegate void Orig();
                    public delegate void Hook(Orig orig);
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

                public struct SetProjectilePrefabs
                {
                    private static readonly MethodBase method = HookHelpers.GetBase( typeof(SetProjectilePrefabs) );
                    public delegate void Orig(global::UnityEngine.GameObject[] prefabs);
                    public delegate void Hook(Orig orig, global::UnityEngine.GameObject[] prefabs);
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
                public struct OnLoad
                {
                    private static readonly MethodBase method = HookHelpers.GetBase( typeof(OnLoad) );
                    public delegate void Orig(global::RoR2.RoR2Application self);
                    public delegate void Hook(Orig orig, global::RoR2.RoR2Application self);
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

            public static class Run
            {
                public struct Awake
                {
                    private static readonly MethodBase method = HookHelpers.GetBase( typeof(Awake) );
                    public delegate void Orig(global::RoR2.Run self);
                    public delegate void Hook(Orig orig, global::RoR2.Run self);
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

                public struct OnFixedUpdate
                {
                    private static readonly MethodBase method = HookHelpers.GetBase( typeof(OnFixedUpdate) );
                    public delegate void Orig(global::RoR2.Run self);
                    public delegate void Hook(Orig orig, global::RoR2.Run self);
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

                public struct RecalculateDifficultyCoefficentInternal
                {
                    private static readonly MethodBase method = HookHelpers.GetBase( typeof(RecalculateDifficultyCoefficentInternal) );
                    public delegate void Orig(global::RoR2.Run self);
                    public delegate void Hook(Orig orig, global::RoR2.Run self);
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

            public static class SceneInfo
            {
                public struct Awake
                {
                    private static readonly MethodBase method = HookHelpers.GetBase( typeof(Awake) );
                    public delegate void Orig(global::RoR2.SceneInfo self);
                    public delegate void Hook(Orig orig, global::RoR2.SceneInfo self);
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
                    private static readonly MethodBase method = HookHelpers.GetBase( typeof(OnTakeDamageServer) );
                    public delegate void Orig(global::RoR2.SetStateOnHurt self, global::RoR2.DamageReport damageReport);
                    public delegate void Hook(Orig orig, global::RoR2.SetStateOnHurt self, global::RoR2.DamageReport damageReport);
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
                    private static readonly MethodBase method = HookHelpers.GetBase( typeof(ApplyAmmoPack) );
                    public delegate void Orig(global::RoR2.SkillLocator self);
                    public delegate void Hook(Orig orig, global::RoR2.SkillLocator self);
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

                public struct FindSkillSlot
                {
                    private static readonly MethodBase method = HookHelpers.GetBase( typeof(FindSkillSlot) );
                    public delegate global::RoR2.SkillSlot Orig(global::RoR2.SkillLocator self, global::RoR2.GenericSkill skillComponent);
                    public delegate global::RoR2.SkillSlot Hook(Orig orig, global::RoR2.SkillLocator self, global::RoR2.GenericSkill skillComponent);
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

            public static class SkinDef
            {
                public struct Awake
                {
                    private static readonly MethodBase method = HookHelpers.GetBase( typeof(Awake) );
                    public delegate void Orig(global::RoR2.SkinDef self);
                    public delegate void Hook(Orig orig, global::RoR2.SkinDef self);
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
                    private static readonly MethodBase method = HookHelpers.GetBase( typeof(OnDisable) );
                    public delegate void Orig(global::RoR2.Stage self);
                    public delegate void Hook(Orig orig, global::RoR2.Stage self);
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
                    private static readonly MethodBase method = HookHelpers.GetBase( typeof(OnDisable) );
                    public delegate void Orig(global::RoR2.Stage self);
                    public delegate void Hook(Orig orig, global::RoR2.Stage self);
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
                    private static readonly MethodBase method = HookHelpers.GetBase( typeof(Init) );
                    public delegate void Orig();
                    public delegate void Hook(Orig orig);
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

                public struct SetSurvivorDefs
                {
                    private static readonly MethodBase method = HookHelpers.GetBase( typeof(SetSurvivorDefs) );
                    public delegate void Orig(global::RoR2.SurvivorDef[] prefabs);
                    public delegate void Hook(Orig orig, global::RoR2.SurvivorDef[] prefabs);
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

            public static class SystemInitializerAttribute
            {
                public struct Execute
                {
                    private static readonly MethodBase method = HookHelpers.GetBase( typeof(Execute) );
                    public delegate void Orig();
                    public delegate void Hook(Orig orig);
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

            public static class TeamManager
            {
                public struct InitialCalcExperience
                {
                    private static readonly MethodBase method = HookHelpers.GetBase( typeof(InitialCalcExperience) );
                    public delegate Double Orig(Double level, Double experienceForFirstLevelUp, Double growthRate);
                    public delegate Double Hook(Orig orig, Double level, Double experienceForFirstLevelUp, Double growthRate);
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
                public struct GiveTeamExperience
                {
                    private static readonly MethodBase method = HookHelpers.GetBase( typeof(GiveTeamExperience) );
                    public delegate void Orig(global::RoR2.TeamManager self, global::RoR2.TeamIndex teamIndex, UInt64 experience);
                    public delegate void Hook(Orig orig, global::RoR2.TeamManager self, global::RoR2.TeamIndex teamIndex, UInt64 experience);
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

            public static class UnlockableCatalog
            {
                public struct Init
                {
                    private static readonly MethodBase method = HookHelpers.GetBase( typeof(Init) );
                    public delegate void Orig();
                    public delegate void Hook(Orig orig);
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

            public static class UnitySystemConsoleRedirector
            {
                public struct Redirect
                {
                    private static readonly MethodBase method = HookHelpers.GetBase( typeof(Redirect) );
                    public delegate void Orig();
                    public delegate void Hook(Orig orig);
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

            public static class UserProfile
            {
                public struct LoadUserProfileFromDisk
                {
                    private static readonly MethodBase method = HookHelpers.GetBase(typeof(LoadUserProfileFromDisk));
                    public delegate global::RoR2.UserProfile.LoadUserProfileOperationResult Orig(global::Zio.IFileSystem fileSystem, global::Zio.UPath path);
                    public delegate global::RoR2.UserProfile.LoadUserProfileOperationResult Hook(Orig orig, global::Zio.IFileSystem fileSystem, global::Zio.UPath path);
                    public static event ILContext.Manipulator Il
                    {
                        add => HookEndpointManager.Modify<Hook>(method, value);
                        remove => HookEndpointManager.Unmodify<Hook>(method, value);
                    }
                    public static event Hook On
                    {
                        add => HookEndpointManager.Add<Hook>(method, value);
                        remove => HookEndpointManager.Remove<Hook>(method, value);
                    }
                }
            }

            public static class Util
            {
                public struct IsPrefab
                {
                    private static readonly MethodBase method = HookHelpers.GetBase( typeof(IsPrefab) );
                    public delegate Boolean Orig(global::UnityEngine.GameObject gameObject);
                    public delegate Boolean Hook(Orig orig, global::UnityEngine.GameObject gameObject);
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

            public struct Artifacts
            {
                public static class DoppelgangerInvasionManager
                {
                    public struct CreateDoppelganger
                    {
                        private static readonly MethodBase method = HookHelpers.GetBase( typeof(CreateDoppelganger) );
                        public delegate void Orig(global::RoR2.CharacterMaster srcCharacterMaster, global::Xoroshiro128Plus rng);
                        public delegate void Hook(Orig orig, global::RoR2.CharacterMaster srcCharacterMaster, global::Xoroshiro128Plus rng);
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

                public static class DoppelgangerSpawnCard
                {
                    public struct FromMaster
                    {
                        private static readonly MethodBase method = HookHelpers.GetBase( typeof(FromMaster) );
                        public delegate DoppelgangerSpawnCard Orig(global::RoR2.CharacterMaster srcCharacterMaster);
                        public delegate DoppelgangerSpawnCard Hook(Orig orig, global::RoR2.CharacterMaster srcCharacterMaster);
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

            public struct Networking
            {
                public static class ServerAuthManager
                {
                    public struct HandleSetClientAuth
                    {
                        private static readonly MethodBase method = HookHelpers.GetBase( typeof(HandleSetClientAuth) );
                        public delegate void Orig(NetworkMessage message);
                        public delegate void Hook(Orig orig, NetworkMessage message);
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

                public static class GameNetworkManager
                {
                    public static class SimpleLocalizedKickReason
                    {
                        public struct GetDisplayTokenAndFormatParams
                        {
                            private static readonly MethodBase method = HookHelpers.GetBase( typeof(GetDisplayTokenAndFormatParams) );
                            public delegate void Orig(global::RoR2.Networking.GameNetworkManager.SimpleLocalizedKickReason self, out String token, out Object[] formatArgs);
                            public delegate void Hook(Orig orig, global::RoR2.Networking.GameNetworkManager.SimpleLocalizedKickReason self, out String token, out Object[] formatArgs);
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

            public struct Orbs
            {
                public static class LightningOrb
                {
                    public struct OnArrival
                    {
                        private static readonly MethodBase method = HookHelpers.GetBase( typeof(OnArrival) );
                        public delegate void Orig(global::RoR2.Orbs.LightningOrb self);
                        public delegate void Hook(Orig orig, global::RoR2.Orbs.LightningOrb self);
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

                public static class OrbCatalog
                {
                    public struct GenerateCatalog
                    {
                        private static readonly MethodBase method = HookHelpers.GetBase( typeof(GenerateCatalog) );
                        public delegate void Orig();
                        public delegate void Hook(Orig orig);
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
                        private static readonly MethodBase method = HookHelpers.GetBase( typeof(Start) );
                        public delegate void Orig(global::RoR2.Projectile.ProjectileController self);
                        public delegate void Hook(Orig orig, global::RoR2.Projectile.ProjectileController self);
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
                        private static readonly MethodBase method = HookHelpers.GetBase( typeof(FixedUpdate) );
                        public delegate void Orig(global::RoR2.Projectile.ProjectileImpactExplosion self);
                        public delegate void Hook(Orig orig, global::RoR2.Projectile.ProjectileImpactExplosion self);
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
                    public struct DetonateServer
                    {
                        private static readonly MethodBase method = HookHelpers.GetBase( typeof(DetonateServer) );
                        public delegate void Orig(global::RoR2.Projectile.ProjectileImpactExplosion self);
                        public delegate void Hook(Orig orig, global::RoR2.Projectile.ProjectileImpactExplosion self);
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
                        private static readonly MethodBase method = HookHelpers.GetBase( typeof(OnAssigned) );
                        public delegate global::RoR2.Skills.SkillDef.BaseSkillInstanceData Orig(global::RoR2.Skills.LunarPrimaryReplacementSkill self, global::RoR2.GenericSkill skillSlot);
                        public delegate global::RoR2.Skills.SkillDef.BaseSkillInstanceData Hook(Orig orig, global::RoR2.Skills.LunarPrimaryReplacementSkill self, global::RoR2.GenericSkill skillSlot);
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
                        private static readonly MethodBase method = HookHelpers.GetBase( typeof(OnUnassigned) );
                        public delegate void Orig(global::RoR2.Skills.LunarPrimaryReplacementSkill self, global::RoR2.GenericSkill skillSlot);
                        public delegate void Hook(Orig orig, global::RoR2.Skills.LunarPrimaryReplacementSkill self, global::RoR2.GenericSkill skillSlot);
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

                public static class SkillCatalog
                {
                    public struct Init
                    {
                        private static readonly MethodBase method = HookHelpers.GetBase(typeof(Init));
                        public delegate void Orig();
                        public delegate void Hook(Orig orig);

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
                public static class BuffIcon
                {
                    public struct UpdateIcon
                    {
                        private static readonly MethodBase method = HookHelpers.GetBase( typeof(UpdateIcon) );
                        public delegate void Orig(global::RoR2.UI.BuffIcon self);
                        public delegate void Hook(Orig orig, global::RoR2.UI.BuffIcon self);
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

                public static class CharacterSelectController
                {
                    public struct Awake
                    {
                        private static readonly MethodBase method = HookHelpers.GetBase( typeof(Awake) );
                        public delegate void Orig(global::RoR2.UI.CharacterSelectController self);
                        public delegate void Hook(Orig orig, global::RoR2.UI.CharacterSelectController self);
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
                        private static readonly MethodBase method = HookHelpers.GetBase( typeof(OnNetworkUserLoadoutChanged) );
                        public delegate void Orig(global::RoR2.UI.CharacterSelectController self, global::RoR2.NetworkUser networkUser);
                        public delegate void Hook(Orig orig, global::RoR2.UI.CharacterSelectController self, global::RoR2.NetworkUser networkUser);
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
                        private static readonly MethodBase method = HookHelpers.GetBase( typeof(RebuildLocal) );
                        public delegate void Orig(global::RoR2.UI.CharacterSelectController self);
                        public delegate void Hook(Orig orig, global::RoR2.UI.CharacterSelectController self);
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
                        private static readonly MethodBase method = HookHelpers.GetBase( typeof(UpdateCrosshair) );
                        public delegate void Orig(global::RoR2.UI.CrosshairManager self, global::RoR2.CharacterBody targetBody, global::UnityEngine.Vector3 crosshairWorldPosition, global::UnityEngine.Camera uiCamera);
                        public delegate void Hook(Orig orig, global::RoR2.UI.CrosshairManager self, global::RoR2.CharacterBody targetBody, global::UnityEngine.Vector3 crosshairWorldPosition, global::UnityEngine.Camera uiCamera);
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

                public static class EclipseRunScreenController
                {
                    public struct SelectSurvivor
                    {
                        private static readonly MethodBase method = HookHelpers.GetBase( typeof(SelectSurvivor) );
                        public delegate void Orig(global::RoR2.UI.EclipseRunScreenController self, global::RoR2.SurvivorIndex survivorIndex);
                        public delegate void Hook(Orig orig, global::RoR2.UI.EclipseRunScreenController self, global::RoR2.SurvivorIndex survivorIndex);
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
                    public static class Row
                    {
                        public struct FromSkillSlot
                        {
                            private static readonly MethodBase method = HookHelpers.GetBase( typeof(FromSkillSlot) );
                            public delegate System.Object Orig(global::RoR2.UI.LoadoutPanelController owner, Int32 bodyIndex, Int32 skillSlotIndex, global::RoR2.GenericSkill skillSlot);
                            public delegate System.Object Hook(Orig orig, global::RoR2.UI.LoadoutPanelController owner, Int32 bodyIndex, Int32 skillSlotIndex, global::RoR2.GenericSkill skillSlot);
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
                    public struct Rebuild
                    {
                        private static readonly MethodBase method = HookHelpers.GetBase( typeof(Rebuild) );
                        public delegate void Orig(global::RoR2.UI.LoadoutPanelController self);
                        public delegate void Hook(Orig orig, global::RoR2.UI.LoadoutPanelController self);
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
                        private static readonly MethodBase method = HookHelpers.GetBase( typeof(OnEnable) );
                        public delegate void Orig(global::RoR2.UI.PauseScreenController self);
                        public delegate void Hook(Orig orig, global::RoR2.UI.PauseScreenController self);
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
                        private static readonly MethodBase method = HookHelpers.GetBase( typeof(OnDisable) );
                        public delegate void Orig(global::RoR2.UI.PauseScreenController self);
                        public delegate void Hook(Orig orig, global::RoR2.UI.PauseScreenController self);
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
                        private static readonly MethodBase method = HookHelpers.GetBase( typeof(Start) );
                        public delegate void Orig(global::RoR2.UI.QuickPlayButtonController self);
                        public delegate void Hook(Orig orig, global::RoR2.UI.QuickPlayButtonController self);
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
