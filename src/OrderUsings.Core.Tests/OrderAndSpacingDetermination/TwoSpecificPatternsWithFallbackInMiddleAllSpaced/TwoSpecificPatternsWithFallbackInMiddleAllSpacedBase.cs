namespace OrderUsings.Tests.OrderAndSpacingDetermination.TwoSpecificPatternsWithFallbackInMiddleAllSpaced
{
    using System.Collections.Generic;

    using OrderUsings.Configuration;

    public abstract class TwoSpecificPatternsWithFallbackInMiddleAllSpacedBase : OrderAndSpacingDeterminationTestBase
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

                ConfigurationRule.ForSpace(),

                ConfigurationRule.ForGroupRule(new GroupRule
                {
                    Type = MatchType.ImportOrAlias,
                    NamespacePattern = "*",
                    AliasPattern = "*",
                    Priority = 2
                }),

                ConfigurationRule.ForSpace(),

                ConfigurationRule.ForGroupRule(new GroupRule
                {
                    Type = MatchType.Import,
                    NamespacePattern = "MyLocal*",
                    Priority = 1
                })
            };
        }
    }
}
