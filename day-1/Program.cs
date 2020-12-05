using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace day_1
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var lines = await File.ReadAllLinesAsync("input.txt");

            var ints = lines.Select(s => int.Parse(s));

            int resultA = -1, resultB = -1;

            foreach (var intA in ints)
            {
                foreach (var intB in ints)
                {
                    if (intA + intB == 2020)
                    {
                        resultA = intA;
                        resultB = intB;
                        goto after;
                    }
                }
            }

        after:
            {
                Console.WriteLine($"{resultA} * {resultB} = {resultA * resultB}");
            }
        }
    }
}
