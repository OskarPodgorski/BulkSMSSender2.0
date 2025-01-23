﻿namespace Settings
{
    public static class Loaded
    {
        private readonly static string settingsPath = Path.Combine(AppContext.BaseDirectory, "Settings.json");

        public static bool commandBlock = false;

        public static string singleNumber = string.Empty;
        public static List<string> messages = new();

        public static int androidCompatibility = 0;
        public static int numbersExtractionRegion = 0;

        public static int betweenMessagesDelay = 2000;
        public static int betweenNumbersDelay = 1000;
        public static int maxMessagesSafeLock = 10000;

        public static string data = string.Empty;

        public static void Load()
        {
            PureDataSettings pureData = SerializeDeserialize.LoadPureDataFile<PureDataSettings>(settingsPath);

            commandBlock = pureData.commandBlock;

            singleNumber = pureData.singleNumber;
            messages = pureData.messages.ToList();

            androidCompatibility = pureData.androidCompatibility;
            numbersExtractionRegion = pureData.numbersExtractionRegion;

            betweenMessagesDelay = pureData.betweenMessagesDelay;
            betweenNumbersDelay = pureData.betweenNumbersDelay;
            maxMessagesSafeLock = pureData.maxMessagesSafeLock;

            data = pureData.data;
        }

        public static void Save()
        {
            PureDataSettings pureData = new()
            {
                commandBlock = commandBlock,

                singleNumber = singleNumber,
                messages = messages.ToArray(),

                androidCompatibility = androidCompatibility,
                numbersExtractionRegion = numbersExtractionRegion,

                betweenMessagesDelay = betweenMessagesDelay,
                betweenNumbersDelay = betweenNumbersDelay,
                maxMessagesSafeLock = maxMessagesSafeLock,

                data = data
            };

            SerializeDeserialize.SavePureDataFile(pureData, settingsPath);
        }
    }
}
