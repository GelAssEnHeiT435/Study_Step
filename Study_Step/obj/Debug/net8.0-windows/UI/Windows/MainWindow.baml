   M S B A M L   `   `   `  ����E  AStudy_Step, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:Study_Step
Study_Step  5   	   E'clr-namespace:Study_Step.CustomControlsStudy_Step.CustomControls  5   	   =#clr-namespace:Study_Step.ViewModelsStudy_Step.ViewModels  5	   	       Study_Step.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   *localclr-namespace:Study_Step     5   	   Bcustomcontrols'clr-namespace:Study_Step.CustomControls     5   	   :
viewmodels#clr-namespace:Study_Step.ViewModels     5	   	   -   5          �Name$  
ChatWindow��5   	    � Title$ 	StudyStep��5   	    � AllowsTransparency$
 True��5   	   
������ $��15(�5   	    � 
ResizeMode$ NoResize=�5   	    � WindowStyle$
 None=�5   	    � WindowStartupLocation$ CenterScreen=�5   	   $	��768��5   	   $
��1024��6    J� InputBindings 5      �� 5   
   
 �� Key$ Enter��6    �� Command 6!   �� *SendMesCommand+	 � 	Resources	 5         ContactInfoButtonTemplate
 �� 
TargetType � ColumnDefinitions j� Stretch �� ImageSource �� FallbackValue �� TargetNullValue �� UpdateSourceTrigger��%t  &	        �� 5   
   "
 ��5      .R�5      �� #��z��6   #��z��5      #��z��5      ._�5      � $��Center=�6    5      �� 5       $��75 �6+   �� 5!      .+�5$      A� $��40��6   $��40��6*   ��5%      �� 5&      $ UniformToFill=�5*   )    5&   )   �� *bitmapPhoto+$C =D:\my_worksdotnetProjectsStudy_StepStudy_StepAssetsMedia5.jpg��$C =D:\my_worksdotnetProjectsStudy_StepStudy_StepAssetsMedia5.jpg��$ PropertyChanged=�5*   )   5,      � 5.      $��1��6   .+�5/      �� �� 51      $
��Bold"�55   "   $��20(�56   "   �51   "   �� *ContactName+$ Contact Name��$ Contact Name��$ PropertyChanged=�56   "   �� 57      �� 59      $��15(�5=   "   �59   "   �� *
LastSeen+$ 	Last Seen��$ 	Last Seen��$ PropertyChanged=�5=   "   5>      5?      5@      5A      5C      5D      .��5G      ��  �� CornerRadius$ 20��6   
�������._�5I   
   �  � RowDefinitions 5J      �� 5K      $
��auto �6    �� 5L       5N      �� 5O      $
��auto �6#   �� 5P      �� 5Q      $
��auto �6#   .+�5U      �� -   5        $ 20, 20, 0, 0��5U      $��3��5V      ���#49a078._�5Y      � .+�5Z      �� $
��Left=�5[       $
��15,0q�5\       $��Center=�5]       
�������$��22(�5_       $
��Bold"�5`       ��5Z       �� *Title+ �� ElementName$ 
ChatWindow��5`       �� 5c       �� Orientation$ 
Horizontal=�6!   $��Right=�5d   !   $
��10,0q�5f   !   .+�5g      �� -   5         ��Name$ btnMinimize��5g   !   $��Center=�5i   !     minimize#��[ 5j   !     IconButtonStyle#��[ 5k   !   �� 5m      -   5        $ btnMaximize��5m   !   $��Center=�5o   !     maximize#��[ 5p   !   #��[ 5q   !   �� 5s      -   5        $ btnClose��5s   !   $��Center=�5u   !    	 close#��[ 5v   !   #��[ 5w   !   5y      5z      5{      � 5~      $��1��6   $	��406��6     5      �� 5�      $
��auto �6$   �� 5�      $
��auto �6$   �� 5�      .+�5�      � $��75��6   $
��15,0q�6#    5�      �� 5�      $
��Auto �6+   �� 5�      �� 5�      $��40 �6+   �� 5�      $��40 �6+   .+�5�      �� $��Chats��6    $
��Left=�5�       $��Center=�5�       $��20(�5�       $��SemiBold"�5�       �� 5�      -   5        $ ChatSearchButton��5�       
 search#��[ 5�      #��[ 5�      $��2��5�      d� 5�      -   5         d�Name$ bntMoreOptions��5�   #     more#��[ 5�   #     ToggleIconButtonStyle#��[ 5�   #   $��3��5�   #   5�      5�      �� 5�      -   5         ��Name$ SearchBoxBorder��5�      ���#dce1de$ 15��5�      $��20q�5�      $��50��5�      $��1��5�      ._�5�      2   ,Study_Step.CustomControls.DropDownListSearch  $��50��68   5�      (   "Study_Step.CustomControls.ChatList  5�      $��2��6*   5�      5�      � 5�      $��1��6   $��1��5�       5�      �� 5�      $
��auto �6$   �� 5�      �� 5�      $
��auto �6$   �� 5�      $
��auto �6$   .+�5�      � $��Center=�6   $��0,0,15,0q�5�      $��75��5�       5�      �� 5�      �� 5�      $��50 �6+   �� 5�      $��50 �6+   �� 5�      $��50 �6+   �� 5�      $��50 �6+   .+�5�      �� #��[  6     ContactInfoButtonStyle#��[ 5�      �� 5�      $��1��6   #��[ 5�       	 	videocall#��[	 5�      �� 5�      $��2��6   #��[ 5�       
 call#��[
 5�      �� 5�      $��3��6   #��[ 5�      #��[ 5�      d� 5�      $��4��6#   #��[ 5�   #   #��[ 5�   #   5�      ,   &Study_Step.CustomControls.Conversation  5�      -	   5          Name$ chatMesseges��5�   .   $��1��5�   .   � 5�      $��3��6   .+�5�      �� ���#dce1de$ 15��5�      $��15, 8q�5�      $��1��5�      ._�5�      �  5�      �� 5�   "   $��* �63   �� 5�   "   $
��Auto �63    5�      �� 5�   "   $
��Auto �60   �� 5�   "   .+�5�      )   #Study_Step.CustomControls.FilesList  $��0��67   $��Stretch=�5�   7   � 5     $��1��6#    5  "   �� 5  &   $��* �67   �� 5  &   $
��Auto �67   .+�5  "   �� -
   5         ��Name$ 
MessageBox��5  *   
������ $��0q�5  *   $��12, 0q�5  *   $��Center=�5  *   $��Stretch=�5  *   $��0��5  *   ��5  *   �� *MessageText+$ PropertyChanged=�5  *   �� 5  "   $��1��6-   $ 
Horizontal=�5  -   .+�5  &   �� -   5        $ AttachButton��5  -   #��[ 5  -    
 attach#��[ 5  -   ��5  -   �� *
ChooseFile+�� 5  &   -   5        $ SendMsgButton��5  -   #��[ 5  -     send#��[ 5  -   ��5  -   �� *SendMesCommand+5   #   5!     5"     5#     5$     ��5&     �� 5'      �� 
TargetType" �6    �� Triggers 5(     �� 5)      �� Binding 6*   �� *ChatIsActive+ �� Value True6K   .a�5*  "   ��  �� Property � y� �� Value$ Visible=�6?   5+     �� 5,      6*   �� *ChatIsActive+	 False6K   .a�5-  "   ��  � y�$ Hidden=�6?   5.     50     52     54     56     58     