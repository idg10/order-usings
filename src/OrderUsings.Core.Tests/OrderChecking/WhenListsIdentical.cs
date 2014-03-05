namespace OrderUsings.Tests.OrderChecking
{
    using NUnit.Framework;

    using OrderUsings.Processing;

    public class WhenListsIdentical
    {
        [Test]
        public void ReturnsNull()
        {
            var d1 = new UsingDirective { Namespace = "System" };
            var d2 = new UsingDirective { Namespace = "System.Linq" };
            var required = new[] { d1, d2 };
            var current = new[] { d1, d2 };
            Assert.IsNull(OrderChecker.GetNextUsingToMove(required, current));
        }
    }
}
