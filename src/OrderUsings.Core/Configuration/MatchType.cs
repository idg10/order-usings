namespace OrderUsings.Configuration
{
    /// <summary>
    /// Describes how a <see cref="GroupRule"/> matches a using directive.
    /// </summary>
    public enum MatchType
    {
        /// <summary>
        /// Match only directives that import types from a namespace (and not using alias directives).
        /// </summary>
        Import,

        /// <summary>
        /// Match only using alias directives.
        /// </summary>
        Alias,

        /// <summary>
        /// Match using directives of either kind.
        /// </summary>
        ImportOrAlias
    }
}
