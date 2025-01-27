namespace BulkSMSSender2._0;

public partial class FinalPage : ContentPage
{
    public static FinalPage? ins { get; private set; }

    private readonly NumbersExtractor numbersExtractor = new();

    private List<string> Numbers
    {
        get
        {
            List<string> numbers = new();

            foreach (var child in numbersGrid.Children)
            {
                if (child is Frame frame && frame.Content is HorizontalStackLayout layout)
                {
                    foreach (var innerChild in layout.Children)
                    {
                        if (innerChild is Label label)
                        {
                            numbers.Add(label.Text);
                        }
                    }
                }
            }
            return numbers;
        }
    }

    public FinalPage()
    {
        InitializeComponent();

        ins ??= this;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await RunLoadingLabel(true);
        await numbersExtractor.ExtractNumbersAsync();
    }

    public async Task RunLoadingLabel(bool animationDelay)
    {
        numbersLabel.Text = "Numbers:";
        timeLabel.Text = "Estimated time:";
        alreadyDoneLabel.Text = "Already done numbers:";

        numbersGrid.Children.Clear();
        numbersGrid.RowDefinitions.Clear();
        numbersGrid.ColumnDefinitions.Clear();

        numbersGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

        Label label = new()
        {
            Text = "Extracting numbers ...",
            VerticalOptions = LayoutOptions.Start,
            HorizontalOptions = LayoutOptions.CenterAndExpand,
            FontSize = 26,
            TextColor = Settings.Loaded.colors.yellow
        };

        numbersGrid.Children.Add(label);
        Grid.SetRow(label, 0);
        Grid.SetColumn(label, 0);

        if (animationDelay)
            await Task.Delay(800);
        else
            await Task.Delay(50);
    }

    public void AddNumbers(IEnumerable<NumberPack> numbers)
    {
        numbersLabel.Text = "Numbers:";
        timeLabel.Text = "Estimated time:";
        alreadyDoneLabel.Text = "Already done numbers:";

        numbersGrid.Children.Clear();
        numbersGrid.RowDefinitions.Clear();
        numbersGrid.ColumnDefinitions.Clear();

        numbersGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
        numbersGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });

        int row = 0;
        int col = 0;

        foreach (NumberPack numberPack in numbers)
        {
            if (col == 0)
            {
                numbersGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            }

            AddNumber(numberPack.number, numberPack.validCheck, row, col);

            col++;

            if (col >= 2)
            {
                col = 0;
                row++;
            }
        }

        numbersLabel.Text = $"Numbers:  {numbers.Count()}";
        timeLabel.Text = $"Elapsed time:  {GetElapsedTime(numbers.Count())} hours";
        alreadyDoneLabel.Text = $"Already done numbers:  {Settings.Loaded.AlreadyDoneCount}";
    }

    private float GetElapsedTime(int numbersCount)
    {
        long msTime = (numbersCount * Settings.Loaded.betweenNumbersDelay) + (numbersCount * (Settings.Loaded.messages.Count > 1 ? (Settings.Loaded.messages.Count - 1) * Settings.Loaded.betweenMessagesDelay : 0));

        return MathF.Round(msTime / 3600000f, 1);
    }

    private void AddNumber(string number, bool checkValid, int row, int column)
    {
        HorizontalStackLayout horizontalLayout = new()
        {
            Padding = 0,
            HorizontalOptions = column == 1 ? LayoutOptions.StartAndExpand : LayoutOptions.EndAndExpand
        };

        Label label = new()
        {
            Text = number,
            VerticalOptions = LayoutOptions.FillAndExpand,
            HorizontalOptions = column == 1 ? LayoutOptions.Start : LayoutOptions.End,
            HorizontalTextAlignment = column == 1 ? TextAlignment.Start : TextAlignment.End,
            FontSize = 16,
            TextColor = Settings.Loaded.colors.gray
        };

        horizontalLayout.Children.Add(label);

        Frame frame = new()
        {
            BackgroundColor = checkValid ? Settings.Loaded.colors.yellow : Settings.Loaded.colors.blue,
            HasShadow = false,
            CornerRadius = 8,
            Padding = 8,
            Content = horizontalLayout
        };

        numbersGrid.Children.Add(frame);
        Grid.SetRow(frame, row);
        Grid.SetColumn(frame, column);
    }

    private async void StartSending(object sender, EventArgs e)
    {
        if (MainPage.ins != null && Application.Current != null)
        {
            await Task.WhenAll(Shell.Current.GoToAsync("//progress"), Settings.Loaded.SaveSettingsAsync());

            if (ProgressPage.ins != null)
                Shell.SetTabBarIsVisible(ProgressPage.ins, false);

            await new SMSSending().StartSendBulkAsync(Numbers);
        }
    }

    private async void ClearAlreadyDone(object sender, EventArgs e)
    {
        Settings.Loaded.ClearAlreadyDone();

        await RunLoadingLabel(false);

        await new NumbersExtractor().ExtractNumbersAsync();
    }

    private async void RecalculateButton(object sender, EventArgs e)
    {
        await RunLoadingLabel(false);

        await new NumbersExtractor().ExtractNumbersAsync();
    }
}