namespace OrderUsings.Tests.OrderAndSpacingDetermination.TwoSpecificPatternsWithFallbackInMiddleAllSpaced
{
    using NUnit.Framework;

    public class WhenNoUsings : TwoSpecificPatternsWithFallbackInMiddleAllSpacedBase
    {
        [Test]
        public void ProducesNoGroups()
        {
            VerifyEmptyHandling();
        }
    }
}
