namespace BulkSMSSender2._0;

public partial class FinalPage : ContentPage
{
    public static FinalPage? ins { get; private set; }

    private bool forceReExtract = false;

    private List<string> Numbers
    {
        get
        {
            List<string> numbers = new();

            if (Settings.Loaded.olderComputer)
            {
                foreach (var child in numbersGrid.Children)
                {
                    if (child is HorizontalStackLayout layout)
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
            }
            else
            {
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

        await new NumbersExtractor().ExtractNumbersAsync();

        startSendingButtonTop.BackgroundColor = Settings.Loaded.colors.green;
        startSendingButtonBottom.BackgroundColor = Settings.Loaded.colors.green;

        startSendingButtonTop.Text = "Start sending";
        startSendingButtonBottom.Text = "Start sending";

        forceReExtract = false;
    }

    private void ClearGrid()
    {
        numbersGrid.Children.Clear();
        numbersGrid.RowDefinitions.Clear();
        numbersGrid.ColumnDefinitions.Clear();
    }

    public async Task RunLoadingLabel(bool animationDelay)
    {
        numbersLabel.Text = "Numbers:";
        timeLabel.Text = "Estimated time:";
        alreadyDoneLabel.Text = "Already done numbers:";

        ClearGrid();

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

        ClearGrid();

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
            Padding = Settings.Loaded.olderComputer ? 8 : 0,
            Spacing = 65,
            HorizontalOptions = column == 1 ? LayoutOptions.StartAndExpand : LayoutOptions.EndAndExpand,
            VerticalOptions = LayoutOptions.Center
        };

        Label label = new()
        {
            Text = number,
            VerticalOptions = LayoutOptions.Center,
            HorizontalOptions = column == 1 ? LayoutOptions.Start : LayoutOptions.End,
            HorizontalTextAlignment = column == 1 ? TextAlignment.Start : TextAlignment.End,
            FontSize = 16,
            TextColor = Settings.Loaded.olderComputer ? checkValid ? Settings.Loaded.colors.yellow : Colors.White : Settings.Loaded.colors.gray
        };

        Button button = new()
        {
            Text = "Add to blacklist +",
            VerticalOptions = LayoutOptions.Center,
            HorizontalOptions = column == 1 ? LayoutOptions.Start : LayoutOptions.End,
            BackgroundColor = Settings.Loaded.colors.red,
            Padding = 0,
            HeightRequest = 24,
            MinimumHeightRequest = 24,
            WidthRequest = 130,
            MinimumWidthRequest = 130,
            Margin = 0,
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

        if (!Settings.Loaded.olderComputer)
        {
            Frame frame = new()
            {
                BackgroundColor = checkValid ? Settings.Loaded.colors.yellow : Settings.Loaded.colors.blue,
                HasShadow = false,
                CornerRadius = 8,
                Padding = 8,
                Content = horizontalLayout
            };

            button.Clicked += (sender, args) =>
            {
                frame.BackgroundColor = Settings.Loaded.colors.red;
                ButtonClicked();
            };

            numbersGrid.Children.Add(frame);
            Grid.SetRow(frame, row);
            Grid.SetColumn(frame, column);
        }
        else
        {
            button.Clicked += (sender, args) =>
            {
                ButtonClicked();
            };

            numbersGrid.Children.Add(horizontalLayout);
            Grid.SetRow(horizontalLayout, row);
            Grid.SetColumn(horizontalLayout, column);
        }

        async void ButtonClicked()
        {
            Settings.Loaded.blacklist.Add(number);

            forceReExtract = true;

            startSendingButtonTop.BackgroundColor = Settings.Loaded.colors.blue;
            startSendingButtonBottom.BackgroundColor = Settings.Loaded.colors.blue;

            startSendingButtonTop.Text = "ReExtract before sending";
            startSendingButtonBottom.Text = "ReExtract before sending";

            await Settings.Loaded.SaveBlacklistAsync();

            button.IsEnabled = false;
        }
    }

    private async void StartSending(object sender, EventArgs e)
    {
        if (!forceReExtract)
        {
            await Task.WhenAll(Shell.Current.GoToAsync("//progress"), Settings.Loaded.SaveSettingsAsync());

            if (ProgressPage.ins != null)
            {
                Shell.SetTabBarIsVisible(ProgressPage.ins, false);

                ProgressPage.ins.SMSSending = new SMSSending(Numbers);
            }
        }
        else
            RecalculateButton(sender, e);
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

        startSendingButtonTop.BackgroundColor = Settings.Loaded.colors.green;
        startSendingButtonBottom.BackgroundColor = Settings.Loaded.colors.green;

        startSendingButtonTop.Text = "Start sending";
        startSendingButtonBottom.Text = "Start sending";

        forceReExtract = false;
    }
}