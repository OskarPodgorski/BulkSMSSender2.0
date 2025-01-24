using AdvancedSharpAdbClient.DeviceCommands;

namespace BulkSMSSender2._0
{
    public static class SMSSending
    {
        public static string GetAndroidCommand(string number, string message)
        {
            return Settings.Loaded.androidCompatibility switch
            {
                0 => $"service call isms 5 i32 0 s16 \"com.android.mms.service\" s16 \"null\" s16 \"{number}\" s16 \"null\" s16 \"{message}\" s16 \"null\" s16 \"null\" i32 0 i64 0",
                _ => string.Empty,
            };
        }

        public static async Task SendAsync(string number, string message)
        {
            if (!Settings.Loaded.commandBlock && !PhoneConnection.devicesList.IsNullOrEmpty())
            {
                //await PhoneConnection.adbClient.ExecuteShellCommandAsync(PhoneConnection.devicesList[0], GetAndroidCommand(number, message)); // disabled for testing
            }
        }

        public static async Task SendAsync(IEnumerable<string> numbers, List<string> messages)
        {
            if (ProgressPage.ins != null)
            {
                ProgressPage.ins.InitializeProgress(numbers.Count(), messages.Count);

                float progressMultiplier = 100f / messages.Count;

                foreach (string number in numbers)
                {
                    (Label, Frame) progressTuple = ProgressPage.ins.AddNumber(number);

                    for (int i = 0; i < messages.Count; i++)
                    {
                        await SendAsync(number, messages[i]);

                        progressTuple.Item1.Text = $"{MathF.Round(progressMultiplier * i, 2)}%";

                        await Task.Delay(Settings.Loaded.betweenMessagesDelay);

                        ProgressPage.ins.EvaluateMessagesProgress();
                    }

                    progressTuple.Item1.Text = "100%";
                    progressTuple.Item2.BackgroundColor = Color.FromArgb("a1c349");

                    ProgressPage.ins.EvaluateNumbersProgress();

                    await Task.Delay(Settings.Loaded.betweenNumbersDelay);
                }
            }
        }
    }
}
