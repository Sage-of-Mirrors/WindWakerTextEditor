using System;
using System.Windows.Data;

namespace WindWakerTextEditor.View
{
    /// <summary>
    /// Null-to-Bool converter. If an object is null, Convert returns false.
    /// </summary>
    public class NullToFalseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value != null;
        }

        public object ConvertBack(object value, Type targetType,
          object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
