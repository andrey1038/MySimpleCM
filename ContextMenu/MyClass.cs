using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using System.Text.RegularExpressions;
using Microsoft.Win32;
using System.IO;
using System.Collections.Specialized;
using Newtonsoft.Json;

namespace ContextMenu
{
    //типы контекстного меню с которыми работает программа
    public enum ContextMenuType
    {
        None = -1,
        DesktopBackground,
        FolderBackground,
        Directory,
        File
    }

    //представляет строковые параметры из реестра
    public struct KeyParam
    {
        public string               Name { get; set; }
        public RegistryValueKind    Type { get; set; }
        public object               Value { get; set; }
    }

    //не используемый класс
    public class ContextMenuContainer
    {
        static public ObservableCollection<ContextMenuKey> GetKeys(ContextMenuType type)
        {
            RegistryKey container = Registry.ClassesRoot;
            switch (type)
            {
                case ContextMenuType.DesktopBackground: container = container.OpenSubKey("DesktopBackground"); break;
                case ContextMenuType.FolderBackground: container = container.OpenSubKey("Folder"); break;
                case ContextMenuType.Directory: container = container.OpenSubKey("Directory"); break;
                case ContextMenuType.File: container = container.OpenSubKey("*"); break;

                default:
                    throw new Exception("Данный тип контекстного меню не поддерживается.");
            }

            if (container == null)
                throw new Exception("Неверно указаный суб ключ.");



            ObservableCollection<ContextMenuKey> keys = new ObservableCollection<ContextMenuKey>();
            string[] subKeys = container.GetSubKeyNames();

            foreach (string str in subKeys)
            {
                if (str == "shell" || str == "Shell")
                {
                    foreach (ContextMenuKey key in ReadContainer(container.OpenSubKey("shell")))
                    {
                        keys.Add(key);
                    }
                }
                else if (str == "shellex")
                {
                    foreach (ContextMenuKey key in ReadContainer(container.OpenSubKey("shellex").OpenSubKey("ContextMenuHandlers")))
                    {
                        keys.Add(key);
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Неизвестный ключ: " + str);
                    Console.ResetColor();

                    //throw new Exception("Неизвестный ключ.");
                }
            }






            return keys;
        }
        static private ObservableCollection<ContextMenuKey> ReadContainer(RegistryKey container)
        {
            ObservableCollection<ContextMenuKey> keys = new ObservableCollection<ContextMenuKey>();

            foreach (string str in container.GetSubKeyNames())
            {
                keys.Add(new ContextMenuKey(container.OpenSubKey(str)));
            }

            return keys;
        }

        public ObservableCollection<ContextMenuKey> keys { get; set; } = null;
    }

