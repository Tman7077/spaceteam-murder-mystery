namespace SMM.Services.DynamicXAML;

[MarkupExtensionReturnType(typeof(Thickness))]
public class ParentBasedMarginExtension : MarkupExtension
{
    public double Ratio { get; set; } = 4.0;

    public ParentBasedMarginExtension() { }
    public ParentBasedMarginExtension(double ratio) => Ratio = ratio;

    public static Binding GetBinding(double ratio)
    {
        return new Binding("ActualWidth")
        {
            RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(FrameworkElement), 1),
            Converter = new ParentBasedMarginConverter(),
            ConverterParameter = ratio,
            Mode = BindingMode.OneWay
        };
    }
    
    public override object ProvideValue(IServiceProvider serviceProvider) =>
        GetBinding(Ratio).ProvideValue(serviceProvider);

    private class ParentBasedMarginConverter : IValueConverter
    {
        // value: parent.ActualWidth
        // parameter: ratio (double)
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
