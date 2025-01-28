namespace BulkSMSSender2._0;

public partial class ProgressPage : ContentPage
{
    public static ProgressPage? ins { get; private set; }

    public SMSSending? SMSSending;

    public ProgressPage()
    {
        InitializeComponent();

        ins ??= this;
    }

    int row, col;
    public (Label, Frame) AddNumber(string number)
    {
        HorizontalStackLayout horizontalLayout = new()
        {
            Padding = 0,
            Spacing = 15,
            HorizontalOptions = LayoutOptions.CenterAndExpand
        };

        Label label = new()
        {
            Text = number,
            VerticalOptions = LayoutOptions.CenterAndExpand,
            HorizontalOptions = LayoutOptions.Center,
            FontSize = 16,
            TextColor = Colors.Black
        };

        horizontalLayout.Children.Add(label);

        Label progressLabel = new()
        {
            Text = "0",
            VerticalOptions = LayoutOptions.CenterAndExpand,
            HorizontalOptions = LayoutOptions.Center,
            FontSize = 16,
            TextColor = Settings.Loaded.colors.gray
        };

        horizontalLayout.Children.Add(progressLabel);

        Frame frame = new()
        {
            BackgroundColor = Settings.Loaded.colors.blue,
            HasShadow = false,
            CornerRadius = 8,
            Padding = 8,
            Content = horizontalLayout
        };

        if (col == 0)
        {
            numbersGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        }

        numbersGrid.Children.Add(frame);
        Grid.SetRow(frame, row);
        Grid.SetColumn(frame, col);

        col++;

        if (col >= 2)
        {
            col = 0;
            row++;
        }

        return new(progressLabel, frame);
    }

    private void ClearGrid()
    {
        numbersGrid.Children.Clear();
        numbersGrid.RowDefinitions.Clear();
        numbersGrid.ColumnDefinitions.Clear();

        numbersGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
        numbersGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });

        row = 0;
        col = 0;
    }

    private void ClearProgress()
    {
        ClearGrid();

        progressMessagesLabel.Text = $"0 / 0";
        progressNumbersLabel.Text = $"0 / 0";
        progressPercentLabel.Text = "0%";
        connectionStatusLabel.Text = "";
    }

    int allMessagesCount = 0;
    int allNumbersCount = 0;
    int progressMessagesCount = 0;
    int progressNumbersCount = 0;
    float progressPercentMultiplier = 0;
    public void InitializeProgress(int numbersCount, int messagesCount)
    {
        ClearProgress();
        progressMessagesCount = 0;
        progressNumbersCount = 0;

        allMessagesCount = numbersCount * messagesCount;
        allNumbersCount = numbersCount;

        progressPercentMultiplier = 100f / allMessagesCount;

        progressMessagesLabel.Text = $"0 / {allMessagesCount}";
        progressNumbersLabel.Text = $"0 / {allNumbersCount}";
        progressPercentLabel.Text = "0%";
    }

    public void EvaluateMessagesProgress()
    {
        progressMessagesCount++;

        progressMessagesLabel.Text = $"{progressMessagesCount} / {allMessagesCount}";
        progressPercentLabel.Text = $"{MathF.Round(progressMessagesCount * progressPercentMultiplier, 2)}%";
    }
    public void EvaluateNumbersProgress()
    {
        progressNumbersCount++;

        progressNumbersLabel.Text = $"{progressNumbersCount} / {allNumbersCount}";
    }

    private void PauseButton(object sender, EventArgs e) => SMSSending?.PauseBulkSending();
    private void ContinueButton(object sender, EventArgs e) => SMSSending?.ContinueBulkSending();

    private async void AbortButton(object sender, EventArgs e)
    {
        if (SMSSending != null)
        {
            SMSSending.aborted = true;
            await SMSSending.sendingTask;
            SMSSending = null;

            Shell.SetTabBarIsVisible(this, true);

            ClearProgress();
        }
    }

    public void SetDisconnectedLabel()
    {
        connectionStatusLabel.Text = "Disconnected! Paused sending";
        connectionStatusLabel.TextColor = Settings.Loaded.colors.red;
    }
    public void SetConnectedLabel(string deviceInfo)
    {
        connectionStatusLabel.Text = $"Connected {deviceInfo}";
        connectionStatusLabel.TextColor = Settings.Loaded.colors.green;
    }
}