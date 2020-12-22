using System;
using System.Text.RegularExpressions;

namespace day_2
{
    public class PasswordWithPolicy
    {
        public int MinOccurrences { get; init; }
        public int MaxOccurrences { get; init; }

        // For part 2, the meaning of these changes, so let's rename them
        public int Position1 => MinOccurrences;
        public int Position2 => MaxOccurrences;

        public char Char { get; init; }
        public string Password { get; init; }

        public static PasswordWithPolicy FromGroupCollection(GroupCollection groupCollection)
        {
            return new PasswordWithPolicy
            {
                MinOccurrences = int.Parse(groupCollection["Min"].Value),
                MaxOccurrences = int.Parse(groupCollection["Max"].Value),
                Char = groupCollection["Char"].Value[0],
                Password = groupCollection["Password"].Value
            };
        }

        public bool PasswordIsValid_Part1()
        {
            int count=0;

            foreach (var item in Password)
            {
                if (item == Char)
                    count++;

                if (count > MaxOccurrences)
                    return false;
            }

            if (count < MinOccurrences)
                return false;

            return true;
        }

        public bool PasswordIsValid_Part2()
        {
            if (Password.Length < Position1 || Password.Length < Position2)
                return false;

            if (Password[Position1 - 1] == Char && Password[Position2 - 1] != Char)
                return true;

            if (Password[Position1 - 1] != Char && Password[Position2 - 1] == Char)
                return true;

            return false;
        }
    }
}
