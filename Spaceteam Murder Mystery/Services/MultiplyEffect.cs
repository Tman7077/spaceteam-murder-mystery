namespace SMM.Services;

using System.Windows.Media.Effects;

/// <summary>
/// Effect to apply a shader (by multiplying color values).
/// </summary>
public class MultiplyEffect : ShaderEffect
{
    /// <summary>
    /// The shader to apply. Pre-compiled and included as a resource.
    /// </summary>
    private static readonly PixelShader _shader = new()
    { UriSource = new Uri("pack://application:,,,/SMM;component/Shaders/MultiplyEffect.ps") };

    /// <summary>
    /// The input brush to apply the shader to. This is the first sampler in the shader.
    /// </summary>
    public static readonly DependencyProperty InputProperty =
        RegisterPixelShaderSamplerProperty(
            "Input", typeof(MultiplyEffect), 0);

    /// <summary>
    /// The color by which to multiply the input brush. This is the second sampler in the shader.
    /// </summary>
    public static readonly DependencyProperty OverlayColorProperty =
        DependencyProperty.Register(
            "OverlayColor",
            typeof(Color),
            typeof(MultiplyEffect),
            new UIPropertyMetadata(
                Colors.White,
                PixelShaderConstantCallback(0)));

    /// <summary>
    /// The input brush to which to apply the shader.
    /// </summary>
    public Brush Input
    {
        get => (Brush)GetValue(InputProperty);
        set => SetValue(InputProperty, value);
    }

    /// <summary>
    /// The color by which to multiply the input brush.
    /// </summary>
    public Color OverlayColor
    {
        get => (Color)GetValue(OverlayColorProperty);
        set => SetValue(OverlayColorProperty, value);
    }

    /// <summary>
    /// Creates a new instance of the shader.
    /// </summary>
    public MultiplyEffect()
    {
        PixelShader = _shader;
        UpdateShaderValue(InputProperty);
        UpdateShaderValue(OverlayColorProperty);
    }
}