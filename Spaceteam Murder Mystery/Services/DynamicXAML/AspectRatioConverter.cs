namespace SMM.Services.DynamicXAML;

public class AspectRatioConverter : IValueConverter
{
    public double Ratio { get; set; } = 0.33; // height = 33% of width (by default)

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is double width && double.TryParse(parameter?.ToString(), out double ratio))
        { return width * ratio; }
        return Binding.DoNothing;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        throw new NotImplementedException();
}
