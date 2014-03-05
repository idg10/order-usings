namespace OrderUsings.ReSharper.Highlightings
{
    using JetBrains.ReSharper.Daemon;
    using JetBrains.ReSharper.Psi.CSharp.Tree;

    using OrderUsings.Configuration;

    /// <summary>
    /// A ReSharper highlighting indicating that a set of using statements don't meet the
    /// configured spacing requirements.
    /// </summary>
    [StaticSeverityHighlighting(ViolationSeverity, "Using Directive Order & Spacing", Title = "Using directive spacing")]
    public class UsingSpacingHighlighting : BaseHighlighting
    {
        private const Severity ViolationSeverity = Severity.WARNING;

        /// <summary>
        /// Initializes a <see cref="UsingOrderHighlighting"/>.
        /// </summary>
        /// <param name="typeAndNamespaceHolder">The file or namespace block that contains
        /// the import list being highlighted.</param>
        /// <param name="config">The configuration that was active when we determined that
        /// the import list does not match the requirements.</param>
        internal UsingSpacingHighlighting(
            ICSharpTypeAndNamespaceHolderDeclaration typeAndNamespaceHolder, OrderUsingsConfiguration config)
            : base(typeAndNamespaceHolder, config)
        {
        }

        /// <inheritdoc/>
        protected override string ToolTipText
        {
            get { return "Using directives do not match configured spacing"; }
        }
    }
}
