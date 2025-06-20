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
}
