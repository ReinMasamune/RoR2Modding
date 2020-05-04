using System;
using BepInEx;
using BepInEx.Logging;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Reflection;
using System.Linq.Expressions;
using Unity.Jobs;
using UnityEngine;

namespace ReinCore
{
    public interface ITextureJob : IJobParallelFor
    {
        Texture2D OutputTextureAndDispose();
    }
}
