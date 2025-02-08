using Newtonsoft.Json.Linq;
using Study_Step.ViewModels;
using Study_Step.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

namespace Study_Step.CustomControls
{
    /// <summary>
    /// Логика взаимодействия для DropDownListSearch.xaml
    /// </summary>
    public partial class DropDownListSearch : UserControl
    {
        public DropDownListSearch()
        {
            InitializeComponent();
        }

        private void ComboBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var tb = (TextBox)e.OriginalSource; // получение объекта ввода текста
            ViewModel viewModel = (ViewModel)DataContext;

            if (tb.SelectionStart != 0)
            {
                SearchUser.IsDropDownOpen = true; // открываем выпадающее меню, когда что-то пишем
                SearchUser.SelectedItem = null; // Если набирается текст сбросить выбраный элемент
            }

            if (tb.SelectionStart == 0 && SearchUser.SelectedItem == null)
            {
                SearchUser.IsDropDownOpen = false; // Если сбросили текст и элемент не выбран, закрываем выпадающее меню
            }

            if (string.IsNullOrEmpty(tb.Text))
            {
                // Если поисковая строка пуста, отображаем все пользователи
                viewModel.UserList = new ObservableCollection<User>(viewModel.users);
            }
            else
            {
                // Фильтруем пользователей по схожести имени
                var filteredUsers = viewModel.users
                                    .Where(u => u.Username.ToLower().Contains(tb.Text.ToLower()))
                                    .ToList();

                viewModel.UserList = new ObservableCollection<User>(filteredUsers);
            }
        }

        private void OpenProfile(object sender, RoutedEventArgs e)
        {
            ViewModel viewModel = (ViewModel)DataContext;

            // Получаем кнопку, которая была нажата
            Button button = sender as Button;

            // Получаем объект User, привязанный к текущему элементу списка
            User user = button?.DataContext as User;

            if (user != null)
            {
                // Создайте новый экземпляр ViewModel для нового окна
                ProfileViewModel profileViewModel = new ProfileViewModel
                {
                    statusThumbsUser = user
                };

                // Создаем окно профиля
                Profile profile_window = new Profile();

                // Устанавливаем DataContext для нового окна профиля
                profile_window.DataContext = profileViewModel; // Привязываем новый ViewModel

                // Теперь MainWindow — главное окно для taskWindow
                profile_window.Owner = Application.Current.MainWindow;

                // Показываем окно
                profile_window.Show();
            }
        }
    }
}