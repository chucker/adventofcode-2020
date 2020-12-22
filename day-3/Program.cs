using System;
using System.IO;
using System.Threading.Tasks;

namespace day_3
{
    class Program
    {
        private const char TreeChar = '#';

        static async Task Main(string[] args)
        {
            var lines = await File.ReadAllLinesAsync("input.txt");

            Part1(lines);

            Part2(lines);
        }

        private static void Part1(string[] lines)
        {
            var slope = (X: 3, Y: 1);
            CountTrees(lines, slope);
        }

        private static void Part2(string[] lines)
        {
            long treesMultiplied = 1;

            var slopes = new (int X, int Y)[]
            {
                (1, 1),
                (3, 1),
                (5, 1),
                (7, 1),
                (1, 2)
            };

            foreach (var slope in slopes)
            {
                var trees = CountTrees(lines, slope);
                treesMultiplied *= trees;
            }

            Console.WriteLine($"Grand product: {treesMultiplied}");
        }

        private static int CountTrees(string[] lines, (int X, int Y) slope)
        {
            var position = (X: 0, Y: 0);

            int trees = 0;

            while (position.Y < lines.Length)
            {
                string line = lines[position.Y];

                if (line[position.X % line.Length] == TreeChar)
                    trees++;

                position = (position.X + slope.X, position.Y + slope.Y);
            }

            Console.WriteLine($"Slope X: {slope.X}, Y: {slope.Y}: {trees} trees");

            return trees;
        }
    }
}
