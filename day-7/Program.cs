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
        private const string QueriedRule = "shiny gold bag";

        static async Task Main(string[] args)
        {
            foreach (var filename in Directory.EnumerateFiles(".", "*.txt"))
            {
                var rules = await ReadAndParseRules(filename);

                    int count = 0;

                    foreach (var item in rules)
                    {
                        if (item.Key == QueriedRule)
                            continue;

                        if (item.Value.DoesRecursivelyContainOtherBagType(QueriedRule, rules))
                            count++;
                    }

                    Console.WriteLine($"File {filename}: {QueriedRule}s are indirectly contained by {count} other bags");

                int containedBags = 0;
                rules[QueriedRule].SumContainedBagsRecursively(ref containedBags, rules);

                Console.WriteLine($"File {filename}: {QueriedRule}s ultimately contain {containedBags} other bags");
            }
        }

        private static async Task<Dictionary<string, BagRule>> ReadAndParseRules(string filename)
        {
            var result = new Dictionary<string, BagRule>();

            foreach (var item in await File.ReadAllLinesAsync(filename))
            {
                if (BagRule.TryParse(item, out var rule))
                    result[rule.BagType]=rule;
            }

            return result;
        }
    }

    public record BagRule(string BagType, ContainedBagRule[] ContainedBagRules)
    {
        private const string RuleRegex = @"(?<BagType>[a-z]+ [a-z]+ bags) contain (?<ContainedBagTypes>(\d [a-z]+ [a-z]+ bags?(, )?)+)\.";
        private const string RuleWithNoOtherBagsRegex = @"(?<BagType>[a-z]+ [a-z]+ bags) contain no other bags\.";

        public static bool TryParse(string rawRule, [NotNullWhen(true)] out BagRule? rule)
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
                                             .Split(new string[] { ", " }, StringSplitOptions.None);

                var parsedContainedBagTypes = new List<ContainedBagRule>();

                foreach (var s in containedBagTypes)
                {
                    if (ContainedBagRule.TryParse(s, out var parsed))
                        parsedContainedBagTypes.Add(parsed);
                }

                rule = new BagRule(bagType, parsedContainedBagTypes.ToArray());
            }
            else
            {
                rule = new BagRule(bagType, Array.Empty<ContainedBagRule>());
            }

            return true;
        }

        public bool DoesRecursivelyContainOtherBagType(string otherBagType, Dictionary<string, BagRule> allRules)
        {
            if (ContainedBagRules.Any(x => x.BagType == otherBagType))
                return true;

            foreach (var item in ContainedBagRules)
            {
                if (!allRules.TryGetValue(item.BagType, out var containedRule))
                    throw new KeyNotFoundException($"Couldn't find rule for {item}");

                if (containedRule.DoesRecursivelyContainOtherBagType(otherBagType, allRules))
                    return true;
            }

            return false;
        }

        public void SumContainedBagsRecursively(ref int totalCount, Dictionary<string, BagRule> allRules)
        {
            foreach (var item in ContainedBagRules)
            {
                //Console.WriteLine($"{this.BagType} contains {item.BagType}; adding {item.Amount} of those");

                for (int i = 0; i < item.Amount; i++)
                {
                    totalCount++;

                    if (!allRules.TryGetValue(item.BagType, out var containedRule))
                        throw new KeyNotFoundException($"Couldn't find rule for {item}");

                    containedRule.SumContainedBagsRecursively(ref totalCount, allRules);
                }
            }
        }
    }

    public record ContainedBagRule(int Amount, string BagType)
    {
        private const string RuleRegex = @"(?<Amount>\d+) (?<BagType>[a-z]+ [a-z]+ bags?)";

        public static bool TryParse(string rawRule, [NotNullWhen(true)] out ContainedBagRule rule)
        {
            rule = null;

            var match = Regex.Match(rawRule, RuleRegex);

            if (!match.Success)
                return false;

            int amount = int.Parse(match.Groups["Amount"].Value);

            string bagType = match.Groups["BagType"].Value.Replace("bags", "bag"); // normalize to singular

            rule = new ContainedBagRule(amount, bagType);

            return true;
        }
    }
}
