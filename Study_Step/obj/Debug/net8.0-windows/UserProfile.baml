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
��10,0q�5/      .+�50      �� -   5         ��Name$ btnMinimize��50   !   $��Center=�52         minimize#��[  53        IconButtonStyle#��[ 54      �� 56      -   5        $ btnMaximize��56   !   $��Center=�58        maximize#��[ 59      #��[ 5:      �� 5<      -   5        $ btnClose��5<   !   $��Center=�5>       	 close#��[ 5?      #��[ 5@      5B      5C      5D      � 5F      $��1��6   $��1��5G       5H      �� 5I      $
��150* �6$   �� 5K      $	��60* �6$   �� 5L      $��0* �6$   �� 5N      $
��220* �6$   .+�5R      ,   &Study_Step.CustomControls.StatusThumbs  $��0��6.   $��0,20,0,0q�5S   .   �� 5U      $��1��6   .+�5V      �� $��Center=�5W      $��22(�5X      $��SemiBold"�5Y      ��5V       �� *statusThumbsUser.Username+5Y      �� 5[      $��Center=�5\       $��14(�5]       ��5[       �� *statusThumbsUser.Email+5]       5^      � 5`      $��3��6   .+�5a      �� -   5        $ btnStartChat��5a      $	��Top=�5b        allchats#��[ 5c      #��[ 5d      ��5e      �� *DataContext.CreateNewChat+ �� RelativeSource ��  �� AncestorType" ���5f      �� 5g      5j      5k      5l      5m      