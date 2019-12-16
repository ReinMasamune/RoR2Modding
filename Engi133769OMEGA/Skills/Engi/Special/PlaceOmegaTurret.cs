using EntityStates;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Engi133769OMEGA.Skills.Engi.Special
{
    public class PlaceOmegaTurret : BaseState
    {
        private struct PlacementInfo
        {
            public Boolean ok;
            public Vector3 position;
            public Quaternion rotation;
        }
        public static Single entryDelay = 0.1f;
        public static Single exitDelay = 0.25f;

        public static Single placementHOffset = 2f;
        public static Single placementVOffset = 1f;
        public static Single placementRadius = 0.5f;
        public static Single placementHeight = 1.82f;
        public static Single placementCapRad = 0.45f;

        public GameObject wristDisplayPrefab = ((EntityStates.Engi.EngiWeapon.PlaceTurret)EntityState.Instantiate(new SerializableEntityStateType( typeof( EntityStates.Engi.EngiWeapon.PlaceTurret ) ) )).wristDisplayPrefab;
        public GameObject blueprintPrefab = ((EntityStates.Engi.EngiWeapon.PlaceTurret)EntityState.Instantiate(new SerializableEntityStateType( typeof( EntityStates.Engi.EngiWeapon.PlaceTurret ) ) )).blueprintPrefab;
        public GameObject turretMasterPrefab = OmegaTurretMain.master;


        private Single entryCountdown;
        private Single exitCountdown;

        private Boolean exitPending;
        private Boolean skill4Released = false;

        private String placeSoundString = ((EntityStates.Engi.EngiWeapon.PlaceTurret)EntityState.Instantiate(new SerializableEntityStateType( typeof( EntityStates.Engi.EngiWeapon.PlaceTurret ) ) )).placeSoundString;

        private PlacementInfo currentPlacementInfo;

        private GameObject wristDisplayObject;

        private BlueprintController blueprints;

        public override void OnEnter()
        {
            base.OnEnter();

            if( base.isAuthority )
            {
                this.currentPlacementInfo = this.GetPlacementInfo();

                this.blueprints = UnityEngine.Object.Instantiate<GameObject>( this.blueprintPrefab, this.currentPlacementInfo.position, this.currentPlacementInfo.rotation ).GetComponent<BlueprintController>();
            }

            base.PlayAnimation( "Gesture", "PrepTurret" );
            this.entryCountdown = entryDelay;
            this.exitCountdown = exitDelay;
            this.exitPending = false;

            if( base.modelLocator )
            {
                ChildLocator childLoc = base.modelLocator.modelTransform.GetComponent<ChildLocator>();

                if( childLoc )
                {
                    Transform transform = childLoc.FindChild( "WristDisplay" );
                    if( transform )
                    {
                        this.wristDisplayObject = UnityEngine.Object.Instantiate<GameObject>( this.wristDisplayPrefab, transform );

                    }
                }
            }
        }

        public override void Update()
        {
            if( base.inputBank && !base.inputBank.skill4.down )
            {
                this.skill4Released = true;
            }
            base.Update();

            this.currentPlacementInfo = GetPlacementInfo();

            if( this.blueprints )
            {
                this.blueprints.PushState( this.currentPlacementInfo.position, this.currentPlacementInfo.rotation, this.currentPlacementInfo.ok );
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if( base.isAuthority )
            {
                this.entryCountdown -= Time.fixedDeltaTime;
                if( this.exitPending )
                {
                    this.exitCountdown -= Time.fixedDeltaTime;
                    if( this.exitCountdown <= 0f )
                    {
                        this.outer.SetNextStateToMain();
                        return;
                    }
                } else if ( base.inputBank && this.entryCountdown <= 0f )
                {
                    if( (base.inputBank.skill1.down || base.inputBank.skill4.justPressed ) && this.currentPlacementInfo.ok )
                    {
                        if( base.characterBody )
                        {
                            base.characterBody.SendConstructTurret( base.characterBody, this.currentPlacementInfo.position, this.currentPlacementInfo.rotation, MasterCatalog.FindMasterIndex( this.turretMasterPrefab ) );
                            if( base.skillLocator )
                            {
                                var skill = base.skillLocator.GetSkill( SkillSlot.Special );
                                if( skill )
                                {
                                    skill.DeductStock( 1 );
                                }
                            }
                        }
                        Util.PlaySound( this.placeSoundString, base.gameObject );
                        this.DestroyBlueprints();
                        this.exitPending = true;
                    }

                    if( base.inputBank.skill2.justPressed )
                    {
                        this.DestroyBlueprints();
                        this.exitPending = true;
                    }
                }
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            base.PlayAnimation( "Gesture", "PlaceTurret" );
            if( this.wristDisplayObject )
            {
                EntityState.Destroy( this.wristDisplayObject );
            }
            this.DestroyBlueprints();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }


        private PlacementInfo GetPlacementInfo()
        {
            // TODO: Check for other uses of constants.
            Ray aimRay = base.GetAimRay();
            Vector3 direction = aimRay.direction;
            direction.y = 0;
            direction.Normalize();
            aimRay.direction = direction;

            PlacementInfo placementInfo = default( PlacementInfo );

            placementInfo.ok = false;
            placementInfo.rotation = Util.QuaternionSafeLookRotation( -direction );

            Ray ray = new Ray( aimRay.GetPoint( placementHOffset ) + Vector3.up * placementVOffset , Vector3.down );

            Single temp1 = 4f;
            Single temp2 = temp1;

            RaycastHit rh;
            if( Physics.SphereCast( ray, placementRadius, out rh, temp1, LayerIndex.world.mask, QueryTriggerInteraction.UseGlobal ) && rh.normal.y > 0.5f )
            {
                temp2 = rh.distance;
                placementInfo.ok = true;
            }

            Vector3 point = ray.GetPoint( temp2 + placementRadius );
            placementInfo.position = point;

            if( placementInfo.ok )
            {
                float temp3 = Mathf.Max( placementHeight, 0f );

                if( Physics.CheckCapsule( placementInfo.position + Vector3.up * (temp3 - placementRadius) , placementInfo.position + Vector3.up * placementRadius, placementCapRad, LayerIndex.world.mask | LayerIndex.defaultLayer.mask ))
                {
                    placementInfo.ok = false;
                }
            }

            return placementInfo;
        }

        private void DestroyBlueprints()
        {
            if( this.blueprints )
            {
                EntityState.Destroy( this.blueprints.gameObject );
                this.blueprints = null;
            }
        }
    }
}
