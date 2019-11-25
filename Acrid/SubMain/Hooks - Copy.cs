using R2API.Utils;
using RoR2;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Acrid
{
    public partial class AcridMain
    {
        public void EditModel()
        {
            body.GetComponent<ModelLocator>().modelBaseTransform.localScale = new Vector3( 0.5f, 0.5f, 0.5f );
        }

        public void MiscEdits()
        {
            KinematicCharacterController.KinematicCharacterMotor kinMot = body.GetComponent<KinematicCharacterController.KinematicCharacterMotor>();
            kinMot.SetFieldValue<Single>( "CapsuleRadius", 0.75f );
            kinMot.SetFieldValue<Single>( "CapsuleHeight", 2f );
            kinMot.SetFieldValue<Single>( "CapsuleYOffset", -1.5f );



            SetStateOnHurt stateOnHurt = body.GetComponent<SetStateOnHurt>();
            stateOnHurt.canBeFrozen = true;
            stateOnHurt.canBeStunned = false;
            stateOnHurt.canBeHitStunned = false;
        }
    }
}
