#if ROGUEWISP
using RoR2;
using System;
using System.Collections.Generic;
//using static RogueWispPlugin.Helpers.APIInterface;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using UnityEngine;

namespace RogueWispPlugin
{

    internal partial class Main
    {
        private Type RW_nameTransformPair;
        private readonly List<GameObject> RW_newHurtBoxes = new List<GameObject>();

        //partial void RW_SetupChildLocator() => this.Load += this.RW_DoChildLocatorSetup;//this.Load += this.RW_NewChildLocatorSetup;

        private void RW_NewChildLocatorSetup()
        {
            Transform model = this.RW_body.GetComponent<ModelLocator>().modelTransform;
            RagdollController rag = model.gameObject.AddComponent<RagdollController>();
            ChildLocator childLoc = model.GetComponent<ChildLocator>();

            FieldInfo transformPairsArray = typeof(ChildLocator).GetField( "transformPairs", BindingFlags.NonPublic | BindingFlags.Instance );
            System.Object pairsObj = transformPairsArray.GetValue( childLoc );
            System.Object[] pairsArray = ((Array)pairsObj).Cast<System.Object>().ToArray();
            Type transformPair = pairsObj.GetType().GetElementType();

        }

        private void AddNewTransformPair( String name, Transform transform, Type pairType, ref System.Object[] pairsArray )
        {
            Int32 index = pairsArray.Length;
            Array.Resize( ref pairsArray, index + 1 );

            System.Object pair = FormatterServices.GetUninitializedObject( pairType );
            FieldInfo nameField = pairType.GetField( "name", BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic );
            FieldInfo transformField = pairType.GetField( "transform", BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic );
            nameField.SetValue( pair, name );
            transformField.SetValue( pair, transform );
            pairsArray[index] = pair;
        }

        private void AddNewHurtBox( GameObject parent, HurtBoxGroup group, HealthComponent health, Boolean isMain, Boolean isBullseye, HurtBox.DamageModifier modifier )
        {
            GameObject obj = new GameObject( "HurtBox" );
            obj.transform.parent = parent.transform;
            obj.transform.localScale = Vector3.one;
            obj.transform.localRotation = Quaternion.identity;
            obj.transform.localPosition = Vector3.zero;

            obj.layer = LayerIndex.entityPrecise.intVal;

            Collider col = null;

            Collider parCol = parent.GetComponent<Collider>();
            if( parCol is SphereCollider )
            {
                SphereCollider sphere = obj.AddComponent<SphereCollider>();
                SphereCollider parSphere = parCol as SphereCollider;
                sphere.center = parSphere.center;
                sphere.contactOffset = parSphere.contactOffset;
                sphere.radius = parSphere.radius;
                sphere.isTrigger = false;
                //sphere.Material

                col = sphere;
            } else if( parCol is BoxCollider )
            {
                BoxCollider box = obj.AddComponent<BoxCollider>();
                BoxCollider parBox = parCol as BoxCollider;
                box.center = parBox.center;
                box.contactOffset = parBox.contactOffset;
                box.size = parBox.size;
                box.isTrigger = false;
                //box.Material

                col = box;

            } else if( parCol is CapsuleCollider )
            {
                CapsuleCollider cap = obj.AddComponent<CapsuleCollider>();
                CapsuleCollider parCap = parCol as CapsuleCollider;
                cap.center = parCap.center;
                cap.contactOffset = parCap.contactOffset;
                cap.direction = parCap.direction;
                cap.height = parCap.height;
                cap.radius = parCap.radius;
                cap.isTrigger = false;
                //cap.material

                col = cap;
            } else
            {
                base.Logger.LogError( "Unhandled collider type: " + parCol.GetType().ToString() );
                Destroy( obj );
                return;
            }

            if( col == null )
            {
                base.Logger.LogError( "Collider was null" );
                Destroy( obj );
                return;
            }

            //parent.transform.parent = obj.transform;

            HurtBox hurtBox = obj.AddComponent<HurtBox>();
            hurtBox.isBullseye = isBullseye;
            hurtBox.healthComponent = health;
            hurtBox.damageModifier = modifier;
            hurtBox.hurtBoxGroup = group;

            if( group.hurtBoxes == null )
            {
                group.hurtBoxes = Array.Empty<HurtBox>();
            }

            if( isMain )
            {
                group.mainHurtBox = hurtBox;
            }

            Int16 index = (Int16)group.hurtBoxes.Length;
            Array.Resize<HurtBox>( ref group.hurtBoxes, index + 1 );
            group.hurtBoxes[index] = hurtBox;
            hurtBox.indexInGroup = index;

            if( isBullseye )
            {
                ++group.bullseyeCount;
            }
        }

