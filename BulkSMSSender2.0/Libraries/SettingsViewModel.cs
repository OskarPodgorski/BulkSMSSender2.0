using Settings;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BulkSMSSender2._0
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<string> androidVersionOptions { get; set; } = new()
        {
            "Newest Androids",
            "Newest Androids Alternative",
            "Older Androids",
            "Older Androids Alternative"
        };

        public int AndroidVersion
        {
            get => Loaded.androidCompatibility;
            set
            {
                if (Loaded.androidCompatibility != value)
                {
                    Loaded.androidCompatibility = value;

                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<string> regionOptions { get; set; } = new()
        {
            "Poland"
        };

        public int Region
        {
            get => Loaded.numbersExtractionRegion;
            set
            {
                if (Loaded.numbersExtractionRegion != value)
                {
                    Loaded.numbersExtractionRegion = value;

                    OnPropertyChanged();
                }
            }
        }

        public bool CommandBlock
        {
            get => Loaded.commandBlock;
            set
            {
                if (Loaded.commandBlock != value)
                {
                    Loaded.commandBlock = value;

                    OnPropertyChanged();
                }
            }
        }

        public int DataOptimizationThreshold
        {
            get => Loaded.dataOptimizationThreshold;
            set
            {
                if (Loaded.dataOptimizationThreshold != value)
                {
                    Loaded.dataOptimizationThreshold = value;

                    OnPropertyChanged();
                }
            }
        }

        public bool OlderComputer
        {
            get => Loaded.olderComputer;
            set
            {
                if (Loaded.olderComputer != value)
                {
                    Loaded.olderComputer = value;

                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
