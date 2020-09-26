namespace BakeTest.Mod
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;

    using RoR2;

    using UnityEngine;
    using UnityEngine.AI;
    using UnityEngine.SceneManagement;

    internal class SceneGeometry : IDisposable
    {
        private static NavMeshBuildSettings buildSettings { get; } = new NavMeshBuildSettings
        {
            agentRadius = 0.5f,
            agentHeight = 2f,
            agentClimb = 0.5f,
            agentSlope = 90f,
            agentTypeID = 0,
            minRegionArea = 20f,
            voxelSize = 0.13f,
            tileSize = 64,
            debug = new NavMeshBuildDebugSettings
            {
                flags = NavMeshBuildDebugFlags.All,
            },
            overrideTileSize = true,
            overrideVoxelSize = true,
        };

        private NavMeshData data { get; } = new();

        //private RentList<NavMeshData> allData { get; set; } = ListRental<NavMeshData>.GetRent();
        //private RentList<NavMeshDataInstance> dataHandles { get; set; } = default;
        public NavMeshDataInstance handle { get; private set; }

        private readonly Bounds clip;

        internal SceneGeometry(SceneInfo scene, Bounds clipTo)
        {
            this.clip = clipTo;
            Log.Warning(clipTo);
            var roots = SceneManager.GetActiveScene().GetRootGameObjects();
            Int32 areaInd = 0;

            BoundsDraw.currentBounds = clipTo;

            //this.data = NavMeshBuilder.BuildNavMeshData(buildSettings, )
            
            for(Int32 i = 0; i < roots.Length; ++i)
            {
                var obj = roots[i];
                if(obj is not null && obj)
                {
                    var localBounds = new Bounds();
                    using var allResults = ListRental<NavMeshBuildSource>.GetRent();
                    using var colliders = ListRental<Collider>.GetRent();
                    obj.GetComponentsInChildren<Collider>(colliders);
                    for(Int32 j = 0; j < colliders.Count; ++j)
                    {
                        var col = colliders[j];
                        if(col is null) continue;
                        if(col.gameObject.layer != LayerIndex.world.intVal) continue;
                        if(col.isTrigger) continue;
                        if(col.GetComponent<Rigidbody>()) continue;
                        if(col is MeshCollider mesh && mesh?.sharedMesh?.isReadable != true)
                        {
                            Log.Error($"Non readable mesh found on gameobject: {col.gameObject.name}");
                            continue;
                        }
                        if(!col.bounds.Overlaps(clipTo)) continue;
                        

                        allResults.Add(Util.GetSource(col, areaInd++));
                        localBounds.Add(col.bounds);
                        var newData = NavMeshBuilder.UpdateNavMeshData(this.data, buildSettings, allResults, clipTo);
                        //newData.
                        //this.allData.Add(newData);
                    }
                    if(!localBounds.Overlaps(clipTo))
                    {
                        localBounds = new();
                        continue;
                    }
                    //if(allResults.Count > 0) this.allData.Add(NavMeshBuilder.BuildNavMeshData(buildSettings, allResults, Bounds., Vector3.zero, Quaternion.identity));
                    localBounds = new();
                }
            }
        }

        internal void AddToNavmesh()
        {
            this.handle = NavMesh.AddNavMeshData(this.data);
            //if(this.dataHandles.list is not null) throw new InvalidOperationException("Cannot add to navmesh multiple times");
            //this.dataHandles = ListRental<NavMeshDataInstance>.GetRent();
            
            //for(Int32 i = 0; i < this.allData.Count; ++i) this.dataHandles.Add(NavMesh.AddNavMeshData(this.allData[i]));
        }

        internal void RemoveFromNavmesh()
        {
            NavMesh.RemoveNavMeshData(this.handle);
            //if(this.dataHandles.list is null) throw new InvalidOperationException("Cannot remove from navmesh without adding");
            //for(Int32 i = 0; i < this.dataHandles.Count; ++i) NavMesh.RemoveNavMeshData(this.dataHandles[i]);
            //this.dataHandles.Dispose();
        }

        public void Dispose()
        {
            if(this.handle.valid)
            {
                this.RemoveFromNavmesh();
            }
        }
    }
}
