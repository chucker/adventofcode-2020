using System;
using System.Text.RegularExpressions;

namespace day_2
{
    public class PasswordWithPolicy
    {
        public int MinOccurrences { get; init; }
        public int MaxOccurrences { get; init; }
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

        public bool PasswordIsValid()
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
    }
}
