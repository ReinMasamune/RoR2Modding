using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.Networking;
using ReinArtificerer;

namespace EntityStates.ReinArtificerer.Artificer.Weapon
{
    public class LightningSpecial : BaseState
    {
        ReinDataLibrary data;
        ReinElementTracker elements;
        ReinLightningBuffTracker lightning;

        public int fireLevel = 0;
        public int iceLevel = 0;
        public int lightningLevel = 0;


        public override void OnEnter()
        {
            base.OnEnter();
            data = base.GetComponent<ReinDataLibrary>();
            elements = data.element;
            lightning = data.lightning;
            Chat.AddMessage("Pretend this is the lightning special");

            elements.ResetElement(ReinElementTracker.Element.lightning);


            //Sound
            //Animation?

            lightning.AddCharged(5f, 1.0f);

            //Element stuff (Double all non lightning charge?)

            elements.AddElement(ReinElementTracker.Element.fire, 2 + fireLevel);
            elements.AddElement(ReinElementTracker.Element.ice, iceLevel);


            if( lightning.GetBuffed() )
            {
                LightningBlink blink = new LightningBlink();
                blink.castValue = data.r_blinkCastValue;
                data.bodyState.SetNextState(blink);
            }
            else
            {
                outer.SetNextStateToMain();
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}
