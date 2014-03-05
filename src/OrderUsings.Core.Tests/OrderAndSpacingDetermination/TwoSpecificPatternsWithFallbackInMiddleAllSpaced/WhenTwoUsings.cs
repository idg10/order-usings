namespace OrderUsings.Tests.OrderAndSpacingDetermination.TwoSpecificPatternsWithFallbackInMiddleAllSpaced
{
    using NUnit.Framework;

    public class WhenTwoUsings : TwoSpecificPatternsWithFallbackInMiddleAllSpacedBase
    {
        [Test]
        public void UsingsMatchFirstAndLastProduceTwoGroups()
        {
            Verify(
                new[]
                {
                    ImportSystem,
                    ImportMyLocal
                },
                new[]
                {
                    new[] { ImportSystem },
                    new[] { ImportMyLocal }
                });
        }

        [Test]
        public void UsingsMatchingFirstRuleProduceOneGroup()
        {
            Verify(
                new[]
                {
                    ImportSystem,
                    ImportSystemLinq
                },
                new[]
                {
                    new[] { ImportSystem, ImportSystemLinq }
                });
        }

        [Test]
        public void UsingsMatchFallbackRuleProduceOneGroup()
        {
            Verify(
                new[]
                {
                    ImportOtherA,
                    ImportOtherB
                },
                new[]
                {
                    new[] { ImportOtherA, ImportOtherB }
                });
        }

        [Test]
        public void UsingsMatchingLastRuleProduceOneGroup()
        {
            Verify(
                new[]
                {
                    ImportMyLocalA,
                    ImportMyLocalB
                },
                new[]
                {
                    new[] { ImportMyLocalA, ImportMyLocalB }
                });
        }
    }
}
