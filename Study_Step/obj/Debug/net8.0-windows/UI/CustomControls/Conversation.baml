   M S B A M L   `   `   `  ����E  AStudy_Step, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullE'clr-namespace:Study_Step.CustomControlsStudy_Step.CustomControls  5      =#clr-namespace:Study_Step.ViewModelsStudy_Step.ViewModels  5      ,    &Study_Step.CustomControls.Conversation   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      9local'clr-namespace:Study_Step.CustomControls     5      :
viewmodels#clr-namespace:Study_Step.ViewModels     5      $
��auto��5	      $
��auto��6     � 	Resources  5
         MessageTextStyle �� 
TargetType �� Property �� Value �� Triggers �� Binding �� Value  ItemTemplate �� 
TargetType ��Name	 �� Padding
 J� SnapsToDevicePixels � RowDefinitions � ColumnDefinitions �� RelativeSource �� AncestorType  docs ��Name �� Padding �� Triggers P� Property P� Value �� 
TargetName �� StringFormat ��Name 	 reply  IconButtonStyle��%M  &	        &	 E    7   7  7  7  �� 5   
   " ��6)   .�5      ��  � ��$ 15(�6)   �� 5       � ��$ SemiBold"�6+   �� 5       � ��$ 10, 10, 10, 0q�6'   �� 5       � ��
 �   �6+   �� 5       � ��$ Center=�62   �� 5       � ��$
 Left=�64   �� 5       � ��$
 Wrap=�6-   �� 5       � �� 6%   �� *Text+ 5      �� 5       6   �� *Text+ 67   Z� .a�5      ��  � y�$ 	Collapsed=�63   5      �� 5       6   �� *Text+  67   .a�5      ��  � y�$ 	Collapsed=�63   5      5      �� 5   
   .R�5      �� ��5       �� 5!      " ��6   .�5"      ��  �@J�SnapsToDevicePixels$
 True��6@   �� 5#       � ��$
 auto��63   �� 5$       � ��$	 4,1q�64   �� 5%       � ��$ 5q�63   �� 5&       � ��$ Center=�6G   �� 5'       � ��$ Center=�6E   �� 5(       � ��$ Center=�6>   �� 5)       � ��
 ���� 67   �� 5*       � ��$ 0q�6<   �� 5.       � �� 5/      �� 50   "   " ��62   .R�51   &   �� $ border��6-   #��z��52   -   #��z��53   -   #��z��54   -   #	 z��55   -   $

 True��56   -   ._�58   *   �  59   .   �� 5:   2   $
��auto �6@   �� 5;   2   �� 5<   2   $
��auto �6@   .+�5?   .   � $��1��63    5@   2   �� 5A   6   $
��auto �6G   �� 5C   6   �� 5E   6   $
��auto �6G   .+�5J   2   �� $��60��5K   9   $	��100��5L   9   $��Center=�5M   9   $��1��5N   9   ��5O   6   �� 5P   :   " ��6@   .�5Q   >   ��  �@��CornerRadius$ 20, 20, 20, 0��6]   �� 5R   >    � �� �#dce1de6[   �� 5S   >    � ��$
 Left=�6d    5T   >   �� 5U   B    6N   �� *	IsOutside+	 False6l   .a�5V   F   ��  �@��CornerRadius$ 20, 20, 0, 20��6e   �� 5W   F    � �� �#7BD0A96c   �� 5X   F    � ��$ Right=�6l   5Y   C   5[   ;   ._�5`   6   �  5a   :   �� 5b   >   $
��auto �6L   �� 5c   >   $
��auto �6L   �� 5d   >   .+�5g   :   �� 8��  $��0��5h   D   �� 5j   :   $��Stretch=�5k   B   
������ $��0q�5m   B   $��1��5n   B   ��5j   B   �� *Files+  5o   >   ���� 5p   B   '��       " ��6H   .�5q   F   ��  � ��
 ���� 6c   �� 5r   F    � ��$ 0q�6h   �� 5s   F    � �� 5t   J   �� 5u   N   " ��6^   .R�5v   R   �� #��z��6Y   #��z��5w   Y   ._�5x   V   �� 5y   S   5z   O   5|   G   5}   C   5~   ?   ��5   >   �� 5�   B   .R�5�   F   �� $	��100��6M   $��Stretch=�5�   M   ��5�   M   �� *DataContext.DownloadFile+ �� " ���5�   M   �� ��5�   J   �� 5�   N   " ��6T   .�5�   R   ��  �@J�SnapsToDevicePixels$
 True��6x   �� 5�   R    � ��$
 auto��6k   �� 5�   R    � ��
 ���� 6o   �� 5�   R    � ��$ 0q�6t   �� 5�   R    � �� 5�   V   �� 5�   Z   " ��6j   .R�5�   ^   � $��10q�6c    5�   b   �� 5�   f   $
��auto �6w   �� 5�   f   $��* �6w   .+�5�   b   F� 8�� $��Center=�5�   g   $��Center=�5�   g   
���   �$��25��5�   g   $��25��6r   $��0��5�   g   �� 5�   b   $ FileName��6l   $��Center=�5�   l   $��Center=�5�   l   $��14(�5�   l   $ 10, 0q�6z   $��SemiBold"�5�   l   $��NoWrap=�5�   l   $��CharacterEllipsis=�5�   l   $��1��5�   l   ��5�   l   �� *Name+5�   l   5�   _    5�   ^   P� 5�   b    �@J�IsMouseOver$
 True��6�   . �5�   f   �� $ FileName��6m    � ��$
 Bold"�6�   5�   c   5�   [   5�   S   5�   O   5�   G   5�   C   5�   ;   �� 5�   :   $��2��6D   $��0, 0, 10, 5q�5�   D   $��Right=�5�   D   ��5�   D   �� *SentAt+$ 	{0:HH:mm}��5�   7   5�   3   �� 5�   2   $ ReplyButton��69   $��2��5�   9   $y�	Collapsed=�5�   9   8�� 8�� 5�   9   5�   /   5�   +   5�   '    5�   &   �� 5�   *   $ False��6T    66   �� *	IsOutside+.a�5�   .   �� $ ReplyButton��65    � ��$ 0��6e   5�   +   P� 5�   *    �@J�IsMouseOver$
 True��6I   . �5�   .   �� $ ReplyButton��65    � y�$ Visible=�6d   5�   +   5�   #   5�      5�      5�      5�      5�      .��5�      �  5�   
   �� 5�      $
��auto �6   �� 5�      .+�5�   
   �� -   5        $ SearchBoxBorder��5�      ���#dce1de �� CornerRadius$ 15��5�      $��20q�5�      $��50��5�      $��1��5�       +� FocusedElement 5�      ��  �� ElementName$ SearchQuery��._�5�      �� -   5        $��SearchQuery��5�      
������ $
��12,0q�5�      $��0q�5�      $��Search Text��5�      $��Center=�5�      $��Stretch=�5�      5�      �� 5�   
   -   5         ��Name$ scroll��5�      $��Disabled=�5�      
�������$��1��5�      .��5�      �� #��[ 5�      $��15,0,0,0q�5�      $��Disabled=�5�      ��5�      �� *Conversations+ �� UpdateSourceTrigger$ PropertyChanged=�5�      5�      5�      5      