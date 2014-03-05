namespace OrderUsings.Tests.OrderAndSpacingDetermination.TwoSpecificAdjacentPatternsWithoutSpaceThenFallback
{
    using NUnit.Framework;

    public class WhenOneImport : TwoSpecificAdjacentPatternsWithoutSpaceThenFallbackBase
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
        public void FallbackRuleMatchProducesOneGroup()
        {
            Verify(new[] { ImportOther }, new[] { new[] { ImportOther } });
        }
    }
}
