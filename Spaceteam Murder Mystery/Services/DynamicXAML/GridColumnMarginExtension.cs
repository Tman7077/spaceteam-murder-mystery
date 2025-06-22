namespace SMM.Services.DynamicXAML;

/// <summary>
/// MarkupExtension that binds Margin to:
///   ratio * (columnStarWeight/totalStarWeight) * Grid.ActualWidth
/// so that your button always gets X% of its own column’s width as margin.
/// </summary>
[MarkupExtensionReturnType(typeof(BindingBase))]
public class GridColumnMarginExtension : MarkupExtension
{
    /// <summary>
    /// Fraction of the column’s width to use on each side.
    /// e.g. 0.4 means 40% on left & right.
    /// </summary>
    public double Ratio { get; set; }

    public GridColumnMarginExtension() { }
    public GridColumnMarginExtension(double ratio) => Ratio = ratio;

    public static BindingBase GetBinding(double ratio)
    {
        MultiBinding mb = new()
        {
            Mode               = BindingMode.OneWay,
            Converter          = new GridColumnMarginMultiConverter(),
            ConverterParameter = ratio
        };

        mb.Bindings.Add(new Binding("ActualWidth")
        {
            RelativeSource = new RelativeSource(
                RelativeSourceMode.FindAncestor,
                typeof(Grid), 1)
        });

        mb.Bindings.Add(new Binding("(Grid.Column)")
        {
            RelativeSource = new RelativeSource(
                RelativeSourceMode.Self)
        });

        mb.Bindings.Add(new Binding("ColumnDefinitions")
        {
            RelativeSource = new RelativeSource(
                RelativeSourceMode.FindAncestor,
                typeof(Grid), 1)
        });

        return mb;
    }

    public override object ProvideValue(IServiceProvider serviceProvider) =>
        GetBinding(Ratio).ProvideValue(serviceProvider);

    // converter that does all the math
    private class GridColumnMarginMultiConverter : IMultiValueConverter
    {
        // values[0] = gridActualWidth (double)
        // values[1] = columnIndex     (int)
        // values[2] = ColumnDefinitionCollection
        // parameter = Ratio (double)
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 3 ||
                values[0] is not double gridWidth ||
                values[1] is not int   colIndex ||
                values[2] is not ColumnDefinitionCollection cols ||
                !double.TryParse(
                    parameter?.ToString(),
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out double ratio))
            { return new Thickness(0); }

            // sum up all star-weights; pick out this column’s weight
            double total = 0, myWeight = 0;
            for (int i = 0; i < cols.Count; i++)
            {
                GridLength gl = cols[i].Width;
                if (gl.GridUnitType == GridUnitType.Star)
                {
                    total += gl.Value;
                    if (i == colIndex) 
                        myWeight = gl.Value;
                }
            }
            if (total <= 0 || myWeight <= 0) 
                return new Thickness(0);

            // compute actual column width, then margin
            double columnWidth = gridWidth * myWeight / total;
            double m = columnWidth * ratio;
            return new Thickness(m, 0, m, 0);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}