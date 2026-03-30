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

        private ObservableCollection<Ticket>? _tickets;
        public ObservableCollection<Ticket>? Tickets
        {
            get => _tickets;
            set
            {
                _tickets = value;
                OnPropertyChanged(nameof(Tickets));
            }
        }

        private ObservableCollection<User>? _authors;
        public ObservableCollection<User>? Authors
        {
            get => _authors;
            set
            {
                _authors = value;
                OnPropertyChanged(nameof(Authors));
            }
        }

        private int _currentPage = 1;
        private int _pageSize = 5;
        private int _totalPages = 1;

        public int CurrentPage
        {
            get => _currentPage;
            set { if (_currentPage != value) { _currentPage = value; OnPropertyChanged(nameof(CurrentPage)); OnPropertyChanged(nameof(PageInfo)); } }
        }

        public int TotalPages
        {
            get => _totalPages;
            set { if (_totalPages != value) { _totalPages = value; OnPropertyChanged(nameof(TotalPages)); OnPropertyChanged(nameof(PageInfo)); } }
        }

        public string PageInfo => $"Pág. {CurrentPage} de {Math.Max(1, TotalPages)}";

        public ICommand SaveCommand { get; }
        public ICommand NextPageCommand { get; }
        public ICommand PrevPageCommand { get; }

        public MainViewModel(ITicketService ticketService)
        {
            _ticketService = ticketService;
            
            SaveCommand = new RelayCommand(
                execute: _ => SaveChanges()
            );
            NextPageCommand = new RelayCommand(
                execute: _ => { if (CurrentPage < TotalPages) { CurrentPage++; LoadData(); } }
            );
            PrevPageCommand = new RelayCommand(
                execute: _ => { if (CurrentPage > 1) { CurrentPage--; LoadData(); } }
            );

            LoadData();
        }

        private async void LoadData() 
        {
            if (Authors == null)
            {
                var users = await _ticketService.GetAuthorsAsync();
                Authors = new ObservableCollection<User>(users);
            }

            var result = await _ticketService.GetTicketsPaginatedAsync(SearchText, CurrentPage, _pageSize);

            TotalPages = (int)Math.Ceiling(result.TotalItems / (double)_pageSize);
            if (CurrentPage > TotalPages) CurrentPage = Math.Max(1, TotalPages);

            Tickets = new ObservableCollection<Ticket>(result.Tickets);
        }

        private void SaveChanges() {
            _ticketService.SaveChanges();
            MessageBox.Show("Cambios guardados con éxito.");
        }

        public string? SearchText
        {
            get => field;
            set
            {
                if (field != value)
                {
                    field = value;
                    OnPropertyChanged(nameof(SearchText));
                    CurrentPage = 1; // Reiniciar página al buscar
                    LoadData();
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

