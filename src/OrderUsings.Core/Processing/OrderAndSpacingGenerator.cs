namespace OrderUsings.Processing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    using OrderUsings.Configuration;

    /// <summary>
    /// Generates the correct order and spacing for a set of using directives
    /// given the rules in a particular configuration.
    /// </summary>
    public static class OrderAndSpacingGenerator
    {
        private static readonly UsingComparer CompareByAlias = new UsingComparer(OrderAliasBy.Alias);
        private static readonly UsingComparer CompareByNamespace = new UsingComparer(OrderAliasBy.Namespace);

        /// <summary>
        /// Calculates how using directives should be ordered and, where appropriate,
        /// split into groups separated by blank lines.
        /// </summary>
        /// <param name="directives">The directives for which to determine the order
        /// and spacing.</param>
        /// <param name="configuration">The configuration settings describing the
        /// required ordering and spacing.</param>
        /// <returns>A list of lists. This will be empty if the input list was empty.
        /// Otherwise there will be at least one list; if the rules require that any
        /// of the directives be separated by spaces this will be indicated by
        /// returning multiple lists - a blank line should appear between each of
        /// the lists. Within each of the nested lists returned, the directives are
        /// in the order they should appear.</returns>
        public static List<List<UsingDirective>> DetermineOrderAndSpacing(
            IEnumerable<UsingDirective> directives, OrderUsingsConfiguration configuration)
        {
            if (directives == null)
            {
                throw new ArgumentNullException("directives");
            }

            Dictionary<GroupRule, Predicate<UsingDirective>> groupNamespaceMatchers = configuration.GroupsAndSpaces
                .Where(gs => !gs.IsSpace)
                .ToDictionary<ConfigurationRule, GroupRule, Predicate<UsingDirective>>(
                    gs => gs.Rule,
                    gs =>
                    {
                        Regex nsRegex = null;
                        Regex aliasRegex = null;
                        MatchType matchType = gs.Rule.Type;
                        switch (matchType)
                        {
                            case MatchType.Import:
                                nsRegex = RegexForPattern(gs.Rule.NamespacePattern);
                                break;

                            case MatchType.Alias:
                                aliasRegex = RegexForPattern(gs.Rule.AliasPattern);
                                break;

                            case MatchType.ImportOrAlias:
                                nsRegex = RegexForPattern(gs.Rule.NamespacePattern);
                                aliasRegex = RegexForPattern(gs.Rule.AliasPattern);
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }

                        return directive =>
                        {
                            bool hasAlias = directive.Alias != null;
                            if (matchType == MatchType.Alias && !hasAlias)
                            {
                                return false;
                            }

                            if (matchType == MatchType.Import && hasAlias)
                            {
                                return false;
                            }

                            return
                                (nsRegex == null || nsRegex.IsMatch(directive.Namespace)) &&
                                (aliasRegex == null || !hasAlias || aliasRegex.IsMatch(directive.Alias));
                        };
                    });

            ILookup<GroupRule, UsingDirective> directivesByGroup = directives.ToLookup(
                d => groupNamespaceMatchers
                    .Where(e => e.Value(d))
                    .OrderBy(e => e.Key.Priority)
                    .First().Key);

            List<UsingDirective> currentItemSet = null;
            var results = new List<List<UsingDirective>>();
            GroupRule lastRule = null;
            foreach (ConfigurationRule ruleEntry in configuration.GroupsAndSpaces)
            {
                if (ruleEntry.IsSpace)
                {
                    MakeGroupFromCurrentItemsIfAny(ref currentItemSet, lastRule, results);
                }
                else
                {
                    UsingComparer comparer = ruleEntry.Rule.OrderAliasesBy == OrderAliasBy.Alias ?
                        CompareByAlias : CompareByNamespace;
                    if (currentItemSet == null)
                    {
                        currentItemSet = new List<UsingDirective>();
                    }

                    currentItemSet.AddRange(directivesByGroup[ruleEntry.Rule].OrderBy(d => d, comparer));
                    lastRule = ruleEntry.Rule;
                }
            }

            MakeGroupFromCurrentItemsIfAny(ref currentItemSet, lastRule, results);

            return results;
        }

        private static Regex RegexForPattern(string pattern)
        {
            return new Regex(pattern.Replace(".", "\\.").Replace("*", ".*"));
        }

        private static void MakeGroupFromCurrentItemsIfAny(
            ref List<UsingDirective> currentItemSet, GroupRule lastRule, List<List<UsingDirective>> results)
        {
            if (currentItemSet != null && currentItemSet.Count > 0 && lastRule != null)
            {
                results.Add(currentItemSet);
                currentItemSet = null;
            }
        }

        /// <summary>
        /// Determines the order in which directives should appear within a group.
        /// </summary>
        private class UsingComparer : IComparer<UsingDirective>
        {
            private readonly OrderAliasBy _orderType;

            public UsingComparer(OrderAliasBy orderType)
            {
                _orderType = orderType;
            }

            public int Compare(UsingDirective x, UsingDirective y)
            {
                string left, right;
                if (_orderType == OrderAliasBy.Alias)
                {
                    left = x.Alias ?? x.Namespace;
                    right = y.Alias ?? y.Namespace;
                }
                else
                {
                    left = x.Namespace;
                    right = y.Namespace;
                }

                int result = string.Compare(left, right, StringComparison.Ordinal);
                if (result == 0)
                {
                    // In general a match means that one is an alias, and the other is an import, e.g.
                    // using System;
                    // using System = Foo.Bar;
                    //
                    // or if we're sorting by Namespace,
                    // using System;
                    // using Bar = System;
                    //
                    // In either case, we want the one that's not an alias to go first.
                    if (!ReferenceEquals(x.Alias, y.Alias))
                    {
                        result = x.Alias == null ? -1 : 1;
                    }
                }

                return result;
            }
        }
    }
}
