   M S B A M L   `   `   `  ����E  AStudy_Step, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullE'clr-namespace:Study_Step.CustomControlsStudy_Step.CustomControls  5      =#clr-namespace:Study_Step.ViewModelsStudy_Step.ViewModels  5      9!clr-namespace:Study_Step.CommandsStudy_Step.Commands  5      K*clr-namespace:Study_Step.Data.UIConvertersStudy_Step.Data.UIConverters  5	      -    'Study_Step.CustomControls.OptionalPopup   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      9local'clr-namespace:Study_Step.CustomControls     5      :
viewmodels#clr-namespace:Study_Step.ViewModels     5      6commands!clr-namespace:Study_Step.Commands     5      ;conv*clr-namespace:Study_Step.Data.UIConverters     5	      -   5          >�Name$
  Root��5
       � 	Resources 5         PopupButtonStyle �� 
TargetType �� Property �� Value �� 
TargetType �� CornerRadius � ColumnDefinitions (� Stretch  defaultPath	 �� RelativeSource
 �� Triggers P� Property P� Value��%�  &	        7  �� 5   
   " ��6)   .�5      ��  � ��
 ���� 6+   �� 5       � ��$ 0q�60   �� 5       � ��$
 auto��6'   �� 5       � ��$
 auto��6&   �� 5       � ��$ 2q�6'   �� 5       � �� 5      �� 5      " ��6&   .R�5      �� $ 5��6!   ._�5      � $��4q�6#    5   "   �� 5   &   $��20 �67   �� 5   &   $��* �67   .+�5   "   (� $ Uniform=�6*   $��17��5   *   $��17��65   $��0��5   *   .��5   &   F� #��z��5    +   #��z��5!   +   #��z��5"   +   8��  ��5   +   �� *Icon+5#   +   5$   #   �� 5&   "   $��Center=�5'   ,   $
��Left=�5(   ,   $��14(�5)   ,   $��1��6:   $
��5, 0q�5*   ,   $��SemiBold"�5+   ,   ��5&   ,   �� *Text+5+   ,   5,      ��5-      �� 5.   "   " ��6(   .�5/   &   ��  � �� 6C   �� *
Background+	 �� *TemplatedParent+�� 50   &    � �� 6>   �� *Width+	 �� *TemplatedParent+�� 51   &    � �� 6?   �� *Height+	 �� *TemplatedParent+
 52   &   P� 53   *    �@J�IsMouseOver$
 True��6I   . �54   .   ��  � �� �#E0DFDE6K   55   +   57   #   59      5:      5<      5=      5>      .��5@      � -   5         �Name$ OptPopup��5@      $
��auto��5F   6    � AllowsTransparency$
 True��5G       � 	StaysOpen$ False��6&   ��5A      �� *IsOpen+ �� ElementName$
 Root�� �� Mode$ TwoWay=� � PlacementTarget 5B      �� *PlacementTarget+$
 Root����5C      �� *	Placement+$
 Root�� � HorizontalOffset 5D      �� *HorizontalOffset+$
 Root�� � VerticalOffset 5E      �� *VerticalOffset+$
 Root����5F      �� *Width+$
 Root��.��5H   
   �� 
����������#DDD$��1q�5J      $ 5��5K      ._�5L      �� ��6   �� *	Buttons+$
 Root����5M      �� 5N      .R�5O      �� #��[  5R   !   ��5O   !   �� *Text+��5P   !   �� *	Command+��5Q   !   �� *CommandParameter+5R   !   5S      5U      5V      5W      5X      