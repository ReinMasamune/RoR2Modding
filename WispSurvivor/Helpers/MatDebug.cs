using BepInEx;
using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using R2API;
using UnityEngine;
using Unity.Collections;
using System.Diagnostics;
using Unity.Jobs;

namespace RogueWispPlugin.Helpers
{
    internal static class DumpMaterialInfo
    {
        internal static void DumpInfo( Material m )
        {
            Main.LogI( "name = " + m.name );
            Main.LogI( "GIFlags = " + m.globalIlluminationFlags );
            Main.LogI( "DoubleSided GI = " + m.doubleSidedGI );
            Main.LogI( "Instancing = " + m.enableInstancing );
            Main.LogI( "RenderQueue = " + m.renderQueue );
            Main.LogI( "Shader = " + m.shader.name );
            var passCount = m.passCount;
            Main.LogI( "Pass Count = " + passCount );
            for( Int32 i = 0; i < passCount; ++i )
            {
                var name = m.GetPassName( i );
                Main.LogI( "Pass: " + i + " = " + name + " enabled = " + m.GetShaderPassEnabled( name ) );
            }


        }
    }
}
