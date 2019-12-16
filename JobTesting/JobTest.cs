using BepInEx;
using RoR2;
using RoR2.Navigation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;
using Unity.Burst;
using System.Numerics;
using R2API.Utils;

namespace JobTesting
{
    [BepInDependency( "com.bepis.r2api" )]
    [BepInPlugin( "com.ReinThings.JobTest", "Rein-JobTest", "1.0.0" )]
    public class JobTest : BaseUnityPlugin
    {
        private bool running = false;

        private Int64 prevTime = 0u;

        private NativeArray<Vector3> tpCheckOffsets;

        private HullData humanData = new HullData
        {
            hullMask = HullMask.Human,
            height = 2f,
            radius = 0.5f
        };

        private HullData golemData = new HullData
        {
            hullMask = HullMask.Golem,
            height = 8f,
            radius = 1.8f
        };

        private HullData queenData = new HullData
        {
            hullMask = HullMask.BeetleQueen,
            height = 20f,
            radius = 5f
        };

        public struct Node
        {
            public Vector3 position;

            public NodeFlags flags;

            public HullMask forbiddenHulls;

            public String gateName;
        }

        public struct Link
        {
            public Boolean valid;
            public Boolean hasLOS;

            public Int32 node1ID;
            public Int32 node2ID;

            public Single distanceScore;
            public Single minJumpHeight;

            public HullMask hullMask;
            public HullMask jumpHullMask;

            public String gateName;
        }

        public struct HullData
        {
            public HullMask hullMask;
            public Single height;
            public Single radius;
        }

        public struct GenerateLinks : IJobParallelFor
        {
            [WriteOnly]
            public NativeArray<Link> _links;

            public Int32 count;

            public void Execute( Int32 index )
            {
                Int32 y = Mathf.FloorToInt(index / count);
                Int32 x = index % count;
                Boolean same = x == y;

                _links[index] = new Link
                {
                    node1ID = x,
                    node2ID = y,
                    valid = x != y
                };
            }
        }

        public struct LOSRayGen : IJobParallelFor
        {
            [ReadOnly]
            public NativeArray<Node> _nodes;
            [ReadOnly]
            public NativeArray<Link> _links;
            [WriteOnly]
            public NativeArray<RaycastCommand> _rays;

            public Int32 count;
            public Int32 mask;

            public void Execute( Int32 index )
            {
                var link = _links[index];

                if( !link.valid ) return;

                var node1 = _nodes[link.node1ID];
                var node2 = _nodes[link.node2ID];

                Vector3 diff = node2.position - node1.position;
                Single dist = Vector3.Magnitude( diff );

                _rays[index] = new RaycastCommand
                {
                    from = node1.position + Vector3.up,
                    direction = diff / dist,
                    distance = dist,
                    layerMask = mask,
                    maxHits = 1
                };
            }
        }

        public struct LOSRayRead : IJobParallelFor
        {
            public NativeArray<Link> _links;
            [ReadOnly]
            public NativeArray<RaycastHit> _hitRes;

            public void Execute( Int32 index )
            {
                var hit = _hitRes[index];
                var link = _links[index];
                link.hasLOS = hit.collider == null;
                _links[index] = link;
            }
        }

        public struct SkyRayGen : IJobParallelFor
        {
            [ReadOnly]
            public NativeArray<Node> _nodes;
            [WriteOnly]
            public NativeArray<RaycastCommand> _rays;

            public void Execute( Int32 index )
            {
                _rays[index] = new RaycastCommand
                {
                    from = _nodes[index].position,
                    direction = Vector3.up,
                    distance = Single.PositiveInfinity,
                    layerMask = LayerIndex.world.mask,
                    maxHits = 1
                };
            }
        }

        public struct SkyRayRead : IJobParallelFor
        {
            [ReadOnly]
            public NativeArray<RaycastHit> _hits;
            public NativeArray<Node> _nodes;

            public void Execute( Int32 index )
            {
                var node = _nodes[index];
                node.flags &= ~NodeFlags.NoCeiling;
                if( _hits[index].collider == null ) node.flags |= NodeFlags.NoCeiling;
                _nodes[index] = node;
            }
        }

