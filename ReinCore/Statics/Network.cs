using System;
using System.Collections.Generic;
using BepInEx;
using Mono.Cecil;
using RoR2;
using RoR2.Networking;

namespace ReinCore
{
    /// <summary>
    /// 
    /// </summary>
    public static class NetworkCore
    {
        internal static void Check()
        {
            var ror2asm = AssemblyDefinition.ReadAssembly(typeof(RoR2Application).Assembly.Location);
            var handlerString = typeof(NetworkMessageHandlerAttribute).FullName;
            var asmSet = new HashSet<AssemblyDefinition>();
            asmSet.Add( ror2asm );

            foreach( var pl in ReinCore.plugins )
            {
                if( pl == null ) continue;
                var asm = AssemblyDefinition.ReadAssembly( pl.Location );
                if( asm == null ) continue;
                asmSet.Add( asm );
            }

            var indexDict = new Dictionary<Int16,List<NetMemberInfo>>();
            foreach( var asm in asmSet )
            {
                if( asm == null ) continue;
                var mod = asm.MainModule;
                if( mod == null ) continue;
                foreach( var t in mod.Types )
                {
                    if( t == null ) continue;
                    foreach( var m in t.Methods )
                    {
                        if( m == null || !m.HasCustomAttributes ) continue;

                        foreach( var at in m.CustomAttributes )
                        {
                            if( at == null ) continue;
                            var atType = at.AttributeType;
                            if( atType.FullName != handlerString ) continue;
                            var info = new NetMemberInfo( at, asm, t, m );
                            indexDict[info.msgIndex].Add( info );
                        }
                    }
                }
            }

            var serverProblemList = new List<NetMemberInfo>();
            var clientProblemList = new List<NetMemberInfo>();

            foreach( var kv in indexDict )
            {
                var curIndex = kv.Key;
                var list = kv.Value;

                if( list.Count > 1 )
                {
                    var serverCount = 0;
                    var clientCount = 0;
                    serverProblemList.Clear();
                    clientProblemList.Clear();
                    foreach( var info in list )
                    {
                        if( info.server ) ++serverCount;
                        if( info.client ) ++clientCount;
                    }

                    if( serverCount > 1 || clientCount > 1 )
                    {
                        Boolean oneIsBaseGame = false;
                        Boolean selfConflictsFound = false;
                        var tempset = new HashSet<AssemblyDefinition>();
                        var selfconflictset = new HashSet<AssemblyDefinition>();
                        foreach( var info in list )
                        {
                            if( info.assembly == ror2asm ) oneIsBaseGame = true;
                            if( tempset.Contains( info.assembly ) )
                            {
                                selfconflictset.Add( info.assembly );
                                tempset.Add( info.assembly );
                                selfConflictsFound = true;
                            }

                            if( serverCount > 0 && info.server ) serverProblemList.Add( info );
                            if( clientCount > 0 && info.client ) clientProblemList.Add( info );
                        }

                        if( !oneIsBaseGame )
                        {
                            Log.Fatal( "Duplicate handlers found for message index: " + curIndex + "\nThis can be caused by a few things:\n" +
                                "1: Two or more installed mods are using the same message index.\n" +
                                "Inform the authors so they can resolve.\n" +
                                "2: Multiple instances of the same mod are running at once.\n" +
                                "Double check your mods folder, if issue persists contact author\n" );
                        } else
                        {
                            Log.Fatal( "Duplicate handlers found for vanilla message index: " + curIndex + "\n" +
                                "This means that at least one mod has a networking conflict with the base game.\n" +
                                "See list below to find which mod and inform the author." );
                        }

                        if( selfConflictsFound )
                        {
                            Log.Fatal( "In addition some instances of mods conflicting with themselves were found. See list below: " );
                            foreach( var asm in selfconflictset )
                            {
                                Log.Fatal( asm.FullName );
                            }
                        }

                        if( serverCount > 1 )
                        {
                            Log.Fatal( "List of duplicate server handlers: " );
                            foreach( var info in serverProblemList )
                            {
                                Log.Fatal( "Method: " + info.method.Name + "\nAssembly: " + info.type.FullName );
                            }
                        }
                        Log.Fatal( "" );
                        if( clientCount > 1 )
                        {
                            Log.Fatal( "List of duplicate client handlers: " );
                            foreach( var info in serverProblemList )
                            {
                                Log.Fatal( "Method: " + info.method.Name + "\nAssembly: " + info.type.FullName );
                            }
                        }
                    }
                }
            }
        }
    }
}
