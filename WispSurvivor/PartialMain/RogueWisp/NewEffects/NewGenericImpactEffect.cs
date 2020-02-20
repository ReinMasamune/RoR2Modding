#if ROGUEWISP
using R2API;
using System;
using UnityEngine;
//using static RogueWispPlugin.Helpers.APIInterface;

namespace RogueWispPlugin
{

    internal partial class Main
    {
        /*
        partial void RW_GenericImpactEffects() => this.Load += this.RW_CreateGenericImpactEffects;

        private void RW_CreateGenericImpactEffects()
        {
            GameObject[] bases = new GameObject[1];
            bases[0] = Resources.Load<GameObject>( "Prefabs/Effects/ImpactEffects/ImpactWispEmber" );

            for( Int32 i = 0; i < 8; i++ )
            {
                genericImpactEffects[i] = CreateImpFx( bases, i );
            }
        }

        private static GameObject[] CreateImpFx( GameObject[] bases, Int32 skinIndex )
        {
            //Material mat = WispMaterialModule.fireMaterials[skinIndex][0];
            //Color col = WispMaterialModule.fireColors[skinIndex];

            GameObject[] effects = new GameObject[bases.Length];

            effects[0] = CreateImpFx00( bases[0], skinIndex );

            return effects;
        }

        private static GameObject CreateImpFx00( GameObject baseObj, Int32 skinIndex )
        {
            const Int32 matIndex = 0;
            GameObject obj = baseObj.InstantiateClone("WispImpact0-" + skinIndex.ToString() , false);


            GameObject flameObj = obj.transform.Find("Flames").gameObject;
            GameObject flashObj = obj.transform.Find("Flash").gameObject;

            ParticleSystem flamePS = flameObj.GetComponent<ParticleSystem>();
            ParticleSystemRenderer flamePSR = flameObj.GetComponent<ParticleSystemRenderer>();

            ParticleSystem flashPS = flashObj.GetComponent<ParticleSystem>();
            ParticleSystemRenderer flashPSR = flashObj.GetComponent<ParticleSystemRenderer>();


            ParticleSystem.ColorOverLifetimeModule flameCOL = flamePS.colorOverLifetime;
            ParticleSystem.MinMaxGradient flameColMMGrad = new ParticleSystem.MinMaxGradient();
            flameColMMGrad.mode = ParticleSystemGradientMode.Gradient;
            flameColMMGrad.gradient = fireGradients[skinIndex];
            flameCOL.color = flameColMMGrad;
            flamePSR.material = fireMaterials[skinIndex][matIndex];

            //Gradient flameColGrad = new Gradient();
            //GradientColorKey[] flameColCols = new GradientColorKey[3];
            //flameColCols[0] = new GradientColorKey(new Color(5f, 5f, 5f), 0f);
            //flameColCols[1] = new GradientColorKey(new Color(1f, 1f, 1f), 0.1f);
            //flameColCols[2] = new GradientColorKey(new Color(0.6f, 0.6f, 0.6f), 1f);
            //flameColGrad.SetKeys(flameColCols, flameCOL.color.gradient.alphaKeys);
            //flameColMMGrad.gradient = flameColGrad;


            ParticleSystem.ColorOverLifetimeModule flashCOL = flashPS.colorOverLifetime;
            ParticleSystem.MinMaxGradient flashColMMGrad = new ParticleSystem.MinMaxGradient();
            flashColMMGrad.mode = ParticleSystemGradientMode.Gradient;
            flashColMMGrad.gradient = fireGradients[skinIndex];
            flashCOL.color = flashColMMGrad;
            flashPSR.material = fireMaterials[skinIndex][matIndex];

            //Gradient flashColGrad = new Gradient();
            //GradientColorKey[] flashColCols = new GradientColorKey[3];
            //flashColCols[0] = new GradientColorKey(new Color(5f, 5f, 5f), 0f);
            //flashColCols[1] = new GradientColorKey(new Color(1f, 1f, 1f), 0.1f);
            //flashColCols[2] = new GradientColorKey(new Color(0.6f, 0.6f, 0.6f), 1f);
            //flashColGrad.SetKeys(flashColCols, flashCOL.color.gradient.alphaKeys);
            return obj;
        }
        */
    }

}
#endif