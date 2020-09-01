using Microsoft.Win32;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Windows.Media.Animation;

namespace ContextMenu
{
    public partial class MainWindow : Window
    {
        private Main.ViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();
            this.viewModel = new Main.ViewModel();
            this.DataContext = this.viewModel;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            bool IsFocusTextBlock = false;
            if (TB1.IsFocused || TB2.IsFocused || TB3.IsFocused)
                IsFocusTextBlock = true;

            RelayCommand command = null;
            switch (e.Key)
            {
                case Key.Escape:        command = viewModel.EscCommand;                break;
                case Key.Delete:        command = viewModel.DeleteKeyCommand;          break;
                case Key.LeftShift:     command = IsFocusTextBlock ? null : viewModel.SHIFTCommand;     break;
                case Key.S:
                    if (e.KeyboardDevice.Modifiers == ModifierKeys.Control)
                    {
                                        command = viewModel.SaveKeyCommand;
                    }
                    break;
            }

            if (command != null) {
                if (command.CanExecute(null))
                {
                    command.Execute(null);
                }
            }
        }
    }
}