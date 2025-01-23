using System.Diagnostics;

namespace Settings
{
    public static class Loaded
    {
        private readonly static string settingsPath = Path.Combine(AppContext.BaseDirectory, "Settings.json");

        public static string singleNumber = string.Empty;
        public static List<string> messages = new();

        public static void Load()
        {
            Debug.WriteLine(settingsPath);

            PureDataSettings pureData = SerializeDeserialize.LoadPureDataFile<PureDataSettings>(settingsPath);

            singleNumber = pureData.singleNumber;
            messages = pureData.messages.ToList();
        }

        public static void Save()
        {
            PureDataSettings pureData = new()
            {
                singleNumber = singleNumber,
                messages = messages.ToArray()
            };

            SerializeDeserialize.SavePureDataFile(pureData, settingsPath);
        }
    }
}
