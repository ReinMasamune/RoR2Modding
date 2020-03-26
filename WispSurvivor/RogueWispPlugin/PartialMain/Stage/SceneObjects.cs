#if STAGE
using System;
using System.Collections.Generic;
using System.Reflection;
using BundledScripts;
using R2API;
using RoR2;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Rein.RogueWispPlugin
{

    internal partial class Main
    {
        internal static AssetBundle sceneAssetBundle;
        internal static String wispScenePath;
        internal static Scene wispScene;
        partial void CreateSceneObjects()
        {
            this.Load += this.SceneAssetBundle;
            this.Load += this.CreateScene;


        }

        private void CreateScene()
        {
            wispScenePath = sceneAssetBundle.GetAllScenePaths()[0];
            wispScene = SceneManager.GetSceneByPath( wispScenePath );
        }
        private void SceneAssetBundle()
        {
            Assembly execAssembly = Assembly.GetExecutingAssembly();
            System.IO.Stream stream = execAssembly.GetManifestResourceStream( "RogueWispPlugin.Bundle.wispscene" );
            sceneAssetBundle = AssetBundle.LoadFromStream( stream );
        }
    }

}
#endif