namespace OrderUsings.ReSharper.QuickFixes
{
    using System;
    using System.Collections.Generic;

    using JetBrains.Application;
    using JetBrains.Application.Progress;
    using JetBrains.DocumentModel.Transactions;
    using JetBrains.ProjectModel;
    using JetBrains.ReSharper.Feature.Services.Bulbs;
    using JetBrains.ReSharper.Intentions.Extensibility;
    using JetBrains.ReSharper.Psi;
    using JetBrains.ReSharper.Psi.CSharp.Parsing;
    using JetBrains.ReSharper.Psi.CSharp.Tree;
    using JetBrains.ReSharper.Psi.ExtensionsAPI;
    using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
    using JetBrains.Text;
    using JetBrains.TextControl;
    using JetBrains.Util;

    using OrderUsings.Processing;
    using OrderUsings.ReSharper.CodeModel;
    using OrderUsings.ReSharper.Highlightings;
    using OrderUsings.ReSharper.Inspection;

    /// <summary>
    /// ReSharper quick fix that fixes the ordering and spacing issues detected during inspection by
    /// <see cref="OrderUsingsDaemonStageProcess"/>.
    /// </summary>
    /// <remarks>
    /// ReSharper constructs this automatically - when it wants to know if any quick fixes are
    /// available for a highlighting, it goes looking for classes annotated with
    /// <see cref="QuickFixAttribute"/> that implement <see cref="IQuickFix"/> (directly or, as in
    /// this case, indirectly), and which have a constructor accepting the relevant highlighting type.
    /// </remarks>
    [QuickFix]
    public class UsingOrderAndSpacingQuickFix : QuickFixBase
    {
        private readonly BaseHighlighting _highlighting;

        /// <summary>
        /// Initialises a <see cref="UsingOrderAndSpacingQuickFix"/> for an order
        /// mismatch highlighting.
        /// </summary>
        /// <param name="highlighting">The highlighting for which this will be
        /// a quick fix.</param>
        public UsingOrderAndSpacingQuickFix(UsingOrderHighlighting highlighting)
        {
            _highlighting = highlighting;
        }

        /// <summary>
        /// Initialises a <see cref="UsingOrderAndSpacingQuickFix"/> for a spacing
        /// mismatch highlighting.
        /// </summary>
        /// <param name="highlighting">The highlighting for which this will be
        /// a quick fix.</param>
        public UsingOrderAndSpacingQuickFix(UsingSpacingHighlighting highlighting)
        {
            _highlighting = highlighting;
        }

        /// <inheritdoc/>
        public override string Text
        {
            get { return "Fix ordering and spacing"; }
        }

        /// <inheritdoc/>
        public override bool IsAvailable(IUserDataHolder cache)
        {
            return true;
        }

        /// <inheritdoc/>
        protected override Action<ITextControl> ExecutePsiTransaction(ISolution solution, IProgressIndicator progress)
        {
            return textControl =>
            {
                using (solution.GetComponent<DocumentTransactionManager>()
                    .CreateTransactionCookie(DefaultAction.Commit, "action name"))
                {
                    var services = solution.GetPsiServices();
                    services.Transactions.Execute(
                        "Code cleanup",
                        () => services.Locks.ExecuteWithWriteLock(() =>
                        {
                            ICSharpTypeAndNamespaceHolderDeclaration holder = _highlighting.TypeAndNamespaceHolder;

                            FixOrder(holder);
                            FixSpacing(holder);
                        }));
                }
            };
        }

        private void FixOrder(ICSharpTypeAndNamespaceHolderDeclaration holder)
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
                    _highlighting.Config, items, out imports, out requiredOrderByGroups);

                if (requiredOrderByGroups != null)
                {
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
        }

        private void FixSpacing(ICSharpTypeAndNamespaceHolderDeclaration holder)
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
                itemCount = items.Count;
                List<UsingDirective> imports;
                List<List<UsingDirective>> requiredOrderByGroups;
                ImportInspector.FlattenImportsAndDetermineOrderAndSpacing(
                    _highlighting.Config, items, out imports, out requiredOrderByGroups);

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
