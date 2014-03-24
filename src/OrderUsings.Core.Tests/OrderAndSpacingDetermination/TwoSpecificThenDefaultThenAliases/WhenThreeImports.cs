namespace OrderUsings.Tests.OrderAndSpacingDetermination.TwoSpecificThenDefaultThenAliases
{
    using NUnit.Framework;

    public class WhenThreeImports : TwoGroupsThenAliasesBase
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
        public void AllMatchNamespaceFallbackProducesOneGroup()
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
        public void TwoMatchFirstOneMatchesFallbackProducesTwoGroups()
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
    }
}
