namespace BulkSMSSender2._0;

public partial class ProgressPage : ContentPage
{
    public static ProgressPage? ins { get; private set; }
    public ProgressPage()
    {
        InitializeComponent();

        ins ??= this;
    }

    public (Label, Frame) AddNumber(string number)
    {
        HorizontalStackLayout horizontalLayout = new()
        {
            Padding = 0,
            Spacing = 15,
            HorizontalOptions = LayoutOptions.FillAndExpand
        };

        Label label = new()
        {
            Text = number,
            VerticalOptions = LayoutOptions.FillAndExpand,
            HorizontalOptions = LayoutOptions.Start,
            FontSize = 16,
            TextColor = Colors.Black
        };

        horizontalLayout.Children.Add(label);

        Label progressLabel = new()
        {
            Text = "0",
            VerticalOptions = LayoutOptions.FillAndExpand,
            HorizontalOptions = LayoutOptions.Start,
            FontSize = 16,
            TextColor = Colors.Black
        };

        horizontalLayout.Children.Add(progressLabel);

        Frame frame = new()
        {
            BackgroundColor = Color.FromArgb("cfdcaa"),
            HasShadow = false,
            CornerRadius = 10,
            Padding = 8,
            Content = horizontalLayout
        };

        numbersLayout.Children.Add(frame);

        return new(progressLabel, frame);
    }

    private void ClearProgressNumbers() => numbersLayout.Children.Clear();

    int allMessagesCount = 0;
    int progressCount = 0;
    float progressPercentMultiplier = 0;
    public void InitializeProgress(int numbersCount, int messagesCount)
    {
        ClearProgressNumbers();
        progressCount = 0;

        allMessagesCount = numbersCount * messagesCount;
        progressPercentMultiplier = 100f / allMessagesCount;

        progressLabel.Text = $"0 / {allMessagesCount}";
        progressPercentLabel.Text = "0%";
    }

    public void EvaluateProgress()
    {
        progressCount++;

        progressLabel.Text = $"{progressCount} / {allMessagesCount}";
        progressPercentLabel.Text = $"{progressCount * progressPercentMultiplier}%";
    }
}