using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using Microsoft.CodeAnalysis.CSharp;

namespace GEnum
{
    [AttributeUsage(AttributeTargets.Enum)]
    public class DisplayingExtensionsAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Field)]
    public class  DisplayNameAttribute : Attribute
    {
        public string Name { get; }
        
        public DisplayNameAttribute(string name)
        {
            Name = name;
        }
    }

    public partial class GEnumGenerator
    {
        private static void EmitDisplayingExtensions(SourceProductionContext context, GeneratorAttributeSyntaxContext source)
        {
            var typeSymbol = (INamedTypeSymbol)source.TargetSymbol;
            var typeName = typeSymbol.Name;

            // 定義名と表示名の組を取得
            var list = new List<(string define, string display)>();
            var noneDefinitions = new List<string>();
            var isFirstDefinition = true;
            foreach (var node in from node in source.TargetNode.ChildNodes()
                                 where node is EnumMemberDeclarationSyntax
                                 select node as EnumMemberDeclarationSyntax)
            {
                var define = node.ChildTokens().First().ValueText;

                var display = (from attributeList in node.AttributeLists
                               from attribute in attributeList.Attributes
                               where attribute.Name.ToString() == "DisplayName"
                               select attribute.ArgumentList?.Arguments.First().Expression.ChildTokens().First().ValueText).FirstOrDefault();

                if (string.IsNullOrWhiteSpace(display))
                {
                    display = define;
                }
                list.Add((define, display));

                // 0の時だけ特別扱いする必要があるのでどうにかして判定する
                var isNone = node.ChildNodes().FirstOrDefault(n => n.IsKind(SyntaxKind.EqualsValueClause))?.ChildNodes().FirstOrDefault(n => n.IsKind(SyntaxKind.NumericLiteralExpression) && n.ToString() == "0") != null;
                if (isNone)
                {
                    noneDefinitions.Add(define);
                }
                else if (isFirstDefinition && !node.ChildNodes().Any(n => n.IsKind(SyntaxKind.EqualsValueClause)))
                {
                    noneDefinitions.Add(define);
                }
                isFirstDefinition = false;
            }

            var code = $$"""
                using System.Text;
                {{GetNameSpaceExpression(typeSymbol)}}

                public static partial class {{typeName}}Extensions
                {
                    public static IReadOnlyDictionary<{{typeName}}, string> DisplayNames { get; } = new Dictionary<{{typeName}}, string>()
                    {
                {{string.Join("\n", list.Select(item => 
                $"        {{ {typeName}.{item.define}, \"{item.display}\" }},"))}}
                    };

                    public static string GetDefineName(this {{typeName}} source)
                    {
                        return source switch
                        {
                {{string.Join("\n", list.Select(item =>
                $"            {typeName}.{item.define} => \"{item.define}\","))}}
                            _ => "Undefined",
                        };
                    }

                    public static string GetDisplayName(this {{typeName}} source)
                    {
                        return source switch
                        {
                {{string.Join("\n", list.Select(item => 
                $"            {typeName}.{item.define} => \"{item.display}\","))}}
                            _ => BuildDisplayName(source),
                        };
                    }

                    private static string BuildDisplayName({{typeName}} source)
                    {
                        if((int)source == 0)
                        {
                            if(DisplayNames.TryGetValue(source, out var display))
                                return display;
                            return "None";
                        }
                        var isFirst = true;
                        var builder = new StringBuilder();
                {{string.Join("\n", list.Where(item => !noneDefinitions.Contains(item.define)).Select(item => $$"""
                        if(source.Contains({{typeName}}.{{item.define}}))
                        {
                            if(!isFirst) builder.Append(" | ");
                            builder.Append("{{item.display}}");
                            isFirst = false;
                        }
                """))}}
                        return isFirst ? "Unknown" : builder.ToString();
                    }
                }
                """;

            var fileName = $"{typeName}Extensions.Displaying.g.cs";
            context.AddSource(fileName, code);
        }
    }
}
