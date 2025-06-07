namespace SMM.Services.DynamicXAML;

public class DivideWidthConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not double width) return 0;

        // default divisor
        double divisor = 6;

        // try to parse the ConverterParameter
        if (parameter != null 
            && double.TryParse(parameter.ToString(), out var p)
            && p != 0)
        { divisor = p; }

        return width / divisor;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
