   M S B A M L   `   `   `  ����F  BContextMenu, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:ContextMenuContextMenu  5   	       ContextMenu.DeletedKeys   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   +localclr-namespace:ContextMenu     5   	     � Title$  DeletedKeys��5   	   $	��450��6   $	��800��6*     ContextMenu.DialogCloser   DialogResult 5	   	   �� *DialogResult+ � 	Resources 5      #  ContextMenu.DateTimeConverter   DateTimeConverter��%   &	          5   
   6"   5      .��5      �  � RowDefinitions 5   
   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   � 
������� K� ZIndex$ 1��5      $��2��5      .+�5      �� $��Center=�6   $��Center=�5      .+�5       �� $��Icon\trash_cat.png��6   $��90��5!      $��90��6    �� 5#      $��1��5$      $��10q�5%      $
��Wrap=�5&       �� TextAlignment$ Center=�5'      $��Center=�5(      $��Center=�5)      $
��Bold"�5*      $��16(�5+      GEЗдесь хранятся ранее удаленные ключи.�� 5-   <   .,Сейчас здесь нет ключей.50      52      ��54      �� 55       �� 
TargetType" �6    �� Triggers 56      �� 57       �� Binding 6&   �� *IsEmptyKeys+	 �� Value	 True6F   .a�58      �� 
 �� Property
 � y� �� Value$ Visible=�6@   59      �� 5:       6&   �� *IsEmptyKeys+		 False6F   .a�5;      �� 
 � y�$ Hidden=�6@   5<      5>      5@      �� 5B   
   $��0��6   $��
10 10 10 0q�6    �� SelectedItem 5C      �� *SelectedKey+��5D      �� *ContextMenuKeys+��5E      �� 5F      " ��6   .�5G      �� 
 � ��$ 0q�68   5H      ��5J      �� 5K      " ��6   .�5L      �� 
 � �� 5M      �� 5N       �� 
TargetType" ��6.   .R�5O   "   �  �Name$ WrapperTemplate��6'   
�������.+�5P   &   �� $��16(�5Q   0   $��	15 5 15 5q�6>   $
��Left=�5R   0   ��5P   0   ��  �� Path$ NameKey�5R   0   �� 5S   &   $��16(�5T   0   $��	15 5 15 5q�6>   $��Center=�5U   0   $��Right=�5V   0   ��5S   0   �� $ TimeKeySerialize� �� 	Converter# [   �� ConverterParameter$ F��5V   0   5W   #    �� Triggers 5X   "   P� 5Y   &    P� Property �@J�IsMouseOver P� Value . . �5Z   *   ��  �� 
TargetName$ WrapperTemplate��61   
 � �� �#d9d9d96d   5[   '   P� 5\   &    �@��
IsSelected . . �5]   *   �� $ WrapperTemplate��61   
 � �� �#d9d9d96d   5^   '   5`      5b      5c      5e      � 5g   
   $��1��6   $��	20 0 20 0q�6    � ColumnDefinitions 5h      �� 5i      $	��100 �6#   �� 5j      $��* �6#   .+�5m      �� $��0��6   $��очистить все��6%     StyleButton#��[ 5n      $��35��5p      ��5o      �� *
ClearAll+5p      �� 5r      #��[ 5s      $��
20 10 0 15q�5t      $��35��5u      $��1��5v      $%��востановить ключ��6!   ��5r      �� *RestoreKeyCommand+5v   !   5x      5z      5{      