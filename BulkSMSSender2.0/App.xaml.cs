namespace BulkSMSSender2._0
{
    public partial class App : Application
    {
        public static App? ins { get; private set; }

        public CancellationTokenSource cancelToken = new();

        public App()
        {
            InitializeComponent();

            Settings.Loaded.Load();

            ApplyUIColors();

            MainPage = new AppShell();

            ins ??= this;
        }

        public static void ApplyUIColors()
        {
            if (Current != null)
            {
                Current.Resources["MyDarkGray"] = Settings.Loaded.colors.darkGray;
                Current.Resources["MyGray"] = Settings.Loaded.colors.gray;
                Current.Resources["MyViolet"] = Settings.Loaded.colors.violet;
                Current.Resources["MyYellow"] = Settings.Loaded.colors.yellow;
                Current.Resources["MyBlue"] = Settings.Loaded.colors.blue;
                Current.Resources["MyGreen"] = Settings.Loaded.colors.green;
                Current.Resources["MyRed"] = Settings.Loaded.colors.red;
            }
            else
            {
                throw new Exception();
            }
        }
    }
}
