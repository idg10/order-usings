namespace OrderUsings.Tests.OrderAndSpacingDetermination.TwoSpecificThenDefaultThenAliases
{
    using NUnit.Framework;

    public class WhenAliasesAndImportsOrderedByAlias : TwoGroupsThenAliasesBase
    {
        [Test]
        public void TwoMatchFirstGroupThirdIsAlias()
        {
            Verify(
                new[]
                {
                    ImportSystem,
                    ImportMicrosoftCSharp,
                    AliasSystemIoPathAsPath
                },
                new[]
                {
                    new[] { ImportSystem, ImportMicrosoftCSharp },
                    new[] { AliasSystemIoPathAsPath }
                });
        }

        [Test]
        public void TwoMatchFirstGroupTwoAreAliasesInOrder()
        {
            Verify(
                new[]
                {
                    ImportSystem,
                    ImportMicrosoftCSharp,
                    AliasSystemIoPathAsPath,
                    AliasSystemAbcAsXyz
                },
                new[]
                {
                    new[] { ImportSystem, ImportMicrosoftCSharp },
                    new[] { AliasSystemIoPathAsPath, AliasSystemAbcAsXyz }
                });
        }


        [Test]
        public void TwoMatchFirstGroupTwoAreAliasesOutOfOrder()
        {
            Verify(
                new[]
                {
                    AliasSystemIoPathAsPath,
                    ImportMicrosoftCSharp,
                    AliasSystemAbcAsXyz,
                    ImportSystem
                },
                new[]
                {
                    new[] { ImportSystem, ImportMicrosoftCSharp },
                    new[] { AliasSystemIoPathAsPath, AliasSystemAbcAsXyz }
                });
        }
    }
}
