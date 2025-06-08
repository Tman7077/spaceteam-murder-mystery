namespace SMM.Services.DynamicXAML;

[MarkupExtensionReturnType(typeof(double))]
public class DivideWidthExtension : MarkupExtension
{
    public double Divisor { get; set; } = 6.0;

    public DivideWidthExtension() { }

    public DivideWidthExtension(double divisor)
    { Divisor = divisor; }

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
}