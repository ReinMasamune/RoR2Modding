

/*
namespace RogueWispPlugin.Modules
{
    public static class WispModelModule
    {
        private const UInt32 matIndex = 0;
        private static Type nameTransformPair;

        public static void DoModule( GameObject body, Dictionary<Type, Component> dic )
        {
            EditModelScale( body, dic );
            EditModelStructure( body, dic );
            EditModelFlare( body, dic );
            EditModelParticles( body, dic );
            EditModelSkins( body, dic );
            EditModelUVs( body, dic );
            DoChildLocator( body, dic );
            DoIDRS( body, dic );
            DoHurtBoxes( body, dic );
        }

        private static void EditModelScale( GameObject body, Dictionary<Type, Component> dic )
        {
            dic.C<ModelLocator>().modelBaseTransform.gameObject.name = "ModelBase";
            dic.C<ModelLocator>().modelBaseTransform.localPosition = new Vector3( 0f, -0.3f, 0f );
            dic.C<ModelLocator>().modelTransform.localScale = new Vector3( 0.8f, 0.8f, 0.8f );
            dic.C<ModelLocator>().modelTransform.localPosition = new Vector3( 0f, 0.3f, 0f );
        }

        private static void EditModelStructure( GameObject body, Dictionary<Type, Component> dic )
        {
            Transform modelTransform = dic.C<ModelLocator>().modelTransform;
            GameObject pivotPoint = new GameObject("CannonPivot");
            pivotPoint.transform.parent = modelTransform;
            pivotPoint.transform.localPosition = new Vector3( 0f, 2f, 0f );
            pivotPoint.transform.localEulerAngles = new Vector3( -90f, 0f, 0f );

            Transform armatureTransform = modelTransform.Find("AncientWispArmature");
            armatureTransform.parent = pivotPoint.transform;

            GameObject beamParent = new GameObject("BeamParent");
            beamParent.transform.parent = pivotPoint.transform;
            beamParent.transform.localPosition = new Vector3( 0f, 0.5f, -1f );
            beamParent.transform.localEulerAngles = new Vector3( 0f, 0f, 0f );
        }

        private static void EditModelFlare( GameObject body, Dictionary<Type, Component> dic )
        {
            GameObject flareObj = dic.C<ModelLocator>().modelTransform.Find("CannonPivot").Find("AncientWispArmature").Find("Head").Find("GameObject").gameObject;
            flareObj.transform.localScale = Vector3.one;

            GameObject flare2Obj = MonoBehaviour.Instantiate<GameObject>(flareObj, flareObj.transform.parent);
            flare2Obj.transform.localEulerAngles = new Vector3( 0f, 180f, 0f );
            GameObject refObj = new GameObject("FlareRef");
            refObj.transform.parent = flareObj.transform.parent;
            refObj.transform.localPosition = Vector3.zero;
            refObj.transform.localEulerAngles = new Vector3( 0f, 180f, 0f );

            flare2Obj.GetComponent<EyeFlare>().directionSource = refObj.transform;

            dic.C<Components.WispFlareController>().flare1 = flareObj.GetComponent<SpriteRenderer>();
            dic.C<Components.WispFlareController>().flare2 = flare2Obj.GetComponent<SpriteRenderer>();

            flareObj.SetActive( false );
            flare2Obj.SetActive( false );

            //MonoBehaviour.Destroy(flareObj.GetComponent<EyeFlare>());
        }

        private static void EditModelParticles( GameObject body, Dictionary<Type, Component> dic )
        {
            Transform modelTransform = dic.C<ModelLocator>().modelTransform;
            CharacterModel bodyCharModel = modelTransform.GetComponent<CharacterModel>();
            MonoBehaviour.Destroy( bodyCharModel.baseLightInfos[0].light.gameObject );
            MonoBehaviour.Destroy( bodyCharModel.baseLightInfos[1].light.gameObject );
            MonoBehaviour.Destroy( bodyCharModel.baseParticleSystemInfos[0].particleSystem.gameObject );
            MonoBehaviour.Destroy( bodyCharModel.baseParticleSystemInfos[1].particleSystem.gameObject );
            MonoBehaviour.Destroy( bodyCharModel.gameObject.GetComponent<AncientWispFireController>() );
            Array.Resize<CharacterModel.LightInfo>( ref bodyCharModel.baseLightInfos, 0 );

            Components.WispFlamesController flameCont = dic.C<Components.WispFlamesController>();
            flameCont.passive = dic.C<Components.WispPassiveController>();

            String tempName;
            ParticleSystem tempPS;
            ParticleSystemRenderer tempPSR;

            Dictionary<String, FlamePSInfo> flames = CreateFlameDictionary();
            List<PSCont> tempPSList = new List<PSCont>();

            foreach( Transform t in modelTransform.GetComponentsInChildren<Transform>() )
            {
                if( !t ) continue;
                tempName = t.gameObject.name;

                if( flames.ContainsKey( tempName ) )
                {
                    tempPS = t.gameObject.AddOrGetComponent<ParticleSystem>();
                    tempPSR = t.gameObject.AddOrGetComponent<ParticleSystemRenderer>();
                    tempPS.SetupFlameParticleSystem( 0, flames[tempName] );
                    tempPSList.Add( new PSCont
                    {
                        ps = tempPS,
                        psr = tempPSR,
                        info = flames[tempName]
                    } );
                }
            }

            Array.Resize<CharacterModel.ParticleSystemInfo>( ref bodyCharModel.baseParticleSystemInfos, tempPSList.Count );

            for( Int32 i = 0; i < tempPSList.Count; i++ )
            {
                bodyCharModel.baseParticleSystemInfos[i] = new CharacterModel.ParticleSystemInfo
                {
                    particleSystem = tempPSList[i].ps,
                    renderer = tempPSList[i].psr,
                    defaultMaterial = Main.fireMaterials[0][0]
                };
                flameCont.flames.Add( tempPSList[i].ps );
                flameCont.flameInfos.Add( tempPSList[i].info.rate );
            }
        }

        private static void EditModelSkins( GameObject body, Dictionary<Type, Component> dic )
        {
            GameObject bodyModel = dic.C<ModelLocator>().modelTransform.gameObject;
            CharacterModel bodyCharModel = bodyModel.GetComponent<CharacterModel>();
            ModelSkinController bodySkins = bodyModel.AddOrGetComponent<ModelSkinController>();

            Renderer armorRenderer = bodyCharModel.baseRendererInfos[0].renderer;
            bodyCharModel.baseRendererInfos[0].defaultMaterial = Main.armorMaterials[0];

            bodyCharModel.baseRendererInfos[0].ignoreOverlays = false;



            CharacterModel.ParticleSystemInfo[] particles = bodyCharModel.baseParticleSystemInfos;


            for( Int32 i = 0; i < particles.Length; i++ )
            {
                particles[i].renderer.material = Main.fireMaterials[0][0];
                particles[i].defaultMaterial = Main.fireMaterials[0][0];
            }

            armorRenderer.material = Main.armorMaterials[0];

            CharacterModel.RendererInfo[][] rendererInfos = new CharacterModel.RendererInfo[8][];
            for( Int32 i = 0; i < 8; i++ )
            {
                rendererInfos[i] = new CharacterModel.RendererInfo[particles.Length + 1];
                for( Int32 j = 0; j < particles.Length; j++ )
                {
                    rendererInfos[i][j] = CreateFlameRendererInfo( particles[j].renderer, Main.fireMaterials[i][matIndex] );
                }
                rendererInfos[i][particles.Length] = new CharacterModel.RendererInfo
                {
                    renderer = armorRenderer,
                    defaultMaterial = Main.armorMaterials[i],
                    defaultShadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On,
                    ignoreOverlays = false
                };
            }

            String[] skinNames = new String[8];
            skinNames[0] = "WISP_SURVIVOR_SKIN_1";
            skinNames[1] = "WISP_SURVIVOR_SKIN_2";
            skinNames[2] = "WISP_SURVIVOR_SKIN_3";
            skinNames[3] = "WISP_SURVIVOR_SKIN_4";
            skinNames[4] = "WISP_SURVIVOR_SKIN_5";
            skinNames[5] = "WISP_SURVIVOR_SKIN_6";
            skinNames[6] = "WISP_SURVIVOR_SKIN_7";
            skinNames[7] = "WISP_SURVIVOR_SKIN_8";

            SkinDef[] skins = new SkinDef[8];

            for( Int32 i = 0; i < 8; i++ )
            {
                R2API.SkinAPI.SkinDefInfo skinInfo = new R2API.SkinAPI.SkinDefInfo
                {
                    baseSkins = Array.Empty<SkinDef>(),
                    icon = Resources.Load<Sprite>("NotAPath"),
                    nameToken = skinNames[i],
                    name = skinNames[i],
                    unlockableName = "",
                    rootObject = bodyModel,
                    rendererInfos = rendererInfos[i]
                };
                skins[i] = CreateNewSkinDef( skinInfo );
            }

            bodySkins.skins = skins;
        }

        private static void EditModelUVs( GameObject body, Dictionary<Type, Component> dic )
        {
            Mesh m = dic.C<ModelLocator>().modelTransform.Find("AncientWispMesh").GetComponent<SkinnedMeshRenderer>().sharedMesh;
            Vector2[] newUvs = new Vector2[m.vertexCount];
            Vector3[] verts = m.vertices;
            Single tempu = 0f;
            Single tempv = 0f;
            Single tempp = 0f;
            Vector2 tempv2 = Vector2.zero;
            for( Int32 i = 0; i < m.vertexCount; i++ )
            {
                tempv = verts[i].z;
                tempu = Mathf.Atan2( verts[i].y, verts[i].x );

                newUvs[i] = new Vector2( tempu, tempv );
            }
            m.uv = newUvs;
        }

        private static void DoChildLocator( GameObject body, Dictionary<Type, Component> dic )
        {
            Transform model = dic.C<ModelLocator>().modelTransform;
            RagdollController rag = model.gameObject.AddComponent<RagdollController>();

            ChildLocator children = model.GetComponent<ChildLocator>();
            FieldInfo f = typeof(ChildLocator).GetField("transformPairs", BindingFlags.NonPublic | BindingFlags.Instance);
            System.Object thing = f.GetValue( children );
            System.Object[] pairs = ((Array)thing).Cast<System.Object>().ToArray();
            Type pairsArray = thing.GetType();
            nameTransformPair = thing.GetType().GetElementType();
            Array.Resize<System.Object>( ref pairs, pairs.Length + 16 );

            List<Transform> bones = new List<Transform>();

            BoxCollider box;
            CapsuleCollider cap;
            Rigidbody rb;

            Transform t2;
            Array v = Array.CreateInstance(nameTransformPair, pairs.Length + 16);

            Int32 i = 0;
            for( i = 0; i < 3; i++ )
            {
                v.SetValue( pairs[i], i );
            }
            foreach( Transform t in model.GetComponentsInChildren<Transform>() )
            {
                if( !t ) continue;
                switch( t.name )
                {
                    default:
                        break;

                    case "ChestCannon1":
                        t2 = new GameObject( "ItemParent" + i.ToString() ).transform;
                        t2.parent = t;
                        t2.localPosition = new Vector3( 0f, 0.65f, 0f );
                        t2.localEulerAngles = new Vector3( 180f, 0f, 0f );
                        t2.localScale = new Vector3( 1.25f, 1.25f, 1.25f );
                        v.SetValue( (CreateNameTransformPair( "Chest", t2 )), i++ );

                        box = t.gameObject.AddComponent<BoxCollider>();
                        box.size = new Vector3( 0.5f, 0.9f, 0.1f );
                        box.center = new Vector3( 0f, 0.4f, 0f );
                        box.enabled = false;
                        t.gameObject.AddComponent<Rigidbody>();
                        bones.Add( t );
                        break;

                    case "ChestCannonGuard1":
                        box = t.gameObject.AddComponent<BoxCollider>();
                        box.size = new Vector3( 0.75f, 0.7f, 0.2f );
                        box.center = new Vector3( 0f, 0.25f, 0.05f );
                        box.enabled = false;
                        t.gameObject.AddComponent<Rigidbody>();
                        bones.Add( t );
                        break;

                    case "ChestCannon2":
                        t2 = new GameObject( "ItemParent" + i.ToString() ).transform;
                        t2.parent = t;
                        t2.localPosition = new Vector3( 0f, 0.5f, 0f );
                        t2.localEulerAngles = new Vector3( 180f, 0f, 0f );
                        t2.localScale = new Vector3( 1.25f, 1.25f, 1.25f );
                        v.SetValue( (CreateNameTransformPair( "Stomach", t2 )), i++ );

                        box = t.gameObject.AddComponent<BoxCollider>();
                        box.size = new Vector3( 0.5f, 0.9f, 0.1f );
                        box.center = new Vector3( 0f, 0.4f, 0f );
                        box.enabled = false;
                        t.gameObject.AddComponent<Rigidbody>();
                        bones.Add( t );
                        break;

                    case "ChestCannonGuard2":
                        box = t.gameObject.AddComponent<BoxCollider>();
                        box.size = new Vector3( 0.7f, 0.5f, 0.2f );
                        box.center = new Vector3( 0f, 0.17f, -0.05f );
                        box.enabled = false;
                        t.gameObject.AddComponent<Rigidbody>();
                        bones.Add( t );
                        break;

                    case "Head":
                        t2 = new GameObject( "ItemParent" + i.ToString() ).transform;
                        t2.parent = t;
                        t2.localPosition = new Vector3( 0f, 0.2f, 0f );
                        t2.localEulerAngles = new Vector3( 0f, 0f, 0f );
                        t2.localScale = new Vector3( 1.25f, 1.25f, 1.25f );
                        v.SetValue( (CreateNameTransformPair( "Head", t2 )), i++ );

                        cap = t.gameObject.AddComponent<CapsuleCollider>();
                        cap.direction = 0;
                        cap.radius = 0.25f;
                        cap.height = 1f;
                        cap.center = new Vector3( 0f, 0.25f, 0.15f );
                        t.gameObject.AddComponent<Rigidbody>();
                        bones.Add( t );
                        break;

                    case "thigh.r":
                        t2 = new GameObject( "ItemParent" + i.ToString() ).transform;
                        t2.parent = t;
                        t2.localPosition = new Vector3( 0f, 0.25f, 0f );
                        t2.localEulerAngles = new Vector3( 0f, -90f, 0f );
                        t2.localScale = new Vector3( 1.25f, 1.25f, 1.25f );
                        v.SetValue( CreateNameTransformPair( "ThighR", t2 ), i++ );

                        cap = t.gameObject.AddComponent<CapsuleCollider>();
                        cap.direction = 1;
                        cap.radius = 0.15f;
                        cap.height = 0.5f;
                        cap.center = new Vector3( 0f, 0.2f, 0f );
                        t.gameObject.AddComponent<Rigidbody>();
                        bones.Add( t );
                        break;

                    case "thigh.l":
                        t2 = new GameObject( "ItemParent" + i.ToString() ).transform;
                        t2.parent = t;
                        t2.localPosition = new Vector3( 0f, 0.25f, 0f );
                        t2.localEulerAngles = new Vector3( 0f, 90f, 0f );
                        t2.localScale = new Vector3( 1.25f, 1.25f, 1.25f );
                        v.SetValue( (CreateNameTransformPair( "ThighL", t2 )), i++ );

                        cap = t.gameObject.AddComponent<CapsuleCollider>();
                        cap.direction = 1;
                        cap.radius = 0.15f;
                        cap.height = 0.5f;
                        cap.center = new Vector3( 0f, 0.2f, 0f );
                        t.gameObject.AddComponent<Rigidbody>();
                        bones.Add( t );
                        break;

                    case "calf.r":
                        t2 = new GameObject( "ItemParent" + i.ToString() ).transform;
                        t2.parent = t;
                        t2.localPosition = new Vector3( 0f, 0f, 0f );
                        t2.localEulerAngles = new Vector3( 0f, 0f, 0f );
                        t2.localScale = new Vector3( 1.25f, 1.25f, 1.25f );
                        v.SetValue( (CreateNameTransformPair( "CalfR", t2 )), i++ );

                        cap = t.gameObject.AddComponent<CapsuleCollider>();
                        cap.direction = 1;
                        cap.radius = 0.125f;
                        cap.height = 0.7f;
                        cap.center = new Vector3( 0f, 0.3f, 0f );
                        t.gameObject.AddComponent<Rigidbody>();
                        bones.Add( t );
                        break;

                    case "calf.l":
                        t2 = new GameObject( "ItemParent" + i.ToString() ).transform;
                        t2.parent = t;
                        t2.localPosition = new Vector3( 0f, 0f, 0f );
                        t2.localEulerAngles = new Vector3( 0f, 0f, 0f );
                        t2.localScale = new Vector3( 1.25f, 1.25f, 1.25f );
                        v.SetValue( (CreateNameTransformPair( "CalfL", t2 )), i++ );

                        cap = t.gameObject.AddComponent<CapsuleCollider>();
                        cap.direction = 1;
                        cap.radius = 0.125f;
                        cap.height = 0.7f;
                        cap.center = new Vector3( 0f, 0.3f, 0f );
                        t.gameObject.AddComponent<Rigidbody>();
                        bones.Add( t );
                        break;

                    case "toe1.l":
                        t2 = new GameObject( "ItemParent" + i.ToString() ).transform;
                        t2.parent = t;
                        t2.localPosition = new Vector3( 0f, 0f, 0f );
                        t2.localEulerAngles = new Vector3( -45f, 0f, 0f );
                        t2.localScale = new Vector3( 1.25f, 1.25f, 1.25f );
                        v.SetValue( (CreateNameTransformPair( "FootL", t2 )), i++ );

                        box = t.gameObject.AddComponent<BoxCollider>();
                        box.size = new Vector3( 0.12f, 0.3f, 0.08f );
                        box.center = new Vector3( 0f, 0.08f, 0.01f );
                        box.enabled = false;
                        t.gameObject.AddComponent<Rigidbody>();
                        bones.Add( t );
                        break;

                    case "toe1.r":
                        t2 = new GameObject( "ItemParent" + i.ToString() ).transform;
                        t2.parent = t;
                        t2.localPosition = new Vector3( 0f, 0f, 0f );
                        t2.localEulerAngles = new Vector3( -45f, 0f, 0f );
                        t2.localScale = new Vector3( 1.25f, 1.25f, 1.25f );
                        v.SetValue( (CreateNameTransformPair( "FootR", t2 )), i++ );

                        box = t.gameObject.AddComponent<BoxCollider>();
                        box.size = new Vector3( 0.12f, 0.3f, 0.08f );
                        box.center = new Vector3( 0f, 0.08f, 0.01f );
                        box.enabled = false;
                        t.gameObject.AddComponent<Rigidbody>();
                        bones.Add( t );
                        break;

                    case "toe2.l":
                        box = t.gameObject.AddComponent<BoxCollider>();
                        box.size = new Vector3( 0.08f, 0.2f, 0.06f );
                        box.center = new Vector3( 0f, 0.06f, 0f );
                        box.enabled = false;
                        t.gameObject.AddComponent<Rigidbody>();
                        bones.Add( t );
                        break;

                    case "toe2.r":
                        box = t.gameObject.AddComponent<BoxCollider>();
                        box.size = new Vector3( 0.08f, 0.2f, 0.06f );
                        box.center = new Vector3( 0f, 0.06f, 0f );
                        box.enabled = false;
                        t.gameObject.AddComponent<Rigidbody>();
                        bones.Add( t );
                        break;

                    case "shoulder.l":
                        box = t.gameObject.AddComponent<BoxCollider>();
                        box.size = new Vector3( 0.3f, 0.5f, 0.5f );
                        box.center = new Vector3( -0.075f, 0.3f, 0f );
                        box.enabled = false;
                        t.gameObject.AddComponent<Rigidbody>();
                        bones.Add( t );
                        break;

                    case "shoulder.r":
                        box = t.gameObject.AddComponent<BoxCollider>();
                        box.size = new Vector3( 0.3f, 0.5f, 0.5f );
                        box.center = new Vector3( 0.075f, 0.3f, 0f );
                        box.enabled = false;
                        t.gameObject.AddComponent<Rigidbody>();
                        bones.Add( t );
                        break;

                    case "upperArm1.l":
                        t2 = new GameObject( "ItemParent" + i.ToString() ).transform;
                        t2.parent = t;
                        t2.localPosition = new Vector3( 0f, 0f, 0f );
                        t2.localEulerAngles = new Vector3( 0f, 0f, 0f );
                        t2.localScale = new Vector3( 1.25f, 1.25f, 1.25f );
                        v.SetValue( (CreateNameTransformPair( "UpperArmL", t2 )), i++ );

                        box = t.gameObject.AddComponent<BoxCollider>();
                        box.size = new Vector3( 0.2f, 0.4f, 0.4f );
                        box.center = new Vector3( 0f, 0.15f, 0f );
                        box.enabled = false;
                        t.gameObject.AddComponent<Rigidbody>();
                        bones.Add( t );
                        break;

                    case "upperArm1.r":
                        t2 = new GameObject( "ItemParent" + i.ToString() ).transform;
                        t2.parent = t;
                        t2.localPosition = new Vector3( 0f, 0f, 0f );
                        t2.localEulerAngles = new Vector3( 0f, 0f, 0f );
                        t2.localScale = new Vector3( 1.25f, 1.25f, 1.25f );
                        v.SetValue( (CreateNameTransformPair( "UpperArmR", t2 )), i++ );

                        box = t.gameObject.AddComponent<BoxCollider>();
                        box.size = new Vector3( 0.2f, 0.4f, 0.4f );
                        box.center = new Vector3( 0f, 0.15f, 0f );
                        box.enabled = false;
                        t.gameObject.AddComponent<Rigidbody>();
                        bones.Add( t );
                        break;

                    case "upperArm2.l":
                        box = t.gameObject.AddComponent<BoxCollider>();
                        box.size = new Vector3( 0.12f, 0.3f, 0.3f );
                        box.center = new Vector3( 0.01f, 0.17f, 0f );
                        box.enabled = false;
                        t.gameObject.AddComponent<Rigidbody>();
                        bones.Add( t );
                        break;

                    case "upperArm2.r":
                        box = t.gameObject.AddComponent<BoxCollider>();
                        box.size = new Vector3( 0.12f, 0.3f, 0.3f );
                        box.center = new Vector3( -0.01f, 0.17f, 0f );
                        box.enabled = false;
                        t.gameObject.AddComponent<Rigidbody>();
                        bones.Add( t );
                        break;

                    case "finger1.l":
                        cap = t.gameObject.AddComponent<CapsuleCollider>();
                        cap.direction = 1;
                        cap.radius = 0.035f;
                        cap.height = 0.3f;
                        cap.center = new Vector3( 0f, 0.09f, 0f );
                        t.gameObject.AddComponent<Rigidbody>();
                        bones.Add( t );
                        break;

                    case "finger1.r":
                        t2 = new GameObject( "ItemParent" + i.ToString() ).transform;
                        t2.parent = t;
                        t2.localPosition = new Vector3( 0f, 0.1f, 0f );
                        t2.localEulerAngles = new Vector3( 0f, 0f, 0f );
                        t2.localScale = new Vector3( 1.25f, 1.25f, 1.25f );
                        v.SetValue( (CreateNameTransformPair( "Finger22R", t2 )), i++ );

                        cap = t.gameObject.AddComponent<CapsuleCollider>();
                        cap.direction = 1;
                        cap.radius = 0.035f;
                        cap.height = 0.3f;
                        cap.center = new Vector3( 0f, 0.09f, 0f );
                        t.gameObject.AddComponent<Rigidbody>();
                        bones.Add( t );
                        break;

                    case "finger2.l":
                        cap = t.gameObject.AddComponent<CapsuleCollider>();
                        cap.direction = 1;
                        cap.radius = 0.035f;
                        cap.height = 0.25f;
                        cap.center = new Vector3( 0f, 0.075f, 0f );
                        t.gameObject.AddComponent<Rigidbody>();
                        bones.Add( t );
                        break;

                    case "finger2.r":
                        t2 = new GameObject( "ItemParent" + i.ToString() ).transform;
                        t2.parent = t;
                        t2.localPosition = new Vector3( 0f, 0.1f, 0f );
                        t2.localEulerAngles = new Vector3( 0f, 0f, 0f );
                        t2.localScale = new Vector3( 1.25f, 1.25f, 1.25f );
                        v.SetValue( (CreateNameTransformPair( "Finger42R", t2 )), i++ );

                        cap = t.gameObject.AddComponent<CapsuleCollider>();
                        cap.direction = 1;
                        cap.radius = 0.035f;
                        cap.height = 0.25f;
                        cap.center = new Vector3( 0f, 0.075f, 0f );
                        t.gameObject.AddComponent<Rigidbody>();
                        bones.Add( t );
                        break;

                    case "thumb.l":
                        t2 = new GameObject( "ItemParent" + i.ToString() ).transform;
                        t2.parent = t;
                        t2.localPosition = new Vector3( 0f, -0.15f, -0.1f );
                        t2.localEulerAngles = new Vector3( 0f, 170f, 0f );
                        t2.localScale = new Vector3( 1.25f, 1.25f, 1.25f );
                        v.SetValue( (CreateNameTransformPair( "HandL", t2 )), i++ );

                        cap = t.gameObject.AddComponent<CapsuleCollider>();
                        cap.direction = 1;
                        cap.radius = 0.05f;
                        cap.height = 0.2f;
                        cap.center = new Vector3( -0.005f, 0.085f, 0f );
                        t.gameObject.AddComponent<Rigidbody>();
                        bones.Add( t );
                        break;

                    case "thumb.r":
                        cap = t.gameObject.AddComponent<CapsuleCollider>();
                        cap.direction = 1;
                        cap.radius = 0.05f;
                        cap.height = 0.2f;
                        cap.center = new Vector3( -0.005f, 0.085f, 0f );
                        t.gameObject.AddComponent<Rigidbody>();
                        bones.Add( t );
                        break;


                    case "chest":
                        t2 = new GameObject( "ItemParent" + i.ToString() ).transform;
                        t2.parent = t;
                        t2.localPosition = new Vector3( 0f, 0.5f, 0f );
                        t2.localEulerAngles = new Vector3( 0f, -90f, 0f );
                        t2.localScale = new Vector3( 1.25f, 1.25f, 1.25f );
                        v.SetValue( (CreateNameTransformPair( "Pelvis", t2 )), i++ );
                        break;

                    case "lowerArm.l":
                        t2 = new GameObject( "ItemParent" + i.ToString() ).transform;
                        t2.parent = t;
                        t2.localPosition = new Vector3( 0f, -0.2f, 0f );
                        t2.localEulerAngles = new Vector3( 0f, 90f, 0f );
                        t2.localScale = new Vector3( 1.25f, 1.25f, 1.25f );
                        v.SetValue( (CreateNameTransformPair( "LowerArmL", t2 )), i++ );
                        break;

                    case "lowerArm.r":
                        t2 = new GameObject( "ItemParent" + i.ToString() ).transform;
                        t2.parent = t;
                        t2.localPosition = new Vector3( 0f, -0.2f, 0f );
                        t2.localEulerAngles = new Vector3( 0f, 90f, 0f );
                        t2.localScale = new Vector3( 1.25f, 1.25f, 1.25f );
                        v.SetValue( (CreateNameTransformPair( "LowerArmR", t2 )), i++ );
                        break;

                    case "AncientWispArmature":
                        t2 = new GameObject( "ItemParent" + i.ToString() ).transform;
                        t2.parent = t;
                        t2.localPosition = new Vector3( 0f, 0f, 1.35f );
                        t2.localEulerAngles = new Vector3( 180f, 0f, 0f );
                        t2.localScale = new Vector3( 1.25f, 1.25f, 1.25f );
                        v.SetValue( (CreateNameTransformPair( "Base", t2 )), i++ );
                        break;
                }
            }

            //MethodInfo m = typeof(WispModelModule).GetMethod("ReflCast").MakeGenericMethod((Type)nameTransformPair.MakeArrayType());
            //MethodInfo m2 = typeof(WispModelModule).GetMethod("CastArray").MakeGenericMethod((Type)nameTransformPair);
            f.SetValue( children, v );
            rag.bones = bones.ToArray();
        }

        private static void DoIDRS( GameObject body, Dictionary<Type, Component> dic )
        {
            ItemDisplayRuleSet refidrs = Resources.Load<GameObject>("Prefabs/CharacterBodies/MageBody").GetComponent<ModelLocator>().modelTransform.GetComponent<CharacterModel>().itemDisplayRuleSet;
            //ItemDisplayRuleSet idrs = ScriptableObject.CreateInstance<ItemDisplayRuleSet>();

            //var refEquip = refidrs.GetFieldValue<Item>


            dic.C<ModelLocator>().modelTransform.GetComponent<CharacterModel>().itemDisplayRuleSet = refidrs;
        }

        private static void DoHurtBoxes( GameObject body, Dictionary<Type, Component> dic )
        {
            Transform model = dic.C<ModelLocator>().modelTransform;
            Transform mesh = model.Find("AncientWispMesh");
            Transform refHb = model.Find("Hurtbox");

            MeshCollider meshCol = refHb.gameObject.AddComponent<MeshCollider>();

            MonoBehaviour.DestroyImmediate( refHb.GetComponent<Collider>() );

            meshCol.sharedMesh = mesh.GetComponent<SkinnedMeshRenderer>().sharedMesh;
            meshCol.isTrigger = false;
        }

        private static System.Object CreateNameTransformPair( String name, Transform transform )
        {
            System.Object o = FormatterServices.GetUninitializedObject( nameTransformPair );
            FieldInfo nameField = nameTransformPair.GetField("name", BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
            FieldInfo transformField = nameTransformPair.GetField("transform", BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
            nameField.SetValue( o, name );
            transformField.SetValue( o, transform );
            return o;
        }

        public static T ReflCast<T>( System.Object o ) => (T)o;

        public static T[] CastArray<T>( System.Object o ) => ((Array)o).Cast<T>().ToArray();

        private static CharacterModel.RendererInfo CreateFlameRendererInfo( Renderer r, Material m ) => CreateRendererInfo( r, m, true, UnityEngine.Rendering.ShadowCastingMode.On );

        private static void EditParticles( ParticleSystem ps )
        {

            Color[] colors = new Color[2];
            colors[0] = new Color( 1f, 1f, 1f );
            colors[1] = new Color( 1f, 1f, 1f );

            Single[] alphas = new Single[4];
            alphas[0] = 0f;
            alphas[1] = 0.9f;
            alphas[2] = 0.65f;
            alphas[3] = 0f;

            ParticleSystem.MainModule flameMain1 = ps.main;
            flameMain1.startSize = 10f;
            flameMain1.gravityModifier = -0.35f;
            flameMain1.emitterVelocityMode = ParticleSystemEmitterVelocityMode.Rigidbody;
            flameMain1.cullingMode = ParticleSystemCullingMode.PauseAndCatchup;
            flameMain1.startLifetime = 0.95f;

            ParticleSystem.ShapeModule flameShape1 = ps.shape;
            flameShape1.scale = new Vector3( 0.4f, 0.4f, 0.4f );
            flameShape1.position = new Vector3( 0f, -0.1f, -0.35f );

            ParticleSystem.SizeOverLifetimeModule flameSOL1 = ps.sizeOverLifetime;
            flameSOL1.sizeMultiplier = 0.3f;

            ParticleSystem.ColorOverLifetimeModule flameCOL1 = ps.colorOverLifetime;
            GradientColorKey[] newColorKeys = new GradientColorKey[flameCOL1.color.gradient.colorKeys.Length];
            GradientAlphaKey[] newAlphaKeys = new GradientAlphaKey[flameCOL1.color.gradient.alphaKeys.Length];
            Gradient newGrad = new Gradient();
            ParticleSystem.MinMaxGradient newGradP2 = new ParticleSystem.MinMaxGradient();
            for( Int32 i = 0; i < flameCOL1.color.gradient.colorKeys.Length; i++ )
            {
                newColorKeys[i].time = i;
                if( colors.Length == newColorKeys.Length )
                {
                    newColorKeys[i].color = colors[i];
                } else
                {
                    newColorKeys[i].color = flameCOL1.color.gradient.colorKeys[i].color;
                }
            }
            for( Int32 i = 0; i < flameCOL1.color.gradient.alphaKeys.Length; i++ )
            {
                newAlphaKeys[i].time = flameCOL1.color.gradient.alphaKeys[i].time;
                if( alphas.Length == newAlphaKeys.Length )
                {
                    newAlphaKeys[i].alpha = alphas[i];
                } else
                {
                    newAlphaKeys[i].alpha = flameCOL1.color.gradient.alphaKeys[i].alpha;
                }
            }
            newGrad.SetKeys( newColorKeys, newAlphaKeys );

            newGradP2.gradient = newGrad;
            newGradP2.mode = ParticleSystemGradientMode.Gradient;

            flameCOL1.color = newGradP2;

            ParticleSystem.EmissionModule flameEmis1 = ps.emission;
            flameEmis1.rateOverDistance = new ParticleSystem.MinMaxCurve
            {
                constant = 0f,
                mode = ParticleSystemCurveMode.Constant
            };
            flameEmis1.rateOverTime = new ParticleSystem.MinMaxCurve
            {
                constant = 100f,
                mode = ParticleSystemCurveMode.Constant
            };

        }

        public struct PSCont
        {
            public ParticleSystem ps;
            public ParticleSystemRenderer psr;
            public FlamePSInfo info;
        }

        public struct FlamePSInfo
        {
            public Int32 matIndex;

            public Single startSpeed;
            public Single startSize;
            public Single gravity;
            public Single rate;
            public Single radius;

            public Vector3 position;
            public Vector3 rotation;
            public Vector3 scale;
        }

        public static Dictionary<String, FlamePSInfo> CreateFlameDictionary()
        {
            Dictionary<String, FlamePSInfo> flames = new Dictionary<String, FlamePSInfo>();
            flames.Add( "Head", new FlamePSInfo
            {
                matIndex = 0,
                startSpeed = 0.5f,
                startSize = 1f,
                gravity = -0.15f,
                rate = 10f,
                radius = 1f,
                position = new Vector3( 0f, 0.15f, 0f ),
                rotation = new Vector3( 180f, 0f, 0f ),
                scale = new Vector3( 0.1f, 0.1f, 0.1f )
            } );

            flames.Add( "ChestCannon1", new FlamePSInfo
            {
                matIndex = 0,
                startSpeed = 1f,
                startSize = 1f,
                gravity = -0.2f,
                rate = 10f,
                radius = 1f,
                position = new Vector3( 0f, 1f, 0f ),
                rotation = new Vector3( 90f, 0f, 0f ),
                scale = new Vector3( 0.25f, 0.2f, 0.6f )
            } );
            flames.Add( "ChestCannon2", new FlamePSInfo
            {
                matIndex = 0,
                startSpeed = 1f,
                startSize = 1f,
                gravity = -0.2f,
                rate = 10f,
                radius = 1f,
                position = new Vector3( 0f, 1f, 0f ),
                rotation = new Vector3( 90f, 0f, 0f ),
                scale = new Vector3( 0.25f, 0.2f, 0.6f )
            } );

            flames.Add( "upperArm1.l", new FlamePSInfo
            {
                matIndex = 0,
                startSpeed = 1f,
                startSize = 1.5f,
                gravity = -0.2f,
                rate = 8f,
                radius = 1f,
                position = new Vector3( 0f, 0.4f, 0f ),
                rotation = new Vector3( 90f, 0f, 0f ),
                scale = new Vector3( 0.15f, 0.15f, 0.5f )
            } );
            flames.Add( "upperArm1.r", new FlamePSInfo
            {
                matIndex = 0,
                startSpeed = 1f,
                startSize = 1.5f,
                gravity = -0.2f,
                rate = 8f,
                radius = 1f,
                position = new Vector3( 0f, 0.4f, 0f ),
                rotation = new Vector3( 90f, 0f, 0f ),
                scale = new Vector3( 0.15f, 0.15f, 0.5f )
            } );

            flames.Add( "MuzzleLeft", new FlamePSInfo
            {
                matIndex = 0,
                startSpeed = 1f,
                startSize = 1.5f,
                gravity = -0.2f,
                rate = 6f,
                radius = 1f,
                position = new Vector3( 0f, 0f, 0.1f ),
                rotation = new Vector3( 180f, 0f, 0f ),
                scale = new Vector3( 0.1f, 0.1f, 0.5f )
            } );
            flames.Add( "MuzzleRight", new FlamePSInfo
            {
                matIndex = 0,
                startSpeed = 1f,
                startSize = 1.5f,
                gravity = -0.2f,
                rate = 6f,
                radius = 1f,
                position = new Vector3( 0f, 0f, 0f ),
                rotation = new Vector3( 180f, 0f, 0f ),
                scale = new Vector3( 0.1f, 0.1f, 0.5f )
            } );

            flames.Add( "calf.l", new FlamePSInfo
            {
                matIndex = 0,
                startSpeed = 1f,
                startSize = 1.5f,
                gravity = -0.3f,
                rate = 10f,
                radius = 1f,
                position = new Vector3( 0f, 0.6f, 0f ),
                rotation = new Vector3( 90f, 0f, 0f ),
                scale = new Vector3( 0.1f, 0.1f, 0.5f )
            } );
            flames.Add( "calf.r", new FlamePSInfo
            {
                matIndex = 0,
                startSpeed = 1f,
                startSize = 1.5f,
                gravity = -0.3f,
                rate = 10f,
                radius = 1f,
                position = new Vector3( 0f, 0.6f, 0f ),
                rotation = new Vector3( 90f, 0f, 0f ),
                scale = new Vector3( 0.1f, 0.1f, 0.5f )
            } );

            return flames;
        }

        private static void SetupFlameParticleSystem( this ParticleSystem ps, Int32 skinIndex, FlamePSInfo psi )
        {
            ParticleSystem.MainModule main = ps.main;
            main.duration = 1f;
            main.loop = true;
            main.prewarm = false;
            main.startDelay = 0f;
            main.startLifetime = new ParticleSystem.MinMaxCurve
            {
                mode = ParticleSystemCurveMode.Constant,
                constant = 0.65f
            };
            main.startSpeed = new ParticleSystem.MinMaxCurve
            {
                mode = ParticleSystemCurveMode.Constant,
                constant = psi.startSpeed
            };
            main.startSize3D = false;
            main.startSize = new ParticleSystem.MinMaxCurve
            {
                mode = ParticleSystemCurveMode.Constant,
                constant = psi.startSize
            };
            main.startRotation3D = false;
            main.startRotation = new ParticleSystem.MinMaxCurve
            {
                mode = ParticleSystemCurveMode.TwoConstants,
                constantMin = 0f,
                constantMax = 360f
            };
            main.flipRotation = 0f;
            main.startColor = new ParticleSystem.MinMaxGradient
            {
                mode = ParticleSystemGradientMode.Color,
                color = new Color( 1f, 1f, 1f, 1f )
            };
            main.gravityModifier = new ParticleSystem.MinMaxCurve
            {
                mode = ParticleSystemCurveMode.Constant,
                constant = psi.gravity
            };
            main.simulationSpace = ParticleSystemSimulationSpace.Local;
            main.simulationSpeed = 1f;
            main.useUnscaledTime = false;
            main.scalingMode = ParticleSystemScalingMode.Local;
            main.playOnAwake = true;
            main.emitterVelocityMode = ParticleSystemEmitterVelocityMode.Transform;
            main.maxParticles = 1000;
            main.stopAction = ParticleSystemStopAction.None;
            main.cullingMode = ParticleSystemCullingMode.AlwaysSimulate;
            main.ringBufferMode = ParticleSystemRingBufferMode.Disabled;

            ParticleSystem.EmissionModule emission = ps.emission;
            emission.enabled = true;
            emission.rateOverTime = new ParticleSystem.MinMaxCurve
            {
                mode = ParticleSystemCurveMode.Constant,
                constant = 10f
            };
            emission.rateOverDistance = new ParticleSystem.MinMaxCurve
            {
                mode = ParticleSystemCurveMode.Constant,
                constant = 0f
            };
            emission.rateOverDistanceMultiplier = 0f;
            emission.rateOverTimeMultiplier = psi.rate;

            ParticleSystem.ShapeModule shape = ps.shape;
            shape.enabled = true;
            shape.shapeType = ParticleSystemShapeType.Cone;
            shape.angle = 38.26f;
            shape.radius = psi.radius;
            shape.radiusThickness = 1f;
            shape.arc = 360f;
            shape.arcMode = ParticleSystemShapeMultiModeValue.Random;
            shape.arcSpread = 0f;
            shape.position = psi.position;
            shape.rotation = psi.rotation;
            shape.scale = psi.scale;
            shape.alignToDirection = false;

            ParticleSystem.VelocityOverLifetimeModule velOverLife = ps.velocityOverLifetime;
            velOverLife.enabled = false;

            ParticleSystem.LimitVelocityOverLifetimeModule limVelOverLife = ps.limitVelocityOverLifetime;
            limVelOverLife.enabled = false;

            ParticleSystem.InheritVelocityModule inheritVel = ps.inheritVelocity;
            inheritVel.enabled = false;

            ParticleSystem.ForceOverLifetimeModule forceOverLife = ps.forceOverLifetime;
            forceOverLife.enabled = false;

            ParticleSystem.ColorOverLifetimeModule colorOverLife = ps.colorOverLifetime;
            colorOverLife.enabled = true;
            colorOverLife.color = new ParticleSystem.MinMaxGradient
            {
                mode = ParticleSystemGradientMode.Gradient,
                gradient = new Gradient
                {
                    mode = GradientMode.Blend,
                    colorKeys = new GradientColorKey[1]
                    {
                        new GradientColorKey( new Color( 1f, 1f, 1f ) , 0f )
                    },
                    alphaKeys = new GradientAlphaKey[4]
                    {
                        new GradientAlphaKey( 0f, 0f ),
                        new GradientAlphaKey( 0.9f, 0.1f ),
                        new GradientAlphaKey(0.6f, 0.6f ),
                        new GradientAlphaKey( 0f, 1f )
                    }
                }
            };

            ParticleSystem.ColorBySpeedModule colorBySpeed = ps.colorBySpeed;
            colorBySpeed.enabled = false;

            ParticleSystem.SizeOverLifetimeModule sizeOverLife = ps.sizeOverLifetime;
            sizeOverLife.enabled = true;
            sizeOverLife.size = new ParticleSystem.MinMaxCurve
            {
                mode = ParticleSystemCurveMode.Curve,
                curve = new AnimationCurve
                {
                    postWrapMode = WrapMode.Clamp,
                    preWrapMode = WrapMode.Clamp,
                    keys = new Keyframe[3]
                    {
                        new Keyframe( 0f, 0.2f),
                        new Keyframe( 0.47f, 0.71f),
                        new Keyframe( 1f, 0.025f )
                    }
                }
            };
            sizeOverLife.sizeMultiplier = 1f;

            ParticleSystem.SizeBySpeedModule sizeBySpeed = ps.sizeBySpeed;
            sizeBySpeed.enabled = false;

            ParticleSystem.RotationOverLifetimeModule rotOverLife = ps.rotationOverLifetime;
            rotOverLife.enabled = true;
            rotOverLife.separateAxes = false;
            rotOverLife.z = new ParticleSystem.MinMaxCurve
            {
                mode = ParticleSystemCurveMode.Constant,
                constant = 3f
            };

            ParticleSystem.RotationBySpeedModule rotBySpeed = ps.rotationBySpeed;
            rotBySpeed.enabled = false;

            ParticleSystem.ExternalForcesModule extForce = ps.externalForces;
            extForce.enabled = false;

            ParticleSystem.NoiseModule noise = ps.noise;
            noise.enabled = false;

            ParticleSystem.CollisionModule col = ps.collision;
            col.enabled = false;

            ParticleSystem.TriggerModule trig = ps.trigger;
            trig.enabled = false;

            ParticleSystem.SubEmittersModule subEmit = ps.subEmitters;
            subEmit.enabled = false;

            ParticleSystem.TextureSheetAnimationModule texSheet = ps.textureSheetAnimation;
            texSheet.enabled = false;

            ParticleSystem.LightsModule light = ps.lights;
            light.enabled = false;

            ParticleSystem.TrailModule trails = ps.trails;
            trails.enabled = false;

            ParticleSystem.CustomDataModule custData = ps.customData;
            custData.enabled = false;
        }

        private static T C<T>( this Dictionary<Type, Component> dic ) where T : Component => dic[typeof( T )] as T;

        private static void DebugMaterialInfo( Material m )
        {
            Debug.Log( "Material name: " + m.name );
            String[] s = m.shaderKeywords;
            Debug.Log( "Shader keywords" );
            for( Int32 i = 0; i < s.Length; i++ )
            {
                Debug.Log( s[i] );
            }

            Debug.Log( "Shader name: " + m.shader.name );

            Debug.Log( "Texture Properties" );
            String[] s2 = m.GetTexturePropertyNames();
            for( Int32 i = 0; i < s2.Length; i++ )
            {
                Debug.Log( s2[i] + " : " + m.GetTexture( s2[i] ) );
            }
        }
    }
}
*/