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

                Console.WriteLine($"{filename} has {groups.Count} groups. There are {SumUpAnswers(groups)} unique answers across groups.");
            }
        }

        private static async Task<List<List<char>>> ReadInput(string filename)
        {
            var groups = new List<List<char>>();

            var currentGroup = new List<char>();

            foreach (var line in await File.ReadAllLinesAsync(filename))
            {
                // finish previous group
                if (string.IsNullOrWhiteSpace(line))
                {
                    groups.Add(currentGroup);

                    currentGroup = new List<char>();
                }

                currentGroup.AddRange(line);
            }

            // finish final group
            groups.Add(currentGroup);

            return groups;
        }

        private static int SumUpAnswers(List<List<char>> groups)
            => groups.Sum(g => g.Distinct().Count());
    }
}
