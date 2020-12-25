using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace day_5
{
    class Program
    {
        private const int MaxRow = 127;
        private const int MaxColumn = 7;

        static async Task Main(string[] args)
        {
            VerifySampleResults();

            var rawSeatData = await File.ReadAllLinesAsync("input.txt");

            var seats = new List<ParsedSeat>();

            foreach (var seatCode in rawSeatData)
            {
                if (TryParse(seatCode) is (true, var parsedSeat))
                    seats.Add(parsedSeat);
            }

            var highestSeatID = seats.Max(s => s.SeatID);

            Console.WriteLine($"Hightest seat ID: {highestSeatID}");

            var seatID = FindMySeat(seats);

            Console.WriteLine($"My own seat ID: {seatID}");
        }

        private static int FindMySeat(List<ParsedSeat> seats)
        {
            var possibleSeatIDs = Enumerable.Range(0, MaxRow * 8 + MaxColumn);

            var knownSeatIDs = seats.Select(s => s.SeatID).OrderBy(i => i);

            var missingSeatIDs = possibleSeatIDs.Except(knownSeatIDs);

            var seatIDsWithNeighbors = missingSeatIDs.Where(s => knownSeatIDs.Contains(s - 1)
                                                                 && knownSeatIDs.Contains(s + 1));

            return seatIDsWithNeighbors.First();
        }

        private static void VerifySampleResults()
        {
            if (TryParse("FBFBBFFRLR") is not (true, { Row: 44, Column: 5 }))
                throw new Exception();

            if (TryParse("BFFFBBFRRR") is not (true, { Row: 70, Column: 7 }))
                throw new Exception();

            if (TryParse("FFFBBBFRRR") is not (true, { Row: 14, Column: 7 }))
                throw new Exception();

            if (TryParse("BBFFBBFRLL") is not (true, { Row: 102, Column: 4 }))
                throw new Exception();
        }

        private static (bool Success, ParsedSeat ParsedSeat) TryParse(string seatCode)
        {
            if (seatCode.Length != 10)
                return (false, null);

            int row, column;
            int lo, hi;

            // seven chars for row
            lo = 0;
            hi = MaxRow;
            for (int i = 0; i < 7; i++)
                BinaryPartition(seatCode[i], loChar: 'F', hiChar: 'B', ref lo, ref hi);
            row = lo;

            // remaining three for column
            lo = 0;
            hi = MaxColumn;
            for (int i = 7; i < 10; i++)
                BinaryPartition(seatCode[i], loChar: 'L', hiChar: 'R', ref lo, ref hi);
            column = lo;

            return (true, new ParsedSeat((short)row, (short)column));
        }

        private static void BinaryPartition(char c, char loChar, char hiChar, ref int lo, ref int hi)
        {
            var range = hi - lo + 1;

            if (c == loChar)
                hi -= range / 2;
            else if (c == hiChar)
                lo += range / 2;
            else
                throw new ArgumentException($"Unexpected char {c}");
        }
    }

    public record ParsedSeat(short Row, short Column)
    {
        public int SeatID => Row * 8 + Column;
    }
}
