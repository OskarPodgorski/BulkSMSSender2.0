namespace BulkSMSSender2._0
{
    public partial class AppShell : Shell
    {
        public static AppShell? ins { get; private set; }
        public AppShell()
        {
            InitializeComponent();

            ins ??= this;
        }

        public void ShowTabBar() => tabs.IsVisible = true;
        public void HideTabBar() => tabs.IsVisible = false;
    }
}
