namespace SMM.Services.DynamicXAML;

public class ParentBasedMarginConverter : IValueConverter
{
    // value: parent.ActualWidth
    // parameter: ratio (double)
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is double width
            && parameter != null
            && double.TryParse(parameter.ToString(), out var ratio)
            && ratio > 0)
        {
            var margin = width * ratio;
            return new Thickness(margin, 0, margin, 0);   // all four sides
        }

        return new Thickness(0);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}