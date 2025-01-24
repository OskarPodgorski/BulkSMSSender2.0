using AdvancedSharpAdbClient.DeviceCommands;

namespace BulkSMSSender2._0
{
    public sealed class SMSSending
    {
        private bool paused = false;

        public static string GetAndroidCommand(string number, string message)
        {
            return Settings.Loaded.androidCompatibility switch
            {
                0 => $"service call isms 5 i32 0 s16 \"com.android.mms.service\" s16 \"null\" s16 \"{number}\" s16 \"null\" s16 \"{message}\" s16 \"null\" s16 \"null\" i32 0 i64 0",
                _ => string.Empty,
            };
        }

        public static async Task SetSMSOutgoingLimitAsync()
        {
            if (!Settings.Loaded.commandBlock && !PhoneConnection.devicesList.IsNullOrEmpty())
                await PhoneConnection.adbClient.ExecuteShellCommandAsync(PhoneConnection.devicesList[0], $"settings put global sms_outgoing_check_max_count {Settings.Loaded.maxMessagesSafeLock}"); // disabled for testing
        }

        public static async Task RestoreDefaultSMSOutgoingLimitAsync()
        {
            if (!Settings.Loaded.commandBlock && !PhoneConnection.devicesList.IsNullOrEmpty())
                await PhoneConnection.adbClient.ExecuteShellCommandAsync(PhoneConnection.devicesList[0], $"settings put global sms_outgoing_check_max_count 30"); // disabled for testing
        }

        public static async Task SendAsync(string number, string message)
        {
            //if (!Settings.Loaded.commandBlock && !PhoneConnection.devicesList.IsNullOrEmpty())
            //await PhoneConnection.adbClient.ExecuteShellCommandAsync(PhoneConnection.devicesList[0], GetAndroidCommand(number, message)); // disabled for testing
        }

        private IEnumerator<string> numbers;
        List<string> messages;
        private int messagesCount;
        private int numbersCount;
        private float progressMultiplier;

        public async Task StartSendBulkAsync(List<string> numbersList, List<string> messagesList)
        {
            if (ProgressPage.ins != null)
            {
                ProgressPage.ins.SMSSending = this;

                messagesCount = messagesList.Count;
                numbersCount = numbersList.Count;
                progressMultiplier = 100f / messagesCount;
                messages = messagesList;
                numbers = numbersList.GetEnumerator();

                ProgressPage.ins.InitializeProgress(numbersCount, messagesCount);
               
                await ContinueSendBulkAsync();
            }
        }

        public void PauseBulkSending() => paused = true;
        public async void ContinueBulkSending()
        {
            paused = false;

            if (numbers != null)
                await ContinueSendBulkAsync();
        }

        private async Task ContinueSendBulkAsync()
        {
            if (ProgressPage.ins != null)
            {
                await SetSMSOutgoingLimitAsync();

                while (!paused && numbers.MoveNext())
                {
                    (Label, Frame) progressTuple = ProgressPage.ins.AddNumber(numbers.Current);

                    for (int i = 0; i < messages.Count; i++)
                    {
                        await SendAsync(numbers.Current, messages[i]);

                        progressTuple.Item1.Text = $"{MathF.Round(progressMultiplier * i, 2)}%";

                        await Task.Delay(Settings.Loaded.betweenMessagesDelay);

                        ProgressPage.ins.EvaluateMessagesProgress();
                    }

                    progressTuple.Item1.Text = "100%";
                    progressTuple.Item2.BackgroundColor = Settings.Loaded.colors.green;

                    ProgressPage.ins.EvaluateNumbersProgress();

                    if (!paused)
                        await Task.Delay(Settings.Loaded.betweenNumbersDelay);
                }
            }
        }
    }
}
