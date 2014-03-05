namespace OrderUsings.Tests.OrderAndSpacingDetermination.SinglePatternMatchesAll
{
    using NUnit.Framework;

    public class WhenNoUsings : SinglePatternMatchesAllTestBase
    {
        [Test]
        public void ProducesNoGroups()
        {
            VerifyEmptyHandling();
        }
    }
}
