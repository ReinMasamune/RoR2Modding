using RoR2;
using System;
using UnityEngine;

namespace RogueWispPlugin
{
    internal partial class Main
    {
        public class WispPassiveController : MonoBehaviour
        {
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

            public void FixedUpdate()
            {
                Int32 buffStacks = this.body.GetBuffCount(this.buffInd);
                this.charge = UpdateCharge( this.charge, Time.fixedDeltaTime * (Single)(buffStacks > 0 ? decayMultWithBuff : 1.0) );
                this.charge += regenPsPs * buffStacks * Time.fixedDeltaTime;
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
                state.chargeScaler = GetDrainScaler( chargeToConsume, (rate * time), scaler );

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
