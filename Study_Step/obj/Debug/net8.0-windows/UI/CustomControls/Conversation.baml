   M S B A M L   `   `   `  ����E  AStudy_Step, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullE'clr-namespace:Study_Step.CustomControlsStudy_Step.CustomControls  5      =#clr-namespace:Study_Step.ViewModelsStudy_Step.ViewModels  5	      K*clr-namespace:Study_Step.Data.UIConvertersStudy_Step.Data.UIConverters  5
      5clr-namespace:Study_Step.ModelsStudy_Step.Models  5      ,    &Study_Step.CustomControls.Conversation   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      9local'clr-namespace:Study_Step.CustomControls     5      e aSyncfusion.SfProgressBar.WPF, Version=29.1.33.0, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89^ ZSyncfusion.Shared.WPF, Version=29.1.33.0, Culture=neutral, PublicKeyToken=3d67ed1f87d44c894
Syncfusion!http://schemas.syncfusion.com/wpf   5      6
viewmodels#clr-namespace:Study_Step.ViewModels   5	      :convert*clr-namespace:Study_Step.Data.UIConverters   5
      .modelsclr-namespace:Study_Step.Models   5      -   5          >�Name$  ChatElement��5      $
��auto��5      $
��auto��6    � 	Resources 5      9   3Study_Step.Data.UIConverters.WordExtensionConverter   WordExtensionConverter:   4Study_Step.Data.UIConverters.ExcelExtensionConverter  ExcelExtensionConverter?  9Study_Step.Data.UIConverters.PowerPointExtensionConverter   PowerPointExtensionConverter;   5Study_Step.Data.UIConverters.MarginOffsetMesConverter  MarginOffsetMesConverter  MessageTextStyle �� 
TargetType �� Property �� Value �� Triggers �� Binding �� Value  ItemTemplate �� 
TargetType	 ��Name
 �� Padding J� SnapsToDevicePixels � RowDefinitions � ColumnDefinitions  TransparentListItem �� RelativeSource �� AncestorType (� Stretch  defailtFile �� 	Converter  docsFile 	 	excelFile 
 powerpointFile:  4Syncfusion.UI.Xaml.ProgressBar.SfCircularProgressBar   IndicatorOuterRadius   IndicatorInnerRadius   TrackOuterRadius   TrackInnerRadius   ShowProgressValue4  .Syncfusion.UI.Xaml.ProgressBar.ProgressBarBase   IndicatorCornerRadius   Progress   RangeColors9  3Syncfusion.UI.Xaml.ProgressBar.RangeColorCollection/  )Syncfusion.UI.Xaml.ProgressBar.RangeColor   
IsGradient   Color   Start
   End   ProgressContent ��Name  K� ZIndex 	 close
! � Tag" �� Width# �� Height  IconButtonStyle$ �� BasedOn% P� Property& P� Value' �� ScaleX( �� ScaleY) ��Name* �� Padding+ �� TargetNullValue, �� FallbackValue- �� Triggers. �� 
TargetName/ �� Orientation0 �� StringFormat 
 Unread  Read 	 more21 `� 	Converter2 �� Path3 �� ElementName  MoreOptButtonStyle4 �� Mode-	   'Study_Step.CustomControls.OptionalPopup5 	  VerticalOffset6 	  Width7 	  IsOpen8 	  PlacementTarget9 	  Buttons*
   $Study_Step.Models.PopupButtonElement: 
  Text  rewrite; 
  Icon< 
  Command= �� Source%  System.Windows.Markup.Reference 	 trash��%"  &	        &	      &	 &     &	 9     &	 L     &	 +    7  7  7  7   7  7  7 	 7  7 
 7  7  7  7  7  7  7  7  7    5   
   6)     5   
   6*     5   
   6/     5   
   6+   �� 5   
   " ��6)   .�5      ��  � ��$ 15(�6)   �� 5       � ��$ SemiBold"�6+   �� 5       � ��$ 10, 10, 10, 0q�6'   �� 5       � ��
 �   �6+   �� 5       � ��$ Center=�62   �� 5       � ��$
 Left=�64   �� 5       � ��$
 Wrap=�6-   �� 5       � �� 6%   �� *Text+ 5      �� 5       6   �� *Text+ 67   Z� .a�5      ��  � y�$ 	Collapsed=�63   5       5"      �� 5$   
   .R�5%      �� ��5&      �� 5'      " ��6   .�5(      ��  �@J�SnapsToDevicePixels$
 True��6@   �� 5)       � ��$
 auto��63   �� 5*       � ��$	 4,1q�64   �� 5+       � ��$ 5q�63   �� 5,       � ��$ Center=�6G   �� 5-       � ��$ Center=�6E   �� 5.       � ��$ Center=�6>   �� 5/       � ��
 ���� 67   �� 50       � ��$ 0q�6<   �� 52       � �� 53      �� 54   "   " ��62   .R�55   &   �� $	 border��6-   #��z��56   -   #��z��57   -   #��z��58   -   #
 z��59   -   $
 True��5:   -   ._�5<   *   �  5=   .   �� 5>   2   $
