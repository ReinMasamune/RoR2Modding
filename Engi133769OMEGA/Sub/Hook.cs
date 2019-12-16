using System;
using System.Collections.Generic;
using System.Text;

namespace Engi133769OMEGA
{
    public partial class OmegaTurretMain
    {
        public override void RemoveHooks()
        {

        }

        public override void CreateHooks()
        {
            //On.RoR2.RunCameraManager.GetNetworkUserBodyObject += this.RunCameraManager_GetNetworkUserBodyObject;
        }

        private UnityEngine.GameObject RunCameraManager_GetNetworkUserBodyObject( On.RoR2.RunCameraManager.orig_GetNetworkUserBodyObject orig, RoR2.NetworkUser networkUser )
        {
            var body = networkUser.GetCurrentBody();
            if( body && body.baseNameToken == "ENGI_BODY_NAME" && body.currentVehicle && body.currentVehicle.gameObject.name == "EngiTurretVehicleSeat" )
            {
                return body.currentVehicle.transform.parent.gameObject;
            } else
            {
                return orig( networkUser );
            }
        }
    }
}
