namespace BulkSMSSender2._0;

public partial class SettingsPage : ContentPage
{
    public SettingsPage()
    {
        InitializeComponent();

        BindingContext = new SettingsViewModel();
    }
}