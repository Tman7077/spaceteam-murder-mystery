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
            RelativeSource = new RelativeSource(
                RelativeSourceMode.FindAncestor,
                typeof(FrameworkElement),
                1),
            Converter = new ParentBasedMarginConverter(),
            ConverterParameter = ratio,
            Mode = BindingMode.OneWay
        };
    }
    public override object ProvideValue(IServiceProvider serviceProvider) =>
        GetBinding(Ratio).ProvideValue(serviceProvider);
}
