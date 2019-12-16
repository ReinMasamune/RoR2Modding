using R2API.Utils;
using RoR2.Orbs;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace WispSurvivor.Helpers
{
    public static class OrbHelper
    {
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
