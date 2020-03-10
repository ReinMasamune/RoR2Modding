#if BOSSHPBAR
using RoR2;
using RoR2.UI;
using ReinCore;
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

        private void AW_UnFixBossHud() => HooksCore.on_RoR2_CameraRigController_Start -= this.HooksCore_on_RoR2_CameraRigController_Start1;

        private void AW_FixBossHud() => HooksCore.on_RoR2_CameraRigController_Start += this.HooksCore_on_RoR2_CameraRigController_Start1;
        private void HooksCore_on_RoR2_CameraRigController_Start1( HooksCore.orig_RoR2_CameraRigController_Start orig, CameraRigController self )
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

        //private void CameraRigController_Start( On.RoR2.CameraRigController.orig_Start orig, CameraRigController self )
        //{
        //    orig( self );
        //    if( self.hud )
        //    {
        //        HUDBossHealthBarController oldBoss = self.hud.GetComponentInChildren<HUDBossHealthBarController>();
        //        if( oldBoss )
        //        {
        //            oldBoss.gameObject.AddComponent<ImprovedBossHealthBarController>();
        //        }
        //    }
        //}
    }
}
#endif
