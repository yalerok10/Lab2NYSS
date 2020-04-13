using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Lab2NYSS
{
	public class TextColorConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if ((bool)value) {
				return Brushes.Red;
			}
			return Brushes.Green;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
