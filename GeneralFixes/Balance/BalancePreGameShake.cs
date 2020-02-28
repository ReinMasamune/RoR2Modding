using BepInEx;
using RoR2;
using UnityEngine;
using System.Collections.Generic;
using RoR2.Navigation;
using R2API;
using R2API.Utils;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using System.Reflection;
using EntityStates;
using RoR2.Skills;
using BepInEx.Configuration;

namespace ReinGeneralFixes
{
    internal partial class Main
    {
        private ConfigEntry<Boolean> characterSelectBalanced;
        partial void BalancePreGameShake()
        {
            this.characterSelectBalanced = base.Config.Bind<Boolean>( "General:", "Character Select balance", true, "Should the balance in character select be extremely amazing? Option is here if you find it distracting, or if a custom character has a strange interaction/bug" );
            if( this.characterSelectBalanced.Value )
            {
                this.Enable += this.Main_Enable;
                this.Disable += this.Main_Disable;
                this.FirstFrame += this.Main_FirstFrame;
            }
        }

        private void Main_FirstFrame()
        {
            foreach( var def in SurvivorCatalog.allSurvivorDefs )
            {
                var body = def.bodyPrefab;
                if( body == null ) continue;
                var display = def.displayPrefab;
                if( display == null ) continue;
                var baseRagdoll = body.GetComponentInChildren<RagdollController>();
                if( baseRagdoll == null ) continue;
                var modelLoc = body.GetComponent<ModelLocator>();
                if( modelLoc == null ) continue;
                var modelObject = modelLoc.modelBaseTransform.gameObject;
                if( modelObject == null ) continue;

                var newDisplay = modelObject.InstantiateClone( body.name + "display", false );
                newDisplay.AddComponent<SillyFunTime>();

                def.displayPrefab = newDisplay;
            }
        }

        private void Main_Disable()
        {

        }
        private void Main_Enable()
        {
            On.RoR2.PreGameShakeController.Awake += this.PreGameShakeController_Awake;
            On.RoR2.PreGameShakeController.DoShake += this.PreGameShakeController_DoShake;
        }

        private void PreGameShakeController_DoShake( On.RoR2.PreGameShakeController.orig_DoShake orig, PreGameShakeController self )
        {
            foreach( var silly in SillyFunTime.instances )
            {
                if( silly.EnableRagdoll() )
                {
                    var ind = self.physicsBodies.Length;
                    Array.Resize<Rigidbody>( ref self.physicsBodies, ind + 1 );
                    self.physicsBodies[ind] = silly.mainRB;
                }
            }


            orig( self );
        }

        private void PreGameShakeController_Awake( On.RoR2.PreGameShakeController.orig_Awake orig, PreGameShakeController self )
        {
            self.transform.localPosition = new Vector3( 0f, 1.24f, 0f );
            var box1 = self.gameObject.AddComponent<BoxCollider>();
            box1.center = new Vector3( 0.5f, 2f, 2f );
            box1.size = new Vector3( 20f, 7f, 1f );
            var box2 = self.gameObject.AddComponent<BoxCollider>();
            box2.center = new Vector3( 0.5f, 5f, 11.5f );
            box2.size = new Vector3( 20f, 1f, 22f );
            var box3 = self.gameObject.AddComponent<BoxCollider>();
            box3.center = new Vector3( 10f, 2f, 11.5f );
            box3.size = new Vector3( 1f, 7f, 22f );
            var box4 = self.gameObject.AddComponent<BoxCollider>();
            box4.center = new Vector3( -9f, 2f, 11.5f );
            box4.size = new Vector3( 1f, 7f, 22f );
            var box5 = self.gameObject.AddComponent<BoxCollider>();
            box5.center = new Vector3( 0.5f, 2f, 22f );
            box5.size = new Vector3( 20f, 7f, 1f );


            //var handParent = GameObject.Find( "HANDTeaser" );
            //var rb = handParent.AddComponent<Rigidbody>();
            //handParent.AddComponent<SphereCollider>().radius = 0.1f;
            //foreach( var t in handParent.GetComponentsInChildren<Transform>() )
            //{
            //    t.gameObject.AddComponent<Rigidbody>();
            //    var sph = t.gameObject.AddComponent<SphereCollider>();
            //    sph.radius = 0.1f;
            //}
            //var ind = self.physicsBodies.Length;
            //Array.Resize<Rigidbody>( ref self.physicsBodies, ind + 1 );
            //self.physicsBodies[ind] = rb;

            self.minInterval *= 0.05f;
            self.maxInterval *= 0.05f;
            self.physicsForce *= 4f;
            orig( self );
        }

        internal class SillyFunTime : MonoBehaviour
        {
            internal static List<SillyFunTime> instances = new List<SillyFunTime>();

            internal Rigidbody mainRB;
            
            
            internal Boolean EnableRagdoll()
            {
                if( this.ragdolled ) return false;
                this.ragdoll.BeginRagdoll( Vector3.up * 5f );
                this.ragdolled = true;
                return true;
            }

            private RagdollController ragdoll;
            private Boolean ragdolled = false;
            private void Awake()
            {
                this.ragdoll = base.GetComponentInChildren<RagdollController>();
                this.mainRB = this.ragdoll.bones[0].GetComponent<Rigidbody>();

            }
            private void OnEnable()
            {
                instances.Add( this );
                this.ragdolled = false;
            }
            private void OnDisable()
            {
                instances.Remove( this );
                this.ragdolled = false;
            }
        }
    }
}
/*
Changelist:
Commando Phase round 2s cooldown, phase blast 3s cooldown



















*/