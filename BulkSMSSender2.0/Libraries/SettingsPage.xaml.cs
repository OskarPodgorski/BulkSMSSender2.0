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
        messageDelayEntry.Text = Settings.Loaded.betweenMessagesDelay.ToString();
        numbersDelayEntry.Text = Settings.Loaded.betweenNumbersDelay.ToString();
        maxMessagesEntry.Text = Settings.Loaded.maxMessagesSafeLock.ToString();

        charTableEntry.Text = Settings.Loaded.charFormulaSerialized;
    }

    private void OnUnfocusedEntry(object? sender, EventArgs e)
    {
        Settings.Loaded.betweenMessagesDelay = messageDelayEntry.Text.ParseFastI();
        Settings.Loaded.betweenNumbersDelay = numbersDelayEntry.Text.ParseFastI();
        Settings.Loaded.maxMessagesSafeLock = maxMessagesEntry.Text.ParseFastI();
    }

    private void OnUnfocusedCharFormulaEntry(object? sender, EventArgs e)
    {
        Settings.Loaded.InsertCharsFromCharFormula(charTableEntry.Text);
    }

    private async void RestoreOutgoingLimitButton(object sender, EventArgs e) => await SMSSending.RestoreDefaultSMSOutgoingLimitAsync();
    private async void SetOutgoingLimitButton(object sender, EventArgs e) => await SMSSending.SetSMSOutgoingLimitAsync();
}