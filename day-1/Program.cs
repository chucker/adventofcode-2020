using System;
using System.Collections.Generic;
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

            TwoInputs(ints);
            ThreeInputs(ints);
        }

        private static void TwoInputs(IEnumerable<int> ints)
        {
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

        private static void ThreeInputs(IEnumerable<int> ints)
        {
            int resultA = -1, resultB = -1, resultC = -1;

            foreach (var intA in ints)
            {
                foreach (var intB in ints)
                {
                    foreach (var intC in ints)
                    {
                        if (intA + intB + intC == 2020)
                        {
                            resultA = intA;
                            resultB = intB;
                            resultC = intC;
                            goto after;
                        }
                    }
                }
            }

        after:
            {
                Console.WriteLine($"{resultA} * {resultB} * {resultC} = {resultA * resultB * resultC}");
            }
        }
    }
}
