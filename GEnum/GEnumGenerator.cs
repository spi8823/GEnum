using Microsoft.CodeAnalysis;
using System.Text;

namespace GEnum
{
    [Generator(LanguageNames.CSharp)]
    public partial class GEnumGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var flagsExtensionsSource = context.SyntaxProvider.ForAttributeWithMetadataName($"GEnum.{nameof(FlagsExtensionsAttribute)}", (_, _) => true, (context, _) => context);
            context.RegisterSourceOutput(flagsExtensionsSource, EmitFlagsExtensions);

            var matchingExtensionsSource = context.SyntaxProvider.ForAttributeWithMetadataName($"GEnum.{nameof(MatchingExtensionsAttribute)}", (_, _) => true, (context, _) => context);
            context.RegisterSourceOutput(matchingExtensionsSource, EmitMatchingExtensions);
        }

        private static string GetNameSpaceExpression(INamedTypeSymbol typeSymbol) =>
            typeSymbol.ContainingNamespace.IsGlobalNamespace ? string.Empty : $"using {typeSymbol.ContainingNamespace};";

        private static void EmitMatchingExtensions(SourceProductionContext context, GeneratorAttributeSyntaxContext source)
        {
            var typeSymbol = (INamedTypeSymbol)source.TargetSymbol;
            var typeName = typeSymbol.Name;
            var valueNames = typeSymbol.MemberNames;
            var methodBuilder = new StringBuilder();
            foreach(var valueName in valueNames)
            {
                methodBuilder.AppendLine($$"""
                        public static bool Is{{valueName}}(this {{typeName}} target) => target == {{typeName}}.{{valueName}};
                    """);
            }

            var code = $$"""
                {{GetNameSpaceExpression(typeSymbol)}}

                public static partial class {{typeName}}Extensions
                {
                {{methodBuilder.ToString()}}
                }
                """;

            var fileName = $"{typeName}Extensions.Matching.g.cs";
            context.AddSource(fileName, code);
        }

        private static void EmitFlagsExtensions(SourceProductionContext context, GeneratorAttributeSyntaxContext source)
        {
            var typeSymbol = (INamedTypeSymbol)source.TargetSymbol;
            var typeName = typeSymbol.Name;

            var code = $$"""
                {{GetNameSpaceExpression(typeSymbol)}}

                public static partial class {{typeName}}Extensions
                {
                    public static bool Contains(this {{typeName}} target, {{typeName}} value)
                    {
                        return (target & value) == value;
                    }

                    public static void Add(ref this {{typeName}} target, {{typeName}} value)
                    {
                        target |= value;
                    }

                    public static void Remove(ref this {{typeName}} target, {{typeName}} value)
                    {
                        target &= ~value;
                    }

                    public static void Clear(ref this {{typeName}} target)
                    {
                        target = ({{typeName}})0;
                    }
                }
                """;

            var fileName = $"{typeName}Extensions.Flags.g.cs";
            context.AddSource(fileName, code);
        }
    }

    [AttributeUsage(AttributeTargets.Enum)]
    public class FlagsExtensionsAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Enum)]
    public class MatchingExtensionsAttribute : Attribute { }
}
