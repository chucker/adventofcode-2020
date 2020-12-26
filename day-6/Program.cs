using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace day_6
{
    class Program
    {
        static async Task Main(string[] args)
        {
            foreach (var filename in new[] { "sample-input.txt", "input.txt" })
            {
                var groups = await ReadInput(filename);

                Console.WriteLine($"{filename}:");

                Console.WriteLine($"    {groups.Count} groups");
                Console.WriteLine($"    {SumUpAnswers(groups)} unique answers across groups");
                Console.WriteLine($"    {AnswersAllYes(groups)} answers where everyone in a group said 'yes'");

                Console.WriteLine();
            }
        }

        private static async Task<List<List<string>>> ReadInput(string filename)
        {
            var groups = new List<List<string>>();

            var currentGroup = new List<string>();

            foreach (var line in await File.ReadAllLinesAsync(filename))
            {
                // finish previous group
                if (string.IsNullOrWhiteSpace(line))
                {
                    groups.Add(currentGroup);

                    currentGroup = new List<string>();

                    continue;
                }

                // each string inside the group corresponds to a participant
                currentGroup.Add(line);
            }

            // finish final group
            groups.Add(currentGroup);

            return groups;
        }

        private static int SumUpAnswers(List<List<string>> groups)
        {
            // here, we have to:
            // * flatten multiple participants inside a group together (SelectMany)
            // * get all unique answers of them (Distinct)
            // * count them
            // * sum that across gruops

            /*
             * For part 1, we could simply use List<char> instead, but for part
             * 2, we want to preserve individual participants.
             */

            return groups.Sum(g => g.SelectMany(s => s).Distinct().Count());
        }

        private static int AnswersAllYes(List<List<string>> groups)
        {
            int count = 0;

            // iterate each group
            foreach (var g in groups)
            {
                // iterate any distinct char (across participants) within the group
                foreach (var ch in g.SelectMany(s => s).Distinct())
                {
                    // if all participants have this char, count it
                    if (g.All(s => s.Contains(ch)))
                        count++;
                }
            }

            return count;
        }
    }
}
