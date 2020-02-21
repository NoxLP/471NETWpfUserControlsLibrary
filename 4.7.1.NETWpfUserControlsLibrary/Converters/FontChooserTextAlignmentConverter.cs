using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using NET471WpfUserControlsLibrary.ChoosersPickers;

namespace NET471WpfUserControlsLibrary.Converters
{
    public class FontChooserTextAlignmentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return null;

            var horizontalItem = value as FontChooserUserControl.HorizontalAlignmentCBItem;
            if (horizontalItem == null)
            {
                var verticalItem = value as FontChooserUserControl.VerticalAlignmentCBItem;
                if (verticalItem == null)
                    return null;

                return verticalItem.VAlign;
            }

            return horizontalItem.HAlign;
        }
    }
}