    //класс предоставляет ключ из реестра
    public class ContextMenuKey :
        INotifyPropertyChanged
    {
        public ContextMenuKey(RegistryKey key)
        {
            if (key == null)
                throw new Exception("KEY IS NULL");

            if (false)
                throw new Exception("This is not a context menu key");

            this.FullNameRegistryKey = key.Name;

            //получение имени ключа
            for (int i = this.FullNameRegistryKey.Length - 1; i > 0; i--)
                if (this.FullNameRegistryKey[i] == '\\')
                {
                    this.registryName = this.FullNameRegistryKey.Substring(i + 1, this.FullNameRegistryKey.Length - i - 1);
                    break;
                }

            this.title = this.registryName;

            //проверка на существование подраздела
            string[] subsection = key.GetSubKeyNames();
            if (subsection.Length != 0)
            {
                if (subsection.Length > 1)
                    throw new Exception("Найдено больше одного подраздела у ключа: " + key.Name);

                if (subsection[0] == "command")
                {
                    //чтение подраздела 'command'
                    RegistryKey command = key.OpenSubKey("command");
                    foreach (string keyParam in command.GetValueNames())
                    {
                        if (keyParam == "") { this.command = (string)command.GetValue(""); break; }
                        if (keyParam == "DelegateExecute") { this.command = (string)command.GetValue("DelegateExecute"); break; }
                    }
                }
                else if (subsection[0] == "shell" || subsection[0] == "Shell" || subsection[0] == "DropTarget")
                {

                }
                else
                {
                    throw new Exception("Неизвестный подраздел");
                }
            }

            //получение параметров ключа и работа с известными
            string[] parameters = key.GetValueNames();
            for (int i = 0; i < parameters.Length; i++)
            {
                KeyParam param = new KeyParam()
                {
                    Name = parameters[i],
                    Type = key.GetValueKind(parameters[i]),
                    Value = key.GetValue(parameters[i], null, RegistryValueOptions.None)
                };

                switch (param.Name)
                {
                    case "": /*this.command = (string)param.Value;*/ break;
                    case "SubCommands":

                        if ((string)param.Value == string.Empty)
                            this.isMenu = true;
                        else
                            this.isMenu = null;

                        break;
                    case "ExtendedSubCommandsKey": this.isMenu = true; break;

                    case "MUIVerb":
                        this.title = (string)param.Value;
                        this.muiVerb = (string)param.Value; break;

                    case "Position":
                        switch (((string)param.Value).ToLower())
                        {
                            case "top": this.position = true; break;
                            case "bottom": this.position = false; break;
                            default: this.position = null; break;
                        }
                        break;

                    case "Extended": this.extended = true; break;
                }
            }

            //дополнительный блок анализа имени ключа : в идеале его не должно быть, но я пока не умею выполнять длл !
            Regex regex = new Regex(@"\.(dll|exe)");
            Match rez = regex.Match(this.title);
            if (rez.Value != string.Empty)
            {
                this.title = this.registryName + "_(dll)";
            }

            //дополнительный блок анализа команды ключа : в идеале его не должно быть, но я пока не умею выполнять CLSID !
            if (this.command != null)
            {
                Regex regex2 = new Regex(@"{[^\s^-]{8}-[^\s^-]{4}-[^\s^-]{4}-[^\s^-]{4}-[^\s^-]{12}}");
                Match rez2 = regex2.Match(this.Command);
                if (rez2.Value != string.Empty)
                {
                    this.command = "this command is a CLSID";
                    this.disabled = true;
                }
            }
        }



        private string title; //переменная для вывода, может принимать значение параметра MUIVerb так и самого ключа 
        public string Title
        {
            get
            {
                return muiVerb == null ? registryName : muiVerb;
            }
        }

        private string registryName; //имя видимое только в реестре
        public string RegistryName
        {
            get { return registryName; }
            set
            {
                registryName = value;
                CanSave = true;
                OnPropertyChanged("RegistryName");
                OnPropertyChanged("Title");
            }
        } //сделать проверку на уникальное имя

        private string muiVerb;
        public string MUIVerb
        {
            get { return muiVerb; }
            set
            {
                muiVerb = value == "" ? null : value;
                CanSave = true;
                OnPropertyChanged("MUIVerb");
                OnPropertyChanged("Title");
            }
        }

        private string icon;
        public string Icon
        {
            get { return icon; }
            set
            {
                icon = value;
                CanSave = true;
                OnPropertyChanged("Icon");
            }
        }

        private string command;
        public string Command
        {
            get { return command; }
            set
            {
                command = value;
                CanSave = true;
                OnPropertyChanged("Command");
            }
        }

        private bool extended; //флаг SHIFT
        public bool Extended
        {
            get { return extended; }
            set
            {
                extended = value;
                CanSave = true;
                OnPropertyChanged("Extended");
            }
        }

        private bool? position = null; //true = top | null = midle | false = bottom
        public bool? Position
        {
            get { return position; }
            set
            {
                position = value;
                CanSave = true;
                OnPropertyChanged("Position");
            }
        }



        public string FullNameRegistryKey { get; private set; }

        public RegistryKey ParentRegistryKey
        {
            get
            {
                RegistryKey key = null;
                string[] path = FullNameRegistryKey.Split(new char[] { '\\' });

                switch (path[0])
                {
                    case "HKEY_CLASSES_ROOT": key = Microsoft.Win32.Registry.ClassesRoot; break;
                    case "HKEY_CURRENT_USER": key = Microsoft.Win32.Registry.CurrentUser; break;
                    case "HKEY_LOCAL_MACHINE": key = Microsoft.Win32.Registry.LocalMachine; break;
                    case "HKEY_USERS": key = Microsoft.Win32.Registry.Users; break;
                    case "HKEY_CURRENT_CONFIG": key = Microsoft.Win32.Registry.CurrentConfig; break;
                }

                for (int i = 1; i < path.Length - 1; i++)
                {
                    key = key.OpenSubKey(path[i], true);
                }

                return key;
            }
        }
        public RegistryKey RegistryKey
        {
            get
            {
                RegistryKey key = null;
                string[] path = FullNameRegistryKey.Split(new char[] { '\\' });

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
        }

        private bool? isMenu = false; //null = RegistryKey => subCommands != null
        public bool? IsMenu
        {
            get { return isMenu; }
        }

        private bool canSave = false;
        public bool CanSave
        {
            get { return canSave; }
            private set
            {
                if (canSave ^ value)
                {
                    canSave = value;
                    OnPropertyChanged("CanSave");
                }
            }
        }



        public string TypeKey
        {
            get
            {
                string[] path = FullNameRegistryKey.Split(new char[] { '\\' });
                return path[1];
            }
        } //используется в DeletedKeys.haml => listbox => template ...

        private bool disabled = false;
        public bool Disabled
        {
            get { return disabled; }
            set
            {
                disabled = value;
                OnPropertyChanged("Disabled");
            }
        }

        //--------- методы класса ---------//

        public void Save()
        {
            if (CanSave)
            {
                Microsoft.Win32.RegistryKey TempRegistryKey = this.RegistryKey;

                //Extended
                {
                    object valueParam;
                    bool existsParam = ParameterExist("Extended", TempRegistryKey, out valueParam);

                    if (Extended ^ existsParam)
                    {
                        if (existsParam)
                            TempRegistryKey.DeleteValue("Extended");
                        else
                            TempRegistryKey.SetValue("Extended", "", RegistryValueKind.String);
                    }
                }

                //Position
                {
                    object valueParam;
                    bool existsParam = ParameterExist("Position", TempRegistryKey, out valueParam);
                    if (existsParam) valueParam = ((string)valueParam).ToLower();

                    if ((position == true ^ (string)valueParam == "top") ||
                        (position == null ^ existsParam == false) ||
                        (position == false ^ (string)valueParam == "bottom"))
                    {
                        if (position == null)
                            TempRegistryKey.DeleteValue("Position");
                        else
                            TempRegistryKey.SetValue("Position", position == true ? "Top" : "Bottom", RegistryValueKind.String);
                    }
                }

                //MUIVerb
                {
                    if (this.muiVerb != null)
                        this.muiVerb = this.muiVerb.Trim();

                    object valueParam;
                    bool existsParam = ParameterExist("MUIVerb", TempRegistryKey, out valueParam);


                    if (!(muiVerb == null && existsParam == false) ||
                        !(muiVerb == (string)valueParam))
                    {
                        if (muiVerb == null)
                            TempRegistryKey.DeleteValue("MUIVerb");
                        else
                            TempRegistryKey.SetValue("MUIVerb", muiVerb.Trim());
                    }
                }

                //Command
                {
                    if ((this.isMenu == true || this.isMenu == null) && command != null)
                        throw new Exception("This key is 'menu', commands are not allowed.");

                    if (!(this.isMenu == true || this.isMenu == null))
                    {
                        if (command == null)
                            throw new Exception("This key must have a command.");

                        this.command = this.command.Trim();

                        string[] subsection = TempRegistryKey.GetSubKeyNames();
                        if (subsection.Length == 1)
                        {
                            if (subsection[0] == "command")
                            {
                                RegistryKey RKCommand = TempRegistryKey.OpenSubKey("command", true);

                                object valueParam_1;
                                object valueParam_2;

                                bool ByDefault_Param = ParameterExist("", RKCommand, out valueParam_1);
                                bool DelegateExecute_Param = ParameterExist("DelegateExecute", RKCommand, out valueParam_2);

                                if (ByDefault_Param)
                                {
                                    if (this.command != (string)valueParam_1)
                                        RKCommand.SetValue("", this.command);
                                }
                                else if (DelegateExecute_Param)
                                {
                                    if (this.command != (string)valueParam_2)
                                        RKCommand.SetValue("DelegateExecute_Param", this.command);
                                }
                                else
                                    throw new Exception("Сommand not detected.");
                            }
                            else
                                throw new Exception("The 'command' subsection was not detected.");
                        }
                        else if (subsection.Length == 0)
                            throw new Exception("The 'command' subsection was not detected.");
                    }
                }

                //RegistryName
                {
                    if (this.registryName.Length == 0)
                        throw new MyProgramException("Имя в реестре не может быть пустым.", TypeException.NotFatal);

                    this.registryName = this.registryName.Trim();

                    Microsoft.Win32.RegistryKey tmpParent = this.ParentRegistryKey;
                    string tmpRKeyName = this.FullNameRegistryKey;

                    //получение имени ключа
                    for (int i = tmpRKeyName.Length - 1; i > 0; i--)
                        if (tmpRKeyName[i] == '\\')
                        {
                            tmpRKeyName = tmpRKeyName.Substring(i + 1, tmpRKeyName.Length - i - 1);
                            break;
                        }

                    if (this.registryName != tmpRKeyName)
                    {
                        string[] sub_names = tmpParent.GetSubKeyNames();
                        if (IsUniqueName(ref sub_names, this.registryName))
                        {
                            Microsoft.Win32.RegistryKey target = tmpParent.CreateSubKey(this.registryName);
                            CopyRegistryKey(this.RegistryKey, target);
                            tmpParent.DeleteSubKeyTree(tmpRKeyName);
                            this.FullNameRegistryKey = target.Name;
                        }
                        else
                            throw new Exception("Не уникльное имя.");
                    }
                }

                canSave = false;
            }
        }
        public void Reset()
        {
            ContextMenuKey tmp = new ContextMenuKey(this.RegistryKey);

            registryName    = tmp.registryName;
            muiVerb         = tmp.muiVerb;
            icon            = tmp.icon;
            command         = tmp.command;
            extended        = tmp.extended;
            position        = tmp.position;
            
            CanSave = false;
            OnPropertyChanged("Title");
        }
        public void DeleteKey()
        {
            this.ParentRegistryKey.DeleteSubKeyTree(this.RegistryName);
        }

        static private string EmptyCommand = ThisProgram.Path_EmptyCommand;

        static public ContextMenuKey CreateEmptyKey(ContextMenuType type)
        {
            RegistryKey parent = Registry.ClassesRoot;
            switch (type)
            {
                case ContextMenuType.DesktopBackground: parent = parent.OpenSubKey("DesktopBackground"); break;
                case ContextMenuType.FolderBackground: parent = parent.OpenSubKey("Folder"); break;
                case ContextMenuType.Directory: parent = parent.OpenSubKey("Directory"); break;
                case ContextMenuType.File: parent = parent.OpenSubKey("*"); break;

                default:
                    throw new Exception("Данный тип контекстного меню не поддерживается.");
            }

            if (parent == null)
                throw new Exception("Неверно указаный суб ключ.");

            parent = parent.OpenSubKey("shell", true);
            string name = GetUniqueName(parent, "Новый_ключ");
            RegistryKey RKey = parent.CreateSubKey(name);
            RKey.SetValue("MUIVerb", (object)name);

            Microsoft.Win32.RegistryKey command = RKey.CreateSubKey("command");
            command.SetValue("", EmptyCommand);

            return new ContextMenuKey(RKey);
        }
        static public ContextMenuKey CreateEmptyKey(ContextMenuKey key)
        {
            if (key.IsMenu == true)
            {
                string[] tmpsub = key.RegistryKey.GetSubKeyNames();

                if (tmpsub.Length != 1)
                    return null;
                else if (tmpsub[0] != "shell")
                    return null;


                RegistryKey parent = key.RegistryKey;
                parent = parent.OpenSubKey("shell", true);
                string name = GetUniqueName(parent, "Новый_ключ");
                RegistryKey RKey = parent.CreateSubKey(name);
                RKey.SetValue("MUIVerb", (object)name);

                Microsoft.Win32.RegistryKey command = RKey.CreateSubKey("command");
                command.SetValue("", EmptyCommand);

                return new ContextMenuKey(RKey);
            }

            return null;
        }

        static public ContextMenuKey CreateEmptyMenuKey(ContextMenuType type)
        {
            RegistryKey parent = Registry.ClassesRoot;
            switch (type)
            {
                case ContextMenuType.DesktopBackground: parent = parent.OpenSubKey("DesktopBackground"); break;
                case ContextMenuType.FolderBackground: parent = parent.OpenSubKey("Folder"); break;
                case ContextMenuType.Directory: parent = parent.OpenSubKey("Directory"); break;
                case ContextMenuType.File: parent = parent.OpenSubKey("*"); break;

                default:
                    throw new Exception("Данный тип контекстного меню не поддерживается.");
            }

            if (parent == null)
                throw new Exception("Неверно указаный суб ключ.");

            parent = parent.OpenSubKey("shell", true);
            string name = GetUniqueName(parent, "Новое_меню");
            RegistryKey RKey = parent.CreateSubKey(name);
            RKey.SetValue("MUIVerb", (object)name);
            RKey.SetValue("SubCommands", (object)"");

            RKey.CreateSubKey("shell");

            return new ContextMenuKey(RKey);
        }
        static public ContextMenuKey CreateEmptyMenuKey(ContextMenuKey key)
        {
            if (key.IsMenu == true)
            {
                string[] tmpsub = key.RegistryKey.GetSubKeyNames();

                if (tmpsub.Length != 1)
                    return null;
                else if (tmpsub[0] != "shell")
                    return null;


                RegistryKey parent = key.RegistryKey;
                parent = parent.OpenSubKey("shell", true);
                string name = GetUniqueName(parent, "Новое_меню");
                RegistryKey RKey = parent.CreateSubKey(name);
                RKey.SetValue("MUIVerb", (object)name);
                RKey.SetValue("SubCommands", (object)"");

                RKey.CreateSubKey("shell");

                return new ContextMenuKey(RKey);
            }

            return null;
        }

        //--------- доп. ---------//

        static private bool ParameterExist(string nameParam, RegistryKey target, out object valueParam)
        {
            string[] parameters = target.GetValueNames();
            foreach (string str in parameters)
            {
                if (str == nameParam)
                {
                    valueParam = target.GetValue(str);
                    return true;
                }
            }

            valueParam = null;
            return false;
        }
        static private void CopyRegistryKey(RegistryKey from, RegistryKey to)
        {
            string[] parameters = from.GetValueNames();
            for (int i = 0; i < parameters.Length; i++)
                to.SetValue(parameters[i], from.GetValue(parameters[i], null, RegistryValueOptions.None), from.GetValueKind(parameters[i]));

            string[] sub_names = from.GetSubKeyNames();
            if (sub_names.Length > 0)
            {
                foreach (string str in sub_names)
                {
                    RegistryKey newTarget = to.CreateSubKey(str);
                    CopyRegistryKey(from.OpenSubKey(str), newTarget);
                }
            }
        } //рекурсивный метод
        static private string RUSENG(string text)
        {
            string returnString = string.Empty;

            for (int i = 0; i < text.Length; i++)
            {
                char temp = Convert.ToChar(Convert.ToString(text[i]).ToLower());

                switch (temp)
                {
                    case ' ': returnString += "_"; break;
                    case 'а': returnString += "a"; break;
                    case 'б': returnString += "b"; break;
                    case 'в': returnString += "v"; break;
                    case 'г': returnString += "g"; break;
                    case 'д': returnString += "d"; break;
                    case 'е': returnString += "e"; break;
                    case 'ё': returnString += "e"; break;
                    case 'ж': returnString += "j"; break;
                    case 'з': returnString += "z"; break;
                    case 'и': returnString += "i"; break;
                    case 'й': returnString += "i"; break;
                    case 'к': returnString += "k"; break;
                    case 'л': returnString += "l"; break;
                    case 'м': returnString += "m"; break;
                    case 'н': returnString += "n"; break;
                    case 'о': returnString += "o"; break;
                    case 'п': returnString += "p"; break;
                    case 'р': returnString += "r"; break;
                    case 'с': returnString += "c"; break;
                    case 'т': returnString += "t"; break;
                    case 'у': returnString += "y"; break;
                    case 'ф': returnString += "f"; break;
                    case 'х': returnString += "x"; break;
                    case 'ц': returnString += "z"; break;
                    case 'ч': returnString += "ch"; break;
                    case 'ш': returnString += "sh"; break;
                    case 'щ': returnString += "sh"; break;
                    case 'ъ': returnString += ""; break;
                    case 'ы': returnString += "y"; break;
                    case 'ь': returnString += ""; break;
                    case 'э': returnString += "e"; break;
                    case 'ю': returnString += "yu"; break;
                    case 'я': returnString += "ya"; break;

                    case 'q':
                    case 'w':
                    case 'e':
                    case 'r':
                    case 't':
                    case 'y':
                    case 'u':
                    case 'i':
                    case 'o':
                    case 'p':
                    case 'a':
                    case 's':
                    case 'd':
                    case 'f':
                    case 'g':
                    case 'h':
                    case 'j':
                    case 'k':
                    case 'l':
                    case 'z':
                    case 'x':
                    case 'c':
                    case 'v':
                    case 'b':
                    case 'n':
                    case 'm': returnString += temp; break;

                    case '_':
                    case '!':
                    case '@':
                    case '#':
                    case '№':
                    case '"':
                    case '\'':
                    case '$':
                    case '%':
                    case '^':
                    case ':':
                    case ';':
                    case '=':
                    case '+':
                    case '-':
                    case ')':
                    case '(':
                    case '*':
                    case '\\':
                    case '/':
                    case '`':
                    case '~':
                    case '&':
                    case '?':
                    case '<':
                    case '>':
                    case '.':
                    case ',':
                    case '[':
                    case ']':
                    case '{':
                    case '}':
                    case '|': returnString += "_"; break;

                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9': returnString += temp; break;

                    default:
                        throw new Exception("возникла исключительная ситуация с символом: " + text[i]);
                }
            }

            return returnString;
        }
        static private bool IsUniqueName(ref string[] names, string name)
        {
            foreach (string nameKey in names)
            {
                if (nameKey == name)
                {
                    return false;
                }
            }

            return true;
        }
        static private string GetUniqueName(RegistryKey registry, string name)
        {
            string[] subNames = registry.GetSubKeyNames();

            int num = 1;
            string returnName = name;
            while (IsUniqueName(ref subNames, returnName) == false)
            {
                returnName = name + "_" + num.ToString();
                num++;
            }

            return returnName;
        }

        //--------- операторы ---------//

        static public bool StringOperator(ref string left, ref string right)
        {
            int minLength = left.Length > right.Length ? right.Length : left.Length;
            for (int i = 0; i < minLength; i++)
            {
                if (left[i] > right[i])
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return false; //неправильно
        }
        static public bool operator >(ContextMenuKey left, ContextMenuKey right)
        {
            if ((left.position == true ^ right.position == true) ||
                (left.position == null ^ right.position == null) ||
                (left.position == false ^ right.position == false))
            {
                if (left.position == true) return true;
                else if (left.position == false) return false;

                else //left.position == null
                    return !((bool)right.position);
            }
            else
                return StringOperator(ref left.registryName, ref right.registryName);
        }
        static public bool operator <(ContextMenuKey left, ContextMenuKey right)
        {
            if ((left.position == true ^ right.position == true) ||
                (left.position == null ^ right.position == null) ||
                (left.position == false ^ right.position == false))
            {
                if (left.position == true)          return false;
                else if (left.position == false)    return true;

                else //left.position == null
                    return ((bool)right.position);
            }
            else
                return StringOperator(ref right.registryName, ref left.registryName);
        }

        //--------- реализация интерфейса 'INotifyPropertyChanged' ---------//

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(prop)); }
        }
    }

