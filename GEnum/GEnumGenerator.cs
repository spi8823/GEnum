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
    }
}