        public struct TPRayGen : IJobParallelFor
        {
            [ReadOnly]
            public NativeArray<Node> _nodes;
            [WriteOnly]
            public NativeArray<RaycastCommand> _rays;
            [ReadOnly]
            public NativeArray<Vector3> _offsets;
            public Single range;

            public void Execute( Int32 index )
            {
                var off = index * _offsets.Length;
                var orig = _nodes[index].position;
                for( Int32 i = 0; i < _offsets.Length; i++ )
                {
                    _rays[off + i] = new RaycastCommand
                    {
                        from = orig + _offsets[i],
                        direction = Vector3.down,
                        distance = range,
                        layerMask = LayerIndex.world.mask,
                        maxHits = 1
                    };
                }
            }
        }

        public struct TPRayRead : IJobParallelFor
        {
            [ReadOnly]
            public NativeArray<RaycastHit> _hits;
            public NativeArray<Node> _nodes;

            public Int32 offCount;

            public void Execute( Int32 index )
            {
                var off = index * offCount;
                bool ok = true;
                var node = _nodes[index];
                node.flags &= ~NodeFlags.TeleporterOK;

                for( Int32 i = 0; i < offCount; i++ )
                {
                    if( _hits[off + i].collider == null )
                    {
                        ok = false;
                        break;
                    }
                }

                if( ok ) node.flags |= NodeFlags.TeleporterOK;

                _nodes[index] = node;
            }
        }

        public struct LinkDistScore : IJobParallelFor
        {
            [ReadOnly]
            public NativeArray<Node> _nodes;
            public NativeArray<Link> _links;

            public Single rangeFactor;

            public void Execute( Int32 index )
            {
                var link = _links[index];
                if( !link.valid ) return;
                var pos1 = _nodes[link.node1ID];
                var pos2 = _nodes[link.node2ID];
                Single mag = ( pos2.position - pos1.position ).sqrMagnitude;

                if( mag < rangeFactor )
                {
                    link.distanceScore = Mathf.Sqrt( mag );
                } else
                {
                    link.valid = false;
                }
                _links[index] = link;
            }
        }

        public struct GroundCastsGen : IJobParallelFor
        {
            [ReadOnly]
            public NativeArray<Node> _nodes;

            public NativeArray<Link> _links;

            [WriteOnly]
            public NativeArray<CapsulecastCommand> _caps;

            [ReadOnly]
            public HullData hull;

            [ReadOnly]
            public Vector3 fudge;


            public void Execute( Int32 index )
            {
                var link = _links[index];
                if( !link.valid ) return;
                var node1 = _nodes[link.node1ID];
                if( node1.forbiddenHulls.HasFlag( hull.hullMask ) )
                {
                    link.hullMask |= hull.hullMask;
                    link.jumpHullMask |= hull.hullMask;
                    _links[index] = link;
                    return;
                }
                var node2 = _nodes[link.node2ID];
                if( node2.forbiddenHulls.HasFlag( hull.hullMask ) )
                {
                    link.hullMask |= hull.hullMask;
                    link.jumpHullMask |= hull.hullMask;
                    _links[index] = link;
                    return;
                }
                var offset = index * 2;

                Vector3 bottomEdgeVec = Vector3.up * ( hull.height * 0.5f - hull.radius );
                Vector3 bottomPointVec = Vector3.up * (hull.height * 0.5f);
                Single radiusMod = hull.radius * 0.5f + 0.005f;


                Vector3 point = node1.position + fudge + Vector3.up * radiusMod;

                _caps[offset] = new CapsulecastCommand
                {
                    point1 = ( point + bottomEdgeVec ),
                    point2 = ( point - bottomEdgeVec ),
                    radius = hull.radius,
                    direction = Vector3.down,
                    distance = (radiusMod * 2f + 0.005f ),
                    layerMask = LayerIndex.world.mask
                };

                point = node2.position + fudge + Vector3.up * radiusMod;

                _caps[offset+1] = new CapsulecastCommand
                {
                    point1 = (point + bottomEdgeVec),
                    point2 = (point - bottomEdgeVec),
                    radius = hull.radius,
                    direction = Vector3.down,
                    distance = (radiusMod * 2f + 0.005f),
                    layerMask = LayerIndex.world.mask
                };
            }
        }

