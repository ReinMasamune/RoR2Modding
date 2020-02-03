#if MATEDITOR
using EntityStates;
using R2API;
using R2API.Utils;
using RoR2;
using RoR2.Skills;
using System;
using UnityEngine;

namespace RogueWispPlugin
{
    internal partial class Main
    {
        partial void SetupMatEditor()
        {
            this.Load += this.CreateMatEditorPrefab;
            this.Load += this.CreateMatGremlin;
        }

        internal GameObject matGremlin;
        private void CreateMatGremlin()
        {
            this.matGremlin = Resources.Load<GameObject>( "Prefabs/CharacterBodies/LemurianBody" ).InstantiateClone( "MatGremlin" );
            BodyCatalog.getAdditionalEntries += ( list ) => list.Add( this.matGremlin );
            SurvivorAPI.AddSurvivor( new SurvivorDef
            {
                bodyPrefab = this.matGremlin,
                descriptionToken = "",
                name = "",
                displayPrefab = this.matGremlin.transform.Find( "ModelBase" ).gameObject,
                primaryColor = Color.clear,
                unlockableName = "",
            } );
            var charBody = this.matGremlin.GetComponent<CharacterBody>();
            charBody.baseArmor = 9001f;
            charBody.baseMaxHealth = 9001f;
            charBody.baseMaxShield = 9001f;
            charBody.baseRegen = 9001f;
            charBody.preferredInitialStateType = new EntityStates.SerializableEntityStateType( typeof( EntityStates.LemurianMonster.SpawnState ) );


            var pDef = ScriptableObject.CreateInstance<SkillDef>();
            pDef.activationState = new EntityStates.SerializableEntityStateType( typeof( SpawnMatPrefab ) );
            pDef.activationStateMachineName = "Weapon";
            pDef.mustKeyPress = true;
            pDef.baseMaxStock = 1;
            pDef.baseRechargeInterval = 3f;
            pDef.beginSkillCooldownOnSkillEnd = true;
            var pFamily = ScriptableObject.CreateInstance<SkillFamily>();
            pFamily.variants = new SkillFamily.Variant[]
            {
                new SkillFamily.Variant
                {
                    skillDef = pDef,
                    unlockableName = "",
                    viewableNode = new ViewablesCatalog.Node( "stuff", false ),
                },
            };
            var loc = this.matGremlin.GetComponent<SkillLocator>();
            loc.primary.SetFieldValue( "_skillFamily", pFamily );
            LoadoutAPI.AddSkill( typeof( SpawnMatPrefab ) );
            LoadoutAPI.AddSkillDef( pDef );
            LoadoutAPI.AddSkillFamily( pFamily );
        }

        internal static Boolean MatPrefabExists()
        {
            return false;
        }

        internal static GameObject matPrefab;
        internal static GameObject matPrefabInstance;

        private void CreateMatEditorPrefab()
        {
            if( matPrefab != null )
            {
                matPrefab = null;
            }
            if( matPrefabInstance != null )
            {
                Destroy( matPrefabInstance );
                matPrefabInstance = null;
            }

            var temp = GameObject.CreatePrimitive( PrimitiveType.Sphere );
            matPrefab = temp.InstantiateClone( "MatPrefab" );
            Destroy( temp );


            var col = matPrefab.GetComponent<Collider>();
            if( col != null ) Destroy( col );
            var rb = matPrefab.GetComponent<Rigidbody>();
            if( rb != null ) Destroy( rb );


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
                    //Spawn mat prefab
                    UnityEngine.Object.Destroy( this.aimEffect );
                }
                base.OnExit();
            }
        }
    }
}
#endif