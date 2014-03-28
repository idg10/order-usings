namespace Resharper.Tests.CodeCleanup
{
    using NUnit.Framework;

    using JetBrains.Application.Settings;
    using JetBrains.ProjectModel;
    using JetBrains.ProjectModel.DataContext;
    using JetBrains.ReSharper.Feature.Services.CodeCleanup;
    using JetBrains.ReSharper.FeaturesTestFramework.CodeCleanup;
    using JetBrains.ReSharper.Psi;
    using JetBrains.TextControl;

    using OrderUsings.ReSharper.Settings;

    [TestFixture]
    public class WhenOrderIsWrong : CodeCleanupTestBase
    {
        protected override string RelativeTestDataPath
        {
            get { return string.Empty; }
        }

        protected override void DoTest(IProject testProject)
        {
            IPsiServices psiServices = testProject.GetSolution().GetPsiServices();
            IContextBoundSettingsStore boundStore = psiServices.SettingsStore.BindToContextTransient(
                ContextRange.Smart(testProject.ToDataContext()));
            boundStore.SetValue<OrderUsingsSettings, string>(
                settings => settings.OrderSpecificationXml,
                "<Groups xmlns=\"http://schemas.interact-sw.co.uk/OrderUsings/2014\">" +
                "<Group Priority='1' NamespacePattern='System*' />" +
                "<Group Priority='1' NamespacePattern='Microsoft*' />" +
                "<Space />" +
                "<Group Priority='9999' NamespacePattern='*' />" +
                "<Space />" +
                "<Group Priority='9999' NamespacePattern='*' AliasPattern='*' Type='Alias' />" +
                "</Groups>");

            base.DoTest(testProject);
        }

        protected override CodeCleanupProfile GetProfile(
            CodeCleanupSettingsComponent codeCleanupSettings,
            ITextControl textControl)
        {
            var profile = new CodeCleanupProfile(
                false,
                "<Profile name=\"Usings order and spacing\"><FixUsingsOrderAndSpacing>True</FixUsingsOrderAndSpacing></Profile>");
            return profile;
        }

        [Test]
        public void TestCleanup()
        {
            DoTestFiles("cleanup-order-01.cs");
        }
    }
}
