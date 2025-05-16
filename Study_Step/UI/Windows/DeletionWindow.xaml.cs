using Study_Step.ViewModels;
using System.Windows;

namespace Study_Step.UI.Windows
{
    /// <summary>
    /// Interaction logic for DeletionWindow.xaml
    /// </summary>
    public partial class DeletionWindow : Window
    {
        public DeletionWindow(ViewModel viewmodel)
        {
            InitializeComponent();
            DataContext = viewmodel;
        }
    }
}
