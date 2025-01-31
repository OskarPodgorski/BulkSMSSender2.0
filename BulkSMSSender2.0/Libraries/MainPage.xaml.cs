namespace BulkSMSSender2._0
{
    public partial class MainPage : ContentPage
    {
        public static bool isAppExiting { get; private set; } = false;

        public static MainPage? ins { get; private set; }

        public readonly PhoneConnection phoneConnection;

        public MainPage()
        {
            InitializeComponent();

            ins ??= this;

            phoneConnection = new(connectedPhonesLabel);

            if (Application.Current != null)
                Application.Current.Windows[0].Destroying += OnDestroy;
            else
            {
                Task.Run(async () =>
                {
                    while (Application.Current == null)
                    {
                        await Task.Delay(50);

                        if (Application.Current != null)
                        {
                            Application.Current.Windows[0].Destroying += OnDestroy;
                            break;
                        }
                    }
                });
            }

            LoadSettings();

            StartPhoneConnectionAsync();
        }
        private void LoadSettings()
        {
            numberEntry.Text = Settings.Loaded.singleNumber;
        }
        private void OnDestroy(object? sender, EventArgs e)
        {
            isAppExiting = true;
            Settings.Loaded.SaveSettings();
        }

        private async void StartPhoneConnectionAsync() => await phoneConnection.StartAsync();

        private async void SendSMSOneNumber(object sender, EventArgs e)
        {
            foreach (string message in Settings.Loaded.messages)
            {
                await SMSSending.TrySendAsync(numberEntry.Text, message);
            }
        }

        private void OnUnfocusedEntry(object? sender, EventArgs e) => Settings.Loaded.singleNumber = numberEntry.Text;

        private async void ClearDataButton(object sender, EventArgs e)
        {
            if (DataPage.ins != null)
                DataPage.ins.ClearEditorField(sender, e);
            else
            {
                Settings.Loaded.data = string.Empty;
                await Settings.Loaded.WriteDataFile();
            }
        }
    }
}
