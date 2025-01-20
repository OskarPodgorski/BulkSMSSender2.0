namespace BulkSMSSender2._0
{
    public partial class MainPage : ContentPage
    {
        private readonly PhoneConnection phoneConnection;
        public MainPage()
        {
            InitializeComponent();

            phoneConnection = new(connectedPhonesLabel);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await phoneConnection.StartAsync();
        }
    }
}
