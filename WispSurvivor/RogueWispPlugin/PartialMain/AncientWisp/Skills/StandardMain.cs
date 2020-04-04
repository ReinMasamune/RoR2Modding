#if ANCIENTWISP
using EntityStates;
using Rein.RogueWispPlugin.Helpers;
using RoR2;
using RoR2.Projectile;
using System;
using UnityEngine;
using System.Linq;

namespace Rein.RogueWispPlugin
{

    internal partial class Main
    {
        internal class AWDefaultMain : FlyState
        {
            const Single enrageStartFrac = 0.5f;

            const UInt32 baseSkin = 0b0001_0000_0000_0010_1011_0111_0011_1111u;

            const Single baseFlareIntensity = 0f;
            const Single baseEmissionRate = 750f;
            


            private WispModelBitSkinController skinController;
            private ParticleSystem bodyParticles;
            private SpriteRenderer flareSprite;
            private EyeFlare eyeFlare;

            private EntityStateMachine weaponStateMachine;


            private Boolean readyToSwitch = false;

            public override void OnEnter()
            {
                base.OnEnter();

                this.skinController = base.GetModelTransform().GetComponent<WispModelBitSkinController>();
                var model = this.skinController.GetComponent<CharacterModel>();
                this.bodyParticles = model.GetComponent<ParticleHolder>().systems[0];
                this.flareSprite = base.gameObject.GetComponentInChildren<SpriteRenderer>();
                this.eyeFlare = this.flareSprite.GetComponent<EyeFlare>();

                this.eyeFlare.localScale = baseFlareIntensity;
                var emis = this.bodyParticles.emission;
                emis.rateOverTime = baseEmissionRate;

                base.characterBody.skinIndex = baseSkin;

                this.skinController.Apply( WispBitSkin.GetWispSkin( baseSkin ) );

                this.flareSprite.color = this.skinController.activeLightColor;
                this.flareSprite.material = this.skinController.activeTracerMaterial;


                this.weaponStateMachine = base.outer.GetComponents<EntityStateMachine>().Where<EntityStateMachine>( ( esm ) => esm.customName == "Weapon" ).Single<EntityStateMachine>();
            }

            public override void FixedUpdate()
            {
                if( this.readyToSwitch )
                {
                    base.inputBank.skill1.down = false;
                    base.inputBank.skill2.down = false;
                    base.inputBank.skill3.down = false;
                    base.inputBank.skill4.down = false;

                    base.inputBank.moveVector = Vector3.zero;

                    if( this.weaponStateMachine.state is Idle )
                    {
                        base.outer.SetNextState( new AWEnrageTransition() );
                    }
                }
                base.FixedUpdate();
                if( !this.readyToSwitch && base.isAuthority && base.healthComponent.combinedHealthFraction <= enrageStartFrac )
                {
                    this.readyToSwitch = true;
                    //this.weaponStateMachine.SetNextStateToMain();

                }
            }
        }
    }
}
#endif