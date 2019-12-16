using System;
using System.Collections.Generic;
using System.Text;
using BepInEx;
using R2API.Utils;
using RoR2.Orbs;
using UnityEngine;

namespace RoR2Plugin
{
    // TODO: Orb API docs
    public static class OrbAPI
    {
        private static Boolean eventRegistered = false;
        private static List<Type> orbs = new List<Type>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Boolean AddOrb( Type t )
        {
            if( t == null || !t.IsSubclassOf( typeof( Orb ) ) )
            {
                Debug.Log( "Type is not based on Orb or is null" );
                return false;
            }

            RegisterEvent();

            orbs.Add( t );

            return true;
        }

        private static void RegisterEvent()
        {
            if( eventRegistered )
            {
                return;
            }

            eventRegistered = true;

            On.RoR2.Orbs.OrbCatalog.GenerateCatalog += AddCustomOrbs;
        }

        private static void AddCustomOrbs( On.RoR2.Orbs.OrbCatalog.orig_GenerateCatalog orig )
        {
            orig();

            Type[] orbCat = typeof(OrbCatalog).GetFieldValue<Type[]>("indexToType");
            Dictionary<Type, Int32> typeToIndex = typeof(OrbCatalog).GetFieldValue<Dictionary<Type, Int32>>("typeToIndex");

            Int32 origLength = orbCat.Length;
            Int32 extraLength = orbs.Count;

            Array.Resize<Type>( ref orbCat, origLength + extraLength );

            Int32 temp;

            for( Int32 i = 0; i < extraLength; i++ )
            {
                temp = i + origLength;
                orbCat[temp] = orbs[i];
                typeToIndex.Add( orbs[i], temp );
            }

            typeof( OrbCatalog ).SetFieldValue<Type[]>( "indexToType", orbCat );
            typeof( OrbCatalog ).SetFieldValue<Dictionary<Type, Int32>>( "typeToIndex", typeToIndex );
        }
    }
}