        private void RW_DoChildLocatorSetup()
        {
            Transform model = this.RW_body.GetComponent<ModelLocator>().modelTransform;
            RagdollController rag = model.gameObject.AddComponent<RagdollController>();

            ChildLocator children = model.GetComponent<ChildLocator>();
            FieldInfo f = typeof(ChildLocator).GetField("transformPairs", BindingFlags.NonPublic | BindingFlags.Instance);
            System.Object thing = f.GetValue( children );
            System.Object[] pairs = ((Array)thing).Cast<System.Object>().ToArray();
            Type pairsArray = thing.GetType();
            this.RW_nameTransformPair = thing.GetType().GetElementType();
            Array.Resize<System.Object>( ref pairs, pairs.Length + 16 );

            List<Transform> bones = new List<Transform>();


            HurtBoxGroup boxGroup = model.GetComponent<HurtBoxGroup>();
            Destroy( boxGroup.hurtBoxes[0].gameObject );
            boxGroup.hurtBoxes = null;
            boxGroup.mainHurtBox = null;
            boxGroup.bullseyeCount = 0;

            HealthComponent health = this.RW_body.GetComponent<HealthComponent>();

            BoxCollider box;
            CapsuleCollider cap;
            Rigidbody rb;

            Transform t2;
            Array v = Array.CreateInstance(this.RW_nameTransformPair, pairs.Length + 16);

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

                    case "ChestCannon1"://
                        t2 = new GameObject( "ItemParent" + i.ToString() ).transform;
                        t2.parent = t;
                        t2.localPosition = new Vector3( 0f, 0.65f, 0f );
                        t2.localEulerAngles = new Vector3( 180f, 0f, 0f );
                        t2.localScale = new Vector3( 1.25f, 1.25f, 1.25f );
                        v.SetValue( (this.CreateNameTransformPair( "Chest", t2 )), i++ );

                        box = t.gameObject.AddComponent<BoxCollider>();
                        box.size = new Vector3( 0.5f, 0.9f, 0.1f );
                        box.center = new Vector3( 0f, 0.4f, 0f );
                        box.enabled = false;
                        t.gameObject.AddComponent<Rigidbody>();
                        bones.Add( t );
                        this.AddNewHurtBox( t.gameObject, boxGroup, health, true, true, HurtBox.DamageModifier.Normal );
                        break;

                    case "ChestCannonGuard1"://
                        box = t.gameObject.AddComponent<BoxCollider>();
                        box.size = new Vector3( 0.75f, 0.7f, 0.2f );
                        box.center = new Vector3( 0f, 0.25f, 0.05f );
                        box.enabled = false;
                        t.gameObject.AddComponent<Rigidbody>();
                        bones.Add( t );
                        this.AddNewHurtBox( t.gameObject, boxGroup, health, false, false, HurtBox.DamageModifier.Normal );
                        break;

                    case "ChestCannon2"://
                        t2 = new GameObject( "ItemParent" + i.ToString() ).transform;
                        t2.parent = t;
                        t2.localPosition = new Vector3( 0f, 0.5f, 0f );
                        t2.localEulerAngles = new Vector3( 180f, 0f, 0f );
                        t2.localScale = new Vector3( 1.25f, 1.25f, 1.25f );
                        v.SetValue( (this.CreateNameTransformPair( "Stomach", t2 )), i++ );

                        box = t.gameObject.AddComponent<BoxCollider>();
                        box.size = new Vector3( 0.5f, 0.9f, 0.1f );
                        box.center = new Vector3( 0f, 0.4f, 0f );
                        box.enabled = false;
                        t.gameObject.AddComponent<Rigidbody>();
                        bones.Add( t );
                        this.AddNewHurtBox( t.gameObject, boxGroup, health, false, false, HurtBox.DamageModifier.Normal );
                        break;

                    case "ChestCannonGuard2"://
                        box = t.gameObject.AddComponent<BoxCollider>();
                        box.size = new Vector3( 0.7f, 0.5f, 0.2f );
                        box.center = new Vector3( 0f, 0.17f, -0.05f );
                        box.enabled = false;
                        t.gameObject.AddComponent<Rigidbody>();
                        bones.Add( t );
                        this.AddNewHurtBox( t.gameObject, boxGroup, health, false, false, HurtBox.DamageModifier.Normal );
                        break;

                    case "Head"://
                        t2 = new GameObject( "ItemParent" + i.ToString() ).transform;
                        t2.parent = t;
                        t2.localPosition = new Vector3( 0f, 0.2f, 0f );
                        t2.localEulerAngles = new Vector3( 0f, 0f, 0f );
                        t2.localScale = new Vector3( 1.25f, 1.25f, 1.25f );
                        v.SetValue( (this.CreateNameTransformPair( "Head", t2 )), i++ );

                        cap = t.gameObject.AddComponent<CapsuleCollider>();
                        cap.direction = 0;
                        cap.radius = 0.25f;
                        cap.height = 1f;
                        cap.center = new Vector3( 0f, 0.25f, 0.15f );
                        t.gameObject.AddComponent<Rigidbody>();
                        bones.Add( t );
                        this.AddNewHurtBox( t.gameObject, boxGroup, health, false, false, HurtBox.DamageModifier.Normal );
                        break;

                    case "thigh.r"://
                        t2 = new GameObject( "ItemParent" + i.ToString() ).transform;
                        t2.parent = t;
                        t2.localPosition = new Vector3( 0f, 0.25f, 0f );
                        t2.localEulerAngles = new Vector3( 0f, -90f, 0f );
                        t2.localScale = new Vector3( 1.25f, 1.25f, 1.25f );
                        v.SetValue( this.CreateNameTransformPair( "ThighR", t2 ), i++ );

                        cap = t.gameObject.AddComponent<CapsuleCollider>();
                        cap.direction = 1;
                        cap.radius = 0.15f;
                        cap.height = 0.5f;
                        cap.center = new Vector3( 0f, 0.2f, 0f );
                        t.gameObject.AddComponent<Rigidbody>();
                        bones.Add( t );
                        this.AddNewHurtBox( t.gameObject, boxGroup, health, false, false, HurtBox.DamageModifier.Normal );
                        break;

                    case "thigh.l"://
                        t2 = new GameObject( "ItemParent" + i.ToString() ).transform;
                        t2.parent = t;
                        t2.localPosition = new Vector3( 0f, 0.25f, 0f );
                        t2.localEulerAngles = new Vector3( 0f, 90f, 0f );
                        t2.localScale = new Vector3( 1.25f, 1.25f, 1.25f );
                        v.SetValue( (this.CreateNameTransformPair( "ThighL", t2 )), i++ );

                        cap = t.gameObject.AddComponent<CapsuleCollider>();
                        cap.direction = 1;
                        cap.radius = 0.15f;
                        cap.height = 0.5f;
                        cap.center = new Vector3( 0f, 0.2f, 0f );
                        t.gameObject.AddComponent<Rigidbody>();
                        bones.Add( t );
                        this.AddNewHurtBox( t.gameObject, boxGroup, health, false, false, HurtBox.DamageModifier.Normal );
                        break;

                    case "calf.r"://
                        t2 = new GameObject( "ItemParent" + i.ToString() ).transform;
                        t2.parent = t;
                        t2.localPosition = new Vector3( 0f, 0f, 0f );
                        t2.localEulerAngles = new Vector3( 0f, 0f, 0f );
                        t2.localScale = new Vector3( 1.25f, 1.25f, 1.25f );
                        v.SetValue( (this.CreateNameTransformPair( "CalfR", t2 )), i++ );

                        cap = t.gameObject.AddComponent<CapsuleCollider>();
                        cap.direction = 1;
                        cap.radius = 0.125f;
                        cap.height = 0.7f;
                        cap.center = new Vector3( 0f, 0.3f, 0f );
                        t.gameObject.AddComponent<Rigidbody>();
                        bones.Add( t );
                        this.AddNewHurtBox( t.gameObject, boxGroup, health, false, false, HurtBox.DamageModifier.Normal );
                        break;

                    case "calf.l"://
                        t2 = new GameObject( "ItemParent" + i.ToString() ).transform;
                        t2.parent = t;
                        t2.localPosition = new Vector3( 0f, 0f, 0f );
                        t2.localEulerAngles = new Vector3( 0f, 0f, 0f );
                        t2.localScale = new Vector3( 1.25f, 1.25f, 1.25f );
                        v.SetValue( (this.CreateNameTransformPair( "CalfL", t2 )), i++ );

                        cap = t.gameObject.AddComponent<CapsuleCollider>();
                        cap.direction = 1;
                        cap.radius = 0.125f;
                        cap.height = 0.7f;
                        cap.center = new Vector3( 0f, 0.3f, 0f );
                        t.gameObject.AddComponent<Rigidbody>();
                        bones.Add( t );
                        this.AddNewHurtBox( t.gameObject, boxGroup, health, false, false, HurtBox.DamageModifier.Normal );
                        break;

                    case "toe1.l"://
                        t2 = new GameObject( "ItemParent" + i.ToString() ).transform;
                        t2.parent = t;
                        t2.localPosition = new Vector3( 0f, 0f, 0f );
                        t2.localEulerAngles = new Vector3( -45f, 0f, 0f );
                        t2.localScale = new Vector3( 1.25f, 1.25f, 1.25f );
                        v.SetValue( (this.CreateNameTransformPair( "FootL", t2 )), i++ );

                        box = t.gameObject.AddComponent<BoxCollider>();
                        box.size = new Vector3( 0.12f, 0.3f, 0.08f );
                        box.center = new Vector3( 0f, 0.08f, 0.01f );
                        box.enabled = false;
                        t.gameObject.AddComponent<Rigidbody>();
                        bones.Add( t );
                        this.AddNewHurtBox( t.gameObject, boxGroup, health, false, false, HurtBox.DamageModifier.Normal );
                        break;

                    case "toe1.r"://
                        t2 = new GameObject( "ItemParent" + i.ToString() ).transform;
                        t2.parent = t;
                        t2.localPosition = new Vector3( 0f, 0f, 0f );
                        t2.localEulerAngles = new Vector3( -45f, 0f, 0f );
                        t2.localScale = new Vector3( 1.25f, 1.25f, 1.25f );
                        v.SetValue( (this.CreateNameTransformPair( "FootR", t2 )), i++ );

                        box = t.gameObject.AddComponent<BoxCollider>();
                        box.size = new Vector3( 0.12f, 0.3f, 0.08f );
                        box.center = new Vector3( 0f, 0.08f, 0.01f );
                        box.enabled = false;
                        t.gameObject.AddComponent<Rigidbody>();
                        bones.Add( t );
                        this.AddNewHurtBox( t.gameObject, boxGroup, health, false, false, HurtBox.DamageModifier.Normal );
                        break;

                    case "toe2.l"://
                        box = t.gameObject.AddComponent<BoxCollider>();
                        box.size = new Vector3( 0.08f, 0.2f, 0.06f );
                        box.center = new Vector3( 0f, 0.06f, 0f );
                        box.enabled = false;
                        t.gameObject.AddComponent<Rigidbody>();
                        bones.Add( t );
                        this.AddNewHurtBox( t.gameObject, boxGroup, health, false, false, HurtBox.DamageModifier.Normal );
                        break;

                    case "toe2.r":
                        box = t.gameObject.AddComponent<BoxCollider>();
                        box.size = new Vector3( 0.08f, 0.2f, 0.06f );
                        box.center = new Vector3( 0f, 0.06f, 0f );
                        box.enabled = false;
                        t.gameObject.AddComponent<Rigidbody>();
                        bones.Add( t );
                        this.AddNewHurtBox( t.gameObject, boxGroup, health, false, false, HurtBox.DamageModifier.Normal );
                        break;

                    case "shoulder.l"://
                        box = t.gameObject.AddComponent<BoxCollider>();
                        box.size = new Vector3( 0.3f, 0.5f, 0.5f );
                        box.center = new Vector3( -0.075f, 0.3f, 0f );
                        box.enabled = false;
                        t.gameObject.AddComponent<Rigidbody>();
                        bones.Add( t );
                        this.AddNewHurtBox( t.gameObject, boxGroup, health, false, false, HurtBox.DamageModifier.Normal );
                        break;

                    case "shoulder.r"://
                        box = t.gameObject.AddComponent<BoxCollider>();
                        box.size = new Vector3( 0.3f, 0.5f, 0.5f );
                        box.center = new Vector3( 0.075f, 0.3f, 0f );
                        box.enabled = false;
                        t.gameObject.AddComponent<Rigidbody>();
                        bones.Add( t );
                        this.AddNewHurtBox( t.gameObject, boxGroup, health, false, false, HurtBox.DamageModifier.Normal );
                        break;

                    case "upperArm1.l"://
                        t2 = new GameObject( "ItemParent" + i.ToString() ).transform;
                        t2.parent = t;
                        t2.localPosition = new Vector3( 0f, 0f, 0f );
                        t2.localEulerAngles = new Vector3( 0f, 0f, 0f );
                        t2.localScale = new Vector3( 1.25f, 1.25f, 1.25f );
                        v.SetValue( (this.CreateNameTransformPair( "UpperArmL", t2 )), i++ );

                        box = t.gameObject.AddComponent<BoxCollider>();
                        box.size = new Vector3( 0.2f, 0.4f, 0.4f );
                        box.center = new Vector3( 0f, 0.15f, 0f );
                        box.enabled = false;
                        t.gameObject.AddComponent<Rigidbody>();
                        bones.Add( t );
                        this.AddNewHurtBox( t.gameObject, boxGroup, health, false, false, HurtBox.DamageModifier.Normal );
                        break;

                    case "upperArm1.r"://
                        t2 = new GameObject( "ItemParent" + i.ToString() ).transform;
                        t2.parent = t;
                        t2.localPosition = new Vector3( 0f, 0f, 0f );
                        t2.localEulerAngles = new Vector3( 0f, 0f, 0f );
                        t2.localScale = new Vector3( 1.25f, 1.25f, 1.25f );
                        v.SetValue( (this.CreateNameTransformPair( "UpperArmR", t2 )), i++ );

                        box = t.gameObject.AddComponent<BoxCollider>();
                        box.size = new Vector3( 0.2f, 0.4f, 0.4f );
                        box.center = new Vector3( 0f, 0.15f, 0f );
                        box.enabled = false;
                        t.gameObject.AddComponent<Rigidbody>();
                        bones.Add( t );
                        this.AddNewHurtBox( t.gameObject, boxGroup, health, false, false, HurtBox.DamageModifier.Normal );
                        break;

                    case "upperArm2.l"://
                        box = t.gameObject.AddComponent<BoxCollider>();
                        box.size = new Vector3( 0.12f, 0.3f, 0.3f );
                        box.center = new Vector3( 0.01f, 0.17f, 0f );
                        box.enabled = false;
                        t.gameObject.AddComponent<Rigidbody>();
                        bones.Add( t );
                        this.AddNewHurtBox( t.gameObject, boxGroup, health, false, false, HurtBox.DamageModifier.Normal );
                        break;

                    case "upperArm2.r"://
                        box = t.gameObject.AddComponent<BoxCollider>();
                        box.size = new Vector3( 0.12f, 0.3f, 0.3f );
                        box.center = new Vector3( -0.01f, 0.17f, 0f );
                        box.enabled = false;
                        t.gameObject.AddComponent<Rigidbody>();
                        bones.Add( t );
                        this.AddNewHurtBox( t.gameObject, boxGroup, health, false, false, HurtBox.DamageModifier.Normal );
                        break;

                    case "finger1.l"://
                        cap = t.gameObject.AddComponent<CapsuleCollider>();
                        cap.direction = 1;
                        cap.radius = 0.035f;
                        cap.height = 0.3f;
                        cap.center = new Vector3( 0f, 0.09f, 0f );
                        t.gameObject.AddComponent<Rigidbody>();
                        bones.Add( t );
                        this.AddNewHurtBox( t.gameObject, boxGroup, health, false, false, HurtBox.DamageModifier.Normal );
                        break;

                    case "finger1.r"://
                        t2 = new GameObject( "ItemParent" + i.ToString() ).transform;
                        t2.parent = t;
                        t2.localPosition = new Vector3( 0f, 0.1f, 0f );
                        t2.localEulerAngles = new Vector3( 0f, 0f, 0f );
                        t2.localScale = new Vector3( 1.25f, 1.25f, 1.25f );
                        v.SetValue( (this.CreateNameTransformPair( "Finger22R", t2 )), i++ );

                        cap = t.gameObject.AddComponent<CapsuleCollider>();
                        cap.direction = 1;
                        cap.radius = 0.035f;
                        cap.height = 0.3f;
                        cap.center = new Vector3( 0f, 0.09f, 0f );
                        t.gameObject.AddComponent<Rigidbody>();
                        bones.Add( t );
                        this.AddNewHurtBox( t.gameObject, boxGroup, health, false, false, HurtBox.DamageModifier.Normal );
                        break;

                    case "finger2.l"://
                        cap = t.gameObject.AddComponent<CapsuleCollider>();
                        cap.direction = 1;
                        cap.radius = 0.035f;
                        cap.height = 0.25f;
                        cap.center = new Vector3( 0f, 0.075f, 0f );
                        t.gameObject.AddComponent<Rigidbody>();
                        bones.Add( t );
                        this.AddNewHurtBox( t.gameObject, boxGroup, health, false, false, HurtBox.DamageModifier.Normal );
                        break;

                    case "finger2.r"://
                        t2 = new GameObject( "ItemParent" + i.ToString() ).transform;
                        t2.parent = t;
                        t2.localPosition = new Vector3( 0f, 0.1f, 0f );
                        t2.localEulerAngles = new Vector3( 0f, 0f, 0f );
                        t2.localScale = new Vector3( 1.25f, 1.25f, 1.25f );
                        v.SetValue( (this.CreateNameTransformPair( "Finger42R", t2 )), i++ );

                        cap = t.gameObject.AddComponent<CapsuleCollider>();
                        cap.direction = 1;
                        cap.radius = 0.035f;
                        cap.height = 0.25f;
                        cap.center = new Vector3( 0f, 0.075f, 0f );
                        t.gameObject.AddComponent<Rigidbody>();
                        bones.Add( t );
                        this.AddNewHurtBox( t.gameObject, boxGroup, health, false, false, HurtBox.DamageModifier.Normal );
                        break;

                    case "thumb.l"://
                        t2 = new GameObject( "ItemParent" + i.ToString() ).transform;
                        t2.parent = t;
                        t2.localPosition = new Vector3( 0f, -0.15f, -0.1f );
                        t2.localEulerAngles = new Vector3( 0f, 170f, 0f );
                        t2.localScale = new Vector3( 1.25f, 1.25f, 1.25f );
                        v.SetValue( (this.CreateNameTransformPair( "HandL", t2 )), i++ );

                        cap = t.gameObject.AddComponent<CapsuleCollider>();
                        cap.direction = 1;
                        cap.radius = 0.05f;
                        cap.height = 0.2f;
                        cap.center = new Vector3( -0.005f, 0.085f, 0f );
                        t.gameObject.AddComponent<Rigidbody>();
                        bones.Add( t );
                        this.AddNewHurtBox( t.gameObject, boxGroup, health, false, false, HurtBox.DamageModifier.Normal );
                        break;

                    case "thumb.r"://
                        cap = t.gameObject.AddComponent<CapsuleCollider>();
                        cap.direction = 1;
                        cap.radius = 0.05f;
                        cap.height = 0.2f;
                        cap.center = new Vector3( -0.005f, 0.085f, 0f );
                        t.gameObject.AddComponent<Rigidbody>();
                        bones.Add( t );
                        this.AddNewHurtBox( t.gameObject, boxGroup, health, false, false, HurtBox.DamageModifier.Normal );
                        break;


                    case "chest":
                        t2 = new GameObject( "ItemParent" + i.ToString() ).transform;
                        t2.parent = t;
                        t2.localPosition = new Vector3( 0f, 0.5f, 0f );
                        t2.localEulerAngles = new Vector3( 0f, -90f, 0f );
                        t2.localScale = new Vector3( 1.25f, 1.25f, 1.25f );
                        v.SetValue( (this.CreateNameTransformPair( "Pelvis", t2 )), i++ );
                        break;

                    case "lowerArm.l":
                        t2 = new GameObject( "ItemParent" + i.ToString() ).transform;
                        t2.parent = t;
                        t2.localPosition = new Vector3( 0f, -0.2f, 0f );
                        t2.localEulerAngles = new Vector3( 0f, 90f, 0f );
                        t2.localScale = new Vector3( 1.25f, 1.25f, 1.25f );
                        v.SetValue( (this.CreateNameTransformPair( "LowerArmL", t2 )), i++ );
                        break;

                    case "lowerArm.r":
                        t2 = new GameObject( "ItemParent" + i.ToString() ).transform;
                        t2.parent = t;
                        t2.localPosition = new Vector3( 0f, -0.2f, 0f );
                        t2.localEulerAngles = new Vector3( 0f, 90f, 0f );
                        t2.localScale = new Vector3( 1.25f, 1.25f, 1.25f );
                        v.SetValue( (this.CreateNameTransformPair( "LowerArmR", t2 )), i++ );
                        break;

                    case "AncientWispArmature":
                        t2 = new GameObject( "ItemParent" + i.ToString() ).transform;
                        t2.parent = t;
                        t2.localPosition = new Vector3( 0f, 0f, 1.35f );
                        t2.localEulerAngles = new Vector3( 180f, 0f, 0f );
                        t2.localScale = new Vector3( 1.25f, 1.25f, 1.25f );
                        v.SetValue( (this.CreateNameTransformPair( "Base", t2 )), i++ );
                        break;
                }
            }




            //MethodInfo m = typeof(WispModelModule).GetMethod("ReflCast").MakeGenericMethod((Type)nameTransformPair.MakeArrayType());
            //MethodInfo m2 = typeof(WispModelModule).GetMethod("CastArray").MakeGenericMethod((Type)nameTransformPair);
            f.SetValue( children, v );
            rag.bones = bones.ToArray();
        }

        private System.Object CreateNameTransformPair( String name, Transform transform )
        {
            System.Object o = FormatterServices.GetUninitializedObject( this.RW_nameTransformPair );
            FieldInfo nameField = this.RW_nameTransformPair.GetField("name", BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
            FieldInfo transformField = this.RW_nameTransformPair.GetField("transform", BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
            nameField.SetValue( o, name );
            transformField.SetValue( o, transform );
            return o;
        }
    }

}
#endif