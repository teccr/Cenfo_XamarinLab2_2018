using System;
using System.Globalization;
using Xamarin.Forms;

namespace Cenfo_XamarinLab2_2018.Views.Converters
{
    public class ProfilePictureConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && !string.IsNullOrWhiteSpace(value.ToString()))
            {
                return value;
            }
            return "takePicture";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value.ToString() == "takePicture")
            {
                return null;
            }
            return value;
        }
    }
}
