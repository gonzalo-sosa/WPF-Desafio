using Incident.WPF.Commands;
using System.ComponentModel;
using System.Windows.Input;

namespace Incident.WPF.ViewModels
{
    public class PaginationViewModel : INotifyPropertyChanged
    {
        private readonly Action _onPageChanged;

        public int CurrentPage
        {
            get => field;
            set
            {
                if (field != value)
                {
                    field = value;
                    OnPropertyChanged(nameof(CurrentPage));
                    OnPropertyChanged(nameof(PageInfo));
                }
            }
        } = 1;

        public int PageSize { get; }

        public int TotalPages
        {
            get => field;
            set
            {
                if (field != value)
                {
                    field = value;
                    OnPropertyChanged(nameof(TotalPages));
                    OnPropertyChanged(nameof(PageInfo));
                }
            }
        } = 1;

        public string PageInfo => $"Pág. {CurrentPage} de {Math.Max(1, TotalPages)}";

        public ICommand NextPageCommand { get; }
        public ICommand PrevPageCommand { get; }

        public PaginationViewModel(int pageSize, Action onPageChanged)
        {
            PageSize = pageSize;
            _onPageChanged = onPageChanged;

            NextPageCommand = new RelayCommand(
                execute: _ => { CurrentPage++; _onPageChanged(); },
                canExecute: _ => CurrentPage < TotalPages
            );

            PrevPageCommand = new RelayCommand(
                execute: _ => { CurrentPage--; _onPageChanged(); },
                canExecute: _ => CurrentPage > 1
            );
        }

        // Método auxiliar para actualizar las páginas totales desde el exterior
        public void UpdateTotalPages(long totalItems)
        {
            TotalPages = (int)Math.Ceiling(totalItems / (double)PageSize);
            if (CurrentPage > TotalPages) CurrentPage = Math.Max(1, TotalPages);
        }

        public void Reset()
        {
            // Solo notificamos si de verdad hay que ir a la página 1
            if (CurrentPage != 1) {
                CurrentPage = 1;
                _onPageChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}