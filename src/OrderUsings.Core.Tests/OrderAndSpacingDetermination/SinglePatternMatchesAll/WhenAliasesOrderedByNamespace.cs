namespace OrderUsings.Tests.OrderAndSpacingDetermination.SinglePatternMatchesAll
{
    using System.Collections.Generic;

    using NUnit.Framework;

    using OrderUsings.Configuration;

    public class WhenAliasesOrderedByNamespace : SinglePatternMatchesAllTestBase
    {
        internal override List<ConfigurationRule> GetRules()
        {
            var r = base.GetRules();
            r[0].Rule.OrderAliasesBy = OrderAliasBy.Namespace;
            return r;
        }

        [Test]
        public void ProducesSingleGroupOrderedByNamespace()
        {
            Verify(
                new[]
                {
                    AliasSystemIoPathAsPath,
                    ImportSystem,
                    ImportRuhroh,
                    AliasSystemLaterAsEarlier
                },
                new[]
                {
                    new[]
                    {
                        ImportRuhroh,
                        ImportSystem,
                        AliasSystemIoPathAsPath,
                        AliasSystemLaterAsEarlier
                    }
                });
        }

        // This class introduces a change in config, so we should verify that
        // empty input handling still works.
        [Test]
        public void EmptyUsingsProducesNoGroups()
        {
            VerifyEmptyHandling();
        }
    }
}
