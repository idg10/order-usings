namespace OrderUsings.Tests.OrderAndSpacingDetermination.TwoSpecificAdjacentPatternsWithoutSpaceThenFallback
{
    using NUnit.Framework;

    public class WhenNoUsings : TwoSpecificAdjacentPatternsWithoutSpaceThenFallbackBase
    {
        [Test]
        public void ProducesNoGroups()
        {
            VerifyEmptyHandling();
        }
    }
}
