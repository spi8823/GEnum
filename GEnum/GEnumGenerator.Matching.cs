using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace GEnum
{

    [AttributeUsage(AttributeTargets.Enum)]
    public class MatchingExtensionsAttribute : Attribute { }
    public partial class GEnumGenerator
    {
        private static void EmitMatchingExtensions(SourceProductionContext context, GeneratorAttributeSyntaxContext source)
        {
            var typeSymbol = (INamedTypeSymbol)source.TargetSymbol;
            var typeName = typeSymbol.Name;
            var valueNames = typeSymbol.MemberNames;
            var methodBuilder = new StringBuilder();
            foreach (var valueName in valueNames)
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
    }
}
