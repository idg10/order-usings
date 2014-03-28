using System.Reflection;
using System.Runtime.CompilerServices;
using JetBrains.ActionManagement;
using JetBrains.Application.PluginSupport;

[assembly: AssemblyTitle("ReSharper820")]
[assembly: AssemblyDescription("Rules-based ordering for C# using directives")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Ian Griffiths")]
[assembly: AssemblyProduct("Order Usings")]
[assembly: AssemblyCopyright("Copyright © Ian Griffiths, 2014")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

[assembly: AssemblyVersion("1.2.0.0")]
[assembly: AssemblyFileVersion("1.2.0.0")]

[assembly: ActionsXml("ReSharper820.Actions.xml")]

// The following information is displayed by ReSharper in the Plugins dialog
[assembly: PluginTitle("Order Usings")]
[assembly: PluginDescription("Rules-based ordering for C# using directives")]
[assembly: PluginVendor("Ian Griffiths")]

[assembly: InternalsVisibleTo("ReSharper820.Tests")]