using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ContextMenu.NewMenuItemDIR
{
    class ViewModel :
        INotifyPropertyChanged
    {
        private ObservableCollection<ContextMenuKey> contextMenuKeys;
        public ObservableCollection<ContextMenuKey> ContextMenuKeys
        {
            get { return contextMenuKeys; }
            set
            {
                contextMenuKeys = value;
                OnPropertyChanged("ContextMenuKeys");
            }
        }


        //--------- конструтор и модель ---------//

        private ContextMenu.NewMenuItemDIR.Model model;
        public ViewModel(ContextMenuType type)
        {
            if (type == ContextMenuType.None) { throw new Exception("input: ContextMenuType.none"); }
            model = new ContextMenu.NewMenuItemDIR.Model();
            ContextMenuKeys = model.GetTemplateKeys(type);
        }
        //--------- доп. ---------//

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(prop)); }
        }
    }
}
