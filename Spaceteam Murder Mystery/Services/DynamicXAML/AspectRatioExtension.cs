namespace SMM.Services.DynamicXAML;

[MarkupExtensionReturnType(typeof(double))]
public class AspectRatioExtension : MarkupExtension
{
    public double Ratio { get; set; } = 0.33;

    public AspectRatioExtension() { }
    public AspectRatioExtension(double ratio) => Ratio = ratio;

    public static Binding GetBinding(double ratio)
    {
        return new Binding("ActualWidth")
        {
            // bind to this element's own width
            RelativeSource = new RelativeSource(RelativeSourceMode.Self),
            Converter = new AspectRatioConverter(),
            ConverterParameter = ratio,
            Mode = BindingMode.OneWay
        };
    }

    public override object ProvideValue(IServiceProvider serviceProvider) =>
        GetBinding(Ratio).ProvideValue(serviceProvider);
        
    private class AspectRatioConverter : IValueConverter
    {
        public double Ratio { get; set; } = 0.33; // height = 33% of width (by default)

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
