namespace BulkSMSSender2._0;

public partial class ProgressPage : ContentPage
{
    public static ProgressPage? ins { get; private set; }
    public ProgressPage()
    {
        InitializeComponent();

        ins ??= this;
    }

    public void AddNumber(string number)
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
            TextColor = Colors.Black
        };

        horizontalLayout.Children.Add(label);

        Frame frame = new()
        {
            BackgroundColor = Color.FromArgb("72a461"),
            HasShadow = false,
            CornerRadius = 10,
            Padding = 8,
            Content = horizontalLayout
        };

        numbersLayout.Children.Add(frame);
    }
}