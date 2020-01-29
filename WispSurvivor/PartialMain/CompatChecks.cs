#if COMPATCHECKS
using System;
using System.Collections.Generic;

namespace RogueWispPlugin
{
    internal partial class Main
    {
        private HashSet<String> potentialConflicts = new HashSet<String>();
        private HashSet<String> knownConflicts = new HashSet<String>();
        partial void CompatChecks()
        {
            this.potentialConflicts.Add( "com.PallesenProductions.ExpandedSkills" );
            this.potentialConflicts.Add( "com.PallesenProductions.RyanSkinAPI" );

            foreach( var p in this.plugins )
            {
                if( this.knownConflicts.Contains( p.Metadata.GUID ) )
                {
                    Main.LogE( "Rogue wisp has known conflicts with: " + p.Metadata.Name + " Please test without " + p.Metadata.Name + " before reporting bugs." );
                    continue;
                }
                if( this.potentialConflicts.Contains( p.Metadata.GUID ) )
                {
                    Main.LogW( "Rogue Wisp may have conflicts with: " + p.Metadata.Name );
                    continue;
                }
            }
        }
    }
}
#endif
