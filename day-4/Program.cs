using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace day_4
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var rawPassports = await ReadRawPassportData();

            var parsedPassports = new List<Passport>();

            foreach (var item in rawPassports)
            {
                if (Passport.TryParse(item, out var passport))
                    parsedPassports.Add(passport);
            }

            var validPassports = parsedPassports.Where(p => p.IsValid()).ToList();

            Console.WriteLine($"Raw passports: {rawPassports.Count}; parsed: {parsedPassports.Count}; valid: {validPassports.Count}");
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

                currentRawPassport += " " + line;
            }

            // if the file ends without an empty line, don't forget the last item
            if (!string.IsNullOrWhiteSpace(currentRawPassport))
                result.Add(currentRawPassport);

            return result;
        }
    }
}
