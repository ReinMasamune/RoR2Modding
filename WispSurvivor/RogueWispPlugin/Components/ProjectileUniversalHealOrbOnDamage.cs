#if ANCIENTWISP
using RoR2;
using RoR2.Orbs;
using RoR2.Projectile;
using System;
using UnityEngine;

namespace Rein.RogueWispPlugin
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
            public Boolean useSkin = false;
            

            private ProjectileController projectileController;
            private HurtBox ownerHB;
            private UInt32 skinInd;

            private void Awake()
            {
                this.projectileController = base.GetComponent<ProjectileController>();
            }

            private void Start()
            {
                var ownerBody = this.projectileController.owner.GetComponent<CharacterBody>();
                this.ownerHB = ownerBody.mainHurtBox;
                this.skinInd = ownerBody.skinIndex;
            }

            public void OnDamageInflictedServer( DamageReport damageReport )
            {
                if( this.projectileController.owner )
                {
                    if( this.ownerHB )
                    {
                        Main.LogI( this.skinInd );
                        OrbManager.instance.AddOrb( new UniversalHealOrb
                        (
                            this.ownerHB,
                            this.effectPrefab,
                            this.useSkin,
                            this.skinInd,
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