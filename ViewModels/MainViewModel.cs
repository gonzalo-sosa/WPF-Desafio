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
        private readonly ITicketService _ticketService;
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
        public PaginationViewModel Paginator { get; }
        public SearchViewModel Search { get; }
        public ICommand SaveCommand { get; }

        public MainViewModel(ITicketService ticketService)
        {
            _ticketService = ticketService;

            // Inicializar Paginador y Búsqueda delegando sus respectivas acciones
            Paginator = new PaginationViewModel(pageSize: 5, onPageChanged: LoadData);
            Search = new SearchViewModel(onSearchChanged: OnSearchChanged);

            SaveCommand = new RelayCommand(
                execute: _ => SaveChanges()
            );

            LoadData();
        }

        private void OnSearchChanged()
        {
            // Resetear la página a 1 para evitar llamadas dobles a LoadData
            Paginator.CurrentPage = 1;

            // Cargar los datos filtrados
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

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

