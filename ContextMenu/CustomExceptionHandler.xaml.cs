using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ContextMenu
{
    /// <summary>
    /// Логика взаимодействия для CustomExceptionHandler.xaml
    /// </summary>
    public partial class CustomExceptionHandler : Window
    {
        private Exception exception;

        public CustomExceptionHandler(Exception exception)
        {
            InitializeComponent();
            ErrorTextBlock.Text += exception.Message;
            this.exception = exception;
        }

        public CustomExceptionHandler(string message)
        {
            InitializeComponent();
            ErrorTextBlock.Text += message;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (exception is MyProgramException)
            {
                if (((MyProgramException)exception).Type == TypeException.Fatal)
                    Environment.Exit(0);
            }
            else if (exception is Exception)
            {
                Environment.Exit(0);
            }   
        }
    }
}
