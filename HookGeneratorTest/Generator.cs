namespace HookGeneratorTest
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    [Generator]
    public class HookGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(Generate<SyntaxHandler>);
        }


        public void Execute(GeneratorExecutionContext context)
        {
            if(context.SyntaxReceiver is not SyntaxHandler handler) return;

            var candidates = handler.candidates;
            for(Int32 i = 0; i < candidates.Count; ++i)
            {
                var cand = candidates[i];
                var fullName = BuildFullName(cand.nameSyntax);
                
            }
        }

        private static String BuildFullName(SimpleNameSyntax nameSyntax) => nameSyntax is null ? "" : $"{BuildFullName(nameSyntax.Parent as SimpleNameSyntax)}{nameSyntax.Identifier.Text}.";



        private static TReciever Generate<TReciever>()
            where TReciever : ISyntaxReceiver, new() => new();
    }

    

    internal class SyntaxHandler : ISyntaxReceiver
    {
        internal List<HookCandidateData> candidates { get; } = new();
        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if(syntaxNode is ExpressionStatementSyntax expression && expression.Expression is BinaryExpressionSyntax binExpression)
            {
                if(binExpression.IsKind(Microsoft.CodeAnalysis.CSharp.SyntaxKind.AddAssignmentExpression) || binExpression.IsKind(Microsoft.CodeAnalysis.CSharp.SyntaxKind.SubtractAssignmentExpression))
                {
                    if(binExpression.Left is MemberAccessExpressionSyntax memberAccess && memberAccess.Name is SimpleNameSyntax name && name.IsMissing)
                    {
                        HookType hookType;
                        switch(name.Identifier.Text)
                        {
                            case "On":
                                hookType = HookType.On;
                                break;
                            case "IL":
                                hookType = HookType.IL;
                                break;
                            default:
                                return;
                        }

                        this.candidates.Add(new HookCandidateData
                        {
                            type = hookType,
                            nameSyntax = name,
                        });
                    }
                }
            }
        }
    }

    internal struct HookCandidateData
    {
        internal HookType type;
        internal SimpleNameSyntax nameSyntax;
    }


    internal enum HookType { On, IL }


    internal static class Helpers
    {
        
        internal static String GetHookSourceString(HookType hookType, String namespaceName, String methodName, String delegateType) => $@"
namespace Hooks.{namespaceName}
{{
    internal static partial class {methodName}
    {{
{WriteHookEvent(2, hookType)}
    }}
}}";

        internal static String Tab(Int32 tabs) => new(' ', tabs);


        internal static String WriteHookEvent(Int32 tabs, HookType hookType) => hookType switch
        {
            HookType.On => WriteOnHookEvent(tabs),
            HookType.IL => WriteILHookEvent(tabs),
        };


        private static HashSet<String> methodGettersGenerated = new();
        internal static String WriteMethodInfoInit() => $"";

        internal static String WriteOnHookEvent(Int32 tabs) => $@"
{Tab(tabs)}{WriteMethodInfoInit()}
{Tab(tabs)}
";
        internal static String WriteILHookEvent(Int32 tabs) => $@"
{Tab(tabs)}{WriteMethodInfoInit()}
{Tab(tabs)}
";
    }
}
