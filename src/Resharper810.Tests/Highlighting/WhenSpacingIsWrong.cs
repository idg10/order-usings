namespace Resharper810.Tests.Highlighting
{
    using System;

    using NUnit.Framework;

    using JetBrains.Application.Settings;
    using JetBrains.ProjectModel;
    using JetBrains.ReSharper.Daemon;
    using JetBrains.ReSharper.Daemon.CSharp;
    using JetBrains.ReSharper.TestFramework;

    using OrderUsings.ReSharper;
    using OrderUsings.ReSharper.Highlightings;
    using OrderUsings.ReSharper.Settings;

    [TestFixture]
    [TestSettingsKey(typeof(OrderUsingsSettings))]
    public class WhenSpacingIsWrong : CSharpHighlightingTestBase
    {
        protected override bool HighlightingPredicate(IHighlighting highlighting, IContextBoundSettingsStore settingsstore)
        {
            return highlighting is UsingSpacingHighlighting;
        }

        // This seems to be the earliest place from which we can get a settings store. We use
        // this to push in settings without having to use a real settings file. (It looks like
        // you can actually provide a test-local settings file, but for now, just providing it
        // programmatically is easiest.)
        protected override void WithProject(IProject project, ISettingsStore settingsStore, Action action)
        {
            // The docs all say to use plain BindToContext, but that has been marked as [Obsolete].
            // This appears to be what that obsolete method actually does. (And teh DataContexts.Empty
            // just copies what the test code uses when it creates a bound settings store to pass to the
            // code under test.)
            IContextBoundSettingsStore boundStore = settingsStore.BindToContextTransient(
                ContextRange.ManuallyRestrictWritesToOneContext(
                (lifetime, contexts) => settingsStore.DataContexts.Empty));
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
            base.WithProject(project, settingsStore, action);
        }

        protected override string RelativeTestDataPath
        {
            get { return @""; }
        }

        [Test]
        [TestCase("highlighting-spacing-01.cs")]
        public void Test(string testName)
        {
            DoTestFiles(testName);
        }
    }
}
