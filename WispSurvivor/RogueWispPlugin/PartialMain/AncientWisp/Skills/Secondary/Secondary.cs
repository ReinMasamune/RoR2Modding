#if ANCIENTWISP
using EntityStates;
using RogueWispPlugin.Helpers;
using RoR2;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;
using System.Collections.ObjectModel;

namespace RogueWispPlugin
{

    internal partial class Main
    {
        internal class AWSecondary : BaseState
        {
            const Single baseWarmupTime = 1f;


            const Single basePlacePillarsTime = 6f;
            const Int32 totalPillars = 10;
            const Int32 maxPlayerTargets = 3;
            const Int32 maxNonPlayerTargets = 3;
            const Int32 maxTargets = 3;
            const Single searchRange = 400f;


            const Single basePreDetonateTime = 1f;


            const Single baseDetonateTime = 1f;
            const Single damageCoef = 0.2f;
            const Single pillarRadius = 5f;


            const Single basePostDetonateTime = 1f;

            const Single firstDetonateCoef = 0.3f;
            const Single lastDetonateCoef = 0.6f;
            const Single pillarSequentialDetonationInterval = 0.1f;
            const Single pillarInitialDetonationDelay = 1.1f;
            const Single pillarInitialDetonationProcCoef = 0.5f;

            const Single maxPredictionVelocity = 50f;
            const Single defaultPredictionVelocity = 10f;




            private Single warmupDuration;
            private Single placePillarDuration;
            private Single preDetonateDuration;
            private Single detonateDuration;
            private Single postDetonateDuration;
            private Single pillarInterval;
            private Single detonateInterval;

            private Boolean enraged = false;

            private SubStateData subState;

            //private List<Vector3> pillars = new List<Vector3>(totalPillars * maxTargets);
            private List<HurtBox> tempTargets = new List<HurtBox>(maxTargets);
            private HashSet<HealthComponent> tempMask = new HashSet<HealthComponent>();
            private BullseyeSearch search;
            private List<TeamComponent> enemyTCs = new List<TeamComponent>();

            private HashSet<GameObject> playerSet = new HashSet<GameObject>();
            private SortedList<Single,HealthComponent> players = new SortedList<Single, HealthComponent>();
            private SortedList<Single,HealthComponent> nonPlayers = new SortedList<Single, HealthComponent>();

            private TeamIndex team;
            private GameObject owner;
            private UInt32 skin;


            private ReadOnlyCollection<TeamComponent> targets1;
            private ReadOnlyCollection<TeamComponent> targets2;
            private ReadOnlyCollection<TeamComponent> targets3;
            private ReadOnlyCollection<TeamComponent> targets4;
            
            //private Queue<Vector3> sequencePillars = new Queue<Vector3>();

            private Queue<PillarInfo> pillars = new Queue<PillarInfo>();

            private class PillarInfo
            {
                private Boolean firstDetonationOver = false;
                private Single timer;
                private Vector3 position;
                private HashSet<HealthComponent> mask = new HashSet<HealthComponent>();
                private TeamIndex team;
                private GameObject owner;
                private Boolean crit;
                private Single radius;
                private Single baseDamage;
                private Single dmgMult1;
                private Single dmgMult2;
                private UInt32 skin;
                internal PillarInfo( Vector3 position, Single radius, Boolean crit, Single baseDamage, Single preMult, Single finalMult, Single firstDetonationTimer, Single totalTimer, TeamIndex team, GameObject owner, UInt32 skinIndex )
                {
                    this.position = position;
                    this.radius = radius;
                    this.crit = crit;
                    this.baseDamage = baseDamage;
                    this.dmgMult1 = preMult;
                    this.dmgMult2 = finalMult;
                    this.timer = firstDetonationTimer;
                    this.team = team;
                    this.owner = owner;
                    this.skin = skinIndex;

                    var data = new EffectData
                    {
                        genericUInt = this.skin,
                        origin = this.position,
                        rotation = Quaternion.identity,
                        genericFloat = totalTimer,
                        start = new Vector3( this.radius * 2f, 1f, this.radius * 2f )
                    };
                    EffectManager.SpawnEffect( Main.AW_secondaryPrediction, data, true );

                    var data2 = new EffectData
                    {
                        genericUInt = this.skin,
                        origin = this.position,
                        rotation = Quaternion.identity,
                        genericFloat = this.timer,
                        start = new Vector3( this.radius * 2f, 1f, this.radius * 2f )
                    };
                    EffectManager.SpawnEffect( Main.AW_secondaryInitialPrediction, data2, true );
                }

