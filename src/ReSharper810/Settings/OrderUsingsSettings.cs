namespace OrderUsings.ReSharper.Settings
{
    using System.Reflection;

    using JetBrains.Application.Settings;

    /// <summary>
    /// Plug-in settings persisted for us by ReSharper.
    /// </summary>
    /// <remarks>
    /// TODO: what are we supposed to use as the 'parent' in SettingsKey?
    /// The docs basically give you no help at all here - there doesn't seem to be a
    /// list of suitable types. The example uses InternetSettings with no indication as
    /// to why you might choose that. I'm going with Missing for now, because some
    /// examples already out there do that too, but it just means you end up as an
    /// uncategorised top-level setting, which is not ideal.
    /// </remarks>
    [SettingsKey(typeof(Missing), "GitHub settings")]
    public class OrderUsingsSettings
    {
        /// <summary>
        /// Gets or sets the XML content specifying the required order.
        /// </summary>
        [SettingsEntry("", "XML file specifying the required order for using directives")]
        public string OrderSpecificationXml { get; set; }
    }
}
