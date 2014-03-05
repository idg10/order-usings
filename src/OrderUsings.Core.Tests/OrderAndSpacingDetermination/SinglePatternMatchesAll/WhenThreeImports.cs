namespace OrderUsings.Tests.OrderAndSpacingDetermination.SinglePatternMatchesAll
{
    using NUnit.Framework;

    public class WhenThreeImports : SinglePatternMatchesAllTestBase
    {
        [Test]
        public void ProducesOneGroupInAlphabeticalOrder()
        {
            Verify(
                new[]
                {
                    ImportSystem,
                    ImportSystemCollectionsGeneric,
                    ImportSystemLinq
                },
                new[]
                {
                    new[]
                    {
                        ImportSystem,
                        ImportSystemCollectionsGeneric,
                        ImportSystemLinq
                    }
                });
        }
    }
}
