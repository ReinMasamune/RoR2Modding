//using static RogueWispPlugin.Helpers.APIInterface;

namespace RogueWispPlugin
{
#if ANCIENTWISP
    internal partial class Main
    {
        private GameObject AW_body;
        private GameObject AW_master;
        private GameObject AW_primaryProj;
        private GameObject AW_secDelayEffect;
        private GameObject AW_secExplodeEffect;
        private GameObject AW_utilProj;
        private GameObject AW_utilZoneProj;

        partial void AW_Test();
        partial void AW_General();
        partial void AW_Hook();

        partial void CreateAncientWisp()
        {
            this.AW_Test();
            this.AW_General();
            this.AW_Hook();
        }
    }
#endif
}
