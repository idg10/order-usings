namespace OrderUsings.Tests.SpacingChecking
{
    using System.Collections.Generic;

    using NUnit.Framework;

    using OrderUsings.Processing;

    public class WhenListsEmpty
    {
        [Test]
        public void ReturnsNull()
        {
            var empty = new List<List<UsingDirective>>();
            Assert.IsNull(SpacingChecker.GetNextModification(empty, empty));
        }
    }
}