        public struct GroundCastsReadGen : IJobParallelFor
        {
            [ReadOnly]
            public NativeArray<RaycastHit> _hitRes;
            [ReadOnly]
            public NativeArray<Node> _nodes;
            public NativeArray<Link> _links;
            public NativeArray<Vector3> _groundPos;

            public NativeArray<CapsulecastCommand> _caps;

            [ReadOnly]
            public HullData hull;
            [ReadOnly]
            public Vector3 fudge;

            public void Execute( Int32 index )
            {
                var link = _links[index];
                if( !link.valid ) return;
                var node1 = _nodes[link.node1ID];
                if( node1.forbiddenHulls.HasFlag( hull.hullMask ) ) return;
                var node2 = _nodes[link.node2ID];
                if( node2.forbiddenHulls.HasFlag( hull.hullMask ) ) return;

                var offset = index * 2;
                var hit1 = _hitRes[offset];
                var hit2 = _hitRes[offset+1];

                Single radiusMod = hull.radius * 0.5f + 0.005f;
                Vector3 bottomEdgeVec = Vector3.up * ( hull.height * 0.5f - hull.radius );
                Vector3 bottomPointVec = Vector3.up * ( hull.height * 0.5f );


                Vector3 foot1 = node1.position + fudge + fudge + bottomPointVec;
                Vector3 foot2 = node2.position + fudge + fudge + bottomPointVec;

                if( hit1.collider != null )
                {
                    foot1 += Vector3.up * radiusMod + Vector3.down * hit1.distance;
                }

                if( hit2.collider != null )
                {
                    foot2 += Vector3.up * radiusMod + Vector3.down * hit2.distance;
                }

                _groundPos[offset] = foot1;
                _groundPos[offset + 1] = foot2;

                _caps[offset] = new CapsulecastCommand
                {
                    point1 = foot1 + bottomEdgeVec,
                    point2 = foot1 - bottomEdgeVec,
                    direction = Vector3.zero,
                    distance = 0f,
                    layerMask = LayerIndex.world.mask | LayerIndex.defaultLayer.mask,
                    radius = hull.radius
                };

                _caps[offset + 1] = new CapsulecastCommand
                {
                    point1 = foot2 + bottomEdgeVec,
                    point2 = foot2 - bottomEdgeVec,
                    direction = Vector3.zero,
                    distance = 0f,
                    layerMask = LayerIndex.world.mask | LayerIndex.defaultLayer.mask,
                    radius = hull.radius
                };
            }
        }

        public struct GroundCastsReadGen2 : IJobParallelFor
        {
            [ReadOnly]
            public NativeArray<Node> _nodes;
            public NativeArray<Link> _links;
            [ReadOnly]
            public NativeArray<RaycastHit> _hitRes;
            

            [ReadOnly]
            public HullData hull;

            public void Execute( Int32 index )
            {
                var link = _links[index];
                if( !link.valid ) return;
                var node1 = _nodes[link.node1ID];
                if( node1.forbiddenHulls.HasFlag( hull.hullMask ) ) return;
                var node2 = _nodes[link.node2ID];
                if( node2.forbiddenHulls.HasFlag( hull.hullMask ) ) return;

                var offset = index * 2;
                var hit1 = _hitRes[offset];
                if( _hitRes[offset].collider != null )
                {
                    link.hullMask |= hull.hullMask;
                    if( hull.hullMask == HullMask.Human )
                    {
                        link.hullMask |= HullMask.Golem;
                        link.hullMask |= HullMask.BeetleQueen;
                    } else if( hull.hullMask == HullMask.Golem )
                    {
                        link.hullMask |= HullMask.BeetleQueen;
                    }
                    _links[index] = link;
                    return;
                }
                if( _hitRes[offset+1].collider != null )
                {
                    link.hullMask |= hull.hullMask;
                    if( hull.hullMask == HullMask.Human )
                    {
                        link.hullMask |= HullMask.Golem;
                        link.hullMask |= HullMask.BeetleQueen;
                    } else if( hull.hullMask == HullMask.Golem )
                    {
                        link.hullMask |= HullMask.BeetleQueen;
                    }
                    _links[index] = link;
                    return;
                }
            }
        }

