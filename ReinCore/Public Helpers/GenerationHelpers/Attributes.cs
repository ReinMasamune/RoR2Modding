namespace ReinCore
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Security;
    using System.Security.Permissions;
    using System.Text;

    using Mono.Cecil;


    public static partial class GenAttributeHelpers
    {
        private static readonly ConstructorInfo unverifiableCodeAttribute;
        private static readonly ConstructorInfo securityPermissionAttribute;
        private static readonly ConstructorInfo ignoreAccessChecksToAttribute;
        private static readonly Dictionary<ModuleDefinition, HashSet<String>> ignoredAssemblies = new Dictionary<ModuleDefinition, HashSet<String>>();

        static GenAttributeHelpers()
        {
            unverifiableCodeAttribute = typeof(UnverifiableCodeAttribute).GetConstructor(Type.EmptyTypes);
            securityPermissionAttribute = typeof(SecurityPermissionAttribute).GetConstructor(new[] { typeof(System.Security.Permissions.SecurityAction) });
            ignoreAccessChecksToAttribute = typeof(IgnoresAccessChecksToAttribute).GetConstructor(new[] { typeof(String) });

            if(unverifiableCodeAttribute is null) Log.Fatal($"Unable to find {nameof(UnverifiableCodeAttribute)} constructor");
            if(securityPermissionAttribute is null) Log.Fatal($"Unable to find {nameof(SecurityPermissionAttribute)} constructor");
            if(ignoreAccessChecksToAttribute is null) Log.Fatal($"Unable to find {nameof(IgnoresAccessChecksToAttribute)} constructor");
        }

        public static void AddStandardAttributes(this ModuleDefinition module)
        {
            module.CustomAttributes.Add(new CustomAttribute(module.ImportReference(unverifiableCodeAttribute)));
            var securityAtr = new CustomAttribute(module.ImportReference(securityPermissionAttribute));
            var securityActionTyperef = module.ImportReference(typeof(System.Security.Permissions.SecurityAction));
            securityAtr.ConstructorArguments.Add(new CustomAttributeArgument(securityActionTyperef, System.Security.Permissions.SecurityAction.RequestMinimum));
            securityAtr.Properties.Add(new Mono.Cecil.CustomAttributeNamedArgument("SkipVerification", new CustomAttributeArgument(module.TypeSystem.Boolean, true)));
            module.Assembly.CustomAttributes.Add(securityAtr);
        }

        public static void AddIgnoreAccessAttribute(this ModuleDefinition module, params Type[] types)
        {
            HashSet<String> ignored = ignoredAssemblies.TryGetValue( module, out HashSet<String> temp ) ? temp : (ignoredAssemblies[module] = new HashSet<String>());
            foreach(String name in types.Select((t) => t.Assembly.FullName).Where((n) => ignored.Add(n))) module.AddOneIgnore(name); 
        }

        private static void AddOneIgnore(this ModuleDefinition module, String name)
        {
            var atr = new CustomAttribute(module.ImportReference(ignoreAccessChecksToAttribute));
            atr.ConstructorArguments.Add(new CustomAttributeArgument(module.TypeSystem.String, name));
            module.Assembly.CustomAttributes.Add(atr);
        }
    }
}
