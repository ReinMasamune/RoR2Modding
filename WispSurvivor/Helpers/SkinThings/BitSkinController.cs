
using RoR2;
using System;
using UnityEngine;

namespace RogueWispPlugin.Helpers
{
    internal abstract class BitSkinController : MonoBehaviour
    {
        internal abstract void Apply( IBitSkin skin );
    }
}
