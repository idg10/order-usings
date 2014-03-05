namespace OrderUsings.Tests.OrderAndSpacingDetermination.TwoSpecificAdjacentPatternsWithoutSpaceThenFallback
{
    using System.Collections.Generic;

    using OrderUsings.Configuration;
    using OrderUsings.Tests.OrderAndSpacingDetermination;

    public abstract class TwoSpecificAdjacentPatternsWithoutSpaceThenFallbackBase : OrderAndSpacingDeterminationTestBase
    {
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
                    Type = MatchType.ImportOrAlias,
                    NamespacePattern = "*",
                    AliasPattern = "*",
                    Priority = 2
                })
            };
        }
    }
}
