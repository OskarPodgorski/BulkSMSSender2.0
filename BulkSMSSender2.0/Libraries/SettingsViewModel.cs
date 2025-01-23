using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BulkSMSSender2._0
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<string> androidVersionOptions { get; set; }
        private string _androidVersion;

        public string AndroidVersion
        {
            get => _androidVersion;
            set
            {
                if (_androidVersion != value)
                {
                    _androidVersion = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<string> regionOptions { get; set; }
        private string _region;

        public string Region
        {
            get => _region;
            set
            {
                if (_region != value)
                {
                    _region = value;
                    OnPropertyChanged();
                }
            }
        }

        public SettingsViewModel()
        {
            androidVersionOptions = new()
            {
                "Android 13 to 14",
                "Android 9 to 12",
                "Android 5 to 9"
            };

            regionOptions = new()
            {
                "Poland",
                "Ukraine",
                "America"
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
