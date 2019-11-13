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
            public double chargeAtStart;
            public float chargeScaler;
        }

        private const double decayRate = -0.1;
        private const double zeroMark = 100f;
        private const double regenPsPs = 2.5;
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

        public ChargeState UseCharge(double percent, float scaler)
        {
            ChargeState state = new ChargeState();

            double startingCharge = charge;
            double chargeToConsume;

            if( startingCharge > zeroMark )
            {
                chargeToConsume = startingCharge * percent / 100.0;
            } else
            {
                chargeToConsume = percent;
            }

            chargeToConsume = Math.Min(startingCharge, chargeToConsume);


            charge -= chargeToConsume;

            state.chargeAtStart = startingCharge;
            state.chargeConsumed = chargeToConsume;
            state.chargeLeft = charge;
            state.chargeScaler = GetChargeScaler(chargeToConsume, percent, scaler);

            return state;
        }

        public ChargeState UseChargeDrain(double rate, float time)
        {
            ChargeState state = new ChargeState();

            double startingCharge = charge;
            double chargeToConsume = startingCharge - (startingCharge * Math.Exp(-rate * time / 100));
            chargeToConsume = Math.Max(0, chargeToConsume);

            charge -= chargeToConsume;
            state.chargeAtStart = startingCharge;
            state.chargeConsumed = chargeToConsume;
            state.chargeLeft = charge;
            state.chargeScaler = 0f;

            return state;
        }

        public static float GetDrainScaler(double drained, double ideal, float scaler)
        {
            float temp = (float)(drained / ideal);
            temp -= 1f;
            temp *= scaler;
            temp += 1f;

            return temp;
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

        private static float GetChargeScaler( double consumed, double percent, float scaler )
        {
            float temp = (float) (consumed / percent);
            temp -= 1f;
            temp *= scaler;
            temp += 1f;

            Debug.Log(temp);

            return temp;
        }
    }
}
