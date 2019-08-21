using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.Networking;
using ReinArtificerer;

namespace EntityStates.ReinArtificerer.Artificer.Weapon
{
    public class Special : BaseState
    {
        ReinDataLibrary data;
        ReinElementTracker elements;
        

        public override void OnEnter()
        {
            base.OnEnter();
            data = base.GetComponent<ReinDataLibrary>();

            elements = data.element;

            EntityState nextState = new BaseSpecial();

            ReinElementTracker.Element mainElem = elements.GetMainElement();
            switch (mainElem)
            {
                case ReinElementTracker.Element.fire:
                    Chat.AddMessage("Fire R");
                    nextState = new FireSpecial();
                    break;
                case ReinElementTracker.Element.ice:
                    Chat.AddMessage("Ice R");
                    nextState = new IceSpecial();
                    break;
                case ReinElementTracker.Element.lightning:
                    Chat.AddMessage("Lightning R");
                    nextState = new LightningSpecial();
                    break;
                case ReinElementTracker.Element.none:
                    Chat.AddMessage("Base R");
                    elements.AddElement(ReinElementTracker.Element.fire, 2);

                    break;
                default:
                    Chat.AddMessage("You fucking broke it... Good job moron");
                    break;
            }

            outer.SetNextState(nextState);
        }
    }
}
