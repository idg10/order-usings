namespace OrderUsings.ReSharper.Highlightings
{
    using JetBrains.ReSharper.Daemon;
    using JetBrains.ReSharper.Psi.CSharp.Tree;

    using OrderUsings.Configuration;

    /// <summary>
    /// Common functionality for any highlighting for a using directive list.
    /// </summary>
    public abstract class BaseHighlighting : IHighlighting
    {
        private readonly ICSharpTypeAndNamespaceHolderDeclaration _typeAndNamespaceHolder;
        private readonly OrderUsingsConfiguration _config;

        /// <summary>
        /// Initializes a <see cref="BaseHighlighting"/>.
        /// </summary>
        /// <param name="typeAndNamespaceHolder">The file or namespace block that contains
        /// the import list being highlighted.</param>
        /// <param name="config">The configuration that was active when we determined that
        /// the import list does not match the requirements.</param>
        internal BaseHighlighting(
            ICSharpTypeAndNamespaceHolderDeclaration typeAndNamespaceHolder, OrderUsingsConfiguration config)
        {
            _config = config;
            _typeAndNamespaceHolder = typeAndNamespaceHolder;
        }

        /// <summary>
        /// Gets file or namespace block that contains the import list being highlighted.
        /// </summary>
        public ICSharpTypeAndNamespaceHolderDeclaration TypeAndNamespaceHolder
        {
            get { return _typeAndNamespaceHolder; }
        }

        /// <inheritdoc/>
        public string ToolTip
        {
            get { return ToolTipText; }
        }

        /// <inheritdoc/>
        public string ErrorStripeToolTip
        {
            get { return ToolTipText; }
        }

        /// <inheritdoc/>
        public int NavigationOffsetPatch
        {
            get { return 0; }
        }

        /// <summary>
        /// Gets configuration that was active when we determined that the import list
        /// does not match the requirements.
        /// </summary>
        internal OrderUsingsConfiguration Config
        {
            get { return _config; }
        }

        /// <summary>
        /// Gets the text used as the main tooltip and the error stripe tooltip.
        /// </summary>
        protected abstract string ToolTipText { get; }

        /// <inheritdoc/>
        public bool IsValid()
        {
            return true;
        }
    }
}
