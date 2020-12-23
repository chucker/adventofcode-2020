using System;
using System.Reflection;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace day_4
{
    public interface IHeight
    {
        public int Value { get; set; }
    }
    public struct CmHeight : IHeight
    {
        public int Value { get; set; }
    }
    public struct InHeight : IHeight
    {
        public int Value { get; set; }
    }

    public enum EyeColor
    {
        amb,
        blu,
        brn,
        gry,
        grn,
        hzl,
        oth
    }

    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public sealed class SerializedAsAttribute : Attribute
    {
        public string InternalName { get; }

        public SerializedAsAttribute(string internalName)
        {
            InternalName = internalName;
        }
    }

    public class Passport
    {
        // TODO: validation rules

        [SerializedAs("byr")]
        public int? BirthYear { get; private set; }

        [SerializedAs("iyr")]
        public int? IssueYear { get; private set; }

        [SerializedAs("eyr")]
        public int? ExpirationYear { get; private set; }

        [SerializedAs("hgt")]
        public IHeight? Height { get; private set; }

        [SerializedAs("hcl")]
        public int? HairColor { get; private set; }

        [SerializedAs("ecl")]
        public EyeColor? EyeColor { get; private set; }

        [SerializedAs("pid")]
        public string? PassportId { get; private set; }

        [SerializedAs("cid")]
        public string? CountryId { get; private set; }

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

            if (TryGetPropValue(dict, nameof(BirthYear), out s) && int.TryParse(s, out i))
                passport.BirthYear = i;

            if (TryGetPropValue(dict, nameof(IssueYear), out s) && int.TryParse(s, out i))
                passport.IssueYear = i;

            if (TryGetPropValue(dict, nameof(ExpirationYear), out s) && int.TryParse(s, out i))
                passport.ExpirationYear = i;

            if (TryGetPropValue(dict, nameof(Height), out s))
                passport.Height = ParseHeight(s);

            if (TryGetPropValue(dict, nameof(HairColor), out s))
                passport.HairColor = ParseHairColor(s);

            if (TryGetPropValue(dict, nameof(EyeColor), out s) &&
                Enum.TryParse(s, out EyeColor eyeColor))
            {
                passport.EyeColor = eyeColor;
            }

            if (TryGetPropValue(dict, nameof(PassportId), out s))
                passport.PassportId = s;

            if (TryGetPropValue(dict, nameof(CountryId), out s))
                passport.CountryId = s;

            return true;
        }

        private static bool TryGetPropValue(Dictionary<string, string> dict, string propertyName, [NotNullWhen(true)] out string? value)
        {
            var serializedAs = typeof(Passport).GetProperty(propertyName)?.GetCustomAttribute<SerializedAsAttribute>()?.InternalName;

            if (serializedAs == null)
                throw new ArgumentException(null, nameof(propertyName));

            return dict.TryGetValue(serializedAs, out value);
        }

        private static IHeight? ParseHeight(string v)
        {
            IHeight result;

            int unitOffset;

            unitOffset = v.IndexOf("cm");

            if (unitOffset > 0)
                result = new CmHeight();
            else
            {
                unitOffset = v.IndexOf("in");

                if (unitOffset < 0)
                    return null;

                result = new InHeight();
            }

            if (!int.TryParse(v[0..unitOffset], out var value))
                return null;

            result.Value = value;

            return result;
        }

        public bool IsValid()
        {
            if (!BirthYear.HasValue ||
                !IssueYear.HasValue ||
                !ExpirationYear.HasValue ||
                Height == null ||
                HairColor == null ||
                EyeColor == null ||
                PassportId == null)
            {
                return false;
            }

            // NOTE: cid is _not_ mandatory. Let's ignore it.

            // TODO: check ranges

            return true;
        }

        private static int? ParseHairColor(string v)
        {
            if (!v.StartsWith("#"))
                return null;

            if (v.Length != 7)
                return null;

            return Convert.ToInt32(v.Substring(1), 16);
        }
    }
}
