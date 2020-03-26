#if ROGUEWISP
using ReinCore;
using MonoMod.Cil;
using RoR2;
using RoR2.Networking;
using System;
using System.Reflection;
using UnityEngine;
using Rein.RogueWispPlugin.Helpers;
//using static RogueWispPlugin.Helpers.APIInterface;

namespace Rein.RogueWispPlugin
{

    internal partial class Main
    {
        private SurvivorDef RW_survivorDef;
        private CharacterBody RW_charBody;

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

#if MATEDITOR
            this.RW_body.AddComponent<MaterialEditor>();
#endif

            WispPassiveController passive = this.RW_body.AddComponent<WispPassiveController>();
            this.RW_body.AddComponent<WispCrosshairManager>();
            //this.RW_body.AddComponent<WispUIController>().passive = passive;
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

            SkinsCore.AddValidSkinOverride( this.RW_body, ( skinInd ) => true );
            SkinsCore.AddLockedSkinOverride( this.RW_body, ( skinInd ) => false );

            this.RW_body.tag = "Finish";
        }

        private void RW_GetBody()
        {
            this.RW_body = Resources.Load<GameObject>( "Prefabs/CharacterBodies/AncientWispBody" ).ClonePrefab( "RogueWispBody", true );
            BodyCatalog.getAdditionalEntries += ( list ) => list.Add( this.RW_body );
            this.RW_charBody = this.RW_body.GetComponent<CharacterBody>();
            GameObject display = this.RW_body.GetComponent<ModelLocator>().modelBaseTransform.gameObject;

            //var dummyBody = new GameObject("Nope").ClonePrefab("NopeNope", false);
            //BodyCatalog.getAdditionalEntries += ( list ) => list.Add( dummyBody );
            this.RW_survivorDef = new SurvivorDef
            {
                bodyPrefab = this.RW_body,
                descriptionToken = "WISP_SURVIVOR_BODY_DESC",
                displayPrefab = display,
                primaryColor = new Color( 0.7f, 0.2f, 0.9f ),
            };
            
            if( SurvivorsCore.loaded )
            {
                SurvivorCatalog.getAdditionalSurvivorDefs += ( list ) => list.Add( this.RW_survivorDef );
            } else
            {
                Main.LogF( "Survivor cannot be added" );
            }

            //R2API.SurvivorAPI.AddSurvivor( this.RW_survivorDef );
            //this.RW_survivorDef.Hide();
            //On.RoR2.SurvivorCatalog.RegisterSurvivor += this.SurvivorCatalog_RegisterSurvivor;
            //IL.RoR2.SurvivorCatalog.Init += this.SurvivorCatalog_Init;
            //IL.RoR2.UnlockableCatalog.Init += this.UnlockableCatalog_Init;
        }

        //private void UnlockableCatalog_Init( ILContext il )
        //{
        //    ILCursor c = new ILCursor( il );
        //    c.EmitDelegate<Action>( () => this.RW_survivorDef.Unhide() );
        //}

        //private void SurvivorCatalog_Init( MonoMod.Cil.ILContext il )
        //{
        //    ILCursor c = new ILCursor( il );
        //    c.EmitDelegate<Action>( () => this.RW_survivorDef.Unhide() );
        //    c.GotoNext( MoveType.Before, x => x.MatchRet() );
        //    c.EmitDelegate<Action>( () => this.RW_survivorDef.Hide() );
        //}

        //public class ReinSurvivorDef : SurvivorDef
        //{
        //    private GameObject hiddenBody;
        //    internal GameObject dummyBody;

        //    internal void Hide()
        //    {
        //        this.hiddenBody = base.bodyPrefab;
        //        base.bodyPrefab = this.dummyBody;
        //    }
        //    internal void Unhide()
        //    {
        //        base.bodyPrefab = this.hiddenBody;
        //    }
        //}

        private void RW_LoadAssetBundle()
        {
            this.RW_assetBundle = AssetBundle.LoadFromMemory( Rein.Properties.Resources.wispsurvivor );
        }
    }

}
#endif