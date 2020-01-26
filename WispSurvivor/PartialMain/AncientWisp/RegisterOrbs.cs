#if ANCIENTWISP
using R2API;

namespace RogueWispPlugin
{
    internal partial class Main
    {
        partial void AW_Orbs()
        {
            this.Load += this.AW_RegisterOrbs;
        }

        private void AW_RegisterOrbs()
        {
            OrbAPI.AddOrb( typeof( UniversalHealOrb ) );
        }
    }
}
#endif
