using R2API;
using UnityEngine;
//using static RogueWispPlugin.Helpers.APIInterface;

namespace RogueWispPlugin
{
#if ROGUEWISP
    internal partial class Main
    {
        private GameObject RW_crosshair;
        partial void RW_UI() => this.Load += this.RW_CreateCrosshair;

        private void RW_CreateCrosshair()
        {
            GameObject baseUI = Resources.Load<GameObject>("Prefabs/Crosshair/HuntressSnipeCrosshair").InstantiateClone("WispCrosshair", false);
            this.RW_crosshair = baseUI;
        }
    }
#endif
}
