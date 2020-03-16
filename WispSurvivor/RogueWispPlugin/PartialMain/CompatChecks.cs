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
        private HashSet<Assembly> asmSet = new HashSet<Assembly>();
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

            this.FirstFrame += this.CheckNetHandlers;
        }

        private void Main_Load2()
        {
            foreach( var p in this.plugins )
            {
                if( p.Metadata.Name == "com.bepis.r2api" )
                {
                    this.r2apiPlugin = p;
                }
            }
        }

        private void CheckNetHandlers()
        {
            var ror2asm = AssemblyDefinition.ReadAssembly()

            this.asmSet.Add( ror2asm );
            foreach( var p in this.plugins )
            {
                this.asmSet.Add( p.Instance.GetType().Assembly );
            }


            var indexDict = new Dictionary<Int16,List<NetMemberInfo>>();
            var allFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
            foreach( var asm in this.asmSet )
            {
                Type[] types;
                try
                {
                    types = asm.GetTypes();
                } catch
                {
                    Main.LogW( "Error during network checks for assembly: " + asm.FullName );
                    continue;
                }
                foreach( var t in asm.GetTypes() )
                {
                    foreach( var member in t.GetMethods(allFlags))
                    {
                        var attrib = member.GetCustomAttribute<NetworkMessageHandlerAttribute>();
                        if( attrib != null )
                        {
                            var index = attrib.msgType;
                            if( !indexDict.ContainsKey( index ) )
                            {
                                indexDict[index] = new List<NetMemberInfo>();
                            }

                            indexDict[index].Add( new NetMemberInfo { assembly = asm, type = t, member = member, attribute = attrib } );
                        }
                    }
                }
            }

            var good = true;
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
                        if( info.attribute.server ) ++serverCount;
                        if( info.attribute.client ) ++clientCount;
                    }

                    if( serverCount > 1 || clientCount > 1 )
                    {
                        Boolean oneIsBaseGame = false;
                        Boolean selfConflictsFound = false;
                        var tempset = new HashSet<Assembly>();
                        var selfconflictset = new HashSet<Assembly>();
                        foreach( var info in list )
                        {
                            if( info.assembly == ror2asm ) oneIsBaseGame = true;
                            if( tempset.Contains( info.assembly ) )
                            {
                                selfconflictset.Add( info.assembly );
                                tempset.Add( info.assembly );
                                selfConflictsFound = true;
                            }

                            if( serverCount > 0 && info.attribute.server ) serverProblemList.Add( info );
                            if( clientCount > 0 && info.attribute.client ) clientProblemList.Add( info );
                        }

                        if( !oneIsBaseGame )
                        {
                            Main.LogF( "Duplicate handlers found for message index: " + curIndex + "\nThis can be caused by a few things:\n" +
                                "1: Two or more installed mods are using the same message index.\n" +
                                "Inform the authors so they can resolve.\n" +
                                "2: Multiple instances of the same mod are running at once.\n" +
                                "Double check your mods folder, if issue persists contact author\n" );
                        } else
                        {
                            Main.LogF( "Duplicate handlers found for vanilla message index: " + curIndex + "\n" +
                                "This means that at least one mod has a networking conflict with the base game.\n" +
                                "See list below to find which mod and inform the author." );
                        }

                        if( selfConflictsFound )
                        {
                            Main.LogF( "In addition some instances of mods conflicting with themselves were found. See list below: " );
                            foreach( var asm in selfconflictset )
                            {
                                Main.LogF( asm.FullName );
                            }
                        }
                   
                        if( serverCount > 1 )
                        {
                            Main.LogF( "List of duplicate server handlers: " );
                            foreach( var info in serverProblemList )
                            {
                                Main.LogF( "Method: " + info.member.Name + "\nAssembly: " + info.type.AssemblyQualifiedName );
                            }
                        }
                        Main.LogF( "" );
                        if( clientCount > 1 )
                        {
                            Main.LogF( "List of duplicate client handlers: " );
                            foreach( var info in serverProblemList )
                            {
                                Main.LogF( "Method: " + info.member.Name + "\nAssembly: " + info.type.AssemblyQualifiedName );
                            }
                        }
                    }    
                }
            }
        }

        private class NetMemberInfo
        {
            internal Assembly assembly;
            internal Type type;
            internal MemberInfo member;
            internal NetworkMessageHandlerAttribute attribute;
        }
    }
}
#endif
