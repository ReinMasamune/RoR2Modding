using System;

using RoR2;

using UnityEngine;

namespace Rein.RogueWispPlugin.Helpers
{
    internal class StartEndSound : MonoBehaviour
    {
        public String startSound;
        public String endSound;
        public Int32 mult = 1;

        public void OnEnable()
        {
            for( Int32 i = 0; i < this.mult; ++i ) Util.PlaySound( this.startSound, base.gameObject );

        }

        public void OnDisable()
        {
            for( Int32 i = 0; i < this.mult; ++i ) Util.PlaySound( this.endSound, base.gameObject );
        }
    }

}
