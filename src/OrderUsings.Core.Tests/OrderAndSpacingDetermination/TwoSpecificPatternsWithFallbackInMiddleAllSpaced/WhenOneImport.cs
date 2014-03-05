namespace OrderUsings.Tests.OrderAndSpacingDetermination.TwoSpecificPatternsWithFallbackInMiddleAllSpaced
{
    using NUnit.Framework;

    public class WhenOneImport : TwoSpecificPatternsWithFallbackInMiddleAllSpacedBase
    {
        [Test]
        public void FirstRuleMatchProducesOneGroup()
        {
            Verify(new[] { ImportSystem }, new[] { new[] { ImportSystem } });
        }

        [Test]
        public void FallbackRuleMatchProducesOneGroup()
        {
            Verify(new[] { ImportOther }, new[] { new[] { ImportOther } });
        }

        [Test]
        public void LastRuleMatchProducesOneGroup()
        {
            Verify(new[] { ImportMyLocal }, new[] { new[] { ImportMyLocal } });
        }
    }
}
