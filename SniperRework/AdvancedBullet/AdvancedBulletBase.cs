using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ReinSniperRework.Components
{
    internal partial class Main
    {
        internal abstract class AdvancedBulletBase
        {
            internal abstract void OnBegin();
            internal abstract void OnEnd();
            internal abstract void Tick( Single time );




            protected internal enum DamageMode
            {
                WorldHit = 0,
                EnemyHit = 1,
                SplashHit = 2
            }

            protected internal void SendDamage(DamageMode mode, DamageInfo damage )
            {
                switch( mode )
                {
                    //default:
                        
                }
            }


            protected internal void EndBullet()
            {

            }
        }
    }
}