                internal Boolean RunTime( Single delta )
                {
                    var res = false;
                    if( !this.firstDetonationOver )
                    {
                        this.timer -= delta;
                        if( this.timer <= 0f )
                        {
                            res = true;
                            this.firstDetonationOver = true;
                        }
                    }
                    return res;
                }

                internal void FirstExplosion( Vector3 force, Single procCoef, DamageType damageType, params IEnumerable<TeamComponent>[] validTargets )
                {
                    var data = new EffectData
                    {
                        origin = this.position,
                        start = new Vector3( this.radius * 2f, 1f, this.radius * 2f ),
                        genericFloat = 0.5f,
                        genericUInt = this.skin,
                    };
                    EffectManager.SpawnEffect( Main.AW_secondaryInitialExplosion, data, true );

                    this.Detonate( this.baseDamage * this.dmgMult1, force, procCoef, damageType, validTargets );
                }

                internal void FinalExplosion( Vector3 force, Single procCoef, DamageType damageType, params IEnumerable<TeamComponent>[] validTargets )
                {
                    var data = new EffectData
                    {
                        origin = this.position,
                        start = new Vector3( this.radius, 1f, this.radius ),
                        genericFloat = 0.5f,
                        genericUInt = this.skin,
                    };
                    EffectManager.SpawnEffect( Main.AW_secondaryExplosion, data, true );

                    this.Detonate( this.baseDamage * this.dmgMult2, force, procCoef, damageType, validTargets );
                }

                private void Detonate( Single damage, Vector3 force, Single procCoef, DamageType damageType, params IEnumerable<TeamComponent>[] validTargets )
                {
                    this.mask.Clear();

                    foreach( var targetSet in validTargets )
                    {
                        if( targetSet == null ) continue;
                        foreach( var target in targetSet )
                        {
                            if( target == null ) continue;
                            if( target.teamIndex == this.team ) continue;
                            var body = target.body;
                            if( body == null ) continue;
                            var hc = body.healthComponent;
                            if( hc == null ) continue;
                            if( this.mask.Contains( hc ) ) continue;
                            this.mask.Add( hc );

                            var diff = hc.transform.position - this.position;
                            if( diff.y < -10f ) continue;
                            diff.y = 0f;
                            if( diff.magnitude <= this.radius )
                            {
                                var info = new DamageInfo
                                {
                                    attacker = this.owner,
                                    crit = this.crit,
                                    damage = damage,
                                    damageColorIndex = DamageColorIndex.Default,
                                    damageType = damageType,
                                    dotIndex = DotController.DotIndex.None,
                                    force = force,
                                    inflictor = null,
                                    position = hc.transform.position,
                                    procChainMask = default,
                                    procCoefficient = procCoef,
                                };
                                hc.TakeDamage( info );
                                GlobalEventManager.instance.OnHitEnemy( info, hc.gameObject );
                                GlobalEventManager.instance.OnHitAll( info, hc.gameObject );
                            }
                        }
                    }
                }

            }



            private struct SubStateDef
            {
                public Single stateDuration;

                public Action stateStart;
                public Action stateUpdate;
                public Action stateEnd;
                public Func<SubState> getNextState;
            }

            private struct SubStateData
            {
                public SubState state;
                public SubStateDef stateDef;

                public Single timer;
                public Single subTimer;

                public Boolean start;

                public Int32 counter;

                public Boolean active;
                public Action endAction;

                public Dictionary<SubState,SubStateDef> stateDefs;

                public SubStateData( SubState initalState, Action endAction, Dictionary<SubState,SubStateDef> stateDefs )
                {
                    this.state = initalState;
                    this.stateDefs = stateDefs;
                    this.stateDef = this.stateDefs[this.state];
                    this.timer = this.stateDef.stateDuration;
                    this.subTimer = 0f;
                    this.start = true;
                    this.counter = 0;
                    this.active = true;
                    this.endAction = endAction;

                }

