using Newtonsoft.Json;
using Study_Step.ViewModels;
using Study_Step.Models;
using Study_Step.Models.DTO;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Study_Step
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(ViewModel viewModel)
        {
            InitializeComponent();
            viewModel.ScrollToEnd += ScrollTo;
            DataContext = viewModel;
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }
        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        private void btnMaximize_Click(object sender, RoutedEventArgs e)
        {
            if(WindowState == WindowState.Normal)
                WindowState = WindowState.Maximized;
            else
                WindowState = WindowState.Normal;
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            ViewModel viewModel = (ViewModel)DataContext;
            if (e.Key == Key.Escape) { 
                viewModel.ChatIsActive = false;
                viewModel.CurrentChat.IsChoosen = false;
                viewModel.CurrentChat = null;
                viewModel.Conversations = null;
            }
        }

        private void ScrollTo() =>
            chatMesseges.scroll.ScrollToEnd();

    }
}