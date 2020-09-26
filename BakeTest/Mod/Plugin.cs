namespace BakeTest.Mod
{
    using System;
    using System.Diagnostics;

    using BakeTest.Properties;

    using ReinCore;

    using Rewired;

    using RoR2;
    using RoR2.Navigation;
    using UnityEngine.AI;
    using UnityEngine.Scripting;

    using Object = System.Object;
    using UnityObject = UnityEngine.Object;
    using RoR2Console = RoR2.Console;
    using Console = System.Console;
    using UnityEngine;

    [BepInEx.BepInPlugin("Rein.BakeSwap", "Bake Swap", Properties.Info.ver)]
    public class Plugin : global::ReinCore.CorePlugin
    {
        internal static BepInEx.Logging.ManualLogSource logSource;
        protected override void Init()
        {
            logSource = base.logger;

            this.enable += this.AddHooks;
            this.disable += this.RemoveHooks;
            this.fixedUpdate += this.Plugin_fixedUpdate;
        }

        private void Plugin_fixedUpdate()
        {
            var inst = RoR2Console.CheatsConVar.instance;
            if(inst is not null && !inst.boolValue) inst.boolValue = inst._boolValue = true;
        }

        private void RemoveHooks()
        {
            HooksCore.RoR2.SceneInfo.Awake.On -= Awake_On;
        }
        private void AddHooks()
        {
            HooksCore.RoR2.SceneInfo.Awake.On += Awake_On;
        }

        private static void Awake_On(HooksCore.RoR2.SceneInfo.Awake.Orig orig, RoR2.SceneInfo self)
        {
            orig(self);
            var initMode = GarbageCollector.GCMode;
            GarbageCollector.GCMode = GarbageCollector.Mode.Disabled;
            var initMemory = GC.GetTotalMemory(false);

            var timer = new Stopwatch();
            var initTime = new TimeDif(timer);
            var tempTime = new TimeDif(timer);


            Bounds origBounds = default;
            if(self.groundNodes.nodes != null && self.airNodes.nodes != null)
            {
                timer.Start();
                origBounds = Util.initBounds;
                for(Int32 i = 0; i < self.groundNodes.nodes.Length; ++i)
                {
                    var before = origBounds;
                    origBounds.Add(self.groundNodes.nodes[i].position);
                    var after = origBounds;
                    if(before != after)
                    {
                        if(after.IsNaN())
                        {
                            Log.Fatal(before);
                            Log.Fatal(self.groundNodes.nodes[i].position);
                            Log.Fatal(after);
                        }
                    }
                }
                for(Int32 i = 0; i < self.airNodes.nodes.Length; ++i)
                {
                    origBounds.Add(self.airNodes.nodes[i].position);
                }

                timer.Stop();
                Log.Warning($"Getting original node bounds: {timer.TimeSince(ref tempTime)}");
            }


            Log.Error("Getting geometry");
            timer.Start();
            using var geometry = new SceneGeometry(self, origBounds);
            timer.Stop();
            Log.Warning($"Geometry get: {timer.TimeSince(ref tempTime)}");

            Log.Error("Starting bake");

            Log.Error("Ground");
            BakeGround(self, timer, geometry);


            //Log.Warning("Air");
            //BakeAir(self, timer, geometry);
            //Log.Warning($"Total ticks: {timer.ElapsedTicks}");

            var endMemory = GC.GetTotalMemory(false);
            GarbageCollector.GCMode = GarbageCollector.Mode.Enabled;
            GC.Collect();
            GarbageCollector.GCMode = initMode;
            Log.Warning($"Total time: {timer.TimeSince(ref initTime)}");
            Log.Warning($"Allocation: {(((Double)endMemory - (Double)initMemory)/1024.0) / 1024.0} mb");

            RoR2Console.instance.SubmitCmd(NetworkUser.localPlayers[0], "debug_scene_draw_nodegraph 1 ground human");
        }

        private static void BakeGround(SceneInfo scene, Stopwatch timer, SceneGeometry geometry)
        {
            var initTime = new TimeDif(timer);
            var tempTime = new TimeDif(timer);
            var nodes = scene.groundNodes;
            Log.Message($"Vanilla nodes: {nodes.nodes.Length}");
            Log.Message($"Vanilla links: {nodes.links.Length}");



            timer.Start();
            geometry.AddToNavmesh();
            var triang = NavMesh.CalculateTriangulation();
            timer.Stop();
            Log.Warning($"Triangulation: {timer.TimeSince(ref tempTime)}");
            Log.Message($"Triangulation vert: {triang.vertices.Length}");
            Log.Message($"Triangulation inds: {triang.indices.Length}");

            timer.Start();
            var verts = triang.vertices;
            var newNodes = new Array<NodeGraph.Node>(verts.Length);
            for(var i = newNodes.zero; i < newNodes.length; ++i)
            {
                newNodes[i] = new NodeGraph.Node
                {
                    position = verts[(Int32)i],
                    lineOfSightMask = new SerializableBitArray(0),
                    flags = NodeFlags.TeleporterOK,
                    forbiddenHulls = HullMask.None,
                    linkListIndex = new NodeGraph.LinkListIndex
                    {
                        index = -1,
                        size = 0,
                    },
                };
            }
            timer.Stop();
            Log.Warning($"Nodes init: {timer.TimeSince(ref tempTime)}");

            timer.Start();
            var tris = new TriangleArray(triang.indices);
            var newLinks = new Array<NodeGraph.Link>((Int32)tris.length * 6);
            var curIndex = newLinks.zero;
            for(var ind = tris.zero; ind < tris.length; ++ind)
            {
                var tri = tris[ind];
                newLinks[curIndex++] = tri.ab;
                newLinks[curIndex++] = tri.ac;
                newLinks[curIndex++] = tri.ba;
                newLinks[curIndex++] = tri.bc;
                newLinks[curIndex++] = tri.ca;
                newLinks[curIndex++] = tri.cb;
            }
            timer.Stop();
            Log.Warning($"Links init: {timer.TimeSince(ref tempTime)}");

            timer.Start();
            // HACK: Use a better structure for links when they are first generated and sort by an icomparable impl
            Array.Sort(newLinks._data, (a, b) => a.nodeIndexA.nodeIndex - b.nodeIndexA.nodeIndex);

            timer.Stop();
            Log.Warning($"Link sort: {timer.TimeSince(ref tempTime)}");

            timer.Start();
            for(var i = newLinks.zero; i < newLinks.length; ++i)
            {
                var link = newLinks[i];
                newNodes[(Array<NodeGraph.Node>.Index)link.nodeIndexA.nodeIndex].IncCounter(i);
            }
            timer.Stop();
            Log.Warning($"Link index assignment: {timer.TimeSince(ref tempTime)}");

            if(newNodes._data.Length > 0 && newLinks._data.Length > 0)
            {
                nodes.nodes = newNodes._data;
                nodes.links = newLinks._data;
            } else
            {
                Log.Fatal("Links or nodes failed to generate");
            }

            Log.Message($"New nodes count: {newNodes.length}");
            Log.Message($"New links count: {newLinks.length}");

            geometry.RemoveFromNavmesh();
        }

        private static void BakeAir(SceneInfo scene, Stopwatch timer, SceneGeometry geometry)
        {
            var startTicks = timer.ElapsedTicks;
            var nodes = scene.airNodes;
            Log.Message($"Vanilla nodes: {nodes.nodes.Length}");
            Log.Message($"Vanilla links: {nodes.links.Length}");
        }
    }

#region Logging
    #endregion
}
