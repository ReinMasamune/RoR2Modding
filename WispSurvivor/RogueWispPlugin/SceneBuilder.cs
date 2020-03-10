using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BundledScripts
{
    public class SceneBuilder : MonoBehaviour
    {
        public static void AddScenePrefab( String path, GameObject prefab )
        {
            if( !scenePrefabs.ContainsKey( path ) )
            {
                scenePrefabs[path] = new List<GameObject>();
            }
            RogueWispPlugin.Main.LogW( "Prefab added: " + path );
            scenePrefabs[path].Add( prefab );
        }
        public static void ClearScenePrefabs( String path )
        {
            if( !scenePrefabs.ContainsKey( path ) )
            {
                scenePrefabs[path] = new List<GameObject>();
            } else
            {
                scenePrefabs[path].Clear();
            }
        }

        private static Dictionary<String,List<GameObject>> scenePrefabs = new Dictionary<String, List<GameObject>>();

        private void Awake()
        {
            RogueWispPlugin.Main.LogW( "Builder awake" );
            var curScene = SceneManager.GetActiveScene().path;
            RogueWispPlugin.Main.LogW( curScene );
            if( scenePrefabs.ContainsKey( curScene ) )
            {
                RogueWispPlugin.Main.LogW( "Builder starting spawns" );
                foreach( var prefab in scenePrefabs[curScene] )
                {
                    UnityEngine.Object.Instantiate<GameObject>( prefab, Vector3.zero, Quaternion.identity );
                }
            }
        }
    }
}