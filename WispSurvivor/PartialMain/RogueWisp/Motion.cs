using BepInEx;
using R2API.Utils;
using RoR2;
using RoR2.Networking;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using RogueWispPlugin.Helpers;
using RogueWispPlugin.Modules;
//using static RogueWispPlugin.Helpers.APIInterface;

namespace RogueWispPlugin
{
    internal partial class Main
    {
        partial void RW_Motion()
        {
            this.Load += this.RW_RigidBodySetup;
            this.Load += this.RW_CapColliderSetup;
            this.Load += this.RW_BodyDirectionSetup;
            this.Load += this.RW_BodyMotorSetup;
            this.Load += this.RW_BodyKinMotorSetup;
        }

        private void RW_BodyKinMotorSetup()
        {
            KinematicCharacterController.KinematicCharacterMotor kinMot = this.RW_body.GetComponent<KinematicCharacterController.KinematicCharacterMotor>();
            kinMot.CharacterController = this.RW_body.GetComponent<CharacterMotor>();
            kinMot.DetectDiscreteCollisions = false;
            kinMot.GroundDetectionExtraDistance = 1f;
            kinMot.MaxStepHeight = 0.2f;
            kinMot.MinRequiredStepDepth = 0.1f;
            kinMot.MaxStableSlopeAngle = 55f;
            kinMot.MaxStableDistanceFromLedge = 0.5f;
            kinMot.PreventSnappingOnLedges = false;
            kinMot.MaxStableDenivelationAngle = 55f;
            kinMot.RigidbodyInteractionType = KinematicCharacterController.RigidbodyInteractionType.None;
            kinMot.PreserveAttachedRigidbodyMomentum = true;
            kinMot.HasPlanarConstraint = false;
            kinMot.StepHandling = KinematicCharacterController.StepHandlingMethod.Standard;
            kinMot.LedgeHandling = true;
            kinMot.InteractiveRigidbodyHandling = true;
            kinMot.SafeMovement = false;
            kinMot.SetFieldValue<Single>( "CapsuleRadius", 0.7f );
            kinMot.SetFieldValue<Single>( "CapsuleHeight", 1.5f );
            kinMot.SetFieldValue<Single>( "CapsuleYOffset", 0f );
        }
        private void RW_BodyMotorSetup()
        {
            CharacterMotor motor = this.RW_body.GetComponent<CharacterMotor>();
            motor.walkSpeedPenaltyCoefficient = 1f;
            motor.characterDirection = this.RW_body.GetComponent<CharacterDirection>();
            motor.muteWalkMotion = false;
            motor.mass = 100f;
            motor.airControl = 0.25f;
            motor.disableAirControlUntilCollision = false;
            motor.generateParametersOnAwake = true;
            motor.useGravity = true;
            motor.isFlying = false;
        }
        private void RW_BodyDirectionSetup()
        {
            CharacterDirection dir = this.RW_body.GetComponent<CharacterDirection>();
            dir.targetTransform = this.RW_body.GetComponent<ModelLocator>().modelBaseTransform;
            //dir.overrideAnimatorForwardTransform = body.transform;
            dir.driveFromRootRotation = false;
            dir.turnSpeed = 300f;
        }
        private void RW_CapColliderSetup()
        {
            CapsuleCollider cap = this.RW_body.GetComponent<CapsuleCollider>();
            cap.radius = 0.7f;
            cap.height = 1.5f;
            cap.center = new Vector3( 0f, 0f, 0f );
        }
        private void RW_RigidBodySetup()
        {
            Rigidbody rb = this.RW_body.GetComponent<Rigidbody>();
            rb.mass = 100f;
            rb.drag = 0f;
            rb.angularDrag = 0f;
            rb.isKinematic = true;
            rb.interpolation = RigidbodyInterpolation.None;
            rb.collisionDetectionMode = CollisionDetectionMode.Discrete;
            rb.constraints = RigidbodyConstraints.None;
        }
    }

}