        public struct GroundCastsMoveProbeInit : IJobParallelFor
        {
            [ReadOnly]
            public NativeArray<Link> _links;
            [ReadOnly]
            public NativeArray<Node> _nodes;
            [WriteOnly]
            public NativeArray<RaycastCommand> _rays;

            [ReadOnly]
            public HullData hull;


            public void Execute( Int32 index )
            {
                var link = _links[index];
                if( !link.valid ) return;
                var node1 = _nodes[link.node1ID];
                if( node1.forbiddenHulls.HasFlag( hull.hullMask ) ) return;
                var node2 = _nodes[link.node2ID];
                if( node2.forbiddenHulls.HasFlag( hull.hullMask ) ) return;
            }
        }

        private IEnumerator WaitForJob( JobHandle handle, Action completeAction)
        {
            yield return new WaitUntil( () => handle.IsCompleted );
            
            //This is technically optional, but is good for safety. Only seems to add around 1ms of execution time.
            handle.Complete();
#if DEBUG
            Debug.Log( "Job completed" );
#endif
            completeAction();
#if DEBUG
            Debug.Log( "Job Action completed" );
#endif
        }

        private void OnDisable()
        {
            //On.RoR2.CombatDirector.Awake -= this.CombatDirector_Awake;
            //On.RoR2.ClassicStageInfo.Awake -= this.ClassicStageInfo_Awake;
        }

        private void OnEnable()
        {
            //On.RoR2.CombatDirector.Awake += this.CombatDirector_Awake;
            //On.RoR2.ClassicStageInfo.Awake += this.ClassicStageInfo_Awake;
            On.RoR2.SceneInfo.Awake += this.SceneInfo_Awake;
        }

        private void SceneInfo_Awake( On.RoR2.SceneInfo.orig_Awake orig, SceneInfo self )
        {
            orig( self );
            Int64 timer = 0;
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var groundNodes = self.groundNodes.GetFieldValue<NodeGraph.Node[]>("nodes");
            var airNodes = self.airNodes.GetFieldValue<NodeGraph.Node[]>("nodes");

            NativeArray<Node> _gNodes = new NativeArray<Node>( groundNodes.Length, Allocator.TempJob );
            NativeArray<Node> _aNodes = new NativeArray<Node>( airNodes.Length, Allocator.TempJob );
            NativeArray<Link> _gLinks = new NativeArray<Link>( groundNodes.Length * groundNodes.Length, Allocator.TempJob );
            NativeArray<Link> _aLinks = new NativeArray<Link>( airNodes.Length * airNodes.Length, Allocator.TempJob );

            GenerateTPOffsets();

            for( Int32 i = 0; i < _gNodes.Length; i++ )
            {
                _gNodes[i] = new Node
                {
                    position = groundNodes[i].position,
                    flags = groundNodes[i].flags,
                    forbiddenHulls = groundNodes[i].forbiddenHulls,
                };
            }

            for( Int32 i = 0; i < _aNodes.Length; i++ )
            {
                _aNodes[i] = new Node
                {
                    position = airNodes[i].position,
                    flags = airNodes[i].flags,
                    forbiddenHulls = airNodes[i].forbiddenHulls,
                };
            }
            UnityEngine.Debug.Log( "Ground Nodes: " + _gNodes.Length );
            UnityEngine.Debug.Log( "Air Nodes: " + _aNodes.Length );

            LogWatch( stopwatch, ref timer, "Reflection bs: " );

            BakeNodes( stopwatch, _gNodes, _gLinks, "Ground: " );
            BakeNodes( stopwatch, _aNodes, _aLinks, "Air: " );

            timer = 0;
            LogWatch( stopwatch, ref timer, "Total: " );

            _gNodes.Dispose();
            _gLinks.Dispose();
            _aNodes.Dispose();
            _aLinks.Dispose();

            tpCheckOffsets.Dispose();

            stopwatch.Stop();
        }

