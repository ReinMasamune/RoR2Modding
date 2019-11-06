using UnityEngine;
using UnityEngine.Networking;
using System;
using RoR2;

namespace WispSurvivor.Components
{
    public class WispPassiveController : MonoBehaviour
    {
        public struct ChargeState
        {
            public double chargeConsumed;
            public double chargeLeft;
        }

        private const double decayRate = -0.1;
        private const double zeroMark = 100f;
        private const double regenPsPs = 0.5;
        private const double decayMultWithBuff = 0.25;

        private double charge;

        private BuffIndex buffInd;

        private CharacterBody body;

        public void Awake()
        {
            charge = zeroMark;
            body = GetComponent<CharacterBody>();
            buffInd = BuffCatalog.FindBuffIndex("WispFlameChargeBuff");
        }

        public void FixedUpdate()
        {
            int buffStacks = body.GetBuffCount(buffInd);            
            charge = UpdateCharge(charge, Time.fixedDeltaTime * (float)( buffStacks > 0 ? decayMultWithBuff : 1.0 ));
            charge += regenPsPs * buffStacks * Time.fixedDeltaTime;
        }

        public void AddCharge(double addedCharge )
        {
            charge += addedCharge;
        }

        public double ConsumeCharge( double consumedCharge )
        {
            double temp = charge;
            double temp2 = charge - consumedCharge;
            if( temp2 < 0 )
            {
                temp += temp2;
                temp2 = 0;
            }
            charge = temp2;
            return temp;
        }

        public ChargeState ConsumePercentCharge( double consumedPercent )
        {
            double chargeToConsume = charge * consumedPercent / 100.0;
            charge -= chargeToConsume;

            return new ChargeState
            {
                chargeConsumed = chargeToConsume,
                chargeLeft = charge
            };
        }

        public double DrainCharge(double drainedCharge)
        {
            if (drainedCharge < charge)
            {
                charge -= drainedCharge;
                return drainedCharge;
            } else
            {
                double temp = charge;
                charge = 0;
                return temp;
            }
        }

        public ChargeState DrainPercentCharge( double drainedPercent )
        {
            double chargeToDrain = charge * drainedPercent / 100.0;
            charge -= chargeToDrain;

            return new ChargeState
            {
                chargeConsumed = chargeToDrain,
                chargeLeft = charge
            };
        }

        public double ReadCharge()
        {
            return charge;
        }

        private static double UpdateCharge(double startVal, float dTime)
        {
            double temp = startVal - zeroMark;
            temp *= Math.Exp(decayRate * dTime);
            temp += zeroMark;
            return temp;
        }


    }
}
