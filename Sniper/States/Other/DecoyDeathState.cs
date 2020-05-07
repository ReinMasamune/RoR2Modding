using System;
using System.Collections.Generic;
using System.Text;
using EntityStates;
using RoR2;
using Sniper.Modules;
using UnityEngine;
using UnityEngine.Networking;

namespace Sniper.States.Other
{
    internal class DecoyDeathState : GenericCharacterDeath
    {
        const Single explosionRadius = 10f;
        const Single damageRatio = 5f;

        internal static GameObject explosionPrefab = VFXModule.GetExplosiveAmmoExplosionPrefab();


        public override void OnEnter()
        {
            base.OnEnter();

            if( NetworkServer.active )
            {
                var ownerBody = base.characterBody?.master?.minionOwnership?.ownerMaster?.GetBody();
                if( ownerBody != null )
                {
                    if( explosionPrefab != null )
                    {
                        EffectManager.SpawnEffect( explosionPrefab, new EffectData
                        {
                            origin = base.transform.position,
                            scale = explosionRadius
                        }, true );
                    }

                    _ = new BlastAttack
                    {
                        attacker = ownerBody.gameObject,
                        attackerFiltering = AttackerFiltering.Default,
                        baseDamage = ownerBody.damage * damageRatio,
                        baseForce = 50f,
                        bonusForce = Vector3.zero,
                        crit = ownerBody.RollCrit(),
                        damageColorIndex = DamageColorIndex.Item,
                        damageType = DamageType.AOE | DamageType.WeakOnHit | DamageType.Stun1s,
                        falloffModel = BlastAttack.FalloffModel.Linear,
                        impactEffect = EffectIndex.Invalid,
                        inflictor = base.gameObject,
                        losType = BlastAttack.LoSType.None,
                        position = base.transform.position,
                        procChainMask = default,
                        procCoefficient = 1f,
                        radius = explosionRadius,
                        teamIndex = ownerBody.teamComponent.teamIndex,
                    }.Fire();


                    // TODO: Play Sound
                } else
                {
                    Log.Warning( "No owner found" );
                }
            }
        }

    }
}
