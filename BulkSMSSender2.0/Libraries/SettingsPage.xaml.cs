namespace BulkSMSSender2._0;

public partial class SettingsPage : ContentPage
{
    public SettingsPage()
    {
        InitializeComponent();

        BindingContext = new SettingsViewModel();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        LoadSettings();
    }
    private void LoadSettings()
    {
        androidPicker.SelectedIndex = Settings.Loaded.androidCompatibility;
        regionPicker.SelectedIndex = Settings.Loaded.numbersExtractionRegion;

        androidPicker.SelectedIndexChanged += OnSelectedPicker;
        regionPicker.SelectedIndexChanged += OnSelectedPicker;

        messageDelayEntry.Text = Settings.Loaded.betweenMessagesDelay;
        numbersDelayEntry.Text = Settings.Loaded.betweenNumbersDelay;
        maxMessagesEntry.Text = Settings.Loaded.maxMessagesSafeLock;
    }

    private void OnUnfocusedEntry(object? sender, EventArgs e)
    {
        Settings.Loaded.betweenMessagesDelay = messageDelayEntry.Text;
        Settings.Loaded.betweenNumbersDelay = numbersDelayEntry.Text;
        Settings.Loaded.maxMessagesSafeLock = maxMessagesEntry.Text;
    }

    private void OnSelectedPicker(object? sender, EventArgs e)
    {
        Settings.Loaded.androidCompatibility = androidPicker.SelectedIndex;
        Settings.Loaded.numbersExtractionRegion = regionPicker.SelectedIndex;
    }
}