using AdvancedSharpAdbClient.DeviceCommands;
using Settings;

namespace BulkSMSSender2._0
{
    public sealed class SMSSending
    {
        private bool paused = false;
        public bool aborted = false;

        public static string GetAndroidCommand(string number, string message)
        {
            return Loaded.androidCompatibility switch
            {
                0 => $"service call isms 5 i32 0 s16 \"com.android.mms.service\" s16 \"null\" s16 \"{number}\" s16 \"null\" s16 \"{message}\" s16 \"null\" s16 \"null\" i32 0 i64 0",
                1 => $"service call isms 7 i32 0 s16 \"com.android.mms.service\" s16 \"{number}\" s16 \"null\" s16 \"{message}\" s16 \"null\" s16 \"null\"",
                _ => string.Empty,
            };
        }

        public static async Task SetSMSOutgoingLimitAsync()
        {
            if (!Loaded.commandBlock && !PhoneConnection.devicesList.IsNullOrEmpty())
                await PhoneConnection.adbClient.ExecuteShellCommandAsync(PhoneConnection.devicesList[0], $"settings put global sms_outgoing_check_max_count {Loaded.maxMessagesSafeLock}"); // disabled for testing
        }

        public static async Task RestoreDefaultSMSOutgoingLimitAsync()
        {
            if (!Loaded.commandBlock && !PhoneConnection.devicesList.IsNullOrEmpty())
                await PhoneConnection.adbClient.ExecuteShellCommandAsync(PhoneConnection.devicesList[0], $"settings put global sms_outgoing_check_max_count 30"); // disabled for testing
        }

        public static async Task SendAsync(string number, string message)
        {
            if (!Loaded.commandBlock && !PhoneConnection.devicesList.IsNullOrEmpty())
                await PhoneConnection.adbClient.ExecuteShellCommandAsync(PhoneConnection.devicesList[0], GetAndroidCommand(number, message)); // disabled for testing
        }

        private IEnumerator<string>? numbers;
        private int numbersCount;
        private float progressMultiplier;
        public Task? sendingTask { get; private set; }

        public async Task StartSendBulkAsync(List<string> numbersList)
        {
            if (ProgressPage.ins != null)
            {
                ProgressPage.ins.SMSSending = this;

                numbersCount = numbersList.Count;
                progressMultiplier = 100f / Loaded.messages.Count;
                numbers = numbersList.GetEnumerator();

                ProgressPage.ins.InitializeProgress(numbersCount, Loaded.messages.Count);

                sendingTask = ContinueSendBulkAsync();
                await sendingTask;
            }
        }

        public void PauseBulkSending() => paused = true;
        public async void ContinueBulkSending()
        {
            paused = false;

            if (numbers != null && (sendingTask == null || sendingTask.IsCompleted))
            {
                sendingTask = ContinueSendBulkAsync();
                await sendingTask;
            }
        }

        private async Task ContinueSendBulkAsync()
        {
            if (ProgressPage.ins != null && numbers != null)
            {
                Loaded.ConnectAlreadyDoneWriter();

                while (!paused && !aborted && numbers.MoveNext())
                {
                    await Loaded.AppendAlreadyDoneAsync(numbers.Current);

                    (Label, Frame) progressTuple = ProgressPage.ins.AddNumber(numbers.Current);

                    for (int i = 0; i < Loaded.messages.Count && !aborted; i++)
                    {
                        await SendAsync(numbers.Current, Loaded.messages[i]);

                        progressTuple.Item1.Text = $"{MathF.Round(progressMultiplier * i, 2)}%";

                        await Task.Delay(Loaded.betweenMessagesDelay);

                        ProgressPage.ins.EvaluateMessagesProgress();
                    }

                    if (!aborted)
                    {
                        progressTuple.Item1.Text = "100%";
                        progressTuple.Item2.BackgroundColor = Loaded.colors.green;

                        ProgressPage.ins.EvaluateNumbersProgress();

                        if (!paused)
                            await Task.Delay(Loaded.betweenNumbersDelay);
                    }
                }

                Loaded.DisconnectAlreadyDoneWriter();
            }
        }
    }
}
