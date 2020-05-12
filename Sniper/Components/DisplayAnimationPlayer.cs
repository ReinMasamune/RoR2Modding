namespace Sniper.Components
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using BepInEx.Logging;
    using ReinCore;
    using RoR2;
    using UnityEngine;

    internal class DisplayAnimationPlayer : MonoBehaviour
    {
        private void Awake()
        {
            Animator animator = base.GetComponentInChildren<Animator>();
            animator.Play( "DisplayEnter" );
        }
    }
}
