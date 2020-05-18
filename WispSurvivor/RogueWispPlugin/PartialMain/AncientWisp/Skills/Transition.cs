#if ANCIENTWISP
using System;

using EntityStates;

using Rein.RogueWispPlugin.Helpers;

using RoR2;

using UnityEngine;
using UnityEngine.Networking;

namespace Rein.RogueWispPlugin
{

    internal partial class Main
    {
        internal class AWEnrageTransition : FlyState
        {
            const Single transitionDuration = 2f;
            internal const UInt32 enrageSkinIndex = 0b1001_0000_0000_0110_1011_0111_0011_1111u;
            const Single enrageFlareIntensity = 0f;
            const Single enrageEmissionRate = 1500f;
            const Single delayFrac = 0.15f;
            const Single animationStartFrac = 0.6f;
            const Single animationSoundPlayFrac = 0f;
            const Single animationParticleSurgeTimeFrac = 0.5f;

            private Single duration;
            private Single delayTime;
            private Single remainingTime;
            private Single animationStartTime;
            private Single animationDuration;
            private Single animationSoundPlayTime;
            private Single animationParticleSurgeTime;
            private Single initialParticleIntensity;

            private Boolean animationStarted = false;
            private Boolean surgeComplete = false;
            private Boolean soundComplete = false;

            private ParticleSystem bodyParticles;
            private WispModelBitSkinController skinController;
            private SpriteRenderer flareSprite;
            private EyeFlare eyeFlare;

            public override void OnEnter()
            {
                base.OnEnter();
                base.characterBody.AddBuff( BuffIndex.Immune );
                this.duration = transitionDuration;

                this.skinController = base.GetModelTransform().GetComponent<WispModelBitSkinController>();
                var model = this.skinController.GetComponent<CharacterModel>();
                var partHolder = model.GetComponent<ParticleHolder>();
                this.bodyParticles = partHolder.systems[0];
                this.flareSprite = base.gameObject.GetComponentInChildren<SpriteRenderer>();
                this.eyeFlare = this.flareSprite.GetComponent<EyeFlare>();


                var emis = this.bodyParticles.emission;
                this.initialParticleIntensity = emis.rateOverTime.constant;


                this.delayTime = this.duration * delayFrac;
                this.remainingTime = this.duration - this.delayTime;

                this.animationStartTime = this.duration * animationStartFrac;
                this.animationDuration = this.duration - this.animationStartTime;
                this.animationSoundPlayTime = animationSoundPlayFrac;
                this.animationParticleSurgeTime = this.animationStartTime + ( this.animationDuration * animationParticleSurgeTimeFrac );
            }

            public override void FixedUpdate()
            {
                base.inputBank.skill1.down = false;
                base.inputBank.skill2.down = false;
                base.inputBank.skill3.down = false;
                base.inputBank.skill4.down = false;

                base.inputBank.moveVector = Vector3.zero;

                base.FixedUpdate();
                var emis = this.bodyParticles.emission;
                if( base.fixedAge <= this.delayTime )
                {
                    emis.rateOverTime = Mathf.Lerp( this.initialParticleIntensity, 0f, ( base.fixedAge / this.delayTime ) );
                }

                if( base.fixedAge >= this.animationSoundPlayTime && !this.soundComplete )
                {
                    this.soundComplete = true;
                    for( Int32 i = 0; i < 3; ++i )
                    {
                        //Util.PlayScaledSound( "Play_gravekeeper_attack1_open", base.gameObject, 1.3f );
                        //Util.PlayScaledSound( "Play_gravekeeper_attack1_open", base.gameObject, 1.6f );
                        Util.PlayScaledSound( "Play_gravekeeper_attack2_charge", base.gameObject, 0.45f );
                        Util.PlayScaledSound( "Play_gravekeeper_attack2_charge", base.gameObject, 0.65f );
                    }
                }

                if( base.fixedAge >= this.animationStartTime )
                {
                    if( !this.animationStarted )
                    {
                        this.animationStarted = true;

                        base.PlayCrossfade( "Gesture", "Enrage", "Enrage.playbackRate", this.animationDuration, 0.2f );
                    }

                    if( base.fixedAge >= this.animationParticleSurgeTime && !this.surgeComplete )
                    {
                        this.surgeComplete = true;

                        this.skinController.Apply( WispBitSkin.GetWispSkin( enrageSkinIndex ) );
                        base.characterBody.skinIndex = enrageSkinIndex;
                        this.flareSprite.color = this.skinController.activeLightColor;
                        this.flareSprite.material = this.skinController.activeTracerMaterial;
                        this.eyeFlare.localScale = enrageFlareIntensity;
                        emis.rateOverTime = enrageEmissionRate;

                        if( NetworkServer.active )
                        {
                            base.characterBody.AddBuff( BuffIndex.EnrageAncientWisp );
                            base.characterBody.inventory?.GiveItem( ItemIndex.AlienHead, 1 );
                            base.characterBody.inventory?.GiveItem( ItemIndex.Syringe, 2 );
                        }
                    }


                }

                if( base.fixedAge >= this.duration && base.isAuthority )
                {
                    base.outer.SetNextState( new AWEnrageMainState() );
                }
            }

            public override void OnExit()
            {
                base.characterBody.RemoveBuff( BuffIndex.Immune );
                base.OnExit();
            }
        }
    }
}
#endif