��auto �6@   �� 5?   2   �� 5@   2   $
��auto �6@   .+�5C   .   � $��1��63    5D   2   �� 5E   6   $
��auto �6G   �� 5G   6   �� 5I   6   $
��auto �6G   .+�5N   2   �� $	 MessageBubble��69   $��60��5O   9   $	��100��5P   9   $��Center=�5Q   9   $��1��5R   9   ��5S   6   �� 5T   :   " ��6@   .�5U   >   ��  �@��CornerRadius$ 20, 20, 20, 0��6]   �� 5V   >    � �� �#dce1de6[   �� 5W   >    � ��$
 Left=�6d    5X   >   �� 5Y   B    6N   �� *	IsOutside+	 False6l   .a�5Z   F   ��  �@��CornerRadius$ 20, 20, 0, 20��6e   �� 5[   F    � �� �#7BD0A96c   �� 5\   F    � ��$ Right=�6l   5]   C   5_   ;   ._�5d   6   �  5e   :   �� 5f   >   $
��auto �6L   �� 5g   >   $
��auto �6L   �� 5h   >   .+�5k   :   �� 8��  $��0��5l   D   �� 5n   :   8�� $��Disabled=�5p   B   $
��Left=�5q   B   
������ $��0q�5s   B   $��1��5t   B   ��5n   B   �� *Files+��5u   >   �� 5v   B   .R�5w   F   �� $��Stretch=�6M   $
��auto��6k   ��5x   M   �� *DataContext.DownloadFile+ �� " ���5y   M   �� ��5z   J   �� 5{   N   " ��6T   .�5|   R   ��  �@J�SnapsToDevicePixels$
 True��6x   �� 5}   R    � ��$
 auto��6k   �� 5~   R    � ��
 ���� 6o   �� 5   R    � ��$ 0q�6t   �� 5�   R    � �� 5�   V   �� 5�   Z   " ��6j   .R�5�   ^   �  5�   b   �� 5�   f   $
