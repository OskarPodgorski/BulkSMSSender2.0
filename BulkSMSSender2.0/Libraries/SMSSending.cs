using AdvancedSharpAdbClient.DeviceCommands;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkSMSSender2._0
{
    public sealed class SMSSending
    {
        public async void SendSingleAsync(string number, string message)
        {
            //await PhoneConnection.adbClient.ExecuteShellCommandAsync(PhoneConnection.devicesList[0], $"adb shell service call isms 7 i32 0 s16 \"null\" s16 \"{number}\" s16 \"null\" s16 \"{message}\"");

            //await PhoneConnection.adbClient.ExecuteShellCommandAsync(PhoneConnection.devicesList[0], $"am start -a android.intent.action.SENDTO -d sms:{number} --es sms_body \"{message}\" --ez exit_on_sent true");

            //await Task.Delay(1000);
            //await PhoneConnection.adbClient.ExecuteShellCommandAsync(PhoneConnection.devicesList[0], $"input keyevent 22");
            //await Task.Delay(100);
            //await PhoneConnection.adbClient.ExecuteShellCommandAsync(PhoneConnection.devicesList[0], $"input keyevent 66");

            await PhoneConnection.adbClient.ExecuteShellCommandAsync(PhoneConnection.devicesList[0], $"service call isms 5 i32 0 s16 \"com.android.mms.service\" s16 \"null\" s16 \"{number}\" s16 \"null\" s16 \"{message}\" s16 \"null\" s16 \"null\" i32 0 i64 0");
        }
    }
}
