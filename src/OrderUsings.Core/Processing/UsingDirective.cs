namespace OrderUsings.Processing
{
    /// <summary>
    /// A non-technology-specific representation of a using directive.
    /// </summary>
    /// <remarks>
    /// We use this so that the core logic doesn't need to depend on any particular
    /// framework. (This decouples us from any single version of ReSharper, or even
    /// ReSharper at all, since I'd like to offer a StyleCop plug-in at some point.)
    /// </remarks>
    public class UsingDirective
    {
        /// <summary>
        /// Gets or sets the alias name created by this declaration, or <c>null</c>
        /// if this is an import.
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// Gets or sets either the namespace that this declaration imports, or
        /// (if <see cref="Alias"/> is non-null) the type or namespace for which
        /// this defines an alias.
        /// </summary>
        public string Namespace { get; set; }

        /// <inheritdoc/>
        public override string ToString()
        {
            return Alias == null ? Namespace : Alias + " = " + Namespace;
        }
    }
}
