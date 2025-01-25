namespace Settings
{
    public sealed class PureDataSettings
    {
        public bool commandBlock = false;

        public string singleNumber = string.Empty;
        public string[] messages = new string[0];

        public int androidCompatibility = 0;
        public int numbersExtractionRegion = 0;

        public int betweenMessagesDelay = 2000;
        public int betweenNumbersDelay = 500;
        public int maxMessagesSafeLock = 10000;

        public string data = string.Empty;
    }
}
