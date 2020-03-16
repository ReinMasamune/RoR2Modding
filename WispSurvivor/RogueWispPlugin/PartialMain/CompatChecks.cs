#if COMPATCHECKS
using System;
using System.Collections.Generic;
using System.Reflection;
using RoR2;
using RoR2.Networking;
using Mono.Cecil;

namespace RogueWispPlugin
{
    internal partial class Main
    {
        private HashSet<String> potentialConflicts = new HashSet<String>();
        private HashSet<String> knownConflicts = new HashSet<String>();
        private HashSet<AssemblyDefinition> asmSet = new HashSet<AssemblyDefinition>();
        private BepInEx.PluginInfo r2apiPlugin;
        private Boolean r2apiExists = false;
        partial void CompatChecks()
        {
            this.potentialConflicts.Add( "com.PallesenProductions.ExpandedSkills" );
            this.potentialConflicts.Add( "com.PallesenProductions.RyanSkinAPI" );

            foreach( var p in this.plugins )
            {
                if( p.Metadata.GUID == "com.bepis.r2api" )
                {
                    r2apiExists = true;
                    this.Load += this.Main_Load2;
                }
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

            //this.FirstFrame += this.CheckNetHandlers;
        }

        private void Main_Load2()
        {
            foreach( var p in this.plugins )
            {
                if( p == null || p.Metadata == null ) continue;
                if( p.Metadata.Name == "com.bepis.r2api" )
                {
                    this.r2apiPlugin = p;
                }
            }
        }

        //private void CheckNetHandlers()
        //{
        //    var ror2asm = AssemblyDefinition.ReadAssembly(typeof(RoR2Application).Assembly.Location);
        //    var handlerString = typeof(NetworkMessageHandlerAttribute).FullName;

        //    this.asmSet.Add( ror2asm );
        //    for( Int32 i = 0; i < this.plugins.Count; ++i )
        //    {
        //        var p = this.plugins[i];
        //        if( p == null ) continue;
        //        var asm = AssemblyDefinition.ReadAssembly( p.Location );
        //        if( asm == null ) continue;
        //        this.asmSet.Add( asm );
        //    }

        //    var indexDict = new Dictionary<Int16,List<NetMemberInfo>>();
        //    foreach( var asm in this.asmSet )
        //    {
        //        if( asm == null ) continue;
        //        var mod = asm.MainModule;
        //        if( mod == null ) continue;
        //        foreach( var t in mod.Types )
        //        {
        //            if( t == null ) continue;
        //            foreach( var m in t.Methods )
        //            {
        //                if( m == null || !m.HasCustomAttributes ) continue;

        //                foreach( var at in m.CustomAttributes )
        //                {
        //                    if( at == null ) continue;
        //                    var atType = at.AttributeType;
        //                    if( atType.FullName != handlerString ) continue;
        //                    var info = new NetMemberInfo( at, asm, t, m );
        //                    indexDict[info.msgIndex].Add( info );
        //                }
        //            }
        //        }
        //    }

        //    var good = true;
        //    var serverProblemList = new List<NetMemberInfo>();
        //    var clientProblemList = new List<NetMemberInfo>();

        //    foreach( var kv in indexDict )
        //    {
        //        var curIndex = kv.Key;
        //        var list = kv.Value;

        //        if( list.Count > 1 )
        //        {
        //            var serverCount = 0;
        //            var clientCount = 0;
        //            serverProblemList.Clear();
        //            clientProblemList.Clear();
        //            foreach( var info in list )
        //            {
        //                if( info.server ) ++serverCount;
        //                if( info.client ) ++clientCount;
        //            }

        //            if( serverCount > 1 || clientCount > 1 )
        //            {
        //                Boolean oneIsBaseGame = false;
        //                Boolean selfConflictsFound = false;
        //                var tempset = new HashSet<AssemblyDefinition>();
        //                var selfconflictset = new HashSet<AssemblyDefinition>();
        //                foreach( var info in list )
        //                {
        //                    if( info.assembly == ror2asm ) oneIsBaseGame = true;
        //                    if( tempset.Contains( info.assembly ) )
        //                    {
        //                        selfconflictset.Add( info.assembly );
        //                        tempset.Add( info.assembly );
        //                        selfConflictsFound = true;
        //                    }

        //                    if( serverCount > 0 && info.server ) serverProblemList.Add( info );
        //                    if( clientCount > 0 && info.client ) clientProblemList.Add( info );
        //                }

        //                if( !oneIsBaseGame )
        //                {
        //                    Main.LogF( "Duplicate handlers found for message index: " + curIndex + "\nThis can be caused by a few things:\n" +
        //                        "1: Two or more installed mods are using the same message index.\n" +
        //                        "Inform the authors so they can resolve.\n" +
        //                        "2: Multiple instances of the same mod are running at once.\n" +
        //                        "Double check your mods folder, if issue persists contact author\n" );
        //                } else
        //                {
        //                    Main.LogF( "Duplicate handlers found for vanilla message index: " + curIndex + "\n" +
        //                        "This means that at least one mod has a networking conflict with the base game.\n" +
        //                        "See list below to find which mod and inform the author." );
        //                }

        //                if( selfConflictsFound )
        //                {
        //                    Main.LogF( "In addition some instances of mods conflicting with themselves were found. See list below: " );
        //                    foreach( var asm in selfconflictset )
        //                    {
        //                        Main.LogF( asm.FullName );
        //                    }
        //                }
                   
        //                if( serverCount > 1 )
        //                {
        //                    Main.LogF( "List of duplicate server handlers: " );
        //                    foreach( var info in serverProblemList )
        //                    {
        //                        Main.LogF( "Method: " + info.method.Name + "\nAssembly: " + info.type.FullName );
        //                    }
        //                }
        //                Main.LogF( "" );
        //                if( clientCount > 1 )
        //                {
        //                    Main.LogF( "List of duplicate client handlers: " );
        //                    foreach( var info in serverProblemList )
        //                    {
        //                        Main.LogF( "Method: " + info.method.Name + "\nAssembly: " + info.type.FullName );
        //                    }
        //                }
        //            }    
        //        }
        //    }
        }

        //private class NetMemberInfo
        //{
        //    internal Int16 msgIndex { get; private set; }
        //    internal Boolean server { get; private set; }
        //    internal Boolean client { get; private set; }

        //    internal NetMemberInfo( CustomAttribute attribute, AssemblyDefinition assembly, TypeDefinition type, MethodDefinition method )
        //    {
        //        this.assembly = assembly;
        //        this.type = type;
        //        this.method = method;
        //        this.handler = attribute;

        //        foreach( var f in this.handler.Fields )
        //        {
        //            var name = f.Name;
        //            if( f.Name == "msgType" )
        //            {
        //                this.msgIndex = (Int16)f.Argument.Value;
        //            } else if( name == "server" )
        //            {
        //                this.server = (Boolean)f.Argument.Value;
        //            } else if( name == "client" )
        //            {
        //                this.client = (Boolean)f.Argument.Value;
        //            } else Main.LogW( name );
        //        }
        //    }
        //    internal AssemblyDefinition assembly;
        //    internal TypeDefinition type;
        //    internal MethodDefinition method;
        //    internal CustomAttribute handler;
        //}
    }
}
#endif
