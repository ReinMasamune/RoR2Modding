namespace Rein.Sniper.Effects
{
    using System;

    using JetBrains.Annotations;

    using Rein.Sniper.Components;
    using Rein.Sniper.Modules;

    using ReinCore;

    using RoR2;
    using RoR2.Orbs;

    using UnityEngine;

    internal static partial class EffectCreator
    {
        const Single rth = 0.4f;



        internal static GameObject CreateSporeOrbEffect()
        {
            GameObject obj = Resources.Load<GameObject>("Prefabs/NetworkedObjects/CaptainSupplyDrops/CaptainHealingWard").transform.Find("Indicator").gameObject.ClonePrefab("SporeOrbEffect", false);
            var mult = ConfigModule.sporeParticleMultiplier;


            var indSphere = obj.transform.Find("IndicatorSphere");
            indSphere.gameObject.SetActive(ConfigModule.showSporeRangeIndicator);
            var renderer = indSphere.GetComponent<Renderer>();
            var indSphereMat = new IntersectionCloudMaterial(renderer.material.Instantiate());
            renderer.material = indSphereMat.material;
            indSphereMat.softFactor = 3.3f;
            indSphereMat.softPower = 1f;
            indSphereMat.brightnessBoost = 1.15f;
            indSphereMat.rimPower = 10f;
            indSphereMat.rimStrength = 2f;
            indSphereMat.alphaBoost = 2f;
            indSphereMat.intersectionStrength = 2f;
            indSphereMat.cull = MaterialBase.CullMode.Off;
            indSphereMat.externalAlpha = 1f;
            indSphereMat.sourceBlend = UnityEngine.Rendering.BlendMode.SrcColor;
            indSphereMat.destinationBlend = UnityEngine.Rendering.BlendMode.OneMinusSrcColor;
            indSphereMat.cutoffScrollSpeed = new(0f, 0f, 5f, 3f);

           
                


            var indRing = obj.transform.Find("IndicatorRing");
            indRing.gameObject.Destroy();



            var efc = obj.AddComponent<EffectComponent>();
            efc.applyScale = false;
            efc.effectIndex = EffectIndex.Invalid;
            efc.positionAtReferencedTransform = false;
            efc.parentToReferencedTransform = false;
            efc.disregardZScale = false;
            efc.soundName = "";

            var vfxAtr = obj.AddComponent<VFXAttributes>();
            vfxAtr.vfxPriority = VFXAttributes.VFXPriority.Always;
            vfxAtr.vfxIntensity = VFXAttributes.VFXIntensity.Low;

            var mgr = obj.AddComponent<SporeEffectManager>();
            mgr.indicatorMaterial = indSphereMat;

            var flashes = obj.transform.Find("Flashes");
            var flashPs = flashes.GetComponent<ParticleSystem>();
            var flashPsr = flashes.GetComponent<ParticleSystemRenderer>();
            var flashShape = flashPs.shape;
            flashShape.shapeType = ParticleSystemShapeType.Sphere;
            flashShape.radiusThickness = rth;
            var flashMain = flashPs.main;
            flashMain.maxParticles = 10000;
            flashMain.startLifetime = 0.5f;
            var flashEmis = flashPs.emission;
            flashEmis.rateOverTime = 0.6f * mult;


            var symbols = obj.transform.Find("HealingSymbols");
            var symPs = symbols.GetComponent<ParticleSystem>();
            var symPsr = symbols.GetComponent<ParticleSystemRenderer>();
            var symShape = symPs.shape;
            symShape.shapeType = ParticleSystemShapeType.Sphere;
            symShape.radiusThickness = rth;
            var symMain = symPs.main;
            symMain.maxParticles = 10000;
            symMain.startLifetime = 1f;
            var symEmis = symPs.emission;
            symEmis.rateOverTime = 0.6f * mult;
            
            var spores = UnityEngine.Object.Instantiate(symbols.gameObject, symbols.parent);
            var sporePs = spores.GetComponent<ParticleSystem>();
            var sporePsr = spores.GetComponent<ParticleSystemRenderer>();
            sporePsr.material = sporePsr.trailMaterial;
            sporePsr.trailMaterial = null;
            var sporeMain = sporePs.main;
            sporeMain.startLifetime = 2f;
            var sporeEm = sporePs.emission;
            sporeEm.rateOverTime = 3f * mult;

            var vec = new Vector3(1f - rth, 1f - rth, 1f - rth);


            var flashFull = UnityEngine.Object.Instantiate(flashes.gameObject, flashes.parent);
            var flashFps = flashFull.GetComponent<ParticleSystem>();
            var flashFEmis = flashFps.emission;
            flashFEmis.rateOverTime = 0.15f * mult;
            var flashFShape = flashFps.shape;
            flashFShape.scale = vec;
            flashFShape.radiusThickness = 1f;

            var symFull = UnityEngine.Object.Instantiate(symbols.gameObject, symbols.parent);
            var symFps = symFull.GetComponent<ParticleSystem>();
            var symFEmis = symFps.emission;
            symFEmis.rateOverTime = 0.15f * mult;
            var symFShape = symFps.shape;
            symFShape.scale = vec;
            symFShape.radiusThickness = 1f;

            var spoFull = UnityEngine.Object.Instantiate(spores.gameObject, spores.transform.parent);
            var spoFps = spoFull.GetComponent<ParticleSystem>();
            var spoFEmis = spoFps.emission;
            spoFEmis.rateOverTime = 0.25f * mult;
            var spoFShape = spoFps.shape;
            spoFShape.scale = vec;
            spoFShape.radiusThickness = 1f;




            foreach(var v in obj.GetComponentsInChildren<IRuntimePrefabComponent>())
            {
                v.InitializePrefab();
            }
            return obj;
        }
    }
}
