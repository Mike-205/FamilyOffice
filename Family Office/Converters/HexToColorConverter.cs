
using Family_Office.ViewModel;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Family_Office.Converters
{
    public class HexToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            try
            {
                string hexColor = value.ToString();
                if (string.IsNullOrEmpty(hexColor)) return null;
                // Ensure the hex color starts with #
                if (!hexColor.StartsWith("#"))
                    hexColor = "#" + hexColor;
                // Convert hex to color using BrushConverter
                var brush = new BrushConverter().ConvertFrom(hexColor) as SolidColorBrush;
                return brush ?? Brushes.Black;
            }
            catch
            {
                return Brushes.Black;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is SolidColorBrush brush)
            {
                Color color = brush.Color;
                return $"#{color.R:X2}{color.G:X2}{color.B:X2}";
            }
            return "#000000";
        }
    }

    public class KeyValueItemToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is KeyValueItem<string> keyValueItem && !string.IsNullOrEmpty(keyValueItem.Value))
            {
                try
                {
                    string hexColor = keyValueItem.Value;
                    if (!hexColor.StartsWith("#"))
                        hexColor = "#" + hexColor;
                    var brush = new BrushConverter().ConvertFrom(hexColor) as SolidColorBrush;
                    return brush ?? Brushes.Black;
                }
                catch
                {
                    return Brushes.Black;
                }
            }
            return Brushes.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is SolidColorBrush brush)
            {
                Color color = brush.Color;
                return new KeyValueItem<string>
                {
                    Value = $"#{color.R:X2}{color.G:X2}{color.B:X2}",
                    DisplayName = $"Color #{color.R:X2}{color.G:X2}{color.B:X2}"
                };
            }
            return null;
        }
    }
}