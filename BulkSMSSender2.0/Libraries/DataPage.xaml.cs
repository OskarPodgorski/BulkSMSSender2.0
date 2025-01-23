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
        dataEditor.Text = string.Empty;
    }

    private async void AcceptEditorText(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(dataEditor.Text))
        {
            await Shell.Current.GoToAsync("//final");

            await numbersExtractor.ExtractNumbersAsync(dataEditor.Text);
        }
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();

        LoadSettings();
    }
    private void LoadSettings()
    {
        dataEditor.Text = Settings.Loaded.data;
    }

    private void OnUnfocusedEditor(object? sender, EventArgs e) => Settings.Loaded.data = dataEditor.Text;
}