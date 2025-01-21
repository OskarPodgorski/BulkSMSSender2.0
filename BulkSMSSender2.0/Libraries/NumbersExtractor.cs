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
        //public static string regexPattern = @"(\+[0-9]{1,3}[- ]?)?[0-9]{2,3}[- ]?[0-9]{2,3}[- ]?[0-9]{2,3}";

        public static string regexPattern = @"(\+[0-9]{2}[- ]?)?[0-9]{3}[- ]?[0-9]{3}[- ]?[0-9]{3}";

        public static string userValidOne = @"^[0-9]{3}[- ]?[0-9]{3}[- ]?[0-9]{3}";
        public static string userValidTwo = @"\+[0-9]{2}[- ]?[0-9]{3}[- ]?[0-9]{3}[- ]?[0-9]{3}";

        public void ExtractNumbers(string siteText)
        {
            if (FinalPage.ins != null)
            {
                MatchCollection matches = Regex.Matches(siteText, regexPattern);
                HashSet<NumberPack> numbers = new();

                foreach (Match match in matches.Cast<Match>())
                {
                    string trimmed = match.Value.Trim();
                    numbers.Add(new(trimmed, UserValidationNeededTest(trimmed)));
                }

                FinalPage.ins.AddNumbers(numbers);
            }
        }

        private static bool UserValidationNeededTest(string number)
        {
            if (!Regex.IsMatch(number, userValidOne) && !Regex.IsMatch(number, userValidTwo))
                return true;
            else
                return false;
        }
    }
}
