namespace SMM.Services.DynamicXAML;

public static class AspectRatioHelper
{
    public static readonly DependencyProperty AspectRatioProperty =
        DependencyProperty.RegisterAttached("AspectRatio", typeof(double), typeof(AspectRatioHelper),
            new PropertyMetadata(0.0, OnAspectRatioChanged));

    public static double GetAspectRatio(DependencyObject obj) => (double)obj.GetValue(AspectRatioProperty);
    public static void SetAspectRatio(DependencyObject obj, double value) => obj.SetValue(AspectRatioProperty, value);

    private static void OnAspectRatioChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is FrameworkElement element && e.NewValue is double ratio && ratio > 0)
        {
            element.SizeChanged += (s, ev) =>
            { element.Height = element.ActualWidth * ratio; };
        }
    }
}
