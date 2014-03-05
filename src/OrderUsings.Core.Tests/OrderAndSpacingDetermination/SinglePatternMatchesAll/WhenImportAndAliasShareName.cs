namespace OrderUsings.Tests.OrderAndSpacingDetermination.SinglePatternMatchesAll
{
    using NUnit.Framework;

    public class WhenImportAndAliasShareName : SinglePatternMatchesAllTestBase
    {
        [Test]
        public void GroupItemsShouldPutNonAliasFirst()
        {
            // Bizarre though it seems, this:
            //  using System;
            //  using System = System.Text;
            // is legal. If a group orders using alias directives by Alias (which is the default)
            // we need to pick one to go first. We put the non-alias one first (i.e., the order
            // shown above), since this seems most consistent with the behaviour of ordering
            // usings lexographically within a group.
            Verify(
                new[]
                {
                    AliasSystemTextAsSystem,
                    ImportSystem
                },
                new[]
                {
                    new[]
                    {
                        ImportSystem,
                        AliasSystemTextAsSystem
                    }
                });
        }
    }
}
