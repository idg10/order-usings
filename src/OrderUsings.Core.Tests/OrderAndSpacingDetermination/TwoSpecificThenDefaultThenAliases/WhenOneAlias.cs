namespace OrderUsings.Tests.OrderAndSpacingDetermination.TwoSpecificThenDefaultThenAliases
{
    using NUnit.Framework;

    public class WhenOneAlias : TwoGroupsThenAliasesBase
    {
        [Test]
        public void AliasFallbackRuleMatchProducesOneGroup()
        {
            Verify(new[] { AliasSystemIoPathAsPath }, new[] { new[] { AliasSystemIoPathAsPath } });
        }
    }
}
