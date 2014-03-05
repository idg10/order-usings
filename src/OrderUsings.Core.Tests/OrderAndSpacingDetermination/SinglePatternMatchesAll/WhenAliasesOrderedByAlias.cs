namespace OrderUsings.Tests.OrderAndSpacingDetermination.SinglePatternMatchesAll
{
    using NUnit.Framework;

    public class WhenAliasesOrderedByAlias : SinglePatternMatchesAllTestBase
    {
        [Test]
        public void ProducesSingleGroupOrderedByAliasWhenAliasesAndNamespaceOtherwise()
        {
            Verify(
                new[]
                {
                    AliasSystemPathAsPath,
                    ImportSystem,
                    ImportRuhroh,
                    AliasSystemLaterAsEarlier
                },
                new[]
                {
                    new[]
                    {
                        AliasSystemLaterAsEarlier,
                        AliasSystemPathAsPath,
                        ImportRuhroh,
                        ImportSystem
                    }
                });
        }
    }
}
