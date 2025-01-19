using AdvancedSharpAdbClient;
using AdvancedSharpAdbClient.Models;
using AdvancedSharpAdbClient.Receivers;

namespace BulkSMSSender2._0.Libraries
{
    internal class PhoneConnection
    {
        private static string adbPath = @"C:\Users\Strix\Documents\GitHub\BulkSMSSender2.0\BulkSMSSender2.0\adb\adb.exe"; // temporary hardcoded
        private readonly static AdbServer adbServer = new();
        private readonly static AdbClient adbClient = new();

        private static List<DeviceData> devices = new();

        public void CheckConnectionAtStart()
        {
            StartServerResult adbServerResult = adbServer.StartServer(adbPath, restartServerIfNewer: false);

            if (adbServerResult != StartServerResult.Started && adbServerResult != StartServerResult.AlreadyRunning)
            {

                return;
            }

            devices = CheckConnection();

            adbClient.ExecuteRemoteCommand("echo 'Hello from ADB'", devices[0], new ConsoleOutputReceiver());
        }

        public List<DeviceData> CheckConnection()
        {
            IEnumerable<DeviceData> devices;

            do
            {
                devices = adbClient.GetDevices();
            }
            while (!devices.Any());
            {
                devices = adbClient.GetDevices();
            }
            return devices.ToList();
        }

    }
}
