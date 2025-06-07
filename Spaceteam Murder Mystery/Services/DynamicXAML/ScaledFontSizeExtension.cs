namespace SMM.Services.DynamicXAML;

[MarkupExtensionReturnType(typeof(double))]
public class ScaledFontSizeExtension : MarkupExtension
{
    public double Scale { get; set; } = 1.0;

    public ScaledFontSizeExtension() { }

    public ScaledFontSizeExtension(double scale)
    { Scale = scale; }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        var binding = new MultiBinding
        {
            Converter = new ScaledFontSizeConverter(),
            ConverterParameter = Scale,
            Mode = BindingMode.OneWay
        };

        binding.Bindings.Add(new Binding("ActualWidth")
        { RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(Window), 1) });

        binding.Bindings.Add(new Binding("ActualHeight")
        { RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(Window), 1) });

        return binding.ProvideValue(serviceProvider);
    }
}
