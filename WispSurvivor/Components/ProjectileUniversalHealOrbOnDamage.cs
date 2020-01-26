#if ANCIENTWISP
using RoR2;
using RoR2.Orbs;
using RoR2.Projectile;
using System;
using UnityEngine;

namespace RogueWispPlugin
{

    internal partial class Main
    {
        [RequireComponent( typeof( ProjectileController) )]
        internal class ProjectileUniversalHealOrbOnDamage : MonoBehaviour, IOnDamageInflictedServerReceiver
        {
            public GameObject effectPrefab;
            public UniversalHealOrb.HealTarget healTarget;
            public UniversalHealOrb.HealType healType;
            public Single value;

            private ProjectileController projectileController;

            private void Awake()
            {
                this.projectileController = base.GetComponent<ProjectileController>();

            }

            public void OnDamageInflictedServer( DamageReport damageReport )
            {
                if( this.projectileController.owner )
                {
                    var hc = this.projectileController.owner.GetComponent<HealthComponent>();
                    if( hc )
                    {
                        OrbManager.instance.AddOrb( new UniversalHealOrb
                        (
                            hc.body.mainHurtBox,
                            this.effectPrefab,
                            damageReport.victimBody.transform.position,
                            this.value,
                            50f,
                            this.healType,
                            this.healTarget
                        ));
                    }
                }
            }
        }
    }

}
#endif