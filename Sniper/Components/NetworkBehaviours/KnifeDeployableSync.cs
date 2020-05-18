namespace Sniper.Components
{
    using System;
    using ReinCore;
    using RoR2;
    using RoR2.Projectile;
    using Sniper.SkillDefs;
    using Sniper.SkillDefTypes.Bases;
    using UnityEngine;
    using UnityEngine.Networking;

    internal class KnifeDeployableSync : NetworkBehaviour, IRuntimePrefabComponent
    {
        private const Single triggerActivationDelay = 0.25f;
        private const Single ownerDetectionRadius = 5f;

        //private static Int32 _commandIndex_
        static KnifeDeployableSync()
        {
            //NetworkBehaviour.RegisterCommandDelegate( typeof(KnifeDeployableSync), )
        }

        private void Awake()
        {
            this.projectileStick.stickEvent.AddListener( this.OnStick );
        }

        private void OnStick()
        {
            this.shouldAdjustKnife = true;
        }


        private void Start()
        {
            if( (this.owner = this.projectileController?.owner)?.GetComponent<SkillLocator>()?.special?.skillInstanceData is ReactivatedSkillDef<KnifeSkillData>.ReactivationInstanceData data )
            {
                this.knifeData = data;
                this.knifeDataInstance = this.knifeData.data;
                this.knifeDataInstance.Initialize( this );
            }

            this.projGhost = this.projectileController.ghost.transform;
        }

        private void OnDestroy()
        {
            if( this.knifeData != null )
            {
                if( this.knifeData.data == this.knifeDataInstance )
                {
                    this.knifeData.InvalidateReactivation();
                }
            }
        }

        private void FixedUpdate()
        {
            if( !this.projectileStick.stuck )
            {
                base.transform.forward = this.rb.velocity.normalized;
            }

            if( this.timer < triggerActivationDelay )
            {
                this.timer += Time.fixedDeltaTime;
                if( this.timer >= triggerActivationDelay )
                {
                    this.timer += triggerActivationDelay;

                    if( Util.HasEffectiveAuthority( this.owner ) )
                    {
                        this.ownerDetection.SetActive( true );
                    }
                }
            }

            if( this.shouldAdjustKnife )
            {

                var pos = base.transform.position;
                var hitCol = this.projectileStick.stuckTransform.GetComponent<Collider>();
                if( hitCol is MeshCollider meshCollider && !meshCollider.convex )
                {
                    var ray = new Ray( base.transform.position, base.transform.forward );
                    if( meshCollider.Raycast( ray, out var hit, 1f ) )
                    {
                        base.transform.position = hit.point;
                    } else
                    {
                        base.transform.position += base.transform.forward * 0.5f;
                    }

                } else if( !(hitCol is null) )
                {
                    var targetPos = hitCol.ClosestPoint( pos );
                    base.transform.position = targetPos;
                }

                this.shouldAdjustKnife = false;
            }

            if( this.shouldSetState )
            {
                if( this.knifeDataInstance.targetStateMachine.CanInterruptState( KnifeSkillData.interruptPriority ) )
                {
                    if( this.knifeDataInstance.targetStateMachine.SetInterruptState( this.knifeDataInstance.InstantiateNextState( this ), KnifeSkillData.interruptPriority ) )
                    {
                        this.shouldSetState = false;
                    }
                }
            }
        }

        private void CheckCollider( Collider col )
        {
            if( col.gameObject == this.owner.gameObject )
            {
                this.shouldSetState = true;
            }
        }


        private ReactivatedSkillDef<KnifeSkillData>.ReactivationInstanceData knifeData;
        private KnifeSkillData knifeDataInstance;
        private GameObject owner;
        private Transform projGhost;
        private Boolean shouldAdjustKnife = false;
        private Single timer = 0f;
        private Boolean shouldSetState = false;


        [SerializeField]
        private ProjectileController projectileController;
        [SerializeField]
        private ProjectileStickOnImpact projectileStick;
        [SerializeField]
        private Collider hitCollider;
        [SerializeField]
        private GameObject ownerDetection;
        [SerializeField]
        private Rigidbody rb;
        void IRuntimePrefabComponent.InitializePrefab()
        {
            this.projectileController = base.GetComponent<ProjectileController>();
            if( this.projectileController is null )
            {
                throw new MissingComponentException( nameof( this.projectileController ) );
            }
            this.projectileStick = base.GetComponent<ProjectileStickOnImpact>();
            if( this.projectileStick is null )
            {
                throw new MissingComponentException( nameof( this.projectileStick ) );
            }

            this.hitCollider = base.GetComponent<Collider>();
            if( this.hitCollider is null )
            {
                throw new MissingComponentException( nameof( this.hitCollider ) );
            }

            this.rb = base.GetComponent<Rigidbody>();
            if( this.rb is null )
            {
                throw new MissingComponentException( nameof( this.rb ) );
            }

            var obj = new GameObject( "OwnerDetection" ).transform;
            obj.parent = base.transform;
            obj.localPosition = Vector3.zero;
            obj.localRotation = Quaternion.identity;
            obj.localScale = Vector3.one;
            obj.gameObject.layer = LayerIndex.defaultLayer.intVal;
            obj.gameObject.SetActive( false );
            this.ownerDetection = obj.gameObject;

            var col = obj.AddComponent<SphereCollider>();
            col.radius = ownerDetectionRadius;
            col.isTrigger = true;
            col.enabled = false;
            var manager = obj.AddComponent<CollisionManager>();
            manager.owner = this;
            (manager as IRuntimePrefabComponent).InitializePrefab();
        }

        private class CollisionManager : MonoBehaviour, IRuntimePrefabComponent
        {
            [SerializeField]
            internal KnifeDeployableSync owner;
            [SerializeField]
            private Collider collider;

            private void OnEnable()
            {
                this.collider.enabled = true;
            }
            private void OnDisable()
            {
                this.collider.enabled = false;
            }

            private void OnTriggerEnter( Collider col )
            {
                if( col is null ) return;

                this.owner.CheckCollider( col );
            }

            void IRuntimePrefabComponent.InitializePrefab()
            {
                if( this.owner is null )
                {
                    throw new MissingComponentException(nameof(this.owner));
                }
                this.collider = base.GetComponent<Collider>();
                if( this.collider is null )
                {
                    throw new MissingComponentException(nameof(this.collider));
                }
            }
        }
    }
}
