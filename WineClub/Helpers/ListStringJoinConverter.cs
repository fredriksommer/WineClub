using Microsoft.UI.Xaml.Data;
using System;
using System.Collections;
using System.Text;

namespace WineClub.Helpers
{
    public class ListStringJoinConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is not IList items || items.Count == 0)
            {
                return "";
            }

            StringBuilder sb = new();

            foreach (object item in items)
            {
                _ = sb.AppendLine(item.ToString());
            }

            return sb.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
