using BepInEx;
using R2API.Utils;
using RoR2;
using RoR2.Networking;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using RogueWispPlugin.Helpers;
using RogueWispPlugin.Modules;
//using static RogueWispPlugin.Helpers.APIInterface;

namespace RogueWispPlugin
{
    internal partial class Main
    {
        partial void RW_LeechOrbEffects()
        {
            this.Load += this.RW_CreateLeechOrbEffects;
        }

        private void RW_CreateLeechOrbEffects()
        {
            GameObject baseFX = Resources.Load<GameObject>("Prefabs/Effects/OrbEffects/HauntOrbEffect");

            for( Int32 i = 0; i < 8; i++ )
            {
                utilityLeech[i] = CreateLeechOrb( baseFX, i );
            }
        }

        private static GameObject CreateLeechOrb( GameObject baseFX, Int32 skinIndex )
        {
            GameObject obj = baseFX.InstantiateClone("LeechEffect"+skinIndex.ToString(), false);

            obj.GetComponent<EffectComponent>().applyScale = true;

            MonoBehaviour.Destroy( obj.GetComponent<AkEvent>() );
            MonoBehaviour.Destroy( obj.GetComponent<AkGameObj>() );

            OrbHelper.ConvertOrbSettings( obj );

            obj.GetComponent<WispOrbEffect>().soundString = "Play_treeBot_m1_hit_heal";

            Transform vfx = obj.transform.Find("VFX");
            Transform core = vfx.Find("Core");

            ParticleSystemRenderer corePSR = core.GetComponent<ParticleSystemRenderer>();
            ParticleSystem corePS = core.GetComponent<ParticleSystem>();

            corePSR.material = fireMaterials[skinIndex][1];

            ParticleSystem.MainModule coreMain = corePS.main;
            coreMain.cullingMode = ParticleSystemCullingMode.AlwaysSimulate;

            return obj;
        }
    }

}
