namespace OrderUsings.Tests.OrderAndSpacingDetermination.TwoSpecificThenDefaultThenAliases
{
    using NUnit.Framework;

    public class WhenImportsAndAliasesMatchingAllGroups : TwoGroupsThenAliasesBase
    {
        [Test]
        public void WhenInOrderProducesThreeGroups()
        {
            Verify(
                new[]
                {
                    ImportSystem,
                    ImportSystemCollectionsGeneric,
                    ImportMicrosoftCSharp,

                    ImportOther,

                    AliasSystemIoPathAsPath
                },
                new[]
                {
                    new[] { ImportSystem, ImportSystemCollectionsGeneric, ImportMicrosoftCSharp },
                    new[] { ImportOther },
                    new[] { AliasSystemIoPathAsPath }
                });
        }

        [Test]
        public void WhenOutOfOrderProducesThreeGroups()
        {
            Verify(
                new[]
                {
                    ImportSystem,
                    ImportSystemCollectionsGeneric,
                    ImportOther,
                    AliasSystemIoPathAsPath,
                    ImportMicrosoftCSharp
                },
                new[]
                {
                    new[] { ImportSystem, ImportSystemCollectionsGeneric, ImportMicrosoftCSharp },
                    new[] { ImportOther },
                    new[] { AliasSystemIoPathAsPath }
                });
        }
    }
}