    //частичное копирование функционала 'Windows Registry Editor Version 5.00'
    public class StoredContextMenuKey
    {
        public StoredContextMenuKey(string PathRegistryFile)
        {
            if (!File.Exists(PathRegistryFile))
                throw new Exception("The file does not exist.");


            this.PathRegistryFile = PathRegistryFile;
            this.CanRestore = true;


            FileInfo file = new FileInfo(PathRegistryFile);
            this.TimeKeySerialize = file.CreationTime;


            //получение строки из файла
            FileStream stream = file.OpenRead();
            byte[] arr = new byte[stream.Length];
            stream.Read(arr, 0, arr.Length);
            stream.Close();

            this.FileText = System.Text.Encoding.Unicode.GetString(arr);


            //получение первого ключа в файле
            Regex regex = new Regex("\\[(.*)\\]");
            Match match = regex.Match(this.FileText);
            string tempKey = match.Groups[1].Value;


            if (tempKey[0] == '-')
                this.CanRestore = false;


            //проверка на строчку ;This file was generated by the 'MySimpleCM' program.
            if (this.FileText.IndexOf(";This file was generated by the 'MySimpleCM' program.", 38) == -1)
                this.CanRestore = false;


            //получение имени ключа
            for (int i = tempKey.Length - 1; i > 0; i--)
                if (tempKey[i] == '\\')
                {
                    this.NameKey = tempKey.Substring(i + 1, tempKey.Length - i - 1);
                    break;
                }
        }


