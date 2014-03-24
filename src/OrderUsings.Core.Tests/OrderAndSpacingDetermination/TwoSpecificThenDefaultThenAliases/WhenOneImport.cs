namespace OrderUsings.Tests.OrderAndSpacingDetermination.TwoSpecificThenDefaultThenAliases
{
    using NUnit.Framework;

    public class WhenOneImport : TwoGroupsThenAliasesBase
    {
        [Test]
        public void FirstRuleMatchProducesOneGroup()
        {
            Verify(new[] { ImportSystem }, new[] { new[] { ImportSystem } });
        }

        [Test]
        public void SecondRuleMatchProducesOneGroup()
        {
            Verify(new[] { ImportMicrosoftCSharp }, new[] { new[] { ImportMicrosoftCSharp } });
        }

        [Test]
        public void NamespaceFallbackRuleMatchProducesOneGroup()
        {
            Verify(new[] { ImportOther }, new[] { new[] { ImportOther } });
        }
    }
}
