   M S B A M L   `   `   `  ����E  AStudy_Step, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:Study_Step.UI.WindowsStudy_Step.UI.Windows  5   	   *    $Study_Step.UI.Windows.DeletionWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   5local#clr-namespace:Study_Step.UI.Windows     5   	   -   5          �Name$  deletedwindow��5   	    � Title$ Удаление��5	   	   $	��300��5
   	   $	��150��6    � WindowStartupLocation$ CenterOwner=�5   	    � 
ResizeMode$ NoResize=�5   	    � WindowStyle$
 None=�5   	    � AllowsTransparency$
 True��5   	   
������ $��10(�5   	   .��5      ��  �� CornerRadius$ 10��6   
�������
���   �$��1q�5      ._�5   
   �  � RowDefinitions 5      �� 5      $
��auto �6    �� 5      .+�5      �� $ 10, 10, 0, 0��6   ���#49a078._�5      � .+�5      �� $
��Left=�5       $
��15,0q�5!      $��Center=�5"      
�������$��22(�5$      $
��Bold"�5%      ��5       �� *Title+ �� ElementName$ deletedwindow��5%      5&      5'      � 5(      $��10, 0, 10, 10q�6   $��1��5)       5*      �� 5+      $��* �6$   �� 5,      $
��Auto �6$   .+�5/      �� 	 �� Orientation$	 Vertical=�6   $
��8, 0q�50      $��0��51      .+�52      �� $'��!Удалить сообщение��6    $
��Left=�53       $��Center=�54       $
��Wrap=�55       $��SemiBold"�56       $��16(�57       �� 58      $U��OВы действительно хотите удалить сообщение?��6    $
��Left=�59       $��Center=�5:       $
��Wrap=�5;       $��12(�5<       �� 5=      $��Center=�5>      $��20��5?      
 d� 	IsChecked
 5=      �� *IsDeleteForAll+ �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=���5@      �� 5A      $$��Удалить для всех��6(   $
��Left=�5B   (   $��Center=�5C   (   $��10(�5D   (   5F      5G      �� 5I      $��1��6   $	 
Horizontal=�6*   $��Center=�6C   .+�5J      �� $��Удалить��6   $��80��6/   $��0,0,40,0q�6:   ��5K      �� *DeleteMessageCommand+�� 5L      $��Отмена��6   $��80��6.   ��5M      �� *CloseDeletionWindowCommand+5N      5O      5P      5Q      5R      