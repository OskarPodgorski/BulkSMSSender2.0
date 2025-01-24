using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Windows.UI;
using Colors = Microsoft.UI.Colors;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace BulkSMSSender2._0.WinUI
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : MauiWinUIApplication
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
        }

        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

        //protected override void OnLaunched(LaunchActivatedEventArgs args)
        //{
        //    base.OnLaunched(args);

        //    var mainWindow = Application.Windows[0].Handler.PlatformView as Microsoft.UI.Xaml.Window;
        //    if (mainWindow != null)
        //    {
        //        var appWindow = AppWindow.GetFromWindowId(Win32Interop.GetWindowIdFromWindow(WinRT.Interop.WindowNative.GetWindowHandle(mainWindow)));
        //        if (appWindow != null)
        //        {
        //            var titleBar = appWindow.TitleBar;
        //            titleBar.ForegroundColor = Colors.White; // Kolor tekstu
        //            titleBar.BackgroundColor = Colors.Wheat; // Kolor tła
        //        }
        //    }
        //}
    }

}