        public string NameKey { get; }
        public DateTime TimeKeySerialize { get; }

        private readonly string PathRegistryFile;
        private readonly string FileText;
        private readonly bool CanRestore;


        public ContextMenuKey FromStore()
        {
            if (!CanRestore) {
                throw new Exception("Ключ не может быть востановлен из файла: " + PathRegistryFile);
            }

            return Deserialize(FileText);
        }
        private ContextMenuKey Deserialize(string str)
        {
            RegistryKey TargetKey = null;

            Regex regexKey = new Regex("\\[(.*)\\]");
            MatchCollection regexKeyCollection = regexKey.Matches(str);
            if (regexKeyCollection.Count > 0)
            {
                //пробег по ключам
                for (int i = 0; i < regexKeyCollection.Count; i++)
                {
                    GroupCollection groupKey = regexKeyCollection[i].Groups;
                    string pathRegexKey = groupKey[1].Value;

                    //востановление ключа
                    RegistryKey registryKey = null;
                    {
                        string[] path = pathRegexKey.Split(new char[] { '\\' });

                        switch (path[0])
                        {
                            case "HKEY_CLASSES_ROOT": registryKey = Microsoft.Win32.Registry.ClassesRoot; break;
                            case "HKEY_CURRENT_USER": registryKey = Microsoft.Win32.Registry.CurrentUser; break;
                            case "HKEY_LOCAL_MACHINE": registryKey = Microsoft.Win32.Registry.LocalMachine; break;
                            case "HKEY_USERS": registryKey = Microsoft.Win32.Registry.Users; break;
                            case "HKEY_CURRENT_CONFIG": registryKey = Microsoft.Win32.Registry.CurrentConfig; break;
                        }

                        for (int iii = 1; iii < path.Length - 1; iii++)
                        {
                            registryKey = registryKey.OpenSubKey(path[iii], true);
                        }

                        registryKey = registryKey.CreateSubKey(path[path.Length - 1]);
                    }
                    if (i == 0)
                    {
                        TargetKey = registryKey;
                    }



                    int startindex = groupKey[0].Index;
                    int lastindex;
                    if (i == regexKeyCollection.Count - 1)
                        lastindex = str.Length;
                    else
                        lastindex = regexKeyCollection[i + 1].Groups[0].Index;

                    //пробег по параметрам
                    Regex regexKeyParam = new Regex("(?:(?:\"(.*)\"|(@))=\"(.*)\")");
                    MatchCollection regexKeyParamCollection = regexKeyParam.Matches(str.Substring(startindex, lastindex - startindex));
                    if (regexKeyParamCollection.Count > 0)
                    {
                        for (int ii = 0; ii < regexKeyParamCollection.Count; ii++)
                        {
                            GroupCollection groupKeyParam = regexKeyParamCollection[ii].Groups;

                            KeyParam param = new KeyParam
                            {
                                Name = groupKeyParam[2].Value == "@" ? "" : groupKeyParam[1].Value,
                                Value = groupKeyParam[3].Value
                            };

                            registryKey.SetValue(param.Name, param.Value);
                        }
                    }
                }
            }
            else
                throw new Exception("EmptyFile");

            if (TargetKey == null)
                throw new Exception("TargetKey is null.");

            return new ContextMenuKey(TargetKey);
        }

