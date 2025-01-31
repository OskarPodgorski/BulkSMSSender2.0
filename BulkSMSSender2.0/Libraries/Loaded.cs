namespace Settings
{
    public static class Loaded
    {
        private readonly static string settingsPath = @"C:\bss\Settings.json";
        private readonly static string colorsPath = @"C:\bss\Colors.json";
        private readonly static string alreadyDonePath = @"C:\bss\AlreadyDone.bss";
        private readonly static string blacklistPath = @"C:\bss\Blacklist.json";
        public readonly static string dataPath = @"C:\bss\Data.txt";

        public static bool commandBlock = false;

        public static bool olderComputer = false;

        public static string singleNumber = string.Empty;
        public static List<string> messages = new();

        public static int androidCompatibility = 0;
        public static int numbersExtractionRegion = 0;

        public static int betweenMessagesDelay = 1500;
        public static int betweenNumbersDelay = 500;
        public static int maxMessagesSafeLock = 20000;

        public static int dataOptimizationThreshold = 1000;

        public static string charFormulaSerialized = string.Empty;
        public static char[] charsOld { get; private set; }
        public static char[] charsNew { get; private set; }

        public static string data = string.Empty;

        private static HashSet<string> alreadyDoneNumbers = LoadAlreadyDone();
        public static int AlreadyDoneCount => alreadyDoneNumbers.Count;

        public static HashSet<string> blacklist { get; private set; } = LoadBlacklist();

        public static Colors colors = LoadColors();

        public static void Load()
        {
            PureDataSettings pureData = SerializeDeserialize.LoadPureDataFile<PureDataSettings>(settingsPath);

            commandBlock = pureData.commandBlock;

            olderComputer = pureData.olderComputer;

            singleNumber = pureData.singleNumber;
            messages = pureData.messages.ToList();

            androidCompatibility = pureData.androidCompatibility;
            numbersExtractionRegion = pureData.numbersExtractionRegion;

            betweenMessagesDelay = pureData.betweenMessagesDelay;
            betweenNumbersDelay = pureData.betweenNumbersDelay;
            maxMessagesSafeLock = pureData.maxMessagesSafeLock;

            dataOptimizationThreshold = pureData.dataOptimizationThreshold;

            InsertCharsFromCharFormula(pureData.charReplaceFormula);

            data = ReadDataFile();
        }

        private static Colors LoadColors()
        {
            PureDataColors pureData = SerializeDeserialize.LoadPureDataFile<PureDataColors>(colorsPath);

            return new Colors(
                pureData.darkGrayColorUI,
                pureData.grayColorUI,
                pureData.violetColorUI,
                pureData.yellowColorUI,
                pureData.blueColorUI,
                pureData.greenColorUI,
                pureData.redColorUI
            );
        }
        private static HashSet<string> LoadBlacklist() => SerializeDeserialize.LoadPureDataFile<PureDataBlacklist>(blacklistPath).blacklisted.ToHashSet();
        public static async Task SaveBlacklistAsync() => await SerializeDeserialize.SavePureDataFileAsync(new PureDataBlacklist() { blacklisted = blacklist.ToArray() }, blacklistPath);

        public static void SaveSettings()
        {
            SerializeDeserialize.SavePureDataFile(PreparePureDataSettings(), settingsPath);
            WriteDataFile(false);
        }

        public static async Task SaveSettingsAsync()
        {
            await SerializeDeserialize.SavePureDataFileAsync(PreparePureDataSettings(), settingsPath);
            await WriteDataFile();
        }

        private static PureDataSettings PreparePureDataSettings()
        {
            return new()
            {
                commandBlock = commandBlock,

                olderComputer = olderComputer,

                singleNumber = singleNumber,
                messages = messages.ToArray(),

                androidCompatibility = androidCompatibility,
                numbersExtractionRegion = numbersExtractionRegion,

                betweenMessagesDelay = betweenMessagesDelay,
                betweenNumbersDelay = betweenNumbersDelay,
                maxMessagesSafeLock = maxMessagesSafeLock,

                dataOptimizationThreshold = dataOptimizationThreshold,

                charReplaceFormula = charFormulaSerialized,
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
                File.Delete(alreadyDonePath);
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

        public static string ReadDataFile()
        {
            if (File.Exists(dataPath))
            {
                string readed;

                using StreamReader reader = new(dataPath);
                {
                    readed = reader.ReadToEnd();
                }

                return readed;
            }
            return string.Empty;
        }

        public static async Task WriteDataFile(bool isAsync = true)
        {
            if (!File.Exists(dataPath))
                File.Create(dataPath).Dispose();

            using StreamWriter writer = new(dataPath);
            {
                writer.WriteLine(string.Empty);

                if (isAsync)
                    await writer.WriteAsync(data);
                else
                    writer.Write(data);
            }
        }
    }
}
