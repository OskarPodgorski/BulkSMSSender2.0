namespace BulkSMSSender2._0;

public partial class FinalPage : ContentPage
{
    public static FinalPage? ins { get; private set; }

    public FinalPage()
    {
        InitializeComponent();

        ins ??= this;
    }

    public void AddNumbers(IEnumerable<NumberPack> numbers)
    {
        numbersLayout.Children.Clear();

        foreach (NumberPack numberPack in numbers)
        {
            AddNumber(numberPack.number, numberPack.validCheck);
        }
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
            TextColor = Colors.Black
        };

        horizontalLayout.Children.Add(label);

        Frame frame = new()
        {
            BackgroundColor = checkValid ? Color.FromArgb("dbc975") : Color.FromArgb("ac99ea"),
            HasShadow = false,
            CornerRadius = 10,
            Padding = 8,
            Content = horizontalLayout
        };

        numbersLayout.Children.Add(frame);
    }

    private async void StartSending(object sender, EventArgs e)
    {
        if (MainPage.ins != null)
        {
            await Shell.Current.GoToAsync("//progress");

            List<string> messages = MainPage.ins.Messages;

            foreach (var child in numbersLayout.Children)
            {
                if (child is Frame frame && frame.Content is HorizontalStackLayout layout)
                {
                    foreach (var innerChild in layout.Children)
                    {
                        if (innerChild is Label label)
                        { 
                            await SMSSending.SendAsync(label.Text, messages);
                        }
                    }
                }
            }
        }
    }
}