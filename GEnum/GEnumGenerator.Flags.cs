using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace GEnum
{
    [AttributeUsage(AttributeTargets.Enum)]
    public class FlagsExtensionsAttribute : Attribute { }

    public partial class GEnumGenerator
    {
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
}
