   M S B A M L   `   `   `  ����E  AStudy_Step, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:Study_Step
Study_Step  5   	   E'clr-namespace:Study_Step.CustomControlsStudy_Step.CustomControls  5   	   =#clr-namespace:Study_Step.ViewModelsStudy_Step.ViewModels  5	   	       Study_Step.UserProfile   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   *localclr-namespace:Study_Step     5   	   Bcustomcontrols'clr-namespace:Study_Step.CustomControls     5   	   :
viewmodels#clr-namespace:Study_Step.ViewModels     5	   	   -   5          �Name$
  Prof��5   	    � Title$ Profile��5   	    � AllowsTransparency$
 True��5   	   
������ $��15(�5   	    � 
ResizeMode$ NoResize=�6    � WindowStyle$
 None=�5   	    � WindowStartupLocation$ CenterScreen=�6   $	��480��5   	   $	��320��6   .��5      ��  �� CornerRadius$ 20��6   
�������._�5   
   �  � RowDefinitions 5      �� 5      $
��auto �6    �� 5       � ColumnDefinitions 5      �� 5      �� 5      $	��250 �6#   �� 5      .+�5       �� -   5        $ 20, 20, 0, 0��5       $��3��5!      ���#49a078._�5$      � .+�5%      �� $
��Left=�5&      $
��15,0q�5'      $��Center=�5(      
�������$��22(�5*      $
��Bold"�5+      ��5%       �� *Title+	 �� ElementName$
	 Prof��5+      �� 5-      
 �� Orientation$
 
Horizontal=�6!   $��Right=�5.      $
��10,0q�5/      .+�50      �� -   5         ��Name$ btnMinimize��50   !   $��Center=�52         minimize#��[  53        IconButtonStyle#��[ 54      
 � Tag 55      �� 56   "    �� Width$ 20Q�6'    �� Height$ 20Q�62   58      �� 5:      -   5        $ btnMaximize��5:   !   $��Center=�5<        maximize#��[ 5=      #��[ 5>       5?      �� 5@   "   $ 20Q�6'   $ 20Q�62   5B      �� 5D      -   5        $ btnClose��5D   !   $��Center=�5F   !    	 close#��[ 5G   !    5H      �� 5I   "   $ 20Q�6'   $ 20Q�62   ��5K      �� 5L   "    �� 
TargetType" ��6(    �� BasedOn# [ 6<    �� Triggers 5M   &   P� 5N   *    P� Property �@J�IsMouseOver P� Value$
 True��6I   . �5O   .   ��  �� Property � �� �� Value �#FF73736K   5P   +   5R   #   5T      5V      5W      5X      � 5Z      $��1��6   $��1��5[       5\      �� 5]      $
��150* �6$   �� 5_      $	��60* �6$   �� 5`      $��0* �6$   �� 5b      $
��220* �6$   .+�5f      ,   &Study_Step.CustomControls.StatusThumbs  $��0��6.   $��0,20,0,0q�5g   .   �� 5i      $��1��6   .+�5j      �� $��Center=�5k      $��22(�5l      $��SemiBold"�5m      ��5j       �� *statusThumbsUser.Username+5m      �� 5o      $��Center=�5p       $��14(�5q       ��5o       �� *statusThumbsUser.Email+5q       5r      � 5t      $��3��6   .+�5u      �� -   5        $ btnStartChat��5u      $	��Top=�5v        allchats#��[ 5w      #��[ 5x      ��5y      �� *DataContext.CreateNewChat+ �� RelativeSource ��  �� AncestorType" ���5z      ��  5{      �� 5|      $ 20Q�6#   $ 20Q�6.   5~      5      5�      5�      5�      5�      