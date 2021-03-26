namespace MaterialTesting
{
    using System;
    using System.Collections;
    using System.IO;
    using System.Linq;

    using BepInEx;

    using UnityEngine;
    using UnityEngine.SceneManagement;

    [BepInPlugin("Rein.MaterialTesting", "Material Testing", "1.0.0")]
    public sealed class Plugin : BaseUnityPlugin
    {
        const String fileName = "terrainstuff";

        private CustomRenderTexture normal;
        private CustomRenderTexture redTop;
        private CustomRenderTexture green;
        private CustomRenderTexture redSide;
        private CustomRenderTexture blue;

        private void Awake()
        {
            var file = new FileInfo(base.Info.Location).Directory.EnumerateFiles().Where(static x => x.Name.ToLower() == fileName).FirstOrDefault();
            if(file is null)
            {
                base.Logger.LogError($"No assetbundle with name '{fileName}' found.");
                return;
            }

            var bund = AssetBundle.LoadFromFile(file.FullName);
            if(!bund)
            {
                base.Logger.LogError($"Unable to load assetbundle '{fileName}'");
                return;
            }

            this.redSide = bund.LoadAsset<CustomRenderTexture>("Assets/TerrainFlowTest/_RedSideBlend.asset");
            this.redTop = bund.LoadAsset<CustomRenderTexture>("Assets/TerrainFlowTest/_RedTopBlend.asset");
            this.normal = bund.LoadAsset<CustomRenderTexture>("Assets/TerrainFlowTest/_NormalBlend.asset");
            this.green = bund.LoadAsset<CustomRenderTexture>("Assets/TerrainFlowTest/_GreenBlend.asset");
            this.blue = bund.LoadAsset<CustomRenderTexture>("Assets/TerrainFlowTest/_BlueBlend.asset");


            SceneManager.sceneLoaded += this.SceneManager_sceneLoaded;
        }

        private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            this.StartCoroutine(this.SwapMats(arg0));

        }

        private IEnumerator SwapMats(Scene scene)
        {
            yield return new WaitForSeconds(5f);
            foreach(var go in scene.GetRootGameObjects())
            {
                foreach(var rend in go.GetComponentsInChildren<Renderer>())
                {
                    var mat = rend.sharedMaterial;
                    if(mat && mat.shader && mat.shader.name is not null && mat.shader.name.ToLower().Contains("terrain"))
                    {
                        this.ApplyStuff(mat);
                    }
                }
            }
        }

        private void ApplyStuff(Material mat)
        {
            var cnorm = mat.GetTexture("_NormalTex");
            var credt = mat.GetTexture("_RedChannelTopTex");
            var creds = mat.GetTexture("_RedChannelSideTex");
            var cgreen = mat.GetTexture("_GreenChannelTex");
            var cblue = mat.GetTexture("_BlueChannelTex");

            if(cnorm is CustomRenderTexture || credt is CustomRenderTexture || creds is CustomRenderTexture || cgreen is CustomRenderTexture || cblue is CustomRenderTexture) return;

            var norm = Instantiate(this.normal);
            var normm = Instantiate(norm.material);
            normm.SetTexture("_Tex1", cnorm);
            norm.material = normm;
            var redtx = Instantiate(this.redTop);
            var redtxm = Instantiate(redtx.material);
            redtxm.SetTexture("_Tex1", credt);
            redtx.material = redtxm;
            var redsx = Instantiate(this.redSide);
            var redsxm = Instantiate(redsx.material);
            redsxm.SetTexture("_Tex1", creds);
            redsx.material = redsxm;
            var greenx = Instantiate(this.green);
            var greenxm = Instantiate(greenx.material);
            greenxm.SetTexture("_Tex1", cgreen);
            greenx.material = greenxm;
            var bluex = Instantiate(this.blue);
            var bluexm = Instantiate(bluex.material);
            bluexm.SetTexture("_Tex1", cblue);
            bluex.material = bluexm;
            mat.SetTexture("_NormalTex", norm);
            mat.SetTexture("_RedChannelTopTex", redtx);
            mat.SetTexture("_RedChannelSideTex", redsx);
            mat.SetTexture("_GreenChannelTex", greenx);
            mat.SetTexture("_BlueChannelTex", bluex);
        }


        //const String bundleFileName = "MaterialTestBundle";

        //private static Plugin instance;
        //private static ManualLogSource log => instance.Logger;

        //private void Awake()
        //{
        //    instance = this;
        //    LoadBundle(new FileInfo(base.Info.Location).Directory);
        //    if(!bundle) return;
        //    LoadMaterialAsset();
        //    if(!material) return;

        //    overlays = typeof(CharacterModel).GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
        //        .Where(f => f.FieldType == typeof(Material))
        //        .ToArray();

        //    //var mesh = material.target;
        //    //mesh.normals = mesh.normals.Select(x => x / 2f).ToArray();
        //}

        //private static AssetBundle bundle;
        //private static MaterialInfo material;
        //private static GameObject prefabInstance;

        //private static FieldInfo[] overlays;

        //private static Int32 counter = 0;

        //private static Material baseMat
        //{
        //    get
        //    {
        //        if(!_mat) _mat = GetMaterial();
        //        return _mat;
        //    }
        //}
        //private static Material _mat;

        //private static Material GetMaterial()
        //{
        //    //return Resources.Load<Material>("Materials/matIsFrozen");
        //    return material.material;
        //}

        //private static void LoadBundle(DirectoryInfo directory)
        //{
        //    var file = directory.EnumerateFiles().Where(static x => x.Name.ToLowerInvariant() == bundleFileName.ToLowerInvariant()).FirstOrDefault();
        //    if(file is null)
        //    {
        //        log.LogError($"No assetbundle with name '{bundleFileName}' found.");
        //        return;
        //    }

        //    var bund = AssetBundle.LoadFromFile(file.FullName);
        //    if(!bund)
        //    {
        //        log.LogError($"Unable to load assetbundle '{bundleFileName}'");
        //        return;
        //    }
        //    bundle = bund;
        //}

        //private static void LoadMaterialAsset()
        //{
        //    var asset = bundle.LoadAllAssets<MaterialInfo>().FirstOrDefault();
        //    if(!asset)
        //    {
        //        log.LogError($"No MaterialInfo found in bundle");
        //        return;
        //    }
        //    material = asset;
        //}


        //private void SpawnPrefabAt(RaycastHit info, Vector3 forward)
        //{
        //    if(prefabInstance) Destroy(prefabInstance);

        //    //var par = new GameObject("parentObj");


        //    var obj = UnityEngine.Object.Instantiate(material.target);
        //    var sz = obj.GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh.bounds.size;
        //    var bbmax = Mathf.Max(sz.x, sz.y, sz.z);

        //    var ratio = material.size / bbmax;

        //    //obj.transform.parent = (prefabInstance = par).transform;
        //    prefabInstance = obj;
        //    obj.transform.localScale = Vector3.one * ratio * 2f;
        //    //obj.transform.localPosition = Vector3.zero;
        //    //obj.transform.localRotation = Quaternion.identity;
        //    //obj.transform.localScale = Vector3.one;
        //    obj.transform.position = info.point + (info.normal * sz.y * ratio * 0.1f);
        //    obj.transform.rotation = Util.QuaternionSafeLookRotation(forward, info.normal);

        //    obj.GetComponent<Animator>().Play("Bark");

        //    //_ = base.StartCoroutine(UpdateYOffset(obj.transform));

        //    counter = 0;
        //}

        //private IEnumerator UpdateYOffset(Transform target)
        //{
        //    const Single distance = 1f;
        //    Single startTime = Time.time;
        //    Single curTime = startTime;
        //    Single startY = target.position.y;
        //    Single endY = startY - distance;
        //    var endTime = startTime + 2;
        //    while(curTime <= endTime)
        //    {
        //        yield return new WaitForEndOfFrame();
        //        curTime = Time.time;

        //        var frac = curTime - startTime / (endTime - startTime);
        //        var pos = target.position;
        //        pos.y = Mathf.Lerp(startY, endY, frac);
        //        target.position = pos;
        //    }
        //}


        //private void Update()
        //{
        //    if(Input.GetKeyDown(KeyCode.F5))
        //    {
        //        if(NetworkUser.localPlayers.FirstOrDefault() is NetworkUser user && user && user.master && user.master.bodyInstanceObject && user.master.bodyInstanceObject.GetComponent<CharacterBody>() is CharacterBody body && body && body.inputBank.GetAimRaycast(100f, out var info))
        //        {
        //            this.SpawnPrefabAt(info, body.characterDirection.forward);
        //        } else
        //        {
        //            base.Logger.LogError("No ground hit for spawn");
        //        }

        //    }

        //    if(Input.GetKeyDown(KeyCode.F6))
        //    {
        //        if(prefabInstance && counter < overlays.Length)
        //        {
        //            Step();
        //        }
        //    }
        //}

        //private void Step()
        //{
        //    var fld = overlays[counter++];
        //    if(fld is null) return;
        //    if(fld.GetValue(null) is not Material mat) return;

        //    var name = fld.Name;
        //    base.Logger.LogMessage($"Material overlay: {name}");

        //    var rend = prefabInstance.GetComponentInChildren<Renderer>();

        //    switch(name)
        //    {
        //        case "cloakedMaterial":
        //            rend.materials = new[] { mat, };
        //            return;
        //        case "doppelgangerMaterial":
        //            rend.materials = new[] { mat, mat, };
        //            return;
        //        case "ghostMaterial":
        //            rend.materials = new[] { mat, mat, };
        //            return;
        //    }


        //    rend.materials = new[] { baseMat, mat, };
        //}
    }
}
