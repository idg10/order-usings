namespace OrderUsings.Tests.OrderAndSpacingDetermination.SpecificThenFallbackThenSpecificThenAliases
{
    using System.Collections.Generic;

    using OrderUsings.Configuration;

    public abstract class SpecificThenFallbackThenSpecificThenAliasesBase : OrderAndSpacingDeterminationTestBase
    {
        protected virtual OrderAliasBy AliasOrder { get { return OrderAliasBy.Alias; } }

        internal override List<ConfigurationRule> GetRules()
        {
            return new List<ConfigurationRule>
            {
                ConfigurationRule.ForGroupRule(new GroupRule
                {
                    Type = MatchType.Import,
                    NamespacePattern = "System*",
                    Priority = 1
                }),
                ConfigurationRule.ForGroupRule(new GroupRule
                {
                    Type = MatchType.Import,
                    NamespacePattern = "Microsoft*",
                    Priority = 1
                }),

                ConfigurationRule.ForSpace(),

                ConfigurationRule.ForGroupRule(new GroupRule
                {
                    Type = MatchType.Import,
                    NamespacePattern = "*",
                    Priority = 9999
                }),

                ConfigurationRule.ForSpace(),

                ConfigurationRule.ForGroupRule(new GroupRule
                {
                    Type = MatchType.Import,
                    NamespacePattern = "*",
                    Priority = 1
                }),

                ConfigurationRule.ForSpace(),

                ConfigurationRule.ForGroupRule(new GroupRule
                {
                    Type = MatchType.Alias,
                    OrderAliasesBy = AliasOrder,
                    AliasPattern = "*",
                    NamespacePattern = "*",
                    Priority = 9999
                })
            };
        }
    }
}
