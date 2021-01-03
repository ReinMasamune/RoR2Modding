namespace MaterialTesting
{
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;

    using BepInEx;
    using BepInEx.Logging;

    using ReinCore;

    using RoR2;

    using UnityEngine;

    [BepInDependency("___AssemblyLoader-com.Rein.Core")]
    [BepInPlugin("Rein.MaterialTesting", "Material Testing", "1.0.0")]
    public sealed class Plugin : BaseUnityPlugin
    {
        const String bundleFileName = "MaterialTestBundle";

        public StandardMaterial newMat;

        private static Plugin instance;
        private static ManualLogSource log => instance.Logger;

        private void Awake()
        {
            instance = this;
            LoadBundle(new FileInfo(base.Info.Location).Directory);
            if(!bundle) return;
            LoadMaterialAsset();
            if(!material) return;
        }

        private static AssetBundle bundle;
        private static MaterialInfo material;
        private static GameObject prefabInstance;
        private static GameObject otherInstance;



        private static void LoadBundle(DirectoryInfo directory)
        {
            var file = directory.EnumerateFiles().Where(static x => x.Name.ToLowerInvariant() == bundleFileName.ToLowerInvariant()).FirstOrDefault();
            if(file is null)
            {
                log.LogError($"No assetbundle with name '{bundleFileName}' found.");
                return;
            }

            var bund = AssetBundle.LoadFromFile(file.FullName);
            if(!bund)
            {
                log.LogError($"Unable to load assetbundle '{bundleFileName}'");
                return;
            }
            bundle = bund;
        }

        private static void LoadMaterialAsset()
        {
            var asset = bundle.LoadAllAssets<MaterialInfo>().FirstOrDefault();
            if(!asset)
            {
                log.LogError($"No MaterialInfo found in bundle");
                return;
            }
            material = asset;
        }


        private static void SpawnPrefabAt(Transform parent)
        {
            if(prefabInstance) Destroy(prefabInstance);

            var obj = new GameObject("materialtestobject", typeof(MeshFilter), typeof(MeshRenderer));
            var fil = obj.GetComponent<MeshFilter>();
            var rend = obj.GetComponent<MeshRenderer>();
            rend.material = material.material;

            fil.mesh = material.target;
            var sz = fil.mesh.bounds.size;
            var bbmax = Mathf.Max(sz.x, sz.y, sz.z);

            var ratio = material.size / bbmax;

            obj.transform.parent = parent;
            obj.transform.localScale = Vector3.one * ratio * 2f;
            obj.transform.localPosition = new(0f, 0f, material.distance);
            obj.transform.localEulerAngles = new(0f, 90f, 0f);
            obj.transform.parent = null;

            prefabInstance = obj;
        }

        private static void SpawnPrefabAt(RaycastHit info, Vector3 forward)
        {
            if(prefabInstance) Destroy(prefabInstance);
            if(otherInstance) Destroy(otherInstance);

            var obj = new GameObject("materialtestobject", typeof(MeshFilter), typeof(MeshRenderer));
            var fil = obj.GetComponent<MeshFilter>();
            var rend = obj.GetComponent<MeshRenderer>();
            rend.material = material.material;

            fil.mesh = material.target;
            var sz = fil.mesh.bounds.size;
            var bbmax = Mathf.Max(sz.x, sz.y, sz.z);

            var ratio = material.size / bbmax;

            obj.transform.localScale = Vector3.one * ratio * 2f;
            obj.transform.position = info.point + (info.normal * sz.y * ratio * 0f);
            obj.transform.rotation = Util.QuaternionSafeLookRotation(forward, info.normal);

            //var obj2 = new GameObject("materialtestobject", typeof(MeshFilter), typeof(MeshRenderer));
            //var fil2 = obj2.GetComponent<MeshFilter>();
            //var rend2= obj2.GetComponent<MeshRenderer>();

            //if(instance.newMat is null)
            //{
            //    instance.newMat = new StandardMaterial("DogMat");
            //    var m = instance.newMat;

            //    var oldEmis = bundle.LoadAsset<Texture2D>("Assets/_SpaceDog/Textures/SD_E.png");
            //    var oldBase = bundle.LoadAsset<Texture2D>("Assets/_SpaceDog/Textures/SD_Base.png");

            //    const Single emisDivisor = 2f;

            //    const Single cutoff = 0.7f;

            //    var newEmis = new Texture2D(oldEmis.width, oldEmis.height, TextureFormat.ARGB32, false);
            //    newEmis.SetPixels(oldEmis.GetPixels().Select(static c => new Color(1f, 1f, 1f, (c.r + c.g + c.b) / emisDivisor)).Select(static c => c.a < cutoff ? new Color(0f, 0f, 0f, 1f) : new Color(c.a, c.a, c.a, 1f)).ToArray());
            //    newEmis.Apply();

            //    m.mainTexture.texture = oldBase;
            //    m.mainTexture.tiling = Vector2.one;
            //    m.mainTexture.offset = Vector2.zero;
            //    m.normalMap.texture = bundle.LoadAsset<Texture2D>("Assets/_SpaceDog/Textures/SD_N.png");
            //    m.normalMap.tiling = Vector2.one;
            //    m.normalMap.offset = Vector2.zero;
            //    m.emissionTexture.texture = newEmis;

            //    m.emissionColor = new Color(1f, 0.3f, 0f, 1f);
            //    m.emissionPower = 1f;
            //    m.specularExponent = 2f;
            //    m.ignoreDiffuseAlphaForSpecular = true;
            //    m.specularStrength = 0.2f;
            //    m.smoothness = 0.1f;


            //    m.flowmapEnabled = true;
            //    //m.material.SetShaderPassEnabled("FLOWMAP", true);



            //    m.flowmapTexture.texture = new Texture2D(256, 256, TextureFormat.ARGB32, false);

            //    var flowHeight = new Texture2D(oldEmis.width, oldEmis.height, TextureFormat.ARGB32, false);

            //    var minDif = Single.MaxValue;
            //    var maxDif = Single.MinValue;

            //    const Single diffScale = 0.8f;
            //    const Single sumDivisor = 2f;
            //    const Single dMin = 0.5f;
            //    const Single dMax = 1f;



            //    const Single invDiffScale = 1f - diffScale;
            //    const Single dRange = dMax - dMin;

            //    Color Eval((Color emis, Color main) input)
            //    {
            //        var (emis, main) = input;
            //        var emisV = (Vector3)(Vector4)emis;
            //        var mainV = (Vector3)(Vector4)main;
            //        var l = emisV.magnitude * mainV.magnitude;
            //        var d = Vector3.Dot(emisV, mainV) / l;
            //        if(d > maxDif) maxDif = d;
            //        if(d < minDif) minDif = d;
            //        var sum = (emis.r + emis.g + emis.b) / sumDivisor;
            //        var a = sum * (invDiffScale + diffScale * (d - dMin) / dRange);
            //        return new(Mathf.Sqrt(a), Mathf.Sqrt(a), Mathf.Sqrt(a), a);
            //    }


            //    flowHeight.SetPixels(oldEmis.GetPixels().Zip(oldBase.GetPixels(), static (emis, main) => (emis, main)).Select(Eval).ToArray());
            //    flowHeight.Apply();

            //    log.LogWarning($"min: {minDif} max: {maxDif}");

            //    m.flowmapHeightmap.texture = flowHeight;
            //    m.flowmapHeightmap.tiling = Vector2.one;
            //    m.flowmapHeightmap.offset = Vector2.zero;

            //    var ramp = new Texture2D(256, 16, TextureFormat.RGBAFloat, false);
            //    var grad = new Gradient()
            //    {
            //        alphaKeys = new GradientAlphaKey[]
            //        {
            //            new(0f, 0f),
            //            new(0f, 0.33f),
            //            new(0.9f, 0.9f),
            //            new(0f, 1f),
            //        },
            //        colorKeys = new GradientColorKey[]
            //        {
            //            new(new(0f, 0f, 0f), 0f),
            //            new(new(0f, 0f, 0f), 0.33f),
            //            new(new(1f, 0.1f, 0f), 0.9f),
            //            new(new(1f, 0.35f, 0f), 0.9999f),
            //            new(new(0f, 0f, 0f), 1f),
            //        },
            //        mode = GradientMode.Blend,
            //    };
            //    ramp.SetPixels(Enumerable.Repeat(Enumerable.Range(0, 256).Select(static i => (Single)i / (Single)255).Select(t => grad.Evaluate(t)), 16).SelectMany(static a => a).ToArray());
            //    ramp.Apply();

            //    m.flowmapHeightRamp.texture = ramp;
            //}

            //rend2.material = instance.newMat.material;

            //fil2.mesh = material.target;
            //var sz2 = fil2.mesh.bounds.size;
            //var bbmax2 = Mathf.Max(sz2.x, sz2.y, sz2.z);

            //var ratio2 = material.size / bbmax2;

            //obj2.transform.localScale = Vector3.one * ratio2 * 2f;
            //obj2.transform.position = info.point + (info.normal * sz.y * ratio * 0f) + (info.normal * 2f);
            //obj2.transform.rotation = Util.QuaternionSafeLookRotation(forward, info.normal);


            prefabInstance = obj;
            //otherInstance = obj2;
        }


        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.F5))
            {
                if(NetworkUser.localPlayers.FirstOrDefault() is NetworkUser user && user && user.master && user.master.bodyInstanceObject && user.master.bodyInstanceObject.GetComponent<CharacterBody>() is CharacterBody body && body && body.inputBank.GetAimRaycast(100f, out var info))
                {
                    SpawnPrefabAt(info, body.characterDirection.forward);
                } else
                {
                    SpawnPrefabAt(Camera.main.transform);
                }
                
            }
        }
    }
}
