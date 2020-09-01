using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContextMenu.NewMenuItemDIR
{
    class Model
    {
        public ObservableCollection<ContextMenuKey> GetTemplateKeys(ContextMenuType type)
        {
            switch (type)
            {
                case ContextMenuType.DesktopBackground:     return GetTemplateKeys_DesktopBackground();
                case ContextMenuType.FolderBackground:      return GetTemplateKeys_FolderBackground();
                case ContextMenuType.Directory:             return GetTemplateKeys_Directory();
                case ContextMenuType.File:                  return GetTemplateKeys_File();

                case ContextMenuType.None:                  return null;
            }
            return null; //Мини костыль. Ошибка: Не все ветви кода возвращают значение!
        }


        private ObservableCollection<ContextMenuKey> GetTemplateKeys_DesktopBackground()
        {
            ObservableCollection<ContextMenuKey> returnCollection = new ObservableCollection<ContextMenuKey>();
            return returnCollection;
        }
        private ObservableCollection<ContextMenuKey> GetTemplateKeys_FolderBackground()
        {
            ObservableCollection<ContextMenuKey> returnCollection = new ObservableCollection<ContextMenuKey>();
            return returnCollection;
        }
        private ObservableCollection<ContextMenuKey> GetTemplateKeys_Directory()
        {
            ObservableCollection<ContextMenuKey> returnCollection = new ObservableCollection<ContextMenuKey>();
            return returnCollection;
        }
        private ObservableCollection<ContextMenuKey> GetTemplateKeys_File()
        {
            ObservableCollection<ContextMenuKey> returnCollection = new ObservableCollection<ContextMenuKey>();
            return returnCollection;
        }

    }
}
