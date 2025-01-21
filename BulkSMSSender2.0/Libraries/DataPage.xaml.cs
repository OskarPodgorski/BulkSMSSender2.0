using System.Diagnostics;

namespace BulkSMSSender2._0;

public partial class DataPage : ContentPage
{
    private readonly NumbersExtractor numbersExtractor = new();

    public DataPage()
    {
        InitializeComponent();
    }

    private void ClearEditorField(object sender, EventArgs e)
    {
        siteTextEditor.Text = string.Empty;
    }

    private async void AcceptEditorText(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(siteTextEditor.Text))
        {
            await Shell.Current.GoToAsync("//final");

            numbersExtractor.ExtractNumbers(siteTextEditor.Text);
        }
    }
}