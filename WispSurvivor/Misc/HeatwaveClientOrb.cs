﻿using RoR2;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RogueWispPlugin.Misc
{
    public class HeatwaveClientOrb : BaseClientOrb
    {
        public System.Single speed = 200f;
        public System.Single damage = 1f;
        public System.Single scale = 1f;
        public System.Single procCoef = 1f;
        public System.Single radius = 1f;
        public Single force = 0f;
        public Single chargeRestore = 0.0f;
        public Single range = 0f;
        public Single falloffStart = 0f;
        public Single endFalloffMult = 0f;
        public System.UInt32 skin = 0;


        public Vector3 startPos;
        public Vector3 targetPos;
        public Vector3 worldNormal;
        public System.Boolean crit = false;
        public Boolean hitWorld = false;


        public TeamIndex team;
        public DamageColorIndex damageColor;

        public GameObject attacker;
        public ProcChainMask procMask;

        private Single dist1;
        private Single dist2;


        private Vector3 lastPos;

        private Vector3 forceVec;

        public List<HealthComponent> mask = new List<HealthComponent>();

        public override void Begin()
        {
            this.forceVec = this.targetPos - this.startPos;
            base.totalDuration = Vector3.Magnitude( this.forceVec ) / this.speed;
            this.forceVec = Vector3.Normalize( this.forceVec );
            this.forceVec *= this.force;

            EffectData effectData = new EffectData
            {
                origin = this.startPos,
                genericFloat = base.totalDuration,
                genericBool = false,
                start = this.targetPos,           
            };

            this.lastPos = this.startPos;

            EffectManager.SpawnEffect( Main.primaryOrbEffects[this.skin], effectData, true );

            this.dist1 = this.range * this.falloffStart;
            this.dist2 = this.range * (1f - this.falloffStart);
        }
        public override void Tick( Single deltaT )
        {
            Vector3 currentPos = Vector3.Lerp( this.targetPos , this.startPos, ( base.remainingDuration / base.totalDuration ) );

            Single curDist = Vector3.Magnitude( currentPos - this.startPos );
            Single curMult = 1f;
            if( curDist > this.dist1 )
            {
                curMult -= ((curDist - this.dist1) / this.dist2) * (1f - this.endFalloffMult);
            }


            Collider[] cols = Physics.OverlapCapsule( this.lastPos, currentPos, this.radius, LayerIndex.entityPrecise.mask, QueryTriggerInteraction.UseGlobal );

            foreach( Collider col in cols )
            {
                if( !col ) continue;
                HurtBox box = col.GetComponent<HurtBox>();
                if( !box ) continue;
                HealthComponent hcomp = box.healthComponent;
                if( !hcomp || this.mask.Contains( hcomp ) || TeamComponent.GetObjectTeam( hcomp.gameObject ) == this.team ) continue;

                DamageInfo dmg = new DamageInfo();
                dmg.damage = this.damage * curMult;
                dmg.attacker = this.attacker;
                dmg.crit = this.crit;
                dmg.damageColorIndex = this.damageColor;
                dmg.damageType = DamageType.Generic;
                dmg.force = this.forceVec;
                dmg.inflictor = this.attacker;
                dmg.position = col.transform.position;
                dmg.procChainMask = this.procMask;
                dmg.procCoefficient = this.procCoef;

                this.mask.Add( hcomp );
                base.SendDamage( dmg, hcomp.gameObject );

                EffectManager.SpawnEffect( Main.genericImpactEffects[this.skin][0], new EffectData
                {
                    origin = box.transform.position
                }, true );

                CharacterBody targetBody = hcomp.GetComponent<CharacterBody>();
                Inventory targetInv = (targetBody ? targetBody.inventory : null);

                Single dur = curMult * this.chargeRestore * this.GetChargeMult( targetInv ? targetInv.GetItemCount( ItemIndex.BoostDamage ) : 0, targetBody.baseNameToken );
                UInt32 stacks = 1u;
                if( dur > 0.75f )
                {
                    stacks = (UInt32)Mathf.RoundToInt( dur / 0.75f );
                    dur = 0.75f;
                }

                Debug.Log( "Stacks:" + stacks.ToString() );
                Debug.Log( "Duration:" + dur.ToString() );

                EffectData fx = new EffectData
                {
                    origin = box.transform.position,
                    start = this.attacker.transform.position,
                    genericFloat = (curDist / this.speed) * 3.25f,
                    genericUInt = stacks,
                    scale = 0.65f * curMult,
                    genericBool = false,
                    color = DoBadThings(dur)
                };
                fx.SetHurtBoxReference( this.attacker );
                EffectManager.SpawnEffect( Main.utilityLeech[this.skin], fx, true );
            }
        }
        public override void End()
        {
        }

        public static Color32 DoBadThings( Single f )
        {
            Byte[] v = BitConverter.GetBytes( f );

            return new Color32
            {
                r = v[0],
                g = v[1],
                b = v[2],
                a = v[3]
            };
        }

        public static Single DoMoreBadThings( Color32 color )
        {
            Byte[] v = new Byte[4]
            {
                color.r,
                color.g,
                color.b,
                color.a
            };

            return BitConverter.ToSingle( v, 0 );
        }

        private Single GetChargeMult( Int32 damageBoost, String name )
        {
            Single temp = 1f;

            temp *= 1f + (damageBoost / 10f);

            switch( this.GetMonsterTier( name ) )
            {
                case MonsterTier.Basic:
                    temp *= 0.35f;
                    break;
                case MonsterTier.Small:
                    temp *= 0.5f;
                    break;
                case MonsterTier.Mid:
                    temp *= 0.65f;
                    break;
                case MonsterTier.Miniboss:
                    temp *= 0.85f;
                    break;
                case MonsterTier.Boss:
                    temp *= 1.0f;
                    break;
                case MonsterTier.SuperBoss:
                    temp *= 6f;
                    break;
                case MonsterTier.HyperBoss:
                    temp *= 1f;
                    break;
                case MonsterTier.Other:
                    temp *= 0.1f;
                    break;
            }
            return temp;
        }

        private enum MonsterTier
        {
            Basic = 0,
            Small = 1,
            Mid = 2,
            Miniboss = 3,
            Boss = 4,
            SuperBoss = 5,
            HyperBoss = 6,
            Other = 7
        }

        private MonsterTier GetMonsterTier( String name )
        {
            switch( name )
            {
                default:
                    Debug.Log( name + " is not a handled monster" );
                    return MonsterTier.Other;

                case "LEMURIAN_BODY_NAME":
                    return MonsterTier.Basic;
                case "WISP_BODY_NAME":
                    return MonsterTier.Basic;
                case "BEETLE_BODY_NAME":
                    return MonsterTier.Basic;
                case "JELLYFISH_BODY_NAME":
                    return MonsterTier.Basic;
                case "CLAY_BODY_NAME":
                    return MonsterTier.Basic;


                case "HERMIT_CRAB_BODY_NAME":
                    return MonsterTier.Small;
                case "IMP_BODY_NAME":
                    return MonsterTier.Small;
                case "ROBOBALLMINI_BODY_NAME":
                    return MonsterTier.Small;
                case "VULTURE_BODY_NAME":
                    return MonsterTier.Small;
                case "URCHINTURRET_BODY_NAME":
                    return MonsterTier.Small;


                case "GOLEM_BODY_NAME":
                    return MonsterTier.Mid;
                case "BEETLEGUARD_BODY_NAME":
                    return MonsterTier.Mid;
                case "BELL_BODY_NAME":
                    return MonsterTier.Mid;
                case "BISON_BODY_NAME":
                    return MonsterTier.Mid;


                case "LEMURIANBRUISER_BODY_NAME":
                    return MonsterTier.Miniboss;
                case "GREATERWISP_BODY_NAME":
                    return MonsterTier.Miniboss;
                case "CLAYBRUISER_BODY_NAME":
                    return MonsterTier.Miniboss;
                case "ARCHWISP_BODY_NAME":
                    return MonsterTier.Miniboss;


                case "BEETLEQUEEN_BODY_NAME":
                    return MonsterTier.Boss;
                case "TITAN_BODY_NAME":
                    return MonsterTier.Boss;
                case "MAGMAWORM_BODY_NAME":
                    return MonsterTier.Boss;
                case "CLAYBOSS_BODY_NAME":
                    return MonsterTier.Boss;
                case "GRAVEKEEPER_BODY_NAME":
                    return MonsterTier.Boss;
                case "IMPBOSS_BODY_NAME":
                    return MonsterTier.Boss;
                case "ROBOBALLBOSS_BODY_NAME":
                    return MonsterTier.Boss;
                case "VAGRANT_BODY_NAME":
                    return MonsterTier.Boss;


                case "ELECTRICWORM_BODY_NAME":
                    return MonsterTier.SuperBoss;
                case "SHOPKEEPER_BODY_NAME":
                    return MonsterTier.SuperBoss;


                case "ANCIENTWISP_BODY_NAME":
                    return MonsterTier.HyperBoss;
                case "TITANGOLD_BODY_NAME":
                    return MonsterTier.HyperBoss;
                case "SUPERROBOBALLBOSS_BODY_NAME":
                    return MonsterTier.HyperBoss;


                case "DRONE_BACKUP_BODY_NAME":
                    return MonsterTier.Basic;
                case "DRONEBACKUP_BODY_NAME":
                    return MonsterTier.Basic;
                case "DRONE_GUNNER_BODY_NAME":
                    return MonsterTier.Basic;
                case "DRONE_HEALING_BODY_NAME":
                    return MonsterTier.Basic;
                case "ENGITURET_BODY_NAME":
                    return MonsterTier.Basic;
                case "ENGI_BODY_NAME":
                    return MonsterTier.Mid;
                case "BANDIT_BODY_NAME":
                    return MonsterTier.Mid;
                case "BOMBER_BODY_NAME":
                    return MonsterTier.Mid;
                case "COMMANDO_BODY_NAME":
                    return MonsterTier.Mid;
                case "ENFORCER_BODY_NAME":
                    return MonsterTier.Mid;
                case "HAND_BODY_NAME":
                    return MonsterTier.Mid;
                case "HUNTRESS_BODY_NAME":
                    return MonsterTier.Mid;
                case "LOADER_BODY_NAME":
                    return MonsterTier.Mid;
                case "MAGE_BODY_NAME":
                    return MonsterTier.Mid;
                case "MERC_BODY_NAME":
                    return MonsterTier.Mid;
                case "SNIPER_BODY_NAME":
                    return MonsterTier.Mid;
                case "TOOLBOT_BODY_NAME":
                    return MonsterTier.Mid;
                case "TREEBOT_BODY_NAME":
                    return MonsterTier.Mid;
                case "TURRET1_BODY_NAME":
                    return MonsterTier.Basic;
                case "SQUIDTURRET_BODY_NAME":
                    return MonsterTier.Mid;
                case "EQUIPMENTDRONE_BODY_NAME":
                    return MonsterTier.Mid;
                case "FLAMEDRONE_BODY_NAME":
                    return MonsterTier.Mid;
                case "POTMOBILE_BODY_NAME":
                    return MonsterTier.Mid;
                case "DRONE_MEGA_BODY_NAME":
                    return MonsterTier.Mid;
                case "DRONE_MISSILE_BODY_NAME":
                    return MonsterTier.Mid;

                case "POT2_BODY_NAME":
                    return MonsterTier.Other;
                case "":
                    return MonsterTier.Other;
            }
        }

    }
}