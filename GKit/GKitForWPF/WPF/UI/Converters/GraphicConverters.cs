﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using GKit;
using GKit.WPF;

namespace GKit.WPF.UI.Converters {
	public class ColorToLightConverter : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			if (value == null)
				return null;

			if(value is SolidColorBrush) {
				Color color = ((SolidColorBrush)value).Color;
				color = color.Light(15);

				return new SolidColorBrush(color);
			} else if(value is Brush) {
				return value;
			}
			throw new InvalidOperationException($"Unsupported type '{value.GetType().Name}'");
		}
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			throw new NotImplementedException();
		}
	}
	public class ColorToDarkConverter : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			if (value == null)
				return null;

			if (value is string) {
				Color color = ((SolidColorBrush)value).Color;
				color = color.Light(-15);

				return new SolidColorBrush(color);
			} else if (value is Brush) {
				return value;
			}
			throw new InvalidOperationException($"Unsupported type '{value.GetType().Name}'");
		}
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			throw new NotImplementedException();
		}
	}
}
