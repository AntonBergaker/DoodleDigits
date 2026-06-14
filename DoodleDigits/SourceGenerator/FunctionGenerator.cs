using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SourceGenerator {
    [Generator]
    public class FunctionGenerator : IIncrementalGenerator {

        private class AttributeData {
            public readonly string[] Names;
            public readonly string Expects;
            public readonly string FunctionPath;
            public readonly (int min, int max)? ArgumentCount;

            public AttributeData(string[] names, string expects, string functionPath, (int min, int max)? argumentCount) {
                Names = names;
                Expects = expects;
                FunctionPath = functionPath;
                ArgumentCount = argumentCount;
            }

            public bool Equals(AttributeData? other) {
                if (other is null) return false;

                return
                    Names.SequenceEqual(other.Names) &&
                    string.Equals(Expects, other.Expects, StringComparison.Ordinal) &&
                    string.Equals(FunctionPath, other.FunctionPath, StringComparison.Ordinal) &&
                    ArgumentCount == other.ArgumentCount;
            }

            public override bool Equals(object? obj) => Equals(obj as AttributeData);

            public override int GetHashCode() {
                unchecked {
                    int hash = 17;

                    if (Names != null) {
                        foreach (var n in Names)
                            hash = hash * 31 + n.GetHashCode();
                    }

                    hash = hash * 31 + Expects.GetHashCode();
                    hash = hash * 31 + FunctionPath.GetHashCode();
                    hash = hash * 31 + ArgumentCount.GetHashCode();

                    return hash;
                }
            }
        }


        public void Initialize(IncrementalGeneratorInitializationContext context) {
            //System.Diagnostics.Debugger.Launch();
            var flaggedFunctions = context.SyntaxProvider.ForAttributeWithMetadataName(
                "DoodleDigits.Core.Functions.CalculatorFunctionAttribute",
                (syntax, _) => syntax is MethodDeclarationSyntax,
                TransformMethod
            );
            var filtered = flaggedFunctions.Where((x) => x != null).Select((x, _) => x!);
            context.RegisterImplementationSourceOutput(filtered.Collect(), Execute);

        }

        private readonly SymbolDisplayFormat _format = new SymbolDisplayFormat(
            globalNamespaceStyle: SymbolDisplayGlobalNamespaceStyle.Omitted,
            typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces,
            memberOptions: SymbolDisplayMemberOptions.IncludeContainingType,
            parameterOptions: SymbolDisplayParameterOptions.None);

        private AttributeData? TransformMethod(GeneratorAttributeSyntaxContext context, CancellationToken token) {
            if (context.TargetSymbol is not IMethodSymbol method) {
                return null;
            }

            var attribute = context.Attributes.First();
            var attributeParameters = attribute.ConstructorArguments;

            if (attributeParameters.Length < 2) {
                return null;
            }

            // First argument is return type as an enum
            var expectsType = $"(DoodleDigits.Core.Functions.FunctionExpectedType){(int)attributeParameters[0].Value!}";
            if (expectsType == null) {
                return null;
            }
            List<string> names = new List<string>();
            (int min, int max)? argumentCount = null;



            for (var i = 1; i < attributeParameters.Length; i++) {
                var parameter = attributeParameters[i];

                if (parameter.Kind == TypedConstantKind.Primitive && parameter.Value is string str) {
                    names.Add(str);
                    continue;
                }
                if (parameter.Kind == TypedConstantKind.Array) {
                    names.AddRange(parameter.Values.Select(x => x.Value).OfType<string>());
                }
                if (parameter.Kind == TypedConstantKind.Primitive && parameter.Value is int int0) {
                    int min = int0;
                    int max = int0;

                    if (attributeParameters.Length > i + 1 && attributeParameters[i + 1].Value is int int1) {
                        max = int1;
                        i++;
                    }
                    argumentCount = (min, max);
                    continue;
                }
            }

            string name = method.ToDisplayString(_format);

            return new AttributeData(names.ToArray(), expectsType, name, argumentCount);
        }


        private void Execute(SourceProductionContext context, ImmutableArray<AttributeData> attributeDatas) {


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

            foreach (var attributeData in attributeDatas) {

                string functionNames = $"new [] {{ {string.Join(", ", attributeData!.Names.Select(x => $"\"{x}\""))} }}";

                if (attributeData.ArgumentCount != null) {
                    string argumentCountString = $"{attributeData.ArgumentCount.Value.min}..{attributeData.ArgumentCount.Value.max}";
                    builder.AddLine($"new({functionNames}, {attributeData.Expects}, {argumentCountString}, {attributeData.FunctionPath}),");
                }
                else {
                    builder.AddLine($"new({functionNames}, {attributeData.Expects}, {attributeData.FunctionPath}),");
                }
            }

            builder.Unindent();
            builder.AddLine("};");

            builder.EndBlock();
            builder.EndBlock();
            
            context.AddSource("FunctionLibrary.g.cs", builder.ToString());
        }

    }
}
