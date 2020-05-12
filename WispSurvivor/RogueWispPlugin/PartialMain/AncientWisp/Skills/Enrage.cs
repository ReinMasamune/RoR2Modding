#if ANCIENTWISP
using EntityStates;

using UnityEngine;

namespace Rein.RogueWispPlugin
{

    internal partial class Main
    {
        internal class AWEnrageMainState : FlyState
        {

            private SpriteRenderer flareSprite;

            public override void OnEnter()
            {
                base.OnEnter();
                this.flareSprite = base.gameObject.GetComponentInChildren<SpriteRenderer>();
            }

            public override void OnExit()
            {
                this.flareSprite.gameObject.SetActive( false );
                base.OnExit();
            }

        }
    }
}
#endif