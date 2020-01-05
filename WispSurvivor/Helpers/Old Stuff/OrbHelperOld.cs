using R2API.Utils;
using RoR2.Orbs;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
/*
namespace RogueWispPlugin.Helpers.OldStuff
{
    public static class OrbHelper
    {
        private static Boolean eventRegistered = false;
        private static List<Type> orbs = new List<Type>();

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

        public static void ConvertOrbSettings( GameObject g )
        {
            OrbEffect orb = g.GetComponent<OrbEffect>();

            Components.WispOrbEffect newOrb = g.AddComponent<Components.WispOrbEffect>();

            FieldInfo f2;

            foreach( FieldInfo f in typeof( OrbEffect ).GetFields( BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic ) )
            {
                f2 = typeof( Components.WispOrbEffect ).GetField( f.Name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic );
                f2.SetValue( newOrb, f.GetValue( orb ) );
            }

            MonoBehaviour.Destroy( orb );
        }
    }
}
*/