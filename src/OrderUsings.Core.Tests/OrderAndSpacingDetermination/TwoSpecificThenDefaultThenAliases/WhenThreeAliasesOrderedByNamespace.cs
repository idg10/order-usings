namespace OrderUsings.Tests.OrderAndSpacingDetermination.TwoSpecificThenDefaultThenAliases
{
    using NUnit.Framework;

    using OrderUsings.Configuration;

    public class WhenThreeAliasesOrderedByNamespace : TwoGroupsThenAliasesBase
    {
        protected override OrderAliasBy AliasOrder { get { return OrderAliasBy.Namespace; } }

        [Test]
        public void AllMatchAliasFallbackInOrderProducesOneGroup()
        {
            Verify(
                new[]
                {
                    AliasSystemIoPathAsPath,
                    AliasSystemLaterAsEarlier,
                    AliasSystemTextAsSystem
                },
                new[]
                {
                    new[] { AliasSystemIoPathAsPath, AliasSystemLaterAsEarlier, AliasSystemTextAsSystem }
                });
        }

        [Test]
        public void AllMatchAliasFallbackOutOfOrderProducesOneGroup()
        {
            Verify(
                new[]
                {
                    AliasSystemTextAsSystem,
                    AliasSystemLaterAsEarlier,
                    AliasSystemIoPathAsPath
                },
                new[]
                {
                    new[] { AliasSystemIoPathAsPath, AliasSystemLaterAsEarlier, AliasSystemTextAsSystem }
                });
        }
    }
}
