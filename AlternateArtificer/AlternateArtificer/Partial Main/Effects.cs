using System;
using System;
using System.Collections.Generic;
using System.Text;
using BepInEx;
using ReinCore;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace Rein.AlternateArtificer
{
    internal partial class Main
    {
        partial void Effects()
        {
            this.awake += this.Main_awake4;
        }

        // TODO: REWRITE
        private void Main_awake4()
        {
            AltArtiPassive.lightningPreFireEffect = new GameObject[5];
            for( Int32 i = 0; i < 5; i++ )
            {
                GameObject effect = this.CreateLightningSwordGhost(i);
                Destroy( effect.GetComponent<ProjectileGhostController>() );
                AltArtiPassive.lightningPreFireEffect[i] = effect;
            }
        }

        // TODO: REWRITE
        private GameObject CreateLightningSwordGhost( Int32 meshInd )
        {
            GameObject obj = Resources.Load<GameObject>("Prefabs/ProjectileGhosts/ElectricWormSeekerGhost" ).ClonePrefab( "LightningSwordGhost", false );

            GameObject model = obj.transform.Find( "mdlRock" ).gameObject;
            GameObject trail = obj.transform.Find("Trail").gameObject;
            TrailRenderer trailRen = trail.GetComponent<TrailRenderer>();
            trail.SetActive( true );
            model.SetActive( true );
            Destroy( model.GetComponent<RotateObject>() );

            Color color1 = trailRen.startColor;
            Color color2 = trailRen.endColor;
            this.DoMesh( model, meshInd, color1, color2 );
            model.GetComponent<MeshRenderer>().material = swordMaterial;
            return obj;
        }

        // TODO: REWRITE
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

                    mesh = Instantiate<Mesh>( Resources.Load<GameObject>( "Prefabs/PickupModels/PickupTriTip" ).transform.Find( "mdlTriTip" ).GetComponent<MeshFilter>().sharedMesh );
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
                    mesh = Instantiate<Mesh>( Resources.Load<GameObject>( "Prefabs/CharacterBodies/ClayBody" ).transform.Find( "ModelBase/mdlClay/ClaymanArmature/ROOT/base/stomach/chest/clavicle.r/upper_arm.r/lower_arm.r/hand.r/sword/ClaymanSwordMesh" ).GetComponent<MeshFilter>().sharedMesh );
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
                    mesh = Instantiate<Mesh>( Resources.Load<GameObject>( "Prefabs/PickupModels/PickupDagger" ).transform.Find( "mdlDagger" ).GetComponent<MeshFilter>().sharedMesh );
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
                    mesh = Instantiate<Mesh>( Resources.Load<GameObject>( "Prefabs/CharacterBodies/MercBody" ).transform.Find( "ModelBase/mdlMerc/MercSwordMesh" ).GetComponent<SkinnedMeshRenderer>().sharedMesh );
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
                    mesh = Instantiate<Mesh>( Resources.Load<GameObject>( "Prefabs/CharacterBodies/TitanGoldBody" ).transform.Find( "ModelBase/mdlTitan/TitanArmature/ROOT/base/stomach/chest/upper_arm.r/lower_arm.r/hand.r/RightFist/Sword" ).GetComponent<MeshFilter>().sharedMesh );
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
            GameObject obj = Resources.Load<GameObject>( "Prefabs/Effects/AffixWhiteDelayEffect" ).ClonePrefab( "iceDelay" , false );
            var sphere = obj.transform.Find("Nova Sphere").GetComponent<ParticleSystemRenderer>();
            Material mat = UnityEngine.Object.Instantiate<Material>(sphere.material);
            mat.SetTexture( "_RemapTex", iceRamp);
            sphere.material = mat;


            EffectsCore.AddEffect( obj );
            return obj;
        }

        // TODO: REWRITE
        private GameObject CreateIceExplosionEffect()
        {

            GameObject obj = Resources.Load<GameObject>("Prefabs/Effects/ImpactEffects/AffixWhiteExplosion").ClonePrefab( "IceExplosion", false );
            var sphere = obj.transform.Find("Nova Sphere").GetComponent<ParticleSystemRenderer>();
            Material mat = UnityEngine.Object.Instantiate<Material>(sphere.material);
            mat.SetTexture( "_RemapTex", iceRamp );
            sphere.material = mat;

            EffectsCore.AddEffect( obj );
            return obj;
        }
    }
}

// TODO: Sword charge effect
// TODO: Sword projectile ghost
