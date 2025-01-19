using BulkSMSSender2._0.Libraries;

namespace BulkSMSSender2._0
{
    public partial class MainPage : ContentPage
    {
        PhoneConnection phoneConnection = new();
        public MainPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await phoneConnection.StartAsync(connectedPhonesLabel);
        }
    }

}
