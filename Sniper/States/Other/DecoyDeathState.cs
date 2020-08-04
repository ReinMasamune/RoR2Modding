namespace Sniper.States.Other
{
    using System;

    using EntityStates;

    using ReinCore;

    using RoR2;

    using Sniper.Components;
    using Sniper.Modules;

    using UnityEngine;
    using UnityEngine.Networking;

    internal class DecoyDeathState : GenericCharacterDeath
    {
        private const Single explosionRadius = 10f;
        private const Single damageRatio = 5f;

        internal static GameObject explosionPrefab = VFXModule.GetExplosiveAmmoExplosionPrefab();


        public override void OnEnter()
        {
            base.OnEnter();
            DecoyDeployableSync sync = base.gameObject?.GetComponent<DecoyDeployableSync>();
            if( NetworkServer.active )
            {
                CharacterBody ownerBody = base.characterBody?.master?.minionOwnership?.ownerMaster?.GetBody();
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
                }
            }
            this._SetRestStopwatch( 100f );
            this._SetFallingStopwatch( 100f );
            // FUTURE: Play Sound
            sync.BodyKilled();
        }

        public override void OnPreDestroyBodyServer()
        {
            base.OnPreDestroyBodyServer();
            base.DestroyModel();
        }

    }
}
