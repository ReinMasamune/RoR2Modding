using BepInEx;
using RoR2;
using UnityEngine;
using System.Collections.Generic;
using RoR2.Navigation;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using System.Reflection;
using EntityStates;
using RoR2.Skills;
using System.Collections;
using ReinCore;

namespace ReinGeneralFixes
{
    internal partial class Main
    {
        partial void BalanceBlight()
        {
            this.Enable += this.Main_Enable3;
            this.Disable += this.Main_Disable3;
        }

        private void Main_Disable3()
        {

        }
        private void Main_Enable3()
        {

        }
    }
}
