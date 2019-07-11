using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WpfAppPhonesMVVM.Converters
{
    public class PriceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value != null)
            {          
                if((int)value == 0)
                {
                    return null;
                }     
                return string.Format("{0} грн", value);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string res = string.Empty;
            foreach(char c in value.ToString())
            {
                if(char.IsDigit(c))
                {
                    res += c;
                }
            }
            if(res.Length>0)
            {
                int price = 0;
                try
                {
                    price = System.Convert.ToInt32(res);
                }
                catch 
                {}
                return price;
               
            }
            return 0;
        }
    }
}
