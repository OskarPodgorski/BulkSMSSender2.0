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
        public static string userValid = @"\+[0-9]{2}[- ]?[0-9]{3}[- ]?[0-9]{3}[- ]?[0-9]{3}";

        public async Task ExtractNumbersAsync()
        {
            if (FinalPage.ins != null && DataPage.ins != null)
            {
                HashSet<NumberPack> numbers = new();

                await Task.Run(() =>
                {
                    MatchCollection matches = Regex.Matches(DataPage.ins.Data, Constants.REGIONREGEX[Settings.Loaded.numbersExtractionRegion]);

                    foreach (Match match in matches.Cast<Match>())
                    {
                        string cleaned = match.Value.RemoveAllWhitespaces();

                        if (!Settings.Loaded.alreadyDoneNumbers.Contains(cleaned))
                            numbers.Add(new(cleaned, UserValidationNeededTest(cleaned)));
                    }
                });

                FinalPage.ins.AddNumbers(numbers);
            }
        }

        private static bool UserValidationNeededTest(string number)
        {
            if (Regex.IsMatch(number, userValid))
                return true;
            else
                return false;
        }
    }
}
