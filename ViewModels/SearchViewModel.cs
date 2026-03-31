using System.ComponentModel;

namespace Incident.WPF.ViewModels
{
    public class SearchViewModel(Action onSearchChanged) : INotifyPropertyChanged
    {
        private readonly Action _onSearchChanged = onSearchChanged;

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

        public void Clear()
        {
            Text = string.Empty;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}