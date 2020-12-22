using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace day_4
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var rawPassports =             await ReadRawPassportData();

            var parsedPassports = ParsePassports(rawPassports);

            Console.WriteLine($"Passports: {parsedPassports.Count}; valid: {parsedPassports.Where(p => PassportIsValid(p)).Count()}");
        }

        private static async Task<List<string>> ReadRawPassportData()
        {
            var result = new List<string>();

            string currentRawPassport = "";

            foreach (var line in await File.ReadAllLinesAsync("input.txt"))
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    // an all-empty line terminates the previous passport

                    if (!string.IsNullOrWhiteSpace(currentRawPassport))
                        result.Add(currentRawPassport);
                    currentRawPassport = "";
                    continue;
                }

                currentRawPassport += line;
            }

            // if the file ends without an empty line, don't forget the last item
            if (!string.IsNullOrWhiteSpace(currentRawPassport))
                result.Add(currentRawPassport);

            return result;
        }

        private const string KeyValuePairRegex = @"(?<Key>\w+):(?<Value>[\w\d#]+)";

        private static List<Dictionary<string, string>> ParsePassports(List<string> rawPassports)
        {
            var result = new List<Dictionary<string, string>>();

            foreach (var item in rawPassports)
            {
                var matches = Regex.Matches(item, KeyValuePairRegex);

                result.Add(matches.OfType<Match>()
                                  .ToDictionary(m => m.Groups["Key"].Value, m => m.Groups["Value"].Value));
            }

            return result;
        }

        private static bool PassportIsValid(Dictionary<string, string> passport)
        {
            // 'cid' also exists, but isn't mandatory

            foreach (var item in new[] { "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid" })
            {
                if (!passport.ContainsKey(item))
                    return false;
            }

            return true;
        }
    }
}
