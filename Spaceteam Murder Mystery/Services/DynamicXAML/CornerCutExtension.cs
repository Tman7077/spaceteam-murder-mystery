namespace SMM.Services.DynamicXAML;

[MarkupExtensionReturnType(typeof(Geometry))]
public class CornerCutExtension : MarkupExtension
{
    public CornerCutExtension() { }

    /// <summary>
    /// Returns the binding to apply to the object.
    /// </summary>
    /// <returns>A Binding that controls the object's shape.</returns>
    public static Binding GetBinding()
    {
        return new Binding()
        {
            Path           = new PropertyPath("ActualWidth"),
            RelativeSource = new RelativeSource(RelativeSourceMode.Self),
            Converter      = new CornerCutConverter(),
            Mode           = BindingMode.OneWay
        };
    }

    /// <summary>
    /// Provides the value for the binding based on the specified service provider.
    /// </summary>
    /// <param name="serviceProvider">Provided by XAML when using the MarkupExtension.</param>
    public override object ProvideValue(IServiceProvider serviceProvider) =>
        GetBinding().ProvideValue(serviceProvider);

    /// <summary>
    /// Converter that calculates the actual width and height based on the predefined gerometry.
    /// </summary>
    private class CornerCutConverter : IValueConverter
    {
        /// <summary>
        /// Predefined geometry for the corner cut shape.
        /// The top-right and botttom-left corners are "cut" (angled).
        /// </summary>
        private static readonly Geometry _geometry = Geometry.Parse("M0,0 L90,0 L100,10 L100,30 L10,30 L0,20 Z");

        /// <summary>
        /// Performs the calculation.
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            _geometry;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }
}
