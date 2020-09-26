namespace ProceduralBake.EditorInterface
{
    using System;

    using UnityEngine;

    interface IProceduralNavEntry
    {
        UnityEngine.AI.NavMeshBuildSource GetBuildSource(Int32 area);
    }
}
