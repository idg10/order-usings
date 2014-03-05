namespace OrderUsings.Tests.OrderChecking
{
    using System.Collections.Generic;

    using NUnit.Framework;

    using OrderUsings.Processing;

    public class WhenListsEmpty
    {
        [Test]
        public void ReturnsNull()
        {
            var empty = new List<UsingDirective>();
            Assert.IsNull(OrderChecker.GetNextUsingToMove(empty, empty));
        }
    }
}
