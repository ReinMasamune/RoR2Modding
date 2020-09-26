namespace BakeTest.Mod
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    using RoR2.Navigation;

    using UnityEngine;
    using UnityEngine.AI;



    internal static class List_Ex
    {
        internal static void Clear<T>(this List<T> self) => self.Clear();
    }

    internal static class Util
    {
        internal static Vector3 NaNV3 => new Vector3(Single.NaN, Single.NaN, Single.NaN);
        internal static Bounds initBounds => new Bounds(NaNV3, NaNV3);

        internal static T Pipe<T>(this Action<T> self, T arg)
        {
            self(arg);
            return arg;
        }

        internal static Vector3 ScaleBy(this Vector3 self, Vector3 by) => Vector3.Scale(self, by);

        internal static Boolean IsNaN(this Vector3 vec) => Single.IsNaN(vec.x) || Single.IsNaN(vec.y) || Single.IsNaN(vec.z);

        internal static Boolean IsNaN(this Bounds bounds) => bounds.center.IsNaN() || bounds.extents.IsNaN();

        internal static void Add(ref this Bounds self, Bounds added)
        {
            if(self.IsNaN())
            {
                self = added;
            } else
            {
                self.min = Vector3.Min(self.min, added.min);
                self.max = Vector3.Max(self.max, added.max);
            }
        }

        internal static void Add(ref this Bounds self, Vector3 added)
        {
            if(self.IsNaN())
            {
                self = new Bounds(added, Vector3.zero);
            } else
            {
                self.min = Vector3.Min(self.min, added);
                self.max = Vector3.Max(self.max, added);
            }
        }

        internal static Boolean Overlaps(in this Bounds self, Bounds item) =>
            (item.min.x <= self.max.x) && (item.max.x >= self.min.x) &&
            (item.min.y <= self.max.y) && (item.max.y >= self.min.y) &&
            (item.min.z <= self.max.z) && (item.max.z >= self.min.z);

        internal static Bounds MultSize(in this Bounds self, Vector3 mult) => new(self.center, Vector3.Scale(self.extents, mult));
        internal static Bounds MultSize(in this Bounds self, Single mult) => self.MultSize(new Vector3(mult, mult, mult));

        internal static Bounds LocalTo(in this Bounds self, Vector3 position) => new(self.center - position, self.extents);

        internal static Boolean InRange(this Single self, Single min, Single max) => self <= max && self >= min;


        internal static TimeDif TimeSince(this Stopwatch self, ref TimeDif temp) => new(temp, temp = new(self));

        internal static void IncCounter(ref this NodeGraph.Node self, Array<NodeGraph.Link>.Index index)
        {
            if(self.linkListIndex.index == -1) self.linkListIndex.index = (Int32)index;
            self.linkListIndex.size++;
        }

        internal static NavMeshBuildSource GetSource(Collider col, Int32 area) => col switch
        {
            MeshCollider mesh => new NavMeshBuildSource
            {
                shape = NavMeshBuildSourceShape.Mesh,
                area = area,
                component = col,
                sourceObject = mesh.sharedMesh,
                transform = col.transform.localToWorldMatrix,
            },
            TerrainCollider terrain => new NavMeshBuildSource
            {
                shape = NavMeshBuildSourceShape.Terrain,
                area = area,
                component = col,
                sourceObject = terrain.terrainData,
                transform = col.transform.localToWorldMatrix,
            },
            BoxCollider box => new NavMeshBuildSource
            {
                shape = NavMeshBuildSourceShape.Box,
                area = area,
                component = col,
                size = box.size,
                transform = col.transform.localToWorldMatrix,
            },
            CapsuleCollider capsule => new NavMeshBuildSource
            {
                shape = NavMeshBuildSourceShape.Capsule,
                area = area,
                component = col,
                size = new Vector3(capsule.radius, capsule.height, 0f),
                transform = col.transform.localToWorldMatrix,
            },
            SphereCollider sphere => new NavMeshBuildSource
            {
                shape = NavMeshBuildSourceShape.Sphere,
                area = area,
                component = col,
                size = new Vector3(sphere.radius, 0f, 0f),
                transform = col.transform.localToWorldMatrix,
            },
            _ => default,
        };
    }
}
