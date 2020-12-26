using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace day_7
{
    class Program
    {
        static async Task Main(string[] args)
        {
            foreach (var filename in new[] { "sample-input.txt", "input.txt" })
            {
                var rules = await ReadAndParseRules(filename);

                int count = 0;

                foreach (var item in rules)
                {
                    if (item.Key == "shiny gold bag")
                        continue;

                    if (item.Value.DoesRecursivelyContainOtherBagType("shiny gold bag", rules))
                        count++;
                }
            }
        }

        private static async Task<Dictionary<string, Rule>> ReadAndParseRules(string filename)
        {
            var result = new Dictionary<string, Rule>();

            foreach (var item in await File.ReadAllLinesAsync(filename))
            {
                if (Rule.TryParse(item, out var rule))
                    result[rule.BagType]=rule;
            }

            return result;
        }
    }

    public record Rule(string BagType, string[] ContainedBagTypes)
    {
        private const string RuleRegex = @"(?<BagType>[a-z]+ [a-z]+ bags) contain (?<ContainedBagTypes>(\d [a-z]+ [a-z]+ bags?(, )?)+)\.";
        private const string RuleWithNoOtherBagsRegex = @"(?<BagType>[a-z]+ [a-z]+ bags) contain no other bags\.";

        public static bool TryParse(string rawRule, [NotNullWhen(true)] out Rule? rule)
        {
            rule = null;

            bool hasOtherBags = true;

            var match = Regex.Match(rawRule, RuleRegex);

            if (!match.Success)
            {
                hasOtherBags = false;

                match = Regex.Match(rawRule, RuleWithNoOtherBagsRegex);

                if (!match.Success)
                    return false;
            }

            string bagType = match.Groups["BagType"].Value.Replace("bags", "bag"); // normalize to singular

            if (hasOtherBags)
            {
                var containedBagTypes = match.Groups["ContainedBagTypes"].Value
                                             .Split(new string[] { ", " }, StringSplitOptions.None)
                                             .Select(s => s.Replace("bags", "bag")); // normalize to singular

                rule = new Rule(bagType, containedBagTypes.ToArray());
            }
            else
            {
                rule = new Rule(bagType, Array.Empty<string>());
            }

            return true;
        }

        public bool DoesRecursivelyContainOtherBagType(string otherBagType, Dictionary<string, Rule> allRules)
        {
            if (ContainedBagTypes.Any(x => x.Substring(2) == otherBagType))
                return true;

            foreach (var item in ContainedBagTypes)
            {
                // UGLY: we're (uncleanly) ignoring the counts here (for now)

                if (!allRules.TryGetValue(item.Substring(2), out var containedRule))
                    throw new KeyNotFoundException($"Couldn't find rule for {item}");

                if (containedRule.DoesRecursivelyContainOtherBagType(otherBagType, allRules))
                    return true;
            }

            return false;
        }
    }
}
