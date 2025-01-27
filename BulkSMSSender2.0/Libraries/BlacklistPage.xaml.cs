namespace BulkSMSSender2._0;

public partial class BlacklistPage : ContentPage
{
    public BlacklistPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await RunLoadingLabel(true);
        AddNumbers(Settings.Loaded.blacklist);
    }

    private async void AddToBlacklistButton(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(numberEntry.Text))
        {
            Settings.Loaded.blacklist.Add(numberEntry.Text);

            numberEntry.Text = string.Empty;

            await Task.WhenAll(RunLoadingLabel(false), Settings.Loaded.SaveBlacklistAsync());

            AddNumbers(Settings.Loaded.blacklist);
        }
    }

    public async Task RunLoadingLabel(bool animationDelay)
    {
        //numbersLabel.Text = "Numbers:";
        //timeLabel.Text = "Estimated time:";
        //alreadyDoneLabel.Text = "Already done numbers:";

        numbersGrid.Children.Clear();
        numbersGrid.RowDefinitions.Clear();
        numbersGrid.ColumnDefinitions.Clear();

        numbersGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

        Label label = new()
        {
            Text = "Loading blacklist ...",
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

    public void AddNumbers(IEnumerable<string> numbers)
    {
        //numbersLabel.Text = "Numbers:";
        //timeLabel.Text = "Estimated time:";
        //alreadyDoneLabel.Text = "Already done numbers:";

        numbersGrid.Children.Clear();
        numbersGrid.RowDefinitions.Clear();
        numbersGrid.ColumnDefinitions.Clear();

        numbersGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
        numbersGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });

        int row = 0;
        int col = 0;

        foreach (string number in numbers)
        {
            if (col == 0)
            {
                numbersGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            }

            AddNumber(number, row, col);

            col++;

            if (col >= 2)
            {
                col = 0;
                row++;
            }
        }

        //numbersLabel.Text = $"Numbers:  {numbers.Count()}";
        //timeLabel.Text = $"Elapsed time:  {GetElapsedTime(numbers.Count())} hours";
        //alreadyDoneLabel.Text = $"Already done numbers:  {Settings.Loaded.AlreadyDoneCount}";
    }

    private void AddNumber(string number, int row, int column)
    {
        HorizontalStackLayout horizontalLayout = new()
        {
            Padding = 0,
            Spacing = 20,
            HorizontalOptions = column == 1 ? LayoutOptions.StartAndExpand : LayoutOptions.EndAndExpand
        };

        Label label = new()
        {
            Text = number,
            VerticalOptions = LayoutOptions.CenterAndExpand,
            HorizontalOptions = column == 1 ? LayoutOptions.Start : LayoutOptions.End,
            HorizontalTextAlignment = column == 1 ? TextAlignment.Start : TextAlignment.End,
            FontSize = 16,
            TextColor = Settings.Loaded.colors.gray
        };

        Button button = new()
        {
            Text = "Remove -",
            VerticalOptions = LayoutOptions.Center,
            HorizontalOptions = column == 1 ? LayoutOptions.Start : LayoutOptions.End,
            BackgroundColor = Settings.Loaded.colors.green,
            Padding = 0,
            HeightRequest = 24,
            MinimumHeightRequest = 24,
            WidthRequest = 78,
            MinimumWidthRequest = 78,
            Margin = 0,
            BindingContext = number

        };

        button.Clicked += async (sender, args) =>
        {
            if (sender is Button btn && btn.BindingContext is string numberToRemove)
            {
                Settings.Loaded.blacklist.Remove(numberToRemove);

                await Task.WhenAll(RunLoadingLabel(false), Settings.Loaded.SaveBlacklistAsync());

                AddNumbers(Settings.Loaded.blacklist);
            }
        };

        if (column == 1)
        {
            horizontalLayout.Add(label);
            horizontalLayout.Add(button);
        }
        else
        {
            horizontalLayout.Add(button);
            horizontalLayout.Add(label);
        }

        Frame frame = new()
        {
            BackgroundColor = Settings.Loaded.colors.red,
            HasShadow = false,
            CornerRadius = 8,
            Padding = 8,
            Content = horizontalLayout
        };

        numbersGrid.Children.Add(frame);

        Grid.SetRow(frame, row);
        Grid.SetColumn(frame, column);
    }
}