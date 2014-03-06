namespace OrderUsings.ReSharper
{
    using System.Collections.Generic;

    using JetBrains.Application;
    using JetBrains.ReSharper.Psi.CSharp.Parsing;
    using JetBrains.ReSharper.Psi.CSharp.Tree;
    using JetBrains.ReSharper.Psi.ExtensionsAPI;
    using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
    using JetBrains.Text;

    using OrderUsings.Configuration;
    using OrderUsings.Processing;
    using OrderUsings.ReSharper.CodeModel;

    /// <summary>
    /// Order and spacing fix logic shared by quick fixes and cleanup module.
    /// </summary>
    internal static class Fixes
    {
        /// <summary>
        /// Fixes the order of the using directives in a given file or namespace block
        /// to match the specified configuration.
        /// </summary>
        /// <param name="holder">The file or namespace block in which to fix the order
        /// of using directives (if any are present).</param>
        /// <param name="configuration">The configuration determining the correct order.</param>
        public static void FixOrder(
            ICSharpTypeAndNamespaceHolderDeclaration holder,
            OrderUsingsConfiguration configuration)
        {
            // The reordering proceeds one item at a time, so we just keep reapplying it
            // until there's nothing left to do.
            // To avoid hanging VS in the event that an error in the logic causes the
            // sequence of modifications not to terminate, we ensure we don't try to
            // apply more changes than there are using directives.
            int tries = 0;
            int itemCount = 0;
            while (tries == 0 || tries <= itemCount)
            {
                List<UsingDirectiveOrSpace> items = ImportReader.ReadImports(holder);
                List<UsingDirective> imports;
                List<List<UsingDirective>> requiredOrderByGroups;
                ImportInspector.FlattenImportsAndDetermineOrderAndSpacing(
                    configuration, items, out imports, out requiredOrderByGroups);

                if (requiredOrderByGroups == null)
                {
                    break;
                }

                itemCount = imports.Count;
                Relocation nextChange = ImportInspector.GetNextUsingToMove(requiredOrderByGroups, imports);
                if (nextChange != null)
                {
                    IUsingDirective toMove = holder.Imports[nextChange.From];
                    IUsingDirective before = holder.Imports[nextChange.To];
                    holder.RemoveImport(toMove);
                    holder.AddImportBefore(toMove, before);
                    tries += 1;
                }
                else
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Fixes the spacing of the using directives in a given file or namespace block
        /// to match the specified configuration. (The directives must already be in
        /// the correct order.)
        /// </summary>
        /// <param name="holder">The file or namespace block in which to fix the spacing
        /// of using directives (if any are present).</param>
        /// <param name="configuration">The configuration determining the correct spacing.</param>
        public static void FixSpacing(
            ICSharpTypeAndNamespaceHolderDeclaration holder,
            OrderUsingsConfiguration configuration)
        {
            // The reordering proceeds one item at a time, so we just keep reapplying it
            // until there's nothing left to do.
            // To avoid hanging VS in the event that an error in the logic causes the
            // sequence of modifications not to terminate, we ensure we don't try to
            // apply more changes than there are either using directives or blank
            // lines in the usings list.
            int tries = 0;
            int itemCount = 0;
            while (tries == 0 || tries <= itemCount)
            {
                List<UsingDirectiveOrSpace> items = ImportReader.ReadImports(holder);
                if (items == null)
                {
                    return;
                }

                itemCount = items.Count;
                List<UsingDirective> imports;
                List<List<UsingDirective>> requiredOrderByGroups;
                ImportInspector.FlattenImportsAndDetermineOrderAndSpacing(
                    configuration, items, out imports, out requiredOrderByGroups);

                SpaceChange nextChange = ImportInspector.GetNextSpacingModification(requiredOrderByGroups, items);
                if (nextChange != null)
                {
                    IUsingDirective usingBeforeSpace = holder.Imports[nextChange.Index - 1];
                    if (nextChange.ShouldInsert)
                    {
                        using (WriteLockCookie.Create())
                        {
                            var newLineText = new StringBuffer("\r\n");

                            LeafElementBase newLine = TreeElementFactory.CreateLeafElement(
                                CSharpTokenType.NEW_LINE, newLineText, 0, newLineText.Length);
                            LowLevelModificationUtil.AddChildAfter(usingBeforeSpace, newLine);
                        }
                    }
                    else
                    {
                        var syb = usingBeforeSpace.NextSibling;
                        for (; syb != null && !(syb is IUsingDirective); syb = syb.NextSibling)
                        {
                            if (syb.NodeType == CSharpTokenType.NEW_LINE)
                            {
                                LowLevelModificationUtil.DeleteChild(syb);
                            }
                        }
                    }
                }
                else
                {
                    break;
                }

                tries += 1;
            }
        }
    }
}
