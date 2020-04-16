using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BepInEx.Logging;
using ReinCore;
using RoR2;
using UnityEngine;

namespace Sniper.Components
{
    internal class DisplayAnimationPlayer : MonoBehaviour
    {
        private void Awake()
        {
            var animator = base.GetComponentInChildren<Animator>();
            animator.Play( "DisplayEnter" );
        }
    }
}
