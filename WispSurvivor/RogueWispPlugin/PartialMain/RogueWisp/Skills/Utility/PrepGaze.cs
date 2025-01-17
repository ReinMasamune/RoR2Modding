﻿#if ROGUEWISP
using EntityStates;

using Rein.RogueWispPlugin.Helpers;

using RoR2;

using UnityEngine;
using UnityEngine.Networking;

namespace Rein.RogueWispPlugin
{

    internal partial class Main
    {
        public class PrepGaze : BaseState
        {
            public static System.Single maxRange = 50f;
            public static System.Single flareDuration = 0.2f;
            public static System.Single castRadius = 0.25f;

            private System.Single radius = FireGaze.baseBlazeOrbRadius;

            private System.UInt32 skin = 0;

            private Vector3 normal;

            private WispPassiveController passive;
            private GameObject line;
            private Transform end;

            public override void OnEnter()
            {
                base.OnEnter();
                this.passive = this.gameObject.GetComponent<WispPassiveController>();
                this.skin = this.characterBody.skinIndex;

                WispPassiveController.ChargeState state = this.passive.UseCharge( FireGaze.chargeUsed, FireGaze.chargeScaler );
                this.radius *= state.chargeScaler;

                this.GetComponent<WispFlareController>().intensity = 0.5f;
            }

            public override void Update()
            {
                base.Update();
                //Update the beam position
                if( !this.line )
                {
                    Transform muzzle = this.GetModelTransform().Find("CannonPivot").Find("AncientWispArmature").Find("Head");
                    this.line = UnityEngine.Object.Instantiate<GameObject>( Main.utilityAim, muzzle.TransformPoint( 0f, 0.1f, 0f ), muzzle.rotation, muzzle );
                    this.line.GetComponent<BitSkinController>().Apply( WispBitSkin.GetWispSkin( this.skin ) );
                    this.end = this.line.transform.Find( "lineEnd" );
                    this.end.parent = null;
                    this.end.localScale = new Vector3( 2 * this.radius, 2 * this.radius, 2 * this.radius );
                }

                if( this.line )
                {
                    Ray r = this.GetAimRay();

                    RaycastHit rh;
                    //if( Physics.SphereCast( r, castRadius, out rh, maxRange, LayerIndex.world.mask | LayerIndex.entityPrecise.mask, QueryTriggerInteraction.UseGlobal ) )
                    if( Util.CharacterSpherecast( base.outer.gameObject, r, castRadius, out rh, maxRange, LayerIndex.world.mask | LayerIndex.entityPrecise.mask, QueryTriggerInteraction.UseGlobal ) )
                    {
                        this.end.position = rh.point;
                        this.normal = rh.normal;
                    } else
                    {
                        this.end.position = r.GetPoint( maxRange );
                        this.normal = Vector3.up;
                    }
                }
            }

            public override void FixedUpdate()
            {
                base.FixedUpdate();
                this.characterBody.SetAimTimer( 1f );
                if( this.inputBank && this.isAuthority && !this.inputBank.skill3.down )
                {
                    //Get the target position
                    this.outer.SetNextState( new FireGaze
                    {
                        orbOrigin = this.end.position,
                        orbNormal = normal,
                        blazeOrbRadius = radius
                    } );
                }
            }

            public override void OnExit()
            {
                base.OnExit();
                //Destroy the beam marker
                Destroy( this.line );
                Destroy( this.end.gameObject );
            }

            public override InterruptPriority GetMinimumInterruptPriority() => InterruptPriority.PrioritySkill;

            public override void OnSerialize( NetworkWriter writer ) => base.OnSerialize( writer );

            public override void OnDeserialize( NetworkReader reader ) => base.OnDeserialize( reader );
        }
    }

}
#endif