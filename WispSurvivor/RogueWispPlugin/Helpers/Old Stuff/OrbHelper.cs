using RoR2.Orbs;
using System.Reflection;
using UnityEngine;

namespace Rein.RogueWispPlugin
{
    internal partial class Main
    {
        public static class OrbHelper
        {
            public static void ConvertOrbSettings( GameObject g )
            {
                OrbEffect orb = g.GetComponent<OrbEffect>();

                WispOrbEffect newOrb = g.AddComponent<WispOrbEffect>();

                FieldInfo f2;

                foreach( FieldInfo f in typeof( OrbEffect ).GetFields( BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic ) )
                {
                    f2 = typeof( WispOrbEffect ).GetField( f.Name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic );
                    f2.SetValue( newOrb, f.GetValue( orb ) );
                }

                MonoBehaviour.Destroy( orb );
            }
        }
    }
}
