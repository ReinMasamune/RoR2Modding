using RoR2;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace WispSurvivor.Modules
{
    public static class WispMaterialModule
    {
        public static Gradient[] fireGradients = new Gradient[8];
        public static Color[] fireColors = new Color[8];
        public static Texture2D[] fireTextures = new Texture2D[8];
        public static Material[][] fireMaterials = new Material[8][];
        public static Material[][] otherMaterials = new Material[8][];
        public static Shader effectShader;

        public static void DoModule( GameObject body , Dictionary<Type,Component> dic)
        {
            GenerateGradients();
            GenerateTextures();
            GenerateMaterials(dic);
        }

        private static void GenerateGradients()
        {
            GradientAlphaKey[][] aKeys = new GradientAlphaKey[8][];
            GradientColorKey[][] cKeys = new GradientColorKey[8][];

            //Ancient wisp generic
            aKeys[0] = new GradientAlphaKey[3];
            aKeys[0][0] = new GradientAlphaKey(0f, 1f);
            aKeys[0][1] = new GradientAlphaKey(0f, 0.5f);
            aKeys[0][2] = new GradientAlphaKey(0.7f, 0f);

            cKeys[0] = new GradientColorKey[2];
            cKeys[0][0] = new GradientColorKey(new Color(0f, 0f, 0f), 1f);
            cKeys[0][1] = new GradientColorKey(new Color(0.8f, 0.300f, 0.9f), 0f);

            fireColors[0] = new Color(0.8f, 0.3f, 0.9f);

            //Lesser wisp
            aKeys[1] = new GradientAlphaKey[3];
            aKeys[1][0] = new GradientAlphaKey(0f, 1f);
            aKeys[1][1] = new GradientAlphaKey(0f, 0.5f);
            aKeys[1][2] = new GradientAlphaKey(0.7f, 0f);

            cKeys[1] = new GradientColorKey[2];
            cKeys[1][0] = new GradientColorKey(new Color(0f, 0f, 0f), 1f);
            cKeys[1][1] = new GradientColorKey(new Color(0.906f, 0.420f, 0.235f), 0f);

            fireColors[1] = new Color(0.906f, 0.420f, 0.235f, 1.0f);

            //Greater wisp
            aKeys[2] = new GradientAlphaKey[3];
            aKeys[2][0] = new GradientAlphaKey(0f, 1f);
            aKeys[2][1] = new GradientAlphaKey(0f, 0.5f);
            aKeys[2][2] = new GradientAlphaKey(0.7f, 0f);

            cKeys[2] = new GradientColorKey[2];
            cKeys[2][0] = new GradientColorKey(new Color(0f, 0f, 0f), 1f);
            cKeys[2][1] = new GradientColorKey(new Color(0.400f, 0.769f, 0.192f), 0f);

            fireColors[2] = new Color(0.400f, 0.769f, 0.192f, 1.0f);

            //Archaic wisp
            aKeys[3] = new GradientAlphaKey[3];
            aKeys[3][0] = new GradientAlphaKey(0f, 1f);
            aKeys[3][1] = new GradientAlphaKey(0f, 0.75f);
            aKeys[3][2] = new GradientAlphaKey(0.7f, 0f);

            cKeys[3] = new GradientColorKey[2];
            cKeys[3][0] = new GradientColorKey(new Color(0f, 0f, 0f), 1f);
            cKeys[3][1] = new GradientColorKey(new Color(1f, 0.590f, 0.806f), 0f);

            fireColors[3] = new Color(1f, 0.590f, 0.806f, 1.0f);

            //Lunar wisp
            aKeys[4] = new GradientAlphaKey[3];
            aKeys[4][0] = new GradientAlphaKey(0f, 1f);
            aKeys[4][1] = new GradientAlphaKey(0f, 0.5f);
            aKeys[4][2] = new GradientAlphaKey(0.7f, 0f);

            cKeys[4] = new GradientColorKey[7];
            cKeys[4][0] = new GradientColorKey(new Color(0f, 0f, 0.05f), 1f);
            cKeys[4][1] = new GradientColorKey(new Color(0f, 0f, 0.1f), 0.75f);
            cKeys[4][2] = new GradientColorKey(new Color(0.1f, 0.11f, 0.6f), 0.7f);
            cKeys[4][3] = new GradientColorKey(new Color(0.19f, 0.19f, 0.85f), 0.63f);
            cKeys[4][4] = new GradientColorKey(new Color(0.4f, 0.55f, 1.0f), 0.32f);
            cKeys[4][5] = new GradientColorKey(new Color(0.48f, 0.69f, 1.0f), 0.25f);
            cKeys[4][6] = new GradientColorKey(new Color(0.5f, 0.75f, 1.0f), 0f);

            fireColors[4] = new Color(0.5f, 0.75f, 1f, 1.0f);

            //Solar wisp
            aKeys[5] = new GradientAlphaKey[3];
            aKeys[5][0] = new GradientAlphaKey(0f, 1f);
            aKeys[5][1] = new GradientAlphaKey(0f, 0.5f);
            aKeys[5][2] = new GradientAlphaKey(0.7f, 0f);

            cKeys[5] = new GradientColorKey[7];
            cKeys[5][0] = new GradientColorKey(new Color(0f, 0f, 0f), 1f);
            cKeys[5][1] = new GradientColorKey(new Color(0f, 0f, 0f), 0.75f);
            cKeys[5][2] = new GradientColorKey(new Color(0.3f, 0.1f, 0.05f), 0.7f);
            cKeys[5][3] = new GradientColorKey(new Color(0.5f, 0.3f, 0.05f), 0.63f);
            cKeys[5][4] = new GradientColorKey(new Color(0.9f, 0.7f, 0.1f), 0.32f);
            cKeys[5][5] = new GradientColorKey(new Color(0.9f, 0.8f, 0.4f), 0.25f);
            cKeys[5][6] = new GradientColorKey(new Color(0.9f, 0.95f, 0.8f), 0f);

            fireColors[5] = new Color(0.95f, 0.95f, 0.05f, 1f);

            //Iridescent wisp
            aKeys[6] = new GradientAlphaKey[3];
            aKeys[6][0] = new GradientAlphaKey(0f, 1f);
            aKeys[6][1] = new GradientAlphaKey(0f, 0.5f);
            aKeys[6][2] = new GradientAlphaKey(0.7f, 0f);

            cKeys[6] = new GradientColorKey[8];
            cKeys[6][0] = new GradientColorKey(new Color(0f, 0f, 0f), 1f);
            cKeys[6][1] = new GradientColorKey(new Color(0f, 0f, 0f), 0.75f);
            cKeys[6][2] = new GradientColorKey(new Color(1f, 1f, 0f), 0.55f);
            cKeys[6][3] = new GradientColorKey(new Color(1f, 0f, 0f), 0.5f);
            cKeys[6][4] = new GradientColorKey(new Color(1f, 0f, 1f), 0.45f);
            cKeys[6][5] = new GradientColorKey(new Color(0f, 0f, 1f), 0.4f);
            cKeys[6][6] = new GradientColorKey(new Color(0f, 1f, 1f), 0.2f);
            cKeys[6][7] = new GradientColorKey(new Color(0f, 1f, 0f), 0f);

            fireColors[6] = new Color(1f, 1f, 1f, 1f);

            //Ascended wisp
            aKeys[7] = new GradientAlphaKey[3];
            aKeys[7][0] = new GradientAlphaKey(0f, 1f);
            aKeys[7][1] = new GradientAlphaKey(0f, 0.75f);
            aKeys[7][2] = new GradientAlphaKey(0.7f, 0f);

            cKeys[7] = new GradientColorKey[1];
            cKeys[7][0] = new GradientColorKey(new Color(1f, 1f, 1f), 0f);

            fireColors[7] = new Color(1f, 1f, 1f, 1f);


            for( int i = 0; i < 8; i++ )
            {
                fireGradients[i] = new Gradient
                {
                    alphaKeys = aKeys[i],
                    colorKeys = cKeys[i],
                    mode = GradientMode.Blend
                };
            }
        }

        private static void GenerateTextures()
        {
            for( int i = 0; i < 8; i++ )
            {
                fireTextures[i] = CreateNewRampTex(fireGradients[i]);
            }
        }

        private static void GenerateMaterials( Dictionary<Type,Component> dic )
        {
            Material[] baseMats = GetBaseMaterials(dic);

            for( int i = 0; i < 8; i++ )
            {
                fireMaterials[i] = new Material[baseMats.Length];
            }

            Material tempMat;
            for( int i = 0; i < baseMats.Length; i++ )
            {
                tempMat = baseMats[i];

                for( int j = 0; j < 8; j++ )
                {
                    fireMaterials[j][i] = MonoBehaviour.Instantiate<Material>(tempMat);
                }
            }

            for( int i = 0; i < 8; i++ )
            {
                fireMaterials[i][0].SetTexture("_RemapTex", fireTextures[i]);
                fireMaterials[i][0].SetFloat("_AlphaBias", 0f);
                fireMaterials[i][0].SetFloat("_Boost", 3.7f);
                fireMaterials[i][0].SetFloat("_DstBlend", 10f);
                fireMaterials[i][0].SetFloat("_InvFade", 2f);
                fireMaterials[i][0].SetFloat("_SrcBlend", 5f);

                fireMaterials[i][1].SetTexture("_RemapTex", fireTextures[i]);
                fireMaterials[i][1].SetFloat("_AlphaBias", 0f);
                fireMaterials[i][1].SetFloat("_Boost", 7f);
                fireMaterials[i][1].SetFloat("_DstBlend", 10f);
                fireMaterials[i][1].SetFloat("_InvFade", 2f);
                fireMaterials[i][1].SetFloat("_SrcBlend", 5f);

                fireMaterials[i][5].SetTexture("_RemapTex", fireTextures[i]);
                fireMaterials[i][5].SetFloat("_AlphaBias", 0f);
                fireMaterials[i][5].SetFloat("_Boost", 2.7f);
                fireMaterials[i][5].SetFloat("_DstBlend", 10f);
                fireMaterials[i][5].SetFloat("_InvFade", 2f);
                fireMaterials[i][5].SetFloat("_SrcBlend", 5f);

                fireMaterials[i][6].SetTexture("_RemapTex", fireTextures[i]);
                fireMaterials[i][6].SetFloat("_AlphaBias", 0f);
                fireMaterials[i][6].SetFloat("_Boost", 4.5f);
                fireMaterials[i][6].SetFloat("_DstBlend", 10f);
                fireMaterials[i][6].SetFloat("_InvFade", 2f);
                fireMaterials[i][6].SetFloat("_SrcBlend", 5f);

                fireMaterials[i][7].SetTexture("_RemapTex", fireTextures[i]);
                fireMaterials[i][7].SetFloat("_AlphaBias", 0f);
                fireMaterials[i][7].SetFloat("_Boost", 4.5f);
                fireMaterials[i][7].SetFloat("_DstBlend", 10f);
                fireMaterials[i][7].SetFloat("_InvFade", 2f);
                fireMaterials[i][7].SetFloat("_SrcBlend", 5f);

                fireMaterials[i][8].SetTexture("_RemapTex", fireTextures[i]);
                fireMaterials[i][8].SetFloat("_AlphaBias", 0f);
                fireMaterials[i][8].SetFloat("_Boost", 4.5f);
                fireMaterials[i][8].SetFloat("_DstBlend", 10f);
                fireMaterials[i][8].SetFloat("_InvFade", 2f);
                fireMaterials[i][8].SetFloat("_SrcBlend", 5f);

                fireMaterials[i][9].SetTexture("_RemapTex", fireTextures[i]);
                fireMaterials[i][9].SetFloat("_AlphaBias", 0f);
                fireMaterials[i][9].SetFloat("_Boost", 4.5f);
                fireMaterials[i][9].SetFloat("_DstBlend", 10f);
                fireMaterials[i][9].SetFloat("_InvFade", 2f);
                fireMaterials[i][9].SetFloat("_SrcBlend", 5f);

                fireMaterials[i][10].SetTexture("_RemapTex", fireTextures[i]);
                fireMaterials[i][10].SetFloat("_AlphaBias", 0f);
                fireMaterials[i][10].SetFloat("_Boost", 4.5f);
                fireMaterials[i][10].SetFloat("_DstBlend", 10f);
                fireMaterials[i][10].SetFloat("_InvFade", 2f);
                fireMaterials[i][10].SetFloat("_SrcBlend", 5f);
            }
        }

        private static Material[] GetBaseMaterials(Dictionary<Type,Component> dic )
        {
            Material[] mats = new Material[11];

            //matAncientWispFire
            mats[0] = dic.C<ModelLocator>().modelTransform.GetComponent<CharacterModel>().baseParticleSystemInfos[0].renderer.material;

            Transform sourceObj1 = Resources.Load<GameObject>("Prefabs/Effects/ImpactEffects/AncientWispExplosion").transform.Find("Particles");
            Transform sourceObj2 = Resources.Load<GameObject>("Prefabs/Effects/HelfireIgniteEffect").transform;
            Transform sourceObj3 = Resources.Load<GameObject>("Prefabs/Effects/BeamSphereExplosion").transform.Find("InitialBurst");
            //matGenericFire
            mats[1] = sourceObj1.Find("Sparks").GetComponent<ParticleSystemRenderer>().material;
            //matCutWispLarge
            mats[2] = sourceObj1.Find("Flames").GetComponent<ParticleSystemRenderer>().material;
            //matCutShockwave
            mats[3] = sourceObj1.Find("Ring").GetComponent<ParticleSystemRenderer>().material;
            //matWispEnrage
            mats[4] = Resources.Load<GameObject>("Prefabs/Effects/AncientWispEnrage").transform.Find("SwingTrail").GetComponent<ParticleSystemRenderer>().material;
            //matAncientWilloWispPillar?
            mats[5] = Resources.Load<GameObject>("Prefabs/Effects/ImpactEffects/AncientWispPillar").transform.Find("Particles").Find("Flames, Tube, CenterHuge").GetComponent<ParticleSystemRenderer>().material;
            //matHelfirePuff
            mats[6] = sourceObj2.Find("Puff").GetComponent<ParticleSystemRenderer>().material;
            //matHelfireIgniteEffectFlare
            mats[7] = sourceObj2.Find("Flare").GetComponent<ParticleSystemRenderer>().material;
            //matBeamSphereBeam
            mats[8] = sourceObj3.Find("Ring").GetComponent<ParticleSystemRenderer>().material;
            //matBeamSphereCenter
            mats[9] = sourceObj3.Find("Flames").GetComponent<ParticleSystemRenderer>().material;
            //matBeamSphereLightning
            mats[10] = sourceObj3.Find("Lightning").GetComponent<ParticleSystemRenderer>().material;

            effectShader = mats[1].shader;

            return mats;
        }

        private static T C<T>( this Dictionary<Type,Component> dic ) where T : Component
        {
            return dic[typeof(T)] as T;
        }

        private static Texture2D CreateNewRampTex(Gradient grad)
        {
            Texture2D tex = new Texture2D(256, 16, TextureFormat.RGBA32, false);

            Color tempC;
            Color[] tempCs = new Color[16];

            for (int i = 0; i < 256; i++)
            {
                tempC = grad.Evaluate(i / 255f);
                for (int j = 0; j < 16; j++)
                {
                    tempCs[j] = tempC;
                }

                tex.SetPixels(255 - i, 0, 1, 16, tempCs);
            }
            tex.wrapMode = TextureWrapMode.Clamp;
            tex.Apply();
            return tex;
        }

        private static void DebugMaterialInfo(Material m)
        {
            Debug.Log("Material name: " + m.name);
            string[] s = m.shaderKeywords;
            Debug.Log("Shader keywords");
            for (int i = 0; i < s.Length; i++)
            {
                Debug.Log(s[i]);
            }

            Debug.Log("Shader name: " + m.shader.name);

            Debug.Log("Texture Properties");
            string[] s2 = m.GetTexturePropertyNames();
            for (int i = 0; i < s2.Length; i++)
            {
                Debug.Log(s2[i] + " : " + m.GetTexture(s2[i]));
            }
        }
    }
}