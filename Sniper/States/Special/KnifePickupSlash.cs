namespace Sniper.States.Special
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using RoR2;
    using Sniper.Modules;
    using Sniper.States.Bases;
    using UnityEngine;
    using UnityEngine.Networking;

    internal class KnifePickupSlash : SniperSkillBaseState
    {
        private const Single baseDuration = 0.5f;
        private const Single damageMult = 2f;
        private const Single slashRadius = 6f;

        private Single duration;

        internal GameObject knifeObject;

        private static GameObject slashEffectPrefab = VFXModule.GetKnifeSlashPrefab();


        public override void OnEnter()
        {
            base.OnEnter();

            this.duration = baseDuration / base.attackSpeedStat;

            if( this.knifeObject != null )
            {
                Log.WarningT( "Non null knife!" );
            }

            if( NetworkServer.active && this.knifeObject != null )
            {
                NetworkServer.Destroy( this.knifeObject );
            }

            if( base.isAuthority )
            {
                _ = new BlastAttack
                {
                    attacker = base.gameObject,
                    attackerFiltering = AttackerFiltering.NeverHit,
                    baseDamage = base.damageStat * damageMult,
                    baseForce = 0f,
                    bonusForce = Vector3.zero,
                    crit = base.RollCrit(),
                    damageColorIndex = DamageColorIndex.Default,
                    damageType = CatalogModule.sniperResetDamageType,
                    falloffModel = BlastAttack.FalloffModel.None,
                    impactEffect = EffectIndex.Invalid,
                    inflictor = null,
                    losType = BlastAttack.LoSType.None,
                    position = base.transform.position,
                    procChainMask = default,
                    procCoefficient = 1f,
                    radius = slashRadius,
                    teamIndex = base.GetTeam(),
                }.Fire();

                var data = new EffectData
                {
                    origin = base.transform.position,
                    scale = slashRadius,
                };
                EffectManager.SpawnEffect( slashEffectPrefab, data, true );

            }
        }

        public override void OnSerialize( NetworkWriter writer )
        {
            base.OnSerialize( writer );
            writer.Write( knifeObject );
        }

        public override void OnDeserialize( NetworkReader reader )
        {
            base.OnDeserialize( reader );
            this.knifeObject = reader.ReadGameObject();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if( base.isAuthority && base.fixedAge >= this.duration )
            {
                base.outer.SetNextStateToMain();
            }
        }
    }
}
