   M S B A M L   `   `   `  ����E  AStudy_Step, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:Study_Step.PagesStudy_Step.Pages  5      =#clr-namespace:Study_Step.ViewModelsStudy_Step.ViewModels  5      !    Study_Step.Pages.SignInPage   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      0localclr-namespace:Study_Step.Pages     5      :
viewmodels#clr-namespace:Study_Step.ViewModels     5        N� Title$  
SignInPage��5	       J� InputBindings 5      �� 5   
   
 �� Key$ Enter��6    �� Command 6!   �� *LoginCommand+.��5      �� $	��200��6   .+�5   
   �� $��Авторизация��6   $��Center=�5      $��22(�5      $
��Bold"�5      $��30��5      �� 5   
   $��Эл. почта��6   $��16(�5      $��SemiBold"�5      $��10, 20, 0, 5q�5      �� 5   
   ���#dce1de �� CornerRadius$ 15��5      $��30��5      ._�5       �� -   5         ��Name$ 
EmailField��5       
������ $��0q�5"      $��12, 0q�5#      $��Stretch=�5$      $��Center=�5%      ��5&      �� *Email+ �� UpdateSourceTrigger$ PropertyChanged=�5'      �� 5)   
   $��Пароль��6   $��16(�5*      $��SemiBold"�5+      $��10, 5q�5,      �� 5.   
   ���#dce1de$ 15��5/      $��30��50      ._�52      G� -   5         G�Name$ PasswordField��52      
������ $��0q�54      $��12, 0q�55      $��Stretch=�56      $��Center=�57      58      59      �� 5<   
   -   5         ��Name$ 	SignInBtn��5<      
������ $��0q�5>      $��0, 20, 0, 5q�5?      $��75��5@      ��5A      �� *LoginCommand+��5B      �� 5C      $��
Войти��6   $��16(�5D      $��SemiBold"�5E      $��Center=�5F      $��Center=�5G      5I      �� 5L   
   
������ $��0q�5M      $	��100��5N      ��5O      �� *NavigateToRegisterPageCommand+��5P      �� 5Q      $��Регистрация��6   $��16(�5R      $��SemiBold"�5S      $��Center=�5T      $��Center=�5U      5W      5X      5Y      