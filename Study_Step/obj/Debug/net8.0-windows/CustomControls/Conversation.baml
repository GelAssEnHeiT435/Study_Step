   M S B A M L   `   `   `  ����E  AStudy_Step, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullE'clr-namespace:Study_Step.CustomControlsStudy_Step.CustomControls  5      =#clr-namespace:Study_Step.ViewModelsStudy_Step.ViewModels  5      ,    &Study_Step.CustomControls.Conversation   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      9local'clr-namespace:Study_Step.CustomControls     5      :
viewmodels#clr-namespace:Study_Step.ViewModels     5      $
��auto��5	      $
��auto��6     � 	Resources  5
         MessageTextStyle �� 
TargetType �� Property �� Value  ItemTemplate �� 
TargetType ��Name �� Padding J� SnapsToDevicePixels � RowDefinitions	 � ColumnDefinitions
 �� Triggers �� Binding �� Value ��Name 	 reply  IconButtonStyle �� Triggers �� 
TargetName P� Property P� Value��%�
  &	        &	 _    7   7  7  �� 5   
   " ��6)   .�5      ��  � ��$ 15(�6)   �� 5       � ��$ SemiBold"�6+   �� 5       � ��$ 20q�6'   �� 5       � ��
 �   �6+   �� 5       � ��$ Center=�64   �� 5       � ��$ Center=�62   �� 5       � ��$
 Wrap=�6-   �� 5       � �� 6%   �� *Text+5      �� 5   
   .R�5      �� ��5      �� 5      " ��6   .�5      ��  �@J�SnapsToDevicePixels$
 True��6@   �� 5       � ��$
 auto��63   �� 5       � ��$	 4,1q�64   �� 5       � ��$ 10q�63   �� 5       � ��$ Center=�6G   �� 5       � ��$ Center=�6E   �� 5        � ��$ Center=�6>   �� 5!       � ��
 ���� 67   �� 5"       � ��$ 0q�6<   �� 5&       � �� 5'      �� 5(   "   " ��62   .R�5)   &   �� $ border��6-   #��z��5*   %   #��z��5+   %   #��z��5,   %   # z��5-   %   $
 True��5.   %   ._�50   *   �  51   .   �� 52   2   $
��auto �6@   �� 53   2   �� 54   2   $
��auto �6@   .+�57   .   � $��1��63   	 58   2   �� 59   6   $
��auto �6G   �� 5:   6   �� 5;   6   $
��auto �6G   .+�5?   2   �� $
��auto��69   $
��auto��6G   $��60��5@   9   $��Center=�5A   9   $��1��5B   9   ��5C   6   �� 5D   :   " ��6@   .�5E   >   ��  �@��CornerRadius$ 30, 30, 30, 0��6]   �� 5F   >    � �� �#dce1de6[   �� 5G   >    � ��$
 Left=�6d   
 5H   >   �� 5I   B    6N   �� *	IsOutside+	 False6l   .a�5J   F   ��  �@��CornerRadius$ 30, 30, 0, 30��6e   �� 5K   F    � �� �#49a0786c   �� 5L   F    � ��$ Right=�6l   5M   C   5O   ;   ._�5T   6   ]� $	��700��6@   .+�5U   :   �� 8��  6D   5V   7   5W   3   �� 5Z   2   $ ReplyButton��69   $��2��5[   9   $y�	Collapsed=�5\   9   8�� 8�� 5^   9   5_   /   �� 5a   .   $��2��68   ��5b   2   �� 5c   6   " ��6<   .�5d   :   ��  � ��$ Center=�6^   �� 5e   :    � ��$ 0, 10, 20, 0q�6S   
 5f   :   �� 5g   >    6J   �� *	IsOutside+ True6h   .a�5h   B   ��  � �� 6Y   �� *SentAt+�� 5i   B    � ��$
 Left=�6h   5j   ?   �� 5k   >    6J   �� *	IsOutside+	 False6h   .a�5l   B   ��  � �� 6Y   �� *SentAt+�� 5m   B    � ��$ Right=�6h   5n   ?   5p   7   5r   /   5s   +   5t   '    5u   &   �� 5v   *   $ False��6T    66   �� *	IsOutside+.a�5w   .   �� $ ReplyButton��65    � ��$ 0��6e   5x   +   P� 5z   *    �@J�IsMouseOver$
 True��6I   . �5{   .   �� $ ReplyButton��65    � y�$ Visible=�6d   5|   +   5~   #   5�      5�      5�      5�      5�      .��5�      �  5�   
   �� 5�      $
��auto �6   �� 5�      .+�5�   
   �� -   5        $ SearchBoxBorder��5�      ���#dce1de �� CornerRadius$ 15��5�      $��20q�5�      $��50��5�      $��1��5�       +� FocusedElement 5�      ��  �� ElementName$ SearchQuery��._�5�      �� -   5        $��SearchQuery��5�      
������ $
��12,0q�5�      $��0q�5�      $��Search Text��5�      $��Center=�5�      $��Stretch=�5�      5�      �� 5�   
   -   5         ��Name$ scroll��5�      $��Disabled=�5�      
�������$��1��5�      .��5�      �� #��[ 5�      $��15,0,0,0q�5�      $��Disabled=�5�      ��5�      �� *Conversations+ �� UpdateSourceTrigger$ PropertyChanged=�5�      5�      5�      5�      