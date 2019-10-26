using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Data;
using System.Windows;

namespace Medibox.Converters
{
    //
    // Konwertuje wartości boolean do Visibility
    //
	public sealed class BooleanToVisibilityNotConverter : IValueConverter 
	{ 
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return (value is bool && (bool)value) ? Visibility.Collapsed : Visibility.Visible; 
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return value is Visibility && (Visibility)value == Visibility.Collapsed; 
		}
	}
}
