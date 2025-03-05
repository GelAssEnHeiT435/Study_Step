using Study_Step.ViewModels;
using System.Windows;
using System.Windows.Controls;


namespace Study_Step.Pages
{
    /// <summary>
    /// Логика взаимодействия для RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : Page
    {
        public RegisterPage(AuthViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private void ChangePassword(object sender, RoutedEventArgs e)
        {
            // Получаем пароль из PasswordBox и передаем его в ViewModel
            PasswordBox passwordBox = (PasswordBox)sender;
            AuthViewModel viewModel = (AuthViewModel)DataContext;
            viewModel.Password = passwordBox.Password;
        }
    }
}
