namespace OrderUsings.Tests.OrderAndSpacingDetermination
{
    using System.Collections.Generic;
    using System.Linq;

    using NUnit.Framework;

    using OrderUsings.Configuration;
    using OrderUsings.Processing;

    public abstract class OrderAndSpacingDeterminationTestBase
    {
        internal static readonly UsingDirective AliasSystemPathAsPath = new UsingDirective { Namespace = "System.IO.Path", Alias = "Path" };
        internal static readonly UsingDirective AliasSystemLaterAsEarlier = new UsingDirective { Namespace = "System.Later", Alias = "Earlier" };
        internal static readonly UsingDirective AliasSystemTextAsSystem = new UsingDirective { Namespace = "System.Text", Alias = "System" };

        internal static readonly UsingDirective ImportSystem = new UsingDirective { Namespace = "System" };
        internal static readonly UsingDirective ImportSystemCollectionsGeneric = new UsingDirective { Namespace = "System.Collections.Generic" };
        internal static readonly UsingDirective ImportSystemLinq = new UsingDirective { Namespace = "System.Linq" };

        internal static readonly UsingDirective ImportMicrosoftCSharp = new UsingDirective { Namespace = "Microsoft.CSharp" };

        internal static readonly UsingDirective ImportOther = new UsingDirective { Namespace = "Other" };
        internal static readonly UsingDirective ImportOtherA = new UsingDirective { Namespace = "Other.A" };
        internal static readonly UsingDirective ImportOtherB = new UsingDirective { Namespace = "Other.B" };

        internal static readonly UsingDirective ImportMyLocal = new UsingDirective { Namespace = "MyLocal" };
        internal static readonly UsingDirective ImportMyLocalA = new UsingDirective { Namespace = "MyLocal.A" };
        internal static readonly UsingDirective ImportMyLocalB = new UsingDirective { Namespace = "MyLocal.B" };

        internal static readonly UsingDirective ImportRuhroh = new UsingDirective { Namespace = "Ruhroh" };


        internal OrderUsingsConfiguration Configuration { get; private set; }

        [SetUp]
        public void BaseInitialize()
        {
            Configuration = new OrderUsingsConfiguration
            {
                GroupsAndSpaces = GetRules()
            };
        }

        internal abstract List<ConfigurationRule> GetRules();

        internal void Verify(UsingDirective[] directivesIn, params UsingDirective[][] expectedGroups)
        {
            foreach (var permutation in AllOrderings(directivesIn))
            {
                var results = OrderAndSpacingGenerator.DetermineOrderAndSpacing(
                    permutation, Configuration);

                Assert.AreEqual(expectedGroups.Length, results.Count);
                for (int i = 0; i < expectedGroups.Length; ++i)
                {
                    Assert.AreEqual(results[i].Count, expectedGroups[i].Length, "Item count in group " + i);
                    for (int j = 0; j < expectedGroups[i].Length; ++j)
                    {
                        UsingDirective expectedUsing = expectedGroups[i][j];
                        UsingDirective actualUsing = results[i][j];
                        string message = string.Format(
                            "Expected {0} at {1},{2}, found {3}", expectedUsing, i, j, actualUsing);
                        Assert.AreSame(expectedUsing, actualUsing, message);
                    }
                }
            }
        }

        // This is the same for all configurations. We only want to run the test
        // once per config, so we don't make this a [Test] in this base class - that
        // would run it once per derived class. Instead, just one derived classes
        // per config will defer to this.
        protected void VerifyEmptyHandling()
        {
            Verify(new UsingDirective[0], new UsingDirective[0][]);
        }

        private static IEnumerable<IEnumerable<T>> AllOrderings<T>(IEnumerable<T> items)
        {
            bool returnedAtLeastOne = false;
            int index = 0;
// ReSharper disable PossibleMultipleEnumeration
            foreach (T item in items)
            {
                returnedAtLeastOne = true;
                int thisIndex = index;
                foreach (var remainders in AllOrderings(items.Where((x, i) => i != thisIndex)))
                {
                    yield return new[] { item }.Concat(remainders);
                }
                index += 1;
            }
            // ReSharper restore PossibleMultipleEnumeration

            if (!returnedAtLeastOne)
            {
                yield return Enumerable.Empty<T>();
            }
        }
    }
}
