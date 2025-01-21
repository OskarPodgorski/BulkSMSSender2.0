using System.Diagnostics;
using System.Text.RegularExpressions;

namespace BulkSMSSender2._0
{
    public sealed class NumbersExtractor
    {
        public static string regexPattern = @"\+?[0-9\s\-\(\)]{9,12}";

        public void ExtractNumbers(string siteText)
        {
            MatchCollection matches = Regex.Matches(siteText, regexPattern);

            foreach (Match match in matches.Cast<Match>())
            {
                Debug.WriteLine(match.Value.Trim());
            }
        }
    }
}