                public void UpdateTime( Single time )
                {
                    this.timer -= time;
                    this.subTimer += time;
                }

                public void CallStart()
                {
                    if( !this.active ) return;

                    if( this.start )
                    {
                        this.start = false;
                        this.timer = this.stateDef.stateDuration;
                        this.stateDef.stateStart?.Invoke();
                        //Main.LogI( "State: " + this.state.ToString() + " " + this.stateDef.stateDuration + "s " + this.timer + "s" );
                    }
                }

                public void CallUpdate()
                {
                    if( !this.active ) return;

                    this.stateDef.stateUpdate?.Invoke();
                }

                public void CallEnd()
                {
                    if( !this.active ) return;

                    if( this.Ending() )
                    {
                        this.stateDef.stateEnd?.Invoke();
                        this.IncState();
                    }
                }

                public void IncState()
                {
                    if( this.stateDef.getNextState != null )
                    {
                        this.state = this.stateDef.getNextState();
                    } else
                    {
                        ++this.state;
                    }

                    if( this.stateDefs.ContainsKey( this.state ) )
                    {
                        this.stateDef = this.stateDefs[this.state];
                        this.timer = this.stateDef.stateDuration;
                    } else
                    {
                        this.active = false;
                        this.endAction?.Invoke();
                    }

                    this.start = true;
                    this.subTimer = 0f;
                    this.counter = 0;
                }

                public Boolean Ending()
                {
                    return this.timer <= 0f;
                }
            }

            private enum SubState
            {
                Warmup = 0,
                PlacePillars = 1,
                PreDetonate = 2,
                Detonate = 3,
                PostDetonate = 4
            }

            public override void OnEnter()
            {
                base.OnEnter();

                this.team = base.GetTeam();
                this.owner = base.gameObject;
                this.skin = base.characterBody.skinIndex;

                this.search = new BullseyeSearch();
                this.search.teamMaskFilter = TeamMask.AllExcept( this.team );
                this.search.maxDistanceFilter = searchRange;
                this.search.maxAngleFilter = 180f;
                this.search.filterByLoS = false;
                //this.search.sortMode = BullseyeSearch.SortMode.Angle;


                this.warmupDuration = baseWarmupTime;
                this.placePillarDuration = basePlacePillarsTime / base.attackSpeedStat;
                this.pillarInterval = this.placePillarDuration / totalPillars;
                this.preDetonateDuration = basePreDetonateTime;
                this.detonateDuration = baseDetonateTime;
                this.postDetonateDuration = basePostDetonateTime;

                this.detonateInterval = pillarSequentialDetonationInterval;

                if( base.HasBuff( BuffIndex.EnrageAncientWisp ) ) this.enraged = true;


                Util.PlayScaledSound( "Play_gravekeeper_attack1_open", base.gameObject, 0.7f );
                Util.PlayScaledSound( "Play_gravekeeper_attack1_open", base.gameObject, 0.7f );
                Util.PlayScaledSound( "Play_gravekeeper_attack1_open", base.gameObject, 0.7f );

                var stateDict = new Dictionary<SubState,SubStateDef>();

                stateDict[SubState.Warmup] = new SubStateDef
                {
                    stateDuration = this.warmupDuration,
                    stateStart = this.WarmupStart,
                    stateUpdate = this.WarmupUpdate,
                    stateEnd = this.WarmupEnd
                };

                stateDict[SubState.PlacePillars] = new SubStateDef
                {
                    stateDuration = this.placePillarDuration,
                    stateStart = this.PlacePillarsStart,
                    stateUpdate = this.PlacePillarsUpdate,
                    stateEnd = this.PlacePillarsEnd,
                };

                stateDict[SubState.PreDetonate] = new SubStateDef
                {
                    stateDuration = this.preDetonateDuration,
                    stateStart = this.PreDetonateStart,
                    stateUpdate = this.PreDetonateUpdate,
                    stateEnd = this.PreDetonateEnd
                };

                stateDict[SubState.Detonate] = new SubStateDef
                {
                    stateDuration = this.detonateDuration,
                    stateStart = this.DetonateStart,
                    stateUpdate = this.DetonateUpdate,
                    stateEnd = this.DetonateEnd,
                };

                stateDict[SubState.PostDetonate] = new SubStateDef
                {
                    stateDuration = this.postDetonateDuration,
                    stateStart = this.PostDetonateStart,
                    stateUpdate = this.PostDetonateUpdate,
                    stateEnd = this.PostDetonateEnd,
                };

                this.subState = new SubStateData( SubState.Warmup, this.End, stateDict );
            }

