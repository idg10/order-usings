namespace OrderUsings.Processing
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Checks that using directives have the required spacing.
    /// </summary>
    public static class SpacingChecker
    {
        /// <summary>
        /// Compares the spacing (as represented by grouping) of using directives in
        /// two lists. Returns null if they are the same, and otherwise returns a
        /// description of the first line at which to either remove or insert space
        /// to bring the list closer into line with the required spacing. (This presumes
        /// that the order is already correct.)
        /// </summary>
        /// <param name="requiredGroups">The required order and spacing, where
        /// spacing is denoted by grouping.</param>
        /// <param name="currentGroups">The current order and spacing.</param>
        /// <returns>Null if the spacing matches. Otherwise, a <see cref="SpaceChange"/>
        /// describing where to add or remove a blank line.</returns>
        public static SpaceChange GetNextModification(
            IEnumerable<IEnumerable<UsingDirective>> requiredGroups,
            IEnumerable<IEnumerable<UsingDirective>> currentGroups)
        {
            var requiredByGroup = requiredGroups
                .SelectMany((items, groupIndex) => items.Select(item => groupIndex));
            var currentByGroup = currentGroups
                .SelectMany((items, groupIndex) => items.Select(item => groupIndex));
            var itemsByGroup = requiredByGroup.Zip(currentByGroup, (required, actual) => new { required, actual });

            int index = 0;
            foreach (var groupIndices in itemsByGroup)
            {
                if (groupIndices.actual < groupIndices.required)
                {
                    return SpaceChange.Insert(index);
                }

                if (groupIndices.actual > groupIndices.required)
                {
                    return SpaceChange.Remove(index);
                }

                index += 1;
            }

            return null;
        }
    }
}
