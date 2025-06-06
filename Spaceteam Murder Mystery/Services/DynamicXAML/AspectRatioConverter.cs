namespace SMM.Services.DynamicXAML;

using System.Globalization;

public class AspectRatioConverter : IValueConverter
{
    public double Ratio { get; set; } = 0.4; // default 1:1

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is double width && double.TryParse(parameter?.ToString(), out double ratio))
        { return width * ratio; }
        return Binding.DoNothing;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    { throw new NotImplementedException(); }
}
