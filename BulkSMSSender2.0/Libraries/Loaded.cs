namespace Settings
{
    public static class Loaded
    {
        private readonly static string settingsPath = Path.Combine(AppContext.BaseDirectory, "Settings.json");
        private readonly static string colorsPath = Path.Combine(AppContext.BaseDirectory, "Colors.json");
        private readonly static string alreadyDonePath = Path.Combine(AppContext.BaseDirectory, "AlreadyDone.bss");

        public static bool commandBlock = false;

        public static string singleNumber = string.Empty;
        public static List<string> messages = new();

        public static int androidCompatibility = 0;
        public static int numbersExtractionRegion = 0;

        public static int betweenMessagesDelay = 2000;
        public static int betweenNumbersDelay = 1000;
        public static int maxMessagesSafeLock = 10000;

        public static string charFormulaSerialized = string.Empty;
        public static char[] charsOld { get; private set; }
        public static char[] charsNew { get; private set; }

        public static string data = string.Empty;

        private static HashSet<string> alreadyDoneNumbers = LoadAlreadyDone();
        public static int AlreadyDoneCount => alreadyDoneNumbers.Count;

        public static Colors colors = LoadColors();

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

            InsertCharsFromCharFormula(pureDataSettings.charReplaceFormula);

            data = pureDataSettings.data;
        }

        private static Colors LoadColors()
        {
            PureDataColors pureDataColors = SerializeDeserialize.LoadPureDataFile<PureDataColors>(colorsPath);

            return new Colors(
                pureDataColors.darkGrayColorUI,
                pureDataColors.grayColorUI,
                pureDataColors.violetColorUI,
                pureDataColors.yellowColorUI,
                pureDataColors.blueColorUI,
                pureDataColors.greenColorUI,
                pureDataColors.redColorUI
            );
        }

        public static void SaveSettings() => SerializeDeserialize.SavePureDataFile(PreparePureDataSettings(), settingsPath);
        public static async Task SaveSettingsAsync() => await SerializeDeserialize.SavePureDataFileAsync(PreparePureDataSettings(), settingsPath);

        private static PureDataSettings PreparePureDataSettings()
        {
            return new()
            {
                commandBlock = commandBlock,

                singleNumber = singleNumber,
                messages = messages.ToArray(),

                androidCompatibility = androidCompatibility,
                numbersExtractionRegion = numbersExtractionRegion,

                betweenMessagesDelay = betweenMessagesDelay,
                betweenNumbersDelay = betweenNumbersDelay,
                maxMessagesSafeLock = maxMessagesSafeLock,

                charReplaceFormula = charFormulaSerialized,

                data = data,
            };
        }

        private static HashSet<string> LoadAlreadyDone()
        {
            if (File.Exists(alreadyDonePath))
            {
                HashSet<string> loaded = new();

                using StreamReader reader = new(alreadyDonePath);
                {
                    string? line;

                    while ((line = reader.ReadLine()) != null)
                    {
                        loaded.Add(line);
                    }
                }
                return loaded;
            }

            return new();
        }

        private static StreamWriter? writer;
        public static void ConnectAlreadyDoneWriter() => writer = new(alreadyDonePath, true);
        public static void DisconnectAlreadyDoneWriter()
        {
            if (writer != null)
            {
                writer.Dispose();
                writer = null;
            }
        }

        /// <summary>
        /// requires ConnectAlreadyDoneWriter() and DisconnectAlreadyDoneWriter() to work properly
        /// </summary>
        /// <returns></returns>
        public static async Task AppendAlreadyDoneAsync(string number)
        {
            if (writer != null)
            {
                if (alreadyDoneNumbers.Add(number))
                    await writer.WriteLineAsync(number);
            }
        }

        public static void ClearAlreadyDone()
        {
            alreadyDoneNumbers.Clear();

            if (File.Exists(alreadyDonePath))
            {
                File.Delete(alreadyDonePath);
            }
        }

        public static bool AlreadyDoneContains(string number) => alreadyDoneNumbers.Contains(number);

        public static void InsertCharsFromCharFormula(string charFormula)
        {
            List<char> charsNewList = new();
            List<char> charsOldList = new();

            charFormulaSerialized = charFormula.RemoveAllWhitespaces();

            string[] splittedOut = charFormulaSerialized.Split('|');

            foreach (string s in splittedOut)
            {
                string[] splittedIn = s.Split("=");

                charsOldList.Add(splittedIn[0][0]);
                charsNewList.Add(splittedIn[1][0]);
            }

            charsOld = charsOldList.ToArray();
            charsNew = charsNewList.ToArray();
        }
    }
}
