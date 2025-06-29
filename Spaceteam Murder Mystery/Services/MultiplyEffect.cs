namespace SMM.Services;

using System.Windows.Media.Effects;

public class MultiplyEffect : ShaderEffect
{
    private static readonly PixelShader _shader = new()
    { UriSource = new Uri("pack://application:,,,/SMM;component/Shaders/MultiplyEffect.ps") };
    public static readonly DependencyProperty InputProperty =
        RegisterPixelShaderSamplerProperty(
            "Input", typeof(MultiplyEffect), 0);
    public static readonly DependencyProperty OverlayColorProperty =
        DependencyProperty.Register(
            "OverlayColor",
            typeof(Color),
            typeof(MultiplyEffect),
            new UIPropertyMetadata(
                Colors.White,
                PixelShaderConstantCallback(0)));

    public Brush Input
    {
        get => (Brush)GetValue(InputProperty);
        set => SetValue(InputProperty, value);
    }
    public Color OverlayColor
    {
        get => (Color)GetValue(OverlayColorProperty);
        set => SetValue(OverlayColorProperty, value);
    }
    
    public MultiplyEffect()
    {
        PixelShader = _shader;
        UpdateShaderValue(InputProperty);
        UpdateShaderValue(OverlayColorProperty);
    }
}