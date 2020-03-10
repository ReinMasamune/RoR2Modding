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

    /// <summary>
    /// 
    /// </summary>
    public static class HooksCore
    {
        #region RoR2
        #region BuffCatalog
        #region Init
        public static event ILContext.Manipulator il_RoR2_BuffCatalog_Init
        {
            add => HookEndpointManager.Modify<hook_RoR2_BuffCatalog_Init>(base_RoR2_BuffCatalog_Init, value );
            remove => HookEndpointManager.Unmodify<hook_RoR2_BuffCatalog_Init>( base_RoR2_BuffCatalog_Init, value );

        }
        public static event hook_RoR2_BuffCatalog_Init on_RoR2_BuffCatalog_Init
        {
            add => HookEndpointManager.Add<hook_RoR2_BuffCatalog_Init>( base_RoR2_BuffCatalog_Init, value );
            remove => HookEndpointManager.Remove<hook_RoR2_BuffCatalog_Init>( base_RoR2_BuffCatalog_Init, value );
        }
        [EditorBrowsable( EditorBrowsableState.Never )]
        public delegate void orig_RoR2_BuffCatalog_Init();
        [EditorBrowsable( EditorBrowsableState.Never )]
        public delegate void hook_RoR2_BuffCatalog_Init( orig_RoR2_BuffCatalog_Init orig );
        private static MethodBase base_RoR2_BuffCatalog_Init
        {
            get
            {
                if( _base_RoR2_BuffCatalog_Init == null ) _base_RoR2_BuffCatalog_Init = HookHelpers.GetMethodBaseRoot( typeof(BuffCatalog), "Init" );
                return _base_RoR2_BuffCatalog_Init;
            }
        }
        private static MethodBase _base_RoR2_BuffCatalog_Init;

        #endregion
        #endregion

        #region CameraRigController
        #region Start
        public static event ILContext.Manipulator il_RoR2_CameraRigController_Start
        {
            add => HookEndpointManager.Modify<hook_RoR2_CameraRigController_Start>(base_RoR2_CameraRigController_Start, value );
            remove => HookEndpointManager.Unmodify<hook_RoR2_CameraRigController_Start>( base_RoR2_CameraRigController_Start, value );

        }
        public static event hook_RoR2_CameraRigController_Start on_RoR2_CameraRigController_Start
        {
            add => HookEndpointManager.Add<hook_RoR2_CameraRigController_Start>( base_RoR2_CameraRigController_Start, value );
            remove => HookEndpointManager.Remove<hook_RoR2_CameraRigController_Start>( base_RoR2_CameraRigController_Start, value );
        }
        [EditorBrowsable( EditorBrowsableState.Never )]
        public delegate void orig_RoR2_CameraRigController_Start( CameraRigController self);
        [EditorBrowsable( EditorBrowsableState.Never )]
        public delegate void hook_RoR2_CameraRigController_Start( orig_RoR2_CameraRigController_Start orig, CameraRigController self );
        private static MethodBase base_RoR2_CameraRigController_Start
        {
            get
            {
                if( _base_RoR2_CameraRigController_Start == null ) _base_RoR2_CameraRigController_Start = HookHelpers.GetMethodBase<CameraRigController>( "Start" );
                return _base_RoR2_CameraRigController_Start;
            }
        }
        private static MethodBase _base_RoR2_CameraRigController_Start;
        #endregion
        #region Update
        public static event ILContext.Manipulator il_RoR2_CameraRigController_Update
        {
            add => HookEndpointManager.Modify<hook_RoR2_CameraRigController_Update>(base_RoR2_CameraRigController_Update, value );
            remove => HookEndpointManager.Unmodify<hook_RoR2_CameraRigController_Update>( base_RoR2_CameraRigController_Update, value );

        }
        public static event hook_RoR2_CameraRigController_Update on_RoR2_CameraRigController_Update
        {
            add => HookEndpointManager.Add<hook_RoR2_CameraRigController_Update>( base_RoR2_CameraRigController_Update, value );
            remove => HookEndpointManager.Remove<hook_RoR2_CameraRigController_Update>( base_RoR2_CameraRigController_Update, value );
        }
        [EditorBrowsable( EditorBrowsableState.Never )]
        public delegate void orig_RoR2_CameraRigController_Update( CameraRigController self );
        [EditorBrowsable( EditorBrowsableState.Never )]
        public delegate void hook_RoR2_CameraRigController_Update( orig_RoR2_CameraRigController_Update orig, CameraRigController self );
        private static MethodBase base_RoR2_CameraRigController_Update
        {
            get
            {
                if( _base_RoR2_CameraRigController_Update == null ) _base_RoR2_CameraRigController_Update = HookHelpers.GetMethodBase<CameraRigController>( "Update" );
                return _base_RoR2_CameraRigController_Update;
            }
        }
        private static MethodBase _base_RoR2_CameraRigController_Update;
        #endregion
        #endregion



        #region CharacterBody
        #region FixedUpdate
        public static event ILContext.Manipulator il_RoR2_CharacterBody_FixedUpdate
        {
            add => HookEndpointManager.Modify<hook_RoR2_CharacterBody_FixedUpdate>(base_RoR2_CharacterBody_FixedUpdate, value );
            remove => HookEndpointManager.Unmodify<hook_RoR2_CharacterBody_FixedUpdate>( base_RoR2_CharacterBody_FixedUpdate, value );

        }
        public static event hook_RoR2_CharacterBody_FixedUpdate on_RoR2_CharacterBody_FixedUpdate
        {
            add => HookEndpointManager.Add<hook_RoR2_CharacterBody_FixedUpdate>( base_RoR2_CharacterBody_FixedUpdate, value );
            remove => HookEndpointManager.Remove<hook_RoR2_CharacterBody_FixedUpdate>( base_RoR2_CharacterBody_FixedUpdate, value );
        }
        [EditorBrowsable( EditorBrowsableState.Never )]
        public delegate void orig_RoR2_CharacterBody_FixedUpdate( CharacterBody self );
        [EditorBrowsable( EditorBrowsableState.Never )]
        public delegate void hook_RoR2_CharacterBody_FixedUpdate( orig_RoR2_CharacterBody_FixedUpdate orig, CharacterBody self );
        private static MethodBase base_RoR2_CharacterBody_FixedUpdate
        {
            get
            {
                if( _base_RoR2_CharacterBody_FixedUpdate == null ) _base_RoR2_CharacterBody_FixedUpdate = HookHelpers.GetMethodBase<CharacterBody>( "FixedUpdate" );
                return _base_RoR2_CharacterBody_FixedUpdate;
            }
        }
        private static MethodBase _base_RoR2_CharacterBody_FixedUpdate;
        #endregion
        #region RecalculateStats
        public static event ILContext.Manipulator il_RoR2_CharacterBody_RecalculateStats
        {
            add => HookEndpointManager.Modify<hook_RoR2_CharacterBody_RecalculateStats>(base_RoR2_CharacterBody_RecalculateStats, value );
            remove => HookEndpointManager.Unmodify<hook_RoR2_CharacterBody_RecalculateStats>( base_RoR2_CharacterBody_RecalculateStats, value );

        }
        public static event hook_RoR2_CharacterBody_RecalculateStats on_RoR2_CharacterBody_RecalculateStats
        {
            add => HookEndpointManager.Add<hook_RoR2_CharacterBody_RecalculateStats>( base_RoR2_CharacterBody_RecalculateStats, value );
            remove => HookEndpointManager.Remove<hook_RoR2_CharacterBody_RecalculateStats>( base_RoR2_CharacterBody_RecalculateStats, value );
        }
        [EditorBrowsable( EditorBrowsableState.Never )]
        public delegate void orig_RoR2_CharacterBody_RecalculateStats( CharacterBody self );
        [EditorBrowsable( EditorBrowsableState.Never )]
        public delegate void hook_RoR2_CharacterBody_RecalculateStats( orig_RoR2_CharacterBody_RecalculateStats orig, CharacterBody self );
        private static MethodBase base_RoR2_CharacterBody_RecalculateStats
        {
            get
            {
                if( _base_RoR2_CharacterBody_RecalculateStats == null ) _base_RoR2_CharacterBody_RecalculateStats = HookHelpers.GetMethodBase<CharacterBody>( "RecalculateStats" );
                return _base_RoR2_CharacterBody_RecalculateStats;
            }
        }
        private static MethodBase _base_RoR2_CharacterBody_RecalculateStats;
        #endregion
        #region Start
        public static event ILContext.Manipulator il_RoR2_CharacterBody_Start
        {
            add => HookEndpointManager.Modify<hook_RoR2_CharacterBody_Start>(base_RoR2_CharacterBody_Start, value );
            remove => HookEndpointManager.Unmodify<hook_RoR2_CharacterBody_Start>( base_RoR2_CharacterBody_Start, value );

        }
        public static event hook_RoR2_CharacterBody_Start on_RoR2_CharacterBody_Start
        {
            add => HookEndpointManager.Add<hook_RoR2_CharacterBody_Start>( base_RoR2_CharacterBody_Start, value );
            remove => HookEndpointManager.Remove<hook_RoR2_CharacterBody_Start>( base_RoR2_CharacterBody_Start, value );
        }
        [EditorBrowsable( EditorBrowsableState.Never )]
        public delegate void orig_RoR2_CharacterBody_Start( CharacterBody self );
        [EditorBrowsable( EditorBrowsableState.Never )]
        public delegate void hook_RoR2_CharacterBody_Start( orig_RoR2_CharacterBody_Start orig, CharacterBody self );
        private static MethodBase base_RoR2_CharacterBody_Start
        {
            get
            {
                if( _base_RoR2_CharacterBody_Start == null ) _base_RoR2_CharacterBody_Start = HookHelpers.GetMethodBase<CharacterBody>( "Start" );
                return _base_RoR2_CharacterBody_Start;
            }
        }
        private static MethodBase _base_RoR2_CharacterBody_Start;
        #endregion
        #endregion

        #region CharacterSpawnCard
        #region Awake
        public static event ILContext.Manipulator il_RoR2_CharacterSpawnCard_Awake
        {
            add => HookEndpointManager.Modify<hook_RoR2_CharacterSpawnCard_Awake>(base_RoR2_CharacterSpawnCard_Awake, value );
            remove => HookEndpointManager.Unmodify<hook_RoR2_CharacterSpawnCard_Awake>( base_RoR2_CharacterSpawnCard_Awake, value );

        }
        public static event hook_RoR2_CharacterSpawnCard_Awake on_RoR2_CharacterSpawnCard_Awake
        {
            add => HookEndpointManager.Add<hook_RoR2_CharacterSpawnCard_Awake>( base_RoR2_CharacterSpawnCard_Awake, value );
            remove => HookEndpointManager.Remove<hook_RoR2_CharacterSpawnCard_Awake>( base_RoR2_CharacterSpawnCard_Awake, value );
        }
        [EditorBrowsable( EditorBrowsableState.Never )]
        public delegate void orig_RoR2_CharacterSpawnCard_Awake( RoR2.CharacterSpawnCard self );
        [EditorBrowsable( EditorBrowsableState.Never )]
        public delegate void hook_RoR2_CharacterSpawnCard_Awake( orig_RoR2_CharacterSpawnCard_Awake orig, RoR2.CharacterSpawnCard self );
        private static MethodBase base_RoR2_CharacterSpawnCard_Awake
        {
            get
            {
                if( _base_RoR2_CharacterSpawnCard_Awake == null ) _base_RoR2_CharacterSpawnCard_Awake = HookHelpers.GetMethodBase<CharacterSpawnCard>( "Awake" );
                return _base_RoR2_CharacterSpawnCard_Awake;
            }
        }
        private static MethodBase _base_RoR2_CharacterSpawnCard_Awake;
        #endregion
        #endregion

        #region ClassicStageInfo
        #region Awake
        public static event ILContext.Manipulator il_RoR2_ClassicStageInfo_Awake
        {
            add => HookEndpointManager.Modify<hook_RoR2_ClassicStageInfo_Awake>(base_RoR2_ClassicStageInfo_Awake, value );
            remove => HookEndpointManager.Unmodify<hook_RoR2_ClassicStageInfo_Awake>( base_RoR2_ClassicStageInfo_Awake, value );

        }
        public static event hook_RoR2_ClassicStageInfo_Awake on_RoR2_ClassicStageInfo_Awake
        {
            add => HookEndpointManager.Add<hook_RoR2_ClassicStageInfo_Awake>( base_RoR2_ClassicStageInfo_Awake, value );
            remove => HookEndpointManager.Remove<hook_RoR2_ClassicStageInfo_Awake>( base_RoR2_ClassicStageInfo_Awake, value );
        }
        [EditorBrowsable( EditorBrowsableState.Never )]
        public delegate void orig_RoR2_ClassicStageInfo_Awake( RoR2.ClassicStageInfo self );
        [EditorBrowsable( EditorBrowsableState.Never )]
        public delegate void hook_RoR2_ClassicStageInfo_Awake( orig_RoR2_ClassicStageInfo_Awake orig, RoR2.ClassicStageInfo self );
        private static MethodBase base_RoR2_ClassicStageInfo_Awake
        {
            get
            {
                if( _base_RoR2_ClassicStageInfo_Awake == null ) _base_RoR2_ClassicStageInfo_Awake = HookHelpers.GetMethodBase<ClassicStageInfo>( "Awake" );
                return _base_RoR2_ClassicStageInfo_Awake;
            }
        }
        private static MethodBase _base_RoR2_ClassicStageInfo_Awake;
        #endregion
        #endregion

        #region EffectCatalog
        #region GetDefaultEffectDefs
        public static event ILContext.Manipulator il_RoR2_EffectCatalog_GetDefaultEffectDefs
        {
            add => HookEndpointManager.Modify<hook_RoR2_EffectCatalog_GetDefaultEffectDefs>(base_RoR2_EffectCatalog_GetDefaultEffectDefs, value );
            remove => HookEndpointManager.Unmodify<hook_RoR2_EffectCatalog_GetDefaultEffectDefs>( base_RoR2_EffectCatalog_GetDefaultEffectDefs, value );

        }
        public static event hook_RoR2_EffectCatalog_GetDefaultEffectDefs on_RoR2_EffectCatalog_GetDefaultEffectDefs
        {
            add => HookEndpointManager.Add<hook_RoR2_EffectCatalog_GetDefaultEffectDefs>( base_RoR2_EffectCatalog_GetDefaultEffectDefs, value );
            remove => HookEndpointManager.Remove<hook_RoR2_EffectCatalog_GetDefaultEffectDefs>( base_RoR2_EffectCatalog_GetDefaultEffectDefs, value );
        }
        [EditorBrowsable( EditorBrowsableState.Never )]
        public delegate EffectDef[] orig_RoR2_EffectCatalog_GetDefaultEffectDefs();
        [EditorBrowsable( EditorBrowsableState.Never )]
        public delegate EffectDef[] hook_RoR2_EffectCatalog_GetDefaultEffectDefs( orig_RoR2_EffectCatalog_GetDefaultEffectDefs orig );
        private static MethodBase base_RoR2_EffectCatalog_GetDefaultEffectDefs
        {
            get
            {
                if( _base_RoR2_EffectCatalog_GetDefaultEffectDefs == null ) _base_RoR2_EffectCatalog_GetDefaultEffectDefs = HookHelpers.GetMethodBaseRoot(typeof(EffectCatalog), "GetDefaultEffectDefs" );
                return _base_RoR2_EffectCatalog_GetDefaultEffectDefs;
            }
        }
        private static MethodBase _base_RoR2_EffectCatalog_GetDefaultEffectDefs;
        #endregion
        #endregion

        #region EffectManager
        #region SpawnEffect
        public static event ILContext.Manipulator il_RoR2_EffectManager_SpawnEffect___EffectIndex_EffectData_Boolean
        {
            add => HookEndpointManager.Modify<hook_RoR2_EffectManager_SpawnEffect___EffectIndex_EffectData_Boolean>(base_RoR2_EffectManager_SpawnEffect___EffectIndex_EffectData_Boolean, value );
            remove => HookEndpointManager.Unmodify<hook_RoR2_EffectManager_SpawnEffect___EffectIndex_EffectData_Boolean>( base_RoR2_EffectManager_SpawnEffect___EffectIndex_EffectData_Boolean, value );

        }
        public static event hook_RoR2_EffectManager_SpawnEffect___EffectIndex_EffectData_Boolean on_RoR2_EffectManager_SpawnEffect___EffectIndex_EffectData_Boolean
        {
            add => HookEndpointManager.Add<hook_RoR2_EffectManager_SpawnEffect___EffectIndex_EffectData_Boolean>( base_RoR2_EffectManager_SpawnEffect___EffectIndex_EffectData_Boolean, value );
            remove => HookEndpointManager.Remove<hook_RoR2_EffectManager_SpawnEffect___EffectIndex_EffectData_Boolean>( base_RoR2_EffectManager_SpawnEffect___EffectIndex_EffectData_Boolean, value );
        }
        [EditorBrowsable( EditorBrowsableState.Never )]
        public delegate void orig_RoR2_EffectManager_SpawnEffect___EffectIndex_EffectData_Boolean( EffectIndex effectIndex, EffectData effectData, Boolean transmit );
        [EditorBrowsable( EditorBrowsableState.Never )]
        public delegate void hook_RoR2_EffectManager_SpawnEffect___EffectIndex_EffectData_Boolean( orig_RoR2_EffectManager_SpawnEffect___EffectIndex_EffectData_Boolean orig, EffectIndex effectIndex, EffectData effectData, Boolean transmit );
        private static MethodBase base_RoR2_EffectManager_SpawnEffect___EffectIndex_EffectData_Boolean
        {
            get
            {
                if( _base_RoR2_EffectManager_SpawnEffect___EffectIndex_EffectData_Boolean == null ) _base_RoR2_EffectManager_SpawnEffect___EffectIndex_EffectData_Boolean = HookHelpers.GetMethodBaseRoot( typeof(EffectManager), "SpawnEffect", new[] { typeof( EffectIndex ), typeof( EffectData ), typeof( Boolean ) } );
                return _base_RoR2_EffectManager_SpawnEffect___EffectIndex_EffectData_Boolean;
            }
        }
        private static MethodBase _base_RoR2_EffectManager_SpawnEffect___EffectIndex_EffectData_Boolean;
        #endregion
        #endregion

        #region GlobalEventManager
        #region OnHitEnemy
        public static event ILContext.Manipulator il_RoR2_GlobalEventManager_OnHitEnemy
        {
            add => HookEndpointManager.Modify<hook_RoR2_GlobalEventManager_OnHitEnemy>(base_RoR2_GlobalEventManager_OnHitEnemy, value );
            remove => HookEndpointManager.Unmodify<hook_RoR2_GlobalEventManager_OnHitEnemy>( base_RoR2_GlobalEventManager_OnHitEnemy, value );

        }
        public static event hook_RoR2_GlobalEventManager_OnHitEnemy on_RoR2_GlobalEventManager_OnHitEnemy
        {
            add => HookEndpointManager.Add<hook_RoR2_GlobalEventManager_OnHitEnemy>( base_RoR2_GlobalEventManager_OnHitEnemy, value );
            remove => HookEndpointManager.Remove<hook_RoR2_GlobalEventManager_OnHitEnemy>( base_RoR2_GlobalEventManager_OnHitEnemy, value );
        }
        [EditorBrowsable( EditorBrowsableState.Never )]
        public delegate void orig_RoR2_GlobalEventManager_OnHitEnemy(  GlobalEventManager self, DamageInfo info, GameObject victim );
        [EditorBrowsable( EditorBrowsableState.Never )]
        public delegate void hook_RoR2_GlobalEventManager_OnHitEnemy( orig_RoR2_GlobalEventManager_OnHitEnemy orig, GlobalEventManager self, DamageInfo info, GameObject victim );
        private static MethodBase base_RoR2_GlobalEventManager_OnHitEnemy
        {
            get
            {
                if( _base_RoR2_GlobalEventManager_OnHitEnemy == null ) _base_RoR2_GlobalEventManager_OnHitEnemy = HookHelpers.GetMethodBase<GlobalEventManager,DamageInfo,GameObject>( "OnHitEnemy" );
                return _base_RoR2_GlobalEventManager_OnHitEnemy;
            }
        }
        private static MethodBase _base_RoR2_GlobalEventManager_OnHitEnemy;
        #endregion
        #endregion

        #region RoR2Application
        #region RoR2Application.UnitySystemConsoleRedirector
        #region Redirect
        public static event ILContext.Manipulator il_RoR2_RoR2Application_UnitySystemConsoleRedirector_Redirect
        {
            add => HookEndpointManager.Modify<hook_RoR2_RoR2Application_UnitySystemConsoleRedirector_Redirect>(base_RoR2_RoR2Application_UnitySystemConsoleRedirector_Redirect, value );
            remove => HookEndpointManager.Unmodify<hook_RoR2_RoR2Application_UnitySystemConsoleRedirector_Redirect>( base_RoR2_RoR2Application_UnitySystemConsoleRedirector_Redirect, value );

        }
        public static event hook_RoR2_RoR2Application_UnitySystemConsoleRedirector_Redirect on_RoR2_RoR2Application_UnitySystemConsoleRedirector_Redirect
        {
            add => HookEndpointManager.Add<hook_RoR2_RoR2Application_UnitySystemConsoleRedirector_Redirect>( base_RoR2_RoR2Application_UnitySystemConsoleRedirector_Redirect, value );
            remove => HookEndpointManager.Remove<hook_RoR2_RoR2Application_UnitySystemConsoleRedirector_Redirect>( base_RoR2_RoR2Application_UnitySystemConsoleRedirector_Redirect, value );
        }
        [EditorBrowsable( EditorBrowsableState.Never )]
        public delegate void orig_RoR2_RoR2Application_UnitySystemConsoleRedirector_Redirect();
        [EditorBrowsable( EditorBrowsableState.Never )]
        public delegate void hook_RoR2_RoR2Application_UnitySystemConsoleRedirector_Redirect( orig_RoR2_RoR2Application_UnitySystemConsoleRedirector_Redirect orig );
        private static MethodBase base_RoR2_RoR2Application_UnitySystemConsoleRedirector_Redirect
        {
            get
            {
                if( _base_RoR2_RoR2Application_UnitySystemConsoleRedirector_Redirect == null ) _base_RoR2_RoR2Application_UnitySystemConsoleRedirector_Redirect = HookHelpers.GetMethodBaseRoot( HookHelpers.GetNestedType( typeof( RoR2Application ), "UnitySystemConsoleRedirector" ), "Redirect" );
                return _base_RoR2_RoR2Application_UnitySystemConsoleRedirector_Redirect;
            }
        }
        private static MethodBase _base_RoR2_RoR2Application_UnitySystemConsoleRedirector_Redirect;

        #endregion
        #endregion

        #endregion

        #region SetStateOnHurt
        #region OnTakeDamageServer
        public static event ILContext.Manipulator il_RoR2_SetStateOnHurt_OnTakeDamageServer
        {
            add => HookEndpointManager.Modify<hook_RoR2_SetStateOnHurt_OnTakeDamageServer>(base_RoR2_SetStateOnHurt_OnTakeDamageServer, value );
            remove => HookEndpointManager.Unmodify<hook_RoR2_SetStateOnHurt_OnTakeDamageServer>( base_RoR2_SetStateOnHurt_OnTakeDamageServer, value );

        }
        public static event hook_RoR2_SetStateOnHurt_OnTakeDamageServer on_RoR2_SetStateOnHurt_OnTakeDamageServer
        {
            add => HookEndpointManager.Add<hook_RoR2_SetStateOnHurt_OnTakeDamageServer>( base_RoR2_SetStateOnHurt_OnTakeDamageServer, value );
            remove => HookEndpointManager.Remove<hook_RoR2_SetStateOnHurt_OnTakeDamageServer>( base_RoR2_SetStateOnHurt_OnTakeDamageServer, value );
        }
        [EditorBrowsable( EditorBrowsableState.Never )]
        public delegate void orig_RoR2_SetStateOnHurt_OnTakeDamageServer( SetStateOnHurt self, DamageReport damageReport );
        [EditorBrowsable( EditorBrowsableState.Never )]
        public delegate void hook_RoR2_SetStateOnHurt_OnTakeDamageServer( orig_RoR2_SetStateOnHurt_OnTakeDamageServer orig, SetStateOnHurt self, DamageReport damageReport );
        private static MethodBase base_RoR2_SetStateOnHurt_OnTakeDamageServer
        {
            get
            {
                if( _base_RoR2_SetStateOnHurt_OnTakeDamageServer == null ) _base_RoR2_SetStateOnHurt_OnTakeDamageServer = HookHelpers.GetMethodBase<SetStateOnHurt,DamageReport>( "OnTakeDamageServer" );
                return _base_RoR2_SetStateOnHurt_OnTakeDamageServer;
            }
        }
        private static MethodBase _base_RoR2_SetStateOnHurt_OnTakeDamageServer;
        #endregion
        #endregion

        #region SurvivorCatalog
        #region Init
        public static event ILContext.Manipulator il_RoR2_SurvivorCatalog_Init
        {
            add => HookEndpointManager.Modify<hook_RoR2_SurvivorCatalog_Init>(base_RoR2_SurvivorCatalog_Init, value );
            remove => HookEndpointManager.Unmodify<hook_RoR2_SurvivorCatalog_Init>( base_RoR2_SurvivorCatalog_Init, value );

        }
        public static event hook_RoR2_SurvivorCatalog_Init on_RoR2_SurvivorCatalog_Init
        {
            add => HookEndpointManager.Add<hook_RoR2_SurvivorCatalog_Init>( base_RoR2_SurvivorCatalog_Init, value );
            remove => HookEndpointManager.Remove<hook_RoR2_SurvivorCatalog_Init>( base_RoR2_SurvivorCatalog_Init, value );
        }
        [EditorBrowsable( EditorBrowsableState.Never )]
        public delegate void orig_RoR2_SurvivorCatalog_Init();
        [EditorBrowsable( EditorBrowsableState.Never )]
        public delegate void hook_RoR2_SurvivorCatalog_Init( orig_RoR2_SurvivorCatalog_Init orig );
        private static MethodBase base_RoR2_SurvivorCatalog_Init
        {
            get
            {
                if( _base_RoR2_SurvivorCatalog_Init == null ) _base_RoR2_SurvivorCatalog_Init = HookHelpers.GetMethodBaseRoot( typeof( RoR2.SurvivorCatalog ), "Init" );
                return _base_RoR2_SurvivorCatalog_Init;
            }
        }
        private static MethodBase _base_RoR2_SurvivorCatalog_Init;
        #endregion
        #endregion

        #region Util
        #region IsPrefab
        public static event ILContext.Manipulator il_RoR2_Util_IsPrefab
        {
            add => HookEndpointManager.Modify<hook_RoR2_Util_IsPrefab>(base_RoR2_Util_IsPrefab, value );
            remove => HookEndpointManager.Unmodify<hook_RoR2_Util_IsPrefab>( base_RoR2_Util_IsPrefab, value );

        }
        public static event hook_RoR2_Util_IsPrefab on_RoR2_Util_IsPrefab
        {
            add => HookEndpointManager.Add<hook_RoR2_Util_IsPrefab>( base_RoR2_Util_IsPrefab, value );
            remove => HookEndpointManager.Remove<hook_RoR2_Util_IsPrefab>( base_RoR2_Util_IsPrefab, value );
        }
        [EditorBrowsable( EditorBrowsableState.Never )]
        public delegate Boolean orig_RoR2_Util_IsPrefab( GameObject gameObject );
        [EditorBrowsable( EditorBrowsableState.Never )]
        public delegate Boolean hook_RoR2_Util_IsPrefab( orig_RoR2_Util_IsPrefab orig, GameObject gameObject );
        private static MethodBase base_RoR2_Util_IsPrefab
        {
            get
            {
                if( _base_RoR2_Util_IsPrefab == null ) _base_RoR2_Util_IsPrefab = HookHelpers.GetMethodBaseRoot( typeof( RoR2.Util ), "IsPrefab", typeof( GameObject ) );
                return _base_RoR2_Util_IsPrefab;
            }
        }
        private static MethodBase _base_RoR2_Util_IsPrefab;
        #endregion
        #endregion


        #region RoR2.Orbs
        #region OrbCatalog
        #region GenerateCatalog
        public static event ILContext.Manipulator il_RoR2_Orbs_OrbCatalog_GenerateCatalog
        {
            add => HookEndpointManager.Modify<hook_RoR2_Orbs_OrbCatalog_GenerateCatalog>(base_RoR2_Orbs_OrbCatalog_GenerateCatalog, value );
            remove => HookEndpointManager.Unmodify<hook_RoR2_Orbs_OrbCatalog_GenerateCatalog>( base_RoR2_Orbs_OrbCatalog_GenerateCatalog, value );

        }
        public static event hook_RoR2_Orbs_OrbCatalog_GenerateCatalog on_RoR2_Orbs_OrbCatalog_GenerateCatalog
        {
            add => HookEndpointManager.Add<hook_RoR2_Orbs_OrbCatalog_GenerateCatalog>( base_RoR2_Orbs_OrbCatalog_GenerateCatalog, value );
            remove => HookEndpointManager.Remove<hook_RoR2_Orbs_OrbCatalog_GenerateCatalog>( base_RoR2_Orbs_OrbCatalog_GenerateCatalog, value );
        }
        [EditorBrowsable( EditorBrowsableState.Never )]
        public delegate void orig_RoR2_Orbs_OrbCatalog_GenerateCatalog();
        [EditorBrowsable( EditorBrowsableState.Never )]
        public delegate void hook_RoR2_Orbs_OrbCatalog_GenerateCatalog( orig_RoR2_Orbs_OrbCatalog_GenerateCatalog orig );
        private static MethodBase base_RoR2_Orbs_OrbCatalog_GenerateCatalog
        {
            get
            {
                if( _base_RoR2_Orbs_OrbCatalog_GenerateCatalog == null ) _base_RoR2_Orbs_OrbCatalog_GenerateCatalog = HookHelpers.GetMethodBaseRoot( typeof( RoR2.Orbs.OrbCatalog ), "GenerateCatalog" );
                return _base_RoR2_Orbs_OrbCatalog_GenerateCatalog;
            }
        }
        private static MethodBase _base_RoR2_Orbs_OrbCatalog_GenerateCatalog;
        #endregion
        #endregion
        #endregion


        #region RoR2.Projectile
        #region ProjectileController
        #region Start
        public static event ILContext.Manipulator il_RoR2_Projectile_ProjectileController_Start
        {
            add => HookEndpointManager.Modify<hook_RoR2_Projectile_ProjectileController_Start>(base_RoR2_Projectile_ProjectileController_Start, value );
            remove => HookEndpointManager.Unmodify<hook_RoR2_Projectile_ProjectileController_Start>( base_RoR2_Projectile_ProjectileController_Start, value );

        }
        public static event hook_RoR2_Projectile_ProjectileController_Start on_RoR2_Projectile_ProjectileController_Start
        {
            add => HookEndpointManager.Add<hook_RoR2_Projectile_ProjectileController_Start>( base_RoR2_Projectile_ProjectileController_Start, value );
            remove => HookEndpointManager.Remove<hook_RoR2_Projectile_ProjectileController_Start>( base_RoR2_Projectile_ProjectileController_Start, value );
        }
        [EditorBrowsable( EditorBrowsableState.Never )]
        public delegate void orig_RoR2_Projectile_ProjectileController_Start( ProjectileController self );
        [EditorBrowsable( EditorBrowsableState.Never )]
        public delegate void hook_RoR2_Projectile_ProjectileController_Start( orig_RoR2_Projectile_ProjectileController_Start orig, ProjectileController self );
        private static MethodBase base_RoR2_Projectile_ProjectileController_Start
        {
            get
            {
                if( _base_RoR2_Projectile_ProjectileController_Start == null ) _base_RoR2_Projectile_ProjectileController_Start = HookHelpers.GetMethodBase<ProjectileController>( "Start" );
                return _base_RoR2_Projectile_ProjectileController_Start;
            }
        }
        private static MethodBase _base_RoR2_Projectile_ProjectileController_Start;
        #endregion
        #endregion

        #region ProjectileImpactExplosion
        #region FixedUpdate
        public static event ILContext.Manipulator il_RoR2_Projectile_ProjectileImpactExplosion_FixedUpdate
        {
            add => HookEndpointManager.Modify<hook_RoR2_Projectile_ProjectileImpactExplosion_FixedUpdate>(base_RoR2_Projectile_ProjectileImpactExplosion_FixedUpdate, value );
            remove => HookEndpointManager.Unmodify<hook_RoR2_Projectile_ProjectileImpactExplosion_FixedUpdate>( base_RoR2_Projectile_ProjectileImpactExplosion_FixedUpdate, value );

        }
        public static event hook_RoR2_Projectile_ProjectileImpactExplosion_FixedUpdate on_RoR2_Projectile_ProjectileImpactExplosion_FixedUpdate
        {
            add => HookEndpointManager.Add<hook_RoR2_Projectile_ProjectileImpactExplosion_FixedUpdate>( base_RoR2_Projectile_ProjectileImpactExplosion_FixedUpdate, value );
            remove => HookEndpointManager.Remove<hook_RoR2_Projectile_ProjectileImpactExplosion_FixedUpdate>( base_RoR2_Projectile_ProjectileImpactExplosion_FixedUpdate, value );
        }
        [EditorBrowsable( EditorBrowsableState.Never )]
        public delegate void orig_RoR2_Projectile_ProjectileImpactExplosion_FixedUpdate( ProjectileImpactExplosion self );
        [EditorBrowsable( EditorBrowsableState.Never )]
        public delegate void hook_RoR2_Projectile_ProjectileImpactExplosion_FixedUpdate( orig_RoR2_Projectile_ProjectileImpactExplosion_FixedUpdate orig, ProjectileImpactExplosion self );
        private static MethodBase base_RoR2_Projectile_ProjectileImpactExplosion_FixedUpdate
        {
            get
            {
                if( _base_RoR2_Projectile_ProjectileImpactExplosion_FixedUpdate == null ) _base_RoR2_Projectile_ProjectileImpactExplosion_FixedUpdate = HookHelpers.GetMethodBase<ProjectileImpactExplosion>( "FixedUpdate" );
                return _base_RoR2_Projectile_ProjectileImpactExplosion_FixedUpdate;
            }
        }
        private static MethodBase _base_RoR2_Projectile_ProjectileImpactExplosion_FixedUpdate;
        #endregion
        #endregion

        #endregion

        #region RoR2.UI
        #region CharacterSelectController
        #region Awake
        public static event ILContext.Manipulator il_RoR2_UI_CharacterSelectController_Awake
        {
            add => HookEndpointManager.Modify<hook_RoR2_UI_CharacterSelectController_Awake>(base_RoR2_UI_CharacterSelectController_Awake, value );
            remove => HookEndpointManager.Unmodify<hook_RoR2_UI_CharacterSelectController_Awake>( base_RoR2_UI_CharacterSelectController_Awake, value );

        }
        public static event hook_RoR2_UI_CharacterSelectController_Awake on_RoR2_UI_CharacterSelectController_Awake
        {
            add => HookEndpointManager.Add<hook_RoR2_UI_CharacterSelectController_Awake>( base_RoR2_UI_CharacterSelectController_Awake, value );
            remove => HookEndpointManager.Remove<hook_RoR2_UI_CharacterSelectController_Awake>( base_RoR2_UI_CharacterSelectController_Awake, value );
        }
        [EditorBrowsable( EditorBrowsableState.Never )]
        public delegate void orig_RoR2_UI_CharacterSelectController_Awake( RoR2.UI.CharacterSelectController self );
        [EditorBrowsable( EditorBrowsableState.Never )]
        public delegate void hook_RoR2_UI_CharacterSelectController_Awake( orig_RoR2_UI_CharacterSelectController_Awake orig, RoR2.UI.CharacterSelectController self );
        private static MethodBase base_RoR2_UI_CharacterSelectController_Awake
        {
            get
            {
                if( _base_RoR2_UI_CharacterSelectController_Awake == null ) _base_RoR2_UI_CharacterSelectController_Awake = HookHelpers.GetMethodBase<CharacterSelectController>( "Awake" );
                return _base_RoR2_UI_CharacterSelectController_Awake;
            }
        }
        private static MethodBase _base_RoR2_UI_CharacterSelectController_Awake;
        #endregion
        #region OnNetworkUserLoadoutChanged
        public static event ILContext.Manipulator il_RoR2_UI_CharacterSelectController_OnNetworkUserLoadoutChanged
        {
            add => HookEndpointManager.Modify<hook_RoR2_UI_CharacterSelectController_OnNetworkUserLoadoutChanged>(base_RoR2_UI_CharacterSelectController_OnNetworkUserLoadoutChanged, value );
            remove => HookEndpointManager.Unmodify<hook_RoR2_UI_CharacterSelectController_OnNetworkUserLoadoutChanged>( base_RoR2_UI_CharacterSelectController_OnNetworkUserLoadoutChanged, value );

        }
        public static event hook_RoR2_UI_CharacterSelectController_OnNetworkUserLoadoutChanged on_RoR2_UI_CharacterSelectController_OnNetworkUserLoadoutChanged
        {
            add => HookEndpointManager.Add<hook_RoR2_UI_CharacterSelectController_OnNetworkUserLoadoutChanged>( base_RoR2_UI_CharacterSelectController_OnNetworkUserLoadoutChanged, value );
            remove => HookEndpointManager.Remove<hook_RoR2_UI_CharacterSelectController_OnNetworkUserLoadoutChanged>( base_RoR2_UI_CharacterSelectController_OnNetworkUserLoadoutChanged, value );
        }
        [EditorBrowsable( EditorBrowsableState.Never )]
        public delegate void orig_RoR2_UI_CharacterSelectController_OnNetworkUserLoadoutChanged( RoR2.UI.CharacterSelectController self, NetworkUser networkUser );
        [EditorBrowsable( EditorBrowsableState.Never )]
        public delegate void hook_RoR2_UI_CharacterSelectController_OnNetworkUserLoadoutChanged( orig_RoR2_UI_CharacterSelectController_OnNetworkUserLoadoutChanged orig, RoR2.UI.CharacterSelectController self, NetworkUser networkUser );
        private static MethodBase base_RoR2_UI_CharacterSelectController_OnNetworkUserLoadoutChanged
        {
            get
            {
                if( _base_RoR2_UI_CharacterSelectController_OnNetworkUserLoadoutChanged == null ) _base_RoR2_UI_CharacterSelectController_OnNetworkUserLoadoutChanged = HookHelpers.GetMethodBase<CharacterSelectController,NetworkUser>( "OnNetworkUserLoadoutChanged" );
                return _base_RoR2_UI_CharacterSelectController_OnNetworkUserLoadoutChanged;
            }
        }
        private static MethodBase _base_RoR2_UI_CharacterSelectController_OnNetworkUserLoadoutChanged;
        #endregion
        #region RebuildLocal
        public static event ILContext.Manipulator il_RoR2_UI_CharacterSelectController_RebuildLocal
        {
            add => HookEndpointManager.Modify<hook_RoR2_UI_CharacterSelectController_RebuildLocal>(base_RoR2_UI_CharacterSelectController_RebuildLocal, value );
            remove => HookEndpointManager.Unmodify<hook_RoR2_UI_CharacterSelectController_RebuildLocal>( base_RoR2_UI_CharacterSelectController_RebuildLocal, value );

        }
        public static event hook_RoR2_UI_CharacterSelectController_RebuildLocal on_RoR2_UI_CharacterSelectController_RebuildLocal
        {
            add => HookEndpointManager.Add<hook_RoR2_UI_CharacterSelectController_RebuildLocal>( base_RoR2_UI_CharacterSelectController_RebuildLocal, value );
            remove => HookEndpointManager.Remove<hook_RoR2_UI_CharacterSelectController_RebuildLocal>( base_RoR2_UI_CharacterSelectController_RebuildLocal, value );
        }
        [EditorBrowsable( EditorBrowsableState.Never )]
        public delegate void orig_RoR2_UI_CharacterSelectController_RebuildLocal( RoR2.UI.CharacterSelectController self );
        [EditorBrowsable( EditorBrowsableState.Never )]
        public delegate void hook_RoR2_UI_CharacterSelectController_RebuildLocal( orig_RoR2_UI_CharacterSelectController_RebuildLocal orig, RoR2.UI.CharacterSelectController self );
        private static MethodBase base_RoR2_UI_CharacterSelectController_RebuildLocal
        {
            get
            {
                if( _base_RoR2_UI_CharacterSelectController_RebuildLocal == null ) _base_RoR2_UI_CharacterSelectController_RebuildLocal = HookHelpers.GetMethodBase<CharacterSelectController>( "RebuildLocal" );
                return _base_RoR2_UI_CharacterSelectController_RebuildLocal;
            }
        }
        private static MethodBase _base_RoR2_UI_CharacterSelectController_RebuildLocal;
        #endregion
        #endregion

        #region LoadoutPanelController
        #region Rebuild
        public static event ILContext.Manipulator il_RoR2_UI_LoadoutPanelController_Rebuild
        {
            add => HookEndpointManager.Modify<hook_RoR2_UI_LoadoutPanelController_Rebuild>(base_RoR2_UI_LoadoutPanelController_Rebuild, value );
            remove => HookEndpointManager.Unmodify<hook_RoR2_UI_LoadoutPanelController_Rebuild>( base_RoR2_UI_LoadoutPanelController_Rebuild, value );

        }
        public static event hook_RoR2_UI_LoadoutPanelController_Rebuild on_RoR2_UI_LoadoutPanelController_Rebuild
        {
            add => HookEndpointManager.Add<hook_RoR2_UI_LoadoutPanelController_Rebuild>( base_RoR2_UI_LoadoutPanelController_Rebuild, value );
            remove => HookEndpointManager.Remove<hook_RoR2_UI_LoadoutPanelController_Rebuild>( base_RoR2_UI_LoadoutPanelController_Rebuild, value );
        }
        [EditorBrowsable( EditorBrowsableState.Never )]
        public delegate void orig_RoR2_UI_LoadoutPanelController_Rebuild( RoR2.UI.LoadoutPanelController self );
        [EditorBrowsable( EditorBrowsableState.Never )]
        public delegate void hook_RoR2_UI_LoadoutPanelController_Rebuild( orig_RoR2_UI_LoadoutPanelController_Rebuild orig, RoR2.UI.LoadoutPanelController self );
        private static MethodBase base_RoR2_UI_LoadoutPanelController_Rebuild
        {
            get
            {
                if( _base_RoR2_UI_LoadoutPanelController_Rebuild == null ) _base_RoR2_UI_LoadoutPanelController_Rebuild = HookHelpers.GetMethodBase<LoadoutPanelController>( "Rebuild" );
                return _base_RoR2_UI_LoadoutPanelController_Rebuild;
            }
        }
        private static MethodBase _base_RoR2_UI_LoadoutPanelController_Rebuild;
        #endregion
        #endregion

        #region CrosshairManager
        #region UpdateCrosshair
        public static event ILContext.Manipulator il_RoR2_UI_CrosshairManager_UpdateCrosshair
        {
            add => HookEndpointManager.Modify<hook_RoR2_UI_CrosshairManager_UpdateCrosshair>(base_RoR2_UI_CrosshairManager_UpdateCrosshair, value );
            remove => HookEndpointManager.Unmodify<hook_RoR2_UI_CrosshairManager_UpdateCrosshair>( base_RoR2_UI_CrosshairManager_UpdateCrosshair, value );

        }
        public static event hook_RoR2_UI_CrosshairManager_UpdateCrosshair on_RoR2_UI_CrosshairManager_UpdateCrosshair
        {
            add => HookEndpointManager.Add<hook_RoR2_UI_CrosshairManager_UpdateCrosshair>( base_RoR2_UI_CrosshairManager_UpdateCrosshair, value );
            remove => HookEndpointManager.Remove<hook_RoR2_UI_CrosshairManager_UpdateCrosshair>( base_RoR2_UI_CrosshairManager_UpdateCrosshair, value );
        }
        [EditorBrowsable( EditorBrowsableState.Never )]
        public delegate void orig_RoR2_UI_CrosshairManager_UpdateCrosshair( CrosshairManager self, CharacterBody targetBody, Vector3 crosshairWorldPosition, Camera uiCamera );
        [EditorBrowsable( EditorBrowsableState.Never )]
        public delegate void hook_RoR2_UI_CrosshairManager_UpdateCrosshair( orig_RoR2_UI_CrosshairManager_UpdateCrosshair orig, CrosshairManager self, CharacterBody targetBody, Vector3 crosshairWorldPosition, Camera uiCamera );
        private static MethodBase base_RoR2_UI_CrosshairManager_UpdateCrosshair
        {
            get
            {
                if( _base_RoR2_UI_CrosshairManager_UpdateCrosshair == null ) _base_RoR2_UI_CrosshairManager_UpdateCrosshair = HookHelpers.GetMethodBase<CrosshairManager, CharacterBody, Vector3, Camera>( "UpdateCrosshair" );
                return _base_RoR2_UI_CrosshairManager_UpdateCrosshair;
            }
        }
        private static MethodBase _base_RoR2_UI_CrosshairManager_UpdateCrosshair;
        #endregion
        #endregion
        #endregion
        #endregion
    }
}
