#if BOSSHPBAR
using RoR2;
using RoR2.UI;
using ReinCore;
//using static RogueWispPlugin.Helpers.APIInterface;

namespace Rein.RogueWispPlugin
{
    internal partial class Main
    {
        partial void EditBossHPBar()
        {
            this.Enable += this.AW_FixBossHud;
            this.Disable += this.AW_UnFixBossHud;
        }

        private void AW_UnFixBossHud() => HooksCore.RoR2.CameraRigController.Start.On -= this.Start_On;

        private void AW_FixBossHud() => HooksCore.RoR2.CameraRigController.Start.On += this.Start_On;
        private void Start_On( HooksCore.RoR2.CameraRigController.Start.Orig orig, CameraRigController self )
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
