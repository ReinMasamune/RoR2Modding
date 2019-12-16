using System;
using System.Collections.Generic;
using System.Text;
using BepInEx;
using UnityEngine;

namespace RoR2Plugin
{
    // TODO: Util docs
    public static class Util
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="g"></param>
        /// <returns></returns>
        public static bool HasComponent<T>( this GameObject g ) where T : Component
        {
            return g.GetComponent( typeof( T ) ) != null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="m"></param>
        /// <returns></returns>
        public static bool HasComponent<T>( this MonoBehaviour m ) where T : Component
        {
            return m.GetComponent( typeof( T ) ) != null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool HasComponent<T>( this Transform t ) where T : Component
        {
            return t.GetComponent( typeof( T ) ) != null;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="g"></param>
        /// <returns></returns>
        public static int ComponentCount<T>( this GameObject g ) where T : Component
        {
            return g.GetComponents( typeof( T ) ).Length;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="m"></param>
        /// <returns></returns>
        public static int ComponentCount<T>( this MonoBehaviour m ) where T : Component
        {
            return m.GetComponents( typeof( T ) ).Length;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static int ComponentCount<T>( this Transform t ) where T : Component
        {
            return t.GetComponents( typeof( T ) ).Length;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="g"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static T GetComponent<T>( this GameObject g, int index ) where T : Component
        {
            return g.GetComponents( typeof( T ) )[index] as T;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="m"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static T GetComponent<T>( this MonoBehaviour m, int index ) where T : Component
        {
            return m.gameObject.GetComponents( typeof( T ) )[index] as T;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static T GetComponent<T>( this Transform t, int index ) where T : Component
        {
            return t.GetComponents( typeof( T ) )[index] as T;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="m"></param>
        /// <returns></returns>
        public static T AddComponent<T>( this MonoBehaviour m ) where T : Component
        {
            return m.gameObject.AddComponent( typeof( T ) ) as T;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static T AddComponent<T>( this Transform t ) where T : Component
        {
            return t.gameObject.AddComponent( typeof( T ) ) as T;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="g"></param>
        /// <returns></returns>
        public static T AddOrGetComponent<T>( this GameObject g ) where T : Component
        {
            return (g.HasComponent<T>() ? g.GetComponent( typeof( T ) ) : g.AddComponent( typeof( T ) )) as T;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="m"></param>
        /// <returns></returns>
        public static T AddOrGetComponent<T>( this MonoBehaviour m ) where T : Component
        {
            return (m.HasComponent<T>() ? m.GetComponent( typeof( T ) ) : m.gameObject.AddComponent( typeof( T ) )) as T;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static T AddOrGetComponent<T>( this Transform t ) where T : Component
        {
            return (t.HasComponent<T>() ? t.GetComponent( typeof( T ) ) : t.gameObject.AddComponent( typeof( T ) )) as T;
        }

        /// <summary>
        /// Simply logs the values of various material properties to console.
        /// Primarily useful for changing textures.
        /// </summary>
        /// <param name="m"></param>
        public static void DebugMaterialInfo( this Material m )
        {
            Debug.Log( "Material name: " + m.name );
            string[] s = m.shaderKeywords;
            Debug.Log( "Shader keywords" );
            for( int i = 0; i < s.Length; i++ )
            {
                Debug.Log( s[i] );
            }

            Debug.Log( "Shader name: " + m.shader.name );

            Debug.Log( "Texture Properties" );
            string[] s2 = m.GetTexturePropertyNames();
            for( int i = 0; i < s2.Length; i++ )
            {
                Debug.Log( s2[i] + " : " + m.GetTexture( s2[i] ) );
            }
        }
    }
}
