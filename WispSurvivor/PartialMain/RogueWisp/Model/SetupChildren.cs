#if ROGUEWISP
using RogueWispPlugin.Helpers;
using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using UnityEngine;
//using static RogueWispPlugin.Helpers.APIInterface;

namespace RogueWispPlugin
{
    internal partial class Main
    {
        private CharacterModel RW_charModel;
        private WispModelBitSkinController RW_skinController;
        private ChildLocator RW_childLoc;
        private RagdollController RW_ragdollController;
        private HurtBoxGroup RW_boxGroup;
        private HealthComponent RW_healthComponent;

        private List<(Transform transform,String name)> pairsList = new List<(Transform transform,String name)>();

        partial void RW_SetupChildren()
        {
            this.Load += this.RW_ChildSetup;
        }

        private void RW_ChildSetup()
        {
            Transform modelTransform = this.RW_body.GetComponent<ModelLocator>().modelTransform;
            this.RW_charModel = modelTransform.GetComponent<CharacterModel>();
            this.RW_skinController = this.RW_charModel.AddComponent<WispModelBitSkinController>();
            this.RW_childLoc = modelTransform.GetComponent<ChildLocator>();
            this.RW_ragdollController = modelTransform.AddComponent<RagdollController>();
            this.RW_boxGroup = modelTransform.GetComponent<HurtBoxGroup>();
            this.RW_healthComponent = this.RW_body.GetComponent<HealthComponent>();

            //foreach( var val in this.RW_charModel.baseLightInfos ) UnityEngine.Object.DestroyImmediate( val.light.gameObject );
            //foreach( var val in this.RW_charModel.baseParticleSystemInfos ) UnityEngine.Object.DestroyImmediate( val.particleSystem.gameObject );

            //Path stuff here
            



            var allFlags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;
            var field = typeof(ChildLocator).GetField( "transformPairs", allFlags );
            var arrayType = field.FieldType;
            var pairType = arrayType.GetElementType();
            var funcType = typeof(Func<,,>).MakeGenericType( typeof(String), typeof(Transform), pairType );
            var nameField = pairType.GetField( "name", allFlags ) as MemberInfo;
            var transformField = pairType.GetField( "transform", allFlags ) as MemberInfo;
            var nameParam = Expression.Parameter( typeof(String), "name" );
            var transformParam = Expression.Parameter( typeof(Transform), "transform" );
            var newPair = Expression.New( pairType );
            var assignName = Expression.Bind( nameField, nameParam );
            var assignTransform = Expression.Bind( transformField, transformParam );
            var init = Expression.MemberInit( newPair, assignName, assignTransform );
            var initFunc = Expression.Lambda( funcType, init, nameParam, transformParam );
            Expression initArray = Expression.NewArrayInit( pairType,
                this.pairsList.Select<(Transform transform, String name),Expression>(
                    (val) => Expression.Invoke( initFunc, Expression.Constant( val.name ), Expression.Constant( val.transform ) ) ) );
            var childLocParam = Expression.Parameter( typeof(ChildLocator), "childLocator" );
            var assignArray = Expression.Assign( Expression.Field( childLocParam, field ), initArray );
            Expression.Lambda<Action<ChildLocator>>( assignArray, childLocParam ).Compile()( this.RW_childLoc );
        }

        private void AddToChildLocator( Transform target, String name )
        {
            this.pairsList.Add((target,name));
        }

        private void AddHurtBox( Transform parent, ICollider info )
        {
            var obj = new GameObject( "HurtBox" );
            obj.transform.parent = parent;
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            obj.transform.localRotation = Quaternion.identity;

            info.Apply( obj );
        }

        private void AddRagdollCollider( Transform parent, ICollider info )
        {
            var obj = new GameObject( "RagdollCollider" );
            obj.transform.parent = parent;
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            obj.transform.localRotation = Quaternion.identity;

            info.Apply( obj );
        }



        private void AddLight( Transform parent, Single intensity, Single range )
        {
            var obj = new GameObject( "Light" );
            obj.transform.parent = parent;
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            obj.transform.localRotation = Quaternion.identity;

            var light = obj.AddComponent<Light>();
            light.intensity = intensity;
            light.range = range;

            var ind = this.RW_charModel.baseLightInfos.Length;
            Array.Resize( ref this.RW_charModel.baseLightInfos, ind + 1 );
            this.RW_charModel.baseLightInfos[ind] = new CharacterModel.LightInfo( light );
        }

        private void AddFireParticles( Transform parent, Vector3 position, Vector3 rotation, Vector3 scale )
        {
            var obj = new GameObject( "Particles" );
            obj.transform.parent = parent;
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            obj.transform.localRotation = Quaternion.identity;

            var ps = obj.AddComponent<ParticleSystem>();
            var psr = obj.AddOrGetComponent<ParticleSystemRenderer>();


            var ind = this.RW_charModel.baseParticleSystemInfos.Length;
            Array.Resize( ref this.RW_charModel.baseParticleSystemInfos, ind + 1 );
            this.RW_charModel.baseParticleSystemInfos[ind] = new CharacterModel.ParticleSystemInfo( ps );

            // TODO: Actually set up the particle system
        }

        private interface ICollider
        {
            Collider Apply( GameObject obj );
        }

        private struct SphereColliderInfo : ICollider
        {
            public Collider Apply( GameObject obj )
            {
                var col = obj.AddComponent<SphereCollider>();


                return col;
            }
        }

        private struct BoxColliderInfo : ICollider
        {
            public Collider Apply( GameObject obj )
            {
                var col = obj.AddComponent<BoxCollider>();

                return col;
            }
        }

        private struct CapsuleColliderInfo : ICollider
        {
            public Collider Apply( GameObject obj )
            {
                var col = obj.AddComponent<CapsuleCollider>();


                return col;
            }
        }
    }
}
#endif