            public override void FixedUpdate()
            {
                base.FixedUpdate();

                foreach( var pillar in this.pillars )
                {
                    if( pillar.RunTime( Time.fixedDeltaTime ) )
                    {
                        this.UpdateTargets();
                        pillar.FirstExplosion( Vector3.zero, pillarInitialDetonationProcCoef, DamageType.SlowOnHit, this.targets1, this.targets2, this.targets3, this.targets4 );
                    }
                }

                this.subState.UpdateTime( Time.fixedDeltaTime );
                this.subState.CallStart();
                this.subState.CallUpdate();
                this.subState.CallEnd();
            }

            public override void OnExit()
            {
                //this.DetonatePillars();
                //base.PlayCrossfade( "Body", "EndRain", "EndRain.playbackRate", 0.5f, 0.2f );
                base.OnExit();
            }

            public override InterruptPriority GetMinimumInterruptPriority()
            {
                return InterruptPriority.PrioritySkill;
            }

    #region Warmup
            private void WarmupStart()
            {
                base.PlayCrossfade( "Body", "ChargeRain", "ChargeRain.playbackRate", this.warmupDuration, 0.2f );
            }
            private void WarmupUpdate()
            {

            }
            private void WarmupEnd()
            {

            }
    #endregion
    #region PlacePillars
            private void PlacePillarsStart()
            {
                base.PlayCrossfade( "Body", "ChannelRain", 0.3f );
            }
            private void PlacePillarsUpdate()
            {
                while( this.subState.subTimer >= this.pillarInterval && this.subState.counter < totalPillars )
                {
                    this.subState.counter++;
                    this.subState.subTimer -= this.pillarInterval;
                    var detDelay = this.subState.timer + this.preDetonateDuration;
                    this.FindTargets( detDelay );
                }
            }
            private void PlacePillarsEnd()
            {

            }
    #endregion
    #region PreDetonate
            private void PreDetonateStart()
            {
                base.PlayCrossfade( "Gesture", "Enrage", "Enrage.playbackRate", this.preDetonateDuration, 0.2f );
            }       
            private void PreDetonateUpdate()
            {
            }
            private void PreDetonateEnd()
            {

            }
    #endregion
    #region Detonate
            private void DetonateStart()
            {
                this.subState.subTimer = this.detonateInterval;
            }
            private void DetonateUpdate()
            {
                while( this.subState.subTimer >= this.detonateInterval && this.pillars.Count > 0 )
                {
                    this.subState.subTimer -= this.detonateInterval;

                    this.pillars.Dequeue().FinalExplosion( Vector3.zero, 1.0f, DamageType.Generic, this.targets1, this.targets2, this.targets3, this.targets4 );
                }
                if( this.pillars.Count == 0 )
                {
                    this.subState.timer = 0f;
                } else
                {
                    this.subState.timer += this.detonateInterval * 2;
                }
            }
            private void DetonateEnd()
            {

            }
    #endregion
    #region PostDetonate
            private void PostDetonateStart()
            {
                base.PlayCrossfade( "Body", "EndRain", "EndRain.playbackRate", this.postDetonateDuration, 0.2f );
            }
            private void PostDetonateUpdate()
            {

            }
            private void PostDetonateEnd()
            {

            }
    #endregion
    #region End
            private void End()
            {
                if( base.isAuthority )
                {
                    base.outer.SetNextStateToMain();
                }
            }
    #endregion

