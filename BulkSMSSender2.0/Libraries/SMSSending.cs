using AdvancedSharpAdbClient.DeviceCommands;

namespace BulkSMSSender2._0
{
    public static class SMSSending
    {
        public static async Task SendAsync(string number, string message)
        {
            await PhoneConnection.adbClient.ExecuteShellCommandAsync(PhoneConnection.devicesList[0], $"service call isms 5 i32 0 s16 \"com.android.mms.service\" s16 \"null\" s16 \"{number}\" s16 \"null\" s16 \"{message}\" s16 \"null\" s16 \"null\" i32 0 i64 0");
        }

        public static async Task SendAsync(IEnumerable<string> numbers, IEnumerable<string> messages)
        {
            foreach (string number in numbers)
            {
                foreach (string message in messages)
                {
                    await SendAsync(number, message);
                }
            }
        }

        public static async Task SendAsync(string number, IEnumerable<string> messages)
        {
            foreach (string message in messages)
            {
                await SendAsync(number, message);
            }
        }
    }
}
