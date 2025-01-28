namespace Settings
{
    public sealed class PureDataSettings
    {
        public bool commandBlock = false;

        public string singleNumber = string.Empty;
        public string[] messages = new string[0];

        public int androidCompatibility = 0;
        public int numbersExtractionRegion = 0;

        public int betweenMessagesDelay = 1500;
        public int betweenNumbersDelay = 500;
        public int maxMessagesSafeLock = 20000;

        public string charReplaceFormula = "ą=a|ś=s|ć=c|ę=e|ó=o|ł=l|ń=n|ż=z|ź=z|Ą=A|Ś=S|Ć=C|Ę=E|Ó=O|Ł=L|Ń=N|Ż=Z|Ź=Z";

        public string data = string.Empty;
    }
}
