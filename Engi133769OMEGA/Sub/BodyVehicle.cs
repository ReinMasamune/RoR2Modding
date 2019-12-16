using Engi133769OMEGA.Components;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Engi133769OMEGA
{
    public partial class OmegaTurretMain
    {
        private void EditBodyVehicle()
        {
            var inputMaster = body.AddComponent<InputMaster>();

            var seatObj = new GameObject("EngiTurretVehicleSeat");
            seatObj.transform.parent = body.transform;
            seatObj.transform.localPosition = new Vector3( 0f, 0f, 0f );
            seatObj.transform.localRotation = Quaternion.identity;
            seatObj.transform.localScale = Vector3.one;

            var seatPos = new GameObject("SeatPos");
            seatPos.transform.parent = body.GetComponent<ModelLocator>().modelTransform.GetComponent<ChildLocator>().FindChild( "Muzzle" );
            seatPos.transform.localPosition = new Vector3( 0f, 0f, -1f );
            seatPos.transform.localRotation = Quaternion.identity;
            seatPos.transform.localScale = Vector3.one;
            seatPos.transform.parent = seatPos.transform.parent.parent.parent.parent.parent.parent;

            var seatEL = seatObj.AddComponent<EntityLocator>();
            seatEL.entity = seatObj;

            var seatRB = seatObj.AddComponent<Rigidbody>();
            seatRB.mass = 1f;
            seatRB.drag = 0.2f;
            seatRB.angularDrag = 0.05f;
            seatRB.useGravity = false;
            seatRB.isKinematic = false;
            seatRB.interpolation = RigidbodyInterpolation.Interpolate;
            seatRB.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

            var seatCol = seatObj.AddComponent<SphereCollider>();
            seatCol.radius = 5f;
            seatCol.isTrigger = true;


            var seat = seatObj.AddComponent<VehicleSeat>();

            seat.disablePassengerMotor = true;
            seat.ejectOnCollision = false;
            seat.hidePassenger = true;

            seat.enterVehicleContextString = "Stuff1";
            seat.exitVehicleContextString = "Stuff2";

            seat.exitVelocityFraction = 0f;

            seat.seatPosition = seatPos.transform;
            seat.exitPosition = seatObj.transform;

            seat.passengerState = new EntityStates.SerializableEntityStateType( typeof( Skills.Other.DriverState ) );




            seatObj.AddComponent<EngiTurretVehicle>();


            var cam = seatObj.AddComponent<CameraTargetParams>();
            cam.cameraPivotTransform = seatPos.transform;
            cam.aimMode = CameraTargetParams.AimType.Standard;
            cam.idealLocalCameraPos = new Vector3( 0f, 6.71f, -10f );

            var camParams = ScriptableObject.CreateInstance<CharacterCameraParams>();
            camParams.minPitch = -70.0f;
            camParams.maxPitch = 70.0f;
            camParams.pivotVerticalOffset = 5.0f;
            camParams.standardLocalCameraPos = new Vector3( 0f, 1f, -10f );
            camParams.wallCushion = 0.2f;

            cam.cameraParams = camParams;

        }



    }
}
