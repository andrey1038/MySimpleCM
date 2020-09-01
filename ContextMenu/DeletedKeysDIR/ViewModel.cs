using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ContextMenu.DeletedKeysDIR
{
    class ViewModel :
        INotifyPropertyChanged
    {
        private ObservableCollection<StoredContextMenuKey> contextMenuKeys; //хранит ключи которые отображаются в левой панели в ListBox
        public ObservableCollection<StoredContextMenuKey> ContextMenuKeys
        {
            get { return contextMenuKeys; }
            set
            {
                contextMenuKeys = value;
                OnPropertyChanged("ContextMenuKeys");
            }
        }

        private StoredContextMenuKey selectedKey;
        public StoredContextMenuKey SelectedKey
        {
            get { return selectedKey; }
            set
            {
                selectedKey = value;
                OnPropertyChanged("SelectedKey");
            }
        }

        private bool? dialogResult = null; //Для доп.информации смотри класс 'DialogCloser'
        public bool? DialogResult
        {
            get { return dialogResult; }
            set
            {
                dialogResult = value;
                OnPropertyChanged("DialogResult");
            }
        }

        private bool isEmptyKeys = false;
        public bool IsEmptyKeys
        {
            get
            {
                return isEmptyKeys;
            }
            set
            {
                isEmptyKeys = value;
                OnPropertyChanged("IsEmptyKeys");
            }
        }

        private ContextMenu.DeletedKeysDIR.Model model;
        private string pathDir;
        public ViewModel(string pathDir)
        {
            this.pathDir = pathDir;
            model = new ContextMenu.DeletedKeysDIR.Model();
            ContextMenuKeys = model.DeserializeKeysFromDIR(pathDir);
            IsEmptyKeys = ContextMenuKeys.Count == 0 ? true : false;
        }

        //--------- команды ---------//

        private RelayCommand restoreKeyCommand;
        public RelayCommand RestoreKeyCommand
        {
            get
            {
                return restoreKeyCommand ?? (restoreKeyCommand = new RelayCommand(obj => {
                    DialogResult = true;
                }, obj => SelectedKey != null));
            }
        }

        private RelayCommand clearAll;
        public RelayCommand ClearAll
        {
            get
            {
                return clearAll ?? (clearAll = new RelayCommand(obj => {

                    if (
                        MessageBox.Show("Будут окончательно удалены все пункты контекстного меню, без возможности востановления. Уверены что хотите продолжить ?",
                                        "Question", MessageBoxButton.YesNo, MessageBoxImage.Question)
                        == MessageBoxResult.Yes
                        )
                    {
                        model.ClearAll(pathDir);
                        IsEmptyKeys = true;
                    }
                }));
            }
        }

        //--------- реализация интерфейса 'INotifyPropertyChanged' ---------//

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(prop)); }
        }
    }
}
