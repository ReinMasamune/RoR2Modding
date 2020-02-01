
using RogueWispPlugin.Helpers;
using UnityEngine;

namespace RogueWispPlugin
{
    internal partial class Main
    {
        partial void CreatePrefabAccessors()
        {
            #region Reference Prefabs
            new GenericAccessor<GameObject>( PrefabIndex.refWillOWispExplosion, () =>
            {
                return Resources.Load<GameObject>( "Prefabs/" );
            }, false, ExecutionState.Awake ).RegisterAccessor();

            new GenericAccessor<GameObject>( PrefabIndex.refNullifierExplosion, () =>
            {
                return Resources.Load<GameObject>( "Prefabs/Effects/NullifierDeathExplosion" );
            }, false, ExecutionState.Awake ).RegisterAccessor();

            new GenericAccessor<GameObject>( PrefabIndex.refNullifierPreBomb, () =>
            {
                return Resources.Load<GameObject>( "Prefabs/ProjectileGhosts/NullifierPreBombGhost" );
            }, false, ExecutionState.Awake ).RegisterAccessor();
            #endregion
        }
    }
}
