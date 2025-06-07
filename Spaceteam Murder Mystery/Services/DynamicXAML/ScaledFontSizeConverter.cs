namespace SMM.Services.DynamicXAML;

public class ScaledFontSizeConverter : IMultiValueConverter
{
    public double BaseFontSize { get; set; } = 16.0;        // logical "default" font size
    public double DesignWidth { get; set; } = 854.0;        // base window width for scaling
    public double MaxHeightProportion { get; set; } = 0.1;  // max 10% of window height

    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values.Length < 2 ||
            values[0] is not double windowWidth ||
            values[1] is not double windowHeight)
            return BaseFontSize;

        double multiplier = 1.0;
        if (parameter != null && double.TryParse(parameter.ToString(), out double parsedMultiplier))
            multiplier = parsedMultiplier;

        // Step 1: Scale based on width
        double widthScale = windowWidth / DesignWidth;
        double scaledFontSize = BaseFontSize * widthScale * multiplier;

        // Step 2: Cap based on height
        double maxFontSize = windowHeight * MaxHeightProportion;
        return Math.Min(scaledFontSize, maxFontSize);
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
