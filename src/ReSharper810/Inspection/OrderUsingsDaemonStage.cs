namespace OrderUsings.ReSharper.Inspection
{
    using System;
    using System.IO;

    using JetBrains.Application.Progress;
    using JetBrains.Application.Settings;
    using JetBrains.ReSharper.Daemon;
    using JetBrains.ReSharper.Daemon.CSharp.Stages;
    using JetBrains.ReSharper.Psi.CSharp.Tree;

    using OrderUsings.Configuration;
    using OrderUsings.ReSharper.Settings;

    /// <summary>
    /// ReSharper entry point, enabling us to inspect source files in the background
    /// as they are opened, and add highlights.
    /// </summary>
    [DaemonStage]
    public class OrderUsingsDaemonStage : CSharpDaemonStageBase
    {
        /// <summary>
        /// Invoked by ReSharper each time it wants us to perform some background processing
        /// of a file.
        /// </summary>
        /// <param name="process">Provides information about and services relating to the
        /// work we are being asked to do.</param>
        /// <param name="settings">Settings information.</param>
        /// <param name="processKind">The kind of processing we're being asked to do.</param>
        /// <param name="file">The file to be processed.</param>
        /// <returns>A process object representing the work, or null if no work will be done.</returns>
        protected override IDaemonStageProcess CreateProcess(
            IDaemonProcess process,
            IContextBoundSettingsStore settings,
            DaemonProcessKind processKind,
            ICSharpFile file)
        {
            if (process == null)
            {
                throw new ArgumentNullException("process");
            }

            // StyleCop's daemon stage looks for a processKind of DaemonProcessKind.OTHER
            // and does nothing (returns null) if it sees it. This turns out to prevent
            // highlights from showing up when you ask ReSharper to inspect code issues
            // across the whole solution. I'm not sure why StyleCop deliberately opts out
            // of it. Perhaps something goes horribly wrong, but I've not seen any sign
            // of that yet, and we really do want solution-wide inspection to work.

            try
            {
                // I guess the base class checks that this is actually a C# file?
                if (!IsSupported(process.SourceFile))
                {
                    return null;
                }

                // StyleCop checks to see if there are already any errors in the file, and if
                // there are, it decides to do nothing.
                // TODO: Do we need to do that?

                // TODO: We should probably check for exemptions, e.g. generated source files.
            }
            catch (ProcessCancelledException)
            {
                return null;
            }

            // TODO: should we get an injected ISettingsOptimization?
            var orderUsingSettings =
                settings.GetKey<OrderUsingsSettings>(SettingsOptimization.DoMeSlowly);

            OrderUsingsConfiguration config = null;
            if (!string.IsNullOrWhiteSpace(orderUsingSettings.OrderSpecificationXml))
            {
                config = ConfigurationSerializer.FromXml(new StringReader(orderUsingSettings.OrderSpecificationXml));
            }

            return new OrderUsingsDaemonStageProcess(process, file, config);
        }
    }
}
