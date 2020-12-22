using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;

namespace day_4
{
    public interface IHeight
    {
        public int Value { get; }
    }
    public struct CmHeight : IHeight
    {
        public int Value { get; private set; }
    }
    public struct InHeight : IHeight
    {
        public int Value { get; private set; }
    }

    public class Passport
    {
        public int? byr { get; private set; }
        public int? iyr { get; private set; }
        public int? eyr { get; private set; }
        public IHeight? hgt { get; private set; }
        public string? hcl { get; private set; }
        public string? ecl { get; private set; }
        public string? pid { get; private set; }
        public string? cid { get; private set; }

        private Passport() { }

        public static bool TryParse(string rawPassport, [NotNullWhen(true)] out Passport? passport)
        {
            passport = null;

            const string KeyValuePairRegex = @"(?<Key>\w+):(?<Value>[\w\d#]+)";

            var matches = Regex.Matches(rawPassport, KeyValuePairRegex);

            if (matches.Count == 0)
                return false;

            var dict = matches.OfType<Match>()
                              .ToDictionary(m => m.Groups["Key"].Value, m => m.Groups["Value"].Value);

            passport = new Passport();

            string? s;
            int i;

            if (dict.TryGetValue(nameof(byr), out s) && int.TryParse(s, out i))
                passport.byr = i;

            if (dict.TryGetValue(nameof(iyr), out s) && int.TryParse(s, out i))
                passport.iyr = i;

            if (dict.TryGetValue(nameof(eyr), out s) && int.TryParse(s, out i))
                passport.eyr = i;

             //   hgt = ParseHeight(dict[nameof(hgt)]),

            //    hcl = dict[nameof(hcl)],
            //    ecl = dict[nameof(ecl)],
            //    pid = dict[nameof(pid)],
            //    cid = dict[nameof(cid)]
            //};

            return true;
        }

        private static IHeight ParseHeight(string v)
        {
            throw new NotImplementedException();
        }

        public bool IsValid()
        {
            if (!byr.HasValue ||
                !iyr.HasValue ||
                !eyr.HasValue ||
                hgt == null ||
                hcl == null ||
                ecl == null ||
                pid == null)
            {
                return false;
            }

            // cid is _not_ mandatory

            // TODO: check ranges

            return true;
        }
    }
}
