   M S B A M L   `   `   `  ����E  AStudy_Step, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullE'clr-namespace:Study_Step.CustomControlsStudy_Step.CustomControls  5      =#clr-namespace:Study_Step.ViewModelsStudy_Step.ViewModels  5      9!clr-namespace:Study_Step.CommandsStudy_Step.Commands  5      2    ,Study_Step.CustomControls.DropDownListSearch   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      9local'clr-namespace:Study_Step.CustomControls     5      :
viewmodels#clr-namespace:Study_Step.ViewModels     5      6commands!clr-namespace:Study_Step.Commands     5      $	��406��5
      .��5      ��   �� Orientation$  Vertical=�6   .+�5   
   �� -   5         ��Name$ 
SearchUser��5      $	��400��5       �� 
IsEditable$
 True��5       �� IsTextSearchEnabled$
 True��5      
������ 
������ $��0q�5      ��5      �� *
UserList+��5      �� 5       �� 
TargetType" ��6"   .R�5      �  � ColumnDefinitions 5      �� 5      �� 5      $
��auto �6/   .+�5      d�  d�Name$ ToggleButton��6'   $��50��5   '   $��50��63   $��1��6>   $|�False��5    '    �� 	ClickMode$ Press=�5"   '   $��Right=�5#   '   $��Center=�5$   '    
  search#��[  5%   '     ToggleIconButtonStyle#��[ 5&   '    d� 	IsChecked 5!   '   �� *IsDropDownOpen+	 �� Mode$	 TwoWay=�
 �� RelativeSource
 �� *TemplatedParent+5'      �� 5*       ��Name$ TextBox��6"   $��50��5+   "   $��0��6.   $��10, 0q�6>   $��Stretch=�5,   "   $��Center=�5-   "   
������ 
������ $��0q�50   "   
���   ���52   "   �� *Text+$	 TwoWay=�
 �� *TemplatedParent+� 55       �Name$ 
PART_Popup��6    $��Bottom=�57       $	��400��58       $|�False��59        � AllowsTransparency$
 True��5:       $��Slide=�5;       ��56       �� *IsDropDownOpen+$	 TwoWay=�
 �� *TemplatedParent+.��5<      ��  ��Name$ DropDownBorder��6%   
�������
�������$��1q�5?   %    �� CornerRadius$ 15��5@   %   ._�5A   "   ��  ��Name$ ListBox��6*   $��5q�6;   #��z��5B   *   
������ ��5C   *   �� *ItemsSource+
 �� *TemplatedParent+��5E   &   �� 5G   *    �� 
TargetType" ��60   .�5H   .   ��  �� Property � �� �� Value
 ���� 6L   �� 5I   .    � ��$ 0q�6P   �� 5J   .    � �� 5K   2   �� 5L   6   " ��6F   .R�5M   :   �� #��z��6A   #��z��5N   A    �� Padding# z��5O   A   $ 15��5P   A    J� SnapsToDevicePixels$
 True��5Q   A   ._�5R   >   �� #��z��6O   #��z��5S   O   $
 True��5T   O   5U   ;   5V   7   5X   /   5Y   +   5[   #   5\      5]      5^      5_      ��5a      �� 5b      .R�5c      �� $��0q�6   $��Center=�5d      ��5e      �� *#!DataContext.GetUserProfileCommand+
 ��  �� AncestorType" ���5f      �� .��5h      �  5i      �� 5j   "   $
��auto �63   �� 5k   "   $��* �63   .+�5n      A� $��40��6&   $��40��61   $��0��6=   $��15, 5q�5o   &   ��5p   "   �� 5q   &    j� Stretch$ UniformToFill=�61    �� ImageSource 5r   1   �� *Photo+ �� FallbackValue$I CD:/my_works/dotnetProjects/Study_Step/Study_Step/Assets/Media/5.jpg�� �� TargetNullValue$I CD:/my_works/dotnetProjects/Study_Step/Study_Step/Assets/Media/5.jpg�� �� UpdateSourceTrigger$ PropertyChanged=�5v   '   5x      �� 5z      $��1��6(   $	��300��68   $��18(�5|   (   $��SemiBold"�66   $
��Left=�5}   (   $��Center=�5~   (   ��5{   (   �� *
Username+5~   (   5�      ��5�      �� 5�      " ��6$    �� Triggers 5�   "   P� 5�   &    P� Property �@J�IsMouseOver P� Value$
 True��6E   . �5�   *   ��  � �� �#E0DFDE6G   5�   '   .�5�   "   ��  � ��
 ���� 6?   �� 5�   "    � �� 5�   &   �� 5�   *   " ��6:   .R�5�   .   �� #��z��65   $ 10��5�   5   ._�5�   2   �� $��Center=�6C   $��Center=�6`   5�   /   5�   +   5�   #   5�      5�      5�      5�      5�      5�      