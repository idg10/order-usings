namespace OrderUsings.Processing
{
    /// <summary>
    /// Describes how to adjust the spacing in a list of using directives to bring them
    /// one step closer to the configured spacing.
    /// </summary>
    public class SpaceChange
    {
        /// <summary>
        /// Gets a value indicating whether the list should be changed by adding or
        /// removing a space.
        /// </summary>
        public bool ShouldInsert { get; private set; }

        /// <summary>
        /// Gets the index at which to add or remove a space.
        /// </summary>
        public int Index { get; private set; }

        /// <summary>
        /// Creates a <see cref="SpaceChange"/> indicating that space should be inserted at
        /// a particular index.
        /// </summary>
        /// <param name="index">The index at which space should be inserted.</param>
        /// <returns>A <see cref="SpaceChange"/>.</returns>
        public static SpaceChange Insert(int index)
        {
            return new SpaceChange { ShouldInsert = true, Index = index };
        }

        /// <summary>
        /// Creates a <see cref="SpaceChange"/> indicating that space should be removed at
        /// a particular index.
        /// </summary>
        /// <param name="index">The index at which space should be removed.</param>
        /// <returns>A <see cref="SpaceChange"/>.</returns>
        public static SpaceChange Remove(int index)
        {
            return new SpaceChange { ShouldInsert = false, Index = index };
        }
    }
}
