namespace OrderUsings.Tests.OrderAndSpacingDetermination.SinglePatternMatchesAll
{
    using NUnit.Framework;

    public class WhenOneImport : SinglePatternMatchesAllTestBase
    {
        [Test]
        public void ProducesOneGroup()
        {
            Verify(new[] { ImportSystem }, new[] { new[] { ImportSystem } });
        }
    }
}
