using BepInEx;
using R2API.Utils;
using RoR2;
using RoR2.Networking;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using RogueWispPlugin.Helpers;
using RogueWispPlugin.Modules;
//using static RogueWispPlugin.Helpers.APIInterface;
using System.Linq;
using System.Runtime.Serialization;

namespace RogueWispPlugin
{
    internal partial class Main
    {
        Type RW_nameTransformPair;

        partial void RW_SetupChildLocator()
        {
            this.Load += this.RW_DoChildLocatorSetup;
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
            RW_nameTransformPair = thing.GetType().GetElementType();
            Array.Resize<System.Object>( ref pairs, pairs.Length + 16 );

            List<Transform> bones = new List<Transform>();

            BoxCollider box;
            CapsuleCollider cap;
            Rigidbody rb;

            Transform t2;
            Array v = Array.CreateInstance(RW_nameTransformPair, pairs.Length + 16);

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
