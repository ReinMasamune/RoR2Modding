#if STAGE
using System;
using System.Collections.Generic;
using R2API;
using RoR2;
using UnityEngine;

namespace RogueWispPlugin
{

    internal partial class Main
    {
        internal static GameObject wispSceneDiorama;
        partial void CreateSceneDef()
        {
            this.Load += this.CreateSceneDioramaPrefab;
            this.Load += this.SceneDefCreation;
        }

        private void CreateSceneDioramaPrefab()
        {
            wispSceneDiorama = GameObject.CreatePrimitive( PrimitiveType.Cube ).InstantiateClone("SceneDioramaThing", false );
            var col = wispSceneDiorama.GetComponent<Collider>();
            var rb = wispSceneDiorama.GetComponent<Rigidbody>();
            if( col ) Destroy( col );
            if( rb ) Destroy( rb );
        }

        private void SceneDefCreation()
        {
            wispSceneDef = ScriptableObject.CreateInstance<SceneDef>();

            wispSceneDef.nameToken = "MAP_WISPSTAGE_TITLE"; // TODO: Name Token
            wispSceneDef.subtitleToken = "MAP_WISPSTAGE_SUBTITLE"; // TODO: Subtitle Token
            wispSceneDef.loreToken = "MAP_WISPSTAGE_LORE"; // TODO: Lore token
            wispSceneDef.stageOrder = 200; // TODO: Stage Order
            wispSceneDef.previewTexture = null; // TODO: Preview Texture
            wispSceneDef.portalMaterial = null; // TODO: Portal Material
            wispSceneDef.portalSelectionMessageString = "MAP_WISPSTAGE_SEERSELECTIONMESSAGE"; // TODO: Portal Selection Message
            wispSceneDef.dioramaPrefab = wispSceneDiorama; // TODO: Diorama Prefab
            wispSceneDef.sceneType = SceneType.Intermission; // TODO: SceneType
            wispSceneDef.songName = "FULLSong18"; // TODO: Song Name
            wispSceneDef.bossSongName = "Song22"; // TODO: Boss Song Name
            wispSceneDef.isOfflineScene = false;
            wispSceneDef.suppressNpcEntry = false; // TODO: Suppress NPC Entry
            wispSceneDef.destinations = Array.Empty<SceneDef>(); // TODO: Destinations
            wispSceneDef.sceneNameOverrides = new List<String>(); // TODO: SceneNameOverrides
            wispSceneDef.sceneDefIndex = 0; // TODO: SceneDefIndex
            //wispSceneDef.baseSceneName = ""; // TODO: BaseSceneName



            SceneCatalog.getAdditionalEntries += ( list ) => list.Add( wispSceneDef );
        }
    }

}
#endif