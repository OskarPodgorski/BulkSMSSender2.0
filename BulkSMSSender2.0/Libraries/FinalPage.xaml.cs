namespace BulkSMSSender2._0;

public partial class FinalPage : ContentPage
{
    public static FinalPage? ins { get; private set; }

    private List<string> Numbers
    {
        get
        {
            List<string> numbers = new();

            foreach (var child in numbersLayout.Children)
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

    public async Task RunLoadingLabel()
    {
        numbersLabel.Text = "Numbers:";
        timeLabel.Text = "Estimated time:";
        alreadyDoneLabel.Text = "Already done numbers:";

        numbersLayout.Children.Clear();

        Label label = new()
        {
            Text = "Extracting numbers ...",
            VerticalOptions = LayoutOptions.CenterAndExpand,
            HorizontalOptions = LayoutOptions.CenterAndExpand,
            FontSize = 26,
            TextColor = Settings.Loaded.colors.yellow
        };

        numbersLayout.Children.Add(label);

        await Task.Delay(40);
    }

    public void AddNumbers(IEnumerable<NumberPack> numbers)
    {
        numbersLabel.Text = "Numbers:";
        timeLabel.Text = "Estimated time:";
        alreadyDoneLabel.Text = "Already done numbers:";

        numbersLayout.Children.Clear();

        foreach (NumberPack numberPack in numbers)
        {
            AddNumber(numberPack.number, numberPack.validCheck);
        }

        numbersLabel.Text = $"Numbers:  {numbers.Count()}";
        timeLabel.Text = $"Elapsed time:  {GetElapsedTime(numbers.Count())} hours";
        alreadyDoneLabel.Text = $"Already done numbers:  {Settings.Loaded.alreadyDoneNumbers.Count}";
    }

    private float GetElapsedTime(int numbersCount)
    {
        if (MainPage.ins != null)
        {
            long msTime = (numbersCount * Settings.Loaded.betweenNumbersDelay) + (numbersCount * MainPage.ins.MessagesCount * Settings.Loaded.betweenMessagesDelay);

            return MathF.Round(msTime / 3600000f, 1);
        }

        return 0f;
    }

    private void AddNumber(string number, bool checkValid)
    {
        HorizontalStackLayout horizontalLayout = new()
        {
            Padding = 0,
            HorizontalOptions = LayoutOptions.FillAndExpand
        };

        Label label = new()
        {
            Text = number,
            VerticalOptions = LayoutOptions.FillAndExpand,
            HorizontalOptions = LayoutOptions.Start,
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

        numbersLayout.Children.Add(frame);
    }

    private async void StartSending(object sender, EventArgs e)
    {
        if (MainPage.ins != null)
        {
            await Task.WhenAll(Shell.Current.GoToAsync("//progress"), Settings.Loaded.SaveSettingsAsync());

            await new SMSSending().StartSendBulkAsync(Numbers, MainPage.ins.Messages);
        }
    }

    private async void ClearAlreadyDone(object sender, EventArgs e)
    {
        Settings.Loaded.alreadyDoneNumbers.Clear();

        await RunLoadingLabel();

        await new NumbersExtractor().ExtractNumbersAsync();
    }

    private async void RecalculateButton(object sender, EventArgs e)
    {
        await RunLoadingLabel();

        await new NumbersExtractor().ExtractNumbersAsync();
    }
}