using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BepInEx.Logging;
using ReinCore;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace Sniper.Modules
{
    internal static class DecoyModule
    {
        private static GameObject decoyPrefab;
        internal static GameObject GetDecoyPrefab()
        {
            if( decoyPrefab == null )
            {
                decoyPrefab = CreateDecoyPrefab();
            }

            return decoyPrefab;
        }

        private static GameObject CreateDecoyPrefab()
        {
            GameObject obj = PrefabsCore.CreatePrefab("SniperDecoy", true );

            NetworkIdentity netId = obj.AddOrGetComponent<NetworkIdentity>();
            // TODO: LocalPlayerAuthority?


            SkillLocator skillLoc = obj.AddOrGetComponent<SkillLocator>();
            skillLoc.primary = null;
            skillLoc.secondary = null;
            skillLoc.utility = null;
            skillLoc.special = null;
            skillLoc.passiveSkill = new SkillLocator.PassiveSkill
            {
                enabled = false,
                icon = null,
                skillDescriptionToken = null,
                skillNameToken = null,
            };




            TeamComponent team = obj.AddOrGetComponent<TeamComponent>();
            team.hideAllyCardDisplay = false;
            team.teamIndex = TeamIndex.Player;



            CharacterBody body = obj.AddOrGetComponent<CharacterBody>();
            body.bodyIndex = -1;
            body.baseNameToken = Properties.Tokens.SNIPER_NAME;
            body.subtitleNameToken = Properties.Tokens.SNIPER_SUBTITLE;
            body.bodyFlags = CharacterBody.BodyFlags.ResistantToAOE;
            //body.


            HealthComponent health = obj.AddOrGetComponent<HealthComponent>();



            Rigidbody rb = obj.AddOrGetComponent<Rigidbody>();



            EntityStateMachine esm = obj.AddOrGetComponent<EntityStateMachine>();



            CharacterDeathBehavior deathBehaviour = obj.AddOrGetComponent<CharacterDeathBehavior>();



            NetworkStateMachine netStates = obj.AddOrGetComponent<NetworkStateMachine>();



            // TODO: Unknown Component


            CapsuleCollider collider = obj.AddOrGetComponent<CapsuleCollider>();



            ModelLocator modelLoc = obj.AddOrGetComponent<ModelLocator>();



            return obj; 
        }
    }
}
