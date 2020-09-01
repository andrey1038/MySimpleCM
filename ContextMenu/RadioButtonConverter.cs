using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace ContextMenu
{
    class RadioButtonBoolConverter :
        IValueConverter
    {
        //https://www.cyberforum.ru/wpf-silverlight/thread1125842.html

        // example in HAML:   IsChecked="{Binding Path=*****, Converter={StaticResource RadioButtonBoolConverter}, ConverterParameter=0}"
        // ConverterParameter:   1 = true  |  -1 = null  |  0 = false
        // !!! USE TwoWay MODE !!!

        private bool? BackValue; //костыль ! так как этот класс работает с радио кнопками возникает ситуация что после вызова ConvertBack вызывается снова ConvetBack но с Value = false что приводит к тому что не должно возвращаться какое либо значение в объект. Решением было использовать return DependencyProperty.UnsetValue; , но этот вариант обводит красным радио кнопку в реалтайме у вью что НЕДОПУСТИМО. В конце концов был придуман этот "костыль". Этот "костыль" принимает значение при первом вызове ConvetBack, а при втором вызове ConvetBack просто отдает свое значение. (будет вызван метод SET два раза и переданые значения будут одинаковыми)

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((string)parameter)
            {
                case "1":       return true  == (bool?)value ? true : false;
                case "-1":      return null  == (bool?)value ? true : false;
                case "0":       return false == (bool?)value ? true : false;

                default:
                    throw new Exception("Unknown parameter.");
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool?)value == true)
            {
                switch ((string)parameter)
                {
                    case "1": BackValue = true; break;
                    case "-1": BackValue = null; break;
                    case "0": BackValue = false; break;

                    default:
                        throw new Exception("Unknown parameter.");
                }

                return BackValue;
            }
            else if ((bool?)value == false)
            {
                return BackValue;
            }
            else
            {
                throw new Exception("BadValue.");
            }
        }
    }
}