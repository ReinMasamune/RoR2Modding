#if PROFILER
using BepInEx;
using RoR2;
using UnityEngine;
using System.Collections.Generic;
using RoR2.Navigation;
using R2API;
using R2API.Utils;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using System.Reflection;
using EntityStates;
using RoR2.Skills;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Linq;

namespace ReinGeneralFixes
{
    internal partial class Main
    {
        const Int64 minProfileTicks = 0L;
        const UInt64 profilerLongDivisor = 1000UL;
        const Int32 numToLog = 100;
        private static HashSet<String> badNames;
        partial void Profiler()
        {
            this.Load += this.Main_Load;
            this.Enable += this.Main_Enable2;
            this.Disable += this.Main_Disable2;


            badNames = new HashSet<string>( new[]
            {
                //These throw weird controller map errors
                "IL.RoR2.DefaultControllerMaps:cctor",

                "IL.RampFogRenderer:Render",
                "IL.SobelOutlineRenderer:Render",
                "IL.SobelRainRenderer:Render",
                "IL.FlashWindow:FlashWindowEx",
                "IL.FlashWindow:GetActiveWindow",
                "IL.FlashWindow:EnumWindows",
                "IL.FlashWindow:GetWindowThreadProcessId",
                "IL.FlashWindow:GetCurrentProcessId",
                "IL.FlashWindow+EnumWindowsProc:ctor",
                "IL.FlashWindow+EnumWindowsProc:Invoke",
                "IL.FlashWindow+EnumWindowsProc:BeginInvoke",
                "IL.FlashWindow+EnumWindowsProc:EndInvoke",
                "IL.ProcessorAffinity:SetProcessAffinityMask",
                "IL.ProcessorAffinity:GetProcessAffinityMask",
                "IL.ProcessorAffinity:GetCurrentProcess",
                "IL.SteamAPIValidator.WinCrypt:CryptQueryObject",
                "IL.SteamAPIValidator.WinCrypt:CryptMsgGetParam_IntPtr_int_int_IntPtr_refInt32",
                "IL.SteamAPIValidator.WinCrypt:CryptMsgGetParam_IntPtr_int_int_ByteArray_refInt32",
                "IL.SteamAPIValidator.WinCrypt:CryptDecodeObject",
                "IL.SteamAPIValidator.SteamApiValidator:LoadLibrary",
                "IL.SteamAPIValidator.SteamApiValidator:GetModuleHandle",
                "IL.SteamAPIValidator.SteamApiValidator:GetModuleFileName",
                "IL.UnityEngine.Rendering.PostProcessing.HopooSSR:IsEnabledAndSupported",
                "IL.RoR2.BulletAttack+HitCallback:ctor",
                "IL.RoR2.BulletAttack+HitCallback:Invoke",
                "IL.RoR2.BulletAttack+HitCallback:BeginInvoke",
                "IL.RoR2.BulletAttack+HitCallback:EndInvoke",
                "IL.RoR2.BulletAttack+FilterCallback:ctor",
                "IL.RoR2.BulletAttack+FilterCallback:Invoke",
                "IL.RoR2.BulletAttack+FilterCallback:BeginInvoke",
                "IL.RoR2.BulletAttack+FilterCallback:EndInvoke",
                "IL.RoR2.CostTypeDef+BuildCostStringDelegate:ctor",
                "IL.RoR2.CostTypeDef+BuildCostStringDelegate:Invoke",
                "IL.RoR2.CostTypeDef+BuildCostStringDelegate:BeginInvoke",
                "IL.RoR2.CostTypeDef+BuildCostStringDelegate:EndInvoke",
                "IL.RoR2.CostTypeDef+GetCostColorDelegate:ctor",
                "IL.RoR2.CostTypeDef+GetCostColorDelegate:Invoke",
                "IL.RoR2.CostTypeDef+GetCostColorDelegate:BeginInvoke",
                "IL.RoR2.CostTypeDef+GetCostColorDelegate:EndInvoke",
                "IL.RoR2.CostTypeDef+BuildCostStringStyledDelegate:ctor",
                "IL.RoR2.CostTypeDef+BuildCostStringStyledDelegate:Invoke",
                "IL.RoR2.CostTypeDef+BuildCostStringStyledDelegate:BeginInvoke",
                "IL.RoR2.CostTypeDef+BuildCostStringStyledDelegate:EndInvoke",
                "IL.RoR2.CostTypeDef+IsAffordableDelegate:ctor",
                "IL.RoR2.CostTypeDef+IsAffordableDelegate:Invoke",
                "IL.RoR2.CostTypeDef+IsAffordableDelegate:BeginInvoke",
                "IL.RoR2.CostTypeDef+IsAffordableDelegate:EndInvoke",
                "IL.RoR2.CostTypeDef+PayCostDelegate:ctor",
                "IL.RoR2.CostTypeDef+PayCostDelegate:Invoke",
                "IL.RoR2.CostTypeDef+PayCostDelegate:BeginInvoke",
                "IL.RoR2.CostTypeDef+PayCostDelegate:EndInvoke",
                "IL.RoR2.CharacterMotor+HitGroundDelegate:ctor",
                "IL.RoR2.CharacterMotor+HitGroundDelegate:Invoke",
                "IL.RoR2.CharacterMotor+HitGroundDelegate:BeginInvoke",
                "IL.RoR2.CharacterMotor+HitGroundDelegate:EndInvoke",
                "IL.RoR2.CharacterMotor+MovementHitDelegate:ctor",
                "IL.RoR2.CharacterMotor+MovementHitDelegate:Invoke",
                "IL.RoR2.CharacterMotor+MovementHitDelegate:BeginInvoke",
                "IL.RoR2.CharacterMotor+MovementHitDelegate:EndInvoke",
                "IL.RoR2.Console:AllocConsole",
                "IL.RoR2.Console:FreeConsole",
                "IL.RoR2.Console:AttachConsole",
                "IL.RoR2.Console:PostMessage",
                "IL.RoR2.Console:GetConsoleWindow",
                "IL.RoR2.Console+LogReceivedDelegate:ctor",
                "IL.RoR2.Console+LogReceivedDelegate:Invoke",
                "IL.RoR2.Console+LogReceivedDelegate:BeginInvoke",
                "IL.RoR2.Console+LogReceivedDelegate:EndInvoke",
                "IL.RoR2.Console+ConCommandDelegate:ctor",
                "IL.RoR2.Console+ConCommandDelegate:Invoke",
                "IL.RoR2.Console+ConCommandDelegate:BeginInvoke",
                "IL.RoR2.Console+ConCommandDelegate:EndInvoke",
                "IL.RoR2.BullseyeSearch+Selector:ctor",
                "IL.RoR2.BullseyeSearch+Selector:Invoke",
                "IL.RoR2.BullseyeSearch+Selector:BeginInvoke",
                "IL.RoR2.BullseyeSearch+Selector:EndInvoke",
                "IL.RoR2.NetworkUser+NetworkUserGenericDelegate:ctor",
                "IL.RoR2.NetworkUser+NetworkUserGenericDelegate:Invoke",
                "IL.RoR2.NetworkUser+NetworkUserGenericDelegate:BeginInvoke",
                "IL.RoR2.NetworkUser+NetworkUserGenericDelegate:EndInvoke",
                "IL.RoR2.RoR2Application:NtSetTimerResolution",
                "IL.RoR2.RoR2Application:NtQueryTimerResolution",
                "IL.RoR2.SceneCamera+SceneCameraDelegate:ctor",
                "IL.RoR2.SceneCamera+SceneCameraDelegate:Invoke",
                "IL.RoR2.SceneCamera+SceneCameraDelegate:BeginInvoke",
                "IL.RoR2.SceneCamera+SceneCameraDelegate:EndInvoke",
                "IL.RoR2.SceneDirector+GenerateSpawnPointsDelegate:ctor",
                "IL.RoR2.SceneDirector+GenerateSpawnPointsDelegate:Invoke",
                "IL.RoR2.SceneDirector+GenerateSpawnPointsDelegate:BeginInvoke",
                "IL.RoR2.SceneDirector+GenerateSpawnPointsDelegate:EndInvoke",
                "IL.RoR2.UICamera+UICameraDelegate:ctor",
                "IL.RoR2.UICamera+UICameraDelegate:Invoke",
                "IL.RoR2.UICamera+UICameraDelegate:BeginInvoke",
                "IL.RoR2.UICamera+UICameraDelegate:EndInvoke",
                "IL.RoR2.VehicleSeat+InteractabilityCheckDelegate:ctor",
                "IL.RoR2.VehicleSeat+InteractabilityCheckDelegate:Invoke",
                "IL.RoR2.VehicleSeat+InteractabilityCheckDelegate:BeginInvoke",
                "IL.RoR2.VehicleSeat+InteractabilityCheckDelegate:EndInvoke",
                "IL.RoR2.InputCatalog+ActionAxisPair:GetHashCode",
                "IL.RoR2.Loadout+BodyLoadoutManager+BodyLoadout:ValueEquals",
                "IL.RoR2.SettingsConVars+PpMotionBlurConVar:ctor",
                "IL.RoR2.SettingsConVars+PpMotionBlurConVar:SetString",
                "IL.RoR2.SettingsConVars+PpMotionBlurConVar:GetString",
                "IL.RoR2.SettingsConVars+PpMotionBlurConVar:cctor",
                "IL.RoR2.SettingsConVars+PpSobelOutlineConVar:ctor",
                "IL.RoR2.SettingsConVars+PpSobelOutlineConVar:SetString",
                "IL.RoR2.SettingsConVars+PpSobelOutlineConVar:GetString",
                "IL.RoR2.SettingsConVars+PpSobelOutlineConVar:cctor",
                "IL.RoR2.SettingsConVars+PpBloomConVar:ctor",
                "IL.RoR2.SettingsConVars+PpBloomConVar:SetString",
                "IL.RoR2.SettingsConVars+PpBloomConVar:GetString",
                "IL.RoR2.SettingsConVars+PpBloomConVar:cctor",
                "IL.RoR2.SettingsConVars+PpAOConVar:ctor",
                "IL.RoR2.SettingsConVars+PpAOConVar:SetString",
                "IL.RoR2.SettingsConVars+PpAOConVar:GetString",
                "IL.RoR2.SettingsConVars+PpAOConVar:cctor",
                "IL.RoR2.SettingsConVars+PpGammaConVar:ctor",
                "IL.RoR2.SettingsConVars+PpGammaConVar:SetString",
                "IL.RoR2.SettingsConVars+PpGammaConVar:GetString",
                "IL.RoR2.SettingsConVars+PpGammaConVar:cctor",
                "IL.RoR2.UserProfile+SaveHelper+FileReference:GetHashCode",
                "IL.RoR2.Stats.StatDef+DisplayValueFormatterDelegate:ctor",
                "IL.RoR2.Stats.StatDef+DisplayValueFormatterDelegate:Invoke",
                "IL.RoR2.Stats.StatDef+DisplayValueFormatterDelegate:BeginInvoke",
                "IL.RoR2.Stats.StatDef+DisplayValueFormatterDelegate:EndInvoke",
                "IL.RoR2.UI.GameEndReportPanelController+DisplayData:GetHashCode",
                "IL.RoR2.UI.MPInputModule+ShouldStartDragDelegate:ctor",
                "IL.RoR2.UI.MPInputModule+ShouldStartDragDelegate:Invoke",
                "IL.RoR2.UI.MPInputModule+ShouldStartDragDelegate:BeginInvoke",
                "IL.RoR2.UI.MPInputModule+ShouldStartDragDelegate:EndInvoke",
                "IL.RoR2.UI.ObjectivePanelController:ctor",
                "IL.RoR2.UI.UserProfileListController+ProfileSelectedDelegate:ctor",
                "IL.RoR2.UI.UserProfileListController+ProfileSelectedDelegate:Invoke",
                "IL.RoR2.UI.UserProfileListController+ProfileSelectedDelegate:BeginInvoke",
                "IL.RoR2.UI.UserProfileListController+ProfileSelectedDelegate:EndInvoke",


                //"IL.RoR2.DefaultControllerMaps:CCExportDefaultControllerMaps",
                //"IL.RoR2.SkinDef:Bake",
                //"IL.RoR2.ViewablesCatalog+Node:GenerateFullName",
                //"IL.RoR2.UI.AchievementListPanelController:AddAchievementToOrderedList",
                //"IL.RoR2.SteamworksRemoteStorageFileSystem:AddNodeToTree",
                //"IL.RoR2.SteamworksRemoteStorageFileSystem:GetDirectoryNode",
                //"IL.RoR2.Console:SubmitCmd",
                //"IL.RoR2.MPEventSystemManager:RefreshEventSystems",
                //"IL.ResourceAvailability:MakeAvailable",
                //"IL.ResourceAvailability:CallWhenAvailable",
                //"IL.RoR2.NetworkUser:SetBodyPreference",
                //"IL.RoR2.RuleBook:GetRuleChoice_int",
                //"IL.RoR2.RuleBook:GetRuleChoice_RuleDef",
                //"IL.RoR2.ModelCamera:PrepareObjectForRendering",
                //"IL.DynamicBone:AppendParticles",
                //"IL.RoR2.ShopTerminalBehavior:SetPickupIndex",
                //"IL.RoR2.VehicleSeat:SetPassenger",
                //"IL.RoR2.NetworkParent:SetParentIdentifier",
                //"IL.RoR2.UI.BuffDisplay:UpdateLayout",
                //"IL.RoR2.UI.BuffDisplay:AllocateIcons",
                //"IL.RoR2.ShopTerminalBehavior:SetHasBeenPurchased",
            } );
        }

