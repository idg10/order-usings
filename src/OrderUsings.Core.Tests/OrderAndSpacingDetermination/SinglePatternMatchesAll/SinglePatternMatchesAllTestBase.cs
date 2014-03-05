namespace OrderUsings.Tests.OrderAndSpacingDetermination.SinglePatternMatchesAll
{
    using System.Collections.Generic;

    using OrderUsings.Configuration;

    public abstract class SinglePatternMatchesAllTestBase : OrderAndSpacingDeterminationTestBase
    {
        internal override List<ConfigurationRule> GetRules()
        {
            return new List<ConfigurationRule>
            {
                ConfigurationRule.ForGroupRule(new GroupRule
                {
                    Type = MatchType.ImportOrAlias,
                    NamespacePattern = "*",
                    AliasPattern = "*",
                    Priority = 1,
                    OrderAliasesBy = OrderAliasBy.Alias
                })
            };
        }
    }
}
