namespace OrderUsings.ReSharper.CodeCleanup
{
    using System.ComponentModel;

    using JetBrains.ReSharper.Feature.Services.CodeCleanup;

    /// <summary>
    /// Represents the setting for cleanup of order and spacing for usings.
    /// </summary>
    [DefaultValue(false)]
    [DisplayName("Fix the order and spacing of using directives")]
    [Category(CSharpCategory)]
    internal class OrderAndSpaceCleanupDescriptor : CodeCleanupBoolOptionDescriptor
    {
        /// <summary>
        /// Initializes a (se<see cref="OrderAndSpaceCleanupDescriptor"/>.
        /// </summary>
        public OrderAndSpaceCleanupDescriptor() : base("FixUsingsOrderAndSpacing")
        {
        }
    }
}
