using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;

namespace ContextMenu.Main
{
    class Model
    {
        
        public void OpenRegedit(ContextMenuKey key)
        {
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software", true).OpenSubKey("Microsoft", true).OpenSubKey("Windows", true).OpenSubKey("CurrentVersion", true).OpenSubKey("Applets", true).OpenSubKey("Regedit", true);
            if (registryKey == null)
                ThisProgram.ThrowException(new MyProgramException(@"Не удалось получить ключ 'HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\Applets\Regedit'", TypeException.NotFatal));
            else
            {
                registryKey.SetValue("LastKey", key.FullNameRegistryKey);
                Process.Start("regedit.exe");
            }
        } //открывает окно реестра

        public RegistryKey GetRegistryKey(string pathKey)
        {
            RegistryKey key = null;
            string[] path = pathKey.Split(new char[] { '\\' });

            switch (path[0])
            {
                case "HKEY_CLASSES_ROOT": key = Microsoft.Win32.Registry.ClassesRoot; break;
                case "HKEY_CURRENT_USER": key = Microsoft.Win32.Registry.CurrentUser; break;
                case "HKEY_LOCAL_MACHINE": key = Microsoft.Win32.Registry.LocalMachine; break;
                case "HKEY_USERS": key = Microsoft.Win32.Registry.Users; break;
                case "HKEY_CURRENT_CONFIG": key = Microsoft.Win32.Registry.CurrentConfig; break;
            }

            for (int i = 1; i < path.Length; i++)
            {
                key = key.OpenSubKey(path[i], true);
            }

            return key;
        }
        public RegistryKey GetParentRegistryKey(RegistryKey key)
        {            
            string[] path = key.Name.Split(new char[] { '\\' });

            string temp = string.Empty;
            for (int i = 0; i < path.Length - 1; i++)
                temp += path[i] + "\\";

            return this.GetRegistryKey(temp);
        }

        public ObservableCollection<ContextMenuKey> Read_RegistryDirectory(ContextMenuType type, bool SHIFT)
        {
            if (type == ContextMenuType.None)
            {
                return null;
            }
            else
            {
                ObservableCollection<ContextMenuKey> returnCollection = new ObservableCollection<ContextMenuKey>();
                RegistryKey RKCMT = Registry.ClassesRoot; //Registry Key Context Menu Type

                switch (type)
                {
                    case ContextMenuType.DesktopBackground:     RKCMT = RKCMT.OpenSubKey("DesktopBackground");

                        //рабочий стол тоже является папкой и по этому часть ключей данной директории хранится в разделе 'Directory'
                        foreach (ContextMenuKey key in this.Read_RegistryDirectory(ContextMenuType.Directory, SHIFT))
                        {
                            key.Disabled = true; //запрет на изменение ключей
                            returnCollection.Add(key);
                        }
                        
                        break;
                    case ContextMenuType.FolderBackground:      RKCMT = RKCMT.OpenSubKey("Folder"); break;
                    case ContextMenuType.Directory:             RKCMT = RKCMT.OpenSubKey("Directory"); break;
                    case ContextMenuType.File:                  RKCMT = RKCMT.OpenSubKey("*"); break;
                }

                RKCMT = RKCMT.OpenSubKey("Shell");
                foreach (string strkey in RKCMT.GetSubKeyNames())
                {
                    RegistryKey key_registry = RKCMT.OpenSubKey(strkey);
                    returnCollection.Add(new ContextMenuKey(key_registry));
                }

                if (SHIFT == false)
                    return this.RemoveExtendedKeys(returnCollection);
                else
                    return returnCollection;
            }
        }
        public ObservableCollection<ContextMenuKey> Read_RegistryDirectory(RegistryKey key, bool SHIFT)
        {
            ObservableCollection<ContextMenuKey> returnCollection = new ObservableCollection<ContextMenuKey>();

            foreach (string strkey in key.GetSubKeyNames())
            {
                RegistryKey key_registry = key.OpenSubKey(strkey);
                returnCollection.Add(new ContextMenuKey(key_registry));
            }

            if (SHIFT == false)
                return this.RemoveExtendedKeys(returnCollection);
            else
                return returnCollection;
        }

        public ObservableCollection<ContextMenuKey> RemoveExtendedKeys(ObservableCollection<ContextMenuKey> keys)
        {
            if (keys == null)
                return null;
            else
            {
                //удаляет все ключи из коллекции с строковым параметром 'Extended'
                for (int i = 0; i < keys.Count; i++)
                {
                    if (keys[i].Extended == true) {
                        keys.RemoveAt(i);
                    }
                }

                return keys;
            }
        }

        public ObservableCollection<ContextMenuKey> ReadMenu(ContextMenuKey key, bool SHIFT)
        {
            ObservableCollection<ContextMenuKey> returnCollection = new ObservableCollection<ContextMenuKey>();

            //Защита на всякий случай, хотя в ViewModel аж 2е точно такие же проверки перед вызовом этого метода.  
            if (key == null) { throw new Exception("KeyNull"); }
            if (key.IsMenu == false) { throw new Exception("KeyIsMenuFalse"); }

            //RegistryKey subShell = key.RegistryKey.OpenSubKey("shell"); //смотри класс ContextMenuKey закоментированое поле 'RegistryKey'
            RegistryKey subShell = key.RegistryKey.OpenSubKey("shell");
            
            foreach (string strkey in subShell.GetSubKeyNames())
            {
                RegistryKey subKey_registry = subShell.OpenSubKey(strkey);
                returnCollection.Add(new ContextMenuKey(subKey_registry));
            }

            return returnCollection;
        }
        
        public ContextMenuKey CreateKey(ContextMenuType type)
        {
            return ContextMenuKey.CreateEmptyKey(type);
        }
        public ContextMenuKey CreateKey(ContextMenuKey key)
        {
            return ContextMenuKey.CreateEmptyKey(key);
        }

        public ContextMenuKey CreateMenuKey(ContextMenuType type)
        {
            return ContextMenuKey.CreateEmptyMenuKey(type);
        }
        public ContextMenuKey CreateMenuKey(ContextMenuKey key)
        {
            return ContextMenuKey.CreateEmptyMenuKey(key);
        }

        static public ContextMenuKey RestoreKey(StoredContextMenuKey key)
        {
            try
            {
                return key.FromStore();
            }
            catch (Exception ex) {
                ThisProgram.ThrowException(ex);
                return null;
            }
        }



        static public void SaveKey(ref ContextMenuKey key)
        {
            try
            {
                key.Save();
            }
            catch (Exception ex) {
                ThisProgram.ThrowException(ex);
            }
        }
        static public void DeleteKey(ContextMenuKey key)
        {
            try
            {
                StoredContextMenuKey.ToStore(key, ThisProgram.Path_RemoveKeys, true);
            }
            catch (Exception ex) {
                ThisProgram.ThrowException(ex);
            }
        }
        static public void ResetKey(ref ContextMenuKey key)
        {
            try
            {
                key.Reset();
            }
            catch (Exception ex) {
                ThisProgram.ThrowException(ex);
            }
        }
    }
}
