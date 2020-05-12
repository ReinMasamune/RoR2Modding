using System;

using RoR2;

using UnityEngine;
using UnityEngine.Events;

namespace Rein.RogueWispPlugin
{
    internal partial class Main
    {
        [RequireComponent( typeof( EffectComponent ) )]
        public class WispOrbEffect : MonoBehaviour
        {
            public Boolean faceMovement = true;
            public Boolean callArrivalIfTargetIsGone;
            public Boolean startEffectCopiesRotation;
            public Boolean endEffectCopiesRotation;

            public Single startEffectScale = 1f;
            public Single endEffectScale = 1f;

            public String soundString;

            public Vector3 startVelocity1;
            public Vector3 startVelocity2;
            public Vector3 endVelocity1;
            public Vector3 endVelocity2;

            public AnimationCurve movementCurve;
            public BezierCurveLine bezierCurveLine;
            public GameObject startEffect;
            public GameObject endEffect;
            public UnityEvent onArrival;


            private Single age;
            private Single duration;

            private Boolean noTarget = false;
            private Boolean playSound = false;

            private Vector3 startPosition;
            private Vector3 previousPosition;
            private Vector3 lastKnownTargetPosition;
            private Vector3 startVelocity;
            private Vector3 endVelocity;

            private Transform targetTransform;

            private void Start()
            {
                EffectComponent component = base.GetComponent<EffectComponent>();
                this.startPosition = component.effectData.origin;
                this.previousPosition = this.startPosition;
                GameObject gameObject = component.effectData.ResolveHurtBoxReference();
                this.targetTransform = ( gameObject ? gameObject.transform : null );
                this.duration = component.effectData.genericFloat;
                if( this.duration == 0f )
                {
                    //Debug.LogFormat( "Zero duration for effect \"{0}\"", new System.Object[]
                    //{
                    //base.gameObject.name
                    //} );
                    UnityEngine.Object.Destroy( base.gameObject );
                    return;
                }
                this.lastKnownTargetPosition = ( this.targetTransform ? this.targetTransform.position : component.effectData.start );
                this.noTarget = !this.targetTransform;
                if( component.effectData.genericBool )
                {
                    this.playSound = true;
                }
                if( this.startEffect )
                {
                    EffectData effectData = new EffectData
                    {
                        origin = base.transform.position,
                        scale = this.startEffectScale
                    };
                    if( this.startEffectCopiesRotation )
                    {
                        effectData.rotation = base.transform.rotation;
                    }
                    //EffectManager.instance.SpawnEffect( this.startEffect, effectData, false );
                }
                this.startVelocity.x = Mathf.Lerp( this.startVelocity1.x, this.startVelocity2.x, UnityEngine.Random.value );
                this.startVelocity.y = Mathf.Lerp( this.startVelocity1.y, this.startVelocity2.y, UnityEngine.Random.value );
                this.startVelocity.z = Mathf.Lerp( this.startVelocity1.z, this.startVelocity2.z, UnityEngine.Random.value );
                this.endVelocity.x = Mathf.Lerp( this.endVelocity1.x, this.endVelocity2.x, UnityEngine.Random.value );
                this.endVelocity.y = Mathf.Lerp( this.endVelocity1.y, this.endVelocity2.y, UnityEngine.Random.value );
                this.endVelocity.z = Mathf.Lerp( this.endVelocity1.z, this.endVelocity2.z, UnityEngine.Random.value );
                this.UpdateOrb( 0f );
            }

            private void Update() => this.UpdateOrb( Time.deltaTime );

            private void UpdateOrb( Single deltaTime )
            {
                if( this.targetTransform )
                {
                    this.lastKnownTargetPosition = this.targetTransform.position;
                }
                Single num = Mathf.Clamp01(this.age / this.duration);
                Single num2 = this.movementCurve.Evaluate(num);
                Vector3 vector = Vector3.Lerp(this.startPosition + this.startVelocity * num2, this.lastKnownTargetPosition + this.endVelocity * (1f - num2), num2);
                base.transform.position = vector;
                if( this.faceMovement && vector != this.previousPosition )
                {
                    base.transform.forward = vector - this.previousPosition;
                }
                this.UpdateBezier();
                if( num == 1f || ( this.callArrivalIfTargetIsGone && this.targetTransform == null && !this.noTarget ) )
                {
                    if( this.playSound ) RoR2.Util.PlaySound( this.soundString, base.gameObject );
                    this.onArrival.Invoke();
                    if( this.endEffect )
                    {
                        EffectData effectData = new EffectData
                        {
                            origin = base.transform.position,
                            scale = this.endEffectScale
                        };
                        if( this.endEffectCopiesRotation )
                        {
                            effectData.rotation = base.transform.rotation;
                        }
                        //EffectManager.SpawnEffect( this.endEffect, effectData, false );
                    }
                    UnityEngine.Object.Destroy( base.gameObject );
                }
                this.previousPosition = vector;
                this.age += deltaTime;
            }

            private void UpdateBezier()
            {
                if( this.bezierCurveLine )
                {
                    this.bezierCurveLine.p1 = this.startPosition;
                    this.bezierCurveLine.v0 = this.endVelocity;
                    this.bezierCurveLine.v1 = this.startVelocity;
                    this.bezierCurveLine.UpdateBezier( 0f );
                }
            }

            public void InstantiatePrefab( GameObject prefab ) => UnityEngine.Object.Instantiate<GameObject>( prefab, base.transform.position, base.transform.rotation );


            public void InstantiateEffect( GameObject prefab ) => EffectManager.SpawnEffect( prefab, new EffectData
            {
                origin = base.transform.position
            }, false );

            public void InstantiateEffectCopyRotation( GameObject prefab ) => EffectManager.SpawnEffect( prefab, new EffectData
            {
                origin = base.transform.position,
                rotation = base.transform.rotation
            }, false );

            public void InstantiateEffectOppositeFacing( GameObject prefab ) => EffectManager.SpawnEffect( prefab, new EffectData
            {
                origin = base.transform.position,
                rotation = RoR2.Util.QuaternionSafeLookRotation( -base.transform.forward )
            }, false );


            public void InstantiatePrefabOppositeFacing( GameObject prefab ) => UnityEngine.Object.Instantiate<GameObject>( prefab, base.transform.position, RoR2.Util.QuaternionSafeLookRotation( -base.transform.forward ) );
        }
    }
}