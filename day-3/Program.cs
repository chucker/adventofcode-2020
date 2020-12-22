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

            var position = (X: 0, Y: 0);

            int trees = 0;

            while (position.Y < lines.Length)
            {
                string line = lines[position.Y];

                if (line[position.X % line.Length] == TreeChar)
                    trees++;

                position = (position.X + 3, position.Y + 1);
            }

            Console.WriteLine(trees);
        }
    }
}
