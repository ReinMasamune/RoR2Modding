using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BepInEx.Logging;
using ReinCore;
using RoR2;
using UnityEngine;

namespace Sniper.Expansions
{
    internal class ExpandableBulletAttack : BulletAttack
    {
        internal delegate void OnHitDelegate( ExpandableBulletAttack bullet, BulletHit hitInfo );
        internal event OnHitDelegate onHit;

        internal ExpandableBulletAttack() : base()
        {
            base.hitCallback = this.ExpandableHitCallback;
        }

        private Boolean ExpandableHitCallback( ref BulletHit hitInfo )
        {
            var result = base.DefaultHitCallback(ref hitInfo );
            if( result && this.onHit != null )
            {
                this.onHit( this, hitInfo );
            }
            return result;
        }
    }
}
