using AdventOfCodeLib;

public static class Day01
{

    public static Puzzle.Solution Part1 = (input) =>
    {
        var sum = 0;
        foreach (var line in input)
        {
            sum += int.Parse($"{line.First((c) => char.IsDigit(c))}{line.Last((c) => char.IsDigit(c))}");
        }

        return sum.ToString();
    };

    public static Puzzle.Solution Part2 = (input) =>
    {
        var digits = new string[] { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
        var sum = 0;
        foreach (var line in input)
        {
            string d1 = "0";
            string d2 = "0";

            var counter = 1;
            var index = line.Length + 1;
            foreach (var digit in digits)
            {
                var idx = line.IndexOf(digit);
                if (0 <= idx && idx < index)
                {
                    d1 = counter.ToString();
                    index = idx;
                }
                counter++;
            }

            if (d1 == "0")
            {
                d1 = line.First((c) => char.IsDigit(c)).ToString();
            }
            else if (line.Any((c) => char.IsDigit(c)))
            {
                var d = line.First((c) => char.IsDigit(c));
                if (line.IndexOf(d) < index)
                {
                    d1 = d.ToString();
                }
            }

            counter = 1;
            index = 0;
            foreach (var digit in digits)
            {
                var idx = line.LastIndexOf(digit);
                if (0 < idx && idx > index)
                {
                    d2 = counter.ToString();
                    index = idx;
                }
                counter++;
            }

            if (d2 == "0")
            {
                d2 = line.Last((c) => char.IsDigit(c)).ToString();
            }
            else if (line.Any((c) => char.IsDigit(c)))
            {
                var d = line.Last((c) => char.IsDigit(c));
                if (line.LastIndexOf(d) > index)
                {
                    d2 = d.ToString();
                }
            }

            sum += int.Parse($"{d1}{d2}");
        }

        return sum.ToString();
    };
}