            private void FindTargets( Single timeUntilDetonationStart )
            {

                if( !NetworkServer.active ) return;
                //Main.LogC();
                this.tempMask.Clear();
                var cachedPos = base.characterBody.transform.position;
                this.players.Clear();
                if( this.team != TeamIndex.Player )
                {
                    //Main.LogI( "Not On PlayerTeam" );
                    foreach( var v in PlayerCharacterMasterController.instances )
                    {
                        if( v == null )
                        {
                            continue;
                        }
                        var master = v.master;
                        if( master == null )
                        {
                            continue;
                        }
                        var body = master.GetBody();
                        if( body == null || body.teamComponent.teamIndex == this.team )
                        {
                            continue;
                        }
                        var hc = body.healthComponent;
                        if( hc == null || hc.alive || this.tempMask.Contains( hc ) ) continue;
                        var dist = Vector3.Distance( body.transform.position, cachedPos );
                        if( dist > searchRange ) continue;
                        this.tempMask.Add( hc );
                        this.players.Add( dist, hc );
                    }
                } else
                {
                    //Main.LogI( "On Player Team" );
                }
                var ray = base.GetAimRay();
                this.nonPlayers.Clear();
                this.search.searchOrigin = ray.origin;
                this.search.searchDirection = ray.direction;
                this.search.RefreshCandidates();
                foreach( var target in this.search.GetResults() )
                {
                    if( target == null ) continue;
                    //Main.LogI( target.name );
                    var hc = target.healthComponent;
                    if( hc.body.teamComponent.teamIndex == this.team ) continue;
                    if( hc == null || !hc.alive || this.tempMask.Contains( hc ) ) continue;
                    this.tempMask.Add( hc );
                    var dist = Vector3.Distance( hc.transform.position, cachedPos );
                    this.nonPlayers.Add( dist, hc );
                }

                var playerCount = Math.Min( this.players.Count, maxPlayerTargets );
                var nonPlayerCount = Math.Min( this.nonPlayers.Count, maxNonPlayerTargets );

                //Main.LogI("playerCount"+ playerCount );
                //Main.LogI("nonplayercount"+ nonPlayerCount );

                for( Int32 i = 0; i < playerCount; ++i )
                {
                    this.CreatePillar( this.players.Values[i], timeUntilDetonationStart );
                }
                for( Int32 i = 0; i < nonPlayerCount; ++i )
                {
                    this.CreatePillar( this.nonPlayers.Values[i], timeUntilDetonationStart );
                }
            }

            private void CreatePillar( HealthComponent hc, Single timeUntilDetonationStart )
            {
                //Main.LogC();
                var startingCount = this.pillars.Count;
                var tOffset = this.detonateInterval * startingCount + timeUntilDetonationStart;
                var startPos = hc.transform.position;
                var downRay = new Ray( startPos, Vector3.down );
                if( Physics.Raycast( downRay, out RaycastHit hit, 500f, LayerIndex.world.mask, QueryTriggerInteraction.UseGlobal ) )
                {
                    startPos = hit.point;
                } else
                {
                    startPos = downRay.GetPoint( 500f );
                }

                this.pillars.Enqueue( new PillarInfo( startPos, pillarRadius, base.RollCrit(), base.damageStat, firstDetonateCoef, lastDetonateCoef, pillarInitialDetonationDelay, tOffset, this.team, this.owner, this.skin ) );

                if( this.enraged )
                {


                    var vel = hc.body.characterMotor.velocity;
                    vel.y = 0f;
                    if( vel.magnitude == 0f )
                    {
                        vel = hc.body.inputBank.aimDirection;
                        vel.y = 0f;
                        vel = vel.normalized * defaultPredictionVelocity;
                    } else
                    {
                        var speed = vel.magnitude;
                        speed = Mathf.Min( speed, maxPredictionVelocity );
                        vel = vel.normalized;
                        vel *= speed;
                    }

                    vel *= pillarInitialDetonationDelay;
                    var predictedPos = startPos + vel;
                    downRay.origin = predictedPos + Vector3.up;
                    if( Physics.Raycast( downRay, out hit, 500f, LayerIndex.world.mask, QueryTriggerInteraction.UseGlobal ) )
                    {
                        predictedPos = hit.point;
                    } else
                    {
                        predictedPos = downRay.GetPoint( 100f );
                    }

                    this.pillars.Enqueue( new PillarInfo( predictedPos, pillarRadius, base.RollCrit(), base.damageStat, firstDetonateCoef, lastDetonateCoef, pillarInitialDetonationDelay, tOffset, this.team, this.owner, this.skin ) );
                }
            }

