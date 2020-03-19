#if MATEDITOR
using EntityStates;
using RogueWispPlugin.Helpers;
using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace RogueWispPlugin
{
    internal partial class Main
    {
        partial void SetupMatEditor()
        {
            this.FirstFrame += this.CreateMatEditorPrefab;
            //this.Load += this.CreateMatGremlin;
            this.Frame += this.Main_Frame;
        }

        private List<MonoBehaviour> tempDisabled = new List<MonoBehaviour>();
        private Dictionary<Light,Color> tempLights = new Dictionary<Light, Color>();

        private void Main_Frame()
        {
            if( Input.GetKeyDown( KeyCode.F5 ) )
            {
                if( MaterialEditor.instance != null )
                {
                    MaterialEditor.instance.enabled = !MaterialEditor.instance.enabled;
                }



                /*
                if( !MatPrefabExists() )
                {
                    var camTransform = Camera.main.transform.root;
                    if( camTransform )
                    {
                        matPrefabInstance = UnityEngine.Object.Instantiate<GameObject>( matPrefab );
                        matPrefabInstance.transform.localScale = new Vector3( 0.9f, 0.9f, 0.9f );
                        matPrefabInstance.transform.parent = camTransform;
                        matPrefabInstance.transform.localPosition = new Vector3( -0.5f, -1.4f, 2f );
                        //matPrefabInstance.transform.localRotation = Quaternion.identity;
                        matPrefabInstance.transform.localEulerAngles = new Vector3(0f, 180f, 0f );

                        foreach(var comp in camTransform.GetComponentsInChildren<BlurOptimized>() )
                        {
                            Main.LogW( comp.name );
                            if( comp.enabled )
                            {
                                this.tempDisabled.Add( comp );
                                comp.enabled = false;
                            }

                        }

                        foreach( var comp in camTransform.GetComponentsInChildren<PostProcessVolume>() )
                        {
                            if( comp.enabled )
                            {
                                Main.LogW( comp.name );
                                if( comp.name == "GlobalPostProcessVolume, Base" )
                                {
                                    Main.LogE( comp.name + "Disabled" );
                                    this.tempDisabled.Add( comp );
                                    comp.enabled = false;
                                }
                            }
                        }

                        foreach( var obj in FindObjectsOfType<Light>() )
                        {
                            this.tempLights.Add( obj, obj.color );
                            obj.color = Color.white;
                        }

                        //camTransform.Find( "UI Camera" ).gameObject.SetActive( false );
                        
                    } else
                    {
                        Main.LogE( "Failed to create material prefab, camera transform not found." );
                    }
                } else
                {
                    Destroy( matPrefabInstance );
                    foreach( var comp in this.tempDisabled )
                    {
                        if( comp != null ) comp.enabled = true;
                    }
                    this.tempDisabled.Clear();

                    foreach( var kv in this.tempLights )
                    {
                        if( kv.Key != null )
                        {
                            kv.Key.color = kv.Value;
                        }
                    }
                    this.tempLights.Clear();
                }
                */
            }

        }

        //internal GameObject matGremlin;
        //private void CreateMatGremlin()
        //{
        //    this.matGremlin = Resources.Load<GameObject>( "Prefabs/CharacterBodies/LemurianBody" ).InstantiateClone( "MatGremlin" );
        //    BodyCatalog.getAdditionalEntries += ( list ) => list.Add( this.matGremlin );
        //    SurvivorAPI.AddSurvivor( new SurvivorDef
        //    {
        //        bodyPrefab = this.matGremlin,
        //        descriptionToken = "",
        //        name = "",
        //        displayPrefab = this.matGremlin.transform.Find( "ModelBase" ).gameObject,
        //        primaryColor = Color.clear,
        //        unlockableName = "",
        //    } );
        //    var charBody = this.matGremlin.GetComponent<CharacterBody>();
        //    charBody.baseArmor = 9001f;
        //    charBody.baseMaxHealth = 9001f;
        //    charBody.baseMaxShield = 9001f;
        //    charBody.baseRegen = 9001f;
        //    charBody.preferredInitialStateType = new EntityStates.SerializableEntityStateType( typeof( EntityStates.LemurianMonster.SpawnState ) );


        //    var pDef = ScriptableObject.CreateInstance<SkillDef>();
        //    pDef.activationState = new EntityStates.SerializableEntityStateType( typeof( SpawnMatPrefab ) );
        //    pDef.activationStateMachineName = "Weapon";
        //    pDef.mustKeyPress = true;
        //    pDef.baseMaxStock = 1;
        //    pDef.baseRechargeInterval = 3f;
        //    pDef.beginSkillCooldownOnSkillEnd = true;
        //    var pFamily = ScriptableObject.CreateInstance<SkillFamily>();
        //    pFamily.variants = new SkillFamily.Variant[]
        //    {
        //        new SkillFamily.Variant
        //        {
        //            skillDef = pDef,
        //            unlockableName = "",
        //            viewableNode = new ViewablesCatalog.Node( "stuff", false ),
        //        },
        //    };
        //    var loc = this.matGremlin.GetComponent<SkillLocator>();
        //    loc.primary.SetFieldValue( "_skillFamily", pFamily );
        //    LoadoutAPI.AddSkill( typeof( SpawnMatPrefab ) );
        //    LoadoutAPI.AddSkillDef( pDef );
        //    LoadoutAPI.AddSkillFamily( pFamily );
        //}

        internal static Boolean MatPrefabExists()
        {
            return matPrefabInstance != null;
        }

        internal static GameObject matPrefab;
        internal static GameObject matPrefabInstance;

        private void CreateMatEditorPrefab()
        {
            /*
            if( matPrefab != null )
            {
                matPrefab = null;
            }
            if( matPrefabInstance != null )
            {
                Destroy( matPrefabInstance );
                matPrefabInstance = null;
            }
            */
            this.RW_body.GetComponent<ModelLocator>().modelBaseTransform.gameObject.AddComponent<MaterialEditor>();
            /*
            matPrefab.AddComponent<MaterialEditor>();

            var obj2 = GameObject.CreatePrimitive( PrimitiveType.Cylinder );
            var col = obj2.GetComponent<Collider>();
            if( col != null ) Destroy( col );
            var rb = obj2.GetComponent<Rigidbody>();
            if( rb != null ) Destroy( rb );

            obj2.transform.parent = matPrefab.transform;

            obj2.transform.localPosition = new Vector3( 5f, 0f, 0f );
            obj2.transform.localScale = new Vector3( 3f, 3f, 3f );

            obj2.GetComponent<MeshRenderer>().material = Main.fireMaterials[0][0];
            */
        }

        internal class SpawnMatPrefab : BaseState
        {
            private GameObject aimEffect;
            public override void OnEnter()
            {
                base.OnEnter();
                if( Main.MatPrefabExists() )
                {
                    base.outer.SetNextStateToMain();
                } else
                {
                    this.aimEffect = GameObject.CreatePrimitive( PrimitiveType.Sphere );
                }
            }

            public override void FixedUpdate()
            {
                base.FixedUpdate();
                if( this.aimEffect )
                {
                    var ray = base.GetAimRay();
                    RaycastHit rh;
                    if( Util.CharacterRaycast( base.gameObject, ray, out rh, 100f, LayerIndex.world.mask, QueryTriggerInteraction.UseGlobal ) )
                    {
                        this.aimEffect.transform.position = rh.point;
                    } else
                    {
                        this.aimEffect.transform.position = ray.GetPoint( 100f );
                    }
                }

                if( base.isAuthority && base.inputBank && !base.inputBank.skill1.down )
                {
                    base.outer.SetNextStateToMain();
                }
            }
            
            public override void OnExit()
            {
                if( this.aimEffect )
                {
                    Main.matPrefabInstance = UnityEngine.Object.Instantiate<GameObject>( Main.matPrefab, this.aimEffect.transform.position, this.aimEffect.transform.rotation );
                    UnityEngine.Object.Destroy( this.aimEffect );
                }
                base.OnExit();
            }
        }
    }
}
#endif