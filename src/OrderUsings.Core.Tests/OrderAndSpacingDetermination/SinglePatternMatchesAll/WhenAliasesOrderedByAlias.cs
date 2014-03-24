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
                    AliasSystemIoPathAsPath,
                    ImportSystem,
                    ImportRuhroh,
                    AliasSystemLaterAsEarlier
                },
                new[]
                {
                    new[]
                    {
                        AliasSystemLaterAsEarlier,
                        AliasSystemIoPathAsPath,
                        ImportRuhroh,
                        ImportSystem
                    }
                });
        }
    }
}
