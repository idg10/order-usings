namespace OrderUsings.Processing
{
    using System.Collections.Generic;
    using System.Linq;

    using OrderUsings.Configuration;

    /// <summary>
    /// Contains logic for import inspection that is common to various processing stages.
    /// </summary>
    public static class ImportInspector
    {
        /// <summary>
        /// Takes a configuration and a description of a using directive list, and
        /// produces two things: a list containing just the directives (with the
        /// items representing blank lines removed), and a description of the
        /// correct order and grouping for these items for the given configuration.
        /// </summary>
        /// <param name="configuration">The configuration that will determine the
        /// correct order and grouping.</param>
        /// <param name="items">A list of using directives and the blank lines
        /// interspersed therein. (To simplify processing, this may be null to
        /// represent the absence of any using directives.)</param>
        /// <param name="imports">The 'flattened' list (just the using directives,
        /// with any blank lines stripped out) will be written to this argument,
        /// unless <c>items</c> is null, in which case this will be set to null.</param>
        /// <param name="requiredOrderByGroups">The correct ordering and spacing
        /// for the using directives (as determined by the configuration) will be
        /// written to this argument (unless <c>items</c> is null, in which case
        /// this will be set to null).</param>
        /// <remarks>
        /// The correct order and spacing is represented as a list of lists. Each
        /// nested list represents a group of usings, where each group should be
        /// separated by a blank line.
        /// </remarks>
        public static void FlattenImportsAndDetermineOrderAndSpacing(
            OrderUsingsConfiguration configuration,
            List<UsingDirectiveOrSpace> items,
            out List<UsingDirective> imports,
            out List<List<UsingDirective>> requiredOrderByGroups)
        {
            imports = null;
            requiredOrderByGroups = null;
            if (items != null)
            {
                imports = items
                    .Where(i => !i.IsBlankLine)
                    .Select(i => i.Directive)
                    .ToList();

                requiredOrderByGroups =
                    OrderAndSpacingGenerator.DetermineOrderAndSpacing(imports, configuration);
            }
        }

        /// <summary>
        /// Determines which using directive should be moved where to bring the directive one
        /// step closer to the correct order. This method should be called repeatedly (applying
        /// each update that it generates between each call) until it returns null to indicate
        /// that the order is correct.
        /// </summary>
        /// <param name="requiredOrderByGroups">The correct order (e.g., as determined by
        /// a call to <see cref="FlattenImportsAndDetermineOrderAndSpacing"/>).</param>
        /// <param name="imports">The using directives in their current order.</param>
        /// <returns>Null if the directives are already in the correct order. Otherwise,
        /// a <see cref="Relocation"/> describing which item to move where.</returns>
        /// <remarks>
        /// This method will only ever indicate that directives should be moved backwards.
        /// This means that it will not necessarily produce the optimal sequence of changes.
        /// (E.g., given a target order of 1,2,3,4,5 and a current order of of 5,1,2,3,4,
        /// you would end up calling this method 4 times even though an optimal re-ordering
        /// that allowed items to move forwards would need only one move). It does keep
        /// things simple, though.
        /// </remarks>
        public static Relocation GetNextUsingToMove(
            List<List<UsingDirective>> requiredOrderByGroups,
            List<UsingDirective> imports)
        {
            var requiredOrder =
                from itemGroup in requiredOrderByGroups
                from item in itemGroup
                select item;
            return OrderChecker.GetNextUsingToMove(requiredOrder, imports);
        }

        /// <summary>
        /// Given a set of using directives which are already in the correct order, determines
        /// where to remove or add a blank line to bring them one step closer to the correct
        /// spacing. This method should be called repeatedly (applying each update that it
        /// generates between each call) until it returns null to indicate that the order
        /// is correct.
        /// </summary>
        /// <param name="requiredOrderByGroups">The correct order and spacing (e.g., as
        /// determined by a call to <see cref="FlattenImportsAndDetermineOrderAndSpacing"/>).</param>
        /// <param name="items">The directives as they are currently ordered and spaced.</param>
        /// <returns>Null if the directives are already in the correct order. Otherwise,
        /// a <see cref="SpaceChange"/> describing where to add or remove a blank line.</returns>
        public static SpaceChange GetNextSpacingModification(
            List<List<UsingDirective>> requiredOrderByGroups,
            List<UsingDirectiveOrSpace> items)
        {
            SpaceChange nextChange = null;
            if (requiredOrderByGroups != null)
            {
                var importsByGroup = new List<List<UsingDirective>>();
                foreach (UsingDirectiveOrSpace item in items)
                {
                    if (importsByGroup.Count == 0)
                    {
                        importsByGroup.Add(new List<UsingDirective>());
                    }

                    List<UsingDirective> currentGroup = importsByGroup[importsByGroup.Count - 1];

                    if (item.IsBlankLine)
                    {
                        if (currentGroup.Count > 0)
                        {
                            importsByGroup.Add(new List<UsingDirective>());
                        }
                    }
                    else
                    {
                        currentGroup.Add(item.Directive);
                    }
                }

                nextChange = SpacingChecker.GetNextModification(
                    requiredOrderByGroups,
                    importsByGroup);
            }

            return nextChange;
        }
    }
}