��auto �6w   �� 5�   f   $��* �6w   .+�5�   b   (� $ Uniform=�6j   $��0��6|   $��Center=�5�   j   $��Center=�5�   j   $��25��5�   j   $��25��6u   .��5�   f   F� ��5�   j   �� 5�   n   " F�6t   .�5�   r   ��  � ��8  6�   �� 5�   r    � ��$ Center=�6�   �� 5�   r    � ��$ Center=�6�   �� 5�   r    � ��
 �   �6�    5�   r   �� 5�   v    6�   �� *	Extension+8   True6�   .a�5�   z   ��  � ��8  6�   5�   w   �� 5�   v    6�   �� *	Extension+8   True6�   .a�5�   z   ��  � ��8  6�   5�   w   �� 5�   v    6�   �� *	Extension+8   True6�   .a�5�   z   ��  � ��8  6�   5�   w   5�   o   5�   g   ��5�   f   �� 5�   j   " (�6p   .�5�   n   ��  � y�$ 	Collapsed=�6�    5�   n   �� 5�   r    6~   �� *Status+ 06�   .a�5�   v   ��  � y�$ Visible=�6�   5�   s   �� 5�   r    6~   �� *Status+ 16�   .a�5�   v   ��  � y�$ 	Collapsed=�6�   5�   s   5�   k   5�   c   � 5�   b   $��0��6g   $��50��6w   $��50��6�   $��Center=�5�   g   $��Center=�5�   g   .+�5�   f     $
 0.75Q�6�   $	 0.6Q�6�   $	 0.7Q�5�   �   $
 0.65Q�6�   $��50��5�   �   $��50��6�   $ False��5�   �   $ 10Q�6�   $��Center=�5�   �    5�   �   �� *
Progress+ 5�   n       5�   r   $
 True��6�   $ #6CD861��6�   $ 10Q�6�   $ 30Q�6�     5�   r   $
 True��6�   $ #11B000��6�   $ 30Q�6�   $ 60Q�6�     5�   r   $
 True��6�   $ #2A8421��6�   $ 60Q�6�   $	 100Q�6�   5�   o    5�   j   �� 5�   n   $ btnDeleteSendFile��6u   $��20��5�   u   $��20��6�   $	  999��6�   $��Center=�5�   u   $��Center=�5�   u   8��	 ��5�   u   �� *#!DataContext.CancelSendFileCommand+ �� " ���5�   u   �� ! 5�   r   �� 5�   v   $" 12Q�6{   $# 12Q�6�   ��5�   r   �� 5�   v   " ��6|   8$ 
  5�   z   P� 5�   ~   % �@J�IsMouseOver$
& True��6�   . �5�   �   ��  �@J�RenderTransformOrigin$ 0.5,0.5&�6�   �� 5�   �    � z� 5�   �   �� 5�   �   $
' 1.35Q�6�   $
( 1.35Q�6�   5�   �   �� 5�   �    � ��
 ���� 6�   5�      5�   w   5�   o   5�   g   ��5�   f   �� 5�   j   " �6p   .�5�   n   ��  � y�$ Visible=�6�    5�   n   �� 5�   r    6~   �� *Status+ 16�   .a�5�   v   ��  � y�$ Visible=�6�   5�   s   �� 5�   r    6~   �� *Status+ 06�   .a�5�   v   ��  � y�$ 	Collapsed=�6�   5�   s   5�   k   5�   c   �� 5�   b   $) FileName��6l   $��Center=�5�   l   $
��Left=�5�   l   $��14(�5�   l   $* 10, 0q�6z   $��SemiBold"�5�   l   $��NoWrap=�5�   l   $��CharacterEllipsis=�5�   l   $	��300��5�   l   $��1��5�   l   ��5�   l   �� *Name+5�   l   ��5�   b   �� 5�   f   " �6l   .�5�   j   ��  � ��$ 10, 0, 5, 0q�6�    5�   j   �� 5�   n    6z   �� *Text+ 6�   Z� .a�5�   r   ��  � ��$ 10, 10, 5, 0q�6�   5�   o   �� 5   n    6z   �� *Text+$+  ��$,  ��  6�   .a�5  r   ��  � ��$ 10, 10, 5, 0q�6�   5  o   5  g   5  _   - 5  ^   P� 5  b   % �@J�IsMouseOver$
& True��6�   . �5	  f   �� $. FileName��6m    � ��$
 Bold"�6�   5
  c   5  [   5  S   5  O   5  G   5  C   5  ;   �� 5  :   $/ 
Horizontal=�6E   $��0, 0, 15, 5q�5  E   $��Right=�5  E   $��2��5  E   $��25��6R   $
��auto��6^   .+�5  >   �� $��14(�6H   $��Center=�6V   ��5  H   �� *SentAt+$0 	{0:HH:mm}��(� 5  >   $ Uniform=�6F   $��Center=�6X   $��20��5  F   $��20��6Q   $��
0, 0, 0, 5q�6]   .��5   B   F� ��5!  F   �� 5"  J   " F�6P   .�5#  N   ��  � ��8  6e   �� 5$  N    � ��$ Center=�6t   �� 5%  N    � ��$ Center=�6r   �� 5&  N    � ��
 �   �6e    5'  N   �� 5(  R    6^   �� *isRead+ true6y   .a�5)  V   ��  � ��8  6m   5*  S   5,  K   5.  C   ��5/  B   �� 50  F   " (�6L    51  J   �� 52  N    6Z   �� *	IsOutside+ true6x   .a�53  R   ��  � y�$ 	Collapsed=�6o   54  O   56  G   58  >   5;  ;   5=  7   5?  3   �� 5A  2   $ btnOpenPopupMes��69   8�� $��Bottom=�5E  9   $
��Left=�6T   $��20��5F  9   $��20��6D   $��1��6P   
���   �$��1q�5H  9   ���#DEDEDE��5B  9   �� *#!DataContext.OpenOptMessageCommand+ �� " >���5C  9   �� ! 5J  6   �� 5K  :   $" 15Q�6?   $# 15Q�6J   ��5N  6   `� 5O  :   81  .1�5P  >   �� $2 ActualWidth�6F   $3 MessageBubble��6Y   �� 5Q  >   $2 	IsOutside�6F   5R  ;   ��5U  6   �� 5V  :   8$  " ��6n   .�5W  >   ��  � y�$ 	Collapsed=�6[    5X  >   �� 5Y  B    6N   �� *IsPopupOpen+$4 TwoWay=� True6{   .a�5Z  F   ��  � y�$ Visible=�6c   5[  C   5]  ;   5_  3   	  5a  2   $	5 -40Q�5c  F   $	6 150��6[   $��1��5d  F   7 5a  F   �� *IsPopupOpen+$4 TwoWay=�8 5b  F   �� $3 btnOpenPopupMes��9 5f  :   
  $ : Редактировать��6T   8;  < 5g  T   �� * DataContext.EditMessageCommand+=   *ChatElement+
  5h  :   $: Удалить��6T   8;  < 5i  T   �� *'%DataContext.OpenDeletionWindowCommand+=   *ChatElement+��5l  6   �� 5m  :   " 	 6@   .�5n  >   ��  �@	 	Placement$
 Left=�6Z    5o  >   �� 5p  B    6N   �� *	IsOutside+	 False6l   .a�5q  F   ��  �@	 	Placement$ Right=�6b   5r  C   5t  ;   5v  3   5�  /   5�  +   5�  '   - 5�  &   �� 5�  *   $ False��6T    66   �� *	IsOutside+.a�5�  .   �� $. btnOpenPopupMes��65    � ��$ Right=�6q   5�  +   P� 5�  *   % �@J�IsMouseOver$
& True��6I   . �5�  .   �� $. btnOpenPopupMes��65    � y�$ Visible=�6h   5�  +   5�  #   5�     5�     5�     5�     5�     .��5�     �  5�  
   �� 5�     $
��auto �6   �� 5�     .+�5�  
   �� -   5        $	 SearchBoxBorder��5�     ���#dce1de> �� CornerRadius$> 15��5�     $��20q�5�     $��50��5�     $��1��5�     ? +� FocusedElement? 5�     �� $3 SearchQuery��._�5�     �� -   5        $��SearchQuery��5�     
������ $
��12,0q�5�     $��0q�5�     $��Search Text��5�     $��Center=�5�     $��Stretch=�5�     5�     �� 5�  
   -   5        @ ��Name$@ scroll��5�     $��Disabled=�5�     
�������$��1��5�     .��5�     �� #��[ 5�     $��15,0,0,0q�5�     $��Disabled=�5�     ��5�     �� *Conversations+A �� UpdateSourceTrigger$A PropertyChanged=�5�     5�     5�     5�     