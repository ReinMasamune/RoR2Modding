namespace AlternateArtificer.States.Main
{
    using EntityStates;
    using RoR2;
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text;
    using UnityEngine;

    public class AltArtiPassive : BaseState
    {
        #region Public Statics
        public static GameObject fireProjectile;
        public static GameObject iceProjectile;
        public static GameObject lightningProjectile;

        public static Dictionary<GameObject,AltArtiPassive> instanceLookup = new Dictionary<GameObject, AltArtiPassive>();
        #endregion

        #region Private Statics

        #endregion


        #region Public Vars

        #endregion

        #region Private Vars
        private Power firePower;
        private Power icePower;
        private Power lightningPower;

        private List<ProjectileNode> projNodes = new List<ProjectileNode>();
        #endregion


        #region Public Typedefs
        [Flags]
        public enum ProjectileType
        {
            Fire = 1,
            Ice = 2,
            Lightning = 4
        }

        public class BatchHandle
        {
            public List<ProjectileData> handledProjectiles = new List<ProjectileData>();

            public void Fire(Single minDelay, Single maxDelay)
            {
                foreach( ProjectileData proj in handledProjectiles )
                {
                    proj.triggered = true;
                    proj.timer = UnityEngine.Random.Range( minDelay, maxDelay );
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
            public ProjectileType type;
            public Boolean isTriggered;
            public Boolean triggered;
            public Single timer;
            public BatchHandle handle;

            public ProjectileData(BatchHandle handle = null)
            {
                if( handle != null )
                {
                    this.isTriggered = true;
                    this.triggered = false;
                    handle.handledProjectiles.Add( this );
                }
            }
        }

        private class ProjectileNode
        {
            public Boolean triggerAllowed;
            public Transform location;
            public List<ProjectileData> queue;
            public Single fireTime;
            public Single fireRadius;

            private Single timer = 0f;

            private ProjectileData nextProj;

            public ProjectileNode( Vector3 position, GameObject parent )
            {
                this.location = new GameObject( "ProjNode" ).transform;
                this.location.parent = parent.transform.parent;
                this.location.localPosition = position;
                this.location.localRotation = Quaternion.identity;
                this.location.localScale = Vector3.one;

                queue = new List<ProjectileData>();
            }

            public void AddToQueue(ProjectileData data )
            {
                queue.Add( data );
            }

            public void AddImmediate( ProjectileData data )
            {
                if( nextProj != null )
                {
                    queue.Insert( 0, nextProj );
                }

                nextProj = data;
            }

            public void UpdateNode( Single deltaT, GameObject target )
            {
                if( nextProj == null ) nextProj = TryGetNextProj();
                if( nextProj == null )
                {
                    timer = 0f;
                    return;
                }

                if( nextProj.isTriggered )
                {
                    if( nextProj.triggered )
                    {
                        timer += deltaT;
                        if( timer >= nextProj.timer )
                        {
                            Fire(target);
                        }
                    }
                } else
                {
                    timer += deltaT;
                    if( timer >= fireTime )
                    {
                        Fire(target);
                    }
                }
            }

            private ProjectileData TryGetNextProj()
            {
                if( queue.Count <= 0 ) return null;
                var temp = queue[0];
                queue.RemoveAt( 0 );
                return temp;
            }

            private void Fire( GameObject target)
            {
                timer = 0f;





                nextProj = null;
            }
        }
        #endregion


        #region External Methods


        #endregion

        #region Internal Methods
        private void GetPowers()
        {
            SkillLocator loc = base.skillLocator;

            firePower = Power.None;
            icePower = Power.None;
            lightningPower = Power.None;

            GetSkillPower( loc.primary );
            GetSkillPower( loc.secondary );
            GetSkillPower( loc.utility );
            GetSkillPower( loc.special );

            Debug.Log( "Fire: " + firePower.ToString() );
            Debug.Log( "Ice: " + icePower.ToString() );
            Debug.Log( "Lightning: " + lightningPower.ToString() );
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
                    firePower++;
                    break;
                case "ice":
                    icePower++;
                    break;
                case "lightning":
                    lightningPower++;
                    break;
            }

            skill.onSkillChanged += ( s ) => GetPowers();
        }

        private void GenerateNodes()
        {

        }

        private GameObject GetTarget()
        {
            return new GameObject();
        }
        #endregion

        #region Hooked Methods
        public override void OnEnter()
        {
            base.OnEnter();

            instanceLookup[base.gameObject] = this;

            GetPowers();
            GenerateNodes();
        }

        public override void Update()
        {
            base.Update();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            GameObject target = GetTarget();

            var deltaT = Time.fixedDeltaTime;
            foreach( ProjectileNode node in projNodes )
            {
                node.UpdateNode( deltaT, target );
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
