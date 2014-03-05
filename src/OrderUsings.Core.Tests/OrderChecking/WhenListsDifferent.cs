namespace OrderUsings.Tests.OrderChecking
{
    using NUnit.Framework;

    using OrderUsings.Processing;

    public class WhenListsDifferent
    {
        private static readonly UsingDirective D1 = new UsingDirective { Namespace = "System" };
        private static readonly UsingDirective D2 = new UsingDirective { Namespace = "System.Collections.Generic" };
        private static readonly UsingDirective D3 = new UsingDirective { Namespace = "System.Linq" };

        [Test]
        public void ReportsPositionWhenExpectedFirstItemInSecondPlace()
        {
            var required = new[] { D1, D2, D3 };
            var current = new[] { D2, D1, D3 };
            Relocation result = OrderChecker.GetNextUsingToMove(required, current);
            Assert.AreEqual(1, result.From, "Source");
            Assert.AreEqual(0, result.To, "Destination");
        }

        [Test]
        public void ReportsPositionWhenExpectedFirstItemInThirdPlace()
        {
            var required = new[] { D1, D2, D3 };
            var current = new[] { D2, D3, D1 };
            Relocation result = OrderChecker.GetNextUsingToMove(required, current);
            Assert.AreEqual(2, result.From, "Source");
            Assert.AreEqual(0, result.To, "Destination");
        }

        [Test]
        public void ReportsPositionWhenExpectedSecondItemInWrongPlace()
        {
            var required = new[] { D1, D2, D3 };
            var current = new[] { D1, D3, D2 };
            Relocation result = OrderChecker.GetNextUsingToMove(required, current);
            Assert.AreEqual(2, result.From, "Source");
            Assert.AreEqual(1, result.To, "Destination");
        }
    }
}
