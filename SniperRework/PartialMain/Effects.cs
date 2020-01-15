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
        #region Primary effects
        private GameObject primaryTracer;
        private GameObject primaryChargedTracer;
        private GameObject primaryImpact;
        private GameObject primaryChargedImpact;
        #endregion

        private GameObject knifeSlash;
        private GameObject knifeBlink;

        partial void Effects()
        {
            this.Load += this.CreateTracerEffect;
            this.Load += this.CreateChargedTracerEffect;
            this.Load += this.CreatePrimaryImpactEffect;
            this.Load += this.CreatePrimaryChargedImpactEffect;
            this.FirstFrame += this.CreateKnifeSlashEffect;
            this.FirstFrame += this.CreateKnifeBlinkEffect;
        }

        private void CreateKnifeBlinkEffect()
        {
            this.knifeBlink = EntityStates.Huntress.BlinkState.blinkPrefab.InstantiateClone( "KinfeBink", false );


            EffectAPI.AddEffect( this.knifeBlink );
        }

        private void CreateKnifeSlashEffect()
        {
            this.knifeSlash = EntityStates.Merc.WhirlwindBase.swingEffectPrefab.InstantiateClone( "KnifeSlash", false );
            this.knifeSlash.transform.Find( "SwingTrail" ).localScale *= 5f;
            this.knifeSlash.transform.Find( "Distortion" ).localScale *= 5f;

            EffectAPI.AddEffect( this.knifeSlash );

        }

        private void CreatePrimaryChargedImpactEffect()
        {

        }

        private void CreatePrimaryImpactEffect()
        {
            this.primaryImpact = Resources.Load<GameObject>( "Prefabs/Effects/ImpactEffects/ImpactHuntress" ).InstantiateClone( "SniperPrimaryImpact", false);

            //EffectAPI.AddEffect( this.primaryImpact );
            
        }

        private void CreateChargedTracerEffect()
        {

        }

        private void CreateTracerEffect()
        {
            //this.primaryTracer = CreateGenericTracer( "SniperPrimaryTracer", false, false );

            //var line = this.primaryTracer.GetComponent<LineRenderer>();

            this.primaryTracer = Resources.Load<GameObject>( "Prefabs/Effects/Tracers/TracerHuntressSnipe" ).InstantiateClone( "SniperPrimaryTracer", false );

            var trace = this.primaryTracer.GetComponent<Tracer>();
            Destroy( trace.headTransform.gameObject );
            Destroy( trace.tailTransform.gameObject );

            //trace.onTailReachedDestination.RemoveAllListeners();

            var fx = this.primaryTracer.AddComponent<VFXAttributes>();
            fx.vfxIntensity = VFXAttributes.VFXIntensity.Low;
            fx.vfxPriority = VFXAttributes.VFXPriority.Always;

            EffectAPI.AddEffect( this.primaryTracer );
        }

        private static GameObject CreateGenericTracer( String name, Boolean doBeam, Boolean lineFromStart )
        {
            var obj = new GameObject( name ).InstantiateClone(name, false );

            var startTransform = new GameObject( "StartTransform" ).transform;
            startTransform.parent = obj.transform;
            startTransform.localPosition = Vector3.zero;
            startTransform.localRotation = Quaternion.identity;
            startTransform.localScale = Vector3.one;

            var headTransform = new GameObject( "HeadTransform" ).transform;
            headTransform.parent = obj.transform;
            headTransform.localPosition = Vector3.zero;
            headTransform.localRotation = Quaternion.identity;
            headTransform.localScale = Vector3.one;

            var tailTransform = new GameObject( "TailTransform" ).transform;
            tailTransform.parent = obj.transform;
            tailTransform.localPosition = Vector3.zero;
            tailTransform.localRotation = Quaternion.identity;
            tailTransform.localScale = Vector3.one;


            var effectComp = obj.AddComponent<EffectComponent>();
            effectComp.effectIndex = EffectIndex.Invalid;
            effectComp.positionAtReferencedTransform = false;
            effectComp.parentToReferencedTransform = false;
            effectComp.applyScale = false;
            effectComp.soundName = "";


            var tracerComp = obj.AddComponent<Tracer>();
            tracerComp.startTransform = startTransform;
            tracerComp.beamDensity = 10f;
            tracerComp.speed = 300f;
            tracerComp.headTransform = headTransform;
            tracerComp.tailTransform = tailTransform;
            tracerComp.length = 3f;
            tracerComp.reverse = false;

            if( doBeam )
            {
                var beamObject = new GameObject( "BeamObject" ).transform;
                beamObject.parent = obj.transform.parent;
                beamObject.localPosition = Vector3.zero;
                beamObject.localRotation = Quaternion.identity;
                beamObject.localScale = Vector3.one;

                beamObject.gameObject.AddComponent<ParticleSystem>();

                tracerComp.beamObject = beamObject.gameObject;
            }


            var lineRender = obj.AddComponent<LineRenderer>();

            var beamPoints = obj.AddComponent<BeamPointsFromTransforms>();
            var transforms = beamPoints.GetFieldValue<Transform[]>("pointTransforms");
            Array.Resize<Transform>( ref transforms, 2 );
            transforms[0] = (lineFromStart ? startTransform : tailTransform);
            transforms[1] = headTransform;

            var vfx = obj.AddComponent<VFXAttributes>();
            vfx.vfxPriority = VFXAttributes.VFXPriority.Always;
            vfx.vfxIntensity = VFXAttributes.VFXIntensity.Low;

            EffectAPI.AddEffect( obj );

            return obj;
        }


        private void LogComponents( Transform t, String space )
        {
            LogW( space + t.name );
            foreach( Component c in t.GetComponents<Component>() )
            {
                LogI( c.GetType() );
            }
            foreach( Transform t2 in t.transform )
            {
                LogI( "\n" );
                LogComponents( t2, space + " " );
            }
        }
    }
}


/*

TracerSmokeChase, TracerAncientWisp, TracerBanditPistol, TracerBanditShotgun
            var effectComp = this.primaryTracer.GetComponent<EffectComponent>();
            var tracerComp = this.primaryTracer.GetComponent<Tracer>();
            var lineComp = this.primaryTracer.GetComponent<LineRenderer>();
            var beamComp = this.primaryTracer.GetComponent<BeamPointsFromTransforms>();
            var events = this.primaryTracer.GetComponent<EventFunctions>();
            var vfxAtrib = this.primaryTracer.AddComponent<VFXAttributes>();
            vfxAtrib.vfxPriority = VFXAttributes.VFXPriority.Always;
            vfxAtrib.vfxIntensity = VFXAttributes.VFXIntensity.Low;
            EffectAPI.AddEffect( this.primaryTracer );








































    */