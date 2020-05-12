namespace ReinCore
{
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
