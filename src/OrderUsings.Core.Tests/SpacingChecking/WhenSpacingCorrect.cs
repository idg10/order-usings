namespace OrderUsings.Tests.SpacingChecking
{
    using NUnit.Framework;

    using OrderUsings.Processing;

    public class WhenSpacingCorrect
    {
        private static readonly UsingDirective D1 = new UsingDirective { Namespace = "System" };
        private static readonly UsingDirective D2 = new UsingDirective { Namespace = "Moq" };
        private static readonly UsingDirective D3 = new UsingDirective { Namespace = "MyProject" };

        [Test]
        public void WithTwoGroups()
        {
            var required = new[] { new[] { D1, D2 }, new[] { D3 } };
            var current = new[] { new[] { D1, D2 }, new[] { D3 } };
            Assert.IsNull(SpacingChecker.GetNextModification(required, current));
        }

        [Test]
        public void WithOneGroup()
        {
            var required = new[] { new[] { D1, D3, D2 } };
            var current = new[] { new[] { D1, D2, D3 } };
            Assert.IsNull(SpacingChecker.GetNextModification(required, current));
        }
    }
}
