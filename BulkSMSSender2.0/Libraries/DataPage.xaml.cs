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
            openDataFileButton.IsVisible = true;

            EditorTextNoInvoke = string.Empty;
        }
        else
        {
            dataEditor.IsVisible = true;
            acceptButtonBottom.IsVisible = true;
            openDataFileButton.IsVisible = false;

            previousTextLength = Settings.Loaded.data.Length;
            EditorTextNoInvoke = Settings.Loaded.data;
        }
    }

    public async void ClearEditorField(object sender, EventArgs e)
    {
        Settings.Loaded.data = string.Empty;
        EditorTextNoInvoke = Settings.Loaded.data;
        await Settings.Loaded.WriteDataFile();
    }

    private async void OpenDataFileButton(object sender, EventArgs e)
    {
        if (!File.Exists(Settings.Loaded.dataPath))
            File.Create(Settings.Loaded.dataPath).Dispose();


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

            Settings.Loaded.data = Settings.Loaded.ReadDataFile();

            if (answer)
            {
                Settings.Loaded.data = await NumbersExtractor.OptimizeData(Settings.Loaded.data);
                await Settings.Loaded.WriteDataFile();
            }
        }
    }

    private async void InsertFileContentButton(object sender, EventArgs e)
    {
        FileResult? result = await FilePicker.Default.PickAsync();

        if (result != null)
        {
            if (Application.Current?.MainPage != null)
            {
                bool answer = await Application.Current.MainPage.DisplayAlert(
                "Optimization",
                "Do you want to optimize picked file data?",
                "Yes",
                "No"
                );

                if (answer)
                {
                    Settings.Loaded.data += await NumbersExtractor.OptimizeData(SerializeDeserialize.ReadFileContent(result.FullPath));
                }
                else
                    Settings.Loaded.data += SerializeDeserialize.ReadFileContent(result.FullPath);

                await Settings.Loaded.WriteDataFile();
            }

            if (!Settings.Loaded.olderComputer)
            {
                previousTextLength = Settings.Loaded.data.Length;
                EditorTextNoInvoke = Settings.Loaded.data;
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

        if (!Settings.Loaded.olderComputer)
            EditorTextNoInvoke = Settings.Loaded.data;

        await Settings.Loaded.WriteDataFile();
    }

    private void OnUnfocusedEditor(object? sender, EventArgs e) => Settings.Loaded.data = EditorTextNoInvoke;

    private int previousTextLength = 0;
    private bool isOptimizing = false;
    private async void OnEditorTextChanged(object? sender, TextChangedEventArgs e)
    {
        if (isOptimizing) return;

        if (e.NewTextValue.Length > previousTextLength + Settings.Loaded.dataOptimizationThreshold)
        {
            dataEditor.TextChanged -= OnEditorTextChanged;

            isOptimizing = true;

            await OptimizeData(e.OldTextValue, e.NewTextValue);

            previousTextLength = dataEditor.Text.Length;

            Debug.WriteLine("Task");

            isOptimizing = false;

            dataEditor.TextChanged += OnEditorTextChanged;
        }
        else
            previousTextLength = dataEditor.Text.Length;
    }

    private async Task OptimizeData(string oldData, string newData)
    {
        string optimizedData = await NumbersExtractor.OptimizeData(newData);

        if (!string.IsNullOrEmpty(optimizedData))
        {
            dataEditor.Text = oldData + optimizedData;
        }
    }
}