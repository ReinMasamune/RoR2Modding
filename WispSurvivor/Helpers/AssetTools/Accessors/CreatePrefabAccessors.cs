
using RogueWispPlugin.Helpers;
using UnityEngine;

namespace RogueWispPlugin
{
    internal partial class Main
    {
        partial void CreatePrefabAccessors()
        {
            #region Reference Prefabs
            new GenericAccessor<GameObject>( PrefabIndex.refWillOWispExplosion,                 () =>
            {
                return Resources.Load<GameObject>( "Prefabs/Effects/WillOWispExplosion" );
            }, false, ExecutionState.Awake ).RegisterAccessor();
            new GenericAccessor<GameObject>( PrefabIndex.refNullifierDeathExplosion,            () =>
            {
                return Resources.Load<GameObject>( "Prefabs/Effects/NullifierDeathExplosion" );
            }, false, ExecutionState.Awake ).RegisterAccessor();
            new GenericAccessor<GameObject>( PrefabIndex.refNullifierPreBombGhost,              () =>
            {
                return Resources.Load<GameObject>( "Prefabs/ProjectileGhosts/NullifierPreBombGhost" );
            }, false, ExecutionState.Awake ).RegisterAccessor();
            new GenericAccessor<GameObject>( PrefabIndex.refLockedMage,                         () =>
            {
                return Resources.Load<GameObject>( "Prefabs/NetworkedObjects/LockedMage" );
            }, false, ExecutionState.Awake ).RegisterAccessor();
            new GenericAccessor<GameObject>( PrefabIndex.refFireTornadoGhost,                   () =>
            {
                return Resources.Load<GameObject>( "Prefabs/ProjectileGhosts/FireTornadoGhost" );
            }, false, ExecutionState.Awake ).RegisterAccessor();
            new GenericAccessor<GameObject>( PrefabIndex.refTitanRechargeRocksEffect,           () =>
            {
                return Resources.Load<GameObject>( "Prefabs/Effects/TitanRechargeRocksEffect" );
            }, false, ExecutionState.Awake ).RegisterAccessor();
            #endregion
        }
    }
}
