using System;
using System.Collections.Generic;
using System.Text;
using EntityStates;
using RoR2;
using Sniper.Components;
using Sniper.Modules;
using UnityEngine;
using UnityEngine.Networking;
using ReinCore;

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
            Log.Warning( "DecoyDeath OnEnter" );
            var sync = base.gameObject?.GetComponent<DecoyDeployableSync>();
            if( NetworkServer.active )
            {
                Log.Warning( "DecoyDeath OnEnter server" );


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
                this._SetRestStopwatch( 100f );
                this._SetFallingStopwatch( 100f );

            }
            sync.BodyKilled();
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        protected override void OnPreDestroyBodyServer()
        {
            base.OnPreDestroyBodyServer();
            base.DestroyModel();
        }

    }
}
