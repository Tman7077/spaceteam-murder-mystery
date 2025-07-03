namespace SMM.Services.DynamicXAML;

[MarkupExtensionReturnType(typeof(Thickness))]
public class ParentBasedMarginExtension : MarkupExtension
{
    /// <summary>
    /// A double representing the number of "columns" into which to divide the parent's width to calculate object width.
    /// </summary>
    public double Divisor { get; set; } = 4.0;

    public ParentBasedMarginExtension() { }
    public ParentBasedMarginExtension(double divisor) => Divisor = divisor;

    /// <summary>
    /// Returns the binding to apply to the object.
    /// </summary>
    /// <param name="divisor">A double representing the number of "columns" into which to divide the parent's width to calculate object width.</param>
    /// <returns>A Binding that controls the object's width.</returns>
    public static Binding GetBinding(double divisor)
    {
        return new Binding("ActualWidth")
        {
            RelativeSource     = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(FrameworkElement), 1),
            Converter          = new ParentBasedMarginConverter(),
            ConverterParameter = divisor,
            Mode               = BindingMode.OneWay
        };
    }
    
    /// <summary>
    /// Provides the value for the binding based on the specified service provider.
    /// </summary>
    /// <param name="serviceProvider">Provided by XAML when using the MarkupExtension.</param>
    public override object ProvideValue(IServiceProvider serviceProvider) =>
        GetBinding(Divisor).ProvideValue(serviceProvider);

    private class ParentBasedMarginConverter : IValueConverter
    {
        /// <summary>
        /// Performs the calculation.
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double width
                && parameter != null
                && double.TryParse(parameter.ToString(), out var ratio)
                && ratio > 0)
            {
                var margin = width * ratio;
                return new Thickness(margin, 0, margin, 0);
            }

            return new Thickness(0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
