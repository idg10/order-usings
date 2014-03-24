namespace OrderUsings.Tests.OrderAndSpacingDetermination.TwoSpecificThenDefaultThenAliases
{
    using NUnit.Framework;

    public class WhenNoUsings : TwoGroupsThenAliasesBase
    {
        [Test]
        public void ProducesNoGroups()
        {
            VerifyEmptyHandling();
        }
    }
}
