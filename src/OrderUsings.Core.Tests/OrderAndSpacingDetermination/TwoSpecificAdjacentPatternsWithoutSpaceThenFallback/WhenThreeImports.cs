namespace OrderUsings.Tests.OrderAndSpacingDetermination.TwoSpecificAdjacentPatternsWithoutSpaceThenFallback
{
    using NUnit.Framework;

    public class WhenThreeImports : TwoSpecificAdjacentPatternsWithoutSpaceThenFallbackBase
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
        public void AllMatchingSecondProducesOneGroup()
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
        public void AllMatchingFirstAndSecondInOrderProducesOneGroup()
        {
            Verify(
                new[]
                {
                    ImportSystem,
                    ImportSystemCollectionsGeneric,
                    ImportSystemLinq,
                    ImportMicrosoftCSharp
                },
                new[]
                {
                    new[] { ImportSystem, ImportSystemCollectionsGeneric, ImportSystemLinq, ImportMicrosoftCSharp }
                });
        }

        [Test]
        public void AllMatchingFirstAndSecondOutOfOrderProducesOneGroup()
        {
            Verify(
                new[]
                {
                    ImportSystem,
                    ImportMicrosoftCSharp,
                    ImportSystemCollectionsGeneric,
                    ImportSystemLinq
                },
                new[]
                {
                    new[] { ImportSystem, ImportSystemCollectionsGeneric, ImportSystemLinq, ImportMicrosoftCSharp }
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
    }
}
