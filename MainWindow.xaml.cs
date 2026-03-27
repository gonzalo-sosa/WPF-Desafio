using Incident.Domain.Entities;
using Incident.Infrastructure.Data;
using Incident.Infrastructure.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;

namespace WPF_Desafio
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly AppDbContext _context;
        private readonly CollectionViewSource ticketsViewSource;

        public MainWindow(AppDbContext context)
        {
            InitializeComponent();
            ticketsViewSource = (CollectionViewSource)FindResource(nameof(ticketsViewSource));
            _context = context;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataSeeder.Initialize(_context);

            _context.Tickets
                .Include(t => t.Author)
                .Include(t => t.Comments!)
                    .ThenInclude(c => c.Author)
                .Load();

            ticketsViewSource.Source = _context.Tickets.Local.ToObservableCollection();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _context.SaveChanges();

            ticketsDataGrid.Items.Refresh();
            commentsDataGrid.Items.Refresh();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
        }
    }
}