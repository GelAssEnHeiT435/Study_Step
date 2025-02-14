using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Study_Step.Models;
using Study_Step.ViewModels;
using Study_Step.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

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
