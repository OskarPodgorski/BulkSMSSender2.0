using AdvancedSharpAdbClient.DeviceCommands;
using AdvancedSharpAdbClient.Models;
using Settings;

namespace BulkSMSSender2._0
{
    public sealed class SMSSending : IDisposable
    {
        public static string GetAndroidCommand(string number, string message)
        {
            return Loaded.androidCompatibility switch
            {
                0 => $"service call isms 5 i32 0 s16 \"com.android.mms.service\" s16 \"null\" s16 \"{number}\" s16 \"null\" s16 \"{message}\" s16 \"null\" s16 \"null\" i32 0 i64 0",
                1 => $"service call isms 7 i32 0 s16 \"com.android.mms.service\" s16 \"null\" s16 \"{number}\" s16 \"null\" s16 \"{message}\" s16 \"null\" s16 \"null\" i32 0 i64 0",
                2 => $"service call isms 5 i32 0 s16 \"com.android.mms.service\" s16 \"{number}\" s16 \"null\" s16 \"{message}\" s16 \"null\" s16 \"null\"",
                3 => $"service call isms 7 i32 0 s16 \"com.android.mms.service\" s16 \"{number}\" s16 \"null\" s16 \"{message}\" s16 \"null\" s16 \"null\"",
                _ => string.Empty,
            };
        }

        public static async Task SetSMSOutgoingLimitAsync()
        {
            if (!Loaded.commandBlock && PhoneConnection.TryGetDevices(out List<DeviceData> devices))
            {
                try
                {
                    await PhoneConnection.adbClient.ExecuteShellCommandAsync(devices[0], $"settings put global sms_outgoing_check_max_count {Loaded.maxMessagesSafeLock}");
                }
                catch { }
            }
        }

        public static async Task RestoreDefaultSMSOutgoingLimitAsync()
        {
            if (!Loaded.commandBlock && PhoneConnection.TryGetDevices(out List<DeviceData> devices))
            {
                try
                {
                    await PhoneConnection.adbClient.ExecuteShellCommandAsync(devices[0], $"settings put global sms_outgoing_check_max_count 30");
                }
                catch { }
            }
        }

        public static async Task<bool> TrySendAsync(string number, string message)
        {
            if (PhoneConnection.TryGetDevices(out List<DeviceData> devices))
            {
                if (Loaded.commandBlock)
                {
                    return true;
                }
                else
                {
                    try
                    {
                        await PhoneConnection.adbClient.ExecuteShellCommandAsync(devices[0], GetAndroidCommand(number, message));
                        return true;
                    }
                    catch { return false; }
                }
            }
            else { return false; }
        }

        private Task sending;
        private bool paused = false;
        private bool aborted = false;

        private IEnumerator<string>? numbers;
        private int numbersCount;
        private float progressMultiplier;

        public SMSSending(List<string> numbersList)
        {
            StartSendBulkAsync(numbersList);
        }

        private async void StartSendBulkAsync(List<string> numbersList)
        {
            if (ProgressPage.ins != null)
            {
                numbersCount = numbersList.Count;
                progressMultiplier = 100f / Loaded.messages.Count;
                numbers = numbersList.GetEnumerator();

                ProgressPage.ins.InitializeProgress(numbersCount, Loaded.messages.Count);

                sending = SendBulkAsync();
                await sending;
            }
        }

        public void PauseBulkSending() => paused = true;
        public void ContinueBulkSending() => paused = false;

        private async Task SendBulkAsync()
        {
            if (ProgressPage.ins != null && MainPage.ins != null && numbers != null)
            {
                MainPage.ins.phoneConnection.DisableConnectionLoop();

                Loaded.ConnectAlreadyDoneWriter();

                await WaitForConnection();

                while (!aborted && numbers.MoveNext())
                {
                    await Loaded.AppendAlreadyDoneAsync(numbers.Current);

                    (Label, Frame) progressTuple = ProgressPage.ins.AddNumber(numbers.Current);

                    bool error = false;
                    for (int i = 0; i < Loaded.messages.Count && !aborted; i++)
                    {
                        if (!await TrySendAsync(numbers.Current, Loaded.messages[i]))
                        {
                            await WaitForConnection();

                            if (!await TrySendAsync(numbers.Current, Loaded.messages[i]))
                            {
                                error = true;
                                break;
                            }
                        }

                        progressTuple.Item1.Text = $"{MathF.Round(progressMultiplier * (i + 1), 2)}%";
                        ProgressPage.ins.EvaluateMessagesProgress();

                        if (i < Loaded.messages.Count - 1)
                            await Task.Delay(Loaded.betweenMessagesDelay);

                        await WaitForConnection();
                    }

                    if (!aborted)
                    {
                        if (!error)
                        {
                            progressTuple.Item1.Text = "100%";
                            progressTuple.Item2.BackgroundColor = Loaded.colors.green;
                        }
                        else
                        {
                            progressTuple.Item1.Text = $"{progressTuple.Item1.Text}   Error!";
                            progressTuple.Item2.BackgroundColor = Loaded.colors.red;
                        }

                        ProgressPage.ins.EvaluateNumbersProgress();

                        if (!paused)
                            await Task.Delay(Loaded.betweenNumbersDelay);
                    }

                    await WaitForUnpause();
                    await WaitForConnection();
                }

                Loaded.DisconnectAlreadyDoneWriter();

                MainPage.ins.phoneConnection.EnableConnectionLoop();
            }
        }

        private async Task WaitForConnection()
        {
            List<DeviceData> devices;

            while (!PhoneConnection.TryGetDevices(out devices) && !aborted)
            {
                ProgressPage.ins?.SetDisconnectedLabel();

                await Task.Delay(500);
            }

            if (!aborted)
                ProgressPage.ins?.SetConnectedLabel($"{devices[0].Model} - {devices[0].Name} - {devices[0].Serial}");
        }

        private async Task WaitForUnpause()
        {
            while (paused && !aborted)
            {
                await Task.Delay(500);
            }
        }

        public async void Dispose()
        {
            paused = false;
            aborted = true;
            await sending;

            if (ProgressPage.ins != null)
            {
                Shell.SetTabBarIsVisible(ProgressPage.ins, true);

                ProgressPage.ins.ClearProgress();

                ProgressPage.ins.SMSSending = null;

                await Shell.Current.GoToAsync("//final");
            }
        }
    }
}
