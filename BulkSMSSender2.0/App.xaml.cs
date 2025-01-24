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
                Current.Resources["MyGray"] = Settings.Loaded.colors.darkGray;
                Current.Resources["MyViolet"] = Settings.Loaded.colors.violet;
                Current.Resources["MyYellow"] = Settings.Loaded.colors.darkGray;
                Current.Resources["MyBlue"] = Settings.Loaded.colors.darkGray;
                Current.Resources["MyGreen"] = Settings.Loaded.colors.darkGray;
            }
            else
            {
                throw new Exception();
            }
        }
    }
}
