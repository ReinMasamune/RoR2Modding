using System;
using UnityEngine;
using BepInEx;
using System.Reflection;
using System.Collections.Generic;

namespace CopyObject
{
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("com.ReinThings.CopyObject", "CopyObject", "1.0.0")]

    public class CopyObject : BaseUnityPlugin
    {
        public GameObject copyGameObject( GameObject g )
        {
            Dictionary<UnityEngine.Object, UnityEngine.Object> dict1 = new Dictionary<UnityEngine.Object, UnityEngine.Object>();
            Dictionary<object, object> dict2 = new Dictionary<object, object>();

            return MakeCopy<GameObject>(g,dict1,dict2);
        }

        public T MakeCopy<T>( T thing , Dictionary<UnityEngine.Object,UnityEngine.Object> unityList , Dictionary<object,object> nonUnityList )
        {
            if( typeof(T).IsValueType || typeof(T) == typeof(string) )
            {
                return thing;
            }

            var unityThing = thing as UnityEngine.Object;
            bool isUnity = unityThing != null;
            var nonUnityThing = thing as object;
            bool isGameObject = unityThing && typeof(T) == typeof(GameObject);

            //Check Dictionary to see if there already is a copy, return that copy if there is and end here
            //Also going to need a blacklist for some types that will break things

            //Make new varible with the type of thing

            if( isUnity )
            {
                //New var = Instantiate( thing )
                //Add to dictionary
            }
            else
            {
                //Initalize and use constructor? Idk
                //Add to dictionary
            }

            if( isGameObject )
            {
                //Need to see if GetComponentsInChildren works on parent as well, unsure...
                foreach( Component C in ( unityThing as GameObject ).GetComponents<Component>() )
                {
                    //If the new varible has the component, call MakeCopy on it.
                    //If It doesnt, add component and call MakeCopy
                }
                foreach( Component C in (unityThing as GameObject ).GetComponentsInChildren<Component>() )
                {
                    //Need the gameobject ref for both the new varible and the old one here, pain in the ass
                    //See above, same thing
                }
            }
            else
            {
                //Loop through all fields and do MakeCopy on them
            }

            return thing;
        }
    }
}
