using Incident.Domain.Entities;
using Incident.Infrastructure.Services.Interfaces;
using Incident.WPF.Commands;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace Incident.WPF.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        // Servicios
        private readonly ITicketService _ticketService;
        // Colecciones
        public ObservableCollection<Ticket>? Tickets
        {
            get;
            set
            {
                field = value;
                OnPropertyChanged(nameof(Tickets));
            }
        }
        public ObservableCollection<User>? Authors
        {
            get;
            set
            {
                field = value;
                OnPropertyChanged(nameof(Authors));
            }
        }
        // Utils
        public SearchViewModel Search { get; }
        public PaginationViewModel Paginator { get; }
        // Comandos
        public ICommand SaveCommand { get; }

        public MainViewModel(ITicketService ticketService)
        {
            _ticketService = ticketService;

            Search = new SearchViewModel(onSearchChanged: OnSearchChanged);
            Paginator = new PaginationViewModel(pageSize: 5, onPageChanged: LoadData);

            SaveCommand = new RelayCommand(
                execute: _ => SaveChanges(),
                canExecute: _ => HasUnsavedChanges()
            );

            LoadData();
        }

        private void OnSearchChanged()
        {
            Paginator.Reset();

            LoadData();
        }

        private async void LoadData()
        {
            if (Authors == null)
            {
                var users = await _ticketService.GetAuthorsAsync();
                Authors = new ObservableCollection<User>(users);
            }

            // Usar estado de Search y Paginator
            var result = await _ticketService.GetTicketsPaginatedAsync(Search.Text, Paginator.CurrentPage, Paginator.PageSize);

            // Actualizar total de páginas
            Paginator.UpdateTotalPages(result.TotalItems);

            Tickets = new ObservableCollection<Ticket>(result.Tickets);
        }

        private void SaveChanges()
        {
            _ticketService.SaveChanges();
            MessageBox.Show("Cambios guardados con éxito.");
        }

        public bool HasUnsavedChanges()
        {
            return _ticketService.HasUnsavedChanges();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

