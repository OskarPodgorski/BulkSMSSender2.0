namespace Settings
{
    public static class Loaded
    {
        private readonly static string settingsPath = Path.Combine(AppContext.BaseDirectory, "Settings.json");
        private readonly static string colorsPath = Path.Combine(AppContext.BaseDirectory, "Colors.json");

        public static bool commandBlock = false;

        public static string singleNumber = string.Empty;
        public static List<string> messages = new();

        public static int androidCompatibility = 0;
        public static int numbersExtractionRegion = 0;

        public static int betweenMessagesDelay = 2000;
        public static int betweenNumbersDelay = 1000;
        public static int maxMessagesSafeLock = 10000;

        public static string data = string.Empty;

        public static Colors colors = new();

        public static void Load()
        {
            PureDataSettings pureDataSettings = SerializeDeserialize.LoadPureDataFile<PureDataSettings>(settingsPath);

            commandBlock = pureDataSettings.commandBlock;

            singleNumber = pureDataSettings.singleNumber;
            messages = pureDataSettings.messages.ToList();

            androidCompatibility = pureDataSettings.androidCompatibility;
            numbersExtractionRegion = pureDataSettings.numbersExtractionRegion;

            betweenMessagesDelay = pureDataSettings.betweenMessagesDelay;
            betweenNumbersDelay = pureDataSettings.betweenNumbersDelay;
            maxMessagesSafeLock = pureDataSettings.maxMessagesSafeLock;

            data = pureDataSettings.data;

            PureDataColors pureDataColors = SerializeDeserialize.LoadPureDataFile<PureDataColors>(colorsPath);

            colors = new(pureDataColors.darkGrayColorUI, pureDataColors.grayColorUI, pureDataColors.violetColorUI, pureDataColors.yellowColorUI, pureDataColors.blueColorUI, pureDataColors.greenColorUI);
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
