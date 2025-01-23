using System.Diagnostics;

namespace Settings
{
    public static class Loaded
    {
        private readonly static string settingsPath = Path.Combine(AppContext.BaseDirectory, "Settings.json");

        public static string singleNumber = string.Empty;
        public static List<string> messages = new();

        public static string data = string.Empty;

        public static int androidCompatibility = 0;
        public static int numbersExtractionRegion = 0;

        public static string betweenMessagesDelay = "1000";
        public static string betweenNumbersDelay = "1000";
        public static string maxMessagesSafeLock = "10000";

        public static void Load()
        {
            PureDataSettings pureData = SerializeDeserialize.LoadPureDataFile<PureDataSettings>(settingsPath);

            singleNumber = pureData.singleNumber;
            messages = pureData.messages.ToList();

            data = pureData.data;

            androidCompatibility = pureData.androidCompatibility;
            numbersExtractionRegion = pureData.numbersExtractionRegion;

            betweenMessagesDelay = pureData.betweenMessagesDelay.ToString();
            betweenNumbersDelay = pureData.betweenNumbersDelay.ToString();
            maxMessagesSafeLock = pureData.maxMessagesSafeLock.ToString();
        }

        public static void Save()
        {
            PureDataSettings pureData = new()
            {
                singleNumber = singleNumber,
                messages = messages.ToArray(),

                data = data,

                androidCompatibility = androidCompatibility,
                numbersExtractionRegion = numbersExtractionRegion,

                betweenMessagesDelay = betweenMessagesDelay.ParseFastI(),
                betweenNumbersDelay = betweenNumbersDelay.ParseFastI(),
                maxMessagesSafeLock = maxMessagesSafeLock.ParseFastI()
            };

            SerializeDeserialize.SavePureDataFile(pureData, settingsPath);
        }
    }
}
