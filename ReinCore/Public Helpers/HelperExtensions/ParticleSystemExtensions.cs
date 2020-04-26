using System;
using System.Linq.Expressions;
using System.Reflection;
using BepInEx;
using UnityEngine;

namespace ReinCore
{
    /// <summary>
    /// 
    /// </summary>
    public static class ParticleSystemExtensions
    {
        public static void PlayOnStart(this ParticleSystem ps)
        {
            var obj = ps.gameObject;
            var comp = obj.AddComponent<ParticleSystemPlayOnStart>();
            comp.SetTargets( ps );
        }
    }
}
