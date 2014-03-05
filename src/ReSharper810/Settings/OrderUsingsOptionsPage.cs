namespace OrderUsings.ReSharper.Settings
{
    using System;
    using System.IO;
    using System.Windows;
    using System.Windows.Controls;

    using JetBrains.Annotations;
    using JetBrains.Application.Settings;
    using JetBrains.DataFlow;
    using JetBrains.ReSharper.Features.Common.Options;
    using JetBrains.UI.CrossFramework;
    using JetBrains.UI.Options;

    /// <summary>
    /// UI for configuring the order and spacing of usings within ReSharper's
    /// settings dialog.
    /// </summary>
    [OptionsPage(PageId, "Order Usings", null, ParentId = ToolsPage.PID)]
    public class OrderUsingsOptionsPage : IOptionsPage
    {
        private const string PageId = "OrderUsingsOptionsId";

        /// <summary>
        /// Initializes a <see cref="OrderUsingsOptionsPage"/>.
        /// </summary>
        /// <param name="lifetime">Passed by ReSharper. Purpose unclear to me.</param>
        /// <param name="settings">Passed by ReSharper, enabling us to get the
        /// current configuration, and bind controls to the configuration system.</param>
        public OrderUsingsOptionsPage([NotNull] Lifetime lifetime, OptionsSettingsSmartContext settings)
        {
            if (lifetime == null) throw new ArgumentNullException("lifetime");

            Control = InitView(lifetime, settings);
        }

        /// <inheritdoc/>
        public EitherControl Control { get; private set; }

        /// <inheritdoc/>
        public string Id { get { return PageId; } }

        /// <inheritdoc/>
        public bool OnOk()
        {
            return true;
        }

        /// <inheritdoc/>
        public bool ValidatePage()
        {
            return true;
        }

        private EitherControl InitView(Lifetime lifetime, OptionsSettingsSmartContext settings)
        {
            var grid = new Grid { Background = SystemColors.ControlBrush };

            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition());

            var margin = new Thickness(3);
            var label = new Label { Content = "Configuration:", Margin = margin };
            var text = new TextBox { AcceptsReturn = true, Margin = margin };
            settings.SetBinding<OrderUsingsSettings, string>(
                lifetime,
                x => x.OrderSpecificationXml,
                text,
                TextBox.TextProperty);

            Grid.SetRow(text, 1);
            Grid.SetColumnSpan(text, 3);

            var buttonPadding = new Thickness(3);
            var resetButton = new Button
            {
                Content = "Reset",
                Margin = margin,
                Padding = buttonPadding
            };
            resetButton.Click += (s, e) => text.SetCurrentValue(TextBox.TextProperty, string.Empty);
            Grid.SetColumn(resetButton, 1);

            var addBasicButton = new Button
            {
                Content = "Create basic configuration",
                Margin = margin,
                Padding = buttonPadding
            };
            addBasicButton.Click += (s, e) =>
            {
                var asm = typeof(OrderUsingsOptionsPage).Assembly;
                Stream stream = asm.GetManifestResourceStream("OrderUsings.ReSharper.Settings.DefaultConfiguration.xml");
                if (stream != null)
                {
                    using (var reader = new StreamReader(stream))
                    {
                        text.SetCurrentValue(TextBox.TextProperty, reader.ReadToEnd());
                    }
                }
            };
            Grid.SetColumn(addBasicButton, 2);

            grid.Children.Add(label);
            grid.Children.Add(text);
            grid.Children.Add(resetButton);
            grid.Children.Add(addBasicButton);
            return grid;
        }
    }
}
