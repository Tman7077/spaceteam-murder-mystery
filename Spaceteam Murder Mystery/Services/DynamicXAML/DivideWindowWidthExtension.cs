namespace SMM.Services.DynamicXAML;

[MarkupExtensionReturnType(typeof(double))]
public class DivideWindowWidthExtension : MarkupExtension
{
    /// <summary>
    /// A double representing the number of "columns" into which to divide the window to calculate object width.
    /// </summary>
    public double Divisor { get; set; } = 6.0;

    public DivideWindowWidthExtension() { }
    public DivideWindowWidthExtension(double divisor) => Divisor = divisor;

    /// <summary>
    /// Returns the binding to apply to the object.
    /// </summary>
    /// <param name="divisor">A double representing the number of "columns" into which to divide the window to calculate object width.</param>
    /// <returns>A Binding that controls the object's width.</returns>
    public static Binding GetBinding(double divisor)
    {
        return new Binding("ActualWidth")
        {
            Source             = Application.Current.MainWindow,
            Converter          = new DivideWindowWidthConverter(),
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
    
    /// <summary>
    /// Converter that calculates the width based on the window's width and a divisor.
    /// </summary>
    private class DivideWindowWidthConverter : IValueConverter
    {
        /// <summary>
        /// Performs the calculation.
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not double width) return 0;

            // default divisor
            double divisor = 6;

            // try to parse the ConverterParameter
            if (parameter != null
                && double.TryParse(parameter.ToString(), out var p)
                && p != 0)
            { divisor = p; }

            return width / divisor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}