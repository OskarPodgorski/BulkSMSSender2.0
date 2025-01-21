using System.Text.RegularExpressions;

namespace BulkSMSSender2._0
{
    public sealed class NumbersExtractor
    {
        public static string regexPattern = @"\+?[0-9\s\-\(\)]{9,12}";

        public void ExtractNumbers(string siteText)
        {
            if (FinalPage.ins != null)
            {
                MatchCollection matches = Regex.Matches(siteText, regexPattern);

                foreach (Match match in matches.Cast<Match>())
                {
                    FinalPage.ins.AddNumber(match.Value.Trim());
                }
            }
        }
    }
}
