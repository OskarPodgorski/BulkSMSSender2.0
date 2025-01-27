namespace BulkSMSSender2._0;

public partial class MessagesPage : ContentPage
{
    public static MessagesPage? ins { get; private set; }

    public MessagesPage()
    {
        InitializeComponent();

        ins ??= this;

        LoadSettings();
    }
    private void LoadSettings()
    {
        Messages = Settings.Loaded.messages;
    }

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
    //private string MessageUnification(string message)
    //{
    //    message.Replace
    //}

    private void AddMessageButton(object sender, EventArgs e) => AddMessageEditor(string.Empty);
    private void AddMessageEditor(string message)
    {
        HorizontalStackLayout horizontalLayout = new()
        {
            Spacing = 10,
            HorizontalOptions = LayoutOptions.FillAndExpand,
            VerticalOptions = LayoutOptions.FillAndExpand,
        };

        Editor newMessageEditor = new()
        {
            HeightRequest = 154,
            WidthRequest = 550,
            HorizontalOptions = LayoutOptions.FillAndExpand,
            VerticalOptions = LayoutOptions.FillAndExpand,
            VerticalTextAlignment = TextAlignment.Start,
            HorizontalTextAlignment = TextAlignment.Start,
            FontSize = 17,
            Placeholder = "Type message here:",
            Text = message,
            MaxLength = 160,
            AutoSize = EditorAutoSizeOption.TextChanges
        };
        newMessageEditor.Unfocused += OnUnfocusedEditor;

        Button button = new()
        {
            Text = "Delete",
            HorizontalOptions = LayoutOptions.Start,
            VerticalOptions = LayoutOptions.Center,
            BackgroundColor = Settings.Loaded.colors.red
        };

        button.Clicked += (sender, args) =>
        {
            messagesLayout.Children.Remove(horizontalLayout);
            OnUnfocusedEditor(sender, args);
        };

        horizontalLayout.Children.Add(newMessageEditor);
        horizontalLayout.Children.Add(button);

        messagesLayout.Children.Add(horizontalLayout);
    }

    private void OnUnfocusedEditor(object? sender, EventArgs e) => Settings.Loaded.messages = Messages;
}