        private void Main_Load()
        {
            //var asm = typeof(RoR2Application).Assembly;
            //var flags = BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Instance;
            //foreach( var t in asm.GetTypes() )
            //{
            //    foreach( var method in t.GetMethods(flags) )
            //    {
            //        hooks.Add( new HookContext( method ) );
            //    }
            //}
            var asm = typeof(IL.BeamPointsFromTransforms).Assembly;
            var flags = BindingFlags.Public | BindingFlags.Static;
            var types = asm.GetTypes();
            for( Int32 i = 0; i < types.Length; ++i )
            {
                var t = types[i];
                var events = t.GetEvents( flags );
                for( Int32 j = 0; j < events.Length; ++j )
                {
                    var eventInfo = events[j];
                    if( eventInfo.EventHandlerType == typeof( ILContext.Manipulator ) )
                    {
                        var name = t.FullName + ":" + eventInfo.Name;
                        if( CheckMethodName( name ) )
                        {
                            hooks.Add( new ILHookProfile( eventInfo, name ) );
                        }
                    }
                }
            }
        }

        private static Boolean CheckMethodName( String name )
        {
            if( badNames.Contains( name ) ) return false;
            //if( name.Contains( "Rewired" ) ) return false;
            //if( name.Contains( "Rule" ) ) return false;
            //if( name.Contains( "EntityStateManager" ) ) return false;
            //if( name.Contains( "HGXml" ) ) return false;
            //if( name.Contains( "SimpleJSON" ) ) return false;
            //if( name.Contains( "UserAchievementManager" ) ) return false;
            //if( name.Contains( "Chat" ) ) return false;
            //if( name.Contains( "Achievement" ) ) return false;
            //if( name.Contains( "ArenaMissionController" ) ) return false;
            return true;
        }

