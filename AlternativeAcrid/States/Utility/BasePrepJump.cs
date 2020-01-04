namespace AlternativeAcrid.States.Utility
{
    using EntityStates;
    using RoR2;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Unity.Collections;
    using Unity.Jobs;
    using UnityEngine;

    public abstract class BasePrepJump: GenericCharacterMain
    {
        public static Boolean useGravity;

        public static Single endScale;
        public static Single baseSpeed;
        public static Single rayRadius;
        public static Single maxDistance;

        public static GameObject arcPrefab;
        public static GameObject endPrefab;
       

        private ArcPointJob arcPointJob;
        private JobHandle arcPointJobHandle;
        private TrajectoryInfo currentTrajInfo;
        private Vector3[] pointsBuffer;

        private Action jobCompleteMethod;
        private LineRenderer arcLineRender;
        private Transform endTransform;


        private struct ArcPointJob : IJobParallelFor, IDisposable
        {
            [ReadOnly]
            private Vector3 origin;
            [ReadOnly]
            private Vector3 velocity;
            [ReadOnly]
            private Single indexMult;
            [ReadOnly]
            private Single gravity;

            [WriteOnly]
            public NativeArray<Vector3> outputPositions;

            public void SetParams( Vector3 origin, Vector3 velocity, Single totalTravelTime, Int32 positionCount, Single gravity )
            {
                this.origin = origin;
                this.velocity = velocity;
                if( this.outputPositions.Length != positionCount )
                {
                    if( this.outputPositions.IsCreated )
                    {
                        this.outputPositions.Dispose();
                    }
                    this.outputPositions = new NativeArray<Vector3>( positionCount, Allocator.Persistent, NativeArrayOptions.UninitializedMemory );
                }
                this.indexMult = totalTravelTime / (Single)(positionCount - 1);
                this.gravity = gravity;
            }

            public void Execute( Int32 index )
            {
                Single t = (Single)index * this.indexMult;
                this.outputPositions[index] = Trajectory.CalculatePositionAtTime( this.origin, this.velocity, t, this.gravity );
            }

            public void Dispose()
            {
                if( this.outputPositions.IsCreated )
                {
                    this.outputPositions.Dispose();
                }
            }
        }
        private struct TrajectoryInfo
        {
            public Ray finalRay;
            public Vector3 hitPoint;
            public Vector3 hitNormal;
            public Single travelTime;
            public Single speedOverride;
        }


        private void JobComplete()
        {
            this.arcPointJobHandle.Complete();
            if( this.arcLineRender )
            {
                Array.Resize<Vector3>( ref this.pointsBuffer, this.arcPointJob.outputPositions.Length );
                //this.arcPointJob.outputPositions.CopyTo( this.pointsBuffer );
                //this.arcLineRender.SetPositions( this.pointsBuffer );
            }
        }

        private void UpdateVisuals( TrajectoryInfo traj )
        {
            if( this.arcLineRender && this.arcPointJobHandle.IsCompleted )
            {
                this.arcPointJob.SetParams( traj.finalRay.origin, traj.finalRay.direction * traj.speedOverride, traj.travelTime, arcLineRender.positionCount, useGravity ? Physics.gravity.y : 0f );
                this.arcPointJobHandle = this.arcPointJob.Schedule( this.arcPointJob.outputPositions.Length, 32, default( JobHandle ) );
            }

            if( this.endTransform )
            {
                this.endTransform.SetPositionAndRotation( traj.hitPoint, Util.QuaternionSafeLookRotation( traj.hitNormal ) );
            }
        }

        private void OnPreRenderScene(SceneCamera sceneCam )
        {
            if( this.arcLineRender )
            {
                this.arcLineRender.renderingLayerMask = ((sceneCam.cameraRigController.target == base.gameObject) ? 1u : 0u);
            }

            if( this.endTransform )
            {
                this.endTransform.gameObject.layer = ((sceneCam.cameraRigController.target == base.gameObject) ? LayerIndex.defaultLayer.intVal : LayerIndex.noDraw.intVal);
            }
        }

        private void UpdateTrajInfo( out TrajectoryInfo traj )
        {
            traj = default( TrajectoryInfo );
            Ray aimRay = base.GetAimRay();
            RaycastHit rHit = default( RaycastHit );

            Vector3 direction = aimRay.direction;

            direction.y = Mathf.Max( direction.y, EntityStates.Croco.BaseLeap.minimumY );
            Vector3 a = direction.normalized * EntityStates.Croco.BaseLeap.aimVelocity * this.moveSpeedStat;
            Vector3 b = Vector3.up * EntityStates.Croco.BaseLeap.upwardVelocity;
            Vector3 b2 = new Vector3(direction.x, 0f, direction.z).normalized * EntityStates.Croco.BaseLeap.forwardVelocity;

            Vector3 velocity = a + b + b2;
            Vector3 tempVelocity = velocity;
            Ray r = new Ray( aimRay.origin, velocity );

            Single time = Trajectory.CalculateFlightDuration( velocity.y );
            Single timeStep = time / 25f;

            Vector3 oldPos = aimRay.origin;

            Boolean hit = false;

            List<Vector3> vecs = new List<Vector3>( 100 );

            for( Int32 i = 0; i < 100; i++ )
            {
                Vector3 pos = Trajectory.CalculatePositionAtTime(r.origin, tempVelocity, timeStep * i, Physics.gravity.y );

                tempVelocity *= Mathf.Exp( timeStep * -0.4f );

                hit = Physics.SphereCast( oldPos, 0.2f, (pos - oldPos), out rHit,( pos-oldPos).magnitude, LayerIndex.world.mask | LayerIndex.entityPrecise.mask, QueryTriggerInteraction.UseGlobal );
                if( hit ) break;

                oldPos = pos;
                vecs.Add( pos );
            }

            arcLineRender.positionCount = vecs.Count;
            arcLineRender.SetPositions( vecs.ToArray() );

            if( hit )
            {
                traj.hitPoint = rHit.point;
                traj.hitNormal = rHit.normal;
            } else
            {
                traj.hitPoint = oldPos;
                traj.hitNormal = Vector3.up;
            }

            Vector3 diff = traj.hitPoint - aimRay.origin;

            if( useGravity )
            {
                Single speed = velocity.magnitude;
                Vector2 flatDiff = new Vector2( diff.x, diff.z );
                Single flatDist = flatDiff.magnitude;
                Single yStart = Trajectory.CalculateInitialYSpeed( flatDist / speed , diff.y );

                Vector3 speedVec = new Vector3( flatDiff.x / flatDist * speed, yStart, flatDiff.y / flatDist * speed );

                traj.speedOverride = speedVec.magnitude;
                traj.finalRay = new Ray( aimRay.origin, speedVec / traj.speedOverride );
                traj.travelTime = Trajectory.CalculateGroundTravelTime( speed, flatDist );
                return;
            }

            traj.speedOverride = baseSpeed;
            traj.finalRay = aimRay;
            traj.travelTime = baseSpeed / diff.magnitude;
        }

        private Boolean KeyIsDown()
        {
            return base.isAuthority && base.inputBank && base.inputBank.skill3.down;
        }


        public abstract EntityState NextState();


        public override void OnEnter()
        {
            base.OnEnter();

            if( arcPrefab )
            {
                this.arcLineRender = UnityEngine.Object.Instantiate<GameObject>( arcPrefab, base.transform.position, Quaternion.identity ).GetComponent<LineRenderer>();
                this.arcPointJob = default( BasePrepJump.ArcPointJob );
                this.jobCompleteMethod = new Action( this.JobComplete );
                RoR2Application.onLateUpdate += this.jobCompleteMethod;
            }

            if( endPrefab )
            {
                endTransform = UnityEngine.Object.Instantiate<GameObject>( endPrefab, base.transform.position, Quaternion.identity ).transform;
                endTransform.localScale = Vector3.one * endScale;
            }

            if( base.characterBody )
            {
                base.characterBody.hideCrosshair = true;
            }

            this.UpdateVisuals( this.currentTrajInfo );
            SceneCamera.onSceneCameraPreRender += this.OnPreRenderScene;
        }

        public override void Update()
        {
            base.Update();
            //if( CameraRigController.IsObjectSpectatedByAnyCamera( base.gameObject ) )
            //{
                this.UpdateTrajInfo( out this.currentTrajInfo );
                this.UpdateVisuals( this.currentTrajInfo );
            //}
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if( base.isAuthority && !this.KeyIsDown() )
            {
                this.UpdateTrajInfo( out this.currentTrajInfo );
                EntityState state = this.NextState();
                if( state != null )
                {
                    this.outer.SetNextState( state );
                    return;
                }
                this.outer.SetNextStateToMain();
            }
        }

        public override void OnExit()
        {
            SceneCamera.onSceneCameraPreRender -= this.OnPreRenderScene;
            if( base.characterBody )
            {
                base.characterBody.hideCrosshair = false;
                base.characterBody.isSprinting = false;
            }
            this.arcPointJobHandle.Complete();
            if( this.arcLineRender )
            {
                EntityState.Destroy( this.arcLineRender.gameObject );
                this.arcLineRender = null;
            }
            if( this.endTransform )
            {
                EntityState.Destroy( this.endTransform.gameObject );
                this.endTransform = null;
            }
            if( this.jobCompleteMethod != null )
            {
                RoR2Application.onLateUpdate -= this.jobCompleteMethod;
                this.jobCompleteMethod = null;
            }
            base.OnExit();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}
