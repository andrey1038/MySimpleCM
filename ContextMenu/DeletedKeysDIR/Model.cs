using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ContextMenu.DeletedKeysDIR
{
    class Model
    {
        public ObservableCollection<StoredContextMenuKey> DeserializeKeysFromDIR(string FolderPath)
        {
            if (!Directory.Exists(FolderPath))
                throw new Exception("The directory does not exist.");


            ObservableCollection<StoredContextMenuKey> returnCollection = new ObservableCollection<StoredContextMenuKey>();

            DirectoryInfo SerializeKeys = new DirectoryInfo(FolderPath);
            foreach (FileInfo file in SerializeKeys.GetFiles())
            {
                if (file.Extension == ".reg")
                    returnCollection.Add(new StoredContextMenuKey(file.FullName));
            }

                
            return SORT(ref returnCollection);
        }
        public ObservableCollection<StoredContextMenuKey> SORT(ref ObservableCollection<StoredContextMenuKey> target)
        {
            ObservableCollection<StoredContextMenuKey> returnCollection = new ObservableCollection<StoredContextMenuKey>();

            var collection = from i in target
                             orderby i.TimeKeySerialize descending
                             select i;

            foreach (StoredContextMenuKey key in collection)
            {
                returnCollection.Add(key);
            }


            return returnCollection;
        }
        public void ClearAll(string FolderPath)
        {
            DirectoryInfo removeKeys = new DirectoryInfo(FolderPath);
            if (removeKeys.Exists)
            {
                foreach (FileInfo file in removeKeys.GetFiles())
                {
                    File.Delete(file.FullName);
                }
            }
        }
    }
}