        private void Main_Disable2()
        {
            On.RoR2.ClassicStageInfo.Awake -= this.ClassicStageInfo_Awake;
            for( Int32 i = 0; i < hooks.Count; ++i )
            {
                var v = hooks[i];
                v.Remove( false );
            }
        }
        private void Main_Enable2()
        {
            On.RoR2.ClassicStageInfo.Awake += this.ClassicStageInfo_Awake;
            for( Int32 i = 0; i < hooks.Count; ++i )
            {
                var v = hooks[i];
                v.Add( true );
            }
        }

        private void ClassicStageInfo_Awake( On.RoR2.ClassicStageInfo.orig_Awake orig, ClassicStageInfo self )
        {
            var longTotal = 0UL;
            var longLongTotal = 0UL;
            stageSorted.Clear();
            totalSorted.Clear();
            for( Int32 i = 0; i < hooks.Count; ++i )
            {
                var v = hooks[i];
                v.Gather();
                longTotal += v.stageTime / Main.profilerLongDivisor;
                longLongTotal += v.longTime / Main.profilerLongDivisor;
                stageSorted.Add( v );
                totalSorted.Add( v );
                if( !history.ContainsKey( v.name ) )
                {
                    history[v.name] = new Histogram( v.name );
                }
                history[v.name].AddTime( v.stageTime );
            }


            stageSorted.OrderByDescending<ILHookProfile, UInt64>( ( prof ) => prof.stageTime );
            totalSorted.OrderByDescending<ILHookProfile, UInt64>( ( prof ) => prof.longTime );

            for( Int32 i = 0; i < Main.numToLog; ++i )
            {
                var v = stageSorted[i];
                var val = v.stageTime;
                var perc = (Double)(v.stageTime / Main.profilerLongDivisor) / (Double)longTotal;
                Main.LogW( v.name + String.Format( ":::Time {0}:::Percent {1}", val, perc ) );
            }
            for( Int32 i = 0; i < Main.numToLog; ++i )
            {
                var v = totalSorted[i];
                var val = v.longTime;
                var perc = (Double)(v.longTime / Main.profilerLongDivisor) / (Double)longLongTotal;
                Main.LogE( v.name + String.Format( ":::Time {0}:::Percent {1}", val, perc ) );
            }


            orig( self );
        }
        internal static List<ILHookProfile> stageSorted = new List<ILHookProfile>();
        internal static List<ILHookProfile> totalSorted = new List<ILHookProfile>();
        internal static Dictionary<String,Histogram> history = new Dictionary<String, Histogram>();
        internal static List<ILHookProfile> hooks = new List<ILHookProfile>();

