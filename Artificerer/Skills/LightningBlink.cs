using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.Networking;
using ReinArtificerer;

namespace EntityStates.ReinArtificerer.Artificer.Weapon
{
    public class LightningBlink : BaseState
    {
        ReinDataLibrary data;
        ReinElementTracker elements;
        ReinLightningBuffTracker lightning;

        public float castValue;

        private float intensity;
        private float duration;

        private float stopwatch = 0f;

        private Vector3 blinkDirection;

        private Transform modelTransform;
        private CharacterModel model;
        private HurtBoxGroup boxGroup;

        public override void OnEnter()
        {
            base.OnEnter();
            data = base.GetComponent<ReinDataLibrary>();
            elements = data.element;
            lightning = data.lightning;
        
            intensity = lightning.GetIntensity();
            Chat.AddMessage("Wheee");

            duration = data.b_baseDuration / intensity;
            Debug.Log(duration);
            Debug.Log(intensity);
            Debug.Log(castValue);
            modelTransform = base.GetModelTransform();
            if( modelTransform )
            {
                model = modelTransform.GetComponent<CharacterModel>();
                boxGroup = modelTransform.GetComponent<HurtBoxGroup>();
            }
            if( model )
            {
                model.invisibilityCount++;
            }
            if( boxGroup )
            {
                boxGroup.hurtBoxesDeactivatorCounter += 1;
            }

            blinkDirection = base.inputBank.moveVector;
            CreateBlinkEffect(Util.GetCorePosition(base.gameObject));
            base.characterMotor.Motor.SafeMovement = true;

            outer.SetNextStateToMain();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            stopwatch += Time.fixedDeltaTime;
            if( base.characterMotor && base.characterDirection )
            {
                base.characterMotor.velocity.y = 0;
                base.characterMotor.rootMotion += blinkDirection * (moveSpeedStat * data.b_speedCoef * castValue * Time.fixedDeltaTime / duration);
            }
            if( stopwatch >= duration && base.isAuthority )
            {
                outer.SetNextStateToMain();
            }
        }

        public override void OnExit()
        {
            if( !outer.destroying )
            {
                //Play sound
                CreateBlinkEffect(Util.GetCorePosition(base.gameObject));
                modelTransform = base.GetModelTransform();
                /*
                if( modelTransform )
                {
                    TemporaryOverlay blinkOverlay1 = modelTransform.gameObject.AddComponent<TemporaryOverlay>();
                    blinkOverlay1.duration = 0.6f;
                    blinkOverlay1.animateShaderAlpha = true;
                    blinkOverlay1.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
                    blinkOverlay1.destroyComponentOnEnd = true;
                    blinkOverlay1.originalMaterial = data.b_mat1;
                    blinkOverlay1.AddToCharacerModel(modelTransform.GetComponent<CharacterModel>());

                    TemporaryOverlay blinkOverlay2 = modelTransform.gameObject.AddComponent<TemporaryOverlay>();
                    blinkOverlay2.duration = 0.7f;
                    blinkOverlay2.animateShaderAlpha = true;
                    blinkOverlay2.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
                    blinkOverlay2.destroyComponentOnEnd = true;
                    blinkOverlay2.originalMaterial = data.b_mat2;
                    blinkOverlay2.AddToCharacerModel(modelTransform.GetComponent<CharacterModel>());
                }
                */
            }
            if( model )
            {
                model.invisibilityCount--;
            }
            if( boxGroup )
            {
                boxGroup.hurtBoxesDeactivatorCounter -= 1;
            }
            base.characterMotor.Motor.SafeMovement = false;
            base.OnExit();
        }

        private void CreateBlinkEffect( Vector3 orig )
        {
            /*
            EffectData effect = new EffectData();
            effect.rotation = Util.QuaternionSafeLookRotation(blinkDirection);
            effect.origin = orig;
            EffectManager.instance.SpawnEffect(data.b_prefab, effect, false);
            */
        }
    }
}
