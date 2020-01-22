namespace RogueWispPlugin
{
#if ANCIENTWISP
    internal partial class Main
    {
        internal class AWSecondary : BaseState
        {
            const Single baseWarmupTime = 1f;


            const Single basePlacePillarsTime = 6f;
            const Int32 totalPillars = 10;
            const Int32 maxTargets = 3;
            const Single searchRange = 100f;


            const Single basePreDetonateTime = 1f;


            const Single baseDetonateTime = 1f;
            const Single damageCoef = 1.0f;
            const Single pillarRadius = 10f;
            const Single pillarHeight = 50f;


            const Single basePostDetonateTime = 1f;




            private Single warmupDuration;
            private Single placePillarDuration;
            private Single preDetonateDuration;
            private Single detonateDuration;
            private Single postDetonateDuration;
            private Single pillarInterval;

            private SubStateData subState;

            private List<GameObject> pillars = new List<GameObject>(totalPillars * maxTargets);
            private List<HurtBox> tempTargets = new List<HurtBox>(maxTargets);
            private HashSet<HealthComponent> tempMask = new HashSet<HealthComponent>();
            private BullseyeSearch search;

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
                        Main.LogI( "State: " + this.state.ToString() + " " + this.stateDef.stateDuration + "s " + this.timer + "s" );
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

                this.search = new BullseyeSearch();
                this.search.teamMaskFilter = TeamMask.allButNeutral;
                this.search.teamMaskFilter.RemoveTeam( base.teamComponent.teamIndex );
                this.search.maxDistanceFilter = searchRange;
                this.search.maxAngleFilter = 180f;
                this.search.filterByLoS = false;
                this.search.sortMode = BullseyeSearch.SortMode.Angle;


                this.warmupDuration = baseWarmupTime;
                this.placePillarDuration = basePlacePillarsTime / base.attackSpeedStat;
                this.pillarInterval = this.placePillarDuration / totalPillars;
                this.preDetonateDuration = basePreDetonateTime;
                this.detonateDuration = baseDetonateTime;
                this.postDetonateDuration = basePostDetonateTime;


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

                this.subState.UpdateTime( Time.fixedDeltaTime );
                this.subState.CallStart();
                this.subState.CallUpdate();
                this.subState.CallEnd();
            }

            public override void OnExit()
            {
                //this.DetonatePillars();
                //base.PlayCrossfade( "Body", "EndRain", "EndRain.playbackRate", 0.5f, 0.2f );
                if( this.pillars != null )
                {
                    foreach( GameObject g in this.pillars ) if( g != null ) Destroy( g );
                }
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

                    this.FirePillar();
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
                this.DetonatePillars();
            }
            private void DetonateUpdate()
            {

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



            private void FirePillar()
            {
                Ray r = base.GetAimRay();
                this.search.searchOrigin = r.origin;
                this.search.searchDirection = r.direction;
                this.search.RefreshCandidates();
                this.tempMask.Clear();
                this.tempTargets.Clear();
                foreach( HurtBox box in this.search.GetResults() )
                {
                    if( this.tempMask.Contains( box.healthComponent ) ) continue;
                    this.tempTargets.Add( box );
                    if( this.tempTargets.Count >= maxTargets ) break;
                    this.tempMask.Add( box.healthComponent );
                }

                foreach( HurtBox box in this.tempTargets )
                {
                    this.pillars.Add( this.CreatePillar( box ) ); 
                }
            }

            private GameObject CreatePillar(HurtBox box)
            {
                var obj = UnityEngine.Object.Instantiate<GameObject>(Main.instance.AW_secDelayEffect);
                obj.transform.position = box.transform.position;
                return obj;
            }

            private void DetonatePillars()
            {
                foreach( GameObject pillar in this.pillars )
                {
                    if( base.isAuthority )
                    {
                        this.CreatePillarExplosion( pillar.transform.position );
                    }
                    UnityEngine.Object.Destroy( pillar );
                }
            }

            private void CreatePillarExplosion( Vector3 position )
            {
                var data = new EffectData
                {
                    origin = position,
                    scale = pillarRadius,
                    genericFloat = pillarHeight
                };
                EffectManager.SpawnEffect( Main.instance.AW_secExplodeEffect, data, true );

                if( false )
                {
                    new BlastAttack
                    {
                        attacker = base.gameObject,
                        baseDamage = base.damageStat * damageCoef,
                        baseForce = 0f,
                        bonusForce = Vector3.zero,
                        canHurtAttacker = false,
                        crit = base.RollCrit(),
                        damageColorIndex = DamageColorIndex.Default,
                        damageType = DamageType.Generic,
                        falloffModel = BlastAttack.FalloffModel.None,
                        impactEffect = EffectIndex.Invalid,
                        inflictor = base.gameObject,
                        losType = BlastAttack.LoSType.None,
                        position = position,
                        procChainMask = default,
                        procCoefficient = 1.0f,
                        radius = pillarRadius,
                        teamIndex = base.teamComponent.teamIndex
                    }.Fire();
                }
            }
        }
    }
#endif
}
