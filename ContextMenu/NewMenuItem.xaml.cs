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
    public partial class NewMenuItem : Window
    {
        private NewMenuItemDIR.ViewModel viewModel;

        public NewMenuItem(ContextMenuType type)
        {
            InitializeComponent();
            this.DataContext = new NewMenuItemDIR.ViewModel(type);
        }
    }
}
