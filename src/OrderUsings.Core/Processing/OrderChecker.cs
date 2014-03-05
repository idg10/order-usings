namespace OrderUsings.Processing
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Checks that the order of using directives matches the required order.
    /// </summary>
    public static class OrderChecker
    {
        /// <summary>
        /// Compares the order of using directives in two lists. Returns null if they are
        /// the same, and otherwise returns a description of the first element to move
        /// to bring the current order closer into line with the required order.
        /// </summary>
        /// <param name="requiredOrder">The order in which the directives should appear.</param>
        /// <param name="currentOrder">The order in which the directives currently appear.</param>
        /// <returns>Null if the orders match. Otherwise, a <see cref="Relocation"/> describing
        /// the first element to move to bring the order closer to the required one.</returns>
        /// <remarks>
        /// Code that simply needs to know whether the order is correct (e.g., when we want to
        /// highlight bad ordering) will just use this to get a yes/no answer. Code that wants
        /// to fix the order will call this repeatedly to generate a sequence of moves.
        /// </remarks>
        public static Relocation GetNextUsingToMove(
            IEnumerable<UsingDirective> requiredOrder, IEnumerable<UsingDirective> currentOrder)
        {
            int expectedIndex = 0;
            using (var reqIt = requiredOrder.GetEnumerator())
            using (var currentIt = currentOrder.GetEnumerator())
            {
                while (reqIt.MoveNext() && currentIt.MoveNext())
                {
                    UsingDirective expected = reqIt.Current;
                    if (!ReferenceEquals(expected, currentIt.Current))
                    {
                        int currentIndex = expectedIndex;
                        while (currentIt.MoveNext())
                        {
                            currentIndex += 1;
                            if (ReferenceEquals(expected, currentIt.Current))
                            {
                                return new Relocation(currentIndex, expectedIndex);
                            }
                        }

                        throw new ArgumentException(
                            "Lists should contain same items, but currentOrder was missing " + expected,
                            "currentOrder");
                    }

                    expectedIndex += 1;
                }
            }

            return null;
        }
    }
}
