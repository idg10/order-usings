namespace OrderUsings.Tests.OrderAndSpacingDetermination.TwoSpecificPatternsWithFallbackInMiddleAllSpaced
{
    using NUnit.Framework;

    public class WhenThreeImports : TwoSpecificPatternsWithFallbackInMiddleAllSpacedBase
    {
        [Test]
        public void AllMatchingFirstProducesOneGroup()
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
                    new[] { ImportSystem, ImportSystemCollectionsGeneric, ImportSystemLinq }
                });
        }

        [Test]
        public void AllMatchFallbackProducesOneGroup()
        {
            Verify(
                new[]
                {
                    ImportOther,
                    ImportOtherA,
                    ImportOtherB
                },
                new[]
                {
                    new[] { ImportOther, ImportOtherA, ImportOtherB }
                });
        }

        [Test]
        public void AllMatchLastProducesOneGroup()
        {
            Verify(
                new[]
                {
                    ImportMyLocal,
                    ImportMyLocalA,
                    ImportMyLocalB
                },
                new[]
                {
                    new[] { ImportMyLocal, ImportMyLocalA, ImportMyLocalB }
                });
        }

        [Test]
        public void MatchingFirstAndFallbackProducesTwoGroups()
        {
            Verify(
                new[]
                {
                    ImportSystem,
                    ImportSystemLinq,
                    ImportOther
                },
                new[]
                {
                    new[] { ImportSystem, ImportSystemLinq },
                    new[] { ImportOther }
                });
        }

        [Test]
        public void MatchingFirstAndLastProducesTwoGroups()
        {
            Verify(
                new[]
                {
                    ImportSystem,
                    ImportMyLocal,
                    ImportMyLocalA
                },
                new[]
                {
                    new[] { ImportSystem },
                    new[] { ImportMyLocal, ImportMyLocalA }
                });
        }

        [Test]
        public void MatchingFallbackAndLastProducesTwoGroups()
        {
            Verify(
                new[]
                {
                    ImportOther,
                    ImportMyLocal,
                    ImportMyLocalA
                },
                new[]
                {
                    new[] { ImportOther },
                    new[] { ImportMyLocal, ImportMyLocalA }
                });
        }

        [Test]
        public void MatchingFirstFallbackAndLastProducesThreeGroups()
        {
            Verify(
                new[]
                {
                    ImportSystem,
                    ImportOther,
                    ImportMyLocal
                },
                new[]
                {
                    new[] { ImportSystem },
                    new[] { ImportOther },
                    new[] { ImportMyLocal }
                });
        }
    }
}
