namespace AlternativeArtificer.States.Main
{
    using EntityStates;
    using RoR2;
    using RoR2.Projectile;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using UnityEngine;
    using UnityEngine.Networking;

    public class AltArtiPassive : BaseState
    {
        #region External Consts
        //External
        const Single nanoBombInterval = 0.35f;
        const Single nanoBombMinDelay = 0.3f;
        const Single nanoBombMaxDelay = 0.7f;
        const Single prepWallMaxDelay = 0.3f;
        const Single prepWallMinDelay = 0.7f;
        const Single flamethrowerInterval = 0.35f;
        const Int32 nanoBombMaxPerTick = 10;
        const Int32 flamethrowerMaxPerTick = 10;
        #endregion

        #region Internal Consts
        const Single nodeYOffset = 1.6f;
        const Single nodeArcFrac = 0.6f;
        const Single nodeMinRadius = 1.15f;
        const Single nodeMaxRadius = 1.6f;
        const Single nodeFireRadius = 0.25f;
        const Single nodeFireRate = 0.65f;
        const Single nodeFireMin = -0.05f;
        const Single nodeFireMax = 0.05f;

        const Int32 nodesToCreate = 16;


        #endregion


        #region Public Statics
        public static Single lightningDamageMult = 0.25f;
        public static Single lightningForce = 1f;
        public static Single lightningProcCoef = 0.25f;

        public static Single burnDamageMult = 0.1f;
        public static Single burnBuffDuration = 2f;

        public static Single targetUpdateFreq = 10f;
        public static Single targetRange = 60f;
        public static Single targetAng = 40f;

        public static Single iceBaseRadius = 3f;
        public static Single iceRadScale = 3f;


        public static BuffIndex fireBuff;

        public static GameObject[] lightningProjectile;
        public static GameObject[] lightningPreFireEffect;

        public static GameObject iceBlast;

        public static Dictionary<GameObject,AltArtiPassive> instanceLookup = new Dictionary<GameObject, AltArtiPassive>();
        #endregion

        #region Private Statics

        #endregion


        #region Public Vars
        public CharacterBody ext_characterBody
        {
            get
            {
                return base.characterBody;
            }
        }
        public Single ext_attackSpeedStat
        {
            get
            {
                return base.characterBody.attackSpeed;
            }
        }
        public Single ext_nanoBombInterval
        {
            get
            {
                return nanoBombInterval;
            }
        }
        public Single ext_nanoBombMinDelay
        {
            get
            {
                return nanoBombMinDelay;
            }
        }
        public Single ext_nanoBombMaxDelay
        {
            get
            {
                return nanoBombMaxDelay;
            }
        }
        public Single ext_prepWallMinDelay
        {
            get
            {
                return prepWallMinDelay;
            }
        }
        public Single ext_prepWallMaxDelay
        {
            get
            {
                return prepWallMaxDelay;
            }
        }
        public Single ext_flamethrowerInterval
        {
            get
            {
                return flamethrowerInterval;
            }
        }
        public Int32 ext_nanoBombMaxPerTick
        {
            get
            {
                return nanoBombMaxPerTick;
            }
        }
        public Int32 ext_flamethrowerMaxPerTick
        {
            get
            {
                return flamethrowerMaxPerTick;
            }
        }
        #endregion

        #region Private Vars
        private Single searchTimer = 0f;

        private Power firePower;
        private Power icePower;
        private Power lightningPower;

        private Helpers.InstancedRandom random;

        private Transform modelTransform;

        private HurtBox target;

        private List<ProjectileNode> projNodes = new List<ProjectileNode>();
        private BullseyeSearch search = new BullseyeSearch();
        #endregion


        #region Public Typedefs
        public class BatchHandle
        {
            public List<ProjectileData> handledProjectiles = new List<ProjectileData>();

