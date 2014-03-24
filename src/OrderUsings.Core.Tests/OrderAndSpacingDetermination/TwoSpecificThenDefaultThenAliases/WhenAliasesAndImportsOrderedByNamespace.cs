namespace OrderUsings.Tests.OrderAndSpacingDetermination.TwoSpecificThenDefaultThenAliases
{
    using NUnit.Framework;

    using OrderUsings.Configuration;

    public class WhenAliasesAndImportsOrderedByNamespace : TwoGroupsThenAliasesBase
    {
        protected override OrderAliasBy AliasOrder { get { return OrderAliasBy.Namespace; } }

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
                    new[] { AliasSystemAbcAsXyz, AliasSystemIoPathAsPath }
                });
        }


        [Test]
        public void TwoMatchFirstGroupTwoAreAliasesOutOfOrder()
        {
            Verify(
                new[]
                {
                    AliasSystemAbcAsXyz,
                    ImportMicrosoftCSharp,
                    AliasSystemIoPathAsPath,
                    ImportSystem
                },
                new[]
                {
                    new[] { ImportSystem, ImportMicrosoftCSharp },
                    new[] { AliasSystemAbcAsXyz, AliasSystemIoPathAsPath }
                    });
        }
    }
}
