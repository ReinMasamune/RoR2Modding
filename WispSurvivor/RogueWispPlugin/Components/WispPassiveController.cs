using System;

using Rein.Properties;

using RoR2;

using UnityEngine;

namespace Rein.RogueWispPlugin
{
    internal partial class Main
    {
        public class WispPassiveController : MonoBehaviour
        {
            public static Single GetMultiplierForBody(CharacterBody target)
            {
                if(target is null) return 1f;
                var boostCount = target.inventory?.GetItemCount(ItemIndex.BoostDamage) ?? 0;
                var nameMult = GetNameMultiplier(target?.baseNameToken);

                var mul = (1f + 0.1f * boostCount) * nameMult;
                if(mul == 0f) Log.Warning($"Unhandled body: {target?.baseNameToken}");
                return mul;
            }
            private static Single GetNameMultiplier(String name) => GetMonsterTier(name) switch
            {
                Mt.Invalid => 0f,
                Mt.Environment => 0.1f,
                Mt.Basic => 0.35f,
                Mt.BasicUp => 0.5f,
                Mt.Miniboss => 0.65f,
                Mt.MinibossUp => 0.85f,
                Mt.MinibossUpUp => 1f,
                Mt.Champion => 1.25f,
                Mt.EliteChamp => 5f,
                Mt.SuperBoss => 1.25f,
                Mt.HyperBoss => 10f,
                Mt.FinalBoss => 10f,
                Mt.Survivor => 1f,
                Mt.SmallDrone => 0.5f,
                Mt.MedDrone => 0.85f,
                Mt.LargeDrone => 1f,
                Mt.Other => 0.5f,
                _ => 0f,
            };

            private enum Mt
            {
                Invalid,
                Environment,
                Basic,
                BasicUp,
                Miniboss,
                MinibossUp,
                MinibossUpUp,
                Champion,
                EliteChamp,
                SuperBoss,
                HyperBoss,
                FinalBoss,
                Survivor,
                SmallDrone,
                MedDrone,
                LargeDrone,
                Other,
            }

            private static Mt GetMonsterTier(String name) => name?.ToLower() switch
            {
                "commando_body_name" => Mt.Survivor,
                "merc_body_name" => Mt.Survivor,
                "engi_body_name" => Mt.Survivor,
                "huntress_body_name" => Mt.Survivor,
                "mage_body_name" => Mt.Survivor,
                "toolbot_body_name" => Mt.Survivor,
                "treebot_body_name" => Mt.Survivor,
                "loader_body_name" => Mt.Survivor,
                "croco_body_name" => Mt.Survivor,
                "captain_body_name" => Mt.Survivor,
                "bandit_body_name" => Mt.Survivor,
                "bomber_body_name" => Mt.Survivor,
                "enforcer_body_name" => Mt.Survivor,
                "enforcer_name" => Mt.Survivor,
                "hand_body_name" => Mt.Survivor,
                "assassin_body_name" => Mt.Survivor,
                "paladin_body_name" => Mt.Survivor,
                "sniper_body_name" => Mt.Survivor,
                "wisp_survivor_body_name" => Mt.Survivor,
                "sniper_name" => Mt.Survivor,
                "aatrox_body_name" => Mt.Survivor,

                "drone_gunner_body_name" => Mt.SmallDrone,
                "drone_healing_body_name" => Mt.SmallDrone,
                "drone_backup_body_name" => Mt.SmallDrone,

                "drone_missile_body_name" => Mt.MedDrone,
                "equipmentdrone_body_name" => Mt.MedDrone,
                "flamedrone_body_name" => Mt.MedDrone,
                "emergencydrone_body_name" => Mt.MedDrone,
                "turret1_body_name" => Mt.MedDrone,
                "engiturret_body_name" => Mt.MedDrone,
                "squidturret_body_name" => Mt.MedDrone,

                "drone_mega_body_name" => Mt.LargeDrone,

                "beetle_body_name" => Mt.BasicUp,
                "wisp_body_name" => Mt.BasicUp,
                "lemurian_body_name" => Mt.BasicUp,
                "jellyfish_body_name" => Mt.BasicUp,
                "clay_body_name" => Mt.BasicUp,
                "hermit_crab_body_name" => Mt.BasicUp,
                "vulture_body_name" => Mt.BasicUp,
                "roboballmini_body_name" => Mt.BasicUp,       
                "imp_body_name" => Mt.BasicUp,
                "urchinturret_body_name" => Mt.BasicUp,

                "bell_body_name" => Mt.Miniboss,
                "minimushroom_body_name" => Mt.Miniboss,
                "beetleguard_body_name" => Mt.Miniboss,
                "bison_body_name" => Mt.Miniboss,
                "golem_body_name" => Mt.Miniboss,

                "greaterwisp_body_name" => Mt.MinibossUp,
                "lemurianbruiser_body_name" => Mt.MinibossUp, 
                "claybruiser_body_name" => Mt.MinibossUp,
                "parent_body_name" => Mt.MinibossUp,

                "nullifier_body_name" => Mt.MinibossUpUp,
                "lunargolem_body_name" => Mt.MinibossUpUp,
                "lunarwisp_body_name" => Mt.MinibossUpUp,
                "archwisp_body_name" => Mt.MinibossUpUp,

                "beetlequeen_body_name" => Mt.Champion,
                "clayboss_body_name" => Mt.Champion,
                "titan_body_name" => Mt.Champion,
                "vagrant_body_name" => Mt.Champion,
                "magmaworm_body_name" => Mt.Champion,
                "impboss_body_name" => Mt.Champion,
                "gravekeeper_body_name" => Mt.Champion,
                "roboballboss_body_name" => Mt.Champion,

                "scav_body_name" => Mt.EliteChamp,
                "electricworm_body_name" => Mt.EliteChamp,
                "ancient_wisp_body_name" => Mt.EliteChamp,       
                
                "titangold_body_name" => Mt.SuperBoss,
                "superroboballboss_body_name" => Mt.SuperBoss,
                "grandparent_body_name" => Mt.SuperBoss,
                "artifactshell_body_name" => Mt.SuperBoss,
                "scavlunar1_body_name" => Mt.SuperBoss,
                "scavlunar2_body_name" => Mt.SuperBoss,
                "scavlunar3_body_name" => Mt.SuperBoss,
                "scavlunar4_body_name" => Mt.SuperBoss,

                "shopkeeper_body_name" => Mt.HyperBoss,
                "first_wisp_body_name" => Mt.HyperBoss,

                "brother_body_name" => Mt.FinalBoss,

                "pod_body_name" => Mt.Environment,
                "pot2_body_name" => Mt.Environment,
                "maulingrock_body_name" => Mt.Environment,
                "timecrystal_body_name" => Mt.Environment,

                "potmobile_body_name" => Mt.Other,

                _ => Mt.Invalid,
            };
            

