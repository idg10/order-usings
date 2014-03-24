namespace OrderUsings.Tests.OrderAndSpacingDetermination.SpecificThenFallbackThenSpecificThenAliases
{
    using NUnit.Framework;

    public class WhenSpecificAndAliasGroupsPopulated : SpecificThenFallbackThenSpecificThenAliasesBase
    {
        // This was the particular failure case that I encountered in a real life issue:
        // https://github.com/idg10/order-usings/issues/1
        [Test]
        public void ProducesThreeGroupsWhenInOrder()
        {
            Verify(
                new[]
                {
                    ImportSystem,
                    ImportSystemData,
                    ImportSystemRuntimeInteropServices,
                    ImportSystemWindowsForms,

                    ImportMmCorePpt,

                    AliasMicrosoftOfficeInteropPowerPointAsPowerPoint
                },
                new[]
                {
                    new[] { ImportSystem, ImportSystemData, ImportSystemRuntimeInteropServices, ImportSystemWindowsForms },
                    new[] { ImportMmCorePpt },
                    new[] { AliasMicrosoftOfficeInteropPowerPointAsPowerPoint }
                });
        }

        [Test]
        public void ProducesThreeGroupsWhenOutOfOrder()
        {
            Verify(
                new[]
                {
                    ImportSystem,
                    ImportSystemRuntimeInteropServices,
                    AliasMicrosoftOfficeInteropPowerPointAsPowerPoint,
                    ImportSystemWindowsForms,
                    ImportSystemData,
                    ImportMmCorePpt
                },
                new[]
                {
                    new[] { ImportSystem, ImportSystemData, ImportSystemRuntimeInteropServices, ImportSystemWindowsForms },
                    new[] { ImportMmCorePpt },
                    new[] { AliasMicrosoftOfficeInteropPowerPointAsPowerPoint }
                });
        }
    }
}
