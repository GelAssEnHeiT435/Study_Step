   M S B A M L   `   `   `  ����E  AStudy_Step, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullE'clr-namespace:Study_Step.CustomControlsStudy_Step.CustomControls  5      =#clr-namespace:Study_Step.ViewModelsStudy_Step.ViewModels  5      K*clr-namespace:Study_Step.Data.UIConvertersStudy_Step.Data.UIConverters  5	      ,    &Study_Step.CustomControls.Conversation   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      9local'clr-namespace:Study_Step.CustomControls     5      e aSyncfusion.SfProgressBar.WPF, Version=29.1.33.0, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89^ ZSyncfusion.Shared.WPF, Version=29.1.33.0, Culture=neutral, PublicKeyToken=3d67ed1f87d44c894
Syncfusion!http://schemas.syncfusion.com/wpf   5      6
viewmodels#clr-namespace:Study_Step.ViewModels   5      8icons*clr-namespace:Study_Step.Data.UIConverters   5	      $
��auto��5      $
��auto��6     � 	Resources  5      9   3Study_Step.Data.UIConverters.WordExtensionConverter   WordExtensionConverter:   4Study_Step.Data.UIConverters.ExcelExtensionConverter  ExcelExtensionConverter?  9Study_Step.Data.UIConverters.PowerPointExtensionConverter   PowerPointExtensionConverter  MessageTextStyle �� 
TargetType �� Property �� Value �� Triggers �� Binding �� Value  ItemTemplate �� 
TargetType ��Name	 �� Padding
 J� SnapsToDevicePixels � RowDefinitions � ColumnDefinitions  TransparentListItem �� RelativeSource �� AncestorType (� Stretch  defailtFile �� 	Converter  docsFile  	excelFile 	 powerpointFile:  4Syncfusion.UI.Xaml.ProgressBar.SfCircularProgressBar   IndicatorOuterRadius   IndicatorInnerRadius   TrackOuterRadius   TrackInnerRadius   ShowProgressValue4  .Syncfusion.UI.Xaml.ProgressBar.ProgressBarBase   IndicatorCornerRadius   Progress   RangeColors9  3Syncfusion.UI.Xaml.ProgressBar.RangeColorCollection/  )Syncfusion.UI.Xaml.ProgressBar.RangeColor   
IsGradient   Color   Start
   End   ProgressContent ��Name K� ZIndex 	
 close
  � Tag! �� Width" �� Height  IconButtonStyle# �� BasedOn$ P� Property% P� Value& �� ScaleX' �� ScaleY( ��Name) �� Padding* �� Triggers+ �� 
TargetName, �� StringFormat 	 reply��%�  &	        &	      &	 &     &	 9     &	     7  7  7  7   7  7  7  7  7 	 7 
 7  7  7    5   
   6'     5   
   6(     5   
   6-   �� 5   
   " ��6)   .�5      ��  � ��$ 15(�6)   �� 5       � ��$ SemiBold"�6+   �� 5       � ��$ 10, 10, 10, 0q�6'   �� 5       � ��
 �   �6+   �� 5       � ��$ Center=�62   �� 5       � ��$
 Left=�64   �� 5       � ��$
 Wrap=�6-   �� 5       � �� 6%   �� *Text+ 5      �� 5       6   �� *Text+ 67   Z� .a�5      ��  � y�$ 	Collapsed=�63   5      5      �� 5!   
   .R�5"      �� ��5#      �� 5$      " ��6   .�5%      ��  �@J�SnapsToDevicePixels$
 True��6@   �� 5&       � ��$
 auto��63   �� 5'       � ��$	 4,1q�64   �� 5(       � ��$ 5q�63   �� 5)       � ��$ Center=�6G   �� 5*       � ��$ Center=�6E   �� 5+       � ��$ Center=�6>   �� 5,       � ��
 ���� 67   �� 5-       � ��$ 0q�6<   �� 5/       � �� 50      �� 51   "   " ��62   .R�52   &   �� $ border��6-   #��z��53   -   #��z��54   -   #��z��55   -   #	 z��56   -   $

 True��57   -   ._�59   *   �  5:   .   �� 5;   2   $
��auto �6@   �� 5<   2   �� 5=   2   $
��auto �6@   .+�5@   .   � $��1��63    5A   2   �� 5B   6   $
��auto �6G   �� 5D   6   �� 5F   6   $
��auto �6G   .+�5K   2   �� $��60��5L   9   $	��100��5M   9   $��Center=�5N   9   $��1��5O   9   ��5P   6   �� 5Q   :   " ��6@   .�5R   >   ��  �@��CornerRadius$ 20, 20, 20, 0��6]   �� 5S   >    � �� �#dce1de6[   �� 5T   >    � ��$
 Left=�6d    5U   >   �� 5V   B    6N   �� *	IsOutside+	 False6l   .a�5W   F   ��  �@��CornerRadius$ 20, 20, 0, 20��6e   �� 5X   F    � �� �#7BD0A96c   �� 5Y   F    � ��$ Right=�6l   5Z   C   5\   ;   ._�5a   6   �  5b   :   �� 5c   >   $
��auto �6L   �� 5d   >   $
��auto �6L   �� 5e   >   .+�5h   :   �� 8��  $��0��5i   D   �� 5k   :   8�� $
��Left=�5m   B   
������ $��0q�5o   B   $��1��5p   B   ��5k   B   �� *Files+��5q   >   �� 5r   B   .R�5s   F   �� $	��100��6M   $��Stretch=�5t   M   ��5u   M   �� *DataContext.DownloadFile+ �� " ���5v   M   �� ��5w   J   �� 5x   N   " ��6T   .�5y   R   ��  �@J�SnapsToDevicePixels$
 True��6x   �� 5z   R    � ��$
 auto��6k   �� 5{   R    � ��
 ���� 6o   �� 5|   R    � ��$ 0q�6t   �� 5}   R    � �� 5~   V   �� 5   Z   " ��6j   .R�5�   ^   � $��10q�6c    5�   b   �� 5�   f   $
��auto �6w   �� 5�   f   $��* �6w   .+�5�   b   (� $ Uniform=�6j   $��0��6|   $��Center=�5�   j   $��Center=�5�   j   $��25��5�   j   $��25��6u   .��5�   f   F� ��5�   j   �� 5�   n   " F�6t   .�5�   r   ��  � ��8  6�   �� 5�   r    � ��$ Center=�6�   �� 5�   r    � ��$ Center=�6�   �� 5�   r    � ��
 �   �6�    5�   r   �� 5�   v    6�   �� *	Extension+8   True6�   .a�5�   z   ��  � ��8  6�   5�   w   �� 5�   v    6�   �� *	Extension+8   True6�   .a�5�   z   ��  � ��8  6�   5�   w   �� 5�   v    6�   �� *	Extension+8   True6�   .a�5�   z   ��  � ��8  6�   5�   w   5�   o   5�   g   ��5�   f   �� 5�   j   " (�6p   .�5�   n   ��  � y�$ 	Collapsed=�6�    5�   n   �� 5�   r    6~   �� *Status+ 06�   .a�5�   v   ��  � y�$ Visible=�6�   5�   s   �� 5�   r    6~   �� *Status+ 16�   .a�5�   v   ��  � y�$ 	Collapsed=�6�   5�   s   5�   k   5�   c   � 5�   b   $��0��6g   $��50��6w   $��50��6�   $��Center=�5�   g   $��Center=�5�   g   .+�5�   f     $
 0.75Q�6�   $	 0.6Q�6�   $	 0.7Q�5�   �   $
 0.65Q�6�   $��50��5�   �   $��50��6�   $ False��5�   �   $ 10Q�6�   $��Center=�5�   �    5�   �   �� *
Progress+ 5�   n       5�   r   $
 True��6�   $ #6CD861��6�   $ 10Q�6�   $ 30Q�6�     5�   r   $
 True��6�   $ #11B000��6�   $ 30Q�6�   $ 60Q�6�     5�   r   $
 True��6�   $ #2A8421��6�   $ 60Q�6�   $	 100Q�6�   5�   o    5�   j   �� 5�   n   $ btnDeleteSendFile��6u   $��20��5�   u   $��20��6�   $	 999��6�   $��Center=�5�   u   $��Center=�5�   u   8��	 ��5�   u   �� *#!DataContext.CancelSendFileCommand+ �� " ���5�   u   ��   5�   r   �� 5�   v   $! 12Q�6{   $" 12Q�6�   ��5�   r   �� 5�   v   " ��6|   8# 
  5�   z   P� 5�   ~   $ �@J�IsMouseOver$
% True��6�   . �5�   �   ��  �@J�RenderTransformOrigin$ 0.5,0.5&�6�   �� 5�   �    � z� 5�   �   �� 5�   �   $
& 1.35Q�6�   $
' 1.35Q�6�   5�   �   �� 5�   �    � ��
 ���� 6�   5�      5�   w   5�   o   5�   g   ��5�   f   �� 5�   j   " �6p   .�5�   n   ��  � y�$ Visible=�6�    5�   n   �� 5�   r    6~   �� *Status+ 16�   .a�5�   v   ��  � y�$ Visible=�6�   5�   s   �� 5�   r    6~   �� *Status+ 06�   .a�5�   v   ��  � y�$ 	Collapsed=�6�   5�   s   5�   k   5�   c   �� 5�   b   $( FileName��6l   $��Center=�5�   l   $
��Left=�5�   l   $��14(�5�   l   $) 10, 0q�6z   $��SemiBold"�5�   l   $��NoWrap=�5�   l   $��CharacterEllipsis=�5�   l   $��1��5�   l   ��5�   l   �� *Name+5�   l   5�   _   * 5�   ^   P� 5�   b   $ �@J�IsMouseOver$
% True��6�   . �5�   f   �� $+ FileName��6m    � ��$
 Bold"�6�   5�   c   5�   [   5�   S   5�   O   5   G   5  C   5  ;   �� 5  :   $��2��6D   $��0, 0, 10, 5q�5  D   $��Right=�5  D   ��5  D   �� *SentAt+$, 	{0:HH:mm}��5	  7   5  3   �� 5  2   $ ReplyButton��69   $��2��5  9   $y�	Collapsed=�5  9   8�� 8��   5  6   �� 5  :   $! 20Q�6?   $" 20Q�6J   5  3   5  /   5  +   5  '   * 5  &   �� 5  *   $ False��6T    66   �� *	IsOutside+.a�5  .   �� $+ ReplyButton��65    � ��$ 0��6e   5  +   P� 5  *   $ �@J�IsMouseOver$
% True��6I   . �5   .   �� $+ ReplyButton��65    � y�$ Visible=�6d   5!  +   5#  #   5%     5&     5(     5*     5+     .��5-     �  5.  
   �� 5/     $
��auto �6   �� 50     .+�54  
   �� -   5        $ SearchBoxBorder��54     ���#dce1de- �� CornerRadius$- 15��56     $��20q�57     $��50��58     $��1��59     . +� FocusedElement. 5:     �� / �� ElementName$/ SearchQuery��._�5<     �� -   5        $��SearchQuery��5=     
������ $
��12,0q�5>     $��0q�5?     $��Search Text��5@     $��Center=�5A     $��Stretch=�5B     5D     �� 5F  
   -   5        0 ��Name$0 scroll��5F     $��Disabled=�5G     
�������$��1��5I     .��5J     �� #��[ 5K     $��15,0,0,0q�5L     $��Disabled=�5M     ��5J     �� *Conversations+1 �� UpdateSourceTrigger$1 PropertyChanged=�5S     5T     5U     5V     