   M S B A M L   `   `   `  ����E  AStudy_Step, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullE'clr-namespace:Study_Step.CustomControlsStudy_Step.CustomControls  5      =#clr-namespace:Study_Step.ViewModelsStudy_Step.ViewModels  5      ,    &Study_Step.CustomControls.Conversation   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      9local'clr-namespace:Study_Step.CustomControls     5      :
viewmodels#clr-namespace:Study_Step.ViewModels     5      $
��auto��5	      $
��auto��6     � 	Resources  5         ItemTemplate �� 
TargetType �� Property �� Value �� 
TargetType ��Name �� Padding J� SnapsToDevicePixels � RowDefinitions	 � ColumnDefinitions
 �� Triggers �� Binding �� Value  MessageTextStyle ��Name 	 reply  IconButtonStyle �� Triggers �� 
TargetName P� Property P� Value��%K	  &	        7  7  7  �� 5   
   .R�5      �� ��5      �� 5      " ��6   .�5      ��  �@J�SnapsToDevicePixels$
 True��6@   �� 5       � ��$
 auto��63   �� 5       � ��$	 4,1q�64   �� 5       � ��$ 10q�63   �� 5       � ��$ Center=�6G   �� 5       � ��$ Center=�6E   �� 5       � ��$ Center=�6>   �� 5       � ��
 ���� 67   �� 5       � ��$ 0q�6<   �� 5       � �� 5       �� 5!   "   " ��62   .R�5"   &   �� $ border��6-   #��z��5#   %   #��z��5$   %   #��z��5%   %   # z��5&   %   $
 True��5'   %   ._�5)   *   �  5*   .   �� 5+   2   $
��auto �6@   �� 5,   2   �� 5-   2   $
��auto �6@   .+�50   .   � $��1��63   	 51   2   �� 52   6   $
��auto �6G   �� 53   6   �� 54   6   $
��auto �6G   .+�58   2   �� $
��auto��69   $
��auto��6G   $��60��59   9   $��Center=�5:   9   $��1��5;   9   ��5<   6   �� 5=   :   " ��6@   .�5>   >   ��  �@��CornerRadius$ 30, 30, 30, 0��6]   �� 5?   >    � �� �#dce1de6[   �� 5@   >    � ��$
 Left=�6d   
 5A   >   �� 5B   B    6N   �� *	IsOutside+	 False6l   .a�5C   F   ��  �@��CornerRadius$ 30, 30, 0, 30��6e   �� 5D   F    � �� �#49a0786c   �� 5E   F    � ��$ Right=�6l   5F   C   5H   ;   ._�5M   6   ]� $	��700��6@   .+�5N   :   �� 8��  6D   5O   7   5P   3   �� 5S   2   $ ReplyButton��69   $��2��5T   9   $y�	Collapsed=�5U   9   8�� 8�� 5W   9   5X   /   �� 5Z   .   $��2��68   ��5[   2   �� 5\   6   " ��6<   .�5]   :   ��  � ��$ Center=�6^   �� 5^   :    � ��$ 0, 10, 20, 0q�6S   
 5_   :   �� 5`   >    6J   �� *	IsOutside+ True6h   .a�5a   B   ��  � �� 6Y   �� *SentAt+�� 5b   B    � ��$
 Left=�6h   5c   ?   �� 5d   >    6J   �� *	IsOutside+	 False6h   .a�5e   B   ��  � �� 6Y   �� *SentAt+�� 5f   B    � ��$ Right=�6h   5g   ?   5i   7   5k   /   5l   +   5m   '    5n   &   �� 5o   *   $ False��6T    66   �� *	IsOutside+.a�5p   .   �� $ ReplyButton��65    � ��$ 0��6e   5q   +   P� 5s   *    �@J�IsMouseOver$
 True��6I   . �5t   .   �� $ ReplyButton��65    � y�$ Visible=�6d   5u   +   5w   #   5y      5z      5|      5~      5      .��5�      �  5�   
   �� 5�      $
��auto �6   �� 5�      .+�5�   
   �� -   5        $ SearchBoxBorder��5�      ���#dce1de �� CornerRadius$ 15��5�      $��20q�5�      $��50��5�      $��1��5�       +� FocusedElement 5�      ��  �� ElementName$ SearchQuery��._�5�      �� -   5        $��SearchQuery��5�      
������ $
��12,0q�5�      $��0q�5�      $��Search Text��5�      $��Center=�5�      $��Stretch=�5�      5�      �� 5�   
   -   5         ��Name$ scroll��5�      $��Disabled=�5�      
�������$��1��5�      .��5�      �� #��[  5�      $��15,0,0,0q�5�      $��Disabled=�5�      ��5�      �� *Conversations+ �� UpdateSourceTrigger$ PropertyChanged=�5�      5�      5�      5�      