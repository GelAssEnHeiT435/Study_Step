   M S B A M L   `   `   `  ����E  AStudy_Step, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:Study_Step.PagesStudy_Step.Pages  5      =#clr-namespace:Study_Step.ViewModelsStudy_Step.ViewModels  5      #    Study_Step.Pages.RegisterPage   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      0localclr-namespace:Study_Step.Pages     5      :
viewmodels#clr-namespace:Study_Step.ViewModels     5        N� Title$  RegisterPage��5	       J� InputBindings 5      �� 5   
   
 �� Key$ Enter��6    �� Command 6!   �� *RegisterCommand+.��5      �� $	��200��6   .+�5   
   �� $��Регистрация��6   $��Center=�5      $��22(�5      $
��Bold"�5      $��30��5      �� 5   
   $��Ваше имя��6   $��16(�5      $��SemiBold"�5      $��10, 20, 0, 5q�5      �� 5   
   ���#dce1de �� CornerRadius$ 15��5      $��30��5      ._�5      �� -   5         ��Name$ UsernameField��5      
������ $��0q�5!      $��12, 0q�5"      $��Stretch=�5#      $��Center=�5$      ��5%      �� *
Username+5&      �� 5(   
   $��Эл. почта��6   $��16(�5)      $��SemiBold"�5*      $
��10,5q�5+      �� 5-   
   ���#dce1de$ 15��5.      $��30��5/      ._�51      �� -   5        $ 
EmailField��51      
������ $��0q�53      $��12, 0q�54      $��Stretch=�55      $��Center=�56      ��57      �� *Email+58      �� 5:   
   $��Пароль��6   $��16(�5;      $��SemiBold"�5<      $��10, 5q�5=      �� 5?   
   ���#dce1de$ 15��5@      $��30��5A      ._�5C      G� -   5         G�Name$ PasswordField��5C      
������ $��0q�5E      $��12, 0q�5F      $��Stretch=�5G      $��Center=�5H      5I      5J      �� 5M   
   -   5         ��Name$ RegisterBtn��5M      
������ $��0q�5O      $��0, 20, 0, 5q�5P      $	��160��5Q      ��5R      �� *RegisterCommand+��5S      �� 5T      $*��$Зарегистрироваться��6   $��16(�5U      $��SemiBold"�5V      $��Center=�5W      $��Center=�5X      5Z      �� 5]   
   
������ $��0q�5^      $	��100��5_      ��5`      �� *NavigateToSignInPageCommand+��5a      �� 5b      $��Вернуться��6   $��16(�5c      $��SemiBold"�5d      $��Center=�5e      $��Center=�5f      5h      5i      5j      