            public void Fire(Single minDelay, Single maxDelay)
            {
                foreach( ProjectileData proj in this.handledProjectiles )
                {
                    proj.triggered = true;
                    proj.timerMin = minDelay;
                    proj.timerMax = maxDelay;
                }
            }
        }
        #endregion

        #region Private Typedefs
        private enum Power
        {
            None = 0,
            Low = 1,
            Medium = 2,
            High = 3,
            Extreme = 4
        }

        public class ProjectileData
        {
            public Boolean isTriggered;
            public Boolean triggered;
            public Boolean timerAssigned;
            public Boolean radiusAssigned;
            public Single timer;
            public Single timerMin = nodeFireMin;
            public Single timerMax = nodeFireMax;
            public Vector3 localPos;
            public Int32 type;
            public Single rotation;
            public BatchHandle handle;

            private Helpers.InstancedRandom random;

            public ProjectileData(Helpers.InstancedRandom random, BatchHandle handle = null)
            {
                this.random = random;
                this.type = Mathf.FloorToInt( this.random.Range( 0f, 2f ) );
                this.rotation = this.random.Range( 0f, 360f );
                localPos = this.random.InsideUnitSphere();
                this.timerAssigned = true;

                if( handle != null )
                {
                    this.isTriggered = true;
                    this.triggered = false;
                    handle.handledProjectiles.Add( this );
                    this.handle = handle;
                    this.timerAssigned = false;
                }
            }

            public void AssignTimer()
            {
                if( this.timerAssigned ) return;
                this.timer = this.random.Range( this.timerMin, this.timerMax );
                this.timerAssigned = true;
            }

            public void AssignRadius( Single radius )
            {
                if( this.radiusAssigned ) return;
                this.localPos *= radius;
                this.radiusAssigned = true;
            }
        }

        private class ProjectileNode
        {
            public Transform location;
            public List<ProjectileData> queue;
            public Single fireTime;
            public Single fireRadius;

            private Single timer = 0f;

            

            public ProjectileData nextProj;

            private GameObject effect;

            private AltArtiPassive passive;

            public ProjectileNode( Vector3 position, Transform parent, AltArtiPassive passive )
            {
                this.location = new GameObject( "ProjNode" ).transform;
                this.location.parent = parent;
                this.location.localPosition = position;
                this.location.localRotation = Quaternion.identity;
                this.location.localScale = Vector3.one;

                this.queue = new List<ProjectileData>();

                this.fireRadius = nodeFireRadius;
                this.fireTime = nodeFireRate;
                this.passive = passive;
            }

            public void AddToQueue(ProjectileData data )
            {
                queue.Add( data );
            }

            public void AddImmediate( ProjectileData data )
            {
                if( this.nextProj != null )
                {
                    this.queue.Insert( 0, this.nextProj );
                }

                this.nextProj = data;
                this.nextProj.AssignRadius( this.fireRadius );
                this.CreateEffect( this.nextProj );
            }

            public void UpdateNode( Single deltaT, HurtBox target, Vector3 direction )
            {
                if( this.nextProj == null ) this.nextProj = this.TryGetNextProj();
                if( this.nextProj == null )
                {
                    this.timer = 0f;
                    return;
                }

                if( this.effect )
                {
                    this.effect.transform.rotation = Quaternion.AngleAxis( this.nextProj.rotation, direction ) * Util.QuaternionSafeLookRotation(direction);
                }

                if( this.nextProj.isTriggered )
                {
                    if( this.nextProj.triggered )
                    {
                        this.nextProj.AssignTimer();
                        this.timer += deltaT;
                        if( this.timer >= this.nextProj.timer )
                        {
                            this.Fire(target);
                        }
                    }
                } else
                {
                    this.timer += deltaT;
                    if( this.timer >= this.fireTime + this.nextProj.timer )
                    {
                        this.Fire(target);
                    }
                }
            }

