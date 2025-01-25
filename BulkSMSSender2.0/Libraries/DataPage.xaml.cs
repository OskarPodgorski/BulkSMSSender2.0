namespace BulkSMSSender2._0;

public partial class DataPage : ContentPage
{
    public static DataPage? ins { get; private set; }

    private readonly NumbersExtractor numbersExtractor = new();

    public string Data => dataEditor.Text;

    public DataPage()
    {
        InitializeComponent();

        ins ??= this;

        LoadSettings();
    }

    private void ClearEditorField(object sender, EventArgs e)
    {
        dataEditor.Text = string.Empty;
    }

    private async void AcceptEditorText(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Data))
        {
            await Shell.Current.GoToAsync("//final");

            FinalPage.ins?.RunLoadingLabel();

            await numbersExtractor.ExtractNumbersAsync();
        }
    }
    private void LoadSettings()
    {
        dataEditor.Text = Settings.Loaded.data;
    }

    private void OnUnfocusedEditor(object? sender, EventArgs e) => Settings.Loaded.data = dataEditor.Text;
}