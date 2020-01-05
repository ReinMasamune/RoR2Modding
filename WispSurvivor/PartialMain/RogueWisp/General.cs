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

namespace RogueWispPlugin
{
    internal partial class Main
    {


        partial void RW_General()
        {
            this.Load += this.RW_LoadAssetBundle;
            this.Load += this.RW_GetBody;
            this.Load += this.RW_Misc;
            this.Load += this.RW_Components;
        }

        private void RW_Components()
        {
            MonoBehaviour.Destroy( this.RW_body.GetComponent<RigidbodyDirection>() );
            MonoBehaviour.Destroy( this.RW_body.GetComponent<RigidbodyMotor>() );
            MonoBehaviour.Destroy( this.RW_body.GetComponent<QuaternionPID>() );
            MonoBehaviour.Destroy( this.RW_body.GetComponent<DeathRewards>() );
            foreach( VectorPID c in this.RW_body.GetComponents<VectorPID>() )
            {
                MonoBehaviour.Destroy( c );
            }
            MonoBehaviour.DestroyImmediate( this.RW_body.GetComponent<CapsuleCollider>() );

            this.RW_body.AddComponent<CharacterMotor>();
            this.RW_body.AddComponent<CharacterDirection>();
            this.RW_body.AddComponent<KinematicCharacterController.KinematicCharacterMotor>();
            this.RW_body.AddComponent<SetStateOnHurt>();
            this.RW_body.AddComponent<WispFlareController>();
            this.RW_body.AddComponent<ClientOrbController>();
            this.RW_body.AddComponent<WispFlamesController>();

            var passive = this.RW_body.AddComponent<WispPassiveController>();
            this.RW_body.AddComponent<WispUIController>().passive = passive;
            this.RW_body.AddComponent<EntityStateMachine>().customName = "Gaze";

            // GET components missing now.
        }

        private void RW_Misc()
        {
            InteractionDriver bodyIntDriver = this.RW_body.GetComponent<InteractionDriver>();
            bodyIntDriver.highlightInteractor = true;
            CharacterNetworkTransform bodyNetTrans = this.RW_body.GetComponent<CharacterNetworkTransform>();
            bodyNetTrans.positionTransmitInterval = 0.05f;
            bodyNetTrans.interpolationFactor = 3f;
        }

        private void RW_GetBody()
        {
            this.RW_body = Resources.Load<GameObject>( "Prefabs/CharacterBodies/AncientWispBody" ).InstantiateClone( "RogueWispBody" );
            BodyCatalog.getAdditionalEntries += ( list ) => list.Add( this.RW_body );
            GameObject display = this.RW_body.GetComponent<ModelLocator>().modelBaseTransform.gameObject;

            SurvivorDef bodySurvivorDef = new SurvivorDef
            {
                bodyPrefab = this.RW_body,
                descriptionToken = "WISP_SURVIVOR_BODY_DESC",
                displayPrefab = display,
                primaryColor = new Color(0.7f, 0.2f, 0.9f),
            };

            R2API.SurvivorAPI.AddSurvivor( bodySurvivorDef );
        }

        private void RW_LoadAssetBundle()
        {
            Assembly execAssembly = Assembly.GetExecutingAssembly();
            System.IO.Stream stream = execAssembly.GetManifestResourceStream( "WispSurvivor.Bundle.wispsurvivor" );
            this.RW_assetBundle = AssetBundle.LoadFromStream(stream);
        }
    }

}
