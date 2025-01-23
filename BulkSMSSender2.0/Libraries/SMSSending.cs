using AdvancedSharpAdbClient.DeviceCommands;

namespace BulkSMSSender2._0
{
    public static class SMSSending
    {
        public static async Task SendAsync(string number, string message)
        {
            if (!Settings.Loaded.commandBlock && !PhoneConnection.devicesList.IsNullOrEmpty())
            {
                if (Settings.Loaded.androidCompatibility == 0)
                    await PhoneConnection.adbClient.ExecuteShellCommandAsync(PhoneConnection.devicesList[0], $"service call isms 5 i32 0 s16 \"com.android.mms.service\" s16 \"null\" s16 \"{number}\" s16 \"null\" s16 \"{message}\" s16 \"null\" s16 \"null\" i32 0 i64 0");
                else if (Settings.Loaded.androidCompatibility == 1)
                    await PhoneConnection.adbClient.ExecuteShellCommandAsync(PhoneConnection.devicesList[0], $""); //to search for
                else if (Settings.Loaded.androidCompatibility == 2)
                    await PhoneConnection.adbClient.ExecuteShellCommandAsync(PhoneConnection.devicesList[0], $""); //to search for
            }
        }

        public static async Task SendAsync(IEnumerable<string> numbers, List<string> messages)
        {
            if (ProgressPage.ins != null)
            {
                ProgressPage.ins.ClearProgress();

                float progressMultiplier = 100f / messages.Count;

                foreach (string number in numbers)
                {
                    (Label, Frame) progressTuple = ProgressPage.ins.AddNumber(number);

                    for (int i = 0; i < messages.Count; i++)
                    {
                        await SendAsync(number, messages[i]);

                        progressTuple.Item1.Text = $"{progressMultiplier * i}%";

                        await Task.Delay(Settings.Loaded.betweenMessagesDelay);
                    }

                    progressTuple.Item1.Text = "100%";
                    progressTuple.Item2.BackgroundColor = Color.FromArgb("72a461");

                    await Task.Delay(Settings.Loaded.betweenNumbersDelay);
                }
            }
        }

        //public static async Task SendAsync(string number, IEnumerable<string> messages)
        //{
        //    if (ProgressPage.ins != null)
        //    {
        //        foreach (string message in messages)
        //        {
        //            await SendAsync(number, message);

        //            ProgressPage.ins.AddNumber(number);

        //            await Task.Delay(Settings.Loaded.betweenMessagesDelay);
        //        }
        //    }
        //}
    }
}
