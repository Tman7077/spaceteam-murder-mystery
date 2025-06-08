namespace SMM.Services.DynamicXAML;

[MarkupExtensionReturnType(typeof(double))]
public class FontScaleExtension : MarkupExtension
{
    public double Scale { get; set; } = 1.0;

    public FontScaleExtension() { }

    public FontScaleExtension(double scale)
    { Scale = scale; }

    public static MultiBinding GetBinding(double scale)
    {
        return new MultiBinding
        {
            Converter = new FontScaleConverter(),
            ConverterParameter = scale,
            Mode = BindingMode.OneWay,
            Bindings =
            {
                new Binding("ActualWidth")
                { RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(Window), 1) },
                new Binding("ActualHeight")
                { RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(Window), 1) }
            }
        };
    }
    public override object ProvideValue(IServiceProvider serviceProvider) =>
        GetBinding(Scale).ProvideValue(serviceProvider);
}
