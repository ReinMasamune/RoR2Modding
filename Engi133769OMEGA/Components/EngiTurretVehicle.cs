using RoR2;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace Engi133769OMEGA.Components
{
    public class EngiTurretVehicle : MonoBehaviour
    {
        private float minExitTime = 1.0f;
        private float minEnterTime = 1.0f;
        private float exitTimer = 0f;
        private float enterTimer = 0f;
        private Boolean prepared = false;
        private VehicleSeat seat;
        private InputBankTest inputs;
        private CharacterMaster turretMaster;
        private MinionOwnership turretOwnership;
        private CharacterBody turretBody;
        private CharacterMaster ownerMaster;

        private void Seat_onPassengerEnter( GameObject obj )
        {
            Debug.Log( "Stuff1" );
            exitTimer = 0f;
            VehicleSeat.onPassengerEnterGlobal += this.VehicleSeat_onPassengerEnterGlobal;
        }

        private void Seat_onPassengerExit( GameObject obj )
        {
            Debug.Log( "Stuff2" );
            enterTimer = 0f;
            VehicleSeat.onPassengerExitGlobal += this.VehicleSeat_onPassengerExitGlobal;
        }

        private void VehicleSeat_onPassengerEnterGlobal( VehicleSeat arg1, GameObject arg2 )
        {
            Debug.Log( "SStuff1" );
            arg1.transform.parent.GetComponent<InputMaster>().SetMaster( arg2 );

            VehicleSeat.onPassengerEnterGlobal -= this.VehicleSeat_onPassengerEnterGlobal;
        }

        private void VehicleSeat_onPassengerExitGlobal( VehicleSeat arg1, GameObject arg2 )
        {
            Debug.Log( "SStuff2" );
            arg1.transform.parent.GetComponent<InputMaster>().UnsetMaster();

            VehicleSeat.onPassengerExitGlobal -= this.VehicleSeat_onPassengerExitGlobal;
        }

        private void OnEnable()
        {
            seat = GetComponent<VehicleSeat>();
            inputs = transform.parent.GetComponent<InputBankTest>();

            seat.onPassengerEnter += this.Seat_onPassengerEnter;
            seat.onPassengerExit += this.Seat_onPassengerExit;
            seat.enterVehicleAllowedCheck.AddCallback( new CallbackCheck<Interactability, CharacterBody>.CallbackDelegate( this.EnterCheck ) );
            seat.exitVehicleAllowedCheck.AddCallback( new CallbackCheck<Interactability, CharacterBody>.CallbackDelegate( this.ExitCheck ) );

            prepared = false;

            StartCoroutine( GetTurretMasterStuff() );
        }

        private void FixedUpdate()
        {
            if( exitTimer <= minExitTime )
            {
                exitTimer += Time.fixedDeltaTime;
            } else if( inputs && inputs.interact.justPressed && seat.currentPassengerBody )
            {
                //inputs = null;
                seat.EjectPassenger( seat.currentPassengerBody.gameObject );
            }

            if( enterTimer <= minEnterTime )
            {
                enterTimer += Time.fixedDeltaTime;
            }
        }

        private void OnDisable()
        {
            seat.onPassengerEnter -= this.Seat_onPassengerEnter;
            seat.onPassengerExit -= this.Seat_onPassengerExit;
            seat.enterVehicleAllowedCheck.RemoveCallback( new CallbackCheck<Interactability, CharacterBody>.CallbackDelegate( this.EnterCheck ) );
            seat.exitVehicleAllowedCheck.RemoveCallback( new CallbackCheck<Interactability, CharacterBody>.CallbackDelegate( this.ExitCheck ) );
        }

        private void EnterCheck( CharacterBody body, ref Interactability? resultOverride )
        {
            if( enterTimer > minEnterTime && prepared && body.master == ownerMaster )
            {
                resultOverride = Interactability.Available;
            } else
            {
                resultOverride = Interactability.ConditionsNotMet;
            }
        }

        private void ExitCheck( CharacterBody body, ref Interactability? resultOverride )
        {
            if( exitTimer > minExitTime )
            {
                resultOverride = Interactability.Available;
            } else
            {
                resultOverride = Interactability.ConditionsNotMet;
            }
        }

        private IEnumerator GetTurretMasterStuff()
        {
            yield return new WaitForSeconds( 3f );

            int i = 0;
            Debug.Log( i++ );
            turretBody = transform.parent.GetComponent<CharacterBody>();
            Debug.Log( i++ );
            turretMaster = turretBody.master;
            Debug.Log( i++ );
            turretOwnership = turretMaster.minionOwnership;
            Debug.Log( i++ );
            ownerMaster = turretOwnership.ownerMaster;
            Debug.Log( i++ );
            prepared = true;
            Debug.Log( i++ );
        }
    }
}
