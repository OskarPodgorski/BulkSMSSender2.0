namespace BulkSMSSender2._0;

public partial class FinalPage : ContentPage
{
    public static FinalPage? ins { get; private set; }

    public FinalPage()
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
            BackgroundColor = Color.FromArgb("ac99ea"),
            HasShadow = false,
            CornerRadius = 10,
            Padding = 10,
            Content = horizontalLayout
        };

        numbersLayout.Children.Add(frame);
    }
}