        internal class Histogram
        {
            internal String name { get; private set; }
            internal List<UInt64> stageTimes = new List<UInt64>();
            internal UInt64 longTotal { get; private set; }

            internal Histogram( String name )
            {
                this.name = name;
            }

            internal void AddTime( UInt64 time )
            {
                this.stageTimes.Add( time );
                this.longTotal += time / Main.profilerLongDivisor;
            }
        }


        internal class ILHookProfile
        {
            internal static void LogDictionary()
            {
                var total = 0ul;
                foreach( var kv in totalTimes )
                {
                    total += kv.Value;
                }

                foreach( var kv in totalTimes )
                {
                    Main.LogE( kv.Key + "::: :::" + (((Double)kv.Value) / (Double)total) );
                }
            }
            private static Dictionary<String,UInt64> totalTimes = new Dictionary<String, UInt64>();


            internal UInt64 stageTime { get; private set; }
            internal UInt64 longTime { get; private set; }
            //internal UInt64 totalTime { get; private set; } = 0u;
            internal ILHookProfile( EventInfo info, String name )
            {
                this.name = name;
                this.hookEvent = info;
                this.hookDelegate = (ILContext.Manipulator)this.ILHook;
            }

            internal void Add(Boolean log)
            {
                if( log )
                {
                    Main.LogW( this.name );
                }

                this.hookEvent.AddEventHandler( null, this.hookDelegate );
            }
            internal void Remove(Boolean log)
            {
                if( log )
                {
                    Main.LogF( this.hookEvent.DeclaringType.FullName );
                }
                this.hookEvent.RemoveEventHandler( null, this.hookDelegate );
            }
            internal void Gather()
            {
                this.watch.Stop();
                this.stageTime = (UInt64)this.watch.ElapsedTicks;
                this.longTime += this.stageTime / Main.profilerLongDivisor;
                this.watch.Reset();
            }




