   M S B A M L   `   `   `  ���� F  BContextMenu, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:ContextMenuContextMenu      ContextMenu.DeletedKeys   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     +localclr-namespace:ContextMenu       � Title$  DeletedKeys��$	��450��$	��800��  ContextMenu.DialogCloser   DialogResult �� *DialogResult+ � 	Resources #  ContextMenu.DateTimeConverter   DateTimeConverter��%   &	          .���  � RowDefinitions �� $��* ��� $
��Auto �.+�� 
������� K� ZIndex$ 1��$��2��.+��� $��Center=�$��Center=�.+��� $��Icon\trash_cat.png��$��90��$��90���� $��1��$��10q�$
��Wrap=� �� TextAlignment$ Center=�$��Center=�$��Center=�$
��Bold"�$��16(�GEЗдесь хранятся ранее удаленные ключи.�� .,Сейчас здесь нет ключей.����  �� 
TargetType" � �� Triggers ��  �� Binding �� *IsEmptyKeys+	 �� Value	 True.a��� 
 �� Property
 � y� �� Value$ Visible=���  �� *IsEmptyKeys+		 False.a��� 
 � y�$ Hidden=��� $��0��$��
10 10 10 0q� �� SelectedItem �� *SelectedKey+���� *ContextMenuKeys+���� " ��.��� 
 � ��$ 0q����� " ��.��� 
 � �� ��  �� 
TargetType" ��.R��  �Name$ WrapperTemplate��
�������.+��� $��16(�$��	15 5 15 5q�$
��Left=�����  �� Path$ NameKey��� $��16(�$��	15 5 15 5q�$��Center=�$��Right=����� $ TimeKeySerialize� �� 	Converter# [   �� ConverterParameter$ F�� �� Triggers P�  P� Property �@J�IsMouseOver P� Value . . ���  �� 
TargetName$ WrapperTemplate��
 � �� �#d9d9d9P�  �@��
IsSelected . . ��� $ WrapperTemplate��
 � �� �#d9d9d9� $��1��$��	20 0 20 0q� � ColumnDefinitions �� $	��100 ��� $��* �.+��� $��0��$��очистить все��  StyleButton#��[ $��35������ *
ClearAll+�� #��[ $��
20 10 0 15q�$��35��$��1��$%��востановить ключ������ *RestoreKeyCommand+