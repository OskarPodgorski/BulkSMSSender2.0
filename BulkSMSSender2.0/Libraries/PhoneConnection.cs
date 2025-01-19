using AdvancedSharpAdbClient;
using AdvancedSharpAdbClient.Models;

namespace BulkSMSSender2._0.Libraries
{
    public sealed class PhoneConnection
    {
        private static string adbPath = @"C:\Users\Strix\Documents\GitHub\BulkSMSSender2.0\BulkSMSSender2.0\adb\adb.exe"; // temporary hardcoded
        private readonly static AdbServer adbServer = new();
        private readonly static AdbClient adbClient = new();

        private static List<DeviceData> devices = new();

        private Label connectedPhonesLabel;

        public async Task StartAsync(Label connectedPhonesLabel)
        {
            this.connectedPhonesLabel = connectedPhonesLabel;

            StartServerResult adbServerResult = adbServer.StartServer(adbPath, restartServerIfNewer: false);

            if (adbServerResult != StartServerResult.Started && adbServerResult != StartServerResult.AlreadyRunning)
            {
                connectedPhonesLabel.Text = "Error starting adb!";
            }

            devices = await CheckConnectionAsync();

            this.connectedPhonesLabel.Text = $"{devices[0].Name} - {devices[0].Serial}";
        }

        private async Task<List<DeviceData>> CheckConnectionAsync()
        {
            IEnumerable<DeviceData> devices = adbClient.GetDevices();

            while (!devices.Any())
            {
                await Task.Delay(500);
                devices = adbClient.GetDevices();
            }
            return devices.ToList();
        }

    }
}
