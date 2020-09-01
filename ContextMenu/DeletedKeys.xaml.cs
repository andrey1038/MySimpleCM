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
    public partial class DeletedKeys : Window
    {
        //поскольку эта форма явялется диалоговым окном, она должна возвращать резульатат своей работы
        //этим результатом является экземпляр класса 'ContextMenuKey'
        //смотри поле ниже:
        public StoredContextMenuKey RestoreKey
        {
            get
            {
                return viewModel.SelectedKey;
            }
        }

        private DeletedKeysDIR.ViewModel viewModel;
        public DeletedKeys(string pathDir)
        {
            InitializeComponent();
            viewModel = new DeletedKeysDIR.ViewModel(pathDir);
            this.DataContext = viewModel;
        }
    }
}
