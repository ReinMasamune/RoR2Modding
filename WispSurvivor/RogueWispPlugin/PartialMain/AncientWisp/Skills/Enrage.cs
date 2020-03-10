#if ANCIENTWISP
using EntityStates;
using RogueWispPlugin.Helpers;
using RoR2;
using RoR2.Projectile;
using System;
using UnityEngine;
using System.Linq;

namespace RogueWispPlugin
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