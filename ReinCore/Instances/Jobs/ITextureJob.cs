namespace ReinCore
{
    using System;
    using BepInEx;
    using BepInEx.Logging;
    using System.Runtime.CompilerServices;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Linq.Expressions;
    using Unity.Jobs;
    using UnityEngine;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public interface ITextureJob : IJobParallelFor
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        Texture2D OutputTextureAndDispose();
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
