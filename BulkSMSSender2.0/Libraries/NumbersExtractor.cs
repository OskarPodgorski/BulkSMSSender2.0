using System.Text.RegularExpressions;

namespace BulkSMSSender2._0
{
    public readonly struct NumberPack(string number, bool validCheck)
    {
        public readonly string number = number;
        public readonly bool validCheck = validCheck;
    }

    public sealed class NumbersExtractor
    {
        public static string regexPattern = @"\+?[0-9\s\-\(\)]{9,12}";

        public void ExtractNumbers(string siteText)
        {
            if (FinalPage.ins != null)
            {
                MatchCollection matches = Regex.Matches(siteText, regexPattern);
                HashSet<NumberPack> numbers = new();

                foreach (Match match in matches.Cast<Match>())
                {
                    numbers.Add(new(match.Value.Trim(), true));
                }

                FinalPage.ins.AddNumbers(numbers);
            }
        }
    }
}
