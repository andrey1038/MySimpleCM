using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ContextMenu
{
    class DialogCloser
    {
        //https://blog.excastle.com/2010/07/25/mvvm-and-dialogresult-with-no-code-behind/
        //https://stackoverflow.com/questions/501886/how-should-the-viewmodel-close-the-form/3329467#3329467
        //класс предназначен для чистого паттерна MVVM. Класс управляет свойством dialogResult.

        // example in ViewModel:
        //
        // private bool? dialogResult = null; //Для доп.информации смотри класс 'DialogCloser'
        // public bool? DialogResult
        // {
        //     get { return dialogResult; }
        //     set
        //     {
        //         dialogResult = value;
        //         OnPropertyChanged("DialogResult");
        //     }
        // }
        //

        // example in View:
        //
        // <Window ... local:DialogCloser.DialogResult="{Binding DialogResult}"> ...
        //

        public static readonly DependencyProperty DialogResultProperty = DependencyProperty.RegisterAttached(
                "DialogResult",
                typeof(bool?),
                typeof(DialogCloser),
                new PropertyMetadata(DialogResultChanged)
            );

        private static void DialogResultChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var window = d as Window;
            if (window != null)
                window.DialogResult = e.NewValue as bool?;
        }
        public static void SetDialogResult(Window target, bool? value)
        {
            target.SetValue(DialogResultProperty, value);
        }
    }
}
