using System.Diagnostics;

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
    private void LoadSettings()
    {
        dataEditor.Text = Settings.Loaded.data;
        dataEditor.TextChanged += OnEditorTextChanged;
    }

    public void ClearEditorField(object sender, EventArgs e)
    {
        dataEditor.Text = string.Empty;
        Settings.Loaded.data = dataEditor.Text;
    }

    private async void AcceptEditorText(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Settings.Loaded.data))
            await Shell.Current.GoToAsync("//final");
    }

    private async void OptimizeDataButton(object sender, EventArgs e)
    {
        dataEditor.TextChanged -= OnEditorTextChanged;
        dataEditor.Text = await NumbersExtractor.OptimizeData(dataEditor.Text);
        Settings.Loaded.data = dataEditor.Text;
        dataEditor.TextChanged += OnEditorTextChanged;
    }

    private void OnUnfocusedEditor(object? sender, EventArgs e) => Settings.Loaded.data = dataEditor.Text;

    private string previousText = string.Empty;
    private bool isOptimizing = false;
    private async void OnEditorTextChanged(object? sender, TextChangedEventArgs e)
    {
        dataEditor.TextChanged -= OnEditorTextChanged;

        if (isOptimizing) return;


        if (e.NewTextValue.Length > previousText.Length + Settings.Loaded.dataOptimizationThreshold)
        {
            isOptimizing = true;

            await OptimizeData(e.NewTextValue);
        }

        previousText = dataEditor.Text;

        isOptimizing = false;

        dataEditor.TextChanged += OnEditorTextChanged;
    }

    private async Task OptimizeData(string data)
    {
        string optimizedData = await NumbersExtractor.OptimizeData(data);

        Debug.WriteLine("test");
        if (!string.IsNullOrEmpty(optimizedData))
        {
            await Task.Delay(500);
            dataEditor.Text = previousText + optimizedData;
        }

        isOptimizing = false;
    }
}