namespace SMM.Services.DynamicXAML;

[MarkupExtensionReturnType(typeof(double))]
public class FontScaleExtension : MarkupExtension
{
    /// <summary>
    /// A double representing the factor (multiplied by the default) to which to scale the text.
    /// </summary>
    public double Scale { get; set; } = 1.0;

    public FontScaleExtension() { }
    public FontScaleExtension(double scale) => Scale = scale;

    /// <summary>
    /// Returns the binding to apply to the object.
    /// </summary>
    /// <param name="scale">A double representing the factor (multiplied by the default) to which to scale the text.</param>
    /// <returns>A Binding that controls the text's size.</returns>
    public static MultiBinding GetBinding(double scale)
    {
        return new MultiBinding
        {
            Converter          = new FontScaleConverter(),
            ConverterParameter = scale,
            Mode               = BindingMode.OneWay,
            Bindings =
            {
                new Binding("ActualWidth")
                { Source = Application.Current.MainWindow },
                new Binding("ActualHeight")
                { Source = Application.Current.MainWindow }
            }
        };
    }

    /// <summary>
    /// Provides the value for the binding based on the specified service provider.
    /// </summary>
    /// <param name="serviceProvider">Provided by XAML when using the MarkupExtension.</param>
    public override object ProvideValue(IServiceProvider serviceProvider) =>
        GetBinding(Scale).ProvideValue(serviceProvider);
    
    /// <summary>
    /// Converter that calculates the font size based on the window width.
    /// </summary>
    private class FontScaleConverter : IMultiValueConverter
    {
        /// <summary>
        /// The base font size to use for scaling.
        /// </summary>
        public double BaseFontSize { get; set; } = 14.0;

        /// <summary>
        /// The design width of the window, used as a base for scaling.
        /// </summary>
        public double DesignWidth { get; set; } = 854.0;

        /// <summary>
        /// The maximum height proportion of the window that the font size can occupy.
        /// </summary>
        public double MaxHeightProportion { get; set; } = 0.1;

        /// <summary>
        /// Performs the calculation.
        /// </summary>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 2 ||
                values[0] is not double windowWidth ||
                values[1] is not double windowHeight)
            { return BaseFontSize; }

            double multiplier = 1.0;
            if (parameter != null && double.TryParse(parameter.ToString(), out double parsedMultiplier))
                multiplier = parsedMultiplier;

            // Step 1: Scale based on width
            double widthScale     = windowWidth / DesignWidth;
            double scaledFontSize = BaseFontSize * widthScale * multiplier;

            // Step 2: Cap based on height
            double maxFontSize = windowHeight * MaxHeightProportion;
            double final       = Math.Min(scaledFontSize, maxFontSize);
            return final == 0  ? BaseFontSize : final;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
