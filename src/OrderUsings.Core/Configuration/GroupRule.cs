namespace OrderUsings.Configuration
{
    /// <summary>
    /// Represents a configuration entry describing a group of namespaces.
    /// </summary>
    public class GroupRule
    {
        /// <summary>
        /// Gets or sets a value that determines which group wins when a directive
        /// matches multiple groups' rules.
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// Gets or sets the regular expression against which a using directive's
        /// namespace will be tested, to determine whether it belongs to this group.
        /// </summary>
        /// <remarks>
        /// Only used for rules that match using directives that import namespaces,
        /// i.e. when <see cref="Type"/> is either <see cref="MatchType.Import"/>
        /// or <see cref="MatchType.ImportOrAlias"/>.
        /// </remarks>
        public string NamespacePattern { get; set; }

        /// <summary>
        /// Gets or sets the pattern against which a using directive's
        /// alias will be tested (in the case where the directive defines an
        /// alias) to determine whether it belongs to this group.
        /// </summary>
        /// <remarks>
        /// <para>Use a * for wildcard matching, e.g. <c>System*</c>.</para>
        /// <para>
        /// Only used for rules that match using alias directives i.e. when
        /// <see cref="Type"/> is either <see cref="MatchType.Alias"/>
        /// or <see cref="MatchType.ImportOrAlias"/>.
        /// </para>
        /// </remarks>
        public string AliasPattern { get; set; }

        /// <summary>
        /// Gets or sets a value indicating what types of using directives this
        /// rule matches.
        /// </summary>
        public MatchType Type { get; set; }

        /// <summary>
        /// Gets or sets a value indicating how using alias directives should be ordered
        /// within the group.
        /// </summary>
        public OrderAliasBy OrderAliasesBy { get; set; }
    }
}
