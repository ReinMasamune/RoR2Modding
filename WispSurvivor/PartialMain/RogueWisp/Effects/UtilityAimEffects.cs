using R2API;
using System;
using UnityEngine;
//using static RogueWispPlugin.Helpers.APIInterface;

namespace RogueWispPlugin
{
#if ROGUEWISP
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
            //Material mat2 = MonoBehaviour.Instantiate<Material>(EntityStates.Huntress.ArrowRain.areaIndicatorPrefab.transform.Find("Expander").Find("Sphere").GetComponent<MeshRenderer>().materials[0]);
            //Material mat2 = MonoBehaviour.Instantiate<Material>(EntityStates.NullifierMonster.DeathState.deathPreExplosionVFX.transform.Find("Expander").Find("Sphere").GetComponent<MeshRenderer>().materials[0]);
            Material mat2 = null;
            Material mat3 = null;
            Boolean mat2Found = false;

            foreach( Transform t in EntityStates.NullifierMonster.DeathState.deathPreExplosionVFX.transform )
            {
                if( t.gameObject.name == "AreaIndicator" )
                {
                    if( mat2Found )
                    {
                        mat3 = t.GetComponent<ParticleSystemRenderer>().material;
                    } else
                    {
                        mat2Found = true;
                        mat2 = t.GetComponent<ParticleSystemRenderer>().material;
                    }
                }
            }

            RoR2Plugin.Main.MiscHelpers.DebugMaterialInfo( mat1 );
            RoR2Plugin.Main.MiscHelpers.DebugMaterialInfo( mat2 );
            RoR2Plugin.Main.MiscHelpers.DebugMaterialInfo( mat3 );

            for( Int32 i = 0; i < 8; i++ )
            {
                otherMaterials[i] = new Material[]
                {
                    MonoBehaviour.Instantiate<Material>(mat1),
                    MonoBehaviour.Instantiate<Material>(mat2),
                    MonoBehaviour.Instantiate<Material>(mat3)
                };

                otherMaterials[i][0].SetTexture( "_RemapTex", fireTextures[i] );
                otherMaterials[i][1].SetTexture( "_RemapTex", fireTextures[i] );
                otherMaterials[i][2].SetTexture( "_RemapTex", fireTextures[i] );

                utilityAim[i].GetComponent<LineRenderer>().material = otherMaterials[i][0];
                utilityAim[i].transform.Find( "lineEnd" ).GetComponent<MeshRenderer>().material = otherMaterials[i][1];
                utilityAim[i].transform.Find( "lineEnd" ).Find( "lineEndSph2" ).GetComponent<MeshRenderer>().material = otherMaterials[i][2];
                utilityFlames[i].transform.Find( "rangeInd" ).GetComponent<MeshRenderer>().material = otherMaterials[i][1];
                utilityFlames[i].transform.Find( "rangeInd2" ).GetComponent<MeshRenderer>().material = otherMaterials[i][2];
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
            MonoBehaviour.Destroy( g.GetComponent<Rigidbody>() );
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

            GameObject g2 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            MonoBehaviour.Destroy( g2.GetComponent<SphereCollider>() );
            MonoBehaviour.Destroy( g2.GetComponent<Rigidbody>() );
            g2.name = "lineEndSph2";
            g2.transform.parent = g.transform;
            g2.transform.localPosition = Vector3.zero;
            g2.transform.localRotation = Quaternion.identity;
            g2.transform.localScale = Vector3.one;

            return obj;
        }
    }
#endif
}
