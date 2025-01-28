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
                "Newest Androids",
                "Newest Androids Alternative",
                "Older Androids",
                "Older Androids Alternative"
            };

            regionOptions = new()
            {
                "Poland"
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