            internal String name { get; private set; }
            private EventInfo hookEvent;
            private Delegate hookDelegate;
            private Stopwatch watch = new Stopwatch();
            private Int64 prevTime;
            private void ILHook( ILContext il )
            {
                ILCursor c = new ILCursor( il );
                //if( c.TryFindNext( out _, x => x.MatchLeaveS( out _ ) ) )
                //{
                //    Main.LogW( "Ignoring LeaveS" );
                //    return;
                //}
                var foundSomething = false;
                if( c.TryFindNext( out ILCursor[] retCursors, x => x.MatchRet() ) )
                {
                    Main.LogE( String.Format( "{0} returns", retCursors.Length ) );
                    foundSomething = true;
                }
                if( c.TryFindNext( out ILCursor[] throwCursors, x => x.MatchRethrow() ) )
                {
                    Main.LogE( String.Format( "{0} throws", throwCursors.Length ) );
                    foundSomething = true;
                }

                if( foundSomething )
                {
                    c.EmitDelegate<Action>( this.Start );
                }

                //var l1 = c.FindNext()

                //while( c.TryGotoNext( MoveType.AfterLabel, x => x.MatchRet() ) )
                //{
                //    c.EmitDelegate<Action>( this.Stop );
                //    ++c.Index;
                //}
            }
            private void Start()
            {
                if( this.watch.IsRunning )
                {
                    this.watch.Stop();
                    Main.LogW( this.name + "\nDidn't stop timer. Resetting" );
                    this.watch.Reset();
                }
                this.watch.Start();
            }
            private void Stop()
            {
                //Main.LogW( "Stopping" );
                this.watch.Stop();
            }
            
        }


        //internal static HashSet<HookContext> hooks = new HashSet<HookContext>();

        //internal class HookContext
        //{
        //    //internal HookContext( MethodInfo method )
        //    //{
        //    //    var watchInstanceParameter = Expression.Constant( this, typeof(HookContext) );
        //    //    var watchStartExpression = Expression.Call( watchInstanceParameter, "Start", null );
        //    //    var watchStopExpression = Expression.Call( watchInstanceParameter, "Stop", null );

