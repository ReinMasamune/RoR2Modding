using BepInEx;
using RoR2;
using UnityEngine;
using R2API;
using R2API.Utils;
using System.Reflection;
using EntityStates;
using RoR2.Skills;
using System;
using MonoMod.Cil;
using Mono.Cecil.Cil;
using System.Collections.Generic;
using System.Collections;

namespace ReinSniperRework
{
    internal partial class Main
    {
        partial void Hooks()
        {
            this.Enable += this.AddHooks;
            this.Disable += this.RemoveHooks;
        }

        private void RemoveHooks()
        {

        }

        private void AddHooks()
        {
            GlobalEventManager.onServerDamageDealt += this.GlobalEventManager_onServerDamageDealt;
            GlobalEventManager.onCharacterDeathGlobal += this.GlobalEventManager_onCharacterDeathGlobal;
        }

        private void GlobalEventManager_onCharacterDeathGlobal( DamageReport obj )
        {
            if( obj.victimBody.HasBuff( this.resetDebuff ) )
            {
                LogI( "ResetKill?" );
                LogI( obj.attackerBody.baseNameToken );
                if( obj.attackerBody.baseNameToken == "SNIPER_BODY_NAME" )
                {
                    obj.attackerBody.skillLocator.ApplyAmmoPack();
                    obj.attackerBody.skillLocator.primary.stateMachine.SetState( new SniperLoaded
                    {
                        quality = SniperShoot.ReloadQuality.Perfect
                    } );
                }
            }
        }

        private void GlobalEventManager_onServerDamageDealt( DamageReport obj )
        {
            if( ( obj.damageInfo.damageType & this.resetOnKill ) > DamageType.Generic )
            {
                obj.victimBody.AddTimedBuff( this.resetDebuff, this.resetDebuffTime );
            }
        }
    }
}


