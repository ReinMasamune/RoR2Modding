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
            var curScene = SceneManager.GetActiveScene().path;
            if( scenePrefabs.ContainsKey( curScene ) )
            {
                for( Int32 i = 0; i < scenePrefabs[curScene].Count; i++ )
                {
                    var prefab = scenePrefabs[curScene][i];
                    UnityEngine.Object.Instantiate<GameObject>( prefab, Vector3.zero, Quaternion.identity );
                }
            }
        }
    }
}