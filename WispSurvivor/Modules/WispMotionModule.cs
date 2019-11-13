using RoR2;
using UnityEngine;
using R2API.Utils;
using System;
using System.Collections.Generic;

namespace WispSurvivor.Modules
{
    public static class WispMotionModule
    {
        public static void DoModule( GameObject body , Dictionary<Type,Component> dic)
        {
            SetupRigidBody(body, dic);
            SetupCapCollider(body, dic);
            SetupBodyDirection(body, dic);
            SetupBodyMotor(body, dic);
            SetupBodyKinMotor(body, dic);
        }

        private static void SetupRigidBody( GameObject body , Dictionary<Type,Component> dic )
        {
            Rigidbody rb = dic.C<Rigidbody>();
            rb.mass = 100f;
            rb.drag = 0f;
            rb.angularDrag = 0f;
            rb.isKinematic = true;
            rb.interpolation = RigidbodyInterpolation.None;
            rb.collisionDetectionMode = CollisionDetectionMode.Discrete;
            rb.constraints = RigidbodyConstraints.None;
        }

        private static void SetupCapCollider(GameObject body, Dictionary<Type, Component> dic)
        {
            CapsuleCollider cap = dic.C<CapsuleCollider>();
            cap.radius = 0.7f;
            cap.height = 1.5f;
            cap.center = new Vector3(0f, 0f, 0f);
        }

        private static void SetupBodyDirection(GameObject body, Dictionary<Type, Component> dic)
        {
            CharacterDirection dir = dic.C<CharacterDirection>();
            dir.targetTransform = dic.C<ModelLocator>().modelBaseTransform;
            //dir.overrideAnimatorForwardTransform = body.transform;
            dir.driveFromRootRotation = false;
            dir.turnSpeed = 300f;
        }

        private static void SetupBodyMotor(GameObject body, Dictionary<Type, Component> dic)
        {
            CharacterMotor motor = dic.C<CharacterMotor>();
            motor.walkSpeedPenaltyCoefficient = 1f;
            motor.characterDirection = dic.C<CharacterDirection>();
            motor.muteWalkMotion = false;
            motor.mass = 100f;
            motor.airControl = 0.25f;
            motor.disableAirControlUntilCollision = false;
            motor.generateParametersOnAwake = true;
            motor.useGravity = true;
            motor.isFlying = false;
        }

        private static void SetupBodyKinMotor(GameObject body, Dictionary<Type, Component> dic)
        {
            KinematicCharacterController.KinematicCharacterMotor kinMot = dic.C<KinematicCharacterController.KinematicCharacterMotor>();
            kinMot.CharacterController = dic.C<CharacterMotor>();
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
            kinMot.SetFieldValue<float>("CapsuleRadius", 0.7f);
            kinMot.SetFieldValue<float>("CapsuleHeight", 1.5f);
            kinMot.SetFieldValue<float>("CapsuleYOffset", 0f);
        }

        private static T C<T>( this Dictionary<Type,Component> dic ) where T : Component
        {
            return dic[typeof(T)] as T;
        }
    }
}
