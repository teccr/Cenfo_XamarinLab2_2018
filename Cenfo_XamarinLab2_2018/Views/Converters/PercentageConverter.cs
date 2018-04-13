using System;
using System.Globalization;
using Xamarin.Forms;

namespace Cenfo_XamarinLab2_2018.Views.Converters
{
    public class PercentageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                float convertedValue = ConvertToFloat(value, culture);
                return (convertedValue * 100).ToString() + " %";
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                float convertedValue = ConvertToFloat(value, culture);
                return convertedValue / 100;
            }
            return 0;
        }

        private float ConvertToFloat(object rawValue, CultureInfo culture) 
        {
            string strValue = rawValue.ToString();
            float result = 0f;

            if(!string.IsNullOrWhiteSpace(strValue)) 
            {
                strValue = strValue.Trim();
                strValue = strValue.Replace(" %", "");
                if (float.TryParse(strValue, NumberStyles.Any, culture, out result))
                {
                    return result;
                }    
            }

            return 0f;
        }
    }
}
