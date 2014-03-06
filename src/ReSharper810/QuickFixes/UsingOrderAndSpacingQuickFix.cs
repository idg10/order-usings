namespace OrderUsings.ReSharper.QuickFixes
{
    using System;

    using JetBrains.Application;
    using JetBrains.Application.Progress;
    using JetBrains.DocumentModel.Transactions;
    using JetBrains.ProjectModel;
    using JetBrains.ReSharper.Feature.Services.Bulbs;
    using JetBrains.ReSharper.Intentions.Extensibility;
    using JetBrains.ReSharper.Psi;
    using JetBrains.ReSharper.Psi.CSharp.Tree;
    using JetBrains.TextControl;
    using JetBrains.Util;

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

                            Fixes.FixOrder(holder, _highlighting.Config);
                            Fixes.FixSpacing(holder, _highlighting.Config);
                        }));
                }
            };
        }
    }
}
