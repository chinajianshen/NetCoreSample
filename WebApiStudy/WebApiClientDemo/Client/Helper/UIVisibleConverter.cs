using System;
using System.Windows;
using System.Windows.Data;
using Client.Models;


namespace Client.Helpers
{
    /// <summary>
    /// 当ModelType为View的时候显示
    /// </summary>
    [ValueConversion(typeof(EnuModelType), typeof(Visibility))]
    public class UIModel_View_Visible : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (EnuModelType)value == EnuModelType.View ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 当ModelType为Add的时候显示
    /// </summary>
    [ValueConversion(typeof(EnuModelType), typeof(Visibility))]
    public class UIModel_Add_Visible : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (EnuModelType)value == EnuModelType.Add ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 当ModelType为Edit的时候显示
    /// </summary>
    [ValueConversion(typeof(EnuModelType), typeof(Visibility))]
    public class UIModel_Edit_Visible : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (EnuModelType)value == EnuModelType.Edit ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 当ModelType为Add或Edit的时候显示
    /// </summary>
    [ValueConversion(typeof(EnuModelType), typeof(Visibility))]
    public class UIModel_AddOrEdit_Visible : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ((EnuModelType)value == EnuModelType.Add || (EnuModelType)value == EnuModelType.Edit) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 当ModelType为View或Add的时候显示
    /// </summary>
    [ValueConversion(typeof(EnuModelType), typeof(Visibility))]
    public class UIModel_EditOrView_Visible : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ((EnuModelType)value == EnuModelType.Edit || (EnuModelType)value == EnuModelType.View) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
