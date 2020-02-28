using BepInEx;
using RoR2;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using R2API;
using UnityEngine;
using MonoMod.Cil;
using MonoMod.RuntimeDetour.HookGen;

namespace GeneralPluginStuff
{
    internal static class HookDelegates
    {
        #region RoR2
        #region CharacterBody
        #region Start
        internal static event ILContext.Manipulator il_RoR2_CharacterBody_Start
        {
            add
            {
                if( base_RoR2_CharacterBody_Start == null )
                    base_RoR2_CharacterBody_Start = HookFuncs.GetMethodBase<CharacterBody,CharacterBody>("Start");
                HookEndpointManager.Modify<hook_RoR2_CharacterBody_Start>(base_RoR2_CharacterBody_Start, value );
            }
            remove
            {
                if( base_RoR2_CharacterBody_Start == null )
                    base_RoR2_CharacterBody_Start = HookFuncs.GetMethodBase<CharacterBody,CharacterBody>("Start");
                HookEndpointManager.Unmodify<hook_RoR2_CharacterBody_Start>( base_RoR2_CharacterBody_Start, value );
            }
        }
        internal static event hook_RoR2_CharacterBody_Start on_RoR2_CharacterBody_Start
        {
            add
            {
                if( base_RoR2_CharacterBody_Start == null )
                    base_RoR2_CharacterBody_Start = HookFuncs.GetMethodBase<CharacterBody, CharacterBody>("Start");
                HookEndpointManager.Add<hook_RoR2_CharacterBody_Start>( base_RoR2_CharacterBody_Start, value );
            }
            remove
            {
                if( base_RoR2_CharacterBody_Start == null )
                    base_RoR2_CharacterBody_Start = HookFuncs.GetMethodBase<CharacterBody, CharacterBody>("Start");
                HookEndpointManager.Remove<hook_RoR2_CharacterBody_Start>( base_RoR2_CharacterBody_Start, value );
            }
        }
        internal delegate void orig_RoR2_CharacterBody_Start( CharacterBody self );
        internal delegate void hook_RoR2_CharacterBody_Start( orig_RoR2_CharacterBody_Start orig, CharacterBody self );
        private static MethodBase base_RoR2_CharacterBody_Start;
        #endregion



        #endregion
        #region GlobalEventManager
        #region OnHitEnemy
        internal static event ILContext.Manipulator il_RoR2_GlobalEventManager_OnHitEnemy
        {
            add
            {
                if( base_RoR2_GlobalEventManager_OnHitEnemy == null )
                    base_RoR2_GlobalEventManager_OnHitEnemy = HookFuncs.GetMethodBase<GlobalEventManager, GlobalEventManager, DamageInfo,GameObject>("OnHitEnemy");
                HookEndpointManager.Modify<hook_RoR2_GlobalEventManager_OnHitEnemy>(base_RoR2_GlobalEventManager_OnHitEnemy, value );
            }
            remove
            {
                if( base_RoR2_GlobalEventManager_OnHitEnemy == null )
                    base_RoR2_GlobalEventManager_OnHitEnemy = HookFuncs.GetMethodBase<GlobalEventManager,GlobalEventManager, DamageInfo,GameObject>("OnHitEnemy");
                HookEndpointManager.Unmodify<hook_RoR2_GlobalEventManager_OnHitEnemy>( base_RoR2_GlobalEventManager_OnHitEnemy, value );
            }
        }
        internal static event hook_RoR2_GlobalEventManager_OnHitEnemy on_RoR2_GlobalEventManager_OnHitEnemy
        {
            add
            {
                if( base_RoR2_GlobalEventManager_OnHitEnemy == null )
                    base_RoR2_GlobalEventManager_OnHitEnemy = HookFuncs.GetMethodBase<GlobalEventManager, GlobalEventManager, DamageInfo,GameObject>("OnHitEnemy");
                HookEndpointManager.Add<hook_RoR2_GlobalEventManager_OnHitEnemy>( base_RoR2_GlobalEventManager_OnHitEnemy, value );
            }
            remove
            {
                if( base_RoR2_GlobalEventManager_OnHitEnemy == null )
                    base_RoR2_GlobalEventManager_OnHitEnemy = HookFuncs.GetMethodBase<GlobalEventManager, GlobalEventManager, DamageInfo,GameObject>("OnHitEnemy");
                HookEndpointManager.Remove<hook_RoR2_GlobalEventManager_OnHitEnemy>( base_RoR2_GlobalEventManager_OnHitEnemy, value );
            }
        }
        internal delegate void orig_RoR2_GlobalEventManager_OnHitEnemy( GlobalEventManager self, DamageInfo damage, GameObject victim );
        internal delegate void hook_RoR2_GlobalEventManager_OnHitEnemy( orig_RoR2_GlobalEventManager_OnHitEnemy orig, GlobalEventManager self, DamageInfo damage, GameObject victim );
        private static MethodBase base_RoR2_GlobalEventManager_OnHitEnemy;
        #endregion


        #endregion




        #endregion
        #region Weird hooks (Hooking other hooks)
        //private static MethodBase base_R2API_LoadoutAPI_BodyLoadout_ToXml;
        //internal delegate System.Xml.Linq.XElement il_R2API_LoadoutAPI_BodyLoadout_ToXml( On.RoR2.Loadout.BodyLoadoutManager.orig_ToXml orig, System.Object self, String elementName );
        //internal static event ILContext.Manipulator il_R2API
        #endregion
    }
}
