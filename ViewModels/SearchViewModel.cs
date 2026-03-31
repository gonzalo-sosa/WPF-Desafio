using System.ComponentModel;

namespace Incident.WPF.ViewModels
{
    public class SearchViewModel : INotifyPropertyChanged
    {
        private readonly Action _onSearchChanged;

        public string? Text
        {
            get => field;
            set
            {
                if (field != value)
                {
                    field = value;
                    OnPropertyChanged(nameof(Text));
                    _onSearchChanged(); // Notificar al padre que la búsqueda cambió
                }
            }
        }

        public SearchViewModel(Action onSearchChanged)
        {
            _onSearchChanged = onSearchChanged;
        }

        public void Clear()
        {
            Text = string.Empty;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}