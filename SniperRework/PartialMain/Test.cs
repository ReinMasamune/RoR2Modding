using BepInEx;
using RoR2;
using UnityEngine;
using R2API;
using R2API.Utils;
using System.Reflection;
using EntityStates;
using RoR2.Skills;
using System;

namespace ReinSniperRework
{
    internal partial class Main
    {
        partial void Test()
        {
            this.FirstFrame += this.ThingsThingsThings;
        }

        private void ThingsThingsThings()
        {
            //var snipe = EntityState.Instantiate( new SerializableEntityStateType( typeof( EntityStates.Huntress.Weapon.FireArrowSnipe ) ) ) as EntityStates.GenericBulletBaseState;
            //Main.LogI( snipe.fireSoundString );
            //Main.LogI( EntityStates.Merc.GroundLight.finisherAttackSoundString );
        }
    }
}


