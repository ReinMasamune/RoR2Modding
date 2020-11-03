namespace HookGeneratorTest
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;

    [Generator]
    public class HookGenerator : ISourceGenerator
    {
        static DiagnosticDescriptor iHateYou = new DiagnosticDescriptor(
            id: "IHateMyLifeSoMuchRightNow",
            title: "TELL ME IF MY CODE IS RUNNING",
            messageFormat: "Message: {0}",
            category: "WHO CARES",
            defaultSeverity: DiagnosticSeverity.Error,
            isEnabledByDefault: true
        );

        static void Log(GeneratorExecutionContext ctx, String message)
        {
            ctx.ReportDiagnostic(Diagnostic.Create(iHateYou, null, message,));
            throw new Exception(message);
        }


        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(Generate<SyntaxHandler>);
        }


        public void Execute(GeneratorExecutionContext context)
        {
            void Log(String str) => HookGenerator.Log(context, str);
            if(context.SyntaxReceiver is not SyntaxHandler handler) return;

            //Log("ExecuteStart");

            var candidates = handler.candidates;
            Log($"{candidates.Count}::    {String.Join(":  ", candidates.Select(a => a.text))}");
            for(Int32 i = 0; i < candidates.Count; ++i)
            {
                var cand = candidates[i];
                //var fullName = BuildFullName(cand.nameSyntax);


                //Log(fullName);              
            }
        }

        private static String BuildFullName(SimpleNameSyntax nameSyntax) => nameSyntax is null ? "" : $"{BuildFullName(nameSyntax.Parent as SimpleNameSyntax)}{nameSyntax.Identifier.Text}.";



        private static TReciever Generate<TReciever>()
            where TReciever : ISyntaxReceiver, new() => new();
    }

    

    internal class SyntaxHandler : ISyntaxReceiver
    {
        private static String BuildFullName(SimpleNameSyntax nameSyntax) => nameSyntax is null ? "" : $"{BuildFullName(nameSyntax.ChildNodes()?.FirstOrDefault() as SimpleNameSyntax)}{nameSyntax.Identifier.Text}.";

        internal List<HookCandidateData> candidates { get; } = new();
        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if(syntaxNode is ExpressionStatementSyntax expression && expression.Expression is AssignmentExpressionSyntax assignExpression)
            {
                if(assignExpression.IsKind(Microsoft.CodeAnalysis.CSharp.SyntaxKind.AddAssignmentExpression) || assignExpression.IsKind(Microsoft.CodeAnalysis.CSharp.SyntaxKind.SubtractAssignmentExpression))
                {
                    if(assignExpression.Left is MemberAccessExpressionSyntax memberAccess && memberAccess.GetDiagnostics() is IEnumerable<Diagnostic> diags && memberAccess.Name is SimpleNameSyntax name)
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

                        static String GetFullName(MemberAccessExpressionSyntax member)
                        {
                            return $"{member.GetText()}";
                        }

                        this.candidates.Add(new HookCandidateData
                        {
                            accessExpressionSyntax = memberAccess,
                            type = hookType,
                            text = GetFullName(memberAccess),
                        });

                        //this.candidates.Add(new HookCandidateData
                        //{
                        //    type = hookType,
                        //    nameSyntax = name,
                        //});
                    }
                }
            }
        }
    }

    internal struct HookCandidateData
    {
        internal HookType type;
        internal MemberAccessExpressionSyntax accessExpressionSyntax;
        internal String text;
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
            _ => null,
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