        //    //    this.hookedMethod = method;
        //    //    var returnParam = method.ReturnParameter;
        //    //    var parameters = method.GetParameters();

        //    //    Type delType = null;
        //    //    LambdaExpression lamb = null;
        //    //    var paramTypes = new List<Type>();
        //    //    var paramExpressions = new ParameterExpression[parameters.Length];
        //    //    if( !method.IsStatic ) paramTypes.Add( method.DeclaringType );
        //    //    for( Int32 i = 0; i < paramExpressions.Length; ++i )
        //    //    {
        //    //        paramExpressions[i] = Expression.Parameter( parameters[i].ParameterType, "param" );
        //    //        paramTypes.Add( parameters[i].ParameterType );
        //    //    }

        //    //    var varParams = new List<ParameterExpression>();
        //    //    if( method.IsStatic )
        //    //    {
        //    //        var callExpression = Expression.Call( method, paramExpressions );
        //    //        BlockExpression block = null;
        //    //        if( returnParam != null )
        //    //        {
        //    //            var temp = Expression.Variable( returnParam.ParameterType, "return" );
        //    //            var assign = Expression.Assign( temp, callExpression );
        //    //            varParams = new List<ParameterExpression>();
        //    //            var returnExpression = Expression.Return( Expression.Label(), temp );
        //    //            varParams.Add( temp );
        //    //            for( Int32 i = 0; i < paramExpressions.Length; ++i )
        //    //            {
        //    //                varParams.Add( paramExpressions[i] );
        //    //            }
        //    //            block = Expression.Block( returnParam.ParameterType, varParams, watchStartExpression, assign, watchStopExpression, returnExpression );

        //    //            paramTypes.Add( returnParam.ParameterType );
        //    //            delType = Expression.GetFuncType( paramTypes.ToArray() );
        //    //        } else
        //    //        {
        //    //            for( Int32 i = 0; i < paramExpressions.Length; ++i )
        //    //            {
        //    //                varParams.Add( paramExpressions[i] );
        //    //            }
        //    //            block = Expression.Block( varParams, watchStartExpression, callExpression, watchStopExpression );
        //    //            delType = Expression.GetActionType( paramTypes.ToArray() );
        //    //        }

        //    //        lamb = Expression.Lambda(delType, block, paramExpressions);

        //    //    } else
        //    //    {
        //    //        var instanceParam = Expression.Parameter( method.DeclaringType, "instance" );
        //    //        var callExpression = Expression.Call( instanceParam, method, paramExpressions );
        //    //        BlockExpression block = null;
        //    //        if( returnParam != null )
        //    //        {
        //    //            var temp = Expression.Variable( returnParam.ParameterType, "return" );
        //    //            var assign = Expression.Assign( temp, callExpression );
        //    //            varParams = new List<ParameterExpression>();
        //    //            var returnExpression = Expression.Return( Expression.Label(), temp );
        //    //            varParams.Add( temp );
        //    //            varParams.Add( instanceParam );
        //    //            for( Int32 i = 0; i < paramExpressions.Length; ++i )
        //    //            {
        //    //                varParams.Add( paramExpressions[i] );
        //    //            }
        //    //            block = Expression.Block( returnParam.ParameterType, varParams, watchStartExpression, assign, watchStopExpression, returnExpression );
        //    //            delType = Expression.GetFuncType( )
        //    //        } else
        //    //        {
        //    //            varParams = new List<ParameterExpression>();
        //    //            varParams.Add( instanceParam );
        //    //            for( Int32 i = 0; i < paramExpressions.Length; ++i )
        //    //            {
        //    //                varParams.Add( paramExpressions[i] );
        //    //            }
        //    //            block = Expression.Block( varParams, watchStartExpression, callExpression, watchStopExpression );
        //    //        }
                    
        //    //    }
        //    //}
        //    internal void AddHook()
        //    {

        //    }
        //    internal void RemoveHook()
        //    {

        //    }


        //    private MethodInfo hookedMethod;
        //    private Delegate generatedDelegate;
            




        //    private UInt64 time = 0u;
        //    private Stopwatch watch = new Stopwatch();
        //    private void Start()
        //    {
        //        this.watch.Start();
        //    }
        //    private void Stop()
        //    {
        //        this.watch.Stop();
        //        this.time += (UInt64)this.watch.ElapsedMilliseconds;
        //        this.watch.Reset();
        //    }

        //}
    }

}
#endif