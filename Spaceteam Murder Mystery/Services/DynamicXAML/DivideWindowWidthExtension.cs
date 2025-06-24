namespace SMM.Services.DynamicXAML;

[MarkupExtensionReturnType(typeof(double))]
public class DivideWindowWidthExtension : MarkupExtension
{
    public double Divisor { get; set; } = 6.0;

    public DivideWindowWidthExtension() { }
    public DivideWindowWidthExtension(double divisor) => Divisor = divisor;

    public static Binding GetBinding(double divisor)
    {
        return new Binding("ActualWidth")
        {
            Source = Application.Current.MainWindow,
            Converter = new DivideWindowWidthConverter(),
            ConverterParameter = divisor,
            Mode = BindingMode.OneWay
        };
    }

    public override object ProvideValue(IServiceProvider serviceProvider) =>
        GetBinding(Divisor).ProvideValue(serviceProvider);
    
    private class DivideWindowWidthConverter : IValueConverter
    {
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