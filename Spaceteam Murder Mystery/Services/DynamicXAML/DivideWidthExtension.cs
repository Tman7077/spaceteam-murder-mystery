namespace SMM.Services.DynamicXAML;

[MarkupExtensionReturnType(typeof(double))]
public class DivideWidthExtension : MarkupExtension
{
    public double Divisor { get; set; } = 6.0;

    public DivideWidthExtension() { }
    public DivideWidthExtension(double divisor) => Divisor = divisor;

    public static Binding GetBinding(double divisor)
    {
        return new Binding("ActualWidth")
        {
            RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(Window), 1),
            Converter = new DivideWidthConverter(),
            ConverterParameter = divisor,
            Mode = BindingMode.OneWay
        };
    }

    public override object ProvideValue(IServiceProvider serviceProvider) =>
        GetBinding(Divisor).ProvideValue(serviceProvider);
    
    private class DivideWidthConverter : IValueConverter
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