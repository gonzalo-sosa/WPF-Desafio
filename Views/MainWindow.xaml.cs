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
            if (DataContext is MainViewModel vm && vm.HasUnsavedChanges())
            {
                var result = MessageBox.Show(
                    "Tienes cambios sin guardar. ¿Estás seguro de que deseas salir y perder los cambios?",
                    "Cambios sin guardar",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.No)
                {
                    e.Cancel = true;
                }
            }
            base.OnClosing(e);
        }
    }
}