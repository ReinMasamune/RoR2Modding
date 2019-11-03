using UnityEngine;
using R2API.Utils;
using System;
using System.Collections.Generic;
using RoR2.Orbs;

namespace WispSurvivor.Util
{
    public static class OrbHelper
    {
        private static bool eventRegistered = false;
        private static List<Type> orbs = new List<Type>();

        public static bool AddOrb(Type t )
        {
            if(t == null || !t.IsSubclassOf( typeof(Orb) ) )
            {
                Debug.Log("Type is not based on Orb or is null");
                return false;
            }

            RegisterEvent();

            orbs.Add(t);

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

        private static void AddCustomOrbs(On.RoR2.Orbs.OrbCatalog.orig_GenerateCatalog orig)
        {
            orig();

            Type[] orbCat = typeof(OrbCatalog).GetFieldValue<Type[]>("indexToType");
            Dictionary<Type, int> typeToIndex = typeof(OrbCatalog).GetFieldValue<Dictionary<Type, int>>("typeToIndex");

            int origLength = orbCat.Length;
            int extraLength = orbs.Count;

            Array.Resize<Type>(ref orbCat, origLength + extraLength);

            int temp;

            for( int i = 0; i < extraLength; i++ )
            {
                temp = i + origLength;
                orbCat[temp] = orbs[i];
                typeToIndex.Add(orbs[i], temp);
            }

            typeof(OrbCatalog).SetFieldValue<Type[]>("indexToType", orbCat);
            typeof(OrbCatalog).SetFieldValue<Dictionary<Type, int>>("typeToIndex",typeToIndex);
        }
    }
}
