using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FindSymbols;

namespace SourceGenerator {
    [Generator]
    public class FunctionGenerator : ISourceGenerator {
        private class SyntaxReceiver : ISyntaxReceiver {
            public List<MethodDeclarationSyntax> References = new();

            public void OnVisitSyntaxNode(SyntaxNode syntaxNode) {
                if (syntaxNode is MethodDeclarationSyntax method) {
                    if (HasAttribute(method)) {
                        References.Add(method);
                    }
                }
            }

            private bool HasAttribute(MethodDeclarationSyntax method) {
                return method.AttributeLists.Any() != false;
            }
        }

        public void Initialize(GeneratorInitializationContext context) {
            context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
            //Debugger.Launch();
        }

        public void Execute(GeneratorExecutionContext context) {
            if (context.Compilation.Assembly.Identity.Name != "DoodleDigits.Core") {
                return;
            }

            if (context.SyntaxReceiver is not SyntaxReceiver syntaxReceiver) {
                return;
            }

            CodeBuilder builder = new CodeBuilder();
            builder.AddLines(
                "using System;",
                "using DoodleDigits.Core;",
                "using DoodleDigits.Core.Functions;",
                "",
                "namespace DoodleDigits.Core.Functions;"
            );
            builder.StartBlock("partial class FunctionLibrary");
            builder.StartBlock("static FunctionLibrary()");
            builder.StartBlock("Functions = new FunctionData[]");

            foreach (MethodDeclarationSyntax method in syntaxReceiver.References) {
                var semanticModel = context.Compilation.GetSemanticModel(method.SyntaxTree);
                if (HasFunctionAttribute(semanticModel, method, out var attributeData) == false) {
                    continue;
                }

                string functionNames = $"new [] {{ {string.Join(", ", attributeData!.Names.Select(x => $"\"{x}\""))} }}";
                string functionPath = GetFullMethodName(semanticModel, method);

                if (attributeData.ArgumentCount != null) {
                    string argumentCountString = $"{attributeData.ArgumentCount.Value.min}..{attributeData.ArgumentCount.Value.max}";
                    builder.AddLine($"new({functionNames}, {attributeData.Expects}, {argumentCountString}, {functionPath}),");
                }
                else {
                    builder.AddLine($"new({functionNames}, {attributeData.Expects}, {functionPath}),");
                }
            }

            builder.Unindent();
            builder.AddLine("};");

            builder.EndBlock();
            builder.EndBlock();
            
            context.AddSource("FunctionLibrary.g.cs", builder.ToString());
        }

        private class AttributeData { 
            public readonly string[] Names;
            public readonly string Expects;
            public readonly (int min, int max)? ArgumentCount;

            public AttributeData(string[] names, string expects, (int min, int max)? argumentCount) {
                Names = names;
                Expects = expects;
                ArgumentCount = argumentCount;
            }
            
        }
        
        private bool HasFunctionAttribute(SemanticModel semanticModel, MethodDeclarationSyntax method, out AttributeData? data) {
            foreach (var attributeDecl in method.AttributeLists.SelectMany(al => al.Attributes)) {
                TypeInfo info = semanticModel.GetTypeInfo(attributeDecl);
                if (info.Type?.ToString() == "DoodleDigits.Core.Functions.CalculatorFunctionAttribute") {
                    if (attributeDecl.ArgumentList == null) {
                        continue;
                    }

                    string expectsType = "";
                    List<string> names = new List<string>();
                    (int min, int max)? argumentCount = null;

                    for (var i = 0; i < attributeDecl.ArgumentList.Arguments.Count; i++) {
                        AttributeArgumentSyntax attributeParameter = attributeDecl.ArgumentList.Arguments[i];
                        // First argument is the enum type
                        if (i == 0) {
                            expectsType = attributeParameter.ToFullString();
                        }
                        
                        if (attributeParameter.Expression is LiteralExpressionSyntax literal) {
                            SyntaxKind kind = literal.Kind();
                            if (kind == SyntaxKind.NumericLiteralExpression || kind == SyntaxKind.NumericLiteralToken) {
                                int val = (int) literal.Token.Value!;
                                if (i == 1) {
                                    argumentCount = (val, val);
                                }
                                else {
                                    argumentCount = (argumentCount?.min ?? val, val);
                                }
                            }
                            else if (literal.IsKind(SyntaxKind.StringLiteralExpression)) {
                                names.Add( (string)literal.Token.Value! );
                            }
                        }
                        else if (attributeParameter.Expression is MemberAccessExpressionSyntax access) {
                            
                            if (access.ToString() == "int.MaxValue") {
                                if (i == 1) {
                                    argumentCount = (int.MaxValue, int.MaxValue);
                                } else {
                                    argumentCount = (argumentCount?.min ?? int.MaxValue, int.MaxValue);
                                }
                            }
                        }
                    }

                    data = new AttributeData(names.ToArray(), expectsType, argumentCount);
                    return true;
                }
            }

            data = null;
            return false;
        }

        public static string GetFullMethodName(SemanticModel model, MethodDeclarationSyntax method) {
            ISymbol symbol = model.GetEnclosingSymbol(method.SpanStart) ?? throw new NullReferenceException("symbol was null");
            return symbol.ToDisplayString() + "." + method.Identifier;
        }
    }
}
