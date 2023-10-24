using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace DetectionLangue.ViewModels.Converters
{
    internal class BoolToVisibility : IValueConverter
    {
        // Conversion du Model vers la View
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Console.WriteLine(value.ToString());
            bool texteExiste = (bool)value;
            return texteExiste ? "Visible" : "Hidden";
        }

        // Conversion de la View vers le Model
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
