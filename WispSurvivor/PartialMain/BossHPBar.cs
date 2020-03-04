#if BOSSHPBAR
using RoR2;
using RoR2.UI;
//using static RogueWispPlugin.Helpers.APIInterface;

namespace RogueWispPlugin
{

    internal partial class Main
    {
        partial void EditBossHPBar()
        {
            this.Enable += this.AW_FixBossHud;
            this.Disable += this.AW_UnFixBossHud;
        }

        private void AW_UnFixBossHud() => On.RoR2.CameraRigController.Start -= this.CameraRigController_Start;

        private void AW_FixBossHud() => On.RoR2.CameraRigController.Start += this.CameraRigController_Start;

        private void CameraRigController_Start( On.RoR2.CameraRigController.orig_Start orig, CameraRigController self )
        {
            orig( self );
            if( self.hud )
            {
                HUDBossHealthBarController oldBoss = self.hud.GetComponentInChildren<HUDBossHealthBarController>();
                if( oldBoss )
                {
                    oldBoss.gameObject.AddComponent<ImprovedBossHealthBarController>();
                }
            }
        }
    }
}
#endif
