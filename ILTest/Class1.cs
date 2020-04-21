using System;
using System.Runtime.CompilerServices;
using System.Reflection;
using BepInEx;
using RoR2;

namespace ILTest
{
    [BepInPlugin( "aGuidGoesHere", "A mod", "1.0.0")]
    public class Class1 : BaseUnityPlugin
    {
        public void Start()
        {
            Chat.AddMessage( "Testing" );
        }


        [MethodImpl( MethodImplOptions.ForwardRef )]
        public extern int Square( int number );
    }
}
