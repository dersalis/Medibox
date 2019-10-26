using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Data;
using System.Threading;

namespace Medibox.Converters
{
    public class DateToStringConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            // Jeśli data to dzień dzisiejszy to wyświetl tylko godzinę
            //if((DateTime)value >= DateTime.Now && (DateTime)value < (DateTime.Now + new TimeSpan(1, 0, 0, 0)))
            //    return string.Format("{0:HH:mm}", (DateTime)value);
            return string.Format("{0:ddd, dd MMM yyyy}", (DateTime)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value.GetType() == typeof(string) && targetType == typeof(DateTime))
            {
                DateTime newDate;
                if (DateTime.TryParse((string)value, out newDate))
                    return newDate;
            }
            //jeśli nie wprowadzamy zmian to zwróć wartość
            return value;
        }
    }
}