            private ProjectileData TryGetNextProj()
            {
                if( this.queue.Count <= 0 ) return null;
                var temp = this.queue[0];
                this.queue.RemoveAt( 0 );

                if( temp != null )
                {
                    temp.AssignTimer();
                    temp.AssignRadius( this.fireRadius );
                    this.CreateEffect( temp );
                }


                return temp;
            }

            private void Fire( HurtBox target)
            {
                this.timer = 0f;
                if( effect == null ) CreateEffect( nextProj );

                if( this.passive.isAuthority )
                {
                    ProjectileManager.instance.FireProjectile( new FireProjectileInfo
                    {
                        crit = passive.RollCrit(),
                        damage = passive.damageStat * AltArtiPassive.lightningDamageMult,
                        damageColorIndex = DamageColorIndex.Default,
                        force = AltArtiPassive.lightningForce,
                        owner = passive.gameObject,
                        position = this.effect.transform.position,
                        procChainMask = default( ProcChainMask ),
                        projectilePrefab = AltArtiPassive.lightningProjectile[this.nextProj.type],
                        rotation = this.effect.transform.rotation,
                        target = target != null ? target.gameObject : null
                    } );
                }
                UnityEngine.Object.Destroy( effect );

                this.nextProj = null;
            }

            private void CreateEffect(ProjectileData proj )
            {
                if( this.effect != null )
                {
                    UnityEngine.Object.Destroy( this.effect );
                }
                this.effect = UnityEngine.Object.Instantiate( AltArtiPassive.lightningPreFireEffect[proj.type], this.location );
                this.effect.transform.localScale = Vector3.one;
                this.effect.transform.localPosition = proj.localPos;
                this.effect.transform.localRotation = Quaternion.identity;
            }
        }
        #endregion


        #region External Methods
        public void SkillCast(BatchHandle handle = null)
        {
            DoLightning( this.lightningPower, handle );
            DoFire( this.firePower );
        }

        public void DoExecute( DamageReport report )
        {
            if( this.icePower > Power.None )
            {
                this.CreateIceBlast( report.damageInfo.position, iceBaseRadius + (Int32)this.icePower * iceRadScale );
            }
        }
        #endregion

        #region Internal Methods
        private void DoLightning( Power power, BatchHandle handle )
        {
            for( Int32 i = 0; i < (Int32)power; i++ )
            {
                var proj = new ProjectileData( this.random, handle );

                this.AddProjectileToRandomNode( proj, handle != null );
            }
        }

        private void DoFire( Power power )
        {
            if( NetworkServer.active )
            {
                for( Int32 i = 0; i < (Int32)power; i++ )
                {
                    base.characterBody.AddTimedBuff( fireBuff, burnBuffDuration );
                }
            }
        }

        private void CreateIceBlast( Vector3 position, Single radius)
        {
            if( NetworkServer.active )
            {
                GameObject blast = UnityEngine.Object.Instantiate<GameObject>( iceBlast , position, Quaternion.identity );
                blast.transform.localScale = new Vector3( radius, radius, radius );
                var delay = blast.GetComponent<DelayBlast>();
                delay.position = position;
                delay.baseDamage = base.damageStat;
                delay.attacker = base.gameObject;
                delay.radius = radius;
                blast.GetComponent<TeamFilter>().teamIndex = base.teamComponent.teamIndex;
            }
        }

        private void GetPowers()
        {
            SkillLocator loc = base.skillLocator;

            this.firePower = Power.None;
            this.icePower = Power.None;
            this.lightningPower = Power.None;

            this.GetSkillPower( loc.primary );
            this.GetSkillPower( loc.secondary );
            this.GetSkillPower( loc.utility );
            this.GetSkillPower( loc.special );

            Debug.Log( "Fire: " + this.firePower.ToString() );
            Debug.Log( "Ice: " + this.icePower.ToString() );
            Debug.Log( "Lightning: " + this.lightningPower.ToString() );
        }