        private void BakeNodes( Stopwatch stopwatch, NativeArray<Node> _nodes, NativeArray<Link> _links, String logPrefix = "" )
        {
            Int32 loopSize = 10;

            Int64 timer = stopwatch.ElapsedMilliseconds;
            LogWatch( stopwatch, ref timer, logPrefix + "Start: " );

            var linkGen = new GenerateLinks
            {
                count = _nodes.Length,
                _links = _links
            };
            var linkGenHandle = linkGen.Schedule( _links.Length, loopSize );
            linkGenHandle.Complete();
            LogWatch( stopwatch, ref timer, logPrefix + "Node link gen: " );


            NativeArray<RaycastCommand> _losRays = new NativeArray<RaycastCommand>( _links.Length , Allocator.TempJob );
            var losRayGen = new LOSRayGen
            {
                count = _nodes.Length,
                _links = _links,
                _nodes = _nodes ,
                _rays = _losRays,
                mask = LayerIndex.world.mask
            };
            var losGenHandle = losRayGen.Schedule( _links.Length, loopSize, linkGenHandle );
            losGenHandle.Complete();
            LogWatch( stopwatch, ref timer, logPrefix + "LOS rays gen: " );


            NativeArray<RaycastHit> _losCast = new NativeArray<RaycastHit>( _links.Length, Allocator.TempJob );
            var losCastHandle = RaycastCommand.ScheduleBatch( _losRays, _losCast, loopSize , losGenHandle);
            losCastHandle.Complete();
            LogWatch( stopwatch, ref timer, logPrefix + "LOS rays cast: " );


            var losRead = new LOSRayRead
            {
                _hitRes = _losCast,
                _links = _links,
            };
            var losReadHandle = losRead.Schedule( _links.Length, loopSize, losCastHandle );
            losReadHandle.Complete();
            LogWatch( stopwatch, ref timer, logPrefix + "LOS rays read: " );


            NativeArray<RaycastCommand> _skyRays = new NativeArray<RaycastCommand>( _nodes.Length, Allocator.TempJob );
            var skyRayGen = new SkyRayGen
            {
                _nodes = _nodes,
                _rays = _skyRays
            };
            var skyGenHandle = skyRayGen.Schedule(_nodes.Length, loopSize );
            skyGenHandle.Complete();
            LogWatch( stopwatch, ref timer, logPrefix + "Sky rays gen: " );


            NativeArray<RaycastHit> _skyHits = new NativeArray<RaycastHit>( _nodes.Length, Allocator.TempJob );
            var skyCastHandle = RaycastCommand.ScheduleBatch(_skyRays, _skyHits, loopSize, skyGenHandle );
            skyCastHandle.Complete();
            LogWatch( stopwatch, ref timer, logPrefix + "Sky rays cast: " );


            var skyRead = new SkyRayRead
            {
                _hits = _skyHits,
                _nodes = _nodes
            };
            var skyReadHandle = skyRead.Schedule(_nodes.Length, loopSize, skyCastHandle );
            skyReadHandle.Complete();
            LogWatch( stopwatch, ref timer, logPrefix + "Sky rays read: " );


            NativeArray<RaycastCommand> _tpRays = new NativeArray<RaycastCommand>( _nodes.Length * tpCheckOffsets.Length, Allocator.TempJob );
            var toRayGen = new TPRayGen
            {
                _offsets = tpCheckOffsets,
                range = 10f,
                _nodes = _nodes,
                _rays = _tpRays
            };
            var tpGenHandle = toRayGen.Schedule( _nodes.Length, loopSize );
            tpGenHandle.Complete();
            LogWatch( stopwatch, ref timer, logPrefix + "TP rays gen: " );


            NativeArray<RaycastHit> _tpHits = new NativeArray<RaycastHit>( _nodes.Length * tpCheckOffsets.Length, Allocator.TempJob );
            var tpCastHandle = RaycastCommand.ScheduleBatch( _tpRays, _tpHits, loopSize, tpGenHandle );
            tpCastHandle.Complete();
            LogWatch( stopwatch, ref timer, logPrefix + "TP rays cast: " );


            var tpRead = new TPRayRead
            {
                offCount = tpCheckOffsets.Length,
                _hits = _tpHits,
                _nodes = _nodes
            };
            var tpReadHandle = tpRead.Schedule( _nodes.Length, loopSize, tpCastHandle );
            tpReadHandle.Complete();
            LogWatch( stopwatch, ref timer, logPrefix + "TP rays read: " );


            var distChecks = new LinkDistScore
            {
                rangeFactor = ( 15f * 2f ) * ( 15f * 2f ),
                _links = _links,
                _nodes = _nodes
            };
            var distCheckHandle = distChecks.Schedule( _links.Length, loopSize, linkGenHandle );
            distCheckHandle.Complete();
            LogWatch( stopwatch, ref timer, logPrefix + "Link distance scoring: " );


            NativeArray<CapsulecastCommand> _groundCapsules = new NativeArray<CapsulecastCommand>( 2 * _links.Length , Allocator.TempJob );
            var groundHumanCastGen = new GroundCastsGen
            {
                _links = _links,
                _nodes = _nodes,
                _caps = _groundCapsules,
                fudge = Vector3.up * 0.01f,
                hull = humanData
            };
            var groundHumanGenHandle = groundHumanCastGen.Schedule(_links.Length, 100, linkGenHandle );
            groundHumanGenHandle.Complete();
            LogWatch( stopwatch, ref timer, logPrefix + "Human ground position gen: " );


            NativeArray<RaycastHit> _groundCapsuleHits = new NativeArray<RaycastHit>( 2 * _links.Length, Allocator.TempJob );
            var groundHumanCastHandle = CapsulecastCommand.ScheduleBatch( _groundCapsules, _groundCapsuleHits, loopSize, groundHumanGenHandle );
            groundHumanCastHandle.Complete();
            LogWatch( stopwatch, ref timer, logPrefix + "Human ground position cast: " );


            NativeArray<CapsulecastCommand> _groundCapsules2 = new NativeArray<CapsulecastCommand>( 2 * _links.Length, Allocator.TempJob );
            NativeArray<Vector3> _groundVecs1 = new NativeArray<Vector3>( 2 * _links.Length, Allocator.TempJob );
            var groundHumanCastRead = new GroundCastsReadGen
            {
                _links = _links,
                _nodes = _nodes,
                _hitRes = _groundCapsuleHits,
                _caps = _groundCapsules2,
                _groundPos = _groundVecs1,
                hull = humanData,
                fudge = Vector3.up * 0.01f
            };
            var groundHumanCastReadHandle = groundHumanCastRead.Schedule( _links.Length, loopSize, groundHumanCastHandle );
            groundHumanCastReadHandle.Complete();
            LogWatch( stopwatch, ref timer, logPrefix + "Human ground position read: " );


            NativeArray<RaycastHit> _groundCapsuleHits2 = new NativeArray<RaycastHit>( 2 * _links.Length, Allocator.TempJob );
            var groundHumanCastHandle2 = CapsulecastCommand.ScheduleBatch( _groundCapsules2, _groundCapsuleHits2, loopSize, groundHumanCastReadHandle );
            groundHumanCastHandle2.Complete();
            LogWatch( stopwatch, ref timer, logPrefix + "Human ground position casts2: " );


            var groundHumanCastRead2 = new GroundCastsReadGen2
            {
                _links = _links,
                _nodes = _nodes,
                _hitRes = _groundCapsuleHits2,
                hull = humanData,
            };
            var groundHumanCastRead2Handle = groundHumanCastRead2.Schedule( _links.Length, loopSize, groundHumanCastHandle2 );
            groundHumanCastRead2Handle.Complete();
            LogWatch( stopwatch, ref timer, logPrefix + "Human ground position read2: " );







            NativeArray<CapsulecastCommand> _groundGolemCapsules = new NativeArray<CapsulecastCommand>( 2 * _links.Length , Allocator.TempJob );
            var groundGolemCastGen = new GroundCastsGen
            {
                _links = _links,
                _nodes = _nodes,
                _caps = _groundGolemCapsules,
                fudge = Vector3.up * 0.01f,
                hull = golemData
            };
            var groundGolemGenHandle = groundGolemCastGen.Schedule(_links.Length, loopSize, groundHumanCastHandle2 );
            groundGolemGenHandle.Complete();
            LogWatch( stopwatch, ref timer, logPrefix + "Golem ground position gen: " );


            NativeArray<RaycastHit> _groundGolemCapsuleHits = new NativeArray<RaycastHit>( 2 * _links.Length, Allocator.TempJob );
            var groundGolemCastHandle = CapsulecastCommand.ScheduleBatch( _groundGolemCapsules, _groundGolemCapsuleHits, loopSize, groundGolemGenHandle );
            groundGolemCastHandle.Complete();
            LogWatch( stopwatch, ref timer, logPrefix + "Golem ground position cast: " );


            NativeArray<CapsulecastCommand> _groundGolemCapsules2 = new NativeArray<CapsulecastCommand>( 2 * _links.Length, Allocator.TempJob );
            NativeArray<Vector3> _groundGolemVecs1 = new NativeArray<Vector3>( 2 * _links.Length, Allocator.TempJob );
            var groundGolemCastRead = new GroundCastsReadGen
            {
                _links = _links,
                _nodes = _nodes,
                _hitRes = _groundGolemCapsuleHits,
                _caps = _groundGolemCapsules2,
                _groundPos = _groundGolemVecs1,
                hull = golemData,
                fudge = Vector3.up * 0.01f
            };
            var groundGolemCastReadHandle = groundGolemCastRead.Schedule( _links.Length, loopSize, groundGolemCastHandle );
            groundGolemCastReadHandle.Complete();
            LogWatch( stopwatch, ref timer, logPrefix + "Golem ground position read: " );


            NativeArray<RaycastHit> _groundGolemCapsuleHits2 = new NativeArray<RaycastHit>( 2 * _links.Length, Allocator.TempJob );
            var groundGolemCastHandle2 = CapsulecastCommand.ScheduleBatch( _groundGolemCapsules2, _groundGolemCapsuleHits2, loopSize, groundGolemCastReadHandle );
            groundGolemCastHandle2.Complete();
            LogWatch( stopwatch, ref timer, logPrefix + "Golem ground position casts2: " );


            var groundGolemCastRead2 = new GroundCastsReadGen2
            {
                _links = _links,
                _nodes = _nodes,
                _hitRes = _groundGolemCapsuleHits2,
                hull = golemData,
            };
            var groundGolemCastRead2Handle = groundGolemCastRead2.Schedule( _links.Length, loopSize, groundGolemCastHandle2 );
            groundGolemCastRead2Handle.Complete();
            LogWatch( stopwatch, ref timer, logPrefix + "Golem ground position read2: " );







            NativeArray<CapsulecastCommand> _groundQueenCapsules = new NativeArray<CapsulecastCommand>( 2 * _links.Length , Allocator.TempJob );
            var groundQueenCastGen = new GroundCastsGen
            {
                _links = _links,
                _nodes = _nodes,
                _caps = _groundQueenCapsules,
                fudge = Vector3.up * 0.01f,
                hull = queenData
            };
            var groundQueenGenHandle = groundQueenCastGen.Schedule(_links.Length, loopSize, groundHumanCastHandle2 );
            groundQueenGenHandle.Complete();
            LogWatch( stopwatch, ref timer, logPrefix + "Queen ground position gen: " );


            NativeArray<RaycastHit> _groundQueenCapsuleHits = new NativeArray<RaycastHit>( 2 * _links.Length, Allocator.TempJob );
            var groundQueenCastHandle = CapsulecastCommand.ScheduleBatch( _groundQueenCapsules, _groundQueenCapsuleHits, loopSize, groundQueenGenHandle );
            groundQueenCastHandle.Complete();
            LogWatch( stopwatch, ref timer, logPrefix + "Queen ground position cast: " );


            NativeArray<CapsulecastCommand> _groundQueenCapsules2 = new NativeArray<CapsulecastCommand>( 2 * _links.Length, Allocator.TempJob );
            NativeArray<Vector3> _groundQueenVecs1 = new NativeArray<Vector3>( 2 * _links.Length, Allocator.TempJob );
            var groundQueenCastRead = new GroundCastsReadGen
            {
                _links = _links,
                _nodes = _nodes,
                _hitRes = _groundQueenCapsuleHits,
                _caps = _groundQueenCapsules2,
                _groundPos = _groundQueenVecs1,
                hull = queenData,
                fudge = Vector3.up * 0.01f
            };
            var groundQueenCastReadHandle = groundQueenCastRead.Schedule( _links.Length, loopSize, groundQueenCastHandle );
            groundQueenCastReadHandle.Complete();
            LogWatch( stopwatch, ref timer, logPrefix + "Queen ground position read: " );


            NativeArray<RaycastHit> _groundQueenCapsuleHits2 = new NativeArray<RaycastHit>( 2 * _links.Length, Allocator.TempJob );
            var groundQueenCastHandle2 = CapsulecastCommand.ScheduleBatch( _groundQueenCapsules2, _groundQueenCapsuleHits2, loopSize, groundQueenCastReadHandle );
            groundQueenCastHandle2.Complete();
            LogWatch( stopwatch, ref timer, logPrefix + "Queen ground position casts2: " );


            var groundQueenCastRead2 = new GroundCastsReadGen2
            {
                _links = _links,
                _nodes = _nodes,
                _hitRes = _groundQueenCapsuleHits2,
                hull = queenData,
            };
            var groundQueenCastRead2Handle = groundQueenCastRead2.Schedule( _links.Length, loopSize, groundQueenCastHandle2 );
            groundQueenCastRead2Handle.Complete();
            LogWatch( stopwatch, ref timer, logPrefix + "Queen ground position read2: " );



            // TODO: Slope
            // TODO: Hull

            _losRays.Dispose();
            _losCast.Dispose();
            _skyRays.Dispose();
            _skyHits.Dispose();
            _tpRays.Dispose();
            _tpHits.Dispose();
            _groundCapsules.Dispose();
            _groundCapsuleHits.Dispose();
            _groundCapsules2.Dispose();
            _groundCapsuleHits2.Dispose();
            _groundVecs1.Dispose();
            _groundGolemCapsules.Dispose();
            _groundGolemCapsuleHits.Dispose();
            _groundGolemCapsules2.Dispose();
            _groundGolemCapsuleHits2.Dispose();
            _groundGolemVecs1.Dispose();
            _groundQueenCapsules.Dispose();
            _groundQueenCapsuleHits.Dispose();
            _groundQueenCapsules2.Dispose();
            _groundQueenCapsuleHits2.Dispose();
            _groundQueenVecs1.Dispose();
        }

        private void LogWatch( Stopwatch stopwatch, ref Int64 timer, String prefix = "" )
        {
            stopwatch.Stop();
            UnityEngine.Debug.Log( prefix + (stopwatch.ElapsedMilliseconds - timer) );
            timer = stopwatch.ElapsedMilliseconds;
            stopwatch.Start();
        }

        private void GenerateTPOffsets()
        {
            Int32 numberOfChecks = 20;

            Single radius = 15f;
            Single verticalOffset = 7f;
            Single angleStep = 360f / (float)numberOfChecks;

            tpCheckOffsets = new NativeArray<Vector3>( numberOfChecks, Allocator.Persistent );
            
            for( Int32 i = 0; i < numberOfChecks; i++ )
            {
                Vector3 b = Quaternion.AngleAxis( angleStep * (float)i, Vector3.up ) * ( Vector3.forward * radius );
                Vector3 origin = b + Vector3.up * verticalOffset;
                tpCheckOffsets[i] = origin;
            }
        }

        private IEnumerator WaitForHandle( Action action , params JobHandle[] handles )
        {
            yield return new WaitForEndOfFrame();
        }
    }
}
