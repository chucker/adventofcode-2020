using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace day_8
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var instructions = await ReadInstructions("sample-input.txt");

            int accumulator = 0;

            instructions[0].Visit(ref accumulator, 1, instructions);
        }

        private static async Task<List<Instruction>> ReadInstructions(string filename)
        {
            var instructions = new List<Instruction>();

            const string regex = @"(?<OpCode>[a-z]{3}) (?<Sign>[+-])(?<Argument>\d+)";

            // only used for debugging purposes
            uint line = 1;

            foreach (var item in await File.ReadAllLinesAsync(filename))
            {
                var match = Regex.Match(item, regex);

                if (!match.Success)
                    throw new ArgumentException($"Unexpected instruction {item}");

                if(!Enum.TryParse<OpCode>(match.Groups["OpCode"].Value, out var opCode))
                    throw new ArgumentException($"Unexpected opcode {opCode}");

                int argument = int.Parse(match.Groups["Argument"].Value);

                if (match.Groups["Sign"].Value == "-")
                    argument *= -1;

                instructions.Add(new Instruction(line++, opCode, argument));
            }

            return instructions;
        }
    }

    public enum OpCode
    {
        nop,
        acc,
        jmp
    }

    public record Instruction(uint Line, OpCode OpCode, int Argument)
    {
        public int VisitCount { get; private set; } = 0;

        public void Visit(ref int accumulator, int maxVisitCount, List<Instruction> allInstructions)
        {
            // BUG: this isn't going to work
            if (VisitCount >= maxVisitCount)
                return;

            VisitCount++;

            var currentIndex = allInstructions.IndexOf(this);
            int nextIndex;

            switch (OpCode)
            {
                case OpCode.acc:
                    accumulator += Argument;

                    nextIndex = currentIndex + 1;
                    break;
                case OpCode.jmp:
                    nextIndex = currentIndex + Argument;
                    break;
                case OpCode.nop:
                default:
                    nextIndex = currentIndex + 1;
                    break;
            }

            allInstructions[nextIndex].Visit(ref accumulator, maxVisitCount, allInstructions);
        }
    }
}
