using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace day_2
{
    class Program
    {
        const string Regex = @"(?<Min>\d+)-(?<Max>\d+) (?<Char>\w): (?<Password>\w+)";

        static async Task Main(string[] args)
        {
            var passwordsWithPolicies = new List<PasswordWithPolicy>();

            foreach (var item in await File.ReadAllLinesAsync("input.txt"))
            {
                var match = System.Text.RegularExpressions.Regex.Match(item, Regex);

                passwordsWithPolicies.Add(PasswordWithPolicy.FromGroupCollection(match.Groups));
            }

            Console.WriteLine(passwordsWithPolicies.Where(pwp => pwp.PasswordIsValid()).Count());
        }
    }
}
