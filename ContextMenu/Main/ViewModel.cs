using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows;
using System.Collections.Specialized;
using System.Diagnostics;

namespace ContextMenu.Main
{
    class ViewModel :
        INotifyPropertyChanged
    {
        public int NumberBackground
        {
            get { return SettingsApp.Default.Background_int; }
            set
            {
                SettingsApp.Default.Background_int = value;
                SettingsApp.Default.Save();
                OnPropertyChanged("NumberBackground");
            }
        } //отвечает за градиент в левой панели программы
        public string HeightContextMenu
        {
            get
            {
                if (ContextMenuKeys == null)
                    return "0";
                else
                    return "Auto";
                
            }
        } //прячет некрасивую полоску в левой панели программы

        private ObservableCollection<ContextMenuKey> contextMenuKeys; //хранит ключи которые отображаются в левой панели в ListBox
        public ObservableCollection<ContextMenuKey> ContextMenuKeys
        {
            get { return contextMenuKeys; }
            set
            {
                contextMenuKeys = value;
                SortKeys();
                OnPropertyChanged("ContextMenuKeys");

                //Exception: нельзя изменять элементы во время OnPropertyChanged !!! (В обход данной ситуации метод SortKeys вызывается вручную в каждом блоке где происходит добавление)
                /*if (contextMenuKeys != null) { 
                    contextMenuKeys.CollectionChanged += delegate (object sender, NotifyCollectionChangedEventArgs e) {
                        if (e.Action == NotifyCollectionChangedAction.Add) SortKeys();
                    };
                }*/
            }
        }
        private void SortKeys()
        {
            if (contextMenuKeys != null)
            {
                //гномья сортировка по параметру Position
                {
                    int first = 1;
                    int second = 2;

                    while (first < contextMenuKeys.Count)
                    {
                        if (contextMenuKeys[first - 1] > contextMenuKeys[first])
                        {
                            first = second;
                            second += 1; 
                        }
                        else
                        {
                            contextMenuKeys.Move(first - 1, first);

                            first -= 1;
                            if (first == 0)
                            {
                                first = second;
                                second += 1;
                            }
                        }
                    }
                }

                
            }
        }



        private bool shift = true; //Режим расширеного списка контекстного меню
        public bool SHIFT
        {
            get { return shift; }
            set
            {
                if (shift ^ value)
                {
                    if (selectedKey != null ? selectedKey.CanSave : false)
                        MessageBox.Show("Переключение режима SHIFT не возможно, пока ключ не сохранен.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    else
                    {
                        shift = value;
                        OnPropertyChanged("SHIFT");

                        if (value == false)
                            ContextMenuKeys = model.RemoveExtendedKeys(ContextMenuKeys);
                        else
                            ContextMenuKeys = model.Read_RegistryDirectory(SelectedMenuType, true);
                    }
                }
            }
        }

        private ContextMenuType selectedMenuType = ContextMenuType.None;
        public ContextMenuType SelectedMenuType
        {
            get { return selectedMenuType; }
            set
            {
                selectedMenuType = value;
                OnPropertyChanged("SelectedType");

                ContextMenuKeys = model.Read_RegistryDirectory(value, SHIFT);
                OnPropertyChanged("HeightContextMenu");
            }
        }

        private ContextMenuKey selectedKey;
        public ContextMenuKey SelectedKey
        {
            get { return selectedKey; }
            set
            {
                if (value != null)
                {
                    if (value.Disabled == false)
                    {
                        if (selectedKey != null ? selectedKey.CanSave : false)
                        {
                            MessageBoxResult result = MessageBox.Show("Данные ключа не были сохранены. Сохранить ?\r\n(подсказка: используйте Ctrl + S)", "Question", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                            if (result == MessageBoxResult.Yes)
                            {
                                SaveKeyCommand.Execute(null);
                                selectedKey = value;
                                OnPropertyChanged("SelectedKey");
                                OnPropertyChanged("CanCommand");
                                OnPropertyChanged("VisibleParamsKey");
                            }
                            else if (result == MessageBoxResult.No)
                            {
                                if (ResetKeyCommand.CanExecute(null)) ResetKeyCommand.Execute(null); 
                                selectedKey = value;
                                OnPropertyChanged("SelectedKey");
                                OnPropertyChanged("CanCommand");
                                OnPropertyChanged("VisibleParamsKey");
                            }
                        }
                        else
                        {
                            selectedKey = value;
                            OnPropertyChanged("SelectedKey");
                            OnPropertyChanged("CanCommand");
                            OnPropertyChanged("VisibleParamsKey");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Данный ключ запрещено редактировать!", "ограничитель", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    selectedKey = value;
                    OnPropertyChanged("SelectedKey");
                    OnPropertyChanged("CanCommand");
                    OnPropertyChanged("VisibleParamsKey");
                }
            }
        }

        //--------- доп логика ---------//

        public bool VisibleParamsKey
        {
            get
            {
                return SelectedKey == null ? true : false;
            }
        } //Скрывает область параметров ключа (биндится в View)
        public bool CanCommand
        {
            get
            {
                if (SelectedKey != null)
                    return SelectedKey.IsMenu == false ? true : false;
                else
                    return true;
            }
        } //сообщает View может ли редактироваться поле Command

        private List<string> LocalPathRegistry = new List<string>(); //локальная логика для перехода по меню. ВНИМАНИЕ!!!: получилось как то костыльно ! В EscCommand (тоесть в вьюмодел) есть логика работы с данными !!!!
        public bool CanUp
        {
            get
            {
                return LocalPathRegistry.Count == 0 ? false : true;
            }
        }

        //--------- конструтор и модель ---------//

        private ContextMenu.Main.Model model;
        public ViewModel()
        {
            model = new ContextMenu.Main.Model();
            ThisProgram.ProgramException += (Exception ex) =>
            {
                CustomExceptionHandler customExceptionHandler = new CustomExceptionHandler(ex);
                customExceptionHandler.ShowDialog();
            };
            ThisProgram.IsExistsEnvironment();
        }

        //--------- команды типа контекстного меню ---------//

        private RelayCommand use_DesktopBackground;
        public RelayCommand Use_DesktopBackground
        {
            get
            {
                return use_DesktopBackground ?? (use_DesktopBackground = new RelayCommand(obj => {
                    this.SelectedMenuType = ContextMenuType.DesktopBackground;
                }));
            }
        }

        private RelayCommand use_FolderBackground;
        public RelayCommand Use_FolderBackground
        {
            get
            {
                return use_FolderBackground ?? (use_FolderBackground = new RelayCommand(obj => {
                    this.SelectedMenuType = ContextMenuType.FolderBackground;
                }));
            }
        }

        private RelayCommand use_Directory;
        public RelayCommand Use_Directory
        {
            get
            {
                return use_Directory ?? (use_Directory = new RelayCommand(obj => {
                    this.SelectedMenuType = ContextMenuType.Directory;
                }));
            }
        }

        private RelayCommand use_File;
        public RelayCommand Use_File
        {
            get
            {
                return use_File ?? (use_File = new RelayCommand(obj => {
                    this.SelectedMenuType = ContextMenuType.File;
                }));
            }
        }

        //--------- команды ---------//

        private RelayCommand createKeyCommand;
        public RelayCommand CreateKeyCommand
        {
            get
            {
                return createKeyCommand ?? (createKeyCommand = new RelayCommand(obj => {

                    ContextMenuKey newKey = null;
                    if (CanUp)
                    {
                        ContextMenuKey tmpkey = new ContextMenuKey(model.GetRegistryKey(LocalPathRegistry[LocalPathRegistry.Count - 1]));
                        if (tmpkey.IsMenu == null)
                        {
                            MessageBox.Show("По некоторым тех причинам нельзя! создать ключ");
                        }
                        else
                        {
                            newKey = model.CreateKey(tmpkey);
                        }
                    }                        
                    else
                        newKey = model.CreateKey(SelectedMenuType);

                    if (newKey != null)
                    {
                        ContextMenuKeys.Add(newKey);
                        SortKeys();
                        SelectedKey = newKey;
                    }

                }, obj => SelectedKey == null && selectedMenuType != ContextMenuType.None));
            }
        }

        private RelayCommand createMenuCommand;
        public RelayCommand CreateMenuCommand
        {
            get
            {
                return createMenuCommand ?? (createMenuCommand = new RelayCommand(obj => {

                    ContextMenuKey newKey = null;
                    if (CanUp)
                    {
                        ContextMenuKey tmpkey = new ContextMenuKey(model.GetRegistryKey(LocalPathRegistry[LocalPathRegistry.Count - 1]));
                        if (tmpkey.IsMenu == null)
                        {
                            MessageBox.Show("По некоторым тех причинам нельзя! создать меню");
                        }
                        else
                        {
                            newKey = model.CreateMenuKey(tmpkey);
                        }
                    }
                    else
                        newKey = model.CreateMenuKey(SelectedMenuType);


                    if (newKey != null)
                    {
                        ContextMenuKeys.Add(newKey);
                        SortKeys();
                        SelectedKey = newKey;
                    }

                }, obj => SelectedMenuType != ContextMenuType.None && SelectedKey == null));
            }
        }

        private RelayCommand openMenuKey;
        public RelayCommand OpenMenuKey
        {
            get
            {
                return openMenuKey ?? (openMenuKey = new RelayCommand(obj => {
                    if (SelectedKey != null)
                    {
                        if (SelectedKey.IsMenu == true)
                        {
                            LocalPathRegistry.Add(selectedKey.FullNameRegistryKey);
                            ContextMenuKeys = model.ReadMenu(SelectedKey, SHIFT);
                        }
                        else if (selectedKey.IsMenu == null)
                        {
                            MessageBox.Show("Данное меню нельзя редактировать тк. разработчик сказал НЕТ.\r\nНажмите 'Yes' что бы выразить недовольство.", "", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                        }
                    }
                }, obj => { if (SelectedKey == null) { return false; } else { return selectedKey.IsMenu != false ? true : false; } }));
            }
        }

        private RelayCommand saveKeyCommand;
        public RelayCommand SaveKeyCommand
        {
            get
            {
                return saveKeyCommand ?? (saveKeyCommand = new RelayCommand(obj => {

                    Model.SaveKey(ref selectedKey);
                    if (SelectedKey.Extended ^ SHIFT)
                    {
                        if (SHIFT == false)
                        {
                            ContextMenuKeys.Remove(SelectedKey);
                            SelectedKey = null;
                        }
                    }

                }, obj => { if (SelectedKey == null) { return false; } else { return SelectedKey.CanSave; } }));
            }
        }

        private RelayCommand escCommand;
        public RelayCommand EscCommand
        {
            get
            {
                return escCommand ?? (escCommand = new RelayCommand(obj => {

                    if (SelectedMenuType != ContextMenuType.None)
                    {
                        if (SelectedKey != null)
                        {
                            if (SelectedKey.CanSave)
                            {
                                MessageBoxResult result = MessageBox.Show("Данные ключа не были сохранены. Сохранить ?\r\n(подсказка: используйте Ctrl + S)", "Question", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                                if (result == MessageBoxResult.Yes)
                                {
                                    SaveKeyCommand.Execute(null);
                                    SelectedKey = null;
                                }
                                else if (result == MessageBoxResult.No)
                                {
                                    Model.ResetKey(ref selectedKey);
                                    SelectedKey = null;
                                }
                                    
                            }
                            else
                                SelectedKey = null;
                        }
                        else
                        {
                            if (CanUp)
                            {
                                if (LocalPathRegistry.Count == 1)
                                {
                                    string[] path = LocalPathRegistry[LocalPathRegistry.Count - 1].Split(new char[] { '\\' });
                                    switch (path[1])
                                    {
                                        case "DesktopBackground": ContextMenuKeys = model.Read_RegistryDirectory(ContextMenuType.DesktopBackground, this.shift); break;
                                        case "Folder": ContextMenuKeys = model.Read_RegistryDirectory(ContextMenuType.FolderBackground, this.shift); break;
                                        case "Directory": ContextMenuKeys = model.Read_RegistryDirectory(ContextMenuType.Directory, this.shift); break;
                                        case "*": ContextMenuKeys = model.Read_RegistryDirectory(ContextMenuType.File, this.shift); break;

                                        default:
                                            throw new Exception("Данный тип контекстного меню не поддерживается.");
                                    }
                                }
                                else
                                {
                                    ContextMenuKeys = model.Read_RegistryDirectory(model.GetParentRegistryKey(model.GetRegistryKey(LocalPathRegistry[LocalPathRegistry.Count - 1])), this.shift);
                                }

                                LocalPathRegistry.RemoveAt(LocalPathRegistry.Count - 1);
                            }
                            else
                            {
                                SelectedMenuType = ContextMenuType.None;
                            }
                        }
                    }
                }
                , obj => selectedMenuType != ContextMenuType.None));
            }
        }

        private RelayCommand shiftCommand;
        public RelayCommand SHIFTCommand
        {
            get
            {
                return shiftCommand ?? (shiftCommand = new RelayCommand(obj => {
                    SHIFT = !SHIFT;
                }));
            }
        }

        private RelayCommand deleteKeyCommand;
        public RelayCommand DeleteKeyCommand
        {
            get
            {
                return deleteKeyCommand ?? (deleteKeyCommand = new RelayCommand(obj => {

                    if (
                        MessageBox.Show("Вы уверены что хотите удалить ключ ?",
                                        "Question", MessageBoxButton.YesNo, MessageBoxImage.Question)
                        == MessageBoxResult.Yes)
                    {
                        ContextMenu.Main.Model.DeleteKey(SelectedKey);
                        ContextMenuKeys.Remove(SelectedKey);
                        SelectedKey = null;
                    }
                }
                , obj => SelectedKey != null));
            }
        }

        private RelayCommand openWindowDeletedKeys;
        public RelayCommand OpenWindowDeletedKeys
        {
            get
            {
                return openWindowDeletedKeys ?? (openWindowDeletedKeys = new RelayCommand(obj => {
                    DeletedKeys deletedKeys = new DeletedKeys(ThisProgram.Path_RemoveKeys);
                    if (deletedKeys.ShowDialog() == true)
                    {
                        if (ContextMenuKeys == null)
                            ContextMenu.Main.Model.RestoreKey(deletedKeys.RestoreKey);
                        else
                            ContextMenuKeys.Add(ContextMenu.Main.Model.RestoreKey(deletedKeys.RestoreKey));
                    }
                }));
            }
        }

        private RelayCommand openInRegedit;
        public RelayCommand OpenInRegedit
        {
            get
            {
                return openInRegedit ?? (openInRegedit = new RelayCommand(obj => {

                    if (MessageBox.Show("Внимание, внесение изменений в реестр может привести к крушению вашей windows. Открыть реестр ?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        model.OpenRegedit(SelectedKey);
                    }
                    
                }, obj => this.SelectedKey != null));
            }
        }

        private RelayCommand resetKeyCommand;
        public RelayCommand ResetKeyCommand
        {
            get
            {
                return resetKeyCommand ?? (resetKeyCommand = new RelayCommand(obj => {
                    Model.ResetKey(ref selectedKey);
                }, obj => selectedKey == null ? false : selectedKey.CanSave));
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