            private void UpdateTargets()
            {
                //Main.LogC();
                this.targets1 = base.GetTeam() == TeamIndex.Monster ? null : TeamComponent.GetTeamMembers( TeamIndex.Monster );
                this.targets2 = base.GetTeam() == TeamIndex.Neutral ? null : TeamComponent.GetTeamMembers( TeamIndex.Neutral );
                this.targets3 = base.GetTeam() == TeamIndex.None ? null : TeamComponent.GetTeamMembers( TeamIndex.None );
                this.targets4 = base.GetTeam() == TeamIndex.Player ? null : TeamComponent.GetTeamMembers( TeamIndex.Player );
            }

            //private void DetonatePillars()
            //{
            //    if( !base.isAuthority ) return;
            //    foreach( Vector3 pillar in this.pillars )
            //    {
            //        if( base.isAuthority )
            //        {
            //            this.CreatePillarExplosion( pillar );
            //        }
            //    }
            //}

            //private void CreatePillarExplosion( Vector3 position )
            //{
            //    if( !NetworkServer.active ) return;
            //    var data = new EffectData
            //    {
            //        origin = position,
            //        start = new Vector3( pillarRadius, 1f, pillarRadius ),
            //        genericFloat = 0.5f,
            //        genericUInt = base.characterBody.skinIndex,
            //    };
            //    EffectManager.SpawnEffect( Main.AW_secondaryExplosion, data, true );
            //    var teamIndex = base.teamComponent.teamIndex;

            //    HashSet<HealthComponent> hitMask = new HashSet<HealthComponent>();
            //    Queue<HealthComponent> hits = new Queue<HealthComponent>();

            //    var enemies = TeamComponent.GetTeamMembers( TeamIndex.Player );
            //    foreach( var enemy in enemies )
            //    {
            //        var hc = enemy.GetComponent<HealthComponent>();
            //    }


            //    foreach( var col in Physics.OverlapBox( position, new Vector3( pillarRadius, 1000f, pillarRadius ), Quaternion.identity, LayerIndex.entityPrecise.mask, QueryTriggerInteraction.UseGlobal ) )
            //    {
            //        if( col == null ) continue;
            //        var hb = col.GetComponent<HurtBox>();
            //        if( hb == null ) continue;
            //        var hc = hb.healthComponent;
            //        if( hitMask.Contains( hc ) ) continue;
            //        hitMask.Add( hc );
            //        if( hb.teamIndex == teamIndex ) continue;
            //        var temp = hb.transform.position - position;
            //        if( temp.y < 0f ) continue;
            //        temp.y = 0f;
            //        if( temp.magnitude > pillarRadius ) continue;
            //        hits.Enqueue( hc );
            //    }
            //    var count = hits.Count;

            //    if( count > 0 )
            //    {
            //        var attacker = base.gameObject;
            //        var crit = base.RollCrit();
            //        var damage = damageCoef * base.damageStat;
            //        var force = new Vector3( 0f, 100f, 0f );


            //        for( Int32 i = 0; i < count; ++i )
            //        {
            //            var target = hits.Dequeue();
            //            var info = new DamageInfo
            //            {
            //                attacker = attacker,
            //                crit = crit,
            //                damage = damage,
            //                damageColorIndex = DamageColorIndex.Default,
            //                damageType = DamageType.Generic,
            //                dotIndex = DotController.DotIndex.None,
            //                force = force,
            //                inflictor = null,
            //                position = target.transform.position,
            //                procChainMask = default,
            //                procCoefficient = 1f,
            //            };

            //            target.TakeDamage( info );
            //            GlobalEventManager.instance.OnHitEnemy( info, target.gameObject );
            //            GlobalEventManager.instance.OnHitAll( info, target.gameObject );
            //        }
            //    }

            //}
        }
    }
}
#endif