        private void GetSkillPower( GenericSkill skill )
        {
            String name = skill.baseSkill.skillNameToken.Split('_')[2].ToLower();
            switch( name )
            {
                default:
                    Debug.Log( "Element: " + name + " is not handled" );
                    break;
                case "fire":
                    this.firePower++;
                    break;
                case "ice":
                    this.icePower++;
                    break;
                case "lightning":
                    this.lightningPower++;
                    break;
            }

            skill.onSkillChanged += ( s ) => GetPowers();
        }

        private void GenerateNodes()
        {
            for( Int32 i = 0; i < nodesToCreate; i++ )
            {
                Single randRot = this.random.Value();
                Single randRad = this.random.Value();
                Single radius = nodeMinRadius + ( randRad * ( nodeMaxRadius - nodeMinRadius ) );
                Single ang = randRot - 0.5f;
                ang *= Mathf.PI * 2f;
                ang *= nodeArcFrac;
                Single x = Mathf.Sin( ang );
                Single y = Mathf.Cos( ang );
                Vector3 localPos = new Vector3( x, y, 0f );
                localPos = Vector3.Normalize( localPos );
                localPos *= radius;
                localPos += new Vector3( 0f, nodeYOffset, 0f );

                projNodes.Add( new ProjectileNode( localPos, this.modelTransform, this ) );
            }
        }

        private HurtBox GetTarget()
        {
            var aimRay = base.GetAimRay();
            this.search.teamMaskFilter = TeamMask.all;
            this.search.teamMaskFilter.RemoveTeam( this.teamComponent.teamIndex );
            this.search.filterByLoS = true;
            this.search.searchOrigin = aimRay.origin;
            this.search.searchDirection = aimRay.direction;
            this.search.sortMode = BullseyeSearch.SortMode.DistanceAndAngle;
            this.search.maxDistanceFilter = targetRange;
            this.search.maxAngleFilter = targetAng;
            this.search.RefreshCandidates();
            return this.search.GetResults().FirstOrDefault<HurtBox>();
        }

        private void AddProjectileToRandomNode( ProjectileData proj, Boolean immediate )
        {
            List<Int32> counts = new List<Int32>();
            for( Int32 i = 0; i < this.projNodes.Count; i++ )
            {
                counts.Add( this.projNodes[i].queue.Count + ( this.projNodes[i].nextProj != null ? 5 : 0 ) );
            }

            Int32 min = counts.Min();
            List<Int32> minInds = new List<Int32>();
            for( Int32 i = 0; i < counts.Count; i++ )
            {
                if( counts[i] == min ) minInds.Add( i );
            }

            Int32 finalIndex = 0;

            if( minInds.Count > 1 ) finalIndex = Mathf.FloorToInt( this.random.Range( 0, minInds.Count ) );

            if( immediate )
            {
                this.projNodes[minInds[finalIndex]].AddImmediate( proj );
            } else
            {
                this.projNodes[minInds[finalIndex]].AddToQueue( proj );
            }
        }
        #endregion

        #region Hooked Methods
        public override void OnEnter()
        {
            base.OnEnter();

            this.random = new Helpers.InstancedRandom((Int32)base.characterBody.netId.Value );

            this.modelTransform = base.GetModelTransform();

            instanceLookup[base.gameObject] = this;

            this.GetPowers();
            this.GenerateNodes();
        }

        public override void Update()
        {
            base.Update();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            var deltaT = Time.fixedDeltaTime;

            this.searchTimer += deltaT;
            if( this.searchTimer >= 1f / targetUpdateFreq )
            {
                this.target = this.GetTarget();
                this.searchTimer = 0f;
            }


            var direction = base.GetAimRay().direction;

            foreach( ProjectileNode node in this.projNodes )
            {
                node.UpdateNode( deltaT, this.target, direction );
            }
        }

        public override void OnExit()
        {
            if( instanceLookup.ContainsKey( base.gameObject ) ) instanceLookup.Remove( base.gameObject );



            base.OnExit();
        }
        #endregion
    }
}
