namespace BulkSMSSender2._0;

public partial class DataPage : ContentPage
{
    public static DataPage? ins { get; private set; }

    public DataPage()
    {
        InitializeComponent();

        ins ??= this;

        LoadSettings();
    }
    private void LoadSettings() => dataEditor.Text = Settings.Loaded.data;

    private void ClearEditorField(object sender, EventArgs e)
    {
        dataEditor.Text = string.Empty;
        Settings.Loaded.data = dataEditor.Text;
    }

    private async void AcceptEditorText(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Settings.Loaded.data))
            await Shell.Current.GoToAsync("//final");
    }

    private void OnUnfocusedEditor(object? sender, EventArgs e) => Settings.Loaded.data = dataEditor.Text;
}