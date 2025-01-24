using Microsoft.Maui.Controls;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

    public void RunLoadingLabel()
    {
        numbersLayout.Children.Clear();

        Label label = new()
        {
            Text = "Extracting numbers...",
            VerticalOptions = LayoutOptions.FillAndExpand,
            HorizontalOptions = LayoutOptions.CenterAndExpand,
            FontSize = 18
        };

        numbersLayout.Children.Add(label);
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
            BackgroundColor = checkValid ? Color.FromArgb("dbad6a") : Color.FromArgb("70C1B3"),
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

            await SMSSending.SendAsync(Numbers, MainPage.ins.Messages);
        }
    }
}