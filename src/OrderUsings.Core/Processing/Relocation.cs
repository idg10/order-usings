namespace OrderUsings.Processing
{
    using System;

    /// <summary>
    /// Describes how a using directive should be repositioned to bring a list
    /// of usings one step closer to the configured order.
    /// </summary>
    public class Relocation
    {
        /// <summary>
        /// Initializes a <see cref="Relocation"/>.
        /// </summary>
        /// <param name="from">The index of the item to be moved.</param>
        /// <param name="to">The index to which to move the item. Must be lower than <c>from</c>.</param>
        public Relocation(int from, int to)
        {
            if (from <= to)
            {
                throw new ArgumentException(
                    "Items must move towards front of list, so to must be lower than from", "to");
            }

            From = from;
            To = to;
        }

        /// <summary>
        /// Gets the index of the item to move. This will always be higher than <see cref="To"/>.
        /// </summary>
        public int From { get; private set; }

        /// <summary>
        /// Gets the index to which the item should be moved. This will always be lower than
        /// <see cref="From"/>.
        /// </summary>
        public int To { get; private set; }
    }
}
