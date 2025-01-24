namespace BulkSMSSender2._0
{
    public partial class MainPage : ContentPage
    {
        public static bool isAppExiting { get; private set; } = false;

        public static MainPage? ins { get; private set; }

        public readonly PhoneConnection phoneConnection;


        public List<string> Messages
        {
            get
            {
                List<string> messages = new();
                foreach (var child in messagesLayout)
                {
                    if (child is HorizontalStackLayout layout)
                        if (layout.Children[0] is Editor editor && !string.IsNullOrEmpty(editor.Text))
                            messages.Add(editor.Text);
                }
                return messages;
            }
            private set
            {
                messagesLayout.Clear();

                for (int i = 0; i < value.Count; i++)
                {
                    AddMessageEditor(value[i]);
                }
            }
        }

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
            Messages = Settings.Loaded.messages;
            numberEntry.Text = Settings.Loaded.singleNumber;
        }
        private async void StartPhoneConnectionAsync() => await phoneConnection.StartAsync();

        private void OnDestroy(object? sender, EventArgs e)
        {
            isAppExiting = true;
            Settings.Loaded.Save();
        }

        private async void SendSMSOneNumber(object sender, EventArgs e)
        {
            foreach (var child in messagesLayout)
            {
                if (child is Editor editor)
                {
                    await SMSSending.SendAsync(numberEntry.Text, editor.Text);
                }
            }
        }

        private void AddMessageButton(object sender, EventArgs e)
        {
            AddMessageEditor(string.Empty);
        }

        private void AddMessageEditor(string message)
        {
            HorizontalStackLayout horizontalLayout = new()
            {
                Spacing = 20,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand,
            };

            Editor newMessageEditor = new()
            {
                MinimumHeightRequest = 80,
                MinimumWidthRequest = 480,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand,
                VerticalTextAlignment = TextAlignment.Start,
                HorizontalTextAlignment = TextAlignment.Start,
                FontSize = 17,
                Placeholder = "Type message here:",
                Text = message,
                MaxLength = 160,
            };
            newMessageEditor.Unfocused += OnUnfocusedEditor;

            Button button = new()
            {
                Text = "Delete",
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.Center,
                BackgroundColor = Settings.Loaded.colors.violet
            };

            button.Clicked += (sender, args) =>
            {
                messagesLayout.Children.Remove(horizontalLayout);
            };

            horizontalLayout.Children.Add(newMessageEditor);
            horizontalLayout.Children.Add(button);

            messagesLayout.Children.Add(horizontalLayout);
        }

        private void OnUnfocusedEditor(object? sender, EventArgs e) => Settings.Loaded.messages = Messages;
        private void OnUnfocusedEntry(object? sender, EventArgs e) => Settings.Loaded.singleNumber = numberEntry.Text;
    }
}
