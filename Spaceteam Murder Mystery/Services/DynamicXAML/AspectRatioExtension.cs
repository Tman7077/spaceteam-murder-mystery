namespace SMM.Services.DynamicXAML;

/// <summary>
/// Enforces a specific aspect ratio for a control based on its width.
/// </summary>
[MarkupExtensionReturnType(typeof(double))]
public class AspectRatioExtension : MarkupExtension
{
    /// <summary>
    /// Gets or sets the aspect ratio to apply.
    /// Defaults to height = width / 3.
    /// </summary>
    public double Ratio { get; set; } = 0.33;

    public AspectRatioExtension() { }
    public AspectRatioExtension(double ratio) => Ratio = ratio;

    /// <summary>
    /// Returns the binding to apply to the object.
    /// </summary>
    /// <param name="ratio">A double from 0 to 1 representing the percentage of the width that the height of the object should be.</param>
    /// <returns>A Binding that controls the object's height.</returns>
    public static Binding GetBinding(double ratio)
    {
        return new Binding("ActualWidth")
        {
            RelativeSource     = new RelativeSource(RelativeSourceMode.Self),
            Converter          = new AspectRatioConverter(),
            ConverterParameter = ratio,
            Mode               = BindingMode.OneWay
        };
    }

    /// <summary>
    /// Provides the value for the binding based on the specified service provider.
    /// </summary>
    /// <param name="serviceProvider">Provided by XAML when using the MarkupExtension.</param>
    public override object ProvideValue(IServiceProvider serviceProvider) =>
        GetBinding(Ratio).ProvideValue(serviceProvider);
    
    /// <summary>
    /// Converter that calculates the height based on the width and the specified aspect ratio.
    /// </summary>
    private class AspectRatioConverter : IValueConverter
    {
        /// <summary>
        /// Performs the calculation.
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double width && double.TryParse(parameter?.ToString(), out double ratio))
            { return width * ratio; }
            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }
}