            public GameObject latestUtilZone
            {
                set => this.onUtilPlaced?.Invoke( value );
            }
            public event Action<GameObject> onUtilPlaced;
            public Single latestUtilRadius
            {
                set => this.onUtilRangeProvided?.Invoke( value );
            }
            public event Action<Single> onUtilRangeProvided;

            public Boolean isDoppleganger { get; private set; }

            public struct ChargeState
            {
                public Double chargeConsumed;
                public Double chargeLeft;
                public Double chargeAtStart;
                public Single chargeScaler;
            }

            private const Double decayRate = -0.1;
            private const Double zeroMark = 100f;
            private const Double regenPsPs = 2;
            private const Double decayMultWithBuff = 0.5;

            private Double charge;

            private BuffIndex buffInd;

            private CharacterBody body;

            public void Awake()
            {
                this.charge = zeroMark;
                this.body = this.GetComponent<CharacterBody>();
                this.buffInd = Main.RW_flameChargeBuff;
            }

            public void Start()
            {
                this.isDoppleganger = ( base.GetComponent<CharacterBody>()?.inventory?.GetItemCount( ItemIndex.InvadingDoppelganger ) ?? 1 ) != 0;
            }

            public void FixedUpdate()
            {
                Int32 buffStacks = this.body.GetBuffCount(this.buffInd);
                this.charge = UpdateCharge( this.charge, Time.fixedDeltaTime * (Single)( buffStacks > 0 ? decayMultWithBuff : 1.0 ) * ( this.isDoppleganger ? 0.25f : 1f ) );
                this.charge += regenPsPs * buffStacks * Time.fixedDeltaTime * ( this.isDoppleganger ? 4f : 1f );
            }

            public void AddCharge( Double addedCharge ) => this.charge += addedCharge;

            public ChargeState UseCharge( Double percent, Single scaler, Boolean floorCost = true )
            {
                ChargeState state = new ChargeState();

                Double startingCharge = this.charge;
                Double chargeToConsume;

                if( startingCharge > zeroMark || !floorCost )
                {
                    chargeToConsume = startingCharge * percent / 100.0;
                } else
                {
                    chargeToConsume = percent;
                }

                chargeToConsume = Math.Min( startingCharge, chargeToConsume );


                this.charge -= chargeToConsume;

                state.chargeAtStart = startingCharge;
                state.chargeConsumed = chargeToConsume;
                state.chargeLeft = this.charge;
                state.chargeScaler = GetChargeScaler( chargeToConsume, percent, scaler );

                return state;
            }

            public ChargeState UseChargeDrain( Double rate, Single time, Single scaler = 0f )
            {
                ChargeState state = new ChargeState();

                Double startingCharge = this.charge;
                Double chargeToConsume = startingCharge - (startingCharge * Math.Exp(-rate * time / 100));

                chargeToConsume = Math.Max( 0, chargeToConsume );

                this.charge -= chargeToConsume;
                state.chargeAtStart = startingCharge;
                state.chargeConsumed = chargeToConsume;
                state.chargeLeft = this.charge;
                state.chargeScaler = GetDrainScaler( chargeToConsume, ( rate * time ), scaler );

                return state;
            }

            public static Single GetDrainScaler( Double drained, Double ideal, Single scaler )
            {
                Single temp = (Single)(drained / ideal);
                temp -= 1f;
                temp *= scaler;
                temp += 1f;

                return temp;
            }

            public Double ReadCharge() => this.charge;

            private static Double UpdateCharge( Double startVal, Single dTime )
            {
                Double temp = startVal - zeroMark;
                temp *= Math.Exp( decayRate * dTime );
                temp += zeroMark;
                return temp;
            }

            private static Single GetChargeScaler( Double consumed, Double percent, Single scaler )
            {
                Single temp = (Single) (consumed / percent);
                temp -= 1f;
                temp *= scaler;
                temp += 1f;

                return temp;
            }
        }
    }
}
