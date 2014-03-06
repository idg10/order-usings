using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;

using NUnit.Framework;

using JetBrains.Application;
using JetBrains.Threading;

using OrderUsings.Configuration;
using OrderUsings.ReSharper.Settings;

[assembly: AssemblyTitle("OrderUsings.ReSharper810.Tests")]
[assembly: AssemblyDescription("Tests for ReSharper v8.1 Order Usings plug-in")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Ian Griffiths")]
[assembly: AssemblyProduct("Order Usings")]
[assembly: AssemblyCopyright("Copyright © Ian Griffiths, 2014")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]

[assembly: ComVisible(false)]

/// <summary>
/// Test environment. Must be in the global namespace.
/// </summary>
[SetUpFixture]
// ReSharper disable once CheckNamespace
public class TestEnvironmentAssembly : ReSharperTestEnvironmentAssembly
{
    /// <summary>
    /// Gets the assemblies to load into test environment.
    /// Should include all assemblies which contain components.
    /// </summary>
    /// <returns>
    /// The assemblies to load.
    /// </returns>
    private static IEnumerable<Assembly> GetAssembliesToLoad()
    {
        // Test assembly
        yield return Assembly.GetExecutingAssembly();

        yield return typeof(GroupRule).Assembly;
        yield return typeof(OrderUsingsSettings).Assembly;
    }

    public override void SetUp()
    {
        base.SetUp();
        ReentrancyGuard.Current.Execute(
            "LoadAssemblies",
            () => Shell.Instance.GetComponent<AssemblyManager>().LoadAssemblies(
                GetType().Name, GetAssembliesToLoad()));
    }

    public override void TearDown()
    {
        ReentrancyGuard.Current.Execute(
            "UnloadAssemblies",
            () => Shell.Instance.GetComponent<AssemblyManager>().UnloadAssemblies(
                GetType().Name, GetAssembliesToLoad()));
        base.TearDown();
    }
}
