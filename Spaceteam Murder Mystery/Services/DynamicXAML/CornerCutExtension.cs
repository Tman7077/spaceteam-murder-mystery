namespace SMM.Services.DynamicXAML;

[MarkupExtensionReturnType(typeof(Geometry))]
public class CornerCutExtension : MarkupExtension
{
    public CornerCutExtension() { }

    public static Binding GetBinding()
    {
        return new Binding()
        {
            Path = new PropertyPath("ActualWidth"),
            RelativeSource = new RelativeSource(RelativeSourceMode.Self),
            Converter = new CornerCutConverter(),
            Mode = BindingMode.OneWay
        };
    }

    public override object ProvideValue(IServiceProvider serviceProvider) =>
        GetBinding().ProvideValue(serviceProvider);

    private class CornerCutConverter : IValueConverter
    {
        private static readonly Geometry _geometry = Geometry.Parse("M0,0 L90,0 L100,10 L100,30 L10,30 L0,20 Z");

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            _geometry;
    
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }
}
