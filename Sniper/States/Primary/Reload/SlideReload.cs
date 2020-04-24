using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BepInEx.Logging;
using ReinCore;
using RoR2;
using RoR2.Networking;
using UnityEngine;
using KinematicCharacterController;
using EntityStates;
using RoR2.Skills;
using System.Reflection;
using Sniper.States.Bases;
using UnityEngine.Networking;

namespace Sniper.States.Primary.Reload
{
    internal class SlideReload : SniperSkillBaseState, ISniperReloadState
    {
        internal Vector3 slideDirection;

        public override void OnEnter()
        {
            base.OnEnter();

            if( base.isAuthority )
            {
                if( base.inputBank )
                {
                    this.slideDirection = base.inputBank.moveVector.normalized;
                }
            }

            if( this.slideDirection != Vector3.zero )
            {
                // TODO: Slide animation
                // TODO: Slide sounds
                // TODO: Slide VFX
            }

        }

        public override void OnSerialize( NetworkWriter writer )
        {
            base.OnSerialize( writer );
            writer.Write( new PackedUnitVector3( this.slideDirection ) );
        }

        public override void OnDeserialize( NetworkReader reader )
        {
            base.OnDeserialize( reader );
            this.slideDirection = reader.ReadPackedUnitVector3().Unpack();
        }


    }
}
