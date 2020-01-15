using BepInEx;
using RoR2;
using UnityEngine;
using R2API;
using R2API.Utils;
using System.Reflection;
using EntityStates;
using RoR2.Skills;
using System;

namespace ReinSniperRework
{
    internal partial class Main
    {
        internal class SniperReload : BaseState
        {
            const Single attackSpeedSoft = 1.0f;
            const Single reloadInterval = 2f;
            internal const Single pFracStart = 0.25f;
            internal const Single pFracEnd = 0.4f;
            internal const Single gFracStart = 0.3f;
            internal const Single gFracEnd = 0.6f;
            const Single perfectStart = reloadInterval * pFracStart;
            const Single perfectEnd = reloadInterval * pFracEnd;
            const Single goodStart = reloadInterval * gFracStart;
            const Single goodEnd = reloadInterval * gFracEnd;

            internal Single reloadFrac
            {
                get
                {
                    return ( this.timerU + this.timerF ) / ( 2f * reloadInterval );
                }
            }

            private Single speed
            {
                get
                {
                    var speed = base.characterBody.attackSpeed;
                    if( speed < 1f )
                    {
                        return speed;
                    }
                    return Mathf.Min( speed, 1f + attackSpeedSoft * (1f - 1f / speed) );
                }
            }

            internal Boolean isActive { get; private set; }

            private Single timerU = 0f;
            private Single timerUL = 0f;
            private Single timerF = 0f;
            private Single timerFL = 0f;

            public override void OnEnter()
            {
                base.OnEnter();
                this.isActive = true;
                base.gameObject.GetComponent<SniperUIController>().SetReloading(this);
            }

            public override void Update()
            {
                base.Update();
                this.timerUL = this.timerU;
                this.timerU += Time.deltaTime * this.speed;

                if( this.timerU >= reloadInterval )
                {
                    this.timerU -= reloadInterval;
                }
            }

            public override void FixedUpdate()
            {
                base.FixedUpdate();
                this.timerFL = this.timerF;
                this.timerF += Time.fixedDeltaTime * this.speed;

                if( this.timerF >= reloadInterval )
                {
                    this.timerF -= reloadInterval;
                }
            }

            public override void OnExit()
            {
                base.OnExit();
                this.isActive = false;
            }

            public override InterruptPriority GetMinimumInterruptPriority()
            {
                base.outer.SetNextState( new SniperLoaded { quality = this.GetQuality() } );

                return InterruptPriority.Death;
            }

            private SniperShoot.ReloadQuality GetQuality()
            {
                var checkF = CheckTimer( this.timerF );
                if( checkF == SniperShoot.ReloadQuality.Perfect ) return checkF;

                var checkFL = CheckTimer( this.timerFL );
                if( checkFL == SniperShoot.ReloadQuality.Perfect ) return checkFL;

                var checkU = CheckTimer( this.timerU );
                if( checkU == SniperShoot.ReloadQuality.Perfect ) return checkU;

                var checkUL = CheckTimer( this.timerUL );
                if( checkUL == SniperShoot.ReloadQuality.Perfect ) return checkUL;

                var checkFav = SniperShoot.ReloadQuality.Normal;
                if( this.timerF > this.timerFL )
                {
                    checkFav = CheckTimer( (this.timerF + this.timerFL) / 2f );
                }
                if( checkFav == SniperShoot.ReloadQuality.Perfect ) return checkFav;

                var checkUav = SniperShoot.ReloadQuality.Normal;
                if( this.timerU > this.timerUL )
                {
                    checkUav = CheckTimer( (this.timerU + this.timerUL) / 2f);
                }
                if( checkUav == SniperShoot.ReloadQuality.Perfect ) return checkUav;

                if( checkF == SniperShoot.ReloadQuality.Good || checkFL == SniperShoot.ReloadQuality.Good || checkU == SniperShoot.ReloadQuality.Good || checkUL == SniperShoot.ReloadQuality.Good || checkFav == SniperShoot.ReloadQuality.Good || checkUav == SniperShoot.ReloadQuality.Good )
                {
                    return SniperShoot.ReloadQuality.Good;
                }
                return SniperShoot.ReloadQuality.Normal;
            }

            private static SniperShoot.ReloadQuality CheckTimer( Single timer )
            {
                var ret = SniperShoot.ReloadQuality.Normal;
                if( timer >= goodStart && timer <= goodEnd ) ret = SniperShoot.ReloadQuality.Good;
                if( timer >= perfectStart && timer <= perfectEnd ) ret = SniperShoot.ReloadQuality.Perfect;
                return ret;
            }
        }
    }
}


