using System;
using System.Collections.Generic;
using System.Text;
using BepInEx;
using ReinCore;
using UnityEngine;

namespace Rein.AlternateArtificer
{
    internal partial class Main
    {
        partial void GetBody()
        {
            base.awake += this.Main_awake;
        }

        private void Main_awake()
        {
            this.artiBodyPrefab = Resources.Load<GameObject>( "Prefabs/CharacterBodies/MageBody" );


            if( this.artiBodyPrefab == null )
            {
                base.fail
            }
        }
    }
}
