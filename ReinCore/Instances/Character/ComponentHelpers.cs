//using System;
//using System.Linq.Expressions;
//using System.Reflection;
//using BepInEx;
//using RoR2;
//using UnityEngine;

//namespace ReinCore
//{
//    public class CharacterDef : ScriptableObject
//    {
//        public static CharacterDef Create(Boolean isSurvivor, GameObject prefab = null)
//        {
//            var def = ScriptableObject.CreateInstance<CharacterDef>();

//            return def;
//        }

//        [field: SerializeField]
//        public GameObject prefab { get; private set; }

//        [field: SerializeField]
//        public CharacterBody body { get; private set; }

//        [field: SerializeField]
//        public Boolean isSurvivor { get; private set; }




//    }
//}
