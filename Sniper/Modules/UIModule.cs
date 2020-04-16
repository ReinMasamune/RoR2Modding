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
using RoR2.UI;

namespace Sniper.Modules
{
    internal static class UIModule
    {
        internal static GameObject GetDefaultDrosshair()
        {
            // TODO: Create default crosshair
            return null;
        }

        internal static GameObject GetScopeCrosshair()
        {
            // TODO: Create scope crosshair
            return null;
        }

        internal static Texture GetPortraitIcon()
        {
            // TODO: Portrait Icon
            return Properties.Tools.LoadTexture2D(Properties.Resources.unknown__11_);
            //return null;
        }

        internal static GameObject GetQuickScope()
        {
            // TODO: Quick scope UI
            return null;
        }

        internal static GameObject GetChargeScope()
        {
            // TODO: Charge scope UI
            return null;
        }
    }
}
