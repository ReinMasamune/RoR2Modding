using RoR2Plugin;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Engi133769OMEGA
{
    public partial class OmegaTurretMain
    {
        private void GetObjects()
        {
            engi = Resources.Load<GameObject>( "Prefabs/CharacterBodies/EngiBody" );
            body = Resources.Load<GameObject>( "Prefabs/CharacterBodies/EngiWalkerTurretBody" ).InstantiateClone("EngiOmegaTurretBody");
            master = Resources.Load<GameObject>( "Prefabs/CharacterMasters/EngiWalkerTurretMaster" ).InstantiateClone("EngiOmegaTurretMaster");
        }
    }
}
