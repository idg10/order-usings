namespace OrderUsings.Configuration
{
    using System.Collections.Generic;

    /// <summary>
    /// Describes the required order and spacing for using directives.
    /// </summary>
    public class OrderUsingsConfiguration
    {
        /// <summary>
        /// Gets or sets the grouping and spacing rules.
        /// </summary>
        public List<ConfigurationRule> GroupsAndSpaces { get; set; }
    }
}
