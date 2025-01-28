using AdvancedSharpAdbClient;
using AdvancedSharpAdbClient.Models;

namespace BulkSMSSender2._0
{
    public sealed class PhoneConnection(Label connectedPhonesLabel)
    {
        //private static string adbPath = @"C:\Users\Strix\Documents\GitHub\BulkSMSSender2.0\BulkSMSSender2.0\adb\adb.exe"; // temporary hardcoded

        private readonly static string adbPath = @"C:\adb\adb.exe";

        private readonly static AdbServer adbServer = new();
        public readonly static AdbClient adbClient = new();

        public Task connectionLoop;
        private bool disableConnectionLoop = false;

        public static List<DeviceData> devicesList { get; private set; } = new();

        public async Task StartAsync()
        {
            await Task.Delay(200);

            if (File.Exists(adbPath))
            {
                StartServerResult adbServerResult = adbServer.StartServer(adbPath, restartServerIfNewer: false);

                if (adbServerResult != StartServerResult.Started && adbServerResult != StartServerResult.AlreadyRunning)
                {
                    connectedPhonesLabel.Text = "Error starting adb!";
                }
                else
                {
                    connectionLoop = CheckConnectionAsync();
                    await connectionLoop;
                }
            }
            else
            {
                connectedPhonesLabel.Text = "No ADB libraries!";
            }
        }

        public async void DisableConnectionLoop()
        {
            disableConnectionLoop = true;

            if (connectionLoop != null)
            {
                await connectionLoop;
            }
        }

        public async void EnableConnectionLoop()
        {
            disableConnectionLoop = false;
            connectionLoop = CheckConnectionAsync();
            await connectionLoop;
        }

        private async Task CheckConnectionAsync()
        {
            IEnumerable<DeviceData> devices;
            bool connected = false;

            while (!disableConnectionLoop || !MainPage.isAppExiting)
            {
                devices = adbClient.GetDevices();

                if (devices.Any())
                {
                    if (!connected)
                    {
                        devicesList = devices.ToList();
                        connectedPhonesLabel.Text = $"Connected: {devicesList[0].Model} - {devicesList[0].Name} - {devicesList[0].Serial}";
                        connected = true;
                    }
                    await Task.Delay(1000);
                }
                else
                {
                    if (connected)
                    {
                        connectedPhonesLabel.Text = "Phone disconnected!";
                        await Task.Delay(2000);
                        connectedPhonesLabel.Text = "Waiting for phone to be connected...";
                        connected = false;
                    }
                    await Task.Delay(250);
                }
            }
        }

        public static List<DeviceData> GetDevices() => devicesList = adbClient.GetDevices().ToList();
    }
}
