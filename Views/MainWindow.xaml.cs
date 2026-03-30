using Incident.WPF.ViewModels;
using System.ComponentModel;
using System.Windows;

namespace WPF_Desafio
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(MainViewModel viewModel)
        {
            InitializeComponent(); 
            DataContext = viewModel;
        }
        
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
        }
    }
}