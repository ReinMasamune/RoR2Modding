//#if ROGUEWISP
//using RoR2;
//using System;
//using UnityEngine;
//using ReinCore;
////using static RogueWispPlugin.Helpers.APIInterface;

//namespace Rein.RogueWispPlugin
//{
//    internal partial class Main
//    {
//        partial void RW_IgnitionOrbEffects() => this.Load += this.RW_CreateIgnitionOrbEffects;

//        private void RW_CreateIgnitionOrbEffects()
//        {
//            GameObject baseFX = Resources.Load<GameObject>("Prefabs/Effects/HelfireIgniteEffect");

//            for( Int32 i = 0; i < 8; i++ )
//            {
//                //utilityBurns[i] = CreateIgniteEffect( baseFX, i );
//            }
//        }

//        private static GameObject CreateIgniteEffect( GameObject baseFX, Int32 skinIndex )
//        {
//            GameObject obj = baseFX.ClonePrefab("IgniteEffect"+skinIndex.ToString(), false);
//            MonoBehaviour.Destroy( obj.GetComponent<DestroyOnTimer>() );
//            obj.AddComponent<WispIgnitionEffectController>();
//            //obj.transform.Find("Point Light").GetComponent<Light>().color = WispMaterialModule.fireColors[skinIndex];
//            MonoBehaviour.Destroy( obj.transform.Find( "Point Light" ).gameObject );
//            Transform flareObj = obj.transform.Find("Flare");
//            Transform puffObj = obj.transform.Find("Puff");
//            ParticleSystem flarePS = flareObj.GetComponent<ParticleSystem>();
//            ParticleSystemRenderer flarePSR = flareObj.GetComponent<ParticleSystemRenderer>();
//            flarePSR.material = fireMaterials[skinIndex][7];

//            ParticleSystem puffPS = puffObj.GetComponent<ParticleSystem>();
//            ParticleSystemRenderer puffPSR = puffObj.GetComponent<ParticleSystemRenderer>();

//            puffPSR.material = fireMaterials[skinIndex][1];

//            ParticleSystem.MainModule puffMain = puffPS.main;
//            puffMain.loop = true;
//            puffMain.duration = 0.05f;
//            puffMain.simulationSpace = ParticleSystemSimulationSpace.World;
//            puffMain.startSize = new ParticleSystem.MinMaxCurve
//            {
//                mode = ParticleSystemCurveMode.TwoConstants,
//                constantMin = 1f,
//                constantMax = 2f
//            };

//            ParticleSystem.EmissionModule puffEmis = puffPS.emission;
//            puffEmis.SetBursts( Array.Empty<ParticleSystem.Burst>() );
//            puffEmis.rateOverTime = new ParticleSystem.MinMaxCurve
//            {
//                mode = ParticleSystemCurveMode.Constant,
//                constant = 5f
//            };

//            return obj;
//        }
//    }
//#endif
//}
