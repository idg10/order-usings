namespace OrderUsings.ReSharper.Highlightings
{
    using JetBrains.ReSharper.Daemon;
    using JetBrains.ReSharper.Psi.CSharp.Tree;

    using OrderUsings.Configuration;

    /// <summary>
    /// A ReSharper highlighting indicating that a set of using directives don't meet the
    /// configured ordering requirements.
    /// </summary>
    [StaticSeverityHighlighting(Severity.WARNING, "Using Directive Order & Spacing", Title = "Using directive order")]
    public class UsingOrderHighlighting : BaseHighlighting
    {
        /// <summary>
        /// Initializes a <see cref="UsingOrderHighlighting"/>.
        /// </summary>
        /// <param name="typeAndNamespaceHolder">The file or namespace block that contains
        /// the import list being highlighted.</param>
        /// <param name="config">The configuration that was active when we determined that
        /// the import list does not match the requirements.</param>
        internal UsingOrderHighlighting(
            ICSharpTypeAndNamespaceHolderDeclaration typeAndNamespaceHolder, OrderUsingsConfiguration config)
            : base(typeAndNamespaceHolder, config)
        {
        }

        /// <inheritdoc/>
        protected override string ToolTipText
        {
            get { return "Using directives do not match configured order"; }
        }
    }
}
