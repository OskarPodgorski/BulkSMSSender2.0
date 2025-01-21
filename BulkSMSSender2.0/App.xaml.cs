namespace BulkSMSSender2._0
{
    public partial class App : Application
    {
        public static App? ins { get; private set; }

        public CancellationTokenSource cancelToken = new();

        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();

            ins ??= this;
        }
    }
}
