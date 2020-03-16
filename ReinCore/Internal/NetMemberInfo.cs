using System;
using BepInEx;
using Mono.Cecil;

namespace ReinCore
{
    internal class NetMemberInfo
    {
        internal Int16 msgIndex { get; private set; }
        internal Boolean server { get; private set; }
        internal Boolean client { get; private set; }

        internal NetMemberInfo( CustomAttribute attribute, AssemblyDefinition assembly, TypeDefinition type, MethodDefinition method )
        {
            this.assembly = assembly;
            this.type = type;
            this.method = method;
            this.handler = attribute;

            foreach( var f in this.handler.Fields )
            {
                var name = f.Name;
                if( f.Name == "msgType" )
                {
                    this.msgIndex = (Int16)f.Argument.Value;
                } else if( name == "server" )
                {
                    this.server = (Boolean)f.Argument.Value;
                } else if( name == "client" )
                {
                    this.client = (Boolean)f.Argument.Value;
                }
            }
        }
        internal AssemblyDefinition assembly;
        internal TypeDefinition type;
        internal MethodDefinition method;
        internal CustomAttribute handler;
    }
}
