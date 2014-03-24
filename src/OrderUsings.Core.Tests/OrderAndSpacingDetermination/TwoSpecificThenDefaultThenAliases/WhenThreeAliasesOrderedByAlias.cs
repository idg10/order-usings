namespace OrderUsings.Tests.OrderAndSpacingDetermination.TwoSpecificThenDefaultThenAliases
{
    using NUnit.Framework;

    public class WhenThreeAliasesOrderedByAlias : TwoGroupsThenAliasesBase
    {
        [Test]
        public void AllMatchAliasFallbackInOrderProducesOneGroup()
        {
            Verify(
                new[]
                {
                    AliasSystemLaterAsEarlier,
                    AliasSystemIoPathAsPath,
                    AliasSystemTextAsSystem
                },
                new[]
                {
                    new[] { AliasSystemLaterAsEarlier, AliasSystemIoPathAsPath, AliasSystemTextAsSystem }
                });
        }

        [Test]
        public void AllMatchAliasFallbackOutOfOrderProducesOneGroup()
        {
            Verify(
                new[]
                {
                    AliasSystemTextAsSystem,
                    AliasSystemIoPathAsPath,
                    AliasSystemLaterAsEarlier
                },
                new[]
                {
                    new[] { AliasSystemLaterAsEarlier, AliasSystemIoPathAsPath, AliasSystemTextAsSystem }
                });
        }
    }
}
