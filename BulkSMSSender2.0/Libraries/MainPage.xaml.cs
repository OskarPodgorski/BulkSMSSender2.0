namespace BulkSMSSender2._0
{
    public partial class MainPage : ContentPage
    {
        public static MainPage? ins { get; private set; }

        public readonly PhoneConnection phoneConnection;

        public string[] Messages
        {
            get
            {
                List<string> messages = new();
                foreach (var child in messagesLayout)
                {
                    if (child is Editor editor)
                    {
                        messages.Add(editor.Text);
                    }
                }
                return messages.ToArray();
            }
        }

        public MainPage()
        {
            InitializeComponent();

            ins ??= this;

            phoneConnection = new(connectedPhonesLabel);

            AddMessageButton(this, EventArgs.Empty);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await phoneConnection.StartAsync();
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
            Editor newMessageEditor = new()
            {
                MinimumHeightRequest = 80,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalTextAlignment = TextAlignment.Start,
                HorizontalTextAlignment = TextAlignment.Start,
                AutoSize = EditorAutoSizeOption.TextChanges,
                Placeholder = "Type message here:",
                MaxLength = 160,
            };

            messagesLayout.Children.Add(newMessageEditor);
        }
    }
}
