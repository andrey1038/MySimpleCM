   M S B A M L   `   `   `  ���� F  BContextMenu, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:ContextMenuContextMenu  O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem     ContextMenu.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     +localclr-namespace:ContextMenu     6sys&clr-namespace:System;assembly=mscorlib     -     � Title$  
MySimpleCM��$	��650��$
��1200�� � WindowStartupLocation$ CenterScreen=� � 	Resources *  $ContextMenu.RadioButtonBoolConverter   RadioButtonBoolConverter+  %ContextMenu.ExpanderMultiplyConverter  ExpanderMultiplyConverter  ExpanderStyle2 �� 
TargetType �� Property �� Value �� 
TargetType ��Name
 � Tag	 `� 	Converter
 �� Path �� ElementName �� RelativeSource ��Name �� Triggers P� Property P� Value �� 
TargetName N� EnterActions �� 
TargetName �� TargetProperty	 W� To g� Duration N� ExitActions  StyleTextBlockNameParam  StyleTextBoxParam  TypeRegButton ��Name��%�  &	        &	      &	 
     7  &	 �    &	 �    &	 �        �� " 9�.���  � ��$ Stretch=���  � ��$ Stretch=���  � �� �� " 9�.R�]� .+��� $ ExpanderScrollViewer��$��Hidden=�$��Hidden=�$��Stretch=�$��Bottom=�$	��Top=� X�0.0Q���`� 8	   .1��� $
 ActualHeight�$ ExpanderContent���� $	
 Tag� �� *Self+.���� $ ExpanderContentPresenter��$��Content�� P�  �@9�
IsExpanded . . ��� $ ExpanderContentPresenter�� � y�$ Visible=� �� .���� .��W� $ ExpanderScrollViewer��$	 Tag�$	 500X�$ 0:0:1D� �� .���� .��W� $ ExpanderScrollViewer��$	 Tag�$ 0X�$ 0:0:0.5D��� " ��.���  � ��$ 18(��� " ��.���  � ��$ 0 0 0 1q���  � ��
 �   ���  � ��$ 18(��� " ��.���  � �� �� " ��.R��� ._�� .+��� $ mytext��.���  � ColumnDefinitions �� $��1* ��� $
��Auto ��� $��2* �$	��740��.+�� $��0������ $ LeftBackraundBrushList��$
 SelectedValue�.+��� $	��Top=�$
��Left=�$��5q�$��16(�
��� ���$(��"Режим SHIFT отключен.������ " �� �� Triggers ��  �� Binding �� *SHIFT+ �� Value True.a���  � y�$ Hidden=���  �� *SHIFT+	 False.a���  � y�$ Visible=��� $��Center=�$��Center=�$	��260��$	��0 5q����#f2f2f2���� *ContextMenuKeys+ �� SelectedItem �� *SelectedKey+���� *HeightContextMenu+���� " ��.���  � ��$ 0 0 0 0q���  � �� �� " ��.R��  �Name$ WrapperTemplate��$��24�� �� $��30 ��� $��* ��� $
��Auto �.+��� $ TitleCMK��$��1��$��Center=����� $
 Title��  �Name$ 	IsMenuKey��$��2��$y�	Collapsed=�$��Center=�$��0 1 7 0q�  � Points  �   ��[ ��[  � 
���   �$��1�� �� $
 True�� �� $
 IsMenu�.a��� $ 	IsMenuKey�� � y�$ Visible=��� $
 Null�� �� $
 IsMenu�.a��� $ 	IsMenuKey�� � y�$ Visible=��� $
 True�� �� $
 Disabled�.a��� $ TitleCMK�� � ��
 �����P�  �@J�IsMouseOver . . ��� $ WrapperTemplate�� � �� �#d9d9d9P�  �@��
IsSelected . . ��� $ WrapperTemplate�� � �� �#d9d9d9�� $��Center=�$��Stretch=�$��4��
�������$��1���� $��2��$��10q�$	��450������ $ RightBackraundBrushList��$
 SelectedValue�.��� $	��700��! � RowDefinitions! �� $
��Auto ��� $��50 ��� $
��Auto ��� $��50 ��� $
��Auto ��� $��50 ��� $
��Auto ��� $��50 ��� $
��Auto ��� $��50 ��� $
��Auto �.+�� $��0��! �� �� .+��� $��Right=�$��0 0 0 10q�$��0��  HeaderTextBlock#��[ $��О программе����   StandartText#��[ $��1��$��	150 0 0 0q�$	��500��" �� TextAlignment$" Right=�$��Right=�$
��Wrap=���MySimpleCM - это программа редактирования элементов контекстного меню, в ОС Widows10. Функционал программы не большой, по этому если вы программист и у вас есть предложения по улучшению программы пишите на почту "andrey.akulinichev@mail.ru" или в VK id280074340. Git ""� $��2��! �� �� �� .+��� $
��Left=�$��0��$��0 0 0 10q�#��[ $��Внешний вид���� $��1��# �� Orientation$# 
Horizontal=�.+��� $��16(�$
��Bold"�Left Background:�� -   $ ��Name$$ LeftBackraundBrushList��
������ $��0q�% �� SelectedIndex% �� *NumberBackground+@��� & �� Color$& Black���� .��� ' � Color$' #232526��( � Offset$( 0Q�� $' 	#FF4B4D4F��$( 1Q��� .��� $' #FF0099��$( 0Q�� $' #493240��$( 1Q��� .��� $' #8A2387��$( 0Q�� $' #E94057��$	( 0.7Q�� $' #F27121��$( 1Q��� .��� $' #fc4a1a��$( 0Q�� $' #f7b733��$( 1Q��� .��� $' #22c1c3��$( 0Q�� $' #fdbb2d��$( 1Q��� .��� $' 	#FFFF00FC��$( 0Q�� $' 	#FFFF669D��$( 1Q��� .��� $' #ff00cc��$( 0Q�� $' #333399��$( 1Q��� .��� $' #fc00ff��$( 0Q�� $' #00dbde��$( 1Q��� .��� $' #1FA2FF��$( 0Q�� $' #12D8FA��$	( 0.5Q�� $' #A5FECB��$( 1Q����� " ��.���  � �� �� " ��.R��� $��15��$��15��$	��1.5q�
���   �$	��2 0q�) �� CornerRadius$) 10������ ���� .R��� $# 
Horizontal=�� $��4��! �� �� .+��� $��Right=�$��0��$��0 0 0 10q�#��[ $.��(Тип контекстного меню���� $��1��$��Right=�$	��400��.+��� ���� *Use_DesktopBackground+���� .R��� * ��Name$* TypeContextMenu_Border��+ �� Padding$+ 6q�$��1q�
������ ._�� ! �� $
��Auto ��� $��* �.+��� $��0��$��18(�$��Right=�$
��Wrap=�$��DesktopBackground���� $��1��$��16(�$��Right=�$" Right=�$
��Wrap=�$����Этот раздел отвечает за элементы появляющиеся при вызове контекстного меню на рабочем столе.�� P�  �@J�IsMouseOver . . ��� $ TypeContextMenu_Border�� � �� �	#FFE7E7E7�� ���� *Use_FolderBackground+���� .R��� $* TypeContextMenu_Border��$+ 6q�$��1q�
������ ._�� ! �� $
��Auto ��� $��* �.+��� $��0��$��18(�$��Right=�$
��Wrap=�$��FolderBackground���� $��1��$��16(�$��Right=�$" Right=�$
��Wrap=�$����Элементы контекстного меню данного раздела можно вызвать правой кнопкой в пустом месте любой папки.�� P�  �@J�IsMouseOver . . ��� $ TypeContextMenu_Border�� � �� �	#FFE7E7E7�� ���� *Use_Directory+���� .R��� $* TypeContextMenu_Border��$+ 6q�$��1q�
������ ._�� ! �� $
��Auto ��� $��* �.+��� $��0��$��18(�$��Right=�$
��Wrap=�$��	Directory���� $��1��$��16(�$��Right=�$" Right=�$
��Wrap=�$T��NЭлементы контекстного меню для всех папок.�� P�  �@J�IsMouseOver . . ��� $ TypeContextMenu_Border�� � �� �	#FFE7E7E7�� ���� *
Use_File+���� .R��� $* TypeContextMenu_Border��$+ 6q�$��1q�
������ ._�� ! �� $
��Auto ��� $��* �.+��� $��0��$��18(�$��Right=�$
��Wrap=�$
��File���� $��1��$��16(�$��Right=�$" Right=�$
��Wrap=�$V��PЭлементы контекстного меню для всех файлов.�� P�  �@J�IsMouseOver . . ��� $ TypeContextMenu_Border�� � �� �	#FFE7E7E7� $��6��! �� �� .+��� $
��Left=�$��0��$��0 0 0 10q�#��[ $��Клавиша SHIFT���� $
��Left=�$��1��$	��400��.+��� $��16(�$
��Left=�$
" Left=�$
��Wrap=�$����Оказывается можно увеличить количество функций в контекстном меню Windows простой кнопкой Shift. Сочетание клавиш Shift + RMB открывает расширеный список команд контекстного меню.���� $# 
Horizontal=�$��0 7 0 0q�.+��� $��клавиша: ��$��0 0 5 0q�$��16(�$��Center=��� , d� 	IsChecked, �� *SHIFT+���� .R��� 
������ $��60��$��25��$��2q�
���   �$) 3��._��� $��Center=�$��Center=�$
��Bold"�$��SHIFT������ " ��.���  �@J�BitmapEffect F� - F� Opacity$
- 0.35Q�. F� ShadowDepth$. 10Q� P�  �@d�	IsChecked$
 TrueY�. ���  �@J�BitmapEffect F� $	- 0.5Q�$. 5Q��� $��15 0 0 0q�$��16(�$��Center=����� " �� ��  �� *SHIFT+ True.a���  � ��$ Активная����  �� *SHIFT+	 False.a���  � ��$ Неактивная��� $��8��! �� �� $��20 ��� �� $��20 ��� .+��� $��Center=�$��0 0 0 10q�$��0��#��[ $��Редактор��� $��2�� �� $
��Auto ��� $��15 ��� $��4* ��� $��15 ��� $��4* ��� $��15 ��� $��2* �! �� $
��Auto ��� $
��Auto �.+���   StyleButton#��[ $(��"создать новый ключ��$��50��$��0��$��2������ *CreateKeyCommand+�� #��[ $��удалить ключ��$��50��$��0��$��4������ *DeleteKeyCommand+�� #��[ $	��esc��$��50��$��50��$��0��$��0������ *
EscCommand+�� #��[ $��сохранить��$��50��$��0��$��6������ *SaveKeyCommand+�� #��[ $��открыть��$��50��$��1��$��6������ *OpenMenuKey+�� #��[ $#��Ранее удаленные��$��50��$��1��$��4������ *OpenWindowDeletedKeys+�� #��[ $��Создать меню��$��50��$��1��$��2������ *CreateMenuCommand+�� #��[ $
��Reg.��$��50��$��50��$��1��$��0������ *OpenInRegedit+� $��4��
�������/ K� ZIndex$/ 1��.+��� 
���   �$��2q�$) 10��._��� $��Center=�$��Center=�#��[ $" Center=�75Выберите ключ в левой панели,�� CAчто бы редактировать его параметры.���� " � ��  �� *VisibleParamsKey+ True.a���  � y�$ Visible=���  �� *VisibleParamsKey+	 False.a���  � y�$ Hidden=�� $��4��0 � DataContext0 �� *SelectedKey+ �� $
��Auto ��� $��15 ��� $��* �! �� $
��Auto ��� $��10 ��� $
��Auto ��� $��10 ��� $
��Auto ��� $��30 ��� $
��Auto ��� $��30 ��� $
��Auto �.+��� #��[ $��0��$��0��$��Имя в реестре���� -   1 ��Name$	1 TB1��#��[ $��2��$��0������ $
 RegistryName�2 �� UpdateSourceTrigger$2 PropertyChanged=��� #��[ $��0��$��2��$��Видимое имя���� -   $	1 TB2��#��[ $��2��$��2������ $
 MUIVerb�$2 PropertyChanged=��� #��[ $��0��$��4��$��Команда���� -   $	1 TB3��$��2��$��4������ $
 Command�$2 PropertyChanged=�.������ " ��.���  � ��$ 0 0 0 1q���  � ��
 �   ���  � ��$ 18(� ��  �� *
CanCommand+ True.a���  � {� . ��  �� *
CanCommand+	 False.a���  � {� .  .���� #��[ $��0��$��6��$��Позиция���� $��2��$��6��.+�	� $	��Top��3 	� 	GroupName$3 Radio_1��, �� $
 Position�4 �� 	Converter#4 [  5 �� ConverterParameter$5 1��	� $��Middle (default)��$3 Radio_1��, �� $
 Position�#4 [  $5 -1��	� $��Bottom��$3 Radio_1��, �� $
 Position�#4 [  $5 0���� #��[ $��0��$��8��$��Shift���� $��2��$��8��, �� $
 Extended