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
        partial void RW_UtilityAimEffects()
        {
            this.Load += this.RW_CreateUtilityAimEffects;
            this.FirstFrame += this.RW_EditUtilityAimEffects;
        }

        private void RW_EditUtilityAimEffects()
        {
            Material mat1 = MonoBehaviour.Instantiate<Material>(EntityStates.GolemMonster.ChargeLaser.laserPrefab.GetComponent<LineRenderer>().material);
            Material mat2 = MonoBehaviour.Instantiate<Material>(EntityStates.Huntress.ArrowRain.areaIndicatorPrefab.transform.Find("Expander").Find("Sphere").GetComponent<MeshRenderer>().materials[0]);

            for( Int32 i = 0; i < 8; i++ )
            {
                otherMaterials[i] = new Material[2]
                {
                    MonoBehaviour.Instantiate<Material>(mat1),
                    MonoBehaviour.Instantiate<Material>(mat2)
                };

                otherMaterials[i][0].SetTexture( "_RemapTex", fireTextures[i] );
                otherMaterials[i][1].SetTexture( "_RemapTex", fireTextures[i] );

                utilityAim[i].GetComponent<LineRenderer>().material = otherMaterials[i][0];
                utilityAim[i].transform.Find( "lineEnd" ).GetComponent<MeshRenderer>().material = otherMaterials[i][1];
                utilityFlames[i].transform.Find( "rangeInd" ).GetComponent<MeshRenderer>().material = otherMaterials[i][1];
            }
        }

        private void RW_CreateUtilityAimEffects()
        {
            GameObject baseFX = new GameObject("Temp", new Type[2]
            {
                typeof(LineRenderer),
                typeof(WispAimLineController),
            });

            for( Int32 i = 0; i < 8; i++ )
            {
                utilityAim[i] = CreateUtilityAim( baseFX, i );
            }

            MonoBehaviour.Destroy( baseFX );
        }

        private static GameObject CreateUtilityAim( GameObject baseFX, Int32 skinIndex )
        {
            GameObject obj = baseFX.InstantiateClone("WispUtilityAimEffect" + skinIndex.ToString() , false);

            LineRenderer lr = obj.GetComponent<LineRenderer>();
            GameObject g = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            MonoBehaviour.Destroy( g.GetComponent<SphereCollider>() );
            g.name = "lineEnd";
            g.transform.parent = obj.transform;
            g.transform.localPosition = Vector3.zero;
            g.transform.localRotation = Quaternion.identity;
            g.transform.localScale = Vector3.one;

            lr.alignment = LineAlignment.View;
            lr.colorGradient = fireGradients[skinIndex];
            lr.generateLightingData = false;
            lr.positionCount = 2;
            lr.startWidth = 0.1f;
            lr.endWidth = 0.1f;
            lr.useWorldSpace = true;


            return obj;
        }
    }

}
