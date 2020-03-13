#if ROGUEWISP
using ReinCore;
//using static RogueWispPlugin.Helpers.APIInterface;

namespace RogueWispPlugin
{

    internal partial class Main
    {
        partial void RW_Orb() => this.Load += this.RW_AddOrbs;

        private void RW_AddOrbs()
        {
            //R2API.OrbAPI.AddOrb( typeof( RestoreOrb ) );
            //R2API.OrbAPI.AddOrb( typeof( SnapOrb ) );
            //R2API.OrbAPI.AddOrb( typeof( SparkOrb ) );
            //R2API.OrbAPI.AddOrb( typeof( BlazeOrb ) );
            //R2API.OrbAPI.AddOrb( typeof( IgnitionOrb ) );
            OrbsCore.getAdditionalOrbs += ( list ) =>
            {
                list.Add( typeof( RestoreOrb ) );
                list.Add( typeof( SnapOrb ) );
                list.Add( typeof( SparkOrb ) );
                list.Add( typeof( BlazeOrb ) );
                list.Add( typeof( IgnitionOrb ) );
            };
        }
    }

}
#endif