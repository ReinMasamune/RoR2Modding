﻿namespace AlternativeArtificer
{
    using R2API;
    using RoR2;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using AlternativeArtificer.Helpers;
    using Unity.Jobs;
    using Unity.Collections;
    using System.Diagnostics;

    public partial class Main
    {
        //public static Func<Color,Single,Single,Color> remapFunc;
        private Texture2D mainTex;
        private HashSet<SkinDef> addedSkins = new HashSet<SkinDef>();

        private Mesh artiDefaultMesh;
        private Mesh artiChangedMesh;

        private void EditModel()
        {
            Transform model = artiBody.GetComponent<ModelLocator>().modelTransform;

            #region Remove jets
            var display = Resources.Load<GameObject>("Prefabs/CharacterDisplays/MageDisplay").transform.Find("mdlMage").GetComponent<CharacterModel>().baseRendererInfos;
            display[0].renderer.gameObject.SetActive( false );
            display[1].renderer.gameObject.SetActive( false );
            #endregion

            #region Edit Mesh
            SkinnedMeshRenderer meshRenderer = model.Find("MageMesh").GetComponent<SkinnedMeshRenderer>();
            this.artiDefaultMesh = meshRenderer.sharedMesh;
            this.artiChangedMesh = Instantiate<Mesh>(this.artiDefaultMesh);

            Int32[] tris = this.artiChangedMesh.triangles;
            Int32 size = 1902;
            Int32 start1 = 4916;
            Int32 start2 = 10054;
            SortedSet<Int32> inds = new SortedSet<Int32>();
            for( Int32 i = 0; i < size; i++ )
            {
                Int32 indOff1 = (i + start1) * 3;
                inds.Add( tris[indOff1] );
                inds.Add( tris[indOff1 + 1] );
                inds.Add( tris[indOff1 + 2] );
                Int32 indOff2 = (i + start2) * 3;
                inds.Add( tris[indOff2] );
                inds.Add( tris[indOff2 + 1] );
                inds.Add( tris[indOff2 + 2] );
            }
            var boneWL = this.artiChangedMesh.boneWeights.ToList();
            var colorL = this.artiChangedMesh.colors.ToList();
            var color32L = this.artiChangedMesh.colors32.ToList();
            var normalL = this.artiChangedMesh.normals.ToList();
            var tanL = this.artiChangedMesh.tangents.ToList();
            var uv1L = this.artiChangedMesh.uv.ToList();
            var vertL = this.artiChangedMesh.vertices.ToList();
            var trisL = this.artiChangedMesh.triangles.ToList();
            foreach( Int32 i in inds.Reverse() )
            {
                if( boneWL.Count >= i ) boneWL.RemoveAt( i );
                if( colorL.Count >= i ) colorL.RemoveAt( i );
                if( color32L.Count >= i ) color32L.RemoveAt( i );
                if( normalL.Count >= i ) normalL.RemoveAt( i );
                if( tanL.Count >= i ) tanL.RemoveAt( i );
                if( uv1L.Count >= i ) uv1L.RemoveAt( i );
                if( vertL.Count >= i ) vertL.RemoveAt( i );
                Int32 offset = 0;
                for( Int32 j = 0; j < trisL.Count + offset; j += 3 )
                {
                    var ind1 = trisL[j - offset];
                    var ind2 = trisL[j + 1 - offset];
                    var ind3 = trisL[j + 2 - offset];
                    if( ind1 == i || ind2 == i || ind3 == i )
                    {
                        trisL.RemoveRange( j - offset, 3 );
                        offset += 3;
                        continue;
                    }
                    if( ind1 > i ) trisL[j - offset] -= 1;
                    if( ind2 > i ) trisL[j + 1 - offset] -= 1;
                    if( ind3 > i ) trisL[j + 2 - offset] -= 1;
                }
            }
            this.artiChangedMesh.triangles = trisL.ToArray();
            this.artiChangedMesh.vertices = vertL.ToArray();
            this.artiChangedMesh.boneWeights = boneWL.ToArray();
            this.artiChangedMesh.colors = colorL.ToArray();
            this.artiChangedMesh.colors32 = color32L.ToArray();
            this.artiChangedMesh.normals = normalL.ToArray();
            this.artiChangedMesh.tangents = tanL.ToArray();
            this.artiChangedMesh.uv = uv1L.ToArray();
            #endregion

            #region Fix Skirt
            model.gameObject.AddComponent<Components.SkirtFix>();
            Resources.Load<GameObject>( "Prefabs/CharacterDisplays/MageDisplay" ).transform.Find( "mdlMage" ).gameObject.AddComponent<Components.SkirtFix>();
            Resources.Load<GameObject>( "Prefabs/NetworkedObjects/LockedMage" ).transform.Find( "ModelBase" ).Find( "mdlMage" ).gameObject.AddComponent<Components.SkirtFix>();
            #endregion

            #region Add Rotator
            model.Find( "MageArmature" ).gameObject.AddComponent<Components.Rotator>();
            #endregion

            model.GetComponent<CharacterModel>().itemDisplayRuleSet = Resources.Load<GameObject>( "Prefabs/CharacterBodies/AncientWispBody" ).GetComponent<ModelLocator>().modelTransform.GetComponent<CharacterModel>().itemDisplayRuleSet;

            #region Skins
            var skins = model.GetComponent<ModelSkinController>();
            var baseSkin = skins.skins[0];
            var masterySkin = skins.skins[1];

            var baseMaterial = baseSkin.rendererInfos[9].defaultMaterial;
            var newMaterial = Instantiate<Material>(baseMaterial);

            var masteryRIs = masterySkin.rendererInfos;
            var rendererInfos = new CharacterModel.RendererInfo[2];

            rendererInfos[0] = masteryRIs[0];
            rendererInfos[1] = masteryRIs[1];

            var texReturn = CreateArtiSkinTex();
            this.mainTex = texReturn[0];


            newMaterial.SetTexture( "_MainTex", this.mainTex );
            newMaterial.SetTexture( "_EmTex", texReturn[1] );


            newMaterial.EnableKeyword( "_EMISSION" );
            //newMaterial.EnableKeyword( "FLOWMAP" );


            //newMaterial.SetInt( "_DitherOn", 1 );
            newMaterial.SetInt( "_PrintDirection", 1 );
            //newMaterial.SetInt( "_RampInfo", 4 );


            newMaterial.SetFloat( "_EmPower", 5f );
            newMaterial.SetFloat( "_PrintBias", 0.32f );
            newMaterial.SetFloat( "_PrintBoost", 3.85f );
            //newMaterial.SetFloat( "_SliceHeight", 0.6f );
            //newMaterial.SetFloat( "_Smoothness", 0.923f );
            //newMaterial.SetFloat( "_SpecularStrength", 0.818f );


            newMaterial.SetColor( "_EmColor", new Color( 1f, 1f, 1f, 1f ) );
            //newMaterial.SetColor( "_Color", new Color( 1f, 1f, 1f, 1f ) );


            rendererInfos[0].defaultMaterial = newMaterial;
            rendererInfos[1].defaultMaterial = newMaterial;
            var spriteTex = baseSkin.icon.texture;

            SkinAPI.SkinDefInfo newSkinInfo = new SkinAPI.SkinDefInfo
            {
                baseSkins = masterySkin.baseSkins,
                icon = CreateSkinIcon( new Color( 1f, 0f, 0f ), new Color( 0f, 1f, 0f ), new Color( 0f, 0f, 1f ), new Color( 0f, 0f, 0f ), new Color( 0.9f, 0.9f, 0.9f ) ),
                name = "NAME",
                nameToken = "REIN_ALTARTI_ALTMASTERYSKIN_NAME",
                rootObject = baseSkin.rootObject,
                unlockableName = masterySkin.unlockableName,
                rendererInfos = rendererInfos
            };

            SkinDef newSkin = SkinAPI.CreateNewSkinDef( newSkinInfo );

            var skinsArray = skins.skins;
            Array.Resize<SkinDef>( ref skinsArray, skinsArray.Length + 1 );
            skinsArray[skinsArray.Length - 1] = newSkin;
            skins.skins = skinsArray;

            addedSkins.Add( newSkin );

            #endregion

            // TODO: Fix+Enable IDRS
            // ATG
            // Infusion
            // Crowbar
            // Cautious Slug
            // Fuel cell
            // Tougher Times
            // Wings
            // Dio (consumed?)
            // Warbanner
            // Blast Shower
            // Harvester Scythe
            // Fireworks
            // Ghors Tome
            // Shattering Justice
        }

        private static RootColor Rebase( RootColor input, RootColor modifier, Single lengthMult )
        {
            /*
            var alpha = input.a;
            var inputLength = Mathf.Sqrt(input.r*input.r + input.g*input.g + input.b*input.b);
            var modifierLength = Mathf.Sqrt( modifier.r*modifier.r + modifier.g*modifier.g + modifier.b*modifier.b);

            var temp = modifier * inputLength * lengthMult / ( modifierLength != 0f ? modifierLength : 1f );
            temp.a = alpha;
            return temp;
            */

            return input.clone.Rebase( modifier ) * lengthMult;
        }

        private static Texture2D GetReadableTextureCopy( Texture2D baseTex )
        {
            var temp = RenderTexture.active;

            var newRT = RenderTexture.GetTemporary( baseTex.width, baseTex.height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear );

            Graphics.Blit( baseTex, newRT );

            RenderTexture.active = newRT;
            var newTex = new Texture2D( baseTex.width, baseTex.height, TextureFormat.RGBA32, false );
            newTex.ReadPixels( new Rect( 0, 0, newRT.width, newRT.height ), 0, 0 );
            newTex.Apply();
            RenderTexture.active = temp;

            return newTex;
        }


        private static Texture2D RemapTextureColors( Texture2D baseTex, Boolean thread )
        {
            Texture2D tex = GetReadableTextureCopy( baseTex );

            var watch = new Stopwatch();
            watch.Start();

            if( thread )
            {
                var job = new TextureColorJob
                {
                    width = tex.width,
                    height = tex.height,
                    texture = tex.GetRawTextureData<Color32>(),
                    def = new Color( 0f, 0f, 0f ),
                    metalBlue = new Color( 0.0225f, 0.0225f, 0.0225f ),
                    color1 = new Color( 0.81f, 0.81f, 0.81f ),
                    color2 = new Color( 0f, 0f, 1f ),
                    color3 = new Color( 0.04f, 0.04f, 0.04f )

                };
                var jobHandle = job.Schedule(tex.width * tex.height, 1);
                watch.Stop();
                UnityEngine.Debug.LogWarning( "Schedule " + watch.ElapsedMilliseconds );
                watch.Start();

                tex.wrapMode = baseTex.wrapMode;

                jobHandle.Complete();
                watch.Stop();
                UnityEngine.Debug.LogWarning( "Complete " + watch.ElapsedMilliseconds );
                watch.Start();
            } else
            {

            }
            tex.Apply();

            watch.Stop();
            UnityEngine.Debug.LogWarning( "Total " + watch.ElapsedMilliseconds );
            return tex;
        }
        public struct TextureColorJob : IJobParallelFor
        {
            [ReadOnly]
            public Int32 width;
            [ReadOnly]
            public Int32 height;

            [ReadOnly]
            public Color def;
            [ReadOnly]
            public Color metalBlue;
            [ReadOnly]
            public Color color1;
            [ReadOnly]
            public Color color2;
            [ReadOnly]
            public Color color3;

            [ReadOnly]
            public NativeArray<Color32> texture;

            public void Execute( Int32 index )
            {
                Color rColor = this.texture[index];
                rColor.r *= rColor.r;
                rColor.g *= rColor.g;
                rColor.b *= rColor.b;
                var outColor = this.def;
                var intensity = ( rColor.r + rColor.g + rColor.b ) / 3;
                var i = Mathf.Pow( intensity, 0.2f );
                Single a = rColor.a;

                Single tr = 0f;
                Single tg = 0f;
                Single tb = 0f;

                Single max = Mathf.Max( rColor.r , rColor.g , rColor.b );

                if( rColor.r == max ) tr += 1f;
                if( rColor.g == max ) tg += 1f;
                if( rColor.b == max ) tb += 1f;

                if( i <= 0.35f )
                {
                    //Foot bottom
                    if( tr == 0f && tg == 1f && tb == 0f )
                    {
                        outColor = Rebase( rColor, this.metalBlue, intensity * 10f );
                        goto End;
                    }
                    //Patch on back of gauntlets
                    if( tr == 1f && tg == 1f && tb == 0f )
                    {
                        outColor = Rebase( rColor, this.metalBlue, intensity * 10f );
                        goto End;
                    }
                    //Upper arms
                    if( tr == 0f && tg == 0f && tb == 1f )
                    {
                        outColor = Rebase( rColor, this.color1, intensity * 100000f );
                        goto End;
                    }

                } else if( i >= 0.65f )
                {
                    if( tr == 1f && tg == 0f && tb == 0f )
                    {
                        outColor = Rebase( rColor, this.color2, intensity );
                        goto End;
                    }
                    //Boots, Gauntlets, Cape
                    if( tr == 0f && tg == 0f && tb == 1f )
                    {
                        outColor = Rebase( rColor, this.color3, intensity * 0.1f );
                        goto End;
                    }
                    //Head, Cape inside, Gauntlet plate, Chestplate
                    if( tr == 1f && tg == 1f && tb == 1f )
                    {
                        outColor = Rebase( rColor, this.metalBlue, intensity * 0.075f );
                        goto End;
                    }
                } else
                {
                    //Lower arms and legs
                    if( tr == 1f && tg == 0f && tb == 0f )
                    {
                        outColor = Rebase( rColor, this.color1, intensity * 1000f );
                        goto End;
                    }
                    //Various straps
                    if( tr == 0f && tg == 1f && tb == 0f )
                    {
                        outColor = Rebase( rColor, this.color3, intensity );
                        goto End;
                    }
                    //Boot trim
                    if( tr == 0f && tg == 0f && tb == 1f )
                    {
                        outColor = Rebase( rColor, this.metalBlue, intensity * 10f );
                        goto End;
                    }
                }

                End:
                outColor.r = Mathf.Sqrt( outColor.r );
                outColor.g = Mathf.Sqrt( outColor.g );
                outColor.b = Mathf.Sqrt( outColor.b );
                outColor.a = a;
                this.texture[index] = outColor;
            }

            public static Color Rebase( Color color, Color mod, Single inputIntensity )
            {
                var modIntensity = ( mod.r + mod.g + mod.b ) / 3;
                mod.r -= modIntensity;
                mod.g -= modIntensity;
                mod.b -= modIntensity;

                mod /= modIntensity;

                mod *= modIntensity;

                mod.r += inputIntensity;
                mod.g += inputIntensity;
                mod.b += inputIntensity;

                return mod;
            }
        }

        private static Sprite CreateSkinIcon( Color top, Color right, Color bottom, Color left, Color cross )
        {
            Texture2D tex = new Texture2D( 128, 128, TextureFormat.RGBA32, false );
            new IconTexJob
            {
                top = top,
                bottom = bottom,
                right = right,
                left = left,
                cross = cross,
                texOutput = tex.GetRawTextureData<Color32>()
            }.Schedule( 16384, 1 ).Complete();
            tex.wrapMode = TextureWrapMode.Clamp;
            tex.Apply();
            return Sprite.Create( tex, new Rect( 0, 0, 128, 128 ), new Vector2( 0.5f, 0.5f ) );
        }
        private struct IconTexJob : IJobParallelFor
        {
            [ReadOnly]
            public Color32 top;
            [ReadOnly]
            public Color32 right;
            [ReadOnly]
            public Color32 bottom;
            [ReadOnly]
            public Color32 left;
            [ReadOnly]
            public Color32 cross;
            public NativeArray<Color32> texOutput;
            public void Execute( Int32 index )
            {
                Int32 x = (index % 128) - 64;
                Int32 y = Mathf.FloorToInt( index / 128 ) - 64;
                if( Math.Abs(Math.Abs(y) - Math.Abs(x)) <= 2 )
                {
                    this.texOutput[index] = this.cross;
                    return;
                }
                if( y > x && y > -x )
                {
                    this.texOutput[index] = this.top;
                    return;
                }
                if( y < x && y < -x )
                {
                    this.texOutput[index] = this.bottom;
                    return;
                }
                if( y > x && y < -x )
                {
                    this.texOutput[index] = this.left;
                    return;
                }
                if( y < x && y > -x )
                {
                    this.texOutput[index] = this.right;
                    return;
                }
            }
        }



        private static Texture2D[] CreateArtiSkinTex()
        {
            Texture2D tex = new Texture2D( 512, 512, TextureFormat.RGBAFloat, false );
            var texArray = tex.GetRawTextureData<Color>();
            var texSize = new Vector2Int( tex.width, tex.height );
            Texture2D tex2 = new Texture2D( 512, 512, TextureFormat.RGBAFloat, false );
            var texArray2 = tex2.GetRawTextureData<Color>();
            var texSize2 = new Vector2Int( tex2.width, tex2.height );

            DrawBlock( texArray, new Color( 0.2f, 0.2f, 0.2f, 1.0f ), new RectInt( 0, 0, 512, 512 ), texSize ).Complete();  //Base
            DrawBlock( texArray2, Color.clear, new RectInt( 0, 0, 512, 512 ), texSize2 ).Complete();
            DrawBlock( texArray, new Color( 0.015f, 0.026f, 0.04f, 0.0f ), new RectInt( 56, 0, 54, 219 ), texSize ).Complete();   //head and gauntlets
            DrawBlock( texArray, new Color( 1.0f, 1.0f, 1.0f, 0.0f ), new RectInt( 0, 485, 400, 27 ), texSize ).Complete(); //Front side gauntlets
            DrawBlock( texArray2, new Color( 0.0f, 0.0f, 10.0f, 1.0f ), new RectInt( 0, 485, 400, 27 ), texSize2 ).Complete();

            DrawBlock( texArray, new Color( 0.1f, 0.1f, 0.1f, 1f ), new RectInt( 0, 0, 56, 55 ), texSize ).Complete();    //Stripe things?
            DrawBlock( texArray, new Color( 0.05f, 0.05f, 0.05f, 1.0f ), new RectInt( 0, 55, 56, 55 ), texSize ).Complete();
            DrawBlock( texArray, new Color( 0.1f, 0.1f, 0.1f, 1.0f ), new RectInt( 0, 110, 56, 55 ), texSize ).Complete(); //Misc trim
            DrawBlock( texArray, new Color( 0.7f, 0.7f, 0.7f, 1.0f ), new RectInt( 0, 165, 56, 55 ), texSize ).Complete();  // body/skin1
            DrawBlock( texArray, new Color( 0.8f, 0.8f, 0.8f, 1.0f ), new RectInt( 0, 220, 56, 55 ), texSize ).Complete();  // body/skin2
            DrawBlock( texArray, new Color( 0f, 0f, 10f, 1f ), new RectInt( 170, 0, 171, 164 ), texSize ).Complete(); //Metal trim stuff
            DrawBlock( texArray2, new Color( 0f, 0f, 10f, 1f ), new RectInt( 170, 0, 171, 164 ), texSize2 ).Complete();
            DrawBlock( texArray, Color.black, new RectInt( 504, 0, 8, 512 ), texSize ).Complete();
            DrawBlock( texArray, Color.black, new RectInt( 482, 0, 15, 137 ), texSize ).Complete();
            DrawBlock( texArray, Color.black, new RectInt( 485, 137, 12, 305 ), texSize ).Complete();
            DrawBlock( texArray, Color.black, new RectInt( 488, 442, 9, 70 ), texSize ).Complete();
            DrawBlock( texArray, Color.black, new RectInt( 482, 466, 6, 45 ), texSize ).Complete();
            DrawBlock( texArray, Color.black, new RectInt( 482, 387, 3, 53 ), texSize ).Complete();
            DrawBlock( texArray, Color.black, new RectInt( 482, 312, 3, 22 ), texSize ).Complete();

            tex.Apply();
            tex2.Apply();

            var texReturn = new Texture2D[2];
            texReturn[0] = tex;
            texReturn[1] = tex2;

            return texReturn;
        }

        private static JobHandle DrawBlock( NativeArray<Color> tex, Color color, RectInt block, Vector2Int texSize )
        {
            var blockJob = new TexBlockJob
            {
                startX = block.xMin,
                startY = block.yMin,
                sizeX = block.width,
                sizeY = block.height,
                texSizeX = texSize.x,
                texSizeY = texSize.y,
                tex = tex,
                color = color
            };

            return blockJob.Schedule( block.width * block.height, 1 );
        }

        private struct TexBlockJob : IJobParallelFor
        {
            public Int32 startX;
            public Int32 startY;
            public Int32 sizeX;
            public Int32 sizeY;
            public Color32 color;
            public NativeArray<Color> tex;
            public Int32 texSizeX;
            public Int32 texSizeY;

            public void Execute( Int32 index )
            {
                var x = this.startX + ( index % this.sizeX );
                var y = this.texSizeY - (this.startY + Mathf.FloorToInt( index / this.sizeX ) ) - 1;
                //y = this.texSizeY - y - 1;
                var id = ( x + y * this.texSizeX );

                this.tex[id] = this.color;
            }
        }

        private static void DebugMaterialInfo( Material m )
        {
            UnityEngine.Debug.Log( "Material name: " + m.name );
            String[] s = m.shaderKeywords;
            UnityEngine.Debug.Log( "Shader keywords" );
            for( Int32 i = 0; i < s.Length; i++ )
            {
                UnityEngine.Debug.Log( s[i] );
            }

            UnityEngine.Debug.Log( "Shader name: " + m.shader.name );

            UnityEngine.Debug.Log( "Texture Properties" );
            String[] s2 = m.GetTexturePropertyNames();
            for( Int32 i = 0; i < s2.Length; i++ )
            {
                UnityEngine.Debug.Log( s2[i] + " : " + m.GetTexture( s2[i] ) );
            }
        }
    }
}