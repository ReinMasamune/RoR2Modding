namespace ReinCore
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;
    using BepInEx;
    using UnityEngine;

    /// <summary>
    /// 
    /// </summary>
    public static class ParticleSystemExtensions
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static void PlayOnStart(this ParticleSystem ps)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            GameObject obj = ps.gameObject;
            ParticleSystemPlayOnStart comp = obj.AddComponent<ParticleSystemPlayOnStart>();
            comp.SetTargets( ps );
        }
    }
}