        static public void ToStore(ContextMenuKey key, string FolderPath, bool DeleteFromRegistry = true)
        {
            if (key == null) {
                throw new Exception("Ключ не существует.");
            }

            if (!Directory.Exists(FolderPath)) {
                throw new Exception("Директория для сериализации не существует.");
            }

            /*if (key.Disabled == true) {
                throw new Exception("Bad key.");
            }*/


            string headerRegFile = "Windows Registry Editor Version 5.00\r\n;This file was generated by the 'MySimpleCM' program.\r\n\r\n";

            //запись
            byte[] text = Encoding.Unicode.GetBytes(headerRegFile + Serialize(key.RegistryKey));
            string nameFile = key.GetHashCode().ToString();
            FileStream stream = File.Create(FolderPath + "\\" + nameFile + ".reg");
            stream.Write(text, 0, text.Length);
            stream.Close();

            if (DeleteFromRegistry)
                key.DeleteKey();
        }
        static private string Serialize(RegistryKey key)
        {
            string str = string.Empty;

            //шапка
            str += "[" + key.Name + "]\r\n";

            //параметры
            string[] parametrs = key.GetValueNames();
            for (int i = 0; i < parametrs.Length; i++)
                str += string.Format("{0}=\"{1}\"\r\n", parametrs[i] == "" ? "@" : "\"" + parametrs[i] + "\"", (string)key.GetValue(parametrs[i]));
            str += "\r\n";

            //подкатегории
            string[] subcategories = key.GetSubKeyNames();
            foreach (string category in subcategories)
                str += Serialize(key.OpenSubKey(category));

            return str;
        }
    }
}
