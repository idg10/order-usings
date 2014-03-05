namespace OrderUsings.Tests.SpacingChecking
{
    using NUnit.Framework;

    using OrderUsings.Processing;

    public class WhenUnwantedSpacePresent
    {
        private static readonly UsingDirective D1 = new UsingDirective { Namespace = "System" };
        private static readonly UsingDirective D2 = new UsingDirective { Namespace = "Moq" };
        private static readonly UsingDirective D3 = new UsingDirective { Namespace = "MyProject" };

        [Test]
        public void ReportsPositionWhenOnlyOneWasExpected()
        {
            var required = new[] { new[] { D1, D2 }, new[] { D3 } };
            var current = new[] { new[] { D1 }, new[] { D2 }, new[] { D3 } };
            SpaceChange result = SpacingChecker.GetNextModification(required, current);
            Assert.IsFalse(result.ShouldInsert, "ShouldInsert");
            Assert.AreEqual(1, result.Index);
        }

        [Test]
        public void ReportsPositionWhenNoSpaceExpected()
        {
            var required = new[] { new[] { D1, D3, D2 } };
            var current = new[] { new[] { D1, D2 }, new[] { D3 } };
            SpaceChange result = SpacingChecker.GetNextModification(required, current);
            Assert.IsFalse(result.ShouldInsert, "ShouldInsert");
            Assert.AreEqual(2, result.Index);
        }
    }
}
