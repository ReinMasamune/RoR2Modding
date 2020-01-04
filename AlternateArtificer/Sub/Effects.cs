namespace AlternativeArtificer
{
    using AlternativeArtificer.States.Main;
    using BepInEx;
    using R2API;
    using RoR2;
    using RoR2.Projectile;
    using System;
    using UnityEngine;

    public partial class Main
    {
        private void DoEffects()
        {
            CreateLightningPreFire();
        }

        private void CreateLightningPreFire()
        {
            AltArtiPassive.lightningPreFireEffect = new GameObject[5];
            for( Int32 i = 0; i < 5; i++ )
            {
                GameObject effect = this.CreateLightningSwordGhost(i);
                Destroy( effect.GetComponent<ProjectileGhostController>() );
                AltArtiPassive.lightningPreFireEffect[i] = effect;
            }

        }

        private GameObject CreateLightningSwordGhost( Int32 meshInd )
        {
            GameObject obj = Resources.Load<GameObject>("Prefabs/ProjectileGhosts/ElectricWormSeekerGhost" ).InstantiateClone( "LightningSwordGhost", false );

            GameObject model = obj.transform.Find( "mdlRock" ).gameObject;
            GameObject trail = obj.transform.Find("Trail").gameObject;
            TrailRenderer trailRen = trail.GetComponent<TrailRenderer>();
            trail.SetActive( false );
            model.SetActive( true );
            Destroy( model.GetComponent<RotateObject>() );

            Color color1 = trailRen.startColor;
            Color color2 = trailRen.endColor;

            this.DoMesh( model, meshInd, color1, color2 );



            Material mat = Instantiate<Material>(trailRen.material);
            model.GetComponent<MeshRenderer>().material = trailRen.material;

            return obj;
        }

        private void DoMesh( GameObject model, Int32 meshInd, Color color1, Color color2 )
        {
            Mesh mesh;
            Vector2 baseUV;
            Color[] colors;
            Vector2[] uvs;
            switch( meshInd )
            {
                default:
                    Debug.LogError( "Mesh index for sword out of range" );
                    break;
                case 0:
                    model.transform.localScale = new Vector3( 0.5f, 0.5f, 1.5f );

                    mesh = Instantiate<Mesh>(Resources.Load<GameObject>("Prefabs/PickupModels/PickupTriTip").transform.Find("mdlTriTip").GetComponent<MeshFilter>().sharedMesh);
                    model.GetComponent<MeshFilter>().sharedMesh = mesh;
                    baseUV = new Vector2( 0.5f, 0.5f );
                    uvs = mesh.uv;
                    colors = new Color[uvs.Length];

                    for( Int32 i = 0; i < uvs.Length; i++ )
                    {
                        Vector2 uv = uvs[i];
                        Single t = Mathf.Pow(uv.x - 0.5f, 2) + Mathf.Pow(uv.y - 0.5f, 2);
                        colors[i] = Color.Lerp( color1, color2, Mathf.Sqrt( t ) );
                        uvs[i] = baseUV;
                        //colors[i] = 
                    }
                    mesh.uv = uvs;
                    mesh.colors = colors;
                    break;

                case 3:
                    model.transform.localScale = new Vector3( 0.4f, 0.4f, 1.25f );
                    model.transform.eulerAngles = new Vector3( 0f, 180f, 90f );
                    mesh = Instantiate<Mesh>(Resources.Load<GameObject>("Prefabs/CharacterBodies/ClayBody").transform.Find("ModelBase/mdlClay/ClaymanArmature/ROOT/base/stomach/chest/clavicle.r/upper_arm.r/lower_arm.r/hand.r/sword/ClaymanSwordMesh").GetComponent<MeshFilter>().sharedMesh);
                    model.GetComponent<MeshFilter>().sharedMesh = mesh;
                    baseUV = new Vector2( 0.5f, 0.5f );
                    uvs = mesh.uv;
                    colors = new Color[uvs.Length];

                    for( Int32 i = 0; i < uvs.Length; i++ )
                    {
                        Vector2 uv = uvs[i];
                        Single t = Mathf.Pow(uv.x - 0.5f, 2) + Mathf.Pow(uv.y - 0.5f, 2);
                        colors[i] = Color.Lerp( color1, color2, Mathf.Sqrt( t ) );
                        uvs[i] = baseUV;
                        //colors[i] = 
                    }
                    mesh.uv = uvs;
                    mesh.colors = colors;
                    break;
                case 2:
                    model.transform.localScale = new Vector3( 0.4f, 0.4f, 1.25f );
                    model.transform.eulerAngles = new Vector3( 0f, 180f, 0f );
                    mesh = Instantiate<Mesh>(Resources.Load<GameObject>("Prefabs/PickupModels/PickupDagger").transform.Find("mdlDagger").GetComponent<MeshFilter>().sharedMesh);
                    model.GetComponent<MeshFilter>().sharedMesh = mesh;
                    baseUV = new Vector2( 0.5f, 0.5f );
                    uvs = mesh.uv;
                    colors = new Color[uvs.Length];

                    for( Int32 i = 0; i < uvs.Length; i++ )
                    {
                        Vector2 uv = uvs[i];
                        Single t = Mathf.Pow(uv.x - 0.5f, 2) + Mathf.Pow(uv.y - 0.5f, 2);
                        colors[i] = Color.Lerp( color1, color2, Mathf.Sqrt( t ) );
                        uvs[i] = baseUV;
                        //colors[i] = 
                    }
                    mesh.uv = uvs;
                    mesh.colors = colors;
                    break;
                
                case 1:
                    model.transform.localScale = new Vector3( 1f, 1f, 1f );
                    mesh = Instantiate<Mesh>(Resources.Load<GameObject>("Prefabs/CharacterBodies/MercBody").transform.Find("ModelBase/mdlMerc/MercSwordMesh").GetComponent<SkinnedMeshRenderer>().sharedMesh);
                    model.GetComponent<MeshFilter>().sharedMesh = mesh;
                    baseUV = new Vector2( 0.5f, 0.5f );
                    uvs = mesh.uv;
                    colors = new Color[uvs.Length];

                    for( Int32 i = 0; i < uvs.Length; i++ )
                    {
                        Vector2 uv = uvs[i];
                        Single t = Mathf.Pow(uv.x - 0.5f, 2) + Mathf.Pow(uv.y - 0.5f, 2);
                        colors[i] = Color.Lerp( color1, color2, Mathf.Sqrt( t ) );
                        uvs[i] = baseUV;
                        //colors[i] = 
                    }
                    mesh.uv = uvs;
                    mesh.colors = colors;
                    break;
                
                case 4:
                    model.transform.localScale = new Vector3( 0.06f, 0.06f, 0.15f );
                    model.transform.eulerAngles = new Vector3( 0f, 180f, 0f );
                    mesh = Instantiate<Mesh>(Resources.Load<GameObject>("Prefabs/CharacterBodies/TitanGoldBody").transform.Find("ModelBase/mdlTitan/TitanArmature/ROOT/base/stomach/chest/upper_arm.r/lower_arm.r/hand.r/RightFist/Sword").GetComponent<MeshFilter>().sharedMesh);
                    model.GetComponent<MeshFilter>().sharedMesh = mesh;
                    baseUV = new Vector2( 0.5f, 0.5f );
                    uvs = mesh.uv;
                    colors = new Color[uvs.Length];

                    for( Int32 i = 0; i < uvs.Length; i++ )
                    {
                        Vector2 uv = uvs[i];
                        Single t = Mathf.Pow(uv.x - 0.5f, 2) + Mathf.Pow(uv.y - 0.5f, 2);
                        colors[i] = Color.Lerp( color1, color2, Mathf.Sqrt( t ) );
                        uvs[i] = baseUV;
                        //colors[i] = 
                    }
                    mesh.uv = uvs;
                    mesh.colors = colors;
                    break;
            }
        }

        private GameObject CreateIceDelayEffect()
        {
            CreateIceBombTex();

            GameObject obj = Resources.Load<GameObject>( "Prefabs/Effects/AffixWhiteDelayEffect" ).InstantiateClone( "iceDelay" , false );
            var sphere = obj.transform.Find("Nova Sphere").GetComponent<ParticleSystemRenderer>();
            Material mat = UnityEngine.Object.Instantiate<Material>(sphere.material);
            mat.SetTexture( "_RemapTex", this.iceBombTex );
            sphere.material = mat;


            EffectAPI.AddEffect( obj );
            return obj;
        }

        private GameObject CreateIceExplosionEffect()
        {
            CreateIceBombTex();

            GameObject obj = Resources.Load<GameObject>("Prefabs/Effects/ImpactEffects/AffixWhiteExplosion").InstantiateClone( "IceExplosion", false );
            var sphere = obj.transform.Find("Nova Sphere").GetComponent<ParticleSystemRenderer>();
            Material mat = UnityEngine.Object.Instantiate<Material>(sphere.material);
            mat.SetTexture( "_RemapTex", this.iceBombTex );
            sphere.material = mat;

            EffectAPI.AddEffect( obj );
            return obj;
        }

        private Texture2D iceBombTex;
        private void CreateIceBombTex()
        {
            if( this.iceBombTex != null ) return;
            Gradient iceGrad = new Gradient
            {
                mode = GradientMode.Blend,
                alphaKeys = new GradientAlphaKey[8]
                {
                    new GradientAlphaKey( 0f, 0f ),
                    new GradientAlphaKey( 0f, 0.14f ),
                    new GradientAlphaKey( 0.22f, 0.46f ),
                    new GradientAlphaKey( 0.22f, 0.61f),
                    new GradientAlphaKey( 0.72f, 0.63f ),
                    new GradientAlphaKey( 0.72f, 0.8f ),
                    new GradientAlphaKey( 0.87f, 0.81f ),
                    new GradientAlphaKey( 0.87f, 1f )
                },
                colorKeys = new GradientColorKey[8]
                {
                    new GradientColorKey( new Color( 0f, 0f, 0f ), 0f ),
                    new GradientColorKey( new Color( 0f, 0f, 0f ), 0.14f ),
                    new GradientColorKey( new Color( 0.179f, 0.278f, 0.250f ), 0.46f ),
                    new GradientColorKey( new Color( 0.179f, 0.278f, 0.250f ), 0.61f ),
                    new GradientColorKey( new Color( 0.612f, 0.906f, 0.815f ), 0.63f ),
                    new GradientColorKey( new Color( 0.612f, 0.906f, 0.815f ), 0.8f ),
                    new GradientColorKey( new Color( 0.776f, 0.957f, 0.861f ), 0.81f ),
                    new GradientColorKey( new Color( 0.776f, 0.957f, 0.861f ), 1f )
                }
            };

            this.iceBombTex = CreateNewRampTex( iceGrad );
        }

        private static Texture2D CreateNewRampTex( Gradient grad )
        {
            Texture2D tex = new Texture2D(256, 8, TextureFormat.RGBA32, false);

            Color tempC;
            Color[] tempCs = new Color[8];

            for( Int32 i = 0; i < 256; i++ )
            {
                tempC = grad.Evaluate( i / 255f );
                for( Int32 j = 0; j < 8; j++ )
                {
                    tempCs[j] = tempC;
                }

                tex.SetPixels( i, 0, 1, 8, tempCs );
            }
            tex.wrapMode = TextureWrapMode.Clamp;
            tex.Apply();
            return tex;
        }
    }
}
