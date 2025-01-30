using System.Diagnostics;

namespace BulkSMSSender2._0;

public partial class DataPage : ContentPage
{
    public static DataPage? ins { get; private set; }

    private string EditorTextNoInvoke
    {
        get
        {
            return dataEditor.Text;
        }
        set
        {
            dataEditor.TextChanged -= OnEditorTextChanged;
            dataEditor.Text = value;
            dataEditor.TextChanged += OnEditorTextChanged;
        }
    }

    public DataPage()
    {
        InitializeComponent();

        ins ??= this;

        LoadSettings();
    }
    private void LoadSettings()
    {
        if (!Settings.Loaded.olderComputer)
            dataEditor.Text = Settings.Loaded.data;

        dataEditor.TextChanged += OnEditorTextChanged;
    }


    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (Settings.Loaded.olderComputer)
        {
            dataEditor.IsVisible = false;
            acceptButtonBottom.IsVisible = false;
        }
        else
        {
            dataEditor.IsVisible = true;
            acceptButtonBottom.IsVisible = true;
        }
    }

    public void ClearEditorField(object sender, EventArgs e)
    {
        Settings.Loaded.data = string.Empty;
        EditorTextNoInvoke = Settings.Loaded.data;
    }

    private async void OpenDataFileButton(object sender, EventArgs e)
    {
        await Launcher.OpenAsync(new OpenFileRequest
        {
            File = new ReadOnlyFile(Settings.Loaded.dataPath),
        });

        if (Application.Current?.MainPage != null)
        {
            bool answer = await Application.Current.MainPage.DisplayAlert(
            "Optimization",
            "Do you want to optimize data?",
            "Yes",
            "No"
            );

            Settings.Loaded.ReadDataFile();

            if (answer)
            {
                Settings.Loaded.data = await NumbersExtractor.OptimizeData(Settings.Loaded.data);
                await Settings.Loaded.WriteDataFileAsync();
            }
        }
    }

    private async void AcceptEditorText(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Settings.Loaded.data))
            await Shell.Current.GoToAsync("//final");
    }

    private async void OptimizeDataButton(object sender, EventArgs e)
    {
        Settings.Loaded.data = await NumbersExtractor.OptimizeData(Settings.Loaded.data);

        if (Settings.Loaded.olderComputer)
            await Settings.Loaded.WriteDataFileAsync();
        else
            EditorTextNoInvoke = Settings.Loaded.data;
    }

    private void OnUnfocusedEditor(object? sender, EventArgs e) => Settings.Loaded.data = EditorTextNoInvoke;

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