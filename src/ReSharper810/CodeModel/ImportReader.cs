namespace OrderUsings.ReSharper.CodeModel
{
    using System.Collections.Generic;

    using JetBrains.ReSharper.Psi.CSharp.Parsing;
    using JetBrains.ReSharper.Psi.CSharp.Tree;

    using OrderUsings.Processing;

    /// <summary>
    /// Converts from ReSharper's representation of an import list to our internal representation.
    /// </summary>
    internal static class ImportReader
    {
        /// <summary>
        /// Returns a list of using directives and spacing from an element that can contain
        /// a directive list (i.e., a file, or a namespace block). Returns null if the element
        /// has no using directives.
        /// </summary>
        /// <param name="holder">The file or namespace block.</param>
        /// <returns>Null if no using directives were present; a <see cref="UsingDirectiveOrSpace"/>
        /// list otherwise.</returns>
        internal static List<UsingDirectiveOrSpace> ReadImports(ICSharpTypeAndNamespaceHolderDeclaration holder)
        {
            List<UsingDirectiveOrSpace> items = null;
            foreach (IUsingDirective item in holder.Imports)
            {
                if (items == null)
                {
                    items = new List<UsingDirectiveOrSpace>();
                }

                var alias = item as IUsingAliasDirective;
                items.Add(new UsingDirectiveOrSpace(new UsingDirective
                {
                    Namespace = alias == null ? item.ImportedSymbolName.QualifiedName : alias.Alias.Name,
                    Alias = alias == null ? null : alias.AliasName
                }));

                var syb = item.NextSibling;
                bool first = true;
                for (; syb != null && !(syb is IUsingDirective); syb = syb.NextSibling)
                {
                    if (syb.NodeType == CSharpTokenType.NEW_LINE)
                    {
                        if (first)
                        {
                            first = false;
                        }
                        else
                        {
                            items.Add(new UsingDirectiveOrSpace());
                        }
                    }
                }
            }

            return items;
        }
    }
}
